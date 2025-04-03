// Copyright 2019 Microsoft Corporation

#nullable enable

#if !MONOMAC && !TVOS

using Foundation;

namespace NetworkExtension {

	public partial class NEHotspotConfiguration {

		/// <param name="ssid">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public NEHotspotConfiguration (string ssid)
		{
			InitializeHandle (initWithSsid (ssid));
		}

		/// <param name="ssid">To be added.</param>
		///         <param name="passphrase">To be added.</param>
		///         <param name="isWep">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public NEHotspotConfiguration (string ssid, string passphrase, bool isWep)
		{
			InitializeHandle (initWithSsid (ssid, passphrase, isWep));
		}

#if NET
		[SupportedOSPlatform ("ios13.0")]
		[SupportedOSPlatform ("maccatalyst")]
		[UnsupportedOSPlatform ("macos")]
		[UnsupportedOSPlatform ("tvos")]
#else
		[iOS (13, 0)]
#endif
		public NEHotspotConfiguration (string ssid, bool ssidIsPrefix)
		{
			var h = ssidIsPrefix ? initWithSsidPrefix (ssid) : initWithSsid (ssid);
			InitializeHandle (h);
		}

#if NET
		[SupportedOSPlatform ("ios13.0")]
		[SupportedOSPlatform ("maccatalyst")]
		[UnsupportedOSPlatform ("macos")]
		[UnsupportedOSPlatform ("tvos")]
#else
		[iOS (13, 0)]
#endif
		public NEHotspotConfiguration (string ssid, string passphrase, bool isWep, bool ssidIsPrefix)
		{
			var h = ssidIsPrefix ? initWithSsidPrefix (ssid, passphrase, isWep) : initWithSsid (ssid, passphrase, isWep);
			InitializeHandle (h);
		}
	}
}

#endif
