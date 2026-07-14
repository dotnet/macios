using System;

using Foundation;
using QuickLook;
using UIKit;

namespace PreviewExtensionTest {
	[Register ("PreviewViewController")]
	public class PreviewViewController : UIViewController, IQLPreviewingController {
		public PreviewViewController (IntPtr handle) : base (handle)
		{
		}

		public void PreparePreviewOfFile (NSUrl url, Action<NSError> completionHandler)
		{
			completionHandler (null);
		}
	}
}
