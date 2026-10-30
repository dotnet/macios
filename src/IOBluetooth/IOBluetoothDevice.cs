// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

#nullable enable

using System;

using Foundation;
using ObjCRuntime;

namespace IOBluetooth {

	// The channel returned in the 'out' parameter is already retained by the
	// native code, so ownership must be transferred to the managed wrapper.
	public partial class IOBluetoothDevice {
		/// <summary>Opens an L2CAP channel to the device and waits for it to open.</summary>
		/// <param name="newChannel">The new channel, if successful.</param>
		/// <param name="psm">The PSM of the channel to open.</param>
		/// <param name="channelDelegate">The delegate for the new channel.</param>
		/// <returns>An IOReturn status code (0 for success).</returns>
		public int OpenL2CapChannelSync (out IOBluetoothL2CAPChannel? newChannel, ushort psm, NSObject? channelDelegate)
		{
			var rv = _OpenL2CapChannelSync (out var handle, psm, channelDelegate);
			newChannel = Runtime.GetNSObject<IOBluetoothL2CAPChannel> (handle, true);
			return rv;
		}

		/// <summary>Opens an L2CAP channel to the device without waiting for it to open.</summary>
		/// <param name="newChannel">The new channel, if successful.</param>
		/// <param name="psm">The PSM of the channel to open.</param>
		/// <param name="channelDelegate">The delegate for the new channel.</param>
		/// <returns>An IOReturn status code (0 for success).</returns>
		public int OpenL2CapChannelAsync (out IOBluetoothL2CAPChannel? newChannel, ushort psm, NSObject? channelDelegate)
		{
			var rv = _OpenL2CapChannelAsync (out var handle, psm, channelDelegate);
			newChannel = Runtime.GetNSObject<IOBluetoothL2CAPChannel> (handle, true);
			return rv;
		}

		/// <summary>Opens an L2CAP channel to the device with the specified configuration and waits for it to open.</summary>
		/// <param name="newChannel">The new channel, if successful.</param>
		/// <param name="psm">The PSM of the channel to open.</param>
		/// <param name="channelConfiguration">The configuration of the channel.</param>
		/// <param name="channelDelegate">The delegate for the new channel.</param>
		/// <returns>An IOReturn status code (0 for success).</returns>
		public int OpenL2CapChannelSync (out IOBluetoothL2CAPChannel? newChannel, ushort psm, NSDictionary? channelConfiguration, NSObject? channelDelegate)
		{
			var rv = _OpenL2CapChannelSyncWithConfiguration (out var handle, psm, channelConfiguration, channelDelegate);
			newChannel = Runtime.GetNSObject<IOBluetoothL2CAPChannel> (handle, true);
			return rv;
		}

		/// <summary>Opens an L2CAP channel to the device with the specified configuration without waiting for it to open.</summary>
		/// <param name="newChannel">The new channel, if successful.</param>
		/// <param name="psm">The PSM of the channel to open.</param>
		/// <param name="channelConfiguration">The configuration of the channel.</param>
		/// <param name="channelDelegate">The delegate for the new channel.</param>
		/// <returns>An IOReturn status code (0 for success).</returns>
		public int OpenL2CapChannelAsync (out IOBluetoothL2CAPChannel? newChannel, ushort psm, NSDictionary? channelConfiguration, NSObject? channelDelegate)
		{
			var rv = _OpenL2CapChannelAsyncWithConfiguration (out var handle, psm, channelConfiguration, channelDelegate);
			newChannel = Runtime.GetNSObject<IOBluetoothL2CAPChannel> (handle, true);
			return rv;
		}

		/// <summary>Opens an RFCOMM channel to the device and waits for it to open.</summary>
		/// <param name="rfcommChannel">The new channel, if successful.</param>
		/// <param name="channelId">The RFCOMM channel ID to open.</param>
		/// <param name="channelDelegate">The delegate for the new channel.</param>
		/// <returns>An IOReturn status code (0 for success).</returns>
		public int OpenRfcommChannelSync (out IOBluetoothRFCOMMChannel? rfcommChannel, byte channelId, NSObject? channelDelegate)
		{
			var rv = _OpenRfcommChannelSync (out var handle, channelId, channelDelegate);
			rfcommChannel = Runtime.GetNSObject<IOBluetoothRFCOMMChannel> (handle, true);
			return rv;
		}

		/// <summary>Opens an RFCOMM channel to the device without waiting for it to open.</summary>
		/// <param name="rfcommChannel">The new channel, if successful.</param>
		/// <param name="channelId">The RFCOMM channel ID to open.</param>
		/// <param name="channelDelegate">The delegate for the new channel.</param>
		/// <returns>An IOReturn status code (0 for success).</returns>
		public int OpenRfcommChannelAsync (out IOBluetoothRFCOMMChannel? rfcommChannel, byte channelId, NSObject? channelDelegate)
		{
			var rv = _OpenRfcommChannelAsync (out var handle, channelId, channelDelegate);
			rfcommChannel = Runtime.GetNSObject<IOBluetoothRFCOMMChannel> (handle, true);
			return rv;
		}
	}
}
