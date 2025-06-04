//
// NWProtocolMetadata.cs: Bindings the Netowrk nw_protocol_metadata_t API.
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
using Security;
using OS_nw_protocol_definition = System.IntPtr;
using OS_nw_protocol_metadata = System.IntPtr;
using nw_service_class_t = System.IntPtr;

namespace Network {
	/// <summary>To be added.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("tvos")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	public class NWProtocolMetadata : NativeObject {

		[DllImport (Constants.NetworkLibrary)]
		internal static extern OS_nw_protocol_metadata nw_ip_create_metadata ();

		[DllImport (Constants.NetworkLibrary)]
		internal static extern OS_nw_protocol_metadata nw_udp_create_metadata ();

		[Preserve (Conditional = true)]
		internal NWProtocolMetadata (NativeHandle handle, bool owns) : base (handle, owns) { }

		[DllImport (Constants.NetworkLibrary)]
		internal static extern OS_nw_protocol_definition nw_protocol_metadata_copy_definition (OS_nw_protocol_metadata metadata);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public NWProtocolDefinition ProtocolDefinition => new NWProtocolDefinition (nw_protocol_metadata_copy_definition (GetCheckedHandle ()), owns: true);

		[DllImport (Constants.NetworkLibrary)]
		internal static extern byte nw_protocol_metadata_is_ip (OS_nw_protocol_metadata metadata);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public bool IsIP => nw_protocol_metadata_is_ip (GetCheckedHandle ()) != 0;

		[DllImport (Constants.NetworkLibrary)]
		internal static extern byte nw_protocol_metadata_is_udp (OS_nw_protocol_metadata metadata);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public bool IsUdp => nw_protocol_metadata_is_udp (GetCheckedHandle ()) != 0;

		[DllImport (Constants.NetworkLibrary)]
		internal static extern byte nw_protocol_metadata_is_tls (OS_nw_protocol_metadata metadata);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public bool IsTls => nw_protocol_metadata_is_tls (GetCheckedHandle ()) != 0;

		[DllImport (Constants.NetworkLibrary)]
		internal static extern byte nw_protocol_metadata_is_tcp (OS_nw_protocol_metadata metadata);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public bool IsTcp => nw_protocol_metadata_is_tcp (GetCheckedHandle ()) != 0;

		[SupportedOSPlatform ("tvos15.0")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios15.0")]
		[SupportedOSPlatform ("maccatalyst")]
		[DllImport (Constants.NetworkLibrary)]
		static extern byte nw_protocol_metadata_is_quic (OS_nw_protocol_metadata metadata);

		[SupportedOSPlatform ("tvos15.0")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios15.0")]
		[SupportedOSPlatform ("maccatalyst")]
		public bool IsQuic => nw_protocol_metadata_is_quic (GetCheckedHandle ()) != 0;

		[DllImport (Constants.NetworkLibrary)]
		internal static extern IntPtr nw_tls_copy_sec_protocol_metadata (IntPtr handle);

		void CheckIsIP ()
		{
			if (!IsIP)
				throw new InvalidOperationException ("This metadata is not IP metadata.");
		}

		void CheckIsTcp ()
		{
			if (!IsTcp)
				throw new InvalidOperationException ("This metadata is not TCP metadata.");
		}

		void CheckIsTls ()
		{
			if (!IsTls)
				throw new InvalidOperationException ("This metadata is not TLS metadata.");
		}

		[DllImport (Constants.NetworkLibrary)]
		internal static extern void nw_ip_metadata_set_ecn_flag (OS_nw_protocol_metadata metadata, NWIPEcnFlag ecn_flag);

		[DllImport (Constants.NetworkLibrary)]
		internal static extern NWIPEcnFlag nw_ip_metadata_get_ecn_flag (OS_nw_protocol_metadata metadata);

		[DllImport (Constants.NetworkLibrary)]
		internal static extern /* uint64_t */ ulong nw_ip_metadata_get_receive_time (OS_nw_protocol_metadata metadata);

		[DllImport (Constants.NetworkLibrary)]
		internal static extern void nw_ip_metadata_set_service_class (OS_nw_protocol_metadata metadata, NWServiceClass service_class);

		[DllImport (Constants.NetworkLibrary)]
		internal static extern NWServiceClass nw_ip_metadata_get_service_class (OS_nw_protocol_metadata metadata);

		[DllImport (Constants.NetworkLibrary)]
		internal extern static /* uint32_t */ uint nw_tcp_get_available_receive_buffer (IntPtr handle);

		[DllImport (Constants.NetworkLibrary)]
		internal extern static /* uint32_t */ uint nw_tcp_get_available_send_buffer (IntPtr handle);

		[SupportedOSPlatform ("tvos13.0")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios13.0")]
		[SupportedOSPlatform ("maccatalyst")]
		[DllImport (Constants.NetworkLibrary)]
		internal static extern byte nw_protocol_metadata_is_framer_message (OS_nw_protocol_metadata metadata);

		[SupportedOSPlatform ("tvos13.0")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios13.0")]
		[SupportedOSPlatform ("maccatalyst")]
		public bool IsFramerMessage => nw_protocol_metadata_is_framer_message (GetCheckedHandle ()) != 0;

		[SupportedOSPlatform ("tvos13.0")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios13.0")]
		[SupportedOSPlatform ("maccatalyst")]
		[DllImport (Constants.NetworkLibrary)]
		internal static extern byte nw_protocol_metadata_is_ws (OS_nw_protocol_metadata metadata);

		[SupportedOSPlatform ("tvos13.0")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios13.0")]
		[SupportedOSPlatform ("maccatalyst")]
		public bool IsWebSocket => nw_protocol_metadata_is_ws (GetCheckedHandle ()) != 0;
	}
}
