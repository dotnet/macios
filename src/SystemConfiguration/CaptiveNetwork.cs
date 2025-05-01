//
// CaptiveNetwork.cs: CaptiveNetwork binding
//
// Authors:
//	Miguel de Icaza (miguel@xamarin.com)
//	Sebastien Pouliot  <sebastien@xamarin.com>
//	Marek Safar (marek.safar@gmail.com)
//
// Copyright 2012-2015 Xamarin Inc. All rights reserved.
//

#nullable enable

using System;
using System.Runtime.InteropServices;
using CoreFoundation;
using Foundation;
using ObjCRuntime;

namespace SystemConfiguration {

	// http://developer.apple.com/library/ios/#documentation/SystemConfiguration/Reference/CaptiveNetworkRef/Reference/reference.html
	// CaptiveNetwork.h
	public static partial class CaptiveNetwork {

#if !__TVOS__
#if !MONOMAC
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[ObsoletedOSPlatform ("ios14.0")]
		[ObsoletedOSPlatform ("maccatalyst14.0")]
		[DllImport (Constants.SystemConfigurationLibrary)]
		extern static IntPtr /* CFDictionaryRef __nullable */  CNCopyCurrentNetworkInfo (
			/* CFStringRef __nonnull */ IntPtr interfaceName);

		/// <summary>Copy the current network information into an <see cref="NSDictionary" />.</summary>
		/// <param name="interfaceName">The name of the interface with the network information to copy.</param>
		/// <param name="currentNetworkInfo">The copied network information upon return.</param>
		/// <returns><see cref="StatusCode.OK" /> if successful, an error status otherwise.</returns>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[ObsoletedOSPlatform ("ios14.0")]
		[ObsoletedOSPlatform ("maccatalyst14.0")]
		static public StatusCode TryCopyCurrentNetworkInfo (string interfaceName, out NSDictionary? currentNetworkInfo)
		{
			if (string.IsNullOrEmpty (interfaceName))
				ThrowHelper.ThrowArgumentNullException (nameof (interfaceName));

			using var nss = new TransientCFString (interfaceName);
			var ni = CNCopyCurrentNetworkInfo (nss);
			if (ni == IntPtr.Zero) {
				currentNetworkInfo = null;
				return StatusCodeError.SCError ();
			}

			currentNetworkInfo = new NSDictionary (ni);

			// Must release since the IntPtr constructor calls Retain
			currentNetworkInfo.DangerousRelease ();
			return StatusCode.OK;
		}

#endif
		[DllImport (Constants.SystemConfigurationLibrary)]
		extern static IntPtr /* CFArrayRef __nullable */ CNCopySupportedInterfaces ();

		/// <summary>Fetch a list of the currently supported network interfaces.</summary>
		/// <param name="supportedInterfaces">The list of supported network interfaces upon return.</param>
		/// <returns><see cref="StatusCode.OK" /> if successful, an error status otherwise.</returns>
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[ObsoletedOSPlatform ("maccatalyst", "Use 'NEHotspotNetwork.FetchCurrent' instead.")]
		[ObsoletedOSPlatform ("ios14.0", "Use 'NEHotspotNetwork.FetchCurrent' instead.")]
		// Not deprecated on macOS
		static public StatusCode TryGetSupportedInterfaces (out string? []? supportedInterfaces)
		{
			IntPtr array = CNCopySupportedInterfaces ();
			if (array == IntPtr.Zero) {
				supportedInterfaces = null;
				return StatusCodeError.SCError ();
			}

			supportedInterfaces = CFArray.StringArrayFromHandle (array, true);
			return StatusCode.OK;
		}

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[ObsoletedOSPlatform ("ios", "Use 'NetworkExtension.NEHotspotHelper' instead.")]
		[ObsoletedOSPlatform ("maccatalyst", "Use 'NetworkExtension.NEHotspotHelper' instead.")]
		// Not deprecated on macOS
		[DllImport (Constants.SystemConfigurationLibrary)]
		extern static byte CNMarkPortalOffline (IntPtr /* CFStringRef __nonnull */ interfaceName);

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[ObsoletedOSPlatform ("ios", "Use 'NetworkExtension.NEHotspotHelper' instead.")]
		[ObsoletedOSPlatform ("maccatalyst", "Use 'NetworkExtension.NEHotspotHelper' instead.")]
		// Not deprecated on macOS
		[DllImport (Constants.SystemConfigurationLibrary)]
		extern static byte CNMarkPortalOnline (IntPtr /* CFStringRef __nonnull */ interfaceName);

		/// <summary>Let the system know that the specified interface has now been authenticated and is now a viable network interface for the rest of the system.</summary>
		/// <param name="iface">The network interface that is now authenticated.</param>
		/// <returns>A boolean value indicating whether the operation was successful or not.</returns>
		/// <remarks>This API is only available on devices. An EntryPointNotFoundException will be thrown on the simulator</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[ObsoletedOSPlatform ("ios", "Use 'NetworkExtension.NEHotspotHelper' instead.")]
		[ObsoletedOSPlatform ("maccatalyst", "Use 'NetworkExtension.NEHotspotHelper' instead.")]
		// Not deprecated on macOS
		static public bool MarkPortalOnline (string iface)
		{
			if (string.IsNullOrEmpty (iface))
				ThrowHelper.ThrowArgumentNullException (nameof (iface));

			using var nss = new TransientCFString (iface);
			return CNMarkPortalOnline (nss) != 0;
		}

		/// <summary>Let the system know that the specified interface has not authenticated and is not a viable network interface for the rest of the system.</summary>
		/// <param name="iface">The network interface that is not authenticated.</param>
		/// <returns>A boolean value indicating whether the operation was successful or not.</returns>
		/// <remarks>This API is only available on devices. An EntryPointNotFoundException will be thrown on the simulator</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[ObsoletedOSPlatform ("ios", "Use 'NetworkExtension.NEHotspotHelper' instead.")]
		[ObsoletedOSPlatform ("maccatalyst", "Use 'NetworkExtension.NEHotspotHelper' instead.")]
		// Not deprecated on macOS
		static public bool MarkPortalOffline (string iface)
		{
			if (string.IsNullOrEmpty (iface))
				ThrowHelper.ThrowArgumentNullException (nameof (iface));

			using var nss = new TransientCFString (iface);
			return CNMarkPortalOffline (nss) != 0;
		}

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[ObsoletedOSPlatform ("ios", "Use 'NetworkExtension.NEHotspotHelper' instead.")]
		[ObsoletedOSPlatform ("maccatalyst", "Use 'NetworkExtension.NEHotspotHelper' instead.")]
		// Not deprecated on macOS
		[DllImport (Constants.SystemConfigurationLibrary)]
		extern static byte CNSetSupportedSSIDs (IntPtr /* CFArrayRef __nonnull */ ssidArray);

		/// <summary>Set an array of SSIDs the application will authenticate.</summary>
		/// <param name="ssids">The array of SSIDs the application will authenticate.</param>
		/// <returns>A boolean value indicating whether the operation was successful or not.</returns>
		/// <remarks>This API is only available on devices. An EntryPointNotFoundException will be thrown on the simulator</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[ObsoletedOSPlatform ("ios", "Use 'NetworkExtension.NEHotspotHelper' instead.")]
		[ObsoletedOSPlatform ("maccatalyst", "Use 'NetworkExtension.NEHotspotHelper' instead.")]
		// Not deprecated on macOS
		static public bool SetSupportedSSIDs (string [] ssids)
		{
			using var arr = NSArray.FromStrings (ssids);
			return CNSetSupportedSSIDs (arr.Handle) != 0;
		}
#endif // __TVOS__
	}
}
