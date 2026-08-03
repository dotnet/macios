using System.ComponentModel;
using System.Reflection;
using System.Collections;

#nullable enable

namespace Foundation {
	public partial class NSBundle : NSObject {
		/// <summary>Gets the localized string identified by the specified key and table.</summary>
		/// <param name="key">The key for the localized string.</param>
		/// <param name="value">The value to return if no localized string exists for <paramref name="key" />.</param>
		/// <param name="table">The table in which to look up <paramref name="key" />.</param>
		/// <returns>The localized string, or <paramref name="value" /> if no localized string is found.</returns>
		public NSString GetLocalizedString (string key, string? value = null, string? table = null)
		{
			return GetLocalizedString ((NSString) key, (NSString?) value, (NSString?) table);
		}

		/// <summary>Returns the paths for all bundle resources with the specified extension.</summary>
		/// <param name="fileExtension">The file extension to filter resources by.</param>
		/// <returns>An array of file paths for matching resources.</returns>
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
