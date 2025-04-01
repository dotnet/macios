// Copyright 2012 Xamarin Inc. All rights reserved.

using System;
using ObjCRuntime;

// Disable until we get around to enable + fix any issues.
#nullable disable

namespace UIKit {

	public partial class UIPopoverController {

		// cute helper to avoid using `Class` in the public API
		/// <summary>This is the type that is used to display the background of the popover.</summary>
		///         <value>
		///         </value>
		///         <remarks>The popover controller will use an instance of this type in order to draw the background of the popover.</remarks>
		public virtual Type PopoverBackgroundViewType {
			get {
				IntPtr p = PopoverBackgroundViewClass;
				if (p == IntPtr.Zero)
					return null;
				return Class.Lookup (p);
			}
			set {
				PopoverBackgroundViewClass = (value is null) ? IntPtr.Zero :
					Class.GetHandle (value);
			}
		}
	}
}
