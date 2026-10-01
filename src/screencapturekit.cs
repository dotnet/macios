//
// ScreenCaptureKit bindings
//
// Authors:
//	Alex Soto  <alexsoto@microsoft.com>
//
// Copyright (C) Microsoft Corporation. All rights reserved.
//

using System.ComponentModel;
using AVFoundation;
using CoreVideo;
using CoreGraphics;
using CoreFoundation;
using CoreMedia;
using UniformTypeIdentifiers;

#if MONOMAC
using AppKit;
#else
using UIKit;
#endif

#if !MONOMAC
using NSWindow = System.Object;
#else
using UIWindowScene = System.Object;
#endif

namespace ScreenCaptureKit {

	[NoiOS, NoTV, Mac (26, 0), MacCatalyst (26, 0)]
	[Native]
	public enum SCScreenshotDisplayIntent : long {
		Canonical,
		Local,
	}

	[NoiOS, NoTV, Mac (26, 0), MacCatalyst (26, 0)]
	[Native]
	public enum SCScreenshotDynamicRange : long {
		Sdr,
		Hdr,
		SdrAndHdr,
	}

	[NoiOS, NoTV, Mac (26, 0), MacCatalyst (26, 0)]
	[BaseType (typeof (NSObject))]
	interface SCScreenshotConfiguration {
		[Export ("width")]
		nint Width { get; set; }

		[Export ("height")]
		nint Height { get; set; }

		[Export ("showsCursor")]
		bool ShowsCursor { get; set; }

		[Export ("sourceRect", ArgumentSemantic.Assign)]
		CGRect SourceRect { get; set; }

		[Export ("destinationRect", ArgumentSemantic.Assign)]
		CGRect DestinationRect { get; set; }

		[Export ("ignoreShadows")]
		bool IgnoreShadows { get; set; }

		[Export ("ignoreClipping")]
		bool IgnoreClipping { get; set; }

		[Export ("includeChildWindows")]
		bool IncludeChildWindows { get; set; }

		[Export ("displayIntent", ArgumentSemantic.Assign)]
		SCScreenshotDisplayIntent DisplayIntent { get; set; }

		[Export ("dynamicRange", ArgumentSemantic.Assign)]
		SCScreenshotDynamicRange DynamicRange { get; set; }

		[Export ("contentType", ArgumentSemantic.Assign)]
		UTType ContentType { get; set; }

		[NullAllowed, Export ("fileURL", ArgumentSemantic.Strong)]
		NSUrl FileUrl { get; set; }

		[Static]
		[Export ("supportedContentTypes")]
		UTType [] SupportedContentTypes { get; }
	}

	[NoiOS, NoTV, Mac (26, 0), MacCatalyst (26, 0)]
	[BaseType (typeof (NSObject))]
	interface SCScreenshotOutput {
		[NullAllowed, Export ("sdrImage", ArgumentSemantic.Strong)]
		CGImage SdrImage { get; set; }

		[NullAllowed, Export ("hdrImage", ArgumentSemantic.Strong)]
		CGImage HdrImage { get; set; }

		[NullAllowed, Export ("fileURL", ArgumentSemantic.Assign)]
		NSUrl FileUrl { get; set; }
	}

	[UnsupportedSimulator ("ios")]
	[UnsupportedSimulator ("tvos")]
	[iOS (27, 0), TV (27, 0), MacCatalyst (18, 2)]
	[ErrorDomain ("SCStreamErrorDomain")]
	[Native]
	enum SCStreamErrorCode : long {
		UserDeclined = -3801,
		FailedToStart = -3802,
		MissingEntitlements = -3803,
		FailedApplicationConnectionInvalid = -3804,
		FailedApplicationConnectionInterrupted = -3805,
		FailedNoMatchingApplicationContext = -3806,
		AttemptToStartStreamState = -3807,
		AttemptToStopStreamState = -3808,
		AttemptToUpdateFilterState = -3809,
		AttemptToConfigState = -3810,
		InternalError = -3811,
		InvalidParameter = -3812,
		NoWindowList = -3813,
		NoDisplayList = -3814,
		NoCaptureSource = -3815,
		RemovingStream = -3816,
		UserStopped = -3817,
		FailedToStartAudioCapture = -3818,
		FailedToStopAudioCapture = -3819,
		FailedToStartMicrophoneCapture = -3820,
		SystemStoppedStream = -3821,
		InsufficientStorage = -3822,
		NotSupported = -3823,
		MissingBackgroundMode = -3824,
	}

	[UnsupportedSimulator ("ios")]
	[UnsupportedSimulator ("tvos")]
	[iOS (27, 0), TV (27, 0), MacCatalyst (18, 2)]
	[Native]
	enum SCFrameStatus : long {
		Complete,
		Idle,
		Blank,
		Suspended,
		Started,
		Stopped,
	}

	[UnsupportedSimulator ("ios")]
	[UnsupportedSimulator ("tvos")]
	[iOS (27, 0), TV (27, 0), MacCatalyst (18, 2)]
	[Native]
	enum SCStreamOutputType : long {
		Screen,
		Audio,
		[NoTV, Mac (15, 0)]
		Microphone,
	}

	[Deprecated (PlatformName.MacOSX, 15, 0, message: "Use 'SCShareableContentStyle' instead.")]
	[NoiOS, NoTV, NoMacCatalyst]
	[Native]
	public enum SCStreamType : long {
		Window,
		Display,
	}

	[NoiOS, NoTV, MacCatalyst (18, 2)]
	[Native]
	public enum SCPresenterOverlayAlertSetting : long {
		System,
		Never,
		Always,
	}

	[NoiOS, NoTV, MacCatalyst (18, 2)]
	[Native]
	public enum SCCaptureResolutionType : long {
		Automatic,
		Best,
		Nominal,
	}

	[Flags, NoiOS, NoTV, MacCatalyst (18, 2)]
	[Native]
	public enum SCContentSharingPickerMode : ulong {
		SingleWindow = 1 << 0,
		MultipleWindows = 1 << 1,
		SingleApplication = 1 << 2,
		MultipleApplications = 1 << 3,
		SingleDisplay = 1 << 4,
	}

	[UnsupportedSimulator ("ios")]
	[UnsupportedSimulator ("tvos")]
	[iOS (27, 0), TV (27, 0), MacCatalyst (18, 2)]
	[Native]
	public enum SCShareableContentStyle : long {
		None,
		Window,
		[NoTV]
		Display,
		[NoiOS, NoTV, MacCatalyst (18, 2)]
		Application,
	}

	[UnsupportedSimulator ("ios")]
	[NoTV, iOS (27, 0), Mac (15, 0), MacCatalyst (18, 2)]
	[Native]
	public enum SCCaptureDynamicRange : long {
		Sdr,
		[NoiOS]
		HdrLocalDisplay,
		HdrCanonicalDisplay,
	}

	[UnsupportedSimulator ("ios")]
	[NoTV, iOS (27, 0), Mac (15, 0), MacCatalyst (18, 2)]
	[Native]
	public enum SCStreamConfigurationPreset : long {
		[NoiOS]
		CaptureHdrStreamLocalDisplay,
		[NoiOS]
		CaptureHdrStreamCanonicalDisplay,
		[NoiOS]
		CaptureHdrScreenshotLocalDisplay,
		[NoiOS]
		CaptureHdrScreenshotCanonicalDisplay,
		[iOS (27, 0), MacCatalyst (26, 0), Mac (26, 0)]
		CaptureHdrRecordingPreservedSdrHdr10,
	}

	[UnsupportedSimulator ("tvos")]
	[NoiOS, NoMac, NoMacCatalyst, TV (27, 0)]
	[Native]
	public enum SCRecordingEditorMode : long {
		Preview,
		Share,
	}

	[UnsupportedSimulator ("ios")]
	[UnsupportedSimulator ("tvos")]
	[iOS (27, 0), TV (27, 0), MacCatalyst (18, 2)]
	[Static]
	interface SCStreamFrameInfoKeys {

		[Field ("SCStreamFrameInfoStatus")]
		NSString Status { get; }

		[Field ("SCStreamFrameInfoDisplayTime")]
		NSString DisplayTime { get; }

		[Field ("SCStreamFrameInfoScaleFactor")]
		NSString InfoScaleFactor { get; }

		[Field ("SCStreamFrameInfoContentScale")]
		NSString ContentScale { get; }

		[Field ("SCStreamFrameInfoContentRect")]
		NSString ContentRect { get; }

		[NoiOS, NoTV, MacCatalyst (18, 2)]
		[Field ("SCStreamFrameInfoDirtyRects")]
		NSString DirtyRects { get; }

		[Field ("SCStreamFrameInfoScreenRect")]
		NSString ScreenRect { get; }

		[NoiOS, NoTV, MacCatalyst (18, 2)]
		[Field ("SCStreamFrameInfoBoundingRect")]
		NSString BoundingRect { get; }

		[NoiOS, NoTV, Mac (14, 2), MacCatalyst (18, 2)]
		[Field ("SCStreamFrameInfoPresenterOverlayContentRect")]
		NSString PresenterOverlayContentRect { get; }

		[NoTV, iOS (27, 0), Mac (27, 0), MacCatalyst (27, 0)]
		[Field ("SCStreamFrameInfoVideoOrientation")]
		NSString VideoOrientation { get; }
	}

	[NoiOS, NoTV, MacCatalyst (18, 2)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface SCRunningApplication {

		[Export ("bundleIdentifier")]
		string BundleIdentifier { get; }

		[Export ("applicationName")]
		string ApplicationName { get; }

		[Export ("processID")]
		int ProcessId { get; }
	}

	[NoiOS, NoTV, MacCatalyst (18, 2)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface SCWindow {

		[Export ("windowID")]
		uint WindowId { get; }

		[Export ("frame")]
		CGRect Frame { get; }

		[NullAllowed, Export ("title")]
		string Title { get; }

		[Export ("windowLayer")]
		nint WindowLayer { get; }

		[NullAllowed, Export ("owningApplication")]
		SCRunningApplication OwningApplication { get; }

		[Export ("onScreen")]
		bool OnScreen { [Bind ("isOnScreen")] get; }

		[Export ("active")]
		bool Active { [Bind ("isActive")] get; }
	}


	[NoiOS, NoTV, MacCatalyst (18, 2)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface SCDisplay {

		[Export ("displayID")]
		uint DisplayId { get; }

		[Export ("width")]
		nint Width { get; }

		[Export ("height")]
		nint Height { get; }

		[Export ("frame")]
		CGRect Frame { get; }
	}

	[NoiOS, NoTV, MacCatalyst (18, 2)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface SCShareableContent {

		[Async]
		[Static]
		[Export ("getShareableContentWithCompletionHandler:")]
		void GetShareableContent (Action<SCShareableContent, NSError> completionHandler);

		[Mac (14, 4)]
		[Async]
		[Static]
		[Export ("getCurrentProcessShareableContentWithCompletionHandler:")]
		void GetCurrentProcessShareableContent (Action<SCShareableContent, NSError> completionHandler);

		[Async]
		[Static]
		[Export ("getShareableContentExcludingDesktopWindows:onScreenWindowsOnly:completionHandler:")]
		void GetShareableContent (bool excludeDesktopWindows, bool onScreenWindowsOnly, Action<SCShareableContent, NSError> completionHandler);

		[Async]
		[Static]
		[Export ("getShareableContentExcludingDesktopWindows:onScreenWindowsOnlyBelowWindow:completionHandler:")]
		void GetShareableContentBelowWindow (bool excludeDesktopWindows, SCWindow onScreenWindowsOnlyBelowWindow, Action<SCShareableContent, NSError> completionHandler);

		[Async]
		[Static]
		[Export ("getShareableContentExcludingDesktopWindows:onScreenWindowsOnlyAboveWindow:completionHandler:")]
		void GetShareableContentAboveWindow (bool excludeDesktopWindows, SCWindow onScreenWindowsOnlyAboveWindow, Action<SCShareableContent, NSError> completionHandler);

		[Export ("windows")]
		SCWindow [] Windows { get; }

		[Export ("displays")]
		SCDisplay [] Displays { get; }

		[Export ("applications")]
		SCRunningApplication [] Applications { get; }

		[Static]
		[Export ("infoForFilter:")]
		SCShareableContentInfo GetInfo (SCContentFilter filter);
	}

	[UnsupportedSimulator ("ios")]
	[UnsupportedSimulator ("tvos")]
	[iOS (27, 0), TV (27, 0), MacCatalyst (18, 2)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface SCContentFilter {

		[NoiOS, NoTV, MacCatalyst (18, 2)]
		[Export ("initWithDesktopIndependentWindow:")]
		NativeHandle Constructor (SCWindow window);

		[Internal]
		[NoiOS, NoTV, MacCatalyst (18, 2)]
		[Export ("initWithDisplay:excludingWindows:")]
		NativeHandle _InitWithDisplayExcludingWindows (SCDisplay display, SCWindow [] excludedWindows);

		[Internal]
		[NoiOS, NoTV, MacCatalyst (18, 2)]
		[Export ("initWithDisplay:includingWindows:")]
		NativeHandle _InitWithDisplayIncludingWindows (SCDisplay display, SCWindow [] includedWindows);

		[Internal]
		[NoiOS, NoTV, MacCatalyst (18, 2)]
		[Export ("initWithDisplay:includingApplications:exceptingWindows:")]
		NativeHandle _InitWithDisplayIncludingApplications (SCDisplay display, SCRunningApplication [] includingApplications, SCWindow [] exceptingWindows);

		[Internal]
		[NoiOS, NoTV, MacCatalyst (18, 2)]
		[Export ("initWithDisplay:excludingApplications:exceptingWindows:")]
		NativeHandle _InitWithDisplayExcludingApplications (SCDisplay display, SCRunningApplication [] excludingApplications, SCWindow [] exceptingWindows);

		// per docs, the following selectors are available for 12.3+
		// but return types are SCStreamType and SCShareableContentStyle are 14.0+
		[Deprecated (PlatformName.MacOSX, 14, 2, message: "Use 'Style' instead.")]
		[NoiOS, NoTV, NoMacCatalyst]
		[Export ("streamType")]
		SCStreamType StreamType { get; }

		[Export ("style")]
		SCShareableContentStyle Style { get; }

		[Export ("pointPixelScale")]
		float PointPixelScale { get; }

		[Export ("contentRect")]
		CGRect ContentRect { get; }

		[NoiOS, NoTV, Mac (14, 2), MacCatalyst (18, 2)]
		[Export ("includeMenuBar")]
		bool IncludeMenuBar { get; set; }

		[NoiOS, NoTV, Mac (15, 2), MacCatalyst (18, 2)]
		[Export ("includedDisplays")]
		SCDisplay [] IncludedDisplays { get; }

		[NoiOS, NoTV, Mac (15, 2), MacCatalyst (18, 2)]
		[Export ("includedApplications")]
		SCRunningApplication [] IncludedApplications { get; }

		[NoiOS, NoTV, Mac (15, 2), MacCatalyst (18, 2)]
		[Export ("includedWindows")]
		SCWindow [] IncludedWindows { get; }

		[NoTV, iOS (27, 0), Mac (27, 0), MacCatalyst (27, 0)]
		[Export ("microphoneEnabled")]
		bool MicrophoneEnabled { [Bind ("isMicrophoneEnabled")] get; }

		[NoTV, NoMac, NoMacCatalyst, iOS (27, 0)]
		[Export ("cameraEnabled")]
		bool CameraEnabled { [Bind ("isCameraEnabled")] get; }
	}

	[UnsupportedSimulator ("ios")]
	[UnsupportedSimulator ("tvos")]
	[iOS (27, 0), TV (27, 0), MacCatalyst (18, 2)]
	[BaseType (typeof (NSObject))]
	interface SCStreamConfiguration {

		[Export ("width")]
		nuint Width { get; set; }

		[Export ("height")]
		nuint Height { get; set; }

		[NoiOS, NoTV, MacCatalyst (18, 2)]
		[Export ("minimumFrameInterval", ArgumentSemantic.Assign)]
		CMTime MinimumFrameInterval { get; set; }

		[NoiOS, NoTV, MacCatalyst (18, 2)]
		[Export ("pixelFormat")]
		CVPixelFormatType PixelFormat { get; set; }

		[NoiOS, NoTV, MacCatalyst (18, 2)]
		[Export ("scalesToFit")]
		bool ScalesToFit { get; set; }

		[NoiOS, NoTV, MacCatalyst (18, 2)]
		[Export ("showsCursor")]
		bool ShowsCursor { get; set; }

		[NoiOS, NoTV, MacCatalyst (18, 2)]
		[Export ("backgroundColor", ArgumentSemantic.Assign)]
		CGColor BackgroundColor { get; set; }

		[NoiOS, NoTV, MacCatalyst (18, 2)]
		[Export ("sourceRect", ArgumentSemantic.Assign)]
		CGRect SourceRect { get; set; }

		[NoiOS, NoTV, MacCatalyst (18, 2)]
		[Export ("destinationRect", ArgumentSemantic.Assign)]
		CGRect DestinationRect { get; set; }

		[NoiOS, NoTV, MacCatalyst (18, 2)]
		[Export ("queueDepth")]
		nint QueueDepth { get; set; }

		// Usign weak prefix in case we want to strong-type these puppies in the future.

		[Advice ("Use the constants inside 'CGDisplayStreamYCbCrMatrixOptionKeys' class.")]
		[NoiOS, NoTV, MacCatalyst (18, 2)]
		[Export ("colorMatrix", ArgumentSemantic.Assign)]
		NSString WeakColorMatrix { get; set; }

		[Advice ("Use the constants inside 'CGColorSpaceNames' class.")]
		[NoiOS, NoTV, MacCatalyst (18, 2)]
		[Export ("colorSpaceName", ArgumentSemantic.Assign)]
		NSString WeakColorSpaceName { get; set; }

		[Export ("capturesAudio")]
		bool CapturesAudio { get; set; }

		[Export ("sampleRate")]
		nint SampleRate { get; set; }

		[Export ("channelCount")]
		nint ChannelCount { get; set; }

		[Export ("excludesCurrentProcessAudio")]
		bool ExcludesCurrentProcessAudio { get; set; }

		[NoiOS, NoTV, MacCatalyst (18, 2)]
		[Export ("preservesAspectRatio")]
		bool PreservesAspectRatio { get; set; }

		[NoiOS, NoTV, MacCatalyst (18, 2)]
		[NullAllowed]
		[Export ("streamName", ArgumentSemantic.Strong)]
		string StreamName { get; set; }

		[NoiOS, NoTV, MacCatalyst (18, 2)]
		[Export ("ignoreShadowsDisplay")]
		bool IgnoreShadowsDisplay { get; set; }

		[NoiOS, NoTV, MacCatalyst (18, 2)]
		[Export ("ignoreShadowsSingleWindow")]
		bool IgnoreShadowsSingleWindow { get; set; }

		[NoiOS, NoTV, MacCatalyst (18, 2)]
		[Export ("captureResolution", ArgumentSemantic.Assign)]
		SCCaptureResolutionType CaptureResolution { get; set; }

		[NoiOS, NoTV, MacCatalyst (18, 2)]
		[Export ("capturesShadowsOnly")]
		bool CapturesShadowsOnly { get; set; }

		[NoiOS, NoTV, MacCatalyst (18, 2)]
		[Export ("shouldBeOpaque")]
		bool ShouldBeOpaque { get; set; }

		[NoiOS, NoTV, MacCatalyst (18, 2)]
		[Export ("ignoreGlobalClipDisplay")]
		bool IgnoreGlobalClipDisplay { get; set; }

		[NoiOS, NoTV, MacCatalyst (18, 2)]
		[Export ("ignoreGlobalClipSingleWindow")]
		bool IgnoreGlobalClipSingleWindow { get; set; }

		[NoiOS, NoTV, MacCatalyst (18, 2)]
		[Export ("presenterOverlayPrivacyAlertSetting", ArgumentSemantic.Assign)]
		SCPresenterOverlayAlertSetting PresenterOverlayPrivacyAlertSetting { get; set; }

		[NoiOS, NoTV, Mac (14, 2), MacCatalyst (18, 2)]
		[Export ("includeChildWindows")]
		bool IncludeChildWindows { get; set; }

		[NoiOS, NoTV, Mac (15, 0), MacCatalyst (18, 2)]
		[Export ("showMouseClicks", ArgumentSemantic.Assign)]
		bool ShowMouseClicks { get; set; }

		[NoiOS, NoTV, Mac (15, 0), MacCatalyst (18, 2)]
		[Export ("captureMicrophone", ArgumentSemantic.Assign)]
		bool CaptureMicrophone { get; set; }

		[NoiOS, NoTV, NoMacCatalyst, Mac (15, 0)]
		[Export ("microphoneCaptureDeviceID", ArgumentSemantic.Strong), NullAllowed]
		string MicrophoneCaptureDeviceId { get; set; }

		[NoTV, iOS (27, 0), Mac (15, 0), MacCatalyst (18, 2)]
		[Export ("captureDynamicRange", ArgumentSemantic.Assign)]
		SCCaptureDynamicRange CaptureDynamicRange { get; set; }

		[Static]
		[NoTV, iOS (27, 0), Mac (15, 0), MacCatalyst (18, 2)]
		[Export ("streamConfigurationWithPreset:")]
		SCStreamConfiguration Create (SCStreamConfigurationPreset preset);
	}

	[UnsupportedSimulator ("ios")]
	[UnsupportedSimulator ("tvos")]
	[iOS (27, 0), TV (27, 0), MacCatalyst (18, 2)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface SCStream {

		[Export ("initWithFilter:configuration:delegate:")]
		NativeHandle Constructor (SCContentFilter contentFilter, SCStreamConfiguration streamConfig, [NullAllowed] ISCStreamDelegate aDelegate);

		[Export ("addStreamOutput:type:sampleHandlerQueue:error:")]
		bool AddStreamOutput (ISCStreamOutput output, SCStreamOutputType type, [NullAllowed] DispatchQueue sampleHandlerQueue, [NullAllowed] out NSError error);

		[Export ("removeStreamOutput:type:error:")]
		bool RemoveStreamOutput (ISCStreamOutput output, SCStreamOutputType type, [NullAllowed] out NSError error);

		[NoiOS, NoTV, MacCatalyst (18, 2)]
		[Async]
		[Export ("updateContentFilter:completionHandler:")]
		void UpdateContentFilter (SCContentFilter contentFilter, [NullAllowed] Action<NSError> completionHandler);

		[NoiOS, NoTV, MacCatalyst (18, 2)]
		[Async]
		[Export ("updateConfiguration:completionHandler:")]
		void UpdateConfiguration (SCStreamConfiguration streamConfig, [NullAllowed] Action<NSError> completionHandler);

		// No Async even on Swift and it makes sense, these are callback APIs.
		[Export ("startCaptureWithCompletionHandler:")]
		void StartCapture ([NullAllowed] Action<NSError> completionHandler);

		// No Async even on Swift and it makes sense, these are callback APIs.
		[Export ("stopCaptureWithCompletionHandler:")]
		void StopCapture ([NullAllowed] Action<NSError> completionHandler);

		[Export ("synchronizationClock")]
		[NullAllowed]
		CMClock SynchronizationClock { get; }

		[iOS (27, 0), TV (27, 0), Mac (15, 0), MacCatalyst (18, 2)]
		[Export ("addRecordingOutput:error:")]
		bool AddRecordingOutput (SCRecordingOutput recordingOutput, [NullAllowed] out NSError error);

		[iOS (27, 0), TV (27, 0), Mac (15, 0), MacCatalyst (18, 2)]
		[Export ("removeRecordingOutput:error:")]
		bool RemoveRecordingOutput (SCRecordingOutput recordingOutput, [NullAllowed] out NSError error);

		[iOS (27, 0), TV (27, 0), Mac (27, 0), MacCatalyst (27, 0)]
		[Export ("capturing")]
		bool Capturing { [Bind ("isCapturing")] get; }

		[iOS (27, 0), TV (27, 0), Mac (27, 0), MacCatalyst (27, 0)]
		[Export ("addClipBufferingOutput:error:")]
		bool AddClipBufferingOutput (SCClipBufferingOutput clipBufferingOutput, [NullAllowed] out NSError error);

		[iOS (27, 0), TV (27, 0), Mac (27, 0), MacCatalyst (27, 0)]
		[Export ("removeClipBufferingOutput:error:")]
		bool RemoveClipBufferingOutput (SCClipBufferingOutput clipBufferingOutput, [NullAllowed] out NSError error);

		[NoTV, NoMac, NoMacCatalyst, iOS (27, 0)]
		[Export ("addVideoEffectOutput:error:")]
		bool AddVideoEffectOutput (SCVideoEffectOutput videoEffectOutput, [NullAllowed] out NSError error);

		[NoTV, NoMac, NoMacCatalyst, iOS (27, 0)]
		[Export ("removeVideoEffectOutput:error:")]
		bool RemoveVideoEffectOutput (SCVideoEffectOutput videoEffectOutput, [NullAllowed] out NSError error);
	}

	interface ISCStreamDelegate { }

	[UnsupportedSimulator ("ios")]
	[UnsupportedSimulator ("tvos")]
	[iOS (27, 0), TV (27, 0), MacCatalyst (18, 2)]
	[Protocol]
	[Model]
	[BaseType (typeof (NSObject))]
	interface SCStreamDelegate {

		[Export ("stream:didStopWithError:")]
		void DidStop (SCStream stream, NSError error);

#if !XAMCORE_5_0
		// Looks like this was a beta method that got removed in stable, but we ended up releasing the binding for it anyways.
		[EditorBrowsable (EditorBrowsableState.Never)]
		[Obsolete ("Do not use this method.")]
		[NoiOS, NoTV, NoMacCatalyst, Mac (14, 4)]
		[Export ("userDidStopStream:")]
		void UserDidStop (SCStream stream);
#endif

		[iOS (27, 0), TV (27, 0)]
		[Export ("outputVideoEffectDidStartForStream:")]
		void OutputVideoEffectDidStart (SCStream stream);

		[iOS (27, 0), TV (27, 0)]
		[Export ("outputVideoEffectDidStopForStream:")]
		void OutputVideoEffectDidStop (SCStream stream);

		[NoTV, NoMac, NoMacCatalyst, iOS (27, 0)]
		[Export ("outputVideoEffectDidFailForStream:withError:")]
		void OutputVideoEffectDidFail (SCStream stream, NSError error);

		[iOS (27, 0), TV (27, 0), Mac (15, 2), MacCatalyst (18, 2)]
		[Export ("streamDidBecomeActive:")]
		void StreamDidBecomeActive (SCStream stream);

		[iOS (27, 0), TV (27, 0), Mac (15, 2), MacCatalyst (18, 2)]
		[Export ("streamDidBecomeInactive:")]
		void StreamDidBecomeInactive (SCStream stream);
	}

	interface ISCStreamOutput { }

	[UnsupportedSimulator ("ios")]
	[UnsupportedSimulator ("tvos")]
	[iOS (27, 0), TV (27, 0), MacCatalyst (18, 2)]
	[Protocol]
	interface SCStreamOutput {

		[Export ("stream:didOutputSampleBuffer:ofType:")]
		void DidOutputSampleBuffer (SCStream stream, CMSampleBuffer sampleBuffer, SCStreamOutputType type);
	}

	[UnsupportedSimulator ("ios")]
	[UnsupportedSimulator ("tvos")]
	[iOS (27, 0), TV (27, 0), MacCatalyst (18, 2)]
	[BaseType (typeof (NSObject))]
	interface SCContentSharingPickerConfiguration {
		[NoiOS, NoTV, MacCatalyst (18, 2)]
		[Export ("allowedPickerModes", ArgumentSemantic.Assign)]
		SCContentSharingPickerMode AllowedPickerModes { get; set; }

		[NoiOS, NoTV, MacCatalyst (18, 2)]
		[Export ("excludedWindowIDs", ArgumentSemantic.Strong)]
		NSNumber [] ExcludedWindowIds { get; set; }

		[NoiOS, NoTV, MacCatalyst (18, 2)]
		[Export ("excludedBundleIDs", ArgumentSemantic.Strong)]
		string [] ExcludedBundleIds { get; set; }

		[NoiOS, NoTV, MacCatalyst (18, 2)]
		[Export ("allowsChangingSelectedContent")]
		bool AllowsChangingSelectedContent { get; set; }

		[NoTV, NoMac, NoMacCatalyst, iOS (27, 0)]
		[Export ("showsMicrophoneControl")]
		bool ShowsMicrophoneControl { get; set; }

		[NoTV, NoMac, NoMacCatalyst, iOS (27, 0)]
		[Export ("showsCameraControl")]
		bool ShowsCameraControl { get; set; }
	}

	[UnsupportedSimulator ("ios")]
	[UnsupportedSimulator ("tvos")]
	[iOS (27, 0), TV (27, 0), MacCatalyst (18, 2)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface SCContentSharingPicker {
		[Static]
		[Export ("sharedPicker")]
		SCContentSharingPicker SharedPicker { get; }

		[Export ("defaultConfiguration", ArgumentSemantic.Copy)]
		SCContentSharingPickerConfiguration DefaultConfiguration { get; set; }

		[NoiOS, NoTV, MacCatalyst (18, 2)]
		[NullAllowed]
		[BindAs (typeof (int))]
		[Export ("maximumStreamCount", ArgumentSemantic.Strong)]
		NSNumber MaximumStreamCount { get; set; }

		[Export ("active")]
		bool Active { [Bind ("isActive")] get; set; }

		[iOS (27, 0), TV (27, 0), Mac (27, 0), MacCatalyst (27, 0)]
		[Export ("available")]
		bool Available { [Bind ("isAvailable")] get; }

		[Export ("addObserver:")]
		void AddObserver (ISCContentSharingPickerObserver observer);

		[Export ("removeObserver:")]
		void RemoveObserver (ISCContentSharingPickerObserver observer);

		[Export ("setConfiguration:forStream:")]
		void SetConfiguration ([NullAllowed] SCContentSharingPickerConfiguration pickerConfig, SCStream stream);

		[NoTV]
		[Export ("present")]
		void Present ();

		[Export ("presentPickerUsingContentStyle:")]
		void Present (SCShareableContentStyle contentStyle);

		[NoiOS, NoTV, MacCatalyst (18, 2)]
		[Export ("presentPickerForStream:")]
		void Present (SCStream stream);

		[NoiOS, NoTV, MacCatalyst (18, 2)]
		[Export ("presentPickerForStream:usingContentStyle:")]
		void Present (SCStream stream, SCShareableContentStyle contentStyle);

		[NoMac, NoMacCatalyst, iOS (27, 0), TV (27, 0)]
		[Export ("presentPickerForCurrentApplication")]
		void PresentForCurrentApplication ();
	}

	interface ISCContentSharingPickerObserver { }

	[UnsupportedSimulator ("ios")]
	[UnsupportedSimulator ("tvos")]
	[iOS (27, 0), TV (27, 0), MacCatalyst (18, 2)]
	[Protocol]
	[Model]
	[BaseType (typeof (NSObject))]
	interface SCContentSharingPickerObserver {
		[Abstract]
		[Export ("contentSharingPicker:didCancelForStream:")]
		void DidCancel (SCContentSharingPicker picker, [NullAllowed] SCStream stream);

		[Abstract]
		[Export ("contentSharingPicker:didUpdateWithFilter:forStream:")]
		void DidUpdate (SCContentSharingPicker picker, SCContentFilter filter, [NullAllowed] SCStream stream);

		[Abstract]
		[Export ("contentSharingPickerStartDidFailWithError:")]
		void DidFail (NSError error);
	}

	[NoiOS, NoTV, MacCatalyst (18, 2)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface SCShareableContentInfo {
		[Export ("style")]
		SCShareableContentStyle Style { get; }

		[Export ("pointPixelScale")]
		float PointPixelScale { get; }

		[Export ("contentRect")]
		CGRect ContentRect { get; }
	}

	[NoiOS, NoTV, MacCatalyst (18, 2)]
	delegate void SCScreenshotManagerCaptureImageCallback ([NullAllowed] CGImage image, [NullAllowed] NSError error);

	[NoiOS, NoTV, Mac (26, 0), MacCatalyst (26, 0)]
	delegate void SCScreenshotManagerCaptureScreenshotCallback ([NullAllowed] SCScreenshotOutput output, [NullAllowed] NSError error);

	[NoiOS, NoTV, MacCatalyst (18, 2)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface SCScreenshotManager {
		[Static]
		[Export ("captureSampleBufferWithFilter:configuration:completionHandler:")]
		[Async]
		void CaptureSampleBuffer (SCContentFilter contentFilter, SCStreamConfiguration config, [NullAllowed] Action<CMSampleBuffer, NSError> completionHandler);

		[Static]
		[Export ("captureImageWithFilter:configuration:completionHandler:")]
		[Async]
		void CaptureImage (SCContentFilter contentFilter, SCStreamConfiguration config, [NullAllowed] Action<CGImage, NSError> completionHandler);

		[Mac (15, 2)]
		[Static]
		[Export ("captureImageInRect:completionHandler:")]
		[Async]
		void CaptureImage (CGRect rect, [NullAllowed] SCScreenshotManagerCaptureImageCallback completionHandler);

		[Async]
		[MacCatalyst (26, 0), Mac (26, 0)]
		[Static]
		[Export ("captureScreenshotWithFilter:configuration:completionHandler:")]
		void CaptureScreenshot (SCContentFilter contentFilter, SCScreenshotConfiguration config, [NullAllowed] SCScreenshotManagerCaptureScreenshotCallback completionHandler);

		[Async]
		[MacCatalyst (26, 0), Mac (26, 0)]
		[Static]
		[Export ("captureScreenshotWithRect:configuration:completionHandler:")]
		void CaptureScreenshot (CGRect rect, SCScreenshotConfiguration config, [NullAllowed] SCScreenshotManagerCaptureScreenshotCallback completionHandler);
	}

	[UnsupportedSimulator ("ios")]
	[UnsupportedSimulator ("tvos")]
	[iOS (27, 0), TV (27, 0), Mac (15, 0), MacCatalyst (18, 2)]
	[BaseType (typeof (NSObject))]
	interface SCRecordingOutputConfiguration {
		[Export ("outputURL", ArgumentSemantic.Copy)]
		NSUrl OutputUrl { get; set; }

		[Export ("videoCodecType", ArgumentSemantic.Copy)]
		[BindAs (typeof (AVVideoCodecType))]
		NSString VideoCodecType { get; set; }

		[Export ("outputFileType", ArgumentSemantic.Copy)]
		[BindAs (typeof (AVFileTypes))]
		NSString OutputFileType { get; set; }

		[Export ("availableVideoCodecTypes")]
		[BindAs (typeof (AVVideoCodecType []))]
		NSString [] AvailableVideoCodecTypes { get; }

		[Export ("availableOutputFileTypes")]
		[BindAs (typeof (AVFileTypes []))]
		NSString [] AvailableOutputFileTypes { get; }

		[iOS (27, 0), TV (27, 0), Mac (27, 0), MacCatalyst (27, 0)]
		[Export ("mixesAudioWithMicrophone")]
		bool MixesAudioWithMicrophone { get; set; }
	}

	[UnsupportedSimulator ("ios")]
	[UnsupportedSimulator ("tvos")]
	[iOS (27, 0), TV (27, 0), Mac (15, 0), MacCatalyst (18, 2)]
	[Protocol (BackwardsCompatibleCodeGeneration = false), Model]
	[BaseType (typeof (NSObject))]
	interface SCRecordingOutputDelegate {
		[Export ("recordingOutputDidStartRecording:")]
		void DidStartRecording (SCRecordingOutput recordingOutput);

		[Export ("recordingOutput:didFailWithError:")]
		void DidFail (SCRecordingOutput recordingOutput, NSError error);

		[Export ("recordingOutputDidFinishRecording:")]
		void DidFinishRecording (SCRecordingOutput recordingOutput);
	}

	interface ISCRecordingOutputDelegate { }

	[UnsupportedSimulator ("ios")]
	[UnsupportedSimulator ("tvos")]
	[iOS (27, 0), TV (27, 0), Mac (15, 0), MacCatalyst (18, 2)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface SCRecordingOutput {
		[Export ("recordedDuration")]
		CMTime RecordedDuration { get; }

		[Export ("recordedFileSize")]
		nint RecordedFileSize { get; }

		[Export ("initWithConfiguration:delegate:")]
		NativeHandle Constructor (SCRecordingOutputConfiguration recordingOutputConfiguration, ISCRecordingOutputDelegate @delegate);
	}

	[iOS (27, 0), TV (27, 0), Mac (27, 0), MacCatalyst (27, 0)]
	delegate void SCClipBufferingOutputExportCompletionHandler ([NullAllowed] NSError error);

	interface ISCClipBufferingOutputDelegate { }

	[UnsupportedSimulator ("ios")]
	[UnsupportedSimulator ("tvos")]
	[iOS (27, 0), TV (27, 0), Mac (27, 0), MacCatalyst (27, 0)]
	[Protocol (BackwardsCompatibleCodeGeneration = false), Model]
	[BaseType (typeof (NSObject))]
	interface SCClipBufferingOutputDelegate {
		[Export ("clipBufferingOutputDidStartBuffering:")]
		void DidStartBuffering (SCClipBufferingOutput clipBufferingOutput);

		[Export ("clipBufferingOutput:didFailWithError:")]
		void DidFail (SCClipBufferingOutput clipBufferingOutput, NSError error);

		[Export ("clipBufferingOutputDidStopBuffering:")]
		void DidStopBuffering (SCClipBufferingOutput clipBufferingOutput);
	}

	[UnsupportedSimulator ("ios")]
	[UnsupportedSimulator ("tvos")]
	[iOS (27, 0), TV (27, 0), Mac (27, 0), MacCatalyst (27, 0)]
	[BaseType (typeof (NSObject))]
	interface SCClipBufferingOutput {
		[Export ("initWithDelegate:")]
		NativeHandle Constructor ([NullAllowed] ISCClipBufferingOutputDelegate @delegate);

		[Async]
		[Export ("exportClipToURL:duration:completionHandler:")]
		void ExportClip (NSUrl url, double duration, [NullAllowed] SCClipBufferingOutputExportCompletionHandler completionHandler);
	}

	[iOS (27, 0), TV (27, 0), Mac (27, 0), MacCatalyst (27, 0)]
	delegate void SCRecordingEditorPresentationCompletionHandler ([NullAllowed] NSError error);

	interface ISCRecordingEditorDelegate { }

	[UnsupportedSimulator ("ios")]
	[UnsupportedSimulator ("tvos")]
	[iOS (27, 0), TV (27, 0), Mac (27, 0), MacCatalyst (27, 0)]
	[Protocol (BackwardsCompatibleCodeGeneration = false), Model]
	[BaseType (typeof (NSObject))]
	interface SCRecordingEditorDelegate {
		[Export ("recordingEditorDidDismiss:")]
		void DidDismiss (SCRecordingEditor editor);

		[Export ("recordingEditor:didFailWithError:")]
		void DidFail (SCRecordingEditor editor, NSError error);
	}

	[UnsupportedSimulator ("ios")]
	[UnsupportedSimulator ("tvos")]
	[iOS (27, 0), TV (27, 0), Mac (27, 0), MacCatalyst (27, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface SCRecordingEditor {
		[DesignatedInitializer]
		[Export ("initWithURL:")]
		NativeHandle Constructor (NSUrl url);

		[Wrap ("WeakDelegate")]
		[NullAllowed]
		ISCRecordingEditorDelegate Delegate { get; set; }

		[NullAllowed]
		[Export ("delegate", ArgumentSemantic.Weak)]
		NSObject WeakDelegate { get; set; }

		[Async]
		[NoiOS, NoTV, NoMacCatalyst, Mac (27, 0)]
		[Export ("presentFromWindow:completionHandler:")]
		void PresentFromWindow (NSWindow window, SCRecordingEditorPresentationCompletionHandler completionHandler);

		[Async]
		[NoMac, iOS (27, 0), TV (27, 0), MacCatalyst (27, 0)]
		[Export ("presentFromWindowScene:completionHandler:")]
		void PresentFromWindowScene (UIWindowScene windowScene, SCRecordingEditorPresentationCompletionHandler completionHandler);

		[Async]
		[NoiOS, NoMac, NoMacCatalyst, TV (27, 0)]
		[Export ("presentFromWindowScene:mode:completionHandler:")]
		void PresentFromWindowScene (UIWindowScene windowScene, SCRecordingEditorMode mode, SCRecordingEditorPresentationCompletionHandler completionHandler);
	}

	[UnsupportedSimulator ("ios")]
	[NoTV, NoMac, NoMacCatalyst, iOS (27, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface SCVideoEffectOutput {
		[Export ("initWithCameraDevice:")]
		NativeHandle Constructor (AVCaptureDevice device);

		[Export ("cameraDevice", ArgumentSemantic.Strong)]
		AVCaptureDevice CameraDevice { get; set; }
	}
}
