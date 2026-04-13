using System;
using UIKit;
using ObjCRuntime;
using Foundation;

namespace Bug35176 {

	[Introduced (PlatformName.iOS, 14, 3)]
	[Introduced (PlatformName.MacOSX, 26, 2)]
	[Introduced (PlatformName.MacCatalyst, 18, 2)]
	[Protocol]
	interface FooInterface {

		[Abstract]
		[Export ("fooView")]
		UIView FooView { get; set; }

		[Export ("BarView")]
		UIView BarView {
			[Introduced (PlatformName.iOS, 14, 4)]
			[Introduced (PlatformName.MacCatalyst, 18, 3)]
			get;
		}

		[Export ("barMember:")]
		UIView GetBarMember (int x);
	}

	[BaseType (typeof (NSObject))]
	interface BarObject : FooInterface {

	}
}
