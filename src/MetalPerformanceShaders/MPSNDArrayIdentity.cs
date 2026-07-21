#nullable enable

using Metal;

namespace MetalPerformanceShaders {
	public partial class MPSNDArrayIdentity {
		public MPSNDArray? Reshape (IMTLCommandBuffer? commandBuffer, MPSNDArray sourceArray, nuint [] dimensionSizes, MPSNDArray? destinationArray)
		{
			MPSNDArray? rv;
			unsafe {
				fixed (nuint* dimensionsPtr = dimensionSizes) {
					rv = _Reshape (commandBuffer, sourceArray, (nuint) dimensionSizes.Length, (IntPtr) dimensionsPtr, destinationArray);
				}
			}
			return rv;
		}

		public MPSNDArray? Reshape (IMTLComputeCommandEncoder? encoder, IMTLCommandBuffer? commandBuffer, MPSNDArray sourceArray, nuint [] dimensionSizes, MPSNDArray? destinationArray)
		{
			MPSNDArray? rv;
			unsafe {
				fixed (nuint* dimensionsPtr = dimensionSizes) {
					rv = _Reshape (encoder, commandBuffer, sourceArray, (nuint) dimensionSizes.Length, (IntPtr) dimensionsPtr, destinationArray);
				}
			}
			return rv;
		}

		/// <summary>Encodes a reshape operation with a Metal 4 compute command encoder.</summary>
		/// <param name="encoder">The Metal 4 compute command encoder.</param>
		/// <param name="sourceArray">The source array.</param>
		/// <param name="dimensionSizes">The extents of each dimension in the destination array.</param>
		/// <param name="destinationArray">The destination array, whose shape must match <paramref name="dimensionSizes" />.</param>
		/// <remarks>The encoder associates the command with <see cref="MTLStages.Dispatch" />. Synchronize dependent workloads against that stage to prevent race conditions.</remarks>
		[SupportedOSPlatform ("ios27.0")]
		[SupportedOSPlatform ("maccatalyst27.0")]
		[SupportedOSPlatform ("macos27.0")]
		[SupportedOSPlatform ("tvos27.0")]
		public void ReshapeWithMtl4CommandEncoder (IMTL4ComputeCommandEncoder encoder, MPSNDArray sourceArray, nuint [] dimensionSizes, MPSNDArray destinationArray)
		{
			if (dimensionSizes is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (dimensionSizes));

			unsafe {
				fixed (nuint* dimensionsPtr = dimensionSizes) {
					_ReshapeWithMtl4CommandEncoder (encoder, sourceArray, (nuint) dimensionSizes.Length, (IntPtr) dimensionsPtr, destinationArray);
				}
			}
		}
	}
}
