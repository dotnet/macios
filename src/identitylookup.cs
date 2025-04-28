//
// IdentityLookup C# bindings
//
// Authors:
//	Alex Soto  <alexsoto@microsoft.com>
//
// Copyright 2017 Xamarin Inc. All rights reserved.
// Copyright 2019 Microsoft Corporation.
//

using System;
using Foundation;
using ObjCRuntime;

#if !NET
using NativeHandle = System.IntPtr;
#endif

namespace IdentityLookup {

	/// <summary>Enumerates actions that can be taken in response to a message.</summary>
	[NoMac]
	[NoTV]
	[MacCatalyst (13, 1)]
	[Native]
	public enum ILMessageFilterAction : long {
		/// <summary>Indicates that there is not enough information to choose an action.</summary>
		None = 0,
		/// <summary>Indicates that the message will be allowed.</summary>
		Allow = 1,
		/// <summary>Indicates that the message will be filtered.</summary>
		Junk = 2,
#if !NET
		[Obsolete ("Use 'Junk' instead.")]
		Filter = Junk,
#endif
		[iOS (14, 0)]
		[Introduced (PlatformName.MacCatalyst, 14, 0)]
		Promotion = 3,
		[iOS (14, 0)]
		[Introduced (PlatformName.MacCatalyst, 14, 0)]
		Transaction = 4,
	}

	[NoMac]
	[NoTV]
	[MacCatalyst (13, 1)]
	[ErrorDomain ("ILMessageFilterErrorDomain")]
	[Native]
	public enum ILMessageFilterError : long {
		/// <summary>To be added.</summary>
		System = 1,
		/// <summary>To be added.</summary>
		InvalidNetworkUrl = 2,
		/// <summary>To be added.</summary>
		NetworkUrlUnauthorized = 3,
		/// <summary>To be added.</summary>
		NetworkRequestFailed = 4,
		/// <summary>To be added.</summary>
		RedundantNetworkDeferral = 5,
	}

	/// <summary>Enumerates message classification actions.</summary>
	[NoMac]
	[NoTV]
	[MacCatalyst (13, 1)]
	[Native]
	enum ILClassificationAction : long {
		/// <summary>Indicates that no action should be taken.</summary>
		None = 0,
		/// <summary>Indicates that the user reported that the message is not junk.</summary>
		ReportNotJunk = 1,
		/// <summary>Indicates that the user reported that the message is junk.</summary>
		ReportJunk = 2,
		/// <summary>Indicates that the user reported that the message is junk, and that they want to block the sender.</summary>
		ReportJunkAndBlockSender = 3,
	}

	[NoTV, NoMac, MacCatalyst (16, 0), iOS (16, 0)]
	[Native]
	public enum ILMessageFilterSubAction : long {
		None = 0,
		TransactionalOthers = 10000,
		TransactionalFinance = 10001,
		TransactionalOrders = 10002,
		TransactionalReminders = 10003,
		TransactionalHealth = 10004,
		TransactionalWeather = 10005,
		TransactionalCarrier = 10006,
		TransactionalRewards = 10007,
		TransactionalPublicServices = 10008,
		PromotionalOthers = 20000,
		PromotionalOffers = 20001,
		PromotionalCoupons = 20002,
	}

	/// <summary>Base class for message filter extensions.</summary>
	[NoMac]
	[NoTV]
	[MacCatalyst (13, 1)]
	[DisableDefaultCtor]
	[BaseType (typeof (NSObject))]
	interface ILMessageFilterExtension {
	}

	/// <summary>Represents the extension context for a message filter.</summary>
	[NoMac]
	[NoTV]
	[MacCatalyst (13, 1)]
	[DisableDefaultCtor]
	[BaseType (typeof (NSExtensionContext))]
	interface ILMessageFilterExtensionContext {

		[Export ("deferQueryRequestToNetworkWithCompletion:")]
		[Async (XmlDocs = """
			<summary>Defers the query request to the network service for the extension and runs a handler when the operation is complete.</summary>
			<returns>
			          <para class="improve-task-t-return-type-description">A task that represents the asynchronous DeferQueryRequestToNetwork operation.  The value of the TResult parameter is of type System.Action&lt;IdentityLookup.ILNetworkResponse,Foundation.NSError&gt;.</para>
			        </returns>
			<remarks>
			          <para copied="true">The DeferQueryRequestToNetworkAsync method is suitable to be used with C# async by returning control to the caller with a Task representing the operation.</para>
			          <para copied="true">To be added.</para>
			        </remarks>
			""")]
		void DeferQueryRequestToNetwork (Action<ILNetworkResponse, NSError> completion);
	}

	interface IILMessageFilterQueryHandling { }

	/// <summary>Interface that is used by a message filter extension to respond to queries.</summary>
	[NoMac]
	[NoTV]
	[MacCatalyst (13, 1)]
	[Protocol]
	interface ILMessageFilterQueryHandling {

		/// <param name="queryRequest">The query for the message.</param>
		/// <param name="context">The app extension context for deferring requests.</param>
		/// <param name="completion">A handler that is run after the operation completes.</param>
		/// <summary>Evaluates the specified request in the provided context, and runs a handler when the operation is complete.</summary>
		/// <remarks>To be added.</remarks>
		[Abstract]
		[Export ("handleQueryRequest:context:completion:")]
		void HandleQueryRequest (ILMessageFilterQueryRequest queryRequest, ILMessageFilterExtensionContext context, Action<ILMessageFilterQueryResponse> completion);
	}

	/// <summary>Represents a request to a message filter to examine a message from an unknown sender for filtering.</summary>
	[NoMac]
	[NoTV]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface ILMessageFilterQueryRequest : NSSecureCoding {

		[NullAllowed, Export ("sender")]
		string Sender { get; }

		[NullAllowed, Export ("messageBody")]
		string MessageBody { get; }

		[NoTV, NoMac, MacCatalyst (16, 0), iOS (16, 0)]
		[NullAllowed, Export ("receiverISOCountryCode")]
		string ReceiverIsoCountryCode { get; }
	}

	/// <summary>Represents a message query response.</summary>
	[NoMac]
	[NoTV]
	[MacCatalyst (13, 1)]
	[DisableDefaultCtor]
	[BaseType (typeof (NSObject))]
	interface ILMessageFilterQueryResponse : NSSecureCoding {

		[Export ("action", ArgumentSemantic.Assign)]
		ILMessageFilterAction Action { get; set; }

		[NoTV, NoMac, MacCatalyst (16, 0), iOS (16, 0)]
		[Export ("subAction", ArgumentSemantic.Assign)]
		ILMessageFilterSubAction SubAction { get; set; }
	}

	/// <summary>Represents a response to a network request by the filter extension.</summary>
	[NoMac]
	[NoTV]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface ILNetworkResponse : NSSecureCoding {

		[Export ("urlResponse")]
		NSHttpUrlResponse UrlResponse { get; }

		[Export ("data")]
		NSData Data { get; }
	}

	/// <summary>A classification request.</summary>
	[NoMac]
	[NoTV]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (ILClassificationRequest))]
	[DisableDefaultCtor]
	interface ILCallClassificationRequest : NSSecureCoding {

		[Export ("callCommunications", ArgumentSemantic.Copy)]
		ILCallCommunication [] CallCommunications { get; }
	}

	/// <summary>An incoming call.</summary>
	[NoMac]
	[NoTV]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (ILCommunication))]
	[DisableDefaultCtor]
	interface ILCallCommunication {

		[Export ("isEqualToCallCommunication:")]
		bool IsEqualTo (ILCallCommunication communication);
	}

	/// <summary>Base class for classes that support users in reporting unwanted communications.</summary>
	[Abstract]
	[NoMac]
	[NoTV]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface ILClassificationRequest : NSSecureCoding {

	}

	/// <summary>Handles the classification of messages to mark them as junk, mark them as not junk, and/or to block the sender.</summary>
	[NoMac]
	[NoTV]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface ILClassificationResponse : NSSecureCoding {

		[Export ("action", ArgumentSemantic.Assign)]
		ILClassificationAction Action { get; }

		[MacCatalyst (13, 1)]
		[NullAllowed, Export ("userString")]
		string UserString { get; set; }

		// Objects can be NSString, NSNumber, NSArray, NSDictionary, or NSNull
		[NullAllowed, Export ("userInfo", ArgumentSemantic.Copy)]
		NSDictionary<NSString, NSObject> UserInfo { get; set; }

		[Export ("initWithClassificationAction:")]
		[DesignatedInitializer]
		NativeHandle Constructor (ILClassificationAction action);
	}

	/// <summary>Base class for incoming calls and messages.</summary>
	[Abstract]
	[NoMac]
	[NoTV]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface ILCommunication : NSSecureCoding {

		[NullAllowed, Export ("sender")]
		string Sender { get; }

		[Export ("dateReceived", ArgumentSemantic.Copy)]
		NSDate DateReceived { get; }

		[Export ("isEqualToCommunication:")]
		bool IsEqualTo (ILCommunication communication);
	}

	/// <summary>A request to report a message.</summary>
	[NoMac]
	[NoTV]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (ILClassificationRequest))]
	[DisableDefaultCtor]
	interface ILMessageClassificationRequest : NSSecureCoding {

		[Export ("messageCommunications", ArgumentSemantic.Copy)]
		ILMessageCommunication [] MessageCommunications { get; }
	}

	/// <summary>An incoming SMS message.</summary>
	[NoMac]
	[NoTV]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (ILCommunication))]
	[DisableDefaultCtor]
	interface ILMessageCommunication {

		[NullAllowed, Export ("messageBody")]
		string MessageBody { get; }

		[Export ("isEqualToMessageCommunication:")]
		bool IsEqualTo (ILMessageCommunication communication);
	}

	[NoTV, NoMac, MacCatalyst (16, 0), iOS (16, 0)]
	[Protocol]
	interface ILMessageFilterCapabilitiesQueryHandling {
		[Abstract]
		[Export ("handleCapabilitiesQueryRequest:context:completion:")]
		void HandleQueryRequest (ILMessageFilterCapabilitiesQueryRequest capabilitiesQueryRequest, ILMessageFilterExtensionContext context, Action<ILMessageFilterCapabilitiesQueryResponse> completion);
	}

	[NoTV, NoMac, MacCatalyst (16, 0), iOS (16, 0)]
	[BaseType (typeof (NSObject))]
	interface ILMessageFilterCapabilitiesQueryResponse : NSSecureCoding {
		[BindAs (typeof (ILMessageFilterSubAction []))]
		[Export ("transactionalSubActions", ArgumentSemantic.Copy)]
		NSNumber [] TransactionalSubActions { get; set; }

		[BindAs (typeof (ILMessageFilterSubAction []))]
		[Export ("promotionalSubActions", ArgumentSemantic.Copy)]
		NSNumber [] PromotionalSubActions { get; set; }
	}

	[NoTV, NoMac, MacCatalyst (16, 0), iOS (16, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface ILMessageFilterCapabilitiesQueryRequest : NSSecureCoding {
	}
}
