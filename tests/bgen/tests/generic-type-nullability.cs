using System;

using Foundation;
using ObjCRuntime;
#if IOS
using UIKit;
#endif

#nullable enable

namespace NS {
	[BaseType (typeof (NSObject))]
	interface Widget {
		// Basic: two nullable generic args
		[Export ("authenticateHandler")]
		[NullAllowed]
		Action<NSObject?, NSError?> AuthenticateHandler { get; set; }

		// Three nullable generic args
		[Export ("completionHandler")]
		[NullAllowed]
		Action<NSObject?, NSArray?, NSError?> CompletionHandler { get; set; }

		// Non-nullable generic args (should NOT get ?)
		[Export ("nonNullableHandler")]
		[NullAllowed]
		Action<NSObject, NSError> NonNullableHandler { get; set; }

		// Value type argument between nullable reference types
		[Export ("withValueType")]
		[NullAllowed]
		Action<NSObject?, int, NSError?> WithValueType { get; set; }

		// Four nullable reference type args
		[Export ("manyNullableArgs")]
		[NullAllowed]
		Action<NSObject?, NSString?, NSArray?, NSError?> ManyNullableArgs { get; set; }

		// Mixed: first and last non-nullable, middle nullable
		[Export ("mixedMiddleNullable")]
		[NullAllowed]
		Action<NSObject, NSString?, NSError> MixedMiddleNullable { get; set; }

		// Multiple value types interleaved with nullable reference types
		[Export ("multipleValueTypes")]
		[NullAllowed]
		Action<NSObject?, int, bool, NSError?> MultipleValueTypes { get; set; }

		// Five args with alternating nullable/non-nullable
		[Export ("alternatingNullability")]
		[NullAllowed]
		Action<NSObject?, NSString, NSArray?, NSError, NSObject?> AlternatingNullability { get; set; }

		// All non-nullable reference types (5 args)
		[Export ("allNonNullable")]
		[NullAllowed]
		Action<NSObject, NSString, NSArray, NSError, NSObject> AllNonNullable { get; set; }

		// Value type at the end
		[Export ("valueTypeAtEnd")]
		[NullAllowed]
		Action<NSObject?, NSError?, int> ValueTypeAtEnd { get; set; }
	}
}
