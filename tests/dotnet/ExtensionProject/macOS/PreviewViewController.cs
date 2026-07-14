using System;

using AppKit;
using Foundation;
using QuickLookUI;

namespace PreviewExtensionTest {
	[Register ("PreviewViewController")]
	public class PreviewViewController : NSViewController, IQLPreviewingController {
		public PreviewViewController (IntPtr handle) : base (handle)
		{
		}

		public override void LoadView ()
		{
			View = new NSView (new CoreGraphics.CGRect (0, 0, 400, 300));
		}

		public void PreparePreviewOfFile (NSUrl url, Action<NSError> completionHandler)
		{
			completionHandler (null);
		}
	}
}
