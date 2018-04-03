using System.Collections.Generic;

namespace Workwiz.PaymentFramework.Mvc.Models
{
    public class CapitaPaymentTransfer
    {
        public string ScpReference { get; set; }
        public string RedirectUrl { get; set; }
        public bool Error { get; set; }
        public string ErrorMessage { get; set; }
    }
}
