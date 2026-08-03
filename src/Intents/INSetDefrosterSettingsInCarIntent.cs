#if IOS
using Intents;

#nullable enable

namespace Intents {

	public partial class INSetDefrosterSettingsInCarIntent {
		/// <summary>Initializes a new intent to change the defroster settings in a car.</summary>
		/// <param name="enable">Whether to enable the defroster, or <see langword="null" /> if unchanged.</param>
		/// <param name="defroster">The defroster to configure.</param>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[ObsoletedOSPlatform ("ios12.0", "Use the overload that takes 'INSpeakableString carName'.")]
		[ObsoletedOSPlatform ("maccatalyst13.1", "Use the overload that takes 'INSpeakableString carName'.")]
		[UnsupportedOSPlatform ("macos")]
		[UnsupportedOSPlatform ("tvos")]
		public INSetDefrosterSettingsInCarIntent (bool? enable, INCarDefroster defroster) :
			this (enable.HasValue ? new NSNumber (enable.Value) : null, defroster)
		{
		}
	}
}

#endif
