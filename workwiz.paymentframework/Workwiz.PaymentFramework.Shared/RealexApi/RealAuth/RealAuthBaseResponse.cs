using System.Xml.Serialization;

namespace Workwiz.PaymentFramework.Shared.RealexApi.RealAuth
{
    public class RealAuthBaseResponse : RealAuthResponse
    {
        [XmlElement("merchantid")]
        public string MerchantId { get; set; }

        [XmlElement("orderid")]
        public string OrderId { get; set; }

        [XmlElement("authcode")]
        public string AuthCode { get; set; }

        [XmlElement("pasref")]
        public string PasRef { get; set; }

        [XmlIgnore]
        public override string[] FieldsForSignature => new string[]
        {
            this.Timestamp,
            this.MerchantId,
            this.OrderId,
            this.ResultString,
            this.Message,
            this.PasRef,
            this.AuthCode
        };
    }
}