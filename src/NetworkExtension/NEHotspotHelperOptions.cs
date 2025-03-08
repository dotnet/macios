#nullable enable

#if !MONOMAC && !TVOS

using Foundation;

namespace NetworkExtension {

	public class NEHotspotHelperOptions : DictionaryContainer {

#if !COREBUILD
		public NEHotspotHelperOptions () : base (new NSMutableDictionary ()) { }
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
