using System;
using System.Collections.Specialized;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Workwiz.PaymentFramework.Mvc.Controllers;

namespace Workwiz.PaymentFramework.Mvc
{
    public static class PaymentFrameworkUtility
    {
        /// <summary>
        ///     Returns a string description of a controller action:
        ///     for a ViewResult it renders the HTML,
        ///     for a RedirectResult it returns the redirect-location
        /// </summary>
        /// <param name="actionResultToDescribe">
        ///     ActionResult to describe in some useful manner for diagnostics purposes in a log
        ///     message
        /// </param>
        /// <param name="compressWhiteSpace">
        ///     If true (default) then consecutive whitespace characters are replaced by single whitespace
        /// </param>
        /// <returns></returns>
        public static string DescribeActionResultForLogging(
            ActionResult actionResultToDescribe,
            bool compressWhiteSpace = true)
        {
            if (null == actionResultToDescribe)
            {
                return "null ActionResult";
            }

            try
            {
                if (actionResultToDescribe is ViewResult)
                {
                    var fakeController = new DummyControllerForRendering();
                    string htmlContent = fakeController.RenderActionResultToString((ViewResult) actionResultToDescribe);
                    if (compressWhiteSpace)
                    {
                        htmlContent = System.Text.RegularExpressions.Regex.Replace(htmlContent, @"\s+", " ");
                    }
                    return htmlContent;
                }
                if (actionResultToDescribe is RedirectResult)
                {
                    var rr = (RedirectResult) actionResultToDescribe;
                    return $"Redirect to {rr.Url}";
                }
                if (actionResultToDescribe is RedirectToRouteResult)
                {
                    var rr = (RedirectToRouteResult) actionResultToDescribe;

                    var fakeController = new DummyControllerForRendering();

                    var url = UrlHelper.GenerateUrl(
                        rr.RouteName,
                        null,
                        null,
                        rr.RouteValues,
                        RouteTable.Routes,
                        fakeController.ControllerContext.RequestContext,
                        false);
                    return $"Redirect to {url}";
                }
                return $"{actionResultToDescribe.GetType().FullName} {actionResultToDescribe}";
            }
            catch (Exception ex)
            {
                return $"Exception describing {actionResultToDescribe.GetType().FullName} : {ex}";
            }
        }

        /// <summary>
        ///     Utility for Web-Forms apps to render an MVC ActionResult to the current response
        /// </summary>
        /// <param name="actionResult">MVC ActionResult to render / execute</param>
        /// <param name="httpContext">optional httpContext to override the default </param>
        public static void RenderActionResultToHttpContext(ActionResult actionResult, HttpContextBase httpContext = null)
        {
            var fakeController = new DummyControllerForRendering();
            actionResult.ExecuteResult(fakeController.ControllerContext);
        }

        public static PartialViewResult CreatePartialView(string viewName, object model)
        {
            return CreateViewOutsideController<PartialViewResult>(viewName, model);
        }

        public static ViewResult CreateView(string viewName, object model)
        {
            return CreateViewOutsideController<ViewResult>(viewName, model);
        }

        public static TView CreateViewOutsideController<TView>(string viewName, object model) 
            where TView : ViewResultBase, new()
        {
            return new TView()
            {
                ViewName = viewName,
                ViewData = new ViewDataDictionary(model),
                ViewEngineCollection = ViewEngines.Engines
            };
        }

        public static string DescribeNameValueCollection(NameValueCollection parametersToDescribe)
        {
            if (null == parametersToDescribe)
            {
                return "null";
            }

            StringBuilder result = new StringBuilder();
            for (int i = 0; i < parametersToDescribe.Count; ++i)
            {
                string key = parametersToDescribe.GetKey(i);
                string val = parametersToDescribe.Get(i);
                if (0 == i)
                {
                    result.AppendFormat(", ");
                }
                result.Append($"{key}={val}");
            }

            return result.ToString();
        }
    }
}
