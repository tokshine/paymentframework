namespace Workwiz.PaymentFramework.Shared.Models
{
    public interface IPaymentAuthorizationResponse
    {
        decimal AmountAuthorized { get; }
        PaymentAuthorizationResult AuthorizationResult { get; }
        string Description { get; }
        string ReceiptNumber { get; }
        string ResponseCode { get; }
        bool ResponseOk { get; }
        SavedCardResponse SavedCard { get; }
        string TransactionId { get;  }
    }
}