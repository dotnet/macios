// Copyright 2013 Xamarin Inc. All rights reserved

#nullable enable

using CoreFoundation;

namespace Foundation {

	// Helper to (mostly) support NS[Mutable]Copying protocols
	/// <summary>An OS-controlled area within memory from which objects are allocated.</summary>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	public class NSZone : INativeObject {
		[DllImport (Constants.FoundationLibrary)]
		static extern /* NSZone* */ IntPtr NSDefaultMallocZone ();

		[DllImport (Constants.FoundationLibrary)]
		static extern IntPtr /* NSString* */ NSZoneName (/* NSZone* */ IntPtr zone);

		[DllImport (Constants.FoundationLibrary)]
		static extern void NSSetZoneName (/* NSZone* */ IntPtr zone, /* NSString* */ IntPtr name);

		[Preserve (Conditional = true)]
		internal NSZone (NativeHandle handle, bool owns)
		{
			// NSZone is just an opaque pointer without reference counting, so we ignore the 'owns' parameter.
			this.Handle = handle;
		}

		/// <summary>Handle (pointer) to the unmanaged object representation.</summary>
		///         <value>A pointer</value>
		///         <remarks>This IntPtr is a handle to the underlying unmanaged representation for this object.</remarks>
		public NativeHandle Handle { get; private set; }

#if !COREBUILD
		/// <summary>Gets or sets the name of the zone.</summary>
		/// <value>The zone name, or <see langword="null" /> if the zone has no name.</value>
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
		/// <summary>The default memory allocation zone.</summary>
		public static readonly NSZone Default = new NSZone (NSDefaultMallocZone (), false);
#endif
	}
}
