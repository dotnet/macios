// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

#if __MACOS__

using AppKit;
using CoreGraphics;

namespace Xamarin.Mac.Tests {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class NSBezierPathTests {
		[Test]
		public void Append ()
		{
			using var path = new NSBezierPath ();
			var points = new [] { new CGPoint (10, 20), new CGPoint (30, 40), new CGPoint (50, 20) };
			path.Append (points);
			Assert.That (path.ElementCount, Is.EqualTo ((nint) points.Length), "ElementCount");
			for (var i = 0; i < points.Length; i++) {
				var element = path.ElementAt (i, out var actual);
				Assert.That (element, Is.EqualTo (i == 0 ? NSBezierPathElement.MoveTo : NSBezierPathElement.LineTo), $"Element {i}");
				Assert.That (actual, Is.EqualTo (new [] { points [i] }), $"Points {i}");
			}
		}

		[Test]
		public void AppendInvalid ()
		{
			using var path = new NSBezierPath ();
			CGPoint [] empty = [];
			Assert.Throws<ArgumentNullException> (() => path.Append ((CGPoint []) null));
			Assert.Throws<ArgumentException> (() => path.Append (empty));
		}

		[Test]
		public void CurvePoints ()
		{
			using var path = new NSBezierPath ();
			var end = new CGPoint (50, 60);
			var control1 = new CGPoint (10, 20);
			var control2 = new CGPoint (30, 40);
			path.MoveTo (CGPoint.Empty);
			path.CurveTo (end, control1, control2);
			Assert.That (path.ElementAt (1, out var points), Is.EqualTo (NSBezierPathElement.CurveTo), "Element");
			Assert.That (points, Is.EqualTo (new [] { control1, control2, end }), "Points");
		}
	}
}

#endif // __MACOS__
