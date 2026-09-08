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
		// === Properties ===

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

		// === Methods with nullable generic type arguments ===

		// Method with nullable Action parameter
		[Export ("doSomething:completionHandler:")]
		void DoSomething (NSObject obj, Action<NSObject?> completionHandler);

		// Method with mixed nullable/non-nullable Action parameter
		[Export ("doSomethingElse:completionHandler:")]
		void DoSomethingElse (NSObject obj, Action<NSObject?, NSError> completionHandler);

		// Async method with nullable NSError in completion handler
		[Async]
		[Export ("confirmAcquired:completionHandler:")]
		void ConfirmAcquired (NSObject obj, Action<bool, NSError?> completionHandler);

		// Async method with non-nullable NSError in completion handler
		[Async]
		[Export ("confirmAcquiredNonNull:completionHandler:")]
		void ConfirmAcquiredNonNull (NSObject obj, Action<bool, NSError> completionHandler);

		// Async method with array arg before NSError (tests depth-first byte counting:
		// the array type consumes 2 bytes — one for the array itself and one for the element type)
		[Async]
		[Export ("fetchItems:completionHandler:")]
		void FetchItems (NSObject obj, Action<NSObject []?, NSError?> completionHandler);

		// Async method with nullable result type (single arg, NSError triggers exception pattern)
		[Async]
		[Export ("loadData:completionHandler:")]
		void LoadData (NSObject obj, Action<NSObject?, NSError?> completionHandler);

		// Async method with non-nullable result type
		[Async]
		[Export ("loadDataNonNull:completionHandler:")]
		void LoadDataNonNull (NSObject obj, Action<NSObject, NSError?> completionHandler);

		// Async method with nullable array result type
		[Async]
		[Export ("loadItems:completionHandler:")]
		void LoadItems (NSObject obj, Action<NSObject []?, NSError?> completionHandler);

		// Async method with non-nullable array result type
		[Async]
		[Export ("loadItemsNonNull:completionHandler:")]
		void LoadItemsNonNull (NSObject obj, Action<NSObject [], NSError?> completionHandler);
	}
}
