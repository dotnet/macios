using System.ComponentModel;

#nullable enable

#if !XAMCORE_5_0 && __MACCATALYST__

namespace AppKit {
	[EditorBrowsable (EditorBrowsableState.Never)]
	[Obsolete (Constants.ApiUnavailableOnThisPlatform)]
	public partial class NSTouch {
	}
}

#endif // !XAMCORE_5_0
