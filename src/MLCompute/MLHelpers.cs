#nullable enable

using System;
using System.Runtime.InteropServices;

using CoreFoundation;
using Foundation;
using ObjCRuntime;

namespace MLCompute {
	[SupportedOSPlatform ("ios14.0")]
	[SupportedOSPlatform ("tvos14.0")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("maccatalyst")]
	public static class MLCActivationTypeExtensions {

		[DllImport (Constants.MLComputeLibrary)]
		static extern /* NSString */ IntPtr MLCActivationTypeDebugDescription (MLCActivationType activationType);

		public static string? GetDebugDescription (this MLCActivationType self)
		{
			return CFString.FromHandle (MLCActivationTypeDebugDescription (self));
		}
	}

	[SupportedOSPlatform ("ios14.0")]
	[SupportedOSPlatform ("tvos14.0")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("maccatalyst")]
	public static class MLCArithmeticOperationExtensions {

		[DllImport (Constants.MLComputeLibrary)]
		static extern /* NSString */ IntPtr MLCArithmeticOperationDebugDescription (MLCArithmeticOperation operation);

		public static string? GetDebugDescription (this MLCArithmeticOperation self)
		{
			return CFString.FromHandle (MLCArithmeticOperationDebugDescription (self));
		}
	}

	[SupportedOSPlatform ("ios14.0")]
	[SupportedOSPlatform ("tvos14.0")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("maccatalyst")]
	public static class MLCPaddingPolicyExtensions {

		[DllImport (Constants.MLComputeLibrary)]
		static extern /* NSString */ IntPtr MLCPaddingPolicyDebugDescription (MLCPaddingPolicy paddingPolicy);

		public static string? GetDebugDescription (this MLCPaddingPolicy self)
		{
			return CFString.FromHandle (MLCPaddingPolicyDebugDescription (self));
		}
	}

	[SupportedOSPlatform ("ios14.0")]
	[SupportedOSPlatform ("tvos14.0")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("maccatalyst")]
	public static class MLCLossTypeExtensions {

		[DllImport (Constants.MLComputeLibrary)]
		static extern /* NSString */ IntPtr MLCLossTypeDebugDescription (MLCLossType lossType);

		public static string? GetDebugDescription (this MLCLossType self)
		{
			return CFString.FromHandle (MLCLossTypeDebugDescription (self));
		}
	}

	[SupportedOSPlatform ("ios14.0")]
	[SupportedOSPlatform ("tvos14.0")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("maccatalyst")]
	public static class MLCReductionTypeExtensions {

		[DllImport (Constants.MLComputeLibrary)]
		static extern /* NSString */ IntPtr MLCReductionTypeDebugDescription (MLCReductionType reductionType);

		public static string? GetDebugDescription (this MLCReductionType self)
		{
			return CFString.FromHandle (MLCReductionTypeDebugDescription (self));
		}
	}

	[SupportedOSPlatform ("ios14.0")]
	[SupportedOSPlatform ("tvos14.0")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("maccatalyst")]
	public static class MLCPaddingTypeExtensions {

		[DllImport (Constants.MLComputeLibrary)]
		static extern /* NSString */ IntPtr MLCPaddingTypeDebugDescription (MLCPaddingType paddingType);

		public static string? GetDebugDescription (this MLCPaddingType self)
		{
			return CFString.FromHandle (MLCPaddingTypeDebugDescription (self));
		}
	}

	[SupportedOSPlatform ("ios14.0")]
	[SupportedOSPlatform ("tvos14.0")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("maccatalyst")]
	public static class MLCConvolutionTypeExtensions {

		[DllImport (Constants.MLComputeLibrary)]
		static extern /* NSString */ IntPtr MLCConvolutionTypeDebugDescription (MLCConvolutionType convolutionType);

		public static string? GetDebugDescription (this MLCConvolutionType self)
		{
			return CFString.FromHandle (MLCConvolutionTypeDebugDescription (self));
		}
	}

	[SupportedOSPlatform ("ios14.0")]
	[SupportedOSPlatform ("tvos14.0")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("maccatalyst")]
	public static class MLCPoolingTypeExtensions {

		[DllImport (Constants.MLComputeLibrary)]
		static extern /* NSString */ IntPtr MLCPoolingTypeDebugDescription (MLCPoolingType poolingType);

		public static string? GetDebugDescription (this MLCPoolingType self)
		{
			return CFString.FromHandle (MLCPoolingTypeDebugDescription (self));
		}
	}

	[SupportedOSPlatform ("ios14.0")]
	[SupportedOSPlatform ("tvos14.0")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("maccatalyst")]
	public static class MLCSoftmaxOperationExtensions {

		[DllImport (Constants.MLComputeLibrary)]
		static extern /* NSString */ IntPtr MLCSoftmaxOperationDebugDescription (MLCSoftmaxOperation operation);

		public static string? GetDebugDescription (this MLCSoftmaxOperation self)
		{
			return CFString.FromHandle (MLCSoftmaxOperationDebugDescription (self));
		}
	}

	[SupportedOSPlatform ("ios14.0")]
	[SupportedOSPlatform ("tvos14.0")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("maccatalyst")]
	public static class MLCSampleModeExtensions {

		[DllImport (Constants.MLComputeLibrary)]
		static extern /* NSString */ IntPtr MLCSampleModeDebugDescription (MLCSampleMode mode);

		public static string? GetDebugDescription (this MLCSampleMode self)
		{
			return CFString.FromHandle (MLCSampleModeDebugDescription (self));
		}
	}

	[SupportedOSPlatform ("ios14.0")]
	[SupportedOSPlatform ("tvos14.0")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("maccatalyst")]
	public static class MLCLstmResultModeExtensions {

		[DllImport (Constants.MLComputeLibrary)]
		static extern /* NSString */ IntPtr MLCLSTMResultModeDebugDescription (MLCLstmResultMode mode);

		public static string? GetDebugDescription (this MLCLstmResultMode self)
		{
			return CFString.FromHandle (MLCLSTMResultModeDebugDescription (self));
		}
	}

	[SupportedOSPlatform ("tvos14.5")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("ios14.5")]
	[SupportedOSPlatform ("maccatalyst")]
	public static class MLCComparisonOperationExtensions {

		[DllImport (Constants.MLComputeLibrary)]
		static extern /* NSString */ IntPtr MLCComparisonOperationDebugDescription (MLCComparisonOperation operation);

		public static string? GetDebugDescription (this MLCComparisonOperation self)
		{
			return CFString.FromHandle (MLCComparisonOperationDebugDescription (self));
		}
	}

	[SupportedOSPlatform ("tvos15.0")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("ios15.0")]
	[SupportedOSPlatform ("maccatalyst")]
	public static class MLCGradientClippingTypeExtensions {

		[DllImport (Constants.MLComputeLibrary)]
		static extern /* NSString */ IntPtr MLCGradientClippingTypeDebugDescription (MLCGradientClippingType gradientClippingType);

		public static string? GetDebugDescription (this MLCGradientClippingType self)
		{
			return CFString.FromHandle (MLCGradientClippingTypeDebugDescription (self));
		}
	}
}
