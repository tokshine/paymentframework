using System;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Workwiz.PaymentFramework.Mvc.Controllers
{
    public class DummyControllerForRendering : Controller
    {
        /// <param name="routeData">optional routeData if needed for the view : if not supplied then an empty RouteData is used</param>
        /// <param name="requestContext">
        ///     optional HttpContextBase for testing : if not supplied then new
        ///     HttpContextWrapper(System.Web.HttpContext.Current) is used
        /// </param>
        public DummyControllerForRendering(
            RouteData routeData = null,
            HttpContextBase requestContext = null)
        {
            // get context wrapper from HttpContext if available
            var wrapper = requestContext;
            if (null == wrapper)
            {
                if (null == System.Web.HttpContext.Current)
                {
                    throw new InvalidOperationException(
                        "Can't create Controller Context if no active HttpContext instance is available.");
                }
                wrapper = new HttpContextWrapper(System.Web.HttpContext.Current);
            }

            routeData = routeData ?? new RouteData();

            // add the controller routing if not existing
            if (!routeData.Values.ContainsKey("controller"))
            {
                routeData.Values.Add("controller", GetType().Name.ToLower().Replace("controller", string.Empty));
            }

            ControllerContext = new ControllerContext(wrapper, routeData, this);
        }

        public string RenderActionResultToString(ViewResultBase viewResultToRender)
        {
            var viewEngineResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewResultToRender.ViewName);
            var view = viewEngineResult.View;

            using (var sw = new StringWriter())
            {
                var viewContext = new ViewContext(ControllerContext, view, viewResultToRender.ViewData,
                    viewResultToRender.TempData, sw);
                view.Render(viewContext, sw);
                viewEngineResult.ViewEngine.ReleaseView(ControllerContext, view);

                return sw.ToString();
            }
        }
    }
}