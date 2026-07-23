//
// Copyright 2012-2014 Xamarin
//

#nullable enable

using CoreFoundation;

namespace CoreFoundation {
	/// <summary>Base type for some Core Foundation classes, such as <see cref="CoreFoundation.CFSocket" /> and <see cref="CoreFoundation.CFStream" />.</summary>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	public class CFType : NativeObject, ICFType {
		/// <summary>Returns the CoreFoundation type for the specified object.</summary>
		/// <param name="typeRef">Handle to a CoreFoundation object.</param>
		[DllImport (Constants.CoreFoundationLibrary, EntryPoint = "CFGetTypeID")]
		public static extern nint GetTypeID (IntPtr typeRef);

		[DllImport (Constants.CoreFoundationLibrary)]
		extern static IntPtr CFCopyDescription (IntPtr ptr);

		/// <summary>Initializes a new instance of the <see cref="CFType" /> class.</summary>
		internal CFType ()
		{
		}

		[Preserve (Conditional = true)]
		internal CFType (NativeHandle handle, bool owns)
			: base (handle, owns)
		{
		}

		/// <summary>Returns a textual representation of the specified object.</summary>
		/// <param name="handle">Handle to the native CoreFoundation object.</param>
		public string? GetDescription (IntPtr handle)
		{
			if (handle == IntPtr.Zero)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (handle));

			return CFString.FromHandle (CFCopyDescription (handle), true);
		}

		[DllImport (Constants.CoreFoundationLibrary)]
		extern static byte CFEqual (/*CFTypeRef*/ IntPtr cf1, /*CFTypeRef*/ IntPtr cf2);

		/// <summary>Compares two handles of native objects for equality.</summary>
		/// <param name="cf1">The first CoreFoundation object handle.</param>
		/// <param name="cf2">The second CoreFoundation object handle.</param>
		/// <returns>true if the types are the same.</returns>
		public static bool Equal (IntPtr cf1, IntPtr cf2)
		{
			// CFEqual is not happy (but crashy) when it receive null
			if (cf1 == IntPtr.Zero)
				return cf2 == IntPtr.Zero;
			else if (cf2 == IntPtr.Zero)
				return false;
			return CFEqual (cf1, cf2) != 0;
		}
	}

	/// <summary>MonoTouch-internal interface for now.</summary>
	///     <remarks>This interface will be used to annotate classes that wrap CoreFoundation types.</remarks>
	public interface ICFType : INativeObject {
	}
}
