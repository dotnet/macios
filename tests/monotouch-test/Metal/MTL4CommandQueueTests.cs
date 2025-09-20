using System;
using System.IO;
using System.Runtime.InteropServices;

using CoreFoundation;
using Foundation;
using ObjCRuntime;

using Metal;

using NUnit.Framework;

namespace MonoTouchFixtures.Metal {
	[Preserve (AllMembers = true)]
	public class MTL4CommandQueueTests {
		[Test]
		public void Commit ()
		{
			using var commandQ = Helper.CreateMTL4CommandQueue (out var device);
			using var allocator = device.CreateCommandAllocator ();
			using var commandBuffer = device.CreateCommandBuffer ();
			commandBuffer.BeginCommandBuffer (allocator);
			commandBuffer.EndCommandBuffer ();
			commandQ.Commit (commandBuffer);

			commandBuffer.BeginCommandBuffer (allocator);
			commandBuffer.EndCommandBuffer ();
			commandQ.Commit (new [] { commandBuffer });

			commandBuffer.BeginCommandBuffer (allocator);
			commandBuffer.EndCommandBuffer ();
			using var options = new MTL4CommitOptions ();
			commandQ.Commit (options, commandBuffer);

			commandBuffer.BeginCommandBuffer (allocator);
			commandBuffer.EndCommandBuffer ();
			commandQ.Commit (options, new [] { commandBuffer });
		}

		[Test]
		public void AddOrRemoveResidencySets ()
		{
			using var commandQ = Helper.CreateMTL4CommandQueue (out var device);
			using var residencySetDescriptor = new MTLResidencySetDescriptor () {
				Label = "Label",
				InitialCapacity = 3
			};
			using var residencySet = device.CreateResidencySet (residencySetDescriptor, out var error);
			Assert.IsNull (error, "Error #1");
			Assert.IsNotNull (residencySet, "ResidencySet #1");

			commandQ.AddResidencySets (residencySet);
			commandQ.RemoveResidencySets (residencySet);

			commandQ.AddResidencySets (new IMTLResidencySet [] { residencySet });
			commandQ.RemoveResidencySets (new IMTLResidencySet [] { residencySet });
		}

		[Test]
		public void UpdateAndCopyTextureMappings ()
		{
			using var commandQ = Helper.CreateMTL4CommandQueue (out var device);
			using var textureDescriptor = MTLTextureDescriptor.CreateTexture2DDescriptor (MTLPixelFormat.RGBA8Unorm, 40, 40, false);
			using var texture = device.CreateTexture (textureDescriptor);

			var mapping = new MTL4UpdateSparseTextureMappingOperation () {
				Mode = MTLSparseTextureMappingMode.Map,
			};
			commandQ.UpdateTextureMappings (texture, null, mapping);
			commandQ.UpdateTextureMappings (texture, null, new [] { mapping });

			using var texture2 = device.CreateTexture (textureDescriptor);
			var mappingOperation = new MTL4CopySparseTextureMappingOperation () {
				SourceRegion = new MTLRegion (new MTLOrigin (1, 2, 3), new MTLSize (10, 20, 30)),
			};
			commandQ.CopyTextureMappings (texture, texture2, mappingOperation);
			commandQ.CopyTextureMappings (texture, texture2, new [] { mappingOperation });
		}

		[Test]
		public void UpdateAndCopyBufferMappings ()
		{
			using var commandQ = Helper.CreateMTL4CommandQueue (out var device);
			using var buffer = device.CreateBuffer (1024, MTLResourceOptions.StorageModePrivate, MTLSparsePageSize.Size16);
			using var heapDescriptor = new MTLHeapDescriptor () {
				Size = 1024,
				StorageMode = MTLStorageMode.Private,
				Type = MTLHeapType.Placement,
				SparsePageSize = MTLSparsePageSize.Size16,
				MaxCompatiblePlacementSparsePageSize = MTLSparsePageSize.Size16,
			};
			using var heap = device.CreateHeap (heapDescriptor);

			var mapping = new MTL4UpdateSparseBufferMappingOperation () {
				Mode = MTLSparseTextureMappingMode.Map,
			};
			commandQ.UpdateBufferMappings (buffer, heap, mapping);
			commandQ.UpdateBufferMappings (buffer, heap, new [] { mapping });

			using var buffer2 = device.CreateBuffer (1024, MTLResourceOptions.StorageModePrivate, MTLSparsePageSize.Size16);
			var mappingOperation = new MTL4CopySparseBufferMappingOperation () {
				SourceRange = new NSRange (1, 2),
			};
			commandQ.CopyBufferMappings (buffer, buffer2, mappingOperation);
			commandQ.CopyBufferMappings (buffer, buffer2, new [] { mappingOperation });
		}
	}
}
