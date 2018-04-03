using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Workwiz.PaymentFramework.Shared.RealexApi
{
    public static class RealVaultFields
    {
        public const string ResponseMaskedCardDigits = "SAVED_PMT_DIGITS";

        public const string ResponseSavedPayerRef = "SAVED_PAYER_REF";

        public const string ResponsePayerSetupResultCode = "PAYER_SETUP";

        public const string ResponseSavedCardRef = "SAVED_PMT_REF";

        public const string ResponseSavedCardResultCode = "PMT_SETUP";

        public const string ResponseSavedCardResultMessage = "PMT_SETUP_MSG";

        public const string ResponseSavedCardExpiryDate = "SAVED_PMT_EXPDATE";
    }
}
