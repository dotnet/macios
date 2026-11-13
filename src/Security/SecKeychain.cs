#if __MACOS__
#nullable enable

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Text;
using ObjCRuntime;
using CoreFoundation;

#if XAMCORE_5_0
using SecKeychain = Security.SecKeychain2;
#endif

namespace Security {

	// Rename to avoid a documentation clash with Security.SecKeyChain on case-insensitive file systems.
#if XAMCORE_5_0
	/// <summary>Represents a keychain on macOS.</summary>
#endif
	[SupportedOSPlatform ("macos")]
	[ObsoletedOSPlatform ("macos10.10", "SecKeychain is deprecated.")]
#if XAMCORE_5_0
	public class SecKeychain2 : NativeObject {
#else
	public class SecKeychain : NativeObject {
#endif
		[Preserve (Conditional = true)]
#if XAMCORE_5_0
		internal SecKeychain2 (NativeHandle handle, bool owns)
#else
		internal SecKeychain (NativeHandle handle, bool owns)
#endif
			: base (handle, owns)
		{
		}
#if !COREBUILD
		[SupportedOSPlatform ("macos")]
		[ObsoletedOSPlatform ("macos10.10")]
		[DllImport (Constants.SecurityLibrary)]
		extern static nint SecKeychainGetTypeID ();

#if XAMCORE_5_0
		/// <summary>Returns the Core Foundation type identifier for <see cref="SecKeychain2" />.</summary>
		/// <returns>The Core Foundation type identifier.</returns>
#endif
		[SupportedOSPlatform ("macos")]
		[ObsoletedOSPlatform ("macos10.10", "SecKeychain is deprecated.")]
		public static nint GetTypeId ()
		{
			return SecKeychainGetTypeID ();
		}

		[SupportedOSPlatform ("macos")]
		[ObsoletedOSPlatform ("macos10.10")]
		[DllImport (Constants.SecurityLibrary)]
		extern static unsafe OSStatus SecKeychainCopyDefault (IntPtr* keychain);

#if XAMCORE_5_0
		/// <summary>Gets the default keychain.</summary>
		/// <param name="status">The status returned by the native operation.</param>
		/// <returns>The default <see cref="SecKeychain2" />, or <see langword="null" /> on failure.</returns>
#endif
		[SupportedOSPlatform ("macos")]
		[ObsoletedOSPlatform ("macos10.10", "SecKeychain is deprecated.")]
		public static SecKeychain? GetDefault (out OSStatus status)
		{
			IntPtr handle = IntPtr.Zero;
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
		extern static unsafe OSStatus SecKeychainOpen (IntPtr pathName, IntPtr* keychain);

		[SupportedOSPlatform ("macos")]
		[ObsoletedOSPlatform ("macos10.10")]
		[DllImport (Constants.SecurityLibrary)]
		extern static unsafe OSStatus SecKeychainGetStatus (IntPtr keychainRef, uint* keychainStatus);

#if XAMCORE_5_0
		/// <summary>Opens the keychain at the specified file path.</summary>
		/// <param name="path">The file system path of the keychain to open.</param>
		/// <param name="status">The status returned when opening the keychain or confirming that it exists.</param>
		/// <param name="keychain">The opened keychain on success; otherwise, <see langword="null" />.</param>
		/// <returns><see langword="true" /> if the keychain was opened; otherwise, <see langword="false" />.</returns>
#endif
		[SupportedOSPlatform ("macos")]
		[ObsoletedOSPlatform ("macos10.10", "SecKeychain is deprecated.")]
		public static bool TryOpen (string path, out OSStatus status, [NotNullWhen (true)] out SecKeychain? keychain)
		{
			if (path is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (path));
			using var pathStr = new TransientString (path);
			IntPtr handle = IntPtr.Zero;
			unsafe {
				status = SecKeychainOpen (pathStr, &handle);
			}
			if (status != 0 || handle == IntPtr.Zero) {
				keychain = null;
				return false;
			}
			uint keychainStatus = 0;
			unsafe {
				status = SecKeychainGetStatus (handle, &keychainStatus);
			}
			if (status != 0) {
				CFObject.CFRelease (handle);
				keychain = null;
				return false;
			}
			keychain = new SecKeychain (handle, owns: true);
			return true;
		}

		[SupportedOSPlatform ("macos")]
		[ObsoletedOSPlatform ("macos10.10")]
		[DllImport (Constants.SecurityLibrary)]
		extern static unsafe OSStatus SecKeychainGetPath (IntPtr keychainRef, uint* ioPathLength, byte* pathName);

#if XAMCORE_5_0
		/// <summary>Gets the file system path of this keychain.</summary>
		/// <param name="status">The status returned by the native operation.</param>
		/// <returns>The POSIX path of the keychain, or <see langword="null" /> on failure.</returns>
#endif
		[SupportedOSPlatform ("macos")]
		[ObsoletedOSPlatform ("macos10.10", "SecKeychain is deprecated.")]
		public string? GetPath (out OSStatus status)
		{
			var buffer = new byte [1024];
			var pathLength = (uint) buffer.Length;
			unsafe {
				fixed (byte* path = buffer) {
					status = SecKeychainGetPath (GetCheckedHandle (), &pathLength, path);
				}
			}
			GC.KeepAlive (this);
			if (status != 0)
				return null;

			var length = checked((int) pathLength);
			if (length > buffer.Length)
				throw new InvalidOperationException ("The native keychain path exceeded the supplied buffer.");
			if (length > 0 && buffer [length - 1] == 0)
				length--;
			return Encoding.UTF8.GetString (buffer, 0, length);
		}
#endif // !COREBUILD
	}
}
#endif // __MACOS__
