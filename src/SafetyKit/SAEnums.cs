//
// SafetyKit enumerations
//
// Author:
//   Israel Soto (issoto@microsoft.com)
//   Rolf Bjarne Kvinge (rolf@xamarin.com)
//
// Copyright 2022, 2024 Microsoft Corporation.
//

#nullable enable

namespace SafetyKit {

	[iOS (16, 0), MacCatalyst (16, 0), NoTV]
	[Native]
	public enum SAAuthorizationStatus : long {
		NotDetermined = 0,
		Denied,
		Authorized,
	}

	[iOS (16, 0), MacCatalyst (16, 0), NoTV]
	[Native]
	public enum SACrashDetectionEventResponse : long {
		Attempted,
		Disabled,
	}

	[iOS (16, 0), MacCatalyst (16, 0), NoTV]
	[Native]
	public enum SAEmergencyResponseManagerVoiceCallStatus : long {
		Dialing,
		Active,
		Disconnected,
		Failed,
	}

	[iOS (16, 0), MacCatalyst (16, 0), NoTV]
	[ErrorDomain ("SAErrorDomain")]
	[Native]
	public enum SAErrorCode : long {
		NotAuthorized = 1,
		NotAllowed,
		InvalidArgument,
		OperationFailed,
	}
}
