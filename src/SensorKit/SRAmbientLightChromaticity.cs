using System;
using System.Runtime.InteropServices;
using ObjCRuntime;

#nullable enable

namespace SensorKit {
	[SupportedOSPlatform ("ios14.0")]
	[SupportedOSPlatform ("maccatalyst")]
	[UnsupportedOSPlatform ("tvos")]
	[UnsupportedOSPlatform ("macos")]
	[StructLayout (LayoutKind.Sequential)]
	public struct SRAmbientLightChromaticity {
		public float X;
		public float Y;
	}
}
