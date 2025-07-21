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
	public class VTOpticalFlowConfigurationTest {
		[Test]
		public void Properties ()
		{
			TestRuntime.AssertXcodeVersion (16, 3);
			TestRuntime.AssertOnlyARM64 (); // "Fail to create effect configuration" is printed to the terminal

			Assert.Multiple (() => {
				using var obj = new VTOpticalFlowConfiguration (320, 320, VTOpticalFlowConfigurationQualityPrioritization.Normal, VTOpticalFlowConfigurationRevision.Revision1);
				Assert.That (obj.FrameWidth, Is.EqualTo ((nint) 320), "FrameWidth");
				Assert.That (obj.FrameHeight, Is.EqualTo ((nint) 320), "FrameHeight");
				Assert.That (obj.QualityPrioritization, Is.EqualTo (VTOpticalFlowConfigurationQualityPrioritization.Normal), "QualityPrioritization");
				Assert.That (obj.Revision, Is.EqualTo (VTOpticalFlowConfigurationRevision.Revision1), "Revision");
				TestRuntime.NSLog ($"FrameSupportedPixelFormats: {obj.FrameSupportedPixelFormats}");
				TestRuntime.NSLog ($"SourcePixelBufferAttributes: {obj.SourcePixelBufferAttributes}");
				TestRuntime.NSLog ($"DestinationPixelBufferAttributes: {obj.DestinationPixelBufferAttributes}");
				Assert.That (VTOpticalFlowConfiguration.ProcessorSupported, Is.True, "ProcessorSupported");

				Assert.That (VTOpticalFlowConfiguration.SupportedRevisions, Is.Not.Null, "SupportedRevisions");
				Assert.That (VTOpticalFlowConfiguration.SupportedRevisions, Does.Contain (VTOpticalFlowConfigurationRevision.Revision1), "SupportedRevisions.Contains");
				Assert.That (VTOpticalFlowConfiguration.WeakSupportedRevisions, Is.Not.Null, "WeakSupportedRevisions");
				Assert.That (VTOpticalFlowConfiguration.WeakSupportedRevisions, Does.Contain ((nuint) 1), "WeakSupportedRevisions.Contains");
				Assert.That (Enum.GetValues<VTOpticalFlowConfigurationRevision> (), Does.Contain (VTOpticalFlowConfiguration.DefaultRevision), "DefaultRevision");
			});
		}
	}
}

#endif // __MACOS__
