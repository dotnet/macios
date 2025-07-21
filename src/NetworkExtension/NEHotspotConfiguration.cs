// Copyright 2019 Microsoft Corporation

#nullable enable

#if !MONOMAC && !TVOS

using Foundation;

namespace NetworkExtension {

	public partial class NEHotspotConfiguration {

		/// <summary>Create a new <see cref="NEHotspotConfiguration" /> with the specified SSID.</summary>
		/// <param name="ssid">The SSID the new <see cref="NEHotspotConfiguration" /> applies to.</param>
		public NEHotspotConfiguration (string ssid)
			: base (NSObjectFlag.Empty)
		{
			InitializeHandle (_InitWithSsid (ssid), "initWithSSID:");
		}

		/// <summary>Create a new <see cref="NEHotspotConfiguration" /> with the specified SSID.</summary>
		/// <param name="ssid">The SSID the new <see cref="NEHotspotConfiguration" /> applies to.</param>
		/// <param name="passphrase">The passphrase for the network specified by <paramref name="ssid" />.</param>
		/// <param name="isWep">Whether the network is a WEP network (otherwise a WPA or WPA2 network).</param>
		public NEHotspotConfiguration (string ssid, string passphrase, bool isWep)
			: base (NSObjectFlag.Empty)
		{
			InitializeHandle (_InitWithSsidAndPassprase (ssid, passphrase, isWep), "initWithSSID:passphrase:isWEP:");
		}

		/// <summary>Create a new <see cref="NEHotspotConfiguration" /> with the specified SSID.</summary>
		/// <param name="ssid">The SSID the new <see cref="NEHotspotConfiguration" /> applies to.</param>
		/// <param name="ssidIsPrefix">Whether <paramref name="ssid" /> specifies the prefix of an SSID, or a complete SSID.</param>
		[SupportedOSPlatform ("ios13.0")]
		[SupportedOSPlatform ("maccatalyst")]
		[UnsupportedOSPlatform ("macos")]
		[UnsupportedOSPlatform ("tvos")]
		public NEHotspotConfiguration (string ssid, bool ssidIsPrefix)
			: base (NSObjectFlag.Empty)
		{
			if (ssidIsPrefix) {
				InitializeHandle (_InitWithSsidPrefix (ssid), "initWithSSIDPrefix:");
			} else {
				InitializeHandle (_InitWithSsid (ssid), "initWithSSID:");
			}
		}

		/// <summary>Create a new <see cref="NEHotspotConfiguration" /> with the specified SSID.</summary>
		/// <param name="ssid">The SSID the new <see cref="NEHotspotConfiguration" /> applies to.</param>
		/// <param name="passphrase">The passphrase for the network specified by <paramref name="ssid" />.</param>
		/// <param name="isWep">Whether the network is a WEP network (otherwise a WPA or WPA2 network).</param>
		/// <param name="ssidIsPrefix">Whether <paramref name="ssid" /> specifies the prefix of an SSID, or a complete SSID.</param>
		[SupportedOSPlatform ("ios13.0")]
		[SupportedOSPlatform ("maccatalyst")]
		[UnsupportedOSPlatform ("macos")]
		[UnsupportedOSPlatform ("tvos")]
		public NEHotspotConfiguration (string ssid, string passphrase, bool isWep, bool ssidIsPrefix)
			: base (NSObjectFlag.Empty)
		{
			if (ssidIsPrefix) {
				InitializeHandle (_InitWithSsidPrefixAndPassphrase (ssid, passphrase, isWep), "initWithSSIDPrefix:passphrase:isWEP:");
			} else {
				InitializeHandle (_InitWithSsidAndPassprase (ssid, passphrase, isWep), "initWithSSID:passphrase:isWEP:");
			}
		}
	}
}

#endif
