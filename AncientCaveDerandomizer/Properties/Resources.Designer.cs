﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AncientCaveDerandomizer.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("AncientCaveDerandomizer.Properties.Resources", typeof(Resources).Assembly);
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
        ///   Looks up a localized string similar to &lt;html&gt;
        ///&lt;a href=&quot;main.html&quot;&gt;Back&lt;/a&gt;
        ///&lt;h3&gt;Derandomization&lt;/h3&gt;
        ///The main purpose of this tool is to generate predictably-constructed Ancient Caves, allowing race runs to occur through identical caves.  This is done by tweaking some Lufia 2 code and injecting some pre-generated random values.
        ///&lt;br&gt;&amp;nbsp;&lt;br&gt;
        ///The original Lufia 2 generates Ancient Cave data on-the-fly when you change floors using a frame counter found in RAM at 7E0040 and a pile of 0x38 values at 7E0520 which get modified when performing cer [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string derandomization {
            get {
                return ResourceManager.GetString("derandomization", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;html&gt;
        ///&lt;a href=&quot;main.html&quot;&gt;Back&lt;/a&gt;
        ///&lt;h3&gt;Item Distributions&lt;/h3&gt;
        ///Up to 8 chests are generated per floor.  For each chest, one of six categories of item is placed into each.  This is done by generating a random 8-bit value using the Ancient Cave random values table, and thresholding it against a bunch of different probability values.  The default values shown for these six options in the tool reflect the values used by the unmodified game; ie, selecting &quot;Manual&quot; and tapping &quot;Up&quot; on blue chests a few times  [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string distribution {
            get {
                return ResourceManager.GetString("distribution", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;html&gt;
        ///&lt;a href=&quot;main.html&quot;&gt;Back&lt;/a&gt;
        ///&lt;h3&gt;Item Quality&lt;/h3&gt;
        ///Item quality is defined by a table of &quot;desirability&quot; values I&apos;ve laid out for each item in the game.  A value of 1 indicates a less desirable item, while a value of 9 indicates a more desirable item.  A value of 0 indicates the item should never appear in chests.  These values were laid out without a whole lot of thought and are subject to change with input from users.  
        ///&lt;br&gt;&amp;nbsp;&lt;br&gt;
        ///Setting item quality higher will result in more desirable it [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string itemquality {
            get {
                return ResourceManager.GetString("itemquality", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;html&gt;
        ///&lt;h3&gt;Ancient Cave Derandomizer Help&lt;/h3&gt;
        ///Basic stuff:&lt;br&gt;
        ///&lt;a href=&quot;derandomization.html&quot;&gt;Derandomization&lt;/a&gt;&lt;br&gt;
        ///&lt;a href=&quot;itemquality.html&quot;&gt;Item quality&lt;/a&gt;&lt;br&gt;
        ///&lt;a href=&quot;distribution.html&quot;&gt;Chest type distribution&lt;/a&gt;&lt;br&gt;
        ///&amp;nbsp;&lt;br&gt;
        ///Technical stuff:&lt;br&gt;
        ///&lt;i&gt;To be added later.&lt;/i&gt;
        ///&lt;/html&gt;.
        /// </summary>
        internal static string main {
            get {
                return ResourceManager.GetString("main", resourceCulture);
            }
        }
    }
}
