using CoreMedia;

#nullable enable

namespace AVFoundation {
	/// <summary>Provides data for the  event.</summary>
	///     <remarks>
	///     </remarks>
	[SupportedOSPlatform ("tvos")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	public partial class AudioRendererWasFlushedAutomaticallyEventArgs {
		/// <summary>Gets the time at which the audio renderer was flushed.</summary>
		/// <value>The presentation time at which the flush occurred.</value>
		public CMTime AudioRendererFlushTime {
			get {
				return _AudioRendererFlushTime.CMTimeValue;
			}
		}
	}
}
