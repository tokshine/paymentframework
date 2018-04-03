using System;
using System.IO;
using System.Xml.Serialization;

namespace Workwiz.PaymentFramework.Shared.RealexApi.RealAuth
{
    [Serializable]
    //    [XmlRoot("request")]
    public abstract class RealAuthRequest
    {
        // to allow XML Serialization
        public RealAuthRequest()
        {
        }

        public RealAuthRequest(string requestType)
        {
            this.TimestampFormatted = DateTime.Now.ToString("yyyyMMddHHmmss");
            this.RequestType = requestType;
        }

        [XmlAttribute("type")]
        public string RequestType;

        [XmlAttribute("timestamp")]
        public string TimestampFormatted;

        [XmlIgnore]
        public abstract string Sha1Hash { get; set; }

        [XmlIgnore]
        public abstract string[] FieldsForSignature { get; }

        public void SetSha1Hash(string secretKey)
        {
            this.Sha1Hash = RealexApiProvider.CalculateRealexSignature(this.FieldsForSignature, secretKey);
        }

        /// <summary>
        /// For diagnostics
        /// </summary>
        /// <returns></returns>
        public string SerializeToTraceString()
        {
            try
            {
                XmlSerializer s = new XmlSerializer(this.GetType());
                using (StringWriter sw = new StringWriter())
                {
                    s.Serialize(sw, this);
                    return sw.ToString();
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}