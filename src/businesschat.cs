//
// BusinessChat bindings
//
// Authors:
//     Manuel de la Peña <mandel@microsoft.com>
//
// Copyright 2017 Microsoft Inc. All rights reserved.
//
using System;
using System.ComponentModel;

using Foundation;
using ObjCRuntime;
#if MONOMAC
using AppKit;
using UIControl = AppKit.NSControl;
#else
using UIKit;
#endif

#if !NET
using NativeHandle = System.IntPtr;
#endif

namespace BusinessChat {

	[Deprecated (PlatformName.MacOSX, 13, 1)]
	[Deprecated (PlatformName.iOS, 16, 2)]
	[MacCatalyst (13, 1)]
	[Deprecated (PlatformName.MacCatalyst, 16, 2)]
	[BaseType (typeof (UIControl))]
	[DisableDefaultCtor]
	interface BCChatButton {
		/// <param name="style">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("initWithStyle:")]
		[DesignatedInitializer]
		NativeHandle Constructor (BCChatButtonStyle style);
	}


	[Deprecated (PlatformName.MacOSX, 13, 1)]
	[Deprecated (PlatformName.iOS, 16, 2)]
	[MacCatalyst (13, 1)]
	[Deprecated (PlatformName.MacCatalyst, 16, 2)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface BCChatAction {

		/// <param name="businessIdentifier">To be added.</param>
		///         <param name="intentParameters">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Static]
		[EditorBrowsable (EditorBrowsableState.Advanced)]
		[Export ("openTranscript:intentParameters:")]
		void OpenTranscript (string businessIdentifier, NSDictionary<NSString, NSString> intentParameters);
	}
}
