namespace Workwiz.PaymentFramework.Mvc.Models
{
    public class RealExPaymentTransfer
    {
        public string ProviderUrl { get; set; }
        public string MerchantId { get; set; }
        public string OrderId { get; set; }
        public int Amount { get; set; }
        public string Currency { get; set; }
        public string Timestamp { get; set; }
        public string Sha1Hash { get; set; }
        public bool HasSavedCard { get; set; }
        public string Account { get; set; }
        public bool? AutoSettleFlag { get; set; }
        public string Comment1 { get; set; }
        public string Comment2 { get; set; }
        public string CustomerNumber { get; set; }
        public string VariableReference { get; set; }
        public string ProductId { get; set; }
        public string ReturnUrl { get; set; }
        public bool CardStorageEnabled { get; set; }
        public bool OfferSaveCard { get; set; }
        public string PayerReference { get; set; }
        public string PaymentReference { get; set; }
        public bool PayerExists { get; set; }
    }
}
