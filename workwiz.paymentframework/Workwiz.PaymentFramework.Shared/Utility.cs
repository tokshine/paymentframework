using System;

namespace Workwiz.PaymentFramework.Shared
{
    public class Utility
    {
        public static DateTime? ParseExpiryDate(string expiryMonthYear)
        {
            if (String.IsNullOrEmpty(expiryMonthYear))
            {
                return null;
            }

            int allDigitsParsed;
            if (!int.TryParse(expiryMonthYear, out allDigitsParsed))
            {
                return null;
            }

            int year2digit = allDigitsParsed % 100;
            int month = (allDigitsParsed - year2digit) / 100;

            DateTime monthStart = new DateTime(year: 2000 + year2digit, month: month, day: 1);
            DateTime monthEnd = monthStart.AddMonths(1).AddMinutes(-1);

            return monthEnd;
        }
    }
}
