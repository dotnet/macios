// Copyright 2014 Xamarin Inc. All rights reserved.
#if !__MACCATALYST__

#nullable enable

using CoreFoundation;

namespace CoreWlan {
	/// <summary>To be added.</summary>
	public unsafe partial class CWConfiguration {
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		public CWNetworkProfile []? NetworkProfiles {
			get {
				NSOrderedSet profiles = _NetworkProfiles;
				return profiles?.ToArray<CWNetworkProfile> ();
			}
		}
	}
}
#endif
