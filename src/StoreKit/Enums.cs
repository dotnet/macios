using Foundation;
using ObjCRuntime;
using System;

namespace StoreKit {

	// typedef NSInteger SKPaymentTransactionState;
	// StoreKit.framework/Headers/SKPaymentTransaction.h
	/// <summary>Transaction state.</summary>
	[Deprecated (PlatformName.iOS, 18, 0 /* Apple's replacement requires Swift */ )]
	[Deprecated (PlatformName.MacCatalyst, 18, 0 /* Apple's replacement requires Swift */ )]
	[Deprecated (PlatformName.MacOSX, 15, 0 /* Apple's replacement requires Swift */ )]
	[Deprecated (PlatformName.TvOS, 18, 0 /* Apple's replacement requires Swift */ )]
	[MacCatalyst (13, 1)]
	[Native]
	public enum SKPaymentTransactionState : long {
		/// <summary>Transaction is being processed by the AppStore.</summary>
		Purchasing,
		/// <summary>The item has been purchased, the application can give the user access to the content.</summary>
		Purchased,
		/// <summary>The transaction failed, check the Error property of the SKPaymentTransaction for actual details.</summary>
		Failed,
		/// <summary>The transaction has restored the original content purchased by the user.</summary>
		Restored,
		/// <summary>To be added.</summary>
		[MacCatalyst (13, 1)]
		Deferred,
	}

	// untyped enum and not used in API - so it _could_ be an `int`
	// OTOH it's meant to be used with NSError.Code which is an NSInteger/nint
	// StoreKit.framework/Headers/SKError.h
	/// <summary>An enumeration whose values specify various errors relating to StoreKit.</summary>
	[Native ("SKErrorCode")]
	[ErrorDomain ("SKErrorDomain")]
	public enum SKError : long {
		/// <summary>Indicates an unknown error.</summary>
		Unknown,
		/// <summary>Indicates that the client may not perform the operation.</summary>
		ClientInvalid,
		/// <summary>Indicates that a payment was cancelled.</summary>
		PaymentCancelled,
		/// <summary>Indicates that a payment was invalid.</summary>
		PaymentInvalid,
		/// <summary>Indicates that a payment was not allowed.</summary>
		PaymentNotAllowed,
		/// <summary>Indicates that a product was unavailable.</summary>
		ProductNotAvailable,
		// iOS 9.3
		/// <summary>Indicates that the cloud service denied permission for the operation.</summary>
		CloudServicePermissionDenied,
		/// <summary>Indicates that the cloud service network connection failed.</summary>
		CloudServiceNetworkConnectionFailed,
		// iOS 10.3
		/// <summary>To be added.</summary>
		CloudServiceRevoked,
#if !NET
		[Obsolete ("Use 'SKError.CloudServiceRevoked' instead.")]
		Revoked = CloudServiceRevoked,
#endif
		// iOS 12.2
		/// <summary>To be added.</summary>
		PrivacyAcknowledgementRequired,
		/// <summary>To be added.</summary>
		UnauthorizedRequestData,
		/// <summary>To be added.</summary>
		InvalidOfferIdentifier,
		/// <summary>To be added.</summary>
		InvalidSignature,
		/// <summary>To be added.</summary>
		MissingOfferParams,
		/// <summary>To be added.</summary>
		InvalidOfferPrice,
		OverlayCancelled = 15,

		// iOS 14
		OverlayInvalidConfiguration = 16,
		OverlayTimeout = 17,
		IneligibleForOffer = 18,
		UnsupportedPlatform = 19,
		// iOS 14.5
		OverlayPresentedInBackgroundScene = 20,
	}

	// typedef NSInteger SKDownloadState;
	// StoreKit.framework/Headers/SKDownload.h
	/// <summary>An enumeration whose values specify the state of an <see cref="T:StoreKit.SKDownload" /> object. Used with the <see cref="P:StoreKit.SKDownload.DownloadState" /> property.</summary>
	[MacCatalyst (13, 1)]
	[Native]
	public enum SKDownloadState : long {
		/// <summary>To be added.</summary>
		Waiting,
		/// <summary>To be added.</summary>
		Active,
		/// <summary>To be added.</summary>
		Paused,
		/// <summary>To be added.</summary>
		Finished,
		/// <summary>To be added.</summary>
		Failed,
		/// <summary>To be added.</summary>
		Cancelled,
	}

	/// <summary>Enumerates authorization states for music store access.</summary>
	[Deprecated (PlatformName.iOS, 18, 0 /* Apple's replacement requires Swift */ )]
	[Deprecated (PlatformName.MacCatalyst, 18, 0 /* Apple's replacement requires Swift */ )]
	[Deprecated (PlatformName.MacOSX, 15, 0 /* Apple's replacement requires Swift */ )]
	[Deprecated (PlatformName.TvOS, 18, 0 /* Apple's replacement requires Swift */ )]
	[MacCatalyst (13, 1)]
	[Native]
	public enum SKCloudServiceAuthorizationStatus : long {
		/// <summary>Indicates that the authorization state cannot be determined.</summary>
		NotDetermined,
		/// <summary>Indicates that the user has denied access to their music library.</summary>
		Denied,
		/// <summary>Indicates that access to the user's music library is restricted and that the user cannot change the authorization status.</summary>
		Restricted,
		/// <summary>Indicates that authorization has been given to play Apple Music tracks and to add tracks to the user's music library.</summary>
		Authorized,
	}

	/// <summary>Enumerates flags that describe the user's local music library capabilities..</summary>
	[Deprecated (PlatformName.iOS, 18, 0 /* Apple's replacement requires Swift */ )]
	[Deprecated (PlatformName.MacCatalyst, 18, 0 /* Apple's replacement requires Swift */ )]
	[Deprecated (PlatformName.MacOSX, 15, 0 /* Apple's replacement requires Swift */ )]
	[Deprecated (PlatformName.TvOS, 18, 0 /* Apple's replacement requires Swift */ )]
	[MacCatalyst (13, 1)]
	[Native]
	public enum SKCloudServiceCapability : ulong {
		/// <summary>The device has no music library capabilities.</summary>
		None = 0,
		/// <summary>Indicates that the device can play tracks in the Apple Music catalog.</summary>
		MusicCatalogPlayback = 1 << 0,
		/// <summary>A constant that indicates that a device allows subscriptions to the Apple Music catalog.</summary>
		[MacCatalyst (13, 1)]
		MusicCatalogSubscriptionEligible = 1 << 1,
		/// <summary>Indicates that the device can add tracks to the user's cloud library.</summary>
		AddToCloudMusicLibrary = 1 << 8,
	}

	/// <summary>Enumerates visiblity settings for a promoted product.</summary>
	[Deprecated (PlatformName.iOS, 18, 0 /* Apple's replacement requires Swift */ )]
	[Deprecated (PlatformName.MacCatalyst, 18, 0 /* Apple's replacement requires Swift */ )]
	[Deprecated (PlatformName.MacOSX, 15, 0 /* Apple's replacement requires Swift */ )]
	[Deprecated (PlatformName.TvOS, 18, 0 /* Apple's replacement requires Swift */ )]
	[MacCatalyst (13, 1)]
	[Native]
	public enum SKProductStorePromotionVisibility : long {
		/// <summary>Specifies the default value that is in iTunes Connect.</summary>
		Default,
		/// <summary>Show the product.</summary>
		Show,
		/// <summary>Hide the product.</summary>
		Hide,
	}

	/// <summary>Defines the natural period for a subscription.</summary>
	[Deprecated (PlatformName.iOS, 18, 0 /* Apple's replacement requires Swift */ )]
	[Deprecated (PlatformName.MacCatalyst, 18, 0 /* Apple's replacement requires Swift */ )]
	[Deprecated (PlatformName.MacOSX, 15, 0 /* Apple's replacement requires Swift */ )]
	[Deprecated (PlatformName.TvOS, 18, 0 /* Apple's replacement requires Swift */ )]
	[MacCatalyst (13, 1)]
	[Native]
	public enum SKProductPeriodUnit : ulong {
		/// <summary>The subscription is defined in terms of days.</summary>
		Day,
		/// <summary>The subscription is defined in terms of weeks.</summary>
		Week,
		/// <summary>The subscription is defined in terms of months.</summary>
		Month,
		/// <summary>The subscription is defined in terms of a year or years.</summary>
		Year,
	}

	/// <summary>Enumerates how a discount price is applied.</summary>
	[Deprecated (PlatformName.iOS, 18, 0 /* Apple's replacement requires Swift */ )]
	[Deprecated (PlatformName.MacCatalyst, 18, 0 /* Apple's replacement requires Swift */ )]
	[Deprecated (PlatformName.MacOSX, 15, 0 /* Apple's replacement requires Swift */ )]
	[Deprecated (PlatformName.TvOS, 18, 0 /* Apple's replacement requires Swift */ )]
	[MacCatalyst (13, 1)]
	[Native]
	public enum SKProductDiscountPaymentMode : ulong {
		/// <summary>The discount is billed to the user.</summary>
		PayAsYouGo,
		/// <summary>The discount is applied to an up-front payment.</summary>
		PayUpFront,
		/// <summary>The discount price is a free trial.</summary>
		FreeTrial,
	}

	[NoTV, NoMac, iOS (14, 0)]
	[MacCatalyst (14, 0)]
	[Native]
	public enum SKOverlayPosition : long {
		SKOverlayPositionBottom = 0,
		Raised = 1,
	}

	[NoMac, iOS (16, 0), MacCatalyst (16, 0), NoTV]
	public enum SKAdNetworkCoarseConversionValue {
		[Field ("SKAdNetworkCoarseConversionValueHigh")]
		High,
		[Field ("SKAdNetworkCoarseConversionValueMedium")]
		Medium,
		[Field ("SKAdNetworkCoarseConversionValueLow")]
		Low,
	}
}
