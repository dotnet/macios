#nullable enable

using ObjCRuntime;
using Foundation;
using CoreGraphics;
using AppKit;

using System;
using System.ComponentModel;

namespace QuickLookUI {
	public partial class QLPreviewPanel {
		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public bool EnterFullScreenMode ()
		{
			return EnterFullScreenMode (null, null);
		}

		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public void ExitFullScreenModeWithOptions ()
		{
			ExitFullScreenModeWithOptions (null);
		}
	}
}
