// Copyright 2013 Xamarin Inc. All rights reserved

#nullable enable

using System;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

using CoreFoundation;
using ObjCRuntime;

#if !NET
using NativeHandle = System.IntPtr;
#endif

namespace Foundation {

	// Helper to (mostly) support NS[Mutable]Copying protocols
#if NET
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
#endif
	public class NSZone : INativeObject {
		[DllImport (Constants.FoundationLibrary)]
		static extern /* NSZone* */ IntPtr NSDefaultMallocZone ();

		[DllImport (Constants.FoundationLibrary)]
		static extern IntPtr /* NSString* */ NSZoneName (/* NSZone* */ IntPtr zone);

		[DllImport (Constants.FoundationLibrary)]
		static extern void NSSetZoneName (/* NSZone* */ IntPtr zone, /* NSString* */ IntPtr name);

#if !NET
		public NSZone (NativeHandle handle)
			: this (handle, false)
		{
		}
#endif

		[Preserve (Conditional = true)]
#if NET
		internal NSZone (NativeHandle handle, bool owns)
#else
		public NSZone (NativeHandle handle, bool owns)
#endif
		{
			// NSZone is just an opaque pointer without reference counting, so we ignore the 'owns' parameter.
			this.Handle = handle;
		}

		/// <summary>Handle (pointer) to the unmanaged object representation.</summary>
		///         <value>A pointer</value>
		///         <remarks>This IntPtr is a handle to the underlying unmanaged representation for this object.</remarks>
		public NativeHandle Handle { get; private set; }

#if !COREBUILD
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public string? Name {
			get {
				return CFString.FromHandle (NSZoneName (Handle));
			}
			set {
				var nsHandle = CFString.CreateNative (value);
				try {
					NSSetZoneName (Handle, nsHandle);
				} finally {
					CFString.ReleaseNative (nsHandle);
				}
			}
		}

		// note: Copy(NSZone) and MutableCopy(NSZone) with a nil pointer == default
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public static readonly NSZone Default = new NSZone (NSDefaultMallocZone (), false);
#endif
	}
}
