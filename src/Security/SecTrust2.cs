//
// SecTrust2.cs: Bindings the Security's sec_trust_t
//
// The difference between SecTrust2 and SecTrust is that the
// SecTrust2 is a binding for the new sec_trust_t API that was
// introduced on iOS 12/OSX Mojave, while SecTrust is the older API
// that binds SecTrustRef.
//
// Authors:
//   Miguel de Icaza (miguel@microsoft.com)
//
// Copyrigh 2018 Microsoft Inc
//

#nullable enable

using System;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using ObjCRuntime;
using Foundation;
using CoreFoundation;

namespace Security {
	/// <summary>To be added.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("tvos")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	public class SecTrust2 : NativeObject {
		[Preserve (Conditional = true)]
		internal SecTrust2 (NativeHandle handle, bool owns) : base (handle, owns) { }

		[DllImport (Constants.SecurityLibrary)]
		extern static IntPtr sec_trust_create (IntPtr sectrustHandle);

		/// <param name="trust">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public SecTrust2 (SecTrust trust)
		{
			if (trust is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (trust));

			Handle = sec_trust_create (trust.Handle);
			GC.KeepAlive (trust);
		}

		[DllImport (Constants.SecurityLibrary)]
		extern static IntPtr sec_trust_copy_ref (IntPtr handle);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public SecTrust Trust => new SecTrust (sec_trust_copy_ref (GetCheckedHandle ()), owns: true);
	}
}
