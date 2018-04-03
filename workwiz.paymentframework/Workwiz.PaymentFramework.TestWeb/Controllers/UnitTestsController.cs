using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Workwiz.PaymentFramework.Mvc;
using Workwiz.PaymentFramework.Mvc.Models;
using Workwiz.PaymentFramework.Mvc.RealEx;
using Workwiz.PaymentFramework.Shared;
using Workwiz.PaymentFramework.Shared.Models;
using Workwiz.PaymentFramework.TestWeb.Models;

namespace Workwiz.PaymentFramework.TestWeb.Controllers
{
    public class UnitTestsController : Controller
    {
        // GET: UnitTests
        public ActionResult Index()
        {
            List<PaymentWebUnitTestViewModel> testOutput = PaymentWebUnitTestViewModel.RunTestsInClass(this);
            return View(model: testOutput);
        }

        [TestMethod]
        public void TestSendToRealexNoSaveCard()
        {
            RealExPaymentProvider realexInstance = new RealExPaymentProvider(new TraceWriterLogAdapter());

            Assert.AreEqual(ProviderType.Realex, realexInstance.ProviderType);

            var scenario = new
            {
                SimulateAt = new DateTime(2016, 02, 03, 10, 34, 21),
                providerConfig = new PaymentProviderConfiguration()
                {
                    AccountIdentifer = "test-id",
                    SharedSecret = "test-secret"
                },
                PaymentTransfer = new GeneralisedPaymentTransfer()
                {
                    Account = "test-account",
                    Amount = 234.56m,
                    Comment1 = "Hello [World] this is a test!",
                    Comment2 = "** some \"unusual<b>tokens</b>'\\ /etc..",
                    CustomerNumber = "1x2x345",
                    ProductId = "PS-a/b\\3",
                    ReturnUrl = "http://unit.localtest.me/something",
                    SaveCard = null,
                    TransactionId = "A453F8F7-E5DD-473F-B555-33839CA44C63",
                    VariableReference = "Another ref"
                }
            };

            var xferResult = realexInstance.SendToPaymentProvider(scenario.providerConfig, scenario.PaymentTransfer, scenario.SimulateAt);

            string generatedHtml = PaymentFrameworkUtility.DescribeActionResultForLogging(xferResult, true);

            string expectedHtml =
                "<form action=\"http://localhost:2437/Realex/epage.cgi.aspx\" method=\"post\" name=\"paymentForm\"> " +
                "<input type=\"hidden\" name=\"MERCHANT_RESPONSE_URL\" value=\"http://unit.localtest.me/something\" /> " +
                "<input type=\"hidden\" name=\"MERCHANT_ID\" value=\"test-id\" /> " +
                "<input type=\"hidden\" name=\"ORDER_ID\" value=\"A453F8F7-E5DD-473F-B555-33839CA44C63\" /> " +
                "<input type=\"hidden\" name=\"AMOUNT\" value=\"23456\" /> " +
                "<input type=\"hidden\" name=\"CURRENCY\" value=\"GBP\" /> " +
                "<input type=\"hidden\" name=\"TIMESTAMP\" value=\"20160203103421\" /> " +
                "<input type=\"hidden\" name=\"SHA1HASH\" value=\"a1134e0c769e30844dd1941c446c9eafe6f78026\" /> " +
                "<input type=\"hidden\" name=\"ACCOUNT\" value=\"test-account\" /> " +
                "<input type=\"hidden\" name=\"AUTO_SETTLE_FLAG\" value=\"1\" /> " +
                "<input type=\"hidden\" name=\"COMMENT1\" value=\"Hello [World] this is a test!\" /> " +
                "<input type=\"hidden\" name=\"COMMENT2\" value=\"** some &quot;unusualbtokens/b&#39;\\ /etc..\" /> " +
                "<input type=\"hidden\" name=\"CUST_NUM\" value=\"1x2x345\" /> " +
                "<input type=\"hidden\" name=\"VAR_REF\" value=\"Anotherref\" /> " +
                "<input type=\"hidden\" name=\"PROD_ID\" value=\"PSab3\" /> " +
                "</form> " +
                "<script type=\"text/javascript\"> document.forms['paymentForm'].submit(); </script>";
            Assert.AreEqual(expectedHtml, generatedHtml);
        }

        [TestMethod]
        public void TestSendToRealexSaveCard()
        {
            RealExPaymentProvider realexInstance = new RealExPaymentProvider(new TraceWriterLogAdapter());

            Assert.AreEqual(ProviderType.Realex, realexInstance.ProviderType);

            var scenario = new
            {
                SimulateAt = new DateTime(2016, 02, 03, 10, 34, 21),
                providerConfig = new PaymentProviderConfiguration()
                {
                    AccountIdentifer = "test-id",
                    SharedSecret = "test-secret"
                },
                PaymentTransfer = new GeneralisedPaymentTransfer()
                {
                    Account = "test-account",
                    Amount = 234.56m,
                    Comment1 = "Hello [World] this is a test!",
                    Comment2 = "** some \"unusual<b>tokens</b>'\\ /etc..",
                    CustomerNumber = "1x2x345",
                    ProductId = "PS-a/b\\3",
                    ReturnUrl = "http://unit.localtest.me/something",
                    SaveCard = new SaveCard()
                    {
                        OfferSaveCard = true,
                        PayerExists = false,
                        PayerReference ="r1234",
                        PaymentReference ="pmt789"
                    },
                    TransactionId = "A453F8F7-E5DD-473F-B555-33839CA44C63",
                    VariableReference = "Another ref"
                }
            };

            var xferResult = realexInstance.SendToPaymentProvider(scenario.providerConfig, scenario.PaymentTransfer, scenario.SimulateAt);

            string generatedHtml = PaymentFrameworkUtility.DescribeActionResultForLogging(xferResult, true);

            string expectedHtml =
                "<form action=\"http://localhost:2437/Realex/epage.cgi.aspx\" method=\"post\" name=\"paymentForm\"> " +
                "<input type=\"hidden\" name=\"MERCHANT_RESPONSE_URL\" value=\"http://unit.localtest.me/something\" /> " +
                "<input type=\"hidden\" name=\"MERCHANT_ID\" value=\"test-id\" /> " +
                "<input type=\"hidden\" name=\"ORDER_ID\" value=\"A453F8F7-E5DD-473F-B555-33839CA44C63\" /> " +
                "<input type=\"hidden\" name=\"AMOUNT\" value=\"23456\" /> " +
                "<input type=\"hidden\" name=\"CURRENCY\" value=\"GBP\" /> " +
                "<input type=\"hidden\" name=\"TIMESTAMP\" value=\"20160203103421\" /> " +
                "<input type=\"hidden\" name=\"SHA1HASH\" value=\"dd3f9a9ac2d01b6a78351809f8cde3bd27f1daa2\" /> " +
                "<input type=\"hidden\" name=\"ACCOUNT\" value=\"test-account\" /> " +
                "<input type=\"hidden\" name=\"AUTO_SETTLE_FLAG\" value=\"1\" /> " +
                "<input type=\"hidden\" name=\"COMMENT1\" value=\"Hello [World] this is a test!\" /> " +
                "<input type=\"hidden\" name=\"COMMENT2\" value=\"** some &quot;unusualbtokens/b&#39;\\ /etc..\" /> " +
                "<input type=\"hidden\" name=\"CUST_NUM\" value=\"1x2x345\" /> " +
                "<input type=\"hidden\" name=\"VAR_REF\" value=\"Anotherref\" /> " +
                "<input type=\"hidden\" name=\"PROD_ID\" value=\"PSab3\" /> " +

                "<input type=\"hidden\" name=\"CARD_STORAGE_ENABLE\" value=\"1\"/> " +
                "<input type=\"hidden\" name=\"OFFER_SAVE_CARD\" value=\"1\" /> " +
                "<input type=\"hidden\" name=\"PAYER_REF\" value=\"r1234\" /> " +
                "<input type=\"hidden\" name=\"PMT_REF\" value=\"pmt789\" /> " +
                "<input type=\"hidden\" name=\"PAYER_EXIST\" value=\"0\" /> " +
                "</form> " +
                "<script type=\"text/javascript\"> document.forms['paymentForm'].submit(); </script>";

            Assert.AreEqual(expectedHtml, generatedHtml);
        }

//        [TestMethod]
        public async Task<ActionResult> UpdateCardHolder()
        {
            RealExPaymentProvider realexInstance = new RealExPaymentProvider(new TraceWriterLogAdapter());

            PaymentProviderConfiguration providerConfig = new PaymentProviderConfiguration()
            {
                AccountIdentifer = "WorkwizTest",
                SharedSecret = "ShouldBeRejected"
            };

            CardholderDetails cardHolder = new CardholderDetails()
            {
                Address1 = "Workwiz Office",
                City = "London",
                Company = "Workwiz",
                Email = "support@workwiz.co.uk",
                FirstName = "UnitTest",
                PayerRef = "1234x5",
//                Country = new Tuple<string, string>("1", "UK"),
                Comments = new List<string>()
            };
            await realexInstance.UpdateCardholderDetails(providerConfig, cardHolder);

            Assert.IsTrue(true, "Reached this point without throwing");
            return Content("done");
        }
    }
}
