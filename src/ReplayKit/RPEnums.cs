//
// ReplayKit enums
//
// Copyright 2015-2016 Xamarin Inc. All rights reserved.
//

using System;
using ObjCRuntime;
using Foundation;

namespace ReplayKit {

	/// <summary>Enumerates errors that can be encountered while recording.</summary>
	/// <remarks>To be added.</remarks>
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
		/// <summary>To be added.</summary>
		FailedToSave = -5812,
		/// <summary>To be added.</summary>
		CarPlay = -5813,
		/// <summary>To be added.</summary>
		FailedApplicationConnectionInvalid = -5814,
		/// <summary>To be added.</summary>
		FailedApplicationConnectionInterrupted = -5815,
		/// <summary>To be added.</summary>
		FailedNoMatchingApplicationContext = -5816,
		/// <summary>To be added.</summary>
		FailedMediaServicesFailure = -5817,
		/// <summary>To be added.</summary>
		VideoMixingFailure = -5818,
		BroadcastSetupFailed = -5819,
		FailedToObtainUrl = -5820,
		FailedIncorrectTimeStamps = -5821,
		FailedToProcessFirstSample = -5822,
		FailedAssetWriterFailedToSave = -5823,
		FailedNoAssetWriter = -5824,
		FailedAssetWriterInWrongState = -5825,
		FailedAssetWriterExportFailed = -5826,
		FailedToRemoveFile = -5827,
		FailedAssetWriterExportCanceled = -5828,
		AttemptToStopNonRecording = -5829,
		AttemptToStartInRecordingState = -5830,
		PhotoFailure = -5831,
		RecordingInvalidSession = -5832,
		FailedToStartCaptureStack = -5833,
		InvalidParameter = -5834,
		FilePermissions = -5835,
		ExportClipToUrlInProgress = -5836,
	}

	[NoiOS]
	[NoMac]
	[NoMacCatalyst]
	[Native]
	public enum RPPreviewViewControllerMode : long {
		Preview,
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
