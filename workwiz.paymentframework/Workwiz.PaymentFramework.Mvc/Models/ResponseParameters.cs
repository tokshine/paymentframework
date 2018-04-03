namespace Workwiz.PaymentFramework.Mvc.Models
{
    public class ResponseParameters
    {
        public string NextUrl { get; set; }
        public string SuccessImageUrl { get; set; }
        public string FailureImageUrl { get; set; }
        public string Reference { get; set; }
        /// <summary>
        /// Optional raw html which will be displayed to the user upon successfull payment after the default view
        /// </summary>
        public string AdditionalSuccessMessage { get; set; }
        /// <summary>
        /// This is being used in Capita. It stores the reference token provided by provider in earlier steps
        /// </summary>
        public string ProviderReference { get; set; }
    }
}
