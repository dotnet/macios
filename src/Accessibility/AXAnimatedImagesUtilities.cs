#nullable enable

using CoreGraphics;

namespace Accessibility {

	public static partial class AXAnimatedImagesUtilities {

		[DllImport (Constants.AccessibilityLibrary)]
		extern static byte AXAnimatedImagesEnabled ();

		public static bool Enabled => AXAnimatedImagesEnabled () != 0;
	}
}
