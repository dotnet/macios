// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using CoreFoundation;
using Foundation;
using ObjCRuntime;

namespace NativeArrayMarshaling {
	delegate void ArrayCallback (string [] strings, NSObject [] objects, CFBundle [] bundles, [NullAllowed] INSCoding [] coders);

	[BaseType (typeof (NSObject))]
	interface ArrayConsumer {
		[Export ("initWithObjects:")]
		NativeHandle Constructor (NSObject [] objects);

		[Export ("consumeStrings:objects:bundles:coders:")]
		int Consume (string [] strings, NSObject [] objects, CFBundle [] bundles, [NullAllowed] INSCoding [] coders);

		[Static]
		[Export ("consumeNullableStrings:objects:")]
		void ConsumeNullable ([NullAllowed] string [] strings, [NullAllowed] NSObject [] objects);

		[Export ("objects"), NullAllowed]
		NSObject [] Objects { get; set; }

		[Export ("strings")]
		string [] Strings { get; set; }

		[Export ("callback")]
		ArrayCallback Callback { get; set; }

		[Export ("replaceStrings:objects:bundles:coders:result:")]
		int Replace (ref string [] strings, ref NSObject [] objects, ref CFBundle [] bundles, ref INSCoding [] coders, out NSObject [] result);

		[Export ("consumeObjects:value:")]
		void ConsumeWithRefValue (NSObject [] objects, ref int value);

		[Export ("consumeJagged:")]
		void ConsumeJagged (NSObject [] [] objects);

		[Export ("consumeMatrix:")]
		void ConsumeMatrix (NSObject [,] objects);
	}
}
