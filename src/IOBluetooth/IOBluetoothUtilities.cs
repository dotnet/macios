// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

#nullable enable

using System;
using System.Runtime.InteropServices;

using Foundation;
using ObjCRuntime;

namespace IOBluetooth {

	/// <summary>Utility functions for working with Bluetooth addresses and HID devices.</summary>
	[NoMacCatalyst]
	public static class IOBluetoothUtilities {
		[DllImport (Constants.IOBluetoothLibrary)]
		static extern int IOBluetoothNSStringToDeviceAddress (IntPtr nameString, ref BluetoothDeviceAddress outDeviceAddress);

		[DllImport (Constants.IOBluetoothLibrary)]
		static extern IntPtr IOBluetoothNSStringFromDeviceAddress (ref BluetoothDeviceAddress deviceAddress);

		[DllImport (Constants.IOBluetoothLibrary)]
		static extern IntPtr IOBluetoothNSStringFromDeviceAddressColon (ref BluetoothDeviceAddress deviceAddress);

		[DllImport (Constants.IOBluetoothLibrary)]
		static extern nint IOBluetoothNumberOfAvailableHIDDevices ();

		[DllImport (Constants.IOBluetoothLibrary)]
		static extern nint IOBluetoothNumberOfPointingHIDDevices ();

		[DllImport (Constants.IOBluetoothLibrary)]
		static extern nint IOBluetoothNumberOfKeyboardHIDDevices ();

		[DllImport (Constants.IOBluetoothLibrary)]
		static extern nint IOBluetoothNumberOfTabletHIDDevices ();

		/// <summary>Parses a Bluetooth device address from a string.</summary>
		/// <param name="address">The string to parse (for instance "00-11-22-33-44-55").</param>
		/// <param name="deviceAddress">The parsed address, if successful.</param>
		/// <returns>True if the string could be parsed, false otherwise.</returns>
		public static bool TryParseDeviceAddress (string address, out BluetoothDeviceAddress deviceAddress)
		{
			if (address is null)
				throw new ArgumentNullException (nameof (address));

			deviceAddress = default;
			var nsAddress = NSString.CreateNative (address);
			try {
				return IOBluetoothNSStringToDeviceAddress (nsAddress, ref deviceAddress) == 0;
			} finally {
				NSString.ReleaseNative (nsAddress);
			}
		}

		/// <summary>Formats a Bluetooth device address using dashes as separators.</summary>
		/// <param name="deviceAddress">The address to format.</param>
		/// <returns>The formatted address.</returns>
		public static string? ToDashSeparatedString (BluetoothDeviceAddress deviceAddress)
			=> CFString.FromHandle (IOBluetoothNSStringFromDeviceAddress (ref deviceAddress));

		/// <summary>Formats a Bluetooth device address using colons as separators.</summary>
		/// <param name="deviceAddress">The address to format.</param>
		/// <returns>The formatted address.</returns>
		public static string? ToColonSeparatedString (BluetoothDeviceAddress deviceAddress)
			=> CFString.FromHandle (IOBluetoothNSStringFromDeviceAddressColon (ref deviceAddress));

		/// <summary>Gets the number of available HID devices.</summary>
		public static nint NumberOfAvailableHidDevices => IOBluetoothNumberOfAvailableHIDDevices ();

		/// <summary>Gets the number of available pointing HID devices.</summary>
		public static nint NumberOfPointingHidDevices => IOBluetoothNumberOfPointingHIDDevices ();

		/// <summary>Gets the number of available keyboard HID devices.</summary>
		public static nint NumberOfKeyboardHidDevices => IOBluetoothNumberOfKeyboardHIDDevices ();

		/// <summary>Gets the number of available tablet HID devices.</summary>
		public static nint NumberOfTabletHidDevices => IOBluetoothNumberOfTabletHIDDevices ();
	}
}
