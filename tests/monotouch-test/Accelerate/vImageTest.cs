// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Accelerate;

namespace MonoTouchFixtures.Accelerate {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class vImageTest {
		[Test]
		public unsafe void ConvolvePreservesBufferDescriptors ()
		{
			byte [] sourceData = [1, 2, 3, 4];
			var destinationData = new byte [4];
			fixed (byte* sourcePointer = sourceData, destinationPointer = destinationData) {
				var source = new vImageBuffer {
					Data = (IntPtr) sourcePointer,
					Width = 2,
					Height = 2,
					BytesPerRow = 2,
				};
				var destination = new vImageBuffer {
					Data = (IntPtr) destinationPointer,
					Width = 2,
					Height = 2,
					BytesPerRow = 2,
				};
				var expectedSource = source;
				var expectedDestination = destination;
				short kernel = 1;
#if XAMCORE_5_0
				var status = vImage.ConvolvePlanar8 (in source, in destination, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, &kernel, 1, 1, 1, 0, vImageFlags.EdgeExtend);
#else
				var status = vImage.ConvolvePlanar8 (ref source, ref destination, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, &kernel, 1, 1, 1, 0, vImageFlags.EdgeExtend);
#endif
				Assert.That (status, Is.EqualTo (vImageError.NoError), "Convolve");
				Assert.That (source, Is.EqualTo (expectedSource), "Source descriptor");
				Assert.That (destination, Is.EqualTo (expectedDestination), "Destination descriptor");
			}
			Assert.That (destinationData, Is.EqualTo (new byte [] { 1, 2, 3, 4 }), "Output pixels");
			Assert.That (sourceData, Is.EqualTo (new byte [] { 1, 2, 3, 4 }), "Unchanged source pixels");
		}
	}
}
