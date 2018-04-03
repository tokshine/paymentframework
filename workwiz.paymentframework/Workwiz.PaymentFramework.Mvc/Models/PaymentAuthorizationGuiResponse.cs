using System;
using System.Web.Mvc;
using Workwiz.PaymentFramework.Shared.Models;

namespace Workwiz.PaymentFramework.Mvc.Models
{
    public class PaymentAuthorizationGuiResponse : IPaymentAuthorizationResponse
    {
        public PaymentAuthorizationGuiResponse(PaymentAuthorizationResponse backendAuthResponse,
            ActionResult serverReply)
        {
            if (backendAuthResponse == null)
            {
                throw new ArgumentNullException(nameof(backendAuthResponse));
            }
            this.AuthorizationResponse = backendAuthResponse;
            this.ServerReply = serverReply;
        }

        public ActionResult ServerReply { get; set; }

        public PaymentAuthorizationResponse AuthorizationResponse { get; set; }

        #region IPaymentAuthorizationResponse delegated to AuthorizationResponse

        public decimal AmountAuthorized => this.AuthorizationResponse.AmountAuthorized;

        public PaymentAuthorizationResult AuthorizationResult => this.AuthorizationResponse.AuthorizationResult;

        public string Description => this.AuthorizationResponse.Description;

        public string ReceiptNumber => this.AuthorizationResponse.ReceiptNumber;

        public string ResponseCode => this.AuthorizationResponse.ResponseCode;

        public bool ResponseOk => this.AuthorizationResponse.ResponseOk;

        public SavedCardResponse SavedCard => this.AuthorizationResponse.SavedCard;

        public string TransactionId => this.AuthorizationResponse.TransactionId;

        #endregion
    }
}