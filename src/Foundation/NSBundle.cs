using System;
using System.ComponentModel;
using System.Reflection;
using System.Collections;
using System.Runtime.InteropServices;

using ObjCRuntime;

#nullable enable

namespace Foundation {
	public partial class NSBundle : NSObject {
		/// <param name="key">The key for the string</param>
		///         <param name="value">The value to use if no string exists at the key.</param>
		///         <param name="table">The table in which to look up the keyed value.</param>
		///         <summary>Gets the localized string for the string that is identified by the provided <paramref name="key" /> into <paramref name="table" />, or <paramref name="value" /> if no string is found.</summary>
		///         <returns>The localized string for the string that is identified by the provided <paramref name="key" /> into <paramref name="table" />, or <paramref name="value" /> if no string is found.</returns>
		///         <remarks>To be added.</remarks>
		public NSString GetLocalizedString (string key, string? value = null, string? table = null)
		{
			return GetLocalizedString ((NSString) key, (NSString?) value, (NSString?) table);
		}

		/// <param name="fileExtension">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public string [] PathsForResources (string fileExtension)
		{
			return PathsForResources (fileExtension, null);
		}

#if !MONOMAC && !XAMCORE_5_0
		/// <summary>Default constructor that initializes a new instance of this class with no parameters.</summary>
		///         <remarks>
		///         </remarks>
		[Obsolete ("Do not use this constructor, it does not work as expected.")]
		[EditorBrowsable (EditorBrowsableState.Never)]
		public NSBundle ()
			: base (NSObjectFlag.Empty)
		{
		}
#endif // !MONOMAC && !XAMCORE_5_0
	}
}
