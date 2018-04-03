namespace Workwiz.PaymentFramework.Shared.Models
{
    public enum PaymentAuthorizationResult
    {
        /// <summary>
        /// Payment engine has neither authorized nor declined the payment - e.g. user did not complete the process or is still in the UI
        /// </summary>
        Unknown,
        Authorized,
        Declined,
        /// <summary>
        /// An error of some kind occured - either in communicating with the service, or as a response from the service.
        /// We can not be sure whether the payment is authorized or declined in this scenario.
        /// </summary>
        ErrorUnknownStatus
    }
}
