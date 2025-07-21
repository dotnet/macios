//
// corevideo.cs: Definitions for CoreVideo
//
// Copyright 2014 Xamarin Inc. All rights reserved.
// Copyright 2020 Microsoft Corporation
//

using System;

using CoreGraphics;
using Foundation;
using ObjCRuntime;
using Metal;

namespace CoreVideo {

	/// <summary>Manages pixel buffer pool allocation settings.</summary>
	[Partial]
	interface CVPixelBufferPoolAllocationSettings {

		[Internal]
		[Field ("kCVPixelBufferPoolAllocationThresholdKey")]
		NSString ThresholdKey { get; }
	}

	/// <summary>A Core Video data buffer, containing video, audio, or other type of data.</summary>
	[Partial]
	interface CVBuffer {

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kCVBufferMovieTimeKey")]
		NSString MovieTimeKey { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kCVBufferTimeValueKey")]
		NSString TimeValueKey { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kCVBufferTimeScaleKey")]
		NSString TimeScaleKey { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kCVBufferPropagatedAttachmentsKey")]
		NSString PropagatedAttachmentsKey { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kCVBufferNonPropagatedAttachmentsKey")]
		NSString NonPropagatedAttachmentsKey { get; }
	}

	/// <summary>A <see cref="CoreVideo.CVBuffer" /> that stores image data.</summary>
	[Partial]
	interface CVImageBuffer : CVBuffer {

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kCVImageBufferCGColorSpaceKey")]
		NSString CGColorSpaceKey { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kCVImageBufferGammaLevelKey")]
		NSString GammaLevelKey { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kCVImageBufferCleanApertureKey")]
		NSString CleanApertureKey { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kCVImageBufferPreferredCleanApertureKey")]
		NSString PreferredCleanApertureKey { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kCVImageBufferCleanApertureWidthKey")]
		NSString CleanApertureWidthKey { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kCVImageBufferCleanApertureHeightKey")]
		NSString CleanApertureHeightKey { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kCVImageBufferCleanApertureHorizontalOffsetKey")]
		NSString CleanApertureHorizontalOffsetKey { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kCVImageBufferCleanApertureVerticalOffsetKey")]
		NSString CleanApertureVerticalOffsetKey { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kCVImageBufferFieldCountKey")]
		NSString FieldCountKey { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kCVImageBufferFieldDetailKey")]
		NSString FieldDetailKey { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kCVImageBufferFieldDetailTemporalTopFirst")]
		NSString FieldDetailTemporalTopFirst { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kCVImageBufferFieldDetailTemporalBottomFirst")]
		NSString FieldDetailTemporalBottomFirst { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kCVImageBufferFieldDetailSpatialFirstLineEarly")]
		NSString FieldDetailSpatialFirstLineEarly { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kCVImageBufferFieldDetailSpatialFirstLineLate")]
		NSString FieldDetailSpatialFirstLineLate { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kCVImageBufferPixelAspectRatioKey")]
		NSString PixelAspectRatioKey { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kCVImageBufferPixelAspectRatioHorizontalSpacingKey")]
		NSString PixelAspectRatioHorizontalSpacingKey { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kCVImageBufferPixelAspectRatioVerticalSpacingKey")]
		NSString PixelAspectRatioVerticalSpacingKey { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kCVImageBufferDisplayDimensionsKey")]
		NSString DisplayDimensionsKey { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kCVImageBufferDisplayWidthKey")]
		NSString DisplayWidthKey { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kCVImageBufferDisplayHeightKey")]
		NSString DisplayHeightKey { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kCVImageBufferYCbCrMatrixKey")]
		NSString YCbCrMatrixKey { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Static]
		[Wrap ("CVImageBufferYCbCrMatrix.ItuR709_2.GetConstant ()")]
		NSString YCbCrMatrix_ITU_R_709_2 { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Static]
		[Wrap ("CVImageBufferYCbCrMatrix.ItuR601_4.GetConstant ()")]
		NSString YCbCrMatrix_ITU_R_601_4 { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Static]
		[Wrap ("CVImageBufferYCbCrMatrix.Smpte240M1995.GetConstant ()")]
		NSString YCbCrMatrix_SMPTE_240M_1995 { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Static]
		[Wrap ("CVImageBufferYCbCrMatrix.DciP3.GetConstant ()")]
		[MacCatalyst (13, 1)]
		NSString YCbCrMatrix_DCI_P3 { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Static]
		[Wrap ("CVImageBufferYCbCrMatrix.P3D65.GetConstant ()")]
		[MacCatalyst (13, 1)]
		NSString YCbCrMatrix_P3_D65 { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Static]
		[Wrap ("CVImageBufferYCbCrMatrix.ItuR2020.GetConstant ()")]
		[MacCatalyst (13, 1)]
		NSString YCbCrMatrix_ITU_R_2020 { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Static]
		[Wrap ("CVImageBufferColorPrimaries.DciP3.GetConstant ()")]
		[MacCatalyst (13, 1)]
		NSString ColorPrimaries_DCI_P3 { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Static]
		[Wrap ("CVImageBufferColorPrimaries.ItuR2020.GetConstant ()")]
		[MacCatalyst (13, 1)]
		NSString ColorPrimaries_ITU_R_2020 { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Static]
		[Wrap ("CVImageBufferColorPrimaries.P3D65.GetConstant ()")]
		[MacCatalyst (13, 1)]
		NSString ColorPrimaries_P3_D65 { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kCVImageBufferChromaSubsamplingKey")]
		NSString ChromaSubsamplingKey { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kCVImageBufferChromaSubsampling_420")]
		NSString ChromaSubsampling_420 { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kCVImageBufferChromaSubsampling_422")]
		NSString ChromaSubsampling_422 { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kCVImageBufferChromaSubsampling_411")]
		NSString ChromaSubsampling_411 { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kCVImageBufferTransferFunctionKey")]
		NSString TransferFunctionKey { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Static]
		[Wrap ("CVImageBufferTransferFunction.ItuR709_2.GetConstant ()")]
		NSString TransferFunction_ITU_R_709_2 { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Static]
		[Wrap ("CVImageBufferTransferFunction.Smpte240M1995.GetConstant ()")]
		NSString TransferFunction_SMPTE_240M_1995 { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Static]
		[Wrap ("CVImageBufferTransferFunction.UseGamma.GetConstant ()")]
		NSString TransferFunction_UseGamma { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Static]
		[Wrap ("CVImageBufferTransferFunction.ItuR2020.GetConstant ()")]
		NSString TransferFunction_ITU_R_2020 { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Static]
		[Wrap ("CVImageBufferTransferFunction.SmpteST428_1.GetConstant ()")]
		NSString TransferFunction_SMPTE_ST_428_1 { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Static]
		[Wrap ("CVImageBufferTransferFunction.SRgb.GetConstant ()")]
		NSString TransferFunction_sRGB { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Static]
		[Wrap ("CVImageBufferTransferFunction.SmpteST2084PQ.GetConstant ()")]
		NSString TransferFunction_SMPTE_ST_2084_PQ { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Static]
		[Wrap ("CVImageBufferTransferFunction.ItuR2100Hlg.GetConstant ()")]
		NSString TransferFunction_ITU_R_2100_HLG { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kCVImageBufferChromaLocationTopFieldKey")]
		NSString ChromaLocationTopFieldKey { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kCVImageBufferChromaLocationBottomFieldKey")]
		NSString ChromaLocationBottomFieldKey { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kCVImageBufferChromaLocation_Left")]
		NSString ChromaLocation_Left { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kCVImageBufferChromaLocation_Center")]
		NSString ChromaLocation_Center { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kCVImageBufferChromaLocation_TopLeft")]
		NSString ChromaLocation_TopLeft { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kCVImageBufferChromaLocation_Top")]
		NSString ChromaLocation_Top { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kCVImageBufferChromaLocation_BottomLeft")]
		NSString ChromaLocation_BottomLeft { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kCVImageBufferChromaLocation_Bottom")]
		NSString ChromaLocation_Bottom { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kCVImageBufferChromaLocation_DV420")]
		NSString ChromaLocation_DV420 { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kCVImageBufferColorPrimariesKey")]
		NSString ColorPrimariesKey { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Static]
		[Wrap ("CVImageBufferColorPrimaries.ItuR709_2.GetConstant ()")]
		NSString ColorPrimaries_ITU_R_709_2 { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Static]
		[Wrap ("CVImageBufferColorPrimaries.Ebu3213.GetConstant ()")]
		NSString ColorPrimaries_EBU_3213 { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Static]
		[Wrap ("CVImageBufferColorPrimaries.SmpteC.GetConstant ()")]
		NSString ColorPrimaries_SMPTE_C { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Static]
		[Wrap ("CVImageBufferColorPrimaries.P22.GetConstant ()")]
		NSString ColorPrimaries_P22 { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Field ("kCVImageBufferAlphaChannelIsOpaque")]
		NSString AlphaChannelIsOpaque { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Field ("kCVImageBufferMasteringDisplayColorVolumeKey")]
		NSString MasteringDisplayColorVolumeKey { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Field ("kCVImageBufferContentLightLevelInfoKey")]
		NSString ContentLightLevelInfoKey { get; }

		[TV (13, 0), iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("kCVImageBufferAlphaChannelModeKey")]
		NSString AlphaChannelModeKey { get; }

		[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
		[Field ("kCVImageBufferRegionOfInterestKey")]
		NSString RegionOfInterestKey { get; }

		[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
		[Field ("kCVImageBufferAmbientViewingEnvironmentKey")]
		NSString AmbientViewingEnvironmentKey { get; }

		[TV (17, 2), Mac (14, 2), iOS (17, 2), MacCatalyst (17, 2)]
		[Field ("kCVImageBufferLogTransferFunctionKey")]
		NSString LogTransferFunctionKey { get; }

		[TV (17, 2), Mac (14, 2), iOS (17, 2), MacCatalyst (17, 2)]
		[Field ("kCVImageBufferLogTransferFunction_AppleLog")]
		NSString LogTransferFunctionAppleLogKey { get; }

		[TV (18, 0), Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
		[Field ("kCVImageBufferSceneIlluminationKey")]
		NSString SceneIlluminationKey { get; }

		[Mac (15, 0), NoiOS, NoTV, NoMacCatalyst]
		[Field ("kCVImageBufferPostDecodeProcessingSequenceMetadataKey")]
		NSString PostDecodeProcessingSequenceMetadataKey { get; }

		[Mac (15, 0), NoiOS, NoTV, NoMacCatalyst]
		[Field ("kCVImageBufferPostDecodeProcessingFrameMetadataKey")]
		NSString PostDecodeProcessingFrameMetadataKey { get; }
	}

	[MacCatalyst (13, 1)]
	enum CVImageBufferTransferFunction {

		/// <summary>To be added.</summary>
		[Field (null)]
		Unknown = 2, // 2 (the code point for "unknown")

		/// <summary>To be added.</summary>
		[Field ("kCVImageBufferTransferFunction_ITU_R_709_2")]
		ItuR709_2,

		/// <summary>To be added.</summary>
		[Field ("kCVImageBufferTransferFunction_SMPTE_240M_1995")]
		Smpte240M1995,

		/// <summary>To be added.</summary>
		[Field ("kCVImageBufferTransferFunction_UseGamma")]
		UseGamma,

		/// <summary>To be added.</summary>
		[MacCatalyst (13, 1)]
		[Field ("kCVImageBufferTransferFunction_ITU_R_2020")]
		ItuR2020,

		/// <summary>To be added.</summary>
		[MacCatalyst (13, 1)]
		[Field ("kCVImageBufferTransferFunction_SMPTE_ST_428_1")]
		SmpteST428_1,

		/// <summary>To be added.</summary>
		[MacCatalyst (13, 1)]
		[Field ("kCVImageBufferTransferFunction_sRGB")]
		SRgb,

		/// <summary>To be added.</summary>
		[MacCatalyst (13, 1)]
		[Field ("kCVImageBufferTransferFunction_SMPTE_ST_2084_PQ")]
		SmpteST2084PQ,

		/// <summary>To be added.</summary>
		[MacCatalyst (13, 1)]
		[Field ("kCVImageBufferTransferFunction_ITU_R_2100_HLG")]
		ItuR2100Hlg,

		/// <summary>To be added.</summary>
		[MacCatalyst (13, 1)]
		[Field ("kCVImageBufferTransferFunction_Linear")]
		Linear,
	}

	[MacCatalyst (13, 1)]
	enum CVImageBufferColorPrimaries {

		/// <summary>To be added.</summary>
		[Field (null)]
		Unknown = 2, // 2 (the code point for "unknown")

		/// <summary>To be added.</summary>
		[Field ("kCVImageBufferColorPrimaries_DCI_P3")]
		[MacCatalyst (13, 1)]
		DciP3,

		/// <summary>To be added.</summary>
		[Field ("kCVImageBufferColorPrimaries_ITU_R_2020")]
		[MacCatalyst (13, 1)]
		ItuR2020,

		/// <summary>To be added.</summary>
		[Field ("kCVImageBufferColorPrimaries_P3_D65")]
		[MacCatalyst (13, 1)]
		P3D65,

		/// <summary>To be added.</summary>
		[Field ("kCVImageBufferColorPrimaries_ITU_R_709_2")]
		ItuR709_2,

		/// <summary>To be added.</summary>
		[Field ("kCVImageBufferColorPrimaries_EBU_3213")]
		Ebu3213,

		/// <summary>To be added.</summary>
		[Field ("kCVImageBufferColorPrimaries_SMPTE_C")]
		SmpteC,

		/// <summary>To be added.</summary>
		[Field ("kCVImageBufferColorPrimaries_P22")]
		P22,
	}

	[MacCatalyst (13, 1)]
	enum CVImageBufferYCbCrMatrix {

		/// <summary>To be added.</summary>
		[Field (null)]
		Unknown = 2, // 2 (the code point for "unknown")

		/// <summary>To be added.</summary>
		[Field ("kCVImageBufferYCbCrMatrix_ITU_R_709_2")]
		ItuR709_2,

		/// <summary>To be added.</summary>
		[Field ("kCVImageBufferYCbCrMatrix_ITU_R_601_4")]
		ItuR601_4,

		/// <summary>To be added.</summary>
		[Field ("kCVImageBufferYCbCrMatrix_SMPTE_240M_1995")]
		Smpte240M1995,

		/// <summary>To be added.</summary>
		[Deprecated (PlatformName.iOS, 14, 0, message: "This API is no longer supported.")]
		[Deprecated (PlatformName.TvOS, 14, 0, message: "This API is no longer supported.")]
		[Deprecated (PlatformName.MacOSX, 11, 0, message: "This API is no longer supported.")]
		[Field ("kCVImageBufferYCbCrMatrix_DCI_P3")]
		[MacCatalyst (13, 1)]
		[Deprecated (PlatformName.MacCatalyst, 14, 0, message: "This API is no longer supported.")]
		DciP3,

		/// <summary>To be added.</summary>
		[Deprecated (PlatformName.iOS, 14, 0, message: "This API is no longer supported.")]
		[Deprecated (PlatformName.TvOS, 14, 0, message: "This API is no longer supported.")]
		[Deprecated (PlatformName.MacOSX, 11, 0, message: "This API is no longer supported.")]
		[Field ("kCVImageBufferYCbCrMatrix_P3_D65")]
		[MacCatalyst (13, 1)]
		[Deprecated (PlatformName.MacCatalyst, 14, 0, message: "This API is no longer supported.")]
		P3D65,

		/// <summary>To be added.</summary>
		[Field ("kCVImageBufferYCbCrMatrix_ITU_R_2020")]
		[MacCatalyst (13, 1)]
		ItuR2020,
	}

	/// <summary>A <see cref="CoreVideo.CVImageBuffer" /> that holds pixels.</summary>
	[Partial]
	interface CVPixelBuffer {

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kCVPixelBufferPixelFormatTypeKey")]
		NSString PixelFormatTypeKey { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kCVPixelBufferMemoryAllocatorKey")]
		NSString MemoryAllocatorKey { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kCVPixelBufferWidthKey")]
		NSString WidthKey { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kCVPixelBufferHeightKey")]
		NSString HeightKey { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kCVPixelBufferExtendedPixelsLeftKey")]
		NSString ExtendedPixelsLeftKey { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kCVPixelBufferExtendedPixelsTopKey")]
		NSString ExtendedPixelsTopKey { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kCVPixelBufferExtendedPixelsRightKey")]
		NSString ExtendedPixelsRightKey { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kCVPixelBufferExtendedPixelsBottomKey")]
		NSString ExtendedPixelsBottomKey { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kCVPixelBufferBytesPerRowAlignmentKey")]
		NSString BytesPerRowAlignmentKey { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kCVPixelBufferCGBitmapContextCompatibilityKey")]
		NSString CGBitmapContextCompatibilityKey { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kCVPixelBufferCGImageCompatibilityKey")]
		NSString CGImageCompatibilityKey { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kCVPixelBufferOpenGLCompatibilityKey")]
		NSString OpenGLCompatibilityKey { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kCVPixelBufferIOSurfacePropertiesKey")]
		NSString IOSurfacePropertiesKey { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kCVPixelBufferPlaneAlignmentKey")]
		NSString PlaneAlignmentKey { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NoMac]
		[NoMacCatalyst]
		[Field ("kCVPixelBufferOpenGLESCompatibilityKey")]
		NSString OpenGLESCompatibilityKey { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NoMac]
		[NoMacCatalyst]
		[Field ("kCVPixelBufferOpenGLESTextureCacheCompatibilityKey")]
		NSString OpenGLESTextureCacheCompatibilityKey { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Field ("kCVPixelBufferMetalCompatibilityKey")]
		NSString MetalCompatibilityKey { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NoiOS, NoTV, NoMacCatalyst]
		[Field ("kCVPixelBufferOpenGLTextureCacheCompatibilityKey")]
		NSString OpenGLTextureCacheCompatibilityKey { get; }

		[NoTV, iOS (14, 0), MacCatalyst (15, 0)]
		[Field ("kCVPixelBufferProResRAWKey_BlackLevel")]
		NSString ProResRawKey_BlackLevel { get; }

		[NoTV, iOS (14, 0), MacCatalyst (15, 0)]
		[Field ("kCVPixelBufferProResRAWKey_ColorMatrix")]
		NSString ProResRawKey_ColorMatrix { get; }

		[NoTV, iOS (14, 0), MacCatalyst (15, 0)]
		[Field ("kCVPixelBufferProResRAWKey_GainFactor")]
		NSString ProResRawKey_GainFactor { get; }

		[NoTV, iOS (14, 0), MacCatalyst (15, 0)]
		[Field ("kCVPixelBufferProResRAWKey_RecommendedCrop")]
		NSString ProResRawKey_RecommendedCrop { get; }

		[NoTV, iOS (14, 0), MacCatalyst (15, 0)]
		[Field ("kCVPixelBufferProResRAWKey_SenselSitingOffsets")]
		NSString ProResRawKey_SenselSitingOffsets { get; }

		[NoTV, iOS (14, 0), MacCatalyst (15, 0)]
		[Field ("kCVPixelBufferProResRAWKey_WhiteBalanceBlueFactor")]
		NSString ProResRawKey_WhiteBalanceBlueFactor { get; }

		[NoTV, iOS (14, 0), MacCatalyst (15, 0)]
		[Field ("kCVPixelBufferProResRAWKey_WhiteBalanceCCT")]
		NSString ProResRawKey_WhiteBalanceCct { get; }

		[NoTV, iOS (14, 0), MacCatalyst (15, 0)]
		[Field ("kCVPixelBufferProResRAWKey_WhiteBalanceRedFactor")]
		NSString ProResRawKey_WhiteBalanceRedFactor { get; }

		[NoTV, iOS (14, 0), MacCatalyst (15, 0)]
		[Field ("kCVPixelBufferProResRAWKey_WhiteLevel")]
		NSString ProResRawKey_WhiteLevel { get; }

		[NoTV, iOS (14, 0), MacCatalyst (15, 0)]
		[Field ("kCVPixelBufferVersatileBayerKey_BayerPattern")]
		NSString VersatileBayerKey_BayerPattern { get; }

		[NoTV, iOS (15, 0), MacCatalyst (15, 0)]
		[Field ("kCVPixelBufferProResRAWKey_MetadataExtension")]
		NSString MetadataExtension { get; }
	}

	/// <summary>A reusable set of <see cref="CoreVideo.CVPixelBuffer" />s.</summary>
	[Partial]
	interface CVPixelBufferPool {
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kCVPixelBufferPoolMinimumBufferCountKey")]
		NSString MinimumBufferCountKey { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kCVPixelBufferPoolMaximumBufferAgeKey")]
		NSString MaximumBufferAgeKey { get; }
	}

	/// <summary>Cache to manage CVMetalTexture objects.</summary>
	[MacCatalyst (13, 1)]
	[Partial]
	interface CVMetalTextureCache {
		[MacCatalyst (13, 1)]
		[Internal]
		[Field ("kCVMetalTextureCacheMaximumTextureAgeKey")]
		IntPtr MaxTextureAge { get; }

		[TV (13, 0), iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("kCVMetalTextureStorageMode")]
		NSString StorageMode { get; }
	}

	// CVOpenGLESTextureCache is bound (manually) in OpenTK[-1.0].dll.
	// [Partial]
	// interface CVOpenGLESTextureCache {
	// 	[Internal]
	// 	[Field ("kCVOpenGLESTextureCacheMaximumTextureAgeKey")]
	// 	IntPtr MaxTextureAge { get; }
	// }

	[MacCatalyst (13, 1)]
	[Static, Internal]
	interface CVMetalTextureAttributesKeys {

		[Field ("kCVMetalTextureUsage")]
		NSString UsageKey { get; }
	}

	[MacCatalyst (13, 1)]
	[StrongDictionary ("CVMetalTextureAttributesKeys")]
	interface CVMetalTextureAttributes {
		// Create stub DictionaryContainer class
	}

	[NoTV, NoMac, iOS (14, 0)]
	[MacCatalyst (14, 0)]
	public enum CVVersatileBayerPattern : uint {
		Rggb = 0,
		Grbg = 1,
		Gbrg = 2,
		Bggr = 3,
	}

	[TV (18, 0), Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
	[Static, Internal]
	interface CVMetalBufferCacheAttributeKeys {
		[Field ("kCVMetalBufferCacheMaximumBufferAgeKey")]
		NSString MaximumBufferAgeKey { get; }
	}

	[StrongDictionary ("CVMetalBufferCacheAttributeKeys")]
	interface CVMetalBufferCacheAttributes {
		double MaximumBufferAge { get; }
	}

	[Partial]
	interface CVPixelFormatKeys {
		[Field ("kCVPixelFormatName")]
		NSString Name { get; }

		[Field ("kCVPixelFormatConstant")]
		NSString Constant { get; }

		[Field ("kCVPixelFormatCodecType")]
		NSString CodecType { get; }

		[Field ("kCVPixelFormatFourCC")]
		NSString FourCC { get; }

		[Field ("kCVPixelFormatContainsAlpha")]
		NSString ContainsAlpha { get; }

		[Field ("kCVPixelFormatContainsYCbCr")]
		NSString ContainsYCbCr { get; }

		[Field ("kCVPixelFormatContainsRGB")]
		NSString ContainsRgb { get; }

		[Field ("kCVPixelFormatContainsGrayscale")]
		NSString ContainsGrayscale { get; }

		[iOS (16, 0), Mac (13, 0), MacCatalyst (16, 0), TV (16, 0)]
		[Field ("kCVPixelFormatContainsSenselArray")]
		NSString ContainsSenselArray { get; }

		[Field ("kCVPixelFormatComponentRange")]
		NSString ComponentRange { get; }

		[Field ("kCVPixelFormatPlanes")]
		NSString Planes { get; }

		[Field ("kCVPixelFormatBlockWidth")]
		NSString BlockWidth { get; }

		[Field ("kCVPixelFormatBlockHeight")]
		NSString BlockHeight { get; }

		[Field ("kCVPixelFormatBitsPerBlock")]
		NSString BitsPerBlock { get; }

		[Field ("kCVPixelFormatBlockHorizontalAlignment")]
		NSString BlockHorizontalAlignment { get; }

		[Field ("kCVPixelFormatBlockVerticalAlignment")]
		NSString BlockVerticalAlignment { get; }

		[Field ("kCVPixelFormatBlackBlock")]
		NSString BlackBlock { get; }

		[Field ("kCVPixelFormatHorizontalSubsampling")]
		NSString HorizontalSubsampling { get; }

		[Field ("kCVPixelFormatVerticalSubsampling")]
		NSString VerticalSubsampling { get; }

		[Field ("kCVPixelFormatOpenGLFormat")]
		NSString OpenGLFormat { get; }

		[Field ("kCVPixelFormatOpenGLType")]
		NSString OpenGLType { get; }

		[Field ("kCVPixelFormatOpenGLInternalFormat")]
		NSString OpenGLInternalFormat { get; }

		[Field ("kCVPixelFormatCGBitmapInfo")]
		NSString CGBitmapInfo { get; }

		[Field ("kCVPixelFormatQDCompatibility")]
		NSString QDCompatibility { get; }

		[Field ("kCVPixelFormatCGBitmapContextCompatibility")]
		NSString CGBitmapContextCompatibility { get; }

		[Field ("kCVPixelFormatCGImageCompatibility")]
		NSString CGImageCompatibility { get; }

		[Field ("kCVPixelFormatOpenGLCompatibility")]
		NSString OpenGLCompatibility { get; }

		[NoMacCatalyst, NoMac]
		[Field ("kCVPixelFormatOpenGLESCompatibility")]
		NSString OpenGlesCompatibility { get; }

		[Field ("kCVPixelFormatFillExtendedPixelsCallback")]
		NSString FillExtendedPixelsCallback { get; }

		[iOS (18, 0), Mac (15, 0), MacCatalyst (18, 0), TV (18, 0)]
		[Field ("kCVPixelFormatBitsPerComponent")]
		NSString BitsPerComponent { get; }
	}

	[Partial]
	interface CVPixelFormatComponentRangeKeys {
		[Field ("kCVPixelFormatComponentRange_VideoRange")]
		NSString VideoRange { get; }

		[Field ("kCVPixelFormatComponentRange_FullRange")]
		NSString FullRange { get; }

		[Field ("kCVPixelFormatComponentRange_WideRange")]
		NSString WideRange { get; }
	}


	[StrongDictionary ("CVPixelFormatComponentRangeKeys", Suffix = "")]
	interface CVPixelFormatComponentRange {
		// there's no documentation about the type, so binding as NSObject
		NSObject VideoRange { get; set; }

		// there's no documentation about the type, so binding as NSObject
		NSObject FullRange { get; set; }

		// there's no documentation about the type, so binding as NSObject
		NSObject WideRange { get; set; }
	}

	[StrongDictionary ("CVPixelFormatKeys", Suffix = "")]
	interface CVPixelFormatDescription {
		string Name { get; set; }

		CVPixelFormatType Constant { get; set; }

		// Documentation says 'CFString', but it also says 'CFString' about another property which clearly isn't, so I don't trust the documentation.
		// Headers don't say, and tests don't show anything useful, there are no hits on Google for the underlying field, so leaving this typed as 'NSObject'.
		NSObject CodecType { get; set; }

		int FourCC { get; set; }

		bool ContainsAlpha { get; set; }

#if XAMCORE_5_0
		bool ContainsYCbCr { get; set; }
#else
		[Export ("ContainsYCbCr")]
		bool FormatContainsYCbCr { get; set; }
#endif

#if XAMCORE_5_0
		bool ContainsRgb { get; set; }
#else
		[Export ("ContainsRgb")]
		bool FormatContainsRgb { get; set; }
#endif

		bool ContainsGrayscale { get; set; }

		[iOS (16, 0), Mac (13, 0), MacCatalyst (16, 0), TV (16, 0)]
#if XAMCORE_5_0
		bool ContainsSenselArray { get; set; }
#else
		[Export ("ContainsSenselArray")]
		bool FormatContainsSenselArray { get; set; }
#endif

		CVPixelFormatComponentRange ComponentRange { get; set; }

		// This can be an array of dictionaries, or a single dictionary when there's only one plane, so we have to type as 'NSObject'.
		NSObject Planes { get; set; }

		int BlockWidth { get; set; }

		int BlockHeight { get; set; }

		int BitsPerBlock { get; set; }

		int BlockHorizontalAlignment { get; set; }

		int BlockVerticalAlignment { get; set; }

		NSData BlackBlock { get; set; }

		int HorizontalSubsampling { get; set; }

		int VerticalSubsampling { get; set; }

		int OpenGLFormat { get; set; }

		int OpenGLType { get; set; }

		int OpenGLInternalFormat { get; set; }

		CGBitmapFlags CGBitmapInfo { get; set; }

		bool QDCompatibility { get; set; }

		bool CGBitmapContextCompatibility { get; set; }

		bool CGImageCompatibility { get; set; }

		bool OpenGLCompatibility { get; set; }

		[NoMacCatalyst, NoMac]
		bool OpenGlesCompatibility { get; set; }

		NSData FillExtendedPixelsCallback { get; set; }

		[iOS (18, 0), Mac (15, 0), MacCatalyst (18, 0), TV (18, 0)]
		int BitsPerComponent { get; set; }
	}
}
