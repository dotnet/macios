// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using CoreVideo;
using Foundation;
using ObjCRuntime;

namespace ProtocolWrap {
	interface IWrappedDelegate { }

	[Protocol]
	interface WrappedDelegate {
	}

	[Protocol (BackwardsCompatibleCodeGeneration =
#if COMPATIBLE
		true
#else
		false
#endif
	)]
	[Model]
	[BaseType (typeof (NSObject))]
	interface WrappedProtocol {
		[Abstract]
		[Export ("value")]
		nint Value { get; set; }

		[Abstract]
		[NullAllowed, Export ("delegate", ArgumentSemantic.Weak)]
		NSObject WeakDelegate { get; set; }

		[NullAllowed]
		[Wrap ("WeakDelegate"), Abstract]
		IWrappedDelegate Delegate { get; set; }

		[Wrap ("Value")]
		nint Alias { get; set; }

		int ConvertedValue {
			[Wrap ("(int) Value")]
			get;
			[Wrap ("Value = value")]
			set;
		}

		int ReadOnlyValue {
			[Wrap ("(int) Value")]
			get;
		}

		int WriteOnlyValue {
			[Wrap ("Value = value")]
			set;
		}

		nint? NullableValue {
			[Wrap ("Value == 0 ? null : Value")]
			get;
			[Wrap ("Value = value ?? 0")]
			set;
		}

		[Abstract]
		[Export ("attributes")]
		NSDictionary WeakAttributes { get; }

		[Wrap ("WeakAttributes")]
		CVPixelBufferAttributes Attributes { get; }

		[Abstract]
		[Export ("compute:")]
		nint Compute (nint value);

		[Wrap ("Compute (value)")]
		nint ComputeAlias (nint value);

		[Abstract]
		[Wrap ("Value = value")]
		void SetValue (nint value);

		[Wrap ("Compute (Value)", IsVirtual = true)]
		nint ComputeValue ();

		[Static]
		[Wrap ("(nint) 42")]
		nint Constant { get; }

		[Static]
		[Wrap ("(nint) 42")]
		nint GetConstant ();
	}

	[Protocol (BackwardsCompatibleCodeGeneration = false)]
	interface DerivedProtocol : WrappedProtocol {
	}

	[BaseType (typeof (NSObject))]
	interface Adopter : DerivedProtocol {
	}
}
