//
// NWEndpoint.cs: Bindings the Netowrk nw_endpoint_t API.
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

using OS_nw_protocol_definition = System.IntPtr;

namespace Network {
	/// <summary>To be added.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("tvos")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	public class NWProtocolDefinition : NativeObject {
		[Preserve (Conditional = true)]
		internal NWProtocolDefinition (NativeHandle handle, bool owns) : base (handle, owns) { }

		[DllImport (Constants.NetworkLibrary)]
		static extern byte nw_protocol_definition_is_equal (OS_nw_protocol_definition definition1, OS_nw_protocol_definition definition2);

		/// <param name="other">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public bool Equals (object other)
		{
			if (other is null)
				return false;
			if (!(other is NWProtocolDefinition otherDefinition))
				return false;
			bool result = nw_protocol_definition_is_equal (GetCheckedHandle (), otherDefinition.Handle) != 0;
			GC.KeepAlive (otherDefinition);
			return result;
		}

		[DllImport (Constants.NetworkLibrary)]
		static extern OS_nw_protocol_definition nw_protocol_copy_ip_definition ();

		public static NWProtocolDefinition CreateIPDefinition () => new NWProtocolDefinition (nw_protocol_copy_ip_definition (), owns: true);

		[DllImport (Constants.NetworkLibrary)]
		static extern OS_nw_protocol_definition nw_protocol_copy_tcp_definition ();

		public static NWProtocolDefinition CreateTcpDefinition () => new NWProtocolDefinition (nw_protocol_copy_tcp_definition (), owns: true);

		[DllImport (Constants.NetworkLibrary)]
		static extern OS_nw_protocol_definition nw_protocol_copy_udp_definition ();

		public static NWProtocolDefinition CreateUdpDefinition () => new NWProtocolDefinition (nw_protocol_copy_udp_definition (), owns: true);

		[DllImport (Constants.NetworkLibrary)]
		static extern OS_nw_protocol_definition nw_protocol_copy_tls_definition ();

		public static NWProtocolDefinition CreateTlsDefinition () => new NWProtocolDefinition (nw_protocol_copy_tls_definition (), owns: true);

		[SupportedOSPlatform ("tvos13.0")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios13.0")]
		[SupportedOSPlatform ("maccatalyst")]
		[DllImport (Constants.NetworkLibrary)]
		static extern OS_nw_protocol_definition nw_protocol_copy_ws_definition ();

		[SupportedOSPlatform ("tvos13.0")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios13.0")]
		[SupportedOSPlatform ("maccatalyst")]
		public static NWProtocolDefinition CreateWebSocketDefinition () => new NWProtocolDefinition (nw_protocol_copy_ws_definition (), owns: true);

		[SupportedOSPlatform ("tvos13.0")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios13.0")]
		[SupportedOSPlatform ("maccatalyst")]
		[DllImport (Constants.NetworkLibrary)]
		static extern unsafe OS_nw_protocol_definition nw_framer_create_definition (IntPtr identifier, NWFramerCreateFlags flags, BlockLiteral* start_handler);

		[UnmanagedCallersOnly]
		[SupportedOSPlatform ("tvos13.0")]
		[SupportedOSPlatform ("ios13.0")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		static NWFramerStartResult TrampolineCreateFramerHandler (IntPtr block, IntPtr framer)
		{
			// get and call, this is internal and we are trying to do all the magic in the call
			var del = BlockLiteral.GetTarget<Func<NWFramer, NWFramerStartResult>> (block);
			if (del is not null) {
				var nwFramer = new NWFramer (framer, owns: true);
				return del (nwFramer);
			}
			return NWFramerStartResult.Unknown;
		}

		[SupportedOSPlatform ("tvos13.0")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios13.0")]
		[SupportedOSPlatform ("maccatalyst")]
		[BindingImpl (BindingImplOptions.Optimizable)]
		public static NWProtocolDefinition CreateFramerDefinition (string identifier, NWFramerCreateFlags flags, Func<NWFramer, NWFramerStartResult> startCallback)
		{
			unsafe {
				delegate* unmanaged<IntPtr, IntPtr, NWFramerStartResult> trampoline = &TrampolineCreateFramerHandler;
				using var block = new BlockLiteral (trampoline, startCallback, typeof (NWProtocolDefinition), nameof (TrampolineCreateFramerHandler));
				using var identifierPtr = new TransientString (identifier);
				return new NWProtocolDefinition (nw_framer_create_definition (identifierPtr, flags, &block), owns: true);
			}
		}

		[SupportedOSPlatform ("tvos15.0")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios15.0")]
		[SupportedOSPlatform ("maccatalyst")]
		[DllImport (Constants.NetworkLibrary)]
		static extern OS_nw_protocol_definition nw_protocol_copy_quic_definition ();

		[SupportedOSPlatform ("tvos15.0")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios15.0")]
		[SupportedOSPlatform ("maccatalyst")]
		public static NWProtocolDefinition CreateQuicDefinition () => new NWProtocolDefinition (nw_protocol_copy_quic_definition (), owns: true);
	}
}
