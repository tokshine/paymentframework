using System.Xml.Serialization;
using Workwiz.PaymentFramework.Shared.Models;

namespace Workwiz.PaymentFramework.Shared.RealexApi.RealAuth
{
    public class RealAuthBasePaymentResponse : RealAuthBaseResponse
    {
        [XmlElement("account")]
        public string Account { get; set; }

        [XmlElement("cvnresult")]
        public string CvnResult { get; set; }

        [XmlElement("avspostcoderesponse")]
        public string AvsPostcodeResponse { get; set; }

        [XmlElement("avsaddressresponse")]
        public string AvsAddressResponse { get; set; }

        [XmlElement("batchid")]
        public string BatchId { get; set; }

        [XmlElement("authtimetaken")]
        public string AuthTimeTaken { get; set; }
    }
}