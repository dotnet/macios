// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

namespace IOBluetooth {

	/// <summary>A Bluetooth device address.</summary>
	[SupportedOSPlatform ("macos")]
	[UnsupportedOSPlatform ("maccatalyst")]
	[StructLayout (LayoutKind.Sequential)]
	public unsafe struct BluetoothDeviceAddress {
		/// <summary>The six bytes of the address.</summary>
		public fixed byte Data [6];

		/// <summary>Creates a device address from the specified six bytes.</summary>
		/// <param name="data">The six bytes of the address.</param>
		public BluetoothDeviceAddress (byte [] data)
		{
			if (data is null)
				throw new ArgumentNullException (nameof (data));
			if (data.Length != 6)
				throw new ArgumentOutOfRangeException (nameof (data), "The address must be six bytes long.");
			for (var i = 0; i < 6; i++)
				Data [i] = data [i];
		}

		/// <summary>Returns the bytes of the address.</summary>
		/// <returns>A new array with the six bytes of the address.</returns>
		public byte [] ToByteArray ()
		{
			var rv = new byte [6];
			for (var i = 0; i < 6; i++)
				rv [i] = Data [i];
			return rv;
		}
	}

	/// <summary>A Bluetooth PIN code.</summary>
	[SupportedOSPlatform ("macos")]
	[UnsupportedOSPlatform ("maccatalyst")]
	[StructLayout (LayoutKind.Sequential)]
	public unsafe struct BluetoothPINCode {
		/// <summary>The (up to) 16 bytes of the PIN code.</summary>
		public fixed byte Data [16];
	}

	/// <summary>The RSSI value for a connection.</summary>
	[SupportedOSPlatform ("macos")]
	[UnsupportedOSPlatform ("maccatalyst")]
	[StructLayout (LayoutKind.Sequential)]
	public struct BluetoothHCIRSSIInfo {
		/// <summary>The connection handle.</summary>
		public ushort Handle;
		/// <summary>The RSSI value, in the range -127 to +20.</summary>
		public sbyte RSSIValue;
	}

	/// <summary>The link quality for a connection.</summary>
	[SupportedOSPlatform ("macos")]
	[UnsupportedOSPlatform ("maccatalyst")]
	[StructLayout (LayoutKind.Sequential)]
	public struct BluetoothHCILinkQualityInfo {
		/// <summary>The connection handle.</summary>
		public ushort Handle;
		/// <summary>The link quality value.</summary>
		public byte QualityValue;
	}
}
