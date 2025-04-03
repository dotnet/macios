#nullable enable

using System;
using ObjCRuntime;

namespace MetalPerformanceShaders {
	public partial class MPSCnnConvolutionDescriptor {

#if NET
		/// <param name="mean">To be added.</param>
		///         <param name="variance">To be added.</param>
		///         <param name="gamma">To be added.</param>
		///         <param name="beta">To be added.</param>
		///         <param name="epsilon">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
#else
		[Introduced (PlatformName.TvOS, 11, 0, PlatformArchitecture.All, null)]
#endif
		public unsafe void SetBatchNormalizationParameters (float [] mean, float [] variance, float [] gamma, float [] beta, float epsilon)
		{
			fixed (void* meanHandle = mean)
			fixed (void* varianceHandle = variance)
			fixed (void* gammaHandle = gamma)
			fixed (void* betaHandle = beta)
				SetBatchNormalizationParameters ((IntPtr) meanHandle, (IntPtr) varianceHandle, (IntPtr) gammaHandle, (IntPtr) betaHandle, epsilon);
		}

	}
}
