using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Workwiz.PaymentFramework.TestWeb.Models
{
    public class PaymentSuccessResponse
    {
        public  string Reference { get; set; }
        public string ReceiptId { get; set; }
        public decimal Amount { get; set; }
    }
}