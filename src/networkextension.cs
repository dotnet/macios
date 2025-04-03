// Copyright 2014-2015 Xamarin Inc. All rights reserved.
// Copyright 2019 Microsoft Corporation

using System;

#if NET
#if IOS && !__MACCATALYST__
using AccessorySetupKit;
#else
using ASAccessory = Foundation.NSObject;
#endif
#endif
using CoreFoundation;
using Foundation;
using ObjCRuntime;
using Security;
using Network;
using OS_nw_parameters = System.IntPtr;
using OS_nw_interface = System.IntPtr;

#if !NET
using NativeHandle = System.IntPtr;
#endif

#if MONOMAC || TVOS
using NEHotspotHelperOptions = Foundation.NSObject;
#endif

#if MONOMAC
using NEHotspotHelperResult = Foundation.NSObject;
using NEHotspotHelperCommandType = Foundation.NSObject;
using NEHotspotHelperConfidence = Foundation.NSObject;
#endif

namespace NetworkExtension {
	/// <summary>Enumerates errors relating to a DNS proxy.</summary>
	[NoTV]
	[ErrorDomain ("NEDNSProxyErrorDomain")]
	[MacCatalyst (13, 1)]
	[Native]
	enum NEDnsProxyManagerError : long {
		/// <summary>To be added.</summary>
		Invalid = 1,
		/// <summary>To be added.</summary>
		Disabled = 2,
		/// <summary>To be added.</summary>
		Stale = 3,
		/// <summary>To be added.</summary>
		CannotBeRemoved = 4,
	}

	/// <summary>Enumerates the actions to be taken when a <see cref="T:NetworkExtension.NEFilterVerdict" /> is that a report should be made.</summary>
	[NoTV]
	[MacCatalyst (13, 1)]
	[Native]
	enum NEFilterAction : long {
		/// <summary>To be added.</summary>
		Invalid = 0,
		/// <summary>To be added.</summary>
		Allow = 1,
		/// <summary>To be added.</summary>
		Drop = 2,
		/// <summary>To be added.</summary>
		Remediate = 3,
		/// <summary>To be added.</summary>
		FilterData = 4,
	}

	[MacCatalyst (13, 1)]
	[Native]
	enum NEVpnIkev2TlsVersion : long {
		/// <summary>To be added.</summary>
		Default = 0,
		/// <summary>To be added.</summary>
		Tls1_0 = 1,
		/// <summary>To be added.</summary>
		Tls1_1 = 2,
		/// <summary>To be added.</summary>
		Tls1_2 = 3,
	}

	[NoTV]
	[NoMac]
	[MacCatalyst (13, 1)]
	[Native]
	enum NEHotspotConfigurationEapType : long {
		/// <summary>To be added.</summary>
		Tls = 13,
		/// <summary>To be added.</summary>
		Ttls = 21,
		/// <summary>To be added.</summary>
		Peap = 25,
		/// <summary>To be added.</summary>
		Fast = 43,
	}

	[NoTV]
	[NoMac]
	[MacCatalyst (13, 1)]
	[Native]
	enum NEHotspotConfigurationTtlsInnerAuthenticationType : long {
		/// <summary>To be added.</summary>
		Pap = 0,
		/// <summary>To be added.</summary>
		Chap = 1,
		/// <summary>To be added.</summary>
		MSChap = 2,
		/// <summary>To be added.</summary>
		MSChapv2 = 3,
		/// <summary>To be added.</summary>
		Eap = 4,
	}

	[NoTV]
	[NoMac]
	[MacCatalyst (13, 1)]
	[Native]
	enum NEHotspotConfigurationEapTlsVersion : long {
		/// <summary>To be added.</summary>
		Tls1_0 = 0,
		/// <summary>To be added.</summary>
		Tls1_1 = 1,
		/// <summary>To be added.</summary>
		Tls1_2 = 2,
	}

	/// <summary>Enumerates errors relating to network hostpot configuration.</summary>
	[NoTV]
	[NoMac]
	[MacCatalyst (13, 1)]
	[Native]
	[ErrorDomain ("NEHotspotConfigurationErrorDomain")]
	public enum NEHotspotConfigurationError : long {
		/// <summary>To be added.</summary>
		Invalid = 0,
		/// <summary>To be added.</summary>
		InvalidSsid = 1,
		/// <summary>To be added.</summary>
		InvalidWpaPassphrase = 2,
		/// <summary>To be added.</summary>
		InvalidWepPassphrase = 3,
		/// <summary>To be added.</summary>
		InvalidEapSettings = 4,
		/// <summary>To be added.</summary>
		InvalidHS20Settings = 5,
		/// <summary>To be added.</summary>
		InvalidHS20DomainName = 6,
		/// <summary>To be added.</summary>
		UserDenied = 7,
		/// <summary>To be added.</summary>
		Internal = 8,
		/// <summary>To be added.</summary>
		Pending = 9,
		/// <summary>To be added.</summary>
		SystemConfiguration = 10,
		/// <summary>To be added.</summary>
		Unknown = 11,
		/// <summary>To be added.</summary>
		JoinOnceNotSupported = 12,
		/// <summary>To be added.</summary>
		AlreadyAssociated = 13,
		/// <summary>To be added.</summary>
		ApplicationIsNotInForeground = 14,
		InvalidSsidPrefix = 15,
		UserUnauthorized = 16,
		SystemDenied = 17,
	}

	[NoTV]
	[NoiOS]
	[NoMacCatalyst]
	[Native]
	enum NEFilterManagerGrade : long {
		Firewall = 1,
		Inspector = 2,
	}

	[NoTV]
	[iOS (13, 0)]
	[MacCatalyst (13, 1)]
	[Native]
	enum NETrafficDirection : long {
		Any = 0,
		Inbound = 1,
		Outbound = 2,
	}

	[NoTV]
	[NoiOS]
	[NoMacCatalyst]
	[Native]
	enum NENetworkRuleProtocol : long {
		Any = 0,
		Tcp = 1,
		Udp = 2,
	}

	[NoTV]
	[NoiOS]
	[NoMacCatalyst]
	[Native]
	enum NEFilterPacketProviderVerdict : long {
		Allow = 0,
		Drop = 1,
		Delay = 2,
	}

	[NoTV]
	[iOS (13, 0)]
	[MacCatalyst (13, 1)]
	[Native]
	enum NEFilterReportEvent : long {
		NewFlow = 1,
		DataDecision = 2,
		FlowClosed = 3,
		[NoiOS]
		[NoMacCatalyst]
		Statistics = 4,
	}

	[NoTV, NoiOS, NoMacCatalyst]
	[Native]
	enum NEFilterReportFrequency : long {
		None,
		Low,
		Medium,
		High,
	}

	[NoTV, NoiOS, NoMacCatalyst]
	[Native]
	public enum NEFilterDataAttribute : long {
		HasIpHeader = 1,
	}

	[NoTV, NoMac, iOS (15, 0), MacCatalyst (15, 0)]
	[Native]
	enum NEHotspotNetworkSecurityType : long {
		Open = 0,
		Wep = 1,
		Personal = 2,
		Enterprise = 3,
		Unknown = 4,
	}

	[Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0), TV (17, 0)]
	[Native]
	[ErrorDomain ("NEVPNConnectionErrorDomain")]
	public enum NEVpnConnectionError : long {
		Overslept = 1,
		NoNetworkAvailable = 2,
		UnrecoverableNetworkChange = 3,
		ConfigurationFailed = 4,
		ServerAddressResolutionFailed = 5,
		ServerNotResponding = 6,
		ServerDead = 7,
		AuthenticationFailed = 8,
		ClientCertificateInvalid = 9,
		ClientCertificateNotYetValid = 10,
		ClientCertificateExpired = 11,
		PluginFailed = 12,
		ConfigurationNotFound = 13,
		PluginDisabled = 14,
		NegotiationFailed = 15,
		ServerDisconnected = 16,
		ServerCertificateInvalid = 17,
		ServerCertificateNotYetValid = 18,
		ServerCertificateExpired = 19,
	}

	delegate void NEAppProxyFlowOpenCallback ([NullAllowed] NSError completionHandler);

	/// <summary>Provides IO over a network socket.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/NetworkExtension/Reference/NEAppProxyFlowClassRef/index.html">Apple documentation for <c>NEAppProxyFlow</c></related>
	[NoTV]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	[Abstract] // documented as such and ...
	[DisableDefaultCtor] // can't be created (with `init`) without crashing introspection tests
	interface NEAppProxyFlow {
		/// <param name="localEndpoint">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <param name="completionHandler">To be added.</param>
		///         <summary>Opens the flow.</summary>
		///         <remarks>To be added.</remarks>
		[Deprecated (PlatformName.iOS, 18, 0, message: "Use 'OpenWithLocalFlowEndpoint' instead.")]
		[Deprecated (PlatformName.MacCatalyst, 18, 0, message: "Use 'OpenWithLocalFlowEndpoint' instead.")]
		[Deprecated (PlatformName.MacOSX, 15, 0, message: "Use 'OpenWithLocalFlowEndpoint' instead.")]
		[Export ("openWithLocalEndpoint:completionHandler:")]
		[Async]
		void OpenWithLocalEndpoint ([NullAllowed] NWHostEndpoint localEndpoint, Action<NSError> completionHandler);

		/// <param name="error">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <summary>Closes the flow for reading.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("closeReadWithError:")]
		void CloseRead ([NullAllowed] NSError error);

		/// <param name="error">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <summary>Closes the flow for writing.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("closeWriteWithError:")]
		void CloseWrite ([NullAllowed] NSError error);

		[Internal]
		[NoTV, NoiOS, MacCatalyst (15, 0)]
		[Export ("setMetadata:")]
		void SetMetadata (OS_nw_parameters nwparameters);

		/// <summary>Gets the flow metadata.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("metaData")]
		NEFlowMetaData MetaData { get; }

		[Internal]
		[NoTV, iOS (13, 4)]
		[MacCatalyst (13, 1)]
		[NullAllowed, Export ("networkInterface", ArgumentSemantic.Copy)]
		OS_nw_interface WeakNetworkInterface { get; set; }

		[NoTV, iOS (13, 4)]
		[MacCatalyst (13, 1)]
		NWInterface NetworkInterface {
			[Wrap ("Runtime.GetINativeObject<NWInterface> (WeakNetworkInterface, false)!")]
			get;
			[Wrap ("WeakNetworkInterface = Runtime.RetainAndAutoreleaseNativeObject (value)")]
			set;
		}

		[iOS (14, 2)]
		[MacCatalyst (14, 2)]
		[Export ("remoteHostname")]
		[NullAllowed]
		string RemoteHostname { get; }

		[iOS (14, 3)]
		[MacCatalyst (14, 3)]
		[Export ("isBound")]
		bool IsBound { get; }

#if !NET
		[Field ("NEAppProxyErrorDomain")]
		NSString ErrorDomain { get; }
#endif

		[Async]
		[NoTV, Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
		[Export ("openWithLocalFlowEndpoint:completionHandler:")]
		void OpenWithLocalFlowEndpoint ([NullAllowed] Network.NWEndpoint localEndpoint, NEAppProxyFlowOpenCallback completionHandler);
	}

	/// <summary>Provides sockets by creating <see cref="T:NetworkExtension.NEAppProxyFlow" /> objects.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/NetworkExtension/Reference/NEAppProxyProviderClassRef/index.html">Apple documentation for <c>NEAppProxyProvider</c></related>
	[NoTV]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NETunnelProvider))]
	[DisableDefaultCtor] // no valid handle when `init` is called
	interface NEAppProxyProvider {
		/// <param name="options">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <param name="completionHandler">To be added.</param>
		///         <summary>Starts the proxy with the specified <paramref name="options" /> and runs <paramref name="completionHandler" /> after the operation is complete.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("startProxyWithOptions:completionHandler:")]
		[Async]
		void StartProxy ([NullAllowed] NSDictionary<NSString, NSObject> options, Action<NSError> completionHandler);

		/// <param name="reason">To be added.</param>
		///         <param name="completionHandler">To be added.</param>
		///         <summary>Stops the proxy with the specified <paramref name="reason" /> and runs <paramref name="completionHandler" /> when the operation is complete.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("stopProxyWithReason:completionHandler:")]
		[Async]
		void StopProxy (NEProviderStopReason reason, Action completionHandler);

		/// <param name="error">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <summary>Cancels the proxy witht the specified error.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("cancelProxyWithError:")]
		void CancelProxy ([NullAllowed] NSError error);

		/// <param name="flow">To be added.</param>
		///         <summary>Handles the provided proxy <paramref name="flow" />.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("handleNewFlow:")]
		bool HandleNewFlow (NEAppProxyFlow flow);

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Export ("handleNewUDPFlow:initialRemoteEndpoint:")]
		[Deprecated (PlatformName.iOS, 18, 0, message: "Call 'HandleNewUdpFlowWithInitialFlowEndPoint' instead.")]
		[Deprecated (PlatformName.MacCatalyst, 18, 0, message: "Call 'HandleNewUdpFlowWithInitialFlowEndPoint' instead.")]
		[Deprecated (PlatformName.MacOSX, 15, 0, message: "Call 'HandleNewUdpFlowWithInitialFlowEndPoint' instead.")]
		bool HandleNewUdpFlow (NEAppProxyUdpFlow flow, NWEndpoint remoteEndpoint);

		[NoTV, Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
		[Export ("handleNewUDPFlow:initialRemoteFlowEndpoint:")]
		bool HandleNewUdpFlowWithInitialFlowEndPoint (NEAppProxyUdpFlow flow, Network.NWEndpoint remoteEndpoint);
	}

	/// <summary>Configures and controls network tunnels received from an App Proxy Provider extension.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/NetworkExtension/Reference/NEAppProxyProviderManagerClassRef/index.html">Apple documentation for <c>NEAppProxyProviderManager</c></related>
	[NoTV]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NETunnelProviderManager))]
	[DisableDefaultCtor] // no valid handle when `init` is called
	interface NEAppProxyProviderManager {
		/// <param name="completionHandler">To be added.</param>
		///         <summary>Loads all proxy configurations for the app that were previously saved in the Network Extensions prefrences and runs <paramref name="completionHandler" /> when the operation is complete.</summary>
		///         <remarks>To be added.</remarks>
		[Static]
		[Export ("loadAllFromPreferencesWithCompletionHandler:")]
		[Async]
		void LoadAllFromPreferences (Action<NSArray, NSError> completionHandler);
	}

	/// <summary>Provides IO over a TCP socket.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/NetworkExtension/Reference/NEAppProxyTCPFlowClassRef/index.html">Apple documentation for <c>NEAppProxyTCPFlow</c></related>
	[NoTV]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NEAppProxyFlow), Name = "NEAppProxyTCPFlow")]
	[DisableDefaultCtor]
	interface NEAppProxyTcpFlow {
		/// <param name="completionHandler">To be added.</param>
		///         <summary>Reads data from the flow and runs <paramref name="completionHandler" /> when the operation is complete.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("readDataWithCompletionHandler:")]
		[Async]
		void ReadData (Action<NSData, NSError> completionHandler);

		/// <param name="data">To be added.</param>
		///         <param name="completionHandler">To be added.</param>
		///         <summary>Writes the provided <paramref name="data" /> to the flow and runs <paramref name="completionHandler" /> when the operation is complete.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("writeData:withCompletionHandler:")]
		[Async]
		void WriteData (NSData data, Action<NSError> completionHandler);

		/// <summary>Gets a description of the remote endpoint.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Deprecated (PlatformName.iOS, 18, 0, message: "Call 'RemoveFlowEndpoint' instead.")]
		[Deprecated (PlatformName.MacCatalyst, 18, 0, message: "Call 'RemoveFlowEndpoint' instead.")]
		[Deprecated (PlatformName.MacOSX, 15, 0, message: "Call 'RemoveFlowEndpoint' instead.")]
		[Export ("remoteEndpoint")]
		NWEndpoint RemoteEndpoint { get; }

		[NoTV, Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
		[Export ("remoteFlowEndpoint")]
		Network.NWEndpoint RemoteFlowEndpoint { get; }
	}

	delegate void NEDatagramRead (NSData [] datagrams, NWEndpoint [] remoteEndpoints, NSError error);
	delegate void NEDatagramAndFlowEndpointsRead ([NullAllowed] NSData [] datagrams, [NullAllowed] Network.NWEndpoint [] remoteEndpoints, [NullAllowed] NSError error);
	delegate void NEDatagramWriteResult ([NullAllowed] NSError error);
	/// <summary>Provides IO over a UDP socket.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/NetworkExtension/Reference/NEAppProxyUDPFlowClassRef/index.html">Apple documentation for <c>NEAppProxyUDPFlow</c></related>
	[NoTV]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NEAppProxyFlow), Name = "NEAppProxyUDPFlow")]
	[DisableDefaultCtor]
	interface NEAppProxyUdpFlow {
		/// <param name="completionHandler">To be added.</param>
		///         <summary>Reads datagrams from the flow and runs <paramref name="completionHandler" /> when the operation is complete.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("readDatagramsWithCompletionHandler:")]
		[Async (ResultTypeName = "NEDatagramReadResult")]
		[Deprecated (PlatformName.iOS, 18, 0, message: "Use 'ReadDatagramsAndFlowEndpoints' instead.")]
		[Deprecated (PlatformName.MacCatalyst, 18, 0, message: "Use 'ReadDatagramsAndFlowEndpoints' instead.")]
		[Deprecated (PlatformName.MacOSX, 15, 0, message: "Use 'ReadDatagramsAndFlowEndpoints' instead.")]
		void ReadDatagrams (NEDatagramRead completionHandler);

		/// <param name="datagrams">To be added.</param>
		///         <param name="remoteEndpoints">To be added.</param>
		///         <param name="completionHandler">To be added.</param>
		///         <summary>Writes the provided <paramref name="datagrams" /> to the specified <paramref name="remoteEndpoints" /> and runs <paramref name="completionHandler" /> when the operation is complete.</summary>
		///         <remarks>To be added.</remarks>
		[Deprecated (PlatformName.iOS, 18, 0, message: "Use 'WriteDatagramsAndFlowEndpoints' instead.")]
		[Deprecated (PlatformName.MacCatalyst, 18, 0, message: "Use 'WriteDatagramsAndFlowEndpoints' instead.")]
		[Deprecated (PlatformName.MacOSX, 15, 0, message: "Use 'WriteDatagramsAndFlowEndpoints' instead.")]
		[Export ("writeDatagrams:sentByEndpoints:completionHandler:")]
		[Async]
		void WriteDatagrams (NSData [] datagrams, NWEndpoint [] remoteEndpoints, Action<NSError> completionHandler);

		/// <summary>Gets a description of the local endpoint.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Deprecated (PlatformName.iOS, 18, 0, message: "Use 'LocalFlowEndpoint' instead.")]
		[Deprecated (PlatformName.MacCatalyst, 18, 0, message: "Use 'LocalFlowEndpoint' instead.")]
		[Deprecated (PlatformName.MacOSX, 15, 0, message: "Use 'LocalFlowEndpoint' instead.")]
		[NullAllowed, Export ("localEndpoint")]
		NWEndpoint LocalEndpoint { get; }

#if NET
		[Async (ResultTypeName = "NEDatagramAndFlowEndpointsReadResult")]
		[NoTV, Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
		[Export ("readDatagramsAndFlowEndpointsWithCompletionHandler:")]
		void ReadDatagramsAndFlowEndpoints (NEDatagramAndFlowEndpointsRead completionHandler);
#endif

		[Async]
		[NoTV, Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
		[Export ("writeDatagrams:sentByFlowEndpoints:completionHandler:")]
		void WriteDatagramsAndFlowEndpoints (NSData [] datagrams, Network.NWEndpoint [] flowEndpoints, NEDatagramWriteResult completionHandler);

		[NoTV, Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
		[Export ("localFlowEndpoint"), NullAllowed]
		Network.NWEndpoint LocalFlowEndpoint { get; }
	}

	/// <summary>Defines a rule that select network connections based on application signing identifier, application path, and/or hostname domain.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/NetworkExtension/Reference/NEAppRuleClassRef/index.html">Apple documentation for <c>NEAppRule</c></related>
	[NoTV]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface NEAppRule : NSSecureCoding, NSCopying {
		[MacCatalyst (13, 1)]
#if NET
		[NoMac]
#endif
		[Export ("initWithSigningIdentifier:")]
		NativeHandle Constructor (string signingIdentifier);

		[NoiOS, NoMacCatalyst]
		[Export ("initWithSigningIdentifier:designatedRequirement:")]
		NativeHandle Constructor (string signingIdentifier, string designatedRequirement);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NoiOS, MacCatalyst (15, 0)]
		[Export ("matchDesignatedRequirement")]
		string MatchDesignatedRequirement { get; }

		/// <summary>Gets or sets the path to the app that matches the rule.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[NullAllowed, Export ("matchPath")]
		string MatchPath { get; set; }

		/// <summary>Gets the siging identifer of the app that the rule matched.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("matchSigningIdentifier")]
		string MatchSigningIdentifier { get; }

		/// <summary>Gets a list of domains for which VPN connection is allowed.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>If this array is empty, then any domain is allowed.</remarks>
		[NullAllowed, Export ("matchDomains", ArgumentSemantic.Copy)]
		string [] MatchDomains { get; set; }

		[NoiOS, MacCatalyst (15, 0)]
		[NullAllowed, Export ("matchTools", ArgumentSemantic.Copy)]
		NEAppRule [] MatchTools { get; set; }
	}

	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject), Name = "NEDNSSettings")]
	[DisableDefaultCtor]
	interface NEDnsSettings : NSSecureCoding, NSCopying {
		[Export ("initWithServers:")]
		NativeHandle Constructor (string [] servers);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("servers")]
		string [] Servers { get; }

		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("searchDomains", ArgumentSemantic.Copy)]
		string [] SearchDomains { get; set; }

		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("domainName")]
		string DomainName { get; set; }

		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("matchDomains", ArgumentSemantic.Copy)]
		string [] MatchDomains { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("matchDomainsNoSearch")]
		bool MatchDomainsNoSearch { get; set; }

		[iOS (14, 0)]
		[MacCatalyst (14, 0)]
		[Export ("dnsProtocol")]
		NEDnsProtocol DnsProtocol { get; }

		[iOS (14, 0), NoTV]
		[MacCatalyst (14, 0)]
		[Notification]
		[Field ("NEDNSSettingsConfigurationDidChangeNotification")]
		NSString ConfigurationDidChangeNotification { get; }
	}

	/// <summary>Provides filter flow control information to a <see cref="T:NetworkExtension.NEFilterDataProvider" />.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/NetworkExtension/Reference/NEFilterControlProviderClassRef/index.html">Apple documentation for <c>NEFilterControlProvider</c></related>
	[NoTV]
	[NoMac]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NEFilterProvider))]
	[DisableDefaultCtor] // no valid handle when `init` is called
	interface NEFilterControlProvider {
		/// <summary>Gets or sets a dictionary of remediation strings.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("remediationMap", ArgumentSemantic.Copy)]
		NSDictionary<NSString, NSDictionary<NSString, NSObject>> RemediationMap { get; set; }

		/// <summary>Gets or sets a dictionary of strings to append to URLs.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("URLAppendStringMap", ArgumentSemantic.Copy)]
		NSDictionary<NSString, NSString> UrlAppendStringMap { get; set; }

		/// <param name="report">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("handleReport:")]
		void HandleReport (NEFilterReport report);

		/// <param name="flow">To be added.</param>
		///         <param name="completionHandler">To be added.</param>
		///         <summary>Handles a user remediation request and runs <paramref name="completionHandler" /> after changing the rules.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("handleRemediationForFlow:completionHandler:")]
		[Async]
		void HandleRemediationForFlow (NEFilterFlow flow, Action<NEFilterControlVerdict> completionHandler);

		/// <param name="flow">To be added.</param>
		///         <param name="completionHandler">To be added.</param>
		///         <summary>Handles new filter rules and runs <paramref name="completionHandler" /> after changing the rules.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("handleNewFlow:completionHandler:")]
		[Async]
		void HandleNewFlow (NEFilterFlow flow, Action<NEFilterControlVerdict> completionHandler);

		/// <summary>Method that is called to notify the Filter Data Provider that the filtering rules changed..</summary>
		///         <remarks>To be added.</remarks>
		[Export ("notifyRulesChanged")]
		void NotifyRulesChanged ();
	}

	/// <summary>Represents a Filter Control Provider decision about network data flow.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/NetworkExtension/Reference/NEFilterControlVerdictClassRef/index.html">Apple documentation for <c>NEFilterControlVerdict</c></related>
	[NoTV]
	[NoMac]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NEFilterNewFlowVerdict))]
	interface NEFilterControlVerdict : NSSecureCoding, NSCopying {
		/// <param name="updateRules">To be added.</param>
		///         <summary>Creates and returns a verdict that allows the data flow, and updates the filtering rules.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Static]
		[Export ("allowVerdictWithUpdateRules:")]
		NEFilterControlVerdict AllowVerdictWithUpdateRules (bool updateRules);

		/// <param name="updateRules">To be added.</param>
		///         <summary>Creates and returns a verdict that drop the data in the flow, and updates the filtering rules.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Static]
		[Export ("dropVerdictWithUpdateRules:")]
		NEFilterControlVerdict DropVerdictWithUpdateRules (bool updateRules);

		/// <summary>Creates and returns a verdict that indicates that the rules have been updated and future data flow will require new decisions.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Static]
		[Export ("updateRules")]
		NEFilterControlVerdict UpdateRules ();
	}

	/// <summary>Filters network data.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/NetworkExtension/Reference/NEFilterDataProviderClassRef/index.html">Apple documentation for <c>NEFilterDataProvider</c></related>
	[NoTV]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NEFilterProvider))]
	[DisableDefaultCtor] // no valid handle when `init` is called
	interface NEFilterDataProvider {
		/// <param name="flow">To be added.</param>
		///         <summary>Method that is called to filter a new network flow.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("handleNewFlow:")]
		NEFilterNewFlowVerdict HandleNewFlow (NEFilterFlow flow);

		[Export ("handleInboundDataFromFlow:readBytesStartOffset:readBytes:")]
		NEFilterDataVerdict HandleInboundDataFromFlow (NEFilterFlow flow, nuint offset, NSData readBytes);

		[Export ("handleOutboundDataFromFlow:readBytesStartOffset:readBytes:")]
		NEFilterDataVerdict HandleOutboundDataFromFlow (NEFilterFlow flow, nuint offset, NSData readBytes);

		/// <param name="flow">To be added.</param>
		///         <summary>Method that is called to make a filtering decision for inbound data flow after the data is seen.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("handleInboundDataCompleteForFlow:")]
		NEFilterDataVerdict HandleInboundDataCompleteForFlow (NEFilterFlow flow);

		/// <param name="flow">To be added.</param>
		///         <summary>ethod that is called to make a filtering decision for outbound data flow after the data is seen.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("handleOutboundDataCompleteForFlow:")]
		NEFilterDataVerdict HandleOutboundDataCompleteForFlow (NEFilterFlow flow);

		/// <param name="flow">To be added.</param>
		///         <summary>Method that is called to handle a user remediation request.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[NoMac]
		[MacCatalyst (13, 1)]
		[Export ("handleRemediationForFlow:")]
		NEFilterRemediationVerdict HandleRemediationForFlow (NEFilterFlow flow);

		/// <summary>Method that is called to handle to handle a change to the filtering rules.</summary>
		///         <remarks>To be added.</remarks>
		[NoMac]
		[MacCatalyst (13, 1)]
		[Export ("handleRulesChanged")]
		void HandleRulesChanged ();

		[NoiOS, NoMacCatalyst]
		[Export ("applySettings:completionHandler:")]
		[Async]
		void ApplySettings ([NullAllowed] NEFilterSettings settings, Action<NSError> completionHandler);

		[NoiOS, NoMacCatalyst]
		[Export ("resumeFlow:withVerdict:")]
		void ResumeFlow (NEFilterFlow flow, NEFilterVerdict verdict);

		[NoTV, NoiOS, NoMacCatalyst]
		[Export ("updateFlow:usingVerdict:forDirection:")]
		void UpdateFlow (NEFilterSocketFlow flow, NEFilterDataVerdict verdict, NETrafficDirection direction);
	}

	/// <summary>Represents a Filter Data Provider decision about network data flow.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/NetworkExtension/Reference/NEFilterDataVerdictClassRef/index.html">Apple documentation for <c>NEFilterDataVerdict</c></related>
	[NoTV]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NEFilterVerdict))]
	interface NEFilterDataVerdict : NSSecureCoding, NSCopying {
		[NoTV, NoiOS, NoMacCatalyst]
		[Export ("statisticsReportFrequency", ArgumentSemantic.Assign)]
		NEFilterReportFrequency StatisticsReportFrequency { get; set; }

		/// <summary>Creates and returns a verdict that allows the current and subsequent data to be passed on.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Static]
		[Export ("allowVerdict")]
		NEFilterDataVerdict AllowVerdict ();

		/// <summary>Creates and returns a verdict that drops the current and subsequent data.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Static]
		[Export ("dropVerdict")]
		NEFilterDataVerdict DropVerdict ();

		/// <param name="remediationUrlMapKey">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <param name="remediationButtonTextMapKey">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <summary>Creates and returns a verdict that drops the current and subsequent data, but allows the user to request access.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Static]
		[Export ("remediateVerdictWithRemediationURLMapKey:remediationButtonTextMapKey:")]
		NEFilterDataVerdict RemediateVerdict ([NullAllowed] string remediationUrlMapKey, [NullAllowed] string remediationButtonTextMapKey);

		[Static]
		[Export ("dataVerdictWithPassBytes:peekBytes:")]
		NEFilterDataVerdict DataVerdict (nuint passBytes, nuint peekBytes);

		/// <summary>Creates and returns a verdict that notifies the system that the Filter Control Provider needs to update the rules before deciding.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Static]
		[Export ("needRulesVerdict")]
		NEFilterDataVerdict NeedRulesVerdict ();

		[NoiOS, MacCatalyst (15, 0)]
		[Static]
		[Export ("pauseVerdict")]
		NEFilterDataVerdict PauseVerdict ();
	}

	/// <summary>Contains information about a network data flow.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/NetworkExtension/Reference/NEFilterFlowClassRef/index.html">Apple documentation for <c>NEFilterFlow</c></related>
	[NoTV]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	interface NEFilterFlow : NSSecureCoding, NSCopying {
		/// <summary>Gets the URL for the flow.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("URL")]
		NSUrl Url { get; }

		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[NullAllowed, Export ("sourceAppUniqueIdentifier")]
		NSData SourceAppUniqueIdentifier { get; }

		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[NullAllowed, Export ("sourceAppIdentifier")]
		string SourceAppIdentifier { get; }

		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[NullAllowed, Export ("sourceAppVersion")]
		string SourceAppVersion { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Export ("direction")]
		NETrafficDirection Direction { get; }

		[NoiOS, MacCatalyst (15, 0)]
		[NullAllowed, Export ("sourceAppAuditToken")]
		NSData SourceAppAuditToken { get; }

		[Mac (13, 0), NoiOS, NoMacCatalyst, NoTV]
		[NullAllowed]
		[Export ("sourceProcessAuditToken")]
		NSData SourceProcessAuditToken { get; }

		[iOS (13, 1)]
		[MacCatalyst (13, 1)]
		[Export ("identifier")]
		NSUuid Identifier { get; }
	}

	/// <summary>Manages content filter configurations.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/NetworkExtension/Reference/NEFilterManagerClassRef/index.html">Apple documentation for <c>NEFilterManager</c></related>
	[NoTV]
	// according to Xcode7 SDK this was available (in parts) in iOS8
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface NEFilterManager {
		/// <summary>Gets the shared filter manager instance.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Static]
		[Export ("sharedManager")]
		NEFilterManager SharedManager { get; }

		/// <param name="completionHandler">To be added.</param>
		///         <summary>Loads the filter from the configuration that is saved in the Network Extension preferences and runs a completion handler after the operation is complete.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("loadFromPreferencesWithCompletionHandler:")]
		[Async]
		void LoadFromPreferences (Action<NSError> completionHandler);

		/// <param name="completionHandler">To be added.</param>
		///         <summary>Removes the filter from the Network Extensions preferences and runs a completion handler when the operation is complete.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("removeFromPreferencesWithCompletionHandler:")]
		[Async]
		void RemoveFromPreferences (Action<NSError> completionHandler);

		/// <param name="completionHandler">To be added.</param>
		///         <summary>Saves the filter in the Network Extensions preferences and runs a completion handler when the operation is complete.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("saveToPreferencesWithCompletionHandler:")]
		[Async]
		void SaveToPreferences (Action<NSError> completionHandler);

		/// <summary>Gets or sets a localized description of the filter.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("localizedDescription")]
		string LocalizedDescription { get; set; }

		/// <summary>Gets or sets an object that contains settings for the filter.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[NullAllowed, Export ("providerConfiguration", ArgumentSemantic.Strong)]
		NEFilterProviderConfiguration ProviderConfiguration { get; set; }

		/// <summary>Get or sets a Boolean value that enables and disables the filter.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("enabled")]
		bool Enabled { [Bind ("isEnabled")] get; set; }

		/// <include file="../docs/api/NetworkExtension/NEFilterManager.xml" path="/Documentation/Docs[@DocId='P:NetworkExtension.NEFilterManager.ConfigurationDidChangeNotification']/*" />
		[Field ("NEFilterConfigurationDidChangeNotification")]
		[Notification]
		NSString ConfigurationDidChangeNotification { get; }

		[NoiOS]
		[NoMacCatalyst]
		[Export ("grade", ArgumentSemantic.Assign)]
		NEFilterManagerGrade Grade { get; set; }

#if !NET
		[Field ("NEFilterErrorDomain")]
		NSString ErrorDomain { get; }
#endif

		[NoTV, Mac (15, 0), NoiOS, MacCatalyst (18, 0)]
		[Export ("disableEncryptedDNSSettings", ArgumentSemantic.Assign)]
		bool DisableEncryptedDnsSettings { get; set; }
	}

	/// <summary>Represents a Filter Provider decision about network data flow the first time that the data is seen.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/NetworkExtension/Reference/NEFilterNewFlowVerdictClassRef/index.html">Apple documentation for <c>NEFilterNewFlowVerdict</c></related>
	[NoTV]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NEFilterVerdict))]
	interface NEFilterNewFlowVerdict : NSSecureCoding, NSCopying {
		[NoTV, NoiOS, NoMacCatalyst]
		[Export ("statisticsReportFrequency", ArgumentSemantic.Assign)]
		NEFilterReportFrequency StatisticsReportFrequency { get; set; }

		/// <summary>Creates and returns a verdict needs filter rules before it can decide.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Static]
		[Export ("needRulesVerdict")]
		NEFilterNewFlowVerdict NeedRulesVerdict ();

		/// <summary>Returns a verdict that allows the data flow to pass.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Static]
		[Export ("allowVerdict")]
		NEFilterNewFlowVerdict AllowVerdict ();

		/// <summary>Returns a verdict that drops the data flow and does not give the user the ability to request access.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Static]
		[Export ("dropVerdict")]
		NEFilterNewFlowVerdict DropVerdict ();

		/// <param name="remediationUrlMapKey">To be added.</param>
		///         <param name="remediationButtonTextMapKey">To be added.</param>
		///         <summary>Returns a verdict that drops the data flow but gives the user the ability to request access.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Static]
		[Export ("remediateVerdictWithRemediationURLMapKey:remediationButtonTextMapKey:")]
		NEFilterNewFlowVerdict RemediateVerdict (string remediationUrlMapKey, string remediationButtonTextMapKey);

		/// <param name="urlAppendMapKey">To be added.</param>
		///         <summary>Returns a verdict that allows the data flow to pass, but that a string will be appended to the URL before the data is passed.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Static]
		[Export ("URLAppendStringVerdictWithMapKey:")]
		NEFilterNewFlowVerdict UrlAppendStringVerdict (string urlAppendMapKey);

		[Static]
		[Export ("filterDataVerdictWithFilterInbound:peekInboundBytes:filterOutbound:peekOutboundBytes:")]
		NEFilterNewFlowVerdict FilterDataVerdict (bool filterInbound, nuint peekInboundBytes, bool filterOutbound, nuint peekOutboundBytes);

		[NoiOS, MacCatalyst (15, 0)]
		[Static]
		[Export ("pauseVerdict")]
		NEFilterDataVerdict PauseVerdict ();
	}

	/// <summary>Base class for data and control filter providers.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/NetworkExtension/Reference/NEFilterProviderClassRef/index.html">Apple documentation for <c>NEFilterProvider</c></related>
	[NoTV]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NEProvider))]
	[Abstract] // documented as such
	interface NEFilterProvider {
		/// <param name="completionHandler">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("startFilterWithCompletionHandler:")]
		[Async]
		void StartFilter (Action<NSError> completionHandler);

		/// <param name="reason">To be added.</param>
		///         <param name="completionHandler">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("stopFilterWithReason:completionHandler:")]
		[Async]
		void StopFilter (NEProviderStopReason reason, Action completionHandler);

		[iOS (13, 0)] // new in this (base) type
		[MacCatalyst (13, 1)]
		[Export ("handleReport:")]
		void HandleReport (NEFilterReport report);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("filterConfiguration")]
		NEFilterProviderConfiguration FilterConfiguration { get; }

#if NET
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NoMac]
		[MacCatalyst (13, 1)]
#endif
		[Field ("NEFilterProviderRemediationMapRemediationButtonTexts")]
		NSString RemediationMapRemediationButtonTexts { get; }

#if NET
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NoMac]
		[MacCatalyst (13, 1)]
#endif
		[Field ("NEFilterProviderRemediationMapRemediationURLs")]
		NSString RemediationMapRemediationUrls { get; }
	}

	/// <summary>Contains Filter Provider configuration data.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/NetworkExtension/Reference/NEFilterProviderConfigurationClassRef/index.html">Apple documentation for <c>NEFilterProviderConfiguration</c></related>
	[NoTV]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	interface NEFilterProviderConfiguration : NSSecureCoding, NSCopying {
		/// <summary>Gets or sets a Boolean value that controls whether data that originates from WebKit browser objects should be filtered.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Deprecated (PlatformName.MacOSX, 10, 15, message: "Not supported on the platform.")]
		[Export ("filterBrowsers")]
		bool FilterBrowsers { get; set; }

		/// <summary>Gets or sets a Boolean value that controls whether data that originates from sockets should be filtered.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("filterSockets")]
		bool FilterSockets { get; set; }

		/// <summary>Gets or sets a dictionary of vendor-specific settings.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("vendorConfiguration", ArgumentSemantic.Copy)]
		NSDictionary<NSString, NSObject> VendorConfiguration { get; set; }

		/// <summary>Gets or sets the configuration server address.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("serverAddress")]
		string ServerAddress { get; set; }

		/// <summary>Gets or sets the user name.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("username")]
		string Username { get; set; }

		/// <summary>Gets or sets the name of the administering organization.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("organization")]
		string Organization { get; set; }

		/// <summary>Gets or sets the persistent reference to the password that is stored in the keychain.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("passwordReference", ArgumentSemantic.Copy)]
		NSData PasswordReference { get; set; }

		/// <summary>Gets or sets the persistent reference to a private key and certificate in the keychain.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("identityReference", ArgumentSemantic.Copy)]
		NSData IdentityReference { get; set; }

		[NoiOS]
		[MacCatalyst (15, 0)]
		[Export ("filterPackets")]
		bool FilterPackets { get; set; }

		[NoiOS]
		[MacCatalyst (15, 0)]
		[NullAllowed, Export ("filterDataProviderBundleIdentifier")]
		string FilterDataProviderBundleIdentifier { get; set; }

		[NoiOS]
		[MacCatalyst (15, 0)]
		[NullAllowed, Export ("filterPacketProviderBundleIdentifier")]
		string FilterPacketProviderBundleIdentifier { get; set; }
	}

	/// <summary>Represents a Filter Data Provider decision about network data flow after the user requests access.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/NetworkExtension/Reference/NEFilterRemediationVerdictClassRef/index.html">Apple documentation for <c>NEFilterRemediationVerdict</c></related>
	[NoTV]
	[NoMac]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NEFilterVerdict))]
	interface NEFilterRemediationVerdict : NSSecureCoding, NSCopying {
		/// <summary>Returns a verdict that indicates that the flow will be allowed to pass if it is requested in the future.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Static]
		[Export ("allowVerdict")]
		NEFilterRemediationVerdict AllowVerdict ();

		/// <summary>Returns a verdict that indicates that the flow will be not allowed to pass if it is requested in the future.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Static]
		[Export ("dropVerdict")]
		NEFilterRemediationVerdict DropVerdict ();

		/// <summary>Returns a verdict that indicates that filtering rules are needed before it can be decided whether the flow will be allowed to pass if it is requested in the future.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Static]
		[Export ("needRulesVerdict")]
		NEFilterRemediationVerdict NeedRulesVerdict ();
	}

	/// <summary>Base class for content filter verdicts.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/NetworkExtension/Reference/NEFilterVerdictClassRef/index.html">Apple documentation for <c>NEFilterVerdict</c></related>
	[NoTV]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	interface NEFilterVerdict : NSSecureCoding, NSCopying {
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("shouldReport")]
		bool ShouldReport { get; set; }
	}

	/// <summary>Contains additional data about a network data flow.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/NetworkExtension/Reference/NEFlowMetaDataClassRef/index.html">Apple documentation for <c>NEFlowMetaData</c></related>
	[NoTV]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	interface NEFlowMetaData : NSCopying, NSSecureCoding {
		/// <summary>Gets the version-specific signing identifier for the source app.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("sourceAppUniqueIdentifier")]
		NSData SourceAppUniqueIdentifier { get; }

		/// <summary>Gets the signing identifier for the source app.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("sourceAppSigningIdentifier")]
		string SourceAppSigningIdentifier { get; }

		[NoiOS]
		[MacCatalyst (15, 0)]
		[NullAllowed, Export ("sourceAppAuditToken")]
		NSData SourceAppAuditToken { get; }

		[NoTV, NoiOS, MacCatalyst (15, 0)]
		[NullAllowed, Export ("filterFlowIdentifier")]
		NSUuid FilterFlowIdentifier { get; }
	}

	/// <summary>A command handler block for a Hotspot Helper.</summary>
	[NoMac]
	[MacCatalyst (13, 1)]
	delegate void NEHotspotHelperHandler (NEHotspotHelperCommand cmd);

	/// <summary>Enables registration for Wi-Fi hotspots.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/NetworkExtension/Reference/NEHotspotHelperClassRef/index.html">Apple documentation for <c>NEHotspotHelper</c></related>
	[NoTV]
	[NoMac]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	interface NEHotspotHelper {
		[Static]
		[Internal]
		[Export ("registerWithOptions:queue:handler:")]
		bool Register ([NullAllowed] NSDictionary options, DispatchQueue queue, NEHotspotHelperHandler handler);

		/// <param name="options">To be added.</param>
		///         <param name="queue">To be added.</param>
		///         <param name="handler">To be added.</param>
		///         <summary>Registers the hotspot helper.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Static]
		[Wrap ("Register (options.GetDictionary (), queue, handler)")]
		bool Register ([NullAllowed] NEHotspotHelperOptions options, DispatchQueue queue, NEHotspotHelperHandler handler);

		/// <param name="network">To be added.</param>
		///         <summary>Ends the helper's authentication session.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Static]
		[Export ("logoff:")]
		bool Logoff (NEHotspotNetwork network);

		/// <summary>Gets the list of supported network interfaces.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Static, NullAllowed]
		[Export ("supportedNetworkInterfaces")]
		NEHotspotNetwork [] SupportedNetworkInterfaces { get; }
	}

	/// <summary>Holds the name of the network handled by the applications.</summary>
	[NoTV]
	[Static]
	[NoMac]
	[MacCatalyst (13, 1)]
	interface NEHotspotHelperOptionInternal {
		/// <summary>The name of the network handled by the application.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kNEHotspotHelperOptionDisplayName")]
		NSString DisplayName { get; }
	}

	/// <summary>A reusable request for registering Wi-Fi hotspots.</summary>
	[NoTV]
	[NoMac]
	[MacCatalyst (13, 1)]
	[Category]
	[BaseType (typeof (NSMutableUrlRequest))]
	interface NSMutableURLRequest_NEHotspotHelper {
		[Export ("bindToHotspotHelperCommand:")]
		void BindTo (NEHotspotHelperCommand command);
	}

	/// <summary>Represents commands to a Hotspot Helper.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/NetworkExtension/Reference/NEHotspotHelperCommandClassRef/index.html">Apple documentation for <c>NEHotspotHelperCommand</c></related>
	[NoTV]
	[NoMac]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	interface NEHotspotHelperCommand {
		/// <summary>Gets the helper command type.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("commandType")]
		NEHotspotHelperCommandType CommandType { get; }

		/// <summary>Gets the associated network.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("network")]
		NEHotspotNetwork Network { get; }

		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>Gets a list of the associated networks.</remarks>
		[NullAllowed, Export ("networkList")]
		NEHotspotNetwork [] NetworkList { get; }

		/// <param name="result">To be added.</param>
		///         <summary>Creates and returns a command response.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("createResponse:")]
		NEHotspotHelperResponse CreateResponse (NEHotspotHelperResult result);

		/// <param name="endpoint">To be added.</param>
		///         <summary>Creates and returns a TCP connection.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Deprecated (PlatformName.iOS, 18, 0, message: "Use 'NWParameters.RequiredInterface' with the 'Interface' property instead.")]
		[Deprecated (PlatformName.MacCatalyst, 18, 0, message: "Use 'NWParameters.RequiredInterface' with the 'Interface' property instead.")]
		[Export ("createTCPConnection:")]
		NWTcpConnection CreateTcpConnection (NWEndpoint endpoint);

		/// <param name="endpoint">To be added.</param>
		///         <summary>Creates and returns a UDP connection.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Deprecated (PlatformName.iOS, 18, 0, message: "Use 'NWParameters.RequiredInterface' with the 'Interface' property instead.")]
		[Deprecated (PlatformName.MacCatalyst, 18, 0, message: "Use 'NWParameters.RequiredInterface' with the 'Interface' property instead.")]
		[Export ("createUDPSession:")]
		NWUdpSession CreateUdpSession (NWEndpoint endpoint);

		[NoTV, NoMac, iOS (18, 0), MacCatalyst (18, 0)]
		[Export ("interface")]
		NWInterface Interface { get; }
	}

	/// <summary>Contains a Hotspot Helper command response.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/NetworkExtension/Reference/NEHotspotHelperResponseClassRef/index.html">Apple documentation for <c>NEHotspotHelperResponse</c></related>
	[NoTV]
	[NoMac]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	interface NEHotspotHelperResponse {
		/// <param name="network">To be added.</param>
		///         <summary>Sets the network that transmits the confidence information.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("setNetwork:")]
		void SetNetwork (NEHotspotNetwork network);

		/// <param name="networkList">To be added.</param>
		///         <summary>Sets the handled networks.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("setNetworkList:")]
		void SetNetworkList (NEHotspotNetwork [] networkList);

		/// <summary>Delivers the response.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("deliver")]
		void Deliver ();
	}

	/// <summary>Contains information about a Wi-Fi network hotspot.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/NetworkExtension/Reference/NEHotspotNetworkClassRef/index.html">Apple documentation for <c>NEHotspotNetwork</c></related>
	[NoTV]
	[NoMac]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	interface NEHotspotNetwork {
		/// <summary>Gets the network SSID.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("SSID")]
		string Ssid { get; }

		/// <summary>Gets the BSSID for the network.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("BSSID")]
		string Bssid { get; }

		/// <summary>Gets a value in [0.0, 1.0] that indicates the recent network strength.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("signalStrength")]
		double SignalStrength { get; }

		/// <summary>Gets a Boolean value that tells whether the network is secure.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("secure")]
		bool Secure { [Bind ("isSecure")] get; }

		/// <summary>Gets a Boolean value that tells whether the network was automatically joined.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("autoJoined")]
		bool AutoJoined { [Bind ("didAutoJoin")] get; }

		/// <summary>Gets a Boolean value that tells whether the network was very recently joined.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("justJoined")]
		bool JustJoined { [Bind ("didJustJoin")] get; }

		/// <summary>Gets a Boolean value that tells whether the hotspot helper is the chosen one for the network.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("chosenHelper")]
		bool ChosenHelper { [Bind ("isChosenHelper")] get; }

		/// <param name="confidence">To be added.</param>
		///         <summary>Sets the hotspot's confidence.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("setConfidence:")]
		void SetConfidence (NEHotspotHelperConfidence confidence);

		/// <param name="password">To be added.</param>
		///         <summary>Sets the network password.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("setPassword:")]
		void SetPassword (string password);

		[Async]
		[NoTV, NoMac, iOS (14, 0)]
		[MacCatalyst (14, 0)]
		[Static]
		[Export ("fetchCurrentWithCompletionHandler:")]
		void FetchCurrent (Action<NEHotspotNetwork> completionHandler);

		[NoTV, NoMac, iOS (15, 0), MacCatalyst (15, 0)]
		[Export ("securityType")]
		NEHotspotNetworkSecurityType SecurityType { get; }
	}

	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface NEIPv4Route : NSSecureCoding, NSCopying {
		[Export ("initWithDestinationAddress:subnetMask:")]
		NativeHandle Constructor (string address, string subnetMask);

		/// <summary>Gets the destination address.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("destinationAddress")]
		string DestinationAddress { get; }

		/// <summary>Gets the destination subnet mask.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("destinationSubnetMask")]
		string DestinationSubnetMask { get; }

		/// <summary>Gets or sets the next-hop gateway address.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("gatewayAddress")]
		string GatewayAddress { get; set; }

		/// <summary>Gets the default route.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Static]
		[Export ("defaultRoute")]
		NEIPv4Route DefaultRoute { get; }
	}

	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface NEIPv6Route : NSSecureCoding, NSCopying {
		[Export ("initWithDestinationAddress:networkPrefixLength:")]
		NativeHandle Constructor (string address, NSNumber networkPrefixLength);

		/// <summary>Gets the destination address.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("destinationAddress")]
		string DestinationAddress { get; }

		/// <summary>Gets the prefix length of the destination network.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("destinationNetworkPrefixLength")]
		NSNumber DestinationNetworkPrefixLength { get; }

		/// <summary>Gets or sets the next-hop gateway address.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("gatewayAddress")]
		string GatewayAddress { get; set; }

		/// <summary>Gets the default route.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Static]
		[Export ("defaultRoute")]
		NEIPv6Route DefaultRoute { get; }
	}

	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface NEIPv4Settings : NSSecureCoding, NSCopying {
		[Export ("initWithAddresses:subnetMasks:")]
		NativeHandle Constructor (string [] addresses, string [] subnetMasks);

		/// <summary>Gets the list of addresses to assign to the TUN interface.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("addresses")]
		string [] Addresses { get; }

		/// <summary>Gets the list of subnet masks for the corresponding addresses.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("subnetMasks")]
		string [] SubnetMasks { get; }

		/// <summary>Gets or sets the list of addresses that will be routed to TUN interface.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("includedRoutes", ArgumentSemantic.Copy)]
		NEIPv4Route [] IncludedRoutes { get; set; }

		/// <summary>Gets or sets the list of addresses that will be routed to the physical device.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("excludedRoutes", ArgumentSemantic.Copy)]
		NEIPv4Route [] ExcludedRoutes { get; set; }

		[NoTV, NoiOS, Mac (13, 0)]
		[NoMacCatalyst]
		[Export ("router")]
		[NullAllowed]
		string Router { get; set; }
	}

	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface NEIPv6Settings : NSSecureCoding, NSCopying {
		[Export ("initWithAddresses:networkPrefixLengths:")]
		NativeHandle Constructor (string [] addresses, NSNumber [] networkPrefixLengths);

		/// <summary>Gets the addresses that are assigned to the TUN interface.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("addresses")]
		string [] Addresses { get; }

		/// <summary>Gets the list of prefix lengths for each corresponding address.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("networkPrefixLengths")]
		NSNumber [] NetworkPrefixLengths { get; }

		/// <summary>Gets or sets the list of addresses that will be routed to TUN interface.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("includedRoutes", ArgumentSemantic.Copy)]
		NEIPv6Route [] IncludedRoutes { get; set; }

		/// <summary>Gets or sets the list of addresses that will be routed to the physical device.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("excludedRoutes", ArgumentSemantic.Copy)]
		NEIPv6Route [] ExcludedRoutes { get; set; }
	}

	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor] // init returns nil
	interface NEProvider {
		/// <param name="completionHandler">To be added.</param>
		///         <summary>Method that is called when the device is about to sleep.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("sleepWithCompletionHandler:")]
		[Async]
		void Sleep (Action completionHandler);

		/// <summary>Method that is called when the device wakes.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("wake")]
		void Wake ();

		/// <param name="remoteEndpoint">To be added.</param>
		///         <param name="enableTLS">To be added.</param>
		///         <param name="TLSParameters">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <param name="connectionDelegate">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <summary>Creates a TCP connection with the specified values.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Deprecated (PlatformName.iOS, 18, 0, message: "Use 'Network.NWConnection' instead.")]
		[Deprecated (PlatformName.MacCatalyst, 18, 0, message: "Use 'Network.NWConnection' instead.")]
		[Deprecated (PlatformName.MacOSX, 15, 0, message: "Use 'Network.NWConnection' instead.")]
		[Deprecated (PlatformName.TvOS, 18, 0, message: "Use 'Network.NWConnection' instead.")]
		[Export ("createTCPConnectionToEndpoint:enableTLS:TLSParameters:delegate:")]
		NWTcpConnection CreateTcpConnectionToEndpoint (NWEndpoint remoteEndpoint, bool enableTLS, [NullAllowed] NWTlsParameters TLSParameters, [NullAllowed] NSObject connectionDelegate);

		/// <param name="remoteEndpoint">To be added.</param>
		///         <param name="localEndpoint">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <summary>Creates a UDP connection with the specified values.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Deprecated (PlatformName.iOS, 18, 0, message: "Use 'Network.NWConnection' instead.")]
		[Deprecated (PlatformName.MacCatalyst, 18, 0, message: "Use 'Network.NWConnection' instead.")]
		[Deprecated (PlatformName.MacOSX, 15, 0, message: "Use 'Network.NWConnection' instead.")]
		[Deprecated (PlatformName.TvOS, 18, 0, message: "Use 'Network.NWConnection' instead.")]
		[Export ("createUDPSessionToEndpoint:fromEndpoint:")]
		NWUdpSession CreateUdpSessionToEndpoint (NWEndpoint remoteEndpoint, [NullAllowed] NWHostEndpoint localEndpoint);

		/// <summary>Gets the default connection path for new connections.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Deprecated (PlatformName.iOS, 18, 0, message: "Use 'Network.NWPathMonitor' instead.")]
		[Deprecated (PlatformName.MacCatalyst, 18, 0, message: "Use 'Network.NWPathMonitor' instead.")]
		[Deprecated (PlatformName.MacOSX, 15, 0, message: "Use 'Network.NWPathMonitor' instead.")]
		[Deprecated (PlatformName.TvOS, 18, 0, message: "Use 'Network.NWPathMonitor' instead.")]
		[NullAllowed, Export ("defaultPath")]
		NWPath DefaultPath { get; }

		/// <param name="message">To be added.</param>
		///         <param name="completionHandler">To be added.</param>
		///         <summary>Displays a message to the user and passes a Boolean result to a completion handler when it is finished.</summary>
		///         <remarks>To be added.</remarks>
		[NoTV]
		[Deprecated (PlatformName.iOS, 12, 0)]
		[Deprecated (PlatformName.MacOSX, 10, 14)]
		[MacCatalyst (13, 1)]
		[Deprecated (PlatformName.MacCatalyst, 13, 1)]
		[Export ("displayMessage:completionHandler:")]
		[Async]
		void DisplayMessage (string message, Action<bool> completionHandler);

		[NoiOS]
		[MacCatalyst (15, 0)]
		[Static]
		[Export ("startSystemExtensionMode")]
		void StartSystemExtensionMode ();
	}

	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	interface NEProxySettings : NSSecureCoding, NSCopying {
		/// <summary>Gets or sets a Boolean value that controls whether proxies can be auto-configured.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("autoProxyConfigurationEnabled")]
		bool AutoProxyConfigurationEnabled { get; set; }

		/// <summary>Gets or sets the URL to the Proxy Auto Configuration script.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("proxyAutoConfigurationURL", ArgumentSemantic.Copy)]
		NSUrl ProxyAutoConfigurationUrl { get; set; }

		/// <summary>Gets or sets the Proxy Auto Configuration source code, in JavaScript.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("proxyAutoConfigurationJavaScript")]
		string ProxyAutoConfigurationJavaScript { get; set; }

		/// <summary>Gets or sets a Boolean value that controls whether the settings are enabled for the HTTP protocol.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("HTTPEnabled")]
		bool HttpEnabled { get; set; }

		/// <summary>Gets or sets the server that contains the settings for static HTTP server proxyies.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("HTTPServer", ArgumentSemantic.Copy)]
		NEProxyServer HttpServer { get; set; }

		/// <summary>Gets or sets a Boolean value that controls whether the settings are enabled for the HTTPS protocol.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("HTTPSEnabled")]
		bool HttpsEnabled { get; set; }

		/// <summary>Gets or sets the server that contains the settings for static HTTPS server proxyies.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("HTTPSServer", ArgumentSemantic.Copy)]
		NEProxyServer HttpsServer { get; set; }

		/// <summary>Gets or sets a Boolean value that controls whether single label host names are exempt from the proxy settings.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("excludeSimpleHostnames")]
		bool ExcludeSimpleHostnames { get; set; }

		/// <summary>Gets or sets a list of domain name patterns for which matching domains re exempt from the proxy settings.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("exceptionList", ArgumentSemantic.Copy)]
		string [] ExceptionList { get; set; }

		/// <summary>Gets or sets a list of domains for which the proxy settings are applied.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("matchDomains", ArgumentSemantic.Copy)]
		string [] MatchDomains { get; set; }
	}

	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface NEProxyServer : NSSecureCoding, NSCopying {
		[Export ("initWithAddress:port:")]
		NativeHandle Constructor (string address, nint port);

		/// <summary>Gets the address that was specified when the proxy server was created.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("address")]
		string Address { get; }

		/// <summary>Gets the port that was specified when the proxy server was created.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("port")]
		nint Port { get; }

		/// <summary>Gets or sets a Boolean value that controls whether authentication is needed on the server.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("authenticationRequired")]
		bool AuthenticationRequired { get; set; }

		/// <summary>Gets or sets the user name.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("username")]
		string Username { get; set; }

		/// <summary>Gets or sets the password.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("password")]
		string Password { get; set; }
	}

	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface NETunnelNetworkSettings : NSSecureCoding, NSCopying {
		[Export ("initWithTunnelRemoteAddress:")]
		NativeHandle Constructor (string address);

		/// <summary>Gets the remote address that was provided when the network tunnel object was created.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("tunnelRemoteAddress")]
		string TunnelRemoteAddress { get; }

		/// <summary>Gets or sets the DNS settings for the tunnel.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("DNSSettings", ArgumentSemantic.Copy)]
		NEDnsSettings DnsSettings { get; set; }

		/// <summary>Gets or sets the proxy settings for the tunnel.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("proxySettings", ArgumentSemantic.Copy)]
		NEProxySettings ProxySettings { get; set; }
	}

	[MacCatalyst (13, 1)]
	[BaseType (typeof (NEProvider))]
	[DisableDefaultCtor] // init returns nil
	interface NETunnelProvider {
		/// <param name="messageData">To be added.</param>
		///         <param name="completionHandler">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <summary>Method that is called to handle messages from the containing app.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("handleAppMessage:completionHandler:")]
		[Async]
		void HandleAppMessage (NSData messageData, [NullAllowed] Action<NSData> completionHandler);

		/// <param name="tunnelNetworkSettings">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <param name="completionHandler">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <summary>Updates the network settings for the tunnel.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("setTunnelNetworkSettings:completionHandler:")]
		[Async]
		void SetTunnelNetworkSettings ([NullAllowed] NETunnelNetworkSettings tunnelNetworkSettings, [NullAllowed] Action<NSError> completionHandler);

		/// <summary>Gets the tunnel configuration.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("protocolConfiguration")]
		NEVpnProtocol ProtocolConfiguration { get; }

		/// <summary>Gets the rules that control which apps will use the tunneling session.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NoTV]
		[NullAllowed, Export ("appRules")]
		NEAppRule [] AppRules { get; }

		/// <summary>Gets the routing method.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NoTV]
		[Export ("routingMethod")]
		NETunnelProviderRoutingMethod RoutingMethod { get; }

		/// <summary>Gets or sets a Boolean value that tells the system that the tunnel is being reestablished.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("reasserting")]
		bool Reasserting { get; set; }
	}

	[MacCatalyst (13, 1)]
	[BaseType (typeof (NEVpnManager))]
	interface NETunnelProviderManager {
		/// <param name="completionHandler">To be added.</param>
		///         <summary>Loads all of the calling app's VPN configurations from the Network Extension preferences and runs a completion handler when the operation is complete.</summary>
		///         <remarks>To be added.</remarks>
		[Static]
		[Export ("loadAllFromPreferencesWithCompletionHandler:")]
		[Async]
		void LoadAllFromPreferences (Action<NSArray, NSError> completionHandler);

		[NoTV, NoiOS, MacCatalyst (15, 0)]
		[Static]
		[Export ("forPerAppVPN")]
		NETunnelProviderManager CreatePerAppVpn ();

		[NoTV]
		[return: NullAllowed]
		[Export ("copyAppRules")]
		NEAppRule [] CopyAppRules ();

		// CopyAppRules was incorrectly bound to AppRules and it is only available on macOS
#if NET || MONOMAC || __MACCATALYST__
		/// <summary>Gets a copy of the rules that control which apps can use the tunnel.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NoTV, NoiOS, MacCatalyst (15, 0)]
		[Export ("appRules", ArgumentSemantic.Copy)]
		NEAppRule [] AppRules { get; set; }
#else
		[NoTV]
		[Obsolete ("Use 'CopyAppRules' instead, this property will be removed in the future.")]
		NEAppRule [] AppRules { [Wrap ("CopyAppRules ()!", IsVirtual = true)] get; }
#endif

		/// <summary>Gets or sets the routing method.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NoTV]
		[Export ("routingMethod")]
		NETunnelProviderRoutingMethod RoutingMethod { get; }

		[NoTV, NoiOS, MacCatalyst (15, 0)]
		[Export ("safariDomains", ArgumentSemantic.Copy)]
		string [] SafariDomains { get; set; }

		[NoTV, NoiOS, MacCatalyst (15, 0)]
		[Export ("mailDomains", ArgumentSemantic.Copy)]
		string [] MailDomains { get; set; }

		[NoTV, NoiOS, MacCatalyst (15, 0)]
		[Export ("calendarDomains", ArgumentSemantic.Copy)]
		string [] CalendarDomains { get; set; }

		[NoTV, NoiOS, MacCatalyst (15, 0)]
		[Export ("contactsDomains", ArgumentSemantic.Copy)]
		string [] ContactsDomains { get; set; }

#if !NET
		[Field ("NETunnelProviderErrorDomain")]
		NSString ErrorDomain { get; }
#endif

		[NoTV, NoiOS, MacCatalyst (15, 0)]
		[Export ("excludedDomains", ArgumentSemantic.Copy)]
		string [] ExcludedDomains { get; set; }

		[NoTV, NoiOS, MacCatalyst (15, 0)]
		[Export ("associatedDomains", ArgumentSemantic.Copy)]
		string [] AssociatedDomains { get; set; }
	}


	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject), Name = "NEVPNManager")]
	[DisableDefaultCtor] // Assertion failed: (0), function -[NEVPNManager init], file /SourceCache/NetworkExtension_Sim/NetworkExtension-168.1.8/Framework/NEVPNManager.m, line 41.
	interface NEVpnManager {

		/// <summary>Gets or sets the rules that control when on-demand connections will occur.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed]
		[Export ("onDemandRules", ArgumentSemantic.Copy)]
		NEOnDemandRule [] OnDemandRules { get; set; }

		/// <summary>Gets or sets a Boolean value that controls whether on-demand connections are allowed.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("onDemandEnabled")]
		bool OnDemandEnabled { [Bind ("isOnDemandEnabled")] get; set; }

		/// <summary>Gets or sets the localized description of the VPN manager.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NullAllowed]
		[Export ("localizedDescription")]
		string LocalizedDescription { get; set; }

		/// <summary>Developers should not use this deprecated property. Developers should use 'ProtocolConfiguration' instead.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NoTV]
		[NullAllowed]
		[Export ("protocol", ArgumentSemantic.Retain)]
		[Deprecated (PlatformName.iOS, 9, 0, message: "Use 'ProtocolConfiguration' instead.")]
		[Deprecated (PlatformName.MacOSX, 10, 11, message: "Use 'ProtocolConfiguration' instead.")]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'ProtocolConfiguration' instead.")]
		NEVpnProtocol Protocol { get; set; }

		/// <summary>Gets or sets the protocol configuration for the VPN tunnel.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[NullAllowed]
		[Export ("protocolConfiguration", ArgumentSemantic.Retain)]
		NEVpnProtocol ProtocolConfiguration { get; set; }

		/// <summary>Gets the VPN connection.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("connection")]
		NEVpnConnection Connection { get; }

		/// <summary>Gets or sets a Boolean value that controls whether the VPN configuration is valid.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("enabled")]
		bool Enabled { [Bind ("isEnabled")] get; set; }

		/// <summary>Gets the shared VPN manager instance.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Static, Export ("sharedManager")]
		NEVpnManager SharedManager { get; }

		/// <param name="completionHandler">To be added.</param>
		///         <summary>Loads the saved VPN configuration from the Network Extension preferences and runs a completion handler when the operation completes.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("loadFromPreferencesWithCompletionHandler:")]
		[Async]
		void LoadFromPreferences (Action<NSError> completionHandler); // nonnull !

		/// <param name="completionHandler">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <summary>Removes the configuration for this VPN manager from the Network Extension preferences and runs a completion handler when the operation completes.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("removeFromPreferencesWithCompletionHandler:")]
		[Async]
		void RemoveFromPreferences ([NullAllowed] Action<NSError> completionHandler);

		/// <param name="completionHandler">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <summary>Saves the configuration for this VPN manager to the Network Extension preferences and runs a completion handler when the operation completes.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("saveToPreferencesWithCompletionHandler:")]
		[Async]
		void SaveToPreferences ([NullAllowed] Action<NSError> completionHandler);

		[NoiOS]
		[NoMacCatalyst]
		[NoTV]
		[Internal]
		[Export ("setAuthorization:")]
		void _SetAuthorization (IntPtr auth);

#if !NET
		[Field ("NEVPNErrorDomain")]
		NSString ErrorDomain { get; }
#endif

		/// <include file="../docs/api/NetworkExtension/NEVpnManager.xml" path="/Documentation/Docs[@DocId='P:NetworkExtension.NEVpnManager.ConfigurationChangeNotification']/*" />
		[Notification]
		[Field ("NEVPNConfigurationChangeNotification")]
		NSString ConfigurationChangeNotification { get; }
	}

	[MacCatalyst (13, 1), TV (17, 0)]
	[BaseType (typeof (NSObject), Name = "NEVPNConnection")]
	interface NEVpnConnection {

		/// <summary>Gets the date and time when the VPN connected.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[NullAllowed, Export ("connectedDate")]
		NSDate ConnectedDate { get; }

		/// <summary>Gets the VPN connection status.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("status")]
		NEVpnStatus Status { get; }

		/// <param name="error">To be added.</param>
		///         <summary>Begins connecting the VPN.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("startVPNTunnelAndReturnError:")]
		bool StartVpnTunnel (out NSError error);

		[MacCatalyst (13, 1)]
		[Internal]
		[Export ("startVPNTunnelWithOptions:andReturnError:")]
		bool StartVpnTunnel ([NullAllowed] NSDictionary options, out NSError error);

		/// <param name="options">To be added.</param>
		///         <param name="error">To be added.</param>
		///         <summary>Begins connecting the VPN.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Wrap ("StartVpnTunnel (options.GetDictionary (), out error);")]
		bool StartVpnTunnel ([NullAllowed] NEVpnConnectionStartOptions options, out NSError error);

		/// <summary>Begins disconnecting the VPN.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("stopVPNTunnel")]
		void StopVpnTunnel ();

		/// <summary>Gets the VPN manager.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("manager")]
		NEVpnManager Manager { get; }

		/// <include file="../docs/api/NetworkExtension/NEVpnConnection.xml" path="/Documentation/Docs[@DocId='P:NetworkExtension.NEVpnConnection.StatusDidChangeNotification']/*" />
		[Notification]
		[Field ("NEVPNStatusDidChangeNotification")]
		NSString StatusDidChangeNotification { get; }

		[Async]
		[Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0)]
		[Export ("fetchLastDisconnectErrorWithCompletionHandler:")]
		void FetchLastDisconnectError (Action<NSError> handler);
	}

	[Static]
	[Internal]
	[MacCatalyst (13, 1)]
	interface NEVpnConnectionStartOptionInternal {
		[Field ("NEVPNConnectionStartOptionPassword")]
		NSString Password { get; }

		[Field ("NEVPNConnectionStartOptionUsername")]
		NSString Username { get; }
	}

	[MacCatalyst (13, 1)]
	[Abstract]
	[BaseType (typeof (NSObject), Name = "NEVPNProtocol")]
	interface NEVpnProtocol : NSCopying, NSSecureCoding {

		/// <summary>Gets or sets the tunneling server address.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed] // by default this property is null
		[Export ("serverAddress")]
		string ServerAddress { get; set; }

		/// <summary>Gets or sets the user name.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed] // by default this property is null
		[Export ("username")]
		string Username { get; set; }

		/// <summary>Gets or sets a reference to the keychain item for the password.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed] // by default this property is null
		[Export ("passwordReference", ArgumentSemantic.Copy)]
		NSData PasswordReference { get; set; }

		/// <summary>Gets or sets a reference to the keychain item for the PKCS12 tunneling credential.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[NullAllowed, Export ("identityReference", ArgumentSemantic.Copy)]
		NSData IdentityReference { get; set; }

		/// <summary>Gets or sets the private key and certificate data for the tunnel as PKCS12 formatted data.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[NullAllowed] // by default this property is null
		[Export ("identityData", ArgumentSemantic.Copy)]
		NSData IdentityData { get; set; }

		/// <summary>Gets or sets the password for the PKCS12 data in the IdentityData property</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[NullAllowed] // by default this property is null
		[Export ("identityDataPassword")]
		string IdentityDataPassword { get; set; }

		/// <summary>Gets or sets a Boolean value that controls whether the VPN should disconnect when the device sleeps.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("disconnectOnSleep")]
		bool DisconnectOnSleep { get; set; }

		/// <summary>Gets or sets the proxy settings for HTTPS and HTTP connections through the VPN tunnel.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[NullAllowed, Export ("proxySettings", ArgumentSemantic.Copy)]
		NEProxySettings ProxySettings { get; set; }

		[iOS (14, 0)]
		[MacCatalyst (14, 0)]
		[Export ("includeAllNetworks")]
		bool IncludeAllNetworks { get; set; }

		[iOS (14, 2)]
		[MacCatalyst (14, 2)]
		[Export ("excludeLocalNetworks")]
		bool ExcludeLocalNetworks { get; set; }

		[NoTV, Mac (13, 3), iOS (16, 4), MacCatalyst (16, 4)]
		[Export ("excludeCellularServices")]
		bool ExcludeCellularServices { get; set; }

		[NoTV, Mac (13, 3), iOS (16, 4), MacCatalyst (16, 4)]
		[Export ("excludeAPNs")]
		bool ExcludeApns { get; set; }

		[NoTV, Mac (14, 4), iOS (17, 4), MacCatalyst (17, 4)]
		[Export ("excludeDeviceCommunication")]
		bool ExcludeDeviceCommunication { get; set; }

		[Mac (11, 0)]
		[iOS (14, 2)]
		[MacCatalyst (14, 2)]
		[Export ("enforceRoutes")]
		bool EnforceRoutes { get; set; }

		[NoTV, NoMac, iOS (18, 0), MacCatalyst (18, 0)]
		[Export ("sliceUUID", ArgumentSemantic.Copy), NullAllowed]
		string SliceUuid { get; set; }
	}

	[MacCatalyst (13, 1)]
	[BaseType (typeof (NEVpnProtocol), Name = "NEVPNProtocolIPSec")]
	interface NEVpnProtocolIpSec {

		/// <summary>Gets or sets the method for IPSec server authentication.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("authenticationMethod")]
		NEVpnIkeAuthenticationMethod AuthenticationMethod { get; set; }

		/// <summary>Gets or sets a Boolean value that controls whether to use extended negotiation.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("useExtendedAuthentication")]
		bool UseExtendedAuthentication { get; set; }

		/// <summary>Gets or sets a reference to the IKE shared secret in the keychain.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed] // by default this property is null
		[Export ("sharedSecretReference", ArgumentSemantic.Copy)]
		NSData SharedSecretReference { get; set; }

		/// <summary>Gets or sets the local device for authentication.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed] // by default this property is null
		[Export ("localIdentifier")]
		string LocalIdentifier { get; set; }

		/// <summary>Gets or sets the identifier IPSec server.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed] // by default this property is null
		[Export ("remoteIdentifier")]
		string RemoteIdentifier { get; set; }
	}

	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject), Name = "NEVPNIKEv2SecurityAssociationParameters")]
	interface NEVpnIke2SecurityAssociationParameters : NSSecureCoding, NSCopying {

		/// <summary>Gets or sets the encryption algorithm for the Security Association.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("encryptionAlgorithm")]
		NEVpnIke2EncryptionAlgorithm EncryptionAlgorithm { get; set; }

		/// <summary>Gets or sets the integrity algorithm for the Security Association.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("integrityAlgorithm")]
		NEVpnIke2IntegrityAlgorithm IntegrityAlgorithm { get; set; }

		/// <summary>Gets or sets the Security Association's Diffie Hellman group.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("diffieHellmanGroup")]
		NEVpnIke2DiffieHellman DiffieHellmanGroup { get; set; }

		/// <summary>Gets or sets the Security Association's lifetime, in mintues.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("lifetimeMinutes")]
		int LifetimeMinutes { get; set; } /* int32_t */
	}

	[MacCatalyst (13, 1)]
	[BaseType (typeof (NEVpnProtocolIpSec), Name = "NEVPNProtocolIKEv2")]
	interface NEVpnProtocolIke2 {

		/// <summary>Gets or sets the rate at which the IKEv2 client will attempt to detect dead peers.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("deadPeerDetectionRate")]
		NEVpnIke2DeadPeerDetectionRate DeadPeerDetectionRate { get; set; }

		/// <summary>Gets or sets the common name of issuer of the server certificate.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed] // by default this property is null
		[Export ("serverCertificateIssuerCommonName")]
		string ServerCertificateIssuerCommonName { get; set; }

		/// <summary>Gets or sets the common name of the server certificate.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed] // by default this property is null
		[Export ("serverCertificateCommonName")]
		string ServerCertificateCommonName { get; set; }

		/// <summary>Gets the security association parameters for the initial negotiation with the IKEv2 server.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("IKESecurityAssociationParameters")]
		NEVpnIke2SecurityAssociationParameters IKESecurityAssociationParameters { get; }

		/// <summary>Gets the security association parameters for child IPSec security associations that will be negotiated with the IKEv2 server.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("childSecurityAssociationParameters")]
		NEVpnIke2SecurityAssociationParameters ChildSecurityAssociationParameters { get; }

		/// <summary>Gets or sets the certificate type.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("certificateType")]
		NEVpnIke2CertificateType CertificateType { get; set; }

		/// <summary>Gets or sets a Boolean value that controls whether to use the internal subnets that are sent by the IKEv2 server.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("useConfigurationAttributeInternalIPSubnet")]
		bool UseConfigurationAttributeInternalIPSubnet { get; set; }

		/// <summary>Gets or sets a Boolean value that enables and disables MOBIKE.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("disableMOBIKE")]
		bool DisableMobike { get; set; }

		/// <summary>Gets or sets a Boolean value that enables and disables redirects by IKEv2.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("disableRedirect")]
		bool DisableRedirect { get; set; }

		/// <summary>Gets or sets a Boolean value that enables and disables Perfect Forward Secrecy.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("enablePFS")]
		bool EnablePfs { get; set; }

		/// <summary>Gets or sets a Boolean value that enables and disables revocation checking.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("enableRevocationCheck")]
		bool EnableRevocationCheck { get; set; }

		/// <summary>Gets or sets a Boolean value that enables or disables strict revocation checks.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("strictRevocationCheck")]
		bool StrictRevocationCheck { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("minimumTLSVersion", ArgumentSemantic.Assign)]
		NEVpnIkev2TlsVersion MinimumTlsVersion { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("maximumTLSVersion", ArgumentSemantic.Assign)]
		NEVpnIkev2TlsVersion MaximumTlsVersion { get; set; }

		[NoMac]
		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Export ("enableFallback")]
		bool EnableFallback { get; set; }

		[iOS (14, 0)]
		[MacCatalyst (14, 0)]
		[Export ("mtu")]
		nuint Mtu { get; set; }

		[TV (18, 0), Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
		[Export ("ppkConfiguration", ArgumentSemantic.Copy), NullAllowed]
		NEVpnIkev2PpkConfiguration PpkConfiguration { get; set; }
	}

	[MacCatalyst (13, 1)]
	[Abstract]
	[BaseType (typeof (NSObject))]
	interface NEOnDemandRule : NSSecureCoding, NSCopying {

		/// <summary>Gets the action for the rule</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("action")]
		NEOnDemandRuleAction Action { get; }

		/// <summary>Gets or sets the DNS search domains to match.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed]
		[Export ("DNSSearchDomainMatch")]
		string [] DnsSearchDomainMatch { get; set; }

		/// <summary>Gets or sets the DNS server addresses to match.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed]
		[Export ("DNSServerAddressMatch")]
		string [] DnsServerAddressMatch { get; set; }

		/// <summary>Gets or sets the interface type to match.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("interfaceTypeMatch")]
		NEOnDemandRuleInterfaceType InterfaceTypeMatch { get; set; }

		/// <summary>Gets or sets the SSID to match.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed]
		[Export ("SSIDMatch")]
		string [] SsidMatch { get; set; }

		/// <summary>Gets or sets the URL at which to probe for an expected resource when the rule matches.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed]
		[Export ("probeURL", ArgumentSemantic.Copy)]
		NSUrl ProbeUrl { get; set; }
	}

	[MacCatalyst (13, 1)]
	[BaseType (typeof (NEOnDemandRule))]
	interface NEOnDemandRuleConnect {
	}

	[MacCatalyst (13, 1)]
	[BaseType (typeof (NEOnDemandRule))]
	interface NEOnDemandRuleDisconnect {
	}

	[MacCatalyst (13, 1)]
	[BaseType (typeof (NEOnDemandRule))]
	interface NEOnDemandRuleIgnore {
	}

	[MacCatalyst (13, 1)]
	[BaseType (typeof (NEOnDemandRule))]
	interface NEOnDemandRuleEvaluateConnection {

		/// <summary>Gets or sets the connection rules.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed]
		[Export ("connectionRules", ArgumentSemantic.Copy)]
		NEEvaluateConnectionRule [] ConnectionRules { get; set; }
	}

	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	interface NEEvaluateConnectionRule : NSSecureCoding, NSCopying {

		[Export ("initWithMatchDomains:andAction:")]
		NativeHandle Constructor (string [] domains, NEEvaluateConnectionRuleAction action);

		/// <summary>Gets the action to run when the network connection properties satisfy the rule for a new connection.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("action")]
		NEEvaluateConnectionRuleAction Action { get; }

		/// <summary>The domains for which to run the rule.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("matchDomains")]
		string [] MatchDomains { get; }

		/// <summary>Gets an array of DNS server names to use for resolving the destination hostname when evaluating connectivity, if the rule is <see cref="F:NetworkExtension.NEEvaluateConnectionRuleAction.ConnectIfNeeded" /></summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed]
		[Export ("useDNSServers", ArgumentSemantic.Copy)]
		string [] UseDnsServers { get; set; }

		/// <summary>A URL for which to start a VPN if a non-success HTTP code is returned, if the rule is <see cref="F:NetworkExtension.NEEvaluateConnectionRuleAction.ConnectIfNeeded" />.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed]
		[Export ("probeURL", ArgumentSemantic.Copy)]
		NSUrl ProbeUrl { get; set; }
	}

	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	[Abstract]
	[Deprecated (PlatformName.iOS, 18, 0, message: "Use 'Network.NWEndpoint' instead.")]
	[Deprecated (PlatformName.MacCatalyst, 18, 0, message: "Use 'Network.NWEndpoint' instead.")]
	[Deprecated (PlatformName.MacOSX, 15, 0, message: "Use 'Network.NWEndpoint' instead.")]
	[Deprecated (PlatformName.TvOS, 18, 0, message: "Use 'Network.NWEndpoint' instead.")]
	interface NWEndpoint : NSSecureCoding, NSCopying {
	}

	[Deprecated (PlatformName.iOS, 18, 0, message: "Use 'Network.NWEndpoint' instead.")]
	[Deprecated (PlatformName.MacCatalyst, 18, 0, message: "Use 'Network.NWEndpoint' instead.")]
	[Deprecated (PlatformName.MacOSX, 15, 0, message: "Use 'Network.NWEndpoint' instead.")]
	[Deprecated (PlatformName.TvOS, 18, 0, message: "Use 'Network.NWEndpoint' instead.")]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NWEndpoint))]
	[DisableDefaultCtor]
	interface NWHostEndpoint {
		/// <param name="hostname">To be added.</param>
		///         <param name="port">To be added.</param>
		///         <summary>Creates and returns a new host endpoint with the provided values.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Deprecated (PlatformName.iOS, 18, 0, message: "Use 'Network.NWEndpoint.Create' instead.")]
		[Deprecated (PlatformName.MacCatalyst, 18, 0, message: "Use 'Network.NWEndpoint.Create' instead.")]
		[Deprecated (PlatformName.MacOSX, 15, 0, message: "Use 'Network.NWEndpoint.Create' instead.")]
		[Deprecated (PlatformName.TvOS, 18, 0, message: "Use 'Network.NWEndpoint.Create' instead.")]
		[Static]
		[Export ("endpointWithHostname:port:")]
		NWHostEndpoint Create (string hostname, string port);

		/// <summary>Gets the host name.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Deprecated (PlatformName.iOS, 18, 0, message: "Use 'Network.NWEndpoint.Hostname' instead.")]
		[Deprecated (PlatformName.MacCatalyst, 18, 0, message: "Use 'Network.NWEndpoint.Hostname' instead.")]
		[Deprecated (PlatformName.MacOSX, 15, 0, message: "Use 'Network.NWEndpoint.Hostname' instead.")]
		[Deprecated (PlatformName.TvOS, 18, 0, message: "Use 'Network.NWEndpoint.Hostname' instead.")]
		[Export ("hostname")]
		string Hostname { get; }

		/// <summary>Gets the port.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Deprecated (PlatformName.iOS, 18, 0, message: "Use 'Network.NWEndpoint.PortNumber' instead.")]
		[Deprecated (PlatformName.MacCatalyst, 18, 0, message: "Use 'Network.NWEndpoint.PortNumber' instead.")]
		[Deprecated (PlatformName.MacOSX, 15, 0, message: "Use 'Network.NWEndpoint.PortNumber' instead.")]
		[Deprecated (PlatformName.TvOS, 18, 0, message: "Use 'Network.NWEndpoint.PortNumber' instead.")]
		[Export ("port")]
		string Port { get; }
	}

	[Deprecated (PlatformName.iOS, 18, 0, message: "Use 'Network.NWEndpoint' instead.")]
	[Deprecated (PlatformName.MacCatalyst, 18, 0, message: "Use 'Network.NWEndpoint' instead.")]
	[Deprecated (PlatformName.MacOSX, 15, 0, message: "Use 'Network.NWEndpoint' instead.")]
	[Deprecated (PlatformName.TvOS, 18, 0, message: "Use 'Network.NWEndpoint' instead.")]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NWEndpoint))]
	[DisableDefaultCtor]
	interface NWBonjourServiceEndpoint {

		/// <param name="name">To be added.</param>
		///         <param name="type">To be added.</param>
		///         <param name="domain">To be added.</param>
		///         <summary>Creates and returns a new Bonjour service endpoint with the provided values.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Deprecated (PlatformName.iOS, 18, 0, message: "Use 'Network.NWEndpoint.CreateBonjourService' instead.")]
		[Deprecated (PlatformName.MacCatalyst, 18, 0, message: "Use 'Network.NWEndpoint.CreateBonjourService' instead.")]
		[Deprecated (PlatformName.MacOSX, 15, 0, message: "Use 'Network.NWEndpoint.CreateBonjourService' instead.")]
		[Deprecated (PlatformName.TvOS, 18, 0, message: "Use 'Network.NWEndpoint.CreateBonjourService' instead.")]
		[Static]
		[Export ("endpointWithName:type:domain:")]
		NWBonjourServiceEndpoint Create (string name, string type, string domain);

		/// <summary>Gets the service name of the Bonjour endpoint.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Deprecated (PlatformName.iOS, 18, 0, message: "Use 'Network.NWEndpoint.BonjourServiceName' instead.")]
		[Deprecated (PlatformName.MacCatalyst, 18, 0, message: "Use 'Network.NWEndpoint.BonjourServiceName' instead.")]
		[Deprecated (PlatformName.MacOSX, 15, 0, message: "Use 'Network.NWEndpoint.BonjourServiceName' instead.")]
		[Deprecated (PlatformName.TvOS, 18, 0, message: "Use 'Network.NWEndpoint.BonjourServiceName' instead.")]
		[Export ("name")]
		string Name { get; }

		/// <summary>Gets the type of the Bonjour service.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Deprecated (PlatformName.iOS, 18, 0, message: "Use 'Network.NWEndpoint.BonjourServiceType' instead.")]
		[Deprecated (PlatformName.MacCatalyst, 18, 0, message: "Use 'Network.NWEndpoint.BonjourServiceType' instead.")]
		[Deprecated (PlatformName.MacOSX, 15, 0, message: "Use 'Network.NWEndpoint.BonjourServiceType' instead.")]
		[Deprecated (PlatformName.TvOS, 18, 0, message: "Use 'Network.NWEndpoint.BonjourServiceType' instead.")]

		[Export ("type")]
		string Type { get; }

		/// <summary>Gets the Bonjour domain of the endpoint.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Deprecated (PlatformName.iOS, 18, 0, message: "Use 'Network.NWEndpoint.BonjourServiceDomain' instead.")]
		[Deprecated (PlatformName.MacCatalyst, 18, 0, message: "Use 'Network.NWEndpoint.BonjourServiceDomain' instead.")]
		[Deprecated (PlatformName.MacOSX, 15, 0, message: "Use 'Network.NWEndpoint.BonjourServiceDomain' instead.")]
		[Deprecated (PlatformName.TvOS, 18, 0, message: "Use 'Network.NWEndpoint.BonjourServiceDomain' instead.")]

		[Export ("domain")]
		string Domain { get; }
	}

	[Deprecated (PlatformName.iOS, 18, 0, message: "Use 'Network.NWPath' instead.")]
	[Deprecated (PlatformName.MacCatalyst, 18, 0, message: "Use 'Network.NWPath' instead.")]
	[Deprecated (PlatformName.MacOSX, 15, 0, message: "Use 'Network.NWPath' instead.")]
	[Deprecated (PlatformName.TvOS, 18, 0, message: "Use 'Network.NWPath' instead.")]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface NWPath {
		/// <summary>Gets a value that describes the status of the path, such as whether it is ready to be used or is invalid.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Deprecated (PlatformName.iOS, 18, 0, message: "Use 'Network.NWPath.Status' instead.")]
		[Deprecated (PlatformName.MacCatalyst, 18, 0, message: "Use 'Network.NWPath.Status' instead.")]
		[Deprecated (PlatformName.MacOSX, 15, 0, message: "Use 'Network.NWPath.Status' instead.")]
		[Deprecated (PlatformName.TvOS, 18, 0, message: "Use 'Network.NWPath.Status' instead.")]
		[Export ("status")]
		NWPathStatus Status { get; }

		/// <summary>Gets a Boolean value that tells whether the path uses an expensive interface, such as a cellular network.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Deprecated (PlatformName.iOS, 18, 0, message: "Use 'Network.NWPath.IsExpensive' instead.")]
		[Deprecated (PlatformName.MacCatalyst, 18, 0, message: "Use 'Network.NWPath.IsExpensive' instead.")]
		[Deprecated (PlatformName.MacOSX, 15, 0, message: "Use 'Network.NWPath.IsExpensive' instead.")]
		[Deprecated (PlatformName.TvOS, 18, 0, message: "Use 'Network.NWPath.IsExpensive' instead.")]
		[Export ("expensive")]
		bool Expensive { [Bind ("isExpensive")] get; }

		/// <param name="path">To be added.</param>
		///         <summary>Returns <see langword="true" /> if this path represents the same path as the specified <paramref name="path" />.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Deprecated (PlatformName.iOS, 18, 0, message: "Use 'Network.NWPath.EqualTo' instead.")]
		[Deprecated (PlatformName.MacCatalyst, 18, 0, message: "Use 'Network.NWPath.EqualTo' instead.")]
		[Deprecated (PlatformName.MacOSX, 15, 0, message: "Use 'Network.NWPath.EqualTo' instead.")]
		[Deprecated (PlatformName.TvOS, 18, 0, message: "Use 'Network.NWPath.EqualTo' instead.")]
		[Export ("isEqualToPath:")]
		bool IsEqualToPath (NWPath path);

		[Deprecated (PlatformName.iOS, 18, 0, message: "Use 'Network.NWPath.IsConstrained' instead.")]
		[Deprecated (PlatformName.MacCatalyst, 18, 0, message: "Use 'Network.NWPath.IsConstrained' instead.")]
		[Deprecated (PlatformName.MacOSX, 15, 0, message: "Use 'Network.NWPath.IsConstrained' instead.")]
		[Deprecated (PlatformName.TvOS, 18, 0, message: "Use 'Network.NWPath.IsConstrained' instead.")]
		[TV (14, 0), iOS (14, 0)]
		[MacCatalyst (14, 0)]
		[Export ("constrained")]
		bool Constrained { [Bind ("isConstrained")] get; }
	}

	[Deprecated (PlatformName.iOS, 18, 0, message: "Use 'Network.NWConnection' instead.")]
	[Deprecated (PlatformName.MacCatalyst, 18, 0, message: "Use 'Network.NWConnection' instead.")]
	[Deprecated (PlatformName.MacOSX, 15, 0, message: "Use 'Network.NWConnection' instead.")]
	[Deprecated (PlatformName.TvOS, 18, 0, message: "Use 'Network.NWConnection' instead.")]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject), Name = "NWTCPConnection")]
	interface NWTcpConnection {
		[Deprecated (PlatformName.iOS, 18, 0, message: "Use the 'Network.NWConnection' constructor instead.")]
		[Deprecated (PlatformName.MacCatalyst, 18, 0, message: "Use the 'Network.NWConnection' constructor instead.")]
		[Deprecated (PlatformName.MacOSX, 15, 0, message: "Use the 'Network.NWConnection' constructor instead.")]
		[Deprecated (PlatformName.TvOS, 18, 0, message: "Use the 'Network.NWConnection' constructor instead.")]
		[Export ("initWithUpgradeForConnection:")]
		NativeHandle Constructor (NWTcpConnection connection);

		/// <summary>Gets the state of the connection.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Deprecated (PlatformName.iOS, 18, 0, message: "Use 'Network.NWConnection.SetStateChangedHandler' instead.")]
		[Deprecated (PlatformName.MacCatalyst, 18, 0, message: "Use 'Network.NWConnection.SetStateChangedHandler' instead.")]
		[Deprecated (PlatformName.MacOSX, 15, 0, message: "Use 'Network.NWConnection.SetStateChangedHandler' instead.")]
		[Deprecated (PlatformName.TvOS, 18, 0, message: "Use 'Network.NWConnection.SetStateChangedHandler' instead.")]
		[Export ("state")]
		NWTcpConnectionState State { get; }

		/// <summary>Gets the TXT record of the connection.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Deprecated (PlatformName.iOS, 18, 0, message: "Use 'Network.NWConnection.SetViabilityChangedHandler' instead.")]
		[Deprecated (PlatformName.MacCatalyst, 18, 0, message: "Use 'Network.NWConnection.SetViabilityChangedHandler' instead.")]
		[Deprecated (PlatformName.MacOSX, 15, 0, message: "Use 'Network.NWConnection.SetViabilityChangedHandler' instead.")]
		[Deprecated (PlatformName.TvOS, 18, 0, message: "Use 'Network.NWConnection.SetViabilityChangedHandler' instead.")]
		[Export ("viable")]
		bool Viable { [Bind ("isViable")] get; }

		/// <summary>Gets a Boolean value that tells whether new connections to the remote endpoint have a better path available to them..</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Deprecated (PlatformName.iOS, 18, 0, message: "Use 'Network.NWConnection.SetBetterPathAvailableHandler' instead.")]
		[Deprecated (PlatformName.MacCatalyst, 18, 0, message: "Use 'Network.NWConnection.SetBetterPathAvailableHandler' instead.")]
		[Deprecated (PlatformName.MacOSX, 15, 0, message: "Use 'Network.NWConnection.SetBetterPathAvailableHandler' instead.")]
		[Deprecated (PlatformName.TvOS, 18, 0, message: "Use 'Network.NWConnection.SetBetterPathAvailableHandler' instead.")]
		[Export ("hasBetterPath")]
		bool HasBetterPath { get; }

		/// <summary>Gets the remote endpoint of the connection.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Deprecated (PlatformName.iOS, 18, 0, message: "Use 'Network.NWConnection.Endpoint' instead.")]
		[Deprecated (PlatformName.MacCatalyst, 18, 0, message: "Use 'Network.NWConnection.Endpoint' instead.")]
		[Deprecated (PlatformName.MacOSX, 15, 0, message: "Use 'Network.NWConnection.Endpoint' instead.")]
		[Deprecated (PlatformName.TvOS, 18, 0, message: "Use 'Network.NWConnection.Endpoint' instead.")]
		[Export ("endpoint")]
		NWEndpoint Endpoint { get; }

		/// <summary>Gets the path through which the connection was made.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Deprecated (PlatformName.iOS, 18, 0, message: "Use 'Network.NWConnection.CurrentPath' instead.")]
		[Deprecated (PlatformName.MacCatalyst, 18, 0, message: "Use 'Network.NWConnection.CurrentPath' instead.")]
		[Deprecated (PlatformName.MacOSX, 15, 0, message: "Use 'Network.NWConnection.CurrentPath' instead.")]
		[Deprecated (PlatformName.TvOS, 18, 0, message: "Use 'Network.NWConnection.CurrentPath' instead.")]
		[NullAllowed, Export ("connectedPath")]
		NWPath ConnectedPath { get; }

		/// <summary>Gets the address from which the connectio was made.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Deprecated (PlatformName.iOS, 18, 0, message: "Use 'Network.NWPath.EffectiveLocalEndpoint' instead.")]
		[Deprecated (PlatformName.MacCatalyst, 18, 0, message: "Use 'Network.NWPath.EffectiveLocalEndpoint' instead.")]
		[Deprecated (PlatformName.MacOSX, 15, 0, message: "Use 'Network.NWPath.EffectiveLocalEndpoint' instead.")]
		[Deprecated (PlatformName.TvOS, 18, 0, message: "Use 'Network.NWPath.EffectiveLocalEndpoint' instead.")]
		[NullAllowed, Export ("localAddress")]
		NWEndpoint LocalAddress { get; }

		/// <summary>Gets the remote endpoint to which the connection was made.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Deprecated (PlatformName.iOS, 18, 0, message: "Use 'Network.NWPath.EffectiveRemoteEndpoint' instead.")]
		[Deprecated (PlatformName.MacCatalyst, 18, 0, message: "Use 'Network.NWPath.EffectiveRemoteEndpoint' instead.")]
		[Deprecated (PlatformName.MacOSX, 15, 0, message: "Use 'Network.NWPath.EffectiveRemoteEndpoint' instead.")]
		[Deprecated (PlatformName.TvOS, 18, 0, message: "Use 'Network.NWPath.EffectiveRemoteEndpoint' instead.")]
		[NullAllowed, Export ("remoteAddress")]
		NWEndpoint RemoteAddress { get; }

		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Deprecated (PlatformName.iOS, 18, 0, message: "Use 'Network.NWEndpoint.TxtRecord' instead.")]
		[Deprecated (PlatformName.MacCatalyst, 18, 0, message: "Use 'Network.NWEndpoint.TxtRecord' instead.")]
		[Deprecated (PlatformName.MacOSX, 15, 0, message: "Use 'Network.NWEndpoint.TxtRecord' instead.")]
		[Deprecated (PlatformName.TvOS, 18, 0, message: "Use 'Network.NWEndpoint.TxtRecord' instead.")]
		[NullAllowed, Export ("txtRecord")]
		NSData TxtRecord { get; }

		/// <summary>Gets the error that caused the connection to fail, if it failed.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Deprecated (PlatformName.iOS, 18, 0, message: "Use 'Network.NWConnection.SetStateChangedHandler' instead.")]
		[Deprecated (PlatformName.MacCatalyst, 18, 0, message: "Use 'Network.NWConnection.SetStateChangedHandler' instead.")]
		[Deprecated (PlatformName.MacOSX, 15, 0, message: "Use 'Network.NWConnection.SetStateChangedHandler' instead.")]
		[Deprecated (PlatformName.TvOS, 18, 0, message: "Use 'Network.NWConnection.SetStateChangedHandler' instead.")]
		[NullAllowed, Export ("error")]
		NSError Error { get; }

		/// <summary>Cancels the connection.</summary>
		///         <remarks>To be added.</remarks>
		[Deprecated (PlatformName.iOS, 18, 0, message: "Use 'Network.NWConnection.Cancel' instead.")]
		[Deprecated (PlatformName.MacCatalyst, 18, 0, message: "Use 'Network.NWConnection.Cancel' instead.")]
		[Deprecated (PlatformName.MacOSX, 15, 0, message: "Use 'Network.NWConnection.Cancel' instead.")]
		[Deprecated (PlatformName.TvOS, 18, 0, message: "Use 'Network.NWConnection.Cancel' instead.")]
		[Export ("cancel")]
		void Cancel ();

		[Deprecated (PlatformName.iOS, 18, 0, message: "Use 'Network.NWConnection.Receive' instead.")]
		[Deprecated (PlatformName.MacCatalyst, 18, 0, message: "Use 'Network.NWConnection.Receive' instead.")]
		[Deprecated (PlatformName.MacOSX, 15, 0, message: "Use 'Network.NWConnection.Receive' instead.")]
		[Deprecated (PlatformName.TvOS, 18, 0, message: "Use 'Network.NWConnection.Receive' instead.")]
		[Export ("readLength:completionHandler:")]
		[Async]
		void ReadLength (nuint length, Action<NSData, NSError> completion);

		[Deprecated (PlatformName.iOS, 18, 0, message: "Use 'Network.NWConnection.Receive' instead.")]
		[Deprecated (PlatformName.MacCatalyst, 18, 0, message: "Use 'Network.NWConnection.Receive' instead.")]
		[Deprecated (PlatformName.MacOSX, 15, 0, message: "Use 'Network.NWConnection.Receive' instead.")]
		[Deprecated (PlatformName.TvOS, 18, 0, message: "Use 'Network.NWConnection.Receive' instead.")]
		[Export ("readMinimumLength:maximumLength:completionHandler:")]
		[Async]
		void ReadMinimumLength (nuint minimum, nuint maximum, Action<NSData, NSError> completion);

		/// <param name="data">To be added.</param>
		///         <param name="completion">To be added.</param>
		///         <summary>Writes the provided <paramref name="data" /> to the connection and runs a completion handler when the operation completes.</summary>
		///         <remarks>To be added.</remarks>
		[Deprecated (PlatformName.iOS, 18, 0, message: "Use 'Network.NWConnection.Send' instead.")]
		[Deprecated (PlatformName.MacCatalyst, 18, 0, message: "Use 'Network.NWConnection.Send' instead.")]
		[Deprecated (PlatformName.MacOSX, 15, 0, message: "Use 'Network.NWConnection.Send' instead.")]
		[Deprecated (PlatformName.TvOS, 18, 0, message: "Use 'Network.NWConnection.Send' instead.")]
		[Export ("write:completionHandler:")]
		[Async]
		void Write (NSData data, Action<NSError> completion);

		/// <summary>Closes the connection for write operations.</summary>
		///         <remarks>To be added.</remarks>
		[Deprecated (PlatformName.iOS, 18, 0, message: "Use 'Network.NWConnection.Send' instead.")]
		[Deprecated (PlatformName.MacCatalyst, 18, 0, message: "Use 'Network.NWConnection.Send' instead.")]
		[Deprecated (PlatformName.MacOSX, 15, 0, message: "Use 'Network.NWConnection.Send' instead.")]
		[Deprecated (PlatformName.TvOS, 18, 0, message: "Use 'Network.NWConnection.Send' instead.")]
		[Export ("writeClose")]
		void WriteClose ();
	}

	interface INWTcpConnectionAuthenticationDelegate { }

	[Deprecated (PlatformName.iOS, 18, 0, message: "Use 'Security.SecProtocolOptions' instead.")]
	[Deprecated (PlatformName.MacCatalyst, 18, 0, message: "Use 'Security.SecProtocolOptions' instead.")]
	[Deprecated (PlatformName.MacOSX, 15, 0, message: "Use 'Security.SecProtocolOptions' instead.")]
	[Deprecated (PlatformName.TvOS, 18, 0, message: "Use 'Security.SecProtocolOptions' instead.")]
	[MacCatalyst (13, 1)]
	[Protocol, Model]
	[BaseType (typeof (NSObject), Name = "NWTCPConnectionAuthenticationDelegate")]
	interface NWTcpConnectionAuthenticationDelegate {
		/// <param name="connection">To be added.</param>
		///         <summary>Method that is called to inform the delegate that it should provide identity information.</summary>
		///         <returns>
		///           <see langword="true" /> to indicate that the delegate will provide identity information.</returns>
		///         <remarks>To be added.</remarks>
		[Deprecated (PlatformName.iOS, 18, 0, message: "Use 'Security.SecProtocolOptions.SetChallengeBlock' instead.")]
		[Deprecated (PlatformName.MacCatalyst, 18, 0, message: "Use 'Security.SecProtocolOptions.SetChallengeBlock' instead.")]
		[Deprecated (PlatformName.MacOSX, 15, 0, message: "Use 'Security.SecProtocolOptions.SetChallengeBlock' instead.")]
		[Deprecated (PlatformName.TvOS, 18, 0, message: "Use 'Security.SecProtocolOptions.SetChallengeBlock' instead.")]
		[Export ("shouldProvideIdentityForConnection:")]
		bool ShouldProvideIdentity (NWTcpConnection connection);

		/// <param name="connection">To be added.</param>
		///         <param name="completion">To be added.</param>
		///         <summary>Method that is called to provide an identity and an optional certificate.</summary>
		///         <remarks>To be added.</remarks>
		[Deprecated (PlatformName.iOS, 18, 0, message: "Use 'Security.SecProtocolOptions.SetChallengeBlock' instead.")]
		[Deprecated (PlatformName.MacCatalyst, 18, 0, message: "Use 'Security.SecProtocolOptions.SetChallengeBlock' instead.")]
		[Deprecated (PlatformName.MacOSX, 15, 0, message: "Use 'Security.SecProtocolOptions.SetChallengeBlock' instead.")]
		[Deprecated (PlatformName.TvOS, 18, 0, message: "Use 'Security.SecProtocolOptions.SetChallengeBlock' instead.")]
		[Export ("provideIdentityForConnection:completionHandler:")]
		void ProvideIdentity (NWTcpConnection connection, Action<SecIdentity, NSArray> completion);

		/// <param name="connection">To be added.</param>
		///         <summary>Method that is called to inform the delegate that it should evaluate trust.</summary>
		///         <returns>
		///           <see langword="true" /> to indicate that the delegate will evaluate trust.</returns>
		///         <remarks>To be added.</remarks>
		[Deprecated (PlatformName.iOS, 18, 0, message: "Use 'Security.SecProtocolOptions.SetVerifyBlock' instead.")]
		[Deprecated (PlatformName.MacCatalyst, 18, 0, message: "Use 'Security.SecProtocolOptions.SetVerifyBlock' instead.")]
		[Deprecated (PlatformName.MacOSX, 15, 0, message: "Use 'Security.SecProtocolOptions.SetVerifyBlock' instead.")]
		[Deprecated (PlatformName.TvOS, 18, 0, message: "Use 'Security.SecProtocolOptions.SetVerifyBlock' instead.")]
		[Export ("shouldEvaluateTrustForConnection:")]
		bool ShouldEvaluateTrust (NWTcpConnection connection);


		/// <param name="connection">To be added.</param>
		///         <param name="peerCertificateChain">To be added.</param>
		///         <param name="completion">To be added.</param>
		///         <summary>When implemented by the developer, overrides the default trust evaluation and runs a completion handler when the operation is complete.</summary>
		///         <remarks>To be added.</remarks>
		[Deprecated (PlatformName.iOS, 18, 0, message: "Use 'Security.SecProtocolOptions.SetVerifyBlock' instead.")]
		[Deprecated (PlatformName.MacCatalyst, 18, 0, message: "Use 'Security.SecProtocolOptions.SetVerifyBlock' instead.")]
		[Deprecated (PlatformName.MacOSX, 15, 0, message: "Use 'Security.SecProtocolOptions.SetVerifyBlock' instead.")]
		[Deprecated (PlatformName.TvOS, 18, 0, message: "Use 'Security.SecProtocolOptions.SetVerifyBlock' instead.")]
		[Export ("evaluateTrustForConnection:peerCertificateChain:completionHandler:")]
		[Async]
		void EvaluateTrust (NWTcpConnection connection, NSArray peerCertificateChain, Action<SecTrust> completion);
		// note: it's not clear (from headers) but based on other API it's likely to accept a mix of SecIdentity
		// and SecCertificate - both *NOT* NSObject -> because of that NSArray is used above
	}

	[Deprecated (PlatformName.iOS, 18, 0, message: "Use 'Security.SecProtocolOptions' instead.")]
	[Deprecated (PlatformName.MacCatalyst, 18, 0, message: "Use 'Security.SecProtocolOptions' instead.")]
	[Deprecated (PlatformName.MacOSX, 15, 0, message: "Use 'Security.SecProtocolOptions' instead.")]
	[Deprecated (PlatformName.TvOS, 18, 0, message: "Use 'Security.SecProtocolOptions' instead.")]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject), Name = "NWTLSParameters")]
	interface NWTlsParameters {
		/// <summary>Gets the session identifier of the TCP connection.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Deprecated (PlatformName.iOS, 18, 0, message: "Use 'Security.SecProtocolOptions.SetTlsResumptionEnabled' instead.")]
		[Deprecated (PlatformName.MacCatalyst, 18, 0, message: "Use 'Security.SecProtocolOptions.SetTlsResumptionEnabled' instead.")]
		[Deprecated (PlatformName.MacOSX, 15, 0, message: "Use 'Security.SecProtocolOptions.SetTlsResumptionEnabled' instead.")]
		[Deprecated (PlatformName.TvOS, 18, 0, message: "Use 'Security.SecProtocolOptions.SetTlsResumptionEnabled' instead.")]
		[NullAllowed, Export ("TLSSessionID", ArgumentSemantic.Copy)]
		NSData TlsSessionID { get; set; }

		/// <summary>Gets or sets an array of cipher suites to use for TLS.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Deprecated (PlatformName.iOS, 18, 0, message: "Use 'Security.SecProtocolOptions.AddTlsCipherSuite' instead.")]
		[Deprecated (PlatformName.MacCatalyst, 18, 0, message: "Use 'Security.SecProtocolOptions.AddTlsCipherSuite' instead.")]
		[Deprecated (PlatformName.MacOSX, 15, 0, message: "Use 'Security.SecProtocolOptions.AddTlsCipherSuite' instead.")]
		[Deprecated (PlatformName.TvOS, 18, 0, message: "Use 'Security.SecProtocolOptions.AddTlsCipherSuite' instead.")]
		[NullAllowed, Export ("SSLCipherSuites", ArgumentSemantic.Copy)]
		NSSet<NSNumber> SslCipherSuites { get; set; }

		/// <summary>Gets or sets the minimum allowed SSL protocol version.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Deprecated (PlatformName.iOS, 18, 0, message: "Use 'Security.SecProtocolOptions.SetTlsMinVersion' instead.")]
		[Deprecated (PlatformName.MacCatalyst, 18, 0, message: "Use 'Security.SecProtocolOptions.SetTlsMinVersion' instead.")]
		[Deprecated (PlatformName.MacOSX, 15, 0, message: "Use 'Security.SecProtocolOptions.SetTlsMinVersion' instead.")]
		[Deprecated (PlatformName.TvOS, 18, 0, message: "Use 'Security.SecProtocolOptions.SetTlsMinVersion' instead.")]
		[Export ("minimumSSLProtocolVersion", ArgumentSemantic.Assign)]
		nuint MinimumSslProtocolVersion { get; set; }

		/// <summary>Gets or sets the maximum allowed SSL protocol version.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Deprecated (PlatformName.iOS, 18, 0, message: "Use 'Security.SecProtocolOptions.SetTlsMaxVersion' instead.")]
		[Deprecated (PlatformName.MacCatalyst, 18, 0, message: "Use 'Security.SecProtocolOptions.SetTlsMaxVersion' instead.")]
		[Deprecated (PlatformName.MacOSX, 15, 0, message: "Use 'Security.SecProtocolOptions.SetTlsMaxVersion' instead.")]
		[Deprecated (PlatformName.TvOS, 18, 0, message: "Use 'Security.SecProtocolOptions.SetTlsMaxVersion' instead.")]
		[Export ("maximumSSLProtocolVersion", ArgumentSemantic.Assign)]
		nuint MaximumSslProtocolVersion { get; set; }
	}

	[Deprecated (PlatformName.iOS, 18, 0, message: "Use 'Network.NWConnection' instead.")]
	[Deprecated (PlatformName.MacCatalyst, 18, 0, message: "Use 'Network.NWConnection' instead.")]
	[Deprecated (PlatformName.MacOSX, 15, 0, message: "Use 'Network.NWConnection' instead.")]
	[Deprecated (PlatformName.TvOS, 18, 0, message: "Use 'Network.NWConnection' instead.")]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject), Name = "NWUDPSession")]
	interface NWUdpSession {
		[Deprecated (PlatformName.iOS, 18, 0, message: "Use the 'Network.NWConnection' constructor instead.")]
		[Deprecated (PlatformName.MacCatalyst, 18, 0, message: "Use the 'Network.NWConnection' constructor instead.")]
		[Deprecated (PlatformName.MacOSX, 15, 0, message: "Use the 'Network.NWConnection' constructor instead.")]
		[Deprecated (PlatformName.TvOS, 18, 0, message: "Use the 'Network.NWConnection' constructor instead.")]
		[Export ("initWithUpgradeForSession:")]
		NativeHandle Constructor (NWUdpSession session);

		/// <summary>Gets the UDP session state.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Deprecated (PlatformName.iOS, 18, 0, message: "Use 'Network.NWConnection.SetStateChangedHandler' instead.")]
		[Deprecated (PlatformName.MacCatalyst, 18, 0, message: "Use 'Network.NWConnection.SetStateChangedHandler' instead.")]
		[Deprecated (PlatformName.MacOSX, 15, 0, message: "Use 'Network.NWConnection.SetStateChangedHandler' instead.")]
		[Deprecated (PlatformName.TvOS, 18, 0, message: "Use 'Network.NWConnection.SetStateChangedHandler' instead.")]
		[Export ("state")]
		NWUdpSessionState State { get; }

		/// <summary>Gets the endpoint that was used to create the UDP session.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Deprecated (PlatformName.iOS, 18, 0, message: "Use 'Network.NWConnection.Endpoint' instead.")]
		[Deprecated (PlatformName.MacCatalyst, 18, 0, message: "Use 'Network.NWConnection.Endpoint' instead.")]
		[Deprecated (PlatformName.MacOSX, 15, 0, message: "Use 'Network.NWConnection.Endpoint' instead.")]
		[Deprecated (PlatformName.TvOS, 18, 0, message: "Use 'Network.NWConnection.Endpoint' instead.")]
		[Export ("endpoint")]
		NWEndpoint Endpoint { get; }

		/// <summary>Gets the current remote endpoint.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Deprecated (PlatformName.iOS, 18, 0, message: "Use 'Network.NWConnection.CurrentPath' instead.")]
		[Deprecated (PlatformName.MacCatalyst, 18, 0, message: "Use 'Network.NWConnection.CurrentPath' instead.")]
		[Deprecated (PlatformName.MacOSX, 15, 0, message: "Use 'Network.NWConnection.CurrentPath' instead.")]
		[Deprecated (PlatformName.TvOS, 18, 0, message: "Use 'Network.NWConnection.CurrentPath' instead.")]
		[NullAllowed, Export ("resolvedEndpoint")]
		NWEndpoint ResolvedEndpoint { get; }

		/// <summary>Gets a Boolean value that tells whether the session can transfer data.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Deprecated (PlatformName.iOS, 18, 0, message: "Use 'Network.NWConnection.SetViabilityChangedHandler' instead.")]
		[Deprecated (PlatformName.MacCatalyst, 18, 0, message: "Use 'Network.NWConnection.SetViabilityChangedHandler' instead.")]
		[Deprecated (PlatformName.MacOSX, 15, 0, message: "Use 'Network.NWConnection.SetViabilityChangedHandler' instead.")]
		[Deprecated (PlatformName.TvOS, 18, 0, message: "Use 'Network.NWConnection.SetViabilityChangedHandler' instead.")]
		[Export ("viable")]
		bool Viable { [Bind ("isViable")] get; }

		/// <summary>Get a Boolean value that tells whether new UDP sessions have a better path available to them.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Deprecated (PlatformName.iOS, 18, 0, message: "Use 'Network.NWConnection.SetBetterPathAvailableHandler' instead.")]
		[Deprecated (PlatformName.MacCatalyst, 18, 0, message: "Use 'Network.NWConnection.SetBetterPathAvailableHandler' instead.")]
		[Deprecated (PlatformName.MacOSX, 15, 0, message: "Use 'Network.NWConnection.SetBetterPathAvailableHandler' instead.")]
		[Deprecated (PlatformName.TvOS, 18, 0, message: "Use 'Network.NWConnection.SetBetterPathAvailableHandler' instead.")]
		[Export ("hasBetterPath")]
		bool HasBetterPath { get; }

		/// <summary>Gets the current path to the resolved endpoint.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Deprecated (PlatformName.iOS, 18, 0, message: "Use 'Network.NWConnection.CurrentPath' instead.")]
		[Deprecated (PlatformName.MacCatalyst, 18, 0, message: "Use 'Network.NWConnection.CurrentPath' instead.")]
		[Deprecated (PlatformName.MacOSX, 15, 0, message: "Use 'Network.NWConnection.CurrentPath' instead.")]
		[Deprecated (PlatformName.TvOS, 18, 0, message: "Use 'Network.NWConnection.CurrentPath' instead.")]
		[NullAllowed, Export ("currentPath")]
		NWPath CurrentPath { get; }

		/// <summary>Mark the current endpoint unusable and try to connect to the next one.</summary>
		///         <remarks>To be added.</remarks>
		[Deprecated (PlatformName.iOS, 18, 0, message: "Use 'Network.NWConnection.CancelCurrentEndpoint' instead.")]
		[Deprecated (PlatformName.MacCatalyst, 18, 0, message: "Use 'Network.NWConnection.CurrentPath' instead.")]
		[Deprecated (PlatformName.MacOSX, 15, 0, message: "Use 'Network.NWConnection.CancelCurrentEndpoint' instead.")]
		[Deprecated (PlatformName.TvOS, 18, 0, message: "Use 'Network.NWConnection.CancelCurrentEndpoint' instead.")]
		[Export ("tryNextResolvedEndpoint")]
		void TryNextResolvedEndpoint ();

		/// <summary>Gets the maximum write length, in bytes, of datagrams.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Deprecated (PlatformName.iOS, 18, 0, message: "Use 'Network.NWConnection.MaximumDatagramSize' instead.")]
		[Deprecated (PlatformName.MacCatalyst, 18, 0, message: "Use 'Network.NWConnection.MaximumDatagramSize' instead.")]
		[Deprecated (PlatformName.MacOSX, 15, 0, message: "Use 'Network.NWConnection.MaximumDatagramSize' instead.")]
		[Deprecated (PlatformName.TvOS, 18, 0, message: "Use 'Network.NWConnection.MaximumDatagramSize' instead.")]
		[Export ("maximumDatagramLength")]
		nuint MaximumDatagramLength { get; }

		[Deprecated (PlatformName.iOS, 18, 0, message: "Use 'Network.NWConnection.Receive' instead.")]
		[Deprecated (PlatformName.MacCatalyst, 18, 0, message: "Use 'Network.NWConnection.Receive' instead.")]
		[Deprecated (PlatformName.MacOSX, 15, 0, message: "Use 'Network.NWConnection.Receive' instead.")]
		[Deprecated (PlatformName.TvOS, 18, 0, message: "Use 'Network.NWConnection.Receive' instead.")]
		[Export ("setReadHandler:maxDatagrams:")]
		void SetReadHandler (Action<NSArray, NSError> handler, nuint maxDatagrams);

		/// <param name="datagramArray">To be added.</param>
		///         <param name="completionHandler">To be added.</param>
		///         <summary>Writes the datagrams in the provided <paramref name="datagramArray" /> to the endpoint, and runs a completion handler when the operation completes.</summary>
		///         <remarks>To be added.</remarks>
		[Deprecated (PlatformName.iOS, 18, 0, message: "Use 'Network.NWConnection.Send' instead.")]
		[Deprecated (PlatformName.MacCatalyst, 18, 0, message: "Use 'Network.NWConnection.Send' instead.")]
		[Deprecated (PlatformName.MacOSX, 15, 0, message: "Use 'Network.NWConnection.Send' instead.")]
		[Deprecated (PlatformName.TvOS, 18, 0, message: "Use 'Network.NWConnection.Send' instead.")]
		[Export ("writeMultipleDatagrams:completionHandler:")]
		[Async]
		void WriteMultipleDatagrams (NSData [] datagramArray, Action<NSError> completionHandler);

		/// <param name="datagram">To be added.</param>
		///         <param name="completionHandler">To be added.</param>
		///         <summary>Writes the provided <paramref name="datagram" /> to the endpoint, and runs a completion handler when the operation completes.</summary>
		///         <remarks>To be added.</remarks>
		[Deprecated (PlatformName.iOS, 18, 0, message: "Use 'Network.NWConnection.Send' instead.")]
		[Deprecated (PlatformName.MacCatalyst, 18, 0, message: "Use 'Network.NWConnection.Send' instead.")]
		[Deprecated (PlatformName.MacOSX, 15, 0, message: "Use 'Network.NWConnection.Send' instead.")]
		[Deprecated (PlatformName.TvOS, 18, 0, message: "Use 'Network.NWConnection.Send' instead.")]
		[Export ("writeDatagram:completionHandler:")]
		[Async]
		void WriteDatagram (NSData datagram, Action<NSError> completionHandler);

		/// <summary>Cancels the UDP session.</summary>
		///         <remarks>To be added.</remarks>
		[Deprecated (PlatformName.iOS, 18, 0, message: "Use 'Network.NWConnection.Cancel' instead.")]
		[Deprecated (PlatformName.MacCatalyst, 18, 0, message: "Use 'Network.NWConnection.Cancel' instead.")]
		[Deprecated (PlatformName.MacOSX, 15, 0, message: "Use 'Network.NWConnection.Cancel' instead.")]
		[Deprecated (PlatformName.TvOS, 18, 0, message: "Use 'Network.NWConnection.Cancel' instead.")]
		[Export ("cancel")]
		void Cancel ();
	}

	/// <summary>Contains information about a WebKit browser network data flow.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/NetworkExtension/Reference/NEFilterBrowserFlowClassRef/index.html">Apple documentation for <c>NEFilterBrowserFlow</c></related>
	[NoTV]
	[NoMac]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NEFilterFlow))]
	interface NEFilterBrowserFlow {

		/// <summary>The HTTP request.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed]
		[Export ("request")]
		NSUrlRequest Request { get; }

		/// <summary>The HTTP response.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Export ("response")]
		[NullAllowed]
		NSUrlResponse Response { get; }

		/// <summary>Gets the URL of the page for which the flow was created.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Export ("parentURL")]
		[NullAllowed]
		NSUrl ParentUrl { get; }
	}

	/// <summary>Contains information about network data flow over a socket.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/NetworkExtension/Reference/NEFilterSocketFlowClassRef/index.html">Apple documentation for <c>NEFilterSocketFlow</c></related>
	[NoTV]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NEFilterFlow))]
	interface NEFilterSocketFlow {
		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>Gets the remote endpoint of the socket.</remarks>
		[NullAllowed]
		[Deprecated (PlatformName.iOS, 18, 0, message: "Use 'RemoteFlowEndpoint' instead.")]
		[Deprecated (PlatformName.MacCatalyst, 18, 0, message: "Use 'RemoteFlowEndpoint' instead.")]
		[Deprecated (PlatformName.MacOSX, 15, 0, message: "Use 'RemoteFlowEndpoint' instead.")]
		[Export ("remoteEndpoint")]
		NWEndpoint RemoteEndpoint { get; }

		/// <summary>Gets the local endpoint of the socket.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed]
		[Deprecated (PlatformName.iOS, 18, 0, message: "Use 'LocalFlowEndpoint' instead.")]
		[Deprecated (PlatformName.MacCatalyst, 18, 0, message: "Use 'LocalFlowEndpoint' instead.")]
		[Deprecated (PlatformName.MacOSX, 15, 0, message: "Use 'LocalFlowEndpoint' instead.")]
		[Export ("localEndpoint")]
		NWEndpoint LocalEndpoint { get; }

		/// <summary>Gets or sets the socket protocol family.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("socketFamily")]
		int SocketFamily {
			get;
#if !NET
			[NotImplemented]
			set;
#endif
		}

		/// <summary>Gets or sets the socket type.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("socketType")]
		int SocketType {
			get;
#if !NET
			[NotImplemented]
			set;
#endif
		}

		/// <summary>Gets or sets the socket protocol.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("socketProtocol")]
		int SocketProtocol {
			get;
#if !NET
			[NotImplemented]
			set;
#endif
		}

		[NullAllowed]
		[NoTV, iOS (14, 0)]
		[MacCatalyst (14, 0)]
		[Export ("remoteHostname")]
		string RemoteHostname { get; }

		[NoTV, Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
		[Export ("localFlowEndpoint"), NullAllowed]
		Network.NWEndpoint LocalFlowEndpoint { get; }

		[NoTV, Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
		[Export ("remoteFlowEndpoint"), NullAllowed]
		Network.NWEndpoint RemoteFlowEndpoint { get; }
	}

	/// <summary>A reporting action taken on a filtered data flow.</summary>
	[NoTV]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	interface NEFilterReport : NSSecureCoding, NSCopying {

		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("flow")]
		NEFilterFlow Flow { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("action")]
		NEFilterAction Action { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Export ("event")]
		NEFilterReportEvent Event { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Export ("bytesInboundCount")]
		nuint BytesInboundCount { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Export ("bytesOutboundCount")]
		nuint BytesOutboundCount { get; }
	}

	[MacCatalyst (13, 1)]
	[BaseType (typeof (NETunnelNetworkSettings))]
	[DisableDefaultCtor]
	interface NEPacketTunnelNetworkSettings {
		[Export ("initWithTunnelRemoteAddress:")]
		NativeHandle Constructor (string address);

		/// <summary>Gets or sets the IPv4 network tunnel settings.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Export ("IPv4Settings", ArgumentSemantic.Copy)]
		[NullAllowed]
		NEIPv4Settings IPv4Settings { get; set; }

		/// <summary>Gets or sets the IPv6 network tunnel settings.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Export ("IPv6Settings", ArgumentSemantic.Copy)]
		[NullAllowed]
		NEIPv6Settings IPv6Settings { get; set; }

		/// <summary>Gets or sets the overhead for storing tunneling protocol headers.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Export ("tunnelOverheadBytes", ArgumentSemantic.Copy)]
		[NullAllowed]
		NSNumber TunnelOverheadBytes { get; set; }

		/// <summary>Gets or sets the Maximum Transmission Unit (MTU) size for the TUN interface, in bytes.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Export ("MTU", ArgumentSemantic.Copy)]
		[NullAllowed]
		NSNumber Mtu { get; set; }
	}

	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	interface NEPacketTunnelFlow {
		/// <param name="completionHandler">To be added.</param>
		///         <summary>Reads packets from the TUN interface and runs a handler when the operation completes.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("readPacketsWithCompletionHandler:")]
		[Async (ResultType = typeof (NEPacketTunnelFlowReadResult))]
		void ReadPackets (Action<NSData [], NSNumber []> completionHandler);

		/// <param name="packets">To be added.</param>
		///         <param name="protocols">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("writePackets:withProtocols:")]
		bool WritePackets (NSData [] packets, NSNumber [] protocols);

		/// <param name="completionHandler">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Async]
		[Export ("readPacketObjectsWithCompletionHandler:")]
		void ReadPacketObjects (Action<NEPacket []> completionHandler);

		/// <param name="packets">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("writePacketObjects:")]
		bool WritePacketObjects (NEPacket [] packets);
	}

	[MacCatalyst (13, 1)]
	[BaseType (typeof (NETunnelProvider))]
	interface NEPacketTunnelProvider {
		/// <param name="options">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <param name="completionHandler">To be added.</param>
		///         <summary>Starts the tunnel.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("startTunnelWithOptions:completionHandler:")]
		[Async]
		void StartTunnel ([NullAllowed] NSDictionary<NSString, NSObject> options, Action<NSError> completionHandler);

		/// <param name="reason">To be added.</param>
		///         <param name="completionHandler">To be added.</param>
		///         <summary>Stops the Tunnel.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("stopTunnelWithReason:completionHandler:")]
		[Async]
		void StopTunnel (NEProviderStopReason reason, Action completionHandler);

		/// <param name="error">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <summary>Cancels the tunnel with the specified error.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("cancelTunnelWithError:")]
		void CancelTunnel ([NullAllowed] NSError error);

		/// <summary>Gets the packet flow that receives IP packets and injects IP packets into the network.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("packetFlow")]
		NEPacketTunnelFlow PacketFlow { get; }

		/// <param name="remoteEndpoint">The remote endpoint for the connection.</param>
		///         <param name="enableTls">Whether TLS is enabled.</param>
		///         <param name="tlsParameters">
		///           <para>TLS parameters, if TLS is enabled.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <param name="delegate">
		///           <para>Handler to run when the connection is created.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <summary>Creates a new tunneled TCP connection.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Deprecated (PlatformName.iOS, 18, 0, message: "Use 'NWParameters.RequiredInterface' with the 'VirtualInterface' property instead.")]
		[Deprecated (PlatformName.MacCatalyst, 18, 0, message: "Use 'NWParameters.RequiredInterface' with the 'VirtualInterface' property instead.")]
		[Deprecated (PlatformName.MacOSX, 15, 0, message: "Use 'NWParameters.RequiredInterface' with the 'VirtualInterface' property instead.")]
		[Deprecated (PlatformName.TvOS, 18, 0, message: "Use 'NWParameters.RequiredInterface' with the 'VirtualInterface' property instead.")]
		[Export ("createTCPConnectionThroughTunnelToEndpoint:enableTLS:TLSParameters:delegate:")]
		NWTcpConnection CreateTcpConnection (NWEndpoint remoteEndpoint, bool enableTls, [NullAllowed] NWTlsParameters tlsParameters, [NullAllowed] INWTcpConnectionAuthenticationDelegate @delegate);

		/// <param name="remoteEndpoint">To be added.</param>
		///         <param name="localEndpoint">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <summary>Creates a new tunneled UDP connection.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Deprecated (PlatformName.iOS, 18, 0, message: "Use 'NWParameters.RequiredInterface' with the 'VirtualInterface' property instead.")]
		[Deprecated (PlatformName.MacCatalyst, 18, 0, message: "Use 'NWParameters.RequiredInterface' with the 'VirtualInterface' property instead.")]
		[Deprecated (PlatformName.MacOSX, 15, 0, message: "Use 'NWParameters.RequiredInterface' with the 'VirtualInterface' property instead.")]
		[Deprecated (PlatformName.TvOS, 18, 0, message: "Use 'NWParameters.RequiredInterface' with the 'VirtualInterface' property instead.")]
		[Export ("createUDPSessionThroughTunnelToEndpoint:fromEndpoint:")]
		NWUdpSession CreateUdpSession (NWEndpoint remoteEndpoint, [NullAllowed] NWHostEndpoint localEndpoint);

		[TV (18, 0), Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
		[Export ("virtualInterface"), NullAllowed]
		NWInterface VirtualInterface { get; }
	}

	[MacCatalyst (13, 1)]
	[BaseType (typeof (NEVpnProtocol))]
	interface NETunnelProviderProtocol {
		/// <summary>Gets or sets a dictionary of developer-specified configuration values.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("providerConfiguration", ArgumentSemantic.Copy)]
		NSDictionary<NSString, NSObject> ProviderConfiguration { get; set; }

		/// <summary>Gets or sets the bundle identifier for this configuration's tunnel provider extension.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("providerBundleIdentifier")]
		string ProviderBundleIdentifier { get; set; }
	}

	[MacCatalyst (13, 1)]
	[BaseType (typeof (NEVpnConnection))]
	interface NETunnelProviderSession {
		/// <param name="options">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <param name="error">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <summary>Begins connecting the tunnel.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("startTunnelWithOptions:andReturnError:")]
		bool StartTunnel ([NullAllowed] NSDictionary<NSString, NSObject> options, [NullAllowed] out NSError error);

		/// <summary>Begins disconnecting the tunnel.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("stopTunnel")]
		void StopTunnel ();

		/// <param name="messageData">To be added.</param>
		///         <param name="error">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <param name="responseHandler">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <summary>Sends the <paramref name="messageData" /> to the Tunnel Provider extension.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("sendProviderMessage:returnError:responseHandler:")]
		bool SendProviderMessage (NSData messageData, [NullAllowed] out NSError error, [NullAllowed] Action<NSData> responseHandler);
	}

	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	interface NEPacket : NSCopying, NSSecureCoding {
		[Export ("initWithData:protocolFamily:")]
		NativeHandle Constructor (NSData data, /* sa_family_t */ byte protocolFamily);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("data", ArgumentSemantic.Copy)]
		NSData Data { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("protocolFamily")]
		byte ProtocolFamily { get; }

		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NoTV]
		[NullAllowed, Export ("metadata")]
		NEFlowMetaData Metadata { get; }

		[NoTV]
		[NoiOS]
		[MacCatalyst (15, 0)]
		[Export ("direction")]
		NETrafficDirection Direction { get; }
	}

	/// <summary>Encapsulates a DNS proxy.</summary>
	[NoTV]
	[MacCatalyst (13, 1)]
	[DisableDefaultCtor]
	[BaseType (typeof (NSObject), Name = "NEDNSProxyManager")]
	interface NEDnsProxyManager {

		/// <include file="../docs/api/NetworkExtension/NEDnsProxyManager.xml" path="/Documentation/Docs[@DocId='P:NetworkExtension.NEDnsProxyManager.ProxyConfigurationDidChangeNotification']/*" />
		[Notification]
		[Field ("NEDNSProxyConfigurationDidChangeNotification")]
		NSString ProxyConfigurationDidChangeNotification { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Static]
		[Export ("sharedManager")]
		NEDnsProxyManager SharedManager { get; }

		/// <param name="completionHandler">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Async]
		[Export ("loadFromPreferencesWithCompletionHandler:")]
		void LoadFromPreferences (Action<NSError> completionHandler);

		/// <param name="completionHandler">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Async]
		[Export ("removeFromPreferencesWithCompletionHandler:")]
		void RemoveFromPreferences (Action<NSError> completionHandler);

		/// <param name="completionHandler">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Async]
		[Export ("saveToPreferencesWithCompletionHandler:")]
		void SaveToPreferences (Action<NSError> completionHandler);

		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("localizedDescription")]
		string LocalizedDescription { get; set; }

		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("providerProtocol", ArgumentSemantic.Strong)]
		NEDnsProxyProviderProtocol ProviderProtocol { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("enabled")]
		bool Enabled { [Bind ("isEnabled")] get; set; }
	}

	/// <summary>The main class for intercepting DNS traffic originating on the device.</summary>
	[NoTV]
	[MacCatalyst (13, 1)]
	[DisableDefaultCtor]
	[BaseType (typeof (NEProvider), Name = "NEDNSProxyProvider")]
	interface NEDnsProxyProvider {

		/// <param name="options">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <param name="completionHandler">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Async]
		[Export ("startProxyWithOptions:completionHandler:")]
		void StartProxy ([NullAllowed] NSDictionary options, Action<NSError> completionHandler);

		/// <param name="reason">To be added.</param>
		///         <param name="completionHandler">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Async]
		[Export ("stopProxyWithReason:completionHandler:")]
		void StopProxy (NEProviderStopReason reason, Action completionHandler);

		/// <param name="error">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("cancelProxyWithError:")]
		void CancelProxy ([NullAllowed] NSError error);

		/// <param name="flow">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("handleNewFlow:")]
		bool HandleNewFlow (NEAppProxyFlow flow);

		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("systemDNSSettings")]
		NEDnsSettings [] SystemDnsSettings { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Export ("handleNewUDPFlow:initialRemoteEndpoint:")]
		[Deprecated (PlatformName.iOS, 18, 0, message: "Use 'HandleNewUdpFlowWithInitialFlowEndPoint' instead.")]
		[Deprecated (PlatformName.MacCatalyst, 18, 0, message: "Use 'HandleNewUdpFlowWithInitialFlowEndPoint' instead.")]
		[Deprecated (PlatformName.MacOSX, 15, 0, message: "Use 'HandleNewUdpFlowWithInitialFlowEndPoint' instead.")]
		bool HandleNewUdpFlow (NEAppProxyUdpFlow flow, NWEndpoint remoteEndpoint);

		[NoTV, Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
		[Export ("handleNewUDPFlow:initialRemoteFlowEndpoint:")]
		bool HandleNewUdpFlowWithInitialFlowEndPoint (NEAppProxyUdpFlow flow, Network.NWEndpoint remoteEndpoint);
	}

	/// <summary>Configuration settings for <see cref="T:NetworkExtension.NEDnsProxyProvider" /> objects.</summary>
	[NoTV]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NEVpnProtocol), Name = "NEDNSProxyProviderProtocol")]
	interface NEDnsProxyProviderProtocol {

		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("providerConfiguration", ArgumentSemantic.Copy)]
		NSDictionary ProviderConfiguration { get; set; }

		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("providerBundleIdentifier")]
		string ProviderBundleIdentifier { get; set; }
	}

	[NoTV]
	[NoMac]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	interface NEHotspotHS20Settings : NSCopying, NSSecureCoding {

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("domainName")]
		string DomainName { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("roamingEnabled")]
		bool RoamingEnabled { [Bind ("isRoamingEnabled")] get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("roamingConsortiumOIs", ArgumentSemantic.Copy)]
		string [] RoamingConsortiumOIs { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("naiRealmNames", ArgumentSemantic.Copy)]
		string [] NaiRealmNames { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("MCCAndMNCs", ArgumentSemantic.Copy)]
		string [] MccAndMncs { get; set; }

		[Export ("initWithDomainName:roamingEnabled:")]
		NativeHandle Constructor (string domainName, bool roamingEnabled);
	}

	[NoTV]
	[NoMac]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject), Name = "NEHotspotEAPSettings")]
	interface NEHotspotEapSettings : NSCopying, NSSecureCoding {

		[Internal]
		[Export ("supportedEAPTypes", ArgumentSemantic.Copy)]
		IntPtr _SupportedEapTypes { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("username")]
		string Username { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("outerIdentity")]
		string OuterIdentity { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("ttlsInnerAuthenticationType", ArgumentSemantic.Assign)]
		NEHotspotConfigurationTtlsInnerAuthenticationType TtlsInnerAuthenticationType { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("password")]
		string Password { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("trustedServerNames", ArgumentSemantic.Copy)]
		string [] TrustedServerNames { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("tlsClientCertificateRequired")]
		bool TlsClientCertificateRequired { [Bind ("isTLSClientCertificateRequired")] get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("preferredTLSVersion", ArgumentSemantic.Assign)]
		NEHotspotConfigurationEapTlsVersion PreferredTlsVersion { get; set; }

		/// <param name="identity">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("setIdentity:")]
		bool SetIdentity (SecIdentity identity);

		/// <param name="certificates">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("setTrustedServerCertificates:")]
		bool SetTrustedServerCertificates (NSObject [] certificates);
	}

	/// <summary>Configuration settings for a network hotspot.</summary>
	[NoTV]
	[NoMac]
	[MacCatalyst (13, 1)]
	[DisableDefaultCtor]
	[BaseType (typeof (NSObject))]
	interface NEHotspotConfiguration : NSCopying, NSSecureCoding {

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("SSID")]
		string Ssid { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("joinOnce")]
		bool JoinOnce { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("lifeTimeInDays", ArgumentSemantic.Copy)]
		NSNumber LifeTimeInDays { get; set; }

		[Internal]
		[Export ("initWithSSID:")]
		IntPtr initWithSsid (string ssid);

		[Internal]
		[Export ("initWithSSID:passphrase:isWEP:")]
		IntPtr initWithSsid (string ssid, string passphrase, bool isWep);

		[Export ("initWithSSID:eapSettings:")]
		NativeHandle Constructor (string ssid, NEHotspotEapSettings eapSettings);

		[Export ("initWithHS20Settings:eapSettings:")]
		NativeHandle Constructor (NEHotspotHS20Settings hs20Settings, NEHotspotEapSettings eapSettings);

		[Internal]
		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Export ("initWithSSIDPrefix:")]
		IntPtr initWithSsidPrefix (string ssidPrefix);

		[Internal]
		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Export ("initWithSSIDPrefix:passphrase:isWEP:")]
		IntPtr initWithSsidPrefix (string ssidPrefix, string passphrase, bool isWep);

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Export ("hidden")]
		bool Hidden { get; set; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Export ("SSIDPrefix")]
		string SsidPrefix { get; }
	}

	delegate void NEHotspotConfigurationManagerJoinHotspotCallback ([NullAllowed] NSError error);

	/// <summary>Manages the application or removal of <see cref="T:NetworkExtension.NEHotspotConfiguration" />.</summary>
	[NoTV]
	[NoMac]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	interface NEHotspotConfigurationManager {

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Static]
		[Export ("sharedManager", ArgumentSemantic.Strong)]
		NEHotspotConfigurationManager SharedManager { get; }

		/// <param name="configuration">To be added.</param>
		///         <param name="completionHandler">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Async]
		[Export ("applyConfiguration:completionHandler:")]
		void ApplyConfiguration (NEHotspotConfiguration configuration, [NullAllowed] Action<NSError> completionHandler);

		/// <param name="ssid">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("removeConfigurationForSSID:")]
		void RemoveConfiguration (string ssid);

		/// <param name="domainName">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("removeConfigurationForHS20DomainName:")]
		void RemoveConfigurationForHS20DomainName (string domainName);

		/// <param name="completionHandler">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Async]
		[Export ("getConfiguredSSIDsWithCompletionHandler:")]
		void GetConfiguredSsids (Action<string []> completionHandler);

#if NET
		// Headers say this method is available on Mac Catalyst, but the AccessorySetupKit framework (thus the ASAccessory type) is not, so we can't expose it in Mac Catalyst for now.
		[NoTV, NoMac, iOS (18, 0), NoMacCatalyst]
		[Async]
		[Export ("joinAccessoryHotspot:passphrase:completionHandler:")]
		void JoinAccessoryHotspot (ASAccessory accessory, string passphrase, [NullAllowed] NEHotspotConfigurationManagerJoinHotspotCallback completionHandler);

		// Headers say this method is available on Mac Catalyst, but the AccessorySetupKit framework (thus the ASAccessory type) is not, so we can't expose it in Mac Catalyst for now.
		[NoTV, NoMac, iOS (18, 0), NoMacCatalyst]
		[Async]
		[Export ("joinAccessoryHotspotWithoutSecurity:completionHandler:")]
		void JoinAccessoryHotspotWithoutSecurit (ASAccessory accessory, [NullAllowed] NEHotspotConfigurationManagerJoinHotspotCallback completionHandler);
#endif
	}

	[NoTV]
	[NoiOS]
	[NoMacCatalyst]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface NENetworkRule : NSSecureCoding, NSCopying {

		[Deprecated (PlatformName.MacOSX, 15, 0, message: "Call the constructor overload that takes an 'NENetworkRuleConstructorOption' instead.")]
		[Export ("initWithDestinationNetwork:prefix:protocol:")]
		NativeHandle Constructor (NWHostEndpoint networkEndpoint, nuint destinationPrefix, NENetworkRuleProtocol protocol);

		[Deprecated (PlatformName.MacOSX, 15, 0, message: "Call the constructor overload that takes an 'NENetworkRuleConstructorOption' instead.")]
		[Export ("initWithDestinationHost:protocol:")]
		NativeHandle Constructor (NWHostEndpoint hostEndpoint, NENetworkRuleProtocol protocol);

		[Deprecated (PlatformName.MacOSX, 15, 0, message: "Call the constructor overload that takes an 'NENetworkRuleConstructorOption' instead.")]
		[Export ("initWithRemoteNetwork:remotePrefix:localNetwork:localPrefix:protocol:direction:")]
		NativeHandle Constructor ([NullAllowed] NWHostEndpoint remoteNetwork, nuint remotePrefix, [NullAllowed] NWHostEndpoint localNetwork, nuint localPrefix, NENetworkRuleProtocol protocol, NETrafficDirection direction);

		[Deprecated (PlatformName.MacOSX, 15, 0, message: "Use 'MatchRemoteHostOrNetworkEndpoint' instead.")]
		[NullAllowed, Export ("matchRemoteEndpoint")]
		NWHostEndpoint MatchRemoteEndpoint { get; }

		[Export ("matchRemotePrefix")]
		nuint MatchRemotePrefix { get; }

		[Deprecated (PlatformName.MacOSX, 15, 0, message: "Use 'MatchLocalNetworkEndpoint' instead.")]
		[NullAllowed, Export ("matchLocalNetwork")]
		NWHostEndpoint MatchLocalNetwork { get; }

		[Export ("matchLocalPrefix")]
		nuint MatchLocalPrefix { get; }

		[Export ("matchProtocol")]
		NENetworkRuleProtocol MatchProtocol { get; }

		[Export ("matchDirection")]
		NETrafficDirection MatchDirection { get; }

		[NoTV, Mac (15, 0), NoiOS, NoMacCatalyst]
		[Export ("initWithDestinationNetworkEndpoint:prefix:protocol:")]
		NativeHandle Constructor (Network.NWEndpoint networkEndpoint, nuint destinationPrefix, NENetworkRuleProtocol protocol);

		[NoTV, Mac (15, 0), NoiOS, NoMacCatalyst]
		[Export ("initWithDestinationHostEndpoint:protocol:")]
		NativeHandle Constructor (Network.NWEndpoint hostEndpoint, NENetworkRuleProtocol protocol);

		[NoTV, Mac (15, 0), NoiOS, NoMacCatalyst]
		[Export ("initWithRemoteNetworkEndpoint:remotePrefix:localNetworkEndpoint:localPrefix:protocol:direction:")]
		NativeHandle Constructor ([NullAllowed] Network.NWEndpoint remoteNetwork, nuint remotePrefix, [NullAllowed] Network.NWEndpoint localNetwork, nuint localPrefix, NENetworkRuleProtocol protocol, NETrafficDirection direction);

		[NoTV, Mac (15, 0), NoiOS, NoMacCatalyst]
		[Export ("matchRemoteHostOrNetworkEndpoint"), NullAllowed]
		Network.NWEndpoint MatchRemoteHostOrNetworkEndpoint { get; }

		[NoTV, Mac (15, 0), NoiOS, NoMacCatalyst]
		[Export ("matchLocalNetworkEndpoint"), NullAllowed]
		Network.NWEndpoint MatchLocalNetworkEndpoint { get; }
	}

	[NoTV]
	[NoiOS]
	[NoMacCatalyst]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface NEFilterRule : NSSecureCoding, NSCopying {

		[Export ("initWithNetworkRule:action:")]
		NativeHandle Constructor (NENetworkRule networkRule, NEFilterAction action);

		[Export ("networkRule", ArgumentSemantic.Copy)]
		NENetworkRule NetworkRule { get; }

		[Export ("action")]
		NEFilterAction Action { get; }
	}

	[NoTV]
	[NoiOS]
	[NoMacCatalyst]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface NEFilterSettings : NSSecureCoding, NSCopying {

		[Export ("initWithRules:defaultAction:")]
		NativeHandle Constructor (NEFilterRule [] rules, NEFilterAction defaultAction);

		[Export ("rules", ArgumentSemantic.Copy)]
		NEFilterRule [] Rules { get; }

		[Export ("defaultAction")]
		NEFilterAction DefaultAction { get; }
	}

	[NoTV]
	[NoiOS]
	[NoMacCatalyst]
	[BaseType (typeof (NSObject))]
	interface NEFilterPacketContext {
	}

	[NoTV]
	[NoiOS]
	[NoMacCatalyst]
	delegate NEFilterPacketProviderVerdict NEFilterPacketHandler (NEFilterPacketContext context, IntPtr @interface, NETrafficDirection directiom, IntPtr packetBytes, nuint packetLength);

	[NoTV]
	[NoiOS]
	[NoMacCatalyst]
	[BaseType (typeof (NEFilterProvider))]
	[DisableDefaultCtor] // returns `nil`
	interface NEFilterPacketProvider {
		[NullAllowed, Export ("packetHandler", ArgumentSemantic.Strong)]
		NEFilterPacketHandler PacketHandler { get; set; }

		[Export ("delayCurrentPacket:")]
		NEPacket DelayCurrentPacket (NEFilterPacketContext context);

		[Export ("allowPacket:")]
		void AllowPacket (NEPacket packet);
	}

	[NoTV]
	[NoiOS]
	[NoMacCatalyst]
	[BaseType (typeof (NEVpnManager))]
	[DisableDefaultCtor]
	interface NETransparentProxyManager {

		[Static]
		[Async]
		[Export ("loadAllFromPreferencesWithCompletionHandler:")]
		void LoadAllFromPreferences (Action<NETransparentProxyManager [], NSError> completionHandler);
	}

	[NoTV]
	[NoiOS]
	[NoMacCatalyst]
	[BaseType (typeof (NETunnelNetworkSettings))]
	interface NETransparentProxyNetworkSettings {

		[NullAllowed, Export ("includedNetworkRules", ArgumentSemantic.Copy)]
		NENetworkRule [] IncludedNetworkRules { get; set; }

		[NullAllowed, Export ("excludedNetworkRules", ArgumentSemantic.Copy)]
		NENetworkRule [] ExcludedNetworkRules { get; set; }
	}

	[NoTV, NoMac, iOS (14, 0)]
	[MacCatalyst (14, 0)]
	[BaseType (typeof (NSObject))]
	interface NEAppPushManager {
		[Export ("matchSSIDs", ArgumentSemantic.Copy)]
		string [] MatchSsids { get; set; }

		[Export ("providerConfiguration", ArgumentSemantic.Copy)]
		NSDictionary<NSString, NSObject> ProviderConfiguration { get; set; }

		[NullAllowed]
		[Export ("providerBundleIdentifier")]
		string ProviderBundleIdentifier { get; set; }

		[Wrap ("WeakDelegate")]
		[NullAllowed]
		INEAppPushDelegate Delegate { get; set; }

		[NullAllowed, Export ("delegate", ArgumentSemantic.Weak)]
		NSObject WeakDelegate { get; set; }

		[Async]
		[Static]
		[Export ("loadAllFromPreferencesWithCompletionHandler:")]
		void LoadAllFromPreferences (Action<NEAppPushManager [], NSError> completionHandler);

		[Async]
		[Export ("loadFromPreferencesWithCompletionHandler:")]
		void LoadFromPreferences (Action<NSError> completionHandler);

		[Async]
		[Export ("removeFromPreferencesWithCompletionHandler:")]
		void RemoveFromPreferences (Action<NSError> completionHandler);

		[Async]
		[Export ("saveToPreferencesWithCompletionHandler:")]
		void SaveToPreferences (Action<NSError> completionHandler);

		[NullAllowed]
		[Export ("localizedDescription")]
		string LocalizedDescription { get; set; }

		[Export ("enabled")]
		bool Enabled { [Bind ("isEnabled")] get; set; }

		[Export ("active")]
		bool Active { [Bind ("isActive")] get; }

		[NoTV, NoMac, iOS (15, 0), MacCatalyst (15, 0)]
		[Export ("matchPrivateLTENetworks", ArgumentSemantic.Copy)]
		NEPrivateLteNetwork [] MatchPrivateLteNetworks { get; set; }
	}

	[NoTV, NoMac, iOS (14, 0)]
	[MacCatalyst (14, 0)]
	[BaseType (typeof (NEProvider))]
	[DisableDefaultCtor] // init returns nil
	interface NEAppPushProvider {
		[NullAllowed]
		[Export ("providerConfiguration")]
		NSDictionary<NSString, NSObject> ProviderConfiguration { get; }

		[Deprecated (PlatformName.iOS, 15, 0, message: "Use the synchronoys 'Start' method instead..")]
		[Deprecated (PlatformName.MacCatalyst, 12, 0, message: "Use the synchronoys 'Start' method instead..")]
		[Async]
		[Export ("startWithCompletionHandler:")]
		void Start (Action<NSError> completionHandler);

		[Async]
		[Export ("stopWithReason:completionHandler:")]
		void Stop (NEProviderStopReason reason, Action completionHandler);

		[Export ("reportIncomingCallWithUserInfo:")]
		void ReportIncomingCall (NSDictionary userInfo);

		[NoTV, NoMacCatalyst, NoMac, iOS (16, 4)]
		[Export ("reportPushToTalkMessageWithUserInfo:")]
		void ReportPushToTalkMessage (NSDictionary userInfo);

		[Export ("handleTimerEvent")]
		void HandleTimerEvent ();

		[NoTV, NoMac, iOS (15, 0), MacCatalyst (15, 0)]
		[Export ("start")]
		void Start ();
	}

	[iOS (14, 0), TV (17, 0)]
	[MacCatalyst (14, 0)]
	[BaseType (typeof (NEDnsSettings), Name = "NEDNSOverHTTPSSettings")]
	interface NEDnsOverHttpsSettings {
		[NullAllowed]
		[Export ("serverURL", ArgumentSemantic.Copy)]
		NSUrl ServerUrl { get; set; }

		[Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0)]
		[Export ("identityReference", ArgumentSemantic.Copy)]
		[NullAllowed]
		NSData IdentityReference { get; set; }
	}

	[iOS (14, 0), TV (17, 0)]
	[MacCatalyst (14, 0)]
	[BaseType (typeof (NEDnsSettings), Name = "NEDNSOverTLSSettings")]
	interface NEDnsOverTlsSettings {
		[NullAllowed]
		[Export ("serverName")]
		string ServerName { get; set; }

		[Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0)]
		[Export ("identityReference", ArgumentSemantic.Copy)]
		[NullAllowed]
		NSData IdentityReference { get; set; }
	}

	[NoTV, iOS (14, 0)]
	[MacCatalyst (14, 0)]
	[DisableDefaultCtor]
	[BaseType (typeof (NSObject), Name = "NEDNSSettingsManager")]
	interface NEDnsSettingsManager {
		[Static]
		[Export ("sharedManager")]
		NEDnsSettingsManager SharedManager { get; }

		[Async]
		[Export ("loadFromPreferencesWithCompletionHandler:")]
		void LoadFromPreferences (Action<NSError> completionHandler);

		[Async]
		[Export ("removeFromPreferencesWithCompletionHandler:")]
		void RemoveFromPreferences (Action<NSError> completionHandler);

		[Async]
		[Export ("saveToPreferencesWithCompletionHandler:")]
		void SaveToPreferences (Action<NSError> completionHandler);

		[NullAllowed]
		[Export ("localizedDescription")]
		string LocalizedDescription { get; set; }

		[NullAllowed]
		[Export ("dnsSettings", ArgumentSemantic.Strong)]
		NEDnsSettings DnsSettings { get; set; }

		[NullAllowed]
		[Export ("onDemandRules", ArgumentSemantic.Copy)]
		NEOnDemandRule [] OnDemandRules { get; set; }

		[Export ("enabled")]
		bool Enabled { [Bind ("isEnabled")] get; }
	}

	interface INEAppPushDelegate { }

	[NoTV, NoMac, iOS (14, 0)]
	[MacCatalyst (14, 0)]
#if NET
	[Protocol, Model]
#else
	[Protocol, Model (AutoGeneratedName = true)]
#endif
	[BaseType (typeof (NSObject))]
	interface NEAppPushDelegate {
		[Abstract]
		[Export ("appPushManager:didReceiveIncomingCallWithUserInfo:")]
		void DidReceiveIncomingCall (NEAppPushManager manager, NSDictionary userInfo);
	}

	[NoMacCatalyst]
	[NoiOS]
	[NoTV]
	[BaseType (typeof (NEAppProxyProvider))]
	[DisableDefaultCtor] // `init` returns `nil`
	interface NETransparentProxyProvider {
	}

	[NoTV, NoMac, iOS (15, 0), MacCatalyst (15, 0)]
	[BaseType (typeof (NSObject), Name = "NEPrivateLTENetwork")]
	interface NEPrivateLteNetwork : NSCopying, NSSecureCoding {
		[Export ("mobileCountryCode")]
		string MobileCountryCode { get; set; }

		[Export ("mobileNetworkCode")]
		string MobileNetworkCode { get; set; }

		[NullAllowed]
		[Export ("trackingAreaCode")]
		string TrackingAreaCode { get; set; }
	}

	[NoTV, NoiOS, Mac (13, 0)]
	[NoMacCatalyst]
	[BaseType (typeof (NEPacketTunnelProvider))]
	[DisableDefaultCtor]
	interface NEEthernetTunnelProvider {
	}

	[NoTV, NoiOS, Mac (13, 0)]
	[NoMacCatalyst]
	[BaseType (typeof (NEPacketTunnelNetworkSettings))]
	interface NEEthernetTunnelNetworkSettings {
		[Export ("initWithTunnelRemoteAddress:ethernetAddress:mtu:")]
		NativeHandle Constructor (string address, string ethernetAddress, nint mtu);

		[Export ("ethernetAddress")]
		string EthernetAddress { get; }
	}

	[TV (17, 0), Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0)]
	[BaseType (typeof (NSObject))]
	interface NERelay : NSCopying, NSSecureCoding {
		[NullAllowed, Export ("HTTP3RelayURL", ArgumentSemantic.Copy)]
		NSUrl Http3RelayUrl { get; set; }

		[NullAllowed, Export ("HTTP2RelayURL", ArgumentSemantic.Copy)]
		NSUrl Http2RelayUrl { get; set; }

		[NullAllowed, Export ("dnsOverHTTPSURL", ArgumentSemantic.Copy)]
		NSUrl DnsOverHttpsUrl { get; set; }

		[NullAllowed, Export ("syntheticDNSAnswerIPv4Prefix")]
		string SyntheticDnsAnswerIPv4Prefix { get; set; }

		[NullAllowed, Export ("syntheticDNSAnswerIPv6Prefix")]
		string SyntheticDnsAnswerIPv6Prefix { get; set; }

		[Export ("additionalHTTPHeaderFields", ArgumentSemantic.Copy)]
		NSDictionary<NSString, NSString> AdditionalHttpHeaderFields { get; set; }

		[NullAllowed, Export ("rawPublicKeys", ArgumentSemantic.Copy)]
		NSData [] RawPublicKeys { get; set; }

		[NullAllowed, Export ("identityData", ArgumentSemantic.Copy)]
		NSData IdentityData { get; set; }

		[NullAllowed, Export ("identityDataPassword")]
		string IdentityDataPassword { get; set; }

		[Field ("NERelayErrorDomain")]
		NSString ErrorDomain { get; }

		[Notification, Field ("NERelayConfigurationDidChangeNotification")]
		NSString ConfigurationDidChangeNotification { get; }
	}

	delegate void NERelayManagerGetLastClientErrorsCallback ([NullAllowed] NSError [] errors);

	[TV (17, 0), Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0)]
	[BaseType (typeof (NSObject))]
	interface NERelayManager {
		[Static]
		[Export ("sharedManager")]
		NERelayManager Shared { get; }

		[Export ("loadFromPreferencesWithCompletionHandler:")]
		[Async]
		void LoadFromPreferences (Action<NSError> completionHandler);

		[Export ("removeFromPreferencesWithCompletionHandler:")]
		[Async]
		void RemoveFromPreferences (Action<NSError> completionHandler);

		[Export ("saveToPreferencesWithCompletionHandler:")]
		[Async]
		void SaveToPreferences (Action<NSError> completionHandler);

		[NullAllowed, Export ("localizedDescription")]
		string LocalizedDescription { get; set; }

		[Export ("enabled")]
		bool Enabled { [Bind ("isEnabled")] get; set; }

		[NullAllowed, Export ("relays", ArgumentSemantic.Strong)]
		NERelay [] Relays { get; set; }

		[NullAllowed, Export ("matchDomains", ArgumentSemantic.Copy)]
		string [] MatchDomains { get; set; }

		[NullAllowed, Export ("excludedDomains", ArgumentSemantic.Copy)]
		string [] ExcludedDomains { get; set; }

		[NullAllowed, Export ("onDemandRules", ArgumentSemantic.Copy)]
		NEOnDemandRule [] OnDemandRules { get; set; }

		[Static]
		[Export ("loadAllManagersFromPreferencesWithCompletionHandler:")]
		[Async]
		void LoadAllManagersFromPreferences (Action<NSArray<NERelayManager>, NSError> completionHandler);

		[TV (18, 0), Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
		[Export ("getLastClientErrors:completionHandler:")]
		[Async]
		void GetLastClientErrors (double seconds, NERelayManagerGetLastClientErrorsCallback completionHandler);

		[Export ("matchFQDNs", ArgumentSemantic.Copy), NullAllowed]
		[TV (18, 4), Mac (15, 4), iOS (18, 4), MacCatalyst (18, 4)]
		string [] MatchFqdns { get; set; }

		[Export ("excludedFQDNs", ArgumentSemantic.Copy), NullAllowed]
		[TV (18, 4), Mac (15, 4), iOS (18, 4), MacCatalyst (18, 4)]
		string [] ExcludedFqdns { get; set; }

		[Export ("UIToggleEnabled")]
		[TV (18, 4), Mac (15, 4), iOS (18, 4), MacCatalyst (18, 4)]
		bool UIToggleEnabled { [Bind ("isUIToggleEnabled")] get; set; }
	}

	[TV (18, 0), Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
	[ErrorDomain ("NERelayClientErrorDomain")]
	[Native]
	enum NERelayManagerClientError : long {
		None = 1,
		DNSFailed = 2,
		ServerUnreachable = 3,
		ServerDisconnected = 4,
		CertificateMissing = 5,
		CertificateInvalid = 6,
		CertificateExpired = 7,
		ServerCertificateInvalid = 8,
		ServerCertificateExpired = 9,
		Other = 10,
	}

	[TV (18, 0), Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
	[BaseType (typeof (NSObject), Name = "NEVPNIKEv2PPKConfiguration")]
	interface NEVpnIkev2PpkConfiguration : NSCopying {
		[Export ("initWithIdentifier:keychainReference:")]
		NativeHandle Constructor (string identifier, NSData keychainReference);

		[Export ("identifier", ArgumentSemantic.Copy)]
		string Identifier { get; }

		[Export ("keychainReference", ArgumentSemantic.Copy)]
		NSData KeychainReference { get; }

		[Export ("isMandatory")]
		bool IsMandatory { get; set; }
	}
}
