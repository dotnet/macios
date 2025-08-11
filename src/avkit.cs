//
// avkit.cs: Definitions for AVKit.cs
//
// Copyright 2014-2015 Xamarin Inc
using System;
using Foundation;
using ObjCRuntime;
using CoreGraphics;
using CoreImage;
using CoreMedia;
using CoreVideo;
using AVFoundation;
#if HAS_OPENGLES
using OpenGLES;
#endif
#if !MONOMAC
using AVPlayerViewControlsStyle = Foundation.NSObject;
using NSColor = UIKit.UIColor;
using NSMenu = Foundation.NSObject;
using NSView = UIKit.UIView;
using UIKit;
#else
using AppKit;
using AVAudioSession = Foundation.NSObject;
using AVContentProposal = Foundation.NSObject;
using AVPlayerViewController = Foundation.NSObject;
using IUIViewControllerTransitionCoordinator = Foundation.NSObject;
using UIColor = AppKit.NSColor;
using UIImage = AppKit.NSImage;
using UIInteraction = Foundation.NSObjectProtocol;
using UILayoutGuide = Foundation.NSObject;
using UITraitCollection = Foundation.NSObject;
using UIView = AppKit.NSView;
using UIViewController = Foundation.NSObject;
using UIWindow = Foundation.NSObject;
using UIAction = Foundation.NSObject;
using UIMenuElement = Foundation.NSObject;
#endif // !MONOMAC

#if TVOS
using AVCustomRoutingController = Foundation.NSObject;
using AVCustomRoutingEvent = Foundation.NSObject;
using AVCustomRoutingActionItem = Foundation.NSObject;
#else
using AVRouting;
#endif

namespace AVKit {
	[NoTV, Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0)]
	[Native]
	enum AVVideoFrameAnalysisType : ulong {
		AVVideoFrameAnalysisTypeNone = 0,
		AVVideoFrameAnalysisTypeDefault = 1 << 0,
		AVVideoFrameAnalysisTypeText = 1 << 1,
		AVVideoFrameAnalysisTypeSubject = 1 << 2,
		AVVideoFrameAnalysisTypeVisualSearch = 1 << 3,
		[NoMac, NoMacCatalyst]
		AVVideoFrameAnalysisTypeMachineReadableCode = 1 << 4,
	}

	/// <summary>Provides video playback in a floating, resizable window on larger devices.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/prerelease/ios/documentation/AVKit/Reference/AVPictureInPictureControllerDelegate_Protocol/index.html#//apple_ref/doc/uid/TP40016161">Apple documentation for <c>AVPictureInPictureController</c></related>
	[TV (14, 0)]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	[Sealed] // Apple docs: Do not subclass AVPictureInPictureController. Overriding this class’s methods is unsupported and results in undefined behavior.
	interface AVPictureInPictureController {
		[Static]
		[Export ("isPictureInPictureSupported")]
		bool IsPictureInPictureSupported { get; }

		[Export ("initWithPlayerLayer:")]
		NativeHandle Constructor (AVPlayerLayer playerLayer);

		[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
		[Export ("initWithContentSource:")]
		[DesignatedInitializer]
		NativeHandle Constructor (AVPictureInPictureControllerContentSource contentSource);

		[Export ("playerLayer")]
		AVPlayerLayer PlayerLayer { get; }

		/// <summary>An instance of the AVKit.IAVPictureInPictureControllerDelegate model class which acts as the class delegate.</summary>
		///         <value>The instance of the AVKit.IAVPictureInPictureControllerDelegate model class</value>
		///         <remarks>
		///           <para>The delegate instance assigned to this object will be used to handle events or provide data on demand to this class.</para>
		///           <para>When setting the Delegate or WeakDelegate values events will be delivered to the specified instance instead of being delivered to the C#-style events</para>
		///           <para>This is the strongly typed version of the object, developers should use the WeakDelegate property instead if they want to merely assign a class derived from NSObject that has been decorated with [Export] attributes.</para>
		///         </remarks>
		[Wrap ("WeakDelegate")]
		[NullAllowed]
		IAVPictureInPictureControllerDelegate Delegate { get; set; }

		[NullAllowed, Export ("delegate", ArgumentSemantic.Weak)]
		NSObject WeakDelegate { get; set; }

		[Export ("startPictureInPicture")]
		void StartPictureInPicture ();

		[Export ("stopPictureInPicture")]
		void StopPictureInPicture ();

		/// <summary>Gets a value that tells whether Picture in Picture is currently possible on the device.</summary>
		///         <value>
		///           <see langword="false" /> if another app is running Picture in Picture.</value>
		///         <remarks>To be added.</remarks>
		[Export ("pictureInPicturePossible")]
		bool PictureInPicturePossible { [Bind ("isPictureInPicturePossible")] get; }

		/// <summary>Gets a value that tells whether Picture in Picture is currently displayed.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("pictureInPictureActive")]
		bool PictureInPictureActive { [Bind ("isPictureInPictureActive")] get; }

		/// <summary>Gets a value that tells whether Picture in Picture playback has been paused and moved off the screen by another application.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("pictureInPictureSuspended")]
		bool PictureInPictureSuspended { [Bind ("isPictureInPictureSuspended")] get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Static]
		[Export ("pictureInPictureButtonStartImage")]
		UIImage PictureInPictureButtonStartImage { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Static]
		[Export ("pictureInPictureButtonStopImage")]
		UIImage PictureInPictureButtonStopImage { get; }

		[NoMac]
		[MacCatalyst (13, 1)]
		[Static]
		[Export ("pictureInPictureButtonStartImageCompatibleWithTraitCollection:")]
		UIImage CreateStartButton ([NullAllowed] UITraitCollection traitCollection);

		[NoMac]
		[MacCatalyst (13, 1)]
		[Static]
		[Export ("pictureInPictureButtonStopImageCompatibleWithTraitCollection:")]
		UIImage CreateStopButton ([NullAllowed] UITraitCollection traitCollection);

		[iOS (14, 0)]
		[MacCatalyst (14, 0)]
		[Export ("requiresLinearPlayback")]
		bool RequiresLinearPlayback { get; set; }

		[NoMac, NoiOS, MacCatalyst (15, 0)]
		[Export ("canStopPictureInPicture")]
		bool CanStopPictureInPicture { get; }

		[iOS (14, 2)]
		[NoTV, NoMac, MacCatalyst (15, 0)]
		[Export ("canStartPictureInPictureAutomaticallyFromInline")]
		bool CanStartPictureInPictureAutomaticallyFromInline { get; set; }

		[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
		[Export ("invalidatePlaybackState")]
		void InvalidatePlaybackState ();

		[NullAllowed]
		[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
		[Export ("contentSource", ArgumentSemantic.Strong)]
		AVPictureInPictureControllerContentSource ContentSource { get; set; }
	}

	/// <summary>Interface representing the required methods (if any) of the protocol <see cref="AVKit.AVPictureInPictureControllerDelegate" />.</summary>
	///     <remarks>
	///       <para>This interface contains the required methods (if any) from the protocol defined by <see cref="AVKit.AVPictureInPictureControllerDelegate" />.</para>
	///       <para>If developers create classes that implement this interface, the implementation methods will automatically be exported to Objective-C with the matching signature from the method defined in the <see cref="AVKit.AVPictureInPictureControllerDelegate" /> protocol.</para>
	///       <para>Optional methods (if any) are provided by the <see cref="AVKit.AVPictureInPictureControllerDelegate_Extensions" /> class as extension methods to the interface, allowing developers to invoke any optional methods on the protocol.</para>
	///     </remarks>
	interface IAVPictureInPictureControllerDelegate { }

	/// <summary>Delegate object providing methods for the application's <see cref="UIKit.UIViewControllerDelegate" /> at the start and stop of picture-in-picture video playback.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/AVKit/Reference/AVPictureInPictureControllerDelegate_Protocol/index.html">Apple documentation for <c>AVPictureInPictureControllerDelegate</c></related>
	[TV (14, 0)]
	[MacCatalyst (13, 1)]
	[Protocol, Model]
	[BaseType (typeof (NSObject))]
	interface AVPictureInPictureControllerDelegate {
		/// <param name="pictureInPictureController">To be added.</param>
		/// <summary>Picture In Picture playback is about to start.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("pictureInPictureControllerWillStartPictureInPicture:")]
		void WillStartPictureInPicture (AVPictureInPictureController pictureInPictureController);

		/// <param name="pictureInPictureController">To be added.</param>
		/// <summary>Picture In Picture playback has started.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("pictureInPictureControllerDidStartPictureInPicture:")]
		void DidStartPictureInPicture (AVPictureInPictureController pictureInPictureController);

		/// <param name="pictureInPictureController">To be added.</param>
		/// <param name="error">To be added.</param>
		/// <summary>Picture In Picture playback failed to start.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("pictureInPictureController:failedToStartPictureInPictureWithError:")]
		void FailedToStartPictureInPicture (AVPictureInPictureController pictureInPictureController, NSError error);

		/// <param name="pictureInPictureController">To be added.</param>
		/// <summary>Picture In Picture playback is about to stop.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("pictureInPictureControllerWillStopPictureInPicture:")]
		void WillStopPictureInPicture (AVPictureInPictureController pictureInPictureController);

		/// <param name="pictureInPictureController">To be added.</param>
		/// <summary>Picture In Picture playback has stopped.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("pictureInPictureControllerDidStopPictureInPicture:")]
		void DidStopPictureInPicture (AVPictureInPictureController pictureInPictureController);

		/// <param name="pictureInPictureController">To be added.</param>
		/// <param name="completionHandler">To be added.</param>
		/// <summary>Picture In Picture playback is about to stop. Called to give the app the opportunity to provide a playback user interface by passing <see langword="true" /> to <paramref name="completionHandler" />.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("pictureInPictureController:restoreUserInterfaceForPictureInPictureStopWithCompletionHandler:")]
		void RestoreUserInterfaceForPictureInPicture (AVPictureInPictureController pictureInPictureController, Action<bool> completionHandler);
	}

	/// <include file="../docs/api/AVKit/AVPlayerViewController.xml" path="/Documentation/Docs[@DocId='T:AVKit.AVPlayerViewController']/*" />
	[NoMac]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (UIViewController))]
	interface AVPlayerViewController {
		/// <param name="nibName">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		/// <param name="bundle">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		/// <summary>Creates a new <see cref="AVKit.AVPlayerViewController" /> for the specified NIB name and bundle.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("initWithNibName:bundle:")]
		[PostGet ("NibBundle")]
		NativeHandle Constructor ([NullAllowed] string nibName, [NullAllowed] NSBundle bundle);

		[NullAllowed] // by default this property is null
		[Export ("player", ArgumentSemantic.Retain)]
		AVPlayer Player { get; set; }

		[Export ("showsPlaybackControls")]
		bool ShowsPlaybackControls { get; set; }

		[Export ("videoGravity")]
		NSString WeakVideoGravity { get; set; }

		/// <summary>Gets a value that tells whether the first frame of vido is ready to display.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("readyForDisplay")]
		bool ReadyForDisplay { [Bind ("isReadyForDisplay")] get; }

		[NoTV]
		[MacCatalyst (13, 1)]
		[Export ("videoBounds")]
		CGRect VideoBounds { get; }

		[NullAllowed]
		[Export ("contentOverlayView")]
		UIView ContentOverlayView { get; }

		[NoiOS]
		[NoMacCatalyst]
		[Export ("unobscuredContentGuide")]
		UILayoutGuide UnobscuredContentGuide { get; }

		[TV (14, 0)]
		[MacCatalyst (13, 1)]
		[Export ("allowsPictureInPicturePlayback")]
		bool AllowsPictureInPicturePlayback { get; set; }

		[NoTV]
		[MacCatalyst (13, 1)]
		[Export ("updatesNowPlayingInfoCenter")]
		bool UpdatesNowPlayingInfoCenter { get; set; }

		[NoTV]
		[MacCatalyst (13, 1)]
		[Export ("entersFullScreenWhenPlaybackBegins")]
		bool EntersFullScreenWhenPlaybackBegins { get; set; }

		[NoTV]
		[MacCatalyst (13, 1)]
		[Export ("exitsFullScreenWhenPlaybackEnds")]
		bool ExitsFullScreenWhenPlaybackEnds { get; set; }

		/// <summary>An instance of the AVKit.IAVPlayerViewControllerDelegate model class which acts as the class delegate.</summary>
		///         <value>The instance of the AVKit.IAVPlayerViewControllerDelegate model class</value>
		///         <remarks>
		///           <para>The delegate instance assigned to this object will be used to handle events or provide data on demand to this class.</para>
		///           <para>When setting the Delegate or WeakDelegate values events will be delivered to the specified instance instead of being delivered to the C#-style events</para>
		///           <para>This is the strongly typed version of the object, developers should use the WeakDelegate property instead if they want to merely assign a class derived from NSObject that has been decorated with [Export] attributes.</para>
		///         </remarks>
		[Wrap ("WeakDelegate")]
		[NullAllowed]
		IAVPlayerViewControllerDelegate Delegate { get; set; }

		[MacCatalyst (13, 1)]
		[NullAllowed, Export ("delegate", ArgumentSemantic.Weak)]
		NSObject WeakDelegate { get; set; }

		[NoMac]
		[iOS (14, 0)]
		[MacCatalyst (14, 0)]
		[Export ("requiresLinearPlayback")]
		bool RequiresLinearPlayback { get; set; }

		#region AVPlayerViewControllerSubtitleOptions
		[NoiOS]
		[NoMac]
		[NoMacCatalyst]
		[NullAllowed, Export ("allowedSubtitleOptionLanguages", ArgumentSemantic.Copy)]
		string [] AllowedSubtitleOptionLanguages { get; set; }

		[NoiOS]
		[NoMac]
		[NoMacCatalyst]
		[Export ("requiresFullSubtitles")]
		bool RequiresFullSubtitles { get; set; }
		#endregion

		[NullAllowed]
		[NoiOS, NoMac]
		[NoMacCatalyst]
		[Export ("contentProposalViewController", ArgumentSemantic.Assign)]
		AVContentProposalViewController ContentProposalViewController { get; set; }

		[NoiOS, NoMac]
		[NoMacCatalyst]
		[Export ("skippingBehavior", ArgumentSemantic.Assign)]
		AVPlayerViewControllerSkippingBehavior SkippingBehavior { get; set; }

		[NoiOS, NoMac]
		[NoMacCatalyst]
		[Export ("skipForwardEnabled")]
		bool SkipForwardEnabled { [Bind ("isSkipForwardEnabled")] get; set; }

		[NoiOS, NoMac]
		[NoMacCatalyst]
		[Export ("skipBackwardEnabled")]
		bool SkipBackwardEnabled { [Bind ("isSkipBackwardEnabled")] get; set; }

		// From AVPlayerViewControllerControls category

		[NoiOS, NoMac]
		[NoMacCatalyst]
		[Export ("playbackControlsIncludeTransportBar")]
		bool PlaybackControlsIncludeTransportBar { get; set; }

		[NoiOS, NoMac]
		[NoMacCatalyst]
		[Export ("playbackControlsIncludeInfoViews")]
		bool PlaybackControlsIncludeInfoViews { get; set; }

		[NullAllowed]
		[NoiOS, NoMac]
		[Deprecated (PlatformName.TvOS, 15, 0, message: "Use 'CustomInfoViewControllers' instead.")]
		[NoMacCatalyst]
		[Export ("customInfoViewController", ArgumentSemantic.Assign)]
		UIViewController CustomInfoViewController { get; set; }

		[NoiOS, NoMac]
		[NoMacCatalyst]
		[Export ("appliesPreferredDisplayCriteriaAutomatically")]
		bool AppliesPreferredDisplayCriteriaAutomatically { get; set; }

		[TV (13, 0)]
		[MacCatalyst (13, 1)]
		[NullAllowed, Export ("pixelBufferAttributes", ArgumentSemantic.Copy)]
		NSDictionary<NSString, NSObject> PixelBufferAttributes { get; set; }

		[NoiOS, TV (13, 0)]
		[NoMacCatalyst]
		[NullAllowed, Export ("customOverlayViewController", ArgumentSemantic.Strong)]
		UIViewController CustomOverlayViewController { get; set; }

		[iOS (14, 0), NoTV]
		[MacCatalyst (14, 0)]
		[Export ("showsTimecodes")]
		bool ShowsTimecodes { get; set; }

		[iOS (14, 2)]
		[NoTV, MacCatalyst (15, 0)]
		[Export ("canStartPictureInPictureAutomaticallyFromInline")]
		bool CanStartPictureInPictureAutomaticallyFromInline { get; set; }

		[TV (15, 0), NoMac, NoiOS, NoMacCatalyst]
		[Export ("contextualActions", ArgumentSemantic.Copy)]
		UIAction [] ContextualActions { get; set; }

		[TV (15, 0), NoMac, NoiOS, NoMacCatalyst]
		[Export ("infoViewActions", ArgumentSemantic.Copy)]
		[NullAllowed]
		UIAction [] InfoViewActions { get; set; }

		[TV (15, 0), NoMac, NoiOS, NoMacCatalyst]
		[Export ("customInfoViewControllers", ArgumentSemantic.Copy)]
		UIViewController [] CustomInfoViewControllers { get; set; }

		[TV (15, 0), NoMac, NoiOS, NoMacCatalyst]
		[Export ("transportBarCustomMenuItems", ArgumentSemantic.Copy)]
		UIMenuElement [] TransportBarCustomMenuItems { get; set; }

		[TV (15, 0), NoMac, NoiOS, NoMacCatalyst]
		[Export ("transportBarIncludesTitleView")]
		bool TransportBarIncludesTitleView { get; set; }

		[NoTV, MacCatalyst (16, 0), NoMac, iOS (16, 0)]
		[Export ("allowsVideoFrameAnalysis")]
		bool AllowsVideoFrameAnalysis { get; set; }

		[iOS (16, 0), MacCatalyst (16, 0), NoMac, TV (16, 0)]
		[Export ("speeds", ArgumentSemantic.Copy)]
		AVPlaybackSpeed [] Speeds { get; set; }

		[iOS (16, 0), MacCatalyst (16, 0), NoMac, TV (16, 0)]
		[NullAllowed, Export ("selectedSpeed")]
		AVPlaybackSpeed SelectedSpeed { get; }

		[iOS (16, 0), MacCatalyst (16, 0), NoMac, TV (16, 0)]
		[Export ("selectSpeed:")]
		void SelectSpeed (AVPlaybackSpeed speed);

		[iOS (17, 0), MacCatalyst (18, 0), NoTV, NoMac]
		[Export ("videoFrameAnalysisTypes")]
		AVVideoFrameAnalysisType VideoFrameAnalysisTypes { get; set; }

		[iOS (17, 0), MacCatalyst (18, 0), NoTV, NoMac]
		[Export ("toggleLookupAction")]
		UIAction ToggleLookupAction { get; }

		[MacCatalyst (26, 0), NoMac, NoTV, iOS (26, 0)]
		[Export ("preferredDisplayDynamicRange", ArgumentSemantic.Assign)]
		AVDisplayDynamicRange PreferredDisplayDynamicRange { get; set; }


		[TV (26, 0), MacCatalyst (26, 0), NoMac, iOS (26, 0)]
		[Static]
		[Export ("mediaCharacteristicsForSupportedCustomMediaSelectionSchemes")]
		string [] MediaCharacteristicsForSupportedCustomMediaSelectionSchemes { get; }
	}

	/// <summary>Interface representing the required methods (if any) of the protocol <see cref="AVKit.AVPlayerViewControllerDelegate" />.</summary>
	///     <remarks>
	///       <para>This interface contains the required methods (if any) from the protocol defined by <see cref="AVKit.AVPlayerViewControllerDelegate" />.</para>
	///       <para>If developers create classes that implement this interface, the implementation methods will automatically be exported to Objective-C with the matching signature from the method defined in the <see cref="AVKit.AVPlayerViewControllerDelegate" /> protocol.</para>
	///       <para>Optional methods (if any) are provided by the <see cref="AVKit.AVPlayerViewControllerDelegate_Extensions" /> class as extension methods to the interface, allowing developers to invoke any optional methods on the protocol.</para>
	///     </remarks>
	interface IAVPlayerViewControllerDelegate { }

	/// <summary>Delegate object for the picture-in-picture controller. When overridden, the methods allow the developer to respond to events relating to p-in-p playback.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/AVKit/Reference/AVPlayerViewControllerDelegate_Protocol/index.html">Apple documentation for <c>AVPlayerViewControllerDelegate</c></related>
	[NoMac]
	[MacCatalyst (13, 1)]
	[Protocol, Model]
	[BaseType (typeof (NSObject))]
	interface AVPlayerViewControllerDelegate {
		/// <param name="playerViewController">To be added.</param>
		/// <summary>Picture In Picture playback is about to start.</summary>
		/// <remarks>To be added.</remarks>
		[TV (14, 0)]
		[MacCatalyst (13, 1)]
		[Export ("playerViewControllerWillStartPictureInPicture:")]
		void WillStartPictureInPicture (AVPlayerViewController playerViewController);

		/// <param name="playerViewController">To be added.</param>
		/// <summary>Picture In Picture playback has started.</summary>
		/// <remarks>To be added.</remarks>
		[TV (14, 0)]
		[MacCatalyst (13, 1)]
		[Export ("playerViewControllerDidStartPictureInPicture:")]
		void DidStartPictureInPicture (AVPlayerViewController playerViewController);

		/// <param name="playerViewController">To be added.</param>
		/// <param name="error">To be added.</param>
		/// <summary>Picture In Picture playback failed to start.</summary>
		/// <remarks>To be added.</remarks>
		[TV (14, 0)]
		[MacCatalyst (13, 1)]
		[Export ("playerViewController:failedToStartPictureInPictureWithError:")]
		void FailedToStartPictureInPicture (AVPlayerViewController playerViewController, NSError error);

		/// <param name="playerViewController">To be added.</param>
		/// <summary>Picture In Picture playback is about to stop.</summary>
		/// <remarks>To be added.</remarks>
		[TV (14, 0)]
		[MacCatalyst (13, 1)]
		[Export ("playerViewControllerWillStopPictureInPicture:")]
		void WillStopPictureInPicture (AVPlayerViewController playerViewController);

		/// <param name="playerViewController">To be added.</param>
		/// <summary>Picture In Picture playback has stopped.</summary>
		/// <remarks>To be added.</remarks>
		[TV (14, 0)]
		[MacCatalyst (13, 1)]
		[Export ("playerViewControllerDidStopPictureInPicture:")]
		void DidStopPictureInPicture (AVPlayerViewController playerViewController);

		/// <param name="playerViewController">To be added.</param>
		/// <summary>App developers should return <see langword="true" /> to indicate that the player viewer should dismiss when Picture In Picture playback starts, or <see langword="false" /> to prevent this.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		[TV (14, 0)]
		[MacCatalyst (13, 1)]
		[Export ("playerViewControllerShouldAutomaticallyDismissAtPictureInPictureStart:")]
		bool ShouldAutomaticallyDismissAtPictureInPictureStart (AVPlayerViewController playerViewController);

		/// <param name="playerViewController">To be added.</param>
		/// <param name="completionHandler">To be added.</param>
		/// <summary>Picture In Picture playback is about to stop. Called to give the app the opportunity to provide a playback user interface by passing <see langword="true" /> to <paramref name="completionHandler" />.</summary>
		/// <remarks>To be added.</remarks>
		[TV (14, 0)]
		[MacCatalyst (13, 1)]
		[Export ("playerViewController:restoreUserInterfaceForPictureInPictureStopWithCompletionHandler:")]
		void RestoreUserInterfaceForPictureInPicture (AVPlayerViewController playerViewController, Action<bool> completionHandler);

		[iOS (16, 0)]
		[NoMac]
		[NoMacCatalyst]
		[Export ("playerViewController:didPresentInterstitialTimeRange:")]
		void DidPresentInterstitialTimeRange (AVPlayerViewController playerViewController, AVInterstitialTimeRange interstitial);

		[NoiOS]
		[NoMac]
		[NoMacCatalyst]
		[Export ("playerViewControllerShouldDismiss:")]
		bool ShouldDismiss (AVPlayerViewController playerViewController);

		[NoiOS]
		[NoMac]
		[NoMacCatalyst]
		[Export ("playerViewControllerWillBeginDismissalTransition:")]
		void WillBeginDismissalTransition (AVPlayerViewController playerViewController);

		[NoiOS]
		[NoMac]
		[NoMacCatalyst]
		[Export ("playerViewControllerDidEndDismissalTransition:")]
		void DidEndDismissalTransition (AVPlayerViewController playerViewController);

		[iOS (16, 0)]
		[NoMac]
		[NoMacCatalyst]
		[Export ("playerViewController:willPresentInterstitialTimeRange:")]
		void WillPresentInterstitialTimeRange (AVPlayerViewController playerViewController, AVInterstitialTimeRange interstitial);

		[NoiOS]
		[NoMac]
		[NoMacCatalyst]
		[Export ("playerViewController:willResumePlaybackAfterUserNavigatedFromTime:toTime:")]
		void WillResumePlaybackAfterUserNavigatedFromTime (AVPlayerViewController playerViewController, CMTime oldTime, CMTime targetTime);

		[NoiOS]
		[NoMac]
		[NoMacCatalyst]
		[Export ("playerViewController:didSelectMediaSelectionOption:inMediaSelectionGroup:")]
		void DidSelectMediaSelectionOption (AVPlayerViewController playerViewController, [NullAllowed] AVMediaSelectionOption mediaSelectionOption, AVMediaSelectionGroup mediaSelectionGroup);

		[NoiOS]
		[NoMac]
		[NoMacCatalyst]
		[Export ("playerViewController:didSelectExternalSubtitleOptionLanguage:")]
		void DidSelectExternalSubtitleOptionLanguage (AVPlayerViewController playerViewController, string language);

		[NoiOS, NoMac]
		[NoMacCatalyst]
		[Export ("playerViewController:timeToSeekAfterUserNavigatedFromTime:toTime:")]
		CMTime GetTimeToSeekAfterUserNavigated (AVPlayerViewController playerViewController, CMTime oldTime, CMTime targetTime);

		[NoiOS, NoMac]
		[NoMacCatalyst]
		[Export ("skipToNextItemForPlayerViewController:")]
		void SkipToNextItem (AVPlayerViewController playerViewController);

		[NoiOS, NoMac]
		[NoMacCatalyst]
		[Export ("skipToPreviousItemForPlayerViewController:")]
		void SkipToPreviousItem (AVPlayerViewController playerViewController);

		[NoiOS, NoMac]
		[NoMacCatalyst]
		[Export ("playerViewController:shouldPresentContentProposal:")]
		bool ShouldPresentContentProposal (AVPlayerViewController playerViewController, AVContentProposal proposal);

		[NoiOS, NoMac]
		[NoMacCatalyst]
		[Export ("playerViewController:didAcceptContentProposal:")]
		void DidAcceptContentProposal (AVPlayerViewController playerViewController, AVContentProposal proposal);

		[NoiOS, NoMac]
		[NoMacCatalyst]
		[Export ("playerViewController:didRejectContentProposal:")]
		void DidRejectContentProposal (AVPlayerViewController playerViewController, AVContentProposal proposal);

		[NoiOS, NoMac]
		[NoMacCatalyst]
		[Export ("playerViewController:willTransitionToVisibilityOfTransportBar:withAnimationCoordinator:")]
		void WillTransitionToVisibilityOfTransportBar (AVPlayerViewController playerViewController, bool visible, IAVPlayerViewControllerAnimationCoordinator coordinator);

		[iOS (13, 0), NoTV, NoMac]
		[MacCatalyst (13, 1)]
		[Export ("playerViewController:willBeginFullScreenPresentationWithAnimationCoordinator:"), EventArgs ("AVPlayerViewFullScreenPresentationWillBegin")]
		void WillBeginFullScreenPresentation (AVPlayerViewController playerViewController, IUIViewControllerTransitionCoordinator coordinator);

		[iOS (13, 0), NoTV, NoMac]
		[MacCatalyst (13, 1)]
		[Export ("playerViewController:willEndFullScreenPresentationWithAnimationCoordinator:"), EventArgs ("AVPlayerViewFullScreenPresentationWillEnd")]
		void WillEndFullScreenPresentation (AVPlayerViewController playerViewController, IUIViewControllerTransitionCoordinator coordinator);

		[TV (13, 0), NoiOS, NoMac]
		[NoMacCatalyst]
		[Export ("nextChannelInterstitialViewControllerForPlayerViewController:")]
		UIViewController GetNextChannelInterstitialViewController (AVPlayerViewController playerViewController);

		[TV (13, 0), NoiOS, NoMac]
		[NoMacCatalyst]
		[Export ("playerViewController:skipToNextChannel:"), EventArgs ("AVPlayerViewSkipToNextChannel")]
		void SkipToNextChannel (AVPlayerViewController playerViewController, Action<bool> completion);

		[TV (13, 0), NoiOS, NoMac]
		[NoMacCatalyst]
		[Export ("playerViewController:skipToPreviousChannel:"), EventArgs ("AVPlayerViewSkipToPreviousChannel")]
		void SkipToPreviousChannel (AVPlayerViewController playerViewController, Action<bool> completion);

		[TV (13, 0), NoiOS, NoMac]
		[NoMacCatalyst]
		[Export ("previousChannelInterstitialViewControllerForPlayerViewController:")]
		UIViewController GetPreviousChannelInterstitialViewController (AVPlayerViewController playerViewController);

		[iOS (15, 0), NoTV, NoMac, MacCatalyst (15, 0)]
		[Export ("playerViewController:restoreUserInterfaceForFullScreenExitWithCompletionHandler:")]
		void RestoreUserInterfaceForFullScreenExit (AVPlayerViewController playerViewController, Action<bool> completionHandler);
	}

	[NoTV, NoMac, iOS (13, 0)]
	[MacCatalyst (13, 1)]
	[Category]
	[BaseType (typeof (AVAudioSession))]
	interface AVAudioSession_AVPlaybackRouteSelecting {

		[Async (ResultTypeName = "PreparingRouteSelectionForPlayback")]
		[Export ("prepareRouteSelectionForPlaybackWithCompletionHandler:")]
		void PrepareRouteSelectionForPlayback (Action<bool, AVAudioSessionRouteSelection> completionHandler);
	}

	interface IAVPlayerViewControllerAnimationCoordinator { }

	[NoiOS, NoMac]
	[NoMacCatalyst]
	[Protocol]
	interface AVPlayerViewControllerAnimationCoordinator {

		[Abstract]
		[Export ("addCoordinatedAnimations:completion:")]
		void AddCoordinatedAnimations (Action animations, Action<bool> completion);
	}

	[NoiOS, NoTV]
	[NoMacCatalyst]
	[BaseType (typeof (NSView))]
	interface AVPlayerView {
		/// <param name="frameRect">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("initWithFrame:")]
		NativeHandle Constructor (CGRect frameRect);

		[NullAllowed]
		[Export ("player")]
		AVPlayer Player { get; set; }

		[Export ("controlsStyle")]
		AVPlayerViewControlsStyle ControlsStyle { get; set; }

		[NoMacCatalyst]
		[Export ("videoGravity")]
		string VideoGravity { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NoMacCatalyst]
		[Export ("readyForDisplay")]
		bool ReadyForDisplay { [Bind ("isReadyForDisplay")] get; }

		[NoMacCatalyst]
		[Export ("videoBounds")]
		CGRect VideoBounds { get; }

		[NullAllowed]
		[NoMacCatalyst]
		[Export ("contentOverlayView")]
		NSView ContentOverlayView { get; }

		[NoMacCatalyst]
		[Export ("updatesNowPlayingInfoCenter")]
		bool UpdatesNowPlayingInfoCenter { get; set; }

		[NullAllowed]
		[Export ("actionPopUpButtonMenu")]
		NSMenu ActionPopUpButtonMenu { get; set; }

		// No async
		[Export ("beginTrimmingWithCompletionHandler:")]
		void BeginTrimming ([NullAllowed] Action<AVPlayerViewTrimResult> handler);

		[Export ("canBeginTrimming")]
		bool CanBeginTrimming { get; }

		/// <param name="chapterNumber">To be added.</param>
		/// <param name="chapterTitle">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("flashChapterNumber:chapterTitle:")]
		void FlashChapter (nuint chapterNumber, [NullAllowed] string chapterTitle);

		[Export ("showsFrameSteppingButtons")]
		bool ShowsFrameSteppingButtons { get; set; }

		[Export ("showsFullScreenToggleButton")]
		bool ShowsFullScreenToggleButton { get; set; }

		[Export ("showsSharingServiceButton")]
		bool ShowsSharingServiceButton { get; set; }

		[NoMacCatalyst]
		[Export ("allowsPictureInPicturePlayback")]
		bool AllowsPictureInPicturePlayback { get; set; }

		[NoMacCatalyst]
		[Wrap ("WeakPictureInPictureDelegate")]
		[NullAllowed]
		IAVPlayerViewPictureInPictureDelegate PictureInPictureDelegate { get; set; }

		[NoMacCatalyst]
		[NullAllowed, Export ("pictureInPictureDelegate", ArgumentSemantic.Weak)]
		NSObject WeakPictureInPictureDelegate { get; set; }

		[NoMacCatalyst]
		[Export ("showsTimecodes")]
		bool ShowsTimecodes { get; set; }

		[NoMacCatalyst]
		[Wrap ("WeakDelegate")]
		IAVPlayerViewDelegate Delegate { get; set; }

		[NoMacCatalyst]
		[NullAllowed, Export ("delegate", ArgumentSemantic.Weak)]
		NSObject WeakDelegate { get; set; }

		[Mac (13, 0), NoiOS, NoMacCatalyst, NoTV]
		[Export ("speeds", ArgumentSemantic.Copy)]
		AVPlaybackSpeed [] Speeds { get; set; }

		[Mac (13, 0), NoiOS, NoMacCatalyst, NoTV]
		[NullAllowed, Export ("selectedSpeed")]
		AVPlaybackSpeed SelectedSpeed { get; }

		[Mac (13, 0), NoiOS, NoMacCatalyst, NoTV]
		[Export ("selectSpeed:")]
		void SelectSpeed (AVPlaybackSpeed speed);

		[NoTV, NoMacCatalyst, NoiOS, Mac (13, 0)]
		[Export ("allowsVideoFrameAnalysis")]
		bool AllowsVideoFrameAnalysis { get; set; }

		[Mac (13, 0), NoiOS, NoMacCatalyst, NoTV]
		[Export ("allowsMagnification")]
		bool AllowsMagnification { get; set; }

		[Mac (13, 0), NoiOS, NoMacCatalyst, NoTV]
		[Export ("magnification")]
		nfloat Magnification { get; set; }

		[Mac (13, 0), NoiOS, NoMacCatalyst, NoTV]
		[Export ("setMagnification:centeredAtPoint:")]
		void SetMagnification (nfloat magnification, CGPoint centeredAtPoint);

		[Mac (14, 0)]
		[Export ("videoFrameAnalysisTypes")]
		AVVideoFrameAnalysisType VideoFrameAnalysisTypes { get; set; }

		[Mac (26, 0)]
		[Export ("preferredDisplayDynamicRange", ArgumentSemantic.Assign)]
		AVDisplayDynamicRange PreferredDisplayDynamicRange { get; set; }
	}

	interface IAVPlayerViewPictureInPictureDelegate { }

	[NoiOS, NoTV]
	[NoMacCatalyst]
	[Protocol, Model]
	[BaseType (typeof (NSObject))]
	interface AVPlayerViewPictureInPictureDelegate {

		[Export ("playerViewWillStartPictureInPicture:")]
		void WillStart (AVPlayerView playerView);

		[Export ("playerViewDidStartPictureInPicture:")]
		void DidStart (AVPlayerView playerView);

		[Export ("playerView:failedToStartPictureInPictureWithError:"), EventArgs ("AVPlayerViewFailedToStart")]
		void FailedToStart (AVPlayerView playerView, NSError error);

		[Export ("playerViewWillStopPictureInPicture:")]
		void WillStop (AVPlayerView playerView);

		[Export ("playerViewDidStopPictureInPicture:")]
		void DidStop (AVPlayerView playerView);

		[Export ("playerView:restoreUserInterfaceForPictureInPictureStopWithCompletionHandler:"), EventArgs ("AVPlayerViewRestoreUserInterface")]
		void RestoreUserInterface (AVPlayerView playerView, Action<bool> completionHandler);

		[Export ("playerViewShouldAutomaticallyDismissAtPictureInPictureStart:")]
		bool ShouldAutomaticallyDismiss (AVPlayerView playerView);
	}

	[NoiOS, NoTV]
	[NoMacCatalyst]
	[BaseType (typeof (NSView))]
	interface AVCaptureView {
		/// <param name="frameRect">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("initWithFrame:")]
		NativeHandle Constructor (CGRect frameRect);

		[Export ("session"), NullAllowed]
		AVCaptureSession Session { get; }

		[Export ("setSession:showVideoPreview:showAudioPreview:")]
		void SetSession ([NullAllowed] AVCaptureSession session, bool showVideoPreview, bool showAudioPreview);

		[Export ("fileOutput"), NullAllowed]
		AVCaptureFileOutput FileOutput { get; }

		[Export ("delegate", ArgumentSemantic.Weak), NullAllowed]
		IAVCaptureViewDelegate Delegate { get; set; }

		[Export ("controlsStyle")]
		AVCaptureViewControlsStyle ControlsStyle { get; set; }

		// TODO: Create an enum version of this property
		[Export ("videoGravity", ArgumentSemantic.Copy)]
		NSString WeakVideoGravity { get; set; }
	}

	interface IAVCaptureViewDelegate { }

	[NoiOS, NoTV]
	[Protocol, Model]
	[NoMacCatalyst]
	[BaseType (typeof (NSObject))]
	interface AVCaptureViewDelegate {
		/// <param name="captureView">To be added.</param>
		/// <param name="fileOutput">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Abstract]
		[Export ("captureView:startRecordingToFileOutput:")]
		void StartRecording (AVCaptureView captureView, AVCaptureFileOutput fileOutput);
	}

	[iOS (16, 0)]
	[NoMac]
	[NoMacCatalyst]
	[BaseType (typeof (NSObject))]
	interface AVInterstitialTimeRange : NSCopying, NSSecureCoding {
		[Export ("initWithTimeRange:")]
		[DesignatedInitializer]
		NativeHandle Constructor (CMTimeRange timeRange);

		[Export ("timeRange")]
		CMTimeRange TimeRange { get; }
	}

	[NoiOS]
	[NoMac]
	[NoMacCatalyst]
	[DisableDefaultCtor]
	[BaseType (typeof (NSObject))]
	interface AVNavigationMarkersGroup {
		[Export ("initWithTitle:timedNavigationMarkers:")]
		[DesignatedInitializer]
		NativeHandle Constructor ([NullAllowed] string title, AVTimedMetadataGroup [] navigationMarkers);

		[Export ("initWithTitle:dateRangeNavigationMarkers:")]
		[DesignatedInitializer]
		NativeHandle Constructor ([NullAllowed] string title, AVDateRangeMetadataGroup [] navigationMarkers);

		[NullAllowed, Export ("title")]
		string Title { get; }

		[NullAllowed, Export ("timedNavigationMarkers")]
		AVTimedMetadataGroup [] TimedNavigationMarkers { get; }

		[NullAllowed, Export ("dateRangeNavigationMarkers")]
		AVDateRangeMetadataGroup [] DateRangeNavigationMarkers { get; }
	}

	[NoMac]
	[NoiOS]
	[NoMacCatalyst]
	[BaseType (typeof (UIViewController))]
	interface AVContentProposalViewController {
		[Export ("initWithNibName:bundle:")]
		[PostGet ("NibBundle")]
		NativeHandle Constructor ([NullAllowed] string nibName, [NullAllowed] NSBundle bundle);

		[NullAllowed, Export ("contentProposal")]
		AVContentProposal ContentProposal { get; }

		[NullAllowed, Export ("playerViewController", ArgumentSemantic.Weak)]
		AVPlayerViewController PlayerViewController { get; }

		[Export ("preferredPlayerViewFrame")]
		CGRect PreferredPlayerViewFrame { get; }

		[Export ("playerLayoutGuide")]
		UILayoutGuide PlayerLayoutGuide { get; }

		[NullAllowed, Export ("dateOfAutomaticAcceptance", ArgumentSemantic.Assign)]
		NSDate DateOfAutomaticAcceptance { get; set; }

		[Export ("dismissContentProposalForAction:animated:completion:")]
		void DismissContentProposal (AVContentProposalAction action, bool animated, [NullAllowed] Action block);
	}

	[Static]
	[NoMac]
	[NoiOS]
	[NoMacCatalyst]
	interface AVKitMetadataIdentifier {

		[Field ("AVKitMetadataIdentifierExternalContentIdentifier")]
		NSString ExternalContentIdentifier { get; }
		[Field ("AVKitMetadataIdentifierExternalUserProfileIdentifier")]
		NSString ExternalUserProfileIdentifier { get; }
		[Field ("AVKitMetadataIdentifierPlaybackProgress")]
		NSString PlaybackProgress { get; }

		[NoMacCatalyst]
		[Field ("AVKitMetadataIdentifierExactStartDate")]
		NSString ExactStartDate { get; }

		[NoMacCatalyst]
		[Field ("AVKitMetadataIdentifierApproximateStartDate")]
		NSString ApproximateStartDate { get; }

		[NoMacCatalyst]
		[Field ("AVKitMetadataIdentifierExactEndDate")]
		NSString ExactEndDate { get; }

		[NoMacCatalyst]
		[Field ("AVKitMetadataIdentifierApproximateEndDate")]
		NSString ApproximateEndDate { get; }

		[NoMacCatalyst]
		[Field ("AVKitMetadataIdentifierServiceIdentifier")]
		NSString ServiceIdentifier { get; }
	}

	[MacCatalyst (13, 1)]
	[BaseType (typeof (UIView))]
	interface AVRoutePickerView {

		/// <param name="frame">Frame used by the view, expressed in iOS points.</param>
		/// <summary>Initializes the AVRoutePickerView with the specified frame.</summary>
		/// <remarks>
		///           <para>This constructor is used to programmatically create a new instance of AVRoutePickerView with the specified dimension in the frame.   The object will only be displayed once it has been added to a view hierarchy by calling AddSubview in a containing view.</para>
		///           <para>This constructor is not invoked when deserializing objects from storyboards or XIB filesinstead the constructor that takes an NSCoder parameter is invoked.</para>
		///         </remarks>
		[Export ("initWithFrame:")]
		NativeHandle Constructor (CGRect frame);

		/// <summary>An instance of the AVKit.IAVRoutePickerViewDelegate model class which acts as the class delegate.</summary>
		///         <value>The instance of the AVKit.IAVRoutePickerViewDelegate model class</value>
		///         <remarks>
		///           <para>The delegate instance assigned to this object will be used to handle events or provide data on demand to this class.</para>
		///           <para>When setting the Delegate or WeakDelegate values events will be delivered to the specified instance instead of being delivered to the C#-style events</para>
		///           <para>This is the strongly typed version of the object, developers should use the WeakDelegate property instead if they want to merely assign a class derived from NSObject that has been decorated with [Export] attributes.</para>
		///         </remarks>
		[Wrap ("WeakDelegate", IsVirtual = true)]
		[NullAllowed]
		IAVRoutePickerViewDelegate Delegate { get; set; }

		[NullAllowed, Export ("delegate", ArgumentSemantic.Weak)]
		NSObject WeakDelegate { get; set; }

		[NoMac]
		[MacCatalyst (13, 1)]
		[Export ("activeTintColor", ArgumentSemantic.Assign), NullAllowed]
		UIColor ActiveTintColor { get; set; }

		[NoiOS, NoMac, NoMacCatalyst]
		[Export ("routePickerButtonStyle", ArgumentSemantic.Assign)]
		AVRoutePickerViewButtonStyle RoutePickerButtonStyle { get; set; }

		[NoMac]
		[TV (13, 0), iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Export ("prioritizesVideoDevices")]
		bool PrioritizesVideoDevices { get; set; }

		[NoiOS, NoTV]
		[NoMacCatalyst]
		[Export ("routePickerButtonColorForState:")]
		NSColor GetRoutePickerButtonColor (AVRoutePickerViewButtonState state);

		[NoiOS, NoTV]
		[NoMacCatalyst]
		[Export ("setRoutePickerButtonColor:forState:")]
		void SetRoutePickerButtonColor ([NullAllowed] NSColor color, AVRoutePickerViewButtonState state);

		[NoiOS, NoTV]
		[NoMacCatalyst]
		[Export ("routePickerButtonBordered")]
		bool RoutePickerButtonBordered { [Bind ("isRoutePickerButtonBordered")] get; set; }

		[NoiOS, NoTV]
		[NoMacCatalyst]
		[NullAllowed, Export ("player", ArgumentSemantic.Assign)]
		AVPlayer Player { get; set; }

		[NoTV, NoMac, iOS (16, 0), MacCatalyst (16, 0)]
		[NullAllowed, Export ("customRoutingController", ArgumentSemantic.Assign)]
		AVCustomRoutingController CustomRoutingController { get; set; }
	}

	[NoiOS, NoMac, NoMacCatalyst]
	[Native]
	public enum AVRoutePickerViewButtonStyle : long {
		System,
		Plain,
		Custom,
	}

	/// <summary>Interface representing the required methods (if any) of the protocol <see cref="AVKit.AVRoutePickerViewDelegate" />.</summary>
	///     <remarks>
	///       <para>This interface contains the required methods (if any) from the protocol defined by <see cref="AVKit.AVRoutePickerViewDelegate" />.</para>
	///       <para>If developers create classes that implement this interface, the implementation methods will automatically be exported to Objective-C with the matching signature from the method defined in the <see cref="AVKit.AVRoutePickerViewDelegate" /> protocol.</para>
	///       <para>Optional methods (if any) are provided by the <see cref="AVKit.AVRoutePickerViewDelegate_Extensions" /> class as extension methods to the interface, allowing developers to invoke any optional methods on the protocol.</para>
	///     </remarks>
	interface IAVRoutePickerViewDelegate { }

	/// <summary>Interface representing the required methods (if any) of the protocol <see cref="AVKit.AVRoutePickerViewDelegate" />.</summary>
	/// <remarks>
	///       <para>This interface contains the required methods (if any) from the protocol defined by <see cref="AVKit.AVRoutePickerViewDelegate" />.</para>
	///       <para>If developers create classes that implement this interface, the implementation methods will automatically be exported to Objective-C with the matching signature from the method defined in the <see cref="AVKit.AVRoutePickerViewDelegate" /> protocol.</para>
	///       <para>Optional methods (if any) are provided by the <see cref="AVKit.AVRoutePickerViewDelegate_Extensions" /> class as extension methods to the interface, allowing developers to invoke any optional methods on the protocol.</para>
	///     </remarks>
	[MacCatalyst (13, 1)]
	[Protocol, Model]
	[BaseType (typeof (NSObject))]
	interface AVRoutePickerViewDelegate {

		/// <param name="routePickerView">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("routePickerViewWillBeginPresentingRoutes:")]
		void WillBeginPresentingRoutes (AVRoutePickerView routePickerView);

		/// <param name="routePickerView">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("routePickerViewDidEndPresentingRoutes:")]
		void DidEndPresentingRoutes (AVRoutePickerView routePickerView);
	}

	[NoiOS, NoMac]
	[NoMacCatalyst]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface AVDisplayManager {

		[NoMacCatalyst]
		[Field ("AVDisplayManagerModeSwitchStartNotification")]
		[Notification]
		NSString ModeSwitchStartNotification { get; }

		[NoMacCatalyst]
		[Field ("AVDisplayManagerModeSwitchEndNotification")]
		[Notification]
		NSString ModeSwitchEndNotification { get; }

		[NoMacCatalyst]
		[Field ("AVDisplayManagerModeSwitchSettingsChangedNotification")]
		[Notification]
		NSString ModeSwitchSettingsChangedNotification { get; }

		[NullAllowed, Export ("preferredDisplayCriteria", ArgumentSemantic.Copy)]
		AVDisplayCriteria PreferredDisplayCriteria { get; set; }

		[Export ("displayModeSwitchInProgress")]
		bool DisplayModeSwitchInProgress { [Bind ("isDisplayModeSwitchInProgress")] get; }

		[NoMacCatalyst]
		[Export ("displayCriteriaMatchingEnabled")]
		bool DisplayCriteriaMatchingEnabled { [Bind ("isDisplayCriteriaMatchingEnabled")] get; }
	}

	[NoiOS, NoMac]
	[NoMacCatalyst]
	[Category]
	[BaseType (typeof (UIWindow))]
	interface UIWindow_AVAdditions {

		[Export ("avDisplayManager")]
		AVDisplayManager GetAVDisplayManager ();
	}

	[NoTV, NoMac, iOS (15, 0), MacCatalyst (15, 0)]
	[BaseType (typeof (UIViewController))]
	interface AVPictureInPictureVideoCallViewController {
		[DesignatedInitializer]
		[Export ("initWithNibName:bundle:")]
		NativeHandle Constructor ([NullAllowed] string nibName, [NullAllowed] NSBundle bundle);
	}

	[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface AVPictureInPictureControllerContentSource {
		[Export ("initWithPlayerLayer:")]
		NativeHandle Constructor (AVPlayerLayer playerLayer);

		[NullAllowed, Export ("playerLayer")]
		AVPlayerLayer PlayerLayer { get; }

		// interface AVPictureInPictureControllerContentSource_VideoCallSupport
		[NoTV, NoMac]
		[Export ("initWithActiveVideoCallSourceView:contentViewController:")]
		NativeHandle Constructor (UIView sourceView, AVPictureInPictureVideoCallViewController contentViewController);

		[NullAllowed]
		[NoTV, NoMac]
		[Export ("activeVideoCallSourceView", ArgumentSemantic.Weak)]
		UIView ActiveVideoCallSourceView { get; }

		[NoTV, NoMac]
		[Export ("activeVideoCallContentViewController")]
		AVPictureInPictureVideoCallViewController ActiveVideoCallContentViewController { get; }

		// interface AVPictureInPictureControllerContentSource_AVSampleBufferDisplayLayerSupport
		[Export ("initWithSampleBufferDisplayLayer:playbackDelegate:")]
		NativeHandle Constructor (AVSampleBufferDisplayLayer sampleBufferDisplayLayer, IAVPictureInPictureSampleBufferPlaybackDelegate playbackDelegate);

		[NullAllowed, Export ("sampleBufferDisplayLayer")]
		AVSampleBufferDisplayLayer SampleBufferDisplayLayer { get; }

		[Wrap ("WeakSampleBufferPlaybackDelegate")]
		[NullAllowed]
		IAVPictureInPictureSampleBufferPlaybackDelegate SampleBufferPlaybackDelegate { get; }

		[NullAllowed, Export ("sampleBufferPlaybackDelegate", ArgumentSemantic.Weak)]
		NSObject WeakSampleBufferPlaybackDelegate { get; }
	}

	interface IAVPictureInPictureSampleBufferPlaybackDelegate { }

	[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
	[Protocol, Model]
	[BaseType (typeof (NSObject))]
	interface AVPictureInPictureSampleBufferPlaybackDelegate {
		[Abstract]
		[Export ("pictureInPictureController:setPlaying:")]
		void SetPlaying (AVPictureInPictureController pictureInPictureController, bool playing);

		[Abstract]
		[Export ("pictureInPictureControllerTimeRangeForPlayback:")]
		CMTimeRange GetTimeRange (AVPictureInPictureController pictureInPictureController);

		[Abstract]
		[Export ("pictureInPictureControllerIsPlaybackPaused:")]
		bool IsPlaybackPaused (AVPictureInPictureController pictureInPictureController);

		[Abstract]
		[Export ("pictureInPictureController:didTransitionToRenderSize:")]
		void DidTransitionToRenderSize (AVPictureInPictureController pictureInPictureController, CMVideoDimensions newRenderSize);

		[Abstract]
		[Export ("pictureInPictureController:skipByInterval:completionHandler:")]
		void SkipByInterval (AVPictureInPictureController pictureInPictureController, CMTime skipInterval, Action completionHandler);

		[Export ("pictureInPictureControllerShouldProhibitBackgroundAudioPlayback:")]
		bool ShouldProhibitBackgroundAudioPlayback (AVPictureInPictureController pictureInPictureController);
	}

	interface IAVPlayerViewDelegate { }

	[NoiOS, NoTV, NoMacCatalyst]
	[Protocol, Model]
	[BaseType (typeof (NSObject))]
	interface AVPlayerViewDelegate {
		[Export ("playerViewWillEnterFullScreen:")]
		void WillEnterFullScreen (AVPlayerView playerView);

		[Export ("playerViewDidEnterFullScreen:")]
		void DidEnterFullScreen (AVPlayerView playerView);

		[Export ("playerViewWillExitFullScreen:")]
		void WillExitFullScreen (AVPlayerView playerView);

		[Export ("playerViewDidExitFullScreen:")]
		void DidExitFullScreen (AVPlayerView playerView);

		[Export ("playerView:restoreUserInterfaceForFullScreenExitWithCompletionHandler:")]
		void RestoreUserInterfaceForFullScreenExit (AVPlayerView playerView, Action<bool> completionHandler);
	}

	[NoiOS]
	[NoTV]
	[NoMacCatalyst]
	[Native]
	public enum AVCaptureViewControlsStyle : long {
		/// <summary>To be added.</summary>
		Inline,
		/// <summary>To be added.</summary>
		Floating,
		/// <summary>To be added.</summary>
		InlineDeviceSelection,
		/// <summary>To be added.</summary>
		Default = Inline,
	}

	[NoiOS]
	[NoTV]
	[NoMacCatalyst]
	[Native]
	public enum AVPlayerViewTrimResult : long {
		/// <summary>To be added.</summary>
		OKButton,
		/// <summary>To be added.</summary>
		CancelButton,
	}

	[TV (16, 0), Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface AVPlaybackSpeed {
		[Static]
		[Export ("systemDefaultSpeeds")]
		AVPlaybackSpeed [] SystemDefaultSpeeds { get; }

		[Export ("initWithRate:localizedName:")]
		NativeHandle Constructor (float rate, string localizedName);

		[Export ("rate")]
		float Rate { get; }

		[Export ("localizedName")]
		string LocalizedName { get; }

		[Export ("localizedNumericName")]
		string LocalizedNumericName { get; }
	}

	[iOS (17, 2), NoMac, MacCatalyst (17, 2), NoTV]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface AVCaptureEvent {
		[Export ("phase")]
		AVCaptureEventPhase Phase { get; }

		[NoTV, NoMacCatalyst, NoMac, iOS (26, 0)]
		[Export ("playSound:")]
		bool PlaySound (AVCaptureEventSound sound);

		[NoTV, NoMacCatalyst, NoMac, iOS (26, 0)]
		[Export ("shouldPlaySound")]
		bool ShouldPlaySound { get; }
	}

	[iOS (17, 2), NoMac, MacCatalyst (17, 2), NoTV]
	[Native]
	public enum AVCaptureEventPhase : ulong {
		Began,
		Ended,
		Cancelled,
	}

	[iOS (17, 2), NoMac, MacCatalyst (17, 2), NoTV]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface AVCaptureEventInteraction : UIInteraction {
		[Export ("initWithEventHandler:")]
		NativeHandle Constructor (Action<AVCaptureEvent> handler);

		[Export ("initWithPrimaryEventHandler:secondaryEventHandler:")]
		NativeHandle Constructor (Action<AVCaptureEvent> primaryHandler, Action<AVCaptureEvent> secondaryHandler);

		[Export ("enabled")]
		bool Enabled { [Bind ("isEnabled")] get; set; }

		[NoTV, NoMacCatalyst, NoMac, iOS (26, 0)]
		[Static]
		[Export ("defaultCaptureSoundDisabled")]
		bool DefaultCaptureSoundDisabled { get; set; }
	}

	[TV (17, 0), NoMac, NoiOS, NoMacCatalyst]
	[BaseType (typeof (UIViewController))]
	interface AVContinuityDevicePickerViewController {
		[Static]
		[Export ("supported")]
		bool Supported { [Bind ("isSupported")] get; }

		[Wrap ("WeakDelegate")]
		[NullAllowed]
		IAVContinuityDevicePickerViewControllerDelegate Delegate { get; set; }

		[NullAllowed, Export ("delegate", ArgumentSemantic.Weak)]
		NSObject WeakDelegate { get; set; }
	}

	[TV (17, 0), NoMac, NoiOS, NoMacCatalyst]
	[Protocol (BackwardsCompatibleCodeGeneration = false), Model]
	[BaseType (typeof (NSObject))]
	interface AVContinuityDevicePickerViewControllerDelegate {
		[Export ("continuityDevicePickerWillBeginPresenting:")]
		void WillBeginPresenting (AVContinuityDevicePickerViewController pickerViewController);

		[Export ("continuityDevicePicker:didConnectDevice:")]
		void DidConnectDevice (AVContinuityDevicePickerViewController pickerViewController, AVContinuityDevice device);

		[Export ("continuityDevicePickerDidCancel:")]
		void DidCancel (AVContinuityDevicePickerViewController pickerViewController);

		[Export ("continuityDevicePickerDidEndPresenting:")]
		void DidEndPresenting (AVContinuityDevicePickerViewController pickerViewController);
	}
	interface IAVContinuityDevicePickerViewControllerDelegate { }

	[NoTV, NoMacCatalyst, NoMac, iOS (26, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface AVCaptureEventSound {
		[Export ("initWithURL:error:")]
		[Internal]
		NativeHandle _InitWithUrl (NSUrl url, [NullAllowed] out NSError error);

		[Static]
		[Export ("cameraShutterSound")]
		AVCaptureEventSound CameraShutterSound { get; }

		[Static]
		[Export ("beginVideoRecordingSound")]
		AVCaptureEventSound BeginVideoRecordingSound { get; }

		[Static]
		[Export ("endVideoRecordingSound")]
		AVCaptureEventSound EndVideoRecordingSound { get; }
	}

	interface IAVInputPickerInteractionDelegate { }

	[NoTV, NoMacCatalyst, NoMac, iOS (26, 0)]
	[Protocol (BackwardsCompatibleCodeGeneration = false), Model]
	[BaseType (typeof (NSObject))]
	interface AVInputPickerInteractionDelegate {
		[Export ("inputPickerInteractionWillBeginPresenting:")]
		void WillBeginPresenting (AVInputPickerInteraction inputPickerInteraction);

		[Export ("inputPickerInteractionDidEndPresenting:")]
		void DidEndPresenting (AVInputPickerInteraction inputPickerInteraction);

		[Export ("inputPickerInteractionWillBeginDismissing:")]
		void WillBeginDismissing (AVInputPickerInteraction inputPickerInteraction);

		[Export ("inputPickerInteractionDidEndDismissing:")]
		void DidEndDismissing (AVInputPickerInteraction inputPickerInteraction);
	}

	[NoTV, NoMacCatalyst, NoMac, iOS (26, 0)]
	[BaseType (typeof (NSObject))]
	interface AVInputPickerInteraction : UIInteraction {
		[Wrap ("WeakDelegate")]
		[NullAllowed]
		IAVInputPickerInteractionDelegate Delegate { get; set; }

		[NullAllowed, Export ("delegate", ArgumentSemantic.Weak)]
		NSObject WeakDelegate { get; set; }

		[Export ("presented")]
		bool Presented { [Bind ("isPresented")] get; }

		[Export ("audioSession", ArgumentSemantic.Strong)]
		AVAudioSession AudioSession { get; set; }

		[Export ("present")]
		void Present ();

		[Export ("dismiss")]
		void Dismiss ();

		[Export ("initWithAudioSession:")]
		NativeHandle Constructor ([NullAllowed] AVAudioSession audioSession);
	}

	[MacCatalyst (26, 0), NoTV, Mac (26, 0), iOS (26, 0)]
	[Native]
	public enum AVDisplayDynamicRange : long {
		Automatic = 0,
		Standard = 1,
		ConstrainedHigh = 2,
		High = 3,
	}
}
