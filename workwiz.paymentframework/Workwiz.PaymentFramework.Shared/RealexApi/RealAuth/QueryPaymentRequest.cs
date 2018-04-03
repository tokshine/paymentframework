using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Workwiz.PaymentFramework.Shared.RealexApi.RealVault;

namespace Workwiz.PaymentFramework.Shared.RealexApi.RealAuth
{
    [XmlRoot("request")]
    public class QueryPaymentRequest : RealAuthRequest
    {
        // to allow XML Serialization
        public QueryPaymentRequest()
        {
        }

        public QueryPaymentRequest(string merchantId, string originalOrderId) : base("query")
        {
            this.MerchantId = merchantId;
            this.OrderId = originalOrderId;
        }

        [XmlElement("merchantid", Order = 1)]
        public string MerchantId;

        [XmlElement("account", Order = 2)]
        public string SubAccount ;

        [XmlElement("orderid", Order = 3)]
        public string OrderId;

        [XmlElement("sha1hash", Order = 100)]
        public override string Sha1Hash { get; set; }

        [XmlIgnore]
        public override string[] FieldsForSignature => new String[]
        {
            this.TimestampFormatted,
            this.MerchantId,
            this.OrderId,
            String.Empty,
            String.Empty,
            String.Empty
        };
    }
}
