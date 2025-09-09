#nullable enable

using System;
using System.Runtime.InteropServices;

using Foundation;
using ObjCRuntime;
using CoreFoundation;

using OS_nw_resolver_config = System.IntPtr;
using OS_nw_endpoint = System.IntPtr;

namespace Network {
	[SupportedOSPlatform ("tvos15.0")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("ios15.0")]
	[SupportedOSPlatform ("maccatalyst")]
	public class NWResolverConfig : NativeObject {

		[Preserve (Conditional = true)]
		internal NWResolverConfig (NativeHandle handle, bool owns) : base (handle, owns) { }

		[DllImport (Constants.NetworkLibrary)]
		static extern OS_nw_resolver_config nw_resolver_config_create_https (OS_nw_endpoint urlEndpoint);

		[DllImport (Constants.NetworkLibrary)]
		static extern OS_nw_resolver_config nw_resolver_config_create_tls (OS_nw_endpoint serverEndpoint);

		public NWResolverConfig (NWEndpoint urlEndpoint, NWResolverConfigEndpointType endpointType)
		{
			if (urlEndpoint is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (urlEndpoint));
			NativeHandle urlEndpointHandle = urlEndpoint.Handle;
			switch (endpointType) {
			case NWResolverConfigEndpointType.Https:
				InitializeHandle (nw_resolver_config_create_https (urlEndpointHandle));
				break;
			case NWResolverConfigEndpointType.Tls:
				InitializeHandle (nw_resolver_config_create_tls (urlEndpointHandle));
				break;
			default:
				throw new ArgumentOutOfRangeException ($"Unknown endpoint type: {endpointType}");
			}
			GC.KeepAlive (urlEndpoint);
		}

		[DllImport (Constants.NetworkLibrary)]
		static extern void nw_resolver_config_add_server_address (OS_nw_resolver_config config, OS_nw_endpoint serverAddress);

		public void AddServerAddress (NWEndpoint serverAddress)
		{
			nw_resolver_config_add_server_address (GetCheckedHandle (), serverAddress.Handle);
			GC.KeepAlive (serverAddress);
		}
	}
}
