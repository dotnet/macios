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

using System.Runtime.InteropServices;
using CoreFoundation;
using ObjCRuntime;

namespace ARKit {

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

		protected internal override void Retain ()
		{
			if (Handle != IntPtr.Zero)
				ar_retain (Handle);
		}

		protected internal override void Release ()
		{
			if (Handle != IntPtr.Zero)
				ar_release (Handle);
		}
	}
}

#endif // __MACOS__
