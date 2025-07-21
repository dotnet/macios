//
// ReplayKit bindings
//
// Authors:
//	Alex Soto  <alex.soto@xamarin.com>
//
// Copyright 2015 Xamarin Inc. All rights reserved.
//

using System;
using AVFoundation;
using CoreMedia;
using ObjCRuntime;
using Foundation;
using CoreGraphics;
#if MONOMAC
using AppKit;
using UIImage = AppKit.NSImage;
using UIViewController = AppKit.NSViewController;
using UIView = AppKit.NSView;
#else
using UIKit;
using NSWindow = Foundation.NSObject;
#endif

namespace ReplayKit {

	/// <summary>A view controller for previewing and editing a ReplayKit recording.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/reference/ReplayKit/RPPreviewViewController">Apple documentation for <c>RPPreviewViewController</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (UIViewController))]
	interface RPPreviewViewController {
		/// <param name="nibName">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		/// <param name="bundle">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		/// <summary>Creates a new Replay Kit preview controller from the named NIB in the specified <paramref name="bundle" />.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("initWithNibName:bundle:")]
		[PostGet ("NibBundle")]
		NativeHandle Constructor ([NullAllowed] string nibName, [NullAllowed] NSBundle bundle);

		[Export ("previewControllerDelegate", ArgumentSemantic.Weak)]
		[NullAllowed]
		IRPPreviewViewControllerDelegate PreviewControllerDelegate { get; set; }

		[NoiOS]
		[NoMac]
		[NoMacCatalyst]
		[Export ("mode", ArgumentSemantic.Assign)]
		RPPreviewViewControllerMode Mode { get; set; }
	}

	/// <summary>Interface representing the required methods (if any) of the protocol <see cref="ReplayKit.RPPreviewViewControllerDelegate" />.</summary>
	///     <remarks>
	///       <para>This interface contains the required methods (if any) from the protocol defined by <see cref="ReplayKit.RPPreviewViewControllerDelegate" />.</para>
	///       <para>If developers create classes that implement this interface, the implementation methods will automatically be exported to Objective-C with the matching signature from the method defined in the <see cref="ReplayKit.RPPreviewViewControllerDelegate" /> protocol.</para>
	///       <para>Optional methods (if any) are provided by the <see cref="ReplayKit.RPPreviewViewControllerDelegate_Extensions" /> class as extension methods to the interface, allowing developers to invoke any optional methods on the protocol.</para>
	///     </remarks>
	interface IRPPreviewViewControllerDelegate { }

	/// <summary>The view controller protocol for previewing and editing a ReplayKit recording.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/ReplayKit/Reference/RPPreviewViewControllerDelegate_Ref/index.html">Apple documentation for <c>RPPreviewViewControllerDelegate</c></related>
	[MacCatalyst (13, 1)]
	[Protocol, Model]
	[BaseType (typeof (NSObject))]
	interface RPPreviewViewControllerDelegate {

		/// <param name="previewController">To be added.</param>
		/// <summary>Method that is called when the previewer is ready to be dismissed.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("previewControllerDidFinish:")]
		void DidFinish (RPPreviewViewController previewController);

		/// <param name="previewController">To be added.</param>
		/// <param name="activityTypes">To be added.</param>
		/// <summary>Method that is called when the previewer is ready to be dismissed.</summary>
		/// <remarks>To be added.</remarks>
		[NoTV]
		[MacCatalyst (13, 1)]
		[Export ("previewController:didFinishWithActivityTypes:")]
		void DidFinish (RPPreviewViewController previewController, NSSet<NSString> activityTypes);
	}

	/// <summary>Enables the user to record visual and audio output of applications, with simultaneous recorded audio (screencasts).</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/ReplayKit/Reference/RPScreenRecorder_Ref/index.html">Apple documentation for <c>RPScreenRecorder</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	[Sealed]
	interface RPScreenRecorder {

		[Static]
		[Export ("sharedRecorder")]
		RPScreenRecorder SharedRecorder { get; }

		[NoMac]
		[Deprecated (PlatformName.iOS, 10, 0, message: "Use 'StartRecording (Action<NSError>)' instead.")]
		[Deprecated (PlatformName.TvOS, 10, 0, message: "Use 'StartRecording (Action<NSError>)' instead.")]
		[MacCatalyst (13, 1)]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'StartRecording (Action<NSError>)' instead.")]
		[Async (XmlDocs = """
			<param name="microphoneEnabled">To be added.</param>
			<summary>Asynchronously starts recording the screen, controlling whether recording is enabled.</summary>
			<returns>A task that represents the asynchronous StartRecording operation</returns>
			<remarks>To be added.</remarks>
			""")]
		[Export ("startRecordingWithMicrophoneEnabled:handler:")]
		void StartRecording (bool microphoneEnabled, [NullAllowed] Action<NSError> handler);

		[MacCatalyst (13, 1)]
		[Async (XmlDocs = """
			<summary>Starts the recording and runs a handler when the recording starts.</summary>
			<returns>A task that represents the asynchronous StartRecording operation</returns>
			<remarks>To be added.</remarks>
			""")]
		[Export ("startRecordingWithHandler:")]
		void StartRecording ([NullAllowed] Action<NSError> handler);

		[Async (XmlDocs = """
			<summary>Stops the recording and runs a handler when the recording stops.</summary>
			<returns>
			          <para class="improve-task-t-return-type-description">A task that represents the asynchronous StopRecording operation.  The value of the TResult parameter is of type System.Action&lt;ReplayKit.RPPreviewViewController,Foundation.NSError&gt;.</para>
			        </returns>
			<remarks>To be added.</remarks>
			""")]
		[Export ("stopRecordingWithHandler:")]
		void StopRecording ([NullAllowed] Action<RPPreviewViewController, NSError> handler);

		[Async (XmlDocs = """
			<summary>Discards the recording.</summary>
			<returns>A task that represents the asynchronous DiscardRecording operation</returns>
			<remarks>To be added.</remarks>
			""")]
		[Export ("discardRecordingWithHandler:")]
		void DiscardRecording (Action handler);

		[Export ("delegate", ArgumentSemantic.Weak)]
		[NullAllowed]
		IRPScreenRecorderDelegate Delegate { get; set; }

		/// <summary>Gets a Boolean that tells whether the screen is being recording.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("recording", ArgumentSemantic.Assign)]
		bool Recording { [Bind ("isRecording")] get; }

		/// <summary>Gets or sets a Boolean value that enables or disables the microphone.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NoTV]
		[MacCatalyst (13, 1)]
		[Export ("microphoneEnabled", ArgumentSemantic.Assign)]
		bool MicrophoneEnabled {
			[Bind ("isMicrophoneEnabled")]
			get;
			[MacCatalyst (13, 1)]
			set;
		}

		/// <summary>Gets a Boolean value that tells whether the screen recorder is available.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("available", ArgumentSemantic.Assign)]
		bool Available { [Bind ("isAvailable")] get; }

		/// <summary>Gets or sest a Boolean value that controls whether the camera enabled.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NoTV]
		[MacCatalyst (13, 1)]
		[Export ("cameraEnabled")]
		bool CameraEnabled { [Bind ("isCameraEnabled")] get; set; }

		[NoTV]
		[MacCatalyst (13, 1)]
		[NullAllowed, Export ("cameraPreviewView")]
		UIView CameraPreviewView { get; }

		[NoTV]
		[MacCatalyst (13, 1)]
		[Export ("cameraPosition", ArgumentSemantic.Assign)]
		RPCameraPosition CameraPosition { get; set; }

		[MacCatalyst (13, 1)]
		[Async (XmlDocs = """
			<param name="captureHandler">To be added.</param>
			<summary>Starts recording.</summary>
			<returns>A task that represents the asynchronous StartCapture operation</returns>
			<remarks>To be added.</remarks>
			""")]
		[Export ("startCaptureWithHandler:completionHandler:")]
		void StartCapture ([NullAllowed] Action<CMSampleBuffer, RPSampleBufferType, NSError> captureHandler, [NullAllowed] Action<NSError> completionHandler);

		[MacCatalyst (13, 1)]
		[Async (XmlDocs = """
			<summary>Stops screen and audio recording.</summary>
			<returns>A task that represents the asynchronous StopCapture operation</returns>
			<remarks>
			          <para copied="true">The StopCaptureAsync method is suitable to be used with C# async by returning control to the caller with a Task representing the operation.</para>
			          <para copied="true">To be added.</para>
			        </remarks>
			""")]
		[Export ("stopCaptureWithHandler:")]
		void StopCapture ([NullAllowed] Action<NSError> handler);

		[Introduced (PlatformName.MacCatalyst, 14, 0)]
		[TV (14, 0), iOS (14, 0)]
		[Async]
		[Export ("stopRecordingWithOutputURL:completionHandler:")]
		void StopRecording (NSUrl url, [NullAllowed] Action<NSError> completionHandler);

		[Async]
		[TV (15, 4), iOS (15, 0), MacCatalyst (15, 0)]
		[Export ("exportClipToURL:duration:completionHandler:")]
		void ExportClip (NSUrl url, double duration, [NullAllowed] Action<NSError> completionHandler);

		[Async]
		[TV (15, 4), iOS (15, 0), MacCatalyst (15, 0)]
		[Export ("startClipBufferingWithCompletionHandler:")]
		void StartClipBuffering ([NullAllowed] Action<NSError> completionHandler);

		[Async]
		[TV (15, 4), iOS (15, 0), MacCatalyst (15, 0)]
		[Export ("stopClipBufferingWithCompletionHandler:")]
		void StopClipBuffering ([NullAllowed] Action<NSError> completionHandler);
	}

	/// <summary>Interface representing the required methods (if any) of the protocol <see cref="ReplayKit.RPScreenRecorderDelegate" />.</summary>
	///     <remarks>
	///       <para>This interface contains the required methods (if any) from the protocol defined by <see cref="ReplayKit.RPScreenRecorderDelegate" />.</para>
	///       <para>If developers create classes that implement this interface, the implementation methods will automatically be exported to Objective-C with the matching signature from the method defined in the <see cref="ReplayKit.RPScreenRecorderDelegate" /> protocol.</para>
	///       <para>Optional methods (if any) are provided by the <see cref="ReplayKit.RPScreenRecorderDelegate_Extensions" /> class as extension methods to the interface, allowing developers to invoke any optional methods on the protocol.</para>
	///     </remarks>
	interface IRPScreenRecorderDelegate { }

	/// <summary>Protocol for enabling the user to record visual and audio output of applications, with simultaneous recorded audio (screencasts)..</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/ReplayKit/Reference/RPScreenRecorderDelegate_Ref/index.html">Apple documentation for <c>RPScreenRecorderDelegate</c></related>
	[MacCatalyst (13, 1)]
	[Protocol, Model]
	[BaseType (typeof (NSObject))]
	interface RPScreenRecorderDelegate {

		/// <param name="screenRecorder">To be added.</param>
		/// <param name="error">To be added.</param>
		/// <param name="previewViewController">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		/// <summary>Developers should not use this deprecated method. Developers should use 'DidStopRecording(RPScreenRecorder,RPPreviewViewController,NSError)' instead.</summary>
		/// <remarks>To be added.</remarks>
		[Deprecated (PlatformName.TvOS, 10, 0, message: "Use 'DidStopRecording(RPScreenRecorder,RPPreviewViewController,NSError)' instead.")]
		[Deprecated (PlatformName.iOS, 10, 0, message: "Use 'DidStopRecording(RPScreenRecorder,RPPreviewViewController,NSError)' instead.")]
		[NoMac]
		[MacCatalyst (13, 1)]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'DidStopRecording(RPScreenRecorder,RPPreviewViewController,NSError)' instead.")]
		[Export ("screenRecorder:didStopRecordingWithError:previewViewController:")]
		void DidStopRecording (RPScreenRecorder screenRecorder, NSError error, [NullAllowed] RPPreviewViewController previewViewController);

		/// <param name="screenRecorder">To be added.</param>
		/// <param name="previewViewController">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		/// <param name="error">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("screenRecorder:didStopRecordingWithPreviewViewController:error:")]
		void DidStopRecording (RPScreenRecorder screenRecorder, [NullAllowed] RPPreviewViewController previewViewController, [NullAllowed] NSError error);

		/// <param name="screenRecorder">To be added.</param>
		/// <summary>Method that is called when the availability status changes.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("screenRecorderDidChangeAvailability:")]
		void DidChangeAvailability (RPScreenRecorder screenRecorder);
	}

	/// <summary>Presents a user interface for choosing third-party broadcast services.</summary>
	///     <remarks>The view controller displays the currently installed broadcast services.</remarks>
	///     <related type="externalDocumentation" href="https://developer.apple.com/reference/ReplayKit/RPBroadcastActivityViewController">Apple documentation for <c>RPBroadcastActivityViewController</c></related>
	[NoMac]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (UIViewController))]
	interface RPBroadcastActivityViewController {
		// inlined
		/// <param name="nibName">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		/// <param name="bundle">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("initWithNibName:bundle:")]
		[PostGet ("NibBundle")]
		NativeHandle Constructor ([NullAllowed] string nibName, [NullAllowed] NSBundle bundle);

		[Static]
		[Async (XmlDocs = """
			<summary>Asynchronously presents the UI for choosing a broadcast activity view controller, and attempts to load the user's choice.</summary>
			<returns>A task that asynchronously presents the UI for choosing a broadcast activity view controller and attempts to load the user's choice.</returns>
			<remarks>
			          <para>The LoadBroadcastActivityViewControllerAsync method is suitable to be used with C# async by returning control to the caller with a Task representing the operation.</para>
			          <para copied="true">The LoadBroadcastActivityViewControllerAsync method is suitable to be used with C# async by returning control to the caller with a Task representing the operation.</para>
			          <para copied="true">To be added.</para>
			        </remarks>
			""")]
		[Export ("loadBroadcastActivityViewControllerWithHandler:")]
		void LoadBroadcastActivityViewController (Action<RPBroadcastActivityViewController, NSError> handler);

		[NullAllowed, Export ("delegate", ArgumentSemantic.Weak)]
		IRPBroadcastActivityViewControllerDelegate Delegate { get; set; }

		[NoTV]
		[MacCatalyst (13, 1)]
		[Static]
		[Async (XmlDocs = """
			<param name="preferredExtension">To be added.</param>
			<summary>Asynchronously presents the UI for choosing a broadcast activity view controller, attempts to load the user's choice.</summary>
			<returns>To be added.</returns>
			<remarks>To be added.</remarks>
			""")]
		[Export ("loadBroadcastActivityViewControllerWithPreferredExtension:handler:")]
		void LoadBroadcastActivityViewController ([NullAllowed] string preferredExtension, Action<RPBroadcastActivityViewController, NSError> handler);
	}

	/// <include file="../docs/api/ReplayKit/IRPBroadcastActivityViewControllerDelegate.xml" path="/Documentation/Docs[@DocId='T:ReplayKit.IRPBroadcastActivityViewControllerDelegate']/*" />
	interface IRPBroadcastActivityViewControllerDelegate { }

	/// <summary>Responds to changes in the UI that is presented by a <see cref="ReplayKit.RPBroadcastActivityViewController" />.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/reference/ReplayKit/RPBroadcastActivityViewControllerDelegate">Apple documentation for <c>RPBroadcastActivityViewControllerDelegate</c></related>
	[NoMac]
	[MacCatalyst (13, 1)]
	[Protocol, Model]
	[BaseType (typeof (NSObject))]
	interface RPBroadcastActivityViewControllerDelegate {
		/// <param name="broadcastActivityViewController">The selection UI to be dismissed. Optional. <see langword="null" /> if the user canceled setup.</param>
		/// <param name="broadcastController">
		///           <para>The broadcast controller.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		/// <param name="error">The error that occurred, if present. Otherwise, <see langword="null" />.<para tool="nullallowed">This parameter can be <see langword="null" />.</para></param>
		/// <summary>Method that is called when the broadcast activity view controller selection UI is about to be dismissed.</summary>
		/// <remarks>If <paramref name="error" /> is <see langword="null" /> then the system is configured for broadcasting.</remarks>
		[Abstract]
		[Export ("broadcastActivityViewController:didFinishWithBroadcastController:error:")]
		void DidFinish (RPBroadcastActivityViewController broadcastActivityViewController, [NullAllowed] RPBroadcastController broadcastController, [NullAllowed] NSError error);
	}

	/// <summary>Manages an audio or video broadcast stream.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/reference/ReplayKit/RPBroadcastController">Apple documentation for <c>RPBroadcastController</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	interface RPBroadcastController {
		/// <summary>Gets a boolean value that tells whether the extension is currently broadcasting.</summary>
		///         <value>A boolean value that tells whether the extension is currently broadcasting.</value>
		///         <remarks>To be added.</remarks>
		[Export ("broadcasting")]
		bool Broadcasting { [Bind ("isBroadcasting")] get; }

		/// <summary>Gets a value that tells whether the broadcast is paused.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("paused")]
		bool Paused { [Bind ("isPaused")] get; }

		[Export ("broadcastURL")]
		NSUrl BroadcastUrl { get; }

		[NullAllowed, Export ("serviceInfo")]
		NSDictionary<NSString, INSCoding> ServiceInfo { get; }

		[NullAllowed, Export ("delegate", ArgumentSemantic.Weak)]
		IRPBroadcastControllerDelegate Delegate { get; set; }

		[Deprecated (PlatformName.TvOS, 11, 0)]
		[Deprecated (PlatformName.iOS, 11, 0)]
		[NoMac]
		[MacCatalyst (13, 1)]
		[Deprecated (PlatformName.MacCatalyst, 13, 1)]
		[Export ("broadcastExtensionBundleID")]
		[NullAllowed]
		string BroadcastExtensionBundleID { get; }

		[Async (XmlDocs = """
			<summary>Starts a new broadcast.</summary>
			<returns>A task that represents the asynchronous StartBroadcast operation</returns>
			<remarks>To be added.</remarks>
			""")]
		[Export ("startBroadcastWithHandler:")]
		void StartBroadcast (Action<NSError> handler);

		[Export ("pauseBroadcast")]
		void PauseBroadcast ();

		[Export ("resumeBroadcast")]
		void ResumeBroadcast ();

		[Async (XmlDocs = """
			<summary>Ends the broadcast.</summary>
			<returns>A task that represents the asynchronous FinishBroadcast operation</returns>
			<remarks>
			          <para copied="true">The FinishBroadcastAsync method is suitable to be used with C# async by returning control to the caller with a Task representing the operation.</para>
			          <para copied="true">To be added.</para>
			        </remarks>
			""")]
		[Export ("finishBroadcastWithHandler:")]
		void FinishBroadcast (Action<NSError> handler);
	}

	/// <summary>Interface representing the required methods (if any) of the protocol <see cref="ReplayKit.RPBroadcastControllerDelegate" />.</summary>
	///     <remarks>
	///       <para>This interface contains the required methods (if any) from the protocol defined by <see cref="ReplayKit.RPBroadcastControllerDelegate" />.</para>
	///       <para>If developers create classes that implement this interface, the implementation methods will automatically be exported to Objective-C with the matching signature from the method defined in the <see cref="ReplayKit.RPBroadcastControllerDelegate" /> protocol.</para>
	///       <para>Optional methods (if any) are provided by the <see cref="ReplayKit.RPBroadcastControllerDelegate_Extensions" /> class as extension methods to the interface, allowing developers to invoke any optional methods on the protocol.</para>
	///     </remarks>
	interface IRPBroadcastControllerDelegate { }

	/// <summary>Delegate object that responds to changes in a broadcast.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/reference/ReplayKit/RPBroadcastControllerDelegate">Apple documentation for <c>RPBroadcastControllerDelegate</c></related>
	[MacCatalyst (13, 1)]
	[Protocol, Model]
	[BaseType (typeof (NSObject))]
	interface RPBroadcastControllerDelegate {
		/// <param name="broadcastController">The controller for the broadcast that finsihed.</param>
		/// <param name="error">
		///           <para>The error, if any, that ended the broadcast.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("broadcastController:didFinishWithError:")]
		void DidFinish (RPBroadcastController broadcastController, [NullAllowed] NSError error);

		/// <param name="broadcastController">To be added.</param>
		/// <param name="serviceInfo">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("broadcastController:didUpdateServiceInfo:")]
		void DidUpdateServiceInfo (RPBroadcastController broadcastController, NSDictionary<NSString, INSCoding> serviceInfo);

		/// <param name="broadcastController">To be added.</param>
		/// <param name="broadcastUrl">To be added.</param>
		/// <summary>Method that is called when the broadcast URL is updated.</summary>
		/// <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("broadcastController:didUpdateBroadcastURL:")]
		void DidUpdateBroadcastUrl (RPBroadcastController broadcastController, NSUrl broadcastUrl);
	}

	/// <related type="externalDocumentation" href="https://developer.apple.com/reference/ReplayKit/RPBroadcastConfiguration">Apple documentation for <c>RPBroadcastConfiguration</c></related>
	[Deprecated (PlatformName.TvOS, 11, 0)]
	[Deprecated (PlatformName.iOS, 11, 0)]
	[NoMac]
	[MacCatalyst (13, 1)]
	[Deprecated (PlatformName.MacCatalyst, 13, 1)]
	[BaseType (typeof (NSObject))]
	interface RPBroadcastConfiguration : NSCoding, NSSecureCoding {
		[Export ("clipDuration")]
		double ClipDuration { get; set; }

		[NullAllowed, Export ("videoCompressionProperties", ArgumentSemantic.Strong)]
		NSDictionary<NSString, INSSecureCoding> WeakVideoCompressionProperties { get; set; }
	}

	/// <param name="bundleID">The bundle ID of the newly loaded broadcasting service.</param>
	///     <param name="displayName">The display name of the newly loaded broadcasting service.</param>
	///     <param name="appIcon">The application icon of the newly loaded broadcasting service.</param>
	///     <summary>Delegate that specifies the signature of the completion handler in calls to the <see cref="ReplayKit.NSExtensionContext_RPBroadcastExtension.LoadBroadcastingApplicationInfo(Foundation.NSExtensionContext,ReplayKit.LoadBroadcastingHandler)" /> method.</summary>
	delegate void LoadBroadcastingHandler (string bundleID, string displayName, [NullAllowed] UIImage appIcon);

	[MacCatalyst (13, 1)]
	[Category]
	[BaseType (typeof (NSExtensionContext))]
	interface NSExtensionContext_RPBroadcastExtension {
		/// <param name="handler">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("loadBroadcastingApplicationInfoWithCompletion:")]
		void LoadBroadcastingApplicationInfo (LoadBroadcastingHandler handler);

		/// <param name="broadcastURL">To be added.</param>
		/// <param name="broadcastConfiguration">To be added.</param>
		/// <param name="setupInfo">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		/// <summary>Developers should not use this deprecated method. Developers should use 'CompleteRequest(NSUrl,NSDictionary&lt;NSString,INSCoding&gt;)' instead.</summary>
		/// <remarks>To be added.</remarks>
		[Deprecated (PlatformName.TvOS, 11, 0, message: "Use 'CompleteRequest(NSUrl,NSDictionary<NSString,INSCoding>)' instead.")]
		[Deprecated (PlatformName.iOS, 11, 0, message: "Use 'CompleteRequest(NSUrl,NSDictionary<NSString,INSCoding>)' instead.")]
		[NoMac]
		[MacCatalyst (13, 1)]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'CompleteRequest(NSUrl,NSDictionary<NSString,INSCoding>)' instead.")]
		[Export ("completeRequestWithBroadcastURL:broadcastConfiguration:setupInfo:")]
		void CompleteRequest (NSUrl broadcastURL, RPBroadcastConfiguration broadcastConfiguration, [NullAllowed] NSDictionary<NSString, INSCoding> setupInfo);

		/// <param name="broadcastURL">To be added.</param>
		/// <param name="setupInfo">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("completeRequestWithBroadcastURL:setupInfo:")]
		void CompleteRequest (NSUrl broadcastURL, [NullAllowed] NSDictionary<NSString, INSCoding> setupInfo);
	}

	/// <summary>Base class for managing Replay Kit broadcasts.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/reference/ReplayKit/RPBroadcastHandler">Apple documentation for <c>RPBroadcastHandler</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	interface RPBroadcastHandler : NSExtensionRequestHandling {
		[Export ("updateServiceInfo:")]
		void UpdateServiceInfo (NSDictionary<NSString, INSCoding> serviceInfo);

		[Export ("updateBroadcastURL:")]
		void UpdateBroadcastUrl (NSUrl broadcastUrl);
	}

	/// <related type="externalDocumentation" href="https://developer.apple.com/reference/ReplayKit/RPBroadcastMP4ClipHandler">Apple documentation for <c>RPBroadcastMP4ClipHandler</c></related>
	[NoMac]
	[Deprecated (PlatformName.TvOS, 11, 0, message: "Use 'RPBroadcastSampleHandler' instead.")]
	[Deprecated (PlatformName.iOS, 11, 0, message: "Use 'RPBroadcastSampleHandler' instead.")]
	[MacCatalyst (13, 1)]
	[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'RPBroadcastSampleHandler' instead.")]
	[BaseType (typeof (RPBroadcastHandler))]
	interface RPBroadcastMP4ClipHandler {
		[Export ("processMP4ClipWithURL:setupInfo:finished:")]
		void ProcessMP4Clip ([NullAllowed] NSUrl mp4ClipURL, [NullAllowed] NSDictionary<NSString, NSObject> setupInfo, bool finished);

		[Export ("finishedProcessingMP4ClipWithUpdatedBroadcastConfiguration:error:")]
		void FinishedProcessingMP4Clip ([NullAllowed] RPBroadcastConfiguration broadcastConfiguration, [NullAllowed] NSError error);
	}

	/// <summary>Processes ReplayKit buffer obects as they arrive.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/reference/ReplayKit/RPBroadcastSampleHandler">Apple documentation for <c>RPBroadcastSampleHandler</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (RPBroadcastHandler))]
	interface RPBroadcastSampleHandler {

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Field ("RPVideoSampleOrientationKey")]
		NSString VideoSampleOrientationKey { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NoTV]
		[MacCatalyst (13, 1)]
		[Field ("RPApplicationInfoBundleIdentifierKey")]
		NSString ApplicationInfoBundleIdentifierKey { get; }

		[Export ("broadcastStartedWithSetupInfo:")]
		void BroadcastStarted ([NullAllowed] NSDictionary<NSString, NSObject> setupInfo);

		[Export ("broadcastPaused")]
		void BroadcastPaused ();

		[Export ("broadcastResumed")]
		void BroadcastResumed ();

		[Export ("broadcastFinished")]
		void BroadcastFinished ();

		[NoTV]
		[MacCatalyst (13, 1)]
		[Export ("broadcastAnnotatedWithApplicationInfo:")]
		void BroadcastAnnotated (NSDictionary applicationInfo);

		[Export ("processSampleBuffer:withType:")]
		void ProcessSampleBuffer (CMSampleBuffer sampleBuffer, RPSampleBufferType sampleBufferType);

		[MacCatalyst (13, 1)]
		[Export ("finishBroadcastWithError:")]
		void FinishBroadcast (NSError error);
	}

	[NoTV]
	[NoMac]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (UIView))]
	interface RPSystemBroadcastPickerView : NSCoding {

		/// <param name="frame">Frame used by the view, expressed in iOS points.</param>
		/// <summary>Initializes the RPSystemBroadcastPickerView with the specified frame.</summary>
		/// <remarks>
		///           <para>This constructor is used to programmatically create a new instance of RPSystemBroadcastPickerView with the specified dimension in the frame.   The object will only be displayed once it has been added to a view hierarchy by calling AddSubview in a containing view.</para>
		///           <para>This constructor is not invoked when deserializing objects from storyboards or XIB filesinstead the constructor that takes an NSCoder parameter is invoked.</para>
		///         </remarks>
		[Export ("initWithFrame:")]
		NativeHandle Constructor (CGRect frame);

		[NullAllowed, Export ("preferredExtension")]
		string PreferredExtension { get; set; }

		[Export ("showsMicrophoneButton")]
		bool ShowsMicrophoneButton { get; set; }
	}

	[NoiOS]
	[NoTV]
	[NoMacCatalyst]
	[BaseType (typeof (NSObject))]
	interface RPBroadcastActivityController {

		[Static]
		[Export ("showBroadcastPickerAtPoint:fromWindow:preferredExtensionIdentifier:completionHandler:")]
		void ShowBroadcastPicker (CGPoint point, [NullAllowed] NSWindow window, [NullAllowed] string preferredExtension, Action<RPBroadcastActivityController, NSError> handler);

		[Wrap ("WeakDelegate")]
		[NullAllowed]
		IRPBroadcastActivityControllerDelegate Delegate { get; set; }

		[NullAllowed, Export ("delegate", ArgumentSemantic.Weak)]
		NSObject WeakDelegate { get; set; }
	}

	interface IRPBroadcastActivityControllerDelegate { }

	[NoiOS]
	[NoTV]
	[NoMacCatalyst]
	[Protocol, Model]
	[BaseType (typeof (NSObject))]
	interface RPBroadcastActivityControllerDelegate {

		[Abstract]
		[Export ("broadcastActivityController:didFinishWithBroadcastController:error:")]
		void DidFinish (RPBroadcastActivityController broadcastActivityController, [NullAllowed] RPBroadcastController broadcastController, [NullAllowed] NSError error);
	}
}
