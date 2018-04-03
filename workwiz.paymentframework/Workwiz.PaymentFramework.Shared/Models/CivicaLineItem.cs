using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Workwiz.PaymentFramework.Shared.Models
{
    public class CivicaLineItem
    {
        public string Reference { get; set; }
        public string FundCode { get; set; }
        public decimal Amount { get; set; }
        public string Narrative { get; set; }
    }
}
