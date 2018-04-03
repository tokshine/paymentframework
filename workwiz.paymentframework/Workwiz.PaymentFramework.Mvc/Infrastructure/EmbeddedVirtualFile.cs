using System.Reflection;
using System.Web;
using System.Web.Hosting;

namespace Workwiz.PaymentFramework.Mvc.Infrastructure
{
    public class EmbeddedVirtualFile : VirtualFile
    {
        private readonly string _resourceName;
        private readonly Assembly assembly;

        public EmbeddedVirtualFile(string virtualPath)
            : base(virtualPath)
        {
            this.assembly = this.GetType().Assembly;            
            this._resourceName = EmbeddedVirtualPathProvider.VirtualPathToResourceName(virtualPath);
        }

        public override System.IO.Stream Open()
        {
            return assembly.GetManifestResourceStream(this._resourceName);
        }
    }
}
