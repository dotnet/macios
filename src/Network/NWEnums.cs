//
// NWEnums.cs: Network.framework enumerations
//
// Authors:
//   Manuel de la Pena (mandel@microsoft.com)
//
// Copyright 2019 Microsoft Inc
//
#nullable enable

using System;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using ObjCRuntime;
using Foundation;
using CoreFoundation;

using OS_nw_browse_result = System.IntPtr;
using OS_nw_endpoint = System.IntPtr;
using OS_nw_txt_record = System.IntPtr;

namespace Network {

	[Flags, TV (13, 0), iOS (13, 0)]
	[MacCatalyst (13, 1)]
	public enum NWBrowseResultChange : ulong {
		Invalid = 0x00,
		Identical = 0x01,
		ResultAdded = 0x02,
		ResultRemoved = 0x04,
		TxtRecordChanged = 0x20,
		InterfaceAdded = 0x08,
		InterfaceRemoved = 0x10,
	}

	[TV (13, 0), iOS (13, 0)]
	[MacCatalyst (13, 1)]
	public enum NWBrowserState {
		Invalid = 0,
		Ready = 1,
		Failed = 2,
		Cancelled = 3,
	}

	[MacCatalyst (13, 1)]
	public enum NWConnectionState {
		/// <summary>To be added.</summary>
		Invalid = 0,
		/// <summary>To be added.</summary>
		Waiting = 1,
		/// <summary>To be added.</summary>
		Preparing = 2,
		/// <summary>To be added.</summary>
		Ready = 3,
		/// <summary>To be added.</summary>
		Failed = 4,
		/// <summary>To be added.</summary>
		Cancelled = 5,
	}

	[TV (14, 0), iOS (14, 0)]
	[MacCatalyst (14, 0)]
	public enum NWConnectionGroupState {
		Invalid = 0,
		Waiting = 1,
		Ready = 2,
		Failed = 3,
		Cancelled = 4,
	}

	[TV (13, 0), iOS (13, 0)]
	[MacCatalyst (13, 1)]
	public enum NWDataTransferReportState {
		Collecting = 1,
		Collected = 2,
	}

	[MacCatalyst (13, 1)]
	public enum NWEndpointType {
		/// <summary>To be added.</summary>
		Invalid = 0,
		/// <summary>To be added.</summary>
		Address = 1,
		/// <summary>To be added.</summary>
		Host = 2,
		/// <summary>To be added.</summary>
		BonjourService = 3,
		[TV (13, 0), iOS (13, 0)]
		[MacCatalyst (13, 1)]
		Url = 4,
	}

	[TV (13, 0), iOS (13, 0)]
	[MacCatalyst (13, 1)]
	public enum NWReportResolutionSource {
		Query = 1,
		Cache = 2,
		ExpiredCache = 3,
	}

	[NoTV, NoiOS]
	[NoMacCatalyst]
	public enum NWEthernetChannelState {
		Invalid = 0,
		Waiting = 1,
		Preparing = 2,
		Ready = 3,
		Failed = 4,
		Cancelled = 5,
	}

	// from System/Library/Frameworks/Network.framework/Headers/framer_options.h:
	[Flags]
	[TV (13, 0), iOS (13, 0)]
	[MacCatalyst (13, 1)]
	public enum NWFramerCreateFlags : uint {
		Default = 0x00,
	}

	// from System/Library/Frameworks/Network.framework/Headers/framer_options.h:
	[TV (13, 0), iOS (13, 0)]
	[MacCatalyst (13, 1)]
	public enum NWFramerStartResult {
		Unknown = 0,
		Ready = 1,
		WillMarkReady = 2,
	}

	[TV (13, 0), iOS (13, 0)]
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

	[TV (13, 0), iOS (13, 0)]
	[MacCatalyst (13, 1)]
	public enum NWTxtRecordFindKey {
		Invalid = 0,
		NotPresent = 1,
		NoValue = 2,
		EmptyValue = 3,
		NonEmptyValue = 4,
	}

	[TV (13, 0), iOS (13, 0)]
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

	[TV (13, 0), iOS (13, 0)]
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
	[TV (13, 0), iOS (13, 0)]
	[MacCatalyst (13, 1)]
	public enum NWWebSocketVersion {
		Invalid = 0,
		Version13 = 1,
	}

	[TV (13, 0), iOS (13, 0)]
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
		[TV (16, 4), Mac (13, 3), iOS (16, 4), MacCatalyst (16, 4)]
		Datagram = 3,
	}

	[NativeName ("nw_link_quality_t")]
	public enum NWLinkQuality : uint {
		Unknown = 0,
		Minimal = 10,
		Moderate = 20,
		Good = 30,
	}
}
