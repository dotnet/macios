//
// AVDepthDataTests.cs
//
// Authors:
//	Alex Soto <alexsoto@microsoft.com>
//
//
// Copyright 2017 Xamarin Inc. All rights reserved.
//

using AVFoundation;
using ImageIO;

namespace MonoTouchFixtures.AVFoundation {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class AVDepthDataTests {

		[Test]
		public void AvailableDepthDataTypesTest ()
		{
#if !MONOMAC
			TestRuntime.AssertDevice ();
#endif
			TestRuntime.AssertXcodeVersion (9, 0);

			// xamarinmonkey.heic is the new photo format, also this one includes depth data
			var imgdata = NSData.FromUrl (NSBundle.MainBundle.GetUrlForResource ("xamarinmonkey", "heic", "CoreImage"));
			Assert.That (imgdata, Is.Not.Null, "imgdata");

			var imageSource = CGImageSource.FromData (imgdata);
			Assert.That (imageSource, Is.Not.Null, "imageSource");

			// fetching the image count works around a crash in CopyAuxiliaryDataInfo on macOS 10.15 (https://github.com/xamarin/maccore/issues/1802).
			Assert.That (imageSource.ImageCount, Is.Not.EqualTo (0), "ImageCount");

			var info = imageSource.CopyAuxiliaryDataInfo (0, CGImageAuxiliaryDataType.Disparity);
			Assert.That (info, Is.Not.Null, "info");

			NSError err;
			var depthData = AVDepthData.Create (info, out err);
			Assert.That (depthData, Is.Not.Null, "depthData");
			Assert.That (depthData.AvailableDepthDataTypes, Is.Not.Null, "AvailableDepthDataTypes");
		}
	}
}
