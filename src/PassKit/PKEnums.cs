// Copyright 2012-2014 Xamarin Inc. All rights reserved.

using System;
using Foundation;
using ObjCRuntime;

namespace PassKit {

#if !NET
	// untyped enum -> PKError.h
	// This never seemed to be deprecatd, yet in iOS8 it's obsoleted
	// this enum does not show up in the headers anymore
	[Obsoleted (PlatformName.iOS, 8, 0)]
	[NoMac]
	public enum PKErrorCode {
		None = 0,
		Unknown = 1,
		NotEntitled,
		PermissionDenied, // new in iOS7
	}
#endif

	// NSInteger -> PKPass.h
	/// <summary>An enumeration whose values specify errors relating to the passes and passbook functionality.</summary>
	[MacCatalyst (13, 1)]
	[ErrorDomain ("PKPassKitErrorDomain")]
	[Native]
	public enum PKPassKitErrorCode : long {
		/// <summary>An unknown error.</summary>
		Unknown = -1,
		/// <summary>No error.</summary>
		None = 0,
		/// <summary>Invalid data.</summary>
		InvalidData = 1,
		/// <summary>The pass version is not supported on this device.</summary>
		UnsupportedVersion,
		/// <summary>Indicates a mismatch in the signature, such as pass type identifier in the certificate versus the pass.</summary>
		InvalidSignature,
		/// <summary>The app does not have the required entitlements.</summary>
		NotEntitled,
	}

	// NSInteger -> PKPassLibrary.h
	/// <summary>An enumeration whose values define possible results when passes are added.</summary>
	[MacCatalyst (13, 1)]
	[Native]
	public enum PKPassLibraryAddPassesStatus : long {
		/// <summary>Passes were successfully added.</summary>
		DidAddPasses,
		/// <summary>The app should display the approval UI to the user.</summary>
		ShouldReviewPasses,
		/// <summary>The user cancelled the addition of passes.</summary>
		DidCancelAddPasses,
	}

	/// <summary>Enumeration of pass types (whether a pass is a barcode or presents a payment card).</summary>
	[MacCatalyst (13, 1)]
	[Native]
	public enum PKPassType : ulong {
		/// <summary>The pass is a barcode.</summary>
		Barcode,
		SecureElement,
		/// <summary>A payment card.</summary>
		[NoMac]
		[Deprecated (PlatformName.iOS, 13, 4, message: "Use 'SecureElement' instead.")]
		[MacCatalyst (13, 1)]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'SecureElement' instead.")]
		Payment = SecureElement,
		Any = ulong.MaxValue,
	}

	/// <summary>Enumeration of results of authorization requests.</summary>
	[MacCatalyst (13, 1)]
	[Native]
	public enum PKPaymentAuthorizationStatus : long {
		/// <summary>The authorization succeeded.</summary>
		Success,
		/// <summary>Authorization failed.</summary>
		Failure,

		/// <summary>Developers should not use this deprecated field. Developers should use 'Failure' and 'PKPaymentRequest.CreatePaymentBillingAddressInvalidError'.</summary>
		[NoMac]
		[Deprecated (PlatformName.iOS, 11, 0, message: "Use 'Failure' and 'PKPaymentRequest.CreatePaymentBillingAddressInvalidError'.")]
		[MacCatalyst (13, 1)]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'Failure' and 'PKPaymentRequest.CreatePaymentBillingAddressInvalidError'.")]
		InvalidBillingPostalAddress,

		/// <summary>Unusable shipping address.</summary>
		[NoMac]
		[Deprecated (PlatformName.iOS, 11, 0, message: "Use 'Failure' and 'PKPaymentRequest.CreatePaymentShippingAddressInvalidError'.")]
		[MacCatalyst (13, 1)]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'Failure' and 'PKPaymentRequest.CreatePaymentShippingAddressInvalidError'.")]
		InvalidShippingPostalAddress,

		/// <summary>Unusable shipping contact.</summary>
		[NoMac]
		[Deprecated (PlatformName.iOS, 11, 0, message: "Use 'Failure' and 'PKPaymentRequest.CreatePaymentContactInvalidError'.")]
		[MacCatalyst (13, 1)]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'Failure' and 'PKPaymentRequest.CreatePaymentContactInvalidError'.")]
		InvalidShippingContact,

		/// <summary>Indicates that a PIN is required for the action.</summary>
		[MacCatalyst (13, 1)]
		PinRequired,
		/// <summary>Indicates that an invalid PIN was entered.</summary>
		[MacCatalyst (13, 1)]
		PinIncorrect,
		/// <summary>Indicates that too many incorrect PINs were entered and the user is locked out for PIN transactions.</summary>
		[MacCatalyst (13, 1)]
		PinLockout,
	}

	/// <summary>Enumeration of valid states of a <see cref="T:PassKit.PKPaymentPass" /> payment card.</summary>
	[NoMac]
	[Deprecated (PlatformName.iOS, 13, 4, message: "Use 'PKSecureElementPassActivationState' instead.")]
	[MacCatalyst (13, 1)]
	[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'PKSecureElementPassActivationState' instead.")]
	[Native]
	public enum PKPaymentPassActivationState : ulong {
		/// <summary>Ready to use for payment.</summary>
		Activated,
		/// <summary>Inactive. Requires user intervention to enable.</summary>
		RequiresActivation,
		/// <summary>Not ready to use, but activation is in progress.</summary>
		Activating,
		/// <summary>Inactive and cannot be reactivated.</summary>
		Suspended,
		/// <summary>The user has proactively disabled the asssociated account.</summary>
		Deactivated,
	}

	[iOS (13, 4)]
	[MacCatalyst (13, 1)]
	[Native]
	public enum PKSecureElementPassActivationState : long {
		Activated,
		RequiresActivation,
		Activating,
		Suspended,
		Deactivated,
	}

	/// <summary>Payment processing capabilities of a merchant.</summary>
	[MacCatalyst (13, 1)]
	[Native]
	public enum PKMerchantCapability : ulong {
		/// <summary>The 3D Secure payment protocol.</summary>
		ThreeDS = 1 << 0,
		/// <summary>Supports the EMV (EuroCard, MasterCard, Visa) payment protocol.</summary>
		EMV = 1 << 1,
		/// <summary>Supports credit cards.</summary>
		Credit = 1 << 2,
		/// <summary>Supports debit cards.</summary>
		Debit = 1 << 3,
		[iOS (17, 0), Mac (14, 0), NoTV, MacCatalyst (17, 0)]
		InstantFundsOut = 1 << 7,
	}

	/// <summary>Holds address information for billing or shipping purposes.</summary>
	[NoMac]
	[Deprecated (PlatformName.iOS, 11, 0, message: "Use 'PKContactField' instead.")]
	[MacCatalyst (13, 1)]
	[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'PKContactField' instead.")]
	[Native]
	[Flags]
	public enum PKAddressField : ulong {
		/// <summary>None of the fields.</summary>
		None = 0,
		/// <summary>The buyer's address.</summary>
		PostalAddress = 1 << 0,
		/// <summary>The buyer's phone number.</summary>
		Phone = 1 << 1,
		/// <summary>The buyer's email.</summary>
		Email = 1 << 2,
		/// <summary>The buyer's full name.</summary>
		[MacCatalyst (13, 1)]
		Name = 1 << 3,
		/// <summary>All the fields.</summary>
		All = PostalAddress | Phone | Email | Name,
	}

	/// <summary>Enumerates available styles for <see cref="T:PassKit.PKPaymentButton" /> objects.</summary>
	[MacCatalyst (13, 1)]
	[Native]
	public enum PKPaymentButtonStyle : long {
		/// <summary>White background with black lettering.</summary>
		White,
		/// <summary>White background with black outline and black lettering.</summary>
		WhiteOutline,
		/// <summary>Black background with white lettering.</summary>
		Black,
		[iOS (14, 0)]
		[MacCatalyst (14, 0)]
		Automatic = 3,
	}

	/// <summary>Enumerates the types of <see cref="T:PassKit.PKPaymentButton" /> objects.</summary>
	[MacCatalyst (13, 1)]
	[Native]
	public enum PKPaymentButtonType : long {
		/// <summary>A button with the Apple Pay logo.</summary>
		Plain,
		/// <summary>A button reading "Buy with" and the Apple Pay logo.</summary>
		Buy,
		/// <summary>A button that prompts the user to set a card up for payment with Apple Pay.</summary>
		[MacCatalyst (13, 1)]
		SetUp,
		/// <summary>A button that reads "Pay with," accompanied by the Apple Pay logo.</summary>
		[MacCatalyst (13, 1)]
		InStore,
		/// <summary>A button that reads "Donate with" and the Apple Pay logo.</summary>
		[MacCatalyst (13, 1)]
		Donate,
#if NET
		[MacCatalyst (13, 1)]
		Checkout,
		[MacCatalyst (13, 1)]
		Book,
#else
		[Obsolete ("Use 'Book2'.")]
		Book,
		[Obsolete ("Use 'Checkout2'.")]
		Checkout,
#endif // !NET
		[MacCatalyst (13, 1)]
		Subscribe,
		[iOS (14, 0)]
		[MacCatalyst (14, 0)]
		Reload = 8,
		[iOS (14, 0)]
		[MacCatalyst (14, 0)]
		AddMoney = 9,
		[iOS (14, 0)]
		[MacCatalyst (14, 0)]
		TopUp = 10,
		[iOS (14, 0)]
		[MacCatalyst (14, 0)]
		Order = 11,
		[iOS (14, 0)]
		[MacCatalyst (14, 0)]
		Rent = 12,
		[iOS (14, 0)]
		[MacCatalyst (14, 0)]
		Support = 13,
		[iOS (14, 0)]
		[MacCatalyst (14, 0)]
		Contribute = 14,
		[iOS (14, 0)]
		[MacCatalyst (14, 0)]
		Tip = 15,
		[iOS (15, 0)]
		[MacCatalyst (15, 0)]
		Continue = 16,
#if !NET
#pragma warning disable 0618 // warning CS0618: 'PKPaymentButtonType.[field]' is obsolete: 'Use '[replacement]'.'
		Book2 = Checkout,
		Checkout2 = Book,
#pragma warning restore
#endif // !NET
	}

	/// <summary>Enumerates shipping methods.</summary>
	[MacCatalyst (13, 1)]
	[Native]
	public enum PKShippingType : ulong {
		/// <summary>The product will be delivered by a third-party service (Default).</summary>
		Shipping,
		/// <summary>The purchase will be delivered by the seller.</summary>
		Delivery,
		/// <summary>The product will be picked up at the seller's place of business.</summary>
		StorePickup,
		/// <summary>The product will be picked up by the delivery service.</summary>
		ServicePickup,
	}

	/// <summary>Enumerates error codes relating to PassKit operations.</summary>
	[MacCatalyst (13, 1)]
	[Native]
	public enum PKAddPaymentPassError : long {
		/// <summary>The operation is not supported on this device.</summary>
		Unsupported,
		/// <summary>The operation was cancelled by the user.</summary>
		UserCancelled,
		/// <summary>The system cancelled the operation.</summary>
		SystemCancelled,
	}

	/// <summary>Enumerates results that are used in calls to <see cref="M:PassKit.PKPassLibrary.RequestAutomaticPassPresentationSuppression(System.Action{PassKit.PKAutomaticPassPresentationSuppressionResult})" />.</summary>
	[Mac (11, 0)]
	[MacCatalyst (13, 1)]
	[Native]
	public enum PKAutomaticPassPresentationSuppressionResult : ulong {
		/// <summary>Suppression is not supported on this device.</summary>
		NotSupported = 0,
		/// <summary>System is already presenting a pass and cannot be suppressed.</summary>
		AlreadyPresenting,
		/// <summary>The user denied the ability to suppress presentation.</summary>
		Denied,
		/// <summary>The suppression was cancelled.</summary>
		Cancelled,
		/// <summary>The suppression of pass presentation succeeded.</summary>
		Success,
	}

	/// <summary>Enumerates the types of cards available to Apple Pay.</summary>
	[MacCatalyst (13, 1)]
	[Native]
	public enum PKPaymentMethodType : ulong {
		/// <summary>The type of card is unknown.</summary>
		Unknown = 0,
		/// <summary>A debit card.</summary>
		Debit,
		/// <summary>A credit card.</summary>
		Credit,
		/// <summary>A prepaid card.</summary>
		Prepaid,
		/// <summary>A store card.</summary>
		Store,
		EMoney,
	}

	/// <summary>Enumerates whether a payment associated with a <see cref="T:PassKit.PKShippingMethod" /> is pending or final.</summary>
	[MacCatalyst (13, 1)]
	[Native]
	public enum PKPaymentSummaryItemType : ulong {
		/// <summary>The payment is final.</summary>
		Final,
		/// <summary>The payment is pending.</summary>
		Pending,
	}

	/// <summary>Enumerates Pass Button styles.</summary>
	[NoMac] // under `#if TARGET_OS_IOS`
	[MacCatalyst (13, 1)]
	[Native]
	public enum PKAddPassButtonStyle : long {
		/// <summary>To be added.</summary>
		Black = 0,
		/// <summary>To be added.</summary>
		Outline,
	}

	/// <summary>Enumerates error conditions for payment operations.</summary>
	[MacCatalyst (13, 1)]
	[ErrorDomain ("PKPaymentErrorDomain")]
	[Native]
	public enum PKPaymentErrorCode : long {
		/// <summary>Indicates that an unknown error occurred.</summary>
		Unknown = -1,
		/// <summary>Indicates that the shipping contact was not valid.</summary>
		ShippingContactInvalid = 1,
		/// <summary>Indicates that the billing contact was not valid.</summary>
		BillingContactInvalid,
		/// <summary>Indicates that the shipping address was not serviceable.</summary>
		ShippingAddressUnserviceable,
		CouponCodeInvalid,
		CouponCodeExpired,
	}

	[MacCatalyst (13, 1)]
	[Native]
	public enum PKAddPaymentPassStyle : ulong {
		/// <summary>To be added.</summary>
		Payment,
		/// <summary>To be added.</summary>
		Access,
	}

	[iOS (13, 4)]
	[MacCatalyst (13, 1)]
	[ErrorDomain ("PKAddSecureElementPassErrorDomain")]
	[Native]
	public enum PKAddSecureElementPassErrorCode : long {
		GenericError = 0,
#if !XAMCORE_5_0
		[Deprecated (PlatformName.iOS, 18, 0, message: "Use 'GenericError' instead.")]
		[Deprecated (PlatformName.MacCatalyst, 18, 0, message: "Use 'GenericError' instead.")]
		[Deprecated (PlatformName.TvOS, 18, 0, message: "Use 'GenericError' instead.")]
		[Deprecated (PlatformName.MacOSX, 15, 0, message: "Use 'GenericError' instead.")]
		UnknownError = GenericError,
#endif
		UserCanceledError,
		UnavailableError,
		InvalidConfigurationError,
		DeviceNotSupportedError,
		DeviceNotReadyError,
		OSVersionNotSupportedError,
	}

	[NoTV, NoMac, iOS (16, 0), MacCatalyst (16, 0)]
	[Native]
	public enum PKIdentityButtonLabel : long {
		VerifyIdentity = 0,
		Verify,
		VerifyAge,
		Continue,
	}

	[NoTV, NoMac, iOS (16, 0), MacCatalyst (16, 0)]
	[Native]
	public enum PKIdentityButtonStyle : long {
		Black = 0,
		Outline,
	}

	[NoTV, NoMac, iOS (16, 0), MacCatalyst (16, 0)]
	[Native]
	[ErrorDomain ("PKIdentityErrorDomain")]
	public enum PKIdentityError : long {
		Unknown = 0,
		NotSupported = 1,
		Cancelled = 2,
		NetworkUnavailable = 3,
		NoElementsRequested = 4,
		RequestAlreadyInProgress = 5,
		InvalidNonce = 6,
		InvalidElement = 7,
		RegionNotSupported = 8,
	}

	[iOS (16, 0), Mac (13, 0), NoTV, MacCatalyst (16, 0)]
	[Native]
	[ErrorDomain ("PKShareSecureElementPassErrorDomain")]
	public enum PKShareSecureElementPassErrorCode : long {
		UnknownError,
		SetupError,
	}

	[iOS (16, 0), MacCatalyst (16, 0), NoTV, NoMac]
	[Native]
	public enum PKShareSecureElementPassResult : long {
		Canceled,
		Shared,
		Failed,
	}

	[iOS (16, 0), Mac (13, 0), NoTV, MacCatalyst (16, 0)]
	[Native]
	public enum PKVehicleConnectionErrorCode : long {
		Unknown = 0,
		SessionUnableToStart,
		SessionNotActive,
	}

	[iOS (16, 0), Mac (13, 0), NoTV, MacCatalyst (16, 0)]
	[Native]
	public enum PKVehicleConnectionSessionConnectionState : long {
		Disconnected = 0,
		Connected,
		Connecting,
		FailedToConnect,
	}

	[iOS (17, 0), Mac (14, 0), TV (17, 0), MacCatalyst (17, 0)]
	[Native]
	public enum PKApplePayLaterAvailability : long {
		Available,
		UnavailableItemIneligible,
		UnavailableRecurringTransaction,
	}

	[NoTV, Mac (15, 0), iOS (17, 0), MacCatalyst (17, 0)]
	[Native]
	[ErrorDomain ("PKDisbursementErrorDomain")]
	public enum PKDisbursementErrorCode : long {
		UnknownError = -1,
		UnsupportedCardError = 1,
		RecipientContactInvalidError,
	}

	[NoTV, NoMac, iOS (17, 0), NoMacCatalyst]
	[Native]
	public enum PKPayLaterAction : long {
		LearnMore = 0,
		Calculator,
	}

	[NoTV, NoMac, iOS (17, 0), NoMacCatalyst]
	[Native]
	public enum PKPayLaterDisplayStyle : long {
		Standard = 0,
		Badge,
		Checkout,
		Price,
	}

	[Static]
	[Internal]
	[TV (18, 0), Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
	interface PKMerchantCategoryCodeValues {
		[Field ("PKMerchantCategoryCodeNone")]
		short None { get; }
	}
}
