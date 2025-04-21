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
	/// <summary>Represents the instantaneous state of a micro gamepad in V100 format at a point in time.</summary>
	///     <remarks>To be added.</remarks>
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

		/// <summary>Converts the snapshot to an <see cref="T:Foundation.NSData" /> object.</summary>
		///         <returns>An <see cref="T:Foundation.NSData" /> object that contains the snapshot data.</returns>
		///         <remarks>To be added.</remarks>
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
	/// <summary>To be added.</summary>
	///     <remarks>To be added.</remarks>
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
		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
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
		/// <param name="data">The NSData from which to get the V100 data.</param>
		///         <param name="snapshotData">The location in which to store the snapshot data.</param>
		///         <summary>Tries to obtain v100 snapshot data from an NSData object.</summary>
		///         <returns>
		///           <see langword="true" /> if the data could be retrieved. Otherwise, <see langword="false" />.</returns>
		///         <remarks>When the return value is <see langword="false" />, <paramref name="snapshotData" /> will contain <format type="text/html"><a href="https://docs.microsoft.com/en-us/search/index?search=System%20Int%20Ptr%20Zero&amp;scope=Xamarin" title="F:System.IntPtr.Zero">F:System.IntPtr.Zero</a></format>.</remarks>
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
		/// <param name="data">To be added.</param>
		///         <param name="snapshotData">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
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
