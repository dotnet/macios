//
// NWProtocolOptions.cs: Bindings the Netowrk nw_protocol_options API.
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
using IntPtr = System.IntPtr;

namespace Network {
	/// <summary>To be added.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("tvos")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	public class NWProtocolOptions : NativeObject {
		[Preserve (Conditional = true)]
		internal NWProtocolOptions (NativeHandle handle, bool owns) : base (handle, owns) { }

		[DllImport (Constants.NetworkLibrary)]
		internal static extern OS_nw_protocol_definition nw_protocol_options_copy_definition (IntPtr options);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public NWProtocolDefinition ProtocolDefinition => new NWProtocolDefinition (nw_protocol_options_copy_definition (GetCheckedHandle ()), owns: true);

		[DllImport (Constants.NetworkLibrary)]
		internal static extern IntPtr nw_tls_create_options ();

		[DllImport (Constants.NetworkLibrary)]
		internal static extern IntPtr nw_tcp_create_options ();

		[DllImport (Constants.NetworkLibrary)]
		internal static extern IntPtr nw_udp_create_options ();

		// added to have a consistent API, but obsolete it

		[SupportedOSPlatform ("tvos15.0")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios15.0")]
		[SupportedOSPlatform ("maccatalyst")]
		[DllImport (Constants.NetworkLibrary)]
		internal static extern IntPtr nw_quic_create_options ();

		//
		// IP Options
		//
		[DllImport (Constants.NetworkLibrary)]
		internal static extern void nw_ip_options_set_version (IntPtr options, NWIPVersion version);

		[DllImport (Constants.NetworkLibrary)]
		internal static extern void nw_ip_options_set_hop_limit (IntPtr options, byte hop_limit);

		[DllImport (Constants.NetworkLibrary)]
		internal static extern void nw_ip_options_set_use_minimum_mtu (IntPtr options, byte use_minimum_mtu);

		[DllImport (Constants.NetworkLibrary)]
		internal static extern void nw_ip_options_set_disable_fragmentation (IntPtr options, byte disable_fragmentation);

		[DllImport (Constants.NetworkLibrary)]
		internal static extern void nw_ip_options_set_calculate_receive_time (IntPtr options, byte calculateReceiveTime);

		[SupportedOSPlatform ("tvos13.0")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios13.0")]
		[SupportedOSPlatform ("maccatalyst")]
		[DllImport (Constants.NetworkLibrary)]
		internal static extern void nw_ip_options_set_local_address_preference (IntPtr options, NWIPLocalAddressPreference preference);

		[SupportedOSPlatform ("tvos13.0")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios13.0")]
		[SupportedOSPlatform ("maccatalyst")]
		public NWIPLocalAddressPreference IPLocalAddressPreference {
			set => nw_ip_options_set_local_address_preference (GetCheckedHandle (), value);
		}
		//
		// TCP Options
		//

		[DllImport (Constants.NetworkLibrary)]
		internal extern static void nw_tcp_options_set_no_delay (IntPtr handle, byte noDelay);

		[DllImport (Constants.NetworkLibrary)]
		internal extern static void nw_tcp_options_set_no_push (IntPtr handle, byte noPush);

		[DllImport (Constants.NetworkLibrary)]
		internal extern static void nw_tcp_options_set_no_options (IntPtr handle, byte noOptions);

		[DllImport (Constants.NetworkLibrary)]
		internal extern static void nw_tcp_options_set_enable_keepalive (IntPtr handle, byte enableKeepAlive);

		[DllImport (Constants.NetworkLibrary)]
		internal extern static void nw_tcp_options_set_keepalive_count (IntPtr handle, uint keepaliveCount);

		[DllImport (Constants.NetworkLibrary)]
		internal extern static void nw_tcp_options_set_keepalive_idle_time (IntPtr handle, uint keepaliveIdleTime);

		[DllImport (Constants.NetworkLibrary)]
		internal extern static void nw_tcp_options_set_keepalive_interval (IntPtr handle, uint keepaliveInterval);

		[DllImport (Constants.NetworkLibrary)]
		internal extern static void nw_tcp_options_set_maximum_segment_size (IntPtr handle, uint maximumSegmentSize);

		[DllImport (Constants.NetworkLibrary)]
		internal extern static void nw_tcp_options_set_connection_timeout (IntPtr handle, uint connectionTimeout);

		[DllImport (Constants.NetworkLibrary)]
		internal extern static void nw_tcp_options_set_persist_timeout (IntPtr handle, uint persistTimeout);

		[DllImport (Constants.NetworkLibrary)]
		internal extern static void nw_tcp_options_set_retransmit_connection_drop_time (IntPtr handle, uint retransmitConnectionDropTime);

		[DllImport (Constants.NetworkLibrary)]
		internal extern static void nw_tcp_options_set_retransmit_fin_drop (IntPtr handle, byte retransmitFinDrop);

		[DllImport (Constants.NetworkLibrary)]
		internal extern static void nw_tcp_options_set_disable_ack_stretching (IntPtr handle, byte disableAckStretching);

		[DllImport (Constants.NetworkLibrary)]
		internal extern static void nw_tcp_options_set_enable_fast_open (IntPtr handle, byte enableFastOpen);

		[DllImport (Constants.NetworkLibrary)]
		internal extern static void nw_tcp_options_set_disable_ecn (IntPtr handle, byte disableEcn);

		//
		// UDP Options
		//
		[DllImport (Constants.NetworkLibrary)]
		internal extern static void nw_udp_options_set_prefer_no_checksum (IntPtr handle, byte preferNoChecksums);

		//
		// TLS options
		//

		[DllImport (Constants.NetworkLibrary)]
		internal extern static IntPtr nw_tls_copy_sec_protocol_options (IntPtr options);

		[SupportedOSPlatform ("tvos15.0")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios15.0")]
		[SupportedOSPlatform ("maccatalyst")]
		[DllImport (Constants.NetworkLibrary)]
		static extern byte nw_protocol_options_is_quic (IntPtr options);

		[SupportedOSPlatform ("tvos15.0")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios15.0")]
		[SupportedOSPlatform ("maccatalyst")]
		public bool IsQuic => nw_protocol_options_is_quic (GetCheckedHandle ()) != 0;
	}
}
