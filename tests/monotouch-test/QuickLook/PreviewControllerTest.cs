//
// Unit tests for QLPreviewController
//
// Authors:
//	Sebastien Pouliot <sebastien@xamarin.com>
//
// Copyright 2012 Xamarin Inc. All rights reserved.
//

#if !__TVOS__ && !MONOMAC

using System.Drawing;
using CoreGraphics;
using UIKit;
using QuickLook;
using Xamarin.Utils;

namespace MonoTouchFixtures.QuickLook {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class PreviewControllerTest {

		[Test]
		public void Defaults ()
		{
			using (QLPreviewController pc = new QLPreviewController ()) {
				Assert.That (pc.CurrentPreviewItem, Is.Null, "CurrentPreviewItem");
				nint index = 0;
#if !__MACCATALYST__
				if (TestRuntime.CheckSystemVersion (ApplePlatform.iOS, 10, 0))
					index = nint.MaxValue;
				else if (TestRuntime.CheckSystemVersion (ApplePlatform.iOS, 7, 1))
					index = -1;
#endif
				Assert.That (pc.CurrentPreviewItemIndex, Is.EqualTo (index), "CurrentPreviewItemIndex");

				Assert.That (pc.Delegate, Is.Null, "Delegate");
				Assert.That (pc.WeakDelegate, Is.Null, "WeakDelegate");

				Assert.That (pc.DataSource, Is.Null, "DataSource");
				Assert.That (pc.WeakDataSource, Is.Null, "WeakDataSource");

				pc.RefreshCurrentPreviewItem ();
				pc.ReloadData ();
			}
		}

		[Test]
		public void DelegateEvents ()
		{
			using (QLPreviewController pc = new QLPreviewController ()) {
				pc.ShouldOpenUrl += delegate (QLPreviewController controller, NSUrl url, IQLPreviewItem item)
				{
					return false;
				};
				pc.FrameForPreviewItem += delegate
				{
					return new CGRect (1, 2, 3, 4);
				};
				pc.TransitionImageForPreviewItem += delegate
				{
					return new UIImage ();
				};

				Assert.That (pc.Delegate, Is.Not.Null, "Delegate");
				Assert.That (pc.WeakDelegate, Is.Not.Null, "WeakDelegate");
			}
		}
	}
}

#endif // !__TVOS__ && !MONOMAC
