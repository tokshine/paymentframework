using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Workwiz.PaymentFramework.Shared.Models;

namespace Workwiz.PaymentFramework.Shared.RealexApi.RealAuth
{
    [Serializable]
    [XmlRoot("response")]
    public class QueryPaymentResponse : RealAuthBasePaymentResponse
    {
        [XmlElement("cardnumber")]
        public string CardNumberMasked { get; set; }

        [XmlIgnore]
        public PaymentAuthorizationResult ResultMappedToFrameworkResult
        {
            get
            {
                if (0 == this.Result)
                {
                    return PaymentAuthorizationResult.Authorized;
                }
                else if (508 == this.Result)
                {
                    return PaymentAuthorizationResult.Unknown;
                }
                else if ((100 <= this.Result) && (this.Result < 400))
                {
                    return PaymentAuthorizationResult.Declined;
                }
                else
                {
                    return PaymentAuthorizationResult.ErrorUnknownStatus;
                }
            }
        }

        /// <summary>
        /// For a query-request, Realex uses the response-code for the payment result : so treat the expected/mapped codes
        /// as non request-failues
        /// </summary>
        public override bool IsResultCodeSuccess
            => (PaymentAuthorizationResult.ErrorUnknownStatus != this.ResultMappedToFrameworkResult);
    }
}
