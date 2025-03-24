// Copyright 2015 Xamarin Inc.

#nullable enable

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using ObjCRuntime;
using Foundation;

namespace GameController {

	// GCMicroGamepadSnapshot.h
	// float_t are 4 bytes (at least for ARM64)
#if NET
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("tvos")]
	[ObsoletedOSPlatform ("macos10.14.4", "Use 'GCController.GetMicroGamepadController()' instead.")]
	[ObsoletedOSPlatform ("tvos12.2", "Use 'GCController.GetMicroGamepadController()' instead.")]
	[ObsoletedOSPlatform ("ios12.2", "Use 'GCController.GetMicroGamepadController()' instead.")]
#else
	[Deprecated (PlatformName.iOS, 12, 2, message: "Use 'GCController.GetMicroGamepadController()' instead.")]
	[Deprecated (PlatformName.MacOSX, 10, 14, 4, message: "Use 'GCController.GetMicroGamepadController()' instead.")]
	[Deprecated (PlatformName.TvOS, 12, 2, message: "Use 'GCController.GetMicroGamepadController()' instead.")]
#endif
	[StructLayout (LayoutKind.Sequential, Pack = 1)]
	public struct GCMicroGamepadSnapShotDataV100 {

		// Standard information
		/// <summary>The micro gamepad version.</summary>
		///         <remarks>To be added.</remarks>
		public ushort /* uint16_t */ Version; // 0x0100
		/// <summary>The size of the snapshot, in bytes.</summary>
		///         <remarks>To be added.</remarks>
		public ushort /* uint16_t */ Size;    // sizeof(GCMicroGamepadSnapShotDataV100) or larger

		// Standard gamepad data
		// Axes in the range [-1.0, 1.0]
		/// <summary>The <c>X</c>v alue of the D-pad input when the snapshot was taken.</summary>
		///         <remarks>To be added.</remarks>
		public float /* float_t = float */ DPadX;
		/// <summary>The <c>Y</c> value of the D-pad input when the snapshot was taken.</summary>
		///         <remarks>To be added.</remarks>
		public float /* float_t = float */ DPadY;

		// Buttons in the range [0.0, 1.0]
		/// <summary>The value of ButtonA when the snapshot was taken.</summary>
		///         <remarks>To be added.</remarks>
		public float /* float_t = float */ ButtonA;
		/// <summary>The value of ButtonX when the snapshot was taken.</summary>
		///         <remarks>To be added.</remarks>
		public float /* float_t = float */ ButtonX;

#if NET
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		[ObsoletedOSPlatform ("macos10.15", "Use 'GCController.GetMicroGamepadController()' instead.")]
		[ObsoletedOSPlatform ("tvos13.0", "Use 'GCController.GetMicroGamepadController()' instead.")]
		[ObsoletedOSPlatform ("ios13.0", "Use 'GCController.GetMicroGamepadController()' instead.")]
#else
		[Deprecated (PlatformName.MacOSX, 10, 15, message: "Use 'GCController.GetMicroGamepadController()' instead.")]
		[Deprecated (PlatformName.iOS, 13, 0, message: "Use 'GCController.GetMicroGamepadController()' instead.")]
		[Deprecated (PlatformName.TvOS, 13, 0, message: "Use 'GCController.GetMicroGamepadController()' instead.")]
#endif
		[DllImport (Constants.GameControllerLibrary)]
		unsafe static extern /* NSData * __nullable */ IntPtr NSDataFromGCMicroGamepadSnapShotDataV100 (
			/* __nullable */ GCMicroGamepadSnapShotDataV100* snapshotData);

		public NSData? ToNSData ()
		{
			unsafe {
				fixed (GCMicroGamepadSnapShotDataV100* self = &this) {
					var p = NSDataFromGCMicroGamepadSnapShotDataV100 (self);
					return p == IntPtr.Zero ? null : new NSData (p);
				}
			}
		}
	}

	// GCMicroGamepadSnapshot.h
	// float_t are 4 bytes (at least for ARM64)
#if NET
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	[ObsoletedOSPlatform ("macos10.15", "Use 'GCController.GetMicroGamepadController()' instead.")]
	[ObsoletedOSPlatform ("tvos13.0", "Use 'GCController.GetMicroGamepadController()' instead.")]
	[ObsoletedOSPlatform ("ios13.0", "Use 'GCController.GetMicroGamepadController()' instead.")]
#else
	[Deprecated (PlatformName.MacOSX, 10, 15, message: "Use 'GCController.GetMicroGamepadController()' instead.")]
	[Deprecated (PlatformName.iOS, 13, 0, message: "Use 'GCController.GetMicroGamepadController()' instead.")]
	[Deprecated (PlatformName.TvOS, 13, 0, message: "Use 'GCController.GetMicroGamepadController()' instead.")]
#endif
	[StructLayout (LayoutKind.Sequential, Pack = 1)]
	public struct GCMicroGamepadSnapshotData {

		// Standard information
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public ushort /* uint16_t */ Version;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public ushort /* uint16_t */ Size;

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
		public float /* float_t = float */ ButtonX;

#if NET
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[ObsoletedOSPlatform ("macos10.15", "Use 'GCController.GetMicroGamepadController()' instead.")]
		[ObsoletedOSPlatform ("tvos13.0", "Use 'GCController.GetMicroGamepadController()' instead.")]
		[ObsoletedOSPlatform ("ios13.0", "Use 'GCController.GetMicroGamepadController()' instead.")]
#endif
		[DllImport (Constants.GameControllerLibrary)]
		unsafe static extern /* NSData * __nullable */ IntPtr NSDataFromGCMicroGamepadSnapshotData (
			/* __nullable */ GCMicroGamepadSnapshotData* snapshotData);

#if NET
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[ObsoletedOSPlatform ("macos10.15", "Use 'GCController.GetMicroGamepadController()' instead.")]
		[ObsoletedOSPlatform ("tvos13.0", "Use 'GCController.GetMicroGamepadController()' instead.")]
		[ObsoletedOSPlatform ("ios13.0", "Use 'GCController.GetMicroGamepadController()' instead.")]
#endif
		public NSData? ToNSData ()
		{
			unsafe {
				fixed (GCMicroGamepadSnapshotData* self = &this) {
					var p = NSDataFromGCMicroGamepadSnapshotData (self);
					return p == IntPtr.Zero ? null : new NSData (p);
				}
			}
		}
	}

	public partial class GCMicroGamepadSnapshot {

		// GCGamepadSnapshot.h
#if NET
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("maccatalyst")]
		[ObsoletedOSPlatform ("macos10.15", "Use 'GCController.GetMicroGamepadController()' instead.")]
		[ObsoletedOSPlatform ("tvos13.0", "Use 'GCController.GetMicroGamepadController()' instead.")]
		[ObsoletedOSPlatform ("ios13.0", "Use 'GCController.GetMicroGamepadController()' instead.")]
#else
		[Deprecated (PlatformName.MacOSX, 10, 15, message: "Use 'GCController.GetMicroGamepadController()' instead.")]
		[Deprecated (PlatformName.iOS, 13, 0, message: "Use 'GCController.GetMicroGamepadController()' instead.")]
		[Deprecated (PlatformName.TvOS, 13, 0, message: "Use 'GCController.GetMicroGamepadController()' instead.")]
#endif
		[DllImport (Constants.GameControllerLibrary)]
		unsafe static extern byte GCMicroGamepadSnapShotDataV100FromNSData (GCMicroGamepadSnapShotDataV100* snapshotData, /* NSData */ IntPtr data);

#if NET
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("maccatalyst")]
		[ObsoletedOSPlatform ("macos10.14.4", "Use 'TryGetSnapshotData (NSData, out GCMicroGamepadSnapshotData)' instead.")]
		[ObsoletedOSPlatform ("tvos12.2", "Use 'TryGetSnapshotData (NSData, out GCMicroGamepadSnapshotData)' instead.")]
		[ObsoletedOSPlatform ("ios12.2", "Use 'TryGetSnapshotData (NSData, out GCMicroGamepadSnapshotData)' instead.")]
#else
		[Deprecated (PlatformName.iOS, 12, 2, message: "Use 'TryGetSnapshotData (NSData, out GCMicroGamepadSnapshotData)' instead.")]
		[Deprecated (PlatformName.MacOSX, 10, 14, 4, message: "Use 'TryGetSnapshotData (NSData, out GCMicroGamepadSnapshotData)' instead.")]
		[Deprecated (PlatformName.TvOS, 12, 2, message: "Use 'TryGetSnapshotData (NSData, out GCMicroGamepadSnapshotData)' instead.")]
#endif
		public static bool TryGetSnapshotData (NSData? data, out GCMicroGamepadSnapShotDataV100 snapshotData)
		{
			snapshotData = default;
			unsafe {
				bool result = GCMicroGamepadSnapShotDataV100FromNSData ((GCMicroGamepadSnapShotDataV100*) Unsafe.AsPointer<GCMicroGamepadSnapShotDataV100> (ref snapshotData), data.GetHandle ()) != 0;
				GC.KeepAlive (data);
				return result;
			}
		}

#if NET
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[ObsoletedOSPlatform ("macos10.15", "Use 'GCController.GetMicroGamepadController()' instead.")]
		[ObsoletedOSPlatform ("tvos13.0", "Use 'GCController.GetMicroGamepadController()' instead.")]
		[ObsoletedOSPlatform ("ios13.0", "Use 'GCController.GetMicroGamepadController()' instead.")]
#else
		[Deprecated (PlatformName.MacOSX, 10, 15, message: "Use 'GCController.GetMicroGamepadController()' instead.")]
		[Deprecated (PlatformName.iOS, 13, 0, message: "Use 'GCController.GetMicroGamepadController()' instead.")]
		[Deprecated (PlatformName.TvOS, 13, 0, message: "Use 'GCController.GetMicroGamepadController()' instead.")]
#endif
		[DllImport (Constants.GameControllerLibrary)]
		unsafe static extern byte GCMicroGamepadSnapshotDataFromNSData (GCMicroGamepadSnapshotData* snapshotData, /* NSData */ IntPtr data);

#if NET
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[ObsoletedOSPlatform ("macos10.15", "Use 'GCController.Capture()' instead.")]
		[ObsoletedOSPlatform ("tvos13.0", "Use 'GCController.Capture()' instead.")]
		[ObsoletedOSPlatform ("ios13.0", "Use 'GCController.Capture()' instead.")]
#endif
		public static bool TryGetSnapshotData (NSData? data, out GCMicroGamepadSnapshotData snapshotData)
		{
			snapshotData = default;
			unsafe {
				bool result = GCMicroGamepadSnapshotDataFromNSData ((GCMicroGamepadSnapshotData*) Unsafe.AsPointer<GCMicroGamepadSnapshotData> (ref snapshotData), data.GetHandle ()) != 0;
				GC.KeepAlive (data);
				return result;
			}
		}

	}
}
