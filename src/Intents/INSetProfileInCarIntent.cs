#if IOS
using Intents;
using UIKit;

#nullable enable

namespace Intents {

	public partial class INSetProfileInCarIntent {
		/// <param name="profileNumber">The number identifying the car profile.</param>
		///         <param name="profileLabel">The display label for the car profile.</param>
		///         <param name="defaultProfile">Whether this is the default car profile.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[ObsoletedOSPlatform ("ios12.0", "Use the overload that takes 'INSpeakableString carName'.")]
		[ObsoletedOSPlatform ("maccatalyst13.1", "Use the overload that takes 'INSpeakableString carName'.")]
		[UnsupportedOSPlatform ("macos")]
		[UnsupportedOSPlatform ("tvos")]
		public INSetProfileInCarIntent (NSNumber profileNumber, string profileLabel, bool? defaultProfile) :
			this (profileNumber, profileLabel, defaultProfile.HasValue ? new NSNumber (defaultProfile.Value) : null)
		{
		}
	}
}

#endif
