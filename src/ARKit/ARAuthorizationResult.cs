//
// ARAuthorizationResult.cs: Bindings for the ARKit C API authorization types
//
// Copyright 2025 Microsoft Corp
//

#if __MACOS__
#nullable enable

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using CoreFoundation;
using ObjCRuntime;

namespace ARKit {

	/// <summary>Represents a single authorization result from the ARKit C API.</summary>
	[SupportedOSPlatform ("macos26.0")]
	public class ARAuthorizationResult : ARObject {

		[DllImport (Constants.ARKitLibrary)]
		static extern /* ar_authorization_type_t */ nuint ar_authorization_result_get_authorization_type (IntPtr authorization_result);

		[DllImport (Constants.ARKitLibrary)]
		static extern /* ar_authorization_status_t */ nint ar_authorization_result_get_status (IntPtr authorization_result);

		[Preserve (Conditional = true)]
		internal ARAuthorizationResult (NativeHandle handle, bool owns)
			: base (handle, owns)
		{
		}

		/// <summary>Gets the authorization type associated with this result.</summary>
		public ARAuthorizationType AuthorizationType {
			get {
				return (ARAuthorizationType) (ulong) ar_authorization_result_get_authorization_type (GetCheckedHandle ());
			}
		}

		/// <summary>Gets the authorization status associated with this result.</summary>
		public ARAuthorizationStatus Status {
			get {
				return (ARAuthorizationStatus) (long) ar_authorization_result_get_status (GetCheckedHandle ());
			}
		}
	}

	/// <summary>Represents a collection of authorization results from the ARKit C API.</summary>
	[SupportedOSPlatform ("macos26.0")]
	public class ARAuthorizationResults : ARObject {

		[DllImport (Constants.ARKitLibrary)]
		static extern /* size_t */ nuint ar_authorization_results_get_count (IntPtr authorization_results);

		[DllImport (Constants.ARKitLibrary)]
		unsafe static extern void ar_authorization_results_enumerate_results_f (
			IntPtr authorization_results,
			void* context,
			delegate* unmanaged<void*, IntPtr, byte> enumerator);

		[Preserve (Conditional = true)]
		internal ARAuthorizationResults (NativeHandle handle, bool owns)
			: base (handle, owns)
		{
		}

		/// <summary>Gets the number of authorization results in this collection.</summary>
		public nuint Count {
			get {
				return ar_authorization_results_get_count (GetCheckedHandle ());
			}
		}

		/// <summary>Returns all authorization results in this collection as an array.</summary>
		public ARAuthorizationResult [] GetResults ()
		{
			var results = new List<ARAuthorizationResult> ();
			unsafe {
				delegate* unmanaged<void*, IntPtr, byte> callback = &EnumerateCallback;
				var handle = GCHandle.Alloc (results);
				try {
					ar_authorization_results_enumerate_results_f (GetCheckedHandle (), (void*) GCHandle.ToIntPtr (handle), callback);
				} finally {
					handle.Free ();
				}
			}
			return results.ToArray ();
		}

		[UnmanagedCallersOnly]
		unsafe static byte EnumerateCallback (void* context, IntPtr authorization_result)
		{
			var handle = GCHandle.FromIntPtr ((IntPtr) context);
			var results = (List<ARAuthorizationResult>) handle.Target!;
			results.Add (new ARAuthorizationResult (authorization_result, owns: false));
			return 1; // continue
		}
	}
}

#endif // __MACOS__
