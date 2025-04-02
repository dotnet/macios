#nullable enable

using System;
using Metal;
using Foundation;

namespace MetalPerformanceShaders {
	public partial class MPSCnnBinaryConvolution {
		/// <param name="device">To be added.</param>
		///         <param name="convolutionData">To be added.</param>
		///         <param name="outputBiasTerms">To be added.</param>
		///         <param name="outputScaleTerms">To be added.</param>
		///         <param name="inputBiasTerms">To be added.</param>
		///         <param name="inputScaleTerms">To be added.</param>
		///         <param name="type">To be added.</param>
		///         <param name="flags">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public unsafe MPSCnnBinaryConvolution (IMTLDevice device, IMPSCnnConvolutionDataSource convolutionData, float [] outputBiasTerms, float [] outputScaleTerms, float [] inputBiasTerms, float [] inputScaleTerms, MPSCnnBinaryConvolutionType type, MPSCnnBinaryConvolutionFlags flags)
			: base (NSObjectFlag.Empty)
		{
			fixed (void* outputBiasTermsHandle = outputBiasTerms)
			fixed (void* outputScaleTermsHandle = outputScaleTerms)
			fixed (void* inputBiasTermsHandle = inputBiasTerms)
			fixed (void* inputScaleTermsHandle = inputScaleTerms)
				InitializeHandle (InitWithDevice (device, convolutionData, (IntPtr) outputBiasTermsHandle, (IntPtr) outputScaleTermsHandle, (IntPtr) inputBiasTermsHandle, (IntPtr) inputScaleTermsHandle, type, flags));
		}
	}

	public partial class MPSCnnBinaryFullyConnected {
		/// <param name="device">To be added.</param>
		///         <param name="convolutionData">To be added.</param>
		///         <param name="outputBiasTerms">To be added.</param>
		///         <param name="outputScaleTerms">To be added.</param>
		///         <param name="inputBiasTerms">To be added.</param>
		///         <param name="inputScaleTerms">To be added.</param>
		///         <param name="type">To be added.</param>
		///         <param name="flags">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public unsafe MPSCnnBinaryFullyConnected (IMTLDevice device, IMPSCnnConvolutionDataSource convolutionData, float [] outputBiasTerms, float [] outputScaleTerms, float [] inputBiasTerms, float [] inputScaleTerms, MPSCnnBinaryConvolutionType type, MPSCnnBinaryConvolutionFlags flags)
			: base (NSObjectFlag.Empty)
		{
			fixed (void* outputBiasTermsHandle = outputBiasTerms)
			fixed (void* outputScaleTermsHandle = outputScaleTerms)
			fixed (void* inputBiasTermsHandle = inputBiasTerms)
			fixed (void* inputScaleTermsHandle = inputScaleTerms)
				InitializeHandle (InitWithDevice (device, convolutionData, (IntPtr) outputBiasTermsHandle, (IntPtr) outputScaleTermsHandle, (IntPtr) inputBiasTermsHandle, (IntPtr) inputScaleTermsHandle, type, flags));
		}
	}
}
