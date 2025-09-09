//
// GCGamepadSnapshot.cs: extensions to GCGamepadSnapshot iOS API
//
// Authors:
//   Aaron Bockover (abock@xamarin.com)
//
// Copyright 2013-2014 Xamarin Inc.

#nullable enable

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using ObjCRuntime;
using Foundation;

namespace GameController {
	// GCGamepadSnapshot.h
	// float_t are 4 bytes (at least for ARM64)
	/// <summary>The state of a <see cref="GameController.GCGamepad" />. Produced by <see cref="GameController.GCGamepadSnapshot.TryGetSnapshotData(Foundation.NSData,out GameController.GCGamepadSnapShotDataV100)" />.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("tvos")]
	[ObsoletedOSPlatform ("macos10.15", "Use 'GCExtendedGamepad' instead.")]
	[ObsoletedOSPlatform ("tvos13.0", "Use 'GCExtendedGamepad' instead.")]
	[ObsoletedOSPlatform ("ios13.0", "Use 'GCExtendedGamepad' instead.")]
	[StructLayout (LayoutKind.Sequential, Pack = 1)]
	public struct GCGamepadSnapShotDataV100 {

		// Standard information
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public ushort /* uint16_t */ Version; // 0x0100
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public ushort /* uint16_t */ Size;    // sizeof(GCGamepadSnapShotDataV100) or larger

		// Standard gamepad data
		// Axes in the range [-1.0, 1.0]
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public float /* float_t = float */ DPadX;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public float /* float_t = float */ DPadY;

		// Buttons in the range [0.0, 1.0]
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public float /* float_t = float */ ButtonA;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public float /* float_t = float */ ButtonB;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public float /* float_t = float */ ButtonX;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public float /* float_t = float */ ButtonY;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public float /* float_t = float */ LeftShoulder;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public float /* float_t = float */ RightShoulder;

		[DllImport (Constants.GameControllerLibrary)]
		unsafe static extern /* NSData * __nullable */ IntPtr NSDataFromGCGamepadSnapShotDataV100 (
			/* GCGamepadSnapShotDataV100 * __nullable */ GCGamepadSnapShotDataV100* snapshotData);

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public NSData? ToNSData ()
		{
			unsafe {
				fixed (GCGamepadSnapShotDataV100* self = &this) {
					var p = NSDataFromGCGamepadSnapShotDataV100 (self);
					return p == IntPtr.Zero ? null : new NSData (p);
				}
			}
		}
	}

	public partial class GCGamepadSnapshot {

		// GCGamepadSnapshot.h
		[DllImport (Constants.GameControllerLibrary)]
		unsafe static extern byte GCGamepadSnapShotDataV100FromNSData (
			/* GCGamepadSnapShotDataV100 * __nullable */ GCGamepadSnapShotDataV100* snapshotData,
			/* NSData * __nullable */ IntPtr data);

		/// <param name="data">To be added.</param>
		///         <param name="snapshotData">To be added.</param>
		///         <summary>Attempts to map the data into <paramref name="snapshotData" />.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static bool TryGetSnapshotData (NSData? data, out GCGamepadSnapShotDataV100 snapshotData)
		{
			snapshotData = default;
			unsafe {
				bool result = GCGamepadSnapShotDataV100FromNSData ((GCGamepadSnapShotDataV100*) Unsafe.AsPointer<GCGamepadSnapShotDataV100> (ref snapshotData), data.GetHandle ()) != 0;
				GC.KeepAlive (data);
				return result;
			}
		}
	}
}
