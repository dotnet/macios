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
			ClassicAssert.True (f.Bounds.IsEmpty, "Bounds");
			Assert.That (f.GetType ().Name, Is.EqualTo ("UIGraphicsRendererFormat"), "Name");
		}

		[Test]
		public void ImageDefaultFormat ()
		{
			var f = UIGraphicsImageRendererFormat.DefaultFormat;
			ClassicAssert.True (f.Bounds.IsEmpty, "Bounds");
			ClassicAssert.False (f.Opaque, "Opaque");
			//ClassicAssert.False (f.PrefersExtendedRange, "PrefersExtendedRange"); // new iPhone (7/7+) returns True
			Assert.That (f.Scale, Is.GreaterThan ((nfloat) 0), "Scale"); // varies on platform
			Assert.That (f.GetType ().Name, Is.EqualTo ("UIGraphicsImageRendererFormat"), "Name");
		}

		[Test]
		public void PdfDefaultFormat ()
		{
			var f = UIGraphicsPdfRendererFormat.DefaultFormat;
			ClassicAssert.True (f.Bounds.IsEmpty, "Bounds");
			ClassicAssert.Null (f.DocumentInfo, "DocumentInfo");
			Assert.That (f.GetType ().Name, Is.EqualTo ("UIGraphicsPdfRendererFormat"), "Name");
		}
	}
}

#endif
