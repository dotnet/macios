#nullable enable

using CoreGraphics;
using AppKit;

using System.ComponentModel;

namespace QuickLookUI {
	public partial class QLPreviewPanel {
		/// <summary>Enters full-screen mode with default options.</summary>
		/// <returns><see langword="true" /> if the panel entered full-screen mode successfully; otherwise, <see langword="false" />.</returns>
		public bool EnterFullScreenMode ()
		{
			return EnterFullScreenMode (null, null);
		}

		/// <summary>Exits full-screen mode with default options.</summary>
		public void ExitFullScreenModeWithOptions ()
		{
			ExitFullScreenModeWithOptions (null);
		}
	}
}
