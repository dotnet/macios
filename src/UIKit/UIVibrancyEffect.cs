// Copyright 2014 Xamarin, Inc.
#if IOS || TVOS
#if HAS_NOTIFICATIONCENTER
using NotificationCenter;
#endif

#nullable enable

namespace UIKit {

	public partial class UIVibrancyEffect {

#if HAS_NOTIFICATIONCENTER
		// This code comes from NotificationCenter
		// This is a [Category] -> C# extension method (see adlib.cs) but it targets on static selector
		// the resulting syntax does not look good in user code so we provide a better looking API
		// https://trello.com/c/iQpXOxCd/227-category-and-static-methods-selectors
		// note: we cannot reuse the same method name - as it would break compilation of existing apps
		/// <summary>Developers should not use this deprecated method. Use <see cref="CreatePrimaryVibrancyEffectForNotificationCenter" /> instead.</summary>
		/// <returns>A <see cref="UIVibrancyEffect" /> for the notification center.</returns>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		[ObsoletedOSPlatform ("ios10.0", "Use 'CreatePrimaryVibrancyEffectForNotificationCenter' instead.")]
		[ObsoletedOSPlatform ("tvos", "Use 'CreatePrimaryVibrancyEffectForNotificationCenter' instead.")]
		[ObsoletedOSPlatform ("maccatalyst", "Use 'CreatePrimaryVibrancyEffectForNotificationCenter' instead.")]
		static public UIVibrancyEffect CreateForNotificationCenter ()
		{
			return (null as UIVibrancyEffect)!.NotificationCenterVibrancyEffect ();
		}

		/// <summary>Returns the primary vibrancy effect for use with the notification center.</summary>
		/// <returns>A primary <see cref="UIVibrancyEffect" /> for the notification center.</returns>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		[ObsoletedOSPlatform ("ios13.0", "Use 'UIVibrancyEffect.CreateWidgetEffectForNotificationCenter' instead.")]
		[ObsoletedOSPlatform ("tvos13.0", "Use 'UIVibrancyEffect.CreateWidgetEffectForNotificationCenter' instead.")]
		[ObsoletedOSPlatform ("maccatalyst", "Use 'UIVibrancyEffect.CreateWidgetEffectForNotificationCenter' instead.")]
		static public UIVibrancyEffect CreatePrimaryVibrancyEffectForNotificationCenter ()
		{
			return (null as UIVibrancyEffect)!.GetWidgetPrimaryVibrancyEffect ();
		}

		/// <summary>Returns the secondary vibrancy effect for use with the notification center.</summary>
		/// <returns>A secondary <see cref="UIVibrancyEffect" /> for the notification center.</returns>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		[ObsoletedOSPlatform ("ios13.0", "Use 'UIVibrancyEffect.CreateWidgetEffectForNotificationCenter' instead.")]
		[ObsoletedOSPlatform ("tvos13.0", "Use 'UIVibrancyEffect.CreateWidgetEffectForNotificationCenter' instead.")]
		[ObsoletedOSPlatform ("maccatalyst", "Use 'UIVibrancyEffect.CreateWidgetEffectForNotificationCenter' instead.")]
		static public UIVibrancyEffect CreateSecondaryVibrancyEffectForNotificationCenter ()
		{
			return (null as UIVibrancyEffect)!.GetWidgetSecondaryVibrancyEffect ();
		}

		/// <summary>Returns a vibrancy effect with the specified style for use with the notification center.</summary>
		/// <param name="vibrancyStyle">The vibrancy effect style to use.</param>
		/// <returns>A <see cref="UIVibrancyEffect" /> with the specified style for the notification center.</returns>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		static public UIVibrancyEffect CreateWidgetEffectForNotificationCenter (UIVibrancyEffectStyle vibrancyStyle)
		{
#pragma warning disable CA1422 // This call site is reachable on: 'ios' 15.0 and later, 'maccatalyst' 15.0 and later. 'UIVibrancyEffect_NCWidgetAdditions.GetWidgetEffect(UIVibrancyEffect, UIVibrancyEffectStyle)' is obsoleted on: 'ios' 14.0 and later, 'maccatalyst' 14.0 and later.
			return (null as UIVibrancyEffect)!.GetWidgetEffect (vibrancyStyle);
#pragma warning restore CA1422
		}
#endif // HAS_NOTIFICATIONCENTER
	}
}
#endif // IOS || TVOS
