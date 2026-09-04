
#nullable enable

namespace SensorKit {
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[UnsupportedOSPlatform ("tvos")]
	[UnsupportedOSPlatform ("macos")]
	[StructLayout (LayoutKind.Sequential)]
	public struct SRAmbientLightChromaticity {
		public float X;
		public float Y;
	}
}
