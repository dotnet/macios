//
// Unit tests for MPSNDArrayIdentity
//

#if HAS_METALPERFORMANCESHADERS

using Metal;
using MetalPerformanceShaders;

namespace MonoTouchFixtures.MetalPerformanceShaders {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class MPSNDArrayIdentityTest {
		[Test]
		public void ReshapeA ()
		{
			TestRuntime.AssertXcodeVersion (16, 0);

			var device = MTLDevice.SystemDefault;
			if (device is null)
				Assert.Inconclusive ($"Metal does not exist on this device.");

			using var identity = new MPSNDArrayIdentity (device);
			using var sourceArray = new MPSNDArray (device, 3.14f);
			using var newArray = identity.Reshape (null, sourceArray, new nuint [] { 1 }, null);
			Assert.That ((int) newArray.NumberOfDimensions, Is.EqualTo (1), "NumberOfDimensions");
			Assert.That ((int) newArray.GetLength (0), Is.EqualTo (1), "Length #0");
		}

		[Test]
		public void ReshapeB ()
		{
			TestRuntime.AssertXcodeVersion (16, 0);

			var device = MTLDevice.SystemDefault;
			if (device is null)
				Assert.Inconclusive ($"Metal does not exist on this device.");

			using var identity = new MPSNDArrayIdentity (device);
			using var sourceArray = new MPSNDArray (device, 3.14f);
			using var newArray = identity.Reshape (null, null, sourceArray, new nuint [] { 1 }, null);
			Assert.That ((int) newArray.NumberOfDimensions, Is.EqualTo (1), "NumberOfDimensions");
			Assert.That ((int) newArray.GetLength (0), Is.EqualTo (1), "Length #0");
		}

		[Test]
		public void DataTypes ()
		{
			TestRuntime.AssertXcodeVersion (27, 0);

			Assert.That ((uint) MPSDataType.Float4E2M1, Is.EqualTo (270598148u), nameof (MPSDataType.Float4E2M1));
			Assert.That ((uint) MPSDataType.Float8E4M3, Is.EqualTo (272826376u), nameof (MPSDataType.Float8E4M3));
			Assert.That ((uint) MPSDataType.Float8E5M2, Is.EqualTo (273809416u), nameof (MPSDataType.Float8E5M2));
			Assert.That ((uint) MPSDataType.Float8E8M0, Is.EqualTo (276824072u), nameof (MPSDataType.Float8E8M0));
		}

		[Test]
		public void ReshapeSourceArray ()
		{
			TestRuntime.AssertXcodeVersion (27, 0);
			TestRuntime.AssertSystemVersion (TestRuntime.CurrentPlatform, 27, 0);

			var device = MTLDevice.SystemDefault;
			if (device is null)
				Assert.Inconclusive ($"Metal does not exist on this device.");

			using var identity = new MPSNDArrayIdentity (device);
			using var sourceArray = new MPSNDArray (device, 3.14f);
			using var newArray = identity.Reshape (sourceArray, new int [] { 1 });
			Assert.That (newArray, Is.Not.Null, "Array");
			Assert.That ((int) newArray.NumberOfDimensions, Is.EqualTo (1), "NumberOfDimensions");
			Assert.That ((int) newArray.GetLength (0), Is.EqualTo (1), "Length #0");
		}

		[Test]
		public void ReshapeWithMtl4CommandEncoder ()
		{
			TestRuntime.AssertXcodeVersion (27, 0);
			TestRuntime.AssertSystemVersion (TestRuntime.CurrentPlatform, 27, 0);

			if (TestRuntime.IsSimulator)
				Assert.Ignore ("Metal 4 command encoders are not available in the simulator.");

			var device = MTLDevice.SystemDefault;
			if (device is null)
				Assert.Inconclusive ($"Metal does not exist on this device.");
			if (!device.SupportsFamily (MTLGpuFamily.Metal4))
				Assert.Inconclusive ("Metal 4 is not supported on this device.");

			using var commandBuffer = device.CreateCommandBuffer ();
			if (commandBuffer is null)
				Assert.Inconclusive ("Could not create a Metal 4 command buffer.");

			using var allocator = device.CreateCommandAllocator ();
			if (allocator is null)
				Assert.Inconclusive ("Could not create a Metal 4 command allocator.");

			using var identity = new MPSNDArrayIdentity (device);
			using var sourceArray = new MPSNDArray (device, 3.14f);
			using var descriptor = MPSNDArrayDescriptor.Create (MPSDataType.Float32, new nuint [] { 1 });
			using var destinationArray = new MPSNDArray (device, descriptor);

			commandBuffer.BeginCommandBuffer (allocator);
			using var encoder = commandBuffer.CreateComputeCommandEncoder ();
			if (encoder is null)
				Assert.Inconclusive ("Could not create a Metal 4 compute command encoder.");

			identity.ReshapeWithMtl4CommandEncoder (encoder, sourceArray, new int [] { 1 }, destinationArray);
			identity.ReshapeWithMtl4CommandEncoder (encoder, sourceArray, new nuint [] { 1 }, destinationArray);
			encoder.EndEncoding ();
			commandBuffer.EndCommandBuffer ();
		}
	}
}
#endif // HAS_METALPERFORMANCESHADERS
