#if IOS
using System;
using Foundation;
using Intents;
using ObjCRuntime;

#nullable enable

namespace Intents {

	public partial class INSetSeatSettingsInCarIntent {
		/// <param name="enableHeating">To be added.</param>
		///         <param name="enableCooling">To be added.</param>
		///         <param name="enableMassage">To be added.</param>
		///         <param name="seat">To be added.</param>
		///         <param name="level">To be added.</param>
		///         <param name="relativeLevelSetting">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[ObsoletedOSPlatform ("ios12.0", "Use the overload that takes 'INSpeakableString carName'.")]
		[ObsoletedOSPlatform ("maccatalyst13.1", "Use the overload that takes 'INSpeakableString carName'.")]
		[UnsupportedOSPlatform ("macos")]
		[UnsupportedOSPlatform ("tvos")]
		public INSetSeatSettingsInCarIntent (bool? enableHeating, bool? enableCooling, bool? enableMassage, INCarSeat seat, NSNumber level, INRelativeSetting relativeLevelSetting) :
			this (enableHeating.HasValue ? new NSNumber (enableHeating.Value) : null, enableCooling.HasValue ? new NSNumber (enableCooling.Value) : null, enableMassage.HasValue ? new NSNumber (enableMassage.Value) : null, seat, level, relativeLevelSetting)
		{
		}
	}
}

#endif
