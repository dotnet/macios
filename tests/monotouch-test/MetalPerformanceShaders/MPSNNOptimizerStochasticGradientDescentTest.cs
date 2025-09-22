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
	public class MPSNNOptimizerStochasticGradientDescentTest {
		[Test]
		public void Create ()
		{
			TestRuntime.AssertXcodeVersion (16, 0);

			var device = MTLDevice.SystemDefault;
			if (device is null)
				Assert.Inconclusive ($"Metal does not exist on this device.");

			using var optimizer = new MPSNNOptimizerStochasticGradientDescent (device, 0.4f, false, new MPSNNOptimizerDescriptor (0.1f, 0.2f, MPSNNRegularizationType.L1, 0.3f));
			Assert.That (optimizer, Is.Not.Null, "Optimizer");
			Assert.That (optimizer.MomentumScale, Is.EqualTo (0.4f), "MomentumScale");
			Assert.That (optimizer.UseNestrovMomentum, Is.EqualTo (false), "UseNestrovMomentum");
			Assert.That (optimizer.UseNesterovMomentum, Is.EqualTo (false), "UseNesterovMomentum");

			using var optimizer2 = new MPSNNOptimizerStochasticGradientDescent (device, 0.2f, true, new MPSNNOptimizerDescriptor (0.1f, 0.2f, MPSNNRegularizationType.L1, 0.3f));
			Assert.That (optimizer2, Is.Not.Null, "Optimizer");
			Assert.That (optimizer2.MomentumScale, Is.EqualTo (0.2f), "MomentumScale");
			Assert.That (optimizer2.UseNestrovMomentum, Is.EqualTo (true), "UseNestrovMomentum");
			Assert.That (optimizer2.UseNesterovMomentum, Is.EqualTo (true), "UseNesterovMomentum");
		}
	}
}
#endif // HAS_METALPERFORMANCESHADERS
