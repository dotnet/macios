//
// Unit tests for CGImage
//
// Authors:
//	Marek Safar (marek.safar@gmail.com)
//	Sebastien Pouliot  <sebastien@xamarin.com>
//
// Copyright 2012-2013, 2015 Xamarin Inc. All rights reserved.
//

using System.IO;

using Foundation;
#if MONOMAC
using AppKit;
#else
using UIKit;
#endif
using CoreGraphics;
using NUnit.Framework;

namespace MonoTouchFixtures.CoreGraphics {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class CGImageTest {

		[Test]
		public void FromPNG ()
		{
			string file = Path.Combine (NSBundle.MainBundle.ResourcePath, "basn3p08.png");
			using (var dp = new CGDataProvider (file))
			using (var img = CGImage.FromPNG (dp, null, false, CGColorRenderingIntent.Default))
#if MONOMAC
			using (var ui = new NSImage (img, new CGSize (10, 10))) {
#else
			using (var ui = new UIImage (img, 1.0f, UIImageOrientation.Up)) {
#endif
				Assert.IsNotNull (ui.CGImage, "CGImage");
				if (TestRuntime.CheckXcodeVersion (7, 0))
					Assert.That (img.UTType.ToString (), Is.EqualTo ("public.png"), "UTType");
			}
		}

		[Test]
		public void ContentHeadroom ()
		{
			TestRuntime.AssertXcodeVersion (16, 0);

			Assert.Multiple (() => {
				var frame = new CGSize (32, 32);
				using var provider = new CGDataProvider (new byte [(int) frame.Width * (int) frame.Height * 4]);
				using var colorSpace = CGColorSpace.CreateWithName (CGColorSpaceNames.Itur_2100_PQ);
				using var img = new CGImage (0.0f, (int) frame.Width, (int) frame.Height, 8, 32, 4 * (int) frame.Width, colorSpace, CGBitmapFlags.ByteOrderDefault | CGBitmapFlags.Last, provider, null, false, CGColorRenderingIntent.Default);
				Assert.IsNotNull (img, "Image");
				Assert.AreEqual (4.92610836f, img.ContentHeadroom, "ContentHeadroom A");
				Assert.IsTrue (img.ShouldToneMap, "ShouldToneMap A");
				Assert.IsFalse (img.ContainsImageSpecificToneMappingMetadata, "ContainsImageSpecificToneMappingMetadata A");

				using var copy = img.Copy (3.0f);
				Assert.IsNotNull (copy, "Copy");
				Assert.AreEqual (3.0f, copy.ContentHeadroom, "ContentHeadroom B");
				Assert.IsTrue (copy.ShouldToneMap, "ShouldToneMap B");
				Assert.IsFalse (copy.ContainsImageSpecificToneMappingMetadata, "ContainsImageSpecificToneMappingMetadata B");

				Assert.AreEqual (4.92610836f, CGImage.DefaultHdrImageContentHeadroom, "DefaultHdrImageContentHeadroom");

				if (TestRuntime.CheckXcodeVersion (26, 0)) {
					Assert.That (copy.CalculatedContentHeadroom, Is.EqualTo (0.0f), "CalculatedContentHeadroom B");
					Assert.That (copy.ContentAverageLightLevel, Is.EqualTo (0.0f), "ContentAverageLightLevel B");
					Assert.That (copy.CalculatedContentAverageLightLevel, Is.Not.EqualTo (0.0f), "CalculatedContentAverageLightLevel B");

					using var copy2 = img.CopyWithContentAverageLightLevel (0.75f);
					Assert.That (copy2.CalculatedContentHeadroom, Is.EqualTo (0.0f), "CalculatedContentHeadroom C");
					Assert.That (copy2.ContentAverageLightLevel, Is.Not.EqualTo (0.0f), "ContentAverageLightLevel C");
					Assert.That (copy2.CalculatedContentAverageLightLevel, Is.Not.EqualTo (0.0f), "CalculatedContentAverageLightLevel C");

					using var copy3 = img.CopyWithCalculatedHdrStats ();
					Assert.That (copy3.CalculatedContentHeadroom, Is.EqualTo (0.0f), "CalculatedContentHeadroom D");
					Assert.That (copy3.ContentAverageLightLevel, Is.Not.EqualTo (0.0f), "ContentAverageLightLevel D");
					Assert.That (copy3.CalculatedContentAverageLightLevel, Is.Not.EqualTo (0.0f), "CalculatedContentAverageLightLevel D");
				}
			});
		}
	}
}
