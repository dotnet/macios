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

		[Export ("copyNSObjectArray")]
		[return: Release]
		NSObject [] CopyNSObjectArray ();

		[Export ("copyNullableNSObjectArray")]
		[return: Release, NullAllowed]
		NSObject [] CopyNullableNSObjectArray ();

		[Export ("getNSObjectArray")]
		NSObject [] GetNSObjectArray ();

		[Export ("copyNativeObjectArray")]
		[return: Release]
		CVPixelBuffer [] CopyNativeObjectArray ();

		[Export ("copyNullableNativeObjectArray")]
		[return: Release, NullAllowed]
		CVPixelBuffer [] CopyNullableNativeObjectArray ();

		[Export ("getNativeObjectArray")]
		CVPixelBuffer [] GetNativeObjectArray ();

		[Export ("copyStringArray")]
		[return: Release]
		string [] CopyStringArray ();

		[Export ("copyNullableStringArray")]
		[return: Release, NullAllowed]
		string [] CopyNullableStringArray ();

		[Export ("getStringArray")]
		string [] GetStringArray ();

		[Export ("copyIntArray")]
		[return: Release, BindAs (typeof (int []))]
		NSNumber [] CopyIntArray ();

		[Export ("copyNullableIntArray")]
		[return: Release, NullAllowed, BindAs (typeof (int []))]
		NSNumber [] CopyNullableIntArray ();

		[Export ("getIntArray")]
		[return: BindAs (typeof (int []))]
		NSNumber [] GetIntArray ();
	}
}
