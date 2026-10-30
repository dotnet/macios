// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using CoreGraphics;
using Foundation;
using ObjCRuntime;

namespace SharedNativeObjectTrampolines {
	delegate void ObjectCallback (NSObject value);
	delegate void StringObjectCallback ([NullAllowed] NSString value);
	delegate void ImageCallback (CGImage value);
	delegate void ProtocolCallback (INSCopying value);
	delegate bool ReturningCallback (NSObject value);
	delegate void RefCallback (ref NSObject value);
	delegate void ArrayCallback (NSObject [] value);
	delegate void StringCallback (string value);
	delegate void MultipleCallback (NSObject first, NSObject second);
	delegate void ValueCallback (int value);
	delegate void NestedObjectCallback (NSUrl value);
	delegate void NestedCallback ([BlockCallback] NestedObjectCallback callback);

	[BaseType (typeof (NSObject))]
	interface Callbacks {
		[Export ("objectCallback")]
		ObjectCallback ObjectCallback { get; set; }

		[Export ("stringObjectCallback")]
		StringObjectCallback StringObjectCallback { get; set; }

		[Export ("imageCallback")]
		ImageCallback ImageCallback { get; set; }

		[Export ("protocolCallback")]
		ProtocolCallback ProtocolCallback { get; set; }

		[Export ("returningCallback")]
		ReturningCallback ReturningCallback { get; set; }

		[Export ("refCallback")]
		RefCallback RefCallback { get; set; }

		[Export ("arrayCallback")]
		ArrayCallback ArrayCallback { get; set; }

		[Export ("stringCallback")]
		StringCallback StringCallback { get; set; }

		[Export ("multipleCallback")]
		MultipleCallback MultipleCallback { get; set; }

		[Export ("valueCallback")]
		ValueCallback ValueCallback { get; set; }

		[Export ("nestedCallback")]
		NestedCallback NestedCallback { get; set; }
	}
}
