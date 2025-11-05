// Copyright 2011 Xamarin Inc. All rights reserved

#if !MONOMAC

using System.Drawing;
using CoreGraphics;
using UIKit;

namespace MonoTouchFixtures.UIKit {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class PageControlTest {

		[Test]
		public void InitWithFrame ()
		{
			var frame = new CGRect (10, 10, 100, 100);
			using (UIPageControl pc = new UIPageControl (frame)) {
				Assert.That (pc.Frame, Is.EqualTo (frame), "Frame");
			}
		}
	}
}

#endif // !MONOMAC
