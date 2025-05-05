// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

#if __MACOS__

using System;
using System.Linq;

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
	public class VTFrameRateConversionConfigurationTest {
		[Test]
		public void Properties ()
		{
			TestRuntime.AssertXcodeVersion (16, 3);
			TestRuntime.AssertOnlyARM64 (); // "Fail to create effect configuration" is printed to the terminal

			Assert.Multiple (() => {
				using var obj = new VTFrameRateConversionConfiguration (320, 320, false, VTFrameRateConversionConfigurationQualityPrioritization.Normal, VTFrameRateConversionConfigurationRevision.Revision1);
				Assert.That (obj.FrameWidth, Is.EqualTo ((nint) 320), "FrameWidth");
				Assert.That (obj.FrameHeight, Is.EqualTo ((nint) 320), "FrameHeight");
				Assert.That (obj.UsePrecomputedFlow, Is.EqualTo (false), "UsePrecomputedFlow");
				Assert.That (obj.QualityPrioritization, Is.EqualTo (VTFrameRateConversionConfigurationQualityPrioritization.Normal), "QualityPrioritization");
				Assert.That (obj.Revision, Is.EqualTo (VTFrameRateConversionConfigurationRevision.Revision1), "Revision");
				Assert.That (obj.FrameSupportedPixelFormats, Is.Not.Null, "FrameSupportedPixelFormats");
				Assert.That (obj.FrameSupportedPixelFormats.Length, Is.EqualTo (obj.WeakFrameSupportedPixelFormats.Length), "FrameSupportedPixelFormats.Length");

				Assert.That (obj.SourcePixelBufferAttributes, Is.Not.Null, "SourcePixelBufferAttributes");
				Assert.That (obj.SourcePixelBufferAttributes.Height, Is.EqualTo ((nint) 320), "SourcePixelBufferAttributes.Height");
				Assert.That (obj.SourcePixelBufferAttributes.Width, Is.EqualTo ((nint) 320), "SourcePixelBufferAttributes.Width");
				Assert.That (obj.SourcePixelBufferAttributes.PixelFormatTypes, Is.Not.Null, "SourcePixelBufferAttributes.PixelFormatTypes");
				Assert.That (obj.SourcePixelBufferAttributes.PixelFormatTypes?.Length, Is.EqualTo (1), "SourcePixelBufferAttributes.PixelFormatTypes.Length");
				Assert.That (obj.SourcePixelBufferAttributes.PixelFormatTypes? [0], Is.EqualTo (CVPixelFormatType.CV64RGBAHalf), "SourcePixelBufferAttributes.PixelFormatTypes[0]");
				Assert.That (obj.WeakSourcePixelBufferAttributes, Is.Not.Null, "WeakSourcePixelBufferAttributes");

				Assert.That (obj.DestinationPixelBufferAttributes, Is.Not.Null, "DestinationPixelBufferAttributes");
				Assert.That (obj.DestinationPixelBufferAttributes.Height, Is.EqualTo ((nint) 320), "DestinationPixelBufferAttributes.Height");
				Assert.That (obj.DestinationPixelBufferAttributes.Width, Is.EqualTo ((nint) 320), "DestinationPixelBufferAttributes.Width");
				Assert.That (obj.DestinationPixelBufferAttributes.PixelFormatTypes, Is.Not.Null, "DestinationPixelBufferAttributes.PixelFormatTypes");
				Assert.That (obj.DestinationPixelBufferAttributes.PixelFormatTypes?.Length, Is.EqualTo (1), "DestinationPixelBufferAttributes.PixelFormatTypes.Length");
				Assert.That (obj.DestinationPixelBufferAttributes.PixelFormatTypes? [0], Is.EqualTo (CVPixelFormatType.CV64RGBAHalf), "DestinationPixelBufferAttributes.PixelFormatTypes[0]");
				Assert.That (obj.WeakDestinationPixelBufferAttributes, Is.Not.Null, "WeakDestinationPixelBufferAttributes");

				Assert.That (VTFrameRateConversionConfiguration.ProcessorSupported, Is.True, "ProcessorSupported");
				Assert.That (VTFrameRateConversionConfiguration.SupportedRevisions, Is.Not.Null, "SupportedRevisions");
				Assert.That (VTFrameRateConversionConfiguration.SupportedRevisions, Does.Contain (VTFrameRateConversionConfigurationRevision.Revision1), "SupportedRevisions.Contains");
				Assert.That (VTFrameRateConversionConfiguration.WeakSupportedRevisions, Is.Not.Null, "WeakSupportedRevisions");
				Assert.That (VTFrameRateConversionConfiguration.WeakSupportedRevisions, Does.Contain ((nuint) 1), "WeakSupportedRevisions.Contains");
				Assert.That (Enum.GetValues<VTFrameRateConversionConfigurationRevision> (), Does.Contain (VTFrameRateConversionConfiguration.DefaultRevision), "DefaultRevision");
			});
		}
	}
}

#endif // __MACOS__
