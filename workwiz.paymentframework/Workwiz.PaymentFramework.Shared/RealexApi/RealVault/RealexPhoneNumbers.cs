using System;
using System.Xml.Serialization;

namespace Workwiz.PaymentFramework.Shared.RealexApi.RealVault
{
    [Serializable]
    public class RealexPhoneNumbers
    {
        [XmlElement("home")]
        public string Home;

        [XmlElement("work")]
        public string Work;

        [XmlElement("fax")]
        public string Fax;

        [XmlElement("mobile")]
        public string Mobile;
    }
}