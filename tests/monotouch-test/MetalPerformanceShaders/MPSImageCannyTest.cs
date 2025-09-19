// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

#if HAS_METALPERFORMANCESHADERS
using System;

using Foundation;
using Metal;
using MetalPerformanceShaders;

using NUnit.Framework;

namespace MonoTouchFixtures.MetalPerformanceShaders {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class MPSImageCannyTest {
		[Test]
		public void Create ()
		{
			TestRuntime.AssertXcodeVersion (16, 0);

			var device = MTLDevice.SystemDefault;
			if (device is null)
				Assert.Inconclusive ($"Metal does not exist on this device.");

			var transform = new float [] { 0.2f, 0.4f, 0.6f };
			using var kernel = new MPSImageCanny (device, transform, 0.8f);
			Assert.That (kernel.ColorTransform, Is.EqualTo (transform), "Transform");
			Assert.That (kernel.Sigma, Is.EqualTo (0.8f), "Sigma");
		}
	}
}
#endif // HAS_METALPERFORMANCESHADERS
