using System;
using System.Xml.Serialization;

namespace Workwiz.PaymentFramework.Shared.RealexApi.RealVault
{
    [Serializable]
    public class RealexCountry
    {
        [XmlAttribute("code")]
        public string CountryCode;

        [XmlText]
        public string Name;
    }
}
