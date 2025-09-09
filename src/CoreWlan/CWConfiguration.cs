// Copyright 2014 Xamarin Inc. All rights reserved.
#if !__MACCATALYST__

#nullable enable

using Foundation;
using CoreFoundation;
using ObjCRuntime;
using System;

namespace CoreWlan {
	/// <summary>To be added.</summary>
	///     <remarks>To be added.</remarks>
	public unsafe partial class CWConfiguration {
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public CWNetworkProfile []? NetworkProfiles {
			get {
				NSOrderedSet profiles = _NetworkProfiles;
				return profiles?.ToArray<CWNetworkProfile> ();
			}
		}
	}
}
#endif
