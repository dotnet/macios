// Copyright 2009, Novell, Inc.
// Copyright 2010, Novell, Inc.
// Copyright 2011, 2012, 2014 Xamarin Inc.
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
//
using System;
using System.ComponentModel;
using Foundation;
using ObjCRuntime;
using System.Runtime.InteropServices;

namespace AVFoundation {

	[Native]
	// NSInteger - AVAudioSettings.h
	public enum AVAudioQuality : long {
		/// <summary>Minimum quality.</summary>
		Min = 0,
		/// <summary>Low quality encoding.</summary>
		Low = 0x20,
		/// <summary>Medium quality.</summary>
		Medium = 0x40,
		/// <summary>High quality encoding.</summary>
		High = 0x60,
		/// <summary>Maximum quality.</summary>
		Max = 0x7F,
	}

	[MacCatalyst (13, 1)]
	[Native]
	// NSInteger - AVAssetExportSession.h
	public enum AVAssetExportSessionStatus : long {
		Unknown,
		Waiting,
		Exporting,
		Completed,
		Failed,
		Cancelled,
	}

	[MacCatalyst (13, 1)]
	[Native]
	// NSInteger - AVAssetReader.h
	public enum AVAssetReaderStatus : long {
		Unknown = 0,
		Reading,
		Completed,
		Failed,
		Cancelled,
	}

	[MacCatalyst (13, 1)]
	[Native]
	// NSInteger - AVAssetWriter.h
	public enum AVAssetWriterStatus : long {
		Unknown = 0,
		Writing,
		Completed,
		Failed,
		Cancelled,
	}

	[Introduced (PlatformName.MacCatalyst, 14, 0)]
	[NoTV]
	[Native]
	// NSInteger - AVCaptureSession.h
	public enum AVCaptureVideoOrientation : long {
		/// <summary>Portrait</summary>
		Portrait = 1,
		/// <summary>Portrait, upside down.</summary>
		PortraitUpsideDown,
		/// <summary>Landscape, turned right.</summary>
		LandscapeRight,
		/// <summary>Landscape, turned left.</summary>
		LandscapeLeft,
	}

	[Introduced (PlatformName.MacCatalyst, 14, 0)]
	[TV (17, 0)]
	[Native]
	// NSInteger - AVCaptureDevice.h
	public enum AVCaptureFlashMode : long {
		Off,
		On,
		Auto,
	}

	[Introduced (PlatformName.MacCatalyst, 14, 0)]
	[TV (17, 0)]
	[Native]
	// NSInteger - AVCaptureDevice.h
	public enum AVCaptureTorchMode : long {
		/// <summary>Never uses the torch.</summary>
		Off,
		/// <summary>Always uses the torch.</summary>
		On,
		/// <summary>Uses the torch based on the available light measured.</summary>
		Auto,
	}

	[Introduced (PlatformName.MacCatalyst, 14, 0)]
	[TV (17, 0)]
	[Native]
	// NSInteger - AVCaptureDevice.h
	public enum AVCaptureFocusMode : long {
		Locked,
		AutoFocus,
		ContinuousAutoFocus,
	}

	[Introduced (PlatformName.MacCatalyst, 14, 0)]
	[TV (17, 0)]
	[Native]
	// NSInteger - AVCaptureDevice.h
	public enum AVCaptureDevicePosition : long {
		Unspecified = 0,
		Back = 1,
		Front = 2,
	}

	[Introduced (PlatformName.MacCatalyst, 14, 0)]
	[TV (17, 0)]
	[Native]
	// NSInteger - AVCaptureDevice.h
	public enum AVCaptureExposureMode : long {
		Locked,
		AutoExpose,
		ContinuousAutoExposure,
		[MacCatalyst (14, 0)]
		Custom,
	}

	[Introduced (PlatformName.MacCatalyst, 14, 0)]
	[TV (17, 0)]
	[Native]
	// NSInteger - AVCaptureDevice.h
	public enum AVCaptureWhiteBalanceMode : long {
		/// <summary>Auto white balance has been locked.</summary>
		Locked,
		/// <summary>Automatic white balance, set it once.</summary>
		AutoWhiteBalance,
		/// <summary>Contimuously evaluate and set the white balance.</summary>
		ContinuousAutoWhiteBalance,
	}

	// Populated in NSError.Code, an NSInteger
	// anonymous enum - AVError.h
	[MacCatalyst (13, 1)]
	[Native]
	public enum AVError : long {
		/// <summary>Indicates that an unknown error occurred.</summary>
		Unknown = -11800,
		/// <summary>Indicates that system memory became full.</summary>
		OutOfMemory = -11801,
		/// <summary>Indicates that no data was received because the session was not running.</summary>
		SessionNotRunning = -11803,
		/// <summary>Indicates that the device was in use by another session.</summary>
		DeviceAlreadyUsedByAnotherSession = -11804,
		/// <summary>Indicates that no data was sent to the recording session.</summary>
		NoDataCaptured = -11805,
		/// <summary>Indicates that the recording session configuration was changed.</summary>
		SessionConfigurationChanged = -11806,
		/// <summary>Indicates that non-volatile storage was full.</summary>
		DiskFull = -11807,
		/// <summary>Indicates that the device was not connected.</summary>
		DeviceWasDisconnected = -11808,
		/// <summary>Indicates that the recording source media changed.</summary>
		MediaChanged = -11809,
		/// <summary>Indicates that the maximum allowable recording duration was reached.</summary>
		MaximumDurationReached = -11810,
		/// <summary>Indicates that the maximum allowable output file size was reached.</summary>
		MaximumFileSizeReached = -11811,
		/// <summary>Indicates that the source data was interrupted.</summary>
		MediaDiscontinuity = -11812,
		/// <summary>Indicates that the maximum number of samples was reached for the output file.</summary>
		MaximumNumberOfSamplesForFileFormatReached = -11813,
		/// <summary>Indicates that the device was not connected.</summary>
		DeviceNotConnected = -11814,
		/// <summary>Indicates that the device was in use by another application.</summary>
		DeviceInUseByAnotherApplication = -11815,
		/// <summary>Indicates that the device is locked for an update or for other configuration.</summary>
		DeviceLockedForConfigurationByAnotherProcess = -11817,
		/// <summary>Indicates that the recording sesion was interrupted.</summary>
		SessionWasInterrupted = -11818,
		/// <summary>Indicates that media services were reset.</summary>
		MediaServicesWereReset = -11819,
		/// <summary>Indicates that data export failed.</summary>
		ExportFailed = -11820,
		/// <summary>Indicates that the media data could not be decoded.</summary>
		DecodeFailed = -11821,
		/// <summary>Indicates that the source media was not valid.</summary>
		InvalidSourceMedia = -11822,
		/// <summary>Indicates that data could not be written because the output file already exists.</summary>
		FileAlreadyExists = -11823,
		/// <summary>Indicates that the source media contained gaps and could not be loaded.</summary>
		CompositionTrackSegmentsNotContiguous = -11824,
		/// <summary>Indicates that the source media's play duration on the device was not valid.</summary>
		InvalidCompositionTrackSegmentDuration = -11825,
		/// <summary>Indicates that the source media could not be opened because a track start time was not valid.</summary>
		InvalidCompositionTrackSegmentSourceStartTime = -11826,
		/// <summary>Indicates that the source media had no duration set.</summary>
		InvalidCompositionTrackSegmentSourceDuration = -11827,
		/// <summary>Indicates that the media was in an urecognizable format and could therefore not be opened.</summary>
		FormatNotRecognized = -11828,
		/// <summary>Indicates that the data could not be parsed.</summary>
		FailedToParse = -11829, // Should have been FileFailedToParse
		/// <summary>Indicates that the maximum number of still images was reached for the output file.</summary>
		MaximumStillImageCaptureRequestsExceeded = -11830,
		/// <summary>Indicates that the app is not authorized to play the content.</summary>
		ContentIsProtected = -11831,
		/// <summary>Indicates that playback failed because no image was present at a specified time.</summary>
		NoImageAtTime = -11832,
		/// <summary>Indicates that a decoder for the media data could not be found.</summary>
		DecoderNotFound = -11833,
		/// <summary>Indicates that a required data encoder was missing.</summary>
		EncoderNotFound = -11834,
		/// <summary>Indicates that the user is not authorized to play the content.</summary>
		ContentIsNotAuthorized = -11835,
		/// <summary>The application is not authorized.</summary>
		ApplicationIsNotAuthorized = -11836,
		/// <summary>Indicates that the device is not available while the app is backgrounded.</summary>
		DeviceIsNotAvailableInBackground = -11837,
		/// <summary>Indicates that an operation that was not supported on a specific asset or asset type was attempted.</summary>
		OperationNotSupportedForAsset = -11838,
		/// <summary>Indicates that the decoder for the media data was unavailable.</summary>
		DecoderTemporarilyUnavailable = -11839,
		/// <summary>Indicates that a required data encoder was unavailable.</summary>
		EncoderTemporarilyUnavailable = -11840,
		/// <summary>Indicates that a video composition operation was not valid.</summary>
		InvalidVideoComposition = -11841,
		/// <summary>Indicates that an attempt was made to follow a reference for which authorization was lacking.</summary>
		ReferenceForbiddenByReferencePolicy = -11842,
		/// <summary>Indicates that the ouptut path contained an invalid extension.</summary>
		InvalidOutputURLPathExtension = -11843,
		/// <summary>Indicates that the screen could not be captured.</summary>
		ScreenCaptureFailed = -11844,
		/// <summary>Indicates that the display was disabled.</summary>
		DisplayWasDisabled = -11845,
		/// <summary>Indicates that a torch level was unavailable, possibly due to overheating.</summary>
		TorchLevelUnavailable = -11846,
		/// <summary>Indicates that an operation was interrupted.</summary>
		OperationInterrupted = -11847,
		/// <summary>Indicates that the media format is not supported by the device.</summary>
		IncompatibleAsset = -11848,
		/// <summary>Indicates taht the media data could not be loaded due to missing required data.</summary>
		FailedToLoadMediaData = -11849,
		/// <summary>Indicates that the media server is not correctly configured.</summary>
		ServerIncorrectlyConfigured = -11850,
		/// <summary>Indicates that the application was not authorized on the device.</summary>
		ApplicationIsNotAuthorizedToUseDevice = -11852,

		/// <summary>Indicates that the data could not be parsed.</summary>
		FailedToParse2 = -11853,
		/// <summary>Indicates that the file type does not support sample references.</summary>
		FileTypeDoesNotSupportSampleReferences = -11854,
		/// <summary>Indicates that the media data was corrupted.</summary>
		UndecodableMediaData = -11855,

		/// <summary>Indicates that playback failed because the controller lacked necessary internet access.</summary>
		AirPlayControllerRequiresInternet = -11856,
		/// <summary>Indicates that playback failed because the receiver lacked necessary internet access.</summary>
		AirPlayReceiverRequiresInternet = -11857,

		/// <summary>Indicates that a video compositor failed.</summary>
		VideoCompositorFailed = -11858,

		/// <summary>Indicates that an attempt was made to start a recording while another was in progress.</summary>
		RecordingAlreadyInProgress = -11859,
		CreateContentKeyRequestFailed = -11860,
		/// <summary>Indicates that the output settings were not supported.</summary>
		UnsupportedOutputSettings = -11861,
		/// <summary>Indicates that a disallowed operation was attempted.</summary>
		OperationNotAllowed = -11862,
		/// <summary>To be added.</summary>
		ContentIsUnavailable = -11863,
		/// <summary>To be added.</summary>
		FormatUnsupported = -11864,
		/// <summary>To be added.</summary>
		MalformedDepth = -11865,
		/// <summary>To be added.</summary>
		ContentNotUpdated = -11866,
		/// <summary>To be added.</summary>
		NoLongerPlayable = -11867,
		/// <summary>To be added.</summary>
		NoCompatibleAlternatesForExternalDisplay = -11868,
		/// <summary>To be added.</summary>
		NoSourceTrack = -11869,
		ExternalPlaybackNotSupportedForAsset = -11870,
		OperationNotSupportedForPreset = -11871,
		SessionHardwareCostOverage = -11872,
		UnsupportedDeviceActiveFormat = -11873,
		IncorrectlyConfigured = -11875,
		SegmentStartedWithNonSyncSample = -11876,
		RosettaNotInstalled = -11877,
		OperationCancelled = -11878,
		RequestCancelled = -11879,

		InvalidSampleCursor = -11880,
		FailedToLoadSampleData = -11881,
		AirPlayReceiverTemporarilyUnavailable = -11882,
		EncodeFailed = -11883,
		SandboxExtensionDenied = -11884,
		ToneMappingFailed = -11885,
		MediaExtensionDisabled = -11886,
		MediaExtensionConflict = -11887,
	}

	[MacCatalyst (13, 1)]
	[Native]
	// NSInteger - AVPlayer.h
	public enum AVPlayerActionAtItemEnd : long {
		/// <summary>Indicates the player should advance to the next item, if it exists.</summary>
		Advance,
		/// <summary>The player should pause.</summary>
		Pause,
		/// <summary>The player should do nothing.</summary>
		None,
	}

	[MacCatalyst (13, 1)]
	[Native]
	// NSInteger - AVPlayerItem.h
	public enum AVPlayerItemStatus : long {
		Unknown,
		ReadyToPlay,
		Failed,
	}

	[MacCatalyst (13, 1)]
	[Native]
	// NSInteger - AVAsynchronousKeyValueLoading.h
	public enum AVKeyValueStatus : long {
		/// <summary>The load attempt's state is unknown.</summary>
		Unknown,
		/// <summary>The load attempt is ongoing.</summary>
		Loading,
		/// <summary>The load attempt succeeded.</summary>
		Loaded,
		/// <summary>The load attempt failed.</summary>
		Failed,
		/// <summary>The load attempt was cancelled.</summary>
		Cancelled,
	}

	[MacCatalyst (13, 1)]
	[Native]
	// NSInteger - AVPlayer.h
	public enum AVPlayerStatus : long {
		/// <summary>The status of the player is unknown, generally because media has not been loaded.</summary>
		Unknown,
		/// <summary>The player is ready to play the <see cref="T:AVFoundation.AVPlayerItem" />.</summary>
		ReadyToPlay,
		/// <summary>The player can no longer play the <see cref="T:AVFoundation.AVPlayerItem" />.</summary>
		Failed,
	}

	[MacCatalyst (13, 1)]
	[Flags]
	[Native]
	// NSUInteger - AVAsset.h
	public enum AVAssetReferenceRestrictions : ulong {
		/// <summary>There are no restrictions on references to media data.</summary>
		ForbidNone = 0,
		/// <summary>Remote assets cannot require a local media resource (e.g., an HTTP URL cannot require a client-side file).</summary>
		ForbidRemoteReferenceToLocal = (1 << 0),
		/// <summary>Local asset references to remote media are not allowed.</summary>
		ForbidLocalReferenceToRemote = (1 << 1),
		/// <summary>Remote asset references to a different site are not allowed.</summary>
		ForbidCrossSiteReference = (1 << 2),
		/// <summary>Local asset references to local media outside the container are not allowed.</summary>
		ForbidLocalReferenceToLocal = (1 << 3),
		/// <summary>Only references to media data stored within the asset's container are allowed.</summary>
		ForbidAll = 0xFFFF,
	}

	[MacCatalyst (13, 1)]
	[Native]
	// NSInteger - AVAssetImageGenerator.h
	public enum AVAssetImageGeneratorResult : long {
		Succeeded,
		Failed,
		Cancelled,
	}

	/// <summary>An enumeration whose values specify whether a <see cref="T:AVFoundation.AVCaptureDevice" /> is playing or not.</summary>
	[NoiOS]
	[Unavailable (PlatformName.MacCatalyst)]
	[NoTV]
	[Native]
	// NSInteger - AVCaptureDevice.h
	public enum AVCaptureDeviceTransportControlsPlaybackMode : long {
		NotPlaying,
		Playing,
	}

	/// <summary>An enumeration whose values specify how interlaced fields should be dealt with.</summary>
	///     
	///     <!-- TODO: This should be used with AVCaptureConnection.VideoFieldMode property, but that doesn't appear to exist  -->
	[NoiOS]
	[Unavailable (PlatformName.MacCatalyst)]
	[NoTV]
	[Native]
	// NSInteger - AVCaptureSession.h
	public enum AVVideoFieldMode : long {
		Both,
		TopOnly,
		BottomOnly,
		Deinterlace,
	}

	[MacCatalyst (13, 1)]
	[Flags]
	[Native]
	// NSUInteger - AVAudioSession.h
	public enum AVAudioSessionInterruptionOptions : ulong {
		ShouldResume = 1,
	}

	[MacCatalyst (13, 1)]
	[Flags]
	[Native]
	// NSUInteger - AVAudioSession.h
	public enum AVAudioSessionSetActiveOptions : ulong {
		NotifyOthersOnDeactivation = 1,
	}

	[MacCatalyst (13, 1)]
	[Native]
	// NSUInteger - AVAudioSession.h
	public enum AVAudioSessionPortOverride : ulong {
		None = 0,
		[NoTV]
		[NoMac] // Removed in Xcode 12 GM
		[MacCatalyst (13, 1)]
		Speaker = 0x73706b72, // 'spkr'
	}

	[MacCatalyst (13, 1)]
	[Native]
	// NSUInteger - AVAudioSession.h
	public enum AVAudioSessionRouteChangeReason : ulong {
		Unknown,
		NewDeviceAvailable,
		OldDeviceUnavailable,
		CategoryChange,
		Override,
		WakeFromSleep = 6,
		NoSuitableRouteForCategory = 7,
		RouteConfigurationChange = 8,
	}

	[Flags]
	[Native]
	// NSUInteger - AVAudioSession.h
	public enum AVAudioSessionCategoryOptions : ulong {
		MixWithOthers = 1,
		DuckOthers = 2,
		[NoMac] // Removed in Xcode 12 GM
		[NoTV]
		[MacCatalyst (13, 1)]
		AllowBluetooth = 4,
		[NoMac] // Removed in Xcode 12 GM
		[NoTV]
		[MacCatalyst (13, 1)]
		DefaultToSpeaker = 8,

		[NoMac] // Removed in Xcode 12 GM
		[MacCatalyst (13, 1)]
		InterruptSpokenAudioAndMixWithOthers = 17,
		[NoMac] // Removed in Xcode 12 GM
		[MacCatalyst (13, 1)]
		AllowBluetoothA2DP = 32,
		[NoMac] // Removed in Xcode 12 GM
		[MacCatalyst (13, 1)]
		AllowAirPlay = 64,
		[NoMac]
		[NoTV]
		[iOS (14, 5)]
		[MacCatalyst (14, 5)]
		OverrideMutedMicrophoneInterruption = 128,
	}

	[MacCatalyst (13, 1)]
	[Native]
	// NSUInteger - AVAudioSession.h
	public enum AVAudioSessionInterruptionType : ulong {
		Ended,
		Began,
	}

	[MacCatalyst (13, 1)]
	[Native]
	// NSInteger - AVAudioSession.h
	// typedef CF_ENUM(NSInteger, AVAudioSessionErrorCode) -> CoreAudioTypes.framework/Headers/AudioSessionTypes.h
	public enum AVAudioSessionErrorCode : long {
		None = 0,
		MediaServicesFailed = 0x6D737276, // 'msrv'
		IsBusy = 0x21616374, // '!act'
		IncompatibleCategory = 0x21636174, // 'cat'
		CannotInterruptOthers = 0x21696e74, // 'int'
		MissingEntitlement = 0x656e743f, // 'ent?'
		SiriIsRecording = 0x73697269, // 'siri'
		CannotStartPlaying = 0x21706c61, // '!pla'
		CannotStartRecording = 0x21726563, // '!rec'
		BadParam = -50,
		InsufficientPriority = 0x21707269, // '!pri'
		ResourceNotAvailable = 0x21726573, // '!res'
		Unspecified = 0x77686174, // 'what'
		ExpiredSession = 0x21736573, // '!ses'
		SessionNotActive = 0x696e6163, // 'inac'
	}

	[Introduced (PlatformName.MacCatalyst, 14, 0)]
	[TV (17, 0)]
	[Native]
	// NSInteger - AVCaptureDevice.h
	public enum AVCaptureAutoFocusRangeRestriction : long {
		None = 0,
		Near = 1,
		Far = 2,
	}

	// Convenience enum for native strings (defined in AVAudioSettings.h)
	public enum AVAudioBitRateStrategy : int {
		Constant,
		LongTermAverage,
		VariableConstrained,
		Variable,
	}

	// Convenience enum for native strings (defined in AVAudioSettings.h)
	public enum AVSampleRateConverterAlgorithm : int {
		/// <summary>To be added.</summary>
		Normal,
		/// <summary>To be added.</summary>
		Mastering,
	}

	[Introduced (PlatformName.MacCatalyst, 14, 0)]
	[TV (17, 0)]
	[Native]
	// NSInteger - AVCaptureDevice.h
	public enum AVAuthorizationStatus : long {
		/// <summary>The user has not interacted with the permissions dialog.</summary>
		NotDetermined,
		/// <summary>The user is denied access and may not change permission.</summary>
		Restricted,
		/// <summary>The user has denied access.</summary>
		Denied,
		/// <summary>The user has granted access.</summary>
		Authorized,
	}

	[MacCatalyst (13, 1)]
	[Native]
	// NSInteger - AVSpeechSynthesis.h
	public enum AVSpeechBoundary : long {
		Immediate,
		Word,
	}

	[MacCatalyst (13, 1)]
	[Native]
	public enum AVAudioCommonFormat : ulong {
		Other = 0,
		PCMFloat32 = 1,
		PCMFloat64 = 2,
		PCMInt16 = 3,
		PCMInt32 = 4,
	}

	[Native]
	public enum AVAudio3DMixingRenderingAlgorithm : long {
		EqualPowerPanning = 0,
		SphericalHead = 1,
		HRTF = 2,
		SoundField = 3,
		StereoPassThrough = 5,
		[MacCatalyst (13, 1)]
		HrtfHQ = 6,
		[iOS (13, 0)]
		[TV (13, 0)]
		[MacCatalyst (13, 1)]
		Auto = 7,
	}

#if XAMCORE_5_0
	[NoTV, NoMac]
#endif
	[Deprecated (PlatformName.iOS, 17, 0, message: "Use 'AVAudioApplicationRecordPermission' instead.")]
	[Deprecated (PlatformName.TvOS, 17, 0, message: "Use 'AVAudioApplicationRecordPermission' instead.")]
	[Deprecated (PlatformName.MacCatalyst, 17, 0, message: "Use 'AVAudioApplicationRecordPermission' instead.")]
	[Deprecated (PlatformName.MacOSX, 14, 0, message: "Use 'AVAudioApplicationRecordPermission' instead.")]
	[MacCatalyst (13, 1)]
	[Native]
	public enum AVAudioSessionRecordPermission : ulong {
		Undetermined = 1970168948 /*'undt'*/,
		Denied = 1684369017 /*'deny'*/,
		Granted = 1735552628 /*'grnt'*/,
	}

	[MacCatalyst (13, 1)]
	[Native]
	public enum AVAudioSessionSilenceSecondaryAudioHintType : ulong {
		Begin = 1,
		End = 0,
	}

	[Flags]
	[Native]
	public enum AVAudioPlayerNodeBufferOptions : ulong {
		/// <summary>The buffer should loop indefinitely.</summary>
		Loops = 0x01,
		/// <summary>The buffer should interrupt any other buffer that is already playing.</summary>
		Interrupts = 0x02,
		/// <summary>The buffer should interrupt any other buffer at its loop point.</summary>
		InterruptsAtLoop = 0x04,
	}

	[Native]
	public enum AVAudioUnitEQFilterType : long {
		/// <summary>Parametric filter based on Butterworth analog prototype. Must have <c>frequency</c> for center, <c>bandwidth</c>, and <c>gain</c></summary>
		Parametric = 0,
		/// <summary>To be added.</summary>
		LowPass = 1,
		/// <summary>To be added.</summary>
		HighPass = 2,
		/// <summary>Low pass filter with resonance. Must have <c>frequency</c> (-3dB cutoff) and <c>bandwidth</c>.</summary>
		ResonantLowPass = 3,
		/// <summary>High pass filter with resonance. Must have <c>frequency</c> (-3dB cutoff) and <c>bandwidth</c>.</summary>
		ResonantHighPass = 4,
		/// <summary>Band pass filter. Must specify <c>frequency</c> and <c>bandwidth</c>.</summary>
		BandPass = 5,
		/// <summary>Band stop / notch filter. Must specify <c>frequency</c> and <c>bandwidth</c>.</summary>
		BandStop = 6,
		/// <summary>To be added.</summary>
		LowShelf = 7,
		/// <summary>To be added.</summary>
		HighShelf = 8,
		/// <summary>Low shelf filter with resonance. Must have <c>frequency</c>, <c>bandwidth</c>, and <c>gain</c>.</summary>
		ResonantLowShelf = 9,
		/// <summary>High shelf filter with resonance. Must have <c>frequency</c>, <c>bandwidth</c>, and <c>gain</c>.</summary>
		ResonantHighShelf = 10,
	}

	[Native]
	public enum AVAudioUnitReverbPreset : long {
		/// <summary>To be added.</summary>
		SmallRoom = 0,
		/// <summary>To be added.</summary>
		MediumRoom = 1,
		/// <summary>To be added.</summary>
		LargeRoom = 2,
		/// <summary>To be added.</summary>
		MediumHall = 3,
		/// <summary>To be added.</summary>
		LargeHall = 4,
		/// <summary>To be added.</summary>
		Plate = 5,
		/// <summary>To be added.</summary>
		MediumChamber = 6,
		/// <summary>To be added.</summary>
		LargeChamber = 7,
		/// <summary>To be added.</summary>
		Cathedral = 8,
		/// <summary>To be added.</summary>
		LargeRoom2 = 9,
		/// <summary>To be added.</summary>
		MediumHall2 = 10,
		/// <summary>To be added.</summary>
		MediumHall3 = 11,
		/// <summary>To be added.</summary>
		LargeHall2 = 12,
	}

	[Native]
	public enum AVAudioUnitDistortionPreset : long {
		/// <summary>To be added.</summary>
		DrumsBitBrush = 0,
		/// <summary>To be added.</summary>
		DrumsBufferBeats = 1,
		/// <summary>To be added.</summary>
		DrumsLoFi = 2,
		/// <summary>To be added.</summary>
		MultiBrokenSpeaker = 3,
		/// <summary>To be added.</summary>
		MultiCellphoneConcert = 4,
		/// <summary>To be added.</summary>
		MultiDecimated1 = 5,
		/// <summary>To be added.</summary>
		MultiDecimated2 = 6,
		/// <summary>To be added.</summary>
		MultiDecimated3 = 7,
		/// <summary>To be added.</summary>
		MultiDecimated4 = 8,
		/// <summary>To be added.</summary>
		MultiDistortedFunk = 9,
		/// <summary>To be added.</summary>
		MultiDistortedCubed = 10,
		/// <summary>To be added.</summary>
		MultiDistortedSquared = 11,
		/// <summary>To be added.</summary>
		MultiEcho1 = 12,
		/// <summary>To be added.</summary>
		MultiEcho2 = 13,
		/// <summary>To be added.</summary>
		MultiEchoTight1 = 14,
		/// <summary>To be added.</summary>
		MultiEchoTight2 = 15,
		/// <summary>To be added.</summary>
		MultiEverythingIsBroken = 16,
		/// <summary>To be added.</summary>
		SpeechAlienChatter = 17,
		/// <summary>To be added.</summary>
		SpeechCosmicInterference = 18,
		/// <summary>To be added.</summary>
		SpeechGoldenPi = 19,
		/// <summary>To be added.</summary>
		SpeechRadioTower = 20,
		/// <summary>To be added.</summary>
		SpeechWaves = 21,
	}

	[Native]
	public enum AVAudioEnvironmentDistanceAttenuationModel : long {
		/// <summary>Gain = (Distance / ReferenceDistance)^(-RolloffFactor)</summary>
		Exponential = 1,
		/// <summary>Gain = ReferenceDistance / (ReferenceDistance + RolloffFactor * (Distance - ReferenceDistance))</summary>
		Inverse = 2,
		/// <summary>Gain = (1 - RolloffFactor * (Distance - ReferenceDistance) / (MaximumDistance - ReferenceDistance))</summary>
		Linear = 3,
	}

	[MacCatalyst (13, 1)]
	[Native]
	public enum AVQueuedSampleBufferRenderingStatus : long {
		/// <summary>No sample buffers have been enqueued yet.</summary>
		Unknown,
		/// <summary>At least one sample buffer is queued for rendering.</summary>
		Rendering,
		/// <summary>The player failed during or preparing for rendering.</summary>
		Failed,
	}

	[Introduced (PlatformName.MacCatalyst, 14, 0)]
	[TV (17, 0)]
	[Native]
	public enum AVCaptureVideoStabilizationMode : long {
		/// <summary>No video stabilization should be applied.</summary>
		Off,
		/// <summary>The default video stabilization algorithm should be used.</summary>
		Standard = 1,
		/// <summary>The "cinematic stabilization" algorithm should be used.</summary>
		Cinematic = 2,
		[iOS (13, 0)]
		[MacCatalyst (14, 0)]
		CinematicExtended = 3,
		[iOS (17, 0), MacCatalyst (17, 0), TV (17, 0), NoMac]
		PreviewOptimized = 4,
		[iOS (18, 0), MacCatalyst (18, 0), TV (18, 0), NoMac]
		CinematicExtendedEnhanced = 5,
		/// <summary>The device determines the stabilization mode.</summary>
		Auto = -1,
	}

	[Introduced (PlatformName.MacCatalyst, 14, 0)]
	[TV (17, 0)]
	[Native]
	public enum AVCaptureAutoFocusSystem : long {
		None,
		ContrastDetection,
		PhaseDetection,
	}

	[Introduced (PlatformName.MacCatalyst, 14, 0)]
	[TV (17, 0)]
	[Native]
	[NoMac]
	public enum AVCaptureSessionInterruptionReason : long {
		/// <summary>Indicates that the app was sent to the background while capturing.</summary>
		VideoDeviceNotAvailableInBackground = 1,
		/// <summary>Indicates that another client began using the audio capture device.</summary>
		AudioDeviceInUseByAnotherClient = 2,
		/// <summary>Indicates that another client began using the video capture device.</summary>
		VideoDeviceInUseByAnotherClient = 3,
		/// <summary>Indicates that the app was displayed in Slide Over, Split View, or PIP mode, which would result in resource contention and degraded capture quality.</summary>
		VideoDeviceNotAvailableWithMultipleForegroundApps = 4,
		/// <summary>To be added.</summary>
		[MacCatalyst (14, 0)]
		VideoDeviceNotAvailableDueToSystemPressure = 5,
	}

	[MacCatalyst (13, 1)]
	[Native]
	public enum AVSpeechSynthesisVoiceQuality : long {
		Default = 1,
		Enhanced = 2,
		[iOS (16, 0), MacCatalyst (16, 0), TV (16, 0), Mac (13, 0)]
		Premium = 3,
	}

	[MacCatalyst (13, 1)]
	[Native]
	public enum AVAudioConverterPrimeMethod : long {
		Pre = 0,
		Normal = 1,
		None = 2,
	}

	[MacCatalyst (13, 1)]
	[Native]
	public enum AVAudioConverterInputStatus : long {
		HaveData = 0,
		NoDataNow = 1,
		EndOfStream = 2,
	}

	[MacCatalyst (13, 1)]
	[Native]
	public enum AVAudioConverterOutputStatus : long {
		HaveData = 0,
		InputRanDry = 1,
		EndOfStream = 2,
		Error = 3,
	}

	[MacCatalyst (13, 1)]
	[Flags]
	[Native]
	public enum AVMusicSequenceLoadOptions : ulong {
		/// <summary>To be added.</summary>
		PreserveTracks = 0,
		/// <summary>To be added.</summary>
		ChannelsToTracks = (1 << 0),
	}

	[NoTV]
	[iOS (13, 0)]
	[MacCatalyst (13, 1)]
	[Flags]
	[Native]
	public enum AVMovieWritingOptions : ulong {
		/// <summary>To be added.</summary>
		AddMovieHeaderToDestination = 0,
		/// <summary>To be added.</summary>
		TruncateDestinationToMovieHeaderOnly = (1 << 0),
	}

	[NoiOS]
	[NoTV]
	[NoMacCatalyst]
	[Native]
	public enum AVContentAuthorizationStatus : long {
		Unknown,
		Completed,
		Cancelled,
		TimedOut,
		Busy,
		NotAvailable,
		NotPossible,
	}

	[iOS (15, 0)]
	[TV (15, 0)]
	[MacCatalyst (15, 0)]
	[Native]
	public enum AVSampleBufferRequestDirection : long {
		/// <summary>To be added.</summary>
		Forward = 1,
		/// <summary>To be added.</summary>
		None = 0,
		/// <summary>To be added.</summary>
		Reverse = -1,
	}

	[iOS (15, 0)]
	[TV (15, 0)]
	[MacCatalyst (15, 0)]
	[Native]
	public enum AVSampleBufferRequestMode : long {
		/// <summary>To be added.</summary>
		Immediate,
		/// <summary>To be added.</summary>
		Scheduled,
		[MacCatalyst (13, 1)]
		Opportunistic = 2,
	}

	[Introduced (PlatformName.MacCatalyst, 14, 0)]
	[TV (17, 0)]
	[Native]
	public enum AVCaptureColorSpace : long {
		/// <summary>Indicates an RGB color space.</summary>
		Srgb = 0,
		/// <summary>Indicates the DCI-P3 color space with a CIE D65 white point.</summary>
		P3D65 = 1,
		[Introduced (PlatformName.MacCatalyst, 14, 1)]
		[iOS (14, 1)]
		[NoMac]
		HlgBT2020 = 2,
		[NoMac, NoiOS, NoMacCatalyst]
		AppleLog = 3,
	}

	[MacCatalyst (13, 1)]
	[Native]
	public enum AVMusicTrackLoopCount : long {
		/// <summary>To be added.</summary>
		Forever = -1,
	}

	[MacCatalyst (13, 1)]
	[Native]
	public enum AVPlayerTimeControlStatus : long {
		/// <summary>Indicates that the player is paused.</summary>
		Paused,
		/// <summary>Indicates that the player is waiting to play at a specified rate.</summary>
		WaitingToPlayAtSpecifiedRate,
		/// <summary>Indicates that the player is playing.</summary>
		Playing,
	}

	[NoTV]
	[MacCatalyst (13, 1)]
	[Native]
	public enum AVAudioSessionIOType : long {
		NotSpecified = 0,
		Aggregated = 1,
	}

	[MacCatalyst (13, 1)]
	[Native]
	public enum AVPlayerLooperStatus : long {
		Unknown,
		Ready,
		Failed,
		Cancelled,
	}

	[NoiOS]
	[NoMac]
	[NoMacCatalyst]
	[Native]
	public enum AVContentProposalAction : long {
		Accept,
		Reject,
		Defer,
	}

	[NoiOS]
	[NoMac]
	[NoMacCatalyst]
	[Native]
	public enum AVPlayerViewControllerSkippingBehavior : long {
		Default = 0,
		SkipItem,
	}

	[MacCatalyst (13, 1)]
	[Native]
	public enum AVContentKeyRequestStatus : long {
		Requesting,
		Received,
		Renewed,
		Retried,
		Cancelled,
		Failed,
	}

	[MacCatalyst (13, 1)]
	public enum AVContentKeyRequestRetryReason {
		[Field ("AVContentKeyRequestRetryReasonTimedOut")]
		TimedOut,
		[Field ("AVContentKeyRequestRetryReasonReceivedResponseWithExpiredLease")]
		ReceivedResponseWithExpiredLease,
		[Field ("AVContentKeyRequestRetryReasonReceivedObsoleteContentKey")]
		ReceivedObsoleteContentKey,
	}

	[MacCatalyst (13, 1)]
	public enum AVContentKeySystem {
		[Field ("AVContentKeySystemFairPlayStreaming")]
		FairPlayStreaming = 0,

		[MacCatalyst (13, 1)]
		[Field ("AVContentKeySystemClearKey")]
		ClearKey = 1,

		[MacCatalyst (13, 1)]
		[Obsolete ("Use 'AVContentKeySystem.SystemClearKey' instead.")]
		AVContentKeySystemClearKey = ClearKey,

		[TV (13, 0)]
		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("AVContentKeySystemAuthorizationToken")]
		AuthorizationToken = 2,
	}

	// Convience enum for native string values 
	[MacCatalyst (13, 1)]
	public enum AVAssetExportSessionPreset {
		[MacCatalyst (13, 1)]
		[Field ("AVAssetExportPresetLowQuality")]
		LowQuality = 0, // AVAssetExportPresetLowQuality
		[MacCatalyst (13, 1)]
		[Field ("AVAssetExportPresetMediumQuality")]
		MediumQuality = 1, // AVAssetExportPresetMediumQuality
		[MacCatalyst (13, 1)]
		[Field ("AVAssetExportPresetHighestQuality")]
		HighestQuality = 2, // AVAssetExportPresetHighestQuality
		[Field ("AVAssetExportPreset640x480")]
		Preset640x480 = 3, // AVAssetExportPreset640x480
		[Field ("AVAssetExportPreset960x540")]
		Preset960x540 = 4, // AVAssetExportPreset960x540
		[Field ("AVAssetExportPreset1280x720")]
		Preset1280x720 = 5, // AVAssetExportPreset1280x720
		[Field ("AVAssetExportPreset1920x1080")]
		Preset1920x1080 = 6, // AVAssetExportPreset1920x1080

		[MacCatalyst (13, 1)]
		[Field ("AVAssetExportPreset3840x2160")]
		Preset3840x2160 = 7, // AVAssetExportPreset3840x2160

		[Field ("AVAssetExportPresetAppleM4A")]
		AppleM4A = 8, // AVAssetExportPresetAppleM4A
		[Field ("AVAssetExportPresetPassthrough")]
		Passthrough = 9, // AVAssetExportPresetPassthrough

		[MacCatalyst (13, 1)]
		[Obsolete ("Use 'AVOutputSettingsPreset.PresetHevc1920x1080' instead.")]
		[Field ("AVOutputSettingsPresetHEVC1920x1080")]
		PresetHevc1920x1080 = 11,

		[MacCatalyst (13, 1)]
		[Obsolete ("Use 'AVOutputSettingsPreset.PresetHevc3840x2160' instead.")]
		[Field ("AVOutputSettingsPresetHEVC3840x2160")]
		PresetHevc3840x2160 = 12,
	}

	[MacCatalyst (13, 1)]
	public enum AVOutputSettingsPreset {
		[Field ("AVOutputSettingsPreset640x480")]
		Preset640x480 = 0,

		[Field ("AVOutputSettingsPreset960x540")]
		Preset960x540 = 1,

		[Field ("AVOutputSettingsPreset1280x720")]
		Preset1280x720 = 2,

		[Field ("AVOutputSettingsPreset1920x1080")]
		Preset1920x1080 = 3,

		[MacCatalyst (13, 1)]
		[Field ("AVOutputSettingsPreset3840x2160")]
		Preset3840x2160 = 4,

		[MacCatalyst (13, 1)]
		[Field ("AVOutputSettingsPresetHEVC1920x1080")]
		PresetHevc1920x1080 = 11, // we added the wrong value in the export enum, we use the same so that they can be swap

		[MacCatalyst (13, 1)]
		[Field ("AVOutputSettingsPresetHEVC3840x2160")]
		PresetHevc3840x2160 = 12, // we added the wrong value in the export enum, we use the same so that they can be swap

		[TV (13, 0)]
		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("AVOutputSettingsPresetHEVC1920x1080WithAlpha")]
		PresetHevc1920x1080WithAlpha = 13,

		[TV (13, 0)]
		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("AVOutputSettingsPresetHEVC3840x2160WithAlpha")]
		PresetHevc3840x2160WithAlpha = 14,

		[NoTV]
		[Mac (12, 1)]
		[NoiOS]
		[NoMacCatalyst]
		[Field ("AVOutputSettingsPresetHEVC7680x4320")]
		PresetHevc7680x4320 = 15,

		[TV (17, 0), MacCatalyst (17, 0), Mac (14, 0), iOS (17, 0)]
		[Field ("AVOutputSettingsPresetMVHEVC960x960")]
		PresetMvHevc960x960 = 16,

		[TV (17, 0), MacCatalyst (17, 0), Mac (14, 0), iOS (17, 0)]
		[Field ("AVOutputSettingsPresetMVHEVC1440x1440")]
		PresetMvHevc1440x1440 = 17,
	}

	[Introduced (PlatformName.MacCatalyst, 14, 0)]
	[Native]
	public enum AVDepthDataAccuracy : long {
		/// <summary>Indicates that the depth data indicate relative depths from depth to depth.</summary>
		Relative = 0,
		/// <summary>Indicates that the depth data indicate distances in the real world.</summary>
		Absolute = 1,
	}

	[MacCatalyst (13, 1)]
	[Native]
	public enum AVAudioEngineManualRenderingMode : long {
		Offline = 0,
		Realtime = 1,
	}

	[MacCatalyst (13, 1)]
	[Native]
	public enum AVAudioEngineManualRenderingStatus : long {
		Error = -1,
		Success = 0,
		InsufficientDataFromInputNode = 1,
		CannotDoInCurrentContext = 2,
	}

	[MacCatalyst (13, 1)]
	[Native]
	public enum AVAudioSessionRouteSharingPolicy : ulong {
		Default = 0,
#if !XAMCORE_5_0
		[Deprecated (PlatformName.iOS, 13, 0, message: "Use 'LongFormAudio' instead.")]
		[Deprecated (PlatformName.TvOS, 13, 0, message: "Use 'LongFormAudio' instead.")]
		[Deprecated (PlatformName.MacOSX, 10, 15, message: "Use 'LongFormAudio' instead.")]
		[Deprecated (PlatformName.MacCatalyst, 13, 11, message: "Use 'LongFormAudio' instead.")]
		LongForm = 1,
#endif
		LongFormAudio = 1,
		Independent = 2,
		[iOS (13, 0)]
		[NoTV]
		[NoMac]
		[MacCatalyst (14, 0)]
		LongFormVideo = 3,
	}

	[MacCatalyst (13, 1)]
	[Native]
	public enum AVAudioPlayerNodeCompletionCallbackType : long {
		/// <summary>To be added.</summary>
		Consumed = 0,
		/// <summary>To be added.</summary>
		Rendered = 1,
		/// <summary>To be added.</summary>
		PlayedBack = 2,
	}

	[MacCatalyst (13, 1)]
	public enum AVAudioEngineManualRenderingError {
		InvalidMode = -80800,
		Initialized = -80801,
		NotRunning = -80802,
	}

	[Introduced (PlatformName.MacCatalyst, 14, 0)]
	[TV (17, 0)]
	[Native]
	public enum AVCaptureLensStabilizationStatus : long {
		Unsupported = 0,
		Off = 1,
		Active = 2,
		OutOfRange = 3,
		Unavailable = 4,
	}

	[Introduced (PlatformName.MacCatalyst, 14, 0)]
	[TV (17, 0)]
	[Native]
	public enum AVCaptureOutputDataDroppedReason : long {
		None = 0,
		LateData = 1,
		OutOfBuffers = 2,
		Discontinuity = 3,
	}

	[MacCatalyst (13, 1)]
	public enum AVVideoApertureMode {
		[Field ("AVVideoApertureModeCleanAperture")]
		CleanAperture = 0,

		[Field ("AVVideoApertureModeProductionAperture")]
		ProductionAperture = 1,

		[Field ("AVVideoApertureModeEncodedPixels")]
		EncodedPixels = 2,
	}
	[NoTV]
	[MacCatalyst (13, 1)]
	public enum AVAssetDownloadedAssetEvictionPriority {
		/// <summary>To be added.</summary>
		[Field ("AVAssetDownloadedAssetEvictionPriorityDefault")]
		Default = 0,

		/// <summary>To be added.</summary>
		[Field ("AVAssetDownloadedAssetEvictionPriorityImportant")]
		Important = 1,
	}

	[MacCatalyst (13, 1)]
	public enum AVAssetWriterInputMediaDataLocation {
		[Field ("AVAssetWriterInputMediaDataLocationInterleavedWithMainMediaData")]
		InterleavedWithMainMediaData = 0,

		[Field ("AVAssetWriterInputMediaDataLocationBeforeMainMediaDataNotInterleaved")]
		BeforeMainMediaDataNotInterleaved = 1,
	}

	[MacCatalyst (15, 0)]
	public enum AVVideoCodecType {
		[Field ("AVVideoCodecTypeH264")]
		H264 = 0,

		[Field ("AVVideoCodecTypeJPEG")]
		Jpeg = 1,

		[Field ("AVVideoCodecTypeAppleProRes422")]
		AppleProRes422 = 3,

		[Field ("AVVideoCodecTypeAppleProRes4444")]
		AppleProRes4444 = 4,

		[Field ("AVVideoCodecTypeHEVC")]
		Hevc = 5,

		[TV (13, 0)]
		[iOS (13, 0)]
		[MacCatalyst (15, 0)]
		[Field ("AVVideoCodecTypeAppleProRes422HQ")]
		AppleProRes422HQ = 6,

		[TV (13, 0)]
		[iOS (13, 0)]
		[MacCatalyst (15, 0)]
		[Field ("AVVideoCodecTypeAppleProRes422LT")]
		AppleProRes422LT = 7,

		[TV (13, 0)]
		[iOS (13, 0)]
		[MacCatalyst (15, 0)]
		[Field ("AVVideoCodecTypeAppleProRes422Proxy")]
		AppleProRes422Proxy = 8,

		[TV (13, 0)]
		[iOS (13, 0)]
		[MacCatalyst (15, 0)]
		[Field ("AVVideoCodecTypeHEVCWithAlpha")]
		HevcWithAlpha = 9,

		[TV (18, 0), MacCatalyst (18, 0), Mac (15, 0), iOS (18, 0)]
		[Field ("AVVideoCodecTypeAppleProRes4444XQ")]
		AppleProRes4444XQ = 10,

		[TV (18, 0), MacCatalyst (18, 0), Mac (15, 0), iOS (18, 0)]
		[Field ("AVVideoCodecTypeJPEGXL")]
		JpegXl = 11,
	}

	[Introduced (PlatformName.MacCatalyst, 14, 0)]
	[Native]
	public enum AVDepthDataQuality : long {
		/// <summary>To be added.</summary>
		Low = 0,
		/// <summary>To be added.</summary>
		High = 1,
	}

	[Introduced (PlatformName.MacCatalyst, 14, 0)]
	[TV (17, 0)]
	[NoMac]
	[Flags]
	[Native]
	public enum AVCaptureSystemPressureFactors : ulong {
		/// <summary>To be added.</summary>
		None = 0,
		/// <summary>To be added.</summary>
		SystemTemperature = (1 << 0),
		/// <summary>To be added.</summary>
		PeakPower = (1 << 1),
		/// <summary>To be added.</summary>
		DepthModuleTemperature = (1 << 2),
		[iOS (17, 0), MacCatalyst (17, 0)]
		CameraTemperature = (1uL << 3),
	}

	[NoMac]
	[MacCatalyst (13, 1)]
	[Native]
	[Flags]
	public enum AVPlayerHdrMode : long {
		/// <summary>To be added.</summary>
		Hlg = 0x1,
		/// <summary>To be added.</summary>
		Hdr10 = 0x2,
		/// <summary>To be added.</summary>
		DolbyVision = 0x4,
	}

	[MacCatalyst (13, 1)]
	[Flags]
	[Native]
	public enum AVAudioSessionActivationOptions : ulong {
		None = 0x0,
	}

	[Native]
	public enum AVAudioSessionPromptStyle : ulong {
		None = 0x6e6f6e65, // 1852796517 - 'none'
		Short = 0x73687274, // 1936224884 - 'shrt'
		Normal = 0x6e726d6c, //1852992876 - 'nrml'
	}

	[iOS (13, 0)]
	[TV (13, 0)]
	[MacCatalyst (13, 1)]
	[Native]
	public enum AVSpeechSynthesisVoiceGender : long {
		Unspecified,
		Male,
		Female,
	}
	[Introduced (PlatformName.MacCatalyst, 14, 0)]
	[TV (17, 0)]
	[Mac (13, 0)]
	[iOS (13, 0)]
	[Native]
	public enum AVCapturePhotoQualityPrioritization : long {
		Speed = 1,
		Balanced = 2,
		Quality = 3,
	}

	[TV (13, 0)]
	[iOS (13, 0)]
	[MacCatalyst (13, 1)]
	[Native]
	public enum AVAudioEnvironmentOutputType : long {
		Auto = 0,
		Headphones = 1,
		BuiltInSpeakers = 2,
		ExternalSpeakers = 3,
	}

	[TV (13, 0)]
	[iOS (13, 0)]
	[MacCatalyst (13, 1)]
	[Native]
	public enum AVAudio3DMixingSourceMode : long {
		SpatializeIfMono = 0,
		Bypass = 1,
		PointSource = 2,
		AmbienceBed = 3,
	}

	[TV (13, 0)]
	[iOS (13, 0)]
	[MacCatalyst (13, 1)]
	[Native]
	public enum AVAudio3DMixingPointSourceInHeadMode : long {
		Mono = 0,
		Bypass = 1,
	}

	[TV (14, 0)]
	[iOS (14, 0)]
	[MacCatalyst (14, 0)]
	[Native]
	public enum AVAssetSegmentType : long {
		Initialization = 1,
		Separable = 2,
	}

	[TV (14, 0)]
	[iOS (14, 0)]
	[MacCatalyst (14, 0)]
	[Flags]
	[Native]
	public enum AVAudioSpatializationFormats : ulong {
		None = 0,
		MonoAndStereo = 3,
		Multichannel = 4,
		MonoStereoAndMultichannel = 7,
	}

	[TV (14, 0)]
	[iOS (14, 0)]
	[MacCatalyst (14, 0)]
	[Native]
	public enum AVAudioStereoOrientation : long {
		None = 0,
		Portrait = 1,
		PortraitUpsideDown = 2,
		LandscapeRight = 3,
		LandscapeLeft = 4,
	}

	[TV (14, 0)]
	[iOS (14, 0)]
	[MacCatalyst (14, 0)]
	public enum AVFileTypeProfile {
		[Field (null)]
		None = 0,
		[Field ("AVFileTypeProfileMPEG4AppleHLS")]
		Mpeg4AppleHls,
		[Field ("AVFileTypeProfileMPEG4CMAFCompliant")]
		Mpeg4CmafCompliant,
	}

	[TV (15, 0)]
	[iOS (15, 0)]
	[MacCatalyst (15, 0)]
	[Native]
	public enum AVAudioRoutingArbitrationCategory : long {
		Playback = 0,
		PlayAndRecord = 1,
		PlayAndRecordVoice = 2,
	}

	[MacCatalyst (13, 1)]
	public enum AVContentKeyResponseDataType {
		FairPlayStreamingKeyResponseData,
		[TV (13, 0)]
		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		AuthorizationTokenData,
	}

	[TV (15, 0)]
	[iOS (15, 0)]
	[MacCatalyst (15, 0)]
	[Flags]
	[Native]
	public enum AVDelegatingPlaybackCoordinatorRateChangeOptions : ulong {
		None = 0,
		PlayImmediately = (1uL << 0),
	}

	[TV (15, 0)]
	[iOS (15, 0)]
	[MacCatalyst (15, 0)]
	[Flags]
	[Native]
	public enum AVDelegatingPlaybackCoordinatorSeekOptions : ulong {
		None = 0,
		ResumeImmediately = (1uL << 0),
	}

	[TV (17, 0)]
	[iOS (15, 0)]
	[MacCatalyst (15, 0)]
	[Native]
	public enum AVCaptureMicrophoneMode : long {
		Standard = 0,
		WideSpectrum = 1,
		VoiceIsolation = 2,
	}

	[TV (17, 0)]
	[iOS (15, 0)]
	[MacCatalyst (15, 0)]
	[Native]
	public enum AVCaptureSystemUserInterface : long {
		VideoEffects = 1,
		MicrophoneModes = 2,
	}

	[TV (15, 0)]
	[iOS (15, 0)]
	[MacCatalyst (15, 0)]
	[Native]
	public enum AVPlayerAudiovisualBackgroundPlaybackPolicy : long {
		Automatic = 1,
		Pauses = 2,
		ContinuesIfPossible = 3,
	}

	[TV (15, 0)]
	[iOS (15, 0)]
	[MacCatalyst (15, 0)]
	public enum AVCoordinatedPlaybackSuspensionReason {

		[Field ("AVCoordinatedPlaybackSuspensionReasonAudioSessionInterrupted")]
		AudioSessionInterrupted,

		[Field ("AVCoordinatedPlaybackSuspensionReasonStallRecovery")]
		StallRecovery,

		[Field ("AVCoordinatedPlaybackSuspensionReasonPlayingInterstitial")]
		PlayingInterstitial,

		[Field ("AVCoordinatedPlaybackSuspensionReasonCoordinatedPlaybackNotPossible")]
		CoordinatedPlaybackNotPossible,

		[Field ("AVCoordinatedPlaybackSuspensionReasonUserActionRequired")]
		UserActionRequired,

		[Field ("AVCoordinatedPlaybackSuspensionReasonUserIsChangingCurrentTime")]
		UserIsChangingCurrentTime,
	}

	[iOS (18, 0), MacCatalyst (18, 0)]
	[NoTV]
	[Native]
	public enum AVCaptionAnimation : long {
		None = 0,
		CharacterReveal = 1,
	}

	[iOS (18, 0), MacCatalyst (18, 0)]
	[NoTV]
	[Native]
	public enum AVCaptionFontWeight : long {
		Unknown = 0,
		Normal = 1,
		Bold = 2,
	}

	[iOS (18, 0), MacCatalyst (18, 0)]
	[NoTV]
	[Native]
	public enum AVCaptionFontStyle : long {
		Unknown = 0,
		Normal = 1,
		Italic = 2,
	}

	[iOS (18, 0), MacCatalyst (18, 0)]
	[NoTV]
	[Flags]
	[Native]
	public enum AVCaptionDecoration : ulong {
		None = 0x0,
		Underline = 1uL << 0,
		LineThrough = 1uL << 1,
		Overline = 1uL << 2,
	}

	[iOS (18, 0), MacCatalyst (18, 0)]
	[NoTV]
	[Native]
	public enum AVCaptionTextCombine : long {
		All = -1,
		None = 0,
		OneDigit = 1,
		TwoDigits = 2,
		ThreeDigits = 3,
		FourDigits = 4,
	}

	[iOS (18, 0), MacCatalyst (18, 0)]
	[NoTV]
	[Native]
	public enum AVCaptionTextAlignment : long {
		Start = 0,
		End = 1,
		Center = 2,
		Left = 3,
		Right = 4,
	}

	[iOS (18, 0), MacCatalyst (18, 0)]
	[NoTV]
	[Native]
	public enum AVCaptionRegionWritingMode : long {
		LeftToRightAndTopToBottom = 0,
		TopToBottomAndRightToLeft = 2,
	}

	[iOS (18, 0), MacCatalyst (18, 0)]
	[NoTV]
	[Native]
	public enum AVCaptionRegionScroll : long {
		None = 0,
		RollUp = 1,
	}

	[iOS (18, 0), MacCatalyst (18, 0)]
	[NoTV]
	[Native]
	public enum AVCaptionRegionDisplayAlignment : long {
		Before = 0,
		Center = 1,
		After = 2,
	}

	[iOS (18, 0), MacCatalyst (18, 0)]
	[NoTV]
	[Native]
	public enum AVCaptionRubyPosition : long {
		Before = 0,
		After = 1,
	}

	[iOS (18, 0), MacCatalyst (18, 0)]
	[NoTV]
	[Native]
	public enum AVCaptionRubyAlignment : long {
		Start = 0,
		Center = 1,
		DistributeSpaceBetween = 2,
		DistributeSpaceAround = 3,
	}

	[iOS (18, 0), MacCatalyst (18, 0)]
	[NoTV]
	[Native]
	public enum AVCaptionConversionValidatorStatus : long {
		Unknown = 0,
		Validating = 1,
		Completed = 2,
		Stopped = 3,
	}

	[TV (17, 0)]
	[MacCatalyst (15, 0)]
	[iOS (15, 0)]
	[Native]
	public enum AVCapturePrimaryConstituentDeviceSwitchingBehavior : long {
		Unsupported = 0,
		Auto = 1,
		Restricted = 2,
		Locked = 3,
	}

	[TV (17, 0)]
	[MacCatalyst (15, 0)]
	[iOS (15, 0)]
	[Flags]
	[Native]
	public enum AVCapturePrimaryConstituentDeviceRestrictedSwitchingBehaviorConditions : ulong {
		None = 0x0,
		VideoZoomChanged = 1uL << 0,
		FocusModeChanged = 1uL << 1,
		ExposureModeChanged = 1uL << 2,
	}

	[TV (17, 2), Mac (14, 2), iOS (17, 2), MacCatalyst (17, 2)]
	[Native]
	public enum AVAudioSessionRenderingMode : long {
		NotApplicable = 0,
		MonoStereo = 1,
		Surround = 2,
		SpatialAudio = 3,
		DolbyAudio = 4,
		DolbyAtmos = 5,
	}

	[TV (16, 0), Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0)]
	[Native]
	[NativeName ("AVMIDIControlChangeMessageType")]
	public enum AVMidiControlChangeMessageType : long {
		BankSelect = 0,
		ModWheel = 1,
		Breath = 2,
		Foot = 4,
		PortamentoTime = 5,
		DataEntry = 6,
		Volume = 7,
		Balance = 8,
		Pan = 10,
		Expression = 11,
		Sustain = 64,
		Portamento = 65,
		Sostenuto = 66,
		Soft = 67,
		LegatoPedal = 68,
		Hold2Pedal = 69,
		FilterResonance = 71,
		ReleaseTime = 72,
		AttackTime = 73,
		Brightness = 74,
		DecayTime = 75,
		VibratoRate = 76,
		VibratoDepth = 77,
		VibratoDelay = 78,
		ReverbLevel = 91,
		ChorusLevel = 93,
		RpnLsb = 100,
		RpnMsb = 101,
		AllSoundOff = 120,
		ResetAllControllers = 121,
		AllNotesOff = 123,
		OmniModeOff = 124,
		OmniModeOn = 125,
		MonoModeOn = 126,
		MonoModeOff = 127,
	}

	[TV (16, 0), Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0)]
	[Native]
	[NativeName ("AVMIDIMetaEventType")]
	public enum AVMidiMetaEventType : long {
		SequenceNumber = 0,
		Text = 1,
		Copyright = 2,
		TrackName = 3,
		Instrument = 4,
		Lyric = 5,
		Marker = 6,
		CuePoint = 7,
		MidiChannel = 32,
		MidiPort = 33,
		EndOfTrack = 47,
		Tempo = 81,
		SmpteOffset = 84,
		TimeSignature = 88,
		KeySignature = 89,
		ProprietaryEvent = 127,
	}

	[TV (17, 0), Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0)]
	[Native]
	public enum AVAudioVoiceProcessingSpeechActivityEvent : long {
		Started = 0,
		Ended = 1,
	}

	[TV (17, 0), Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0)]
	[Native]
	public enum AVSpeechSynthesisPersonalVoiceAuthorizationStatus : ulong {
		NotDetermined,
		Denied,
		Unsupported,
		Authorized,
	}

	[TV (17, 0), Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0)]
	[Native]
	public enum AVSpeechSynthesisVoiceTraits : ulong {
		None = 0x0,
		IsNoveltyVoice = 1uL << 0,
		IsPersonalVoice = 1uL << 1,
	}
	[TV (16, 0), Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0)]
	[Native, Flags]
	public enum AVAssetTrackGroupOutputHandling : ulong {
		None = 0x0,
		PreserveAlternateTracks = (1uL << 0),
		DefaultPolicy = None,
	}

	[TV (16, 0), Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0)]
	[Native]
	public enum AVSpeechSynthesisMarkerMark : long {
		Phoneme,
		Word,
		Sentence,
		Paragraph,
		Bookmark,
	}

	[NoTV, Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0)]
	[Native]
	public enum AVAudioVoiceProcessingOtherAudioDuckingLevel : long {
		Default = 0,
		Min = 10,
		Mid = 20,
		Max = 30,
	}

	[TV (17, 0), MacCatalyst (17, 0), Mac (14, 0), iOS (17, 0)]
	[Native]
	public enum AVCapturePhotoOutputCaptureReadiness : long {
		SessionNotRunning = 0,
		Ready = 1,
		NotReadyMomentarily = 2,
		NotReadyWaitingForCapture = 3,
		NotReadyWaitingForProcessing = 4,
	}

	[TV (17, 0), Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0)]
	[Native]
	public enum AVPlayerLooperItemOrdering : long {
		PrecedeExistingItems = 0,
		FollowExistingItems = 1,
	}

	[TV (17, 4), Mac (14, 4), iOS (17, 4), MacCatalyst (17, 4)]
	[Native]
	public enum AVExternalContentProtectionStatus : long {
		Pending = 0,
		Sufficient = 1,
		Insufficient = 2,
	}

	[TV (16, 4), Mac (13, 3), iOS (16, 4), MacCatalyst (16, 4)]
	[Native]
	public enum AVPlayerInterstitialEventAssetListResponseStatus : long {
		Available = 0,
		Cleared = 1,
		Unavailable = 2,
	}

	[TV (16, 0), Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0)]
	enum AVAssetPlaybackConfigurationOption {
		[Field ("AVAssetPlaybackConfigurationOptionStereoVideo")]
		StereoVideo,

		[Field ("AVAssetPlaybackConfigurationOptionStereoMultiviewVideo")]
		StereoMultiviewVideo,

		[MacCatalyst (18, 0), TV (18, 0), Mac (15, 0), iOS (18, 0)]
		[Field ("AVAssetPlaybackConfigurationOptionSpatialVideo")]
		SpatialVideo,
	}

	[TV (16, 0), Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0)]
	enum AVPlayerInterstitialEventCue {
		[Field ("AVPlayerInterstitialEventNoCue")]
		NoCue,

		[Field ("AVPlayerInterstitialEventJoinCue")]
		JoinCue,

		[Field ("AVPlayerInterstitialEventLeaveCue")]
		LeaveCue,
	}

	[TV (18, 0), Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
	[Native]
	public enum AVPlayerInterstitialEventTimelineOccupancy : long {
		SinglePoint = 0,
		Fill = 1,
	}

	// Yes, this type is in AVFoundation, not CoreMedia
	[TV (17, 2), Mac (14, 2), iOS (17, 2), MacCatalyst (17, 2)]
	public enum CMTagCollectionVideoOutputPreset : uint {
		Monoscopic,
		Stereoscopic,
	}
}
