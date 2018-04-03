using System;
using System.Collections.Specialized;
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
    public class CapitaController : Controller
    {

        private string _providerName = "Capita";
        private string _capitaMerchantid = "516";
        private string _capitaScpid = "373029584";
        private string _capitaSecretKey = "DO3DHDLbJTO1Natclx1uC6R740mI3eXuVSNsKcmeuHwJXwaFmDERwe11GcFFbicYoJ/N2tq87glTOTHB4dAGeQ==|456";

        private IPaymentProvider _paymentProvider;

        public CapitaController()
        {
            _paymentProvider = PaymentProviderFactory.GetProvider((ProviderType) Enum.Parse(typeof (ProviderType), _providerName, true),
                new TraceWriterLogAdapter());
        }

        // GET: Default
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ProcessPayment(FormCollection form)
        {
            string requestId = _paymentProvider.GenerateUniqueIdentifier();
            this.Session["RequestId"] = requestId;
                                   

            var xfer = new GeneralisedPaymentTransfer()
            {
                Amount = decimal.Parse(form["Amount"]), // -> "AMOUNT"
                ReturnUrl = "http://localhost:59412" + Url.Action("ProviderCallback", "Capita"),
                TransactionId = requestId
            };

            PaymentProviderConfiguration providerConfig = new PaymentProviderConfiguration()
            {
                AccountIdentifer = this._capitaMerchantid,
                SharedSecret = this._capitaSecretKey,
                SubAccountNumber = this._capitaScpid
            };
            
            xfer.Account = this._capitaScpid;
            xfer.VariableReference = "164 Wandsworth Borough Council";
            xfer.Comment1 = "Activity : After school club";
            xfer.Comment2 = "PS56789/456"; 
            xfer.CustomerNumber = "50939";
            xfer.GeneralLedgerCode = "WE333292049,WE330192071,WE333592049,WE333792049";
            xfer.IsMediated = false;
            xfer.ProductId = "56789";            
            //xfer.VatCode = CapitaVatCode.StandardRate;
            //xfer.VatRate = 20;
            xfer.VatCode = CapitaVatCode.ZeroRate;
            xfer.VatRate = 0;
            //xfer.SaveCard = new SaveCard()
            //{
            //    PayerReference = "s12u34"
            //};
            xfer.LineItems = new System.Collections.Generic.List<CivicaLineItem>()
            {
                new CivicaLineItem()
                {
                    Amount = decimal.Parse(form["Amount"]),
                    FundCode = "16",
                    Narrative = "Play Service booking MULTI_153997_112816_138579",
                    Reference = "56789"
                }
            };

            var result = _paymentProvider.SendToPaymentProvider(providerConfig, xfer, SaveProviderReference);

            string rawHtml = PaymentFrameworkUtility.DescribeActionResultForLogging(result);
            System.Diagnostics.Trace.WriteLine(rawHtml);
            System.Diagnostics.Trace.WriteLine("Provider Reference: " + this.Session["ProviderReference"]);

            return result;
        }
        
        public async Task<ActionResult> ProviderCallback(FormCollection form)
        {
            PaymentProviderConfiguration providerConfig = new PaymentProviderConfiguration()
            {
                AccountIdentifer = this._capitaMerchantid,
                SharedSecret = this._capitaSecretKey,
                SubAccountNumber = this._capitaScpid
            };

            string scpReference = string.Empty;

            if (this.Session["ProviderReference"] != null)
            {
                scpReference = (string) this.Session["ProviderReference"];
            }

            if (form != null && form[this._paymentProvider.RoundTripTokenKey] == null && this.Request.QueryString["RequestId"] != null)
            {
                form[this._paymentProvider.RoundTripTokenKey] = this.Request.QueryString["RequestId"];
            }
            

            var responseParameters = new ResponseParameters()
            {
                NextUrl = "",
                SuccessImageUrl = "http://www.myplayservice.co.uk/App_Themes/images/Allow.gif",
                FailureImageUrl = "http://www.myplayservice.co.uk/App_Themes/images/deny.png",
                ProviderReference = scpReference
            };

            var response = await _paymentProvider.ProcessResponse(providerConfig, form, responseParameters);

            if (response.ServerReply != null)
            {
                return response.ServerReply;
            }
            
            if (response.AuthorizationResult == PaymentAuthorizationResult.Authorized)
            {
                return Content("Booking confirmed. A confirmation email will be sent shortly");
            }
            else
            {
                return Content("Unsuccessful Payment." + response.Description);
            }

            
        }

        public void SaveProviderReference(PaymentProviderConfiguration config,
            GeneralisedPaymentTransfer paymentTransfer, string providerReference)
        {
            this.Session["ProviderReference"] = providerReference;
        }
    }
}