// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using Foundation;
using ObjCRuntime;

namespace NS {
	delegate T GenericBlock<T> () where T : NSObject;
	delegate T GenericTransform<T> (T value) where T : NSObject;
	delegate T1 GenericPairBlock<T1, T2> (T1 first, T2 second) where T1 : NSObject where T2 : NSObject;
	delegate T1 GenericNestedBlock<T1, T2> (T1 first, T2 second, [BlockCallback] GenericTransform<T1> callback) where T1 : NSObject where T2 : NSObject;

	[BaseType (typeof (NSObject))]
	interface OneGenericType<T> where T : NSObject {
	}

	[BaseType (typeof (NSObject))]
	interface TwoGenericTypes<T1, T2> where T1 : NSObject where T2 : NSObject {
	}

	[BaseType (typeof (NSObject))]
	interface GenericBlocks<T> where T : NSObject {
		[NullAllowed, Export ("predicate", ArgumentSemantic.Copy)]
		Func<T, bool> Predicate { get; set; }

		[NullAllowed, Export ("action", ArgumentSemantic.Copy)]
		Action<T> Action { get; set; }

		[NullAllowed, Export ("transform", ArgumentSemantic.Copy)]
		Func<T, OneGenericType<T>, OneGenericType<T>> Transform { get; set; }

		[NullAllowed, Export ("factory", ArgumentSemantic.Copy)]
		GenericBlock<T> Factory { get; set; }

		[NullAllowed, Export ("customTransform", ArgumentSemantic.Copy)]
		GenericTransform<T> CustomTransform { get; set; }

		[Export ("setCallback:")]
		void SetCallback (Action<T> callback);

		[return: NullAllowed]
		[Export ("getCallback")]
		GenericTransform<T> GetCallback ();
	}

	[BaseType (typeof (NSObject))]
	interface GenericPairBlocks<T1, T2> where T1 : NSObject where T2 : NSObject {
		[NullAllowed, Export ("transform", ArgumentSemantic.Copy)]
		Func<T1, bool, T2> Transform { get; set; }

		[NullAllowed, Export ("action", ArgumentSemantic.Copy)]
		Action<T1, T2> Action { get; set; }

		[NullAllowed, Export ("genericTransform", ArgumentSemantic.Copy)]
		Func<T1, T2, TwoGenericTypes<T1, T2>, TwoGenericTypes<T1, T2>> GenericTransform { get; set; }

		[NullAllowed, Export ("customTransform", ArgumentSemantic.Copy)]
		GenericPairBlock<T1, T2> CustomTransform { get; set; }

		[NullAllowed, Export ("nested", ArgumentSemantic.Copy)]
		GenericNestedBlock<T1, T2> Nested { get; set; }
	}

	[BaseType (typeof (NSObject))]
	interface ClosedBlocks {
		[NullAllowed, Export ("transform", ArgumentSemantic.Copy)]
		GenericTransform<NSString> Transform { get; set; }

		[NullAllowed, Export ("nested", ArgumentSemantic.Copy)]
		GenericNestedBlock<NSString, NSNumber> Nested { get; set; }
	}
}
