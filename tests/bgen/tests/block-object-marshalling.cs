// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using CoreGraphics;
using Foundation;
using ObjCRuntime;

namespace BlockObjectMarshalling {
	delegate NSData ErrorHandler (NSObject reply, ref NSError error);
	delegate NSData OutErrorHandler (out NSError error);
	delegate Callbacks BoundObjectHandler (ref Callbacks value);
	delegate NSError ForcedObjectHandler ([ForcedType] ref NSError value);
	delegate Callbacks ForcedOwnedObjectHandler ([ForcedType (true)] ref Callbacks value);
	delegate CGPDFDocument NativeObjectHandler (ref CGPDFDocument value);
	delegate INSCoding ProtocolHandler (ref INSCoding value);

	[BaseType (typeof (NSObject))]
	interface Callbacks {
		[Export ("errorHandler")]
		ErrorHandler ErrorHandler { get; set; }

		[Export ("outErrorHandler")]
		OutErrorHandler OutErrorHandler { get; set; }

		[Export ("boundObjectHandler")]
		BoundObjectHandler BoundObjectHandler { get; set; }

		[Export ("forcedObjectHandler")]
		ForcedObjectHandler ForcedObjectHandler { get; set; }

		[Export ("forcedOwnedObjectHandler")]
		ForcedOwnedObjectHandler ForcedOwnedObjectHandler { get; set; }

		[Export ("nativeObjectHandler")]
		NativeObjectHandler NativeObjectHandler { get; set; }

		[Export ("protocolHandler")]
		ProtocolHandler ProtocolHandler { get; set; }
	}
}
