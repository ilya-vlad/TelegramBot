﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Bot.Resources {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Messages {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Messages() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Bot.Resources.Messages", typeof(Messages).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to UAH.
        /// </summary>
        internal static string CurrencyUAH {
            get {
                return ResourceManager.GetString("CurrencyUAH", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Error getting data..
        /// </summary>
        internal static string ErrorGettingData {
            get {
                return ResourceManager.GetString("ErrorGettingData", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Example: &quot;usd 10.10.2020&quot; or &quot;usd&quot;.
        /// </summary>
        internal static string ExampleRequest {
            get {
                return ResourceManager.GetString("ExampleRequest", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Exchange rate data not found on.
        /// </summary>
        internal static string ExchangeRateNotFound {
            get {
                return ResourceManager.GetString("ExchangeRateNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Exchange rate on.
        /// </summary>
        internal static string ExchangeRateOn {
            get {
                return ResourceManager.GetString("ExchangeRateOn", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Hello! It&apos;s exchange rates bot!.
        /// </summary>
        internal static string Greeting {
            get {
                return ResourceManager.GetString("Greeting", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Send me the currency and the date separated by a space or just the currency..
        /// </summary>
        internal static string Help {
            get {
                return ResourceManager.GetString("Help", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Sorry, but your request is incorrect..
        /// </summary>
        internal static string IncorrectRequest {
            get {
                return ResourceManager.GetString("IncorrectRequest", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Please, try again :).
        /// </summary>
        internal static string TryAgain {
            get {
                return ResourceManager.GetString("TryAgain", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unknown error..
        /// </summary>
        internal static string UnknownError {
            get {
                return ResourceManager.GetString("UnknownError", resourceCulture);
            }
        }
    }
}
