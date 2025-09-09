// Copyright 2015-2016 Xamarin Inc. All rights reserved.

#nullable enable

using System;
using System.Runtime.InteropServices;
using CoreGraphics;
using Foundation;
using Metal;
using ObjCRuntime;

namespace MetalPerformanceShaders {
	public partial class MPSKernel : NSObject {

#if !COREBUILD
		[DllImport (Constants.MetalPerformanceShadersLibrary)]
		extern static byte MPSSupportsMTLDevice (/* __nullable id <MTLDevice> */ IntPtr device);

		/// <param name="device">To be added.</param>
		///         <summary>Determines if the device is supported.</summary>
		///         <returns>
		///           <see langword="true" /> if <paramref name="device" /> is supported. Oterwise, returns <see langword="false" /></returns>
		///         <remarks>
		///           <para>Before copying shaders to a new device, application developers should call the <see cref="MetalPerformanceShaders.MPSKernel.Supports(Metal.IMTLDevice)" /> method to determine if the <paramref name="device" /> is supported.</para>
		///         </remarks>
		public static bool Supports (IMTLDevice device)
		{
			bool result = MPSSupportsMTLDevice (device.GetHandle ()) != 0;
			GC.KeepAlive (device);
			return result;
		}

		[SupportedOSPlatform ("tvos13.0")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios13.0")]
		[SupportedOSPlatform ("maccatalyst")]
		[DllImport (Constants.MetalPerformanceShadersLibrary)]
		static extern /* id<MTLDevice> _Nullable */ IntPtr MPSGetPreferredDevice (nuint options);

		[SupportedOSPlatform ("tvos13.0")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios13.0")]
		[SupportedOSPlatform ("maccatalyst")]
		public static IMTLDevice? GetPreferredDevice (MPSDeviceOptions options)
		{
			var h = MPSGetPreferredDevice ((nuint) (ulong) options);
			return Runtime.GetINativeObject<IMTLDevice> (h, false);
		}

		internal unsafe static float []? GetTransform (IntPtr transform)
		{
			var t = (float*) transform;
			if (t is null)
				return null;
			return new float [3] { t [0], t [1], t [2] };
		}

		/// <summary>Gets a region that represents the default clipping rectangle.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MPSRectNoClip", "MetalPerformanceShaders")]
		public unsafe static MTLRegion RectNoClip {
			get {
				var p = Dlfcn.dlsym (Libraries.MetalPerformanceShaders.Handle, "MPSRectNoClip");
				if (p == IntPtr.Zero)
					return new MTLRegion ();
				unsafe {
					nint* ptr = (nint*) p;
					return MTLRegion.Create3D (ptr [0], ptr [1], ptr [2], ptr [3], ptr [4], ptr [5]);
				}
			}
		}

		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[DllImport (Constants.MetalPerformanceShadersLibrary)]
		static extern void MPSHintTemporaryMemoryHighWaterMark (IntPtr commandBuffer, nuint bytes);

		/// <param name="commandBuffer">To be added.</param>
		/// <param name="sizeInBytes">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		public static void HintTemporaryMemoryHighWaterMark (IMTLCommandBuffer commandBuffer, nuint sizeInBytes)
		{
			MPSHintTemporaryMemoryHighWaterMark (commandBuffer.GetHandle (), sizeInBytes);
			GC.KeepAlive (commandBuffer);
		}

		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[DllImport (Constants.MetalPerformanceShadersLibrary)]
		static extern void MPSSetHeapCacheDuration (IntPtr commandBuffer, double seconds);

		/// <param name="commandBuffer">To be added.</param>
		///         <param name="seconds">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		public static void SetHeapCacheDuration (IMTLCommandBuffer commandBuffer, double seconds)
		{
			MPSSetHeapCacheDuration (commandBuffer.GetHandle (), seconds);
			GC.KeepAlive (commandBuffer);
		}
#endif // !COREBUILD
	}

#if !COREBUILD
	public partial class MPSImage {
		[SupportedOSPlatform ("ios13.0")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[DllImport (Constants.MetalPerformanceShadersLibrary)]
		static extern MPSImageType MPSGetImageType (IntPtr image);

		[SupportedOSPlatform ("ios13.0")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		public MPSImageType ImageType => MPSGetImageType (Handle);
	}

	public partial class MPSImageDilate {

		/// <param name="device">To be added.</param>
		/// <param name="kernelWidth">To be added.</param>
		/// <param name="kernelHeight">To be added.</param>
		/// <param name="values">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[DesignatedInitializer]
		public MPSImageDilate (IMTLDevice device, nuint kernelWidth, nuint kernelHeight, float [] values)
			: base (NSObjectFlag.Empty)
		{
			if (values is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (values));

			unsafe {
				fixed (float* ptr = values)
					InitializeHandle (InitWithDevice (device, kernelWidth, kernelHeight, (IntPtr) ptr), "initWithDevice:kernelWidth:kernelHeight:values:");
			}
		}
	}

	public partial class MPSImageErode : MPSImageDilate {

		/// <param name="device">To be added.</param>
		/// <param name="kernelWidth">To be added.</param>
		/// <param name="kernelHeight">To be added.</param>
		/// <param name="values">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		public MPSImageErode (IMTLDevice device, nuint kernelWidth, nuint kernelHeight, float [] values)
			: base (device, kernelWidth, kernelHeight, values)
		{
		}
	}

	public partial class MPSImageThresholdBinary {

		/// <param name="device">The device on which the filter will run.</param>
		///         <param name="thresholdValue">The value above which pixels will be brightened to the maximum value.</param>
		///         <param name="maximumValue">The value to which to brighten pixels brighter than the threshold.</param>
		///         <param name="transform">A color transform that maps 3-channel pixels to single-channel values.</param>
		///         <summary>Constructs a new <c>MPSImageThresholdBinary</c> with the specified values.</summary>
		///         <remarks>To be added.</remarks>
		public MPSImageThresholdBinary (IMTLDevice device, float thresholdValue, float maximumValue, /*[NullAllowed]*/ float [] transform)
			: base (NSObjectFlag.Empty)
		{
			// From: https://github.com/dotnet/csharplang/blob/master/spec/unsafe-code.md#unsafe-code
			// If the array expression is null or if the array has zero elements, the initializer computes an address equal to zero.
			unsafe {
				fixed (float* ptr = transform)
					InitializeHandle (InitWithDevice (device, thresholdValue, maximumValue, (IntPtr) ptr), "initWithDevice:thresholdValue:maximumValue:linearGrayColorTransform:");
			}
		}

		/// <summary>Gets the color transform that maps 3-channel pixels to single-channel values.</summary>
		///         <value>The color transform that maps 3-channel pixels to single-channel values.</value>
		///         <remarks>To be added.</remarks>
		public float []? Transform {
			get { return MPSKernel.GetTransform (_Transform); }
		}
	}

	public partial class MPSImageThresholdBinaryInverse {

		/// <param name="device">The device on which the filter will run.</param>
		///         <param name="thresholdValue">The value above which pixels will be darkened to 0 brightness.</param>
		///         <param name="maximumValue">The value to which to brighten pixels that are dimmer than the threshold.</param>
		///         <param name="transform">A color transform that maps 3-channel pixels to single-channel values.</param>
		///         <summary>Constructs a new <c>MPSImageThresholdBinaryInverse</c> with the specified values.</summary>
		///         <remarks>To be added.</remarks>
		[DesignatedInitializer]
		public MPSImageThresholdBinaryInverse (IMTLDevice device, float thresholdValue, float maximumValue, /*[NullAllowed]*/ float [] transform)
			: base (NSObjectFlag.Empty)
		{
			unsafe {
				fixed (float* ptr = transform)
					InitializeHandle (InitWithDevice (device, thresholdValue, maximumValue, (IntPtr) ptr), "initWithDevice:thresholdValue:maximumValue:linearGrayColorTransform:");
			}
		}

		/// <summary>Gets the color transform that maps 3-channel pixels to single-channel values.</summary>
		///         <value>The color transform that maps 3-channel pixels to single-channel values.</value>
		///         <remarks>To be added.</remarks>
		public float []? Transform {
			get { return MPSKernel.GetTransform (_Transform); }
		}
	}

	public partial class MPSImageThresholdTruncate {

		/// <param name="device">The device on which the filter will run.</param>
		///         <param name="thresholdValue">The value to which pixel brightensses will be clamped.</param>
		///         <param name="transform">A color transform that maps 3-channel pixels to single-channel values.</param>
		///         <summary>Constructs a new <c>MPSImageThresholdTruncate</c> with the specified values.</summary>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[DesignatedInitializer]
		public MPSImageThresholdTruncate (IMTLDevice device, float thresholdValue, /*[NullAllowed]*/ float [] transform)
			: base (NSObjectFlag.Empty)
		{
			unsafe {
				fixed (float* ptr = transform)
					InitializeHandle (InitWithDevice (device, thresholdValue, (IntPtr) ptr), "initWithDevice:thresholdValue:linearGrayColorTransform:");
			}
		}

		/// <summary>Gets the color transform that maps 3-channel pixels to single-channel values.</summary>
		///         <value>The color transform that maps 3-channel pixels to single-channel values.</value>
		///         <remarks>To be added.</remarks>
		public float []? Transform {
			get { return MPSKernel.GetTransform (_Transform); }
		}
	}

	public partial class MPSImageThresholdToZero {

		/// <param name="device">The device on which the filter will run.</param>
		///         <param name="thresholdValue">The value above which pixels will be left unchanged.</param>
		///         <param name="transform">A color transform that maps 3-channel pixels to single-channel values.</param>
		///         <summary>Constructs a new <c>MPSImageThresholdToZero</c> with the specified values.</summary>
		///         <remarks>To be added.</remarks>
		[DesignatedInitializer]
		public MPSImageThresholdToZero (IMTLDevice device, float thresholdValue, /*[NullAllowed]*/ float [] transform)
			: base (NSObjectFlag.Empty)
		{
			unsafe {
				fixed (float* ptr = transform)
					InitializeHandle (InitWithDevice (device, thresholdValue, (IntPtr) ptr), "initWithDevice:thresholdValue:linearGrayColorTransform:");
			}
		}

		/// <summary>Gets the color transform that maps 3-channel pixels to single-channel values.</summary>
		///         <value>The color transform that maps 3-channel pixels to single-channel values.</value>
		///         <remarks>To be added.</remarks>
		public float []? Transform {
			get { return MPSKernel.GetTransform (_Transform); }
		}
	}

	public partial class MPSImageThresholdToZeroInverse {

		/// <param name="device">The device on which the filter will run.</param>
		///         <param name="thresholdValue">The value above which pixels will be darkened to 0.</param>
		///         <param name="transform">A color transform that maps 3-channel pixels to single-channel values.</param>
		///         <summary>Constructs a new <c>MPSImageThresholdToZeroInverse</c> with the specified values.</summary>
		///         <remarks>To be added.</remarks>
		[DesignatedInitializer]
		public MPSImageThresholdToZeroInverse (IMTLDevice device, float thresholdValue, /*[NullAllowed]*/ float [] transform)
			: base (NSObjectFlag.Empty)
		{
			unsafe {
				fixed (float* ptr = transform)
					InitializeHandle (InitWithDevice (device, thresholdValue, (IntPtr) ptr), "initWithDevice:thresholdValue:linearGrayColorTransform:");
			}
		}

		/// <summary>Gets the color transform that maps 3-channel pixels to single-channel values.</summary>
		///         <value>The color transform that maps 3-channel pixels to single-channel values.</value>
		///         <remarks>To be added.</remarks>
		public float []? Transform {
			get { return MPSKernel.GetTransform (_Transform); }
		}
	}

	public partial class MPSImageSobel {

		/// <param name="device">The device on which the filter will run.</param>
		///         <param name="transform">An array of 3 floating point values that is dot-multiplied with the components of the color to produce a gray scale tone.</param>
		///         <summary>Constructs a new <c>MPSImageSobel</c> with the specified device and color transform.</summary>
		///         <remarks>To be added.</remarks>
		[DesignatedInitializer]
		public MPSImageSobel (IMTLDevice device, float [] transform)
			: base (NSObjectFlag.Empty)
		{
			if (transform is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (transform));

			unsafe {
				fixed (float* ptr = transform)
					InitializeHandle (InitWithDevice (device, (IntPtr) ptr), "initWithDevice:linearGrayColorTransform:");
			}
		}

		/// <summary>Gets the array of 3 floating point values that is dot-multiplied with the components of the color to produce a gray scale tone..</summary>
		///         <value>The array of 3 floating point values that is dot-multiplied with the components of the color to produce a gray scale tone.</value>
		///         <remarks>To be added.</remarks>
		public float []? ColorTransform {
			get { return MPSKernel.GetTransform (_ColorTransform); }
		}
	}

	public partial class MPSCnnConvolution {

		/// <param name="device">To be added.</param>
		///         <param name="convolutionDescriptor">To be added.</param>
		///         <param name="kernelWeights">To be added.</param>
		///         <param name="biasTerms">To be added.</param>
		///         <param name="flags">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[DesignatedInitializer]
		public MPSCnnConvolution (IMTLDevice device, MPSCnnConvolutionDescriptor convolutionDescriptor, float [] kernelWeights, float [] biasTerms, MPSCnnConvolutionFlags flags)
			: base (NSObjectFlag.Empty)
		{
			if (kernelWeights is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (kernelWeights));

			unsafe {
				fixed (float* kernelWeightsptr = kernelWeights)
				fixed (float* biasTermsptr = biasTerms)
					InitializeHandle (InitWithDevice (device, convolutionDescriptor, (IntPtr) kernelWeightsptr, (IntPtr) biasTermsptr, flags), "initWithDevice:convolutionDescriptor:kernelWeights:biasTerms:flags:");
			}
		}
	}

	public partial class MPSCnnFullyConnected {
		/// <param name="device">To be added.</param>
		///         <param name="convolutionDescriptor">To be added.</param>
		///         <param name="kernelWeights">To be added.</param>
		///         <param name="biasTerms">To be added.</param>
		///         <param name="flags">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[ObsoletedOSPlatform ("tvos11.0", "Use any of the other 'DesignatedInitializer' ctors.")]
		[ObsoletedOSPlatform ("ios11.0", "Use any of the other 'DesignatedInitializer' ctors.")]
		[ObsoletedOSPlatform ("macos", "Use any of the other 'DesignatedInitializer' ctors.")]
		[ObsoletedOSPlatform ("maccatalyst", "Use any of the other 'DesignatedInitializer' ctors.")]
		public MPSCnnFullyConnected (IMTLDevice device, MPSCnnConvolutionDescriptor convolutionDescriptor, float [] kernelWeights, float [] biasTerms, MPSCnnConvolutionFlags flags)
			: base (NSObjectFlag.Empty)
		{
			if (kernelWeights is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (kernelWeights));

			unsafe {
				fixed (float* kernelWeightsptr = kernelWeights)
				fixed (float* biasTermsptr = biasTerms)
					InitializeHandle (InitWithDevice (device, convolutionDescriptor, (IntPtr) kernelWeightsptr, (IntPtr) biasTermsptr, flags), "initWithDevice:convolutionDescriptor:kernelWeights:biasTerms:flags:");
			}
		}
	}

	public partial class MPSImageConversion {
		/// <param name="device">To be added.</param>
		/// <param name="srcAlpha">To be added.</param>
		/// <param name="destAlpha">To be added.</param>
		/// <param name="backgroundColor">To be added.</param>
		/// <param name="conversionInfo">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		public MPSImageConversion (IMTLDevice device, MPSAlphaType srcAlpha, MPSAlphaType destAlpha, nfloat [] backgroundColor, CGColorConversionInfo conversionInfo)
			: base (NSObjectFlag.Empty)
		{
			unsafe {
				fixed (nfloat* ptr = backgroundColor)
					InitializeHandle (InitWithDevice (device, srcAlpha, destAlpha, (IntPtr) ptr, conversionInfo), "initWithDevice:srcAlpha:destAlpha:backgroundColor:conversionInfo:");
			}
		}
	}

	public partial class MPSImagePyramid {

		/// <param name="device">To be added.</param>
		/// <param name="kernelWidth">To be added.</param>
		/// <param name="kernelHeight">To be added.</param>
		/// <param name="kernelWeights">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[DesignatedInitializer]
		public MPSImagePyramid (IMTLDevice device, nuint kernelWidth, nuint kernelHeight, float [] kernelWeights)
			: base (NSObjectFlag.Empty)
		{
			if (kernelWeights is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (kernelWeights));

			unsafe {
				fixed (float* ptr = kernelWeights)
					InitializeHandle (InitWithDevice (device, kernelWidth, kernelHeight, (IntPtr) ptr), "initWithDevice:kernelWidth:kernelHeight:weights:");
			}
		}
	}

	public partial class MPSImageGaussianPyramid {

		/// <param name="device">To be added.</param>
		/// <param name="kernelWidth">To be added.</param>
		/// <param name="kernelHeight">To be added.</param>
		/// <param name="kernelWeights">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[DesignatedInitializer]
		public MPSImageGaussianPyramid (IMTLDevice device, nuint kernelWidth, nuint kernelHeight, float [] kernelWeights)
			: base (NSObjectFlag.Empty)
		{
			if (kernelWeights is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (kernelWeights));

			unsafe {
				fixed (float* ptr = kernelWeights)
					InitializeHandle (InitWithDevice (device, kernelWidth, kernelHeight, (IntPtr) ptr), "initWithDevice:kernelWidth:kernelHeight:weights:");
			}
		}
	}

	/// <summary>To be added.</summary>
	///     <remarks>To be added.</remarks>
	public partial class MPSImageLaplacianPyramid {
		[DesignatedInitializer]
		public MPSImageLaplacianPyramid (IMTLDevice device, nuint kernelWidth, nuint kernelHeight, float [] kernelWeights) : base (NSObjectFlag.Empty)
		{
			if (kernelWeights is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (kernelWeights));
			if ((nuint) kernelWeights.Length < kernelWidth * kernelHeight)
				throw new ArgumentException ($"'{nameof (kernelWeights)}' size must be at least '{nameof (kernelWidth)}' * '{nameof (kernelHeight)}'.");

			unsafe {
				fixed (float* ptr = kernelWeights)
					InitializeHandle (InitWithDevice (device, kernelWidth, kernelHeight, (IntPtr) ptr), "initWithDevice:kernelWidth:kernelHeight:weights:");
			}
		}
	}

	/// <summary>To be added.</summary>
	///     <remarks>To be added.</remarks>
	public partial class MPSImageLaplacianPyramidSubtract {
		[DesignatedInitializer]
		public MPSImageLaplacianPyramidSubtract (IMTLDevice device, nuint kernelWidth, nuint kernelHeight, float [] kernelWeights) : base (NSObjectFlag.Empty)
		{
			if (kernelWeights is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (kernelWeights));
			if ((nuint) kernelWeights.Length < kernelWidth * kernelHeight)
				throw new ArgumentException ($"'{nameof (kernelWeights)}' size must be at least '{nameof (kernelWidth)}' * '{nameof (kernelHeight)}'.");

			unsafe {
				fixed (float* ptr = kernelWeights)
					InitializeHandle (InitWithDevice (device, kernelWidth, kernelHeight, (IntPtr) ptr), "initWithDevice:kernelWidth:kernelHeight:weights:");
			}
		}
	}

	/// <summary>To be added.</summary>
	///     <remarks>To be added.</remarks>
	public partial class MPSImageLaplacianPyramidAdd {
		[DesignatedInitializer]
		public MPSImageLaplacianPyramidAdd (IMTLDevice device, nuint kernelWidth, nuint kernelHeight, float [] kernelWeights) : base (NSObjectFlag.Empty)
		{
			if (kernelWeights is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (kernelWeights));
			if ((nuint) kernelWeights.Length < kernelWidth * kernelHeight)
				throw new ArgumentException ($"'{nameof (kernelWeights)}' size must be at least '{nameof (kernelWidth)}' * '{nameof (kernelHeight)}'.");

			unsafe {
				fixed (float* ptr = kernelWeights)
					InitializeHandle (InitWithDevice (device, kernelWidth, kernelHeight, (IntPtr) ptr), "initWithDevice:kernelWidth:kernelHeight:weights:");
			}
		}
	}

	public partial class MPSCnnBinaryConvolutionNode {
		/// <summary>Create a new <see cref="MPSCnnBinaryConvolutionNode" /> instance.</summary>
		/// <param name="sourceNode">An <see cref="MPSNNImageNode" /> node for the source image.</param>
		/// <param name="weights">An <see cref="IMPSCnnConvolutionDataSource" /> instance that provides weights and biases.</param>
		/// <param name="outputBiasTerms">An array of bias terms to be applied to the convolution output.</param>
		/// <param name="outputScaleTerms">An array of scale terms to be applied to the convolution output.</param>
		/// <param name="inputBiasTerms">An array of bias terms to be applied to the input before convulution and input scaling.</param>
		/// <param name="inputScaleTerms">An array of scale terms to be applied to the input before convulution and input scaling.</param>
		/// <param name="type">Which type of binary convulution to use.</param>
		/// <param name="flags">Any flags for the new instance.</param>
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		public static MPSCnnBinaryConvolutionNode Create (MPSNNImageNode sourceNode, IMPSCnnConvolutionDataSource weights, float []? outputBiasTerms, float []? outputScaleTerms, float []? inputBiasTerms, float []? inputScaleTerms, MPSCnnBinaryConvolutionType type, MPSCnnBinaryConvolutionFlags flags)
		{
			unsafe {
				fixed (void* outputBiasTermsHandle = outputBiasTerms)
				fixed (void* outputScaleTermsHandle = outputScaleTerms)
				fixed (void* inputBiasTermsHandle = inputBiasTerms)
				fixed (void* inputScaleTermsHandle = inputScaleTerms)
					return Create (sourceNode, weights, (IntPtr) outputBiasTermsHandle, (IntPtr) outputScaleTermsHandle, (IntPtr) inputBiasTermsHandle, (IntPtr) inputScaleTermsHandle, type, flags);
			}
		}

		/// <summary>Create a new <see cref="MPSCnnBinaryConvolutionNode" /> instance.</summary>
		/// <param name="sourceNode">An <see cref="MPSNNImageNode" /> node for the source image.</param>
		/// <param name="weights">An <see cref="IMPSCnnConvolutionDataSource" /> instance that provides weights and biases.</param>
		/// <param name="outputBiasTerms">An array of bias terms to be applied to the convolution output.</param>
		/// <param name="outputScaleTerms">An array of scale terms to be applied to the convolution output.</param>
		/// <param name="inputBiasTerms">An array of bias terms to be applied to the input before convulution and input scaling.</param>
		/// <param name="inputScaleTerms">An array of scale terms to be applied to the input before convulution and input scaling.</param>
		/// <param name="type">Which type of binary convulution to use.</param>
		/// <param name="flags">Any flags for the new instance.</param>
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		public MPSCnnBinaryConvolutionNode (MPSNNImageNode sourceNode, IMPSCnnConvolutionDataSource weights, float []? outputBiasTerms, float []? outputScaleTerms, float []? inputBiasTerms, float []? inputScaleTerms, MPSCnnBinaryConvolutionType type, MPSCnnBinaryConvolutionFlags flags) : base (NSObjectFlag.Empty)
		{
			unsafe {
				fixed (void* outputBiasTermsHandle = outputBiasTerms)
				fixed (void* outputScaleTermsHandle = outputScaleTerms)
				fixed (void* inputBiasTermsHandle = inputBiasTerms)
				fixed (void* inputScaleTermsHandle = inputScaleTerms)
					InitializeHandle (_InitWithSource (sourceNode, weights, (IntPtr) outputBiasTermsHandle, (IntPtr) outputScaleTermsHandle, (IntPtr) inputBiasTermsHandle, (IntPtr) inputScaleTermsHandle, type, flags));
			}
		}
	}

	public partial class MPSCnnBinaryFullyConnectedNode {
		/// <summary>Create a new <see cref="MPSCnnBinaryConvolutionNode" /> instance.</summary>
		/// <param name="sourceNode">An <see cref="MPSNNImageNode" /> node for the source image.</param>
		/// <param name="weights">An <see cref="IMPSCnnConvolutionDataSource" /> instance that provides weights and biases.</param>
		/// <param name="outputBiasTerms">An array of bias terms to be applied to the convolution output.</param>
		/// <param name="outputScaleTerms">An array of scale terms to be applied to the convolution output.</param>
		/// <param name="inputBiasTerms">An array of bias terms to be applied to the input before convulution and input scaling.</param>
		/// <param name="inputScaleTerms">An array of scale terms to be applied to the input before convulution and input scaling.</param>
		/// <param name="type">Which type of binary convulution to use.</param>
		/// <param name="flags">Any flags for the new instance.</param>
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		public new static MPSCnnBinaryFullyConnectedNode Create (MPSNNImageNode sourceNode, IMPSCnnConvolutionDataSource weights, float []? outputBiasTerms, float []? outputScaleTerms, float []? inputBiasTerms, float []? inputScaleTerms, MPSCnnBinaryConvolutionType type, MPSCnnBinaryConvolutionFlags flags)
		{
			unsafe {
				fixed (void* outputBiasTermsHandle = outputBiasTerms)
				fixed (void* outputScaleTermsHandle = outputScaleTerms)
				fixed (void* inputBiasTermsHandle = inputBiasTerms)
				fixed (void* inputScaleTermsHandle = inputScaleTerms)
					return Create (sourceNode, weights, (IntPtr) outputBiasTermsHandle, (IntPtr) outputScaleTermsHandle, (IntPtr) inputBiasTermsHandle, (IntPtr) inputScaleTermsHandle, type, flags);
			}
		}

		/// <summary>Create a new <see cref="MPSCnnBinaryConvolutionNode" /> instance.</summary>
		/// <param name="sourceNode">An <see cref="MPSNNImageNode" /> node for the source image.</param>
		/// <param name="weights">An <see cref="IMPSCnnConvolutionDataSource" /> instance that provides weights and biases.</param>
		/// <param name="outputBiasTerms">An array of bias terms to be applied to the convolution output.</param>
		/// <param name="outputScaleTerms">An array of scale terms to be applied to the convolution output.</param>
		/// <param name="inputBiasTerms">An array of bias terms to be applied to the input before convulution and input scaling.</param>
		/// <param name="inputScaleTerms">An array of scale terms to be applied to the input before convulution and input scaling.</param>
		/// <param name="type">Which type of binary convulution to use.</param>
		/// <param name="flags">Any flags for the new instance.</param>
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		public MPSCnnBinaryFullyConnectedNode (MPSNNImageNode sourceNode, IMPSCnnConvolutionDataSource weights, float []? outputBiasTerms, float []? outputScaleTerms, float []? inputBiasTerms, float []? inputScaleTerms, MPSCnnBinaryConvolutionType type, MPSCnnBinaryConvolutionFlags flags) : base (NSObjectFlag.Empty)
		{
			unsafe {
				fixed (void* outputBiasTermsHandle = outputBiasTerms)
				fixed (void* outputScaleTermsHandle = outputScaleTerms)
				fixed (void* inputBiasTermsHandle = inputBiasTerms)
				fixed (void* inputScaleTermsHandle = inputScaleTerms)
					InitializeHandle (_InitWithSource (sourceNode, weights, (IntPtr) outputBiasTermsHandle, (IntPtr) outputScaleTermsHandle, (IntPtr) inputBiasTermsHandle, (IntPtr) inputScaleTermsHandle, type, flags));
			}
		}
	}
#endif // COREBUILD
}
