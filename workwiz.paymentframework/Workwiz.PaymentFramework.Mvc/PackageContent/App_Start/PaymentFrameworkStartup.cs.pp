using System.Web.Hosting;
using Workwiz.PaymentFramework.Mvc.Infrastructure;

[assembly: WebActivatorEx.PostApplicationStartMethod(typeof($rootnamespace$.App_Start.PaymentFrameworkStartup), "Register")]

namespace $rootnamespace$.App_Start
{
    public static class PaymentFrameworkStartup
    {
        public static void Register()
        {
            HostingEnvironment.RegisterVirtualPathProvider(new EmbeddedVirtualPathProvider());
        }
    }

}