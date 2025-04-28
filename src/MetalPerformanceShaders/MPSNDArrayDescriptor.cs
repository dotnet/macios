#nullable enable

using System;

using Metal;
using Foundation;
using ObjCRuntime;

namespace MetalPerformanceShaders {
	public partial class MPSNDArrayDescriptor {
		[SupportedOSPlatform ("ios18.0")]
		[SupportedOSPlatform ("maccatalyst18.0")]
		[SupportedOSPlatform ("macos15.0")]
		[SupportedOSPlatform ("tvos18.0")]
		public void PermuteWithDimensionOrder (nuint [] dimensionOrder)
		{
			if (dimensionOrder is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (dimensionOrder));

			if (dimensionOrder.Length != (int) NumberOfDimensions)
				ObjCRuntime.ThrowHelper.ThrowArgumentOutOfRangeException (nameof (dimensionOrder), $"Length must be equal to 'NumberOfDimensions'.");

			unsafe {
				fixed (nuint* ptr = dimensionOrder) {
					_PermuteWithDimensionOrder ((IntPtr) ptr);
				}
			}
		}
	}
}
