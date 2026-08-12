
namespace CoreTelephony {

	[iOS (26, 0), Mac (26, 0), MacCatalyst (26, 0)]
	[Native]
	public enum CTCellularPlanCapability : long {
		Only,
		AndVoice,
	}

	/// <summary>Constants that indicate whether the device has a cellular plan for a phone number.</summary>
	[NoTV, NoMac, iOS (27, 0), MacCatalyst (27, 0)]
	[Native]
	public enum CTCellularPlanStatusAvailability : long {
		/// <summary>The phone number's cellular plan is inactive, or the system can't determine its status.</summary>
		Unavailable,
		/// <summary>The phone number has an active cellular plan on the device.</summary>
		Available,
	}

	/// <summary>Constants that indicate the system's confidence that the device has a cellular plan for a phone number.</summary>
	[NoTV, NoMac, iOS (27, 0), MacCatalyst (27, 0)]
	[Native]
	public enum CTCellularPlanStatusAvailabilityConfidence : long {
		/// <summary>A low level of confidence about the availability of a cellular plan.</summary>
		Low,
		/// <summary>A high level of confidence about the availability of a cellular plan.</summary>
		High,
	}

	/// <summary>Constants that indicate the authorization status for accessing cellular plan information for a phone number.</summary>
	[NoTV, NoMac, iOS (27, 0), MacCatalyst (27, 0)]
	[Native]
	public enum CTCellularPlanStatusAuthorization : long {
		/// <summary>The user didn't grant authorization, or explicitly denied it.</summary>
		NotAuthorized,
		/// <summary>The user granted authorization to access cellular plan status information for the phone number.</summary>
		Authorized,
		/// <summary>Cellular plan status checks are unavailable for the phone number.</summary>
		Restricted,
		/// <summary>The device doesn't support authorization requests for cellular plan status.</summary>
		NotSupported,
	}

	/// <summary>Values that describe a device's quick-switch status.</summary>
	[NoTV, NoMac, iOS (27, 0), MacCatalyst (27, 0)]
	[Native]
	public enum CTQuickSwitchState : long {
		/// <summary>The framework couldn't determine the device's state due to an error.</summary>
		Failed,
		/// <summary>The device or phone number isn't enrolled in quick switch.</summary>
		NotEnrolled,
		/// <summary>The device is the active participant, and cellular service is available on it.</summary>
		Active,
		/// <summary>The device is passive, and another device holds the cellular service.</summary>
		Passive,
	}

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
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	interface CTCellularData {
		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("cellularDataRestrictionDidUpdateNotifier", ArgumentSemantic.Copy)]
		Action<CTCellularDataRestrictedState> RestrictionDidUpdateNotifier { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
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
	[Protocol, Model]
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

		[MacCatalyst (14, 0)]
		[Notification]
		[Field ("CTServiceRadioAccessTechnologyDidChangeNotification")]
		NSString ServiceRadioAccessTechnologyDidChangeNotification { get; }

		[MacCatalyst (14, 0)]
		[NullAllowed, Export ("dataServiceIdentifier")]
		string DataServiceIdentifier { get; }

		[MacCatalyst (14, 0)]
		[Wrap ("WeakDelegate")]
		[NullAllowed]
		ICTTelephonyNetworkInfoDelegate Delegate { get; set; }

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
		/// <param name="subscriber">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
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
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("carrierToken")]
		[NullAllowed]
		NSData CarrierToken { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("identifier")]
		string Identifier { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("delegate", ArgumentSemantic.Weak)]
		NSObject WeakDelegate { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
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
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Deprecated (PlatformName.iOS, 12, 1, message: "Use 'Subscribers' instead.")]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'Subscribers' instead.")]
		[Static]
		[Export ("subscriber")]
		CTSubscriber Subscriber { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Static]
		[Export ("subscribers")]
		CTSubscriber [] Subscribers { get; }
	}

	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	interface CTCellularPlanProvisioningRequest : NSSecureCoding {
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("address")]
		string Address { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("matchingID")]
		string MatchingId { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("OID")]
		string Oid { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("confirmationCode")]
		string ConfirmationCode { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("ICCID")]
		string Iccid { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("EID")]
		string Eid { get; set; }
	}

	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	interface CTCellularPlanProvisioning {
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("supportsCellularPlan")]
		bool SupportsCellularPlan { get; }

		/// <param name="request">To be added.</param>
		///         <param name="completionHandler">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Async (XmlDocs = """
			<param name="request">To be added.</param>
			<summary>To be added.</summary>
			<returns>To be added.</returns>
			<remarks>To be added.</remarks>
			""")]
		[Export ("addPlanWith:completionHandler:")]
		void AddPlan (CTCellularPlanProvisioningRequest request, Action<CTCellularPlanProvisioningAddPlanResult> completionHandler);

		[iOS (16, 0), MacCatalyst (16, 0)]
		[Export ("supportsEmbeddedSIM")]
		bool SupportsEmbeddedSim { get; }

		[Async]
		[NoMacCatalyst] /* headers say yes, but introspection says no, so keep it out of Mac Catalyst for now */
		[NoTV, NoMac, iOS (26, 0)]
		[Export ("addPlanWithRequest:properties:completionHandler:")]
		void AddPlan (CTCellularPlanProvisioningRequest request, [NullAllowed] CTCellularPlanProperties properties, CTCellularPlanProvisioningAddPlanCompletionHandler completionHandler);

		[Async]
		[NoTV, NoMac, iOS (26, 0), MacCatalyst (26, 0)]
		[Export ("updateCellularPlanProperties:completionHandler:")]
		void UpdateCellularPlan (CTCellularPlanProperties properties, CTCellularPlanProvisioningUpdateCellularPlanCompletionHandler completionHandler);
	}

	delegate void CTCellularPlanProvisioningAddPlanCompletionHandler (CTCellularPlanProvisioningAddPlanResult result);
	delegate void CTCellularPlanProvisioningUpdateCellularPlanCompletionHandler ([NullAllowed] NSError error);

	/// <summary>Represents lifecycle properties for a cellular plan.</summary>
	[NoTV, NoMac, iOS (26, 4), MacCatalyst (26, 4)]
	[BaseType (typeof (NSObject))]
	interface CTCellularPlanLifecycleProperties : NSSecureCoding {
		/// <summary>Gets or sets the expiration date of the cellular plan.</summary>
		[Export ("expirationDate", ArgumentSemantic.Assign)]
		NSDateComponents ExpirationDate { get; set; }
	}

	[NoTV, NoMac, iOS (26, 0), MacCatalyst (26, 0)]
	[BaseType (typeof (NSObject))]
	interface CTCellularPlanProperties : NSSecureCoding {
		[NullAllowed, Export ("associatedIccid")]
		string AssociatedIccid { get; set; }

		[Export ("simCapability", ArgumentSemantic.Assign)]
		CTCellularPlanCapability SimCapability { get; set; }

		[Export ("supportedRegionCodes", ArgumentSemantic.Assign)]
		string [] SupportedRegionCodes { get; set; }

		/// <summary>Gets or sets the lifecycle-related properties of the cellular plan.</summary>
		[iOS (26, 4), MacCatalyst (26, 4)]
		[NullAllowed, Export ("lifecycleProperties", ArgumentSemantic.Assign)]
		CTCellularPlanLifecycleProperties LifecycleProperties { get; set; }
	}

	[iOS (26, 0), MacCatalyst (26, 0), NoTV, NoMac]
	[BaseType (typeof (NSObject))]
	interface CTCellularPlanStatus {
		[Async]
		[Static]
		[Export ("getTokenWithCompletion:")]
		void GetToken (CTCellularPlanStatusGetTokenCompletionHandler completionHandler);

		[Async]
		[Static]
		[Export ("checkValidityOfToken:completionHandler:")]
		void CheckValidity (string token, CTCellularPlanStatusCheckValidityCompletionHandler completionHandler);

		/// <param name="phoneNumber">A phone number in E.164 format, such as <c>+15550001234</c>.</param>
		/// <param name="completionHandler">The handler to invoke with the authorization status or an error.</param>
		/// <summary>Presents a prompt that asks the user to allow cellular plan checks for a phone number.</summary>
		[NoTV, NoMac, iOS (27, 0), MacCatalyst (27, 0)]
		[Async (XmlDocs = """
			<param name="phoneNumber">A phone number in E.164 format, such as <c>+15550001234</c>.</param>
			<summary>Presents a prompt that asks the user to allow cellular plan checks for a phone number.</summary>
			<returns>A task that represents the asynchronous operation. The task result contains the authorization status.</returns>
			""")]
		[Static]
		[Export ("requestAuthorizationForPhoneNumber:completion:")]
		void RequestAuthorization (string phoneNumber, CTCellularPlanStatusAuthorizationCompletionHandler completionHandler);

		/// <param name="phoneNumber">A phone number in E.164 format, such as <c>+15550001234</c>.</param>
		/// <param name="completionHandler">The handler to invoke with the authorization status or an error.</param>
		/// <summary>Gets the current authorization status for a phone number without presenting any UI.</summary>
		[NoTV, NoMac, iOS (27, 0), MacCatalyst (27, 0)]
		[Async (XmlDocs = """
			<param name="phoneNumber">A phone number in E.164 format, such as <c>+15550001234</c>.</param>
			<summary>Gets the current authorization status for a phone number without presenting any UI.</summary>
			<returns>A task that represents the asynchronous operation. The task result contains the authorization status.</returns>
			""")]
		[Static]
		[Export ("getAuthorizationStatusForPhoneNumber:completion:")]
		void GetAuthorizationStatus (string phoneNumber, CTCellularPlanStatusAuthorizationCompletionHandler completionHandler);

		/// <param name="phoneNumber">A phone number in E.164 format, such as <c>+15550001234</c>.</param>
		/// <param name="completionHandler">The handler to invoke with the availability status, confidence, or an error.</param>
		/// <summary>Estimates whether the device has an active cellular plan for a phone number and the system's confidence in that determination.</summary>
		/// <remarks>Call this method only after an authorization request or status query returns <see cref="CTCellularPlanStatusAuthorization.Authorized" />.</remarks>
		[NoTV, NoMac, iOS (27, 0), MacCatalyst (27, 0)]
		[Async (ResultTypeName = "CTCellularPlanStatusHintResult", XmlDocs = """
			<param name="phoneNumber">A phone number in E.164 format, such as <c>+15550001234</c>.</param>
			<summary>Estimates whether the device has an active cellular plan for a phone number and the system's confidence in that determination.</summary>
			<returns>A task that represents the asynchronous operation. The task result contains the availability status and its confidence.</returns>
			<remarks>Call this method only after an authorization request or status query returns <see cref="CTCellularPlanStatusAuthorization.Authorized" />.</remarks>
			""")]
		[Static]
		[Export ("getStatusHintForPhoneNumber:completion:")]
		void GetStatusHint (string phoneNumber, CTCellularPlanStatusHintCompletionHandler completionHandler);
	}

	delegate void CTCellularPlanStatusGetTokenCompletionHandler ([NullAllowed] string token, [NullAllowed] NSError error);
	delegate void CTCellularPlanStatusCheckValidityCompletionHandler (bool isValid, [NullAllowed] NSError error);

	/// <param name="status">The authorization status.</param>
	/// <param name="error">The error that occurred, or <see langword="null" /> if the operation succeeded.</param>
	/// <summary>Handles the result of a cellular plan status authorization operation.</summary>
	[NoTV, NoMac, iOS (27, 0), MacCatalyst (27, 0)]
	delegate void CTCellularPlanStatusAuthorizationCompletionHandler (CTCellularPlanStatusAuthorization status, [NullAllowed] NSError error);

	/// <param name="availability">The cellular plan availability.</param>
	/// <param name="confidence">The confidence in the availability status.</param>
	/// <param name="error">The error that occurred, or <see langword="null" /> if the operation succeeded.</param>
	/// <summary>Handles the result of a cellular plan status hint operation.</summary>
	[NoTV, NoMac, iOS (27, 0), MacCatalyst (27, 0)]
	delegate void CTCellularPlanStatusHintCompletionHandler (CTCellularPlanStatusAvailability availability, CTCellularPlanStatusAvailabilityConfidence confidence, [NullAllowed] NSError error);

	interface ICTQuickSwitchManagerDelegate { }

	/// <summary>Methods for responding to changes in a device's quick-switch state.</summary>
	[NoTV, NoMac, iOS (27, 0), MacCatalyst (27, 0)]
	[Protocol (BackwardsCompatibleCodeGeneration = false), Model]
	[BaseType (typeof (NSObject))]
	interface CTQuickSwitchManagerDelegate {
		/// <param name="manager">The quick-switch manager whose state changed.</param>
		/// <param name="state">The new quick-switch state.</param>
		/// <summary>Notifies the delegate that the device's quick-switch state changed.</summary>
		[Export ("quickSwitchManager:didChangeToState:")]
		void DidChangeToState (CTQuickSwitchManager manager, CTQuickSwitchState state);
	}

	/// <param name="state">The quick-switch state.</param>
	/// <param name="error">The error that occurred, or <see langword="null" /> if the operation succeeded.</param>
	/// <summary>Handles the result of a quick-switch state query.</summary>
	[NoTV, NoMac, iOS (27, 0), MacCatalyst (27, 0)]
	delegate void CTQuickSwitchManagerStateCompletionHandler (CTQuickSwitchState state, [NullAllowed] NSError error);

	/// <param name="error">The error that occurred, or <see langword="null" /> if the operation succeeded.</param>
	/// <summary>Handles the completion of a quick-switch manager operation.</summary>
	[NoTV, NoMac, iOS (27, 0), MacCatalyst (27, 0)]
	delegate void CTQuickSwitchManagerCompletionHandler ([NullAllowed] NSError error);

	/// <summary>Enables an app to register for and query a device's quick-switch state.</summary>
	[NoTV, NoMac, iOS (27, 0), MacCatalyst (27, 0)]
	[BaseType (typeof (NSObject))]
	interface CTQuickSwitchManager {
		/// <summary>Gets or sets the object that the system notifies about quick-switch events.</summary>
		[Wrap ("WeakDelegate")]
		[NullAllowed]
		ICTQuickSwitchManagerDelegate Delegate { get; set; }

		/// <summary>Gets or sets the untyped delegate object.</summary>
		[NullAllowed, Export ("delegate", ArgumentSemantic.Weak)]
		NSObject WeakDelegate { get; set; }

		/// <param name="phoneNumberSuffix">The last four digits of the phone number whose state to query.</param>
		/// <param name="completionHandler">The handler to invoke with the quick-switch state or an error.</param>
		/// <summary>Gets the quick-switch state for a phone number whose suffix matches the provided value.</summary>
		/// <remarks>The framework presents a consent screen. If the user denies consent, the operation returns <see cref="CTQuickSwitchState.NotEnrolled" /> without an error.</remarks>
		[Async (XmlDocs = """
			<param name="phoneNumberSuffix">The last four digits of the phone number whose state to query.</param>
			<summary>Gets the quick-switch state for a phone number whose suffix matches the provided value.</summary>
			<returns>A task that represents the asynchronous operation. The task result contains the quick-switch state.</returns>
			<remarks>The framework presents a consent screen. If the user denies consent, the operation returns <see cref="CTQuickSwitchState.NotEnrolled" /> without an error.</remarks>
			""")]
		[Export ("getPhoneNumberStateForSuffix:completion:")]
		void GetPhoneNumberState (string phoneNumberSuffix, CTQuickSwitchManagerStateCompletionHandler completionHandler);

		/// <param name="completionHandler">The handler to invoke with the quick-switch state or an error.</param>
		/// <summary>Gets the device's quick-switch state.</summary>
		[Async (XmlDocs = """
			<summary>Gets the device's quick-switch state.</summary>
			<returns>A task that represents the asynchronous operation. The task result contains the quick-switch state.</returns>
			""")]
		[Export ("getDeviceState:")]
		void GetDeviceState (CTQuickSwitchManagerStateCompletionHandler completionHandler);

		/// <param name="completionHandler">The handler to invoke when registration completes.</param>
		/// <summary>Registers the app for background launch whenever the device's quick-switch state changes.</summary>
		[Async (XmlDocs = """
			<summary>Registers the app for background launch whenever the device's quick-switch state changes.</summary>
			<returns>A task that represents the asynchronous operation.</returns>
			""")]
		[Static]
		[Export ("registerForLaunchOnQuickSwitchStateEvents:")]
		void RegisterForLaunchOnQuickSwitchStateEvents (CTQuickSwitchManagerCompletionHandler completionHandler);

		/// <param name="completionHandler">The handler to invoke when unregistration completes.</param>
		/// <summary>Removes the app's registration for background launch on quick-switch state changes.</summary>
		[Async (XmlDocs = """
			<summary>Removes the app's registration for background launch on quick-switch state changes.</summary>
			<returns>A task that represents the asynchronous operation.</returns>
			""")]
		[Static]
		[Export ("unregisterForLaunchOnQuickSwitchStateEvents:")]
		void UnregisterForLaunchOnQuickSwitchStateEvents (CTQuickSwitchManagerCompletionHandler completionHandler);
	}
}
