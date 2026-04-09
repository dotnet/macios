//
// ARError.cs: Bindings for the ARKit C API ar_error_t
//
// Copyright 2025 Microsoft Corp
//

#if __MACOS__
#nullable enable

using System.Runtime.InteropServices;
using CoreFoundation;
using ObjCRuntime;

namespace ARKit {

	/// <summary>Represents an error from the ARKit C API.</summary>
	[SupportedOSPlatform ("macos26.0")]
	public class ARError : ARObject {

		[DllImport (Constants.ARKitLibrary)]
		static extern /* ar_error_code_t */ nint ar_error_get_error_code (IntPtr error);

		[DllImport (Constants.ARKitLibrary)]
		static extern /* CFErrorRef */ IntPtr ar_error_copy_cf_error (IntPtr error);

		[Preserve (Conditional = true)]
		internal ARError (NativeHandle handle, bool owns)
			: base (handle, owns)
		{
		}

		/// <summary>Gets the error domain string for ARKit errors.</summary>
		public static NSString? ErrorDomain {
			get {
				var h = Dlfcn.dlopen (Constants.ARKitLibrary, 0);
				try {
					return Dlfcn.GetStringConstant (h, "ar_error_domain");
				} finally {
					Dlfcn.dlclose (h);
				}
			}
		}

		/// <summary>Gets the error code associated with this error.</summary>
		public nint ErrorCode {
			get {
				return ar_error_get_error_code (GetCheckedHandle ());
			}
		}

		/// <summary>Gets a <see cref="CFException"/> representation of this ARKit error.</summary>
		public CFException CFError {
			get {
				return CFException.FromCFError (ar_error_copy_cf_error (GetCheckedHandle ()), true);
			}
		}
	}
}

#endif // __MACOS__
