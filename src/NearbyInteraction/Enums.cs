//
// NearbyInteraction enums
//
// Authors:
//	Whitney Schmidt  <whschm@microsoft.com>
//
// Copyright 2020 Microsoft Inc.
//

namespace NearbyInteraction {

	[NoTV, iOS (14, 0)]
	[MacCatalyst (14, 0)]
#if !__MACOS__
	[ErrorDomain ("NIErrorDomain")]
#endif
	[Native]
	public enum NIErrorCode : long {
		UnsupportedPlatform = -5889,
		InvalidConfiguration = -5888,
		SessionFailed = -5887,
		ResourceUsageTimeout = -5886,
		ActiveSessionsLimitExceeded = -5885,
		UserDidNotAllow = -5884,
		AccessoryPeerDeviceUnavailable = -5882,
		InvalidARConfiguration = -5883,
		IncompatiblePeerDevice = -5881,
		ActiveExtendedDistanceSessionsLimitExceeded = -5880,
	}

	[NoTV, iOS (14, 0)]
	[MacCatalyst (14, 0)]
	[Native]
	public enum NINearbyObjectRemovalReason : long {
		Timeout,
		PeerEnded,
	}

	[iOS (16, 0), NoTV, MacCatalyst (16, 0)]
	[Native]
	public enum NIAlgorithmConvergenceStatus : long {
		Unknown,
		NotConverged,
		Converged,
	}

	[iOS (16, 0), NoTV, MacCatalyst (16, 0)]
	[Native]
	public enum NINearbyObjectVerticalDirectionEstimate : long {
		Unknown = 0,
		Same = 1,
		Above = 2,
		Below = 3,
		AboveOrBelow = 4,
	}

	[NoTV, NoMac, iOS (15, 0), MacCatalyst (15, 0)]
	[Native]
	public enum NIMotionActivityState : long {
		Unknown = 0,
		Stationary = 1,
		Moving = 2,
	}
}
