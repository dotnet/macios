//
// NWPath.cs: Bindings the Netowrk nw_path_monitor_t API.
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

using OS_nw_path_monitor = System.IntPtr;

namespace Network {
	/// <summary>To be added.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("tvos")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	public class NWPathMonitor : NativeObject {
		[Preserve (Conditional = true)]
		internal NWPathMonitor (NativeHandle handle, bool owns) : base (handle, owns)
		{
			_SetUpdatedSnapshotHandler (SetUpdatedSnapshotHandlerWrapper);
		}

		[DllImport (Constants.NetworkLibrary)]
		extern static IntPtr nw_path_monitor_create ();

		NWPath? currentPath;
		public NWPath? CurrentPath => currentPath;

		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public NWPathMonitor ()
			: this (nw_path_monitor_create (), true)
		{
		}

		[DllImport (Constants.NetworkLibrary)]
		extern static IntPtr nw_path_monitor_create_with_type (NWInterfaceType interfaceType);

		/// <param name="interfaceType">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public NWPathMonitor (NWInterfaceType interfaceType)
			: this (nw_path_monitor_create_with_type (interfaceType), true)
		{
		}

		[DllImport (Constants.NetworkLibrary)]
		extern static void nw_path_monitor_cancel (IntPtr handle);

		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public void Cancel () => nw_path_monitor_cancel (GetCheckedHandle ());

		[DllImport (Constants.NetworkLibrary)]
		extern static void nw_path_monitor_start (IntPtr handle);

		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public void Start () => nw_path_monitor_start (GetCheckedHandle ());

		[DllImport (Constants.NetworkLibrary)]
		extern static void nw_path_monitor_set_queue (IntPtr handle, IntPtr queue);

		/// <param name="queue">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public void SetQueue (DispatchQueue queue)
		{
			if (queue is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (queue));
			nw_path_monitor_set_queue (GetCheckedHandle (), queue.Handle);
			GC.KeepAlive (queue);
		}

		[UnmanagedCallersOnly]
		static void TrampolineUpdatedSnapshot (IntPtr block, IntPtr path)
		{
			var del = BlockLiteral.GetTarget<Action<NWPath>> (block);
			if (del is not null) {
				var nwPath = new NWPath (path, owns: false);
				del (nwPath);
			}
		}

		[DllImport (Constants.NetworkLibrary)]
		static extern unsafe void nw_path_monitor_set_update_handler (IntPtr handle, BlockLiteral* callback);

		[BindingImpl (BindingImplOptions.Optimizable)]
		void _SetUpdatedSnapshotHandler (Action<NWPath> callback)
		{
			unsafe {
				if (callback is null) {
					nw_path_monitor_set_update_handler (GetCheckedHandle (), null);
					return;
				}

				delegate* unmanaged<IntPtr, IntPtr, void> trampoline = &TrampolineUpdatedSnapshot;
				using var block = new BlockLiteral (trampoline, callback, typeof (NWPathMonitor), nameof (TrampolineUpdatedSnapshot));
				nw_path_monitor_set_update_handler (GetCheckedHandle (), &block);
			}
		}

		Action<NWPath>? userSnapshotHandler;
		public Action<NWPath>? SnapshotHandler {
			get => userSnapshotHandler;
			set => userSnapshotHandler = value;
		}

		void SetUpdatedSnapshotHandlerWrapper (NWPath path)
		{
			currentPath = path;
			if (userSnapshotHandler is not null) {
				userSnapshotHandler (currentPath);
			}
		}

		[DllImport (Constants.NetworkLibrary)]
		static extern unsafe void nw_path_monitor_set_cancel_handler (IntPtr handle, BlockLiteral* callback);

		/// <param name="callback">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[BindingImpl (BindingImplOptions.Optimizable)]
		public void SetMonitorCanceledHandler (Action callback)
		{
			unsafe {
				if (callback is null) {
					nw_path_monitor_set_cancel_handler (GetCheckedHandle (), null);
					return;
				}

				using var block = BlockStaticDispatchClass.CreateBlock (callback);
				nw_path_monitor_set_cancel_handler (GetCheckedHandle (), &block);
			}
		}

		[SupportedOSPlatform ("tvos15.0")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios15.0")]
		[SupportedOSPlatform ("maccatalyst")]
		[DllImport (Constants.NetworkLibrary)]
		static extern void nw_path_monitor_prohibit_interface_type (OS_nw_path_monitor monitor, NWInterfaceType interfaceType);

		[SupportedOSPlatform ("tvos15.0")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios15.0")]
		[SupportedOSPlatform ("maccatalyst")]
		public void ProhibitInterfaceType (NWInterfaceType interfaceType)
			=> nw_path_monitor_prohibit_interface_type (GetCheckedHandle (), interfaceType);

#if MONOMAC
		[SupportedOSPlatform ("macos13.0")]
		[UnsupportedOSPlatform ("tvos")]
		[UnsupportedOSPlatform ("ios")]
		[DllImport (Constants.NetworkLibrary)]
		static extern OS_nw_path_monitor nw_path_monitor_create_for_ethernet_channel ();

		[SupportedOSPlatform ("macos13.0")]
		[UnsupportedOSPlatform ("tvos")]
		[UnsupportedOSPlatform ("ios")]
		[UnsupportedOSPlatform ("maccatalyst")]
		public static NWPathMonitor CreateForEthernetChannel ()
			=> new NWPathMonitor (nw_path_monitor_create_for_ethernet_channel (), true);
#endif
	}

}
