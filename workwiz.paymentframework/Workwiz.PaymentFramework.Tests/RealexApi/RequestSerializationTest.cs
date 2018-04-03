using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Workwiz.PaymentFramework.Shared.RealexApi.RealVault;

namespace Workwiz.PaymentFramework.Tests.RealexApi
{
    [TestClass]
    public class RequestSerializationTest
    {
        /// <summary>
        /// The fields _MUST_ be int he correct order - i.e. with sha1hash before comments
        /// </summary>
        [TestMethod]
        public void SerializedFieldOrder()
        {
            ReceiptInRequest r = new ReceiptInRequest("UnitTest", "123456", "testcard", 1.23m);
            r.TimestampFormatted = "20160102030405";
            r.Comments = RealexComment.CreateCommentList(new string[] {"Test Commend on Transaction"});
            r.OrderId = "15d0df68-8e2d-46c9-b23e-eff539b1bfad";
            r.SetSha1Hash("TestKey");

            XmlSerializer s = new XmlSerializer(r.GetType());
            StringWriter sr = new StringWriter();
            s.Serialize(sr, r);

            string[] resultLines = sr.ToString().Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
                .Select(row => row.Trim())
                .ToArray();

            Assert.AreEqual(13, resultLines.Length);

            Assert.AreEqual("<?xml version=\"1.0\" encoding=\"utf-16\"?>", resultLines[0]);
//            Assert.AreEqual("<request xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" type=\"receipt-in\" timestamp=\"20160102030405\">", resultLines[1]);
            Assert.AreEqual("<merchantid>UnitTest</merchantid>", resultLines[2]);
            Assert.AreEqual("<orderid>15d0df68-8e2d-46c9-b23e-eff539b1bfad</orderid>", resultLines[3]);
            Assert.AreEqual("<autosettleflag flag=\"1\" />", resultLines[4]);
            Assert.AreEqual("<amount currency=\"GBP\">123</amount>", resultLines[5]);
            Assert.AreEqual("<payerref>123456</payerref>", resultLines[6]);
            Assert.AreEqual("<paymentmethod>testcard</paymentmethod>", resultLines[7]);
            Assert.AreEqual("<sha1hash>861acecbd73e038ef45c1c39cfa86f3165e34102</sha1hash>", resultLines[8]);
            Assert.AreEqual("<comments>", resultLines[9]);
            Assert.AreEqual("<comment id=\"1\">Test Commend on Transaction</comment>", resultLines[10]);
            Assert.AreEqual("</comments>", resultLines[11]);
            Assert.AreEqual("</request>", resultLines[12]);
        }
    }
}
