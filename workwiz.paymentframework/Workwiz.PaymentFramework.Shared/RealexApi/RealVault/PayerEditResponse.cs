using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Workwiz.PaymentFramework.Shared.RealexApi.RealAuth;

namespace Workwiz.PaymentFramework.Shared.RealexApi.RealVault
{
    [Serializable]
    [XmlRoot("response")]
    public class PayerEditResponse : RealAuthBaseResponse
    {
        [XmlElement("account")]
        public string Account { get; set; }
    }
}
