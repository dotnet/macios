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

	/// <summary>To be added.</summary>
	public partial class NEHotspotEapSettings {

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
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
