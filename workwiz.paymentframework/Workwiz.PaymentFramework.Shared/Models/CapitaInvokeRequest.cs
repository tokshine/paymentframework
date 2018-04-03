using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Workwiz.PaymentFramework.Shared.Models
{
    public class CapitaInvokeRequest
    {
        public int SiteId { get; set; }
        public int ScpId { get; set; }
        public int HmacKeyId { get; set; }
        public string HmacKey { get; set; }
        public string UniqueReference { get; set; }
        public int PaymentTotal { get; set; }
        public string ReturnUrl { get; set; }
        public string PurchaseId { get; set; }
        public string BookingRef { get; set; }
        public string PurchaseDescription { get; set; }
        public string IntegraCode { get; set; }
        public bool SaveCard { get; set; }
        public string CardHolderId { get; set; }
        public bool IsMediated { get; set; }
        public string FundCode { get; set; }
        public string VatCode { get; set; }
        public decimal VatRate { get; set; }

    }
}
