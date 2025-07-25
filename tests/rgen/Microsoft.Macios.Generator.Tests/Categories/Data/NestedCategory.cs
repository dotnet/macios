// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
#pragma warning disable APL0003

using System;
using System.Runtime.Versioning;
using AVFoundation;
using CoreGraphics;
using Foundation;
using ObjCBindings;
using ObjCRuntime;
using nfloat = System.Runtime.InteropServices.NFloat;

namespace TestNamespace;

public partial class OuterClass {

	public partial class InnerClass {

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst13.1")]
		[BindingType<Category> (typeof (NSObject))]
		public static partial class NestedCategory {

			[SupportedOSPlatform ("ios")]
			[SupportedOSPlatform ("tvos")]
			[SupportedOSPlatform ("macos")]
			[SupportedOSPlatform ("maccatalyst13.1")]
			[Export<Method> ("valueForKey:", Flags = Method.MarshalNativeExceptions)]
			public static unsafe partial NSObject ValueForKey (this NSObject self, NSString key);

			[SupportedOSPlatform ("ios")]
			[SupportedOSPlatform ("tvos")]
			[SupportedOSPlatform ("macos")]
			[SupportedOSPlatform ("maccatalyst13.1")]
			[Export<Method> ("setValue:forKey:")]
			public static unsafe partial void SetValueForKey (this NSObject self, NSObject value, NSString key);

			[SupportedOSPlatform ("ios")]
			[SupportedOSPlatform ("tvos")]
			[SupportedOSPlatform ("macos")]
			[SupportedOSPlatform ("maccatalyst13.1")]
			[Export<Method> ("writeToFile:atomically:")]
			public static unsafe partial bool WriteToFile (this NSObject self, string path, bool useAuxiliaryFile);

			[SupportedOSPlatform ("ios")]
			[SupportedOSPlatform ("tvos")]
			[SupportedOSPlatform ("macos")]
			[SupportedOSPlatform ("maccatalyst13.1")]
			[Export<Method> ("arrayWithContentsOfFile:")]
			public static unsafe partial static NSArray FromFile (this NSObject self, string path);

			[SupportedOSPlatform ("ios")]
			[SupportedOSPlatform ("tvos")]
			[SupportedOSPlatform ("macos")]
			[SupportedOSPlatform ("maccatalyst13.1")]
			[Export<Method> ("sortedArrayUsingComparator:")]
			public static unsafe partial NSArray Sort (this NSObject self, NSComparator cmptr);

			[SupportedOSPlatform ("ios")]
			[SupportedOSPlatform ("tvos")]
			[SupportedOSPlatform ("macos")]
			[SupportedOSPlatform ("maccatalyst13.1")]
			[Export<Method> ("filteredArrayUsingPredicate:")]
			public static unsafe partial NSArray Filter (this NSObject self, NSPredicate predicate);

			[SupportedOSPlatform ("ios")]
			[SupportedOSPlatform ("tvos")]
			[SupportedOSPlatform ("macos")]
			[SupportedOSPlatform ("maccatalyst13.1")]
			[Export<Method> ("filteredArrayUsingStrings:")]
			public static unsafe partial NSArray FilterStrings (this NSObject self, string [] predicate);

			[SupportedOSPlatform ("ios")]
			[SupportedOSPlatform ("tvos")]
			[SupportedOSPlatform ("macos")]
			[SupportedOSPlatform ("maccatalyst13.1")]
			[Export<Method> ("completeRequestReturningItems:completionHandler:", Flags = ObjCBindings.Method.Async)]
			public static unsafe partial void CompleteRequest (this NSObject self, NSExtensionItem [] returningItems,
				Action<bool>? completionHandler);

			[SupportedOSPlatform ("ios")]
			[SupportedOSPlatform ("tvos")]
			[SupportedOSPlatform ("macos")]
			[SupportedOSPlatform ("maccatalyst13.1")]
			[Export<Method> ("completeRequestReturningItems:completionHandler:", Flags = ObjCBindings.Method.Async)]
			public static unsafe partial void CompleteRequest (this NSObject self, NSExtensionItem [] returningItems,
				Action<bool, string?>? completionHandler);

			[SupportedOSPlatform ("ios")]
			[SupportedOSPlatform ("tvos")]
			[SupportedOSPlatform ("macos")]
			[SupportedOSPlatform ("maccatalyst13.1")]
			[Export<Method> ("completeRequestReturningItems:completionHandler:",
				Flags = ObjCBindings.Method.Async,
				ResultType = typeof ((bool Success, string Name, string? Surname)))]
			public static unsafe partial void CompleteRequest (this NSObject self, NSExtensionItem [] returningItems,
				Action<bool, string, string?>? completionHandler);

			[SupportedOSPlatform ("ios")]
			[SupportedOSPlatform ("tvos")]
			[SupportedOSPlatform ("macos")]
			[SupportedOSPlatform ("maccatalyst13.1")]
			[Export<Method> ("bookmarkDataWithContentsOfURL:error:")]
			public static unsafe partial NSData GetBookmarkData (this NSObject self, NSUrl bookmarkFileUrl,
				out NSError? error);

			// the following methods are used to test that the conversions are in the correc order.

			[SupportedOSPlatform ("ios")]
			[SupportedOSPlatform ("tvos")]
			[SupportedOSPlatform ("macos")]
			[SupportedOSPlatform ("maccatalyst13.1")]
			[Export<Method> ("bookmarkDataWithContentsOfURL:subdomain:error:")]
			public static unsafe partial NSData GetBookmarkData (this NSObject self, NSUrl bookmarkFileUrl,
				string subdomain, out NSError? error);
		}
	}
}

