// Copyright 2011 Xamarin Inc. All rights reserved

#if !__TVOS__ && !MONOMAC

using System.Drawing;
using CoreGraphics;
using MapKit;

namespace MonoTouchFixtures.MapKit {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class OverlayPathViewTest {

		[Test]
		public void InitWithFrame ()
		{
			var frame = new CGRect (10, 10, 100, 100);
			using (MKOverlayPathView opv = new MKOverlayPathView (frame)) {
				Assert.That (opv.Frame, Is.EqualTo (frame), "Frame");
			}
		}
	}
}

#endif // !__TVOS__ && !MONOMAC
