//
// ARAnchor.cs: Bindings for the ARKit C API anchor types
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

	/// <summary>Represents an ARKit anchor in the C API.</summary>
	[SupportedOSPlatform ("macos26.0")]
	public class ARAnchor : ARObject {

		[DllImport (Constants.ARKitLibrary)]
		static extern /* simd_float4x4 */ SimdFloat4x4 ar_anchor_get_origin_from_anchor_transform (IntPtr anchor);

		[DllImport (Constants.ARKitLibrary)]
		unsafe static extern void ar_anchor_get_identifier (IntPtr anchor, byte* out_identifier);

		[DllImport (Constants.ARKitLibrary)]
		static extern double ar_anchor_get_timestamp (IntPtr anchor);

		[Preserve (Conditional = true)]
		internal ARAnchor (NativeHandle handle, bool owns)
			: base (handle, owns)
		{
		}

		/// <summary>Gets the transform from this anchor to the origin coordinate system.</summary>
		public Matrix4 OriginFromAnchorTransform {
			get {
				var simd = ar_anchor_get_origin_from_anchor_transform (GetCheckedHandle ());
				return simd.ToNMatrix4 ();
			}
		}

		/// <summary>Gets the unique identifier of this anchor.</summary>
		public Guid Identifier {
			get {
				unsafe {
					byte* uuid = stackalloc byte [16];
					ar_anchor_get_identifier (GetCheckedHandle (), uuid);
					return new Guid (new ReadOnlySpan<byte> (uuid, 16));
				}
			}
		}

		/// <summary>Gets the timestamp associated with this anchor.</summary>
		public double Timestamp {
			get {
				return ar_anchor_get_timestamp (GetCheckedHandle ());
			}
		}
	}

	/// <summary>Represents a trackable ARKit anchor that can report whether it is currently tracked.</summary>
	[SupportedOSPlatform ("macos26.0")]
	public class ARTrackableAnchor : ARAnchor {

		[DllImport (Constants.ARKitLibrary)]
		static extern byte ar_trackable_anchor_is_tracked (IntPtr anchor);

		[Preserve (Conditional = true)]
		internal ARTrackableAnchor (NativeHandle handle, bool owns)
			: base (handle, owns)
		{
		}

		/// <summary>Gets a value indicating whether this anchor is currently tracked.</summary>
		public bool IsTracked {
			get {
				return ar_trackable_anchor_is_tracked (GetCheckedHandle ()) != 0;
			}
		}
	}
}

#endif // __MACOS__
