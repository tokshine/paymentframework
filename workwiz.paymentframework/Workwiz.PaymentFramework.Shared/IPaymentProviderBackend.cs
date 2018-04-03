using System.Threading.Tasks;
using Workwiz.PaymentFramework.Shared.Models;

namespace Workwiz.PaymentFramework.Shared
{
    /// <summary>
    ///     Represents the non-GUI interface to a payment provider
    /// </summary>
    public interface IPaymentProviderBackend
    {
        ProviderType ProviderType { get; }
        ProviderFeatures SupportedFeatures { get; }

        /// <returns>will throw an exception if the call fails, or a transaction reference-number otherwise</returns>
        Task<string> UpdateCardholderDetails(PaymentProviderConfiguration configuration, CardholderDetails cardholder);

        /// <returns>will throw an exception if the call fails, or a transaction reference-number otherwise</returns>
        Task<string> CancelSavedCard(
            PaymentProviderConfiguration configuration,
            string cardOwnerIdentifer,
            string savedCardIdentified);

        Task<IPaymentAuthorizationResponse> CheckAuthorization(
            PaymentProviderConfiguration configuration,
            string uniqueIdentifier,
            decimal expectedAmount,
            string paymentProviderUniqueReference);
    }
}
