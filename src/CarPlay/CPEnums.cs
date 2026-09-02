//
// CPEnums.cs
//
// Authors:
//	Whitney Schmidt <whschm@microsoft.com>
//
// Copyright (c) Microsoft Corporation.
// Licensed under MIT License.
//

#nullable enable

namespace CarPlay {

	[NoTV, NoMac, iOS (14, 0)]
	public enum CPMessageListItemType {
		Identifier,
		FullName,
	}

	[NoTV, NoMac, iOS (26, 0), MacCatalyst (26, 0)]
	[Native]
	public enum CPListImageRowItemCondensedElementShape : long {
		RoundedRect = 0,
		Circle = 1,
	}

	[NoTV, NoMac, iOS (26, 0), MacCatalyst (26, 0)]
	[Native]
	public enum CPListImageRowItemImageGridElementShape : long {
		RoundedRect = 0,
		Circle = 1,
	}

	[NoTV, NoMac, iOS (26, 4), MacCatalyst (26, 4)]
	[Native]
	public enum CPRerouteReason : long {
		Unknown = 0,
		MissedTurn,
		Offline,
		AlternateRoute,
		WaypointModified,
		Mandated,
	}

	[NoTV, NoMac, iOS (26, 4), MacCatalyst (26, 4)]
	[Native]
	public enum CPRouteSource : ulong {
		Inactive = 0,
		IOSUnchanged = 1,
		IOSRouteModified = 2,
		IOSRouteDestinationsModified = 3,
		IOSDestinationsOnly = 4,
		Vehicle = 5,
	}

	[NoTV, NoMac, iOS (26, 4), MacCatalyst (26, 4)]
	[Native]
	public enum CPImageOverlayAlignment : long {
		Leading,
		Center,
		Trailing,
	}

	[NoTV, NoMac, iOS (26, 4), MacCatalyst (26, 4)]
	[Native]
	public enum CPPlaybackPresentation : long {
		None = 0,
		Audio,
		Video,
	}

	[NoTV, NoMac, iOS (26, 4), MacCatalyst (26, 4)]
	[Native]
	public enum CPPlaybackAction : long {
		None = 0,
		Play,
		Pause,
		Replay,
	}
}
