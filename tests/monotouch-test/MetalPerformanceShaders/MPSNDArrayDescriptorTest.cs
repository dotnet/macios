//
// Unit tests for MPSNDArrayDescriptor
//

#if HAS_METALPERFORMANCESHADERS

using CoreGraphics;
using Metal;
using MetalPerformanceShaders;

namespace MonoTouchFixtures.MetalPerformanceShaders {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class MPSNDArrayDescriptorTest {
		[Test]
		public void PermuteWithDimensionOrderTest ()
		{
			var device = MTLDevice.SystemDefault;
			if (device is null)
				Assert.Inconclusive ($"Metal does not exist on this device.");

			using var desc = MPSNDArrayDescriptor.Create (MPSDataType.Int32, new nuint [] { 10 });
			desc.PermuteWithDimensionOrder (new nuint [] { 0 });
			Assert.AreEqual (1, (int) desc.NumberOfDimensions, "NumberOfDimensions");
		}

		[Test]
		public void DimensionOrderTest ()
		{
			var device = MTLDevice.SystemDefault;
			if (device is null)
				Assert.Inconclusive ($"Metal does not exist on this device.");

			using var desc = MPSNDArrayDescriptor.Create (MPSDataType.Int32, new nuint [] { 10, 20, 30 });
			var expected = new NVector16b (0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15);
			Assert.That (desc.DimensionOrder, Is.EqualTo (expected), "DimensionOrder 1");

			desc.Transpose (0, 1);
			expected = new NVector16b (1, 0, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15);
			Assert.That (desc.DimensionOrder, Is.EqualTo (expected), "DimensionOrder 2");
		}
	}
}
#endif // HAS_METALPERFORMANCESHADERS
