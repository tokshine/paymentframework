using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Workwiz.PaymentFramework.Shared;
using Workwiz.PaymentFramework.Shared.RealexApi;

namespace Workwiz.PaymentFramework.Tests.RealexApi
{
    [TestClass]
    public class ExpiryDateParsingTests
    {
        [TestMethod]
        public void DateFormatTesting()
        {
            KeyValuePair<string, DateTime?>[] testCases = new KeyValuePair<string, DateTime?>[]
            {
                new KeyValuePair<string, DateTime?>("0419", new DateTime(2019, 4, 30, 23, 59, 00)),
                new KeyValuePair<string, DateTime?>("0299", new DateTime(2099, 2, 28, 23, 59, 00)),
                new KeyValuePair<string, DateTime?>("1200", new DateTime(2000, 12, 31, 23, 59, 00)),
                new KeyValuePair<string, DateTime?>(String.Empty, null),
                new KeyValuePair<string, DateTime?>("Error", null)
            };

            foreach (var testCase in testCases)
            {
                DateTime? expected = testCase.Value;
                DateTime? actual = Utility.ParseExpiryDate(testCase.Key);
                Assert.AreEqual(expected, actual, $"Parsing {testCase.Key}");
            }
        }
    }
}
