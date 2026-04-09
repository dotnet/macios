#if __MACOS__
#nullable enable

using System;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using ObjCRuntime;
using CoreFoundation;

namespace Security {

	/// <summary>Represents a keychain on macOS.</summary>
	public class SecKeychain : NativeObject {
		[Preserve (Conditional = true)]
		internal SecKeychain (NativeHandle handle, bool owns)
			: base (handle, owns)
		{
		}
#if !COREBUILD
		[SupportedOSPlatform ("macos")]
		[ObsoletedOSPlatform ("macos10.10")]
		[DllImport (Constants.SecurityLibrary)]
		extern static unsafe int /* OSStatus */ SecKeychainGetTypeID ();

		/// <summary>Returns the Core Foundation type identifier for SecKeychain.</summary>
		/// <returns>The Core Foundation type identifier.</returns>
		public static nint GetTypeID ()
		{
			return SecKeychainGetTypeID ();
		}

		[SupportedOSPlatform ("macos")]
		[ObsoletedOSPlatform ("macos10.10")]
		[DllImport (Constants.SecurityLibrary)]
		extern static unsafe int /* OSStatus */ SecKeychainCopyDefault (IntPtr* keychain);

		/// <summary>Gets the default keychain.</summary>
		/// <returns>The default <see cref="SecKeychain" />, or <see langword="null" /> on failure.</returns>
		public static SecKeychain? GetDefault ()
		{
			IntPtr handle;
			int status;
			unsafe {
				status = SecKeychainCopyDefault (&handle);
			}
			if (status != 0 || handle == IntPtr.Zero)
				return null;
			return new SecKeychain (handle, owns: true);
		}

		[SupportedOSPlatform ("macos")]
		[ObsoletedOSPlatform ("macos10.10")]
		[DllImport (Constants.SecurityLibrary)]
		extern static unsafe int /* OSStatus */ SecKeychainOpen (IntPtr pathName, IntPtr* keychain);

		/// <summary>Opens the keychain at the specified file path.</summary>
		/// <param name="path">The file system path of the keychain to open.</param>
		/// <returns>A <see cref="SecKeychain" /> for the specified path, or <see langword="null" /> on failure.</returns>
		public static SecKeychain? Open (string path)
		{
			if (path is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (path));
			using var pathStr = new TransientString (path);
			IntPtr handle;
			int status;
			unsafe {
				status = SecKeychainOpen (pathStr, &handle);
			}
			if (status != 0 || handle == IntPtr.Zero)
				return null;
			return new SecKeychain (handle, owns: true);
		}

		[SupportedOSPlatform ("macos")]
		[ObsoletedOSPlatform ("macos10.10")]
		[DllImport (Constants.SecurityLibrary)]
		extern static unsafe int /* OSStatus */ SecKeychainGetPath (IntPtr keychainRef, int* ioPathLength, IntPtr pathName);

		/// <summary>Gets the file system path of this keychain.</summary>
		/// <returns>The POSIX path of the keychain, or <see langword="null" /> on failure.</returns>
		public string? GetPath ()
		{
			int pathLength = 1024;
			IntPtr buffer = Marshal.AllocHGlobal (pathLength);
			try {
				int status;
				unsafe {
					status = SecKeychainGetPath (Handle, &pathLength, buffer);
				}
				if (status != 0)
					return null;
				return Marshal.PtrToStringUTF8 (buffer, pathLength);
			} finally {
				Marshal.FreeHGlobal (buffer);
			}
		}
#endif // !COREBUILD
	}
}
#endif // __MACOS__
