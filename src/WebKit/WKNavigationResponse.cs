using System;
using System.ComponentModel;

#nullable enable

namespace WebKit {
	public partial class WKNavigationResponse {
#if !XAMCORE_5_0
		[Obsolete ("Do not use this constructor, it does not work as expected.")]
		[EditorBrowsable (EditorBrowsableState.Never)]
		[UnsupportedOSPlatform ("ios")]
		[UnsupportedOSPlatform ("maccatalyst")]
		[UnsupportedOSPlatform ("macos")]
		public WKNavigationResponse ()
			: base ()
		{
		}
#endif // !XAMCORE_5_0
	}
}
