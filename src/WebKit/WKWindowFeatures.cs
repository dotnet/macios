//
// WebKit/WKWindowFeatures.cs
//
// Authors:
//   Aaron Bockover (abock@xamarin.com)
//
// Copyright 2014 Xamarin Inc. All rights reserved.
//

#nullable enable

namespace WebKit {
	public partial class WKWindowFeatures {
		/// <summary>Gets whether the menu bar should be visible.</summary>
		/// <value><see langword="true" /> if the menu bar should be visible; <see langword="false" /> if not; <see langword="null" /> if not specified.</value>
		public bool? MenuBarVisibility {
			get => menuBarVisibility?.BoolValue;
		}

		/// <summary>Gets whether the status bar should be visible.</summary>
		/// <value><see langword="true" /> if the status bar should be visible; <see langword="false" /> if not; <see langword="null" /> if not specified.</value>
		public bool? StatusBarVisibility {
			get => statusBarVisibility?.BoolValue;
		}

		/// <summary>Gets whether toolbars should be visible.</summary>
		/// <value><see langword="true" /> if toolbars should be visible; <see langword="false" /> if not; <see langword="null" /> if not specified.</value>
		public bool? ToolbarsVisibility {
			get => toolbarsVisibility?.BoolValue;
		}

		/// <summary>Gets whether the containing window should be resizable.</summary>
		/// <value><see langword="true" /> if the window is resizable; <see langword="false" /> if not; <see langword="null" /> if not specified.</value>
		public bool? AllowsResizing {
			get => allowsResizing?.BoolValue;
		}

		static nfloat? NFloatValue (NSNumber? number)
		{
			if (number is null)
				return null;
			else
				return (nfloat) number.DoubleValue;
		}

		/// <summary>Gets the x coordinate of the containing window.</summary>
		/// <value>The x coordinate, or <see langword="null" /> if not specified.</value>
		public nfloat? X {
			get { return NFloatValue (x); }
		}

		/// <summary>Gets the y coordinate of the containing window.</summary>
		/// <value>The y coordinate, or <see langword="null" /> if not specified.</value>
		public nfloat? Y {
			get { return NFloatValue (y); }
		}

		/// <summary>Gets the width of the containing window.</summary>
		/// <value>The width, or <see langword="null" /> if not specified.</value>
		public nfloat? Width {
			get { return NFloatValue (width); }
		}

		/// <summary>Gets the height of the containing window.</summary>
		/// <value>The height, or <see langword="null" /> if not specified.</value>
		public nfloat? Height {
			get { return NFloatValue (height); }
		}
	}
}
