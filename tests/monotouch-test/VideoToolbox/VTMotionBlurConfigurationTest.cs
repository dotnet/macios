// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

#if __MACOS__

using System;

using AVFoundation;
using CoreFoundation;
using CoreMedia;
using CoreVideo;
using Foundation;
using ObjCRuntime;
using VideoToolbox;

using NUnit.Framework;

using Xamarin.Utils;

namespace MonoTouchFixtures.VideoToolbox {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class VTMotionBlurConfigurationTEst {
		[Test]
		public void Properties ()
		{
			TestRuntime.AssertXcodeVersion (16, 3);

			Assert.Multiple (() => {
				using var obj = new VTMotionBlurConfiguration (320, 320, true, VTMotionBlurConfigurationQualityPrioritization.Normal, VTMotionBlurConfigurationRevision.Revision1);
				Assert.That (obj.FrameWidth, Is.EqualTo ((nint) 320), "FrameWidth");
				Assert.That (obj.FrameHeight, Is.EqualTo ((nint) 320), "FrameHeight");
				Assert.That (obj.UsePrecomputedFlow, Is.EqualTo (true), "UsePrecomputedFlow");
				Assert.That (obj.QualityPrioritization, Is.EqualTo (VTMotionBlurConfigurationQualityPrioritization.Normal), "QualityPrioritization");
				Assert.That (obj.Revision, Is.EqualTo (VTMotionBlurConfigurationRevision.Revision1), "Revision");
				TestRuntime.NSLog ($"FrameSupportedPixelFormats: {obj.FrameSupportedPixelFormats}");
				TestRuntime.NSLog ($"SourcePixelBufferAttributes: {obj.SourcePixelBufferAttributes}");
				TestRuntime.NSLog ($"DestinationPixelBufferAttributes: {obj.DestinationPixelBufferAttributes}");
				Assert.That (VTMotionBlurConfiguration.ProcessorSupported, Is.True, "ProcessorSupported");

				Assert.That (VTMotionBlurConfiguration.SupportedRevisions, Is.Not.Null, "SupportedRevisions");
				Assert.That (VTMotionBlurConfiguration.SupportedRevisions, Does.Contain (VTMotionBlurConfigurationRevision.Revision1), "SupportedRevisions.Contains");
				Assert.That (VTMotionBlurConfiguration.WeakSupportedRevisions, Is.Not.Null, "WeakSupportedRevisions");
				Assert.That (VTMotionBlurConfiguration.WeakSupportedRevisions, Does.Contain ((nuint) 1), "WeakSupportedRevisions.Contains");
				Assert.That (Enum.GetValues<VTMotionBlurConfigurationRevision> (), Does.Contain (VTMotionBlurConfiguration.DefaultRevision), "DefaultRevision");
			});
		}
	}
}

#endif // __MACOS__
