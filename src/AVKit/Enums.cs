using XamCore.ObjCRuntime;

namespace XamCore.AVKit {

#if !XAMCORE_3_0 || MONOMAC
	// this enum only exists for OSX (not iOS)
	[Native]
	public enum AVPlayerViewControlsStyle : nint {
		None,
		Inline,
		Floating,
		Minimal,
		Default = Inline 
	}
#endif

#if MONOMAC
	[Mac (10,10)]
	[Native]
	public enum AVCaptureViewControlsStyle : nint {
		Inline,
		Floating,
		InlineDeviceSelection,
		Default = Inline,
	}

	[Mac (10,9)]
	[Native]
	public enum AVPlayerViewTrimResult : nint {
		OKButton,
		CancelButton
	}
#endif

#if !TVOS && (!MONOMAC || !XAMCORE_4_0)
	[iOS (9,0)]
	[Native]
	[ErrorDomain ("AVKitErrorDomain")]
	public enum AVKitError : nint {
		None = 0,
		Unknown = -1000,
		PictureInPictureStartFailed = -1001
	}
#endif
}	
