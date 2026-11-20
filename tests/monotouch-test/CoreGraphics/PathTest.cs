//
// Unit tests for CGPath
//
// Authors:
//	Sebastien Pouliot <sebastien@xamarin.com>
//
// Copyright 2013-2014 Xamarin Inc. All rights reserved.
//

using CoreGraphics;

namespace MonoTouchFixtures.CoreGraphics {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class PathTest {

		static void AssertPoint (CGPoint actual, CGPoint expected, string message)
		{
			Assert.That ((double) actual.X, Is.EqualTo ((double) expected.X).Within (0.00001), message + " X");
			Assert.That ((double) actual.Y, Is.EqualTo ((double) expected.Y).Within (0.00001), message + " Y");
		}

		static void AssertBounds (CGPath path, CGRect expected)
		{
			AssertPoint (path.BoundingBox.Location, expected.Location, "Bounds origin");
			Assert.That ((double) path.BoundingBox.Width, Is.EqualTo ((double) expected.Width).Within (0.00001), "Bounds width");
			Assert.That ((double) path.BoundingBox.Height, Is.EqualTo ((double) expected.Height).Within (0.00001), "Bounds height");
		}

		static void AssertTranslatedPath (CGPath actual, CGPath original)
		{
			var elements = new System.Collections.Generic.List<CGPathElement> ();
			original.Apply (elements.Add);
			var index = 0;
			actual.Apply (element => {
				Assert.That (index, Is.LessThan (elements.Count), "Element index");
				var expected = elements [index++];
				Assert.That (element.Type, Is.EqualTo (expected.Type), "Element type");
				if (element.Type == CGPathElementType.CloseSubpath)
					return;
				AssertPoint (element.Point1, new CGPoint (expected.Point1.X + 11, expected.Point1.Y + 17), "Point1");
				if (element.Type == CGPathElementType.AddQuadCurveToPoint || element.Type == CGPathElementType.AddCurveToPoint)
					AssertPoint (element.Point2, new CGPoint (expected.Point2.X + 11, expected.Point2.Y + 17), "Point2");
				if (element.Type == CGPathElementType.AddCurveToPoint)
					AssertPoint (element.Point3, new CGPoint (expected.Point3.X + 11, expected.Point3.Y + 17), "Point3");
			});
			Assert.That (index, Is.EqualTo (elements.Count), "Element count");
		}

		[TestCase (0)]
		[TestCase (1)]
		[TestCase (2)]
		[TestCase (3)]
		[TestCase (4)]
		[TestCase (5)]
		[TestCase (6)]
		[TestCase (7)]
		[TestCase (8)]
		[TestCase (9)]
		[TestCase (10)]
		[TestCase (11)]
		[TestCase (12)]
		[TestCase (13)]
		[TestCase (14)]
		[TestCase (15)]
		[TestCase (16)]
		[TestCase (17)]
		public void TransformDrawingInputs (int operation)
		{
			var transform = CGAffineTransform.MakeTranslation (11, 17);
			var saved = transform;
			var rect = new CGRect (2, 3, 6, 6);
			var rects = new [] { rect, new CGRect (20, 30, 4, 5) };
			var points = new [] { new CGPoint (2, 3), new CGPoint (8, 9), new CGPoint (20, 30) };
			using var source = CGPath.FromRect (rect);
			using var original = new CGPath ();
			using var actual = new CGPath ();
			if ((operation >= 2 && operation <= 6) || operation == 14) {
				original.MoveToPoint (0, 0);
				actual.MoveToPoint (11, 17);
			}
			switch (operation) {
			case 0: original.MoveToPoint (2, 3); break;
			case 1: original.MoveToPoint (points [0]); break;
			case 2: original.AddLineToPoint (8, 9); break;
			case 3: original.AddLineToPoint (points [1]); break;
			case 4: original.AddQuadCurveToPoint (4, 5, 8, 9); break;
			case 5: original.AddCurveToPoint (2, 3, 4, 5, 8, 9); break;
			case 6: original.AddCurveToPoint (points [0], new CGPoint (4, 5), points [1]); break;
			case 7: original.AddRect (rect); break;
			case 8: original.AddRects (rects); break;
			case 9: original.AddRects (rects, 1); break;
			case 10: original.AddLines (points); break;
			case 11: original.AddLines (points, 2); break;
			case 12: original.AddEllipseInRect (rect); break;
			case 13: original.AddArc (2, 3, 6, 0, (nfloat) (Math.PI / 2), false); break;
			case 14: original.AddArcToPoint (8, 0, 8, 8, 2); break;
			case 15: original.AddRelativeArc (2, 3, 6, 0, (nfloat) (Math.PI / 2)); break;
			case 16: original.AddPath (source); break;
			case 17: original.AddRoundedRect (rect, 1, 2); break;
			}
#if XAMCORE_5_0
			switch (operation) {
			case 0: actual.MoveToPoint (in transform, 2, 3); break;
			case 1: actual.MoveToPoint (in transform, points [0]); break;
			case 2: actual.AddLineToPoint (in transform, 8, 9); break;
			case 3: actual.AddLineToPoint (in transform, points [1]); break;
			case 4: actual.AddQuadCurveToPoint (in transform, 4, 5, 8, 9); break;
			case 5: actual.AddCurveToPoint (in transform, 2, 3, 4, 5, 8, 9); break;
			case 6: actual.AddCurveToPoint (in transform, points [0], new CGPoint (4, 5), points [1]); break;
			case 7: actual.AddRect (in transform, rect); break;
			case 8: actual.AddRects (in transform, rects); break;
			case 9: actual.AddRects (in transform, rects, 1); break;
			case 10: actual.AddLines (in transform, points); break;
			case 11: actual.AddLines (in transform, points, 2); break;
			case 12: actual.AddEllipseInRect (in transform, rect); break;
			case 13: actual.AddArc (in transform, 2, 3, 6, 0, (nfloat) (Math.PI / 2), false); break;
			case 14: actual.AddArcToPoint (in transform, 8, 0, 8, 8, 2); break;
			case 15: actual.AddRelativeArc (in transform, 2, 3, 6, 0, (nfloat) (Math.PI / 2)); break;
			case 16: actual.AddPath (in transform, source); break;
			case 17: actual.AddRoundedRect (in transform, rect, 1, 2); break;
			}
#else
			switch (operation) {
			case 0: actual.MoveToPoint (transform, 2, 3); break;
			case 1: actual.MoveToPoint (transform, points [0]); break;
			case 2: actual.AddLineToPoint (transform, 8, 9); break;
			case 3: actual.AddLineToPoint (transform, points [1]); break;
			case 4: actual.AddQuadCurveToPoint (transform, 4, 5, 8, 9); break;
			case 5: actual.AddCurveToPoint (transform, 2, 3, 4, 5, 8, 9); break;
			case 6: actual.AddCurveToPoint (transform, points [0], new CGPoint (4, 5), points [1]); break;
			case 7: actual.AddRect (transform, rect); break;
			case 8: actual.AddRects (transform, rects); break;
			case 9: actual.AddRects (transform, rects, 1); break;
			case 10: actual.AddLines (transform, points); break;
			case 11: actual.AddLines (transform, points, 2); break;
			case 12: actual.AddEllipseInRect (transform, rect); break;
			case 13: actual.AddArc (transform, 2, 3, 6, 0, (nfloat) (Math.PI / 2), false); break;
			case 14: actual.AddArcToPoint (transform, 8, 0, 8, 8, 2); break;
			case 15: actual.AddRelativeArc (transform, 2, 3, 6, 0, (nfloat) (Math.PI / 2)); break;
			case 16: actual.AddPath (transform, source); break;
			case 17: actual.AddRoundedRect (transform, rect, 1, 2); break;
			}
#endif
			var bounds = rect;
			if (operation <= 1)
				bounds = new CGRect (2, 3, 0, 0);
			else if (operation <= 6)
				bounds = new CGRect (0, 0, 8, 9);
			else if (operation == 8)
				bounds = new CGRect (2, 3, 22, 32);
			else if (operation == 10)
				bounds = new CGRect (2, 3, 18, 27);
			else if (operation == 14)
				bounds = new CGRect (0, 0, 8, 2);
			AssertBounds (original, bounds);
			AssertBounds (actual, new CGRect (bounds.X + 11, bounds.Y + 17, bounds.Width, bounds.Height));
			AssertTranslatedPath (actual, original);
			Assert.That (transform, Is.EqualTo (saved), "Transform unchanged");
			Assert.That (rect, Is.EqualTo (new CGRect (2, 3, 6, 6)), "Rectangle unchanged");
			Assert.That (rects, Is.EqualTo (new [] { rect, new CGRect (20, 30, 4, 5) }), "Rectangles unchanged");
			Assert.That (points, Is.EqualTo (new [] { new CGPoint (2, 3), new CGPoint (8, 9), new CGPoint (20, 30) }), "Points unchanged");
			AssertBounds (source, rect);
		}

		[TestCase (0)]
		[TestCase (1)]
		[TestCase (2)]
		[TestCase (3)]
		[TestCase (4)]
		public void TransformCopyAndFactoryInputs (int operation)
		{
			var transform = CGAffineTransform.MakeTranslation (11, 17);
			var saved = transform;
			var rect = new CGRect (2, 3, 6, 6);
			using var source = CGPath.FromRect (rect);
			using var original = operation switch {
				0 => new CGPath (source),
				1 => source.Copy (),
				2 => CGPath.FromRect (rect),
				3 => CGPath.EllipseFromRect (rect),
				_ => CGPath.FromRoundedRect (rect, 1, 2),
			};
#if XAMCORE_5_0
			using var actual = operation switch {
				0 => new CGPath (source, in transform),
				1 => source.CopyByTransformingPath (in transform),
				2 => CGPath.FromRect (rect, in transform),
				3 => CGPath.EllipseFromRect (rect, in transform),
				_ => CGPath.FromRoundedRect (rect, 1, 2, in transform),
			};
#else
			using var actual = operation switch {
				0 => new CGPath (source, transform),
				1 => source.CopyByTransformingPath (transform),
				2 => CGPath.FromRect (rect, transform),
				3 => CGPath.EllipseFromRect (rect, transform),
				_ => CGPath.FromRoundedRect (rect, 1, 2, transform),
			};
#endif
			AssertBounds (original, rect);
			AssertBounds (actual, new CGRect (13, 20, 6, 6));
			AssertTranslatedPath (actual, original);
			AssertBounds (source, rect);
			Assert.That (transform, Is.EqualTo (saved), "Transform unchanged");
			Assert.That (rect, Is.EqualTo (new CGRect (2, 3, 6, 6)), "Rectangle unchanged");
		}

		[Test]
		public void TransformDashStrokeAndContainsInputs ()
		{
			var transform = CGAffineTransform.MakeTranslation (11, 17);
			var saved = transform;
			var lengths = new nfloat [] { 2, 2 };
			using var source = new CGPath ();
			source.MoveToPoint (0, 0);
			source.AddLineToPoint (10, 0);
			using var original = source.CopyByDashingPath (lengths);
			using var originalPhase = source.CopyByDashingPath (lengths, 1);
			using var originalStroke = source.CopyByStrokingPath (2, CGLineCap.Butt, CGLineJoin.Miter, 10);
#if XAMCORE_5_0
			using var actual = source.CopyByDashingPath (in transform, lengths);
			using var actualPhase = source.CopyByDashingPath (in transform, lengths, 1);
			using var actualStroke = source.CopyByStrokingPath (in transform, 2, CGLineCap.Butt, CGLineJoin.Miter, 10);
#else
			using var actual = source.CopyByDashingPath (transform, lengths);
			using var actualPhase = source.CopyByDashingPath (transform, lengths, 1);
			using var actualStroke = source.CopyByStrokingPath (transform, 2, CGLineCap.Butt, CGLineJoin.Miter, 10);
#endif
			AssertBounds (original, new CGRect (0, 0, 10, 0));
			AssertBounds (originalPhase, new CGRect (0, 0, 9, 0));
			AssertBounds (originalStroke, new CGRect (0, -1, 10, 2));
			AssertTranslatedPath (actual, original);
			AssertTranslatedPath (actualPhase, originalPhase);
			AssertTranslatedPath (actualStroke, originalStroke);
			var endpoints = new System.Collections.Generic.List<nfloat> ();
			original.Apply (element => endpoints.Add (element.Point1.X));
			Assert.That (endpoints, Is.EqualTo (new nfloat [] { 0, 2, 4, 6, 8, 10 }), "Dash endpoints");
			endpoints.Clear ();
			originalPhase.Apply (element => endpoints.Add (element.Point1.X));
			Assert.That (endpoints, Is.EqualTo (new nfloat [] { 0, 1, 3, 5, 7, 9 }), "Phase dash endpoints");
			using var rectangle = CGPath.FromRect (new CGRect (0, 0, 10, 10));
			foreach (var evenOdd in new [] { false, true }) {
				Assert.That (rectangle.ContainsPoint (new CGPoint (5, 5), evenOdd), Is.True, "Untransformed inside");
				Assert.That (rectangle.ContainsPoint (new CGPoint (-6, -12), evenOdd), Is.False, "Untransformed outside");
#if XAMCORE_5_0
				Assert.That (rectangle.ContainsPoint (in transform, new CGPoint (-6, -12), evenOdd), Is.True, "Transformed inside");
				Assert.That (rectangle.ContainsPoint (in transform, new CGPoint (5, 5), evenOdd), Is.False, "Transformed outside");
#else
				Assert.That (rectangle.ContainsPoint (transform, new CGPoint (-6, -12), evenOdd), Is.True, "Transformed inside");
				Assert.That (rectangle.ContainsPoint (transform, new CGPoint (5, 5), evenOdd), Is.False, "Transformed outside");
#endif
			}
			AssertBounds (source, new CGRect (0, 0, 10, 0));
			Assert.That (lengths, Is.EqualTo (new nfloat [] { 2, 2 }), "Dash lengths unchanged");
			Assert.That (transform, Is.EqualTo (saved), "Transform unchanged");
		}

		[DllImport (Constants.CoreFoundationLibrary)]
		extern static nint CFGetRetainCount (IntPtr handle);

		[Test]
		public void ApplyKeepsPathAlive ()
		{
			var path = new CGPath ();
			path.MoveToPoint (0, 0);
			path.AddLineToPoint (10, 10);
			path.CloseSubpath ();
			var reference = new WeakReference (path);
			var handle = path.Handle;
			var elements = new List<CGPathElementType> ();
			var alive = true;

			// An independent native retain makes a lifetime regression fail without crashing.
			TestRuntime.CFRetain (handle);
			try {
				path.Apply (element => {
					GC.Collect ();
					GC.WaitForPendingFinalizers ();
					alive &= reference.IsAlive;
					elements.Add (element.Type);
				});
				Assert.That (alive, Is.True, "Path must remain alive during enumeration");
				Assert.That (elements, Is.EqualTo (new [] {
					CGPathElementType.MoveToPoint, CGPathElementType.AddLineToPoint, CGPathElementType.CloseSubpath,
				}), "Elements");
			} finally {
				TestRuntime.CFRelease (handle);
			}
		}

		[Test]
		public void EllipseFromRect ()
		{
			var rect = new CGRect (0, 0, 15, 15);
			var matrix = CGAffineTransform.MakeIdentity ();
			using (CGPath p = CGPath.EllipseFromRect (rect, matrix)) {
				Assert.That (p, Is.Not.Null, "non-null");
			}
		}

		[Test]
		public void CopyByDashingPath_18764 ()
		{
			var identity = CGAffineTransform.MakeIdentity ();
			using (var path = CGPath.EllipseFromRect (CGRect.Empty, identity)) {
				var lengths = new nfloat [] { 10, 10 };
				var phase = 2;
				using (var d1 = path.CopyByDashingPath (lengths)) {
					Assert.That (d1.Handle, Is.Not.EqualTo (IntPtr.Zero), "d1");
				}
				using (var d2 = path.CopyByDashingPath (lengths, phase)) {
					Assert.That (d2.Handle, Is.Not.EqualTo (IntPtr.Zero), "d2");
				}
				using (var d3 = path.CopyByDashingPath (identity, lengths)) {
					Assert.That (d3.Handle, Is.Not.EqualTo (IntPtr.Zero), "d3");
				}
				using (var d4 = path.CopyByDashingPath (identity, lengths, phase)) {
					Assert.That (d4.Handle, Is.Not.EqualTo (IntPtr.Zero), "d4");
				}
			}
		}

		[Test]
		public void MoveToPoint ()
		{
			var matrix = CGAffineTransform.MakeIdentity ();
			using (CGPath p1 = new CGPath ())
			using (CGPath p2 = new CGPath ()) {
				Assert.That (p1.IsEmpty, Is.True, "IsEmpty-1");
				p1.MoveToPoint (0, 0);
				p2.MoveToPoint (matrix, 0, 0);
				Assert.That (p1.IsEmpty, Is.False, "IsEmpty-2");
				Assert.That (p1, Is.EqualTo (p2), "CGPathEqualToPath");
			}
		}

		[Test]
		public void AddLineToPoint ()
		{
			var matrix = CGAffineTransform.MakeIdentity ();
			using (CGPath p1 = new CGPath ())
			using (CGPath p2 = new CGPath ()) {
				Assert.That (p1.IsEmpty, Is.True, "IsEmpty-1");
				p1.MoveToPoint (0, 0);
				p1.AddLineToPoint (1, 1);
				p2.MoveToPoint (matrix, 0, 0);
				p2.AddLineToPoint (matrix, 1, 1);
				Assert.That (p1.IsEmpty, Is.False, "IsEmpty-2");
				Assert.That (p1, Is.EqualTo (p2), "CGPathEqualToPath");
			}
		}

		[Test]
		public void AddCurveToPoint ()
		{
			var matrix = CGAffineTransform.MakeIdentity ();
			using (CGPath p1 = new CGPath ())
			using (CGPath p2 = new CGPath ()) {
				Assert.That (p1.IsEmpty, Is.True, "IsEmpty-1");
				p1.MoveToPoint (0, 0);
				p1.AddCurveToPoint (1, 2, 3, 4, 5, 6);
				p2.MoveToPoint (0, 0);
				p2.AddCurveToPoint (matrix, 1, 2, 3, 4, 5, 6);
				Assert.That (p1.IsEmpty, Is.False, "IsEmpty-2");
				Assert.That (p1, Is.EqualTo (p2), "CGPathEqualToPath");
			}
		}

		[Test]
		public void AddQuadCurveToPoint ()
		{
			var matrix = CGAffineTransform.MakeIdentity ();
			using (CGPath p1 = new CGPath ())
			using (CGPath p2 = new CGPath ()) {
				Assert.That (p1.IsEmpty, Is.True, "IsEmpty-1");
				p1.MoveToPoint (0, 0);
				p1.AddQuadCurveToPoint (1, 2, 3, 4);
				p1.CloseSubpath ();
				p2.MoveToPoint (0, 0);
				p2.AddQuadCurveToPoint (matrix, 1, 2, 3, 4);
				Assert.That (p1.IsEmpty, Is.False, "IsEmpty-2");
				Assert.That (p1, Is.Not.EqualTo (p2), "CGPathEqualToPath-2");
				p2.CloseSubpath ();
				Assert.That (p1, Is.EqualTo (p2), "CGPathEqualToPath");
			}
		}

		[Test]
		public void AddRect ()
		{
			var rect = new CGRect (0, 0, 15, 15);
			var matrix = CGAffineTransform.MakeIdentity ();
			using (CGPath p1 = new CGPath ())
			using (CGPath p2 = new CGPath ()) {
				Assert.That (p1.IsEmpty, Is.True, "IsEmpty-1");
				p1.AddRect (rect);
				p2.AddRect (matrix, rect);
				Assert.That (p1.IsEmpty, Is.False, "IsEmpty-2");
				Assert.That (p1, Is.EqualTo (p2), "CGPathEqualToPath");
			}
		}

		[Test]
		public void AddRects ()
		{
			var rect = new CGRect (0, 0, 15, 15);
			var matrix = CGAffineTransform.MakeIdentity ();
			using (CGPath p1 = new CGPath ())
			using (CGPath p2 = new CGPath ()) {
				Assert.That (p1.IsEmpty, Is.True, "IsEmpty-1");
				p1.AddRects (new [] { rect, rect }, 1);
				p2.AddRects (matrix, new [] { rect }, 1);
				Assert.That (p1.IsEmpty, Is.False, "IsEmpty-2");
				Assert.That (p1, Is.EqualTo (p2), "CGPathEqualToPath");
			}
		}

		[Test]
		public void AddLines ()
		{
			var matrix = CGAffineTransform.MakeIdentity ();
			using (CGPath p1 = new CGPath ())
			using (CGPath p2 = new CGPath ()) {
				Assert.That (p1.IsEmpty, Is.True, "IsEmpty-1");
				p1.AddLines (new [] { CGPoint.Empty });
				p2.AddLines (matrix, new [] { CGPoint.Empty });
				Assert.That (p1.IsEmpty, Is.False, "IsEmpty-2");
				Assert.That (p1, Is.EqualTo (p2), "CGPathEqualToPath");
			}
		}

		[Test]
		public void AddEllipseInRect ()
		{
			var rect = new CGRect (0, 0, 15, 15);
			var matrix = CGAffineTransform.MakeIdentity ();
			using (CGPath p1 = new CGPath ())
			using (CGPath p2 = new CGPath ()) {
				Assert.That (p1.IsEmpty, Is.True, "IsEmpty-1");
				p1.AddEllipseInRect (rect);
				p2.AddEllipseInRect (matrix, rect);
				Assert.That (p1.IsEmpty, Is.False, "IsEmpty-2");
				Assert.That (p1, Is.EqualTo (p2), "CGPathEqualToPath");
			}
		}

		[Test]
		public void AddArc ()
		{
			var matrix = CGAffineTransform.MakeIdentity ();
			using (CGPath p1 = new CGPath ())
			using (CGPath p2 = new CGPath ()) {
				Assert.That (p1.IsEmpty, Is.True, "IsEmpty-1");
				p1.AddArc (0, 0, 10, 0, 90, true);
				p2.AddArc (matrix, 0, 0, 10, 0, 90, true);
				Assert.That (p1.IsEmpty, Is.False, "IsEmpty-2");
				Assert.That (p1, Is.EqualTo (p2), "CGPathEqualToPath");
			}
		}

		[Test]
		public void AddArcToPoint ()
		{
			var matrix = CGAffineTransform.MakeIdentity ();
			using (CGPath p1 = new CGPath ())
			using (CGPath p2 = new CGPath ()) {
				Assert.That (p1.IsEmpty, Is.True, "IsEmpty-1");
				p1.MoveToPoint (0, 0);
				p1.AddArcToPoint (0, 0, 10, 0, 90);
				p2.MoveToPoint (0, 0);
				p2.AddArcToPoint (matrix, 0, 0, 10, 0, 90);
				Assert.That (p1.IsEmpty, Is.False, "IsEmpty-2");
				Assert.That (p1, Is.EqualTo (p2), "CGPathEqualToPath");
			}
		}

		[Test]
		public void AddRelativeArc ()
		{
			var matrix = CGAffineTransform.MakeIdentity ();
			using (CGPath p1 = new CGPath ())
			using (CGPath p2 = new CGPath ()) {
				Assert.That (p1.IsEmpty, Is.True, "IsEmpty-1");
				p1.MoveToPoint (0, 0);
				p1.AddRelativeArc (0, 0, 10, 0, 90);
				p2.MoveToPoint (0, 0);
				p2.AddRelativeArc (matrix, 0, 0, 10, 0, 90);
				Assert.That (p1.IsEmpty, Is.False, "IsEmpty-2");
				Assert.That (p1, Is.EqualTo (p2), "CGPathEqualToPath");
			}
		}

		[Test]
		public void AddPath ()
		{
			using (CGPath p1 = new CGPath ())
			using (CGPath p2 = new CGPath ()) {
				p1.MoveToPoint (0, 0);
				p2.AddPath (p1);
				Assert.That (p2.IsEmpty, Is.False, "IsEmpty");
			}
		}

		[Test]
		public void Normalizing ()
		{
			TestRuntime.AssertXcodeVersion (14, 0);
			using (CGPath p1 = new CGPath ()) {
				p1.MoveToPoint (0, 0);
				p1.AddLineToPoint (1, 1);
				Assert.That (p1.CreateByNormalizing (false), Is.Not.Null);
				Assert.That (p1.CreateByNormalizing (true), Is.Not.Null);
			}
		}

		[Test]
		public void Union ()
		{
			TestRuntime.AssertXcodeVersion (14, 0);
			using (CGPath p1 = new CGPath ()) {
				p1.MoveToPoint (0, 0);
				p1.AddLineToPoint (1, 1);
				using (CGPath p2 = new CGPath ()) {
					p2.MoveToPoint (2, 2);
					p2.AddLineToPoint (0, 0);
					Assert.That (p1.CreateByUnioningPath (p2, false), Is.Not.Null);
					Assert.That (p1.CreateByUnioningPath (p2, true), Is.Not.Null);
				}
			}
		}

		[Test]
		public void Intersecting ()
		{
			TestRuntime.AssertXcodeVersion (14, 0);
			using (CGPath p1 = new CGPath ()) {
				p1.MoveToPoint (0, 0);
				p1.AddLineToPoint (1, 1);
				using (CGPath p2 = new CGPath ()) {
					p2.MoveToPoint (2, 2);
					p2.AddLineToPoint (0, 0);
					Assert.That (p1.CreateByIntersectingPath (p2, false), Is.Not.Null);
					Assert.That (p1.CreateByIntersectingPath (p2, true), Is.Not.Null);
				}
			}
		}

		[Test]
		public void Subtracting ()
		{
			TestRuntime.AssertXcodeVersion (14, 0);
			using (CGPath p1 = new CGPath ()) {
				p1.MoveToPoint (0, 0);
				p1.AddLineToPoint (1, 1);
				using (CGPath p2 = new CGPath ()) {
					p2.MoveToPoint (2, 2);
					p2.AddLineToPoint (0, 0);
					Assert.That (p1.CreateBySubtractingPath (p2, false), Is.Not.Null);
					Assert.That (p1.CreateBySubtractingPath (p2, true), Is.Not.Null);
				}
			}
		}

		[Test]
		public void SymmetricDifference ()
		{
			TestRuntime.AssertXcodeVersion (14, 0);
			using (CGPath p1 = new CGPath ()) {
				p1.MoveToPoint (0, 0);
				p1.AddLineToPoint (1, 1);
				using (CGPath p2 = new CGPath ()) {
					p2.MoveToPoint (2, 2);
					p2.AddLineToPoint (0, 0);
					Assert.That (p1.CreateBySymmetricDifferenceOfPath (p2, false), Is.Not.Null);
					Assert.That (p1.CreateBySymmetricDifferenceOfPath (p2, true), Is.Not.Null);
				}
			}
		}

		[Test]
		public void LineBySubtracting ()
		{
			TestRuntime.AssertXcodeVersion (14, 0);
			using (CGPath p1 = new CGPath ()) {
				p1.MoveToPoint (0, 0);
				p1.AddLineToPoint (1, 1);
				using (CGPath p2 = new CGPath ()) {
					p2.MoveToPoint (2, 2);
					p2.AddLineToPoint (0, 0);
					Assert.That (p1.CreateLineBySubtractingPath (p2, false), Is.Not.Null);
					Assert.That (p1.CreateLineBySubtractingPath (p2, true), Is.Not.Null);
				}
			}
		}

		[Test]
		public void LineByIntersecting ()
		{
			TestRuntime.AssertXcodeVersion (14, 0);
			using (CGPath p1 = new CGPath ()) {
				p1.MoveToPoint (0, 0);
				p1.AddLineToPoint (1, 1);
				using (CGPath p2 = new CGPath ()) {
					p2.MoveToPoint (2, 2);
					p2.AddLineToPoint (0, 0);
					Assert.That (p1.CreateLineByIntersectingPath (p2, false), Is.Not.Null);
					Assert.That (p1.CreateLineByIntersectingPath (p2, true), Is.Not.Null);
				}
			}
		}

		[Test]
		public void GetSeparateComponents ()
		{
			TestRuntime.AssertXcodeVersion (14, 0);
			using (CGPath p1 = new CGPath ()) {
				p1.MoveToPoint (0, 0);
				p1.AddLineToPoint (1, 1);
				Assert.That (p1.GetSeparateComponents (true).Length, Is.EqualTo (0));
				Assert.That (p1.GetSeparateComponents (false).Length, Is.EqualTo (0));
			}
		}

		[Test]
		public void CreateByFlattening ()
		{
			TestRuntime.AssertXcodeVersion (14, 0);
			using (CGPath p1 = new CGPath ()) {
				p1.MoveToPoint (0, 0);
				p1.AddLineToPoint (1, 1);
				Assert.That (p1.CreateByFlattening (new nfloat (0.5)), Is.Not.Null);
			}
		}

		[Test]
		public void DoesIntersect ()
		{
			TestRuntime.AssertXcodeVersion (14, 0);
			using (CGPath p1 = new CGPath ()) {
				p1.MoveToPoint (0, 0);
				p1.AddLineToPoint (2, 2);
				using (CGPath p2 = new CGPath ()) {
					p2.MoveToPoint (0, 2);
					p2.AddLineToPoint (2, 0);
					Assert.That (p1.DoesIntersect (p2, false), Is.False);
					Assert.That (p1.DoesIntersect (p2, false), Is.False);
				}
			}
		}

		[Test]
		public void Bug40230 ()
		{
			var rect = new CGRect (1, 1, 25, 25);
			// Assertion failed: (corner_width >= 0 && 2 * corner_width <= CGRectGetWidth(rect)), function CGPathCreateWithRoundedRect
			Assert.Throws<ArgumentException> (() => CGPath.FromRoundedRect (rect, 13.5f, 1), "width");
			// Assertion failed: (corner_height >= 0 && 2 * corner_height <= CGRectGetHeight(rect)), function CGPathRef CGPathCreateWithRoundedRect
			Assert.Throws<ArgumentException> (() => CGPath.FromRoundedRect (rect, 1, 13.5f), "height");
			using (var path = CGPath.FromRoundedRect (rect, 1, 1)) {
				Assert.That (path, Is.Not.Null, "path");
			}
		}

		[Test]
		public void IncreaseRetainCountMakeMutable ()
		{
			// ensure that we do not crash and that the retain count is changed.
			using (CGPath p1 = new CGPath ()) {
				var count = CFGetRetainCount (p1.Handle);
				using (var copy = p1.Copy ()) {
					var newRetainCount = CFGetRetainCount (copy.Handle);
					Assert.That (newRetainCount, Is.EqualTo (count), "Ref count should not have changed.");
					Assert.That (count, Is.EqualTo ((nint) 1), "Original count.");
					Assert.That (newRetainCount, Is.EqualTo ((nint) 1), "New count");
				}
			}
		}
	}
}
