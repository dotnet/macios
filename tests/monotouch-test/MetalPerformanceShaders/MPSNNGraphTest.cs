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
	public class MPSNNGraphTest {
		[Test]
		public void Ctor ()
		{
			TestRuntime.AssertXcodeVersion (16, 0);

			var device = MTLDevice.SystemDefault;
			if (device is null)
				Assert.Inconclusive ($"Metal does not exist on this device.");

			var nodes = new MPSNNImageNode [] { MPSNNImageNode.Create (null) };
			using var graph1 = new MPSNNGraph (device, nodes, null);
			Assert.That (graph1, Is.Not.Null, "Graph 1");

			using var graph2 = new MPSNNGraph (device, nodes, new bool [] { true });
			Assert.That (graph2, Is.Not.Null, "Graph 2");

			using var array3 = new MPSNNGraph (device, nodes, new bool [] { true, false });
			Assert.That (array3, Is.Not.Null, "Graph 3");

			Assert.Throws<ArgumentOutOfRangeException> (() => new MPSNNGraph (device, nodes, new bool [0]), "AOORE 1");
		}

		[Test]
		public void Create ()
		{
			TestRuntime.AssertXcodeVersion (16, 0);

			var device = MTLDevice.SystemDefault;
			if (device is null)
				Assert.Inconclusive ($"Metal does not exist on this device.");

			var nodes = new MPSNNImageNode [] { MPSNNImageNode.Create (null) };
			using var graph1 = MPSNNGraph.Create (device, nodes, null);
			Assert.That (graph1, Is.Not.Null, "Graph 1");

			using var graph2 = MPSNNGraph.Create (device, nodes, new bool [] { true });
			Assert.That (graph2, Is.Not.Null, "Graph 2");

			using var array3 = MPSNNGraph.Create (device, nodes, new bool [] { true, false });
			Assert.That (array3, Is.Not.Null, "Graph 3");

			Assert.Throws<ArgumentOutOfRangeException> (() => MPSNNGraph.Create (device, nodes, new bool [0]), "AOORE 1");
		}
	}
}
#endif // HAS_METALPERFORMANCESHADERS
