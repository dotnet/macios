using System;

using CoreVideo;
using Foundation;
using ObjCRuntime;

namespace NS {
	[BaseType (typeof (NSObject))]
	interface ReleaseAttributeTest {
		[Export ("getNSObject")]
		[return: Release]
		NSObject GetNSObject ();

		[Export ("getNativeObject")]
		[return: Release]
		CVPixelBuffer GetNativeObject ();

		[Export ("getINativeObject")]
		[return: Release]
		Selector GetINativeObject ();

		[Export ("newObjects")]
		[return: Release]
		NSObject [] GetObjects ();

		[Export ("newObject")]
		NSObject NewObject { [return: Release] get; }

		[Export ("newWritableObject")]
		NSObject NewWritableObject { [return: Release] get; set; }

		[Export ("newObjectArray")]
		NSObject [] NewObjectArray { [return: Release] get; }
	}
}
