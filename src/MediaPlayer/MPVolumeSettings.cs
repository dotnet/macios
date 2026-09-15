//
// Authors:
//   Miguel de Icaza
//
// Copyright 2009, Novell, Inc.
// Copyright 2011-2015 Xamarin, Inc.
//

#if !TVOS && !MONOMAC

#nullable enable

namespace MediaPlayer {

	// MPVolumeSettings.h
	/// <summary>Encapsulates functions relating to the display or hiding of volume controls.</summary>
	public static class MPVolumeSettings {
		/// <summary>Displays the volume settings alert.</summary>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[ObsoletedOSPlatform ("ios11.3", "Use 'MPVolumeView' to present volume controls.")]
		[ObsoletedOSPlatform ("maccatalyst", "Use 'MPVolumeView' to present volume controls.")]
		[DllImport (Constants.MediaPlayerLibrary, EntryPoint = "MPVolumeSettingsAlertShow")]
		public extern static void AlertShow ();

		/// <summary>Hides the volume settings alert.</summary>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[ObsoletedOSPlatform ("ios11.3", "Use 'MPVolumeView' to present volume controls.")]
		[ObsoletedOSPlatform ("maccatalyst", "Use 'MPVolumeView' to present volume controls.")]
		[DllImport (Constants.MediaPlayerLibrary, EntryPoint = "MPVolumeSettingsAlertHide")]
		public extern static void AlertHide ();

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[ObsoletedOSPlatform ("ios11.3", "Use 'MPVolumeView' to present volume controls.")]
		[ObsoletedOSPlatform ("maccatalyst", "Use 'MPVolumeView' to present volume controls.")]
		[DllImport (Constants.MediaPlayerLibrary)]
		extern static /* BOOL */ byte MPVolumeSettingsAlertIsVisible ();

		/// <summary>Returns a value indicating whether the volume settings alert is currently visible.</summary>
		/// <returns><see langword="true" /> if the volume settings alert is visible; otherwise, <see langword="false" />.</returns>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[ObsoletedOSPlatform ("ios11.3", "Use 'MPVolumeView' to present volume controls.")]
		[ObsoletedOSPlatform ("maccatalyst", "Use 'MPVolumeView' to present volume controls.")]
		public static bool AlertIsVisible ()
		{
			return MPVolumeSettingsAlertIsVisible () != 0;
		}
	}
}

#endif
