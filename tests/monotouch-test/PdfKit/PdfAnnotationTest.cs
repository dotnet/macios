//
// PdfAnnotation Unit Tests
//
// Authors:
//	Rolf Bjarne Kvinge <rolf@xamarin.com>
//
// Copyright 2017 Microsoft Inc.
//

#if __IOS__ || MONOMAC

using CoreGraphics;
using Foundation;
using PdfKit;

namespace MonoTouchFixtures.PdfKit {

	[TestFixture]
	// we want the test to be available if we use the linker
	[Preserve (AllMembers = true)]
	public class PdfAnnotationTest {
		[OneTimeSetUp]
		public void Setup ()
		{
			TestRuntime.AssertXcodeVersion (9, 0);
		}

		[Test]
		public void QuadrilateralPoints ()
		{
			using (var obj = new PdfAnnotation ()) {
				Assert.IsNotNull (obj.QuadrilateralPoints, "Q1");
				Assert.AreEqual (0, obj.QuadrilateralPoints.Length, "Q1b");

				var points = new CGPoint []
				{
					new CGPoint (0, 1),
					new CGPoint (2, 3),
					new CGPoint (4, 5),
					new CGPoint (6, 7),
				};

				obj.QuadrilateralPoints = points;
				Assert.AreEqual (points, obj.QuadrilateralPoints, "Q2");

				obj.QuadrilateralPoints = null;
				Assert.IsNotNull (obj.QuadrilateralPoints, "Q3");
				Assert.AreEqual (0, obj.QuadrilateralPoints.Length, "Q3b");
			}
		}

		[Test]
		public void AnnotationHitEventArgs ()
		{
			using (var annotation = new PdfAnnotation ())
			using (var key = new NSString ("PDFAnnotationHit"))
			using (var userInfo = new NSMutableDictionary ()) {
				userInfo.Add (key, annotation);

				using (var notification = NSNotification.FromName (PdfView.AnnotationHitNotification, null, userInfo)) {
					var args = new PdfViewAnnotationHitEventArgs (notification);
					var actual = args.AnnotationHit;

					Assert.That (actual, Is.Not.Null, "AnnotationHit");
					if (actual is null)
						return;

					Assert.That (actual.Handle, Is.EqualTo (annotation.Handle), "Handle");
				}
			}
		}

		[Test]
		public void AnnotationHitEventArgsNoUserInfo ()
		{
			using (var notification = NSNotification.FromName (PdfView.AnnotationHitNotification, null)) {
				var args = new PdfViewAnnotationHitEventArgs (notification);
				Assert.That (args.AnnotationHit, Is.Null, "AnnotationHit");
			}
		}
	}
}

#endif // __IOS__ || MONOMAC
