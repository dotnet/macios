// Copyright 2012 Xamarin Inc. All rights reserved.

#nullable enable

namespace UIKit {

	public partial class UIPopoverController {

		// cute helper to avoid using `Class` in the public API
		/// <summary>Gets or sets the type used to display the background of the popover.</summary>
		/// <remarks>The popover controller uses an instance of this type to draw the background of the popover.</remarks>
		public virtual Type? PopoverBackgroundViewType {
			get {
				IntPtr p = PopoverBackgroundViewClass;
				if (p == IntPtr.Zero)
					return null;
				return Class.Lookup (p);
			}
			set {
				PopoverBackgroundViewClass = Class.GetHandle (value);
			}
		}
	}
}
