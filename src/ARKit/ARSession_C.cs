//
// ARSession.cs: Bindings for the ARKit C API session types
//
// Copyright 2025 Microsoft Corp
//

#if __MACOS__
#nullable enable

using System;
using System.Runtime.InteropServices;
using CoreFoundation;
using ObjCRuntime;

namespace ARKit {

	/// <summary>Represents an ARKit device for session creation on macOS.</summary>
	[SupportedOSPlatform ("macos26.0")]
	public class ARDevice : ARObject {

		[Preserve (Conditional = true)]
		internal ARDevice (NativeHandle handle, bool owns)
			: base (handle, owns)
		{
		}
	}

	/// <summary>Represents an ARKit session that manages data providers.</summary>
	[SupportedOSPlatform ("macos26.0")]
	public class ARSession : ARObject {

		[DllImport (Constants.ARKitLibrary)]
		static extern /* ar_session_t */ IntPtr ar_session_create_with_device (IntPtr device);

		[DllImport (Constants.ARKitLibrary)]
		static extern void ar_session_run (IntPtr session, IntPtr data_providers);

		[DllImport (Constants.ARKitLibrary)]
		static extern void ar_session_stop (IntPtr session);

		[DllImport (Constants.ARKitLibrary)]
		static extern /* ar_data_providers_t */ IntPtr ar_session_copy_data_providers (IntPtr session);

		[DllImport (Constants.ARKitLibrary)]
		unsafe static extern void ar_session_set_data_provider_state_change_handler_f (
			IntPtr session,
			IntPtr queue,
			void* context,
			delegate* unmanaged<void*, IntPtr, nint, IntPtr, IntPtr, void> handler);

		[Preserve (Conditional = true)]
		internal ARSession (NativeHandle handle, bool owns)
			: base (handle, owns)
		{
		}

		/// <summary>Creates a new ARKit session connected to the specified device.</summary>
		public ARSession (ARDevice device)
			: base (ar_session_create_with_device (device.GetCheckedHandle ()), owns: true)
		{
			GC.KeepAlive (device);
		}

		/// <summary>Runs the specified data providers on this session.</summary>
		public void Run (ARDataProviders dataProviders)
		{
			if (dataProviders is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (dataProviders));
			ar_session_run (GetCheckedHandle (), dataProviders.GetCheckedHandle ());
			GC.KeepAlive (dataProviders);
		}

		/// <summary>Stops all running data providers on this session.</summary>
		public void Stop ()
		{
			ar_session_stop (GetCheckedHandle ());
		}

		/// <summary>Gets a copy of the collection of all data providers on this session.</summary>
		public ARDataProviders CopyDataProviders ()
		{
			return new ARDataProviders (ar_session_copy_data_providers (GetCheckedHandle ()), owns: true);
		}

		/// <summary>Delegate for handling data provider state changes.</summary>
		public delegate void DataProviderStateChangeHandler (ARDataProviders dataProviders, ARDataProviderState newState, ARError? error, ARDataProvider? failedDataProvider);

		DataProviderStateChangeHandler? _stateChangeHandler;
		GCHandle _stateChangeGCHandle;

		/// <summary>Sets a handler for responding to data provider state changes.</summary>
		public void SetDataProviderStateChangeHandler (DispatchQueue? queue, DataProviderStateChangeHandler? handler)
		{
			var oldGCHandle = _stateChangeGCHandle;
			_stateChangeHandler = handler;

			if (handler is null) {
				_stateChangeGCHandle = default;
				unsafe {
					ar_session_set_data_provider_state_change_handler_f (
						GetCheckedHandle (),
						queue.GetHandle (),
						null,
						null);
				}
			} else {
				_stateChangeGCHandle = GCHandle.Alloc (handler);
				unsafe {
					delegate* unmanaged<void*, IntPtr, nint, IntPtr, IntPtr, void> callback = &StateChangeTrampoline;
					ar_session_set_data_provider_state_change_handler_f (
						GetCheckedHandle (),
						queue.GetHandle (),
						(void*) GCHandle.ToIntPtr (_stateChangeGCHandle),
						callback);
				}
			}

			// Free old GCHandle after setting the new native handler to avoid
			// a race where an in-flight callback uses a freed handle.
			if (oldGCHandle.IsAllocated)
				oldGCHandle.Free ();
			GC.KeepAlive (queue);
		}

		[UnmanagedCallersOnly]
		unsafe static void StateChangeTrampoline (void* context, IntPtr dataProviders, nint newState, IntPtr error, IntPtr failedDataProvider)
		{
			var handle = GCHandle.FromIntPtr ((IntPtr) context);
			var handler = (DataProviderStateChangeHandler) handle.Target!;
			handler (
				new ARDataProviders (dataProviders, owns: false),
				(ARDataProviderState) (long) newState,
				error == IntPtr.Zero ? null : new ARError (error, owns: false),
				failedDataProvider == IntPtr.Zero ? null : new ARDataProvider (failedDataProvider, owns: false));
		}

		protected override void Dispose (bool disposing)
		{
			if (_stateChangeGCHandle.IsAllocated)
				_stateChangeGCHandle.Free ();
			base.Dispose (disposing);
		}
	}
}

#endif // __MACOS__
