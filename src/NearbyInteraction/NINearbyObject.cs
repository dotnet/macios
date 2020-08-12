//
// NearbyInteraction manual bindings
//
// Authors:
//	Whitney Schmidt  <whschm@microsoft.com>
//
// Copyright 2020 Microsoft Inc.
//

using System;
using System.Runtime.InteropServices;

using CoreFoundation;
using Foundation;
using ObjCRuntime;
using Vector3 = global::OpenTK.Vector3;

#if __IOS__
namespace NearbyInteraction {
    partial class NINearbyObject
    {
        static Vector3? _DirectionNotAvailable;

        // TODO: https://github.com/xamarin/maccore/issues/2274
        // We do not have generator support to trampoline Vector3 -> vector_float3 for Fields
        [Field ("NINearbyObjectDirectionNotAvailable",  "NearbyInteraction")]
        public static Vector3 DirectionNotAvailable {
            get {
                if (_DirectionNotAvailable == null) {
                    unsafe {
                        Vector3 *pointer = (Vector3 *) Dlfcn.GetIndirect (Libraries.NearbyInteraction.Handle, "NINearbyObjectDirectionNotAvailable");
                        _DirectionNotAvailable = *pointer;
                    }
                }
                return (Vector3)_DirectionNotAvailable;
            }
        }
    }

}
#endif //__IOS__
