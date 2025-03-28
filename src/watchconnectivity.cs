//
// WatchConnectivity bindings
//
// Authors:
//	Alex Soto  <alex.soto@xamarin.com>
//
// Copyright 2015 Xamarin Inc. All rights reserved.
//

using System;
using System.ComponentModel;
using ObjCRuntime;
using Foundation;

namespace WatchConnectivity {

	/// <summary>The reply handler for use with <see cref="M:WatchConnectivity.WCSessionDelegate_Extensions.DidReceiveMessageData(WatchConnectivity.IWCSessionDelegate,WatchConnectivity.WCSession,Foundation.NSData,WatchConnectivity.WCSessionReplyDataHandler)" />.</summary>
	delegate void WCSessionReplyHandler (NSDictionary<NSString, NSObject> replyMessage);
	/// <summary>The reply handler for use with <format type="text/html"><a href="https://docs.microsoft.com/en-us/search/index?search=Watch%20Connectivity%20WCSession%20Delegate%20Did%20Receive%20Message%20Data&amp;scope=Xamarin" title="M:WatchConnectivity.WCSessionDelegate.DidReceiveMessageData*">M:WatchConnectivity.WCSessionDelegate.DidReceiveMessageData*</a></format>.</summary>
	delegate void WCSessionReplyDataHandler (NSData replyMessage);

	/// <summary>Mediates the transfer of information between a WatchKit extension app and the container app on the device.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/WatchConnectivity/Reference/WCSession_class/index.html">Apple documentation for <c>WCSession</c></related>
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface WCSession {

		/// <summary>Whether the current device supports <see cref="T:WatchConnectivity.WCSession" /> objects.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Static]
		[Export ("isSupported")]
		bool IsSupported { get; }

		/// <summary>Produces the shared view of the app's <see cref="T:WatchConnectivity.WCSession" /> on the current device.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Static]
		[Export ("defaultSession")]
		WCSession DefaultSession { get; }

		/// <summary>An instance of the WatchConnectivity.IWCSessionDelegate model class which acts as the class delegate.</summary>
		///         <value>The instance of the WatchConnectivity.IWCSessionDelegate model class</value>
		///         <remarks>
		///           <para>The delegate instance assigned to this object will be used to handle events or provide data on demand to this class.</para>
		///           <para>When setting the Delegate or WeakDelegate values events will be delivered to the specified instance instead of being delivered to the C#-style events</para>
		///           <para>This is the strongly typed version of the object, developers should use the WeakDelegate property instead if they want to merely assign a class derived from NSObject that has been decorated with [Export] attributes.</para>
		///         </remarks>
		[Export ("delegate", ArgumentSemantic.Weak)]
		[NullAllowed]
		IWCSessionDelegate Delegate { get; set; }

		[Export ("activateSession")]
		void ActivateSession ();

		/// <summary>Whether the current iPhone is paired to an Apple Watch.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("paired")]
		bool Paired { [Bind ("isPaired")] get; }

		/// <summary>Whether the watch app is installed on the paired Apple Watch.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("watchAppInstalled")]
		bool WatchAppInstalled { [Bind ("isWatchAppInstalled")] get; }

		/// <summary>Whether this application's complication is in use on the watch face.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("complicationEnabled")]
		bool ComplicationEnabled { [Bind ("isComplicationEnabled")] get; }

		/// <summary>The directory in which information about the paired Apple Watch is stored.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("watchDirectoryURL")]
		[NullAllowed]
		NSUrl WatchDirectoryUrl { get; }

		/// <summary>Whether the paired device is reachable.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("reachable")]
		bool Reachable { [Bind ("isReachable")] get; }

		[NoiOS]
		[Export ("iOSDeviceNeedsUnlockAfterRebootForReachability")]
		bool iOSDeviceNeedsUnlockAfterRebootForReachability { get; }

		[Export ("sendMessage:replyHandler:errorHandler:")]
		void SendMessage (NSDictionary<NSString, NSObject> message, [NullAllowed] WCSessionReplyHandler replyHandler, [NullAllowed] Action<NSError> errorHandler);

		[Export ("sendMessageData:replyHandler:errorHandler:")]
		void SendMessage (NSData data, [NullAllowed] WCSessionReplyDataHandler replyHandler, [NullAllowed] Action<NSError> errorHandler);

		/// <summary>The most recent contextual data sent to the companion app.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("applicationContext", ArgumentSemantic.Copy)]
		NSDictionary<NSString, NSObject> ApplicationContext { get; }

		[Export ("updateApplicationContext:error:")]
		bool UpdateApplicationContext (NSDictionary<NSString, NSObject> applicationContext, out NSError error);

		/// <summary>The most recent data sent from the companion app.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("receivedApplicationContext", ArgumentSemantic.Copy)]
		NSDictionary<NSString, NSObject> ReceivedApplicationContext { get; }

		[Export ("transferUserInfo:")]
		WCSessionUserInfoTransfer TransferUserInfo (NSDictionary<NSString, NSObject> userInfo);

		[Export ("transferCurrentComplicationUserInfo:")]
		WCSessionUserInfoTransfer TransferCurrentComplicationUserInfo (NSDictionary<NSString, NSObject> userInfo);

		/// <summary>The currently in-progress data transfers.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("outstandingUserInfoTransfers", ArgumentSemantic.Copy)]
		WCSessionUserInfoTransfer [] OutstandingUserInfoTransfers { get; }

		[Export ("transferFile:metadata:")]
		WCSessionFileTransfer TransferFile (NSUrl file, [NullAllowed] NSDictionary<NSString, NSObject> metadata);

		/// <summary>The currently in-progress file transfers.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("outstandingFileTransfers", ArgumentSemantic.Copy)]
		WCSessionFileTransfer [] OutstandingFileTransfers { get; }

		/// <summary>Gets the error domain in which errors are reported.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("WCErrorDomain")]
		NSString ErrorDomain { get; }

		/// <summary>Get the activation state of the session.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("activationState")]
		WCSessionActivationState ActivationState { get; }

		/// <summary>Gets a Boolean value that tells whether there is more content to transfer.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("hasContentPending")]
		bool HasContentPending { get; }

		/// <summary>Gets the number of remaining times that complication data can be sent to the extension.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("remainingComplicationUserInfoTransfers")]
		nuint RemainingComplicationUserInfoTransfers { get; }

		[NoiOS]
		[Export ("companionAppInstalled")]
		bool CompanionAppInstalled { [Bind ("isCompanionAppInstalled")] get; }
	}

	/// <summary>Interface representing the required methods (if any) of the protocol <see cref="T:WatchConnectivity.WCSessionDelegate" />.</summary>
	///     <remarks>
	///       <para>This interface contains the required methods (if any) from the protocol defined by <see cref="T:WatchConnectivity.WCSessionDelegate" />.</para>
	///       <para>If developers create classes that implement this interface, the implementation methods will automatically be exported to Objective-C with the matching signature from the method defined in the <see cref="T:WatchConnectivity.WCSessionDelegate" /> protocol.</para>
	///       <para>Optional methods (if any) are provided by the <see cref="T:WatchConnectivity.WCSessionDelegate_Extensions" /> class as extension methods to the interface, allowing developers to invoke any optional methods on the protocol.</para>
	///     </remarks>
	interface IWCSessionDelegate { }

	/// <summary>Delegate object whose methods, when overridden, allow the app developer to respond to messages sent between a WatchKit extension app and it's container app.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/WatchConnectivity/Reference/WCSessionDelegate_protocol/index.html">Apple documentation for <c>WCSessionDelegate</c></related>
	[Protocol, Model]
	[BaseType (typeof (NSObject))]
	interface WCSessionDelegate {
		[Export ("sessionWatchStateDidChange:")]
		void SessionWatchStateDidChange (WCSession session);

		[Export ("sessionReachabilityDidChange:")]
		void SessionReachabilityDidChange (WCSession session);

		[Export ("session:didReceiveMessage:")]
		void DidReceiveMessage (WCSession session, NSDictionary<NSString, NSObject> message);

		[Export ("session:didReceiveMessage:replyHandler:")]
		void DidReceiveMessage (WCSession session, NSDictionary<NSString, NSObject> message, WCSessionReplyHandler replyHandler);

		[Export ("session:didReceiveMessageData:")]
		void DidReceiveMessageData (WCSession session, NSData messageData);

		[Export ("session:didReceiveMessageData:replyHandler:")]
		void DidReceiveMessageData (WCSession session, NSData messageData, WCSessionReplyDataHandler replyHandler);

		[Export ("session:didReceiveApplicationContext:")]
		void DidReceiveApplicationContext (WCSession session, NSDictionary<NSString, NSObject> applicationContext);

		[Export ("session:didFinishUserInfoTransfer:error:")]
		void DidFinishUserInfoTransfer (WCSession session, WCSessionUserInfoTransfer userInfoTransfer, [NullAllowed] NSError error);

		[Export ("session:didReceiveUserInfo:")]
		void DidReceiveUserInfo (WCSession session, NSDictionary<NSString, NSObject> userInfo);

		[Export ("session:didFinishFileTransfer:error:")]
		void DidFinishFileTransfer (WCSession session, WCSessionFileTransfer fileTransfer, [NullAllowed] NSError error);

		[Export ("session:didReceiveFile:")]
		void DidReceiveFile (WCSession session, WCSessionFile file);

#if NET
		[Abstract] // OS 10 beta 1 SDK made this required
#endif
		[Export ("session:activationDidCompleteWithState:error:")]
		void ActivationDidComplete (WCSession session, WCSessionActivationState activationState, [NullAllowed] NSError error);

#if NET
		[Abstract] // OS 10 beta 1 SDK made this required
#endif
		[Export ("sessionDidBecomeInactive:")]
		void DidBecomeInactive (WCSession session);

#if NET
		[Abstract] // OS 10 beta 1 SDK made this required
#endif
		[Export ("sessionDidDeactivate:")]
		void DidDeactivate (WCSession session);

		[NoiOS]
		[Export ("sessionCompanionAppInstalledDidChange:")]
		void CompanionAppInstalledDidChange (WCSession session);
	}

	/// <summary>Holds data relating to a file being transferred between a WatchKit extension app and it's container app.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/WatchConnectivity/Reference/WCSessionFile_class/index.html">Apple documentation for <c>WCSessionFile</c></related>
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor] // no handle, doc: You do not create instances of this class directly.
	interface WCSessionFile {

		/// <summary>The URL to a received file.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("fileURL")]
		NSUrl FileUrl { get; }

		/// <summary>Additional data sent with a received file.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed]
		[Export ("metadata", ArgumentSemantic.Copy)]
		NSDictionary<NSString, NSObject> Metadata { get; }
	}

	/// <summary>Represents an ongoing file transfer between a WatchKit extension app and it's container app.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/WatchConnectivity/Reference/WCSessionFileTransfer_class/index.html">Apple documentation for <c>WCSessionFileTransfer</c></related>
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor] // no handle, doc: You do not create instances of this class yourself.
	interface WCSessionFileTransfer {

		/// <summary>The file being transferred.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("file")]
		WCSessionFile File { get; }

		/// <summary>Gets a Boolean value the tells whether the transfer is currently happening.</summary>
		///         <value>A Boolean value the tells whether the transfer is currently happening.</value>
		///         <remarks>To be added.</remarks>
		[Export ("transferring")]
		bool Transferring { [Bind ("isTransferring")] get; }

		[Export ("cancel")]
		void Cancel ();

		/// <summary>Gets the progress indicator for the file transfer.</summary>
		///         <value>The progress indicator for the file transfer.</value>
		///         <remarks>To be added.</remarks>
		[Export ("progress")]
		NSProgress Progress { get; }
	}

	/// <summary>Represents an ongoing data transfer between a WatchKit extension app and it's container app.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/WatchConnectivity/Reference/WCSessionUserInfoTransfer_class/index.html">Apple documentation for <c>WCSessionUserInfoTransfer</c></related>
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor] // no handle, doc: You do not create instances of this class yourself.
	interface WCSessionUserInfoTransfer : NSSecureCoding {

		/// <summary>Whether the data being transferred relates to a complication.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("currentComplicationInfo")]
		bool CurrentComplicationInfo { [Bind ("isCurrentComplicationInfo")] get; }

		/// <summary>The data being transferred.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("userInfo", ArgumentSemantic.Copy)]
		NSDictionary<NSString, NSObject> UserInfo { get; }

		/// <summary>Whether the transfer is currently happening.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("transferring")]
		bool Transferring { [Bind ("isTransferring")] get; }

		[Export ("cancel")]
		void Cancel ();
	}

}
