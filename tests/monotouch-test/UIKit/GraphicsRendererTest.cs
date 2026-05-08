//
// UIGraphicsRenderer* Unit Tests
//
// Authors:
//	Sebastien Pouliot  <sebastien@xamarin.com>
//
// Copyright 2016 Xamarin Inc.
//

#if !MONOMAC

using UIKit;

namespace MonoTouchFixtures.UIKit {

	[TestFixture]
	// we want the test to be availble if we use the linker
	[Preserve (AllMembers = true)]
	public class GraphicsRendererTest {

		[SetUp]
		public void Setup ()
		{
			TestRuntime.AssertXcodeVersion (8, 0);
		}

		[Test]
		public void BaseDefaultFormat ()
		{
			var f = UIGraphicsRendererFormat.DefaultFormat;
			Assert.That (f.Bounds.IsEmpty, Is.True, "Bounds");
			Assert.That (f.GetType ().Name, Is.EqualTo ("UIGraphicsRendererFormat"), "Name");
		}

		[Test]
		public void ImageDefaultFormat ()
		{
			var f = UIGraphicsImageRendererFormat.DefaultFormat;
			Assert.That (f.Bounds.IsEmpty, Is.True, "Bounds");
			Assert.That (f.Opaque, Is.False, "Opaque");
			//Assert.That (f.PrefersExtendedRange, Is.False, "PrefersExtendedRange"); // new iPhone (7/7+) returns True
			Assert.That (f.Scale, Is.GreaterThan ((nfloat) 0), "Scale"); // varies on platform
			Assert.That (f.GetType ().Name, Is.EqualTo ("UIGraphicsImageRendererFormat"), "Name");
		}

		[Test]
		public void PdfDefaultFormat ()
		{
			var f = UIGraphicsPdfRendererFormat.DefaultFormat;
			Assert.That (f.Bounds.IsEmpty, Is.True, "Bounds");
			Assert.That (f.DocumentInfo, Is.Null, "DocumentInfo");
			Assert.That (f.GetType ().Name, Is.EqualTo ("UIGraphicsPdfRendererFormat"), "Name");
		}
	}
}

#endif
