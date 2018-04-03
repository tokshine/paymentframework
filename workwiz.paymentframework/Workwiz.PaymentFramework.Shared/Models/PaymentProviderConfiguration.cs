using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Workwiz.PaymentFramework.Shared.Models
{
    [Serializable]
    public class PaymentProviderConfiguration
    {
        /// <summary>
        /// For Realex : realex's MerchantId field (NOTE: this is not the bank's MerchantID, but realex's account identifier)
        /// For Civica : Not used (yet... but could form part of the request URL to distinguish accounts)
        /// </summary>
        [JsonProperty("Account")]
        public String AccountIdentifer { get; set; }

        /// <summary>
        /// The shared secret key for authenticating requests to the provider
        /// For Realex and Capita : the Secret-Key used for hash signatures
        /// For Civica : not used
        /// </summary>
        [JsonProperty("SharedSecret")]
        public String SharedSecret { get; set; }

        /// <summary>
        /// For Realex : Not used for Realex
        /// For Civica : Not used (yet... but could be used for Civica's sub account)
        /// For Capita:  Capita's ScpId is used as a sub account
        /// </summary>
        [JsonProperty("SubAccount")]
        public string SubAccountNumber { get; set; }
    }
}
