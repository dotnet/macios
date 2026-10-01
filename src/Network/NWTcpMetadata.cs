//
// NWTcpMetadata.cs: Bindings the Netowrk nw_protocol_metadata_t API that is an Tcp.
//
// Authors:
//   Manuel de la Pena <mandel@microsoft.com>
//
// Copyrigh 2019 Microsoft
//

#nullable enable

using CoreFoundation;

namespace Network {
	[SupportedOSPlatform ("tvos")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	public class NWTcpMetadata : NWProtocolMetadata {

		[Preserve (Conditional = true)]
		internal NWTcpMetadata (NativeHandle handle, bool owns) : base (handle, owns) { }

		public uint AvailableReceiveBuffer => nw_tcp_get_available_receive_buffer (GetCheckedHandle ());

		public uint AvailableSendBuffer => nw_tcp_get_available_send_buffer (GetCheckedHandle ());

		[SupportedOSPlatform ("tvos27.0")]
		[SupportedOSPlatform ("macos27.0")]
		[SupportedOSPlatform ("ios27.0")]
		[SupportedOSPlatform ("maccatalyst27.0")]
		[DllImport (Constants.NetworkLibrary)]
		static extern int nw_tcp_set_max_pacing_rate (IntPtr metadata, ulong maximumPacingRate);

		/// <summary>Sets the maximum pacing rate for TCP transmissions.</summary>
		/// <param name="maximumPacingRate">The maximum pacing rate, in bytes per second.</param>
		/// <returns>Zero on success; otherwise, a POSIX error code.</returns>
		/// <remarks>
		/// A value of 0 or <c>ulong.MaxValue</c> disables pacing. Values between 1 and 12,499 are clamped to 12,500.
		/// Each call replaces the previous pacing rate.
		/// </remarks>
		[SupportedOSPlatform ("tvos27.0")]
		[SupportedOSPlatform ("macos27.0")]
		[SupportedOSPlatform ("ios27.0")]
		[SupportedOSPlatform ("maccatalyst27.0")]
		public int SetMaximumPacingRate (ulong maximumPacingRate)
			=> nw_tcp_set_max_pacing_rate (GetCheckedHandle (), maximumPacingRate);
	}
}
