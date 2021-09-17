//
// EventKitUIBundle C# bindings
//
// Authors:
//	Alex Soto  <alexsoto@microsoft.com>
//
// Copyright 2017 Xamarin Inc. All rights reserved.
//

using System;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using Foundation;
using ObjCRuntime;

namespace EventKitUI {
#if !NET
	[iOS (11,0)]
#else
	[SupportedOSPlatform ("ios11.0")]
#endif
	public static class EKUIBundle {

		[DllImport (Constants.EventKitUILibrary)]
		static extern IntPtr EventKitUIBundle ();

		public static NSBundle UIBundle {
			get {
				return Runtime.GetNSObject<NSBundle> (EventKitUIBundle ());
			}
		}
	}
}
