﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RegexHotKey {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Errors {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Errors() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("RegexHotKey.Errors", typeof(Errors).Assembly);
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
        ///   Looks up a localized string similar to Failed to register subscriber  {0}.{1}{2}  in assembly {3}. The specified callback type was {4}..
        /// </summary>
        internal static string CALLBACK_TYPE_INVALID {
            get {
                return ResourceManager.GetString("CALLBACK_TYPE_INVALID", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Failed to register method {0}.{1}{2}  in assembly {3}. Expected 1 parameter of type System.Text.Match..
        /// </summary>
        internal static string METHOD_SIGNATURE_INVALID {
            get {
                return ResourceManager.GetString("METHOD_SIGNATURE_INVALID", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to .
        /// </summary>
        internal static string NULL_ARGUMENT {
            get {
                return ResourceManager.GetString("NULL_ARGUMENT", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Failed to log null item parameter name: {0}.
        /// </summary>
        internal static string NULL_ITEM_LOGGED {
            get {
                return ResourceManager.GetString("NULL_ITEM_LOGGED", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Failed to register method {0}.{1}{2} in assembley {3}. Specified callback type was was {4}..
        /// </summary>
        internal static string SUBSCRIBER_PARAMETER_TYPE_MISMATCH {
            get {
                return ResourceManager.GetString("SUBSCRIBER_PARAMETER_TYPE_MISMATCH", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Attempted to transform {1} while regex {2} returnerned no match..
        /// </summary>
        internal static string TRANSFORMING_UNMATHCED_STREAM {
            get {
                return ResourceManager.GetString("TRANSFORMING_UNMATHCED_STREAM", resourceCulture);
            }
        }
    }
}
