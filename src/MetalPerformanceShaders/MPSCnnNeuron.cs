#nullable enable

using Metal;

namespace MetalPerformanceShaders {
	public partial class MPSCnnNeuronPReLU {
		/// <summary>Initializes a new PReLU neuron with the specified device and alpha values.</summary>
		/// <param name="device">The Metal device to use for computation.</param>
		/// <param name="a">The per-feature-channel alpha values for the PReLU activation function.</param>
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[ObsoletedOSPlatform ("maccatalyst", "Please use the '.ctor (IMTLDevice, MPSNNNeuronDescriptor)' overload instead.")]
		[ObsoletedOSPlatform ("tvos12.0", "Please use the '.ctor (IMTLDevice, MPSNNNeuronDescriptor)' overload instead.")]
		[ObsoletedOSPlatform ("macos10.14", "Please use the '.ctor (IMTLDevice, MPSNNNeuronDescriptor)' overload instead.")]
		[ObsoletedOSPlatform ("ios12.0", "Please use the '.ctor (IMTLDevice, MPSNNNeuronDescriptor)' overload instead.")]
		public unsafe MPSCnnNeuronPReLU (IMTLDevice device, float [] a) : this (NSObjectFlag.Empty)
		{
			fixed (void* aHandle = a)
				InitializeHandle (InitWithDevice (device, (IntPtr) aHandle, (nuint) a.Length));
		}
	}
}
