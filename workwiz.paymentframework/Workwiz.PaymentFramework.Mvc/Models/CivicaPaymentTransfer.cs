using System.Collections.Generic;

namespace Workwiz.PaymentFramework.Mvc.Models
{
    public class CivicaPaymentTransfer
    {
        public string CallingApplicationId { get; set; }
        public string CallingApplicationTransactionReference { get; set; }
        public decimal PaymentTotal { get; set; }
        public string GeneralLedgerCode { get; set; }
        public string ReturnUrl { get; set; }
        public string PaymentSourceCode { get; set; }
        public List<string> PaymentLines { get; set; }
        public string ProviderUrl { get; set; }
    }
}
