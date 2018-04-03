using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Workwiz.PaymentFramework.Shared.RealexApi;

namespace Workwiz.PaymentFramework.Shared.RealexApi.RealVault
{
    [Serializable]
    public class RealexTssInfo
    {
        /// <summary>
        /// Description: Your reference number for the customer
        /// Format: a-z A-Z 0-9 – “” _ . , + @ 
        /// </summary>
        [XmlElement("custnum")]
        public string CustomerNumber;

        /// <summary>
        /// Description: An email address or mobile number or any other useful value can be sent with each request
        /// Format: a-z A-Z 0-9 – “” _ . , + @
        /// </summary>
        [XmlElement("varref")]
        public string VarReference;

        /// <summary>
        /// Description: Typically your product code.
        /// Format: a-z A-Z 0-9 – “” _ . , + @
        /// </summary>
        [XmlElement("prodid")]
        public string ProductCode;

        public void TruncateAndStripDisallowed()
        {
            this.CustomerNumber = MessageContentUtility.TruncateAndStripDisallowed(this.CustomerNumber, disallowedCharacters: RealexFields.RealexFieldProductIdDisallowRegex);
            this.VarReference = MessageContentUtility.TruncateAndStripDisallowed(this.VarReference, truncateTo: 50, disallowedCharacters: RealexFields.RealexFieldVarRefDisallowRegex);
            this.ProductCode = MessageContentUtility.TruncateAndStripDisallowed(this.ProductCode, truncateTo: 50, disallowedCharacters: RealexFields.RealexFieldProductIdDisallowRegex);
        }
    }
}
