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
		[Export ("authenticateHandler")]
		[NullAllowed]
		Action<NSObject?, NSError?> AuthenticateHandler { get; set; }

		[Export ("completionHandler")]
		[NullAllowed]
		Action<NSObject?, NSArray?, NSError?> CompletionHandler { get; set; }

		[Export ("nonNullableHandler")]
		[NullAllowed]
		Action<NSObject, NSError> NonNullableHandler { get; set; }
	}
}
