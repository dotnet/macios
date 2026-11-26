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
		/// <summary>The payload URL isn't allowed.</summary>
		Disallowed = 1,
		/// <summary>The payload URL doesn't match the specified region.</summary>
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
