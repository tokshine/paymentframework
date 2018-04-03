using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Workwiz.PaymentFramework.Shared.Models
{
    public class SaveCard
    {
        public bool OfferSaveCard { get; set; }
        public string PayerReference { get; set; }
        public string PaymentReference { get; set; }
        public bool PayerExists { get; set; }
    }
}
