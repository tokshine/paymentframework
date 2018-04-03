using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Workwiz.Common.Logging;
using Workwiz.PaymentFramework.Mvc.Models;
using Workwiz.PaymentFramework.Shared;
using Workwiz.PaymentFramework.Shared.CivicaApi;
using Workwiz.PaymentFramework.Shared.CivicaAuthRequest;
using Workwiz.PaymentFramework.Shared.Models;

namespace Workwiz.PaymentFramework.Mvc.Civica
{
    public class CivicaPaymentProvider : CivicaApiProvider, IPaymentProvider
    {
        public override ProviderFeatures SupportedFeatures =>
            base.SupportedFeatures |
            ProviderFeatures.MediatedModeGui;

        public string RoundTripTokenKey => "CallingApplicationTransactionReference";

        public CivicaPaymentProvider(IWorkwizLoggerFactory loggerFactory) : base(loggerFactory)
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
            var civicaXfer = new CivicaPaymentTransfer()
            {
                ProviderUrl = Shared.Civica.Default.CivicaPostUrl,
                ReturnUrl = transferObject.ReturnUrl,
                CallingApplicationId = Shared.Civica.Default.CivicaAppId,
                CallingApplicationTransactionReference = transferObject.TransactionId,
                GeneralLedgerCode = $"{transferObject.GeneralLedgerCode}-{transferObject.ProductId}",
                //PaymentSourceCode = transferObject.PaymentSourceCode,
                PaymentSourceCode = transferObject.IsMediated ? "02" : "01",
                PaymentTotal = transferObject.Amount,
                PaymentLines = new List<string>()
            };

            foreach (var item in transferObject.LineItems)
            {
                civicaXfer.PaymentLines.Add(FormatPaymentLine(item));
            }
            var resultView =  PaymentFrameworkUtility.CreateView("~/Views/Payment/SendToCivica.cshtml", civicaXfer);

            var sendToPaymentLogMessage = PaymentFrameworkUtility.DescribeActionResultForLogging(resultView, true);
            this.Logger.CreateEntry(typeof(CivicaPaymentProvider), LogLevel.Info, sendToPaymentLogMessage);
            return resultView;
        }

        public async Task<PaymentAuthorizationGuiResponse> ProcessResponse(
            PaymentProviderConfiguration configuration,
            NameValueCollection paramsCollection,
            ResponseParameters additionalParameters)
        {
            string paramsDesc = PaymentFrameworkUtility.DescribeNameValueCollection(paramsCollection);
            Logger.CreateEntry(typeof(CivicaPaymentProvider), LogLevel.Debug, $"ProcessResponse({paramsDesc})");

            string responseCode = paramsCollection["ResponseCode"] ?? "-1";

            int responseCodeParsed = -1;
            bool didParse = Int32.TryParse(responseCode, out responseCodeParsed);
            if (didParse && (0 == responseCodeParsed))
            {

                string roundTripTransactionReference = paramsCollection["CallingApplicationTransactionReference"];
                if (String.IsNullOrEmpty(roundTripTransactionReference))
                {
                    throw new InvalidOperationException(String.Format("response did not include required field '{0}'",
                        "CallingApplicationTransactionReference"));
                } 

                Guid pendingPaymentRef = new Guid(roundTripTransactionReference);

                var backendResponse = await CheckAuthorizationInternal(configuration, pendingPaymentRef.ToString());
                backendResponse.ResponseCode = responseCode;
                backendResponse.TransactionId = roundTripTransactionReference;
                return new PaymentAuthorizationGuiResponse(backendResponse, null);
            }
            else
            {
                var backendResponse = new PaymentAuthorizationResponse(false, PaymentAuthorizationResult.Declined, 
                    !string.IsNullOrEmpty(paramsCollection["PaymentAmount"]) ? decimal.Parse(paramsCollection["PaymentAmount"]) : 0.00m,
                    paramsCollection["ResponseDescription"],
                    paramsCollection["IncomeManagementReceiptNumber"])
                {
                    ResponseCode = responseCode,
                    TransactionId = paramsCollection["CallingApplicationTransactionReference"]
                };
                return new PaymentAuthorizationGuiResponse(backendResponse, null);
            }
        }

        public string EncodeStringForPaymentRow(string inputString, int fieldTruncationLength)
        {
            string result = inputString.Replace('|', ' ');
            if (result.Length > fieldTruncationLength)
            {
                result = result.Substring(0, fieldTruncationLength);
            }
            return result;
        }        

        public string FormatPaymentLine(CivicaLineItem lineItem)
        {
            return
                $"{EncodeStringForPaymentRow(lineItem.Reference, 35)}|{EncodeStringForPaymentRow(lineItem.FundCode, 5)}|{lineItem.Amount:0.00}||{EncodeStringForPaymentRow(lineItem.Narrative, 100)}||||||||";
        }
    }
}
