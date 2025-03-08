using Foundation;
using ObjCRuntime;
using System;

namespace CoreTelephony {
	/// <summary>Encapsulates a unique identifier for a call and it's state.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/NetworkingInternet/Reference/CTCall/index.html">Apple documentation for <c>CTCall</c></related>
	[MacCatalyst (14, 0)]
	[Deprecated (PlatformName.MacCatalyst, 14, 0, message: Constants.UseCallKitInstead)]
	[Deprecated (PlatformName.iOS, 10, 0, message: Constants.UseCallKitInstead)]
	[BaseType (typeof (NSObject))]
	interface CTCall {
		/// <summary>Developers should not use this deprecated property. Developers should use 'CallKit' instead.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("callID")]
		string CallID { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("callState")]
		string CallState { get; }

	}

	/// <related type="externalDocumentation" href="https://developer.apple.com/reference/CoreTelephony/CTCellularData">Apple documentation for <c>CTCellularData</c></related>
	[NoMacCatalyst]
	[BaseType (typeof (NSObject))]
	interface CTCellularData {
		[NullAllowed, Export ("cellularDataRestrictionDidUpdateNotifier", ArgumentSemantic.Copy)]
		Action<CTCellularDataRestrictedState> RestrictionDidUpdateNotifier { get; set; }

		[Export ("restrictedState")]
		CTCellularDataRestrictedState RestrictedState { get; }
	}

	/// <summary>Defines constants describing various telephone radio technogies.</summary>
	[MacCatalyst (14, 0)]
	[Static]
	interface CTRadioAccessTechnology {
		/// <summary>Represents the value associated with the constant CTRadioAccessTechnologyGPRS</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("CTRadioAccessTechnologyGPRS")]
		NSString GPRS { get; }

		/// <summary>Represents the value associated with the constant CTRadioAccessTechnologyEdge</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("CTRadioAccessTechnologyEdge")]
		NSString Edge { get; }

		/// <summary>Represents the value associated with the constant CTRadioAccessTechnologyWCDMA</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("CTRadioAccessTechnologyWCDMA")]
		NSString WCDMA { get; }

		/// <summary>Represents the value associated with the constant CTRadioAccessTechnologyHSDPA</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("CTRadioAccessTechnologyHSDPA")]
		NSString HSDPA { get; }

		/// <summary>Represents the value associated with the constant CTRadioAccessTechnologyHSUPA</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("CTRadioAccessTechnologyHSUPA")]
		NSString HSUPA { get; }

		/// <summary>Represents the value associated with the constant CTRadioAccessTechnologyCDMA1x</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("CTRadioAccessTechnologyCDMA1x")]
		NSString CDMA1x { get; }

		/// <summary>Represents the value associated with the constant CTRadioAccessTechnologyCDMAEVDORev0</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("CTRadioAccessTechnologyCDMAEVDORev0")]
		NSString CDMAEVDORev0 { get; }

		/// <summary>Represents the value associated with the constant CTRadioAccessTechnologyCDMAEVDORevA</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("CTRadioAccessTechnologyCDMAEVDORevA")]
		NSString CDMAEVDORevA { get; }

		/// <summary>Represents the value associated with the constant CTRadioAccessTechnologyCDMAEVDORevB</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("CTRadioAccessTechnologyCDMAEVDORevB")]
		NSString CDMAEVDORevB { get; }

		/// <summary>Represents the value associated with the constant CTRadioAccessTechnologyeHRPD</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("CTRadioAccessTechnologyeHRPD")]
		NSString EHRPD { get; }

		/// <summary>Represents the value associated with the constant CTRadioAccessTechnologyLTE</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("CTRadioAccessTechnologyLTE")]
		NSString LTE { get; }

		[iOS (14, 1)]
		[MacCatalyst (14, 1)]
		[Field ("CTRadioAccessTechnologyNRNSA")]
		NSString NRNsa { get; }

		[iOS (14, 1)]
		[MacCatalyst (14, 1)]
		[Field ("CTRadioAccessTechnologyNR")]
		NSString NR { get; }
	}

	interface ICTTelephonyNetworkInfoDelegate { }

	[MacCatalyst (14, 0)]
	[iOS (13, 0)]
#if NET
	[Protocol, Model]
#else
	[Protocol, Model (AutoGeneratedName = true)]
#endif
	[BaseType (typeof (NSObject))]
	interface CTTelephonyNetworkInfoDelegate {

		[Export ("dataServiceIdentifierDidChange:")]
		void DataServiceIdentifierDidChange (string identifier);
	}

	/// <summary>A class that holds information on the application user's cellular service provider.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/NetworkingInternet/Reference/CTTelephonyNetworkInfo/index.html">Apple documentation for <c>CTTelephonyNetworkInfo</c></related>
	[MacCatalyst (14, 0)]
	[BaseType (typeof (NSObject))]
	interface CTTelephonyNetworkInfo {
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Deprecated (PlatformName.iOS, 12, 0, message: "Use 'ServiceSubscriberCellularProviders' instead.")]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'ServiceSubscriberCellularProviders' instead.")]
		[Export ("subscriberCellularProvider", ArgumentSemantic.Retain)]
		[NullAllowed]
		CTCarrier SubscriberCellularProvider { get; }

		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Deprecated (PlatformName.iOS, 12, 0, message: "Use 'ServiceSubscriberCellularProvidersDidUpdateNotifier' instead.")]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'ServiceSubscriberCellularProvidersDidUpdateNotifier' instead.")]
		[NullAllowed] // by default this property is null
		[Export ("subscriberCellularProviderDidUpdateNotifier")]
		Action<CTCarrier> CellularProviderUpdatedEventHandler { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Deprecated (PlatformName.iOS, 12, 0, message: "Use 'ServiceCurrentRadioAccessTechnology' instead.")]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'ServiceCurrentRadioAccessTechnology' instead.")]
		[Export ("currentRadioAccessTechnology")]
		[NullAllowed]
		NSString CurrentRadioAccessTechnology { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (14, 0)]
		[Deprecated (PlatformName.iOS, 16, 0)]
		[Deprecated (PlatformName.MacCatalyst, 16, 0)]
		[NullAllowed]
		[Export ("serviceSubscriberCellularProviders", ArgumentSemantic.Retain)]
		NSDictionary<NSString, CTCarrier> ServiceSubscriberCellularProviders { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (14, 0)]
		[NullAllowed]
		[Export ("serviceCurrentRadioAccessTechnology", ArgumentSemantic.Retain)]
		NSDictionary<NSString, NSString> ServiceCurrentRadioAccessTechnology { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (14, 0)]
		[Deprecated (PlatformName.iOS, 16, 0)]
		[Deprecated (PlatformName.MacCatalyst, 16, 0)]
		[NullAllowed]
		[Export ("serviceSubscriberCellularProvidersDidUpdateNotifier", ArgumentSemantic.Copy)]
		Action<NSString> ServiceSubscriberCellularProvidersDidUpdateNotifier { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (14, 0)]
		[Notification]
		[Field ("CTServiceRadioAccessTechnologyDidChangeNotification")]
		NSString ServiceRadioAccessTechnologyDidChangeNotification { get; }

		[iOS (13, 0)]
		[MacCatalyst (14, 0)]
		[NullAllowed, Export ("dataServiceIdentifier")]
		string DataServiceIdentifier { get; }

		[iOS (13, 0)]
		[MacCatalyst (14, 0)]
		[Wrap ("WeakDelegate")]
		[NullAllowed]
		ICTTelephonyNetworkInfoDelegate Delegate { get; set; }

		[iOS (13, 0)]
		[MacCatalyst (14, 0)]
		[NullAllowed, Export ("delegate", ArgumentSemantic.Weak)]
		NSObject WeakDelegate { get; set; }
	}

	/// <summary>Holds a list of current calls and triggers events when their states change.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/NetworkingInternet/Reference/CTCallCenter/index.html">Apple documentation for <c>CTCallCenter</c></related>
	[MacCatalyst (14, 0)]
	[Deprecated (PlatformName.MacCatalyst, 14, 0, message: Constants.UseCallKitInstead)]
	[Deprecated (PlatformName.iOS, 10, 0, message: Constants.UseCallKitInstead)]
	[BaseType (typeof (NSObject))]
	interface CTCallCenter {
		/// <summary>Developers should not use this deprecated property. Developers should use 'CallKit' instead.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed] // by default this property is null
		[Export ("callEventHandler")]
		Action<CTCall> CallEventHandler { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("currentCalls")]
		[NullAllowed]
		NSSet CurrentCalls { get; }

	}

	/// <summary>Contains information about the application user's home cellular service provider.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/NetworkingInternet/Reference/CTCarrier/index.html">Apple documentation for <c>CTCarrier</c></related>
	[Deprecated (PlatformName.MacCatalyst, 16, 0, message: Constants.UseCallKitInstead)]
	[Deprecated (PlatformName.iOS, 16, 0, message: Constants.UseCallKitInstead)]
	[MacCatalyst (14, 0)]
	[BaseType (typeof (NSObject))]
	interface CTCarrier {
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NullAllowed]
		[Export ("mobileCountryCode")]
		string MobileCountryCode { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NullAllowed]
		[Export ("mobileNetworkCode")]
		string MobileNetworkCode { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NullAllowed]
		[Export ("isoCountryCode")]
		string IsoCountryCode { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("allowsVOIP")]
		bool AllowsVoip { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NullAllowed]
		[Export ("carrierName")]
		string CarrierName { get; }
	}

	interface ICTSubscriberDelegate { }

	[NoMacCatalyst]
	[Protocol]
	interface CTSubscriberDelegate {
		[Abstract]
		[Export ("subscriberTokenRefreshed:")]
		void SubscriberTokenRefreshed (CTSubscriber subscriber);
	}

	/// <summary>Carrier information for a subscriber.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/CoreTelephony/Reference/CTSubscriber/index.html">Apple documentation for <c>CTSubscriber</c></related>
	[NoMacCatalyst]
	[BaseType (typeof (NSObject))]
	partial interface CTSubscriber {
		[Export ("carrierToken")]
		[NullAllowed]
		NSData CarrierToken { get; }

		[Export ("identifier")]
		string Identifier { get; }

		[NullAllowed, Export ("delegate", ArgumentSemantic.Weak)]
		NSObject WeakDelegate { get; set; }

		[Wrap ("WeakDelegate")]
		[NullAllowed]
		ICTSubscriberDelegate Delegate { get; set; }

		// available since iOS 6 according to the headers
		[Export ("refreshCarrierToken")]
		bool RefreshCarrierToken ();

		[iOS (18, 0)]
		[Export ("SIMInserted")]
		bool IsSimInserted { [Bind ("isSIMInserted")] get; }
	}

	/// <summary>Information on a subscriber to a telephone service.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/reference/CoreTelephony/CTSubscriberInfo">Apple documentation for <c>CTSubscriberInfo</c></related>
	[NoMacCatalyst]
	[BaseType (typeof (NSObject))]
	partial interface CTSubscriberInfo {
		[Deprecated (PlatformName.iOS, 12, 1, message: "Use 'Subscribers' instead.")]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'Subscribers' instead.")]
		[Static]
		[Export ("subscriber")]
		CTSubscriber Subscriber { get; }

		[Static]
		[Export ("subscribers")]
		CTSubscriber [] Subscribers { get; }
	}

	[NoMacCatalyst]
	[BaseType (typeof (NSObject))]
	interface CTCellularPlanProvisioningRequest : NSSecureCoding {
		[Export ("address")]
		string Address { get; set; }

		[NullAllowed, Export ("matchingID")]
		string MatchingId { get; set; }

		[NullAllowed, Export ("OID")]
		string Oid { get; set; }

		[NullAllowed, Export ("confirmationCode")]
		string ConfirmationCode { get; set; }

		[NullAllowed, Export ("ICCID")]
		string Iccid { get; set; }

		[NullAllowed, Export ("EID")]
		string Eid { get; set; }
	}

	[NoMacCatalyst]
	[BaseType (typeof (NSObject))]
	interface CTCellularPlanProvisioning {
		[Export ("supportsCellularPlan")]
		bool SupportsCellularPlan { get; }

		[Async]
		[Export ("addPlanWith:completionHandler:")]
		void AddPlan (CTCellularPlanProvisioningRequest request, Action<CTCellularPlanProvisioningAddPlanResult> completionHandler);

		[iOS (16, 0)]
		[Export ("supportsEmbeddedSIM")]
		bool SupportsEmbeddedSim { get; }
	}
}
