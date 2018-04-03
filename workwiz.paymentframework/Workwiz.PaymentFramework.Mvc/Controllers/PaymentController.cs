using System.Web.Mvc;
using Workwiz.PaymentFramework.Mvc.Models;

namespace Workwiz.PaymentFramework.Mvc.Controllers
{
    public class PaymentController : Controller
    {
        // GET: Payment
        public ActionResult SendToCivica(CivicaPaymentTransfer model)
        {
            return View(model);
        }

        public ActionResult SendToRealEx(RealExPaymentTransfer model)
        {
            return View(model);
        }

        public ActionResult ReplyToRealEx()
        {
            return View();
        }
    }
}