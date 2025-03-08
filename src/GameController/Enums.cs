//
// Enums.cs
//
// Authors:
//   Chris Hamons (chris.hamons@xamarin.com)
//   Whitney Schmidt (whschm@microsoft.com)
//
// Copyright 2019, 2020 Microsoft Corporation

using System;

using ObjCRuntime;
using Foundation;

namespace GameController {
	[Deprecated (PlatformName.MacOSX, 10, 15, message: "Use 'GCController.GetMicroGamepadController()' instead.")]
	[Deprecated (PlatformName.iOS, 13, 0, message: "Use 'GCController.GetMicroGamepadController()' instead.")]
	[Deprecated (PlatformName.TvOS, 13, 0, message: "Use 'GCController.GetMicroGamepadController()' instead.")]
	[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'GCController.GetMicroGamepadController()' instead.")]
	[Native]
	public enum GCExtendedGamepadSnapshotDataVersion : long {
		/// <summary>To be added.</summary>
		Version1 = 0x0100,
		/// <summary>To be added.</summary>
		Version2 = 0x0101,
	}

	[Native]
	public enum GCMicroGamepadSnapshotDataVersion : long {
		/// <summary>To be added.</summary>
		Version1 = 0x0100,
	}

	[TV (14, 0), iOS (14, 0)]
	[MacCatalyst (14, 0)]
	[Native]
	public enum GCTouchState : long {
		Up,
		Down,
		Moving,
	}

	[TV (14, 0), iOS (14, 0)]
	[MacCatalyst (14, 0)]
	[Native]
	public enum GCDeviceBatteryState : long {
		Unknown = -1,
		Discharging,
		Charging,
		Full,
	}

	[TV (14, 0), iOS (14, 0)]
	[MacCatalyst (14, 0)]
	[Native]
	public enum GCSystemGestureState : long {
		Enabled = 0,
		AlwaysReceive,
		Disabled,
	}

	/// <summary>Enumerates player controller indices.</summary>
	[MacCatalyst (13, 1)]
	[Native]
	public enum GCControllerPlayerIndex : long {
		/// <summary>The index that represents the condition where the player index is not set. (<c>-1</c>)</summary>
		Unset = -1,
		/// <summary>The index for the first player. (<c>0</c>)</summary>
		Index1 = 0,
		/// <summary>The index for the second player. (<c>1</c>)</summary>
		Index2,
		/// <summary>The index for the third player. (<c>2</c>)</summary>
		Index3,
		/// <summary>The index for the fourth player. (<c>3</c>)</summary>
		Index4,
	}

	[iOS (16, 0), Mac (13, 0), TV (16, 0), MacCatalyst (16, 0)]
	[Native]
	public enum GCDevicePhysicalInputElementChange : long {
		UnknownChange = -1,
		NoChange = 0,
		Changed = 1,
	}
}
