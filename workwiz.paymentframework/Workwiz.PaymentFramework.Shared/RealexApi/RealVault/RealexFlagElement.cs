using System;
using System.Xml.Serialization;

namespace Workwiz.PaymentFramework.Shared.RealexApi.RealVault
{
    [Serializable]
    public class RealexFlagElement
    {
        [XmlAttribute("flag")]
        public string Flag;

        [XmlText]
        public string Content;
    }
}
