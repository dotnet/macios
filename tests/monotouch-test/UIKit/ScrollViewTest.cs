// Copyright 2011 Xamarin Inc. All rights reserved

#if !MONOMAC

using System.Drawing;
using CoreGraphics;
using UIKit;

namespace MonoTouchFixtures.UIKit {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class ScrollViewTest {

		[Test]
		public void InitWithFrame ()
		{
			var frame = new CGRect (10, 10, 100, 100);
			using (UIScrollView sv = new UIScrollView (frame)) {
				Assert.That (sv.Frame, Is.EqualTo (frame), "Frame");
			}
		}
	}
}

#endif // !MONOMAC
