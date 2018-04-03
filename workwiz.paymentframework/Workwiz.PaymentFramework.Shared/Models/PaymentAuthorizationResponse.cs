
namespace Workwiz.PaymentFramework.Shared.Models
{

    public class PaymentAuthorizationResponse : IPaymentAuthorizationResponse
    {
        public bool ResponseOk { get; set; }
        public PaymentAuthorizationResult AuthorizationResult { get; set; }
        public decimal AmountAuthorized { get; set; }
        public string Description { get; set; }
        public string ReceiptNumber { get; set; }
        public string TransactionId { get; set; }
        public string ResponseCode { get; set; }
        public SavedCardResponse SavedCard { get; set; }

        public PaymentAuthorizationResponse(bool responseOk, PaymentAuthorizationResult response, decimal amount, string description, string receiptNumber)
        {
            ResponseOk = responseOk;
            AuthorizationResult = response;
            AmountAuthorized = amount;
            Description = description ?? string.Empty;
            ReceiptNumber = receiptNumber ?? string.Empty;
        }
    }
}
