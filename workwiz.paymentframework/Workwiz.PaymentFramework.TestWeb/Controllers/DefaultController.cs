using System;
using System.Configuration;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Workwiz.PaymentFramework.Mvc;
using Workwiz.PaymentFramework.Mvc.Models;
using Workwiz.PaymentFramework.Shared;
using Workwiz.PaymentFramework.Shared.Models;
using Workwiz.PaymentFramework.TestWeb.Models;

namespace Workwiz.PaymentFramework.TestWeb.Controllers
{
    public class DefaultController : Controller
    {

        private string _providerName = "Realex";
        private string _realExMerchantid = "MockMerchantId";
        private string _realExSecretKey = "MockSecretKey";

        private IPaymentProvider _paymentProvider;
        private IPaymentProviderBackend _paymentProviderBackend;  

        public DefaultController()
        {
            _paymentProvider = PaymentProviderFactory.GetProvider((ProviderType) Enum.Parse(typeof (ProviderType), _providerName, true),
                new TraceWriterLogAdapter());
            _paymentProviderBackend = PaymentProviderBackendFactory.GetProvider(ProviderType.Realex, new TraceWriterLogAdapter());
        }

        // GET: Default
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ProcessPayment(FormCollection form)
        {
            var xfer = new GeneralisedPaymentTransfer()
            {
                Amount = decimal.Parse(form["Amount"]), // -> "AMOUNT"
                ReturnUrl = "http://localhost:59412" + Url.Action("ProviderCallback", "Default"),
                TransactionId = _paymentProvider.GenerateUniqueIdentifier(), // -> "ORDER_ID"
                GeneralLedgerCode = "SOMECIVICACODE"
            };

            PaymentProviderConfiguration providerConfig = new PaymentProviderConfiguration()
            {
                AccountIdentifer = _realExMerchantid,
                SharedSecret = _realExSecretKey
            };
            
            xfer.Account = "";
            xfer.VariableReference = "";
            xfer.Comment1 = "";
            xfer.Comment2 = "";
            xfer.LineItems.Add(new CivicaLineItem()
            {
                Reference = "LINEREF",
                Amount = 10.00m,
                Narrative = "Stuff paid for",
                FundCode = "FUND"
            });

            var result = _paymentProvider.SendToPaymentProvider(providerConfig, xfer, null);

            string rawHtml = PaymentFrameworkUtility.DescribeActionResultForLogging(result);
            System.Diagnostics.Trace.WriteLine(rawHtml);

            return result;
        }
        
        public async Task<ActionResult> ProviderCallback(FormCollection form)
        {
            PaymentProviderConfiguration providerConfig = new PaymentProviderConfiguration()
            {
                AccountIdentifer = _realExMerchantid,
                SharedSecret = _realExSecretKey
            };

            var responseParameters = new ResponseParameters()
            {
                NextUrl = "",
                SuccessImageUrl = "http://www.myplayservice.co.uk/App_Themes/images/Allow.gif",
                FailureImageUrl = "http://www.myplayservice.co.uk/App_Themes/images/deny.png",
                Reference = "REFXXXX"
            };

            var response = await _paymentProvider.ProcessResponse(providerConfig, form, responseParameters);

            return response.ServerReply;
        }

        public async Task<ActionResult> TestBackEndApi()
        {
                        
            PaymentProviderConfiguration providerConfig = new PaymentProviderConfiguration()
            {
                AccountIdentifer = _realExMerchantid,
                SharedSecret = _realExSecretKey
            };

            try
            {
                var pmtAuthResponse = await _paymentProviderBackend.CheckAuthorization(
                                 providerConfig,
                                 _paymentProvider.GenerateUniqueIdentifier(),
                                 1234,
                                 string.Empty);
            }
            catch(Exception ex)
            {
                throw ex;
            }

            return Content("Success");
            
        }
    }
}