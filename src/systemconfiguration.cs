//
// systemconfiguration.cs: Definitions for SystemConfiguration
//
// Authors: 
//  Sebastien Pouliot  <sebastien@xamarin.com>
//
// Copyright 2014 Xamarin Inc.
//

using System;
using Foundation;
using ObjCRuntime;

namespace SystemConfiguration {

	/// <summary>Deprecated: Use <see cref="T:NetworkExtension.NEHotspotHelper" /> instead. A class that encapsulates the Captive Network system component, which is responsible for detecting networks that require user interaction prior to providing Internet access.</summary>
	[Static]
	interface CaptiveNetwork {

		/// <summary>Represents the value associated with the constant kCNNetworkInfoKeyBSSID</summary>
		///         <value>
		///         </value>
		///         <remarks>This API is only available on devices. An EntryPointNotFoundException will be thrown on the simulator</remarks>
		[NoTV]
		[NoMac]
		[MacCatalyst (13, 1)]
		[Field ("kCNNetworkInfoKeyBSSID")]
		NSString NetworkInfoKeyBSSID { get; }

		/// <summary>Represents the value associated with the constant kCNNetworkInfoKeySSID</summary>
		///         <value>
		///         </value>
		///         <remarks>This API is only available on devices. An EntryPointNotFoundException will be thrown on the simulator</remarks>
		[NoTV]
		[NoMac]
		[MacCatalyst (13, 1)]
		[Field ("kCNNetworkInfoKeySSID")]
		NSString NetworkInfoKeySSID { get; }

		/// <summary>Represents the value associated with the constant kCNNetworkInfoKeySSIDData</summary>
		///         <value>
		///         </value>
		///         <remarks>This API is only available on devices. An EntryPointNotFoundException will be thrown on the simulator</remarks>
		[NoTV]
		[NoMac]
		[MacCatalyst (13, 1)]
		[Field ("kCNNetworkInfoKeySSIDData")]
		NSString NetworkInfoKeySSIDData { get; }
	}
}
