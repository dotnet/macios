//
// coremedia.cs: Definitions for CoreMedia
//
// Copyright 2014 Xamarin Inc. All rights reserved.
//

using System;

using CoreFoundation;
using Foundation;
using ObjCRuntime;

namespace CoreMedia {
	[Static]
	interface CMFormatDescriptionCleanApertureKeys {
		[Field ("kCMFormatDescriptionKey_CleanApertureWidth")]
		NSString Width { get; }

		[Field ("kCMFormatDescriptionKey_CleanApertureHeight")]
		NSString Height { get; }

		[Field ("kCMFormatDescriptionKey_CleanApertureHorizontalOffset")]
		NSString HorizontalOffset { get; }

		[Field ("kCMFormatDescriptionKey_CleanApertureVerticalOffset")]
		NSString VerticalOffset { get; }

		[Field ("kCMFormatDescriptionKey_CleanApertureWidthRational")]
		NSString WidthRational { get; }

		[Field ("kCMFormatDescriptionKey_CleanApertureHeightRational")]
		NSString HeightRational { get; }

		[Field ("kCMFormatDescriptionKey_CleanApertureHorizontalOffsetRational")]
		NSString HorizontalOffsetRational { get; }

		[Field ("kCMFormatDescriptionKey_CleanApertureVerticalOffsetRational")]
		NSString VerticalOffsetRational { get; }
	}

	[StrongDictionary ("CMFormatDescriptionCleanApertureKeys", Suffix = "")]
	interface CMFormatDescriptionCleanAperture {
		[NullAllowed]
		float Width { get; }

		[NullAllowed]
		float Height { get; }

		[NullAllowed]
		float HorizontalOffset { get; }

		[NullAllowed]
		float VerticalOffset { get; }

		/// <remarks>Array of two numbers: numerator and denominator. Also set <see cref="Width" /> because not all modules read the rational value.</remarks>
		[NullAllowed]
		NSNumber [] WidthRational { get; }

		/// <remarks>Array of two numbers: numerator and denominator. Also set <see cref="Height" /> because not all modules read the rational value.</remarks>
		[NullAllowed]
		NSNumber [] HeightRational { get; }

		/// <remarks>Array of two numbers: numerator and denominator. Also set <see cref="HorizontalOffset" /> because not all modules read the rational value.</remarks>
		[NullAllowed]
		NSNumber [] HorizontalOffsetRational { get; }

		/// <remarks>Array of two numbers: numerator and denominator. Also set <see cref="VerticalOffset" /> because not all modules read the rational value.</remarks>
		[NullAllowed]
		NSNumber [] VerticalOffsetRational { get; }
	}

	enum CMFormatDescriptionFieldDetail {
		[Field ("kCMFormatDescriptionFieldDetail_TemporalTopFirst")]
		TemporalTopFirst,

		[Field ("kCMFormatDescriptionFieldDetail_TemporalBottomFirst")]
		TemporalBottomFirst,

		[Field ("kCMFormatDescriptionFieldDetail_SpatialFirstLineEarly")]
		SpatialFirstLineEarly,

		[Field ("kCMFormatDescriptionFieldDetail_SpatialFirstLineLate")]
		SpatialFirstLineLate,
	}

	[Static]
	interface CMFormatDescriptionPixelAspectRatioKeys {
		[Field ("kCMFormatDescriptionKey_PixelAspectRatioHorizontalSpacing")]
		NSString HorizontalSpacing { get; }

		[Field ("kCMFormatDescriptionKey_PixelAspectRatioVerticalSpacing")]
		NSString VerticalSpacing { get; }
	}

	[StrongDictionary ("CMFormatDescriptionPixelAspectRatioKeys", Suffix = "")]
	interface CMFormatDescriptionPixelAspectRatio {
		[NullAllowed]
		nint HorizontalSpacing { get; set; }

		[NullAllowed]
		nint VerticalSpacing { get; set; }
	}

	enum CMFormatDescriptionColorPrimaries {
		[Field ("kCMFormatDescriptionColorPrimaries_ITU_R_709_2")]
		ITU_R_709_2,

		[Field ("kCMFormatDescriptionColorPrimaries_EBU_3213")]
		EBU_3213,

		[Field ("kCMFormatDescriptionColorPrimaries_SMPTE_C")]
		SMPTE_C,

		[Field ("kCMFormatDescriptionColorPrimaries_DCI_P3")]
		DCI_P3,

		[Field ("kCMFormatDescriptionColorPrimaries_P3_D65")]
		P3_D65,

		[Field ("kCMFormatDescriptionColorPrimaries_ITU_R_2020")]
		ITU_R_2020,

		[Field ("kCMFormatDescriptionColorPrimaries_P22")]
		P22,
	}

	enum CMFormatDescriptionTransferFunction {
		[Field ("kCMFormatDescriptionTransferFunction_ITU_R_709_2")]
		ITU_R_709_2,

		[Field ("kCMFormatDescriptionTransferFunction_SMPTE_240M_1995")]
		SMPTE_240M_1995,

		[Field ("kCMFormatDescriptionTransferFunction_UseGamma")]
		UseGamma,

		[Field ("kCMFormatDescriptionTransferFunction_ITU_R_2020")]
		ITU_R_2020,

		[Field ("kCMFormatDescriptionTransferFunction_SMPTE_ST_428_1")]
		SMPTE_ST_428_1,

		[Field ("kCMFormatDescriptionTransferFunction_SMPTE_ST_2084_PQ")]
		SMPTE_ST_2084_PQ,

		[Field ("kCMFormatDescriptionTransferFunction_ITU_R_2100_HLG")]
		ITU_R_2100_HLG,

		[Field ("kCMFormatDescriptionTransferFunction_Linear")]
		Linear,

		[Field ("kCMFormatDescriptionTransferFunction_sRGB")]
		SRGB,
	}

	enum CMFormatDescriptionYCbCrMatrix {
		[Field ("kCMFormatDescriptionYCbCrMatrix_ITU_R_709_2")]
		ITU_R_709_2,

		[Field ("kCMFormatDescriptionYCbCrMatrix_ITU_R_601_4")]
		ITU_R_601_4,

		[Field ("kCMFormatDescriptionYCbCrMatrix_SMPTE_240M_1995")]
		SMPTE_240M_1995,

		[Field ("kCMFormatDescriptionYCbCrMatrix_ITU_R_2020")]
		ITU_R_2020,
	}

	enum CMFormatDescriptionChromaLocation {
		[Field ("kCMFormatDescriptionChromaLocation_Left")]
		Left,

		[Field ("kCMFormatDescriptionChromaLocation_Center")]
		Center,

		[Field ("kCMFormatDescriptionChromaLocation_TopLeft")]
		TopLeft,

		[Field ("kCMFormatDescriptionChromaLocation_Top")]
		Top,

		[Field ("kCMFormatDescriptionChromaLocation_BottomLeft")]
		BottomLeft,

		[Field ("kCMFormatDescriptionChromaLocation_Bottom")]
		Bottom,

		[Field ("kCMFormatDescriptionChromaLocation_DV420")]
		DV420,
	}

	enum CMMPEG2VideoProfile : int {
		HDV_720p30 = 0x68647631, // 'hdv1'
		HDV_1080i60 = 0x68647632, // 'hdv2'
		HDV_1080i50 = 0x68647633, // 'hdv3'
		HDV_720p24 = 0x68647634, // 'hdv4'
		HDV_720p25 = 0x68647635, // 'hdv5'
		HDV_1080p24 = 0x68647636, // 'hdv6'
		HDV_1080p25 = 0x68647637, // 'hdv7'
		HDV_1080p30 = 0x68647638, // 'hdv8'
		HDV_720p60 = 0x68647639, // 'hdv9'
		HDV_720p50 = 0x68647661, // 'hdva'
		XDCAM_HD_1080i60_VBR35 = 0x78647632, // 'xdv2'
		XDCAM_HD_1080i50_VBR35 = 0x78647633, // 'xdv3'
		XDCAM_HD_1080p24_VBR35 = 0x78647636, // 'xdv6'
		XDCAM_HD_1080p25_VBR35 = 0x78647637, // 'xdv7'
		XDCAM_HD_1080p30_VBR35 = 0x78647638, // 'xdv8'
		XDCAM_EX_720p24_VBR35 = 0x78647634, // 'xdv4'
		XDCAM_EX_720p25_VBR35 = 0x78647635, // 'xdv5'
		XDCAM_EX_720p30_VBR35 = 0x78647631, // 'xdv1'
		XDCAM_EX_720p50_VBR35 = 0x78647661, // 'xdva'
		XDCAM_EX_720p60_VBR35 = 0x78647639, // 'xdv9'
		XDCAM_EX_1080i60_VBR35 = 0x78647662, // 'xdvb'
		XDCAM_EX_1080i50_VBR35 = 0x78647663, // 'xdvc'
		XDCAM_EX_1080p24_VBR35 = 0x78647664, // 'xdvd'
		XDCAM_EX_1080p25_VBR35 = 0x78647665, // 'xdve'
		XDCAM_EX_1080p30_VBR35 = 0x78647666, // 'xdvf'
		XDCAM_HD422_720p50_CBR50 = 0x78643561, // 'xd5a'
		XDCAM_HD422_720p60_CBR50 = 0x78643539, // 'xd59'
		XDCAM_HD422_1080i60_CBR50 = 0x78643562, // 'xd5b'
		XDCAM_HD422_1080i50_CBR50 = 0x78643563, // 'xd5c'
		XDCAM_HD422_1080p24_CBR50 = 0x78643564, // 'xd5d'
		XDCAM_HD422_1080p25_CBR50 = 0x78643565, // 'xd5e'
		XDCAM_HD422_1080p30_CBR50 = 0x78643566, // 'xd5f'
		XDCAM_HD_540p = 0x78646864, // 'xdhd'
		XDCAM_HD422_540p = 0x78646832, // 'xdh2'
		XDCAM_HD422_720p24_CBR50 = 0x78643534, // 'xd54'
		XDCAM_HD422_720p25_CBR50 = 0x78643535, // 'xd55'
		XDCAM_HD422_720p30_CBR50 = 0x78643531, // 'xd51'
		XF = 0x78667a31, // 'xfz1'
	}

	[iOS (13, 0), TV (13, 0)]
	enum CMFormatDescriptionAlphaChannelMode {
		[Field ("kCMFormatDescriptionAlphaChannelMode_StraightAlpha")]
		StraightAlpha,

		[Field ("kCMFormatDescriptionAlphaChannelMode_PremultipliedAlpha")]
		PremultipliedAlpha,
	}

	[iOS (17, 2), TV (17, 2), Mac (14, 2), MacCatalyst (17, 2)]
	enum CMFormatDescriptionLogTransferFunction {
		[Field ("kCMFormatDescriptionLogTransferFunction_AppleLog")]
		AppleLog,
	}

	[Mac (14, 0), iOS (17, 0), NoTV, MacCatalyst (17, 0)]
	enum CMFormatDescriptionHeroEye {
		[Field ("kCMFormatDescriptionHeroEye_Left")]
		Left,

		[Field ("kCMFormatDescriptionHeroEye_Right")]
		Right,
	}

	[Static]
	interface CMMetadataFormatDescriptionKeys {
		[Field ("kCMMetadataFormatDescriptionKey_Namespace")]
		NSString Namespace { get; }

		[Field ("kCMMetadataFormatDescriptionKey_Value")]
		NSString Value { get; }

		[Field ("kCMMetadataFormatDescriptionKey_LocalID")]
		NSString LocalId { get; }

		[Field ("kCMMetadataFormatDescriptionKey_DataType")]
		NSString DataType { get; }

		[Field ("kCMMetadataFormatDescriptionKey_DataTypeNamespace")]
		NSString DataTypeNamespace { get; }

		[Field ("kCMMetadataFormatDescriptionKey_ConformingDataTypes")]
		NSString ConformingDataTypes { get; }

		[Field ("kCMMetadataFormatDescriptionKey_LanguageTag")]
		NSString LanguageTag { get; }

		[Field ("kCMMetadataFormatDescriptionKey_StructuralDependency")]
		NSString StructuralDependency { get; }

		[Field ("kCMMetadataFormatDescriptionKey_SetupData")]
		NSString SetupData { get; }
	}

	[StrongDictionary ("CMMetadataFormatDescriptionKeys", Suffix = "")]
	interface CMMetadataFormatDescription {
		uint Namespace { get; }

		NSData Value { get; }

		uint LocalId { get; }

		NSData DataType { get; }

		int DataTypeNamespace { get; }

		/* Not sure what this is supposed to be, headers say "CFArray(CFDictionary) of DataType and DataTypeNamespace", which is not really clear, so binding as NSObject for now */
		NSObject ConformingDataTypes { get; }

		string LanguageTag { get; }

		NSDictionary StructuralDependency { get; }

		NSData SetupData { get; }
	}

	[Static]
	interface CMFormatDescriptionExtensionKeys {
		[Field ("kCMFormatDescriptionExtension_OriginalCompressionSettings")]
		NSString OriginalCompressionSettings { get; }

		[Field ("kCMFormatDescriptionExtension_SampleDescriptionExtensionAtoms")]
		NSString SampleDescriptionExtensionAtoms { get; }

		[Field ("kCMFormatDescriptionExtension_VerbatimSampleDescription")]
		NSString VerbatimSampleDescription { get; }

		[Field ("kCMFormatDescriptionExtension_VerbatimISOSampleEntry")]
		NSString VerbatimISOSampleEntry { get; }

		[Field ("kCMFormatDescriptionExtension_FormatName")]
		NSString FormatName { get; }

		[Field ("kCMFormatDescriptionExtension_Depth")]
		NSString Depth { get; }

		[Field ("kCMFormatDescriptionExtension_CleanAperture")]
		NSString CleanAperture { get; }

		[Field ("kCMFormatDescriptionExtension_FieldCount")]
		NSString FieldCount { get; }

		[Field ("kCMFormatDescriptionExtension_FieldDetail")]
		NSString FieldDetail { get; }

		[Field ("kCMFormatDescriptionExtension_PixelAspectRatio")]
		NSString PixelAspectRatio { get; }

		[Field ("kCMFormatDescriptionExtension_ColorPrimaries")]
		NSString ColorPrimaries { get; }

		[Field ("kCMFormatDescriptionExtension_TransferFunction")]
		NSString TransferFunction { get; }

		[Field ("kCMFormatDescriptionExtension_GammaLevel")]
		NSString GammaLevel { get; }

		[Field ("kCMFormatDescriptionExtension_YCbCrMatrix")]
		NSString YCbCrMatrix { get; }

		[Field ("kCMFormatDescriptionExtension_FullRangeVideo")]
		NSString FullRangeVideo { get; }

		[Field ("kCMFormatDescriptionExtension_ICCProfile")]
		NSString ICCProfile { get; }

		[Field ("kCMFormatDescriptionExtension_BytesPerRow")]
		NSString BytesPerRow { get; }

		[Field ("kCMFormatDescriptionExtension_ChromaLocationTopField")]
		NSString ChromaLocationTopField { get; }

		[Field ("kCMFormatDescriptionExtension_ChromaLocationBottomField")]
		NSString ChromaLocationBottomField { get; }

		[Field ("kCMFormatDescriptionConformsToMPEG2VideoProfile")]
		NSString ConformsToMPEG2VideoProfile { get; }

		[iOS (14, 0), TV (14, 0)]
		[Field ("kCMFormatDescriptionExtension_ProtectedContentOriginalFormat")]
		NSString ProtectedContentOriginalFormat { get; }

		[Field ("kCMFormatDescriptionExtension_TemporalQuality")]
		NSString TemporalQuality { get; }

		[Field ("kCMFormatDescriptionExtension_SpatialQuality")]
		NSString SpatialQuality { get; }

		[Field ("kCMFormatDescriptionExtension_VerbatimImageDescription")]
		NSString VerbatimImageDescription { get; }

		[Field ("kCMFormatDescriptionExtension_Version")]
		NSString Version { get; }

		[Field ("kCMFormatDescriptionExtension_RevisionLevel")]
		NSString RevisionLevel { get; }

		[Field ("kCMFormatDescriptionExtension_Vendor")]
		NSString Vendor { get; }

		[Field ("kCMFormatDescriptionExtension_MasteringDisplayColorVolume")]
		NSString MasteringDisplayColorVolume { get; }

		[Field ("kCMFormatDescriptionExtension_ContentLightLevelInfo")]
		NSString ContentLightLevelInfo { get; }

		[Field ("kCMFormatDescriptionExtensionKey_MetadataKeyTable")]
		NSString MetadataKeyTable { get; }

		[iOS (17, 0), TV (17, 0), MacCatalyst (17, 0), Mac (14, 0)]
		[Field ("kCMFormatDescriptionExtension_ContentColorVolume")]
		NSString ContentColorVolume { get; }

		[Field ("kCMFormatDescriptionExtension_AlternativeTransferCharacteristics")]
		NSString AlternativeTransferCharacteristics { get; }

		[iOS (13, 0), TV (13, 0)]
		[Field ("kCMFormatDescriptionExtension_AuxiliaryTypeInfo")]
		NSString AuxiliaryTypeInfo { get; }

		[iOS (13, 0), TV (13, 0)]
		[Field ("kCMFormatDescriptionExtension_AlphaChannelMode")]
		NSString AlphaChannelMode { get; }

		[iOS (13, 0), TV (13, 0)]
		[Field ("kCMFormatDescriptionExtension_ContainsAlphaChannel")]
		NSString ContainsAlphaChannel { get; }

		[iOS (15, 0), TV (15, 0)]
		[Field ("kCMFormatDescriptionExtension_BitsPerComponent")]
		NSString BitsPerComponent { get; }

		[iOS (15, 0), TV (15, 0)]
		[Field ("kCMFormatDescriptionExtension_HorizontalFieldOfView")]
		NSString HorizontalFieldOfView { get; }

		[iOS (15, 0), TV (15, 0)]
		[Field ("kCMFormatDescriptionExtension_AmbientViewingEnvironment")]
		NSString AmbientViewingEnvironment { get; }

		[iOS (17, 2), TV (17, 2), Mac (14, 2), MacCatalyst (17, 2)]
		[Field ("kCMFormatDescriptionExtension_LogTransferFunction")]
		NSString LogTransferFunction { get; }

		[iOS (17, 0), NoTV, Mac (14, 0), MacCatalyst (17, 0)]
		[Field ("kCMFormatDescriptionExtension_HeroEye")]
		NSString HeroEye { get; }

		[iOS (17, 0), NoTV, Mac (14, 0), MacCatalyst (17, 0)]
		[Field ("kCMFormatDescriptionExtension_StereoCameraBaseline")]
		NSString StereoCameraBaseline { get; }

		[iOS (17, 0), NoTV, Mac (14, 0), MacCatalyst (17, 0)]
		[Field ("kCMFormatDescriptionExtension_HorizontalDisparityAdjustment")]
		NSString HorizontalDisparityAdjustment { get; }

		[iOS (17, 0), NoTV, Mac (14, 0), MacCatalyst (17, 0)]
		[Field ("kCMFormatDescriptionExtension_HasLeftStereoEyeView")]
		NSString HasLeftStereoEyeView { get; }

		[iOS (17, 0), NoTV, Mac (14, 0), MacCatalyst (17, 0)]
		[Field ("kCMFormatDescriptionExtension_HasRightStereoEyeView")]
		NSString HasRightStereoEyeView { get; }

		[iOS (17, 0), NoTV, Mac (14, 0), MacCatalyst (17, 0)]
		[Field ("kCMFormatDescriptionExtension_HasAdditionalViews")]
		NSString HasAdditionalViews { get; }

		[iOS (18, 0), NoTV, Mac (15, 0), MacCatalyst (18, 0)]
		[Field ("kCMFormatDescriptionExtension_ProjectionKind")]
		NSString ProjectionKind { get; }

		[iOS (18, 0), NoTV, Mac (15, 0), MacCatalyst (18, 0)]
		[Field ("kCMFormatDescriptionExtension_ViewPackingKind")]
		NSString ViewPackingKind { get; }
	}

	[StrongDictionary ("CMFormatDescriptionExtensionKeys", Suffix = "")]
	interface CMFormatDescriptionExtensions {
		NSDictionary<NSString, NSObject> OriginalCompressionSettings { get; set; }

		NSDictionary<NSString, NSObject> SampleDescriptionExtensionAtoms { get; set; }

		NSData VerbatimSampleDescription { get; set; }

		NSData VerbatimISOSampleEntry { get; set; }

		string FormatName { get; set; }

		/// <seealso>http://developer.apple.com/qa/qa2001/qa1183.html </seealso>
		int Depth { get; set; }

		CMFormatDescriptionCleanAperture CleanAperture { get; set; }

		int FieldCount { get; set; }

		CMFormatDescriptionFieldDetail FieldDetail { get; set; }

		CMFormatDescriptionPixelAspectRatio PixelAspectRatio { get; set; }

		CMFormatDescriptionColorPrimaries ColorPrimaries { get; set; }

		CMFormatDescriptionTransferFunction TransferFunction { get; set; }

		nint GammaLevel { get; set; }

		CMFormatDescriptionYCbCrMatrix YCbCrMatrix { get; set; }

		bool FullRangeVideo { get; set; }

		NSData ICCProfile { get; set; }

		int BytesPerRow { get; set; }

		CMFormatDescriptionChromaLocation ChromaLocationTopField { get; set; }

		CMFormatDescriptionChromaLocation ChromaLocationBottomField { get; set; }

		CMMPEG2VideoProfile ConformsToMPEG2VideoProfile { get; set; }

		[iOS (14, 0), TV (14, 0)]
		int ProtectedContentOriginalFormat { get; set; }

		NSNumber TemporalQuality { get; set; }

		NSNumber SpatialQuality { get; set; }

		string VerbatimImageDescription { get; set; }

		int Version { get; set; }

		int RevisionLevel { get; set; }

		NSString Vendor { get; set; }

		NSData MasteringDisplayColorVolume { get; set; }

		NSData ContentLightLevelInfo { get; set; }

		CMMetadataFormatDescription MetadataKeyTable { get; }

		[iOS (17, 0), TV (17, 0), MacCatalyst (17, 0), Mac (14, 0)]
		NSData ContentColorVolume { get; set; }

		// not strongly typed to CMFormatDescriptionTransferFunction_ because other values are allowed as well
		NSString AlternativeTransferCharacteristics { get; set; }

		[iOS (13, 0), TV (13, 0)]
		string AuxiliaryTypeInfo { get; set; }

		[iOS (13, 0), TV (13, 0)]
		CMFormatDescriptionAlphaChannelMode AlphaChannelMode { get; set; }

		[iOS (13, 0), TV (13, 0)]
		bool ContainsAlphaChannel { get; set; }

		[iOS (15, 0), TV (15, 0)]
		int BitsPerComponent { get; set; }

		[iOS (15, 0), TV (15, 0)]
		nint HorizontalFieldOfView { get; set; }

		[iOS (15, 0), TV (15, 0)]
		NSData AmbientViewingEnvironment { get; set; }

		[iOS (17, 2), TV (17, 2), Mac (14, 2), MacCatalyst (17, 2)]
		// not strongly typed to CMFormatDescriptionLogTransferFunction because other values are allowed as well
		NSString LogTransferFunction { get; set; }

		[iOS (17, 0), NoTV, Mac (14, 0), MacCatalyst (17, 0)]
		CMFormatDescriptionHeroEye HeroEye { get; set; }

		[iOS (17, 0), NoTV, Mac (14, 0), MacCatalyst (17, 0)]
		uint StereoCameraBaseline { get; set; }

		[iOS (17, 0), NoTV, Mac (14, 0), MacCatalyst (17, 0)]
		int HorizontalDisparityAdjustment { get; set; }

		[iOS (17, 0), NoTV, Mac (14, 0), MacCatalyst (17, 0)]
		bool HasLeftStereoEyeView { get; set; }

		[iOS (17, 0), NoTV, Mac (14, 0), MacCatalyst (17, 0)]
		bool HasRightStereoEyeView { get; set; }

		[iOS (17, 0), NoTV, Mac (14, 0), MacCatalyst (17, 0)]
		bool HasAdditionalViews { get; set; }

		[iOS (18, 0), NoTV, Mac (15, 0), MacCatalyst (18, 0)]
		CMFormatDescriptionProjectionKind ProjectionKind { get; set; }

		[iOS (18, 0), NoTV, Mac (15, 0), MacCatalyst (18, 0)]
		CMFormatDescriptionViewPackingKind ViewPackingKind { get; set; }
	}

	/// <summary>Class that manages the repetitive allocation and deallocation of large blocks of memory.</summary>
	///     
	///     <!-- TODO: No Apple documentation on this as of 2013-05-01 -->
	[MacCatalyst (13, 1)]
	[Partial]
	interface CMMemoryPool {

		[Internal]
		[Field ("kCMMemoryPoolOption_AgeOutPeriod")]
		IntPtr AgeOutPeriodSelector { get; }
	}

	[NoTV, Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
	public enum CMFormatDescriptionProjectionKind {
		[Field ("kCMFormatDescriptionProjectionKind_Rectilinear")]
		Rectilinear,

		[Field ("kCMFormatDescriptionProjectionKind_Equirectangular")]
		Equirectangular,

		[Field ("kCMFormatDescriptionProjectionKind_HalfEquirectangular")]
		HalfEquirectangular,
	}

	[NoTV, Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
	public enum CMFormatDescriptionViewPackingKind {
		[Field ("kCMFormatDescriptionViewPackingKind_SideBySide")]
		SideBySide,

		[Field ("kCMFormatDescriptionViewPackingKind_OverUnder")]
		OverUnder,
	}

	[Static]
	[Internal]
	[MacCatalyst (13, 1)]
	interface CMTextMarkupAttributesKeys {
		[Internal]
		[Field ("kCMTextMarkupAttribute_ForegroundColorARGB")]
		NSString ForegroundColorARGB { get; }

		[Internal]
		[Field ("kCMTextMarkupAttribute_BackgroundColorARGB")]
		NSString BackgroundColorARGB { get; }

		[Internal]
		[Field ("kCMTextMarkupAttribute_BoldStyle")]
		NSString BoldStyle { get; }

		[Internal]
		[Field ("kCMTextMarkupAttribute_ItalicStyle")]
		NSString ItalicStyle { get; }

		[Internal]
		[Field ("kCMTextMarkupAttribute_UnderlineStyle")]
		NSString UnderlineStyle { get; }

		[Internal]
		[Field ("kCMTextMarkupAttribute_FontFamilyName")]
		NSString FontFamilyName { get; }

		[Internal]
		[Field ("kCMTextMarkupAttribute_RelativeFontSize")]
		NSString RelativeFontSize { get; }

		[Internal]
		[Field ("kCMTextMarkupAttribute_BaseFontSizePercentageRelativeToVideoHeight")]
		NSString BaseFontSizePercentageRelativeToVideoHeight { get; }
	}

	[Static]
	[Internal]
	[MacCatalyst (13, 1)]
	interface CMSampleAttachmentKey {
		[Field ("kCMSampleAttachmentKey_NotSync")]
		NSString NotSync { get; }

		[Field ("kCMSampleAttachmentKey_PartialSync")]
		NSString PartialSync { get; }

		[Field ("kCMSampleAttachmentKey_HasRedundantCoding")]
		NSString HasRedundantCoding { get; }

		[Field ("kCMSampleAttachmentKey_IsDependedOnByOthers")]
		NSString IsDependedOnByOthers { get; }

		[Field ("kCMSampleAttachmentKey_DependsOnOthers")]
		NSString DependsOnOthers { get; }

		[Field ("kCMSampleAttachmentKey_EarlierDisplayTimesAllowed")]
		NSString EarlierDisplayTimesAllowed { get; }

		[Field ("kCMSampleAttachmentKey_DisplayImmediately")]
		NSString DisplayImmediately { get; }

		[Field ("kCMSampleAttachmentKey_DoNotDisplay")]
		NSString DoNotDisplay { get; }

		[Field ("kCMSampleBufferAttachmentKey_ResetDecoderBeforeDecoding")]
		NSString ResetDecoderBeforeDecoding { get; }

		[Field ("kCMSampleBufferAttachmentKey_DrainAfterDecoding")]
		NSString DrainAfterDecoding { get; }

		[Field ("kCMSampleBufferAttachmentKey_PostNotificationWhenConsumed")]
		NSString PostNotificationWhenConsumedKey { get; }

		[Field ("kCMSampleBufferAttachmentKey_ResumeOutput")]
		NSString ResumeOutputKey { get; }

		[Field ("kCMSampleBufferAttachmentKey_TransitionID")]
		NSString TransitionIdKey { get; }

		[Field ("kCMSampleBufferAttachmentKey_TrimDurationAtStart")]
		NSString TrimDurationAtStartKey { get; }

		[Field ("kCMSampleBufferAttachmentKey_TrimDurationAtEnd")]
		NSString TrimDurationAtEndKey { get; }

		[Field ("kCMSampleBufferAttachmentKey_SpeedMultiplier")]
		NSString SpeedMultiplierKey { get; }

		[Field ("kCMSampleBufferAttachmentKey_Reverse")]
		NSString Reverse { get; }

		[Field ("kCMSampleBufferAttachmentKey_FillDiscontinuitiesWithSilence")]
		NSString FillDiscontinuitiesWithSilence { get; }

		[Field ("kCMSampleBufferAttachmentKey_EmptyMedia")]
		NSString EmptyMedia { get; }

		[Field ("kCMSampleBufferAttachmentKey_PermanentEmptyMedia")]
		NSString PermanentEmptyMedia { get; }

		[Field ("kCMSampleBufferAttachmentKey_DisplayEmptyMediaImmediately")]
		NSString DisplayEmptyMediaImmediately { get; }

		[Field ("kCMSampleBufferAttachmentKey_EndsPreviousSampleDuration")]
		NSString EndsPreviousSampleDuration { get; }

		[Field ("kCMSampleBufferAttachmentKey_SampleReferenceURL")]
		NSString SampleReferenceUrlKey { get; }

		[Field ("kCMSampleBufferAttachmentKey_SampleReferenceByteOffset")]
		NSString SampleReferenceByteOffsetKey { get; }

		[Field ("kCMSampleBufferAttachmentKey_GradualDecoderRefresh")]
		NSString GradualDecoderRefreshKey { get; }

		[NoMac]
		[MacCatalyst (13, 1)]
		[Field ("kCMSampleBufferAttachmentKey_DroppedFrameReason")]
		NSString DroppedFrameReason { get; }

		[NoMac]
		[MacCatalyst (13, 1)]
		[Field ("kCMSampleBufferAttachmentKey_StillImageLensStabilizationInfo")]
		NSString StillImageLensStabilizationInfo { get; }

		[NoMac]
		[MacCatalyst (13, 1)]
		[Field ("kCMSampleBufferLensStabilizationInfo_Active")]
		NSString BufferLensStabilizationInfo_Active { get; }

		[NoMac]
		[MacCatalyst (13, 1)]
		[Field ("kCMSampleBufferLensStabilizationInfo_OutOfRange")]
		NSString BufferLensStabilizationInfo_OutOfRange { get; }

		[NoMac]
		[MacCatalyst (13, 1)]
		[Field ("kCMSampleBufferLensStabilizationInfo_Unavailable")]
		NSString BufferLensStabilizationInfo_Unavailable { get; }

		[NoMac]
		[MacCatalyst (13, 1)]
		[Field ("kCMSampleBufferLensStabilizationInfo_Off")]
		NSString BufferLensStabilizationInfo_Off { get; }

		[MacCatalyst (13, 1)]
		[Field ("kCMSampleAttachmentKey_HEVCTemporalLevelInfo")]
		NSString HevcTemporalLevelInfoKey { get; }

		[MacCatalyst (13, 1)]
		[Field ("kCMSampleAttachmentKey_HEVCTemporalSubLayerAccess")]
		NSString HevcTemporalSubLayerAccessKey { get; }

		[MacCatalyst (13, 1)]
		[Field ("kCMSampleAttachmentKey_HEVCStepwiseTemporalSubLayerAccess")]
		NSString HevcStepwiseTemporalSubLayerAccessKey { get; }

		[MacCatalyst (13, 1)]
		[Field ("kCMSampleAttachmentKey_HEVCSyncSampleNALUnitType")]
		NSString HevcSyncSampleNalUnitTypeKey { get; }

		[MacCatalyst (13, 1)]
		[Field ("kCMSampleBufferAttachmentKey_CameraIntrinsicMatrix")]
		NSString CameraIntrinsicMatrixKey { get; }

		[iOS (13, 0), TV (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("kCMSampleAttachmentKey_AudioIndependentSampleDecoderRefreshCount")]
		NSString AudioIndependentSampleDecoderRefreshCountKey { get; }

		[MacCatalyst (13, 1)]
		[Field ("kCMSampleBufferAttachmentKey_ForceKeyFrame")]
		NSString ForceKeyFrameKey { get; }

		[TV (16, 0), Mac (13, 0), iOS (16, 0)]
		[MacCatalyst (16, 0)]
		[Field ("kCMSampleAttachmentKey_HDR10PlusPerFrameData")]
		NSString Hdr10PlusPerFrameDataKey { get; }

		[NoTV, NoiOS, NoMacCatalyst, Mac (15, 0)]
		[Field ("kCMSampleAttachmentKey_PostDecodeProcessingMetadata")]
		NSString PostDecodeProcessingMetadataKey { get; }
	}

	/// <summary>The keys for <see cref="CoreMedia.CMSampleBuffer" /> attachments.</summary>
	[MacCatalyst (13, 1)]
	[StrongDictionary ("CMSampleAttachmentKey")]
	interface CMSampleBufferAttachmentSettings {

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		NSDictionary PostNotificationWhenConsumed { get; set; }
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		bool ResumeOutput { get; set; }
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		int TransitionId { get; set; }
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		NSDictionary TrimDurationAtStart { get; set; }
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		NSDictionary TrimDurationAtEnd { get; set; }
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		float SpeedMultiplier { get; set; }
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		NSUrl SampleReferenceUrl { get; set; }
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		int SampleReferenceByteOffset { get; set; }
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		NSNumber GradualDecoderRefresh { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[StrongDictionary]
		CMHevcTemporalLevelInfoSettings HevcTemporalLevelInfo { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		bool HevcTemporalSubLayerAccess { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		bool HevcStepwiseTemporalSubLayerAccess { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		int HevcSyncSampleNalUnitType { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		NSData CameraIntrinsicMatrix { get; set; }

		[iOS (13, 0), TV (13, 0)]
		[MacCatalyst (13, 1)]
		nint AudioIndependentSampleDecoderRefreshCount { get; set; }

		[MacCatalyst (13, 1)]
		bool ForceKeyFrame { get; set; }

		[TV (16, 0), Mac (13, 0), iOS (16, 0)]
		[MacCatalyst (16, 0)]
		NSData Hdr10PlusPerFrameData { get; set; } // it is a CFData, but that is a toll-free bridged

		[NoTV, NoiOS, NoMacCatalyst, Mac (15, 0)]
		NSDictionary PostDecodeProcessingMetadata { get; set; } // it is a CFDictionary, but that is a toll-free bridged
	}

	[Internal]
	[MacCatalyst (13, 1)]
	[Static]
	interface CMHevcTemporalLevelInfoKeys {

		[Field ("kCMHEVCTemporalLevelInfoKey_TemporalLevel")]
		NSString TemporalLevelKey { get; }

		[Field ("kCMHEVCTemporalLevelInfoKey_ProfileSpace")]
		NSString ProfileSpaceKey { get; }

		[Field ("kCMHEVCTemporalLevelInfoKey_TierFlag")]
		NSString TierFlagKey { get; }

		[Field ("kCMHEVCTemporalLevelInfoKey_ProfileIndex")]
		NSString ProfileIndexKey { get; }

		[Field ("kCMHEVCTemporalLevelInfoKey_ProfileCompatibilityFlags")]
		NSString ProfileCompatibilityFlagsKey { get; }

		[Field ("kCMHEVCTemporalLevelInfoKey_ConstraintIndicatorFlags")]
		NSString ConstraintIndicatorFlagsKey { get; }

		[Field ("kCMHEVCTemporalLevelInfoKey_LevelIndex")]
		NSString LevelIndexKey { get; }
	}

	[MacCatalyst (13, 1)]
	[StrongDictionary ("CMHevcTemporalLevelInfoKeys")]
	interface CMHevcTemporalLevelInfoSettings {

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		int TemporalLevel { get; set; }
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		int ProfileSpace { get; set; }
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		int TierFlag { get; set; }
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		int ProfileIndex { get; set; }
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		NSData ProfileCompatibilityFlags { get; set; }
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		NSData ConstraintIndicatorFlags { get; set; }
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		int LevelIndex { get; set; }
	}

#if false
	// right now the generator can't add fields in a partial struct
	[Partial]
	interface CMTime {
		[Field ("kCMTimeValueKey")]
		NSString ValueKey { get; }

		[Field ("kCMTimeScaleKey")]
		NSString ScaleKey { get; }

		[Field ("kCMTimeEpochKey")]
		NSString EpochKey { get; }

		[Field ("kCMTimeFlagsKey")]
		NSString FlagsKey { get; }
	}
#endif

	[Flags]
	[TV (17, 0), Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0)]
	public enum CMStereoViewComponents : ulong {
		None = 0x0,
		LeftEye = 1uL << 0,
		RightEye = 1uL << 1,
	}

	[Flags]
	[TV (17, 0), Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0)]
	public enum CMStereoViewInterpretationOptions : ulong {
		Default = 0,
		StereoOrderReversed = 1uL << 0,
		AdditionalViews = 1uL << 1,
	}

	[MacCatalyst (17, 0), TV (17, 0), Mac (14, 0), iOS (17, 0)]
	public enum CMTagCollectionError {
		Success = 0,
		ParamErr = -15740,
		AllocationFailed = -15741,
		InternalError = -15742,
		InvalidTag = -15743,
		InvalidTagCollectionDictionary = -15744,
		InvalidTagCollectionData = -15745,
		TagNotFound = -15746,
		InvalidTagCollectionDataVersion = -15747,
		ExhaustedBufferSize = -15748,
		NotYetImplemented = -15749,
	}

	[TV (17, 0), Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0)]
	public enum CMTaggedBufferGroupError {
		Success = 0,
		ParamErr = -15780,
		AllocationFailed = -15781,
		InternalError = -15782,
	}

	[MacCatalyst (17, 0), TV (17, 0), Mac (14, 0), iOS (17, 0)]
	public enum CMTagError {
		Success = 0,
		ParamErr = -15730,
		AllocationFailed = -15731,
	}

	[MacCatalyst (17, 0), TV (17, 0), Mac (14, 0), iOS (17, 0)]
	public enum CMTagCategory : uint {
		Undefined = 0,
		MediaType = ('m' << 24) + ('d' << 16) + ('i' << 8) + 'a', // 'mdia'
		MediaSubType = ('m' << 24) + ('s' << 16) + ('u' << 8) + 'b', // 'msub'
		TrackId = ('t' << 24) + ('r' << 16) + ('a' << 8) + 'k', // 'trak'
		ChannelId = ('v' << 24) + ('c' << 16) + ('h' << 8) + 'n', // 'vchn'
		VideoLayerId = ('v' << 24) + ('l' << 16) + ('a' << 8) + 'y', // 'vlay'
		PixelFormat = ('p' << 24) + ('i' << 16) + ('x' << 8) + 'f', // 'pixf'
		PackingType = ('p' << 24) + ('a' << 16) + ('c' << 8) + 'k', // 'pack'
		ProjectionType = ('p' << 24) + ('r' << 16) + ('o' << 8) + 'j', // 'proj'
		StereoView = ('e' << 24) + ('y' << 16) + ('e' << 8) + 's', // 'eyes'
		StereoViewInterpretation = ('e' << 24) + ('y' << 16) + ('i' << 8) + 'p', // 'eyip'
	}

	[MacCatalyst (17, 0), TV (17, 0), Mac (14, 0), iOS (17, 0)]
	public enum CMTagDataType : uint {
		Invalid = 0,
		SInt64 = 2,
		Float64 = 3,
		OSType = 5,
		Flags = 7,
	}

	[Internal]
	[Partial]
	interface CMTagCollectionConstants {
		[MacCatalyst (17, 0), TV (17, 0), Mac (14, 0), iOS (17, 0)]
		[Field ("kCMTagCollectionTagsArrayKey")]
		NSString ArrayKey { get; }
	}

	[Internal]
	[Partial]
	[MacCatalyst (17, 0), TV (17, 0), Mac (14, 0), iOS (17, 0)]
	interface CMTagConstants {
		[Field ("kCMTagInvalid")]
		CMTag Invalid { get; }

		[Field ("kCMTagMediaTypeVideo")]
		CMTag MediaTypeVideo { get; }

		[Field ("kCMTagMediaSubTypeMebx")]
		CMTag MediaSubTypeMebx { get; }

		[Field ("kCMTagMediaTypeAudio")]
		CMTag MediaTypeAudio { get; }

		[Field ("kCMTagMediaTypeMetadata")]
		CMTag MediaTypeMetadata { get; }

		[Field ("kCMTagStereoLeftEye")]
		CMTag StereoLeftEye { get; }

		[Field ("kCMTagStereoRightEye")]
		CMTag StereoRightEye { get; }

		[Field ("kCMTagStereoLeftAndRightEye")]
		CMTag StereoLeftAndRightEye { get; }

		[Field ("kCMTagStereoNone")]
		CMTag StereoNone { get; }

		[Field ("kCMTagStereoInterpretationOrderReversed")]
		CMTag StereoInterpretationOrderReversed { get; }

		[Field ("kCMTagProjectionTypeRectangular")]
		CMTag ProjectionTypeRectangular { get; }

		[Field ("kCMTagProjectionTypeEquirectangular")]
		CMTag ProjectionTypeEquirectangular { get; }

		[iOS (18, 0), TV (18, 0), MacCatalyst (18, 0), Mac (15, 0)]
		[Field ("kCMTagProjectionTypeHalfEquirectangular")]
		CMTag ProjectionTypeHalfEquirectangular { get; }

		[Field ("kCMTagProjectionTypeFisheye")]
		CMTag ProjectionTypeFisheye { get; }

		[Field ("kCMTagPackingTypeNone")]
		CMTag PackingTypeNone { get; }

		[Field ("kCMTagPackingTypeSideBySide")]
		CMTag PackingTypeSideBySide { get; }

		[Field ("kCMTagPackingTypeOverUnder")]
		CMTag PackingTypeOverUnder { get; }

		[Field ("kCMTagValueKey")]
		NSString ValueKey { get; }

		[Field ("kCMTagCategoryKey")]
		NSString CategoryKey { get; }

		[Field ("kCMTagDataTypeKey")]
		NSString DataTypeKey { get; }
	}

	[MacCatalyst (17, 0), TV (17, 0), Mac (14, 0), iOS (17, 0)]
	public enum CMProjectionType : ulong {
		Rectangular = ('r' << 24) + ('e' << 16) + ('c' << 8) + 't', // 'rect',
		Equirectangular = ('e' << 24) + ('q' << 16) + ('u' << 8) + 'i', // 'equi',
		HalfEquirectangular = ('h' << 24) + ('e' << 16) + ('q' << 8) + 'u', // 'hequ',
		Fisheye = ('f' << 24) + ('i' << 16) + ('s' << 8) + 'h', // 'fish',
	}

	[MacCatalyst (17, 0), TV (17, 0), Mac (14, 0), iOS (17, 0)]
	public enum CMPackingType : ulong {
		None = ('n' << 24) + ('o' << 16) + ('n' << 8) + 'e', // 'none',
		SideBySide = ('s' << 24) + ('i' << 16) + ('d' << 8) + 'e', // 'side',
		OverUnder = ('o' << 24) + ('v' << 16) + ('e' << 8) + 'r', // 'over',
	}

	[MacCatalyst (17, 0), TV (17, 0), Mac (14, 0), iOS (17, 0)]
	public enum CMTaggedBufferGroupFormatType {
		TaggedBufferGroup = ('t' << 24) + ('b' << 16) + ('g' << 8) + 'r', // 'tbgr',
	}
}
