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

[BindingType<Class>]
public partial class MethodTests {

	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("tvos")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("maccatalyst13.1")]
	[Export<Method> ("valueForKey:", Flags: Method.MarshalNativeExceptions)]
	public partial NSObject ValueForKey (NSString key);

	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("tvos")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("maccatalyst13.1")]
	[Export<Method> ("setValue:forKey:")]
	public partial void SetValueForKey (NSObject value, NSString key);

	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("tvos")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("maccatalyst13.1")]
	[Export<Method> ("writeToFile:atomically:")]
	public partial bool WriteToFile (string path, bool useAuxiliaryFile);

	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("tvos")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("maccatalyst13.1")]
	[Export<Method> ("arrayWithContentsOfFile:")]
	public partial static NSArray FromFile (string path);

	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("tvos")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("maccatalyst13.1")]
	[Export<Method> ("sortedArrayUsingComparator:")]
	public partial NSArray Sort (NSComparator cmptr);

	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("tvos")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("maccatalyst13.1")]
	[Export<Method> ("filteredArrayUsingPredicate:")]
	public partial NSArray Filter (NSPredicate predicate);

#if !__TVOS__

	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("maccatalyst13.1")]
	[UnsupportedOSPlatform ("tvos")]
	[Export<Method> ("loadFromHTMLWithRequest:options:completionHandler:",
		Flags = ObjCBindings.Method.Async,
		ResultTypeName = "NSLoadFromHtmlResult")]
	public partial static void LoadFromHtml (NSUrlRequest request, NSDictionary options, NSAttributedStringCompletionHandler completionHandler);

	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("maccatalyst13.1")]
	[UnsupportedOSPlatform ("tvos")]
	[Export<Method> ("loadFromHTMLWithRequest:options:completionHandler:",
		Flags = ObjCBindings.Method.Async)]
	public partial static void LoadFromHtmlNoName (NSUrlRequest request, NSDictionary options, NSAttributedStringCompletionHandler completionHandler);

#endif

	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("tvos")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("maccatalyst13.1")]
	[Export<Method> ("completeRequestReturningItems:completionHandler:", Flags = ObjCBindings.Method.Async)]
	public partial void CompleteRequest (NSExtensionItem [] returningItems, Action<bool>? completionHandler);

	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("tvos")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("maccatalyst13.1")]
	[Export<Method> ("completeRequestReturningItems:completionHandler:", Flags = ObjCBindings.Method.Async)]
	public partial void CompleteRequest (NSExtensionItem [] returningItems, Action<bool, string?>? completionHandler);

	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("tvos")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("maccatalyst13.1")]
	[Export<Method> ("completeRequestReturningItems:completionHandler:",
		Flags = ObjCBindings.Method.Async,
		ResultType = typeof ((bool Success, string Name, string? Surname)))]
	public partial void CompleteRequest (NSExtensionItem [] returningItems, Action<bool, string, string?>? completionHandler);
}
