﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Workwiz.PaymentFramework.Shared.CivicaAuthRequest {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://tempuri.org/QueryAuthRequests/Service1", ConfigurationName="CivicaAuthRequest.QueryAuthRequestSrvSoap")]
    public interface QueryAuthRequestSrvSoap {
        
        // CODEGEN: Generating message contract since message part namespace (http://altsql71/XMLSchema/epayments/standard) does not match the default value (http://tempuri.org/QueryAuthRequests/Service1)
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/QueryAuthRequests/Service1/Query", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        [return: System.ServiceModel.MessageParameterAttribute(Name="RespMessage")]
        Workwiz.PaymentFramework.Shared.CivicaAuthRequest.QueryResponse Query(Workwiz.PaymentFramework.Shared.CivicaAuthRequest.QueryRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/QueryAuthRequests/Service1/Query", ReplyAction="*")]
        System.Threading.Tasks.Task<Workwiz.PaymentFramework.Shared.CivicaAuthRequest.QueryResponse> QueryAsync(Workwiz.PaymentFramework.Shared.CivicaAuthRequest.QueryRequest request);
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1064.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://altsql71/XMLSchema/epayments/standard")]
    public partial class ReqMessage : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string callingApplicationIdentifierField;
        
        private CriteriaStructure[] criteraListField;
        
        private string pageNumField;
        
        private string pageSizeField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public string CallingApplicationIdentifier {
            get {
                return this.callingApplicationIdentifierField;
            }
            set {
                this.callingApplicationIdentifierField = value;
                this.RaisePropertyChanged("CallingApplicationIdentifier");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("CriteraList", Order=1)]
        public CriteriaStructure[] CriteraList {
            get {
                return this.criteraListField;
            }
            set {
                this.criteraListField = value;
                this.RaisePropertyChanged("CriteraList");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType="integer", Order=2)]
        public string pageNum {
            get {
                return this.pageNumField;
            }
            set {
                this.pageNumField = value;
                this.RaisePropertyChanged("pageNum");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType="integer", Order=3)]
        public string pageSize {
            get {
                return this.pageSizeField;
            }
            set {
                this.pageSizeField = value;
                this.RaisePropertyChanged("pageSize");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1064.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://altsql71/XMLSchema/epayments/standard")]
    public partial class CriteriaStructure : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string columnField;
        
        private string valueField;
        
        private string operatorField;
        
        private AndOr booleanOpField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public string Column {
            get {
                return this.columnField;
            }
            set {
                this.columnField = value;
                this.RaisePropertyChanged("Column");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=1)]
        public string Value {
            get {
                return this.valueField;
            }
            set {
                this.valueField = value;
                this.RaisePropertyChanged("Value");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=2)]
        public string Operator {
            get {
                return this.operatorField;
            }
            set {
                this.operatorField = value;
                this.RaisePropertyChanged("Operator");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=3)]
        public AndOr BooleanOp {
            get {
                return this.booleanOpField;
            }
            set {
                this.booleanOpField = value;
                this.RaisePropertyChanged("BooleanOp");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1064.2")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://altsql71/XMLSchema/epayments/standard")]
    public enum AndOr {
        
        /// <remarks/>
        And,
        
        /// <remarks/>
        Or,
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1064.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://altsql71/XMLSchema/epayments/standard")]
    public partial class PaymentStructure : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string originatorsReferenceField;
        
        private string lineNumberField;
        
        private System.DateTime requestDateField;
        
        private string requestMessageField;
        
        private string responseMessageField;
        
        private string requestStatusField;
        
        private string callingApplicationIdentifierField;
        
        private string callingApplicationTransactionReferenceField;
        
        private string accountReferenceField;
        
        private string accountTypeField;
        
        private string accountMethodOfPaymentField;
        
        private decimal accountPaymentAmountField;
        
        private string accountPaymentNarrativeField;
        
        private string cardNumberField;
        
        private string cardStartDateField;
        
        private string cardEndDateField;
        
        private string cardIssueNumberField;
        
        private string merchantNumberField;
        
        private string generalLedgerCodeField;
        
        private string incomeManagementReceiptNumberField;
        
        private string paymentAuthorisationCodeField;
        
        private string cardSchemeField;
        
        private string cardTypeField;
        
        private string tranTypeField;
        
        private string iCCAppIDField;
        
        private string iCCAppEffDateField;
        
        private string iCCAppExpDateField;
        
        private string iCCAppPanSeqField;
        
        private string refundTransactionRefField;
        
        private string cHNPResultCodeField;
        
        private decimal vATField;
        
        private string vATCodeField;
        
        private string cHNameField;
        
        private string cHBusinessNameField;
        
        private string cHPremiseNumberField;
        
        private string cHPremiseNameField;
        
        private string cHStreetField;
        
        private string cHAreaField;
        
        private string cHTownField;
        
        private string cHCountyField;
        
        private string cHPostCodeField;
        
        private string pLNameField;
        
        private string pLBusinessNameField;
        
        private string pLPremiseNumberField;
        
        private string pLPremiseNameField;
        
        private string pLStreetField;
        
        private string pLAreaField;
        
        private string pLTownField;
        
        private string pLCountyField;
        
        private string pLPostcodeField;
        
        private string trueAccountReferenceField;
        
        private bool cryptoMethodField;
        
        private decimal transactionChargeField;
        
        private decimal fullCardAmountField;
        
        private bool sourceReferralFlagField;
        
        private string iCCCTTypeField;
        
        private string iCCPreferredCardNameField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public string OriginatorsReference {
            get {
                return this.originatorsReferenceField;
            }
            set {
                this.originatorsReferenceField = value;
                this.RaisePropertyChanged("OriginatorsReference");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType="integer", Order=1)]
        public string LineNumber {
            get {
                return this.lineNumberField;
            }
            set {
                this.lineNumberField = value;
                this.RaisePropertyChanged("LineNumber");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=2)]
        public System.DateTime RequestDate {
            get {
                return this.requestDateField;
            }
            set {
                this.requestDateField = value;
                this.RaisePropertyChanged("RequestDate");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=3)]
        public string RequestMessage {
            get {
                return this.requestMessageField;
            }
            set {
                this.requestMessageField = value;
                this.RaisePropertyChanged("RequestMessage");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=4)]
        public string ResponseMessage {
            get {
                return this.responseMessageField;
            }
            set {
                this.responseMessageField = value;
                this.RaisePropertyChanged("ResponseMessage");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=5)]
        public string RequestStatus {
            get {
                return this.requestStatusField;
            }
            set {
                this.requestStatusField = value;
                this.RaisePropertyChanged("RequestStatus");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=6)]
        public string CallingApplicationIdentifier {
            get {
                return this.callingApplicationIdentifierField;
            }
            set {
                this.callingApplicationIdentifierField = value;
                this.RaisePropertyChanged("CallingApplicationIdentifier");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=7)]
        public string CallingApplicationTransactionReference {
            get {
                return this.callingApplicationTransactionReferenceField;
            }
            set {
                this.callingApplicationTransactionReferenceField = value;
                this.RaisePropertyChanged("CallingApplicationTransactionReference");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=8)]
        public string AccountReference {
            get {
                return this.accountReferenceField;
            }
            set {
                this.accountReferenceField = value;
                this.RaisePropertyChanged("AccountReference");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=9)]
        public string AccountType {
            get {
                return this.accountTypeField;
            }
            set {
                this.accountTypeField = value;
                this.RaisePropertyChanged("AccountType");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=10)]
        public string AccountMethodOfPayment {
            get {
                return this.accountMethodOfPaymentField;
            }
            set {
                this.accountMethodOfPaymentField = value;
                this.RaisePropertyChanged("AccountMethodOfPayment");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=11)]
        public decimal AccountPaymentAmount {
            get {
                return this.accountPaymentAmountField;
            }
            set {
                this.accountPaymentAmountField = value;
                this.RaisePropertyChanged("AccountPaymentAmount");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=12)]
        public string AccountPaymentNarrative {
            get {
                return this.accountPaymentNarrativeField;
            }
            set {
                this.accountPaymentNarrativeField = value;
                this.RaisePropertyChanged("AccountPaymentNarrative");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=13)]
        public string CardNumber {
            get {
                return this.cardNumberField;
            }
            set {
                this.cardNumberField = value;
                this.RaisePropertyChanged("CardNumber");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=14)]
        public string CardStartDate {
            get {
                return this.cardStartDateField;
            }
            set {
                this.cardStartDateField = value;
                this.RaisePropertyChanged("CardStartDate");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=15)]
        public string CardEndDate {
            get {
                return this.cardEndDateField;
            }
            set {
                this.cardEndDateField = value;
                this.RaisePropertyChanged("CardEndDate");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=16)]
        public string CardIssueNumber {
            get {
                return this.cardIssueNumberField;
            }
            set {
                this.cardIssueNumberField = value;
                this.RaisePropertyChanged("CardIssueNumber");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=17)]
        public string MerchantNumber {
            get {
                return this.merchantNumberField;
            }
            set {
                this.merchantNumberField = value;
                this.RaisePropertyChanged("MerchantNumber");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=18)]
        public string GeneralLedgerCode {
            get {
                return this.generalLedgerCodeField;
            }
            set {
                this.generalLedgerCodeField = value;
                this.RaisePropertyChanged("GeneralLedgerCode");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=19)]
        public string IncomeManagementReceiptNumber {
            get {
                return this.incomeManagementReceiptNumberField;
            }
            set {
                this.incomeManagementReceiptNumberField = value;
                this.RaisePropertyChanged("IncomeManagementReceiptNumber");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=20)]
        public string PaymentAuthorisationCode {
            get {
                return this.paymentAuthorisationCodeField;
            }
            set {
                this.paymentAuthorisationCodeField = value;
                this.RaisePropertyChanged("PaymentAuthorisationCode");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=21)]
        public string CardScheme {
            get {
                return this.cardSchemeField;
            }
            set {
                this.cardSchemeField = value;
                this.RaisePropertyChanged("CardScheme");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=22)]
        public string CardType {
            get {
                return this.cardTypeField;
            }
            set {
                this.cardTypeField = value;
                this.RaisePropertyChanged("CardType");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=23)]
        public string TranType {
            get {
                return this.tranTypeField;
            }
            set {
                this.tranTypeField = value;
                this.RaisePropertyChanged("TranType");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=24)]
        public string ICCAppID {
            get {
                return this.iCCAppIDField;
            }
            set {
                this.iCCAppIDField = value;
                this.RaisePropertyChanged("ICCAppID");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=25)]
        public string ICCAppEffDate {
            get {
                return this.iCCAppEffDateField;
            }
            set {
                this.iCCAppEffDateField = value;
                this.RaisePropertyChanged("ICCAppEffDate");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=26)]
        public string ICCAppExpDate {
            get {
                return this.iCCAppExpDateField;
            }
            set {
                this.iCCAppExpDateField = value;
                this.RaisePropertyChanged("ICCAppExpDate");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType="integer", Order=27)]
        public string ICCAppPanSeq {
            get {
                return this.iCCAppPanSeqField;
            }
            set {
                this.iCCAppPanSeqField = value;
                this.RaisePropertyChanged("ICCAppPanSeq");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=28)]
        public string RefundTransactionRef {
            get {
                return this.refundTransactionRefField;
            }
            set {
                this.refundTransactionRefField = value;
                this.RaisePropertyChanged("RefundTransactionRef");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=29)]
        public string CHNPResultCode {
            get {
                return this.cHNPResultCodeField;
            }
            set {
                this.cHNPResultCodeField = value;
                this.RaisePropertyChanged("CHNPResultCode");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=30)]
        public decimal VAT {
            get {
                return this.vATField;
            }
            set {
                this.vATField = value;
                this.RaisePropertyChanged("VAT");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=31)]
        public string VATCode {
            get {
                return this.vATCodeField;
            }
            set {
                this.vATCodeField = value;
                this.RaisePropertyChanged("VATCode");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=32)]
        public string CHName {
            get {
                return this.cHNameField;
            }
            set {
                this.cHNameField = value;
                this.RaisePropertyChanged("CHName");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=33)]
        public string CHBusinessName {
            get {
                return this.cHBusinessNameField;
            }
            set {
                this.cHBusinessNameField = value;
                this.RaisePropertyChanged("CHBusinessName");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=34)]
        public string CHPremiseNumber {
            get {
                return this.cHPremiseNumberField;
            }
            set {
                this.cHPremiseNumberField = value;
                this.RaisePropertyChanged("CHPremiseNumber");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=35)]
        public string CHPremiseName {
            get {
                return this.cHPremiseNameField;
            }
            set {
                this.cHPremiseNameField = value;
                this.RaisePropertyChanged("CHPremiseName");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=36)]
        public string CHStreet {
            get {
                return this.cHStreetField;
            }
            set {
                this.cHStreetField = value;
                this.RaisePropertyChanged("CHStreet");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=37)]
        public string CHArea {
            get {
                return this.cHAreaField;
            }
            set {
                this.cHAreaField = value;
                this.RaisePropertyChanged("CHArea");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=38)]
        public string CHTown {
            get {
                return this.cHTownField;
            }
            set {
                this.cHTownField = value;
                this.RaisePropertyChanged("CHTown");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=39)]
        public string CHCounty {
            get {
                return this.cHCountyField;
            }
            set {
                this.cHCountyField = value;
                this.RaisePropertyChanged("CHCounty");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=40)]
        public string CHPostCode {
            get {
                return this.cHPostCodeField;
            }
            set {
                this.cHPostCodeField = value;
                this.RaisePropertyChanged("CHPostCode");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=41)]
        public string PLName {
            get {
                return this.pLNameField;
            }
            set {
                this.pLNameField = value;
                this.RaisePropertyChanged("PLName");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=42)]
        public string PLBusinessName {
            get {
                return this.pLBusinessNameField;
            }
            set {
                this.pLBusinessNameField = value;
                this.RaisePropertyChanged("PLBusinessName");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=43)]
        public string PLPremiseNumber {
            get {
                return this.pLPremiseNumberField;
            }
            set {
                this.pLPremiseNumberField = value;
                this.RaisePropertyChanged("PLPremiseNumber");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=44)]
        public string PLPremiseName {
            get {
                return this.pLPremiseNameField;
            }
            set {
                this.pLPremiseNameField = value;
                this.RaisePropertyChanged("PLPremiseName");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=45)]
        public string PLStreet {
            get {
                return this.pLStreetField;
            }
            set {
                this.pLStreetField = value;
                this.RaisePropertyChanged("PLStreet");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=46)]
        public string PLArea {
            get {
                return this.pLAreaField;
            }
            set {
                this.pLAreaField = value;
                this.RaisePropertyChanged("PLArea");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=47)]
        public string PLTown {
            get {
                return this.pLTownField;
            }
            set {
                this.pLTownField = value;
                this.RaisePropertyChanged("PLTown");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=48)]
        public string PLCounty {
            get {
                return this.pLCountyField;
            }
            set {
                this.pLCountyField = value;
                this.RaisePropertyChanged("PLCounty");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=49)]
        public string PLPostcode {
            get {
                return this.pLPostcodeField;
            }
            set {
                this.pLPostcodeField = value;
                this.RaisePropertyChanged("PLPostcode");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=50)]
        public string TrueAccountReference {
            get {
                return this.trueAccountReferenceField;
            }
            set {
                this.trueAccountReferenceField = value;
                this.RaisePropertyChanged("TrueAccountReference");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=51)]
        public bool CryptoMethod {
            get {
                return this.cryptoMethodField;
            }
            set {
                this.cryptoMethodField = value;
                this.RaisePropertyChanged("CryptoMethod");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=52)]
        public decimal TransactionCharge {
            get {
                return this.transactionChargeField;
            }
            set {
                this.transactionChargeField = value;
                this.RaisePropertyChanged("TransactionCharge");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=53)]
        public decimal FullCardAmount {
            get {
                return this.fullCardAmountField;
            }
            set {
                this.fullCardAmountField = value;
                this.RaisePropertyChanged("FullCardAmount");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=54)]
        public bool SourceReferralFlag {
            get {
                return this.sourceReferralFlagField;
            }
            set {
                this.sourceReferralFlagField = value;
                this.RaisePropertyChanged("SourceReferralFlag");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=55)]
        public string ICCCTType {
            get {
                return this.iCCCTTypeField;
            }
            set {
                this.iCCCTTypeField = value;
                this.RaisePropertyChanged("ICCCTType");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=56)]
        public string ICCPreferredCardName {
            get {
                return this.iCCPreferredCardNameField;
            }
            set {
                this.iCCPreferredCardNameField = value;
                this.RaisePropertyChanged("ICCPreferredCardName");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1064.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://altsql71/XMLSchema/epayments/standard")]
    public partial class RespMessage : object, System.ComponentModel.INotifyPropertyChanged {
        
        private PaymentStructure[] paymentListField;
        
        private string paymentListCountField;
        
        private string responseCodeField;
        
        private string responseDescriptionField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("PaymentList", Order=0)]
        public PaymentStructure[] PaymentList {
            get {
                return this.paymentListField;
            }
            set {
                this.paymentListField = value;
                this.RaisePropertyChanged("PaymentList");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType="integer", Order=1)]
        public string PaymentListCount {
            get {
                return this.paymentListCountField;
            }
            set {
                this.paymentListCountField = value;
                this.RaisePropertyChanged("PaymentListCount");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=2)]
        public string ResponseCode {
            get {
                return this.responseCodeField;
            }
            set {
                this.responseCodeField = value;
                this.RaisePropertyChanged("ResponseCode");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=3)]
        public string ResponseDescription {
            get {
                return this.responseDescriptionField;
            }
            set {
                this.responseDescriptionField = value;
                this.RaisePropertyChanged("ResponseDescription");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="Query", WrapperNamespace="http://tempuri.org/QueryAuthRequests/Service1", IsWrapped=true)]
    public partial class QueryRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://altsql71/XMLSchema/epayments/standard", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(Namespace="http://altsql71/XMLSchema/epayments/standard")]
        public Workwiz.PaymentFramework.Shared.CivicaAuthRequest.ReqMessage ReqMessage;
        
        public QueryRequest() {
        }
        
        public QueryRequest(Workwiz.PaymentFramework.Shared.CivicaAuthRequest.ReqMessage ReqMessage) {
            this.ReqMessage = ReqMessage;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="QueryResponse", WrapperNamespace="http://tempuri.org/QueryAuthRequests/Service1", IsWrapped=true)]
    public partial class QueryResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://altsql71/XMLSchema/epayments/standard", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(Namespace="http://altsql71/XMLSchema/epayments/standard")]
        public Workwiz.PaymentFramework.Shared.CivicaAuthRequest.RespMessage RespMessage;
        
        public QueryResponse() {
        }
        
        public QueryResponse(Workwiz.PaymentFramework.Shared.CivicaAuthRequest.RespMessage RespMessage) {
            this.RespMessage = RespMessage;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface QueryAuthRequestSrvSoapChannel : Workwiz.PaymentFramework.Shared.CivicaAuthRequest.QueryAuthRequestSrvSoap, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class QueryAuthRequestSrvSoapClient : System.ServiceModel.ClientBase<Workwiz.PaymentFramework.Shared.CivicaAuthRequest.QueryAuthRequestSrvSoap>, Workwiz.PaymentFramework.Shared.CivicaAuthRequest.QueryAuthRequestSrvSoap {
        
        public QueryAuthRequestSrvSoapClient() {
        }
        
        public QueryAuthRequestSrvSoapClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public QueryAuthRequestSrvSoapClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public QueryAuthRequestSrvSoapClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public QueryAuthRequestSrvSoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        Workwiz.PaymentFramework.Shared.CivicaAuthRequest.QueryResponse Workwiz.PaymentFramework.Shared.CivicaAuthRequest.QueryAuthRequestSrvSoap.Query(Workwiz.PaymentFramework.Shared.CivicaAuthRequest.QueryRequest request) {
            return base.Channel.Query(request);
        }
                
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<Workwiz.PaymentFramework.Shared.CivicaAuthRequest.QueryResponse> Workwiz.PaymentFramework.Shared.CivicaAuthRequest.QueryAuthRequestSrvSoap.QueryAsync(Workwiz.PaymentFramework.Shared.CivicaAuthRequest.QueryRequest request) {
            return base.Channel.QueryAsync(request);
        }        
    }
}