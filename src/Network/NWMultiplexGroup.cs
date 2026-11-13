using CoreFoundation;
using OS_nw_group_descriptor = System.IntPtr;
using OS_nw_endpoint = System.IntPtr;

#nullable enable

namespace Network {
	[SupportedOSPlatform ("tvos")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	public class NWMultiplexGroup : NWMulticastGroup, ICreatableNativeObject<NWMultiplexGroup> {

		[DllImport (Constants.NetworkLibrary)]
		static extern OS_nw_group_descriptor nw_group_descriptor_create_multiplex (OS_nw_endpoint remoteEndpoint);

		static NWMultiplexGroup ICreatableNativeObject<NWMultiplexGroup>.Create (NativeHandle handle, bool owns) => new NWMultiplexGroup (handle, owns);

		internal NWMultiplexGroup (NativeHandle handle, bool owns) : base (handle, owns) { }

		public NWMultiplexGroup (NWEndpoint endpoint)
			: base (nw_group_descriptor_create_multiplex (endpoint.GetCheckedHandle ()), true)
		{
			GC.KeepAlive (endpoint);
		}
	}
}
