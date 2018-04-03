using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Workwiz.PaymentFramework.Shared.RealexApi.RealAuth
{
    /// <summary>
    /// Base fields for all responses from Realex and RealAuth
    /// </summary>
    [Serializable]
    [XmlRoot("response")]
    public abstract class RealAuthResponse
    {
        [XmlAttribute("timestamp")]
        public string Timestamp;

        /// <summary>
        /// Parses a realex/realvault response code as an integer, or sentinal value -1 is unparsable
        /// </summary>
        /// <param name="rawResultString"></param>
        /// <returns></returns>
        public static int ParseResultString(string rawResultString)
        {
            int resultInt;
            if (Int32.TryParse(rawResultString, out resultInt))
            {
                return resultInt;
            }
            else
            {
                return -1;
            }
        }

        private string _resultString;
        /// <summary>
        /// NOTE: this MUST be a string because checking the hash-signature relies on
        /// the exact formatting of this string
        /// </summary>
        [XmlElement("result")]
        public string ResultString
        {
            get { return _resultString; }
            set
            {
                _resultString = value;
                this._resultParsedInt = ParseResultString(value);
            }
        }

        private int _resultParsedInt;

        /// <summary>
        /// General Success and Errors
        /// Code Description
        /// 00 	Successful – the transaction has been authorised successfully; you may proceed with the sale.
        /// 1xx A failed transaction. You can treat any 1xx code as a failed transaction and inform your customer that they should either try again or try another payment method.If you wish you may provide alternate flows based on the specific codes as follows: 101 Declined by Bank – generally insufficient funds or incorrect card details (e.g.expiry date, card security code) 102 Referral to Bank(usually treated as a standard decline for ecommerce systems) 103 Card reported lost or stolen; may be indicative of attempted fraud 107 The transaction has been blocked as potentially fraudulent by your fraud management configuration 1xx Other reason.Treat as a decline like 101.
        /// 2xx Error with bank systems – generally you can tell the customer to try again later.The resolution time depends on the issue.
        /// 3xx Error with Realex Payments systems – generally you can tell the customer to try again later. The resolution time depends on the issue.
        /// 5xx Incorrect XML message formation or content.These are either development errors, configuration errors or customer errors.There is a large list below, but in general: 508 Issue with integration, e.g.incorrect account data provided. Check the message and correct. 509 Issue with customer supplied data, e.g.invalid characters. Check the message and correct. 5xx Issue with account configuration – you may need to contact Realex Payments support team to have this issue resolved It is important that you implement sufficient validation rules and test extensively so that the above errors can be avoided when processing live transactions so as not to lose sales.
        /// 666 Account deactivated – your Realex Payments account has been suspended.Please contact Realex Payments.
        /// 
        /// See https://developer.realexpayments.com/#!/technical-resources/messages for full list of codes
        /// </summary>
        [XmlIgnore]
        public int Result => this._resultParsedInt;

        [XmlElement("message")]
        public string Message;

        [XmlElement("timetaken")]
        public string TimeTaken { get; set; }

        [XmlElement("sha1hash")]
        public string Sha1Hash;

        /// <summary>
        /// Are both the HTTP-response and 'Result' a success?
        /// </summary>
        [XmlIgnore]
        public bool IsSuccess => this.IsResultCodeSuccess && (HttpStatusCode.OK == this.HttpStatus);

        public virtual bool IsResultCodeSuccess => (0 == this.Result);

        [XmlIgnore]
        public abstract string[] FieldsForSignature { get; }

        public string CalculateExpectedSha1Hash(string secretKey)
        {
            return RealexApiProvider.CalculateRealexSignature(this.FieldsForSignature, secretKey);
        }

        public bool IsSha1HashCorrect(string secretKey)
        {
            // special case for some error responses: treat a missing hash as a correct hash
            if (String.IsNullOrEmpty(this.Sha1Hash))
            {
                return this.Result >= 300;
            }
            string expectedHash = CalculateExpectedSha1Hash(secretKey);
            return String.Equals(expectedHash, this.Sha1Hash, StringComparison.OrdinalIgnoreCase);
        }

        #region additional meta-data added by PaymentFramework
        [XmlIgnore]
        public HttpStatusCode HttpStatus { get; set; }

        [XmlIgnore]
        public RealAuthRequest OriginalRequest { get; set; }

        [XmlIgnore]
        public string RawResponseString { get; set; }

        public void ThrowIfNotSuccess()
        {
            if (!this.IsSuccess)
            {
                string reqString = OriginalRequest.SerializeToTraceString();
                bool isUserError = (100 <= this.Result) && (this.Result < 200);
                throw new PaymentApiException(
                    description: this.Message,
                    rawRequest: reqString, 
                    rawResponse: this.RawResponseString, 
                    isUserError: isUserError);
            }
        }

        #endregion
    }
}
