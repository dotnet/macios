//
// Authors:
//   Miguel de Icaza
//
// Copyright 2009, Novell, Inc.
// Copyright 2011-2015 Xamarin, Inc.
//

#if !TVOS && !MONOMAC

using System;
using System.Runtime.InteropServices;
using Foundation;
using ObjCRuntime;

#nullable enable

namespace MediaPlayer {

	// MPVolumeSettings.h
	/// <summary>Encapsulates functions relating to the display or hiding of volume controls.</summary>
	///     <remarks>To be added.</remarks>
	public static class MPVolumeSettings {
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[ObsoletedOSPlatform ("ios11.3", "Use 'MPVolumeView' to present volume controls.")]
		[ObsoletedOSPlatform ("maccatalyst", "Use 'MPVolumeView' to present volume controls.")]
		[DllImport (Constants.MediaPlayerLibrary, EntryPoint = "MPVolumeSettingsAlertShow")]
		public extern static void AlertShow ();

		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
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

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
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
