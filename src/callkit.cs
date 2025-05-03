//
// CallKit bindings
//
// Authors:
//	Alex Soto  <alex.soto@xamarin.com>
//
// Copyright 2016 Xamarin Inc. All rights reserved.
//

using System;
using AVFoundation;
using CoreFoundation;
using Foundation;
using ObjCRuntime;

#if !NET
using NativeHandle = System.IntPtr;
#endif

namespace CallKit {

	/// <summary>Enumerates call directory states.</summary>
	[NoMac]
	[MacCatalyst (13, 1)]
	[Native]
	public enum CXCallDirectoryEnabledStatus : long {
		/// <summary>The enabled status of the call directory is unknown.</summary>
		Unknown = 0,
		/// <summary>The call directory is disabled.</summary>
		Disabled = 1,
		/// <summary>The call directory is enabled.</summary>
		Enabled = 2,
	}

	/// <summary>Enumerates Call Kit errors.</summary>
	[NoMac, MacCatalyst (14, 0)]
	[ErrorDomain ("CXErrorDomain")]
	[Native]
	public enum CXErrorCode : long {
		/// <summary>An unknown error occurred.</summary>
		Unknown = 0,
		Unentitled = 1,
		InvalidArgument = 2,
		MissingVoIPBackgroundMode = 3,
	}

	/// <summary>Enumerates incoming call errors.</summary>
	[NoMac]
	[MacCatalyst (13, 1)]
	[ErrorDomain ("CXErrorDomainIncomingCall")]
	[Native]
	public enum CXErrorCodeIncomingCallError : long {
		/// <summary>An unknown error occurred.</summary>
		Unknown = 0,
		/// <summary>To be added.</summary>
		Unentitled = 1,
		/// <summary>The UUID for the call already exists.</summary>
		CallUuidAlreadyExists = 2,
		/// <summary>The device is in Do Not Disturb mode.</summary>
		FilteredByDoNotDisturb = 3,
		/// <summary>The caller is blocked.</summary>
		FilteredByBlockList = 4,
		FilteredDuringRestrictedSharingMode = 5,
		CallIsProtected = 6,
	}

	/// <summary>Enumerates transaction request errors.</summary>
	[NoMac]
	[MacCatalyst (13, 1)]
	[ErrorDomain ("CXErrorDomainRequestTransaction")]
	[Native]
	public enum CXErrorCodeRequestTransactionError : long {
		/// <summary>An unknown error occured.</summary>
		Unknown = 0,
		/// <summary>To be added.</summary>
		Unentitled = 1,
		/// <summary>The call provider was unknown.</summary>
		UnknownCallProvider = 2,
		/// <summary>The transaction was empty.</summary>
		EmptyTransaction = 3,
		/// <summary>The call UUID was unknown.</summary>
		UnknownCallUuid = 4,
		/// <summary>To be added.</summary>
		CallUuidAlreadyExists = 5,
		/// <summary>The action was invalid.</summary>
		InvalidAction = 6,
		/// <summary>The maximum number of call groups was reached.</summary>
		MaximumCallGroupsReached = 7,
		CallIsProtected = 8,
	}

	/// <summary>Enumerates directory manager errors.</summary>
	[NoMac]
	[MacCatalyst (13, 1)]
	[ErrorDomain ("CXErrorDomainCallDirectoryManager")]
	[Native]
	public enum CXErrorCodeCallDirectoryManagerError : long {
		/// <summary>An unknown error occured.</summary>
		Unknown = 0,
		/// <summary>The extension could not be found.</summary>
		NoExtensionFound = 1,
		/// <summary>The loading of the directory extension was interrupted.</summary>
		LoadingInterrupted = 2,
		/// <summary>To be added.</summary>
		EntriesOutOfOrder = 3,
		/// <summary>To be added.</summary>
		DuplicateEntries = 4,
		/// <summary>The maximum number of directory entries was exceeded.</summary>
		MaximumEntriesExceeded = 5,
		/// <summary>The directory extension was disabled.</summary>
		ExtensionDisabled = 6,
		/// <summary>To be added.</summary>
		CurrentlyLoading = 7,
		/// <summary>To be added.</summary>
		UnexpectedIncrementalRemoval = 8,
	}

	[iOS (14, 5), NoTV, NoMac]
	[Introduced (PlatformName.MacCatalyst, 14, 5)]
	[ErrorDomain ("CXErrorDomainNotificationServiceExtension")]
	[Native]
	public enum CXErrorCodeNotificationServiceExtensionError : long {
		Unknown = 0,
		InvalidClientProcess = 1,
		MissingNotificationFilteringEntitlement = 2,
	}

#if NET
	/// <summary>Enumerates DTMF play action types.</summary>
	[NoMac]
#else
	[Obsoleted (PlatformName.MacOSX, 12, 1)]
#endif
	[MacCatalyst (13, 1)]
	[Native]
	public enum CXPlayDtmfCallActionType : long {
		/// <summary>Indicates that a single tone, rather than dual tone, signal will be sent.</summary>
		SingleTone = 1,
		/// <summary>Indicates that the system will prompt the user to approve the sending of the digit tones.</summary>
		SoftPause = 2,
		/// <summary>Indicates that there will be a delay of 2 to 3 seconds before the digit tones are sent.</summary>
		HardPause = 3,
	}

#if NET
	/// <summary>Enumerates reasons that calls can end.</summary>
	[NoMac]
#else
	[Obsoleted (PlatformName.MacOSX, 12, 1)]
#endif
	[MacCatalyst (13, 1)]
	[Native]
	public enum CXCallEndedReason : long {
		/// <summary>The call failed.</summary>
		Failed = 1,
		/// <summary>The remote participant ended the call.</summary>
		RemoteEnded = 2,
		/// <summary>The call went unanswered.</summary>
		Unanswered = 3,
		/// <summary>To be added.</summary>
		AnsweredElsewhere = 4,
		/// <summary>To be added.</summary>
		DeclinedElsewhere = 5,
	}

#if NET
	/// <summary>Enumerates handle types.</summary>
	///     <remarks>Handles act as identifiers for VOIP users.</remarks>
	[NoMac]
#else
	[Obsoleted (PlatformName.MacOSX, 12, 1)]
#endif
	[MacCatalyst (13, 1)]
	[Native]
	public enum CXHandleType : long {
		/// <summary>The handle is an arbitrary string.</summary>
		Generic = 1,
		/// <summary>The handle is a phone number.</summary>
		PhoneNumber = 2,
		/// <summary>The handle is an email address.</summary>
		EmailAddress = 3,
	}

	/// <summary>A unique identifier for a VOIP user.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/reference/CallKit/CXHandle">Apple documentation for <c>CXHandle</c></related>
	[NoMac]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface CXHandle : NSCopying, NSSecureCoding {

		[Export ("type")]
		CXHandleType Type { get; }

		[Export ("value")]
		string Value { get; }

		[Export ("initWithType:value:")]
		[DesignatedInitializer]
		NativeHandle Constructor (CXHandleType type, string value);

		[Export ("isEqualToHandle:")]
		bool IsEqual (CXHandle handle);
	}

	/// <summary>Base class for CallKit actions, such as those taken when a call begins or ends, a call is put on hold, and so on.</summary>
	///     <remarks>Developers manage the life cycle of a call by sending and receiving objects that derive from <see cref="CallKit.CXAction" /> to and from <see cref="CallKit.CXProvider" /> and <see cref="CallKit.CXCallController" /> objects.</remarks>
	///     <related type="externalDocumentation" href="https://developer.apple.com/reference/CallKit/CXAction">Apple documentation for <c>CXAction</c></related>
	[NoMac]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor] // designated
	interface CXAction : NSCopying, NSSecureCoding {

		[DesignatedInitializer]
		[Export ("init")]
		NativeHandle Constructor ();

		[Export ("UUID", ArgumentSemantic.Copy)]
		NSUuid Uuid { get; }

		/// <summary>Gets or sets a value that tells whether the action has already completed.</summary>
		///         <value>A value that tells whether the action has already completed.</value>
		///         <remarks>To be added.</remarks>
		[Export ("complete", ArgumentSemantic.Assign)]
		bool Complete { [Bind ("isComplete")] get; }

		[Export ("timeoutDate", ArgumentSemantic.Strong)]
		NSDate TimeoutDate { get; }

		[Export ("fulfill")]
		void Fulfill ();

		[Export ("fail")]
		void Fail ();
	}

	/// <summary>Contains the information that the application needs in order to answer a call at the user's request.</summary>
	///     <remarks>
	///       <see cref="CallKit.CXAnswerCallAction" /> objects are passed to the developer's <see cref="CallKit.CXProviderDelegate.PerformAnswerCallAction(CallKit.CXProvider,CallKit.CXAnswerCallAction)" /> method when the user answers a call.</remarks>
	///     <related type="externalDocumentation" href="https://developer.apple.com/reference/CallKit/CXAnswerCallAction">Apple documentation for <c>CXAnswerCallAction</c></related>
	[NoMac]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (CXCallAction))]
	[DisableDefaultCtor]
	interface CXAnswerCallAction {

		[Export ("initWithCallUUID:")]
		[DesignatedInitializer]
		NativeHandle Constructor (NSUuid callUuid);

		[Export ("fulfillWithDateConnected:")]
		void Fulfill (NSDate dateConnected);
	}

	/// <summary>Represents a CallKit call.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/reference/CallKit/CXCall">Apple documentation for <c>CXCall</c></related>
	[NoMac]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface CXCall {

		[Export ("UUID", ArgumentSemantic.Copy)]
		NSUuid Uuid { get; }

		/// <summary>Gets a <see langword="bool" /> that tells whether the call is an outgoing call.</summary>
		///         <value>A <see langword="bool" /> that tells whether the call is an outgoing call.</value>
		///         <remarks>To be added.</remarks>
		[Export ("outgoing")]
		bool Outgoing { [Bind ("isOutgoing")] get; }

		/// <summary>Gets a <see langword="bool" /> that tells whether the call is on hold.</summary>
		///         <value>A <see langword="bool" /> that tells whether the call is on hold.</value>
		///         <remarks>To be added.</remarks>
		[Export ("onHold")]
		bool OnHold { [Bind ("isOnHold")] get; }

		[Export ("hasConnected")]
		bool HasConnected { get; }

		[Export ("hasEnded")]
		bool HasEnded { get; }

		[Export ("isEqualToCall:")]
		bool IsEqual (CXCall call);
	}

	/// <summary>Base class for objects that contain the information that is needed to perform an action on a call.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/reference/CallKit/CXCallAction">Apple documentation for <c>CXCallAction</c></related>
	[NoMac]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (CXAction))]
	[DisableDefaultCtor]
	interface CXCallAction {

		[Export ("callUUID", ArgumentSemantic.Copy)]
		NSUuid CallUuid { get; }

		[Export ("initWithCallUUID:")]
		[DesignatedInitializer]
		NativeHandle Constructor (NSUuid callUuid);
	}

	/// <summary>Informs the system about in-band user actions, such as reqeusts to start a call, or to put a call on hold.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/reference/CallKit/CXCallController">Apple documentation for <c>CXCallController</c></related>
	[NoMac]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	interface CXCallController {

		[Export ("initWithQueue:")]
		[DesignatedInitializer]
		NativeHandle Constructor (DispatchQueue queue);

		[Export ("callObserver", ArgumentSemantic.Strong)]
		CXCallObserver CallObserver { get; }

		[Async (XmlDocs = """
			<param name="transaction">The transaction to run.</param>
			<summary>Requests that the system run a transaction.</summary>
			<returns>A task that represents the asynchronous RequestTransaction operation</returns>
			<remarks>To be added.</remarks>
			""")]
		[Export ("requestTransaction:completion:")]
		void RequestTransaction (CXTransaction transaction, Action<NSError> completion);

		[MacCatalyst (13, 1)]
		[Async (XmlDocs = """
			<param name="actions">To be added.</param>
			<summary>Requests that the specified actions be performed by the provider, and runs a handler on the call controller's queue when the operation is complete.</summary>
			<returns>A task that represents the asynchronous RequestTransaction operation</returns>
			<remarks>To be added.</remarks>
			""")]
		[Export ("requestTransactionWithActions:completion:")]
		void RequestTransaction (CXAction [] actions, Action<NSError> completion);

		[MacCatalyst (13, 1)]
		[Async (XmlDocs = """
			<param name="action">The transaction that is being requested.</param>
			<summary>Requests that the specified action be performed by the provider, and runs a handler on the call controller's queue when the operation is complete.</summary>
			<returns>A task that represents the asynchronous RequestTransaction operation</returns>
			<remarks>
			          <para copied="true">The RequestTransactionAsync method is suitable to be used with C# async by returning control to the caller with a Task representing the operation.</para>
			          <para copied="true">To be added.</para>
			        </remarks>
			""")]
		[Export ("requestTransactionWithAction:completion:")]
		void RequestTransaction (CXAction action, Action<NSError> completion);
	}

	/// <summary>Extension context for a call directory.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/reference/CallKit/CXCallDirectoryExtensionContext">Apple documentation for <c>CXCallDirectoryExtensionContext</c></related>
	[NoMac]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSExtensionContext))]
	interface CXCallDirectoryExtensionContext {

		[Export ("addBlockingEntryWithNextSequentialPhoneNumber:")]
		void AddBlockingEntry (/* CXCallDirectoryPhoneNumber -> int64_t */ long phoneNumber);

		[Export ("addIdentificationEntryWithNextSequentialPhoneNumber:label:")]
		void AddIdentificationEntry (/* CXCallDirectoryPhoneNumber -> int64_t */ long phoneNumber, string label);

		[Async (XmlDocs = """
			<summary>Completes the call directory extension request.</summary>
			<returns>A task that accepts the result of the request completion.</returns>
			<remarks>
			          <para copied="true">The CompleteRequestAsync method is suitable to be used with C# async by returning control to the caller with a Task representing the operation.</para>
			          <para copied="true">To be added.</para>
			        </remarks>
			""")]
		[Export ("completeRequestWithCompletionHandler:")]
		void CompleteRequest ([NullAllowed] Action<bool> completion);

		[NullAllowed, Export ("delegate", ArgumentSemantic.Weak)]
		[MacCatalyst (13, 1)]
		ICXCallDirectoryExtensionContextDelegate Delegate { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("incremental")]
		bool Incremental { [Bind ("isIncremental")] get; }

		[MacCatalyst (13, 1)]
		[Export ("removeBlockingEntryWithPhoneNumber:")]
		void RemoveBlockingEntry (/* CXCallDirectoryPhoneNumber -> int64_t */ long phoneNumber);

		[MacCatalyst (13, 1)]
		[Export ("removeAllBlockingEntries")]
		void RemoveAllBlockingEntries ();

		[MacCatalyst (13, 1)]
		[Export ("removeIdentificationEntryWithPhoneNumber:")]
		void RemoveIdentificationEntry (/* CXCallDirectoryPhoneNumber -> int64_t */ long phoneNumber);

		[MacCatalyst (13, 1)]
		[Export ("removeAllIdentificationEntries")]
		void RemoveAllIdentificationEntries ();
	}

	interface ICXCallDirectoryExtensionContextDelegate { }

	/// <related type="externalDocumentation" href="https://developer.apple.com/reference/CallKit/CXCallDirectoryExtensionContextDelegate">Apple documentation for <c>CXCallDirectoryExtensionContextDelegate</c></related>
	[NoMac]
	[MacCatalyst (13, 1)]
	[Protocol]
	[Model]
	[BaseType (typeof (NSObject))]
	interface CXCallDirectoryExtensionContextDelegate {

		/// <param name="extensionContext">To be added.</param>
		/// <param name="error">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Abstract]
		[Export ("requestFailedForExtensionContext:withError:")]
		void RequestFailed (CXCallDirectoryExtensionContext extensionContext, NSError error);
	}

	/// <summary>Manages a call directory extension.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/reference/CallKit/CXCallDirectoryManager">Apple documentation for <c>CXCallDirectoryManager</c></related>
	[NoMac]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	interface CXCallDirectoryManager {

		[Static]
		[Export ("sharedInstance")]
		CXCallDirectoryManager SharedInstance { get; }

		[Async (XmlDocs = """
			<param name="identifier">The unique id of the directory extension to check.</param>
			<summary>Asynchronously reloads the identified directory extension.</summary>
			<returns>A task that represents the asynchronous ReloadExtension operation</returns>
			<remarks>To be added.</remarks>
			""")]
		[Export ("reloadExtensionWithIdentifier:completionHandler:")]
		void ReloadExtension (string identifier, [NullAllowed] Action<NSError> completion);

		[Async (XmlDocs = """
			<param name="identifier">The unique id of the directory extension to check.</param>
			<summary>Asynchronously gets the enabled status of the identified extension.</summary>
			<returns>A task that processes the enabled status of the extension.</returns>
			<remarks>
			          <para copied="true">The GetEnabledStatusForExtensionAsync method is suitable to be used with C# async by returning control to the caller with a Task representing the operation.</para>
			          <para copied="true">To be added.</para>
			        </remarks>
			""")]
		[Export ("getEnabledStatusForExtensionWithIdentifier:completionHandler:")]
		void GetEnabledStatusForExtension (string identifier, Action<CXCallDirectoryEnabledStatus, NSError> completion);

		[NoTV, NoMac, iOS (13, 4), MacCatalyst (14, 0)]
		[Async]
		[Export ("openSettingsWithCompletionHandler:")]
		void OpenSettings ([NullAllowed] Action<NSError> completion);
	}

	/// <summary>Call directory extension provider.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/reference/CallKit/CXCallDirectoryProvider">Apple documentation for <c>CXCallDirectoryProvider</c></related>
	[NoMac]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	interface CXCallDirectoryProvider : NSExtensionRequestHandling {

	}

	/// <summary>Interface that represents the required methods (if any) of the <see cref="CallKit.CXCallObserverDelegate" /> protocol.</summary>
	interface ICXCallObserverDelegate { }

	/// <summary>Delegate object that responds to call changes.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/reference/CallKit/CXCallObserverDelegate">Apple documentation for <c>CXCallObserverDelegate</c></related>
	[NoMac]
	[MacCatalyst (13, 1)]
	[Protocol, Model]
	[BaseType (typeof (NSObject))]
	interface CXCallObserverDelegate {

		/// <param name="callObserver">The object on which this method operates.</param>
		/// <param name="call">The new call.</param>
		/// <summary>Method that is called when the call changes.</summary>
		/// <remarks>To be added.</remarks>
		[Abstract]
		[Export ("callObserver:callChanged:")]
		void CallChanged (CXCallObserver callObserver, CXCall call);
	}

	/// <summary>Observer for the calls in a <see cref="CallKit.CXCallController" />.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/reference/CallKit/CXCallObserver">Apple documentation for <c>CXCallObserver</c></related>
	[NoMac]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	interface CXCallObserver {

		[Export ("calls", ArgumentSemantic.Copy)]
		CXCall [] Calls { get; }

		[Export ("setDelegate:queue:")]
		void SetDelegate ([NullAllowed] ICXCallObserverDelegate aDelegate, [NullAllowed] DispatchQueue queue);
	}

	/// <summary>Contains values with which to update a call's parameters.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/reference/CallKit/CXCallUpdate">Apple documentation for <c>CXCallUpdate</c></related>
	[NoMac]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	interface CXCallUpdate : NSCopying {

		[NullAllowed, Export ("remoteHandle", ArgumentSemantic.Copy)]
		CXHandle RemoteHandle { get; set; }

		[NullAllowed, Export ("localizedCallerName")]
		string LocalizedCallerName { get; set; }

		[Export ("supportsHolding", ArgumentSemantic.Assign)]
		bool SupportsHolding { get; set; }

		[Export ("supportsGrouping", ArgumentSemantic.Assign)]
		bool SupportsGrouping { get; set; }

		[Export ("supportsUngrouping", ArgumentSemantic.Assign)]
		bool SupportsUngrouping { get; set; }

		[Export ("supportsDTMF", ArgumentSemantic.Assign)]
		bool SupportsDtmf { get; set; }

		[Export ("hasVideo")]
		bool HasVideo { get; set; }
	}

	/// <summary>Contains the information that the application needs in order to end a call.</summary>
	///     <remarks>
	///       <see cref="CallKit.CXAnswerCallAction" /> objects are passed to the developer's <see cref="CallKit.CXProviderDelegate.PerformEndCallAction(CallKit.CXProvider,CallKit.CXEndCallAction)" /> method when a call is ended.</remarks>
	///     <related type="externalDocumentation" href="https://developer.apple.com/reference/CallKit/CXEndCallAction">Apple documentation for <c>CXEndCallAction</c></related>
	[NoMac]
	[MacCatalyst (13, 1)]
	[DisableDefaultCtor]
	[BaseType (typeof (CXCallAction))]
	interface CXEndCallAction {

		[Export ("initWithCallUUID:")]
		[DesignatedInitializer]
		NativeHandle Constructor (NSUuid callUuid);

		[Export ("fulfillWithDateEnded:")]
		void Fulfill (NSDate dateEnded);
	}

	/// <summary>Contains the information that is needed to play a DTMF signal that represents a touch tone.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/reference/CallKit/CXPlayDTMFCallAction">Apple documentation for <c>CXPlayDTMFCallAction</c></related>
	[NoMac]
	[MacCatalyst (13, 1)]
	[DisableDefaultCtor]
	[BaseType (typeof (CXCallAction), Name = "CXPlayDTMFCallAction")]
	interface CXPlayDtmfCallAction {

		[Export ("initWithCallUUID:digits:type:")]
		[DesignatedInitializer]
		NativeHandle Constructor (NSUuid callUuid, string digits, CXPlayDtmfCallActionType type);

		[Export ("digits")]
		string Digits { get; set; }

		[Export ("type", ArgumentSemantic.Assign)]
		CXPlayDtmfCallActionType Type { get; set; }
	}

	/// <summary>Interface representing the required methods (if any) of the protocol <see cref="CallKit.CXProviderDelegate" />.</summary>
	///     <remarks>
	///       <para>This interface contains the required methods (if any) from the protocol defined by <see cref="CallKit.CXProviderDelegate" />.</para>
	///       <para>If developers create classes that implement this interface, the implementation methods will automatically be exported to Objective-C with the matching signature from the method defined in the <see cref="CallKit.CXProviderDelegate" /> protocol.</para>
	///       <para>Optional methods (if any) are provided by the <see cref="CallKit.CXProviderDelegate_Extensions" /> class as extension methods to the interface, allowing developers to invoke any optional methods on the protocol.</para>
	///     </remarks>
	interface ICXProviderDelegate { }

	/// <related type="externalDocumentation" href="https://developer.apple.com/reference/CallKit/CXProviderDelegate">Apple documentation for <c>CXProviderDelegate</c></related>
	[Protocol, Model]
	[NoMac]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	interface CXProviderDelegate {

		/// <param name="provider">The provider to which the provider delegate belongs.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Abstract]
		[Export ("providerDidReset:")]
		void DidReset (CXProvider provider);

		/// <param name="provider">The provider to which the provider delegate belongs.</param>
		/// <summary>The system began a call for the app.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("providerDidBegin:")]
		void DidBegin (CXProvider provider);

		/// <param name="provider">The provider to which the provider delegate belongs.</param>
		/// <param name="transaction">The transaction to run.</param>
		/// <summary>Atomically runs the actions that are contained in the <paramref name="transaction" />.</summary>
		/// <returns>
		///           <see langword="true" /> if the transaction succeeded. Otherwise, <see langword="false" />.</returns>
		/// <remarks>To be added.</remarks>
		[Export ("provider:executeTransaction:")]
		bool ExecuteTransaction (CXProvider provider, CXTransaction transaction);

		/// <param name="provider">The provider to which the provider delegate belongs.</param>
		/// <param name="action">The start call action to perform.</param>
		/// <summary>Performs a start call action.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("provider:performStartCallAction:")]
		void PerformStartCallAction (CXProvider provider, CXStartCallAction action);

		/// <param name="provider">The provider to which the provider delegate belongs.</param>
		/// <param name="action">The answer call action to perform.</param>
		/// <summary>Performs an answer call action.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("provider:performAnswerCallAction:")]
		void PerformAnswerCallAction (CXProvider provider, CXAnswerCallAction action);

		/// <param name="provider">The provider to which the provider delegate belongs.</param>
		/// <param name="action">The end call action to perform.</param>
		/// <summary>Performs an end call action.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("provider:performEndCallAction:")]
		void PerformEndCallAction (CXProvider provider, CXEndCallAction action);

		/// <param name="provider">The provider to which the provider delegate belongs.</param>
		/// <param name="action">The hold call action to perform.</param>
		/// <summary>Performs a hold call action.</summary>
		/// <remarks>This method can also be used to resume, or unhold, a call.</remarks>
		[Export ("provider:performSetHeldCallAction:")]
		void PerformSetHeldCallAction (CXProvider provider, CXSetHeldCallAction action);

		/// <param name="provider">The provider to which the provider delegate belongs.</param>
		/// <param name="action">The set muted call action to perform.</param>
		/// <summary>Performs a set muted call action.</summary>
		/// <remarks>This method can also be used to unmute a call.</remarks>
		[Export ("provider:performSetMutedCallAction:")]
		void PerformSetMutedCallAction (CXProvider provider, CXSetMutedCallAction action);

		/// <param name="provider">The provider to which the provider delegate belongs.</param>
		/// <param name="action">The set group call action to perform.</param>
		/// <summary>Performs a set group call action.</summary>
		/// <remarks>This method can also be used to unset a group call.</remarks>
		[Export ("provider:performSetGroupCallAction:")]
		void PerformSetGroupCallAction (CXProvider provider, CXSetGroupCallAction action);

		/// <param name="provider">The provider to which the provider delegate belongs.</param>
		/// <param name="action">The DTMF play call action to perform.</param>
		/// <summary>Performs a DTMF play call action.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("provider:performPlayDTMFCallAction:")]
		void PerformPlayDtmfCallAction (CXProvider provider, CXPlayDtmfCallAction action);

		/// <param name="provider">The provider to which the provider delegate belongs.</param>
		/// <param name="action">The action that timed out.</param>
		/// <summary>Method that is called when a timeout is hit before an action is finished performing.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("provider:timedOutPerformingAction:")]
		void TimedOutPerformingAction (CXProvider provider, CXAction action);

		// Xcode 12 beta 1 issue, AVAudioSession does not appear on Mac OS X but this methods do: https://github.com/xamarin/maccore/issues/2257 
		/// <param name="provider">The provider to which the provider delegate belongs.</param>
		/// <param name="audioSession">To be added.</param>
		/// <summary>The system activated a telephony-priority audio session for the call.</summary>
		/// <remarks>To be added.</remarks>
		[NoMac]
		[MacCatalyst (13, 1)]
		[Export ("provider:didActivateAudioSession:")]
		void DidActivateAudioSession (CXProvider provider, AVAudioSession audioSession);

		/// <param name="provider">The provider to which the provider delegate belongs.</param>
		/// <param name="audioSession">The audio session that was deactivated.</param>
		/// <summary>The system deactivated an audio session that the app had been using for a call.</summary>
		/// <remarks>To be added.</remarks>
		[NoMac]
		[MacCatalyst (13, 1)]
		[Export ("provider:didDeactivateAudioSession:")]
		void DidDeactivateAudioSession (CXProvider provider, AVAudioSession audioSession);
	}

	/// <summary>Reports external (out-of-band) events, such as incoming calls, to the system, and receives internal (in-band) user action events from the system.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/reference/CallKit/CXProvider">Apple documentation for <c>CXProvider</c></related>
	[NoMac]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface CXProvider {

		[Export ("initWithConfiguration:")]
		[DesignatedInitializer]
		NativeHandle Constructor (CXProviderConfiguration configuration);

		[Export ("setDelegate:queue:")]
		void SetDelegate ([NullAllowed] ICXProviderDelegate aDelegate, [NullAllowed] DispatchQueue queue);

		[Async (XmlDocs = """
			<param name="uuid">The identifier of the call.</param>
			<param name="update">An object that contains the updated parameters for the call.</param>
			<summary>Reports a new incoming call to the system.</summary>
			<returns>A task that represents the asynchronous ReportNewIncomingCall operation</returns>
			<remarks>
			          <para copied="true">The ReportNewIncomingCallAsync method is suitable to be used with C# async by returning control to the caller with a Task representing the operation.</para>
			          <para copied="true">To be added.</para>
			        </remarks>
			""")]
		[Export ("reportNewIncomingCallWithUUID:update:completion:")]
		void ReportNewIncomingCall (NSUuid uuid, CXCallUpdate update, Action<NSError> completion);

		[Export ("reportCallWithUUID:updated:")]
		void ReportCall (NSUuid uuid, CXCallUpdate update);

		[Export ("reportCallWithUUID:endedAtDate:reason:")]
		void ReportCall (NSUuid uuid, [NullAllowed] NSDate dateEnded, CXCallEndedReason endedReason);

		[Export ("reportOutgoingCallWithUUID:startedConnectingAtDate:")]
		void ReportConnectingOutgoingCall (NSUuid uuid, [NullAllowed] NSDate dateStartedConnecting);

		[Export ("reportOutgoingCallWithUUID:connectedAtDate:")]
		void ReportConnectedOutgoingCall (NSUuid uuid, [NullAllowed] NSDate dateConnected);

		[NoTV, NoMac, iOS (14, 5)]
		[Introduced (PlatformName.MacCatalyst, 14, 5)]
		[Static, Async]
		[Export ("reportNewIncomingVoIPPushPayload:completion:")]
		void ReportNewIncomingVoIPPushPayload (NSDictionary dictionaryPayload, [NullAllowed] Action<NSError> completion);

		[Export ("configuration", ArgumentSemantic.Copy)]
		CXProviderConfiguration Configuration { get; set; }

		[Export ("invalidate")]
		void Invalidate ();

		[Export ("pendingTransactions", ArgumentSemantic.Copy)]
		CXTransaction [] PendingTransactions { get; }

		[Export ("pendingCallActionsOfClass:withCallUUID:")]
		CXCallAction [] GetPendingCallActions (Class callActionClass, NSUuid callUuid);
	}

	/// <summary>Contains values that control miscellaneous call properties, such as the ringtone, whether the call supports video, the maximum number of callers, and so on.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/reference/CallKit/CXProviderConfiguration">Apple documentation for <c>CXProviderConfiguration</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface CXProviderConfiguration : NSCopying {

		[NoMac] // deprecated and was never added to Mac OS X before
		[Deprecated (PlatformName.iOS, 14, 0)]
		[MacCatalyst (13, 1)]
		[Deprecated (PlatformName.MacCatalyst, 14, 0)]
		[Export ("localizedName"), NullAllowed]
		string LocalizedName { get; }

		[NullAllowed, Export ("ringtoneSound", ArgumentSemantic.Strong)]
		string RingtoneSound { get; set; }

		[Advice ("40x40 points squared image.")]
		[NullAllowed, Export ("iconTemplateImageData", ArgumentSemantic.Copy)]
		NSData IconTemplateImageData { get; set; }

		[Export ("maximumCallGroups")]
		nuint MaximumCallGroups { get; set; }

		[Export ("maximumCallsPerCallGroup")]
		nuint MaximumCallsPerCallGroup { get; set; }

		[MacCatalyst (13, 1)]
		[Export ("includesCallsInRecents")]
		bool IncludesCallsInRecents { get; set; }

		[Export ("supportsVideo")]
		bool SupportsVideo { get; set; }

		[Export ("supportedHandleTypes", ArgumentSemantic.Copy)]
		NSSet<NSNumber> SupportedHandleTypes { get; set; }

		[NoMac] // deprecated and was never added to Mac OS X before 
		[Deprecated (PlatformName.iOS, 14, 0, message: "Use the default constructor instead.")]
		[Deprecated (PlatformName.MacCatalyst, 14, 0, message: "Use the default constructor instead.")]
		[MacCatalyst (13, 1)]
		[Export ("initWithLocalizedName:")]
		NativeHandle Constructor (string localizedName);

		[iOS (14, 0)]
		[MacCatalyst (14, 0)]
		[DesignatedInitializer]
		[Export ("init")]
		NativeHandle Constructor ();
	}

	/// <summary>Contains the data that are needed to join a group call.</summary>
	///     <remarks>
	///       <see cref="CallKit.CXSetGroupCallAction" /> objects are passed to the developer's <see cref="CallKit.CXProviderDelegate.PerformSetGroupCallAction(CallKit.CXProvider,CallKit.CXSetGroupCallAction)" /> method when the user joins a call.</remarks>
	///     <related type="externalDocumentation" href="https://developer.apple.com/reference/CallKit/CXSetGroupCallAction">Apple documentation for <c>CXSetGroupCallAction</c></related>
	[NoMac]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (CXCallAction))]
	[DisableDefaultCtor]
	interface CXSetGroupCallAction {

		[Export ("initWithCallUUID:callUUIDToGroupWith:")]
		[DesignatedInitializer]
		NativeHandle Constructor (NSUuid callUuid, [NullAllowed] NSUuid callUuidToGroupWith);

		[NullAllowed, Export ("callUUIDToGroupWith", ArgumentSemantic.Copy)]
		NSUuid CallUuidToGroupWith { get; set; }
	}

	/// <summary>Contains the information that is needed to put a call on hold or take a call off hold.</summary>
	///     <remarks>
	///       <see cref="CallKit.CXSetHeldCallAction" /> objects are passed to the developer's <see cref="CallKit.CXProviderDelegate.PerformSetHeldCallAction(CallKit.CXProvider,CallKit.CXSetHeldCallAction)" /> method when the user puts a call on hold or takes a call off hold.</remarks>
	///     <related type="externalDocumentation" href="https://developer.apple.com/reference/CallKit/CXSetHeldCallAction">Apple documentation for <c>CXSetHeldCallAction</c></related>
	[NoMac]
	[MacCatalyst (13, 1)]
	[DisableDefaultCtor]
	[BaseType (typeof (CXCallAction))]
	interface CXSetHeldCallAction {

		[Export ("initWithCallUUID:onHold:")]
		[DesignatedInitializer]
		NativeHandle Constructor (NSUuid callUuid, bool onHold);

		/// <summary>Gets or sets a value that tells whether the call is on hold.</summary>
		///         <value>A value that tells whether the call is on hold.</value>
		///         <remarks>To be added.</remarks>
		[Export ("onHold")]
		bool OnHold { [Bind ("isOnHold")] get; set; }
	}

	/// <summary>Contains the information that is necessary to mute or unmute a call.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/reference/CallKit/CXSetMutedCallAction">Apple documentation for <c>CXSetMutedCallAction</c></related>
	[NoMac]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (CXCallAction))]
	[DisableDefaultCtor]
	interface CXSetMutedCallAction {

		// needs to be reexposed
		[Export ("initWithCallUUID:")]
		[DesignatedInitializer]
		NativeHandle Constructor (NSUuid callUuid);

		[Export ("initWithCallUUID:muted:")]
		NativeHandle Constructor (NSUuid callUuid, bool muted);

		/// <summary>Gets or sets a value that controls whether the call is muted.</summary>
		///         <value>A value that controls whether the call is muted.</value>
		///         <remarks>To be added.</remarks>
		[Export ("muted")]
		bool Muted { [Bind ("isMuted")] get; set; }
	}

	/// <summary>Contains the information that is necessary to start a call.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/reference/CallKit/CXStartCallAction">Apple documentation for <c>CXStartCallAction</c></related>
	[NoMac]
	[MacCatalyst (13, 1)]
	[DisableDefaultCtor]
	[BaseType (typeof (CXCallAction))]
	interface CXStartCallAction {

		// initWithCallUUID: explicitly marked with NS_UNAVAILABLE

		[Export ("initWithCallUUID:handle:")]
		[DesignatedInitializer]
		NativeHandle Constructor (NSUuid callUuid, CXHandle callHandle);

		[Export ("handle", ArgumentSemantic.Copy)]
		CXHandle CallHandle { get; set; }

		[NullAllowed, Export ("contactIdentifier")]
		string ContactIdentifier { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("video")]
		bool Video { [Bind ("isVideo")] get; set; }

		[Export ("fulfillWithDateStarted:")]
		void Fulfill (NSDate dateStarted);
	}

	/// <summary>Runs a group of Call Kit actions atomically.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/reference/CallKit/CXTransaction">Apple documentation for <c>CXTransaction</c></related>
	[NoMac]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor] // there's a designated initializer that does not accept null
	interface CXTransaction : NSCopying, NSSecureCoding {

		[Export ("initWithActions:")]
		[DesignatedInitializer]
		NativeHandle Constructor (CXAction [] actions);

		[Export ("initWithAction:")]
		NativeHandle Constructor (CXAction action);

		[Export ("UUID", ArgumentSemantic.Copy)]
		NSUuid Uuid { get; }

		/// <summary>Gets a value that tells whether the transaction is complete.</summary>
		///         <value>
		///           <see langword="true" /> if the transaction is complete. Otherwise, <see langword="false" />.</value>
		///         <remarks>To be added.</remarks>
		[Export ("complete", ArgumentSemantic.Assign)]
		bool Complete { [Bind ("isComplete")] get; }

		[Export ("actions", ArgumentSemantic.Copy)]
		CXAction [] Actions { get; }

		[Export ("addAction:")]
		void AddAction (CXAction action);
	}
}
