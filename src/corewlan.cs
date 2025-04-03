// corewlan.cs: bindings for CoreWLAN
//
// Author:
//   Ashok Gelal, Chris Hamons
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
using Foundation;
using CoreFoundation;
using ObjCRuntime;
using Security;
using System;

#if !NET
using NativeHandle = System.IntPtr;
#endif

namespace CoreWlan {

	[NoMacCatalyst]
	[BaseType (typeof (NSObject))]
	interface CWChannel : NSCoding, NSSecureCoding, NSCopying {
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("channelNumber")]
		nint ChannelNumber { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("channelWidth")]
		CWChannelWidth ChannelWidth { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("channelBand")]
		CWChannelBand ChannelBand { get; }

		/// <param name="channel">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("isEqualToChannel:")]
		bool IsEqualToChannel (CWChannel channel);
	}

#if !NET
	[NoMacCatalyst]
	[Deprecated (PlatformName.MacOSX, 10, 7), Obsoleted (PlatformName.MacOSX, 10, 10)]
	[BaseType (typeof (NSObject))]
	interface CW8021XProfile : NSCoding, NSCopying {
		[Deprecated (PlatformName.MacOSX, 10, 7)]
		[Export ("userDefinedName", ArgumentSemantic.Copy)]
		string UserDefinedName { get; set; }

		[Deprecated (PlatformName.MacOSX, 10, 7)]
		[Export ("ssid", ArgumentSemantic.Copy)]
		string Ssid { get; set; }

		[Deprecated (PlatformName.MacOSX, 10, 7)]
		[Export ("username", ArgumentSemantic.Copy)]
		string Username { get; set; }

		[Deprecated (PlatformName.MacOSX, 10, 7)]
		[Export ("password", ArgumentSemantic.Copy)]
		string Password { get; set; }

		[Deprecated (PlatformName.MacOSX, 10, 7)]
		[Export ("alwaysPromptForPassword")]
		bool AlwaysPromptForPassword { get; set; }

		[Static]
		[Export ("profile")]
		[Deprecated (PlatformName.MacOSX, 10, 7), Obsoleted (PlatformName.MacOSX, 10, 10)]
		CW8021XProfile Profile { get; }

		[Deprecated (PlatformName.MacOSX, 10, 7), Obsoleted (PlatformName.MacOSX, 10, 10)]
		[Export ("isEqualToProfile:")]
		bool IsEqualToProfile (CW8021XProfile profile);

		[Deprecated (PlatformName.MacOSX, 10, 7), Obsoleted (PlatformName.MacOSX, 10, 10)]
		[Static]
		[Export ("allUser8021XProfiles")]
		CW8021XProfile [] AllUser8021XProfiles { get; }
	}
#endif

	[NoMacCatalyst]
	[BaseType (typeof (NSObject))]
	interface CWConfiguration : NSSecureCoding, NSMutableCopying {
#if !NET
		[Deprecated (PlatformName.MacOSX, 10, 7), Obsoleted (PlatformName.MacOSX, 10, 9)]
		[Export ("rememberedNetworks")]
		NSSet RememberedNetworks { get; set; }

		[Deprecated (PlatformName.MacOSX, 10, 7), Obsoleted (PlatformName.MacOSX, 10, 9)]
		[Export ("preferredNetworks")]
		CWWirelessProfile [] PreferredNetworks { get; set; }

		[Deprecated (PlatformName.MacOSX, 10, 7), Obsoleted (PlatformName.MacOSX, 10, 9)]
		[Export ("alwaysRememberNetworks")]
		bool AlwaysRememberNetworks { get; set; }

		[Deprecated (PlatformName.MacOSX, 10, 7), Obsoleted (PlatformName.MacOSX, 10, 9)]
		[Export ("disconnectOnLogout")]
		bool DisconnectOnLogout { get; set; }

		[Deprecated (PlatformName.MacOSX, 10, 7), Obsoleted (PlatformName.MacOSX, 10, 9)]
		[Export ("requireAdminForNetworkChange")]
		bool RequireAdminForNetworkChange { get; set; }

		[Deprecated (PlatformName.MacOSX, 10, 7), Obsoleted (PlatformName.MacOSX, 10, 9)]
		[Export ("requireAdminForPowerChange")]
		bool RequireAdminForPowerChange { get; set; }

		[Deprecated (PlatformName.MacOSX, 10, 7), Obsoleted (PlatformName.MacOSX, 10, 9)]
		[Export ("requireAdminForIBSSCreation")]
		bool RequireAdminForIBSSCreation { get; set; }
#endif

		[Export ("networkProfiles", ArgumentSemantic.Copy)]
		[Internal]
		NSOrderedSet _NetworkProfiles { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("requireAdministratorForAssociation", ArgumentSemantic.Assign)]
		bool RequireAdministratorForAssociation { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("requireAdministratorForPower", ArgumentSemantic.Assign)]
		bool RequireAdministratorForPower { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("requireAdministratorForIBSSMode", ArgumentSemantic.Assign)]
		bool RequireAdministratorForIbssMode { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("rememberJoinedNetworks", ArgumentSemantic.Assign)]
		bool RememberJoinedNetworks { get; }

		[Export ("initWithConfiguration:")]
		NativeHandle Constructor (CWConfiguration configuration);

		/// <param name="configuration">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("isEqualToConfiguration:")]
		bool IsEqualToConfiguration (CWConfiguration configuration);

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Static]
		[Export ("configuration")]
		CWConfiguration Create ();

		[Static]
		[Export ("configurationWithConfiguration:")]
		CWConfiguration Create (CWConfiguration configuration);
	}

	[NoMacCatalyst]
	[BaseType (typeof (CWConfiguration))]
	interface CWMutableConfiguration {

		[Export ("networkProfiles", ArgumentSemantic.Copy)]
		NSOrderedSet<CWNetworkProfile> NetworkProfiles { get; set; }

		[Export ("requireAdministratorForAssociation")]
		bool RequireAdministratorForAssociation { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("requireAdministratorForPower", ArgumentSemantic.Assign)]
		bool RequireAdministratorForPower { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Deprecated (PlatformName.MacOSX, 11, 0)]
		[Export ("requireAdministratorForIBSSMode", ArgumentSemantic.Assign)]
		bool RequireAdministratorForIbssMode { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("rememberJoinedNetworks", ArgumentSemantic.Assign)]
		bool RememberJoinedNetworks { get; set; }
	}

	[NoMacCatalyst]
	[BaseType (typeof (NSObject))]
	interface CWInterface {
#if !NET
		[Deprecated (PlatformName.MacOSX, 10, 7), Obsoleted (PlatformName.MacOSX, 10, 9)]
		[Export ("supportsWoW")]
		bool SupportsWow { get; }

		[Deprecated (PlatformName.MacOSX, 10, 7), Obsoleted (PlatformName.MacOSX, 10, 9)]
		[Export ("supportsWEP")]
		bool SupportsWep { get; }

		[Deprecated (PlatformName.MacOSX, 10, 7), Obsoleted (PlatformName.MacOSX, 10, 9)]
		[Export ("supportsAES_CCM")]
		bool SupportsAesCcm { get; }

		[Deprecated (PlatformName.MacOSX, 10, 7), Obsoleted (PlatformName.MacOSX, 10, 9)]
		[Export ("supportsIBSS")]
		bool SupportsIbss { get; }

		[Deprecated (PlatformName.MacOSX, 10, 7), Obsoleted (PlatformName.MacOSX, 10, 9)]
		[Export ("supportsTKIP")]
		bool SupportsTkip { get; }

		[Deprecated (PlatformName.MacOSX, 10, 7), Obsoleted (PlatformName.MacOSX, 10, 9)]
		[Export ("supportsPMGT")]
		bool SupportsPmgt { get; }

		[Deprecated (PlatformName.MacOSX, 10, 7), Obsoleted (PlatformName.MacOSX, 10, 9)]
		[Export ("supportsHostAP")]
		bool SupportsHostAP { get; }

		[Deprecated (PlatformName.MacOSX, 10, 7), Obsoleted (PlatformName.MacOSX, 10, 9)]
		[Export ("supportsMonitorMode")]
		bool SupportsMonitorMode { get; }

		[Deprecated (PlatformName.MacOSX, 10, 7), Obsoleted (PlatformName.MacOSX, 10, 9)]
		[Export ("supportsWPA")]
		bool SupportsWpa { get; }

		[Deprecated (PlatformName.MacOSX, 10, 7), Obsoleted (PlatformName.MacOSX, 10, 9)]
		[Export ("supportsWPA2")]
		bool SupportsWpa2 { get; }

		[Deprecated (PlatformName.MacOSX, 10, 7), Obsoleted (PlatformName.MacOSX, 10, 9)]
		[Export ("supportsWME")]
		bool SupportsWme { get; }

		[Deprecated (PlatformName.MacOSX, 10, 7), Obsoleted (PlatformName.MacOSX, 10, 9)]
		[Export ("supportsShortGI40MHz")]
		bool SupportsShortGI40MHz { get; }

		[Deprecated (PlatformName.MacOSX, 10, 7), Obsoleted (PlatformName.MacOSX, 10, 9)]
		[Export ("supportsShortGI20MHz")]
		bool SupportsShortGI20MHz { get; }

		[Deprecated (PlatformName.MacOSX, 10, 7), Obsoleted (PlatformName.MacOSX, 10, 9)]
		[Export ("supportsTSN")]
		bool SupportsTsn { get; }

		[Deprecated (PlatformName.MacOSX, 10, 7), Obsoleted (PlatformName.MacOSX, 10, 9)]
		[Export ("power")]
		bool Power { get; }

		[Deprecated (PlatformName.MacOSX, 10, 7), Obsoleted (PlatformName.MacOSX, 10, 9)]
		[Export ("powerSave")]
		bool PowerSave { get; }

		[Deprecated (PlatformName.MacOSX, 10, 7), Obsoleted (PlatformName.MacOSX, 10, 9)]
		[Export ("name")]
		string Name { get; }

		[Deprecated (PlatformName.MacOSX, 10, 7), Obsoleted (PlatformName.MacOSX, 10, 9)]
		[Export ("supportedChannels")]
		NSNumber [] SupportedChannels { get; }

		[Deprecated (PlatformName.MacOSX, 10, 7), Obsoleted (PlatformName.MacOSX, 10, 9)]
		[Export ("supportedPHYModes")]
		NSNumber [] SupportedPhyModes { get; }

		[Deprecated (PlatformName.MacOSX, 10, 7), Obsoleted (PlatformName.MacOSX, 10, 9)]
		[Export ("channel")]
		NSNumber Channel { get; }

		[Deprecated (PlatformName.MacOSX, 10, 7), Obsoleted (PlatformName.MacOSX, 10, 9)]
		[Export ("phyMode")]
		NSNumber PhyMode { get; }

		[Deprecated (PlatformName.MacOSX, 10, 7), Obsoleted (PlatformName.MacOSX, 10, 9)]
		[Export ("bssidData")]
		NSData BssidData { get; }

		[Deprecated (PlatformName.MacOSX, 10, 7), Obsoleted (PlatformName.MacOSX, 10, 9)]
		[Export ("rssi")]
		NSNumber Rssi { get; }

		[Deprecated (PlatformName.MacOSX, 10, 7), Obsoleted (PlatformName.MacOSX, 10, 9)]
		[Export ("noise")]
		NSNumber Noise { get; }

		[Deprecated (PlatformName.MacOSX, 10, 7), Obsoleted (PlatformName.MacOSX, 10, 9)]
		[Export ("txRate")]
		NSNumber TxRate { get; }

		[Deprecated (PlatformName.MacOSX, 10, 7), Obsoleted (PlatformName.MacOSX, 10, 9)]
		[Export ("securityMode")]
		NSNumber SecurityMode { get; }

		[Deprecated (PlatformName.MacOSX, 10, 7), Obsoleted (PlatformName.MacOSX, 10, 9)]
		[Export ("interfaceState")]
		NSNumber InterfaceState { get; }

		[Deprecated (PlatformName.MacOSX, 10, 7), Obsoleted (PlatformName.MacOSX, 10, 9)]
		[Export ("opMode")]
		NSNumber OpMode { get; }

		[Deprecated (PlatformName.MacOSX, 10, 7), Obsoleted (PlatformName.MacOSX, 10, 9)]
		[Export ("txPower")]
		NSNumber TxPower { get; }

		[Deprecated (PlatformName.MacOSX, 10, 7), Obsoleted (PlatformName.MacOSX, 10, 9)]
		[Static]
		[Export ("supportedInterfaces")]
		string [] SupportedInterfaces { get; }

		[Deprecated (PlatformName.MacOSX, 10, 7), Obsoleted (PlatformName.MacOSX, 10, 9)]
		[Static]
		[Export ("interface")]
		CWInterface MainInterface { get; }

		[Deprecated (PlatformName.MacOSX, 10, 7), Obsoleted (PlatformName.MacOSX, 10, 9)]
		[Static]
		[Export ("interfaceWithName:")]
		CWInterface FromName ([NullAllowed] string name);

		[Deprecated (PlatformName.MacOSX, 10, 7), Obsoleted (PlatformName.MacOSX, 10, 9)]
		[Export ("isEqualToInterface:")]
		bool IsEqualToInterface (CWInterface intface);

		[Deprecated (PlatformName.MacOSX, 10, 7), Obsoleted (PlatformName.MacOSX, 10, 9)]
		[Export ("setChannel:error:")]
		bool SetChannel (nuint channel, out NSError error);

		[Deprecated (PlatformName.MacOSX, 10, 7), Obsoleted (PlatformName.MacOSX, 10, 9)]
		[Export ("scanForNetworksWithParameters:error:")]
		CWNetwork [] ScanForNetworksWithParameters ([NullAllowed] NSDictionary parameters, out NSError error);

		[Deprecated (PlatformName.MacOSX, 10, 7), Obsoleted (PlatformName.MacOSX, 10, 9)]
		[Export ("associateToNetwork:parameters:error:")]
		bool AssociateToNetwork ([NullAllowed] CWNetwork network, [NullAllowed] NSDictionary parameters, out NSError error);

		[Deprecated (PlatformName.MacOSX, 10, 7), Obsoleted (PlatformName.MacOSX, 10, 9)]
		[Export ("enableIBSSWithParameters:error:")]
		bool EnableIBSSWithParameters ([NullAllowed] NSDictionary parameters, out NSError error);

		[Deprecated (PlatformName.MacOSX, 10, 7), Obsoleted (PlatformName.MacOSX, 10, 9)]
		[Export ("commitConfiguration:error:")]
		bool CommitConfiguration (CWConfiguration config, out NSError error);
#endif // !NET

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("powerOn", ArgumentSemantic.Assign)]
		bool PowerOn { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NullAllowed]
		[Export ("interfaceName", ArgumentSemantic.Copy)]
		string InterfaceName { get; }

		[NullAllowed]
		[Export ("supportedWLANChannels")]
		[Internal]
		NSSet _SupportedWlanChannels { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NullAllowed]
		[Export ("wlanChannel")]
		CWChannel WlanChannel { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("activePHYMode")]
		CWPhyMode ActivePHYMode { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NullAllowed]
		[Export ("ssid")]
		string Ssid { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NullAllowed]
		[Export ("ssidData")]
		NSData SsidData { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NullAllowed]
		[Export ("bssid")]
		string Bssid { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("rssiValue")]
		nint RssiValue { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("noiseMeasurement")]
		nint NoiseMeasurement { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("security")]
		CWSecurity Security { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("transmitRate")]
		double TransmitRate { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NullAllowed]
		[Export ("countryCode")]
		string CountryCode { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("interfaceMode")]
		CWInterfaceMode InterfaceMode { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("transmitPower")]
		nint TransmitPower { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NullAllowed]
		[Export ("hardwareAddress")]
		string HardwareAddress { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("deviceAttached", ArgumentSemantic.Assign)]
		bool DeviceAttached { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("serviceActive")]
		bool ServiceActive { get; }

		[NullAllowed]
		[Export ("cachedScanResults")]
		[Internal]
		NSSet _CachedScanResults { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NullAllowed]
		[Export ("configuration")]
		CWConfiguration Configuration { get; }

		[NullAllowed]
		[Static]
		[Export ("interfaceNames")]
		[Internal]
		NSSet _InterfaceNames { get; }

		[Deprecated (PlatformName.MacOSX, 10, 10, message: "Use 'CWWiFiClient.FromName' instead.")]
		[Export ("initWithInterfaceName:")]
		NativeHandle Constructor ([NullAllowed] string name);

		/// <param name="power">To be added.</param>
		///         <param name="error">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("setPower:error:")]
		bool SetPower (bool power, out NSError error);

		/// <param name="channel">To be added.</param>
		///         <param name="error">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("setWLANChannel:error:")]
		bool SetWlanChannel (CWChannel channel, out NSError error);

		/// <param name="key">To be added.</param>
		///         <param name="error">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("setPairwiseMasterKey:error:")]
		bool SetPairwiseMasterKey ([NullAllowed] NSData key, out NSError error);

		[Export ("setWEPKey:flags:index:error:")]
		bool SetWEPKey ([NullAllowed] NSData key, CWCipherKeyFlags flags, nint index, out NSError error);

		[return: NullAllowed]
		[Export ("scanForNetworksWithSSID:error:")]
		[Internal]
		NSSet _ScanForNetworksWithSsid ([NullAllowed] NSData ssid, out NSError error);

		[return: NullAllowed]
		[Export ("scanForNetworksWithName:error:")]
		[Internal]
		NSSet _ScanForNetworksWithName ([NullAllowed] string networkName, out NSError error);

		/// <param name="network">To be added.</param>
		///         <param name="password">To be added.</param>
		///         <param name="error">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("associateToNetwork:password:error:")]
		bool AssociateToNetwork (CWNetwork network, [NullAllowed] string password, out NSError error);

		/// <param name="network">To be added.</param>
		///         <param name="identity">To be added.</param>
		///         <param name="username">To be added.</param>
		///         <param name="password">To be added.</param>
		///         <param name="error">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("associateToEnterpriseNetwork:identity:username:password:error:")]
		bool AssociateToEnterpriseNetwork (CWNetwork network, [NullAllowed] SecIdentity identity, [NullAllowed] string username, [NullAllowed] string password, out NSError error);

		[Deprecated (PlatformName.MacOSX, 11, 0)]
		[Export ("startIBSSModeWithSSID:security:channel:password:error:")]
		bool StartIbssModeWithSsid (NSData ssidData, CWIbssModeSecurity security, nuint channel, [NullAllowed] string password, out NSError error);

		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("disassociate")]
		void Disassociate ();

		/// <param name="configuration">To be added.</param>
		///         <param name="authorization">To be added.</param>
		///         <param name="error">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("commitConfiguration:authorization:error:")]
		bool CommitConfiguration (CWConfiguration configuration, [NullAllowed] NSObject authorization, out NSError error);

		[Export ("scanForNetworksWithSSID:includeHidden:error:")]
		[return: NullAllowed]
		[Internal]
		NSSet _ScanForNetworksWithSsid ([NullAllowed] NSData ssid, bool includeHidden, [NullAllowed] out NSError error);

		[Export ("scanForNetworksWithName:includeHidden:error:")]
		[return: NullAllowed]
		[Internal]
		NSSet _ScanForNetworksWithName ([NullAllowed] string networkName, bool includeHidden, [NullAllowed] out NSError error);
	}

#if !NET
	[NoMacCatalyst]
	[BaseType (typeof (NSObject))]
	[Deprecated (PlatformName.MacOSX, 10, 7), Obsoleted (PlatformName.MacOSX, 10, 10)]
	interface CWWirelessProfile : NSCoding, NSCopying {
		[Deprecated (PlatformName.MacOSX, 10, 7)]
		[Export ("ssid", ArgumentSemantic.Copy)]
		string Ssid { get; set; }

		[Deprecated (PlatformName.MacOSX, 10, 7)]
		[Export ("securityMode", ArgumentSemantic.Retain)]
		NSNumber SecurityMode { get; set; }

		[Deprecated (PlatformName.MacOSX, 10, 7)]
		[Export ("passphrase", ArgumentSemantic.Copy)]
		string Passphrase { get; set; }

		[Deprecated (PlatformName.MacOSX, 10, 7), Obsoleted (PlatformName.MacOSX, 10, 10)]
		[Export ("user8021XProfile", ArgumentSemantic.Retain)]
		CW8021XProfile User8021XProfile { get; set; }

		[Deprecated (PlatformName.MacOSX, 10, 7)]
		[Export ("isEqualToProfile:")]
		bool IsEqualToProfile (CWWirelessProfile profile);
	}
#endif // !NET

	[NoMacCatalyst]
	[BaseType (typeof (NSObject))]
	interface CWNetwork : NSSecureCoding, NSCopying {
#if !NET
		[Deprecated (PlatformName.MacOSX, 10, 7), Obsoleted (PlatformName.MacOSX, 10, 9)]
		[NullAllowed]
		[Export ("bssidData")]
		NSData BssidData { get; }

		[Deprecated (PlatformName.MacOSX, 10, 7), Obsoleted (PlatformName.MacOSX, 10, 9)]
		[Export ("securityMode")]
		NSNumber SecurityMode { get; }

		[Deprecated (PlatformName.MacOSX, 10, 7), Obsoleted (PlatformName.MacOSX, 10, 9)]
		[Export ("phyMode")]
		NSNumber PhyMode { get; }

		[Deprecated (PlatformName.MacOSX, 10, 7), Obsoleted (PlatformName.MacOSX, 10, 9)]
		[Export ("channel")]
		NSNumber Channel { get; }

		[Deprecated (PlatformName.MacOSX, 10, 7), Obsoleted (PlatformName.MacOSX, 10, 9)]
		[Export ("rssi")]
		NSNumber Rssi { get; }

		[Deprecated (PlatformName.MacOSX, 10, 7), Obsoleted (PlatformName.MacOSX, 10, 9)]
		[Export ("noise")]
		NSNumber Noise { get; }

		[Deprecated (PlatformName.MacOSX, 10, 7), Obsoleted (PlatformName.MacOSX, 10, 9)]
		[Export ("ieData")]
		NSData IeData { get; }

		[Deprecated (PlatformName.MacOSX, 10, 7), Obsoleted (PlatformName.MacOSX, 10, 9)]
		[Export ("isIBSS")]
		bool IsIBSS { get; }

		[Deprecated (PlatformName.MacOSX, 10, 7), Obsoleted (PlatformName.MacOSX, 10, 9)]
		[Export ("wirelessProfile")]
		CWWirelessProfile WirelessProfile { get; }
#endif

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NullAllowed]
		[Export ("ssid")]
		string Ssid { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NullAllowed]
		[Export ("ssidData")]
		NSData SsidData { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NullAllowed]
		[Export ("bssid")]
		string Bssid { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NullAllowed]
		[Export ("wlanChannel")]
		CWChannel WlanChannel { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("rssiValue")]
		nint RssiValue { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("noiseMeasurement")]
		nint NoiseMeasurement { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NullAllowed]
		[Export ("informationElementData")]
		NSData InformationElementData { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NullAllowed]
		[Export ("countryCode")]
		string CountryCode { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("beaconInterval")]
		nint BeaconInterval { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("ibss")]
		bool Ibss { get; }

		/// <param name="network">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("isEqualToNetwork:")]
		bool IsEqualToNetwork (CWNetwork network);

		/// <param name="security">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("supportsSecurity:")]
		bool SupportsSecurity (CWSecurity security);

		/// <param name="phyMode">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("supportsPHYMode:")]
		bool SupportsPhyMode (CWPhyMode phyMode);
	}

	[NoMacCatalyst]
	[BaseType (typeof (NSObject))]
	interface CWNetworkProfile : NSCoding, NSSecureCoding, NSCopying, NSMutableCopying {
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NullAllowed]
		[Export ("ssid", ArgumentSemantic.Copy)]
		string Ssid { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("ssidData", ArgumentSemantic.Copy)]
		[NullAllowed]
		NSData SsidData { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("security", ArgumentSemantic.Assign)]
		CWSecurity Security { get; }

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Static]
		[Export ("networkProfile")]
		NSObject NetworkProfile ();

		[Export ("initWithNetworkProfile:")]
		NativeHandle Constructor (CWNetworkProfile networkProfile);

		/// <param name="networkProfile">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Static]
		[Export ("networkProfileWithNetworkProfile:")]
		NSObject NetworkProfileWithNetworkProfile (CWNetworkProfile networkProfile);

		/// <param name="networkProfile">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("isEqualToNetworkProfile:")]
		bool IsEqualToNetworkProfile (CWNetworkProfile networkProfile);
	}

	[NoMacCatalyst]
	[BaseType (typeof (CWNetworkProfile))]
	interface CWMutableNetworkProfile : NSCoding, NSSecureCoding, NSCopying, NSMutableCopying {
		[NullAllowed]
		[Export ("ssidData", ArgumentSemantic.Copy)]
		NSData SsidData { get; set; }

		[Export ("security", ArgumentSemantic.Assign)]
		CWSecurity Security { get; set; }
	}

	[NoMacCatalyst]
	[BaseType (typeof (NSObject))]
	interface CWWiFiClient {
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("delegate", ArgumentSemantic.Weak)]
		[NullAllowed]
		ICWEventDelegate Delegate { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("interface")]
		[NullAllowed]
		CWInterface MainInterface { get; }

		/// <param name="name">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[return: NullAllowed]
		[Export ("interfaceWithName:")]
		CWInterface FromName ([NullAllowed] string name);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("interfaces")]
		[NullAllowed]
		CWInterface [] Interfaces { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NullAllowed]
		[Export ("interfaceNames")]
		[Static]
		string [] InterfaceNames { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("sharedWiFiClient")]
		[Static]
		CWWiFiClient SharedWiFiClient { get; }

		/// <param name="type">To be added.</param>
		///         <param name="error">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("startMonitoringEventWithType:error:")]
		bool StartMonitoringEvent (CWEventType type, out NSError error);

		/// <param name="error">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("stopMonitoringAllEventsAndReturnError:")]
		bool StopMonitoringAllEvents (out NSError error);

		/// <param name="type">To be added.</param>
		///         <param name="error">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("stopMonitoringEventWithType:error:")]
		bool StopMonitoringEvent (CWEventType type, out NSError error);
	}

	interface ICWEventDelegate { }

	[BaseType (typeof (NSObject))]
	[Model]
	[Protocol]
	interface CWEventDelegate {
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("clientConnectionInterrupted")]
		void ClientConnectionInterrupted ();

		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("clientConnectionInvalidated")]
		void ClientConnectionInvalidated ();

		/// <param name="interfaceName">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("powerStateDidChangeForWiFiInterfaceWithName:")]
		void PowerStateDidChangeForWiFi (string interfaceName);

		/// <param name="interfaceName">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("ssidDidChangeForWiFiInterfaceWithName:")]
		void SsidDidChangeForWiFi (string interfaceName);

		/// <param name="interfaceName">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("bssidDidChangeForWiFiInterfaceWithName:")]
		void BssidDidChangeForWiFi (string interfaceName);

		/// <param name="interfaceName">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("countryCodeDidChangeForWiFiInterfaceWithName:")]
		void CountryCodeDidChangeForWiFi (string interfaceName);

		/// <param name="interfaceName">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("linkDidChangeForWiFiInterfaceWithName:")]
		void LinkDidChangeForWiFi (string interfaceName);

		/// <param name="interfaceName">To be added.</param>
		///         <param name="rssi">To be added.</param>
		///         <param name="transmitRate">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("linkQualityDidChangeForWiFiInterfaceWithName:rssi:transmitRate:")]
		void LinkQualityDidChangeForWiFi (string interfaceName, int rssi, double transmitRate);

		/// <param name="interfaceName">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("modeDidChangeForWiFiInterfaceWithName:")]
		void ModeDidChangeForWiFi (string interfaceName);

		/// <param name="interfaceName">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("scanCacheUpdatedForWiFiInterfaceWithName:")]
		void ScanCacheUpdatedForWiFi (string interfaceName);
	}
}
