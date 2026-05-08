// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.Numerics;

using Foundation;
using Metal;
using MetalKit;
using ModelIO;

using NUnit.Framework;

using Vector3i = global::CoreGraphics.NVector3i;

namespace MonoTouchFixtures.MetalKit {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class MTKMeshTest {

		[Test]
		public void FromAsset ()
		{
			var device = MTLDevice.SystemDefault;
			if (device is null)
				Assert.Inconclusive ("Metal is not supported on this device.");

			using var allocator = new MTKMeshBufferAllocator (device);
			using var mesh = MDLMesh.CreateBox (new Vector3 (1, 1, 1), new Vector3i (1, 1, 1), MDLGeometryType.Triangles, false, allocator);
			using var asset = new MDLAsset ();
			asset.AddObject (mesh);

			var result = MTKMesh.FromAsset (asset, device, out var sourceMeshes, out var error);

			Assert.That (error, Is.Null, "error");
			Assert.That (result, Is.Not.Null, "result");
			Assert.That (result!.Length, Is.GreaterThan (0), "result length");
			Assert.That (sourceMeshes, Is.Not.Null, "sourceMeshes");
			Assert.That (sourceMeshes!.Length, Is.EqualTo (result.Length), "sourceMeshes length");
		}
	}
}
