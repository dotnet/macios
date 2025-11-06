#nullable enable

#if __IOS__

using CoreFoundation;

namespace StoreKit {

#if !COREBUILD
	public partial class SKOverlayAppConfiguration {
		public NSObject? this [string i] {
			get => GetAdditionalValue (i);
			set => SetAdditionalValue (value, i);
		}
	}
#endif
}
#endif
