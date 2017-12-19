﻿// Copyright 2015 Xamarin Inc.

#if IOS || TVOS

using System;
using System.Runtime.InteropServices;

using XamCore.ObjCRuntime;
using XamCore.Foundation;

namespace XamCore.GameController {

	[iOS (10,0)][TV (9,0)]
	// GCMicroGamepadSnapshot.h
	// float_t are 4 bytes (at least for ARM64)
	[StructLayout (LayoutKind.Sequential, Pack = 1)]
	public struct GCMicroGamepadSnapShotDataV100 {

		// Standard information
		public ushort /* uint16_t */ Version; // 0x0100
		public ushort /* uint16_t */ Size;    // sizeof(GCMicroGamepadSnapShotDataV100) or larger

		// Standard gamepad data
		// Axes in the range [-1.0, 1.0]
		public float /* float_t = float */ DPadX;
		public float /* float_t = float */ DPadY;

		// Buttons in the range [0.0, 1.0]
		public float /* float_t = float */ ButtonA;
		public float /* float_t = float */ ButtonX;

		[DllImport (Constants.GameControllerLibrary)]
		static extern /* NSData * __nullable */ IntPtr NSDataFromGCMicroGamepadSnapShotDataV100 (
			/* __nullable */ ref GCMicroGamepadSnapShotDataV100 snapshotData);

		public NSData ToNSData ()
		{
			var p = NSDataFromGCMicroGamepadSnapShotDataV100 (ref this);
			return p == IntPtr.Zero ? null : new NSData (p);
		}
	}

	public partial class GCMicroGamepadSnapshot {

		// GCGamepadSnapshot.h
		[DllImport (Constants.GameControllerLibrary)]
		static extern bool GCMicroGamepadSnapShotDataV100FromNSData (out GCMicroGamepadSnapShotDataV100 snapshotData, /* NSData */ IntPtr data);

		public static bool TryGetSnapshotData (NSData data, out GCMicroGamepadSnapShotDataV100 snapshotData)
		{
			return GCMicroGamepadSnapShotDataV100FromNSData (out snapshotData, data == null ? IntPtr.Zero : data.Handle);
		}
	}
}

#endif // IOS || TVOS
