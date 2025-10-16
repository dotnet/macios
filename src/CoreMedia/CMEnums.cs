// 
// CMEnums.cs: Enumerations for CoreMedia
//
// Authors: Mono Team
//          Marek Safar (marek.safar@gmail.com)
//
// Copyright 2010-2011 Novell Inc
// Copyright 2012-2014 Xamarin Inc
//

using AudioToolbox;

#nullable enable

namespace CoreMedia {

	// FourCharCode -> CMFormatDescription.h
	/// <summary>An enumeration whose values specify valid types of media.</summary>
	[MacCatalyst (13, 1)]
	public enum CMMediaType : uint {
		/// <summary>To be added.</summary>
		Video = 1986618469, // 'vide'
		/// <summary>To be added.</summary>
		Audio = 1936684398, // 'soun'
		/// <summary>To be added.</summary>
		Muxed = 1836415096, // 'muxx'
		/// <summary>To be added.</summary>
		Text = 1952807028, // 'text'
		/// <summary>To be added.</summary>
		ClosedCaption = 1668047728, // 'clcp'
		/// <summary>To be added.</summary>
		Subtitle = 1935832172, // 'sbtl'
		/// <summary>To be added.</summary>
		TimeCode = 1953325924, // 'tmcd'
							   // note: the 4CC was obsoleted, i.e. Metadata is a new 4CC
		/// <summary>To be added.</summary>
		Metadata = 0x6D657461, // 'meta'
		[TV (17, 0), Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0)]
		TaggedBufferGroup = ('t' << 24) + ('b' << 16) + ('g' << 8) + 'r', // 'tbgr',
	}

	// FourCharCode -> CMFormatDescription.h
	/// <summary>An enumeration whose values specify a closed-captioning standard.</summary>
	[MacCatalyst (13, 1)]
	public enum CMClosedCaptionFormatType : uint {
		/// <summary>To be added.</summary>
		CEA608 = 0x63363038, // 'c608',
		/// <summary>To be added.</summary>
		CEA708 = 0x63373038, // 'c708',
		/// <summary>To be added.</summary>
		ATSC = 0x61746363, // 'atcc'

		// any new entries here might need new entries in the CMMediaSubType enum as well
	}

	// FourCharCode -> CMFormatDescription.h
	/// <summary>An enumeration whose values specify valid types of multiplexed media.</summary>
	[MacCatalyst (13, 1)]
	public enum CMMuxedStreamType : uint {
		/// <summary>To be added.</summary>
		MPEG1System = 0x6D703173, // 'mp1s',
		/// <summary>To be added.</summary>
		MPEG2Transport = 0x6D703274, // 'mp2t',
		/// <summary>To be added.</summary>
		MPEG2Program = 0x6D703270, // 'mp2p',
		/// <summary>To be added.</summary>
		DV = 0x64762020, // 'dv  '
		[iOS (17, 0), MacCatalyst (17, 0), TV (17, 0), Mac (14, 0)]
		EmbeddedDeviceScreenRecording = 0x69737220, // 'isr '

		// any new entries here might need new entries in the CMMediaSubType enum as well
	}

	// FourCharCode -> CMFormatDescription.h
	/// <summary>An enumeration whose values specify a subtitling standard.</summary>
	[MacCatalyst (13, 1)]
	public enum CMSubtitleFormatType : uint {
		/// <summary>To be added.</summary>
		Text3G = 0x74783367, // 'tx3g'
		/// <summary>To be added.</summary>
		WebVTT = 0x77767474, // 'wvtt'

		// any new entries here might need new entries in the CMMediaSubType enum as well
	}

	// FourCharCode -> CMFormatDescription.h
	/// <summary>An enumeration whose values specify types of timed metadata.</summary>
	[MacCatalyst (13, 1)]
	public enum CMMetadataFormatType : uint {
		/// <summary>To be added.</summary>
		ICY = 0x69637920, // 'icy '
		/// <summary>To be added.</summary>
		ID3 = 0x69643320, // 'id3 '
		/// <summary>To be added.</summary>
		Boxed = 0x6d656278, // 'mebx'
		/// <summary>To be added.</summary>
		Emsg = 0x656d7367, // 'emsg'

		// any new entries here might need new entries in the CMMediaSubType enum as well
	}

	// FourCharCode -> CMFormatDescription.h
	/// <summary>An enumeration whose values specify the type of a time code.</summary>
	[MacCatalyst (13, 1)]
	public enum CMTimeCodeFormatType : uint {
		/// <summary>To be added.</summary>
		TimeCode32 = 0x746D6364, // 'tmcd',
		/// <summary>To be added.</summary>
		TimeCode64 = 0x74633634, // 'tc64',
		/// <summary>To be added.</summary>
		Counter32 = 0x636E3332, // 'cn32',
		/// <summary>To be added.</summary>
		Counter64 = 0x636E3634, // 'cn64'

		// any new entries here might need new entries in the CMMediaSubType enum as well
	}

	// uint32_t -> CMTime.h
	/// <summary>An enumeration whose values specify the rounding method to be used with a <see cref="CoreMedia.CMTime" />.</summary>
	[MacCatalyst (13, 1)]
	public enum CMTimeRoundingMethod : uint {
		/// <summary>To be added.</summary>
		RoundHalfAwayFromZero = 1,
		/// <summary>To be added.</summary>
		RoundTowardZero = 2,
		/// <summary>To be added.</summary>
		RoundAwayFromZero = 3,
		/// <summary>To be added.</summary>
		QuickTime = 4,
		/// <summary>To be added.</summary>
		RoundTowardPositiveInfinity = 5,
		/// <summary>To be added.</summary>
		RoundTowardNegativeInfinity = 6,
		/// <summary>To be added.</summary>
		Default = RoundHalfAwayFromZero,
	}

	// FourCharCode -> CMFormatDescription.h
	/// <summary>An enumeration whose values specify types of video codecs.</summary>
	[MacCatalyst (13, 1)]
	public enum CMVideoCodecType : uint {
		/// <summary>Indicates YCbCR content.</summary>
		YUV422YpCbCr8 = 0x32767579,
		/// <summary>Indicates Apple animation format.</summary>
		Animation = 0x726c6520,
		/// <summary>Indicates Cinepak content.</summary>
		Cinepak = 0x63766964,
		/// <summary>Indicates JPEG content.</summary>
		JPEG = 0x6a706567,
		/// <summary>Indicates Open-DML JPEG content.</summary>
		JPEG_OpenDML = 0x646d6231,
		JPEG_XL = ('j' << 24) + ('x' << 16) + ('l' << 8) + 'c', // 'jxlc'
		/// <summary>Indicates Sorenson content.</summary>
		SorensonVideo = 0x53565131,
		/// <summary>Indicates Sorenson 3 content.</summary>
		SorensonVideo3 = 0x53565133,
		/// <summary>Indicates ITU-T H.263 content.</summary>
		H263 = 0x68323633,
		/// <summary>Indicates ITU-T H.264 content.</summary>
		H264 = 0x61766331,
		/// <summary>Indicates MPEG-4 Part 2 content.</summary>
		Mpeg4Video = 0x6d703476,
		/// <summary>Indicates MPEG-2 content.</summary>
		Mpeg2Video = 0x6d703276,
		/// <summary>Indicates MPEG-1 content.</summary>
		Mpeg1Video = 0x6d703176,
		[iOS (14, 0), TV (14, 0)]
		[MacCatalyst (14, 0)]
		VP9 = 0x76703039,
		/// <summary>Indicates DV NTSC content.</summary>
		DvcNtsc = 0x64766320,
		/// <summary>Indicates DVC PAL content.</summary>
		DvcPal = 0x64766370,
		/// <summary>Indicates Panasonic DVCPro PAL content.</summary>
		DvcProPal = 0x64767070,
		/// <summary>Indicates Panasonic DVCPro-50 NTSC content.</summary>
		DvcPro50NTSC = 0x6476356e,
		/// <summary>Indicates Panasonic DVCPro PAL content.</summary>
		DvcPro50PAL = 0x64763570,
		/// <summary>Indicates Panasonic DVCPro-HD 720p60 content.</summary>
		DvcProHD720p60 = 0x64766870,
		/// <summary>Indicates Panasonic DVCPro-HD 720p50 content.</summary>
		DvcProHD720p50 = 0x64766871,
		/// <summary>Indicates Panasonic DVCPro-HD 1080i60 content.</summary>
		DvcProHD1080i60 = 0x64766836,
		/// <summary>Indicates Panasonic DVCPro-HD 1080i50 content.</summary>
		DvcProHD1080i50 = 0x64766835,
		/// <summary>Indicates Panasonic DVCPro-HD 1080p30 content.</summary>
		DvcProHD1080p30 = 0x64766833,
		/// <summary>Indicates Panasonic DVCPro-HD 1080p25 content.</summary>
		DvcProHD1080p25 = 0x64766832,
		AppleProRes4444XQ = 0x61703478, // 'ap4x'
		/// <summary>To be added.</summary>
		AppleProRes4444 = 0x61703468,
		/// <summary>To be added.</summary>
		AppleProRes422HQ = 0x61706368,
		/// <summary>To be added.</summary>
		AppleProRes422 = 0x6170636e,
		/// <summary>To be added.</summary>
		AppleProRes422LT = 0x61706373,
		/// <summary>To be added.</summary>
		AppleProRes422Proxy = 0x6170636f,
		AppleProResRaw = 0x6170726e, // 'aprn'
		AppleProResRawHQ = 0x61707268, // 'aprh'
		/// <summary>To be added.</summary>
		Hevc = 0x68766331,
		HevcWithAlpha = 0x6d757861, // 'muxa'
		[iOS (14, 5)]
		[TV (14, 5)]
		[MacCatalyst (14, 5)]
		DolbyVisionHevc = 0x64766831,
		DisparityHevc = 0x64697368,
		DepthHevc = 0x64657068,
		/// <summary>Indicates AV1 content.</summary>
		AV1 = 0x61763031, // 'av01'

		// any new entries here might need new entries in the CMMediaSubType enum as well
	}

	// UInt32 enum => CMFormatDescription.h
	/// <summary>Enumerates pixel formats.</summary>
	[MacCatalyst (13, 1)]
	public enum CMPixelFormat : uint {
		/// <summary>To be added.</summary>
		AlphaRedGreenBlue32bits = 32,
		/// <summary>To be added.</summary>
		BlueGreenRedAlpha32bits = 1111970369, // 'BGRA'
		/// <summary>To be added.</summary>
		RedGreenBlue24bits = 24,
		/// <summary>To be added.</summary>
		BigEndian555_16bits = 16,
		/// <summary>To be added.</summary>
		BigEndian565_16bits = 1110783541, // 'B565'
		/// <summary>To be added.</summary>
		LittleEndian555_16bits = 1278555445, // 'L555'
		/// <summary>To be added.</summary>
		LittleEndian565_16bits = 1278555701, // 'L565'
		/// <summary>To be added.</summary>
		LittleEndian5551_16bits = 892679473, // '5551'
		/// <summary>To be added.</summary>
		YpCbCr422_8bits = 846624121, // '2vuy'
		/// <summary>To be added.</summary>
		YpCbCr422yuvs_8bits = 2037741171, // 'yuvs'
		/// <summary>To be added.</summary>
		YpCbCr444_8bits = 1983066168, // 'v308'
		/// <summary>To be added.</summary>
		YpCbCrA4444_8bits = 1983131704, // 'v408'
		/// <summary>To be added.</summary>
		YpCbCr422_16bits = 1983000886, // 'v216'
		/// <summary>To be added.</summary>
		YpCbCr422_10bits = 1983000880, // 'v210'
		/// <summary>To be added.</summary>
		YpCbCr444_10bits = 1983131952, // 'v410'
		/// <summary>To be added.</summary>
		IndexedGrayWhiteIsZero_8bits = 40,

		// any new entries here might need new entries in the CMMediaSubType enum as well
	}

	/// <summary>Enumerates attachment propagation policies.</summary>
	///     <remarks>This setting controls whether <see cref="CoreMedia.CMAttachmentBearer.PropagateAttachments(CoreMedia.ICMAttachmentBearer,CoreMedia.ICMAttachmentBearer)" /> will propagate a given attachment.</remarks>
	[MacCatalyst (13, 1)]
	public enum CMAttachmentMode : uint {
		/// <summary>Indicates that the attachment does not propagate.</summary>
		ShouldNotPropagate = 0,
		/// <summary>Indicates that the attachment will propagate.</summary>
		ShouldPropagate = 1,
	};

	// untyped enum (used as OSStatus) -> CMBlockBuffer.h
	/// <summary>An enumeration whose values specify errors relating to <see cref="CoreMedia.CMBlockBuffer" />s.</summary>
	[MacCatalyst (13, 1)]
	public enum CMBlockBufferError : int {
		/// <summary>To be added.</summary>
		None = 0,
		/// <summary>To be added.</summary>
		StructureAllocationFailed = -12700,
		/// <summary>To be added.</summary>
		BlockAllocationFailed = -12701,
		/// <summary>To be added.</summary>
		BadCustomBlockSource = -12702,
		/// <summary>To be added.</summary>
		BadOffsetParameter = -12703,
		/// <summary>To be added.</summary>
		BadLengthParameter = -12704,
		/// <summary>To be added.</summary>
		BadPointerParameter = -12705,
		/// <summary>To be added.</summary>
		EmptyBlockBuffer = -12706,
		/// <summary>To be added.</summary>
		UnallocatedBlock = -12707,
		/// <summary>To be added.</summary>
		InsufficientSpace = -12708,
	}

	// uint32_t -> CMBlockBuffer.h
	/// <summary>An enumeration of flags to be used with <see cref="CoreMedia.CMBlockBuffer.CreateEmpty(System.UInt32,CoreMedia.CMBlockBufferFlags,out CoreMedia.CMBlockBufferError)" />.</summary>
	[Flags]
	[MacCatalyst (13, 1)]
	public enum CMBlockBufferFlags : uint {
		/// <summary>To be added.</summary>
		AssureMemoryNow = (1 << 0),
		/// <summary>To be added.</summary>
		AlwaysCopyData = (1 << 1),
		/// <summary>To be added.</summary>
		DontOptimizeDepth = (1 << 2),
		/// <summary>To be added.</summary>
		PermitEmptyReference = (1 << 3),
	}

	// untyped enum (uses as OSStatus) -> CMFormatDescription.h
	/// <summary>An enumeration whose values specify errors relating to <see cref="CoreMedia.CMFormatDescription" />s.</summary>
	[MacCatalyst (13, 1)]
	public enum CMFormatDescriptionError : int {
		/// <summary>To be added.</summary>
		None = 0,
		/// <summary>To be added.</summary>
		InvalidParameter = -12710,
		/// <summary>To be added.</summary>
		AllocationFailed = -12711,
		/// <summary>To be added.</summary>
		ValueNotAvailable = -12718,
	}

	// untyped enum (used as an OSStatus) -> CMSampleBuffer.h
	/// <summary>An enumeration whose values specify errors relating to <see cref="CoreMedia.CMSampleBuffer" />s.</summary>
	[MacCatalyst (13, 1)]
	public enum CMSampleBufferError : int {
		/// <summary>To be added.</summary>
		None = 0,
		/// <summary>To be added.</summary>
		AllocationFailed = -12730,
		/// <summary>To be added.</summary>
		RequiredParameterMissing = -12731,
		/// <summary>To be added.</summary>
		AlreadyHasDataBuffer = -12732,
		/// <summary>To be added.</summary>
		BufferNotReady = -12733,
		/// <summary>To be added.</summary>
		SampleIndexOutOfRange = -12734,
		/// <summary>To be added.</summary>
		BufferHasNoSampleSizes = -12735,
		/// <summary>To be added.</summary>
		BufferHasNoSampleTimingInfo = -12736,
		/// <summary>To be added.</summary>
		ArrayTooSmall = -12737,
		/// <summary>To be added.</summary>
		InvalidEntryCount = -12738,
		/// <summary>To be added.</summary>
		CannotSubdivide = -12739,
		/// <summary>To be added.</summary>
		SampleTimingInfoInvalid = -12740,
		/// <summary>To be added.</summary>
		InvalidMediaTypeForOperation = -12741,
		/// <summary>To be added.</summary>
		InvalidSampleData = -12742,
		/// <summary>To be added.</summary>
		InvalidMediaFormat = -12743,
		/// <summary>To be added.</summary>
		Invalidated = -12744,
	}

	public enum LensStabilizationStatus {
		/// <summary>To be added.</summary>
		Active,
		/// <summary>To be added.</summary>
		OutOfRange,
		/// <summary>To be added.</summary>
		Unavailable,
		/// <summary>To be added.</summary>
		Off,
		/// <summary>To be added.</summary>
		None,
	}

	// untyped enum (used as OSStatus) -> CMSync.h
	/// <summary>An enumeration whose values specify errors relating to <see cref="CoreMedia.CMClock" />s.</summary>
	[MacCatalyst (13, 1)]
	public enum CMClockError : int {
		/// <summary>To be added.</summary>
		None = 0,
		/// <summary>To be added.</summary>
		MissingRequiredParameter = -12745,
		/// <summary>To be added.</summary>
		InvalidParameter = -12746,
		/// <summary>To be added.</summary>
		AllocationFailed = -12747,
		/// <summary>To be added.</summary>
		UnsupportedOperation = -12756,
	}

	// untyped enum (used as OSStatus) -> CMSync.h
	/// <summary>An enumeration whose values specify errors relating to <see cref="CoreMedia.CMTimebase" />s.</summary>
	[MacCatalyst (13, 1)]
	public enum CMTimebaseError : int {
		/// <summary>To be added.</summary>
		None = 0,
		/// <summary>To be added.</summary>
		MissingRequiredParameter = -12748,
		/// <summary>To be added.</summary>
		InvalidParameter = -12749,
		/// <summary>To be added.</summary>
		AllocationFailed = -12750,
		/// <summary>To be added.</summary>
		TimerIntervalTooShort = -12751,
		/// <summary>To be added.</summary>
		ReadOnly = -12757,
	}

	// untyped enum (used as OSStatus) -> CMSync.h
	/// <summary>An enumeration whose values specify errors returned by <see cref="CoreMedia.CMClockOrTimebase.GetRelativeRateAndAnchorTime(CoreMedia.CMClockOrTimebase,CoreMedia.CMClockOrTimebase,out System.Double,out CoreMedia.CMTime,out CoreMedia.CMTime)" />.</summary>
	[MacCatalyst (13, 1)]
	public enum CMSyncError : int {
		/// <summary>To be added.</summary>
		None = 0,
		/// <summary>To be added.</summary>
		MissingRequiredParameter = -12752,
		/// <summary>To be added.</summary>
		InvalidParameter = -12753,
		/// <summary>To be added.</summary>
		AllocationFailed = -12754,
		/// <summary>To be added.</summary>
		RateMustBeNonZero = -12755,
	}

	[MacCatalyst (13, 1)]
	public enum CMAudioCodecType : uint {
		AacLCProtected = 0x70616163, // 'paac'
		AacAudibleProtected = 0x61616163, // 'aaac'

		// any new entries here might need new entries in the CMMediaSubType enum as well
	}

	[MacCatalyst (13, 1)]
	public enum CMTextFormatType : uint {
		QTText = 0x74657874, // 'text'
		Text3G = 0x74783367, // 'tx3g'

		// any new entries here might need new entries in the CMMediaSubType enum as well
	}

	// The entries from this enum is from the Swift struct CMFormatDescription.MediaSubType (https://developer.apple.com/documentation/coremedia/cmformatdescription/mediasubtype-swift.struct)
	// The easiest way to get a list of all the names is to look in /Applications/Xcode.app/Contents/Developer/Platforms/iPhoneOS.platform/Developer/SDKs/iPhoneOS.sdk/usr/lib/swift/CoreMedia.swiftmodule/arm64e-apple-ios.swiftinterface
	// Then the values are looked up manually using the corresponding Objective-C constant (this involves a lot of searching and guessing)
	/// <summary>This enum contains most values that can be returned from <see cref="CMFormatDescription.MediaSubType" />.</summary>
	[MacCatalyst (13, 1)]
	public enum CMMediaSubType : uint {
		/// <summary>This has the same value as <see cref="AudioFormatType.LinearPCM" />.</summary>
		LinearPcm = (uint) AudioFormatType.LinearPCM,
		/// <summary>This has the same value as <see cref="AudioFormatType.AC3" />.</summary>
		Ac3 = (uint) AudioFormatType.AC3,
		/// <summary>This has the same value as <see cref="AudioFormatType.AC3_60958" />.</summary>
		Iec60958Ac3 = (uint) AudioFormatType.AC3_60958,
		/// <summary>This has the same value as <see cref="AudioFormatType.AppleIMA4" />.</summary>
		AppleIma4 = (uint) AudioFormatType.AppleIMA4,
		/// <summary>This has the same value as <see cref="AudioFormatType.MPEG4AAC" />.</summary>
		Mpeg4Aac = (uint) AudioFormatType.MPEG4AAC,
		/// <summary>This has the same value as <see cref="AudioFormatType.MPEG4CELP" />.</summary>
		Mpeg4Celp = (uint) AudioFormatType.MPEG4CELP,
		/// <summary>This has the same value as <see cref="AudioFormatType.MPEG4HVXC" />.</summary>
		Mpeg4Hvxc = (uint) AudioFormatType.MPEG4HVXC,
		/// <summary>This has the same value as <see cref="AudioFormatType.MPEG4TwinVQ" />.</summary>
		Mpeg4TwinVQ = (uint) AudioFormatType.MPEG4TwinVQ,
		/// <summary>This has the same value as <see cref="AudioFormatType.MACE3" />.</summary>
		Mace3 = (uint) AudioFormatType.MACE3,
		/// <summary>This has the same value as <see cref="AudioFormatType.MACE6" />.</summary>
		Mace6 = (uint) AudioFormatType.MACE6,
		/// <summary>This has the same value as <see cref="AudioFormatType.ULaw" />.</summary>
		ULaw = (uint) AudioFormatType.ULaw,
		/// <summary>This has the same value as <see cref="AudioFormatType.ALaw" />.</summary>
		ALaw = (uint) AudioFormatType.ALaw,
		/// <summary>This has the same value as <see cref="AudioFormatType.QDesign" />.</summary>
		QDesign = (uint) AudioFormatType.QDesign,
		/// <summary>This has the same value as <see cref="AudioFormatType.QDesign2" />.</summary>
		QDesign2 = (uint) AudioFormatType.QDesign2,
		/// <summary>This has the same value as <see cref="AudioFormatType.QUALCOMM" />.</summary>
		Qualcomm = (uint) AudioFormatType.QUALCOMM,
		/// <summary>This has the same value as <see cref="AudioFormatType.MPEGLayer1" />.</summary>
		MpegLayer1 = (uint) AudioFormatType.MPEGLayer1,
		/// <summary>This has the same value as <see cref="AudioFormatType.MPEGLayer2" />.</summary>
		MpegLayer2 = (uint) AudioFormatType.MPEGLayer2,
		/// <summary>This has the same value as <see cref="AudioFormatType.MPEGLayer3" />.</summary>
		MpegLayer3 = (uint) AudioFormatType.MPEGLayer3,
		/// <summary>This has the same value as <see cref="AudioFormatType.TimeCode" />.</summary>
		TimeCode = (uint) AudioFormatType.TimeCode,
		/// <summary>This has the same value as <see cref="AudioFormatType.MIDIStream" />.</summary>
		MidiStream = (uint) AudioFormatType.MIDIStream,
		/// <summary>This has the same value as <see cref="AudioFormatType.ParameterValueStream" />.</summary>
		ParameterValueStream = (uint) AudioFormatType.ParameterValueStream,
		/// <summary>This has the same value as <see cref="AudioFormatType.AppleLossless" />.</summary>
		AppleLossless = (uint) AudioFormatType.AppleLossless,
		/// <summary>This has the same value as <see cref="AudioFormatType.MPEG4AAC_HE" />.</summary>
		Mpeg4Aac_HE = (uint) AudioFormatType.MPEG4AAC_HE,
		/// <summary>This has the same value as <see cref="AudioFormatType.MPEG4AAC_LD" />.</summary>
		Mpeg4Aac_LD = (uint) AudioFormatType.MPEG4AAC_LD,
		/// <summary>This has the same value as <see cref="AudioFormatType.MPEG4AAC_ELD" />.</summary>
		Mpeg4Aac_Eld = (uint) AudioFormatType.MPEG4AAC_ELD,
		/// <summary>This has the same value as <see cref="AudioFormatType.MPEG4AAC_ELD_SBR" />.</summary>
		Mpeg4Aac_Eld_SBR = (uint) AudioFormatType.MPEG4AAC_ELD_SBR,
		/// <summary>This has the same value as <see cref="AudioFormatType.MPEG4AAC_ELD_V2" />.</summary>
		Mpeg4Aac_Eld_V2 = (uint) AudioFormatType.MPEG4AAC_ELD_V2,
		/// <summary>This has the same value as <see cref="AudioFormatType.MPEG4AAC_HE_V2" />.</summary>
		Mpeg4Aac_HE_V2 = (uint) AudioFormatType.MPEG4AAC_HE_V2,
		/// <summary>This has the same value as <see cref="AudioFormatType.MPEG4AAC_Spatial" />.</summary>
		Mpeg4Aac_Spatial = (uint) AudioFormatType.MPEG4AAC_Spatial,
		/// <summary>This has the same value as <see cref="AudioFormatType.MpegD_Usac" />.</summary>
		MpegD_USAC = (uint) AudioFormatType.MpegD_Usac,
		/// <summary>This has the same value as <see cref="AudioFormatType.AMR" />.</summary>
		Amr = (uint) AudioFormatType.AMR,
		/// <summary>This has the same value as <see cref="AudioFormatType.AMRWideBand" />.</summary>
		Amr_WB = (uint) AudioFormatType.AMRWideBand,
		/// <summary>This has the same value as <see cref="AudioFormatType.Audible" />.</summary>
		Audible = (uint) AudioFormatType.Audible,
		/// <summary>This has the same value as <see cref="AudioFormatType.iLBC" />.</summary>
		ILbc = (uint) AudioFormatType.iLBC,
		/// <summary>This has the same value as <see cref="AudioFormatType.DVIIntelIMA" />.</summary>
		DviIntelIma = (uint) AudioFormatType.DVIIntelIMA,
		/// <summary>This has the same value as <see cref="AudioFormatType.MicrosoftGSM" />.</summary>
		MicrosoftGsm = (uint) AudioFormatType.MicrosoftGSM,
		/// <summary>This has the same value as <see cref="AudioFormatType.AES3" />.</summary>
		Aes3 = (uint) AudioFormatType.AES3,
		/// <summary>This has the same value as <see cref="AudioFormatType.EnhancedAC3" />.</summary>
		EnhancedAC3 = (uint) AudioFormatType.EnhancedAC3,
		/// <summary>This has the same value as <see cref="AudioFormatType.Flac" />.</summary>
		Flac = (uint) AudioFormatType.Flac,
		/// <summary>This has the same value as <see cref="AudioFormatType.Opus" />.</summary>
		Opus = (uint) AudioFormatType.Opus,
		/// <summary>This has the same value as <see cref="CMAudioCodecType.AacLCProtected" />.</summary>
		AacLCProtected = (uint) CMAudioCodecType.AacLCProtected,
		/// <summary>This has the same value as <see cref="CMAudioCodecType.AacAudibleProtected" />.</summary>
		AacAudibleProtected = (uint) CMAudioCodecType.AacAudibleProtected,
		/// <summary>This has the same value as <see cref="CMPixelFormat.AlphaRedGreenBlue32bits" />.</summary>
		PixelFormat32ARGB = (uint) CMPixelFormat.AlphaRedGreenBlue32bits,
		/// <summary>This has the same value as <see cref="CMPixelFormat.BlueGreenRedAlpha32bits" />.</summary>
		PixelFormat32BGRA = (uint) CMPixelFormat.BlueGreenRedAlpha32bits,
		/// <summary>This has the same value as <see cref="CMPixelFormat.RedGreenBlue24bits" />.</summary>
		PixelFormat24RGB = (uint) CMPixelFormat.RedGreenBlue24bits,
		/// <summary>This has the same value as <see cref="CMPixelFormat.BigEndian555_16bits" />.</summary>
		PixelFormat16BE555 = (uint) CMPixelFormat.BigEndian555_16bits,
		/// <summary>This has the same value as <see cref="CMPixelFormat.BigEndian565_16bits" />.</summary>
		PixelFormat16BE565 = (uint) CMPixelFormat.BigEndian565_16bits,
		/// <summary>This has the same value as <see cref="CMPixelFormat.LittleEndian555_16bits" />.</summary>
		PixelFormat16LE555 = (uint) CMPixelFormat.LittleEndian555_16bits,
		/// <summary>This has the same value as <see cref="CMPixelFormat.LittleEndian565_16bits" />.</summary>
		PixelFormat16LE565 = (uint) CMPixelFormat.LittleEndian565_16bits,
		/// <summary>This has the same value as <see cref="CMPixelFormat.LittleEndian5551_16bits" />.</summary>
		PixelFormat16LE5551 = (uint) CMPixelFormat.LittleEndian5551_16bits,
		/// <summary>This has the same value as <see cref="CMPixelFormat.YpCbCr422_8bits" />.</summary>
		PixelFormat422YpCbCr8 = (uint) CMPixelFormat.YpCbCr422_8bits,
		/// <summary>This has the same value as <see cref="CMPixelFormat.YpCbCr422yuvs_8bits" />.</summary>
		PixelFormat422YpCbCr8_yuvs = (uint) CMPixelFormat.YpCbCr422yuvs_8bits,
		/// <summary>This has the same value as <see cref="CMPixelFormat.YpCbCr444_8bits" />.</summary>
		PixelFormat444YpCbCr8 = (uint) CMPixelFormat.YpCbCr444_8bits,
		/// <summary>This has the same value as <see cref="CMPixelFormat.YpCbCrA4444_8bits" />.</summary>
		PixelFormat4444YpCbCrA8 = (uint) CMPixelFormat.YpCbCrA4444_8bits,
		/// <summary>This has the same value as <see cref="CMPixelFormat.YpCbCr422_16bits" />.</summary>
		PixelFormat422YpCbCr16 = (uint) CMPixelFormat.YpCbCr422_16bits,
		/// <summary>This has the same value as <see cref="CMPixelFormat.YpCbCr422_10bits" />.</summary>
		PixelFormat422YpCbCr10 = (uint) CMPixelFormat.YpCbCr422_10bits,
		/// <summary>This has the same value as <see cref="CMPixelFormat.YpCbCr444_10bits" />.</summary>
		PixelFormat444YpCbCr10 = (uint) CMPixelFormat.YpCbCr444_10bits,
		/// <summary>This has the same value as <see cref="CMPixelFormat.IndexedGrayWhiteIsZero_8bits" />.</summary>
		PixelFormat8IndexedGray_WhiteIsZero = (uint) CMPixelFormat.IndexedGrayWhiteIsZero_8bits,
		/// <summary>This has the same value as <see cref="CMVideoCodecType.Animation" />.</summary>
		Animation = (uint) CMVideoCodecType.Animation,
		/// <summary>This has the same value as <see cref="CMVideoCodecType.Cinepak" />.</summary>
		Cinepak = (uint) CMVideoCodecType.Cinepak,
		/// <summary>This has the same value as <see cref="CMVideoCodecType.JPEG" />.</summary>
		Jpeg = (uint) CMVideoCodecType.JPEG,
		/// <summary>This has the same value as <see cref="CMVideoCodecType.JPEG_OpenDML" />.</summary>
		Jpeg_OpenDml = (uint) CMVideoCodecType.JPEG_OpenDML,
		/// <summary>This has the same value as <see cref="CMVideoCodecType.SorensonVideo" />.</summary>
		SorensonVideo = (uint) CMVideoCodecType.SorensonVideo,
		/// <summary>This has the same value as <see cref="CMVideoCodecType.SorensonVideo3" />.</summary>
		SorensonVideo3 = (uint) CMVideoCodecType.SorensonVideo3,
		/// <summary>This has the same value as <see cref="CMVideoCodecType.H263" />.</summary>
		H263 = (uint) CMVideoCodecType.H263,
		/// <summary>This has the same value as <see cref="CMVideoCodecType.H264" />.</summary>
		H264 = (uint) CMVideoCodecType.H264,
		/// <summary>This has the same value as <see cref="CMVideoCodecType.Hevc" />.</summary>
		Hevc = (uint) CMVideoCodecType.Hevc,
		/// <summary>This has the same value as <see cref="CMVideoCodecType.HevcWithAlpha" />.</summary>
		HevcWithAlpha = (uint) CMVideoCodecType.HevcWithAlpha,
		/// <summary>This has the same value as <see cref="CMVideoCodecType.Mpeg4Video" />.</summary>
		Mpeg4Video = (uint) CMVideoCodecType.Mpeg4Video,
		/// <summary>This has the same value as <see cref="CMVideoCodecType.Mpeg2Video" />.</summary>
		Mpeg2Video = (uint) CMVideoCodecType.Mpeg2Video,
		/// <summary>This has the same value as <see cref="CMVideoCodecType.Mpeg1Video" />.</summary>
		Mpeg1Video = (uint) CMVideoCodecType.Mpeg1Video,
		/// <summary>This has the same value as <see cref="CMVideoCodecType.DvcNtsc" />.</summary>
		DvcNtsc = (uint) CMVideoCodecType.DvcNtsc,
		/// <summary>This has the same value as <see cref="CMVideoCodecType.DvcPal" />.</summary>
		DvcPal = (uint) CMVideoCodecType.DvcPal,
		/// <summary>This has the same value as <see cref="CMVideoCodecType.DvcProPal" />.</summary>
		DvcProPal = (uint) CMVideoCodecType.DvcProPal,
		/// <summary>This has the same value as <see cref="CMVideoCodecType.DvcPro50NTSC" />.</summary>
		DvcPro50Ntsc = (uint) CMVideoCodecType.DvcPro50NTSC,
		/// <summary>This has the same value as <see cref="CMVideoCodecType.DvcPro50PAL" />.</summary>
		DvcPro50Pal = (uint) CMVideoCodecType.DvcPro50PAL,
		/// <summary>This has the same value as <see cref="CMVideoCodecType.DvcProHD720p60" />.</summary>
		DvcProHD720p60 = (uint) CMVideoCodecType.DvcProHD720p60,
		/// <summary>This has the same value as <see cref="CMVideoCodecType.DvcProHD720p50" />.</summary>
		DvcProHD720p50 = (uint) CMVideoCodecType.DvcProHD720p50,
		/// <summary>This has the same value as <see cref="CMVideoCodecType.DvcProHD1080i60" />.</summary>
		DvcProHD1080i60 = (uint) CMVideoCodecType.DvcProHD1080i60,
		/// <summary>This has the same value as <see cref="CMVideoCodecType.DvcProHD1080i50" />.</summary>
		DvcProHD1080i50 = (uint) CMVideoCodecType.DvcProHD1080i50,
		/// <summary>This has the same value as <see cref="CMVideoCodecType.DvcProHD1080p30" />.</summary>
		DvcProHD1080p30 = (uint) CMVideoCodecType.DvcProHD1080p30,
		/// <summary>This has the same value as <see cref="CMVideoCodecType.DvcProHD1080p25" />.</summary>
		DvcProHD1080p25 = (uint) CMVideoCodecType.DvcProHD1080p25,
		/// <summary>This has the same value as <see cref="CMVideoCodecType.AppleProRes4444XQ" />.</summary>
		ProRes4444XQ = (uint) CMVideoCodecType.AppleProRes4444XQ,
		/// <summary>This has the same value as <see cref="CMVideoCodecType.AppleProRes4444" />.</summary>
		ProRes4444 = (uint) CMVideoCodecType.AppleProRes4444,
		/// <summary>This has the same value as <see cref="CMVideoCodecType.AppleProRes422HQ" />.</summary>
		ProRes422HQ = (uint) CMVideoCodecType.AppleProRes422HQ,
		/// <summary>This has the same value as <see cref="CMVideoCodecType.AppleProRes422" />.</summary>
		ProRes422 = (uint) CMVideoCodecType.AppleProRes422,
		/// <summary>This has the same value as <see cref="CMVideoCodecType.AppleProRes422LT" />.</summary>
		ProRes422LT = (uint) CMVideoCodecType.AppleProRes422LT,
		/// <summary>This has the same value as <see cref="CMVideoCodecType.AppleProRes422Proxy" />.</summary>
		ProRes422Proxy = (uint) CMVideoCodecType.AppleProRes422Proxy,
		/// <summary>This has the same value as <see cref="CMVideoCodecType.AppleProResRaw" />.</summary>
		ProResRAW = (uint) CMVideoCodecType.AppleProResRaw,
		/// <summary>This has the same value as <see cref="CMVideoCodecType.AppleProResRawHQ" />.</summary>
		ProResRAWHQ = (uint) CMVideoCodecType.AppleProResRawHQ,
		/// <summary>This has the same value as <see cref="CMMuxedStreamType.MPEG1System" />.</summary>
		Mpeg1System = (uint) CMMuxedStreamType.MPEG1System,
		/// <summary>This has the same value as <see cref="CMMuxedStreamType.MPEG2Transport" />.</summary>
		Mpeg2Transport = (uint) CMMuxedStreamType.MPEG2Transport,
		/// <summary>This has the same value as <see cref="CMMuxedStreamType.MPEG2Program" />.</summary>
		Mpeg2Program = (uint) CMMuxedStreamType.MPEG2Program,
		/// <summary>This has the same value as <see cref="CMMuxedStreamType.DV" />.</summary>
		Dv = (uint) CMMuxedStreamType.DV,
		/// <summary>This has the same value as <see cref="CMMuxedStreamType.EmbeddedDeviceScreenRecording" />.</summary>
		[iOS (17, 0), Mac (14, 0), TV (17, 0), MacCatalyst (17, 0)]
		EmbeddedDeviceScreenRecording = (uint) CMMuxedStreamType.EmbeddedDeviceScreenRecording,
		/// <summary>This has the same value as <see cref="CMClosedCaptionFormatType.CEA608" />.</summary>
		Cea608 = (uint) CMClosedCaptionFormatType.CEA608,
		/// <summary>This has the same value as <see cref="CMClosedCaptionFormatType.CEA708" />.</summary>
		Cea708 = (uint) CMClosedCaptionFormatType.CEA708,
		/// <summary>This has the same value as <see cref="CMClosedCaptionFormatType.ATSC" />.</summary>
		Atsc = (uint) CMClosedCaptionFormatType.ATSC,
		/// <summary>This has the same value as <see cref="CMTextFormatType.QTText" />.</summary>
		Qt = (uint) CMTextFormatType.QTText,
		/// <summary>This has the same value as <see cref="CMSubtitleFormatType.Text3G" />.</summary>
		Mobile3Gpp = (uint) CMSubtitleFormatType.Text3G,
		/// <summary>This has the same value as <see cref="CMSubtitleFormatType.WebVTT" />.</summary>
		WebVtt = (uint) CMSubtitleFormatType.WebVTT,
		/// <summary>This has the same value as <see cref="CMTimeCodeFormatType.TimeCode32" />.</summary>
		TimeCode32 = (uint) CMTimeCodeFormatType.TimeCode32,
		/// <summary>This has the same value as <see cref="CMTimeCodeFormatType.TimeCode64" />.</summary>
		TimeCode64 = (uint) CMTimeCodeFormatType.TimeCode64,
		/// <summary>This has the same value as <see cref="CMTimeCodeFormatType.Counter32" />.</summary>
		Counter32 = (uint) CMTimeCodeFormatType.Counter32,
		/// <summary>This has the same value as <see cref="CMTimeCodeFormatType.Counter64" />.</summary>
		Counter64 = (uint) CMTimeCodeFormatType.Counter64,
		/// <summary>This has the same value as <see cref="CMMetadataFormatType.ICY" />.</summary>
		Icy = (uint) CMMetadataFormatType.ICY,
		/// <summary>This has the same value as <see cref="CMMetadataFormatType.ID3" />.</summary>
		Id3 = (uint) CMMetadataFormatType.ID3,
		/// <summary>This has the same value as <see cref="CMMetadataFormatType.Boxed" />.</summary>
		Boxed = (uint) CMMetadataFormatType.Boxed,
		/// <summary>This has the same value as <see cref="CMMetadataFormatType.Emsg" />.</summary>
		Emsg = (uint) CMMetadataFormatType.Emsg,
		/// <summary>This has the same value as <see cref="CMMediaType.TaggedBufferGroup" />.</summary>
		Tbgr = (uint) CMMediaType.TaggedBufferGroup,
	}
}
