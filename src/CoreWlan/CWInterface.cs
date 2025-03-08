// Copyright 2014 Xamarin Inc. All rights reserved.
#if !__MACCATALYST__

#nullable enable

using Foundation;
using CoreFoundation;
using ObjCRuntime;
using System;

namespace CoreWlan {
	public unsafe partial class CWInterface {
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public CWChannel []? SupportedWlanChannels {
			get {
				NSSet? channels = _SupportedWlanChannels;
				return channels?.ToArray<CWChannel> ();
			}
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public CWNetwork []? CachedScanResults {
			get {
				NSSet? results = _CachedScanResults;
				return results?.ToArray<CWNetwork> ();
			}
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public static string []? InterfaceNames {
			get {
				NSSet? interfaceNames = _InterfaceNames;
				if (interfaceNames is not null)
					return Array.ConvertAll (interfaceNames.ToArray<NSString> (), item => (string) item);
				return null;
			}
		}

		public CWNetwork []? ScanForNetworksWithSsid (NSData ssid, out NSError error)
		{
			NSSet? networks = _ScanForNetworksWithSsid (ssid, out error);
			return networks?.ToArray<CWNetwork> ();
		}

		public CWNetwork []? ScanForNetworksWithName (string networkName, out NSError error)
		{
			NSSet? networks = _ScanForNetworksWithName (networkName, out error);
			return networks?.ToArray<CWNetwork> ();
		}

#if NET
		[SupportedOSPlatform ("macos")]
		[UnsupportedOSPlatform ("maccatalyst")]
#endif
		public CWNetwork []? ScanForNetworksWithSsid (NSData ssid, bool includeHidden, out NSError? error)
		{
			NSSet? networks = _ScanForNetworksWithSsid (ssid, includeHidden, out error);
			return networks?.ToArray<CWNetwork> ();
		}

#if NET
		[SupportedOSPlatform ("macos")]
		[UnsupportedOSPlatform ("maccatalyst")]
#endif
		public CWNetwork []? ScanForNetworksWithName (string networkName, bool includeHidden, out NSError? error)
		{
			NSSet? networks = _ScanForNetworksWithName (networkName, includeHidden, out error);
			return networks?.ToArray<CWNetwork> ();
		}

	}
}
#endif // !__MACCATALYST__
