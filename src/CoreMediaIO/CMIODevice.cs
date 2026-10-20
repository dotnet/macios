// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

#nullable enable

namespace CoreMediaIO {
	/// <summary>Starts and stops Core Media I/O device streams.</summary>
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("maccatalyst")]
	[UnsupportedOSPlatform ("ios")]
	[UnsupportedOSPlatform ("tvos")]
	public static class CMIODevice {
		[DllImport (Constants.CoreMediaIOLibrary)]
		static extern int CMIODeviceStartStream (uint deviceId, uint streamId);

		/// <summary>Starts a device's stream, returning an OSStatus error code or zero on success.</summary>
		public static int StartStream (uint deviceId, uint streamId) => CMIODeviceStartStream (deviceId, streamId);

		[DllImport (Constants.CoreMediaIOLibrary)]
		static extern int CMIODeviceStopStream (uint deviceId, uint streamId);

		/// <summary>Stops a device's stream, returning an OSStatus error code or zero on success.</summary>
		public static int StopStream (uint deviceId, uint streamId) => CMIODeviceStopStream (deviceId, streamId);
	}
}
