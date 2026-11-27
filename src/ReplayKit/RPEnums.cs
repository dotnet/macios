//
// ReplayKit enums
//
// Copyright 2015-2016 Xamarin Inc. All rights reserved.
//

namespace ReplayKit {

	/// <summary>Enumerates errors that can be encountered while recording.</summary>
	[MacCatalyst (13, 1)]
	[Native ("RPRecordingErrorCode")]
	[ErrorDomain ("RPRecordingErrorDomain")]
	public enum RPRecordingError : long {
		/// <summary>Indicates that no error occurred.</summary>
		None = 0,
		/// <summary>Indicates that an unknown error occurred.</summary>
		Unknown = -5800,
		/// <summary>Indicates that the user declined to begin the recording.</summary>
		UserDeclined = -5801,
		/// <summary>Indicates that parental controls prevented the recording.</summary>
		Disabled = -5802,
		/// <summary>Indicates that the recording failed to start.</summary>
		FailedToStart = -5803,
		/// <summary>Indicates that an error occurred.</summary>
		Failed = -5804,
		/// <summary>Indicates that the available storage was not sufficient.</summary>
		InsufficientStorage = -5805,
		/// <summary>Indicates that another app interrupted the recording.</summary>
		Interrupted = -5806,
		/// <summary>Indicates that the recording was interrupted when the content was resized or the user multitasked.</summary>
		ContentResize = -5807,
		/// <summary>Indicates that the broadcast session was not started.</summary>
		BroadcastInvalidSession = -5808,
		/// <summary>Indicates that the user pressed the power button.</summary>
		SystemDormancy = -5809,
		/// <summary>Indicates that required entitlements were missing.</summary>
		Entitlements = -5810,
		/// <summary>Indicates that an active phone call prevented recording.</summary>
		ActivePhoneCall = -5811,
		/// <summary>The recording failed to save to the camera roll.</summary>
		FailedToSave = -5812,
		/// <summary>The recording failed to start because CarPlay is active.</summary>
		CarPlay = -5813,
		/// <summary>The connection to the app being recorded became invalid.</summary>
		FailedApplicationConnectionInvalid = -5814,
		/// <summary>The connection to the app being recorded was interrupted.</summary>
		FailedApplicationConnectionInterrupted = -5815,
		/// <summary>No matching application context was found for the recording.</summary>
		FailedNoMatchingApplicationContext = -5816,
		/// <summary>The media services failed during the recording.</summary>
		FailedMediaServicesFailure = -5817,
		/// <summary>Mixing the audio and video failed during the recording.</summary>
		VideoMixingFailure = -5818,
		/// <summary>The broadcast setup failed.</summary>
		BroadcastSetupFailed = -5819,
		/// <summary>The system couldn't obtain the URL for the recording.</summary>
		FailedToObtainUrl = -5820,
		/// <summary>The start or end time interval was malformed.</summary>
		FailedIncorrectTimeStamps = -5821,
		/// <summary>The system failed to process the first sample of the recording.</summary>
		FailedToProcessFirstSample = -5822,
		/// <summary>The asset writer failed to save the recording.</summary>
		FailedAssetWriterFailedToSave = -5823,
		/// <summary>There was no asset writer for the recording.</summary>
		FailedNoAssetWriter = -5824,
		/// <summary>The asset writer was in the wrong state.</summary>
		FailedAssetWriterInWrongState = -5825,
		/// <summary>The asset writer failed to export the recording.</summary>
		FailedAssetWriterExportFailed = -5826,
		/// <summary>The temporary recording file couldn't be removed.</summary>
		FailedToRemoveFile = -5827,
		/// <summary>The export by the asset writer was canceled.</summary>
		FailedAssetWriterExportCanceled = -5828,
		/// <summary>The app tried to stop a recording that wasn't in progress.</summary>
		AttemptToStopNonRecording = -5829,
		/// <summary>The app tried to start a recording while one was already in progress.</summary>
		AttemptToStartInRecordingState = -5830,
		/// <summary>Saving the recording to the camera roll failed.</summary>
		PhotoFailure = -5831,
		/// <summary>The recording session is invalid.</summary>
		RecordingInvalidSession = -5832,
		/// <summary>The system failed to start the capture stack.</summary>
		FailedToStartCaptureStack = -5833,
		/// <summary>A parameter passed to the operation was invalid.</summary>
		InvalidParameter = -5834,
		/// <summary>The recording failed because of a file permissions error.</summary>
		FilePermissions = -5835,
		/// <summary>A clip export to a URL is already in progress.</summary>
		ExportClipToUrlInProgress = -5836,
	}

	/// <summary>Enumerates the modes of a preview view controller.</summary>
	[NoiOS]
	[NoMac]
	[NoMacCatalyst]
	[Native]
	public enum RPPreviewViewControllerMode : long {
		/// <summary>The view controller shows a preview of the recording.</summary>
		Preview,
		/// <summary>The view controller offers to share the recording.</summary>
		Share,
	}

	[Native]
	[MacCatalyst (13, 1)]
	public enum RPSampleBufferType : long {
		/// <summary>To be added.</summary>
		Video = 1,
		/// <summary>To be added.</summary>
		AudioApp,
		/// <summary>To be added.</summary>
		AudioMic,
	}

	/// <summary>Enumerates camera locations on the device.</summary>
	[Native]
	[NoTV]
	[MacCatalyst (13, 1)]
	public enum RPCameraPosition : long {
		/// <summary>Indicates that the camera in use is on the front of the device.</summary>
		Front = 1,
		/// <summary>Indicates that the camera in use is on the back of the device.</summary>
		Back,
	}
}
