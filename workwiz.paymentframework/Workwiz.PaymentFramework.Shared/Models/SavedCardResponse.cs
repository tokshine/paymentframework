using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Workwiz.PaymentFramework.Shared.Models
{
    public class SavedCardResponse
    {
        public bool NewCardholderSaved { get; set; }
        public string CardholderReference { get; set; }
        public bool CardSaved { get; set; }
        /// <summary>
        /// For Capita this field uses for CardKey token
        /// </summary>
        public string CardReference { get; set; }
        public string CardSaveStatus { get; set; }
        /// <summary>
        /// Capita only returns last 4 digit of card
        /// </summary>
        public string CardDigits { get; set; }
        /// <summary>
        /// Capita returns ExpiryDate in MMYY format and hence we use day = last date of the month
        /// </summary>
        public DateTime? ExpiryDate { get; set; }
    }

    public class CapitaSavedCardResponse: SavedCardResponse
    {
        /// <summary>
        /// In Capita it corresponds to enum of CardDescription and here is the list of enum on 08/03/2017.
        /// VISA  = 0
        /// MASTERCARD = 1
        /// AMERICANEXPRESS = 2,
        /// LASER = 3,
        /// DINERS = 4,
        /// JCBC = 5
        /// NONE = 6
        /// </summary>
        public string CardDescription { get; set; }
        /// <summary>
        /// In Capita it corresponds to enum of CardTypes and here is the list of enum on 08/03/2017.
        /// CREDIT = 0,
        /// DEBIT = 1,
        /// NONE = 2
        /// </summary>
        public string CardType { get; set; }
    }
}
