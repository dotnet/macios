using System;
using System.Runtime.InteropServices;

using Foundation;
using ObjCRuntime;
using Metal;

namespace MetalPerformanceShaders {

	/// <summary>Enumerates ORable kernel options that improve performance in certain cases.</summary>
	[Introduced (PlatformName.MacCatalyst, 13, 0)]
	[Native] // NSUInteger
	[Flags] // NS_OPTIONS
	public enum MPSKernelOptions : ulong {
		/// <summary>Validate the kernel and use standard-precision types in calculations. Default.</summary>
		None = 0,
		/// <summary>Skip Metal's validation layer.</summary>
		SkipApiValidation = 1 << 0,
		/// <summary>To be added.</summary>
		AllowReducedPrecision = 1 << 1,
		/// <summary>To be added.</summary>
		[MacCatalyst (13, 1)]
		DisableInternalTiling = 1 << 2,
		/// <summary>To be added.</summary>
		[MacCatalyst (13, 1)]
		InsertDebugGroups = 1 << 3,
		/// <summary>To be added.</summary>
		[MacCatalyst (13, 1)]
		Verbose = 1 << 4,
	}

	/// <summary>Enumerates shader behavior at the edges of regions and images.</summary>
	[Introduced (PlatformName.MacCatalyst, 13, 0)]
	[Native] // NSUInteger
	public enum MPSImageEdgeMode : ulong {
		/// <summary>Pixels outside the region of interest are set to zero. (The alpha channel is set to 0.0 for pixels with an alpha channel, and to 1.0 for those without.)</summary>
		Zero,
		/// <summary>Pixels outside the region of interest are clamped to the values at the edge of the region.</summary>
		Clamp = 1,
		/// <summary>To be added.</summary>
		[MacCatalyst (13, 1)]
		Mirror,
		/// <summary>To be added.</summary>
		[MacCatalyst (13, 1)]
		MirrorWithEdge,
		/// <summary>To be added.</summary>
		[MacCatalyst (13, 1)]
		Constant,
	}

	/// <summary>Enumerates values that indicate if and what kind of color premultiplication will be applied to color values.</summary>
	[MacCatalyst (13, 1)]
	[Native]
	public enum MPSAlphaType : ulong {
		/// <summary>Indicates that the image will not be premultiplied, and the alpha channel will not be guaranteed to be 1.</summary>
		NonPremultiplied = 0,
		/// <summary>Indicates that the alpha channel will be clamped to 1, even if it is not encoded in the source data.</summary>
		AlphaIsOne = 1,
		/// <summary>Indicates that the image will be premultiplied, and the alpha channel will not be guaranteed to be 1.</summary>
		Premultiplied = 2,
	}

	/// <summary>Enumerates values that specify floating point data types.</summary>
	[Introduced (PlatformName.MacCatalyst, 13, 0)]
	public enum MPSDataType : uint { // uint32_t
		/// <summary>To be added.</summary>
		Invalid = 0,

		/// <summary>Indicates floating point format data of any width.</summary>
		FloatBit = 0x10000000,
		/// <summary>To be added.</summary>
		Float16 = FloatBit | 16,
		/// <summary>Indicates 32-bit floating point format data.</summary>
		Float32 = FloatBit | 32,

		/// <summary>To be added.</summary>
		SignedBit = 0x20000000,
		[TV (18, 4), Mac (15, 4), iOS (18, 4), MacCatalyst (18, 4)]
		Int2 = SignedBit | 2,
		[TV (18, 0), Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
		Int4 = SignedBit | 4,
		/// <summary>To be added.</summary>
		Int8 = SignedBit | 8,
		/// <summary>To be added.</summary>
		Int16 = SignedBit | 16,
		Int32 = SignedBit | 32,
		[iOS (14, 1)]
		[TV (14, 2)]
		[MacCatalyst (14, 1)]
		Int64 = SignedBit | 64,

		[TV (18, 4), Mac (15, 4), iOS (18, 4), MacCatalyst (18, 4)]
		UInt2 = 2,
		[TV (18, 0), Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
		UInt4 = 4,
		/// <summary>To be added.</summary>
		UInt8 = 8,
		/// <summary>To be added.</summary>
		UInt16 = 16,
		/// <summary>To be added.</summary>
		UInt32 = 32,
		[iOS (14, 1)]
		[TV (14, 2)]
		[MacCatalyst (14, 1)]
		UInt64 = 64,

		/// <summary>To be added.</summary>
		[MacCatalyst (13, 1)]
		NormalizedBit = 0x40000000,
		/// <summary>To be added.</summary>
		[MacCatalyst (13, 1)]
		Unorm1 = NormalizedBit | 1,
		/// <summary>To be added.</summary>
		[MacCatalyst (13, 1)]
		Unorm8 = NormalizedBit | 8,
	}

	[Flags]
	[Native]
	[iOS (13, 0), TV (13, 0)]
	[Introduced (PlatformName.MacCatalyst, 13, 0)]
	public enum MPSAliasingStrategy : ulong {
		Default = 0x0,
		DontCare = Default,
		ShallAlias = 1uL << 0,
		ShallNotAlias = 1uL << 1,
		AliasingReserved = ShallAlias | ShallNotAlias,
		PreferTemporaryMemory = 1uL << 2,
		PreferNonTemporaryMemory = 1uL << 3,
	}

	/// <summary>Enumerates image channel descriptions.</summary>
	[Introduced (PlatformName.MacCatalyst, 13, 0)]
	[Native]
	public enum MPSImageFeatureChannelFormat : ulong {
		/// <summary>Indicates an invalid format.</summary>
		Invalid = 0,
		/// <summary>Indicates an unsigned 8-bit integer that encodes values in [0,1.0].</summary>
		Unorm8 = 1,
		/// <summary>Indicates an unsigned 16-bit integer that encodes values in [0,1.0].</summary>
		Unorm16 = 2,
		/// <summary>Indicates a half-precision floating point format.</summary>
		Float16 = 3,
		/// <summary>Indicates a single-precision floating point format.</summary>
		Float32 = 4,
		[iOS (13, 0), TV (13, 0)]
		[MacCatalyst (13, 1)]
		Reserved0 = 5,

		//Count, // must always be last, and because of this it will cause breaking changes.
	}

	/// <summary>Enumerates the result forms of a matrix decomposition.</summary>
	[Introduced (PlatformName.MacCatalyst, 13, 0)]
	public enum MPSMatrixDecompositionStatus {
		/// <summary>To be added.</summary>
		Success = 0,
		/// <summary>To be added.</summary>
		Failure = -1,
		/// <summary>To be added.</summary>
		Singular = -2,
		/// <summary>To be added.</summary>
		NonPositiveDefinite = -3,
	}

	[iOS (13, 0), TV (13, 0)]
	[Introduced (PlatformName.MacCatalyst, 13, 0)]
	[Flags]
	[Native]
	public enum MPSMatrixRandomDistribution : ulong {
		Default = 0x1,
		Uniform = 0x2,
		[iOS (14, 0), TV (14, 0)]
		[Introduced (PlatformName.MacCatalyst, 14, 0)]
		Normal = Default | Uniform,
	}

	/// <summary>Enumerates the propagation direction in a layer in a recurrent neural net.</summary>
	[MacCatalyst (13, 1)]
	[Native]
	public enum MPSRnnSequenceDirection : ulong {
		/// <summary>To be added.</summary>
		Forward = 0,
		/// <summary>To be added.</summary>
		Backward,
	}

	/// <summary>Enumerates how input matrices or images should be combined in a recurrent neural net.</summary>
	[MacCatalyst (13, 1)]
	[Native]
	public enum MPSRnnBidirectionalCombineMode : ulong {
		/// <summary>To be added.</summary>
		None = 0,
		/// <summary>To be added.</summary>
		Add,
		/// <summary>To be added.</summary>
		Concatenate,
	}

	/// <summary>Enumerates the available activation functions of a neuron.</summary>
	[MacCatalyst (13, 1)]
	public enum MPSCnnNeuronType {
		/// <summary>To be added.</summary>
		None = 0,
		/// <summary>To be added.</summary>
		ReLU,
		/// <summary>To be added.</summary>
		Linear,
		/// <summary>To be added.</summary>
		Sigmoid,
		/// <summary>To be added.</summary>
		HardSigmoid,
		/// <summary>To be added.</summary>
		TanH,
		/// <summary>To be added.</summary>
		Absolute,
		/// <summary>To be added.</summary>
		SoftPlus,
		/// <summary>To be added.</summary>
		SoftSign,
		/// <summary>To be added.</summary>
		Elu,
		/// <summary>To be added.</summary>
		PReLU,
		/// <summary>To be added.</summary>
		ReLun,
		/// <summary>To be added.</summary>
		[MacCatalyst (13, 1)]
		Power,
		/// <summary>To be added.</summary>
		[MacCatalyst (13, 1)]
		Exponential,
		/// <summary>To be added.</summary>
		[MacCatalyst (13, 1)]
		Logarithm,
		[TV (13, 0), iOS (13, 0)]
		[MacCatalyst (13, 1)]
		GeLU,
	}

	/// <summary>Flagging enumeration for options available to binary convolution kernels.</summary>
	[MacCatalyst (13, 1)]
	[Native]
	public enum MPSCnnBinaryConvolutionFlags : ulong {
		/// <summary>To be added.</summary>
		None = 0,
		/// <summary>To be added.</summary>
		UseBetaScaling = 1 << 0,
	}

	/// <summary>Enumerates the operation used in a binary convolution.</summary>
	[MacCatalyst (13, 1)]
	[Native]
	public enum MPSCnnBinaryConvolutionType : ulong {
		/// <summary>To be added.</summary>
		BinaryWeights = 0,
		/// <summary>To be added.</summary>
		Xnor,
		/// <summary>To be added.</summary>
		And,
	}

	/// <summary>Options for how a neural network graph will pad results.</summary>
	[MacCatalyst (13, 1)]
	[Native]
	public enum MPSNNPaddingMethod : ulong {
		/// <summary>To be added.</summary>
		AlignCentered = 0,
		/// <summary>To be added.</summary>
		AlignTopLeft = 1,
		/// <summary>To be added.</summary>
		AlignBottomRight = 2,
		/// <summary>To be added.</summary>
		AlignReserved = 3,
		/// <summary>To be added.</summary>
		AddRemainderToTopLeft = 0 << 2,
		/// <summary>To be added.</summary>
		AddRemainderToTopRight = 1 << 2,
		/// <summary>To be added.</summary>
		AddRemainderToBottomLeft = 2 << 2,
		/// <summary>To be added.</summary>
		AddRemainderToBottomRight = 3 << 2,
		/// <summary>To be added.</summary>
		SizeValidOnly = 0,
		/// <summary>To be added.</summary>
		SizeSame = 1 << 4,
		/// <summary>To be added.</summary>
		SizeFull = 2 << 4,
		/// <summary>To be added.</summary>
		SizeReserved = 3 << 4,
		/// <summary>To be added.</summary>
		CustomWhitelistForNodeFusion = (1 << 13),
		/// <summary>To be added.</summary>
		Custom = (1 << 14),
		/// <summary>To be added.</summary>
		SizeMask = 2032,
		/// <summary>To be added.</summary>
		ExcludeEdges = (1 << 15),
	}

	/// <summary>Enumerates whether a data buffer is row- or column-major</summary>
	[Introduced (PlatformName.MacCatalyst, 13, 0)]
	[Native]
	public enum MPSDataLayout : ulong {
		/// <summary>To be added.</summary>
		HeightPerWidthPerFeatureChannels = 0,
		/// <summary>To be added.</summary>
		FeatureChannelsPerHeightPerWidth = 1,
	}

	[Introduced (PlatformName.MacCatalyst, 13, 0)]
	[Native]
	public enum MPSStateResourceType : ulong {
		/// <summary>To be added.</summary>
		None = 0,
		/// <summary>To be added.</summary>
		Buffer = 1,
		/// <summary>To be added.</summary>
		Texture = 2,
	}

	[MacCatalyst (13, 1)]
	[Native]
	public enum MPSIntersectionType : ulong {
		/// <summary>To be added.</summary>
		Nearest = 0,
		/// <summary>To be added.</summary>
		Any = 1,
	}

	[MacCatalyst (13, 1)]
	[Native]
	public enum MPSTriangleIntersectionTestType : ulong {
		/// <summary>To be added.</summary>
		Default = 0,
		/// <summary>To be added.</summary>
		Watertight = 1,
	}

	[MacCatalyst (13, 1)]
	[Native]
	public enum MPSBoundingBoxIntersectionTestType : ulong {
		/// <summary>To be added.</summary>
		Default = 0,
		/// <summary>To be added.</summary>
		AxisAligned = 1,
		[TV (13, 0), iOS (13, 0)]
		[MacCatalyst (13, 1)]
		Fast = 2,
	}

	[MacCatalyst (13, 1)]
	[Flags]
	[Native]
	public enum MPSRayMaskOptions : ulong {
		/// <summary>To be added.</summary>
		None = 0,
		/// <summary>To be added.</summary>
		Primitive = 1,
		/// <summary>To be added.</summary>
		Instance = 2,
	}

	[MacCatalyst (13, 1)]
	[Native]
	public enum MPSRayDataType : ulong {
		/// <summary>To be added.</summary>
		OriginDirection = 0,
		/// <summary>To be added.</summary>
		OriginMinDistanceDirectionMaxDistance = 1,
		/// <summary>To be added.</summary>
		OriginMaskDirectionMaxDistance = 2,
		[TV (13, 0), iOS (13, 0)]
		[MacCatalyst (13, 1)]
		PackedOriginDirection = 3,
	}

	[MacCatalyst (13, 1)]
	[Native]
	public enum MPSIntersectionDataType : ulong {
		/// <summary>To be added.</summary>
		Distance = 0,
		/// <summary>To be added.</summary>
		PrimitiveIndex = 1,
		/// <summary>To be added.</summary>
		PrimitiveIndexCoordinates = 2,
		/// <summary>To be added.</summary>
		PrimitiveIndexInstanceIndex = 3,
		/// <summary>To be added.</summary>
		PrimitiveIndexInstanceIndexCoordinates = 4,
	}

	[MacCatalyst (13, 1)]
	[Native]
	public enum MPSTransformType : ulong {
		/// <summary>To be added.</summary>
		Float4x4 = 0,
		/// <summary>To be added.</summary>
		Identity = 1,
	}

	[MacCatalyst (13, 1)]
	[Flags]
	[Native]
	public enum MPSAccelerationStructureUsage : ulong {
		/// <summary>To be added.</summary>
		None = 0,
		/// <summary>To be added.</summary>
		Refit = 1,
		/// <summary>To be added.</summary>
		FrequentRebuild = 2,
		[TV (13, 0), iOS (13, 0)]
		[MacCatalyst (13, 1)]
		PreferGpuBuild = 4,
		[TV (13, 0), iOS (13, 0)]
		[MacCatalyst (13, 1)]
		PreferCpuBuild = 8,
	}

	[MacCatalyst (13, 1)]
	[Native]
	public enum MPSAccelerationStructureStatus : ulong {
		/// <summary>To be added.</summary>
		Unbuilt = 0,
		/// <summary>To be added.</summary>
		Built = 1,
	}

	[MacCatalyst (13, 1)]
	public enum MPSCnnWeightsQuantizationType : uint {
		/// <summary>To be added.</summary>
		None = 0,
		/// <summary>To be added.</summary>
		Linear = 1,
		/// <summary>To be added.</summary>
		LookupTable = 2,
	}

	[Flags]
	[Native]
	[MacCatalyst (13, 1)]
	public enum MPSCnnConvolutionGradientOption : ulong {
		/// <summary>To be added.</summary>
		GradientWithData = 0x1,
		/// <summary>To be added.</summary>
		GradientWithWeightsAndBias = 0x2,
		/// <summary>To be added.</summary>
		All = GradientWithData | GradientWithWeightsAndBias,
	}

	[Flags]
	[Native]
	[MacCatalyst (13, 1)]
	public enum MPSNNComparisonType : ulong {
		/// <summary>To be added.</summary>
		Equal,
		/// <summary>To be added.</summary>
		NotEqual,
		/// <summary>To be added.</summary>
		Less,
		/// <summary>To be added.</summary>
		LessOrEqual,
		/// <summary>To be added.</summary>
		Greater,
		/// <summary>To be added.</summary>
		GreaterOrEqual,
	}

	[Introduced (PlatformName.MacCatalyst, 13, 0)]
	public enum MPSCnnLossType : uint {
		/// <summary>To be added.</summary>
		MeanAbsoluteError = 0,
		/// <summary>To be added.</summary>
		MeanSquaredError,
		/// <summary>To be added.</summary>
		SoftMaxCrossEntropy,
		/// <summary>To be added.</summary>
		SigmoidCrossEntropy,
		/// <summary>To be added.</summary>
		CategoricalCrossEntropy,
		/// <summary>To be added.</summary>
		Hinge,
		/// <summary>To be added.</summary>
		Huber,
		/// <summary>To be added.</summary>
		CosineDistance,
		/// <summary>To be added.</summary>
		Log,
		/// <summary>To be added.</summary>
		KullbackLeiblerDivergence,
		//Count, // must always be last, and because of this it will cause breaking changes.
	}

	[Introduced (PlatformName.MacCatalyst, 13, 0)]
	public enum MPSCnnReductionType {
		/// <summary>To be added.</summary>
		None = 0,
		/// <summary>To be added.</summary>
		Sum,
		/// <summary>To be added.</summary>
		Mean,
		/// <summary>To be added.</summary>
		SumByNonZeroWeights,
		//Count, // must always be last, and because of this it will cause breaking changes.
	}

	[Flags]
	[Native]
	[MacCatalyst (13, 1)]
	public enum MPSNNConvolutionAccumulatorPrecisionOption : ulong {
		/// <summary>To be added.</summary>
		Half = 0x0,
		/// <summary>To be added.</summary>
		Float = 1uL << 0,
	}

	[Flags]
	[Native]
	[MacCatalyst (13, 1)]
	public enum MPSCnnBatchNormalizationFlags : ulong {
		/// <summary>To be added.</summary>
		Default = 0x0,
		/// <summary>To be added.</summary>
		CalculateStatisticsAutomatic = Default,
		/// <summary>To be added.</summary>
		CalculateStatisticsAlways = 0x1,
		/// <summary>To be added.</summary>
		CalculateStatisticsNever = 0x2,
		/// <summary>To be added.</summary>
		CalculateStatisticsMask = 0x3,
	}

	[MacCatalyst (13, 1)]
	[Native]
	public enum MPSNNRegularizationType : ulong {
		/// <summary>To be added.</summary>
		None = 0,
		/// <summary>To be added.</summary>
		L1 = 1,
		/// <summary>To be added.</summary>
		L2 = 2,
	}

	[MacCatalyst (13, 1)]
	[Flags]
	[Native]
	public enum MPSNNTrainingStyle : ulong {
		/// <summary>To be added.</summary>
		None = 0x0,
		/// <summary>To be added.</summary>
		Cpu = 0x1,
		/// <summary>To be added.</summary>
		Gpu = 0x2,
	}

	[Native]
	[MacCatalyst (13, 1)]
	public enum MPSRnnMatrixId : ulong {
		/// <summary>To be added.</summary>
		SingleGateInputWeights = 0,
		/// <summary>To be added.</summary>
		SingleGateRecurrentWeights,
		/// <summary>To be added.</summary>
		SingleGateBiasTerms,
		/// <summary>To be added.</summary>
		LstmInputGateInputWeights,
		/// <summary>To be added.</summary>
		LstmInputGateRecurrentWeights,
		/// <summary>To be added.</summary>
		LstmInputGateMemoryWeights,
		/// <summary>To be added.</summary>
		LstmInputGateBiasTerms,
		/// <summary>To be added.</summary>
		LstmForgetGateInputWeights,
		/// <summary>To be added.</summary>
		LstmForgetGateRecurrentWeights,
		/// <summary>To be added.</summary>
		LstmForgetGateMemoryWeights,
		/// <summary>To be added.</summary>
		LstmForgetGateBiasTerms,
		/// <summary>To be added.</summary>
		LstmMemoryGateInputWeights,
		/// <summary>To be added.</summary>
		LstmMemoryGateRecurrentWeights,
		/// <summary>To be added.</summary>
		LstmMemoryGateMemoryWeights,
		/// <summary>To be added.</summary>
		LstmMemoryGateBiasTerms,
		/// <summary>To be added.</summary>
		LstmOutputGateInputWeights,
		/// <summary>To be added.</summary>
		LstmOutputGateRecurrentWeights,
		/// <summary>To be added.</summary>
		LstmOutputGateMemoryWeights,
		/// <summary>To be added.</summary>
		LstmOutputGateBiasTerms,
		/// <summary>To be added.</summary>
		GruInputGateInputWeights,
		/// <summary>To be added.</summary>
		GruInputGateRecurrentWeights,
		/// <summary>To be added.</summary>
		GruInputGateBiasTerms,
		/// <summary>To be added.</summary>
		GruRecurrentGateInputWeights,
		/// <summary>To be added.</summary>
		GruRecurrentGateRecurrentWeights,
		/// <summary>To be added.</summary>
		GruRecurrentGateBiasTerms,
		/// <summary>To be added.</summary>
		GruOutputGateInputWeights,
		/// <summary>To be added.</summary>
		GruOutputGateRecurrentWeights,
		/// <summary>To be added.</summary>
		GruOutputGateInputGateWeights,
		/// <summary>To be added.</summary>
		GruOutputGateBiasTerms,
		//Count, // must always be last, and because of this it will cause breaking changes.
	}

	[Introduced (PlatformName.MacCatalyst, 13, 0)]
	public enum MPSCustomKernelIndex : uint {
		DestIndex = 0,
		Src0Index = 0,
		Src1Index = 1,
		Src2Index = 2,
		Src3Index = 3,
		Src4Index = 4,
		UserDataIndex = 30,
	}

	[Introduced (PlatformName.MacCatalyst, 13, 0)]
	public enum MPSImageType : uint {
		Type2d = 0,
		Type2dArray = 1,
		Array2d = 2,
		Array2dArray = 3,

		ArrayMask = 1,
		BatchMask = 2,
		TypeMask = 3,
		NoAlpha = 4,
		TexelFormatMask = 56,
		TexelFormatShift = 3,
		TexelFormatStandard = 0u << (int) TexelFormatShift,
		TexelFormatUnorm8 = 1u << (int) TexelFormatShift,
		TexelFormatFloat16 = 2u << (int) TexelFormatShift,
		TexelFormatBFloat16 = 3u << (int) TexelFormatShift,
		BitCount = 6,
		Mask = (1u << (int) BitCount) - 1,
		Type2dNoAlpha = Type2d | NoAlpha,
		Type2dArrayNoAlpha = Type2dArray | NoAlpha,
		Array2dNoAlpha = Type2d | BatchMask | NoAlpha,
		Array2dArrayNoAlpha = Type2dArray | BatchMask | NoAlpha,

		DestTextureType = (MPSConstants.FunctionConstantIndex >> (int) (0 * BitCount)) & Mask,
		Src0TextureType = (MPSConstants.FunctionConstantIndex >> (int) (0 * BitCount)) & Mask,
		Src1TextureType = (MPSConstants.FunctionConstantIndex >> (int) (1 * BitCount)) & Mask,
		Src2TextureType = (MPSConstants.FunctionConstantIndex >> (int) (2 * BitCount)) & Mask,
		Src3TextureType = (MPSConstants.FunctionConstantIndex >> (int) (3 * BitCount)) & Mask,
		Src4TextureType = (MPSConstants.FunctionConstantIndex >> (int) (4 * BitCount)) & Mask,
	}

	[Flags]
	[Native]
	[Introduced (PlatformName.MacCatalyst, 13, 0)]
	public enum MPSDeviceOptions : ulong {
		Default = 0x0,
		LowPower = 0x1,
		SkipRemovable = 0x2,
	}
}
