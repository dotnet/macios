// Copyright 2014 Xamarin Inc. All rights reserved.
#if !__MACCATALYST__
using Foundation;
using CoreFoundation;
using ObjCRuntime;
using System;

namespace CoreWlan {
	public unsafe partial class CWConfiguration {
		public CWNetworkProfile [] NetworkProfiles {
			get {
				NSOrderedSet profiles = _NetworkProfiles;
				if (profiles != null)
					return profiles.ToArray<CWNetworkProfile> ();
				return null;
			}
		}
	}
}
#endif
