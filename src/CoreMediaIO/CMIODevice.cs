// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

#nullable enable

namespace CoreMediaIO {
	/// <summary>Controls Core Media I/O devices and their streams.</summary>
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

		[StructLayout (LayoutKind.Sequential, Pack = 4)]
		unsafe struct Command {
			public byte* CommandData;
			public uint CommandLength;
			public byte* ResponseData;
			public uint ResponseLength;
			public uint ResponseUsed;
		}

		[DllImport (Constants.CoreMediaIOLibrary)]
		static extern unsafe int CMIODeviceProcessAVCCommand (uint deviceId, ref Command command);

		/// <summary>Sends an AV/C command and receives its response, returning an OSStatus error code or zero on success.</summary>
		/// <param name="deviceId">The device to receive the command.</param>
		/// <param name="command">The command bytes.</param>
		/// <param name="response">Storage for the response bytes.</param>
		/// <param name="responseUsed">The number of response bytes written by the device.</param>
		public static unsafe int ProcessAvcCommand (uint deviceId, ReadOnlySpan<byte> command, Span<byte> response, out uint responseUsed)
		{
			fixed (byte* commandData = command)
			fixed (byte* responseData = response) {
				var nativeCommand = new Command {
					CommandData = commandData,
					CommandLength = (uint) command.Length,
					ResponseData = responseData,
					ResponseLength = (uint) response.Length,
				};
				var status = CMIODeviceProcessAVCCommand (deviceId, ref nativeCommand);
				responseUsed = nativeCommand.ResponseUsed;
				return status;
			}
		}

		[DllImport (Constants.CoreMediaIOLibrary)]
		static extern unsafe int CMIODeviceProcessRS422Command (uint deviceId, ref Command command);

		/// <summary>Sends an RS-422 command and receives its response, returning an OSStatus error code or zero on success.</summary>
		/// <param name="deviceId">The device to receive the command.</param>
		/// <param name="command">The command bytes.</param>
		/// <param name="response">Storage for the response bytes.</param>
		/// <param name="responseUsed">The number of response bytes written by the device.</param>
		public static unsafe int ProcessRS422Command (uint deviceId, ReadOnlySpan<byte> command, Span<byte> response, out uint responseUsed)
		{
			fixed (byte* commandData = command)
			fixed (byte* responseData = response) {
				var nativeCommand = new Command {
					CommandData = commandData,
					CommandLength = (uint) command.Length,
					ResponseData = responseData,
					ResponseLength = (uint) response.Length,
				};
				var status = CMIODeviceProcessRS422Command (deviceId, ref nativeCommand);
				responseUsed = nativeCommand.ResponseUsed;
				return status;
			}
		}
	}
}
