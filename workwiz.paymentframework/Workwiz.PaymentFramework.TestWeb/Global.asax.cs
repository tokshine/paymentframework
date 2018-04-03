using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Routing;

namespace Workwiz.PaymentFramework.TestWeb
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}
