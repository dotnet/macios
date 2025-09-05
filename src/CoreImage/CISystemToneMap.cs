// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace CoreImage;

partial class CISystemToneMap {
	[SupportedOSPlatform ("ios26.0")]
	[SupportedOSPlatform ("macos26.0")]
	[SupportedOSPlatform ("tvos26.0")]
	[SupportedOSPlatform ("maccatalyst26.0")]
	public CIDynamicRangeOption? PreferredDynamicRange {
		get {
			return ((ICISystemToneMapProtocol) this).PreferredDynamicRange;
		}
		set {
			((ICISystemToneMapProtocol) this).PreferredDynamicRange = value;
		}
	}
}
