// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;

using ObjCRuntime;

namespace IOBluetoothUI {

	/// <summary>Options for the Bluetooth service browser, device selector and pairing controllers.</summary>
	[NoMacCatalyst]
	[Flags]
	public enum IOBluetoothServiceBrowserControllerOptions : uint {
		/// <summary>No options.</summary>
		None = 0,
		/// <summary>Automatically start an inquiry when the window is shown.</summary>
		AutoStartInquiry = 1 << 0,
		/// <summary>Disconnect from the device when done.</summary>
		DisconnectWhenDone = 1 << 1,
	}

	/// <summary>The type of the return key on a keyboard, used when displaying a passkey.</summary>
	[NoMacCatalyst]
	public enum BluetoothKeyboardReturnType {
		/// <summary>An ANSI return key.</summary>
		AnsiReturn = 0,
		/// <summary>An ISO return key.</summary>
		IsoReturn = 1,
		/// <summary>A JIS return key.</summary>
		JisReturn = 2,
		/// <summary>No return key.</summary>
		NoReturn = 3,
	}
}
