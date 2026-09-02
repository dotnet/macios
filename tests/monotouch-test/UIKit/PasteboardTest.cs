// Copyright 2012 Xamarin Inc. All rights reserved

#if !__TVOS__ && !MONOMAC

using System.Drawing;
using System.IO;

using UIKit;
using CoreGraphics;

namespace MonoTouchFixtures.UIKit {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class PasteboardTest {

		[Test]
		[Retry (10)]
		[Ignore ("Fails randomly on the 'b - length' assert, even after retrying 10 times (the clipboard sometimes has just one image for some reason)")]
		public void ImagesTest ()
		{
			string file = Path.Combine (NSBundle.MainBundle.ResourcePath, "basn3p08.png");
			using (var dp = new CGDataProvider (file)) {
				using (var cgimg = CGImage.FromPNG (dp, null, false, CGColorRenderingIntent.Default)) {
					using (var img = new UIImage (cgimg)) {
						UIPasteboard.General.Images = new UIImage [] { img };
						if (TestRuntime.CheckXcodeVersion (8, 0))
							Assert.That (UIPasteboard.General.HasImages, Is.True, "HasImages");

						Assert.That (UIPasteboard.General.Images.Length, Is.EqualTo (1), "a - length");

						UIPasteboard.General.Images = new UIImage [] { img, img };
						Assert.That (UIPasteboard.General.Images.Length, Is.EqualTo (2), "b - length");
						Assert.That (UIPasteboard.General.Images [0], Is.Not.Null, "b - nonnull[0]");
						Assert.That (UIPasteboard.General.Images [1], Is.Not.Null, "b - nonnull[0]");
					}
				}
			}
		}
	}
}

#endif // !__TVOS__ && !MONOMAC
