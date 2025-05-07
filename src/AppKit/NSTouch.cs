using System;
using System.ComponentModel;

#nullable enable

#if !XAMCORE_5_0 && __MACCATALYST__

namespace AppKit {
	[EditorBrowsable (EditorBrowsableState.Never)]
	[Obsolete ("This type is does not exist on this platform.")]
	public partial class NSTouch {
	}
}

#endif // !XAMCORE_5_0
