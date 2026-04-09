#if __MACOS__
#nullable enable

using System;
using System.Runtime.Versioning;
using CoreFoundation;
using ObjCRuntime;

namespace Security {

	/// <summary>Represents an opaque reference to an authorization engine used in authorization plugin views.</summary>
	public class AuthorizationEngine : NativeObject {
		[Preserve (Conditional = true)]
		internal AuthorizationEngine (NativeHandle handle, bool owns)
			: base (handle, owns)
		{
		}
#if !COREBUILD
		/// <summary>Creates an <see cref="AuthorizationEngine" /> from a raw handle without taking ownership.</summary>
		/// <param name="handle">The native <c>AuthorizationEngineRef</c> handle.</param>
		/// <returns>A managed wrapper, or <see langword="null" /> if the handle is zero.</returns>
		public static AuthorizationEngine? Create (NativeHandle handle)
		{
			if (handle == IntPtr.Zero)
				return null;
			return new AuthorizationEngine (handle, owns: false);
		}
#endif // !COREBUILD
	}
}
#endif // __MACOS__
