//
// Unit tests for VTPixelRotationSession
//
// Authors:
//	Israel Soto <issoto@microsoft.com>
//	
//
// Copyright 2022 Microsoft Corporation.
//

#nullable enable

using System.Threading;

using VideoToolbox;
using CoreMedia;
using CoreVideo;
using AVFoundation;
using Xamarin.Utils;

namespace MonoTouchFixtures.VideoToolbox {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class VTPixelRotationSessionTests {
		[OneTimeSetUp]
		public void Init () => TestRuntime.AssertXcodeVersion (14, 0);

		[Test]
		public void CreateTest ()
		{
			using var session = VTPixelRotationSession.Create ();
			Assert.That (session, Is.Not.Null, "Session should not be null");
		}

		[Test]
		public void RotateImageTest ()
		{
			using var session = VTPixelRotationSession.Create ();
			using var sourcePixelBuffer = new CVPixelBuffer (640, 480, CVPixelFormatType.CV420YpCbCr8BiPlanarFullRange);
			using var destinationPixelBuffer = new CVPixelBuffer (480, 640, CVPixelFormatType.CV420YpCbCr8BiPlanarFullRange);

			var result = session.SetProperty (VTPixelRotationPropertyKeys.Rotation, VTRotation.ClockwiseNinety.GetConstant ());
			Assert.That (VTStatus.Ok, Is.EqualTo (result), "SetProperty");

			result = session.RotateImage (sourcePixelBuffer, destinationPixelBuffer);
			Assert.That (VTStatus.Ok, Is.EqualTo (result), "RotateImage");
		}

		[Test]
		public void SetRotationPropertiesTest ()
		{
			using var session = VTPixelRotationSession.Create ();
			var result = session.SetRotationProperties (new VTPixelRotationProperties {
				Rotation = VTRotation.ClockwiseNinety
			});

			Assert.That (VTStatus.Ok, Is.EqualTo (result), "SetRotationProperties");
		}
	}
}
