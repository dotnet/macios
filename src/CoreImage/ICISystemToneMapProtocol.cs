// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace CoreImage;

partial interface ICISystemToneMapProtocol {
	[SupportedOSPlatform ("ios26.0")]
	[SupportedOSPlatform ("macos26.0")]
	[SupportedOSPlatform ("tvos26.0")]
	[SupportedOSPlatform ("maccatalyst26.0")]
	public CIDynamicRangeOption? PreferredDynamicRange {
		get {
			var value = WeakPreferredDynamicRange;
			if (value is null)
				return null;
			return CIDynamicRangeOptionExtensions.GetValue (value);
		}
		set {
			WeakPreferredDynamicRange = value.HasValue ? value.Value.GetConstant () : null;
		}
	}
}
