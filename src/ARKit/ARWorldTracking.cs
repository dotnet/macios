//
// ARWorldTracking.cs: Bindings for the ARKit C API world tracking types
//
// Copyright 2025 Microsoft Corp
//

#if __MACOS__
#nullable enable

using System;
using System.Runtime.InteropServices;
using CoreFoundation;
using ObjCRuntime;

using Matrix4 = global::CoreGraphics.NMatrix4;

namespace ARKit {

	/// <summary>Represents a device anchor that provides device pose information.</summary>
	[SupportedOSPlatform ("macos26.0")]
	public class ARDeviceAnchor : ARTrackableAnchor {

		[DllImport (Constants.ARKitLibrary)]
		static extern /* ar_device_anchor_t */ IntPtr ar_device_anchor_create ();

		[DllImport (Constants.ARKitLibrary)]
		unsafe static extern void ar_device_anchor_get_identifier (IntPtr anchor, byte* out_identifier);

		[DllImport (Constants.ARKitLibrary)]
		static extern /* simd_float4x4 */ Matrix4 ar_device_anchor_get_origin_from_anchor_transform (IntPtr anchor);

		[DllImport (Constants.ARKitLibrary)]
		static extern double ar_device_anchor_get_timestamp (IntPtr anchor);

		[DllImport (Constants.ARKitLibrary)]
		static extern byte ar_device_anchor_is_tracked (IntPtr anchor);

		[DllImport (Constants.ARKitLibrary)]
		static extern /* ar_device_anchor_tracking_state_t */ nint ar_device_anchor_get_tracking_state (IntPtr anchor);

		[Preserve (Conditional = true)]
		internal ARDeviceAnchor (NativeHandle handle, bool owns)
			: base (handle, owns)
		{
		}

		/// <summary>Creates a new device anchor.</summary>
		public ARDeviceAnchor ()
			: base (ar_device_anchor_create (), owns: true)
		{
		}

		/// <summary>Gets the unique identifier of this device anchor.</summary>
		public new Guid Identifier {
			get {
				unsafe {
					byte* uuid = stackalloc byte [16];
					ar_device_anchor_get_identifier (GetCheckedHandle (), uuid);
					return new Guid (new ReadOnlySpan<byte> (uuid, 16));
				}
			}
		}

		/// <summary>Gets the transform from this device anchor to the origin coordinate system.</summary>
		public new Matrix4 OriginFromAnchorTransform {
			get {
				return ar_device_anchor_get_origin_from_anchor_transform (GetCheckedHandle ());
			}
		}

		/// <summary>Gets the timestamp associated with this device anchor.</summary>
		public new double Timestamp {
			get {
				return ar_device_anchor_get_timestamp (GetCheckedHandle ());
			}
		}

		/// <summary>Gets a value indicating whether this device anchor is currently tracked.</summary>
		public new bool IsTracked {
			get {
				return ar_device_anchor_is_tracked (GetCheckedHandle ()) != 0;
			}
		}

		/// <summary>Gets the tracking state of this device anchor.</summary>
		public ARDeviceAnchorTrackingState TrackingState {
			get {
				return (ARDeviceAnchorTrackingState) (long) ar_device_anchor_get_tracking_state (GetCheckedHandle ());
			}
		}
	}

	/// <summary>Represents a world tracking configuration.</summary>
	[SupportedOSPlatform ("macos26.0")]
	public class ARWorldTrackingConfiguration : ARObject {

		[DllImport (Constants.ARKitLibrary)]
		static extern /* ar_world_tracking_configuration_t */ IntPtr ar_world_tracking_configuration_create ();

		[Preserve (Conditional = true)]
		internal ARWorldTrackingConfiguration (NativeHandle handle, bool owns)
			: base (handle, owns)
		{
		}

		/// <summary>Creates a new world tracking configuration.</summary>
		public ARWorldTrackingConfiguration ()
			: base (ar_world_tracking_configuration_create (), owns: true)
		{
		}
	}

	/// <summary>Represents a world tracking data provider.</summary>
	[SupportedOSPlatform ("macos26.0")]
	public class ARWorldTrackingProvider : ARDataProvider {

		[DllImport (Constants.ARKitLibrary)]
		static extern /* ar_world_tracking_provider_t */ IntPtr ar_world_tracking_provider_create (IntPtr world_tracking_configuration);

		[DllImport (Constants.ARKitLibrary)]
		static extern byte ar_world_tracking_provider_is_supported ();

		[DllImport (Constants.ARKitLibrary)]
		static extern /* ar_device_anchor_query_status_t */ nint ar_world_tracking_provider_query_device_anchor_at_timestamp (
			IntPtr world_tracking_provider,
			double timestamp,
			IntPtr device_anchor);

		[DllImport (Constants.ARKitLibrary)]
		static extern /* ar_authorization_type_t */ nuint ar_world_tracking_provider_get_required_authorization_type ();

		[Preserve (Conditional = true)]
		internal ARWorldTrackingProvider (NativeHandle handle, bool owns)
			: base (handle, owns)
		{
		}

		/// <summary>Creates a new world tracking provider with the specified configuration.</summary>
		public ARWorldTrackingProvider (ARWorldTrackingConfiguration configuration)
			: base (ar_world_tracking_provider_create (configuration.GetCheckedHandle ()), owns: true)
		{
			GC.KeepAlive (configuration);
		}

		/// <summary>Gets a value indicating whether this device supports the world tracking provider.</summary>
		public static bool IsSupported {
			get {
				return ar_world_tracking_provider_is_supported () != 0;
			}
		}

		/// <summary>Gets the authorization type required by the world tracking provider.</summary>
		public static new ARAuthorizationType RequiredAuthorizationType {
			get {
				return (ARAuthorizationType) (ulong) ar_world_tracking_provider_get_required_authorization_type ();
			}
		}

		/// <summary>Queries the device anchor at the given timestamp.</summary>
		/// <param name="timestamp">The timestamp to query, as mach absolute time in seconds.</param>
		/// <param name="deviceAnchor">The device anchor to populate with the query result.</param>
		/// <returns>The status of the query.</returns>
		/// <remarks>This API is not thread safe.</remarks>
		public ARDeviceAnchorQueryStatus QueryDeviceAnchor (double timestamp, ARDeviceAnchor deviceAnchor)
		{
			if (deviceAnchor is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (deviceAnchor));
			var result = (ARDeviceAnchorQueryStatus) (long) ar_world_tracking_provider_query_device_anchor_at_timestamp (
				GetCheckedHandle (), timestamp, deviceAnchor.GetCheckedHandle ());
			GC.KeepAlive (deviceAnchor);
			return result;
		}
	}
}

#endif // __MACOS__
