using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Workwiz.PaymentFramework.Shared.Models
{
    public class CapitaInvokeResponse
    {
        public string ScpReference { get; set; }
        public string RedirectUrl { get; set; }
        public bool Error { get; set; }
        public string ErrorMessage { get; set; }
    }
}
