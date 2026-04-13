//
// AREnums.cs: Enums for the ARKit C API
//
// Copyright 2025 Microsoft Corp
//

#if __MACOS__
#nullable enable

using System;
using ObjCRuntime;

namespace ARKit {

	/// <summary>Status of an authorization for ARKit data.</summary>
	[SupportedOSPlatform ("macos26.0")]
	[NativeName ("ar_authorization_status_t")]
	public enum ARAuthorizationStatus : long {
		/// <summary>The user has not yet granted permission.</summary>
		NotDetermined = 0,
		/// <summary>The user has explicitly granted permission.</summary>
		Allowed = 1,
		/// <summary>The user has explicitly denied permission.</summary>
		Denied = 2,
	}

	/// <summary>Types of authorization for ARKit data.</summary>
	[Flags]
	[SupportedOSPlatform ("macos26.0")]
	[NativeName ("ar_authorization_type_t")]
	public enum ARAuthorizationType : ulong {
		/// <summary>No authorization type.</summary>
		None = 0,
		/// <summary>Authorization type used when requesting hand tracking.</summary>
		HandTracking = (1 << 0),
		/// <summary>Authorization type used when requesting world sensing (image tracking, plane detection, scene reconstruction).</summary>
		WorldSensing = (1 << 1),
		/// <summary>Authorization type used when requesting camera access.</summary>
		CameraAccess = (1 << 3),
	}

	/// <summary>State of an ARKit data provider.</summary>
	[SupportedOSPlatform ("macos26.0")]
	[NativeName ("ar_data_provider_state_t")]
	public enum ARDataProviderState : long {
		/// <summary>The data provider is initialized but not yet running.</summary>
		Initialized = 0,
		/// <summary>The data provider is running.</summary>
		Running = 1,
		/// <summary>The data provider is paused.</summary>
		Paused = 2,
		/// <summary>The data provider has stopped.</summary>
		Stopped = 3,
	}

	/// <summary>Status of a device anchor query.</summary>
	[SupportedOSPlatform ("macos26.0")]
	[NativeName ("ar_device_anchor_query_status_t")]
	public enum ARDeviceAnchorQueryStatus : long {
		/// <summary>The device anchor at the specified timestamp was successfully obtained.</summary>
		Success = 0,
		/// <summary>The device anchor at the specified timestamp failed to be obtained.</summary>
		Failure = 1,
	}

	/// <summary>Tracking states of a device anchor.</summary>
	[SupportedOSPlatform ("macos26.0")]
	[NativeName ("ar_device_anchor_tracking_state_t")]
	public enum ARDeviceAnchorTrackingState : long {
		/// <summary>The anchor is not tracked.</summary>
		Untracked = 0,
		/// <summary>Only orientation is currently tracked.</summary>
		OrientationTracked = 1,
		/// <summary>Both position and orientation are currently tracked.</summary>
		Tracked = 2,
	}

	/// <summary>Error codes for ARKit session operations.</summary>
	[SupportedOSPlatform ("macos26.0")]
	[NativeName ("ar_session_error_code_t")]
	public enum ARSessionErrorCode : long {
		/// <summary>A data provider requires an authorization that has not been granted by the user.</summary>
		DataProviderNotAuthorized = 100,
		/// <summary>A data provider has failed to run.</summary>
		DataProviderFailedToRun = 101,
	}

	/// <summary>Error codes for ARKit world tracking operations.</summary>
	[SupportedOSPlatform ("macos26.0")]
	[NativeName ("ar_world_tracking_error_code_t")]
	public enum ARWorldTrackingErrorCode : long {
		/// <summary>A world anchor failed to be added.</summary>
		AddAnchorFailed = 200,
		/// <summary>The maximum number of world anchors has been reached.</summary>
		AnchorMaxLimitReached = 201,
		/// <summary>A world anchor failed to be removed.</summary>
		RemoveAnchorFailed = 202,
	}
}

#endif // __MACOS__
