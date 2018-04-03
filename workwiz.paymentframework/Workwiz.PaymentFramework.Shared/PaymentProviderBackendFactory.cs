using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Workwiz.Common.Logging;
using Workwiz.PaymentFramework.Shared.CapitaApi;
using Workwiz.PaymentFramework.Shared.CivicaApi;
using Workwiz.PaymentFramework.Shared.RealexApi;

namespace Workwiz.PaymentFramework.Shared
{
    public class PaymentProviderBackendFactory
    {
        public IWorkwizLoggerFactory LoggerFactory { get; set; }

        public PaymentProviderBackendFactory(IWorkwizLoggerFactory loggerFactory)
        {
            this.LoggerFactory = loggerFactory;
        }

        public IPaymentProviderBackend GetProvider(ProviderType providerType)
        {
            return GetProvider(providerType, this.LoggerFactory);
        }

        public static IPaymentProviderBackend GetProvider(ProviderType providerType, IWorkwizLoggerFactory loggerFactory)
        {
            switch (providerType)
            {
                case ProviderType.Civica:
                    return new CivicaApiProvider(loggerFactory);
                case ProviderType.Realex:
                    return new RealexApiProvider(loggerFactory);
                case ProviderType.Capita:
                    return new CapitaApiProvider(loggerFactory);
                default:
                    throw new ArgumentOutOfRangeException(paramName: nameof(providerType), actualValue: providerType,
                        message: "Provider not supported");
            }
        }
    }
}
