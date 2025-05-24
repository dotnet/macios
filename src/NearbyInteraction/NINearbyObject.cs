//
// NearbyInteraction manual bindings
//
// Authors:
//	Whitney Schmidt  <whschm@microsoft.com>
//
// Copyright 2020 Microsoft Inc.
//

#nullable enable

using System;
using System.Numerics;
using System.Runtime.InteropServices;

using CoreFoundation;
using Foundation;
using ObjCRuntime;

using MatrixFloat4x4 = global::CoreGraphics.NMatrix4;

#if __IOS__
namespace NearbyInteraction {
	partial class NINearbyObject {
		static Vector3? _DirectionNotAvailable;

		// TODO: https://github.com/xamarin/maccore/issues/2274
		// We do not have generator support to trampoline Vector3 -> vector_float3 for Fields
		[Field ("NINearbyObjectDirectionNotAvailable", "NearbyInteraction")]
		public static Vector3 DirectionNotAvailable {
			get {
				if (_DirectionNotAvailable is null) {
					unsafe {
						Vector3* pointer = (Vector3*) Dlfcn.GetIndirect (Libraries.NearbyInteraction.Handle, "NINearbyObjectDirectionNotAvailable");
						_DirectionNotAvailable = *pointer;
					}
				}
				return (Vector3) _DirectionNotAvailable;
			}
		}

		static MatrixFloat4x4? _WorldTransformNotAvailable;

		[SupportedOSPlatform ("ios16.0")]
		[SupportedOSPlatform ("maccatalyst16.0")]
		[UnsupportedOSPlatform ("tvos")]
		[UnsupportedOSPlatform ("macos")]
		// Following similar strategy found here: https://github.com/xamarin/maccore/issues/2274
		[Field ("NINearbyObjectWorldTransformNotAvailable", "NearbyInteraction")]
		public static MatrixFloat4x4 WorldTransformNotAvailable {
			get {
				if (_WorldTransformNotAvailable is null) {
					unsafe {
						MatrixFloat4x4* pointer = (MatrixFloat4x4*) Dlfcn.GetIndirect (Libraries.NearbyInteraction.Handle, "NINearbyObjectWorldTransformNotAvailable");
						if (pointer is null)
							throw new PlatformNotSupportedException ("This property is not supported on this version of the OS");
						_WorldTransformNotAvailable = *pointer;
					}
				}
				return (MatrixFloat4x4) _WorldTransformNotAvailable;
			}
		}
	}
}
#endif
