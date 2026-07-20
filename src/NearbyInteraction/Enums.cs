//
// NearbyInteraction enums
//
// Authors:
//	Whitney Schmidt  <whschm@microsoft.com>
//
// Copyright 2020 Microsoft Inc.
//

using System.ComponentModel;

namespace NearbyInteraction {

#if XAMCORE_5_0
	[NoTV, NoMac, iOS (14, 0)]
#else
	[NoTV, Mac (12, 0), iOS (14, 0)]
#endif
	[MacCatalyst (14, 0)]
#if !__MACOS__
	[ErrorDomain ("NIErrorDomain")]
#endif
	[Native]
#if __MACOS__ && !XAMCORE_5_0
	[Obsolete ("Not available on this platform.")]
	[EditorBrowsable (EditorBrowsableState.Never)]
#endif
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

#if XAMCORE_5_0
	[NoTV, NoMac, iOS (14, 0)]
#else
	[NoTV, Mac (13, 0), iOS (14, 0)]
#endif
	[MacCatalyst (14, 0)]
	[Native]
#if __MACOS__ && !XAMCORE_5_0
	[Obsolete ("Not available on this platform.")]
	[EditorBrowsable (EditorBrowsableState.Never)]
#endif
	public enum NINearbyObjectRemovalReason : long {
		Timeout,
		PeerEnded,
	}

#if XAMCORE_5_0
	[iOS (16, 0), NoMac, NoTV, MacCatalyst (16, 0)]
#else
	[iOS (16, 0), Mac (13, 0), NoTV, MacCatalyst (16, 0)]
#endif
	[Native]
#if __MACOS__ && !XAMCORE_5_0
	[Obsolete ("Not available on this platform.")]
	[EditorBrowsable (EditorBrowsableState.Never)]
#endif
	public enum NIAlgorithmConvergenceStatus : long {
		Unknown,
		NotConverged,
		Converged,
	}

#if XAMCORE_5_0
	[iOS (16, 0), NoMac, NoTV, MacCatalyst (16, 0)]
#else
	[iOS (16, 0), Mac (13, 0), NoTV, MacCatalyst (16, 0)]
#endif
	[Native]
#if __MACOS__ && !XAMCORE_5_0
	[Obsolete ("Not available on this platform.")]
	[EditorBrowsable (EditorBrowsableState.Never)]
#endif
	public enum NINearbyObjectVerticalDirectionEstimate : long {
		Unknown = 0,
		Same = 1,
		Above = 2,
		Below = 3,
		AboveOrBelow = 4,
	}
}
