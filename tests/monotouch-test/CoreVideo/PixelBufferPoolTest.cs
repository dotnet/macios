//
// Unit tests for CVPixelBufferPool
//
// Authors:
//	Marek Safar (marek.safar@gmail.com)
//
// Copyright 2012 Xamarin Inc. All rights reserved.
//

using System.Drawing;
using CoreVideo;

namespace MonoTouchFixtures.CoreVideo {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class PixelBufferPoolTest {
		[Test]
		public void AllocationSettings_Threshold ()
		{
			var pbp = new CVPixelBufferPool (
				new CVPixelBufferPoolSettings (),
				new CVPixelBufferAttributes (CVPixelFormatType.CV24RGB, 100, 50)
			);

			var a = new CVPixelBufferPoolAllocationSettings () {
				Threshold = 2
			};

			CVReturn error;
			ClassicAssert.IsNotNull (pbp.CreatePixelBuffer (a, out error), "#1");
			ClassicAssert.IsNotNull (pbp.CreatePixelBuffer (a, out error), "#2");
			ClassicAssert.IsNull (pbp.CreatePixelBuffer (a, out error), "#3");
			ClassicAssert.AreEqual (CVReturn.WouldExceedAllocationThreshold, error, "#3a");
		}
	}
}
