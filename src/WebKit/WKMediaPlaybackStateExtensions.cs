// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

#nullable enable

#if !__TVOS__

namespace WebKit {
	/// <summary>Converts media playback states between managed and native representations.</summary>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[UnsupportedOSPlatform ("tvos")]
	public static class WKMediaPlaybackStateExtensions {
		/// <summary>Converts a media playback state to its native representation.</summary>
		/// <param name="value">The managed playback state.</param>
		/// <returns>The native playback state.</returns>
		public static nuint ToNative (WKMediaPlaybackState value)
		{
#if !XAMCORE_5_0
			switch (value) {
			case WKMediaPlaybackState.Playing:
				return 1;
			case WKMediaPlaybackState.Paused:
				return 2;
			case WKMediaPlaybackState.Suspended:
				return 3;
			}
#endif
			return (nuint) (ulong) value;
		}

		/// <summary>Converts a native media playback state to its managed representation.</summary>
		/// <param name="value">The native playback state.</param>
		/// <returns>The managed playback state.</returns>
		public static WKMediaPlaybackState ToManaged (nuint value)
		{
#if !XAMCORE_5_0
			switch (value) {
			case 1:
				return WKMediaPlaybackState.Playing;
			case 2:
				return WKMediaPlaybackState.Paused;
			case 3:
				return WKMediaPlaybackState.Suspended;
			}
#endif
			return (WKMediaPlaybackState) (ulong) value;
		}
	}
}

#endif
