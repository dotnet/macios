// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

#nullable enable

using Foundation;

namespace Propagation {
	[BaseType (typeof (NSObject))]
	interface Base {
		[PropagateToSubclasses]
		[Static]
		[Export ("isSupported")]
		bool IsSupported { get; }

		[PropagateToSubclasses]
		[Static]
		[Export ("value")]
		int Value { get; set; }

		[PropagateToSubclasses]
		[Static]
		[Export ("supportsValue:")]
		bool SupportsValue (int value);

		[PropagateToSubclasses]
		[Static]
		[Export ("supportsString:")]
		bool SupportsValue (string value);

		[PropagateToSubclasses]
		[Static]
		[iOS (26, 0)]
		[Mac (26, 0)]
		[TV (26, 0)]
		[MacCatalyst (26, 0)]
		[Export ("newValue")]
		int NewValue { get; }

		[PropagateToSubclasses]
		[Static]
		[NoTV]
		[Export ("notOnTV")]
		bool NotOnTV { get; }

		[Static]
		[Export ("notPropagated")]
		bool NotPropagated { get; }

		[Static]
		[Export ("notPropagatedMethod")]
		bool NotPropagatedMethod ();
	}

	[BaseType (typeof (Base))]
	interface Derived {
	}

	[BaseType (typeof (Derived))]
	interface Grandchild {
	}

	[BaseType (typeof (Base))]
	interface Redeclared {
		[New]
		[Static]
		[Export ("isSupported")]
		bool IsSupported { get; }

		[New]
		[PropagateToSubclasses]
		[Static]
		[Export ("supportsValue:")]
		bool SupportsValue (int value);
	}

	[BaseType (typeof (Redeclared))]
	interface RedeclaredChild {
	}
}
