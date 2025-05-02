using ObjCRuntime;
using Foundation;

#nullable enable

namespace AVKit {

#if MONOMAC
	// this enum only exists for OSX (not iOS)
	/// <summary>Determines which user interface elements are visible in the AVPlayerView</summary>
	[Native]
	public enum AVPlayerViewControlsStyle : long {
		/// <summary>Do not show any controls.</summary>
		None,
		/// <summary>Use inline controls.</summary>
		Inline,
		/// <summary>Use floating cnotrols.</summary>
		Floating,
		/// <summary>Use minimal controls.</summary>
		Minimal,
		/// <summary>Use the default style (Inline)</summary>
		Default = Inline,
	}
#endif

	// The version of the AVError.h header file in the tvOS SDK is much newer than in the iOS SDKs,
	// (copyright 2016 vs 2019), so this is reflecting the tvOS SDK.
	/// <summary>Enumeration of error states that can occur while using AVKit.</summary>
	/// <remarks>To be added.</remarks>
	[TV (13, 0)]
#if NET
	[NoMac]
	[MacCatalyst (13, 1)]
#endif
	[Native]
	[ErrorDomain ("AVKitErrorDomain")]
	public enum AVKitError : long {
		/// <summary>To be added.</summary>
		None = 0,
		/// <summary>To be added.</summary>
		Unknown = -1000,
		/// <summary>To be added.</summary>
		PictureInPictureStartFailed = -1001,
		ContentRatingUnknown = -1100,
		ContentDisallowedByPasscode = -1101,
		ContentDisallowedByProfile = -1102,
		RecordingFailed = -1200,
	}

	[NoTV]
	[NoMac]
	[iOS (13, 0)]
	[MacCatalyst (13, 1)]
	[Native]
	public enum AVAudioSessionRouteSelection : long {
		None = 0,
		Local = 1,
		External = 2,
	}

	[NoiOS]
	[NoTV]
	[NoMacCatalyst]
	[Native]
	public enum AVRoutePickerViewButtonState : long {
		Normal,
		NormalHighlighted,
		Active,
		ActiveHighlighted,
	}

}
