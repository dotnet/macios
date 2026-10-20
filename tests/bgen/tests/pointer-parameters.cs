// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using CoreGraphics;
using Foundation;
using ObjCRuntime;

namespace PointerParameters {
	delegate void StructCallback ([PointerAsStruct] ref CGPoint value);

	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface PointerBindings {
		[Export ("pointer:")]
		unsafe void Pointer ([PointerAsStruct] CGPoint* value);

		[Export ("nullablePointer:")]
		unsafe void NullablePointer ([NullAllowed, PointerAsStruct] CGPoint* value);

		[Export ("unannotatedPointer:")]
		unsafe void UnannotatedPointer ([NullAllowed] CGPoint* value);

		[Export ("unannotatedReference:")]
		void UnannotatedReference (ref CGPoint value);

		[Export ("unannotatedOutput:")]
		void UnannotatedOutput (out CGPoint value);

		[Export ("reference:")]
		void Reference ([PointerAsStruct] ref CGPoint value);

		[Export ("output:")]
		void Output ([PointerAsStruct] out CGPoint value);

		[Export ("observe:")]
		void Observe (StructCallback callback);

		[Export ("objects:")]
		void Objects (NSObject [] values);

		[Export ("points:count:")]
		void Points ([PointerAsArray (LengthFromParameter = nameof (count))] CGPoint [] values, nint count);

		[Export ("ranges:count:")]
		[Static]
		void Ranges ([PointerAsArray (LengthFromParameter = nameof (count))] NSRange [] values, nuint count);

		[Export ("prefix:count:")]
		void Prefix ([PointerAsArray (LengthFromParameter = nameof (count))] CGPoint [] values, long count);

		[Export ("unsigned:count:")]
		void Unsigned ([PointerAsArray (LengthFromParameter = nameof (count))] CGPoint [] values, ulong count);

		[Export ("three:")]
		void Three ([PointerAsArray (Length = 3)] CGPoint [] values);

		[Export ("result:tag:point:")]
		bool Result ([PointerAsArray (Length = 1)] CGPoint [] values, string tag, out CGPoint point);

		[Export ("point:")]
		CGPoint Point ([PointerAsArray (Length = 1)] CGPoint [] values);

		[Export ("empty:")]
		void Empty ([PointerAsArray (Length = 0)] CGPoint [] values);

		[Export ("nullable:count:")]
		void Nullable ([NullAllowed, PointerAsArray (LengthFromParameter = nameof (count))] CGPoint [] values, int count);

		[Export ("count")]
		nuint Count { get; }

		[Export ("member:")]
		void Member ([PointerAsArray (LengthFromMember = nameof (Count))] CGPoint [] values);

		[Export ("staticCount")]
		[Static]
		int StaticCount { get; }

		[Export ("staticMember:")]
		[Static]
		void StaticMember ([PointerAsArray (LengthFromMember = nameof (StaticCount))] CGPoint [] values);

		const uint FixedCount = 2;

		[Export ("constantMember:")]
		void ConstantMember ([PointerAsArray (LengthFromMember = nameof (FixedCount))] CGPoint [] values);

		[Export ("twoArrays:ranges:count:")]
		void TwoArrays ([PointerAsArray (LengthFromParameter = nameof (count))] CGPoint [] points,
			[PointerAsArray (LengthFromParameter = nameof (count))] NSRange [] ranges, uint count);

		[Export ("keyword:count:")]
		void Keyword ([PointerAsArray (LengthFromParameter = nameof (@event))] CGPoint [] @class, int @event);

		[Export ("initWithPoints:count:")]
		NativeHandle Constructor ([PointerAsArray (LengthFromParameter = nameof (count))] CGPoint [] values, int count);
	}

	[BaseType (typeof (PointerBindings))]
	[DisableDefaultCtor]
	interface DerivedPointerBindings {
		[Export ("inherited:")]
		void Inherited ([PointerAsArray (LengthFromMember = nameof (PointerBindings.Count))] CGPoint [] values);

		[Static]
		[Export ("inheritedStatic:")]
		void InheritedStatic ([PointerAsArray (LengthFromMember = nameof (PointerBindings.StaticCount))] CGPoint [] values);
	}
}
