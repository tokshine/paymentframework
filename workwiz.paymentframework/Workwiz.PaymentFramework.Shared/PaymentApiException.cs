using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Workwiz.PaymentFramework.Shared
{
    public class PaymentApiException : Exception
    {
        public PaymentApiException(
            string description,
            string rawRequest,
            string rawResponse,
            bool isUserError) : base(description)
        {
            this.RequestString = rawRequest;
            this.ResponseString = rawResponse;
            this.IsUserError = isUserError;
        }

        public string RequestString { get; }
        public string ResponseString { get; }

        public bool IsUserError { get; }
        public bool IsSystemError => !this.IsUserError;

        public override string Message => $"{base.Message} response: {this.ResponseString} request: {this.RequestString}";
    }
}
