//
// Unit tests for VTRawProcessingSession
//

#if MONOMAC

using System.Drawing;

using AVFoundation;
using CoreGraphics;
using CoreMedia;
using CoreVideo;
using VideoToolbox;
#if MONOMAC
using AppKit;
#else
using UIKit;
#endif

using Xamarin.Utils;

namespace MonoTouchFixtures.VideoToolbox {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class VTRawProcessingSessionTest {
		[Test]
		public void Create_NSDictionary_Test ()
		{
			TestRuntime.AssertXcodeVersion (16, 0);

			using var pixelBuffer = new CVPixelBuffer (720, 480, CVPixelFormatType.CV422YpCbCr8BiPlanarFullRange);
			using var desc = CMVideoFormatDescription.CreateForImageBuffer (pixelBuffer, out var fde);
			Assert.That (desc, Is.Not.Null, "Desc");
			Assert.That (fde, Is.EqualTo (CMFormatDescriptionError.None), "vdf error");
			using var session = VTRawProcessingSession.Create (desc, (NSDictionary) null, (NSDictionary) null, out var vtStatus);
			// I have not been able to figure out what kind of CMVideoFormatDescription is needed to create a VTRawProcessingSession.
			Assert.That (vtStatus, Is.EqualTo (VTStatus.CouldNotCreateInstance), "status");
		}

		[Test]
		public void Create_CVPixelBufferAttributes_Test ()
		{
			TestRuntime.AssertXcodeVersion (16, 0);

			using var pixelBuffer = new CVPixelBuffer (480, 360, CVPixelFormatType.CV422YpCbCr8BiPlanarFullRange);
			using var desc = CMVideoFormatDescription.CreateForImageBuffer (pixelBuffer, out var fde);
			Assert.That (desc, Is.Not.Null, "Desc");
			Assert.That (fde, Is.EqualTo (CMFormatDescriptionError.None), "vdf error");
			using var session = VTRawProcessingSession.Create (desc, (CVPixelBufferAttributes) null, (VTRawProcessingParameters) null, out var vtStatus);
			// I have not been able to figure out what kind of CMVideoFormatDescription is needed to create a VTRawProcessingSession.
			Assert.That (vtStatus, Is.EqualTo (VTStatus.CouldNotCreateInstance), "status");
		}

		[Test]
		public void GetTypeId ()
		{
			TestRuntime.AssertXcodeVersion (16, 0);

			Assert.That (VTRawProcessingSession.GetTypeId (), Is.Not.EqualTo (0), "GetTypeId");
		}

		[Test]
		public void ProcessingTest ()
		{
			TestRuntime.AssertXcodeVersion (16, 0);

			using var pixelBuffer = new CVPixelBuffer (20, 10, CVPixelFormatType.CV24RGB);
			using var desc = CMVideoFormatDescription.CreateForImageBuffer (pixelBuffer, out var fde);
			Assert.That (desc, Is.Not.Null, "Desc");
			Assert.That (fde, Is.EqualTo (CMFormatDescriptionError.None), "vdf error");
			using var session = VTRawProcessingSession.Create (desc, (CVPixelBufferAttributes) null, (VTRawProcessingParameters) null, out var vtStatus);
			Assert.That (vtStatus, Is.EqualTo (VTStatus.CouldNotCreateInstance), "Create status");

			// I have not been able to figure out what kind of CMVideoFormatDescription
			// is needed to successfully create a VTRawProcessingSession,
			// so the rest of this test has been commented out.

			// session.SetParameterChangedHandler ((NSObject []? newParameters) => {
			// 	Console.WriteLine ($"ParameterChanged: {newParameters}");
			// });

			// var parameters = session.CopyProcessingParameters (out vtStatus);
			// Assert.That (vtStatus, Is.EqualTo (VTStatus.Ok), "CopyProcessingParameters status");
			// Assert.That (parameters, Is.Not.Null, "Parameters");
			// Console.WriteLine (parameters);
			// var prms = session.ProcessingParameters;
			// Assert.That (prms, Is.Not.Null, "ProcessingParameters");
			// Console.WriteLine (prms);

			// vtStatus = session.SetProcessingParameters (new NSDictionary ());
			// Assert.That (vtStatus, Is.EqualTo (VTStatus.Ok), "SetProcessingParameters status");

			// vtStatus = session.SetProcessingParameters (new VTRawProcessingParameters ());
			// Assert.That (vtStatus, Is.EqualTo (VTStatus.Ok), "SetProcessingParameters status (VTRawProcessingParameter)");

			// session.ProcessFrame (pixelBuffer, (NSDictionary) null, (VTStatus status, CVPixelBuffer? processedPixelBuffer) =>
			// {
			// 	Console.WriteLine ($"ProcessFrameCallback, status: {status} pb: {processedPixelBuffer}");
			// });

			// session.CompleteFrames ();

			// session.Invalidate ();
		}
	}
}

#endif // MONOMAC
