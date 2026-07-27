#if __MACOS__
#nullable enable

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Text;
using ObjCRuntime;
using CoreFoundation;

namespace Security {

	/// <summary>Represents a keychain on macOS.</summary>
	[SupportedOSPlatform ("macos")]
	[ObsoletedOSPlatform ("macos10.10", "SecKeychain is deprecated.")]
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
		extern static nint SecKeychainGetTypeID ();

		/// <summary>Returns the Core Foundation type identifier for SecKeychain.</summary>
		/// <returns>The Core Foundation type identifier.</returns>
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

		/// <summary>Gets the default keychain.</summary>
		/// <param name="status">The status returned by the native operation.</param>
		/// <returns>The default <see cref="SecKeychain" />, or <see langword="null" /> on failure.</returns>
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

		/// <summary>Opens the keychain at the specified file path.</summary>
		/// <param name="path">The file system path of the keychain to open.</param>
		/// <param name="status">The status returned by the native operation.</param>
		/// <param name="keychain">The opened keychain on success; otherwise, <see langword="null" />.</param>
		/// <returns><see langword="true" /> if the keychain was opened; otherwise, <see langword="false" />.</returns>
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
			keychain = new SecKeychain (handle, owns: true);
			return true;
		}

		[SupportedOSPlatform ("macos")]
		[ObsoletedOSPlatform ("macos10.10")]
		[DllImport (Constants.SecurityLibrary)]
		extern static unsafe OSStatus SecKeychainGetPath (IntPtr keychainRef, uint* ioPathLength, byte* pathName);

		/// <summary>Gets the file system path of this keychain.</summary>
		/// <param name="status">The status returned by the native operation.</param>
		/// <returns>The POSIX path of the keychain, or <see langword="null" /> on failure.</returns>
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
