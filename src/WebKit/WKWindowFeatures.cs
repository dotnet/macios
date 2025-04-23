//
// WebKit/WKWindowFeatures.cs
//
// Authors:
//   Aaron Bockover (abock@xamarin.com)
//
// Copyright 2014 Xamarin Inc. All rights reserved.
//

using System;

using Foundation;
using ObjCRuntime;

#nullable enable

namespace WebKit {
	public partial class WKWindowFeatures {
		/// <summary>Whether the menu bar should be visible. null if menu bar visibility was not specified.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public bool? MenuBarVisibility {
			get => menuBarVisibility?.BoolValue;
		}

		/// <summary>Whether the status bar should be visible. null if status bar visibility was not specified.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public bool? StatusBarVisibility {
			get => statusBarVisibility?.BoolValue;
		}

		/// <summary>Whether toolbars should be visible. null if toolbar visibility was not specified.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public bool? ToolbarsVisibility {
			get => toolbarsVisibility?.BoolValue;
		}

		/// <summary>Whether the containing window should be resizable. null if resizability was not specified.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
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

		/// <summary>The x coordinate of the containing window. null if the x coordinate was not specified.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public nfloat? X {
			get { return NFloatValue (x); }
		}

		/// <summary>The y coordinate of the containing window. null if the y coordinate was not specified.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public nfloat? Y {
			get { return NFloatValue (y); }
		}

		/// <summary>The width coordinate of the containing window. null if the width was not specified.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public nfloat? Width {
			get { return NFloatValue (width); }
		}

		/// <summary>The height coordinate of the containing window. nil if the height was not specified</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public nfloat? Height {
			get { return NFloatValue (height); }
		}
	}
}
