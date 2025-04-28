#if !TVOS
//
// MidiBluetoothDriver.cs
//
// Authors: TJ Lambert (TJ.Lambert@microsoft.com)
//
// Copyright 2022 Microsoft Corp.
//

#nullable enable

using System;
using System.Runtime.InteropServices;

using ObjCRuntime;
using CoreFoundation;
using Foundation;

namespace CoreMidi {
	[SupportedOSPlatform ("ios16.0")]
	[SupportedOSPlatform ("maccatalyst16.0")]
	[SupportedOSPlatform ("tvos16.0")]
	[SupportedOSPlatform ("macos13.0")]
	public partial class MidiBluetoothDriver {
		[DllImport (Constants.CoreMidiLibrary)]
		static extern int MIDIBluetoothDriverActivateAllConnections ();

		public static int ActivateAllConnections () => MIDIBluetoothDriverActivateAllConnections ();

		[DllImport (Constants.CoreMidiLibrary)]
		static extern unsafe int MIDIBluetoothDriverDisconnect (/* CFStringRef* */ NativeHandle uuid);

		public static int Disconnect (NSString uuid)
		{
			int result = MIDIBluetoothDriverDisconnect (uuid.GetHandle ());
			GC.KeepAlive (uuid);
			return result;
		}
	}
}
#endif
