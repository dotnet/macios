#nullable enable

using System.ComponentModel;
using CoreLocation;

namespace AppClip {

	/// <summary>Enumerates the errors that can occur when working with an App Clip activation payload.</summary>
	[iOS (14, 0)]
	[MacCatalyst (14, 0)]
	[ErrorDomain ("APActivationPayloadErrorDomain")]
	[Native]
	public enum APActivationPayloadErrorCode : long {
		/// <summary>The user denied location access, or the App Clip wasn't launched from an NFC tag or a visual code.</summary>
		Disallowed = 1,
		/// <summary>The URL doesn't match the registered App Clip URL.</summary>
		DoesNotMatch = 2,
	}

	[iOS (14, 0)]
	[MacCatalyst (14, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface APActivationPayload : NSSecureCoding, NSCopying {

		[NullAllowed, Export ("URL", ArgumentSemantic.Strong)]
		NSUrl Url { get; }

		[Async]
		[Export ("confirmAcquiredInRegion:completionHandler:")]
		void ConfirmAcquired (CLRegion region, Action<bool, NSError?> completionHandler);
	}
}
