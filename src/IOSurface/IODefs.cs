//
// IOSurface
//
// Authors: 
//    Miguel de Icaza (miguel@xamarin.com)
//
// Copyright 2017 Microsoft
//

#nullable enable

using System;
using ObjCRuntime;

namespace IOSurface {

	public enum IOSurfaceLockOptions : uint {
		/// <summary>To be added.</summary>
		ReadOnly = 1,
		/// <summary>To be added.</summary>
		AvoidSync = 2,
	}

	public enum IOSurfacePurgeabilityState : uint {
		/// <summary>To be added.</summary>
		NonVolatile = 0,
		/// <summary>To be added.</summary>
		Volatile = 1,
		/// <summary>To be added.</summary>
		Empty = 2,
		/// <summary>To be added.</summary>
		KeepCurrent = 3,
	}

	// To be used with kIOSurfaceCacheMode or IOSurfacePropertyKeyCacheMode
	public enum IOSurfaceMemoryMap {
		/// <summary>To be added.</summary>
		DefaultCache = 0,
		/// <summary>To be added.</summary>
		InhibitCache = 1 << 8,
		/// <summary>To be added.</summary>
		WriteThruCache = 2 << 8,
		/// <summary>To be added.</summary>
		CopybackCache = 3 << 8,
		/// <summary>To be added.</summary>
		WriteCombineCache = 4 << 8,
		/// <summary>To be added.</summary>
		CopybackInnerCache = 5 << 8,
	};


}
