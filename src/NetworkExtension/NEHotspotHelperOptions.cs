#nullable enable

#if !MONOMAC && !TVOS

using Foundation;

namespace NetworkExtension {

	public class NEHotspotHelperOptions : DictionaryContainer {

#if !COREBUILD
		/// <summary>Creates a new empty hotspot helper options object.</summary>
		///         <remarks>To be added.</remarks>
		public NEHotspotHelperOptions () : base (new NSMutableDictionary ()) { }
		/// <param name="dictionary">To be added.</param>
		///         <summary>Creates a new hotspot helper options object from the provided dictionary.</summary>
		///         <remarks>To be added.</remarks>
		public NEHotspotHelperOptions (NSDictionary dictionary) : base (dictionary) { }

		/// <summary>Gets or sets the display name for the helper.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
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
