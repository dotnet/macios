#nullable enable

using System.Runtime.Versioning;
using Foundation;

namespace NetworkExtension {

	// needed for generated Async support
	//public delegate void NEPacketTunnelFlowReadHandler (NSData [] packets, NSNumber [] protocols);

	// avoid generator default `Arg1` and `Arg2` since Action<> was used
#if NET
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos17.0")]
#endif
	public class NEPacketTunnelFlowReadResult {

#if !COREBUILD
		public NEPacketTunnelFlowReadResult (NSData [] packets, NSNumber [] protocols)
		{
			Packets = packets;
			Protocols = protocols;
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public NSData [] Packets { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public NSNumber [] Protocols { get; set; }
#endif
	}
}
