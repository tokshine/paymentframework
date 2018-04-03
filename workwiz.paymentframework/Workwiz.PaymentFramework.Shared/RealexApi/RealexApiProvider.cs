using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Workwiz.Common.Logging;
using Workwiz.PaymentFramework.Shared.Models;
using Workwiz.PaymentFramework.Shared.RealexApi.RealAuth;
using Workwiz.PaymentFramework.Shared.RealexApi.RealVault;

namespace Workwiz.PaymentFramework.Shared.RealexApi
{
    public class RealexApiProvider : IPaymentProviderBackend
    {
        public RealexApiProvider(IWorkwizLoggerFactory loggerFactory)
        {
            this.Logger = loggerFactory;
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
        }

        protected IWorkwizLoggerFactory Logger { get; }
        public ProviderType ProviderType => ProviderType.Realex;

        public virtual ProviderFeatures SupportedFeatures =>
            ProviderFeatures.CheckAuthorization |
            ProviderFeatures.UpdateCardholder |
            ProviderFeatures.DeleteSavedCard;

        public async Task<IPaymentAuthorizationResponse> CheckAuthorization(
            PaymentProviderConfiguration configuration,
            string uniqueIdentifier,
            decimal expectedAmount,
            string paymentProviderUniqueRefrence)
        {
            QueryPaymentResponse response = await RealexQueryPayment(configuration, uniqueIdentifier, expectedAmount, subAccount: String.Empty);
            PaymentAuthorizationResponse result = new PaymentAuthorizationResponse(
                responseOk: response.HttpStatus == HttpStatusCode.OK,
                response: response.ResultMappedToFrameworkResult,
                amount: expectedAmount,
                description: response.Message,
                receiptNumber: response.AuthCode);
            return result;
        }

        /// <remarks>
        /// Wraps calls to Realex's 'Query' API function
        /// </remarks>
        public async Task<QueryPaymentResponse> RealexQueryPayment(
            PaymentProviderConfiguration configuration,
            string uniqueIdentifier,
            decimal expectedAmount,
            string subAccount)
        {
            QueryPaymentRequest qRequest = new QueryPaymentRequest(merchantId:
                configuration.AccountIdentifer,
                originalOrderId: uniqueIdentifier);
            qRequest.SubAccount = subAccount;

            QueryPaymentResponse response = await PostXmlToRealex<QueryPaymentResponse, QueryPaymentRequest>(
                qRequest,
                secretKey: configuration.SharedSecret,
                logLevelForSuccess: LogLevel.Debug);
            return response;
        }

        public async Task<string> UpdateCardholderDetails(
            PaymentProviderConfiguration configuration,
            CardholderDetails cardholder)
        {
            var realExXfer = new PayerEditRequest(configuration.AccountIdentifer, cardholder.PayerRef)
            {
                OrderId = Guid.NewGuid().ToString(),
            };
            realExXfer.Payer.Title = cardholder.Title;
            realExXfer.Payer.FirstName = cardholder.FirstName;
            realExXfer.Payer.Surname = cardholder.Surname;
            realExXfer.Payer.PayerType = "Customer";
            realExXfer.Payer.Address = new RealexAddress()
            {
                Line1 = cardholder.Address1,
                Line2 = cardholder.Address2,
                Line3 = cardholder.Address3,
                City = cardholder.City,
                County = cardholder.County,
                Postcode = cardholder.Postcode,
                Country =
                    new RealexCountry()
                    {
                        CountryCode = "GB",
                        Name = "United Kingdom"
                    }
            };
            realExXfer.Payer.PhoneNumbers = new RealexPhoneNumbers()
            {
                Home = cardholder.HomePhone,
                Work = cardholder.WorkPhone,
                Mobile = cardholder.MobilePhone
            };
            realExXfer.Payer.EMail = cardholder.Email;
            realExXfer.Payer.Comments = RealexComment.CreateCommentList(cardholder.Comments, maxNumberOfComments: 2);

            PayerEditResponse result = await PostXmlToRealex<PayerEditResponse, PayerEditRequest>(
                realExXfer,
                configuration.SharedSecret,
                logLevelForSuccess: LogLevel.Audit);

            result.ThrowIfNotSuccess();
            return result.PasRef;
        }

        public async Task<string> CancelSavedCard(
            PaymentProviderConfiguration configuration,
            string cardOwnerIdentifer,
            string savedCardIdentified)
        {
            CardCancelCardRequest req = new CardCancelCardRequest(
                merchantId: configuration.AccountIdentifer,
                payerRef: cardOwnerIdentifer,
                savedCardName: savedCardIdentified);

            CardCancelCardResponse response =
                await PostXmlToRealex<CardCancelCardResponse, CardCancelCardRequest>(
                    req,
                    configuration.SharedSecret,
                    logLevelForSuccess: LogLevel.Audit);

            response.ThrowIfNotSuccess();
            return response.PasRef;
        }

        /// <remarks>
        /// Wraps calls to Realex's ReceiptIn API function
        /// </remarks>
        public async Task<ReceiptInResponse> RealexReceiptIn(
            PaymentProviderConfiguration configuration,
            string realexPayerRef,
            decimal amount,
            string subAccount,
            string cardName,
            RealexTssInfo additionalInfo,
            IEnumerable<string> optionalComments)
        {
            ReceiptInRequest req = new ReceiptInRequest(
                merchantId: configuration.AccountIdentifer,
                payerRef: realexPayerRef,
                paymentMethod: cardName,
                amount: amount);
            req.Account = subAccount;
            if (RealEx.Default.RealVaultRemoteUrl.ToLower().Contains("sandbox"))
            {
                req.Account = req.Account + "test";
            }

            req.Comments = RealexComment.CreateCommentList(optionalComments);

            if (null != additionalInfo)
            {
                additionalInfo.TruncateAndStripDisallowed();
                req.TssInfo = additionalInfo;
            }

            ReceiptInResponse response = await PostXmlToRealex<ReceiptInResponse, ReceiptInRequest>(
                    req,
                    configuration.SharedSecret,
                    logLevelForSuccess: LogLevel.Audit);

            return response;
        }

        public async Task<TResponse> PostXmlToRealex<TResponse, TRequest>(
            TRequest request,
            string secretKey,
            LogLevel logLevelForSuccess)
            where TRequest : RealAuthRequest
            where TResponse : RealAuthResponse, new()
        {
            request.SetSha1Hash(secretKey);

            string postUrl = RealEx.Default.RealVaultRemoteUrl;
            var client = LoggingHttpHandler.CreateLoggingClient(
                this.Logger, 
                typeof(RealexApiProvider),
                logLevelRequest: LogLevel.Info,
                logLevelResponse: LogLevel.Debug);
            // MUST use the XmlSerializer instead of default DataContractSerializer to honour the XmlAttribute and XmlRoot attributes
            XmlMediaTypeFormatter formatter = new XmlMediaTypeFormatter()
            {
                UseXmlSerializer = true
            };            
            HttpResponseMessage response = await client.PostAsync<TRequest>(
                postUrl,
                request,
                formatter,
                System.Threading.CancellationToken.None);

            string responseFromServer = await response.Content.ReadAsStringAsync();

            TResponse parsedResponse = RealAuthResponseParser<TResponse>.DeserializeFromString(
                responseFromServer,
                response.StatusCode,
                request);

            LogLevel levelToLog = parsedResponse.IsSuccess ? logLevelForSuccess : LogLevel.Error;
            this.Logger.CreateEntry(typeof(RealexApiProvider), levelToLog,
                $"StatusCode {(int) response.StatusCode} content {responseFromServer}");

            if (!parsedResponse.IsSha1HashCorrect(secretKey))
            {
                this.Logger.CreateEntry(typeof(RealexApiProvider), LogLevel.Warn,
                    $"Incorrect Sha1Hash {responseFromServer}");
            }

            return parsedResponse;
        }

        public static string CalculateRealexSignature(
            string[] fieldsToSign,
            string secretKey)
        {
            string concatenatedInput = string.Join(".", fieldsToSign);

            SHA1 sha = new SHA1CryptoServiceProvider();
            ASCIIEncoding encoder = new ASCIIEncoding();

            byte[] intermediateHashBytes = sha.ComputeHash(encoder.GetBytes(concatenatedInput));
            string intermediateHashString = MessageContentUtility.ByteArrayToHexString(intermediateHashBytes).ToLower();

            string finalStringToHash = $"{intermediateHashString}.{secretKey}";
            byte[] finalHashBytes = sha.ComputeHash(encoder.GetBytes(finalStringToHash));
            string finalHashedString = MessageContentUtility.ByteArrayToHexString(finalHashBytes).ToLower();

            return finalHashedString;
        }
       
    }
}