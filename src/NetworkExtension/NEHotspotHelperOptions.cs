#nullable enable

#if !MONOMAC && !TVOS

namespace NetworkExtension {

	/// <summary>Represents options for registering a Hotspot Helper.</summary>
	public class NEHotspotHelperOptions : DictionaryContainer {

#if !COREBUILD
		/// <summary>Creates a new empty hotspot helper options object.</summary>
		public NEHotspotHelperOptions () : base (new NSMutableDictionary ()) { }
		/// <param name="dictionary">The dictionary.</param>
		///         <summary>Creates a new hotspot helper options object from the provided dictionary.</summary>
		public NEHotspotHelperOptions (NSDictionary dictionary) : base (dictionary) { }

		/// <summary>Gets or sets the display name for the helper.</summary>
		public NSString? DisplayName {
			get {
				return GetNSStringValue (NEHotspotHelperOptionInternal.DisplayName);
			}
			set {
				SetStringValue (NEHotspotHelperOptionInternal.DisplayName, value);
			}
		}
#endif
	}
}

#endif // !MONOMAC
