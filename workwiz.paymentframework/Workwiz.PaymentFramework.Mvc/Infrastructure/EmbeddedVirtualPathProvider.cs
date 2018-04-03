using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Caching;
using System.Web.Hosting;

namespace Workwiz.PaymentFramework.Mvc.Infrastructure
{
    public class EmbeddedVirtualPathProvider : VirtualPathProvider
    {
        private readonly Assembly assembly = typeof(EmbeddedVirtualPathProvider).Assembly;
        private readonly string[] resourceNames;

        public EmbeddedVirtualPathProvider()
        {
            this.resourceNames = assembly.GetManifestResourceNames();
        }

        private bool IsEmbeddedResourcePath(string virtualPath)
        {
            string resourceName = VirtualPathToResourceName(virtualPath);
            return this.resourceNames.Contains(resourceName);
        }

        public static string VirtualPathToResourceName(string virtualPath)
        {
            string checkPath = VirtualPathUtility.ToAppRelative(virtualPath);
            string resourceName = typeof(ResourceRootNamespace).Namespace + "." + checkPath.Replace("~/", "").Replace("/", ".");
            return resourceName;
        }

        public override bool FileExists(string virtualPath)
        {
            if (IsEmbeddedResourcePath(virtualPath))
            {
                return true;
            }
            else
            {
                return base.FileExists(virtualPath);
            }
        }

        public override VirtualFile GetFile(string virtualPath)
        {
            if (IsEmbeddedResourcePath(virtualPath))
            {
                return new EmbeddedVirtualFile(virtualPath);
            }
            return base.GetFile(virtualPath);
        }

        public override CacheDependency GetCacheDependency(string virtualPath, IEnumerable virtualPathDependencies, DateTime utcStart)
        {
            if (IsEmbeddedResourcePath(virtualPath))
            {
                return null;
            }
            return base.GetCacheDependency(virtualPath, virtualPathDependencies, utcStart);
        }
    }
}
