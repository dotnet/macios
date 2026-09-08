#nullable enable

namespace AVFoundation {
	public partial class AVPlayerItem {
		/// <summary>Gets or sets the video aperture mode for the player item.</summary>
		/// <value>The video aperture mode.</value>
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		public AVVideoApertureMode VideoApertureMode {
			get { return AVVideoApertureModeExtensions.GetValue (_VideoApertureMode); }
			set {
				var val = value.GetConstant ();
				if (val is not null)
					_VideoApertureMode = val;
			}
		}
	}
}
