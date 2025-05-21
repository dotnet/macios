// Compatibility stubs

using System;
using System.ComponentModel;
using ObjCRuntime;

#nullable enable

namespace CoreSpotlight {

#if !TV
	public partial class CSSearchQueryContext {

		[EditorBrowsable (EditorBrowsableState.Never)]
		[Obsolete ("This property was removed. The getter always returns null and the setter throws and InvalidOperationException.")]
		[UnsupportedOSPlatform ("ios")]
		[UnsupportedOSPlatform ("tvos")]
		[UnsupportedOSPlatform ("maccatalyst")]
		[UnsupportedOSPlatform ("macos")]
		public virtual string []? ProtectionClasses {
			get => null;
			set => throw new InvalidOperationException (Constants.ApiRemovedGeneral);
		}
	}
#endif
}

