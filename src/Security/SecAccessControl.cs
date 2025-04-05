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

#if !NET
using NativeHandle = System.IntPtr;
#endif

namespace Security {

	/// <summary>Contains access control flags for creating keychain entries.</summary>
	///     <remarks>To be added.</remarks>
	[Flags]
	[Native]
#if NET
	// changed to CFOptionFlags in Xcode 8 SDK
	public enum SecAccessControlCreateFlags : ulong {
#else
	// CFOptionFlags -> SecAccessControl.h
	public enum SecAccessControlCreateFlags : long {
#endif
		/// <summary>Requires the user to validate, either biometrically or via the device passcode.</summary>
		UserPresence = 1 << 0,

#if NET
		/// <summary>Developers should use <see cref="F:Security.SecAccessControlCreateFlags.BiometryAny" /> instead.</summary>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
#endif
		[Advice ("'BiometryAny' is preferred over 'TouchIDAny' since Xcode 9.3. Touch ID and Face ID together are biometric authentication mechanisms.")]
		TouchIDAny = BiometryAny,

#if NET
		/// <summary>Developers should use <see cref="F:Security.SecAccessControlCreateFlags.BiometryCurrentSet" /> instead.</summary>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
#endif
		[Advice ("'BiometryCurrentSet' is preferred over 'TouchIDCurrentSet' since Xcode 9.3. Touch ID and Face ID together are biometric authentication mechanisms.")]
		TouchIDCurrentSet = BiometryCurrentSet,

		// Added in iOS 11.3 and macOS 10.13.4 but keeping initial availability attribute because it's using the value
		// of 'TouchIDAny' which iOS 9 / macOS 10.12.1 will accept.
#if NET
		/// <summary>Require any biometric for access.</summary>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
#endif
		BiometryAny = 1 << 1,

		// Added in iOS 11.3 and macOS 10.13.4 but keeping initial availability attribute because it's using the value
		// of 'TouchIDCurrentSet' which iOS 9 / macOS 10.12.1 will accept.
#if NET
		/// <summary>Require the currently set biometric for access.</summary>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
#endif
		BiometryCurrentSet = 1 << 3,

#if NET
		/// <summary>Validation via the device passcode.</summary>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
#endif
		DevicePasscode = 1 << 4,

#if NET
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[ObsoletedOSPlatform ("macos", "Use 'Companion' instead.")]
		[ObsoletedOSPlatform ("maccatalyst", "Use 'Companion' instead.")]
		[UnsupportedOSPlatform ("ios")]
		[UnsupportedOSPlatform ("tvos")]
#else
		[NoiOS]
		[NoTV]
#endif
		Watch = 1 << 5,

#if NET
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[UnsupportedOSPlatform ("ios")]
		[UnsupportedOSPlatform ("tvos")]
#else
		[NoiOS]
		[NoTV]
#endif
		Companion = 1 << 5,

#if NET
		/// <summary>An "OR" operation applied to other flags.</summary>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
#endif
		Or = 1 << 14,

#if NET
		/// <summary>An "And" operation applied to other flags.</summary>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
#endif
		And = 1 << 15,

#if NET
		/// <summary>Require a private key for access.</summary>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
#endif
		PrivateKeyUsage = 1 << 30,

#if NET
		/// <summary>Require an application password for access.</summary>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
#endif
#if NET
		ApplicationPassword = 1UL << 31,
#else
		ApplicationPassword = 1 << 31,
#endif
	}

#if NET
	/// <summary>Class that contains accessibility flags and access control object creation flags.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("tvos")]
#endif
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
