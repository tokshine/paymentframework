﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Workwiz.PaymentFramework.Shared {
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "14.0.0.0")]
    public sealed partial class Civica : global::System.Configuration.ApplicationSettingsBase {
        
        private static Civica defaultInstance = ((Civica)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Civica())));
        
        public static Civica Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("CIT_AI_CIVIC_eBOOKINGS_V1")]
        public string CivicaAppId {
            get {
                return ((string)(this["CivicaAppId"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("http://localhost:2437/CivicaIcon/WebForm1.aspx")]
        public string CivicaPostUrl {
            get {
                return ((string)(this["CivicaPostUrl"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("http://localhost:2437/CivicaIcon/QueryAuthRequests.asmx")]
        public string CivicaQueryUrl {
            get {
                return ((string)(this["CivicaQueryUrl"]));
            }
        }
    }
}
