using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workwiz.Common.Logging;
using Workwiz.PaymentFramework.Shared.CivicaAuthRequest;
using Workwiz.PaymentFramework.Shared.Models;

namespace Workwiz.PaymentFramework.Shared.CivicaApi
{
    public class CivicaApiProvider : IPaymentProviderBackend
    {
        public ProviderType ProviderType => ProviderType.Civica;

        public virtual ProviderFeatures SupportedFeatures => ProviderFeatures.CheckAuthorization;

        protected IWorkwizLoggerFactory Logger { get; }

        public CivicaApiProvider(IWorkwizLoggerFactory loggerFactory )
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

        public async Task<IPaymentAuthorizationResponse> CheckAuthorization(
            PaymentProviderConfiguration configuration,
            string uniqueIdentifier,
            decimal expectedAmount,
            string paymentProviderUniqueRefrence)
        {
            return await CheckAuthorizationInternal(configuration, uniqueIdentifier);
        }

        protected async Task<PaymentAuthorizationResponse> CheckAuthorizationInternal(
            PaymentProviderConfiguration configuration,
            string uniqueIdentifier)
        {

            var request = CreateCivicaRequest(Shared.Civica.Default.CivicaAppId, uniqueIdentifier);
            RespMessage civicaResponse;
            using (QueryAuthRequestSrvSoapClient civicaClient = new QueryAuthRequestSrvSoapClient("QueryAuthRequestSrvSoap", Shared.Civica.Default.CivicaQueryUrl))
            {
                civicaResponse = await civicaClient.QueryAsync(request);
            }

            return ParseResponseFromCivicaResponse(civicaResponse);
        }

        private ReqMessage CreateCivicaRequest(
            string callingApplicationIdentifier,
            string callingApplicationTransactionReference)
        {
            CriteriaStructure queryCriteria = new CriteriaStructure();
            queryCriteria.BooleanOp = AndOr.And;
            queryCriteria.Operator = "=";
            // 'paylink_payment_ref' == CallingApplicationTransactionReference, see "SRV - QueryPayments - v1.00r8.doc"
            // queryCriteria.Column = "paylink_payment_ref";
            // for QueryAuthRequests : from PaylinkXML UI User Guide v1.1.doc
            queryCriteria.Column = "callingapplicationtransactionreference";
            queryCriteria.Value = callingApplicationTransactionReference;

            ReqMessage query = new ReqMessage();
            query.CallingApplicationIdentifier = callingApplicationIdentifier;
            //query.pageNum = "1";
            //query.pageSize = "2";
            query.CriteraList = new CriteriaStructure[]
                {
                    queryCriteria
                };
            return query;
        }

        private PaymentAuthorizationResponse ParseResponseFromCivicaResponse(RespMessage civicaResponse)
        {
            if (null == civicaResponse)
            {
                return new PaymentAuthorizationResponse(true, PaymentAuthorizationResult.ErrorUnknownStatus, 0, "null response", null);
            }

            switch (civicaResponse.ResponseCode)
            {
                case "00000":
                    // continue processing
                    break;
                case "00001":
                    // record not found : either user is still in the GUI or abandonded the process
                    return new PaymentAuthorizationResponse(true, PaymentAuthorizationResult.Unknown, 0, civicaResponse.ResponseDescription, null);
                default:
                    string failureReason = String.Format("Response code = {0}, Description = '{1}'",
                        civicaResponse.ResponseCode,
                        civicaResponse.ResponseDescription);
                    return new PaymentAuthorizationResponse(true, PaymentAuthorizationResult.ErrorUnknownStatus, 0, failureReason, null);
            }

            bool anyAuthorized = false;
            bool anyDeclined = false;
            decimal totalPayed = 0;
            List<String> receiptNumbers = new List<string>();
            if (null != civicaResponse.PaymentList)
            {
                foreach (PaymentStructure payStruct in civicaResponse.PaymentList)
                {
                    // workaround for Issue (SR-985 : civica not returning any value for TrueAccountReference)
                    if (String.IsNullOrEmpty(payStruct.TrueAccountReference))
                    {
                        payStruct.TrueAccountReference = payStruct.AccountReference;
                    }

                    // according to documentation, additional charges (e.g. card charges on top of the bill) have a different TrueAccountReference
                    if (payStruct.AccountReference == payStruct.TrueAccountReference)
                    {
                        if ("A" == payStruct.RequestStatus)
                        {
                            totalPayed += payStruct.AccountPaymentAmount;
                            if (!String.IsNullOrEmpty(payStruct.IncomeManagementReceiptNumber))
                            {
                                receiptNumbers.Add(payStruct.IncomeManagementReceiptNumber);
                            }
                            anyAuthorized = true;
                        }
                        else
                        {
                            anyDeclined = true;
                        }
                    }
                }
            }

            string uniqueReceipts = String.Join(", ", receiptNumbers.Distinct().ToArray());

            if (anyAuthorized)
            {
                if (anyDeclined)
                {
                    return new PaymentAuthorizationResponse(
                        true,
                        PaymentAuthorizationResult.ErrorUnknownStatus,
                        totalPayed,
                        "Payment contains both authorized and not-authorized payments",
                        uniqueReceipts);
                }
                else
                {
                    return new PaymentAuthorizationResponse(
                        true,
                        PaymentAuthorizationResult.Authorized,
                        totalPayed,
                        "Authorized",
                        uniqueReceipts);
                }
            }
            else
            {
                if (anyDeclined)
                {
                    return new PaymentAuthorizationResponse(true, PaymentAuthorizationResult.Declined, totalPayed, "Payment not authorized", uniqueReceipts);
                }
                else
                {
                    return new PaymentAuthorizationResponse(true, PaymentAuthorizationResult.ErrorUnknownStatus, 0, "PaymentList is empty", uniqueReceipts);
                }
            }
        }
    }
}
