using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Workwiz.PaymentFramework.Shared.RealexApi.RealAuth;

namespace Workwiz.PaymentFramework.Shared.RealexApi.RealVault
{
    [XmlRoot("request")]
    public class PayerEditRequest : RealAuthRequest
    {
        // to allow XML Serialization
        public PayerEditRequest()
        {
        }

        public PayerEditRequest(string merchantId, string payerRef) : base("payer-edit")
        {
            this.MerchantId = merchantId;
            this.Payer = new RealexPayer() { PayerRef = payerRef };
        }

        [XmlElement("merchantid", Order = 1)]
        public string MerchantId;

        [XmlElement("orderid", Order = 2)]
        public string OrderId;

        [XmlElement("payer", Order = 3)]
        public RealexPayer Payer;

        [XmlElement("sha1hash", Order = 4)]
        public override string Sha1Hash { get; set; }

        [XmlArray("comments", Order = 5)]
        [XmlArrayItem("comment")]
        public List<RealexComment> Comments = new List<RealexComment>();

        [XmlIgnore]
        public override string[] FieldsForSignature => new String[]
        {
            this.TimestampFormatted,
            this.MerchantId,
            this.OrderId,
            string.Empty,
            string.Empty,
            this.Payer.PayerRef
        };
    }
}