using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Workwiz.PaymentFramework.Shared.CapitaSimple;

namespace Workwiz.PaymentFramework.Shared.CapitaApi
{
    public class CapitaApiHelpers
    {
        public static void GetHmacIdAndSecretKey(string sharedSecret, out int hmacKeyId, out string hmacSecretKey)
        {
            //For capita HmackKeyAnd HmacKeyId and combined into one PMSK parameter separated by '|'
            hmacSecretKey = sharedSecret.Substring(0, sharedSecret.LastIndexOf("|", StringComparison.InvariantCultureIgnoreCase));
            string hmacKeyIdString = sharedSecret.Substring(sharedSecret.LastIndexOf("|", StringComparison.InvariantCultureIgnoreCase) + 1);
            
            int.TryParse(hmacKeyIdString, out hmacKeyId);

        }

        /// <summary>
        /// 1. Concatenate subjectType, identifier, uniqueReference, timestamp, algorithm and hmacKeyID into a single string, with a '!' inserted between each concatenated value.E.g. the result might be:
        ///CapitaPortal!37!X326736B!20110203201814!Original!2
        /// </summary>
        /// <param name="requestingCredentials"></param>
        /// <returns></returns>
        public static string GetCredentialsToHash(credentials requestingCredentials)
        {
            if (requestingCredentials != null)
            {
                string subjectType = string.Empty;
                string identifier = string.Empty;
                string uniqueReference = string.Empty;
                string timeStamp = string.Empty;
                string algorithm = string.Empty;
                string hmacKeyId = string.Empty;

                if (requestingCredentials.subject != null)
                {
                    subjectType = requestingCredentials.subject.subjectType.ToString();
                }

                if (requestingCredentials.subject != null)
                {
                    identifier = requestingCredentials.subject.identifier.ToString();
                }

                if (requestingCredentials.requestIdentification != null)
                {
                    uniqueReference = requestingCredentials.requestIdentification.uniqueReference;
                }

                if (requestingCredentials.requestIdentification != null)
                {
                    timeStamp = requestingCredentials.requestIdentification.timeStamp;
                }

                if (requestingCredentials.signature != null)
                {
                    algorithm = requestingCredentials.signature.algorithm.ToString();
                }

                if (requestingCredentials.signature != null)
                {
                    hmacKeyId = requestingCredentials.signature.hmacKeyID.ToString();
                }

                return $"{subjectType}!{identifier}!{uniqueReference}!{timeStamp}!{algorithm}!{hmacKeyId}";

            }

            return string.Empty;
        }

        public static string CalculateDigest(string secretkey, string credentialsToHash)
        {
            byte[] keyBytes = Convert.FromBase64String(secretkey);
            byte[] bytesToHash = (new UTF8Encoding()).GetBytes(credentialsToHash);
            HMACSHA256 hmac = new HMACSHA256(keyBytes);
            byte[] hash = hmac.ComputeHash(bytesToHash);
            return Convert.ToBase64String(hash);
        }
    }
}
