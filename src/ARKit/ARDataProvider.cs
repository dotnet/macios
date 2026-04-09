//
// ARDataProvider.cs: Bindings for the ARKit C API data provider types
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

	/// <summary>Represents an ARKit data provider.</summary>
	[SupportedOSPlatform ("macos26.0")]
	public class ARDataProvider : ARObject {

		[DllImport (Constants.ARKitLibrary)]
		static extern /* ar_data_provider_state_t */ nint ar_data_provider_get_state (IntPtr data_provider);

		[DllImport (Constants.ARKitLibrary)]
		static extern /* ar_authorization_type_t */ nuint ar_data_provider_get_required_authorization_type (IntPtr data_provider);

		[Preserve (Conditional = true)]
		internal ARDataProvider (NativeHandle handle, bool owns)
			: base (handle, owns)
		{
		}

		/// <summary>Gets the current state of this data provider.</summary>
		public ARDataProviderState State {
			get {
				return (ARDataProviderState) (long) ar_data_provider_get_state (GetCheckedHandle ());
			}
		}

		/// <summary>Gets the authorization type required by this data provider.</summary>
		public ARAuthorizationType RequiredAuthorizationType {
			get {
				return (ARAuthorizationType) (ulong) ar_data_provider_get_required_authorization_type (GetCheckedHandle ());
			}
		}
	}

	/// <summary>Represents a mutable collection of ARKit data providers.</summary>
	[SupportedOSPlatform ("macos26.0")]
	public class ARDataProviders : ARObject {

		[DllImport (Constants.ARKitLibrary)]
		static extern /* ar_data_providers_t */ IntPtr ar_data_providers_create ();

		[DllImport (Constants.ARKitLibrary)]
		static extern void ar_data_providers_add_data_provider (IntPtr data_providers, IntPtr data_provider_to_add);

		[DllImport (Constants.ARKitLibrary)]
		static extern void ar_data_providers_add_data_providers (IntPtr data_providers, IntPtr data_providers_to_add);

		[DllImport (Constants.ARKitLibrary)]
		static extern void ar_data_providers_remove_data_provider (IntPtr data_providers, IntPtr data_provider_to_remove);

		[DllImport (Constants.ARKitLibrary)]
		static extern void ar_data_providers_remove_data_providers (IntPtr data_providers, IntPtr data_providers_to_remove);

		[DllImport (Constants.ARKitLibrary)]
		static extern /* size_t */ nuint ar_data_providers_get_count (IntPtr data_providers);

		[DllImport (Constants.ARKitLibrary)]
		unsafe static extern void ar_data_providers_enumerate_data_providers_f (
			IntPtr data_providers,
			void* context,
			delegate* unmanaged<void*, IntPtr, byte> enumerator);

		[Preserve (Conditional = true)]
		internal ARDataProviders (NativeHandle handle, bool owns)
			: base (handle, owns)
		{
		}

		/// <summary>Creates a new empty collection of data providers.</summary>
		public ARDataProviders ()
			: base (ar_data_providers_create (), owns: true)
		{
		}

		/// <summary>Gets the number of data providers in this collection.</summary>
		public nuint Count {
			get {
				return ar_data_providers_get_count (GetCheckedHandle ());
			}
		}

		/// <summary>Adds a data provider to this collection.</summary>
		public void Add (ARDataProvider dataProvider)
		{
			if (dataProvider is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (dataProvider));
			ar_data_providers_add_data_provider (GetCheckedHandle (), dataProvider.GetCheckedHandle ());
			GC.KeepAlive (dataProvider);
		}

		/// <summary>Adds all data providers from another collection to this collection.</summary>
		public void Add (ARDataProviders dataProviders)
		{
			if (dataProviders is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (dataProviders));
			ar_data_providers_add_data_providers (GetCheckedHandle (), dataProviders.GetCheckedHandle ());
			GC.KeepAlive (dataProviders);
		}

		/// <summary>Removes a data provider from this collection.</summary>
		public void Remove (ARDataProvider dataProvider)
		{
			if (dataProvider is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (dataProvider));
			ar_data_providers_remove_data_provider (GetCheckedHandle (), dataProvider.GetCheckedHandle ());
			GC.KeepAlive (dataProvider);
		}

		/// <summary>Removes all data providers from another collection from this collection.</summary>
		public void Remove (ARDataProviders dataProviders)
		{
			if (dataProviders is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (dataProviders));
			ar_data_providers_remove_data_providers (GetCheckedHandle (), dataProviders.GetCheckedHandle ());
			GC.KeepAlive (dataProviders);
		}

		/// <summary>Returns all data providers in this collection as an array.</summary>
		public ARDataProvider [] GetDataProviders ()
		{
			var results = new List<ARDataProvider> ();
			unsafe {
				delegate* unmanaged<void*, IntPtr, byte> callback = &EnumerateCallback;
				var handle = GCHandle.Alloc (results);
				try {
					ar_data_providers_enumerate_data_providers_f (GetCheckedHandle (), (void*) GCHandle.ToIntPtr (handle), callback);
				} finally {
					handle.Free ();
				}
			}
			return results.ToArray ();
		}

		[UnmanagedCallersOnly]
		unsafe static byte EnumerateCallback (void* context, IntPtr data_provider)
		{
			var handle = GCHandle.FromIntPtr ((IntPtr) context);
			var results = (List<ARDataProvider>) handle.Target!;
			results.Add (new ARDataProvider (data_provider, owns: false));
			return 1; // continue
		}
	}
}

#endif // __MACOS__
