using System.Text.RegularExpressions;

namespace Workwiz.PaymentFramework.Shared.RealexApi
{
    public static class RealexFields
    {
        /// <summary>
        /// Required field. Realex: The unique order id that you sent to us, Workwiz: PendingPayment.UniqueTokenAsString
        /// </summary>
        public const string RealexFieldOrderId = "ORDER_ID";
        public const string RealexFieldMerchantId = "MERCHANT_ID";
        public const string RealexFieldHashSignature = "SHA1HASH";
        public const string RealexFieldTimestamp = "TIMESTAMP";
        public const string RealexFieldCurrency = "CURRENCY";
        public const string RealexFieldAmount = "AMOUNT";
        public const string RealexFieldAutoSettle = "AUTO_SETTLE_FLAG";
        public const string RealexFieldSubAccount = "ACCOUNT";
        public const string RealexFieldVarRef = "VAR_REF";
        public static readonly Regex RealexFieldVarRefDisallowRegex = new Regex(@"[^a-zA-Z0-9\–""_.,+\@]");

        public const string RealexFieldComment1 = "COMMENT1";
        public const string RealexFieldComment2 = "COMMENT2";
        public static readonly Regex RealexFieldCommentDisallowRegex = new Regex(@"[^ a-zA-Z0-9'"",\+._\-&\\/@!?%\(\)*:£$&€#\[\]|=]");

        /// <summary>
        /// Response field : Will contain a valid authcode if the transaction was successful. Will be empty otherwise
        /// </summary>
        public const string RealexFieldAuthCode = "AUTHCODE";
        /// <summary>
        /// Response field : A unique reference that RealexPayments assign to your transaction
        /// </summary>
        public const string RealexFieldPasRef = "PASREF";
        /// <summary>
        /// Response field : The outcome of the transaction. Will contain “00” if the transaction was a 
        /// success or another value (depending on the error) if not. See the result codes 
        /// table in Appendix A (of RealAuth Developers guide)
        /// </summary>
        public const string RealexFieldResult = "RESULT";
        /// <summary>
        /// Response field : Will contain a text message that describes the result code above (the "RESULT" field)
        /// </summary>
        public const string RealexFieldResponseTextMessage = "MESSAGE";
        /// <summary>
        /// Optional field. Realex: The customer number of the customer, Workwiz: PendingPayment.fkUserMakingPayment
        /// </summary>
        public const string RealexFieldCustomerNumber = "CUST_NUM";
        /// <summary>
        /// Optional field. Realex: A product id associated with this product, Workwiz: PendingPayment.fkPurchase
        /// </summary>
        public const string RealexFieldProductId = "PROD_ID";
        public static readonly Regex RealexFieldProductIdDisallowRegex = new Regex(@"[^a-zA-Z0-9\–""_.,+\@]");

        /// <summary>
        /// (FIELD ADDED IN HPP / Nov 2013 : not available in RealauthRedirect)
        /// Used to set which URL that Realex Payments 
        /// will send the transaction response to. When 
        /// you are creating your account with Realex
        /// Payments, you will be asked to provide a 
        /// response URL for your account. If this field is 
        /// sent in the post request, it takes precedence 
        /// over the response URL set on your Realex
        /// Payments account. If Realex Payments 
        /// cannot connect to this URL, the response 
        /// URL set on your account will be attempted. 
        /// </summary>
        public const string RealexFieldResponseUrl = "MERCHANT_RESPONSE_URL";

        /// <summary>
        /// (FIELD ADDED IN HPP / Nov 2013 : not available in RealauthRedirect)
        /// This field determines whether RealVault is activated or not. If it is set 
        /// to "1" then card storage is enabled, if set to "0" then it is not.
        /// <table>
        /// <tr><th>CARD_STORAGE_ENABLE</th><th>OFFER_SAVE_CARD</th><th>Description</th></tr>
        /// <tr><td>                  1</td><td>              1</td><td>Card storage enabled. Checkbox displayed to the customer. Card details will be stored if customer selects the checkbox. If not, card details will not be saved. (Scenario 1)</td></tr>
        /// <tr><td>                  1</td><td>              0</td><td>Card storage enabled. Checkbox not displayed to the customer. Card details will be stored automatically. (Scenario 2)</td></tr>
        /// <tr><td>                  0</td><td>              0</td><td>Card Storage disabled. Checkbox not presented to customer. (Scenario 3)</td></tr>
        /// <tr><td>                  0</td><td>              1</td><td>Error thrown.</td></tr>
        /// </table>
        /// </summary>
        public const string RealVaultFieldSaveCard = "CARD_STORAGE_ENABLE";
        /// <summary>
        /// This hidden field activates RealVault. It also determines 
        /// whether or not to show the “Save Card Details” tick box to the 
        /// customer. If this field is set to “1” then the tick box is shown, if set to 
        /// “0” then it is not. 
        /// If the “Save Card Details” tick box is hidden on the payment page for 
        /// auto-storing of card details, this value will need to be set to “1”. 
        /// </summary>
        public const string RealVaultFieldOfferSaveCard = "OFFER_SAVE_CARD";
        /// <summary>
        /// This field contains the payer reference used for this cardholder. If 
        /// this field is empty or missing and PAYER_EXIST = 0, then a payer ref 
        /// will be automatically generated. 
        /// To add another card to an existing payer the PAYER_REF field should 
        /// be set to their existing payer reference. 
        /// This field is mandatory if the OFFER_SAVE_CARD is set to 1 
        /// and the PAYER_EXIST flag is set to 1. If PAYER_EXIST = 1 and 
        /// OFFER_SAVE_CARD = 1, a 5xx error should be returned if the field 
        /// is empty or missing:
        /// 5xx “Mandatory field missing. 
        /// PAYER_REF not present in request” 
        /// Realex suggests that the merchant supplies the payer 
        /// reference as it will be easier for the merchant to manage their 
        /// payers. 
        /// </summary>
        public const string RealVaultPayerRef = "PAYER_REF";
        public static readonly Regex RealexFieldPayerRefDisallowRegex = new Regex(@"[^a-zA-Z0-9_]");

        /// <summary>
        /// The reference to use for the payment method saved. If this field 
        /// is not present an alphanumeric reference will be automatically
        /// generated. 
        /// Realex suggests that the merchant supplies the payer 
        /// reference as it will be easier for the merchant to manage their 
        /// payers. 
        /// </summary>
        public const string RealVaultPaymentRef = "PMT_REF";
        public static readonly Regex RealexFieldPaymentRefDisallowRegex = new Regex(@"[^a-zA-Z0-9]");
        /// <summary>
        /// If you wish to add a new card to an existing payer, this field should be 
        /// set to 1. Otherwise this should be set to “0”, i.e. if it is a new payer. 
        /// If the payer exists and the value is set to “1”, Realex will not create a 
        /// new payer, but will add the new card used to the payer specified in 
        /// the PAYER_REF field. 
        /// </summary>
        public const string RealVaultPayerExist = "PAYER_EXIST";
    }
}
