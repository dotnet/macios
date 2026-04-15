//
// ARObject.cs: Base class for the ARKit C API object types
//
// Provides ar_retain/ar_release lifecycle management following the
// same pattern as CoreFoundation.OSLog (os_retain/os_release).
//
// Copyright 2025 Microsoft Corp
//

#if __MACOS__

#nullable enable

using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using CoreFoundation;
using ObjCRuntime;

using Matrix4 = global::CoreGraphics.NMatrix4;

namespace ARKit {

	// On ARM64, simd_float4x4 (4 × simd_float4) is an HVA returned in NEON registers v0-v3.
	// NMatrix4 has 16 individual float fields and is NOT recognized as HVA by .NET, causing
	// garbage when used as a P/Invoke return type. This struct uses Vector4 fields (128-bit
	// SIMD type on ARM64) which .NET correctly classifies as HVA.
	[StructLayout (LayoutKind.Sequential)]
	internal struct SimdFloat4x4 {
		public Vector4 Column0;
		public Vector4 Column1;
		public Vector4 Column2;
		public Vector4 Column3;

		public unsafe Matrix4 ToNMatrix4 ()
		{
			// Both SimdFloat4x4 and NMatrix4 are 64-byte column-major matrices with
			// identical memory layout, so we can safely reinterpret the bits.
			var self = this;
			return *(Matrix4*) &self;
		}
	}

	/// <summary>Base class for ARKit C API object types that use ar_retain/ar_release for lifecycle management.</summary>
	[SupportedOSPlatform ("macos26.0")]
	public class ARObject : NativeObject {

		[DllImport (Constants.ARKitLibrary)]
		static extern IntPtr ar_retain (IntPtr obj);

		[DllImport (Constants.ARKitLibrary)]
		static extern void ar_release (IntPtr obj);

		[Preserve (Conditional = true)]
		internal ARObject (NativeHandle handle, bool owns)
			: base (handle, owns)
		{
		}

		/// <summary>Retains the native ARKit object by calling ar_retain.</summary>
		protected internal override void Retain ()
		{
			if (Handle != IntPtr.Zero)
				ar_retain (Handle);
		}

		/// <summary>Releases the native ARKit object by calling ar_release.</summary>
		protected internal override void Release ()
		{
			if (Handle != IntPtr.Zero)
				ar_release (Handle);
		}
	}
}

#endif // __MACOS__
