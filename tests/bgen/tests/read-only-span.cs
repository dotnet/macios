// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

#nullable enable

using System;
using Foundation;
using ObjCRuntime;

namespace SpanTests {
	[BaseType (typeof (NSObject))]
	interface SpanConsumer {
		[Export ("initWithString:")]
		NativeHandle Constructor (string value);

		[Bind ("initWithString:")]
		NativeHandle Constructor (ReadOnlySpan<char> value);

		[Bind ("initWithString:attributes:")]
		NativeHandle Constructor (ReadOnlySpan<char> @string, [NullAllowed] NSObject attributes);

		[Static]
		[Bind ("consume:")]
		void Consume (ReadOnlySpan<char> value);

		[Bind ("consume:other:")]
		void Consume (ReadOnlySpan<char> first, ReadOnlySpan<char> second);
	}
}
