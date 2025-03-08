#nullable enable

using System.Runtime.Versioning;

using Foundation;

namespace NetworkExtension {

#if NET
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos17.0")]
#endif
	public class NEVpnConnectionStartOptions : DictionaryContainer {

#if !COREBUILD
		public NEVpnConnectionStartOptions () : base (new NSMutableDictionary ()) { }
		public NEVpnConnectionStartOptions (NSDictionary dictionary) : base (dictionary) { }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public NSString? Username {
			get {
				return GetNSStringValue (NEVpnConnectionStartOptionInternal.Username);
			}
			set {
				SetStringValue (NEVpnConnectionStartOptionInternal.Username, value);
			}
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public NSString? Password {
			get {
				return GetNSStringValue (NEVpnConnectionStartOptionInternal.Password);
			}
			set {
				SetStringValue (NEVpnConnectionStartOptionInternal.Password, value);
			}
		}
#endif
	}
}
