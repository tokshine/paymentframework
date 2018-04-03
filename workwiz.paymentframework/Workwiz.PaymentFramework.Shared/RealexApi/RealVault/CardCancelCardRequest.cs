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
    public class CardCancelCardRequest : RealAuthRequest
    {
        public class CardDetail
        {
            [XmlElement("ref", Order = 1)]
            public string CardName { get; set; }

            [XmlElement("payerref", Order = 2)]
            public string PayerRef;
        }

        // to allow XML Serialization
        public CardCancelCardRequest()
        {
        }

        public CardCancelCardRequest(
            string merchantId,
            string payerRef,
            string savedCardName)
            : base("card-cancel-card")
        {
            this.MerchantId = merchantId;
            this.CardToDelete = new CardDetail()
            {
                CardName = savedCardName,
                PayerRef = payerRef
            };
        }

        [XmlIgnore]
        public override string[] FieldsForSignature => new String[]
            {
                this.TimestampFormatted,
                this.MerchantId,
                this.CardToDelete.PayerRef,
                this.CardToDelete.CardName
            };

        [XmlElement("merchantid", Order = 1)]
        public string MerchantId;

        [XmlElement("card", Order = 2)]
        public CardDetail CardToDelete { get; set; }

        [XmlElement("sha1hash", Order = 3)]
        public override string Sha1Hash { get; set; }
    }
}
