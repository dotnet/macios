#if __IOS__
using System;
using System.ComponentModel;

using ObjCRuntime;

namespace WatchKit {

	[Unavailable (PlatformName.iOS, PlatformArchitecture.All)]
	[Obsolete (Constants.WatchKitRemoved)]
	[EditorBrowsable (EditorBrowsableState.Never)]
	public enum WKUserNotificationInterfaceType : long {
		Default = 0,
		Custom = 1,
	}
}
#endif // __IOS__
