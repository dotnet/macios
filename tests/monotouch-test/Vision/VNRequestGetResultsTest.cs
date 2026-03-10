// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

//
// Unit tests for VNRequest.GetResults<T>
//

using CoreGraphics;
using Vision;

namespace MonoTouchFixtures.Vision {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class VNRequestGetResultsTest {

		[Test]
		public void GetResults_BeforePerform_ReturnsNull ()
		{
			TestRuntime.AssertXcodeVersion (11, 0);

			using var request = new VNDetectFaceRectanglesRequest ((request, error) => { });
			var results = request.GetResults<VNFaceObservation> ();
			Assert.IsNull (results, "GetResults/before-perform");
		}

		[Test]
		public void GetResults_AfterPerform ()
		{
			TestRuntime.AssertXcodeVersion (11, 0);

			using var request = new VNDetectRectanglesRequest ((request, error) => { });
			// Create a simple 100x100 white image
			var colorSpace = CGColorSpace.CreateDeviceRGB ();
			using var context = new CGBitmapContext (null, 100, 100, 8, 400, colorSpace, CGImageAlphaInfo.PremultipliedLast);
			context.SetFillColor (new CGColor (1, 1, 1));
			context.FillRect (new CGRect (0, 0, 100, 100));
			using var image = context.ToImage ()!;

			using var handler = new VNImageRequestHandler (image, new NSDictionary ());
			handler.Perform (new VNRequest [] { request }, out var error);

			// Results may be empty but should not be null after performing
			var results = request.GetResults<VNRectangleObservation> ();
			Assert.IsNotNull (results, "GetResults/after-perform");
		}
	}
}
