#if !__TVOS__

using System;
using System.ComponentModel;

using Foundation;
using ObjCRuntime;

#nullable enable

namespace Phase {
	public partial class PhaseConeDirectivityModelParameters {
		[EditorBrowsable (EditorBrowsableState.Never)]
		[UnsupportedOSPlatform ("ios")]
		[UnsupportedOSPlatform ("maccatalyst")]
		[UnsupportedOSPlatform ("tvos")]
		[UnsupportedOSPlatform ("macos")]
		public PhaseConeDirectivityModelParameters ()
			: base (NSObjectFlag.Empty)
		{
			throw new PlatformNotSupportedException (Constants.ApiRemovedGeneral);
		}
	}
}

#endif // !__TVOS__
