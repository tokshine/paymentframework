using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Workwiz.PaymentFramework.Shared
{
    public static class MessageContentUtility
    {
        public static string ByteArrayToHexString(byte[] source)
        {
            StringBuilder result = new StringBuilder();
            foreach (byte b in source)
            {
                result.Append(b.ToString("x2"));
            }
            return result.ToString();
        }

        public static string TruncateAndStripDisallowed(string value, int? truncateTo = null, System.Text.RegularExpressions.Regex disallowedCharacters = null)
        {
            if (null == value)
            {
                return null;
            }

            if (null != disallowedCharacters)
            {
                value = disallowedCharacters.Replace(value, String.Empty);
            }

            if (truncateTo.HasValue)
            {
                if (value.Length > truncateTo.Value)
                {
                    value = value.Substring(0, truncateTo.Value);
                }
            }

            return value;
        }
    }
}
