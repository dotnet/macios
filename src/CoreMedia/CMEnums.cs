// 
// CMEnums.cs: Enumerations for CoreMedia
//
// Authors: Mono Team
//          Marek Safar (marek.safar@gmail.com)
//
// Copyright 2010-2011 Novell Inc
// Copyright 2012-2014 Xamarin Inc
//
using System;
using System.Runtime.InteropServices;
using Foundation;
using ObjCRuntime;

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
	}

	// FourCharCode -> CMFormatDescription.h
	/// <summary>An enumeration whose values specify a subtitling standard.</summary>
	[MacCatalyst (13, 1)]
	public enum CMSubtitleFormatType : uint {
		/// <summary>To be added.</summary>
		Text3G = 0x74783367, // 'tx3g'
		/// <summary>To be added.</summary>
		WebVTT = 0x77767474, // 'wvtt'
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
	}

	// uint32_t -> CMTime.h
	/// <summary>An enumeration whose values specify the rounding method to be used with a <see cref="T:CoreMedia.CMTime" />.</summary>
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
		/// <summary>To be added.</summary>
		Hevc = 0x68766331,
		[iOS (14, 5)]
		[TV (14, 5)]
		[MacCatalyst (14, 5)]
		DolbyVisionHevc = 0x64766831,
		DisparityHevc = 0x64697368,
		DepthHevc = 0x64657068,
	}

	// UInt32 enum => CMFormatDescription.h
	/// <summary>Enumerates pixel formats.</summary>
	[MacCatalyst (13, 1)]
	public enum CMPixelFormat : uint {
		/// <summary>To be added.</summary>
		AlphaRedGreenBlue32bits = 32,
		/// <summary>To be added.</summary>
		BlueGreenRedAlpha32bits = 1111970369,
		/// <summary>To be added.</summary>
		RedGreenBlue24bits = 24,
		/// <summary>To be added.</summary>
		BigEndian555_16bits = 16,
		/// <summary>To be added.</summary>
		BigEndian565_16bits = 1110783541,
		/// <summary>To be added.</summary>
		LittleEndian555_16bits = 1278555445,
		/// <summary>To be added.</summary>
		LittleEndian565_16bits = 1278555701,
		/// <summary>To be added.</summary>
		LittleEndian5551_16bits = 892679473,
		/// <summary>To be added.</summary>
		YpCbCr422_8bits = 846624121,
		/// <summary>To be added.</summary>
		YpCbCr422yuvs_8bits = 2037741171,
		/// <summary>To be added.</summary>
		YpCbCr444_8bits = 1983066168,
		/// <summary>To be added.</summary>
		YpCbCrA4444_8bits = 1983131704,
		/// <summary>To be added.</summary>
		YpCbCr422_16bits = 1983000886,
		/// <summary>To be added.</summary>
		YpCbCr422_10bits = 1983000880,
		/// <summary>To be added.</summary>
		YpCbCr444_10bits = 1983131952,
		/// <summary>To be added.</summary>
		IndexedGrayWhiteIsZero_8bits = 40,
	}

	/// <summary>Enumerates attachment propagation policies.</summary>
	///     <remarks>This setting controls whether <see cref="M:CoreMedia.CMAttachmentBearer.PropagateAttachments(CoreMedia.ICMAttachmentBearer,CoreMedia.ICMAttachmentBearer)" /> will propagate a given attachment.</remarks>
	[MacCatalyst (13, 1)]
	public enum CMAttachmentMode : uint {
		/// <summary>Indicates that the attachment does not propagate.</summary>
		ShouldNotPropagate = 0,
		/// <summary>Indicates that the attachment will propagate.</summary>
		ShouldPropagate = 1,
	};

	// untyped enum (used as OSStatus) -> CMBlockBuffer.h
	/// <summary>An enumeration whose values specify errors relating to <see cref="T:CoreMedia.CMBlockBuffer" />s.</summary>
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
	/// <summary>An enumeration of flags to be used with <see cref="M:CoreMedia.CMBlockBuffer.CreateEmpty(System.UInt32,CoreMedia.CMBlockBufferFlags,CoreMedia.CMBlockBufferError@)" />.</summary>
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
	/// <summary>An enumeration whose values specify errors relating to <see cref="T:CoreMedia.CMFormatDescription" />s.</summary>
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
	/// <summary>An enumeration whose values specify errors relating to <see cref="T:CoreMedia.CMSampleBuffer" />s.</summary>
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
	/// <summary>An enumeration whose values specify errors relating to <see cref="T:CoreMedia.CMClock" />s.</summary>
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
	/// <summary>An enumeration whose values specify errors relating to <see cref="T:CoreMedia.CMTimebase" />s.</summary>
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
	/// <summary>An enumeration whose values specify errors returned by <see cref="M:CoreMedia.CMClockOrTimebase.GetRelativeRateAndAnchorTime(CoreMedia.CMClockOrTimebase,CoreMedia.CMClockOrTimebase,System.Double@,CoreMedia.CMTime@,CoreMedia.CMTime@)" />.</summary>
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
}
