//
// NEHotspotEapSettings.cs
//
// Authors:
//	Alex Soto  <alexsoto@microsoft.com>
//
// Copyright 2017 Xamarin Inc. All rights reserved.
//

#nullable enable

#if !MONOMAC && !TVOS

namespace NetworkExtension {

	/// <summary>Provides EAP settings for configuring hotspot networks.</summary>
	public partial class NEHotspotEapSettings {

		/// <summary>Gets or sets the supported EAP types for hotspot authentication.</summary>
		/// <value>An array of supported EAP authentication types.</value>
		public NEHotspotConfigurationEapType [] SupportedEapTypes {
			get {
				return NSArray.NonNullEnumsFromHandle<NEHotspotConfigurationEapType> (_SupportedEapTypes);
			}
			set {
				if (value is null)
					ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (value));

				var ret = NSArray.From (value, value.Length);
				_SupportedEapTypes = ret.Handle;
				GC.KeepAlive (ret);
			}
		}
	}
}
#endif
