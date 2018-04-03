using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Mvc;
using Workwiz.Common.Logging;
using Workwiz.PaymentFramework.Mvc.Models;
using Workwiz.PaymentFramework.Shared;
using Workwiz.PaymentFramework.Shared.Models;
using Workwiz.PaymentFramework.Shared.RealexApi;
using Workwiz.PaymentFramework.Shared.RealexApi.RealAuth;

namespace Workwiz.PaymentFramework.Mvc.RealEx
{
    public class RealExPaymentProvider : RealexApiProvider, IPaymentProvider
    {
        public override ProviderFeatures SupportedFeatures =>
            base.SupportedFeatures |
            ProviderFeatures.SaveCardholder |
            ProviderFeatures.SaveCard;

        public RealExPaymentProvider(IWorkwizLoggerFactory loggerFactory) : base(loggerFactory)
        {
        }

        public string RoundTripTokenKey => RealexFields.RealexFieldOrderId;

        public string GenerateUniqueIdentifier()
        {
            return Guid.NewGuid().ToString("D");
        }

        public ActionResult SendToPaymentProvider(
            PaymentProviderConfiguration configuration,
            GeneralisedPaymentTransfer transferObject,
            Action<PaymentProviderConfiguration, GeneralisedPaymentTransfer, string> saveProviderReference)
        {
            return SendToPaymentProvider(configuration, transferObject, DateTime.Now);
        }

        public ActionResult SendToPaymentProvider(
            PaymentProviderConfiguration configuration,
            GeneralisedPaymentTransfer transferObject, DateTime nowLocalTime)
        {

            var realExXfer = new RealExPaymentTransfer()
            {
                MerchantId = configuration.AccountIdentifer,
                OrderId = transferObject.TransactionId,
                Amount = (int)(transferObject.Amount * 100),
                ProviderUrl = Shared.RealEx.Default.RealExPostUrl,
                ReturnUrl = transferObject.ReturnUrl,
                Currency = "GBP",
                Timestamp = nowLocalTime.ToString("yyyyMMddHHmmss"),
                AutoSettleFlag = true
            };

            realExXfer.Account = transferObject.Account;
            realExXfer.VariableReference = MessageContentUtility.TruncateAndStripDisallowed(transferObject.VariableReference, 50, RealexFields.RealexFieldVarRefDisallowRegex);
            realExXfer.CustomerNumber = transferObject.CustomerNumber;
            realExXfer.ProductId = MessageContentUtility.TruncateAndStripDisallowed(transferObject.ProductId, null, RealexFields.RealexFieldProductIdDisallowRegex);
            realExXfer.Comment1 = MessageContentUtility.TruncateAndStripDisallowed(transferObject.Comment1, 255, RealexFields.RealexFieldCommentDisallowRegex);
            realExXfer.Comment2 = MessageContentUtility.TruncateAndStripDisallowed(transferObject.Comment2, 255, RealexFields.RealexFieldCommentDisallowRegex);

            var fieldsForSignature = new List<string>();
            fieldsForSignature.Add(realExXfer.Timestamp);
            fieldsForSignature.Add(realExXfer.MerchantId);
            fieldsForSignature.Add(realExXfer.OrderId);
            fieldsForSignature.Add(realExXfer.Amount.ToString());
            fieldsForSignature.Add(realExXfer.Currency);

            if (transferObject.SaveCard != null)
            {
                realExXfer.HasSavedCard = true;
                realExXfer.CardStorageEnabled = true;
                realExXfer.OfferSaveCard = transferObject.SaveCard.OfferSaveCard;
                realExXfer.PaymentReference = MessageContentUtility.TruncateAndStripDisallowed(transferObject.SaveCard.PaymentReference, 30, RealexFields.RealexFieldPaymentRefDisallowRegex);
                realExXfer.PayerReference = MessageContentUtility.TruncateAndStripDisallowed(transferObject.SaveCard.PayerReference, 50, RealexFields.RealexFieldPayerRefDisallowRegex);
                realExXfer.PayerExists = transferObject.SaveCard.PayerExists;
                fieldsForSignature.Add(realExXfer.PayerReference);
                fieldsForSignature.Add(realExXfer.PaymentReference);
            }

            realExXfer.Sha1Hash = CalculateRealexSignature(fieldsForSignature.ToArray(), configuration.SharedSecret);

            var resultView = PaymentFrameworkUtility.CreateView("~/Views/Payment/SendToRealEx.cshtml", realExXfer);

            var sendToPaymentLogMessage = PaymentFrameworkUtility.DescribeActionResultForLogging(resultView, true);
            this.Logger.CreateEntry(typeof(RealExPaymentProvider), LogLevel.Info, sendToPaymentLogMessage);
            return resultView;
        }

        private static bool IsResponseCodeSuccess(string rawResponseCode)
        {
            int resultCode = RealAuthResponse.ParseResultString(rawResponseCode);
            return 0 == resultCode;
        }

        public Task<PaymentAuthorizationGuiResponse> ProcessResponse(
            PaymentProviderConfiguration configuration,
            NameValueCollection paramsCollection,
            ResponseParameters additionalParameters)
        {
            string timestamp = paramsCollection[RealexFields.RealexFieldTimestamp];
            string merchantid = paramsCollection[RealexFields.RealexFieldMerchantId];
            string orderid = paramsCollection[RealexFields.RealexFieldOrderId];
            string authcode = paramsCollection[RealexFields.RealexFieldAuthCode];
            string resultCode = paramsCollection[RealexFields.RealexFieldResult];
            string textmsg = paramsCollection[RealexFields.RealexFieldResponseTextMessage];
            string receiptNumber = paramsCollection[RealexFields.RealexFieldPasRef];
            string responseSignature = paramsCollection[RealexFields.RealexFieldHashSignature];
            string amountString = paramsCollection[RealexFields.RealexFieldAmount];

            string[] fieldsForResponseSignature = new string[]
            {
                timestamp,
                merchantid,
                orderid,
                resultCode,
                textmsg,
                receiptNumber, // PASREF
                authcode
            };
            string expectedResponseHash = CalculateRealexSignature(fieldsForResponseSignature, configuration.SharedSecret);

            decimal amount;
            if (decimal.TryParse(amountString, out amount))
            {
                amount = amount / 100m;
            }
            else
            {
                amount = 0;
            }

            if (expectedResponseHash != responseSignature)
            {
                PaymentAuthorizationResponse dummyBackendResponse = new PaymentAuthorizationResponse(false, PaymentAuthorizationResult.Unknown, amount,
                    "incorrect SHA1HASH.", receiptNumber)
                {
                    ResponseCode = resultCode,
                    TransactionId = orderid
                };
                return Task.FromResult(new PaymentAuthorizationGuiResponse(dummyBackendResponse, null));
            }

            PaymentAuthorizationResult isAuthorized = IsResponseCodeSuccess(resultCode) ? PaymentAuthorizationResult.Authorized : PaymentAuthorizationResult.Declined;

            var savedCardResponse = new SavedCardResponse();
            if (!string.IsNullOrEmpty(paramsCollection[RealVaultFields.ResponsePayerSetupResultCode]) && paramsCollection[RealVaultFields.ResponsePayerSetupResultCode] == "00")
            {
                savedCardResponse.NewCardholderSaved = true;
            }

            if (!string.IsNullOrEmpty(paramsCollection[RealVaultFields.ResponseSavedPayerRef]))
            {
                savedCardResponse.CardholderReference = paramsCollection[RealVaultFields.ResponseSavedPayerRef];
            }

            if (!string.IsNullOrEmpty(paramsCollection[RealVaultFields.ResponseSavedCardResultCode]))
            {
                if (IsResponseCodeSuccess(paramsCollection[RealVaultFields.ResponseSavedCardResultCode]))
                {
                    savedCardResponse.CardSaved = true;
                    savedCardResponse.CardReference = paramsCollection[RealVaultFields.ResponseSavedCardRef];
                    savedCardResponse.CardDigits = paramsCollection[RealVaultFields.ResponseMaskedCardDigits];
                    savedCardResponse.ExpiryDate =
                        Utility.ParseExpiryDate(paramsCollection[RealVaultFields.ResponseSavedCardExpiryDate]);
                }
                else
                {
                    savedCardResponse.CardSaved = false;
                    savedCardResponse.CardSaveStatus = paramsCollection[RealVaultFields.ResponseSavedCardResultMessage];
                }
            }

            PaymentAuthorizationResponse response = new PaymentAuthorizationResponse(
                IsResponseCodeSuccess(resultCode), isAuthorized, amount, textmsg, receiptNumber)
            {
                ResponseCode = resultCode,
                TransactionId = orderid,
                SavedCard = savedCardResponse
            };

            var realExResponse = new RealExServerResponse();

            StringBuilder displayText = new StringBuilder();
            if (!response.ResponseOk)
            {

                realExResponse.ImagePath = additionalParameters.FailureImageUrl;
                realExResponse.ConfigDescription = "Payment failed";

                displayText.Append("There was an error while processing your payment.<br />Please contact <a href=\"mailto:support@myplayservice.co.uk\">My Play Service Support</a><br />");

                displayText.AppendFormat($"ERROR DETAILS : {textmsg}");
            }
            else
            {
                // TODO: show AUTHCODE to user (a legal requirement?) and an absolute hyperlink back in to the ebooking application

                realExResponse.ConfigDescription = "Payment Success";

                displayText.AppendFormat($"Payment Reference Number = {response.ReceiptNumber}<br />");
                displayText.AppendFormat($"Amount Paid = {response.AmountAuthorized:c}<br />");
                displayText.AppendFormat("Booking Reference Number {0}<br />", additionalParameters.Reference);
                displayText.AppendFormat("A confirmation email will be sent shortly with all details and invoice");

                realExResponse.ImagePath = additionalParameters.SuccessImageUrl;
                realExResponse.AdditionalSuccessMessage = additionalParameters.AdditionalSuccessMessage;
            }
            realExResponse.MainResult = displayText.ToString();

            realExResponse.NextUrl = additionalParameters.NextUrl;

            PartialViewResult serverReply = PaymentFrameworkUtility.CreatePartialView("~/Views/Payment/ReplyToRealEx.cshtml", realExResponse);

            PaymentAuthorizationGuiResponse guiResponse = new PaymentAuthorizationGuiResponse(response, serverReply);
            return Task.FromResult<PaymentAuthorizationGuiResponse>(guiResponse);
        }
    }
}
