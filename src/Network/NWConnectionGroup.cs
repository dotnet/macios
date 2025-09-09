using System;
using System.Runtime.InteropServices;
using ObjCRuntime;
using Foundation;
using CoreFoundation;
using OS_nw_connection = System.IntPtr;
using OS_nw_connection_group = System.IntPtr;
using OS_nw_group_descriptor = System.IntPtr;
using OS_nw_parameters = System.IntPtr;
using OS_nw_content_context = System.IntPtr;
using OS_nw_path = System.IntPtr;
using OS_nw_endpoint = System.IntPtr;
using OS_nw_protocol_metadata = System.IntPtr;
using OS_nw_protocol_definition = System.IntPtr;
using OS_nw_protocol_options = System.IntPtr;

#nullable enable

namespace Network {
	public delegate void NWConnectionGroupReceiveDelegate (DispatchData content, NWContentContext context, bool isCompleted);

	public delegate void NWConnectionGroupStateChangedDelegate (NWConnectionGroupState state, NWError? error);

	[SupportedOSPlatform ("tvos14.0")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("ios14.0")]
	[SupportedOSPlatform ("maccatalyst")]
	public class NWConnectionGroup : NativeObject {
		[Preserve (Conditional = true)]
		protected internal NWConnectionGroup (NativeHandle handle, bool owns) : base (handle, owns) { }

		[DllImport (Constants.NetworkLibrary)]
		static extern OS_nw_connection_group nw_connection_group_create (OS_nw_group_descriptor group_descriptor, OS_nw_parameters parameters);

		public NWConnectionGroup (NWMulticastGroup groupDescriptor, NWParameters parameters)
		{
			if (groupDescriptor is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (groupDescriptor));
			if (parameters is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (parameters));

			InitializeHandle (nw_connection_group_create (groupDescriptor.GetCheckedHandle (), parameters.GetCheckedHandle ()));
			GC.KeepAlive (groupDescriptor);
			GC.KeepAlive (parameters);
		}

		[DllImport (Constants.NetworkLibrary)]
		static extern OS_nw_group_descriptor nw_connection_group_copy_descriptor (OS_nw_connection_group group);

		public NWMulticastGroup? GroupDescriptor {
			get {
				var x = nw_connection_group_copy_descriptor (GetCheckedHandle ());
				if (x == IntPtr.Zero)
					return null;
				return new NWMulticastGroup (x, owns: true);
			}
		}

		[DllImport (Constants.NetworkLibrary)]
		static extern OS_nw_parameters nw_connection_group_copy_parameters (OS_nw_connection_group group);

		public NWParameters? Parameters {
			get {
				var x = nw_connection_group_copy_parameters (GetCheckedHandle ());
				if (x == IntPtr.Zero)
					return null;
				return new NWParameters (x, owns: true);
			}
		}

		[DllImport (Constants.NetworkLibrary)]
		static extern void nw_connection_group_start (OS_nw_connection_group group);

		public void Start () => nw_connection_group_start (GetCheckedHandle ());

		[DllImport (Constants.NetworkLibrary)]
		static extern void nw_connection_group_cancel (OS_nw_connection_group group);

		public void Cancel () => nw_connection_group_cancel (GetCheckedHandle ());

		[DllImport (Constants.NetworkLibrary)]
		static extern void nw_connection_group_set_queue (OS_nw_connection_group group, IntPtr queue);

		public void SetQueue (DispatchQueue queue)
		{
			if (queue is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (queue));

			nw_connection_group_set_queue (GetCheckedHandle (), queue.GetCheckedHandle ());
			GC.KeepAlive (queue);
		}

		// can return null
		[DllImport (Constants.NetworkLibrary)]
		static extern OS_nw_endpoint nw_connection_group_copy_local_endpoint_for_message (OS_nw_connection_group group, OS_nw_content_context context);

		public NWEndpoint? GetLocalEndpoint (NWContentContext context)
		{
			if (context is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (context));
			var ptr = nw_connection_group_copy_local_endpoint_for_message (GetCheckedHandle (), context.GetCheckedHandle ());
			GC.KeepAlive (context);
			return ptr == IntPtr.Zero ? null : new NWEndpoint (ptr, owns: true);
		}

		// can return null
		[DllImport (Constants.NetworkLibrary)]
		static extern OS_nw_path nw_connection_group_copy_path_for_message (OS_nw_connection_group group, OS_nw_content_context context);

		public NWPath? GetPath (NWContentContext context)
		{
			if (context is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (context));
			var ptr = nw_connection_group_copy_path_for_message (GetCheckedHandle (), context.GetCheckedHandle ());
			GC.KeepAlive (context);
			return ptr == IntPtr.Zero ? null : new NWPath (ptr, owns: true);
		}

		// can return null
		[DllImport (Constants.NetworkLibrary)]
		static extern OS_nw_endpoint nw_connection_group_copy_remote_endpoint_for_message (OS_nw_connection_group group, OS_nw_content_context context);

		public NWEndpoint? GetRemmoteEndpoint (NWContentContext context)
		{
			if (context is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (context));
			var ptr = nw_connection_group_copy_remote_endpoint_for_message (GetCheckedHandle (), context.GetCheckedHandle ());
			GC.KeepAlive (context);
			return ptr == IntPtr.Zero ? null : new NWEndpoint (ptr, owns: true);
		}

		// can return null
		[DllImport (Constants.NetworkLibrary)]
		static extern OS_nw_connection nw_connection_group_extract_connection_for_message (OS_nw_connection_group group, OS_nw_content_context context);

		public NWConnection? GetConnection (NWContentContext context)
		{
			if (context is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (context));
			var ptr = nw_connection_group_extract_connection_for_message (GetCheckedHandle (), context.GetCheckedHandle ());
			GC.KeepAlive (context);
			return ptr == IntPtr.Zero ? null : new NWConnection (ptr, owns: true);
		}

		[DllImport (Constants.NetworkLibrary)]
		static extern void nw_connection_group_reply (OS_nw_connection_group group, OS_nw_content_context inbound_message, OS_nw_content_context outbound_message, /* [NullAllowed]  DispatchData */ IntPtr content);

		public void Reply (NWContentContext inboundMessage, NWContentContext outboundMessage, DispatchData content)
		{
			if (inboundMessage is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (inboundMessage));
			if (outboundMessage is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (outboundMessage));

			nw_connection_group_reply (GetCheckedHandle (), inboundMessage.GetCheckedHandle (), outboundMessage.GetCheckedHandle (), content.GetHandle ());
			GC.KeepAlive (inboundMessage);
			GC.KeepAlive (outboundMessage);
			GC.KeepAlive (content);
		}

		[DllImport (Constants.NetworkLibrary)]
		unsafe static extern void nw_connection_group_send_message (OS_nw_connection_group group, /* [NullAllowed] DispatchData */ IntPtr content, /* [NullAllowed] */ OS_nw_endpoint endpoint, OS_nw_content_context context, BlockLiteral* handler);

		[UnmanagedCallersOnly]
		static void TrampolineSendCompletion (IntPtr block, IntPtr error)
		{
			var del = BlockLiteral.GetTarget<Action<NWError?>> (block);
			if (del is not null) {
				using var err = error == IntPtr.Zero ? null : new NWError (error, owns: false);
				del (err);
			}
		}

		[BindingImpl (BindingImplOptions.Optimizable)]
		public void Send (DispatchData? content, NWEndpoint? endpoint, NWContentContext context, Action<NWError?>? handler)
		{
			unsafe {
				if (handler is null) {
					nw_connection_group_send_message (GetCheckedHandle (),
						content.GetHandle (),
						endpoint.GetHandle (),
						context.GetCheckedHandle (),
						null);
					GC.KeepAlive (content);
					GC.KeepAlive (endpoint);
					GC.KeepAlive (context);
					return;
				}

				delegate* unmanaged<IntPtr, IntPtr, void> trampoline = &TrampolineSendCompletion;
				using var block = new BlockLiteral (trampoline, handler, typeof (NWConnectionGroup), nameof (TrampolineSendCompletion));
				nw_connection_group_send_message (GetCheckedHandle (),
					content.GetHandle (),
					endpoint.GetHandle (),
					context.GetCheckedHandle (),
					&block);
				GC.KeepAlive (content);
				GC.KeepAlive (endpoint);
				GC.KeepAlive (context);
			}
		}

		[DllImport (Constants.NetworkLibrary)]
		unsafe static extern void nw_connection_group_set_receive_handler (OS_nw_connection_group group, uint maximum_message_size, byte reject_oversized_messages, BlockLiteral* handler);

		[UnmanagedCallersOnly]
		static void TrampolineReceiveHandler (IntPtr block, IntPtr content, IntPtr context, byte isCompleted)
		{
			var del = BlockLiteral.GetTarget<NWConnectionGroupReceiveDelegate> (block);
			if (del is not null) {
				using var nsContent = new DispatchData (content, owns: false);
				using var nsContext = new NWContentContext (context, owns: false);
				del (nsContent, nsContext, isCompleted != 0);
			}
		}

		[BindingImpl (BindingImplOptions.Optimizable)]
		public void SetReceiveHandler (uint maximumMessageSize, bool rejectOversizedMessages, NWConnectionGroupReceiveDelegate? handler)
		{
			unsafe {
				if (handler is null) {
					nw_connection_group_set_receive_handler (GetCheckedHandle (), maximumMessageSize, rejectOversizedMessages.AsByte (), null);
					return;
				}

				delegate* unmanaged<IntPtr, IntPtr, IntPtr, byte, void> trampoline = &TrampolineReceiveHandler;
				using var block = new BlockLiteral (trampoline, handler, typeof (NWConnectionGroup), nameof (TrampolineReceiveHandler));
				nw_connection_group_set_receive_handler (GetCheckedHandle (), maximumMessageSize, rejectOversizedMessages.AsByte (), &block);
			}
		}

		[DllImport (Constants.NetworkLibrary)]
		unsafe static extern void nw_connection_group_set_state_changed_handler (OS_nw_connection_group group, BlockLiteral* handler);

		[UnmanagedCallersOnly]
		static void TrampolineStateChangedHandler (IntPtr block, NWConnectionGroupState state, IntPtr error)
		{
			var del = BlockLiteral.GetTarget<NWConnectionGroupStateChangedDelegate> (block);
			if (del is not null) {
				using var nwError = (error == IntPtr.Zero) ? null : new NWError (error, owns: false);
				del (state, nwError);
			}
		}

		[BindingImpl (BindingImplOptions.Optimizable)]
		public void SetStateChangedHandler (NWConnectionGroupStateChangedDelegate handler)
		{
			unsafe {
				if (handler is null) {
					nw_connection_group_set_state_changed_handler (GetCheckedHandle (), null);
					return;
				}

				delegate* unmanaged<IntPtr, NWConnectionGroupState, IntPtr, void> trampoline = &TrampolineStateChangedHandler;
				using var block = new BlockLiteral (trampoline, handler, typeof (NWConnectionGroup), nameof (TrampolineStateChangedHandler));
				nw_connection_group_set_state_changed_handler (GetCheckedHandle (), &block);
			}
		}

		[SupportedOSPlatform ("tvos15.0")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios15.0")]
		[SupportedOSPlatform ("maccatalyst")]
		[DllImport (Constants.NetworkLibrary)]
		static extern OS_nw_protocol_metadata nw_connection_group_copy_protocol_metadata (OS_nw_connection_group group, OS_nw_protocol_definition definition);

		[SupportedOSPlatform ("tvos15.0")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios15.0")]
		[SupportedOSPlatform ("maccatalyst")]
		public NWProtocolMetadata? GetProtocolMetadata (NWContentContext context)
		{
			if (context is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (context));
			var ptr = nw_connection_group_copy_protocol_metadata (GetCheckedHandle (), context.Handle);
			GC.KeepAlive (context);
			return ptr == IntPtr.Zero ? null : new NWProtocolMetadata (ptr, true);
		}

		[SupportedOSPlatform ("tvos15.0")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios15.0")]
		[SupportedOSPlatform ("maccatalyst")]
		[DllImport (Constants.NetworkLibrary)]
		static extern OS_nw_protocol_metadata nw_connection_group_copy_protocol_metadata_for_message (OS_nw_connection_group group, OS_nw_content_context context, OS_nw_protocol_definition definition);

		[SupportedOSPlatform ("tvos15.0")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios15.0")]
		[SupportedOSPlatform ("maccatalyst")]
		public NWProtocolMetadata? GetProtocolMetadata (NWContentContext context, NWProtocolDefinition definition)
		{
			if (context is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (context));
			if (definition is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (context));
			var ptr = nw_connection_group_copy_protocol_metadata_for_message (GetCheckedHandle (), context.Handle, definition.Handle);
			GC.KeepAlive (context);
			GC.KeepAlive (definition);
			return ptr == IntPtr.Zero ? null : new NWProtocolMetadata (ptr, true);
		}

		[SupportedOSPlatform ("tvos15.0")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios15.0")]
		[SupportedOSPlatform ("maccatalyst")]
		[DllImport (Constants.NetworkLibrary)]
		static extern OS_nw_connection nw_connection_group_extract_connection (OS_nw_connection_group group, OS_nw_endpoint endpoint, OS_nw_protocol_options protocolOptions);

		[SupportedOSPlatform ("tvos15.0")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios15.0")]
		[SupportedOSPlatform ("maccatalyst")]
		public NWConnection? ExtractConnection (NWEndpoint endpoint, NWProtocolOptions protocolOptions)
		{
			var ptr = nw_connection_group_extract_connection (GetCheckedHandle (), endpoint.GetCheckedHandle (), protocolOptions.GetCheckedHandle ());
			GC.KeepAlive (endpoint);
			GC.KeepAlive (protocolOptions);
			return ptr == IntPtr.Zero ? null : new NWConnection (ptr, true);
		}

		[SupportedOSPlatform ("tvos15.0")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios15.0")]
		[SupportedOSPlatform ("maccatalyst")]
		[DllImport (Constants.NetworkLibrary)]
		static extern byte nw_connection_group_reinsert_extracted_connection (OS_nw_connection_group group, OS_nw_connection connection);

		[SupportedOSPlatform ("tvos15.0")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios15.0")]
		[SupportedOSPlatform ("maccatalyst")]
		public bool TryReinsertExtractedConnection (NWConnection connection)
		{
			if (connection is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (connection));
			bool result = nw_connection_group_reinsert_extracted_connection (GetCheckedHandle (), connection.Handle) != 0;
			GC.KeepAlive (connection);
			return result;
		}

		[SupportedOSPlatform ("tvos15.0")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios15.0")]
		[SupportedOSPlatform ("maccatalyst")]
		[DllImport (Constants.NetworkLibrary)]
		unsafe static extern void nw_connection_group_set_new_connection_handler (OS_nw_connection_group group, BlockLiteral* connectionHandler);

		[UnmanagedCallersOnly]
		static void TrampolineSetNewConnectionHandler (IntPtr block, IntPtr connection)
		{
			var del = BlockLiteral.GetTarget<Action<NWConnection>> (block);
			if (del is not null) {
				// the ownership of the object is for the caller
				using var nwReport = new NWConnection (connection, owns: true);
				del (nwReport);
			}
		}

		[SupportedOSPlatform ("tvos15.0")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios15.0")]
		[SupportedOSPlatform ("maccatalyst")]
		[BindingImpl (BindingImplOptions.Optimizable)]
		public void SetNewConnectionHandler (Action<NWConnection> handler)
		{
			if (handler is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (handler));

			unsafe {
				delegate* unmanaged<IntPtr, IntPtr, void> trampoline = &TrampolineSetNewConnectionHandler;
				using var block = new BlockLiteral (trampoline, handler, typeof (NWConnectionGroup), nameof (TrampolineSetNewConnectionHandler));
				nw_connection_group_set_new_connection_handler (GetCheckedHandle (), &block);
			}
		}
	}
}
