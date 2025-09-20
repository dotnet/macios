using System;
using System.IO;
using System.Runtime.InteropServices;

using CoreFoundation;
using CoreGraphics;
using Foundation;
using MetalKit;
using ObjCRuntime;

using Metal;

using NUnit.Framework;

namespace MonoTouchFixtures.Metal {
	[Preserve (AllMembers = true)]
	public class MTL4RenderCommandEncoderTests {
		[Test]
		public void Viewports ()
		{
			using var commandBuffer = Helper.CreateMTL4CommandBuffer (out var device);
			using var allocator = device.CreateCommandAllocator ();
			using var view = new MTKView (new CGRect (0, 0, 64, 64), device);
			// 'currentMTL4RenderPassDescriptor' isn't bound yet, this can be changed once that's one
			using var descriptor = (MTL4RenderPassDescriptor) view.PerformSelector (new Selector ("currentMTL4RenderPassDescriptor"));
			commandBuffer.BeginCommandBuffer (allocator);
			using var renderCommandEncoder = commandBuffer.CreateRenderCommandEncoder (descriptor);

			var viewport = new MTLViewport (1, 2, 3, 4, 5, 6);
			renderCommandEncoder.SetViewports (viewport);
			renderCommandEncoder.SetViewports (new [] { viewport });

			var viewMapping = new MTLVertexAmplificationViewMapping () {
				ViewportArrayIndexOffset = 1,
				RenderTargetArrayIndexOffset = 2,
			};
			renderCommandEncoder.SetVertexAmplificationCount (viewMapping);
			renderCommandEncoder.SetVertexAmplificationCount (new [] { viewMapping });

			var scissorRect = new MTLScissorRect (1, 2, 3, 4);
			renderCommandEncoder.SetScissorRects (scissorRect);
			renderCommandEncoder.SetScissorRects (new [] { scissorRect });
			commandBuffer.EndCommandBuffer ();
		}
	}
}
