// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

#nullable enable

using Metal;

namespace MonoTouchFixtures.Metal {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class MTLRenderCommandEncoderTest {
		[Test]
		public void NullResources ()
		{
			var device = MTLDevice.SystemDefault;
			if (device is null)
				Assert.Inconclusive ("Metal is not supported");

			using var commandQueue = device.CreateCommandQueue ();
			if (commandQueue is null)
				Assert.Inconclusive ("Could not create a command queue.");

			using var commandBuffer = commandQueue.CommandBuffer ();
			if (commandBuffer is null)
				Assert.Inconclusive ("Could not create a command buffer.");

			using var textureDescriptor = MTLTextureDescriptor.CreateTexture2DDescriptor (MTLPixelFormat.RGBA8Unorm, 1, 1, false);
			textureDescriptor.Usage = MTLTextureUsage.RenderTarget;
			using var texture = device.CreateTexture (textureDescriptor);
			Assert.That (texture, Is.Not.Null, "Render target");
			using var renderPass = MTLRenderPassDescriptor.CreateRenderPassDescriptor ();
			renderPass.ColorAttachments [0].Texture = texture;
			renderPass.ColorAttachments [0].LoadAction = MTLLoadAction.Clear;
			renderPass.ColorAttachments [0].StoreAction = MTLStoreAction.DontCare;
			using var encoder = commandBuffer.CreateRenderCommandEncoder (renderPass) ?? throw new AssertionException ("Could not create a render command encoder.");

			try {
				Assert.DoesNotThrow (() => encoder.SetFragmentTexture (null, 0), "Fragment texture");
				Assert.DoesNotThrow (() => encoder.SetVertexTexture (null, 0), "Vertex texture");
				Assert.DoesNotThrow (() => encoder.SetFragmentBuffer (null, 0, 0), "Fragment buffer");
				Assert.DoesNotThrow (() => encoder.SetVertexBuffer (null, 0, 0), "Vertex buffer");
				Assert.DoesNotThrow (() => encoder.SetFragmentSamplerState (null, 0), "Fragment sampler");
				Assert.DoesNotThrow (() => encoder.SetVertexSamplerState (null, 0), "Vertex sampler");
				Assert.DoesNotThrow (() => encoder.SetFragmentSamplerState (null, 0, float.MaxValue, 0), "Fragment sampler with LOD clamps");
				Assert.DoesNotThrow (() => encoder.SetVertexSamplerState (null, 0, float.MaxValue, 0), "Vertex sampler with LOD clamps");
				Assert.DoesNotThrow (() => encoder.SetDepthStencilState (null), "Depth stencil state");
				Assert.Throws<ArgumentNullException> (() => encoder.SetRenderPipelineState (null), "Render pipeline state is required");
			} finally {
				encoder.EndEncoding ();
			}
		}
	}
}
