using System.Collections.Generic;
using Workwiz.PaymentFramework.Shared.Models;

namespace Workwiz.PaymentFramework.Mvc.Models
{
    public class GeneralisedPaymentTransfer
    {
        public GeneralisedPaymentTransfer()
        {
            LineItems = new List<CivicaLineItem>();
        }
        public string TransactionId { get; set; }
        public decimal Amount { get; set; }
        public SaveCard SaveCard { get; set; }
        public string Account { get; set; }
        public string Comment1 { get; set; }
        public string Comment2 { get; set; }
        public string CustomerNumber { get; set; }
        public string VariableReference { get; set; }
        public string ProductId { get; set; }
        public string ReturnUrl { get; set; }
        public string GeneralLedgerCode { get; set; }
        public bool IsMediated { get; set; }       
        public string VatCode { get; set; }
        public decimal VatRate { get; set; }
        public List<CivicaLineItem> LineItems { get; set; }
    }
}
