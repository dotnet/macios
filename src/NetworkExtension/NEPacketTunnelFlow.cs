#nullable enable

using System.Runtime.Versioning;
using Foundation;

namespace NetworkExtension {

	// needed for generated Async support
	//public delegate void NEPacketTunnelFlowReadHandler (NSData [] packets, NSNumber [] protocols);

	// avoid generator default `Arg1` and `Arg2` since Action<> was used
#if NET
	/// <summary>To be added.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos17.0")]
#endif
	public class NEPacketTunnelFlowReadResult {

#if !COREBUILD
		/// <param name="packets">To be added.</param>
		///         <param name="protocols">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
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
