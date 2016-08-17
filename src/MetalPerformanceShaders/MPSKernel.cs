// Copyright 2015-2016 Xamarin Inc. All rights reserved.

using System;
using System.Runtime.InteropServices;
using XamCore.CoreGraphics;
using XamCore.Foundation;
using XamCore.Metal;
using XamCore.ObjCRuntime;

namespace XamCore.MetalPerformanceShaders {

	public partial class MPSKernel : NSObject {

#if !COREBUILD
		[DllImport (Constants.MetalPerformanceShadersLibrary)]
		extern static bool MPSSupportsMTLDevice (/* __nullable id <MTLDevice> */ IntPtr device);

		public static bool Supports (IMTLDevice device)
		{
			return MPSSupportsMTLDevice (device == null ? IntPtr.Zero : device.Handle);
		}

		internal static IntPtr GetPtr (float [] values, bool throwOnNull)
		{
			if (throwOnNull && (values == null))
				throw new ArgumentNullException ("values");
			unsafe {
				fixed (float *ptr = values)
					return (IntPtr) ptr;
			}
		}

#if XAMCORE_2_0
		internal static IntPtr GetPtr (nfloat [] values, bool throwOnNull)
		{
			if (throwOnNull && (values == null))
				throw new ArgumentNullException ("values");
			unsafe {
				fixed (nfloat *ptr = values)
					return (IntPtr) ptr;
			}
		}
#endif

		internal unsafe static float [] GetTransform (IntPtr transform)
		{
			var t = (float*) transform;
			if (t == null)
				return null;
			return new float [3] { t [0], t [1], t [2] };
		}

		[Field ("MPSRectNoClip", "MetalPerformanceShaders")]
		public unsafe static MTLRegion RectNoClip {
			get {
				var p = Dlfcn.dlsym (Libraries.MetalPerformanceShaders.Handle, "MPSRectNoClip");
				if (p == IntPtr.Zero)
					return new MTLRegion ();
				unsafe {
					nint *ptr = (nint *) p;
					return MTLRegion.Create3D (ptr [0], ptr [1], ptr [2], ptr [3], ptr [4], ptr [5]);
				}
			}
		}
#endif
	}

#if !COREBUILD
	public partial class MPSImageDilate {

		public MPSImageDilate (IMTLDevice device, nuint kernelWidth, nuint kernelHeight, float[] values)
			: this (device, kernelWidth, kernelHeight, MPSKernel.GetPtr (values, true))
		{
		}
	}

	public partial class MPSImageErode : MPSImageDilate {

		public MPSImageErode (IMTLDevice device, nuint kernelWidth, nuint kernelHeight, float[] values)
			: base (device, kernelWidth, kernelHeight, values)
		{
		}
	}

	public partial class MPSImageThresholdBinary {

		public MPSImageThresholdBinary (IMTLDevice device, float thresholdValue, float maximumValue, /*[NullAllowed]*/ float[] transform)
			: this (device, thresholdValue, maximumValue, MPSKernel.GetPtr (transform, false))
		{
		}

		public float[] Transform {
			get { return MPSKernel.GetTransform (_Transform); }
		}
	}

	public partial class MPSImageThresholdBinaryInverse {

		public MPSImageThresholdBinaryInverse (IMTLDevice device, float thresholdValue, float maximumValue, /*[NullAllowed]*/ float[] transform)
			: this (device, thresholdValue, maximumValue, MPSKernel.GetPtr (transform, false))
		{
		}

		public float[] Transform {
			get { return MPSKernel.GetTransform (_Transform); }
		}
	}

	public partial class MPSImageThresholdTruncate {

		public MPSImageThresholdTruncate (IMTLDevice device, float thresholdValue, /*[NullAllowed]*/ float[] transform)
			: this (device, thresholdValue, MPSKernel.GetPtr (transform, false))
		{
		}

		public float[] Transform {
			get { return MPSKernel.GetTransform (_Transform); }
		}
	}

	public partial class MPSImageThresholdToZero {

		public MPSImageThresholdToZero (IMTLDevice device, float thresholdValue, /*[NullAllowed]*/ float[] transform)
			: this (device, thresholdValue, MPSKernel.GetPtr (transform, false))
		{
		}

		public float[] Transform {
			get { return MPSKernel.GetTransform (_Transform); }
		}
	}

	public partial class MPSImageThresholdToZeroInverse {

		public MPSImageThresholdToZeroInverse (IMTLDevice device, float thresholdValue, /*[NullAllowed]*/ float[] transform)
			: this (device, thresholdValue, MPSKernel.GetPtr (transform, false))
		{
		}

		public float[] Transform {
			get { return MPSKernel.GetTransform (_Transform); }
		}
	}

	public partial class MPSImageSobel {
		public MPSImageSobel (IMTLDevice device, float[] transform)
			: this (device, MPSKernel.GetPtr (transform, true))
		{
		}

		public float[] ColorTransform {
			get { return MPSKernel.GetTransform (_ColorTransform); }
		}
	}

	public partial class MPSCnnConvolution {
		public MPSCnnConvolution (IMTLDevice device, MPSCnnConvolutionDescriptor convolutionDescriptor, float[] kernelWeights, float[] biasTerms, MPSCnnConvolutionFlags flags)
			: this (device, convolutionDescriptor, MPSKernel.GetPtr (kernelWeights, true), MPSKernel.GetPtr (biasTerms, false), flags)
		{
		}
	}

	public partial class MPSCnnFullyConnected {
		public MPSCnnFullyConnected (IMTLDevice device, MPSCnnConvolutionDescriptor convolutionDescriptor, float[] kernelWeights, float[] biasTerms, MPSCnnConvolutionFlags flags)
			: this (device, convolutionDescriptor, MPSKernel.GetPtr (kernelWeights, true), MPSKernel.GetPtr (biasTerms, false), flags)
		{
		}
	}

	public partial class MPSImageConversion {
		public MPSImageConversion (IMTLDevice device, MPSAlphaType srcAlpha, MPSAlphaType destAlpha, nfloat[] backgroundColor, CGColorConversionInfo conversionInfo)
			: this (device, srcAlpha, destAlpha, MPSKernel.GetPtr (backgroundColor, false), conversionInfo)
		{
		}
	}

	public partial class MPSImagePyramid {
		public MPSImagePyramid (IMTLDevice device, nuint kernelWidth, nuint kernelHeight, float[] kernelWeights)
			: this (device, kernelWidth, kernelHeight, MPSKernel.GetPtr (kernelWeights, true))
		{
		}
	}

	public partial class MPSImageGaussianPyramid {
		public MPSImageGaussianPyramid (IMTLDevice device, nuint kernelWidth, nuint kernelHeight, float[] kernelWeights)
			: this (device, kernelWidth, kernelHeight, MPSKernel.GetPtr (kernelWeights, true))
		{
		}
	}
#endif
}
