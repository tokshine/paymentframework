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
    [XmlRoot("request")]
    public class ReceiptInRequest : RealAuthRequest
    {
        // to allow XML Serialization
        public ReceiptInRequest()
        {
        }

        public ReceiptInRequest(
            string merchantId,
            string payerRef,
            string paymentMethod,
            decimal amount) : base("receipt-in")
        {
            this.MerchantId = merchantId;
            this.AutoSettleFlag.Flag = "1";
            this.PayerRef = payerRef;
            this.PaymentMethod = paymentMethod;
            this.Amount.Amount = amount;
            this.OrderId = Guid.NewGuid().ToString("D");
        }

        [XmlElement("merchantid", Order = 1)]
        public string MerchantId;

        [XmlElement("account", Order = 2)]
        public string Account;

        [XmlElement("orderid", Order = 3)]
        public string OrderId;

        [XmlElement("autosettleflag", Order = 4)]
        public RealexFlagElement AutoSettleFlag = new RealexFlagElement();

        [XmlElement("amount", Order = 5)]
        public RealexAmountElement Amount = new RealexAmountElement();

        [XmlElement("authcode", Order = 6)]
        public string AuthCode;

        [XmlElement("payerref", Order = 7)]
        public string PayerRef;

        [XmlElement("paymentmethod", Order = 8)]
        public string PaymentMethod;

        [XmlElement("sha1hash", Order = 9)]
        public override string Sha1Hash { get; set; }

        [XmlArray("comments", Order = 10)]
        [XmlArrayItem("comment")]
        public List<RealexComment> Comments = new List<RealexComment>();

        [XmlElement("tssinfo", Order = 11)]
        public RealexTssInfo TssInfo;

        [XmlIgnore]
        public override string[] FieldsForSignature => new string[]
        {
            this.TimestampFormatted,
            this.MerchantId,
            this.OrderId,
            this.Amount.ValueText,
            this.Amount.Currency,
            this.PayerRef
        };
    }
}
