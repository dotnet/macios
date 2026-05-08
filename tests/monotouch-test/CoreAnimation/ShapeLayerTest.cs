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
			ClassicAssert.NotNull (sl.FillColor, "FillColor");
			sl.FillColor = null;
			ClassicAssert.Null (sl.Path, "Path");
			sl.Path = null;
			ClassicAssert.Null (sl.LineDashPattern, "LineDashPattern");
			sl.LineDashPattern = null;
			ClassicAssert.Null (sl.StrokeColor, "StrokeColor");
			sl.StrokeColor = null;

			sl.FillColor = TestRuntime.GetCGColor (UIColor.Black);
			ClassicAssert.NotNull (sl.FillColor, "FillColor");
			sl.Path = new CGPath ();
			ClassicAssert.NotNull (sl.Path, "Path");
			sl.LineDashPattern = new [] { new NSNumber (5), new NSNumber (10) };
			ClassicAssert.NotNull (sl.LineDashPattern, "LineDashPattern");
			sl.StrokeColor = TestRuntime.GetCGColor (UIColor.White);
			ClassicAssert.NotNull (sl.StrokeColor, "StrokeColor");
		}
	}
}
