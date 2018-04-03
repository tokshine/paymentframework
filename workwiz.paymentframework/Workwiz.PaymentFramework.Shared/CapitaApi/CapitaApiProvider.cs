using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.ServiceModel;
using System.ServiceModel.Configuration;
using System.Text;
using System.Threading.Tasks;
using Workwiz.Common.Logging;
using Workwiz.PaymentFramework.Shared.CapitaSimple;
using Workwiz.PaymentFramework.Shared.CivicaAuthRequest;
using Workwiz.PaymentFramework.Shared.Models;

namespace Workwiz.PaymentFramework.Shared.CapitaApi
{
    public class CapitaApiProvider : IPaymentProviderBackend
    {
        public ProviderType ProviderType => ProviderType.Capita;

        public virtual ProviderFeatures SupportedFeatures => ProviderFeatures.CheckAuthorization;

        protected IWorkwizLoggerFactory Logger { get; }

        public CapitaApiProvider(IWorkwizLoggerFactory loggerFactory)
        {
            this.Logger = loggerFactory;
        }

        public Task<string> UpdateCardholderDetails(
            PaymentProviderConfiguration configuration,
            CardholderDetails cardholder)
        {
            throw new NotImplementedException();
        }

        public Task<string> CancelSavedCard(
            PaymentProviderConfiguration configuration,
            string cardOwnerIdentifer,
            string savedCardIdentified)
        {
            throw new NotImplementedException();
        }

        public CapitaInvokeResponse InvokeRequest(CapitaInvokeRequest request)
        {
            scpSimpleInvokeRequest scpInvokeRequest = CreateCapitaInvokeRequest(request);
            CapitaInvokeResponse response = new CapitaInvokeResponse();
            using (scpClient capitaClient = new scpClient("CapitaScpSoap", Shared.Capita.Default.CapitaWebServiceUrl))
            {
                try
                {
                    scpInvokeResponse scpResponse = capitaClient.scpSimpleInvoke(scpInvokeRequest);
                    string errorMessage = string.Empty;
                    if (scpResponse?.invokeResult != null)
                    {
                        if (scpResponse.requestId == request.UniqueReference)
                        {
                            if (scpResponse.transactionState == transactionState.INVALID_REFERENCE)
                            {
                                errorMessage = "Transaction aborted! It may be because of session time out or some other technical glitch from Capita";
                            }
                            else
                            {
                                response.ScpReference = scpResponse.scpReference;
                                if (scpResponse.invokeResult.status == status.SUCCESS)
                                {
                                    response.RedirectUrl = (string)scpResponse.invokeResult.Item;

                                }
                                else
                                {
                                    errorDetails item = scpResponse.invokeResult.Item as errorDetails;
                                    errorMessage = item != null ? $"ErrorId: {item.errorId}, Message: {item.errorMessage}" : "Transaction failed for some unknown reason.";
                                }
                            }

                        }
                        else
                        {
                            errorMessage = "Transaction is being invalidated b/c the unique reference returned from Capita is wrong.";
                        }
                    }

                    if (!string.IsNullOrEmpty(errorMessage))
                    {
                        response.Error = true;
                        response.ErrorMessage = errorMessage;
                    }
                }
                catch (Exception ex)
                {
                    response.Error = true;
                    response.ErrorMessage = "Fatal error";
                }

                return response;
            }
        }

        public async Task<IPaymentAuthorizationResponse> CheckAuthorization(
            PaymentProviderConfiguration configuration,
            string uniqueIdentifier,
            decimal expectedAmount,
            string paymentProviderUniqueRefrence)
        {
            return await CheckAuthorizationInternal(configuration, uniqueIdentifier, paymentProviderUniqueRefrence);
        }

        protected async Task<PaymentAuthorizationResponse> CheckAuthorizationInternal(
            PaymentProviderConfiguration configuration,
            string uniqueIdentifier, string scpUniqueReference)
        {
            int siteId;
            int scpId;
            int hmacKeyId;
            string hmacSecretKey;

            int.TryParse(configuration.AccountIdentifer, out siteId);
            int.TryParse(configuration.SubAccountNumber, out scpId);

            CapitaApiHelpers.GetHmacIdAndSecretKey(configuration.SharedSecret, out hmacKeyId, out hmacSecretKey);

            scpQueryRequest request = CreateCapitaQueryRequest(siteId, scpId, hmacKeyId, hmacSecretKey, scpUniqueReference, uniqueIdentifier);
            scpSimpleQueryResponse1 response;            

            try
            {
                using (CapitaSimple.scpClient capitaClient = new scpClient("CapitaScpSoap", Shared.Capita.Default.CapitaWebServiceUrl))
                {
                    try
                    {
                        response = await capitaClient.scpSimpleQueryAsync(request);
                    }
                    catch (Exception ex)
                    {
                        this.Logger.CreateEntry(typeof(CapitaApiProvider), LogLevel.Error, $"Token:{uniqueIdentifier},ProviderToken:{scpUniqueReference}", ex);
                        return new PaymentAuthorizationResponse(false, PaymentAuthorizationResult.ErrorUnknownStatus, 0, "Error occurred: Provider returned an error in response", null);
                    }

                }
            }
             //This is a quick fix to cover the scenario where we're calling from AzureFunction and AzureFunction does not have any app.config
            //So we need to build the bindings and endpoint in the code
            catch (InvalidOperationException invalidOperationException)
            {
                // Create endpoint address gathered from functions app settings
                var address = new EndpointAddress(Shared.Capita.Default.CapitaWebServiceUrl);

                // Create binding to match the client proxy configuration
                var binding = new BasicHttpBinding(BasicHttpSecurityMode.Transport);
                binding.Security.Mode = BasicHttpSecurityMode.Transport;

                using (CapitaSimple.scpClient capitaClient = new scpClient(binding, address))
                {
                    try
                    {
                        response = await capitaClient.scpSimpleQueryAsync(request);
                    }
                    catch (Exception ex)
                    {
                        this.Logger.CreateEntry(typeof(CapitaApiProvider), LogLevel.Error, $"Token:{uniqueIdentifier},ProviderToken:{scpUniqueReference}", ex);
                        return new PaymentAuthorizationResponse(false, PaymentAuthorizationResult.ErrorUnknownStatus, 0, "Error occurred: Provider returned an error in response", null);
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }



            return ParseResponseFromCapitaResponse(response);
        }

        private scpSimpleInvokeRequest CreateCapitaInvokeRequest(CapitaInvokeRequest request)
        {
            scpSimpleInvokeRequest invokeRequest = new scpSimpleInvokeRequest
            {
                credentials = new credentials
                {
                    subject = new subject()
                    {
                        subjectType = subjectType.CapitaPortal,
                        identifier = request.ScpId,
                        systemCode = systemCode.SCP
                    },
                    requestIdentification = new requestIdentification()
                    {
                        uniqueReference = Guid.NewGuid().ToString(),
                        timeStamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss")
                    },
                    signature = new signature()
                    {
                        algorithm = algorithm.Original,
                        hmacKeyID = request.HmacKeyId
                    }
                }
            };

            string credentialsToHash = CapitaApiHelpers.GetCredentialsToHash(invokeRequest.credentials);
            invokeRequest.credentials.signature.digest = CapitaApiHelpers.CalculateDigest(request.HmacKey, credentialsToHash);
            invokeRequest.requestType = request.SaveCard ? requestType.payAndAutoStore : requestType.payOnly;

            //store card details
            if (request.SaveCard)
            {
                invokeRequest.additionalInstructions = new additionalInstructions()
                {
                    cardholderID = request.CardHolderId
                };
            }

            invokeRequest.requestId = request.UniqueReference;

            //ECOM – Indicates that the cardholder will be interacting directly with the web pages displayed by the SCP. 
            //CNP – Indicates that the transaction is being processed by a third party(e.g.a telesales operative) on behalf of the cardholder.
            //N.B. In order to comply with bank rules and to ensure that 3 - D Secure authentication is processed correctly it is important to set this element correctly.
            invokeRequest.panEntryMethod = request.IsMediated ? panEntryMethod.CNP : panEntryMethod.ECOM;

            invokeRequest.routing = new routing()
            {
                siteId = request.SiteId,
                scpId = request.ScpId,
                returnUrl = request.ReturnUrl
            };

            invokeRequest.sale = new simpleSale()
            {
                saleSummary = new summaryData()
                {
                    description = request.PurchaseDescription,
                    reference = request.IntegraCode,
                    amountInMinorUnits = request.PaymentTotal,
                    displayableReference = $"Booking reference : {request.BookingRef}"
                },
                items = new[]
                {
                    new simpleItem()
                    {
                        itemSummary = new summaryData()
                        {
                            description = request.PurchaseDescription,
                            amountInMinorUnits = request.PaymentTotal,
                            reference = request.IntegraCode,
                            displayableReference =  $"Booking reference : {request.BookingRef}"
                        },
                        lgItemDetails = new lgItemDetails()
                        {
                            additionalReference = request.PurchaseId,
                            fundCode  = string.IsNullOrEmpty(request.FundCode) ? null : request.FundCode,
                            isFundItem = string.IsNullOrEmpty(request.FundCode) ? false : true
                        },
                        lineId = request.PurchaseId,
                        tax = string.IsNullOrEmpty(request.VatCode) ? null :
                                new taxItem()
                                {
                                    vat = new vatItem ()
                                    {
                                        vatCode = request.VatCode,
                                        vatRate = request.VatRate
                                    }
                                }
                    }
                }
            };

            return invokeRequest;
        }

        private scpQueryRequest CreateCapitaQueryRequest(
            int customerSiteId,
            int customerScpId,
            int hmacKeyId,
            string secretKey,
            string scpUniqueReference,
            string callingApplicationTransactionReference)
        {
            scpQueryRequest queryRequest = new scpQueryRequest
            {
                siteId = customerSiteId,
                scpReference = scpUniqueReference,
                credentials = new credentials
                {
                    subject = new subject()
                    {
                        subjectType = subjectType.CapitaPortal,
                        identifier = customerScpId,
                        systemCode = systemCode.SCP
                    },
                    requestIdentification = new requestIdentification()
                    {
                        uniqueReference = callingApplicationTransactionReference,
                        timeStamp = DateTime.UtcNow.ToString("yyyyMMddhhmmss")
                    },
                    signature = new signature()
                    {
                        algorithm = algorithm.Original,
                        hmacKeyID = hmacKeyId
                    }
                }
            };



            string credentialsToHash = CapitaApiHelpers.GetCredentialsToHash(queryRequest.credentials);
            queryRequest.credentials.signature.digest = CapitaApiHelpers.CalculateDigest(secretKey, credentialsToHash);


            return queryRequest;
        }


        private PaymentAuthorizationResponse ParseResponseFromCapitaResponse(scpSimpleQueryResponse1 capitaResponse)
        {
            if (null == capitaResponse?.scpSimpleQueryResponse)
            {
                return new PaymentAuthorizationResponse(true, PaymentAuthorizationResult.ErrorUnknownStatus, 0, "null response", null);
            }

            switch (capitaResponse.scpSimpleQueryResponse.transactionState)
            {
                case transactionState.COMPLETE:
                    // continue processing
                    break;
                case transactionState.IN_PROGRESS:
                    return new PaymentAuthorizationResponse(true, PaymentAuthorizationResult.Unknown, 0, "Transaction is still in progress", null);
                case transactionState.INVALID_REFERENCE:
                    return new PaymentAuthorizationResponse(true, PaymentAuthorizationResult.Unknown, 0, "Transaction has been in validated", null);

            }


            string errorMessage = string.Empty;
            decimal amount = 0;
            errorDetails item = capitaResponse.scpSimpleQueryResponse.paymentResult.Item as errorDetails;
            if (item != null)
            {
                errorMessage = $"Response code = {capitaResponse.scpSimpleQueryResponse.paymentResult.status}, ErrorId: {item.errorId}, ErrorMsg: {item.errorMessage}";
            }
            else
            {
                string responseDetails = $"Capita Query Response for RequestId {capitaResponse.scpSimpleQueryResponse.requestId}: ScpReference = {capitaResponse.scpSimpleQueryResponse.scpReference} ";

                simplePayment paymentDetails = capitaResponse.scpSimpleQueryResponse.paymentResult.Item as simplePayment;
                if (paymentDetails != null)
                {
                    if (paymentDetails.paymentHeader != null)
                    {
                        responseDetails += $", TransactionDateUtc: {paymentDetails.paymentHeader.transactionDate.ToUniversalTime()}, MachineCode: {paymentDetails.paymentHeader.machineCode}, TransactionId: {paymentDetails.paymentHeader.uniqueTranId}";
                    }


                    authDetails authInfo = paymentDetails.Item as authDetails;
                    if (authInfo != null)
                    {
                        amount = authInfo.amountInMinorUnits;
                        //Convert minor unit amount value into larger unit. For example from pennies to pounds
                        amount = amount / 100m;
                        responseDetails += $", Amount (Minor Units): {authInfo.amountInMinorUnits}, AuthCode: {authInfo.authCode}, MerchantNumber: {authInfo.merchantNumber}";
                    }

                }

                this.Logger.CreateEntry(typeof(CapitaApiProvider), LogLevel.Info, responseDetails);

            }

            switch (capitaResponse.scpSimpleQueryResponse.paymentResult.status)
            {
                case status.SUCCESS:

                    CapitaSavedCardResponse cardResponse = null;
                    string storeCardDetailsFailureMessage = string.Empty;
                    if (capitaResponse.scpSimpleQueryResponse.storeCardResult != null)
                    {
                        if (capitaResponse.scpSimpleQueryResponse.storeCardResult.status != status.SUCCESS &&
                            capitaResponse.scpSimpleQueryResponse.storeCardResult.status != status.NOT_ATTEMPTED)
                        {
                            errorDetails storeCardErrorDetails =
                                capitaResponse.scpSimpleQueryResponse.storeCardResult.Item as errorDetails;

                            if (storeCardErrorDetails != null)
                            {
                                storeCardDetailsFailureMessage = $"ErrorId: {storeCardErrorDetails.errorId}, ErrorMessage: {storeCardErrorDetails.errorMessage}";
                            }
                            else
                            {
                                storeCardDetailsFailureMessage = "Error: Card details could not be stored. ";
                            }

                            cardResponse = new CapitaSavedCardResponse()
                            {
                                CardSaved = false,
                                CardSaveStatus = storeCardDetailsFailureMessage
                            };

                        }
                        else
                        {
                            storedCardDetails cardDetails = capitaResponse.scpSimpleQueryResponse.storeCardResult.Item as storedCardDetails;
                            if (cardDetails != null)
                            {
                                cardResponse = new CapitaSavedCardResponse()
                                {
                                    CardSaved = true,
                                    CardReference = cardDetails.storedCardKey.token,
                                    CardDigits = cardDetails.storedCardKey.lastFourDigits,
                                    CardDescription = cardDetails.cardDescription.ToString(),
                                    CardType = cardDetails.cardType.ToString(),
                                    ExpiryDate = Utility.ParseExpiryDate(cardDetails.expiryDate)
                                };
                            }

                        }
                    }

                    string description =
                        $"{storeCardDetailsFailureMessage} ScpReference = {capitaResponse.scpSimpleQueryResponse.scpReference} and RequestId = {capitaResponse.scpSimpleQueryResponse.requestId}.";

                    var authorizationResponse = new PaymentAuthorizationResponse(true, PaymentAuthorizationResult.Authorized, amount, description, capitaResponse.scpSimpleQueryResponse.requestId);

                    if (cardResponse != null)
                    {
                        authorizationResponse.SavedCard = cardResponse;
                    }

                    return authorizationResponse;

                case status.INVALID_REQUEST:
                case status.CARD_DETAILS_REJECTED:
                case status.CANCELLED:
                case status.LOGGED_OUT:
                case status.NOT_ATTEMPTED:
                    // record not found : either user is still in the GUI or abandonded the process
                    return new PaymentAuthorizationResponse(true, PaymentAuthorizationResult.Declined, 0, errorMessage, capitaResponse.scpSimpleQueryResponse.requestId);
                default:
                    return new PaymentAuthorizationResponse(true, PaymentAuthorizationResult.ErrorUnknownStatus, 0, errorMessage, capitaResponse.scpSimpleQueryResponse.requestId);
            }

        }
    }
}
