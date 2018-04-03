using System;
using System.Collections.Specialized;
using System.ServiceModel.Security;
using System.Threading.Tasks;
using System.Web.Mvc;
using Workwiz.PaymentFramework.Mvc.Models;
using Workwiz.PaymentFramework.Shared;
using Workwiz.PaymentFramework.Shared.Models;

namespace Workwiz.PaymentFramework.Mvc
{
    /// <summary>
    ///     MVC based GUI features for a payment provider (e.g. redirect to take payment, process callback / redirect-return,
    ///     generate responses to send to provider)
    /// </summary>
    public interface IPaymentProvider : IPaymentProviderBackend
    {
        string RoundTripTokenKey { get; }
        string GenerateUniqueIdentifier();

        ActionResult SendToPaymentProvider(PaymentProviderConfiguration configuration,
            GeneralisedPaymentTransfer transferObject, Action<PaymentProviderConfiguration, GeneralisedPaymentTransfer, string> saveProviderReference);

        Task<PaymentAuthorizationGuiResponse> ProcessResponse(PaymentProviderConfiguration configuration,
            NameValueCollection paramsCollection, ResponseParameters additionalParameters);
    }
}