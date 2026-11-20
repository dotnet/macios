// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Foundation;
using UIKit;

namespace Test {
	[BaseType (typeof (UINavigationController))]
	interface MyNavigationController {
	}

	[Category]
	[BaseType (typeof (MyNavigationController))]
	interface MyCategory : IUIAdaptivePresentationControllerDelegate {
		[Export ("categoryMethod")]
		void CategoryMethod ();
	}

	[Protocol]
	interface MyProtocol {
		[Abstract]
		[Export ("protocolMethod")]
		void ProtocolMethod ();
	}

	[Category]
	[BaseType (typeof (MyNavigationController))]
	interface MyProtocolCategory : MyProtocol {
	}
}
