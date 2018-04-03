using System;
using System.Xml.Serialization;

namespace Workwiz.PaymentFramework.Shared.RealexApi.RealVault
{
    [Serializable]
    public class RealexAddress
    {
        [XmlElement("line1")]
        public string Line1;

        [XmlElement("line2")]
        public string Line2;

        [XmlElement("line3")]
        public string Line3;

        [XmlElement("city")]
        public string City;

        [XmlElement("county")]
        public string County;

        [XmlElement("postcode")]
        public string Postcode;

        [XmlElement("country")]
        public RealexCountry Country;
    }
}