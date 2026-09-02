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
			Assert.That (pbp.CreatePixelBuffer (a, out error), Is.Not.Null, "#1");
			Assert.That (pbp.CreatePixelBuffer (a, out error), Is.Not.Null, "#2");
			Assert.That (pbp.CreatePixelBuffer (a, out error), Is.Null, "#3");
			Assert.That (error, Is.EqualTo (CVReturn.WouldExceedAllocationThreshold), "#3a");
		}
	}
}
