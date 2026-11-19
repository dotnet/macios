// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Foundation;
using ObjCRuntime;

namespace NullArgumentChecks {
	[BaseType (typeof (NSObject))]
	interface NullChecked {
		[Export ("check:values:optional:")]
		void Check (string @event, string [] values, [NullAllowed] string optional);
	}
}
