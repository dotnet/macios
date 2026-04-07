#nullable enable

#if !TVOS
namespace AVKit {
	[SupportedOSPlatform ("ios26.4")]
	[SupportedOSPlatform ("macos26.4")]
	[SupportedOSPlatform ("maccatalyst26.4")]
	[UnsupportedOSPlatform ("tvos")]
	[StructLayout (LayoutKind.Sequential)]
	public struct AVLegibleMediaOptionsMenuState {
#if !COREBUILD
		byte enabled;

		public bool Enabled {
			get => enabled != 0;
			set => enabled = value.AsByte ();
		}

		nint reason;

		public AVLegibleMediaOptionsMenuStateChangeReason Reason {
			get => (AVLegibleMediaOptionsMenuStateChangeReason) (long) reason;
			set => reason = (nint) (long) value;
		}
#endif
	}
}
#endif
