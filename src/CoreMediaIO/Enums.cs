//
// Enums.cs: enums for CoreMediaIO
//
// Authors:
//   GitHub Copilot
//

using System;
using ObjCRuntime;

#nullable enable

namespace CoreMediaIO {

	/// <summary>Specifies the direction of a CoreMediaIO extension stream.</summary>
	[NoiOS, NoTV, Mac (12, 3), MacCatalyst (15, 4)]
	[Native]
	public enum CMIOExtensionStreamDirection : long {
		Source = 0,
		Sink = 1,
	}

	/// <summary>Specifies the clock type for a CoreMediaIO extension stream.</summary>
	[NoiOS, NoTV, Mac (12, 3), MacCatalyst (15, 4)]
	[Native]
	public enum CMIOExtensionStreamClockType : long {
		HostTime = 0,
		LinkedCoreAudioDeviceUid = 1,
		Custom = 2,
	}

	/// <summary>Flags indicating the type of discontinuity in a CoreMediaIO extension stream.</summary>
	[NoiOS, NoTV, Mac (12, 3), MacCatalyst (15, 4)]
	[Flags]
	public enum CMIOExtensionStreamDiscontinuityFlags : uint {
		None = 0,
		Unknown = (1 << 0),
		Time = (1 << 1),
		SampleDropped = (1 << 6),
	}
}
