using System;
using System.Web.Hosting;
using Workwiz.PaymentFramework.Shared;
using WebActivatorEx;
using Workwiz.PaymentFramework.Mvc.Infrastructure;

[assembly: WebActivatorEx.PostApplicationStartMethod(typeof(Workwiz.PaymentFramework.TestWeb.App_Start.PaymentFrameworkStartup), "Register")]

namespace Workwiz.PaymentFramework.TestWeb.App_Start
{
    public static class PaymentFrameworkStartup
    {
        public static void Register()
        {
            HostingEnvironment.RegisterVirtualPathProvider(new EmbeddedVirtualPathProvider());
        }
    }
}
