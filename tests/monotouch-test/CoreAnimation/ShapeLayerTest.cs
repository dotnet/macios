//
// Unit tests for CAShapeLayer
//
// Authors:
//	Rolf Bjarne Kvinge <rolf@xamarin.com>
//
// Copyright 2012 Xamarin Inc. All rights reserved.
//

using CoreAnimation;
using CoreGraphics;
#if MONOMAC
using AppKit;
using UIColor = AppKit.NSColor;
#else
using UIKit;
#endif

namespace MonoTouchFixtures.CoreAnimation {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class ShapeLayerTest {
		[Test]
		public void NullableProperties ()
		{
			var sl = new CAShapeLayer ();
			Assert.That (sl.FillColor, Is.Not.Null, "FillColor");
			sl.FillColor = null;
			Assert.That (sl.Path, Is.Null, "Path");
			sl.Path = null;
			Assert.That (sl.LineDashPattern, Is.Null, "LineDashPattern");
			sl.LineDashPattern = null;
			Assert.That (sl.StrokeColor, Is.Null, "StrokeColor");
			sl.StrokeColor = null;

			sl.FillColor = TestRuntime.GetCGColor (UIColor.Black);
			Assert.That (sl.FillColor, Is.Not.Null, "FillColor");
			sl.Path = new CGPath ();
			Assert.That (sl.Path, Is.Not.Null, "Path");
			sl.LineDashPattern = new [] { new NSNumber (5), new NSNumber (10) };
			Assert.That (sl.LineDashPattern, Is.Not.Null, "LineDashPattern");
			sl.StrokeColor = TestRuntime.GetCGColor (UIColor.White);
			Assert.That (sl.StrokeColor, Is.Not.Null, "StrokeColor");
		}
	}
}
