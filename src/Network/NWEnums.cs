//
// NWEnums.cs: Network.framework enumerations
//
// Authors:
//   Manuel de la Pena (mandel@microsoft.com)
//
// Copyright 2019 Microsoft Inc
//
#nullable enable

using System.Runtime.CompilerServices;
using CoreFoundation;

using OS_nw_browse_result = System.IntPtr;
using OS_nw_endpoint = System.IntPtr;
using OS_nw_txt_record = System.IntPtr;

namespace Network {

	/// <summary>Describes changes between two network browse results.</summary>
	[Flags]
	[MacCatalyst (13, 1)]
	public enum NWBrowseResultChange : ulong {
		/// <summary>The change is invalid.</summary>
		Invalid = 0x00,
		/// <summary>The browse results are identical.</summary>
		Identical = 0x01,
		/// <summary>A browse result was added.</summary>
		ResultAdded = 0x02,
		/// <summary>A browse result was removed.</summary>
		ResultRemoved = 0x04,
		/// <summary>The TXT record changed.</summary>
		TxtRecordChanged = 0x20,
		/// <summary>A network interface became available.</summary>
		InterfaceAdded = 0x08,
		/// <summary>A network interface was removed.</summary>
		InterfaceRemoved = 0x10,
	}

	/// <summary>Describes the state of a network browser.</summary>
	[MacCatalyst (13, 1)]
	public enum NWBrowserState {
		/// <summary>The browser state is invalid.</summary>
		Invalid = 0,
		/// <summary>The browser can receive endpoint updates.</summary>
		Ready = 1,
		/// <summary>The browser has failed and cannot be restarted.</summary>
		Failed = 2,
		/// <summary>The browser was cancelled and cannot be restarted.</summary>
		Cancelled = 3,
	}

	/// <summary>Describes the state of a network connection.</summary>
	[MacCatalyst (13, 1)]
	public enum NWConnectionState {
		/// <summary>The connection state is invalid.</summary>
		Invalid = 0,
		/// <summary>The connection is waiting for a usable network.</summary>
		Waiting = 1,
		/// <summary>The connection is being established.</summary>
		Preparing = 2,
		/// <summary>The connection can send and receive data.</summary>
		Ready = 3,
		/// <summary>The connection has irrecoverably failed or closed.</summary>
		Failed = 4,
		/// <summary>The connection was cancelled.</summary>
		Cancelled = 5,
	}

	/// <summary>Describes the state of a network connection group.</summary>
	[TV (14, 0), iOS (14, 0)]
	[MacCatalyst (14, 0)]
	public enum NWConnectionGroupState {
		/// <summary>The connection group state is invalid.</summary>
		Invalid = 0,
		/// <summary>The group is waiting for a usable network.</summary>
		Waiting = 1,
		/// <summary>The group can receive and process incoming messages.</summary>
		Ready = 2,
		/// <summary>The group has irrecoverably failed.</summary>
		Failed = 3,
		/// <summary>The group was cancelled.</summary>
		Cancelled = 4,
	}

	/// <summary>Describes whether a network data transfer report is still collecting data.</summary>
	[MacCatalyst (13, 1)]
	public enum NWDataTransferReportState {
		/// <summary>The report is collecting transfer data.</summary>
		Collecting = 1,
		/// <summary>The report has finished collecting transfer data.</summary>
		Collected = 2,
	}

	/// <summary>Identifies the kind of network endpoint.</summary>
	[MacCatalyst (13, 1)]
	public enum NWEndpointType {
		/// <summary>The endpoint type is invalid.</summary>
		Invalid = 0,
		/// <summary>An IP address and port.</summary>
		Address = 1,
		/// <summary>A hostname and port.</summary>
		Host = 2,
		/// <summary>A Bonjour service name, type, and domain.</summary>
		BonjourService = 3,
		/// <summary>A URL endpoint.</summary>
		[MacCatalyst (13, 1)]
		Url = 4,
	}

	/// <summary>Identifies where a network name resolution result came from.</summary>
	[MacCatalyst (13, 1)]
	public enum NWReportResolutionSource {
		/// <summary>The result came from a network query.</summary>
		Query = 1,
		/// <summary>The result came from a cached response.</summary>
		Cache = 2,
		/// <summary>The result came from an expired cached response.</summary>
		ExpiredCache = 3,
	}

	/// <summary>Describes the state of an Ethernet channel.</summary>
	[NoTV, NoiOS]
	[NoMacCatalyst]
	[NativeName ("nw_ethernet_channel_state_t")]
	public enum NWEthernetChannelState {
		/// <summary>The channel state is invalid.</summary>
		Invalid = 0,
		/// <summary>The channel is waiting for a usable network.</summary>
		Waiting = 1,
		/// <summary>The channel is being established.</summary>
		Preparing = 2,
		/// <summary>The channel can send and receive data.</summary>
		Ready = 3,
		/// <summary>The channel has irrecoverably failed or closed.</summary>
		Failed = 4,
		/// <summary>The channel was cancelled.</summary>
		Cancelled = 5,
	}

	// from System/Library/Frameworks/Network.framework/Headers/framer_options.h:
	[Flags]
	[MacCatalyst (13, 1)]
	public enum NWFramerCreateFlags : uint {
		Default = 0x00,
	}

	// from System/Library/Frameworks/Network.framework/Headers/framer_options.h:
	[MacCatalyst (13, 1)]
	public enum NWFramerStartResult {
		Unknown = 0,
		Ready = 1,
		WillMarkReady = 2,
	}

	[MacCatalyst (13, 1)]
	public enum NWIPLocalAddressPreference {
		Default = 0,
		Temporary = 1,
		Stable = 2,
	}

	[MacCatalyst (13, 1)]
	public enum NWIPVersion {
		/// <summary>To be added.</summary>
		Any = 0,
		/// <summary>To be added.</summary>
		Version4 = 1,
		/// <summary>To be added.</summary>
		Version6 = 2,
	}

	[MacCatalyst (13, 1)]
	public enum NWInterfaceType {
		/// <summary>To be added.</summary>
		Other = 0,
		/// <summary>To be added.</summary>
		Wifi = 1,
		/// <summary>To be added.</summary>
		Cellular = 2,
		/// <summary>To be added.</summary>
		Wired = 3,
		/// <summary>To be added.</summary>
		Loopback = 4,
	}

	[MacCatalyst (13, 1)]
	public enum NWListenerState {
		/// <summary>To be added.</summary>
		Invalid = 0,
		/// <summary>To be added.</summary>
		Waiting = 1,
		/// <summary>To be added.</summary>
		Ready = 2,
		/// <summary>To be added.</summary>
		Failed = 3,
		/// <summary>To be added.</summary>
		Cancelled = 4,
	}

	[MacCatalyst (13, 1)]
	public enum NWMultiPathService {
		/// <summary>To be added.</summary>
		Disabled = 0,
		/// <summary>To be added.</summary>
		Handover = 1,
		/// <summary>To be added.</summary>
		Interactive = 2,
		/// <summary>To be added.</summary>
		Aggregate = 3,
	}

	[MacCatalyst (13, 1)]
	public enum NWParametersExpiredDnsBehavior {
		/// <summary>To be added.</summary>
		Default = 0,
		/// <summary>To be added.</summary>
		Allow = 1,
		/// <summary>To be added.</summary>
		Prohibit = 2,
		[TV (18, 0), Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
		Persistent = 3,
	}

	// this maps to `nw_path_status_t` in Network/Headers/path.h (and not the enum from NetworkExtension)
	[MacCatalyst (13, 1)]
	public enum NWPathStatus {
		/// <summary>To be added.</summary>
		Invalid = 0,
		/// <summary>To be added.</summary>
		Satisfied = 1,
		/// <summary>To be added.</summary>
		Unsatisfied = 2,
		/// <summary>To be added.</summary>
		Satisfiable = 3,
	}

	public enum NWServiceClass {
		/// <summary>To be added.</summary>
		BestEffort = 0,
		/// <summary>To be added.</summary>
		Background = 1,
		/// <summary>To be added.</summary>
		InteractiveVideo = 2,
		/// <summary>To be added.</summary>
		InteractiveVoice = 3,
		/// <summary>To be added.</summary>
		ResponsiveData = 4,
		/// <summary>To be added.</summary>
		Signaling = 5,
	}

	public enum NWIPEcnFlag {
		/// <summary>To be added.</summary>
		NonEct = 0,
		/// <summary>To be added.</summary>
		Ect = 2,
		/// <summary>To be added.</summary>
		Ect1 = 1,
		/// <summary>To be added.</summary>
		Ce = 3,
	}

	[MacCatalyst (13, 1)]
	public enum NWTxtRecordFindKey {
		Invalid = 0,
		NotPresent = 1,
		NoValue = 2,
		EmptyValue = 3,
		NonEmptyValue = 4,
	}

	[MacCatalyst (13, 1)]
	public enum NWWebSocketOpCode : int {
		Cont = 0x0,
		Text = 0x1,
		Binary = 0x2,
		Close = 0x8,
		Ping = 0x9,
		Pong = 0xA,
		Invalid = -1,
	}

	[MacCatalyst (13, 1)]
	public enum NWWebSocketCloseCode : int {
		NormalClosure = 1000,
		GoingAway = 1001,
		ProtocolError = 1002,
		UnsupportedData = 1003,
		NoStatusReceived = 1005,
		AbnormalClosure = 1006,
		InvalidFramePayloadData = 1007,
		PolicyViolation = 1008,
		MessageTooBig = 1009,
		MandatoryExtension = 1010,
		InternalServerError = 1011,
		TlsHandshake = 1015,
	}

	// this maps to `nw_ws_version_t` in Network.framework/Headers/ws_options.h (and not the enum from NetworkExtension)
	[MacCatalyst (13, 1)]
	public enum NWWebSocketVersion {
		Invalid = 0,
		Version13 = 1,
	}

	[MacCatalyst (13, 1)]
	public enum NWWebSocketResponseStatus {
		Invalid = 0,
		Accept = 1,
		Reject = 2,
	}

	[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
	public enum NWReportResolutionProtocol {
		Unknown = 0,
		Udp = 1,
		Tcp = 2,
		Tls = 3,
		Https = 4,
	}

	[TV (14, 0), iOS (14, 0)]
	[MacCatalyst (14, 0)]
	public enum NWResolverConfigEndpointType {
		Https,
		Tls,
	}

	[TV (15, 0), iOS (15, 0)]
	[MacCatalyst (15, 0)]
	public enum NWMultipathVersion {
		Unspecified = -1,
		Version0 = 0,
		Version1 = 1,
	}

	[TV (15, 0), iOS (15, 0)]
	[MacCatalyst (15, 0)]
	public enum NWInterfaceRadioType {
		Unknown = 0,
		WifiB = 1,
		WifiA = 2,
		WifiG = 3,
		WifiN = 4,
		WifiAC = 5,
		WifiAX = 6,

		CellLte = 0x80,
		CellEndcSub6 = 0x81,
		CellEndcMmw = 0x82,
		CellNrSaSub6 = 0x83,
		CellNrSaMmw = 0x84,
		CellWcdma = 0x85,
		CellGsm = 0x86,
		CellCdma = 0x87,
		CellEvdo = 0x88,
	}

	[TV (15, 0), iOS (15, 0)]
	[MacCatalyst (15, 0)]
	public enum NWParametersAttribution {
		Developer = 1,
		User = 2,
	}

	[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
	public enum NWQuicStreamType {
		Unknown = 0,
		Bidirectional = 1,
		Unidirectional = 2,
		[TV (16, 4), iOS (16, 4), MacCatalyst (16, 4)]
		Datagram = 3,
	}

	[NativeName ("nw_link_quality_t")]
#if XAMCORE_5_0
	public enum NWLinkQuality : byte {
#else
	public enum NWLinkQuality : uint {
#endif
		Unknown = 0,
		Minimal = 10,
		Moderate = 20,
		Good = 30,
	}
}
