////
// VideoToolbox core types and enumerations
//
// Author: Miguel de Icaza (miguel@xamarin.com)
//         Alex Soto (alex.soto@xamarin.com)
//
// Copyright 2014 Xamarin Inc
//
using System;
using System.Collections.Generic;
using Foundation;
using ObjCRuntime;
using CoreMedia;
using AVFoundation;
using CoreVideo;
using Metal;

namespace VideoToolbox {

	/// <summary>A class that encapsulates keys necessary for compression sessions. Used by <see cref="VideoToolbox.VTCompressionProperties" /></summary>
	[MacCatalyst (13, 1)]
	[Static]
	interface VTCompressionPropertyKey {
		// Buffers

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kVTCompressionPropertyKey_NumberOfPendingFrames")]
		NSString NumberOfPendingFrames { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kVTCompressionPropertyKey_PixelBufferPoolIsShared")]
		NSString PixelBufferPoolIsShared { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kVTCompressionPropertyKey_VideoEncoderPixelBufferAttributes")]
		NSString VideoEncoderPixelBufferAttributes { get; }

		// Frame dependency

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kVTCompressionPropertyKey_MaxKeyFrameInterval")]
		NSString MaxKeyFrameInterval { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kVTCompressionPropertyKey_MaxKeyFrameIntervalDuration")]
		NSString MaxKeyFrameIntervalDuration { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kVTCompressionPropertyKey_AllowTemporalCompression")]
		NSString AllowTemporalCompression { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kVTCompressionPropertyKey_AllowFrameReordering")]
		NSString AllowFrameReordering { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Field ("kVTCompressionPropertyKey_AllowOpenGOP")]
		NSString AllowOpenGop { get; }

		// Rate control

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kVTCompressionPropertyKey_AverageBitRate")]
		NSString AverageBitRate { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kVTCompressionPropertyKey_DataRateLimits")]
		NSString DataRateLimits { get; } // NSArray of an even number of CFNumbers alternating [int, double](bytes, seconds] Read/write

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kVTCompressionPropertyKey_Quality")]
		NSString Quality { get; }

		[iOS (13, 0), TV (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("kVTCompressionPropertyKey_TargetQualityForAlpha")]
		NSString TargetQualityForAlpha { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kVTCompressionPropertyKey_MoreFramesBeforeStart")]
		NSString MoreFramesBeforeStart { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kVTCompressionPropertyKey_MoreFramesAfterEnd")]
		NSString MoreFramesAfterEnd { get; }

		// Bitstream configuration

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kVTCompressionPropertyKey_ProfileLevel")]
		NSString ProfileLevel { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kVTCompressionPropertyKey_H264EntropyMode")]
		[MacCatalyst (13, 1)]
		NSString H264EntropyMode { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kVTCompressionPropertyKey_Depth")]
		NSString Depth { get; }

		[Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0), TV (16, 0)]
		[Field ("kVTCompressionPropertyKey_PreserveAlphaChannel")]
		NSString PreserveAlphaChannel { get; }

		// Runtime restrictions

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kVTCompressionPropertyKey_MaxFrameDelayCount")]
		NSString MaxFrameDelayCount { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kVTCompressionPropertyKey_MaxH264SliceBytes")]
		NSString MaxH264SliceBytes { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kVTCompressionPropertyKey_RealTime")]
		[MacCatalyst (13, 1)]
		NSString RealTime { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kVTCompressionPropertyKey_MaximizePowerEfficiency")]
		[MacCatalyst (13, 1)]
		NSString MaximizePowerEfficiency { get; }

		// Hints

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kVTCompressionPropertyKey_SourceFrameCount")]
		NSString SourceFrameCount { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kVTCompressionPropertyKey_ExpectedFrameRate")]
		NSString ExpectedFrameRate { get; }

		[TV (18, 0), Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
		[Field ("kVTCompressionPropertyKey_MaximumRealTimeFrameRate")]
		NSString MaximumRealTimeFrameRate { get; }

		[TV (14, 5)]
		[iOS (14, 5)]
		[MacCatalyst (14, 5)]
		[Field ("kVTCompressionPropertyKey_BaseLayerFrameRateFraction")]
		NSString BaseLayerFrameRateFraction { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kVTCompressionPropertyKey_ExpectedDuration")]
		NSString ExpectedDuration { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Field ("kVTCompressionPropertyKey_BaseLayerFrameRate")]
		NSString BaseLayerFrameRate { get; }

		[Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0), TV (16, 0)]
		[Field ("kVTCompressionPropertyKey_ReferenceBufferCount")]
		NSString ReferenceBufferCount { get; }

		[Mac (14, 4), iOS (17, 4), TV (17, 4), MacCatalyst (17, 4)]
		[Field ("kVTCompressionPropertyKey_CalculateMeanSquaredError")]
		NSString CalculateMeanSquaredError { get; }

		// Hardware acceleration
		// Hardware acceleration is default behavior on iOS. No opt-in required.

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kVTCompressionPropertyKey_UsingHardwareAcceleratedVideoEncoder")]
		NSString UsingHardwareAcceleratedVideoEncoder { get; } // CFBoolean Read

		// Clean aperture and pixel aspect ratio

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kVTCompressionPropertyKey_CleanAperture")]
		NSString CleanAperture { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kVTCompressionPropertyKey_PixelAspectRatio")]
		NSString PixelAspectRatio { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kVTCompressionPropertyKey_FieldCount")]
		NSString FieldCount { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kVTCompressionPropertyKey_FieldDetail")]
		NSString FieldDetail { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kVTCompressionPropertyKey_AspectRatio16x9")]
		NSString AspectRatio16x9 { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kVTCompressionPropertyKey_ProgressiveScan")]
		NSString ProgressiveScan { get; }

		// Color

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kVTCompressionPropertyKey_ColorPrimaries")]
		NSString ColorPrimaries { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kVTCompressionPropertyKey_TransferFunction")]
		NSString TransferFunction { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kVTCompressionPropertyKey_YCbCrMatrix")]
		NSString YCbCrMatrix { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kVTCompressionPropertyKey_ICCProfile")]
		NSString ICCProfile { get; }

		// AlphaChannelMode

		[iOS (13, 0), TV (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("kVTCompressionPropertyKey_AlphaChannelMode")]
		NSString AlphaChannelMode { get; }

		[iOS (13, 0), TV (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("kVTCompressionPropertyKey_GammaLevel")]
		NSString GammaLevel { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Field ("kVTCompressionPropertyKey_MasteringDisplayColorVolume")]
		NSString MasteringDisplayColorVolume { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Field ("kVTCompressionPropertyKey_ContentLightLevelInfo")]
		NSString ContentLightLevelInfo { get; }

		// Pre-compression processing

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kVTCompressionPropertyKey_PixelTransferProperties")]
		NSString PixelTransferProperties { get; }

		// Multi-pass

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kVTCompressionPropertyKey_MultiPassStorage")]
		[MacCatalyst (13, 1)]
		NSString MultiPassStorage { get; }

		// Encoder information

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kVTCompressionPropertyKey_EncoderID")]
		[MacCatalyst (13, 1)]
		NSString EncoderId { get; }

		[NoiOS, NoTV, NoMacCatalyst, Mac (14, 0)]
		[Field ("kVTCompressionPropertyKey_RecommendedParallelizationLimit")]
		NSString RecommendedParallelizationLimit { get; }

		[NoiOS, NoTV, NoMacCatalyst, Mac (14, 0)]
		[Field ("kVTCompressionPropertyKey_RecommendedParallelizedSubdivisionMinimumFrameCount")]
		NSString RecommendedParallelizedSubdivisionMinimumFrameCount { get; }

		[NoiOS, NoTV, NoMacCatalyst, Mac (14, 0)]
		[Field ("kVTCompressionPropertyKey_RecommendedParallelizedSubdivisionMinimumDuration")]
		NSString RecommendedParallelizedSubdivisionMinimumDuration { get; }

		[iOS (13, 0), TV (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("kVTCompressionPropertyKey_UsingGPURegistryID")]
		NSString UsingGpuRegistryId { get; }

		[TV (14, 0), iOS (14, 0)]
		[MacCatalyst (14, 0)]
		[Field ("kVTCompressionPropertyKey_HDRMetadataInsertionMode")]
		NSString HdrMetadataInsertionMode { get; }

		[TV (14, 0), iOS (14, 0)]
		[MacCatalyst (14, 0)]
		[Field ("kVTCompressionPropertyKey_PrioritizeEncodingSpeedOverQuality")]
		NSString PrioritizeEncodingSpeedOverQuality { get; }

		[Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0), TV (16, 0)]
		[Field ("kVTCompressionPropertyKey_ConstantBitRate")]
		NSString ConstantBitRate { get; }

		[Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0), TV (16, 0)]
		[Field ("kVTCompressionPropertyKey_EstimatedAverageBytesPerFrame")]
		NSString EstimatedAverageBytesPerFrame { get; }

		[iOS (14, 1)]
		[TV (14, 2)]
		[MacCatalyst (14, 1)]
		[Field ("kVTCompressionPropertyKey_PreserveDynamicHDRMetadata")]
		NSString PreserveDynamicHdrMetadata { get; }

		[TV (14, 5)]
		[iOS (14, 5)]
		[MacCatalyst (14, 5)]
		[Field ("kVTVideoEncoderSpecification_EnableLowLatencyRateControl")]
		NSString EnableLowLatencyRateControl { get; }

		[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
		[Field ("kVTCompressionPropertyKey_BaseLayerBitRateFraction")]
		NSString BaseLayerBitRateFraction { get; }

		[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
		[Field ("kVTCompressionPropertyKey_EnableLTR")]
		NSString EnableLtr { get; }

		[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
		[Field ("kVTCompressionPropertyKey_MaxAllowedFrameQP")]
		NSString MaxAllowedFrameQP { get; }

		[Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0), TV (16, 0)]
		[Field ("kVTCompressionPropertyKey_MinAllowedFrameQP")]
		NSString MinAllowedFrameQP { get; }

		[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
		[Field ("kVTCompressionPropertyKey_SupportsBaseFrameQP")]
		NSString SupportsBaseFrameQP { get; }

		[TV (15, 4), Mac (12, 3), iOS (15, 4), MacCatalyst (15, 4)]
		[Field ("kVTCompressionPropertyKey_OutputBitDepth")]
		NSString OutputBitDepth { get; }

		[NoTV, Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
		[Field ("kVTCompressionPropertyKey_ProjectionKind")]
		NSString ProjectionKind { get; }

		[NoTV, Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
		[Field ("kVTCompressionPropertyKey_ViewPackingKind")]
		NSString ViewPackingKind { get; }

		[NoTV, Mac (15, 0), NoiOS, NoMacCatalyst]
		[Field ("kVTCompressionPropertyKey_SuggestedLookAheadFrameCount")]
		NSString SuggestedLookAheadFrameCount { get; }

		[NoTV, Mac (15, 0), NoiOS, NoMacCatalyst]
		[Field ("kVTCompressionPropertyKey_SpatialAdaptiveQPLevel")]
		NSString SpatialAdaptiveQPLevel { get; }

		[iOS (17, 0), NoTV, MacCatalyst (17, 0), Mac (14, 0)]
		[Field ("kVTCompressionPropertyKey_MVHEVCVideoLayerIDs")]
		NSString MvHevcVideoLayerIds { get; }

		[iOS (17, 0), NoTV, MacCatalyst (17, 0), Mac (14, 0)]
		[Field ("kVTCompressionPropertyKey_MVHEVCViewIDs")]
		NSString MvHevcViewIds { get; }

		[iOS (17, 0), NoTV, MacCatalyst (17, 0), Mac (14, 0)]
		[Field ("kVTCompressionPropertyKey_MVHEVCLeftAndRightViewIDs")]
		NSString MvHevcLeftAndRightViewIds { get; }

		[iOS (17, 0), NoTV, MacCatalyst (17, 0), Mac (14, 0)]
		[Field ("kVTCompressionPropertyKey_HeroEye")]
		NSString HeroEye { get; }

		[iOS (17, 0), NoTV, MacCatalyst (17, 0), Mac (14, 0)]
		[Field ("kVTCompressionPropertyKey_StereoCameraBaseline")]
		NSString StereoCameraBaseline { get; }

		[iOS (17, 0), NoTV, MacCatalyst (17, 0), Mac (14, 0)]
		[Field ("kVTCompressionPropertyKey_HorizontalDisparityAdjustment")]
		NSString HorizontalDisparityAdjustment { get; }

		[iOS (17, 0), NoTV, MacCatalyst (17, 0), Mac (14, 0)]
		[Field ("kVTCompressionPropertyKey_HasLeftStereoEyeView")]
		NSString HasLeftStereoEyeView { get; }

		[iOS (17, 0), NoTV, MacCatalyst (17, 0), Mac (14, 0)]
		[Field ("kVTCompressionPropertyKey_HasRightStereoEyeView")]
		NSString HasRightStereoEyeView { get; }

		[iOS (17, 0), NoTV, MacCatalyst (17, 0), Mac (14, 0)]
		[Field ("kVTCompressionPropertyKey_HorizontalFieldOfView")]
		NSString HorizontalFieldOfView { get; }

	}

	[iOS (13, 0), TV (13, 0)]
	[MacCatalyst (13, 1)]
	enum VTAlphaChannelMode {
		[Field ("kVTAlphaChannelMode_StraightAlpha")]
		StraightAlpha,
		[DefaultEnumValue]
		[Field ("kVTAlphaChannelMode_PremultipliedAlpha")]
		PremultipliedAlpha,
	}

	/// <summary>Strongly typed set of options for compression sessions</summary>
	[MacCatalyst (13, 1)]
	[StrongDictionary ("VTCompressionPropertyKey")]
	interface VTCompressionProperties {

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("NumberOfPendingFrames")]
		int NumberOfPendingFrames { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("PixelBufferPoolIsShared")]
		bool PixelBufferPoolIsShared { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("VideoEncoderPixelBufferAttributes")]
		NSDictionary VideoEncoderPixelBufferAttributes { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("MaxKeyFrameInterval")]
		int MaxKeyFrameInterval { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("MaxKeyFrameIntervalDuration")]
		double MaxKeyFrameIntervalDuration { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("AllowTemporalCompression")]
		bool AllowTemporalCompression { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("AllowFrameReordering")]
		bool AllowFrameReordering { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("AllowOpenGop")]
		bool AllowOpenGop { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("AverageBitRate")]
		int AverageBitRate { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("Quality")]
		float Quality { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("MoreFramesBeforeStart")]
		bool MoreFramesBeforeStart { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("MoreFramesAfterEnd")]
		bool MoreFramesAfterEnd { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("Depth")]
		CMPixelFormat Depth { get; set; }

		[Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0), TV (16, 0)]
		[Export ("PreserveAlphaChannel")]
		bool PreserveAlphaChannel { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("MaxFrameDelayCount")]
		int MaxFrameDelayCount { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("MaxH264SliceBytes")]
		int MaxH264SliceBytes { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("RealTime")]
		bool RealTime { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("MaximizePowerEfficiency")]
		bool MaximizePowerEfficiency { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("SourceFrameCount")]
		uint SourceFrameCount { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("ExpectedFrameRate")]
		double ExpectedFrameRate { get; set; }

		[TV (18, 0), Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
		[Export ("MaximumRealTimeFrameRate")]
		double MaximumRealTimeFrameRate { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("ExpectedDuration")]
		double ExpectedDuration { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("BaseLayerFrameRate")]
		double BaseLayerFrameRate { get; set; }

		[Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0), TV (16, 0)]
		[Export ("ReferenceBufferCount")]
		long ReferenceBufferCount { get; }

		[Mac (14, 4), iOS (17, 4), TV (17, 4), MacCatalyst (17, 4)]
		[Export ("CalculateMeanSquaredError")]
		bool CalculateMeanSquaredError { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("UsingHardwareAcceleratedVideoEncoder")]
		bool UsingHardwareAcceleratedVideoEncoder { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("CleanAperture")]
		NSDictionary CleanAperture { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("PixelAspectRatio")]
		NSDictionary PixelAspectRatio { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("FieldCount")]
		VTFieldCount FieldCount { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("AspectRatio16x9")]
		bool AspectRatio16x9 { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("ProgressiveScan")]
		bool ProgressiveScan { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("ICCProfile")]
		NSData ICCProfile { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("MasteringDisplayColorVolume")]
		NSData MasteringDisplayColorVolume { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("ContentLightLevelInfo")]
		NSData ContentLightLevelInfo { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("PixelTransferProperties")]
		NSDictionary PixelTransferProperties { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("EncoderId")]
		string EncoderId { get; set; }

		[NoiOS, NoTV, NoMacCatalyst, Mac (14, 0)]
		[Export ("RecommendedParallelizationLimit")]
		int RecommendedParallelizationLimit { get; }

		[NoiOS, NoTV, NoMacCatalyst, Mac (14, 0)]
		[Export ("RecommendedParallelizedSubdivisionMinimumFrameCount")]
		ulong RecommendedParallelizedSubdivisionMinimumFrameCount { get; }

		[NoiOS, NoTV, NoMacCatalyst, Mac (14, 0)]
		[Export ("RecommendedParallelizedSubdivisionMinimumDuration")]
		NSDictionary RecommendedParallelizedSubdivisionMinimumDuration { get; }

		[iOS (13, 0), TV (13, 0)]
		[MacCatalyst (13, 1)]
		[Export ("TargetQualityForAlpha")]
		float TargetQualityForAlpha { get; set; }

		[iOS (13, 0), TV (13, 0)]
		[MacCatalyst (13, 1)]
		[Export ("GammaLevel")]
		double GammaLevel { get; set; }

		[iOS (13, 0), TV (13, 0)]
		[MacCatalyst (13, 1)]
		[Export ("UsingGpuRegistryId")]
		uint UsingGpuRegistryId { get; }

		[Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0), TV (16, 0)]
		[Export ("ConstantBitRate")]
		long ConstantBitRate { get; set; }

		[Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0), TV (16, 0)]
		[Export ("EstimatedAverageBytesPerFrame")]
		long EstimatedAverageBytesPerFrame { get; }

		[iOS (14, 1)]
		[TV (14, 2)]
		[MacCatalyst (14, 1)]
		[Export ("PreserveDynamicHdrMetadata")]
		bool PreserveDynamicHdrMetadata { get; set; }

		[TV (14, 5)]
		[iOS (14, 5)]
		[MacCatalyst (14, 5)]
		[Export ("EnableLowLatencyRateControl")]
		bool EnableLowLatencyRateControl { get; set; }

		[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
		[Export ("BaseLayerBitRateFraction")]
		float BaseLayerBitRateFraction { get; set; }

		[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
		[Export ("EnableLtr")]
		bool EnableLtr { get; set; }

		[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
		[Export ("MaxAllowedFrameQP")]
		uint MaxAllowedFrameQP { get; set; }

		[Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0), TV (16, 0)]
		[Export ("MinAllowedFrameQP")]
		uint MinAllowedFrameQP { get; }

		[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
		[Export ("SupportsBaseFrameQP")]
		bool SupportsBaseFrameQP { get; }

		[TV (15, 4), Mac (12, 3), iOS (15, 4), MacCatalyst (15, 4)]
		[Export ("OutputBitDepth")]
		bool OutputBitDepth { get; set; }

		[NoTV, Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
		[Export ("ProjectionKind")]
		CMFormatDescriptionProjectionKind /* NSString */ ProjectionKind { get; }

		[NoTV, Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
		[Export ("ViewPackingKind")]
		CMFormatDescriptionViewPackingKind /* NSString */ ViewPackingKind { get; }

		[NoTV, Mac (15, 0), NoiOS, NoMacCatalyst]
		[Export ("SuggestedLookAheadFrameCount")]
		nint /* NSNumber */ SuggestedLookAheadFrameCount { get; }

		[NoTV, Mac (15, 0), NoiOS, NoMacCatalyst]
		[Export ("SpatialAdaptiveQPLevel")]
		VTQPModulationLevel /* NSNumber */ SpatialAdaptiveQPLevel { get; }

		[iOS (17, 0), NoTV, MacCatalyst (17, 0), Mac (14, 0)]
		[Export ("MvHevcVideoLayerIds")]
		NSNumber [] MvHevcVideoLayerIds { get; }

		[iOS (17, 0), NoTV, MacCatalyst (17, 0), Mac (14, 0)]
		[Export ("MvHevcViewIds")]
		NSNumber [] MvHevcViewIds { get; }

		[iOS (17, 0), NoTV, MacCatalyst (17, 0), Mac (14, 0)]
		[Export ("MvHevcLeftAndRightViewIds")]
		NSNumber [] MvHevcLeftAndRightViewIds { get; }

		[iOS (17, 0), NoTV, MacCatalyst (17, 0), Mac (14, 0)]
		[Export ("HeroEye")]
		string HeroEye { get; }

		[iOS (17, 0), NoTV, MacCatalyst (17, 0), Mac (14, 0)]
		[Export ("StereoCameraBaseline")]
		uint StereoCameraBaseline { get; }

		[iOS (17, 0), NoTV, MacCatalyst (17, 0), Mac (14, 0)]
		[Export ("HorizontalDisparityAdjustment")]
		int HorizontalDisparityAdjustment { get; }

		[iOS (17, 0), NoTV, MacCatalyst (17, 0), Mac (14, 0)]
		[Export ("HasLeftStereoEyeView")]
		bool HasLeftStereoEyeView { get; }

		[iOS (17, 0), NoTV, MacCatalyst (17, 0), Mac (14, 0)]
		[Export ("HasRightStereoEyeView")]
		bool HasRightStereoEyeView { get; }

		[iOS (17, 0), NoTV, MacCatalyst (17, 0), Mac (14, 0)]
		[Export ("HorizontalFieldOfView")]
		uint HorizontalFieldOfView { get; }
	}

	[NoTV, Mac (15, 0), NoiOS, NoMacCatalyst]
	public enum VTQPModulationLevel {
		Default = -1,
		Disable = 0,
	}

	/// <summary>A class that encapsulates keys necessary by <see cref="VideoToolbox.VTProfileLevel" />.</summary>
	[MacCatalyst (13, 1)]
	[Static]
	interface VTProfileLevelKeys {
		// HEVC

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Field ("kVTProfileLevel_HEVC_Main_AutoLevel")]
		NSString Hevc_Main_AutoLevel { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Field ("kVTProfileLevel_HEVC_Main10_AutoLevel")]
		NSString Hevc_Main10_AutoLevel { get; }

		[TV (15, 4), Mac (12, 3), iOS (15, 4), MacCatalyst (15, 4)]
		[Field ("kVTProfileLevel_HEVC_Main42210_AutoLevel")]
		NSString Hevc_Main42210_AutoLevel { get; }

		[Field ("kVTProfileLevel_HEVC_Monochrome_AutoLevel")]
		NSString Hevc_Monochrome_AutoLevel { get; }

		[TV (13, 0), iOS (13, 0), MacCatalyst (13, 0)]
		[Field ("kVTProfileLevel_HEVC_Monochrome10_AutoLevel")]
		NSString Hevc_Monochrome10_AutoLevel { get; }

		// H264

		/// <summary>The key for H.264, baseline profile, level 1.3.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kVTProfileLevel_H264_Baseline_1_3")]
		NSString H264_Baseline_1_3 { get; }

		/// <summary>The key for H.264, baseline profile, level 3.0.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kVTProfileLevel_H264_Baseline_3_0")]
		NSString H264_Baseline_3_0 { get; }

		/// <summary>The key for H.264, baseline profile, level 3.1.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kVTProfileLevel_H264_Baseline_3_1")]
		NSString H264_Baseline_3_1 { get; }

		/// <summary>The key for H.264, baseline profile, level 3.2.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kVTProfileLevel_H264_Baseline_3_2")]
		NSString H264_Baseline_3_2 { get; }

		/// <summary>The key for H.264, baseline profile, level 4.0.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kVTProfileLevel_H264_Baseline_4_0")]
		[MacCatalyst (13, 1)]
		NSString H264_Baseline_4_0 { get; }

		/// <summary>The key for H.264, baseline profile, level 4.1.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kVTProfileLevel_H264_Baseline_4_1")]
		NSString H264_Baseline_4_1 { get; }

		/// <summary>The key for H.264, baseline profile, level 4.2.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kVTProfileLevel_H264_Baseline_4_2")]
		[MacCatalyst (13, 1)]
		NSString H264_Baseline_4_2 { get; }

		/// <summary>The key for H.264, baseline profile, level 5.0.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kVTProfileLevel_H264_Baseline_5_0")]
		[MacCatalyst (13, 1)]
		NSString H264_Baseline_5_0 { get; }

		/// <summary>The key for H.264, baseline profile, level 5.1.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kVTProfileLevel_H264_Baseline_5_1")]
		[MacCatalyst (13, 1)]
		NSString H264_Baseline_5_1 { get; }

		/// <summary>The key for H.264, baseline profile, level 5.2.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kVTProfileLevel_H264_Baseline_5_2")]
		[MacCatalyst (13, 1)]
		NSString H264_Baseline_5_2 { get; }

		/// <summary>The key for H.264, baseline profile, with automatically determined level.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kVTProfileLevel_H264_Baseline_AutoLevel")]
		[MacCatalyst (13, 1)]
		NSString H264_Baseline_AutoLevel { get; }

		/// <summary>The key for H.264, main profile, level 3.0.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kVTProfileLevel_H264_Main_3_0")]
		NSString H264_Main_3_0 { get; }

		/// <summary>The key for H.264, main profile, level 3.1.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kVTProfileLevel_H264_Main_3_1")]
		NSString H264_Main_3_1 { get; }

		/// <summary>The key for H.264, main profile, level 3.2.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kVTProfileLevel_H264_Main_3_2")]
		NSString H264_Main_3_2 { get; }

		/// <summary>The key for H.264, main profile, level 4.0.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kVTProfileLevel_H264_Main_4_0")]
		NSString H264_Main_4_0 { get; }

		/// <summary>The key for H.264, main profile, level 4.1.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kVTProfileLevel_H264_Main_4_1")]
		NSString H264_Main_4_1 { get; }

		/// <summary>The key for H.264, main profile, level 4.2.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kVTProfileLevel_H264_Main_4_2")]
		[MacCatalyst (13, 1)]
		NSString H264_Main_4_2 { get; }

		/// <summary>The key for H.264, main profile, level 5.0.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kVTProfileLevel_H264_Main_5_0")]
		NSString H264_Main_5_0 { get; }

		/// <summary>The key for H.264, main profile, level 5.1.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kVTProfileLevel_H264_Main_5_1")]
		[MacCatalyst (13, 1)]
		NSString H264_Main_5_1 { get; }

		/// <summary>The key for H.264, main profile, level 5.2.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kVTProfileLevel_H264_Main_5_2")]
		[MacCatalyst (13, 1)]
		NSString H264_Main_5_2 { get; }

		/// <summary>The key for H.264, main profile, with automatically determined level.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kVTProfileLevel_H264_Main_AutoLevel")]
		[MacCatalyst (13, 1)]
		NSString H264_Main_AutoLevel { get; }

		/// <summary>The key for H.264, extended profile, level 5.0.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kVTProfileLevel_H264_Extended_5_0")]
		NSString H264_Extended_5_0 { get; }

		/// <summary>The key for H.264, extended profile, with automatically determined level.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kVTProfileLevel_H264_Extended_AutoLevel")]
		[MacCatalyst (13, 1)]
		NSString H264_Extended_AutoLevel { get; }

		/// <summary>The key for H.264, high profile, level 3.0.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kVTProfileLevel_H264_High_3_0")]
		[MacCatalyst (13, 1)]
		NSString H264_High_3_0 { get; }

		/// <summary>The key for H.264, high profile, level 3.1.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kVTProfileLevel_H264_High_3_1")]
		[MacCatalyst (13, 1)]
		NSString H264_High_3_1 { get; }

		/// <summary>The key for H.264, high profile, level 3.2.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kVTProfileLevel_H264_High_3_2")]
		[MacCatalyst (13, 1)]
		NSString H264_High_3_2 { get; }

		/// <summary>The key for H.264, high profile, level 4.0.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kVTProfileLevel_H264_High_4_0")]
		[MacCatalyst (13, 1)]
		NSString H264_High_4_0 { get; }

		/// <summary>The key for H.264, high profile, level 4.1.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kVTProfileLevel_H264_High_4_1")]
		[MacCatalyst (13, 1)]
		NSString H264_High_4_1 { get; }

		/// <summary>The key for H.264, high profile, level 4.2.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kVTProfileLevel_H264_High_4_2")]
		[MacCatalyst (13, 1)]
		NSString H264_High_4_2 { get; }

		/// <summary>The key for H.264, high profile, level 5.0.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kVTProfileLevel_H264_High_5_0")]
		NSString H264_High_5_0 { get; }

		/// <summary>The key for H.264, high profile, level 5.1.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kVTProfileLevel_H264_High_5_1")]
		[MacCatalyst (13, 1)]
		NSString H264_High_5_1 { get; }

		/// <summary>The key for H.264, high profile, level 5.2.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kVTProfileLevel_H264_High_5_2")]
		[MacCatalyst (13, 1)]
		NSString H264_High_5_2 { get; }

		/// <summary>The key for H.264, high profile, with automatically determined level.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kVTProfileLevel_H264_High_AutoLevel")]
		[MacCatalyst (13, 1)]
		NSString H264_High_AutoLevel { get; }

		[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
		[Field ("kVTProfileLevel_H264_ConstrainedBaseline_AutoLevel")]
		NSString H264_ConstrainedBaseline_AutoLevel { get; }

		[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
		[Field ("kVTProfileLevel_H264_ConstrainedHigh_AutoLevel")]
		NSString H264_ConstrainedHigh_AutoLevel { get; }

		// MP4V

		/// <summary>The key for MPEG-4 video, simple profile, level 0.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kVTProfileLevel_MP4V_Simple_L0")]
		NSString MP4V_Simple_L0 { get; }

		/// <summary>The key for MPEG-4 video, simple profile, level 1.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kVTProfileLevel_MP4V_Simple_L1")]
		NSString MP4V_Simple_L1 { get; }

		/// <summary>The key for MPEG-4 video, simple profile, level 2.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kVTProfileLevel_MP4V_Simple_L2")]
		NSString MP4V_Simple_L2 { get; }

		/// <summary>The key for MPEG-4 video, simple profile, level 3.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kVTProfileLevel_MP4V_Simple_L3")]
		NSString MP4V_Simple_L3 { get; }

		/// <summary>The key for MPEG-4 video, main profile, level 2.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kVTProfileLevel_MP4V_Main_L2")]
		NSString MP4V_Main_L2 { get; }

		/// <summary>The key for MPEG-4 video, main profile, level 3.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kVTProfileLevel_MP4V_Main_L3")]
		NSString MP4V_Main_L3 { get; }

		/// <summary>The key for MPEG-4 video, main profile, level 4.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kVTProfileLevel_MP4V_Main_L4")]
		NSString MP4V_Main_L4 { get; }

		/// <summary>The key for MPEG-4 video, advanced simple profile, level 0.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kVTProfileLevel_MP4V_AdvancedSimple_L0")]
		NSString MP4V_AdvancedSimple_L0 { get; }

		/// <summary>The key for MPEG-4 video, advanced simple profile, level 1.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kVTProfileLevel_MP4V_AdvancedSimple_L1")]
		NSString MP4V_AdvancedSimple_L1 { get; }

		/// <summary>The key for MPEG-4 video, advanced simple profile, level 2.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kVTProfileLevel_MP4V_AdvancedSimple_L2")]
		NSString MP4V_AdvancedSimple_L2 { get; }

		/// <summary>The key for MPEG-4 video, advanced simple profile, level 3.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kVTProfileLevel_MP4V_AdvancedSimple_L3")]
		NSString MP4V_AdvancedSimple_L3 { get; }

		/// <summary>The key for MPEG-4 video, advanced simple profile, level 4.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kVTProfileLevel_MP4V_AdvancedSimple_L4")]
		NSString MP4V_AdvancedSimple_L4 { get; }

		// H263

		/// <summary>The key for H.263, profile 0, level 1.0.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kVTProfileLevel_H263_Profile0_Level10")]
		NSString H263_Profile0_Level10 { get; }

		/// <summary>The key for H.263, profile 0, level 4.5.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kVTProfileLevel_H263_Profile0_Level45")]
		NSString H263_Profile0_Level45 { get; }

		/// <summary>The key for H.263, profile 3, level 4.5.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kVTProfileLevel_H263_Profile3_Level45")]
		NSString H263_Profile3_Level45 { get; }
	}

	/// <summary>A class that encapsulates keys necessary by <see cref="VideoToolbox.VTH264EntropyMode" />.</summary>
	[Static]
	[MacCatalyst (13, 1)]
	interface VTH264EntropyModeKeys {
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kVTH264EntropyMode_CAVLC")]
		NSString CAVLC { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kVTH264EntropyMode_CABAC")]
		NSString CABAC { get; }
	}

	/// <summary>Strongly typed representation of a video encoder.</summary>
	[MacCatalyst (13, 1)]
	[StrongDictionary ("VTVideoEncoderSpecificationKeys")]
	interface VTVideoEncoderSpecification {

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[iOS (17, 4), TV (17, 4), MacCatalyst (17, 4)]
		[Export ("EnableHardwareAcceleratedVideoEncoder")]
		bool EnableHardwareAcceleratedVideoEncoder { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[iOS (17, 4), TV (17, 4), MacCatalyst (17, 4)]
		[Export ("RequireHardwareAcceleratedVideoEncoder")]
		bool RequireHardwareAcceleratedVideoEncoder { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("EncoderID")]
		string EncoderID { get; set; }

		[iOS (13, 0), TV (13, 0)]
		[MacCatalyst (13, 1)]
		[Export ("RequiredEncoderGpuRegistryId")]
		uint RequiredEncoderGpuRegistryId { get; set; }

		[iOS (13, 0), TV (13, 0)]
		[MacCatalyst (13, 1)]
		[Export ("PreferredEncoderGpuRegistryId")]
		uint PreferredEncoderGpuRegistryId { get; set; }
	}

	/// <summary>A class that encapsulates keys necessary by <see cref="VideoToolbox.VTVideoEncoderSpecification" />.</summary>
	[MacCatalyst (13, 1)]
	[Static]
	interface VTVideoEncoderSpecificationKeys {

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kVTVideoEncoderSpecification_EnableHardwareAcceleratedVideoEncoder")]
		[iOS (17, 4), TV (17, 4), MacCatalyst (17, 4)]
		NSString EnableHardwareAcceleratedVideoEncoder { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kVTVideoEncoderSpecification_RequireHardwareAcceleratedVideoEncoder")]
		[iOS (17, 4), TV (17, 4), MacCatalyst (17, 4)]
		NSString RequireHardwareAcceleratedVideoEncoder { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kVTVideoEncoderSpecification_EncoderID")]
		NSString EncoderID { get; }

		[iOS (13, 0), TV (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("kVTVideoEncoderSpecification_RequiredEncoderGPURegistryID")]
		NSString RequiredEncoderGpuRegistryId { get; }

		[iOS (13, 0), TV (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("kVTVideoEncoderSpecification_PreferredEncoderGPURegistryID")]
		NSString PreferredEncoderGpuRegistryId { get; }
	}

	/// <summary>Strongly typed set of options.</summary>
	[MacCatalyst (13, 1)]
	[StrongDictionary ("VTEncodeFrameOptionKey")]
	interface VTEncodeFrameOptions {

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("ForceKeyFrame")]
		bool ForceKeyFrame { get; set; }
	}

	/// <summary>A class that encapsulates keys necessary by <see cref="VideoToolbox.VTEncodeFrameOptions" /></summary>
	[MacCatalyst (13, 1)]
	[Static]
	interface VTEncodeFrameOptionKey {
		// Per-frame configuration

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kVTEncodeFrameOptionKey_ForceKeyFrame")]
		NSString ForceKeyFrame { get; }

		[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
		[Field ("kVTEncodeFrameOptionKey_AcknowledgedLTRTokens")]
		NSString AcknowledgedLtrTokens { get; }

		[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
		[Field ("kVTEncodeFrameOptionKey_BaseFrameQP")]
		NSString BaseFrameQP { get; }

		[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
		[Field ("kVTEncodeFrameOptionKey_ForceLTRRefresh")]
		NSString ForceLtrRefresh { get; }

		[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
		[Field ("kVTSampleAttachmentKey_RequireLTRAcknowledgementToken")]
		NSString RequireLtrAcknowledgementToken { get; }
	}

	[Static]
	interface VTSampleAttachmentKey {
		[TV (15, 0), Mac (12, 0), iOS (15, 0), MacCatalyst (15, 0)]
		[Field ("kVTSampleAttachmentKey_RequireLTRAcknowledgementToken")]
		NSString RequireLtrAcknowledgementToken { get; }

		[iOS (17, 4), TV (17, 4), Mac (14, 4), MacCatalyst (17, 4)]
		[Field ("kVTSampleAttachmentKey_QualityMetrics")]
		NSString QualityMetrics { get; }
	}

	[Static]
	[iOS (17, 4), TV (17, 4), Mac (14, 4), MacCatalyst (17, 4)]
	interface VTSampleAttachmentQualityMetricsKey {
		[Field ("kVTSampleAttachmentQualityMetricsKey_LumaMeanSquaredError")]
		NSString LumaMeanSquaredError { get; }

		[Field ("kVTSampleAttachmentQualityMetricsKey_ChromaBlueMeanSquaredError")]
		NSString ChromaBlueMeanSquaredError { get; }

		[Field ("kVTSampleAttachmentQualityMetricsKey_ChromaRedMeanSquaredError")]
		NSString ChromaRedMeanSquaredError { get; }
	}

	/// <summary>A class that encapsulates keys necessary for decompression sessions. Used by <see cref="VideoToolbox.VTDecompressionProperties" /></summary>
	[MacCatalyst (13, 1)]
	[Static]
	interface VTDecompressionPropertyKey {
		// Pixel buffer pools

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kVTDecompressionPropertyKey_PixelBufferPool")]
		NSString PixelBufferPool { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kVTDecompressionPropertyKey_PixelBufferPoolIsShared")]
		NSString PixelBufferPoolIsShared { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kVTDecompressionPropertyKey_OutputPoolRequestedMinimumBufferCount")]
		[MacCatalyst (13, 1)]
		NSString OutputPoolRequestedMinimumBufferCount { get; }

		// Asynchronous state

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kVTDecompressionPropertyKey_NumberOfFramesBeingDecoded")]
		NSString NumberOfFramesBeingDecoded { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kVTDecompressionPropertyKey_MinOutputPresentationTimeStampOfFramesBeingDecoded")]
		NSString MinOutputPresentationTimeStampOfFramesBeingDecoded { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kVTDecompressionPropertyKey_MaxOutputPresentationTimeStampOfFramesBeingDecoded")]
		NSString MaxOutputPresentationTimeStampOfFramesBeingDecoded { get; }

		// Content

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kVTDecompressionPropertyKey_ContentHasInterframeDependencies")]
		NSString ContentHasInterframeDependencies { get; }

		// Hardware acceleration
		// hardware acceleration is default behavior on iOS.  no opt-in required.

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kVTDecompressionPropertyKey_UsingHardwareAcceleratedVideoDecoder")]
		[MacCatalyst (13, 1)]
		NSString UsingHardwareAcceleratedVideoDecoder { get; }

		// Decoder behavior

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kVTDecompressionPropertyKey_RealTime")]
		[MacCatalyst (13, 1)]
		NSString RealTime { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kVTDecompressionPropertyKey_MaximizePowerEfficiency")]
		[MacCatalyst (13, 1)]
		NSString MaximizePowerEfficiency { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kVTDecompressionPropertyKey_ThreadCount")]
		NSString ThreadCount { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kVTDecompressionPropertyKey_FieldMode")]
		NSString FieldMode { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kVTDecompressionProperty_FieldMode_BothFields")]
		NSString FieldMode_BothFields { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kVTDecompressionProperty_FieldMode_TopFieldOnly")]
		NSString FieldMode_TopFieldOnly { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kVTDecompressionProperty_FieldMode_BottomFieldOnly")]
		NSString FieldMode_BottomFieldOnly { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kVTDecompressionProperty_FieldMode_SingleField")]
		NSString FieldMode_SingleField { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kVTDecompressionProperty_FieldMode_DeinterlaceFields")]
		NSString FieldMode_DeinterlaceFields { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kVTDecompressionPropertyKey_DeinterlaceMode")]
		NSString DeinterlaceMode { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kVTDecompressionProperty_DeinterlaceMode_VerticalFilter")]
		NSString DeinterlaceMode_VerticalFilter { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kVTDecompressionProperty_DeinterlaceMode_Temporal")]
		NSString DeinterlaceMode_Temporal { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kVTDecompressionPropertyKey_ReducedResolutionDecode")]
		NSString ReducedResolutionDecode { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kVTDecompressionPropertyKey_ReducedCoefficientDecode")]
		NSString ReducedCoefficientDecode { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kVTDecompressionPropertyKey_ReducedFrameDelivery")]
		NSString ReducedFrameDelivery { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kVTDecompressionPropertyKey_OnlyTheseFrames")]
		NSString OnlyTheseFrames { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kVTDecompressionProperty_OnlyTheseFrames_AllFrames")]
		NSString OnlyTheseFrames_AllFrames { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kVTDecompressionProperty_OnlyTheseFrames_NonDroppableFrames")]
		NSString OnlyTheseFrames_NonDroppableFrames { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kVTDecompressionProperty_OnlyTheseFrames_IFrames")]
		NSString OnlyTheseFrames_IFrames { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kVTDecompressionProperty_OnlyTheseFrames_KeyFrames")]
		NSString OnlyTheseFrames_KeyFrames { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Field ("kVTDecompressionProperty_TemporalLevelLimit")]
		NSString TemporalLevelLimit { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kVTDecompressionPropertyKey_SuggestedQualityOfServiceTiers")]
		NSString SuggestedQualityOfServiceTiers { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kVTDecompressionPropertyKey_SupportedPixelFormatsOrderedByQuality")]
		NSString SupportedPixelFormatsOrderedByQuality { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kVTDecompressionPropertyKey_SupportedPixelFormatsOrderedByPerformance")]
		NSString SupportedPixelFormatsOrderedByPerformance { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kVTDecompressionPropertyKey_PixelFormatsWithReducedResolutionSupport")]
		NSString PixelFormatsWithReducedResolutionSupport { get; }

		[iOS (13, 0), TV (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("kVTDecompressionPropertyKey_UsingGPURegistryID")]
		NSString UsingGpuRegistryId { get; }

		//Post-decompression processing

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kVTDecompressionPropertyKey_PixelTransferProperties")]
		NSString PixelTransferProperties { get; }

		[iOS (14, 1)]
		[TV (14, 2)]
		[MacCatalyst (14, 1)]
		[Field ("kVTDecompressionPropertyKey_PropagatePerFrameHDRDisplayMetadata")]
		NSString PropagatePerFrameHdrDisplayMetadata { get; }

		[iOS (17, 0), TV (17, 0), MacCatalyst (17, 0), Mac (14, 0)]
		[Field ("kVTDecompressionPropertyKey_GeneratePerFrameHDRDisplayMetadata")]
		NSString GeneratePerFrameHdrDisplayMetadata { get; }

		[iOS (18, 0), TV (18, 0), MacCatalyst (18, 0), Mac (15, 0)]
		[Field ("kVTDecompressionPropertyKey_AllowBitstreamToChangeFrameDimensions")]
		NSString AllowBitstreamToChangeFrameDimensions { get; }

		[NoiOS, NoTV, NoMacCatalyst, Mac (15, 0)]
		[Field ("kVTDecompressionPropertyKey_DecoderProducesRAWOutput")]
		NSString DecoderProducesRawOutput { get; }

		[NoiOS, NoTV, NoMacCatalyst, Mac (15, 0)]
		[Field ("kVTDecompressionPropertyKey_RequestRAWOutput")]
		NSString RequestRawWOutput { get; }

		[iOS (17, 0), NoTV, Mac (14, 0), MacCatalyst (17, 0)]
		[Field ("kVTDecompressionPropertyKey_RequestedMVHEVCVideoLayerIDs")]
		NSString RequestedMvHevcVideoLayerIds { get; }
	}

	/// <summary>Strongly typed set of options for decompression sessions.</summary>
	[MacCatalyst (13, 1)]
	[StrongDictionary ("VTDecompressionPropertyKey")]
	interface VTDecompressionProperties {

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("PixelBufferPoolIsShared")]
		bool PixelBufferPoolIsShared { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("OutputPoolRequestedMinimumBufferCount")]
		uint OutputPoolRequestedMinimumBufferCount { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("NumberOfFramesBeingDecoded")]
		uint NumberOfFramesBeingDecoded { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("MinOutputPresentationTimeStampOfFramesBeingDecoded")]
		NSDictionary MinOutputPresentationTimeStampOfFramesBeingDecoded { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("MaxOutputPresentationTimeStampOfFramesBeingDecoded")]
		NSDictionary MaxOutputPresentationTimeStampOfFramesBeingDecoded { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("ContentHasInterframeDependencies")]
		bool ContentHasInterframeDependencies { get; }

		// Hardware acceleration
		// hardware acceleration is default behavior on iOS.  no opt-in required.

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("UsingHardwareAcceleratedVideoDecoder")]
		[MacCatalyst (13, 1)]
		bool UsingHardwareAcceleratedVideoDecoder { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("RealTime")]
		bool RealTime { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("MaximizePowerEfficiency")]
		bool MaximizePowerEfficiency { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("ThreadCount")]
		uint ThreadCount { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[StrongDictionary]
		[Export ("ReducedResolutionDecode")]
		VTDecompressionResolutionOptions ReducedResolutionDecode { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("ReducedCoefficientDecode")]
		uint ReducedCoefficientDecode { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("ReducedFrameDelivery")]
		float ReducedFrameDelivery { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("TemporalLevelLimit")]
		int TemporalLevelLimit { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("SuggestedQualityOfServiceTiers")]
		NSDictionary [] SuggestedQualityOfServiceTiers { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("SupportedPixelFormatsOrderedByQuality")]
		CMPixelFormat [] SupportedPixelFormatsOrderedByQuality { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("SupportedPixelFormatsOrderedByPerformance")]
		CMPixelFormat [] SupportedPixelFormatsOrderedByPerformance { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("PixelFormatsWithReducedResolutionSupport")]
		CMPixelFormat [] PixelFormatsWithReducedResolutionSupport { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Advice ("Use Strongly typed version PixelTransferSettings")]
		[Export ("PixelTransferProperties")]
		NSDictionary PixelTransferProperties { get; set; }

		// VTPixelTransferProperties are available in iOS 9 radar://22614931 https://trello.com/c/bTl6hRu9
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[StrongDictionary]
		[MacCatalyst (13, 1)]
		[Export ("PixelTransferProperties")]
		VTPixelTransferProperties PixelTransferSettings { get; set; }

		[iOS (13, 0), TV (13, 0)]
		[MacCatalyst (13, 1)]
		[Export ("UsingGpuRegistryId")]
		uint UsingGpuRegistryId { get; }

		[iOS (14, 1)]
		[TV (14, 2)]
		[MacCatalyst (14, 1)]
		[Export ("PropagatePerFrameHdrDisplayMetadata")]
		bool PropagatePerFrameHhrDisplayMetadata { get; set; }

		[Export ("GeneratePerFrameHdrDisplayMetadata")]
		[iOS (17, 0), TV (17, 0), MacCatalyst (17, 0), Mac (14, 0)]
		bool GeneratePerFrameHdrDisplayMetadata { get; }

		[Export ("AllowBitstreamToChangeFrameDimensions")]
		[iOS (18, 0), TV (18, 0), MacCatalyst (18, 0), Mac (15, 0)]
		bool AllowBitstreamToChangeFrameDimensions { get; }

		[Export ("DecoderProducesRawOutput")]
		[NoiOS, NoTV, NoMacCatalyst, Mac (15, 0)]
		bool DecoderProducesRawOutput { get; }

		[Export ("RequestRawWOutput")]
		[NoiOS, NoTV, NoMacCatalyst, Mac (15, 0)]
		bool RequestRawWOutput { get; }

		[iOS (17, 0), NoTV, Mac (14, 0), MacCatalyst (17, 0)]
		[Export ("RequestedMvHevcVideoLayerIds")]
		NSNumber [] RequestedMvHevcVideoLayerIds { get; }
	}

	/// <summary>Strongly typed set of options.</summary>
	[MacCatalyst (13, 1)]
	[StrongDictionary ("VTVideoDecoderSpecificationKeys")]
	interface VTVideoDecoderSpecification {
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("EnableHardwareAcceleratedVideoDecoder")]
		bool EnableHardwareAcceleratedVideoDecoder { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("RequireHardwareAcceleratedVideoDecoder")]
		bool RequireHardwareAcceleratedVideoDecoder { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NoiOS]
		[NoTV]
		[NoMacCatalyst]
		[Export ("RequiredDecoderGpuRegistryId")]
		NSNumber RequiredDecoderGpuRegistryId { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NoiOS]
		[NoTV]
		[NoMacCatalyst]
		[Export ("PreferredDecoderGpuRegistryId")]
		NSNumber PreferredDecoderGpuRegistryId { get; }
	}

	/// <summary>A class that encapsulates keys necessary by <see cref="VideoToolbox.VTVideoDecoderSpecification" />.</summary>
	[MacCatalyst (13, 1)]
	[Static]
	interface VTVideoDecoderSpecificationKeys {
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kVTVideoDecoderSpecification_EnableHardwareAcceleratedVideoDecoder")]
		NSString EnableHardwareAcceleratedVideoDecoder { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kVTVideoDecoderSpecification_RequireHardwareAcceleratedVideoDecoder")]
		NSString RequireHardwareAcceleratedVideoDecoder { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NoiOS]
		[NoTV]
		[NoMacCatalyst]
		[Field ("kVTVideoDecoderSpecification_RequiredDecoderGPURegistryID")]
		NSString RequiredDecoderGpuRegistryId { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NoiOS]
		[NoTV]
		[NoMacCatalyst]
		[Field ("kVTVideoDecoderSpecification_PreferredDecoderGPURegistryID")]
		NSString PreferredDecoderGpuRegistryId { get; }
	}

	/// <summary>Strongly typed set of options used by <see cref="VideoToolbox.VTDecompressionProperties.ReducedResolutionDecode" />.</summary>
	[MacCatalyst (13, 1)]
	[StrongDictionary ("VTDecompressionResolutionKeys")]
	interface VTDecompressionResolutionOptions {
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("Width")]
		float Width { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("Height")]
		float Height { get; set; }
	}

	/// <summary>A class that encapsulates keys necessary by <see cref="VideoToolbox.VTEncodeFrameOptions" />.</summary>
	[MacCatalyst (13, 1)]
	[Static]
	interface VTDecompressionResolutionKeys {
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kVTDecompressionResolutionKey_Width")]
		NSString Width { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kVTDecompressionResolutionKey_Height")]
		NSString Height { get; }
	}

	// VTSession.h
	/// <summary>Strongly typed set of options.</summary>
	[MacCatalyst (13, 1)]
	[StrongDictionary ("VTPropertyKeys")]
	interface VTPropertyOptions {
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("ShouldBeSerialized")]
		bool ShouldBeSerialized { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("SupportedValueMinimumKey")]
		NSNumber SupportedValueMinimum { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("SupportedValueMaximumKey")]
		NSNumber SupportedValueMaximum { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("SupportedValueListKey")]
		NSNumber [] SupportedValueList { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("DocumentationKey")]
		NSString Documentation { get; set; }
	}

	/// <summary>A class that encapsulates keys necessary by <see cref="VideoToolbox.VTPropertyOptions" />.</summary>
	[MacCatalyst (13, 1)]
	[Static]
	interface VTPropertyKeys {
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kVTPropertyTypeKey")]
		NSString Type { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kVTPropertyReadWriteStatusKey")]
		NSString ReadWriteStatus { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kVTPropertyShouldBeSerializedKey")]
		NSString ShouldBeSerialized { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kVTPropertySupportedValueMinimumKey")]
		NSString SupportedValueMinimumKey { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kVTPropertySupportedValueMaximumKey")]
		NSString SupportedValueMaximumKey { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kVTPropertySupportedValueListKey")]
		NSString SupportedValueListKey { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kVTPropertyDocumentationKey")]
		NSString DocumentationKey { get; }
	}

	/// <summary>A class that encapsulates keys necessary by <see cref="VideoToolbox.VTPropertyType" />.</summary>
	[MacCatalyst (13, 1)]
	[Static]
	interface VTPropertyTypeKeys {
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kVTPropertyType_Boolean")]
		NSString Boolean { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kVTPropertyType_Enumeration")]
		NSString Enumeration { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kVTPropertyType_Number")]
		NSString Number { get; }
	}

	/// <summary>A class that encapsulates keys necessary by <see cref="VideoToolbox.VTPropertyOptions.ReadWriteStatus" />.</summary>
	[MacCatalyst (13, 1)]
	[Static]
	interface VTPropertyReadWriteStatusKeys {
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kVTPropertyReadWriteStatus_ReadOnly")]
		NSString ReadOnly { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kVTPropertyReadWriteStatus_ReadWrite")]
		NSString ReadWrite { get; }
	}

	// VTVideoEncoderList.h
	[MacCatalyst (13, 1)]
	[Static]
	[Internal]
	interface VTVideoEncoderList {
		[Field ("kVTVideoEncoderList_CodecName")]
		NSString CodecName { get; }

		[Field ("kVTVideoEncoderList_CodecType")]
		NSString CodecType { get; }

		[Field ("kVTVideoEncoderList_DisplayName")]
		NSString DisplayName { get; }

		[Field ("kVTVideoEncoderList_EncoderID")]
		NSString EncoderID { get; }

		[Field ("kVTVideoEncoderList_EncoderName")]
		NSString EncoderName { get; }

		[iOS (13, 0), TV (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("kVTVideoEncoderList_GPURegistryID")]
		NSString GpuRegistryId { get; }

		[iOS (13, 0), TV (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("kVTVideoEncoderList_SupportedSelectionProperties")]
		NSString SupportedSelectionProperties { get; }

		[iOS (13, 0), TV (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("kVTVideoEncoderList_PerformanceRating")]
		NSString PerformanceRating { get; }

		[iOS (13, 0), TV (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("kVTVideoEncoderList_QualityRating")]
		NSString QualityRating { get; }

		[iOS (13, 0), TV (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("kVTVideoEncoderList_InstanceLimit")]
		NSString InstanceLimit { get; }

		[iOS (13, 0), TV (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("kVTVideoEncoderList_IsHardwareAccelerated")]
		NSString IsHardwareAccelerated { get; }

		[iOS (14, 2)]
		[TV (14, 2)]
		[MacCatalyst (14, 2)]
		[Field ("kVTVideoEncoderList_SupportsFrameReordering")]
		NSString SupportsFrameReordering { get; }

		[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
		[Field ("kVTVideoEncoderListOption_IncludeStandardDefinitionDVEncoders")]
		NSString IncludeStandardDefinitionDVEncoders { get; }

		// VTVideoEncoder.cs should be updated when new constants are added here
		// some are missing https://github.com/dotnet/macios/issues/9904
	}

	// VTMultiPassStorage.h
	/// <summary>A class that encapsulates keys necessary by <see cref="VideoToolbox.VTMultiPassStorageCreationOptions" /></summary>
	[MacCatalyst (13, 1)]
	[Static]
	interface VTMultiPassStorageCreationOptionKeys {
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kVTMultiPassStorageCreationOption_DoNotDelete")]
		NSString DoNotDelete { get; }
	}

	/// <summary>Strongly typed set of options.</summary>
	[MacCatalyst (13, 1)]
	[StrongDictionary ("VTMultiPassStorageCreationOptionKeys")]
	interface VTMultiPassStorageCreationOptions {
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("DoNotDelete")]
		bool DoNotDelete { get; set; }
	}

	// VTPixelTransferProperties are available in iOS 9 radar://22614931 https://trello.com/c/bTl6hRu9
	/// <summary>Strongly typed set of options used by <see cref="VideoToolbox.VTDecompressionProperties.PixelTransferSettings" />.</summary>
	[MacCatalyst (13, 1)]
	[StrongDictionary ("VTPixelTransferPropertyKeys")]
	interface VTPixelTransferProperties {
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[StrongDictionary]
		[Export ("DestinationCleanAperture")]
		AVVideoCleanApertureSettings DestinationCleanAperture { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[StrongDictionary]
		[Export ("DestinationPixelAspectRatio")]
		AVVideoPixelAspectRatioSettings DestinationPixelAspectRatio { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("DestinationICCProfile")]
		NSData DestinationICCProfile { get; set; }

		[iOS (13, 0), TV (13, 0)]
		[MacCatalyst (13, 1)]
		[Export ("RealTime")]
		bool RealTime { get; set; }
	}

	// VTPixelTransferProperties are available in iOS 9 radar://22614931 https://trello.com/c/bTl6hRu9
	/// <summary>A class that encapsulates keys needed by <see cref="VideoToolbox.VTPixelTransferProperties" />.</summary>
	[MacCatalyst (13, 1)]
	[Static]
	[AdvancedAttribute]
	interface VTPixelTransferPropertyKeys {

		// ScalingMode

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kVTPixelTransferPropertyKey_ScalingMode")]
		NSString ScalingMode { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kVTScalingMode_Normal")]
		NSString ScalingMode_Normal { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kVTScalingMode_CropSourceToCleanAperture")]
		NSString ScalingMode_CropSourceToCleanAperture { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kVTScalingMode_Letterbox")]
		NSString ScalingMode_Letterbox { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kVTScalingMode_Trim")]
		NSString ScalingMode_Trim { get; }

		// DestinationCleanAperture

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kVTPixelTransferPropertyKey_DestinationCleanAperture")]
		NSString DestinationCleanAperture { get; }

		// DestinationCleanAperture

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kVTPixelTransferPropertyKey_DestinationPixelAspectRatio")]
		NSString DestinationPixelAspectRatio { get; }

		// DownsamplingMode

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kVTPixelTransferPropertyKey_DownsamplingMode")]
		NSString DownsamplingMode { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kVTDownsamplingMode_Decimate")]
		NSString DownsamplingMode_Decimate { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kVTDownsamplingMode_Average")]
		NSString DownsamplingMode_Average { get; }

		// DestinationColorPrimaries

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Field ("kVTPixelTransferPropertyKey_DestinationColorPrimaries")]
		NSString DestinationColorPrimaries { get; }

		// DestinationColorPrimaries

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Field ("kVTPixelTransferPropertyKey_DestinationTransferFunction")]
		NSString DestinationTransferFunction { get; }

		// DestinationICCProfile

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Field ("kVTPixelTransferPropertyKey_DestinationICCProfile")]
		NSString DestinationICCProfile { get; }

		// DestinationYCbCrMatrix

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kVTPixelTransferPropertyKey_DestinationYCbCrMatrix")]
		NSString DestinationYCbCrMatrix { get; }

		[iOS (13, 0), TV (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("kVTPixelTransferPropertyKey_RealTime")]
		NSString RealTime { get; }
	}

	[Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0), TV (16, 0)]
	[StrongDictionary ("VTPixelRotationPropertyKeys")]
	interface VTPixelRotationProperties {
		[Export ("FlipHorizontalOrientation")]
		bool FlipHorizontalOrientation { get; set; }

		[Export ("FlipVerticalOrientation")]
		bool FlipVerticalOrientation { get; set; }
	}

	[Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0), TV (16, 0)]
	[Static]
	[Advanced]
	interface VTPixelRotationPropertyKeys {

		// Rotation

		[Field ("kVTPixelRotationPropertyKey_Rotation")]
		NSString Rotation { get; }

		// FlipHorizontalOrientation

		[Field ("kVTPixelRotationPropertyKey_FlipHorizontalOrientation")]
		NSString FlipHorizontalOrientation { get; }

		// FlipVerticalOrientation

		[Field ("kVTPixelRotationPropertyKey_FlipVerticalOrientation")]
		NSString FlipVerticalOrientation { get; }
	}

	[TV (18, 0), Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
	public enum VTHdrPerFrameMetadataGenerationHdrFormatType {
		[Field ("kVTHDRPerFrameMetadataGenerationHDRFormatType_DolbyVision")]
		DolbyVision,
	}

	[TV (18, 0), Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
	[Static]
	interface VTHdrPerFrameMetadataGenerationOptionsKey {
		[Field ("kVTHDRPerFrameMetadataGenerationOptionsKey_HDRFormats")]
		NSString HdrFormats { get; }

	}

	[TV (18, 0), Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
	[StrongDictionary ("VTHdrPerFrameMetadataGenerationOptionsKey")]
	interface VTHdrPerFrameMetadataGenerationOptions {
		[Export ("HdrFormats")]
		VTHdrPerFrameMetadataGenerationHdrFormatType HdrFormats { get; set; }
	}

	[NoTV, NoiOS, NoMacCatalyst, Mac (15, 0)]
	[Static]
	interface VTExtensionPropertiesKey {
		[Field ("kVTExtensionProperties_ExtensionIdentifierKey")]
		NSString ExtensionIdentifier { get; }

		[Field ("kVTExtensionProperties_ExtensionNameKey")]
		NSString ExtensionName { get; }

		[Field ("kVTExtensionProperties_ContainingBundleNameKey")]
		NSString ContainingBundleName { get; }

		[Field ("kVTExtensionProperties_ExtensionURLKey")]
		NSString ExtensionUrl { get; }

		[Field ("kVTExtensionProperties_ContainingBundleURLKey")]
		NSString ContainingBundleUrl { get; }

		[Field ("kVTExtensionProperties_CodecNameKey")]
		NSString CodecName { get; }
	}

	[NoTV, NoiOS, NoMacCatalyst, Mac (15, 0)]
	[StrongDictionary ("VTExtensionPropertiesKey")]
	interface VTDecoderExtensionProperties {
		[Export ("ExtensionIdentifier")]
		string ExtensionIdentifier { get; set; }

		[Export ("ExtensionName")]
		string ExtensionName { get; set; }

		[Export ("ContainingBundleName")]
		string ContainingBundleName { get; set; }

		[Export ("ExtensionUrl")]
		NSUrl ExtensionUrl { get; set; }

		[Export ("ContainingBundleUrl")]
		NSUrl ContainingBundleUrl { get; set; }

		[Export ("CodecName")]
		string CodecName { get; set; }
	}

	[NoTV, NoiOS, NoMacCatalyst, Mac (15, 0)]
	[Static]
	interface VTRawProcessingParameterKey {
		[Field ("kVTRAWProcessingParameter_Key")]
		NSString Key { get; }

		[Field ("kVTRAWProcessingParameter_Name")]
		NSString Name { get; }

		[Field ("kVTRAWProcessingParameter_Description")]
		NSString Description { get; }

		[Field ("kVTRAWProcessingParameter_Enabled")]
		NSString Enabled { get; }

		[Field ("kVTRAWProcessingParameter_ValueType")]
		NSString ValueType { get; }

		[Field ("kVTRAWProcessingParameter_ListArray")]
		NSString ListArray { get; }

		[Field ("kVTRAWProcessingParameter_SubGroup")]
		NSString SubGroup { get; }

		[Field ("kVTRAWProcessingParameter_MaximumValue")]
		NSString MaximumValue { get; }

		[Field ("kVTRAWProcessingParameter_MinimumValue")]
		NSString MinimumValue { get; }

		[Field ("kVTRAWProcessingParameter_InitialValue")]
		NSString InitialValue { get; }

		[Field ("kVTRAWProcessingParameter_NeutralValue")]
		NSString NeutralValue { get; }

		[Field ("kVTRAWProcessingParameter_CameraValue")]
		NSString CameraValue { get; }

		[Field ("kVTRAWProcessingParameter_CurrentValue")]
		NSString CurrentValue { get; }
	}

	[NoTV, NoiOS, NoMacCatalyst, Mac (15, 0)]
	[Static]
	interface VTRawProcessingParameterListElementKey {
		[Field ("kVTRAWProcessingParameterListElement_Label")]
		NSString Label { get; }

		[Field ("kVTRAWProcessingParameterListElement_Description")]
		NSString Description { get; }

		[Field ("kVTRAWProcessingParameterListElement_ListElementID")]
		NSString ListElementId { get; }
	}

	[NoTV, NoiOS, NoMacCatalyst, Mac (15, 0)]
	[StrongDictionary ("VTRawProcessingParameterListElementKey")]
	interface VTRawProcessingParametersListElement {
		[Export ("Label")]
		string Label { get; set; }

		[Export ("Description")]
		string Description { get; set; }

		[Export ("ListElementId")]
		nint ListElementId { get; set; }
	}

	[NoTV, NoiOS, NoMacCatalyst, Mac (15, 0)]
	[Static]
	interface VTRawProcessingParameterValueTypeKey {
		[Field ("kVTRAWProcessingParameterValueType_Boolean")]
		NSString Boolean { get; }

		[Field ("kVTRAWProcessingParameterValueType_Integer")]
		NSString Integer { get; }

		[Field ("kVTRAWProcessingParameterValueType_Float")]
		NSString Float { get; }

		[Field ("kVTRAWProcessingParameterValueType_List")]
		NSString List { get; }

		[Field ("kVTRAWProcessingParameterValueType_SubGroup")]
		NSString SubGroup { get; }
	}

	[NoTV, NoiOS, NoMacCatalyst, Mac (15, 0)]
	[StrongDictionary ("VTRawProcessingParameterValueTypeKey")]
	interface VTRawProcessingParameterValueType {
		[Export ("Boolean")]
		bool Boolean { get; set; }

		[Export ("Integer")]
		int Integer { get; set; }

		[Export ("Float")]
		float Float { get; set; }

		[Export ("List")]
		NSObject [] List { get; set; }

		[Export ("SubGroup")]
		NSDictionary SubGroup { get; set; }
	}

	[NoTV, NoiOS, NoMacCatalyst, Mac (15, 0)]
	[StrongDictionary ("VTRawProcessingParameterKey")]
	interface VTRawProcessingParameters {
		[Export ("Key")]
		string Key { get; set; }

		[Export ("Name")]
		string Name { get; set; }

		[Export ("Description")]
		string Description { get; set; }

		[Export ("Enabled")]
		bool Enabled { get; set; }

		[Export ("ValueType")]
		VTRawProcessingParameterValueType ValueType { get; set; }

		// FIXME: Generated code doesn't compile
		// [Export ("ListArray")]
		// VTRawProcessingParametersListElement [] ListArray { get; set; }

		// FIXME: Generated code doesn't compile
		// [Export ("SubGroup")]
		// VTRawProcessingParameters[] SubGroup { get; set; }

		[Export ("MaximumValue")]
		NSObject MaximumValue { get; set; }

		[Export ("MinimumValue")]
		NSObject MinimumValue { get; set; }

		[Export ("InitialValue")]
		NSObject InitialValue { get; set; }

		[Export ("NeutralValue")]
		NSObject NeutralValue { get; set; }

		[Export ("CameraValue")]
		NSObject CameraValue { get; set; }

		[Export ("CurrentValue")]
		NSObject CurrentValue { get; set; }
	}

	[NoTV, NoiOS, NoMacCatalyst, Mac (15, 0)]
	[Static]
	interface VTRawProcessingPropertyKey {
		[Field ("kVTRAWProcessingPropertyKey_MetalDeviceRegistryID")]
		NSString MetalDeviceRegistryId { get; }

		[Field ("kVTRAWProcessingPropertyKey_OutputColorAttachments")]
		NSString OutputColorAttachments { get; }
	}

	[NoTV, NoiOS, NoMacCatalyst, Mac (15, 0)]
	[StrongDictionary ("VTRawProcessingPropertyKey", Suffix = "")]
	interface VTRawProcessingProperty {
		ulong MetalDeviceRegistryId { get; set; }

		NSDictionary OutputColorAttachments { get; }
	}

	[MacCatalyst (13, 1)]
	[StrongDictionary ("VTSampleAttachmentKey", Suffix = "")]
	interface VTSampleAttachments {
		[TV (15, 0), Mac (12, 0), iOS (15, 0), MacCatalyst (15, 0)]
		NSNumber RequireLtrAcknowledgementToken { get; set; }

		[iOS (17, 4), TV (17, 4), Mac (14, 4), MacCatalyst (17, 4)]
		VTSampleAttachmentQualityMetrics QualityMetrics { get; }
	}

	[StrongDictionary ("VTSampleAttachmentQualityMetricsKey", Suffix = "")]
	[iOS (17, 4), TV (17, 4), Mac (14, 4), MacCatalyst (17, 4)]
	interface VTSampleAttachmentQualityMetrics {
		// This can be either CFNumber or CFArray, so we have to bind it as NSObject
		NSObject LumaMeanSquaredError { get; }

		// This can be either CFNumber or CFArray, so we have to bind it as NSObject
		NSObject ChromaBlueMeanSquaredError { get; }

		// This can be either CFNumber or CFArray, so we have to bind it as NSObject
		NSString ChromaRedMeanSquaredError { get; }
	}

	[NoMacCatalyst, NoTV, NoiOS, Mac (15, 4)]
	[BaseType (typeof (NSObject))]
	interface VTFrameProcessor {
		[Export ("startSessionWithConfiguration:error:")]
		bool StartSession (IVTFrameProcessorConfiguration configuration, [NullAllowed] out NSError error);

		[Export ("processWithParameters:error:")]
		bool Process (IVTFrameProcessorParameters parameters, [NullAllowed] out NSError error);

		[Export ("processWithParameters:completionHandler:")]
		void Process (IVTFrameProcessorParameters parameters, VTFrameProcessorProcessHandler completionHandler);

		[Export ("processWithCommandBuffer:parameters:")]
		void Process (IMTLCommandBuffer commandBuffer, IVTFrameProcessorParameters parameters);

		[Export ("endSession")]
		void EndSession ();
	}

	delegate void VTFrameProcessorProcessHandler (IVTFrameProcessorParameters parameters, [NullAllowed] NSError error);

	[NoMacCatalyst, NoTV, NoiOS, Mac (15, 4)]
	[Protocol (BackwardsCompatibleCodeGeneration = false)]
	interface VTFrameProcessorConfiguration {
		[Static, Abstract]
		[Export ("processorSupported")]
		bool ProcessorSupported { get; }

		[Abstract]
		[Export ("frameSupportedPixelFormats")]
		NSNumber [] WeakFrameSupportedPixelFormats { get; }

		CVPixelFormatType [] FrameSupportedPixelFormats {
			[Wrap ("Array.ConvertAll (this.WeakFrameSupportedPixelFormats, (v) => (CVPixelFormatType) v.UInt32Value);")]
			get;
		}

		[Abstract]
		[Export ("sourcePixelBufferAttributes")]
		NSDictionary WeakSourcePixelBufferAttributes { get; }

		[Wrap ("WeakSourcePixelBufferAttributes")]
		CVPixelBufferAttributes SourcePixelBufferAttributes { get; }

		[Abstract]
		[Export ("destinationPixelBufferAttributes")]
		NSDictionary WeakDestinationPixelBufferAttributes { get; }

		[Wrap ("WeakDestinationPixelBufferAttributes")]
		CVPixelBufferAttributes DestinationPixelBufferAttributes { get; }

		[Export ("nextFrameCount")]
		nint NextFrameCount { get; }

		[Export ("previousFrameCount")]
		nint PreviousFrameCount { get; }

		[Static]
		[Export ("maximumDimensions")]
		CMVideoDimensions MaximumDimensions { get; }

		[Static]
		[Export ("minimumDimensions")]
		CMVideoDimensions MinimumDimensions { get; }
	}

	interface IVTFrameProcessorConfiguration { }

	[MacCatalyst (18, 4), NoTV, NoiOS, Mac (15, 4)]
#if !__MACCATALYST__
	[ErrorDomain ("VTFrameProcessorErrorDomain")]
#endif
	[Native]
	public enum VTFrameProcessorError : long {
		UnknownError = -19730,
		UnsupportedResolution = -19731,
		SessionNotStarted = -19732,
		SessionAlreadyActive = -19733,
		FatalError = -19734,
		SessionLevelError = -19735,
		InitializationFailed = -19736,
		UnsupportedInput = -19737,
		MemoryAllocationFailure = -19738,
		RevisionNotSupported = -19739,
		ProcessingError = -19740,
		InvalidParameterError = -19741,
		InvalidFrameTiming = -19742
	}

	[NoMacCatalyst, NoTV, NoiOS, Mac (15, 4)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface VTFrameProcessorFrame {
		[Export ("initWithBuffer:presentationTimeStamp:")]
		NativeHandle Constructor (CVPixelBuffer buffer, CMTime presentationTimeStamp);

		[Export ("buffer")]
		CVPixelBuffer Buffer { get; }

		[Export ("presentationTimeStamp")]
		CMTime PresentationTimeStamp { get; }
	}

	[NoMacCatalyst, NoTV, NoiOS, Mac (15, 4)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface VTFrameProcessorOpticalFlow {
		[Export ("initWithForwardFlow:backwardFlow:")]
		NativeHandle Constructor (CVPixelBuffer forwardFlow, CVPixelBuffer backwardFlow);

		[Export ("forwardFlow")]
		CVPixelBuffer ForwardFlow { get; }

		[Export ("backwardFlow")]
		CVPixelBuffer BackwardFlow { get; }
	}

	[NoMacCatalyst, NoTV, NoiOS, Mac (15, 4)]
	[Protocol (BackwardsCompatibleCodeGeneration = false)]
	interface VTFrameProcessorParameters {
		[Abstract]
		[Export ("sourceFrame")]
		VTFrameProcessorFrame SourceFrame { get; }
	}

	interface IVTFrameProcessorParameters { }

	[MacCatalyst (18, 4), NoTV, NoiOS, Mac (15, 4)]
	[Native]
	public enum VTFrameRateConversionConfigurationQualityPrioritization : long {
		Normal = 1,
		Quality = 2,
	}

	[MacCatalyst (18, 4), NoTV, NoiOS, Mac (15, 4)]
	[Native]
	public enum VTFrameRateConversionConfigurationRevision : long {
		Revision1 = 1,
	}

	[MacCatalyst (18, 4), NoTV, NoiOS, Mac (15, 4)]
	[Native]
	public enum VTFrameRateConversionParametersSubmissionMode : long {
		Random = 1,
		Sequential = 2,
		SequentialReferencesUnchanged = 3,
	}

	[NoMacCatalyst, NoTV, NoiOS, Mac (15, 4)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface VTFrameRateConversionConfiguration : VTFrameProcessorConfiguration {
		[Export ("initWithFrameWidth:frameHeight:usePrecomputedFlow:qualityPrioritization:revision:")]
		NativeHandle Constructor (nint frameWidth, nint frameHeight, bool usePrecomputedFlow, VTFrameRateConversionConfigurationQualityPrioritization qualityPrioritization, VTFrameRateConversionConfigurationRevision revision);

		[Export ("frameWidth")]
		nint FrameWidth { get; }

		[Export ("frameHeight")]
		nint FrameHeight { get; }

		[Export ("usePrecomputedFlow")]
		bool UsePrecomputedFlow { get; }

		[Export ("qualityPrioritization")]
		VTFrameRateConversionConfigurationQualityPrioritization QualityPrioritization { get; }

		[Export ("revision")]
		VTFrameRateConversionConfigurationRevision Revision { get; }

		[Static]
		[Export ("supportedRevisions")]
		NSIndexSet WeakSupportedRevisions { get; }

		[Static]
		[Wrap ("WeakSupportedRevisions.ToInt64EnumHashSet<VTFrameRateConversionConfigurationRevision> ()")]
		HashSet<VTFrameRateConversionConfigurationRevision> SupportedRevisions { get; }

		[Static]
		[Export ("defaultRevision")]
		VTFrameRateConversionConfigurationRevision DefaultRevision { get; }
	}

	[NoMacCatalyst, NoTV, NoiOS, Mac (15, 4)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface VTFrameRateConversionParameters : VTFrameProcessorParameters {
		[Export ("initWithSourceFrame:nextFrame:opticalFlow:interpolationPhase:submissionMode:destinationFrames:")]
		NativeHandle Constructor (VTFrameProcessorFrame sourceFrame, VTFrameProcessorFrame nextFrame, [NullAllowed] VTFrameProcessorOpticalFlow opticalFlow, NSNumber [] interpolationPhase, VTFrameRateConversionParametersSubmissionMode submissionMode, VTFrameProcessorFrame [] destinationFrame);

		[Export ("sourceFrame")]
		new VTFrameProcessorFrame SourceFrame { get; }

		[NullAllowed, Export ("nextFrame")]
		VTFrameProcessorFrame NextFrame { get; }

		[NullAllowed, Export ("opticalFlow")]
		VTFrameProcessorOpticalFlow OpticalFlow { get; }

		[BindAs (typeof (float []))]
		[Export ("interpolationPhase")]
		NSNumber [] InterpolationPhase { get; }

		[Export ("submissionMode")]
		VTFrameRateConversionParametersSubmissionMode SubmissionMode { get; }

		[Export ("destinationFrames")]
		VTFrameProcessorFrame [] DestinationFrames { get; }
	}

	[MacCatalyst (18, 4), NoTV, NoiOS, Mac (15, 4)]
	[Native]
	public enum VTMotionBlurConfigurationQualityPrioritization : long {
		Normal = 1,
		Quality = 2,
	}

	[MacCatalyst (18, 4), NoTV, NoiOS, Mac (15, 4)]
	[Native]
	public enum VTMotionBlurConfigurationRevision : long {
		Revision1 = 1,
	}

	[MacCatalyst (18, 4), NoTV, NoiOS, Mac (15, 4)]
	[Native]
	public enum VTMotionBlurParametersSubmissionMode : long {
		Random = 1,
		Sequential = 2,
	}

	[NoMacCatalyst, NoTV, NoiOS, Mac (15, 4)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface VTMotionBlurConfiguration : VTFrameProcessorConfiguration {
		[Export ("initWithFrameWidth:frameHeight:usePrecomputedFlow:qualityPrioritization:revision:")]
		NativeHandle Constructor (nint frameWidth, nint frameHeight, bool usePrecomputedFlow, VTMotionBlurConfigurationQualityPrioritization qualityPrioritization, VTMotionBlurConfigurationRevision revision);

		[Export ("frameWidth")]
		nint FrameWidth { get; }

		[Export ("frameHeight")]
		nint FrameHeight { get; }

		[Export ("usePrecomputedFlow")]
		bool UsePrecomputedFlow { get; }

		[Export ("qualityPrioritization")]
		VTMotionBlurConfigurationQualityPrioritization QualityPrioritization { get; }

		[Export ("revision")]
		VTMotionBlurConfigurationRevision Revision { get; }

		[Static]
		[Export ("supportedRevisions")]
		NSIndexSet WeakSupportedRevisions { get; }

		[Static]
		[Wrap ("WeakSupportedRevisions.ToInt64EnumHashSet<VTMotionBlurConfigurationRevision> ()")]
		HashSet<VTMotionBlurConfigurationRevision> SupportedRevisions { get; }

		[Static]
		[Export ("defaultRevision")]
		VTMotionBlurConfigurationRevision DefaultRevision { get; }
	}

	[NoMacCatalyst, NoTV, NoiOS, Mac (15, 4)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface VTMotionBlurParameters : VTFrameProcessorParameters {
		[Export ("initWithSourceFrame:nextFrame:previousFrame:nextOpticalFlow:previousOpticalFlow:motionBlurStrength:submissionMode:destinationFrame:")]
		NativeHandle Constructor (VTFrameProcessorFrame sourceFrame, [NullAllowed] VTFrameProcessorFrame nextFrame, [NullAllowed] VTFrameProcessorFrame previousFrame, [NullAllowed] VTFrameProcessorOpticalFlow nextOpticalFlow, [NullAllowed] VTFrameProcessorOpticalFlow previousOpticalFlow, nint motionBlurStrength, VTMotionBlurParametersSubmissionMode submissionMode, VTFrameProcessorFrame destinationFrame);

		[Export ("sourceFrame")]
		new VTFrameProcessorFrame SourceFrame { get; }

		[NullAllowed, Export ("nextFrame")]
		VTFrameProcessorFrame NextFrame { get; }

		[NullAllowed, Export ("previousFrame")]
		VTFrameProcessorFrame PreviousFrame { get; }

		[NullAllowed, Export ("nextOpticalFlow")]
		VTFrameProcessorOpticalFlow NextOpticalFlow { get; }

		[NullAllowed, Export ("previousOpticalFlow")]
		VTFrameProcessorOpticalFlow PreviousOpticalFlow { get; }

		[Export ("motionBlurStrength")]
		nint MotionBlurStrength { get; }

		[Export ("submissionMode")]
		VTMotionBlurParametersSubmissionMode SubmissionMode { get; }

		[Export ("destinationFrame")]
		VTFrameProcessorFrame DestinationFrame { get; }
	}

	[MacCatalyst (18, 4), NoTV, NoiOS, Mac (15, 4)]
	[Native]
	public enum VTOpticalFlowConfigurationQualityPrioritization : long {
		Normal = 1,
		Quality = 2,
	}

	[MacCatalyst (18, 4), NoTV, NoiOS, Mac (15, 4)]
	[Native]
	public enum VTOpticalFlowConfigurationRevision : long {
		Revision1 = 1,
	}

	[MacCatalyst (18, 4), NoTV, NoiOS, Mac (15, 4)]
	[Native]
	public enum VTOpticalFlowParametersSubmissionMode : long {
		Random = 1,
		Sequential = 2,
	}

	[NoMacCatalyst, NoTV, NoiOS, Mac (15, 4)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface VTOpticalFlowConfiguration : VTFrameProcessorConfiguration {
		[Export ("initWithFrameWidth:frameHeight:qualityPrioritization:revision:")]
		NativeHandle Constructor (nint frameWidth, nint frameHeight, VTOpticalFlowConfigurationQualityPrioritization qualityPrioritization, VTOpticalFlowConfigurationRevision revision);

		[Export ("frameWidth")]
		nint FrameWidth { get; }

		[Export ("frameHeight")]
		nint FrameHeight { get; }

		[Export ("qualityPrioritization")]
		VTOpticalFlowConfigurationQualityPrioritization QualityPrioritization { get; }

		[Export ("revision")]
		VTOpticalFlowConfigurationRevision Revision { get; }

		[Static]
		[Export ("supportedRevisions")]
		NSIndexSet WeakSupportedRevisions { get; }

		[Static]
		[Wrap ("WeakSupportedRevisions.ToInt64EnumHashSet<VTOpticalFlowConfigurationRevision> ()")]
		HashSet<VTOpticalFlowConfigurationRevision> SupportedRevisions { get; }

		[Static]
		[Export ("defaultRevision")]
		VTOpticalFlowConfigurationRevision DefaultRevision { get; }
	}

	[NoMacCatalyst, NoTV, NoiOS, Mac (15, 4)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface VTOpticalFlowParameters : VTFrameProcessorParameters {
		[Export ("initWithSourceFrame:nextFrame:submissionMode:destinationOpticalFlow:")]
		NativeHandle Constructor (VTFrameProcessorFrame sourceFrame, VTFrameProcessorFrame nextFrame, VTOpticalFlowParametersSubmissionMode submissionMode, VTFrameProcessorOpticalFlow destinationOpticalFlow);

		[Export ("sourceFrame")]
		new VTFrameProcessorFrame SourceFrame { get; }

		[Export ("nextFrame")]
		VTFrameProcessorFrame NextFrame { get; }

		[Export ("submissionMode")]
		VTOpticalFlowParametersSubmissionMode SubmissionMode { get; }

		[Export ("destinationOpticalFlow")]
		VTFrameProcessorOpticalFlow DestinationOpticalFlow { get; }
	}
}
