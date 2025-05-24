//
// VideoSubscriberAccount bindings
//
// Authors:
//	Alex Soto  <alex.soto@xamarin.com>
//
// Copyright 2016 Xamarin Inc. All rights reserved.
// Copyright 2018-2019 Microsoft Corporation.
//

using System;
using System.ComponentModel;
using Foundation;
using ObjCRuntime;
#if MONOMAC
using UIViewController = AppKit.NSViewController;
#else
using UIKit;
#endif

namespace VideoSubscriberAccount {

	/// <summary>Encapsulates errors that may occur during attempts to verify credentials.</summary>
	[Native]
	[NoMacCatalyst]
	[ErrorDomain ("VSErrorDomain")]
	public enum VSErrorCode : long {
		/// <summary>The user has not affirmatively provided access.</summary>
		AccessNotGranted = 0,
		/// <summary>The requested provider is not supported.</summary>
		UnsupportedProvider = 1,
		/// <summary>The application cancelled the request.</summary>
		UserCancelled = 2,
		/// <summary>A transient error has occurred.</summary>
		ServiceTemporarilyUnavailable = 3,
		/// <summary>To be added.</summary>
		ProviderRejected = 4,
		/// <summary>The provider does not recognize the verification token provided by the app.</summary>
		InvalidVerificationToken = 5,
		Rejected = 6,
		Unsupported = 7,
	}

	/// <summary>Enumerates the types of access the app has to the user's subscription.</summary>
	[Native]
	[NoMacCatalyst]
	public enum VSAccountAccessStatus : long {
		/// <summary>The user has not interacted with the permissions dialog.</summary>
		NotDetermined = 0,
		/// <summary>The user is denied access and may not change permission.</summary>
		Restricted = 1,
		/// <summary>The user has denied the app access.</summary>
		Denied = 2,
		/// <summary>The user has granted the app access.</summary>
		Granted = 3,
	}

#if !XAMCORE_5_0
	[EditorBrowsable (EditorBrowsableState.Never)]
	[Obsolete ("Use 'VSUserAccountQueryOptions' instead.")]
	[TV (16, 0), Mac (13, 0), iOS (16, 0), NoMacCatalyst]
	[Native]
	enum VSUserAccountQueryOption : ulong {
		None = 0,
		AllDevices,
	}
#endif // !XAMCORE_5_0

	[TV (16, 0), NoMacCatalyst, iOS (16, 0), Mac (13, 0)]
	[Flags]
	[Native]
	public enum VSUserAccountQueryOptions : long {
		None = 0x0,
		AllDevices,
	}

	[TV (16, 0), NoMacCatalyst, iOS (16, 0), Mac (13, 0)]
	[Native]
	public enum VSUserAccountType : long {
		Free,
		Paid,
	}

	[TV (16, 0), NoMacCatalyst, iOS (16, 0), Mac (13, 0)]
	[Native]
	public enum VSOriginatingDeviceCategory : long {
		Mobile,
		Other,
	}


	[NoMacCatalyst]
	[Static]
	[Internal]
	interface VSErrorInfoKeys {

		[Field ("VSErrorInfoKeySAMLResponse")]
		NSString SamlResponseKey { get; }

		[Field ("VSErrorInfoKeySAMLResponseStatus")]
		NSString SamlResponseStatusKey { get; }

		[Field ("VSErrorInfoKeyUnsupportedProviderIdentifier")]
		NSString UnsupportedProviderIdentifierKey { get; }

		[Field ("VSErrorInfoKeyAccountProviderResponse")]
		NSString AccountProviderResponseKey { get; }
	}

	[NoMacCatalyst]
	[StrongDictionary ("VSErrorInfoKeys")]
	interface VSErrorInfo {

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		string SamlResponse { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		string SamlResponseStatus { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		string UnsupportedProviderIdentifier { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		string AccountProviderResponse { get; }
	}

	/// <summary>Interface representing the required methods (if any) of the protocol <see cref="VideoSubscriberAccount.VSAccountManagerDelegate" />.</summary>
	///     <remarks>
	///       <para>This interface contains the required methods (if any) from the protocol defined by <see cref="VideoSubscriberAccount.VSAccountManagerDelegate" />.</para>
	///       <para>If developers create classes that implement this interface, the implementation methods will automatically be exported to Objective-C with the matching signature from the method defined in the <see cref="VideoSubscriberAccount.VSAccountManagerDelegate" /> protocol.</para>
	///       <para>Optional methods (if any) are provided by the <see cref="VideoSubscriberAccount.VSAccountManagerDelegate_Extensions" /> class as extension methods to the interface, allowing developers to invoke any optional methods on the protocol.</para>
	///     </remarks>
	interface IVSAccountManagerDelegate { }

	/// <related type="externalDocumentation" href="https://developer.apple.com/reference/VideoSubscriberAccount/VSAccountManagerDelegate">Apple documentation for <c>VSAccountManagerDelegate</c></related>
	[Protocol, Model]
	[NoMacCatalyst]
	[BaseType (typeof (NSObject))]
	interface VSAccountManagerDelegate {

		/// <param name="accountManager">To be added.</param>
		///         <param name="viewController">To be added.</param>
		///         <summary>Developers override this to specify the <see cref="UIKit.UIViewController" /> to be shown when the <see cref="VideoSubscriberAccounts.VSAccountManager" /> requires user interaction.</summary>
		///         <remarks>To be added.</remarks>
		[Abstract]
		[NoMac]
		[Export ("accountManager:presentViewController:")]
		void PresentViewController (VSAccountManager accountManager, UIViewController viewController);

		/// <param name="accountManager">To be added.</param>
		///         <param name="viewController">To be added.</param>
		///         <summary>Called after the user has interacted with the <paramref name="viewController" />.</summary>
		///         <remarks>To be added.</remarks>
		[Abstract]
		[NoMac]
		[Export ("accountManager:dismissViewController:")]
		void DismissViewController (VSAccountManager accountManager, UIViewController viewController);

		/// <param name="accountManager">To be added.</param>
		///         <param name="accountProviderIdentifier">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("accountManager:shouldAuthenticateAccountProviderWithIdentifier:")]
		bool ShouldAuthenticateAccountProvider (VSAccountManager accountManager, string accountProviderIdentifier);
	}

	/// <summary>Coordinates access to the user's subscription.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/reference/VideoSubscriberAccount/VSAccountManager">Apple documentation for <c>VSAccountManager</c></related>
	[NoMacCatalyst]
	[BaseType (typeof (NSObject))]
	interface VSAccountManager {

		/// <summary>An instance of the VideoSubscriberAccount.IVSAccountManagerDelegate model class which acts as the class delegate.</summary>
		///         <value>The instance of the VideoSubscriberAccount.IVSAccountManagerDelegate model class</value>
		///         <remarks>
		///           <para>The delegate instance assigned to this object will be used to handle events or provide data on demand to this class.</para>
		///           <para>When setting the Delegate or WeakDelegate values events will be delivered to the specified instance instead of being delivered to the C#-style events</para>
		///           <para>This is the strongly typed version of the object, developers should use the WeakDelegate property instead if they want to merely assign a class derived from NSObject that has been decorated with [Export] attributes.</para>
		///         </remarks>
		[NullAllowed, Export ("delegate", ArgumentSemantic.Weak)]
		IVSAccountManagerDelegate Delegate { get; set; }

		/// <param name="options">If not empty, may contain the key <see cref="VideoSubscriberAccount.VSCheckAccessOptionKeys" />.</param>
		///         <param name="completionHandler">Called by the system with the results of the permission check.</param>
		///         <summary>Checks whether the user has provided permission for the app to access their subscription information.</summary>
		///         <remarks>To be added.</remarks>
		[NoMac]
		[Async (XmlDocs = """
			<param name="options">If not empty, may contain the key .</param>
			<summary>Checks whether the user has provided permission for the app to access their subscription information.</summary>
			<returns>
			          <para class="improve-task-t-return-type-description">A task that represents the asynchronous CheckAccessStatus operation.  The value of the TResult parameter is of type System.Action&lt;VideoSubscriberAccount.VSAccountAccessStatus,Foundation.NSError&gt;.</para>
			        </returns>
			<remarks>To be added.</remarks>
			""")]
		[Export ("checkAccessStatusWithOptions:completionHandler:")]
		void CheckAccessStatus (NSDictionary options, Action<VSAccountAccessStatus, NSError> completionHandler);

		/// <param name="accountMetadataRequest">To be added.</param>
		///         <param name="completionHandler">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[NoMac]
		[Async (XmlDocs = """
			<param name="accountMetadataRequest">To be added.</param>
			<summary>To be added.</summary>
			<returns>
			          <para class="improve-task-t-return-type-description">A task that represents the asynchronous Enqueue operation.  The value of the TResult parameter is of type System.Action&lt;VideoSubscriberAccount.VSAccountMetadata,Foundation.NSError&gt;.</para>
			        </returns>
			<remarks>
			          <para copied="true">The EnqueueAsync method is suitable to be used with C# async by returning control to the caller with a Task representing the operation.</para>
			          <para copied="true">To be added.</para>
			        </remarks>
			""",
			XmlDocsWithOutParameter = """
			<param name="accountMetadataRequest">To be added.</param>
			<param name="result">To be added.</param>
			<summary>To be added.</summary>
			<returns>To be added.</returns>
			<remarks>To be added.</remarks>
			""")]
		[Export ("enqueueAccountMetadataRequest:completionHandler:")]
		VSAccountManagerResult Enqueue (VSAccountMetadataRequest accountMetadataRequest, Action<VSAccountMetadata, NSError> completionHandler);

		[NoMac]
		[TV (13, 0)]
		[iOS (13, 0)]
		[Field ("VSOpenTVProviderSettingsURLString")]
		NSString OpenTVProviderSettingsUrl { get; }
	}

	[Static]
	[Internal]
	[NoMacCatalyst]
	interface VSCheckAccessOptionKeys {

		[Field ("VSCheckAccessOptionPrompt")]
		NSString CheckAccessOptionPrompt { get; }
	}

	/// <summary>A <see cref="Foundation.DictionaryContainer" /> holding keys appropriate to <see cref="VideoSubscriberAccount.VSAccountManager.CheckAccessStatusAsync(VideoSubscriberAccount.VSAccountManagerAccessOptions)" /> and <see cref="VideoSubscriberAccount.VSAccountManager.CheckAccessStatusAsync(VideoSubscriberAccount.VSAccountManagerAccessOptions)" />.</summary>
	[NoMacCatalyst]
	[StrongDictionary ("VSCheckAccessOptionKeys")]
	interface VSAccountManagerAccessOptions {

		/// <summary>If not <see langword="null" />, specifies whether the user should be asked for access permission.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("CheckAccessOptionPrompt")]
		bool CheckAccessOptionPrompt { get; set; }
	}

	/// <summary>Represents a cancellable request that is still "in flight".</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/reference/VideoSubscriberAccount/VSAccountManagerResult">Apple documentation for <c>VSAccountManagerResult</c></related>
	[NoMacCatalyst]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface VSAccountManagerResult {

		/// <summary>Informs the <see cref="VSAccountManager" /> that the app no longer needs the requested work.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("cancel")]
		void Cancel ();
	}

	/// <summary>Information about a subscription.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/reference/VideoSubscriberAccount/VSAccountMetadata">Apple documentation for <c>VSAccountMetadata</c></related>
	[NoMacCatalyst]
	[BaseType (typeof (NSObject))]
	interface VSAccountMetadata {

		/// <summary>A unique identifier for the account provider.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("accountProviderIdentifier")]
		string AccountProviderIdentifier { get; }

		/// <summary>When the user will be asked to re-authenticate.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("authenticationExpirationDate", ArgumentSemantic.Copy)]
		NSDate AuthenticationExpirationDate { get; }

		/// <summary>Arbitrary data, for instance to hold cryptographic verify the <see cref="VideoSubscriberAccount.VSAccountMetadata.SamlAttributeQueryResponse" />.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("verificationData", ArgumentSemantic.Copy)]
		NSData VerificationData { get; }

		/// <summary>If not <see langword="null" />, the SAML AttributeQuery response from the provider.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("SAMLAttributeQueryResponse")]
		string SamlAttributeQueryResponse { get; }

		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("accountProviderResponse", ArgumentSemantic.Strong)]
		VSAccountProviderResponse AccountProviderResponse { get; }
	}

	/// <summary>Specifies information being requested from the subscriber's account.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/reference/VideoSubscriberAccount/VSAccountMetadataRequest">Apple documentation for <c>VSAccountMetadataRequest</c></related>
	[NoMacCatalyst]
	[BaseType (typeof (NSObject))]
	interface VSAccountMetadataRequest {

		/// <summary>If not <see langword="null" />, identifies the source of the request.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("channelIdentifier")]
		string ChannelIdentifier { get; set; }

		/// <summary>If not empty, a list of provider identifiers that are allowed to respond to the request.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("supportedAccountProviderIdentifiers", ArgumentSemantic.Copy)]
		string [] SupportedAccountProviderIdentifiers { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("featuredAccountProviderIdentifiers", ArgumentSemantic.Copy)]
		string [] FeaturedAccountProviderIdentifiers { get; set; }

		/// <summary>If not <see langword="null" />, a value that the provider requires to verify the identity of the requesting app.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("verificationToken")]
		string VerificationToken { get; set; }

		/// <summary>Whether to ask for the provider's unique identification string.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("includeAccountProviderIdentifier")]
		bool IncludeAccountProviderIdentifier { get; set; }

		/// <summary>Whether to ask when the authentication expires.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("includeAuthenticationExpirationDate")]
		bool IncludeAuthenticationExpirationDate { get; set; }

		/// <summary>If not <see langword="null" />, a user-readable title of the video that the app will play on successful authentication.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("localizedVideoTitle")]
		string LocalizedVideoTitle { get; set; }

		/// <summary>If <see langword="true" />, the user may be prompted to authenticate.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("interruptionAllowed")]
		bool InterruptionAllowed { [Bind ("isInterruptionAllowed")] get; set; }

		/// <summary>If <see langword="true" />, cached credentials will not be used.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("forceAuthentication")]
		bool ForceAuthentication { get; set; }

		/// <summary>Values to be added to the SAML <c>attributeQuery</c> sent to the provider.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("attributeNames", ArgumentSemantic.Copy)]
		string [] AttributeNames { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Protected]
		[Export ("supportedAuthenticationSchemes", ArgumentSemantic.Copy)]
		NSString [] SupportedAuthenticationSchemesString { get; set; }

		[iOS (13, 0)]
		[TV (13, 0)]
		[NullAllowed, Export ("accountProviderAuthenticationToken")]
		string AccountProviderAuthenticationToken { get; set; }

		[TV (14, 2), iOS (14, 2)]
		[NullAllowed, Export ("applicationAccountProviders", ArgumentSemantic.Copy)]
		VSAccountApplicationProvider [] ApplicationAccountProviders { get; set; }
	}

	[NoMacCatalyst]
	[BaseType (typeof (NSObject))]
	interface VSAccountProviderResponse {

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Protected]
		[Export ("authenticationScheme")]
		NSString AuthenticationSchemeString { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Wrap ("VSAccountProviderAuthenticationSchemeExtensions.GetValue (AuthenticationSchemeString)")]
		VSAccountProviderAuthenticationScheme AuthenticationScheme { get; }

		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("status")]
		string Status { get; }

		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("body")]
		string Body { get; }
	}

	[NoMacCatalyst]
	enum VSAccountProviderAuthenticationScheme {
		/// <summary>To be added.</summary>
		[Field ("VSAccountProviderAuthenticationSchemeSAML")]
		Saml,

		[iOS (13, 0)]
		[TV (13, 0)]
		[Field ("VSAccountProviderAuthenticationSchemeAPI")]
		Api,
	}

	[Deprecated (PlatformName.iOS, 18, 0, message: "Use the 'VSUserAccountType' enum instead.")]
	[Deprecated (PlatformName.TvOS, 18, 0, message: "Use the 'VSUserAccountType' enum instead.")]
	[Deprecated (PlatformName.MacOSX, 15, 0, message: "Use the 'VSUserAccount' type instead.")]
	[NoMacCatalyst]
	[Native]
	public enum VSSubscriptionAccessLevel : long {
		/// <summary>To be added.</summary>
		Unknown,
		/// <summary>To be added.</summary>
		FreeWithAccount,
		/// <summary>To be added.</summary>
		Paid,
	}

	[Deprecated (PlatformName.iOS, 18, 0, message: "Use the 'VSUserAccount' type instead.")]
	[Deprecated (PlatformName.TvOS, 18, 0, message: "Use the 'VSUserAccount' type instead.")]
	[Deprecated (PlatformName.MacOSX, 15, 0, message: "Use the 'VSUserAccount' type instead.")]
	[NoMacCatalyst]
	[BaseType (typeof (NSObject))]
	interface VSSubscription {
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NullAllowed] // null_resettable
		[Export ("expirationDate", ArgumentSemantic.Copy)]
		NSDate ExpirationDate { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("accessLevel", ArgumentSemantic.Assign)]
		VSSubscriptionAccessLevel AccessLevel { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NullAllowed] // null_resettable
		[Export ("tierIdentifiers", ArgumentSemantic.Copy)]
		string [] TierIdentifiers { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("billingIdentifier")]
		string BillingIdentifier { get; set; }
	}

	[Deprecated (PlatformName.iOS, 18, 0, message: "Use the 'VSUserAccountManager' type instead.")]
	[Deprecated (PlatformName.TvOS, 18, 0, message: "Use the 'VSUserAccountManager' type instead.")]
	[Deprecated (PlatformName.MacOSX, 15, 0, message: "Use the 'VSUserAccount' type instead.")]
	[NoMacCatalyst]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface VSSubscriptionRegistrationCenter {
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Static]
		[Export ("defaultSubscriptionRegistrationCenter")]
		VSSubscriptionRegistrationCenter Default { get; }

		/// <param name="currentSubscription">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("setCurrentSubscription:")]
		void SetCurrentSubscription ([NullAllowed] VSSubscription currentSubscription);
	}

	[TV (14, 2), iOS (14, 2)]
	[NoMacCatalyst]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface VSAccountApplicationProvider {

		[Export ("initWithLocalizedDisplayName:identifier:")]
		NativeHandle Constructor (string localizedDisplayName, string identifier);

		[Export ("localizedDisplayName")]
		string LocalizedDisplayName { get; }

		[Export ("identifier")]
		string Identifier { get; }
	}

	[TV (16, 0), NoMacCatalyst, iOS (16, 0), Mac (13, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface VSUserAccountManager {
		[Static]
		[Export ("sharedUserAccountManager")]
		VSUserAccountManager SharedUserAccountManager { get; }

		[Async]
		[Export ("updateUserAccount:completion:")]
		void UpdateUserAccount (VSUserAccount account, [NullAllowed] Action<NSError> completion);

		[Async]
		[Export ("queryUserAccountsWithOptions:completion:")]
		void QueryUserAccounts (VSUserAccountQueryOptions options, Action<NSArray<VSUserAccount>, NSError> completion);
	}

	[TV (16, 0), NoMacCatalyst, iOS (16, 0), Mac (13, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface VSUserAccount {
		[NullAllowed, Export ("updateURL", ArgumentSemantic.Copy)]
		NSUrl UpdateUrl { get; set; }

		[Export ("requiresSystemTrust")]
		bool RequiresSystemTrust { get; set; }

		[NullAllowed, Export ("accountProviderIdentifier", ArgumentSemantic.Strong)]
		string AccountProviderIdentifier { get; set; }

		[NullAllowed, Export ("identifier")]
		string Identifier { get; set; }

		[Export ("accountType", ArgumentSemantic.Assign)]
		VSUserAccountType AccountType { get; set; }

		[Obsoleted (PlatformName.iOS, 16, 4, message: Constants.ApiRemovedGeneral)]
		[Obsoleted (PlatformName.TvOS, 16, 4, message: Constants.ApiRemovedGeneral)]
		[Obsoleted (PlatformName.MacOSX, 13, 3, message: Constants.ApiRemovedGeneral)]
		[Export ("deleted")]
		bool Deleted { [Bind ("isDeleted")] get; set; }

		[TV (16, 4), NoMacCatalyst, iOS (16, 4), Mac (13, 3)]
		[Export ("signedOut")]
		bool SignedOut { [Bind ("isSignedOut")] get; set; }

		[NullAllowed, Export ("subscriptionBillingCycleEndDate", ArgumentSemantic.Copy)]
		NSDate SubscriptionBillingCycleEndDate { get; set; }

		[NullAllowed, Export ("tierIdentifiers", ArgumentSemantic.Copy)]
		string [] TierIdentifiers { get; set; }

		[NullAllowed, Export ("billingIdentifier")]
		string BillingIdentifier { get; set; }

		[NullAllowed, Export ("authenticationData")]
		string AuthenticationData { get; set; }

		[Export ("fromCurrentDevice")]
		bool FromCurrentDevice { [Bind ("isFromCurrentDevice")] get; }

		[Export ("deviceCategory")]
		VSOriginatingDeviceCategory DeviceCategory { get; }

		[TV (17, 4), NoMacCatalyst, Mac (14, 4), iOS (17, 4)]
		[NullAllowed, Export ("appleSubscription", ArgumentSemantic.Strong)]
		VSAppleSubscription AppleSubscription { get; set; }

		[Export ("initWithAccountType:updateURL:")]
		NativeHandle Constructor (VSUserAccountType accountType, [NullAllowed] NSUrl url);
	}

	[TV (17, 4), NoMacCatalyst, Mac (14, 4), iOS (17, 4)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface VSAppleSubscription {

		[Export ("customerID", ArgumentSemantic.Strong)]
		string CustomerId { get; set; }

		[Export ("productCodes", ArgumentSemantic.Strong)]
		string [] ProductCodes { get; set; }

		[Export ("initWithCustomerID:productCodes:")]
		NativeHandle Constructor (string customerId, string [] productCodes);
	}

}
