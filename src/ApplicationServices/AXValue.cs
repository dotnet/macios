// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

#nullable enable

#if __MACOS__
using CoreFoundation;
using CoreGraphics;

namespace ApplicationServices {
	/// <summary>Wraps a structure or error code used as an accessibility attribute value.</summary>
	[SupportedOSPlatform ("macos")]
	[UnsupportedOSPlatform ("ios")]
	[UnsupportedOSPlatform ("tvos")]
	[UnsupportedOSPlatform ("maccatalyst")]
	public class AXValue : NativeObject {
		[Preserve (Conditional = true)]
		internal AXValue (NativeHandle handle, bool owns) : base (handle, owns)
		{
		}

		[DllImport (Constants.ApplicationServicesLibrary)]
		extern static nuint AXValueGetTypeID ();

		/// <summary>Gets the Core Foundation type identifier of accessibility values.</summary>
		/// <returns>The native type identifier.</returns>
		public static nuint GetTypeID () => AXValueGetTypeID ();

		[DllImport (Constants.ApplicationServicesLibrary)]
		extern static AXValueType AXValueGetType (IntPtr value);

		/// <summary>Gets the type of the wrapped structure.</summary>
		public AXValueType ValueType {
			get {
				var type = AXValueGetType (GetCheckedHandle ());
				GC.KeepAlive (this);
				return type;
			}
		}

		[DllImport (Constants.ApplicationServicesLibrary)]
		unsafe extern static IntPtr AXValueCreate (AXValueType type, void* value);

		static unsafe IntPtr Create<T> (AXValueType type, T value) where T : unmanaged => AXValueCreate (type, &value);

		/// <summary>Creates an accessibility value containing a point.</summary>
		/// <param name="value">The point to wrap.</param>
		public AXValue (CGPoint value) : base (Create (AXValueType.CGPoint, value), true) { }

		/// <summary>Creates an accessibility value containing a size.</summary>
		/// <param name="value">The size to wrap.</param>
		public AXValue (CGSize value) : base (Create (AXValueType.CGSize, value), true) { }

		/// <summary>Creates an accessibility value containing a rectangle.</summary>
		/// <param name="value">The rectangle to wrap.</param>
		public AXValue (CGRect value) : base (Create (AXValueType.CGRect, value), true) { }

		/// <summary>Creates an accessibility value containing a range.</summary>
		/// <param name="value">The range to wrap.</param>
		public AXValue (CFRange value) : base (Create (AXValueType.CFRange, value), true) { }

		/// <summary>Creates an accessibility value containing an error code.</summary>
		/// <param name="value">The error code to wrap.</param>
		public AXValue (AXError value) : base (Create (AXValueType.AXError, value), true) { }

		[DllImport (Constants.ApplicationServicesLibrary)]
		unsafe extern static byte AXValueGetValue (IntPtr value, AXValueType type, void* result);

		unsafe bool TryGetValue<T> (AXValueType type, out T value) where T : unmanaged
		{
			value = default;
			bool result;
			fixed (T* ptr = &value)
				result = AXValueGetValue (GetCheckedHandle (), type, ptr) != 0;
			GC.KeepAlive (this);
			return result;
		}

		/// <summary>Attempts to extract a point.</summary>
		/// <param name="value">The point, or its default value on a type mismatch.</param>
		/// <returns>Whether the value contains a point.</returns>
		public bool TryGetValue (out CGPoint value) => TryGetValue (AXValueType.CGPoint, out value);

		/// <summary>Attempts to extract a size.</summary>
		/// <param name="value">The size, or its default value on a type mismatch.</param>
		/// <returns>Whether the value contains a size.</returns>
		public bool TryGetValue (out CGSize value) => TryGetValue (AXValueType.CGSize, out value);

		/// <summary>Attempts to extract a rectangle.</summary>
		/// <param name="value">The rectangle, or its default value on a type mismatch.</param>
		/// <returns>Whether the value contains a rectangle.</returns>
		public bool TryGetValue (out CGRect value) => TryGetValue (AXValueType.CGRect, out value);

		/// <summary>Attempts to extract a range.</summary>
		/// <param name="value">The range, or its default value on a type mismatch.</param>
		/// <returns>Whether the value contains a range.</returns>
		public bool TryGetValue (out CFRange value) => TryGetValue (AXValueType.CFRange, out value);

		/// <summary>Attempts to extract an error code.</summary>
		/// <param name="value">The error code, or its default value on a type mismatch.</param>
		/// <returns>Whether the value contains an error code.</returns>
		public bool TryGetValue (out AXError value) => TryGetValue (AXValueType.AXError, out value);
	}
}
#endif
