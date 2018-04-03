using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Workwiz.PaymentFramework.Shared.RealexApi.RealVault
{
    [Serializable]
    public class RealexPayer
    {
        [XmlAttribute("type")]
        public string PayerType = "Customer";

        [XmlAttribute("ref")]
        public string PayerRef;

        [XmlElement("title")]
        public string Title;

        [XmlElement("firstname")]
        public string FirstName;

        [XmlElement("surname")]
        public string Surname;

        [XmlElement("company")]
        public string Company;

        [XmlElement("address")]
        public RealexAddress Address = new RealexAddress();

        [XmlElement("phonenumbers")]
        public RealexPhoneNumbers PhoneNumbers = new RealexPhoneNumbers();

        [XmlElement("email")]
        public string EMail;

        [XmlArray("comments")]
        [XmlArrayItem("comment")]
        public List<RealexComment> Comments = new List<RealexComment>();
    }
}