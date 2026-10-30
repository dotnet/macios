// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

#nullable enable

using Foundation;
using ObjCRuntime;

namespace NewInstances {
	[BaseType (typeof (NSObject))]
	interface Font {
#if INVALID_RETURN_TYPE
		[Static]
		[Export ("invalid")]
		[return: AlwaysReturnNewInstance]
		string Invalid ();
#elif VOID_RETURN_TYPE
		[Static]
		[Export ("invalid")]
		[return: AlwaysReturnNewInstance]
		void Invalid ();
#elif ABSTRACT_RETURN_TYPE
		[Static]
		[Export ("invalid")]
		[return: AlwaysReturnNewInstance]
		AbstractFont Invalid ();
#elif DICTIONARY_RETURN_TYPE
		[Static]
		[Export ("invalid")]
		[return: AlwaysReturnNewInstance]
		FontOptions Invalid ();
#elif BINDAS_RETURN_TYPE
		[Static]
		[Export ("invalid")]
		[return: AlwaysReturnNewInstance, BindAs (typeof (int))]
		NSNumber Invalid ();
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

#if ABSTRACT_RETURN_TYPE
	[Abstract]
	[BaseType (typeof (NSObject))]
	interface AbstractFont {
	}
#endif

#if DICTIONARY_RETURN_TYPE
	[StrongDictionary ("FontOptionKeys")]
	interface FontOptions {
	}

	[Static]
	interface FontOptionKeys {
	}
#endif
}
