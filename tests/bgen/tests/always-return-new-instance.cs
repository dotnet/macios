// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

#nullable enable

using Foundation;

namespace NewInstances {
	[BaseType (typeof (NSObject))]
	interface Font {
#if INVALID_RETURN_TYPE
		[Static]
		[Export ("invalid")]
		[return: AlwaysReturnNewInstance]
		string Invalid ();
#endif

		[Static]
		[Export ("font")]
		[return: AlwaysReturnNewInstance]
		Font Create ();

		[Export ("resizedFont:")]
		[return: AlwaysReturnNewInstance, NullAllowed]
		Font Resize (float size);

		[Export ("printerFont")]
		[AlwaysReturnNewInstance]
		Font PrinterFont { get; }

		[Export ("newFont")]
		[return: AlwaysReturnNewInstance, Release]
		Font CreateOwned ();
	}
}
