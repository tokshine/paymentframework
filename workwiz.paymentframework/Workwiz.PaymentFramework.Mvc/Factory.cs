using System;
using Workwiz.Common.Logging;
using Workwiz.PaymentFramework.Mvc.Capita;
using Workwiz.PaymentFramework.Mvc.Civica;
using Workwiz.PaymentFramework.Mvc.RealEx;
using Workwiz.PaymentFramework.Shared;

namespace Workwiz.PaymentFramework.Mvc
{
    public class PaymentProviderFactory
    {
        public IWorkwizLoggerFactory LoggerFactory { get; set; }

        public PaymentProviderFactory(IWorkwizLoggerFactory loggerFactory)
        {
            this.LoggerFactory = loggerFactory;
        }

        public IPaymentProvider GetProvider(ProviderType providerType)
        {
            return GetProvider(providerType, this.LoggerFactory);
        }

        public static IPaymentProvider GetProvider(ProviderType providerType, IWorkwizLoggerFactory loggerFactory)
        {
            switch (providerType)
            {
                case ProviderType.Civica:
                    return new CivicaPaymentProvider(loggerFactory);
                case ProviderType.Realex:
                    return new RealExPaymentProvider(loggerFactory);
                case ProviderType.Capita:
                    return new CapitaPaymentProvider(loggerFactory);
                default:
                    throw new ArgumentOutOfRangeException(paramName: nameof(providerType), actualValue: providerType,
                        message: "Provider not supported");
            }
        }
    }
}
