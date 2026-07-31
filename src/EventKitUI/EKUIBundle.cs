//
// EventKitUIBundle C# bindings
//
// Authors:
//	Alex Soto  <alexsoto@microsoft.com>
//
// Copyright 2017 Xamarin Inc. All rights reserved.
//

#nullable enable

namespace EventKitUI {
	/// <summary>To be added.</summary>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	public static class EKUIBundle {

		[DllImport (Constants.EventKitUILibrary)]
		static extern IntPtr EventKitUIBundle ();

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		public static NSBundle? UIBundle {
			get {
				return Runtime.GetNSObject<NSBundle> (EventKitUIBundle ());
			}
		}
	}
}
