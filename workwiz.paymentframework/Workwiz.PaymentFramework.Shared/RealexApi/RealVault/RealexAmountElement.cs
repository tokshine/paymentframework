using System;
using System.Xml.Serialization;

namespace Workwiz.PaymentFramework.Shared.RealexApi.RealVault
{
    [Serializable]
    public class RealexAmountElement
    {
        [XmlAttribute("currency")]
        public string Currency = "GBP";

        [XmlText]
        public string ValueText;

        [XmlIgnore]
        public decimal Amount
        {
            set
            {
                ValueText = (value * 100m).ToString("F0");
            }
            get
            {
                decimal result;
                if (decimal.TryParse(this.ValueText, out result))
                {
                    return result / 100m;
                }
                else
                {
                    return 0;
                }
            }
        }
    }
}