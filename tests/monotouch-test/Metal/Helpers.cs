using Metal;

using NUnit.Framework;

namespace MonoTouchFixtures.Metal {
	static class Helper {
		public static void AssertMetal4Available (out IMTLDevice device)
		{
			TestRuntime.AssertXcodeVersion (26, 0);

			device = MTLDevice.SystemDefault;
			// some older hardware won't have a default
			if (device is null)
				Assert.Inconclusive ("Metal is not supported");

			var metal4 = device.SupportsFamily (MTLGpuFamily.Metal4);
			if (!metal4)
				Assert.Inconclusive ("Metal 4 is not supported on this device.");
		}

		public static IMTL4CommandBuffer CreateMTL4CommandBuffer (out IMTLDevice device)
		{
			AssertMetal4Available (out device);

			var commandBuffer = device.CreateCommandBuffer ();
			if (commandBuffer is null)
				Assert.Inconclusive ("Could not create a command buffer for the metal device.");
			return commandBuffer;
		}

		public static IMTL4CommandQueue CreateMTL4CommandQueue (out IMTLDevice device)
		{
			AssertMetal4Available (out device);

			var commandQueue = device.CreateMTL4CommandQueue ();
			if (commandQueue is null)
				Assert.Inconclusive ("Could not create a command queue for the metal device.");
			return commandQueue;
		}
	}
}
