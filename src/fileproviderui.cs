//
// FileProvider C# bindings
//
// Authors:
//	Alex Soto  <alexsoto@microsoft.com>
//
// Copyright 2017 Xamarin Inc. All rights reserved.
//

using System;
using ObjCRuntime;
using Foundation;
#if IOS
using UIKit;
#else
using AppKit;
#endif
using FileProvider;

#if !NET
using NativeHandle = System.IntPtr;
#endif

namespace FileProviderUI {

	/// <summary>Enumerates file provider UI extension errors.</summary>
	[ErrorDomain ("FPUIErrorDomain")]
	[Native]
	enum FPUIExtensionErrorCode : ulong {
		/// <summary>Indicates that the user cancelled an action.</summary>
		UserCancelled,
		/// <summary>Indicates that a requested action failed.</summary>
		Failed,
	}

	/// <summary>File Provider UI extension context.</summary>
	[DisableDefaultCtor]
	[BaseType (typeof (NSExtensionContext))]
	interface FPUIActionExtensionContext {

		/// <summary>Gets the unique ID of the file provider domain.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("domainIdentifier")]
		string DomainIdentifier { get; }

		/// <summary>Marks the requested action complete.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("completeRequest")]
		void CompleteRequest ();

		/// <param name="error">On failure, contains the error that occurred.</param>
		///         <summary>Cancels the request with the specified error.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("cancelRequestWithError:")]
		void CancelRequest (NSError error);
	}

#if IOS
	/// <summary>A user action UI.</summary>
	[BaseType (typeof (UIViewController))]
#else
	[BaseType (typeof (NSViewController))]
#endif
	interface FPUIActionExtensionViewController {

		[Export ("initWithNibName:bundle:")]
		[PostGet ("NibBundle")]
		NativeHandle Constructor ([NullAllowed] string nibName, [NullAllowed] NSBundle bundle);

		/// <summary>Gets the host app's extension context.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("extensionContext", ArgumentSemantic.Strong)]
		FPUIActionExtensionContext ExtensionContext { get; }

		/// <param name="error">The error for which to prepare.</param>
		///         <summary>When implemented by the developer, presents UI to the user in response to the specified error.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("prepareForError:")]
		void Prepare (NSError error);

		/// <param name="actionIdentifier">The action identifier for the user action.</param>
		///         <param name="itemIdentifiers">The item identifiers for the affected items.</param>
		///         <summary>When implemented by the developer, presents UI to the user in response to the specified action and items.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("prepareForActionWithIdentifier:itemIdentifiers:")]
		void Prepare (string actionIdentifier, NSString [] itemIdentifiers);
	}
}
