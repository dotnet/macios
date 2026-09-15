// Copyright 2014 Xamarin Inc. All rights reserved.
#if !__MACCATALYST__

#nullable enable

using CoreFoundation;

namespace CoreWlan {
	/// <summary>Encapsulates a CoreWLAN network configuration.</summary>
	public unsafe partial class CWConfiguration {
		/// <summary>Gets the ordered list of preferred network profiles.</summary>
		/// <value>An array of network profiles, or <see langword="null" /> if none are configured.</value>
		public CWNetworkProfile []? NetworkProfiles {
			get {
				NSOrderedSet profiles = _NetworkProfiles;
				return profiles?.ToArray<CWNetworkProfile> ();
			}
		}
	}
}
#endif
