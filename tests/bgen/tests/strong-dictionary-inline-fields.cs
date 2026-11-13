// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using CoreGraphics;
using Foundation;
using ObjCRuntime;
using Vision;

namespace InlineStrongDictionary {
	[iOS (18, 0), TV (18, 0), Mac (15, 0), MacCatalyst (18, 0)]
	[StrongDictionary]
	interface Options {
		[Field ("NativeCropRect", "__Internal")]
		CGRect CropRect { get; set; }

		[Field ("NativeCropAndScale", "__Internal")]
		VNImageCropAndScaleOption CropAndScale { get; set; }

		[Field ("NativeName", "/tmp/libInline.dylib")]
		string Name { get; set; }

		[Field ("NativeEnabled", "__Internal")]
		bool Enabled { get; set; }

		[Field ("NativeValues", "__Internal")]
		string [] Values { get; set; }

		[Field ("NativeReadOnly", "__Internal")]
		int ReadOnly { get; }

		[Field ("NativeWriteOnly", "__Internal")]
		int WriteOnly { set; }

		[iOS (18, 1), TV (18, 1), Mac (15, 1), MacCatalyst (18, 1)]
		[Field ("NativeNewer", "__Internal")]
		double Newer { get; set; }

		[NoTV]
		[Field ("NativeNotOnTV", "__Internal")]
		int NotOnTV { get; set; }
	}

	[Static]
	interface LegacyKeys {
		[Field ("NativeLegacy", "__Internal")]
		NSString LegacyKey { get; }
	}

	[StrongDictionary ("LegacyKeys")]
	interface MixedOptions {
		int Legacy { get; set; }

		[Export ("LegacyKey")]
		int Renamed { get; set; }

		[Export ("LegacyKeys.LegacyKey")]
		int Qualified { get; set; }

		[Field ("NativeInline", "__Internal")]
		int Inline { get; set; }
	}

	[StrongDictionary]
	interface QualifiedOptions {
		[Export ("LegacyKeys.LegacyKey")]
		int Legacy { get; set; }
	}

	[StrongDictionary]
	interface Keys {
		[Field ("NativeKeys", "__Internal")]
		string [] _Keys { get; set; }

		[Field ("NativeValue", "__Internal")]
		int Value { get; set; }

		[Field ("NativeOtherValue", "__Internal")]
		int _Value { get; set; }
	}

	[StrongDictionary]
	interface KeyPropertyOptions {
		[Field ("NativeKeys", "__Internal")]
		string [] Keys { get; set; }
	}
}
