// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Foundation;
using UIKit;

namespace Test {
	[BaseType (typeof (UITabBarController))]
	interface MyObject {
		[New]
		[Export ("supportedInterfaceOrientations")]
		UIInterfaceOrientationMask SupportedInterfaceOrientations { get; }
	}

	[BaseType (typeof (UITabBarController))]
	interface MyWriteOnlyObject {
		[New]
		[Export ("supportedInterfaceOrientations")]
		UIInterfaceOrientationMask SupportedInterfaceOrientations { set; }
	}
}
