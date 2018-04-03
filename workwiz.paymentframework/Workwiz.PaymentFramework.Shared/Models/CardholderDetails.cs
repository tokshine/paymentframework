using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Workwiz.PaymentFramework.Shared.Models
{
    public class CardholderDetails
    {
        public string OrderId{ get; set; }
        public string PayerRef{ get; set; }
        public string Title{ get; set; }
        public string FirstName{ get; set; }
        public string Surname{ get; set; }
        public string Company{ get; set; }
        public string Address1{ get; set; }
        public string Address2{ get; set; }
        public string Address3{ get; set; }
        public string City{ get; set; }
        public string County{ get; set; }
        public string Postcode{ get; set; }
//        public Tuple<string, string> Country{ get; set; }
        public string HomePhone{ get; set; }
        public string WorkPhone{ get; set; }
        public string MobilePhone{ get; set; }
        public string Email{ get; set; }
        public List<string> Comments{ get; set; }
    }
}
