using System;

using Foundation;
using ObjCRuntime;
using QuickLook;
using UIKit;

namespace PreviewExtensionTest {
	[Register ("PreviewViewController")]
	public class PreviewViewController : UIViewController, IQLPreviewingController {
		public PreviewViewController (NativeHandle handle) : base (handle)
		{
		}

		public void PreparePreviewOfFile (NSUrl url, Action<NSError> completionHandler)
		{
			completionHandler (null);
		}
	}
}
