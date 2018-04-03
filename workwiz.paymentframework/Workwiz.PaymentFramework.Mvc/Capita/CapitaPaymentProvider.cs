using System;
using System.Collections.Specialized;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using Workwiz.Common.Logging;
using Workwiz.PaymentFramework.Mvc.Models;
using Workwiz.PaymentFramework.Shared;
using Workwiz.PaymentFramework.Shared.CapitaApi;
using Workwiz.PaymentFramework.Shared.Models;
using Workwiz.PaymentFramework.Shared.RealexApi;

namespace Workwiz.PaymentFramework.Mvc.Capita
{
    public class CapitaPaymentProvider : CapitaApiProvider, IPaymentProvider
    {
        public override ProviderFeatures SupportedFeatures =>
            base.SupportedFeatures |
            ProviderFeatures.MediatedModeGui;

        public string RoundTripTokenKey => "RequestId";

        public CapitaPaymentProvider(IWorkwizLoggerFactory loggerFactory) : base(loggerFactory)
        {
        }

        public string GenerateUniqueIdentifier()
        {
            return Guid.NewGuid().ToString();
        }

        public ActionResult SendToPaymentProvider(
            PaymentProviderConfiguration configuration,
            GeneralisedPaymentTransfer transferObject,
            Action<PaymentProviderConfiguration, GeneralisedPaymentTransfer, string> saveProviderReference)
        {
            int siteId;
            int.TryParse(configuration.AccountIdentifer, out siteId);

            int scpId;
            int.TryParse(transferObject.Account, out scpId);

            int hmacKeyId;
            string hmacSecretKey;

            CapitaApiHelpers.GetHmacIdAndSecretKey(configuration.SharedSecret, out hmacKeyId, out hmacSecretKey);

            string returnUrl = $"{transferObject.ReturnUrl}?{RoundTripTokenKey}={transferObject.TransactionId}";

            CapitaInvokeRequest request = new CapitaInvokeRequest()
            {
                SiteId = siteId,
                ScpId = scpId,
                HmacKeyId = hmacKeyId,
                HmacKey = hmacSecretKey,
                UniqueReference =  transferObject.TransactionId,
                PurchaseId = transferObject.ProductId,
                BookingRef = transferObject.Comment2,
                PurchaseDescription = transferObject.Comment1,
                PaymentTotal = (int)(transferObject.Amount * 100),
                ReturnUrl = returnUrl,
                IntegraCode = transferObject.GeneralLedgerCode,
                IsMediated = transferObject.IsMediated,
                FundCode = Shared.Capita.Default.FundCode,
                VatCode = transferObject.VatCode,
                VatRate = transferObject.VatRate
            };

            if (transferObject.SaveCard != null)
            {
                request.SaveCard = true;
                request.CardHolderId = MessageContentUtility.TruncateAndStripDisallowed(transferObject.SaveCard.PayerReference, 50, null);
            }

            //Call Capita web service to set up the payment
            CapitaInvokeResponse response = InvokeRequest(request);
            
            if (response != null && !response.Error)
            {
                //call this action method to save scpReference into PendingPayment table 
                saveProviderReference(configuration, transferObject, response.ScpReference);
                RedirectResult resultView = new RedirectResult(response.RedirectUrl, true);
                var sendToPaymentLogMessage = PaymentFrameworkUtility.DescribeActionResultForLogging(resultView, true);
                this.Logger.CreateEntry(typeof(CapitaPaymentProvider), LogLevel.Info, sendToPaymentLogMessage);
                return resultView;
            }
            else
            {
                string errorMessage = "Capita Server returns null response.";
                if (response != null)
                    errorMessage += " " + response.ErrorMessage;

                this.Logger.CreateEntry(typeof(CapitaPaymentProvider), LogLevel.Error, errorMessage);
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }

           
        }

        public async Task<PaymentAuthorizationGuiResponse> ProcessResponse(
            PaymentProviderConfiguration configuration,
            NameValueCollection paramsCollection,
            ResponseParameters additionalParameters)
        {
            string paramsDesc = PaymentFrameworkUtility.DescribeNameValueCollection(paramsCollection);
            this.Logger.CreateEntry(typeof(CapitaPaymentProvider), LogLevel.Debug, $"ProcessResponse({paramsDesc})");

            string roundTripTransactionReference = paramsCollection[RoundTripTokenKey];
            if (String.IsNullOrEmpty(roundTripTransactionReference))
            {
                throw new InvalidOperationException($"response did not include required field '{RoundTripTokenKey}'");
            } 

            Guid pendingPaymentRef = new Guid(roundTripTransactionReference);

            string paymentProviderTransactionReference = additionalParameters.ProviderReference;
            if (String.IsNullOrEmpty(paymentProviderTransactionReference))
            {
                throw new InvalidOperationException("response did not include required field \'PaymentProviderTransactionReference\'");
            }

            var backendResponse = await CheckAuthorizationInternal(configuration, pendingPaymentRef.ToString(), paymentProviderTransactionReference);

            return new PaymentAuthorizationGuiResponse(backendResponse, null);
           
        }
    
       
    }
}
