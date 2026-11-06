// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Foundation;

namespace NativeLibrary {
	delegate Widget2 [] ObjectArrayHandler ();

	[BaseType (typeof (NSObject))]
	interface Widget {
		[Export ("center")]
		ObjectArrayHandler Center { get; set; }

		[Export ("widgets")]
		Widget2 [] Widgets { get; }
	}

	[BaseType (typeof (NSObject))]
	interface Widget2 {
	}
}
