// 
// SecAccessControl.cs: Implements the managed SecAccessControl representation
//
// Authors: 
//  Miguel de Icaza  <miguel@xamarin.com>
//
// Copyright 2014, 2015 Xamarin Inc.
//
// Notice: to avoid having to track the object and then having to remove it
// this class exists merely to set the desired flags, and the sole consumer
// of this API, creates the object on demands and passes ownership when
// calling SecAddItem.
//

#nullable enable

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using ObjCRuntime;
using CoreFoundation;
using Foundation;

namespace Security {

	/// <summary>Contains access control flags for creating keychain entries.</summary>
	///     <remarks>To be added.</remarks>
	[Flags]
	[Native]
	// changed to CFOptionFlags in Xcode 8 SDK
	public enum SecAccessControlCreateFlags : ulong {
		/// <summary>Requires the user to validate, either biometrically or via the device passcode.</summary>
		UserPresence = 1 << 0,

		/// <summary>Developers should use <see cref="Security.SecAccessControlCreateFlags.BiometryAny" /> instead.</summary>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		[Advice ("'BiometryAny' is preferred over 'TouchIDAny' since Xcode 9.3. Touch ID and Face ID together are biometric authentication mechanisms.")]
		TouchIDAny = BiometryAny,

		/// <summary>Developers should use <see cref="Security.SecAccessControlCreateFlags.BiometryCurrentSet" /> instead.</summary>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		[Advice ("'BiometryCurrentSet' is preferred over 'TouchIDCurrentSet' since Xcode 9.3. Touch ID and Face ID together are biometric authentication mechanisms.")]
		TouchIDCurrentSet = BiometryCurrentSet,

		// Added in iOS 11.3 and macOS 10.13.4 but keeping initial availability attribute because it's using the value
		// of 'TouchIDAny' which iOS 9 / macOS 10.12.1 will accept.
		/// <summary>Require any biometric for access.</summary>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		BiometryAny = 1 << 1,

		// Added in iOS 11.3 and macOS 10.13.4 but keeping initial availability attribute because it's using the value
		// of 'TouchIDCurrentSet' which iOS 9 / macOS 10.12.1 will accept.
		/// <summary>Require the currently set biometric for access.</summary>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		BiometryCurrentSet = 1 << 3,

		/// <summary>Validation via the device passcode.</summary>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		DevicePasscode = 1 << 4,

		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[ObsoletedOSPlatform ("macos", "Use 'Companion' instead.")]
		[ObsoletedOSPlatform ("maccatalyst", "Use 'Companion' instead.")]
		[UnsupportedOSPlatform ("ios")]
		[UnsupportedOSPlatform ("tvos")]
		Watch = 1 << 5,

		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[UnsupportedOSPlatform ("ios")]
		[UnsupportedOSPlatform ("tvos")]
		Companion = 1 << 5,

		/// <summary>An "OR" operation applied to other flags.</summary>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		Or = 1 << 14,

		/// <summary>An "And" operation applied to other flags.</summary>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		And = 1 << 15,

		/// <summary>Require a private key for access.</summary>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		PrivateKeyUsage = 1 << 30,

		/// <summary>Require an application password for access.</summary>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		ApplicationPassword = 1UL << 31,
	}

	/// <summary>Class that contains accessibility flags and access control object creation flags.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("tvos")]
	public partial class SecAccessControl : NativeObject {
#if !COREBUILD
		[Preserve (Conditional = true)]
		internal SecAccessControl (NativeHandle handle, bool owns)
			: base (handle, owns)
		{
		}

		/// <param name="accessible">To be added.</param>
		///         <param name="flags">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public SecAccessControl (SecAccessible accessible, SecAccessControlCreateFlags flags = SecAccessControlCreateFlags.UserPresence)
			: base (SecAccessControlCreateWithFlags (IntPtr.Zero, KeysAccessible.FromSecAccessible (accessible), (nint) (long) flags, out var _), true)
		{
			Accessible = accessible;
			Flags = flags;
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public SecAccessible Accessible { get; private set; }
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public SecAccessControlCreateFlags Flags { get; private set; }

		[DllImport (Constants.SecurityLibrary)]
		unsafe extern static IntPtr SecAccessControlCreateWithFlags (IntPtr allocator, /* CFTypeRef */ IntPtr protection, /* SecAccessControlCreateFlags */ nint flags, IntPtr* error);

		unsafe static IntPtr SecAccessControlCreateWithFlags (IntPtr allocator, /* CFTypeRef */ IntPtr protection, /* SecAccessControlCreateFlags */ nint flags, out IntPtr error)
		{
			error = default;
			return SecAccessControlCreateWithFlags (allocator, protection, flags, (IntPtr*) Unsafe.AsPointer<IntPtr> (ref error));
		}
#endif
	}
}
