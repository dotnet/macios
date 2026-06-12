//
// SafetyKit C# bindings
//
// Authors:
//	Israel Soto <issoto@microsoft.com>
//	Rolf Bjarne Kvinge <rolf@xamarin.com>
//
// Copyright 2022, 2024 Microsoft Corporation.
//

using CoreLocation;

namespace SafetyKit {

	[iOS (16, 0), MacCatalyst (16, 0), NoTV]
	// SafetyKit (Crash Detection) is device-only; the framework was only added to the iOS simulator later.
	[SupportedSimulator ("ios16.4")]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface SACrashDetectionEvent : NSSecureCoding, NSCopying {
		[Export ("date")]
		NSDate Date { get; }

		[Export ("response")]
		SACrashDetectionEventResponse Response { get; }

		[NullAllowed, Export ("location")]
		CLLocation Location { get; }
	}

	delegate void SACrashDetectionManagerRequestAuthorizationCompletionHandler (SAAuthorizationStatus status, [NullAllowed] NSError error);

	[iOS (16, 0), MacCatalyst (16, 0), NoTV]
	[SupportedSimulator ("ios16.4")]
	[BaseType (typeof (NSObject))]
	interface SACrashDetectionManager {
		[Static]
		[Export ("available")]
		bool Available { [Bind ("isAvailable")] get; }

		[Export ("authorizationStatus")]
		SAAuthorizationStatus AuthorizationStatus { get; }

		[Wrap ("WeakDelegate")]
		[NullAllowed]
		ISACrashDetectionDelegate Delegate { get; set; }

		[NullAllowed, Export ("delegate", ArgumentSemantic.Weak)]
		NSObject WeakDelegate { get; set; }

		[Async]
		[Export ("requestAuthorizationWithCompletionHandler:")]
		void RequestAuthorization (SACrashDetectionManagerRequestAuthorizationCompletionHandler handler);
	}

	interface ISACrashDetectionDelegate { }

	[iOS (16, 0), MacCatalyst (16, 0), NoTV]
	[Protocol, Model]
	[BaseType (typeof (NSObject))]
	interface SACrashDetectionDelegate {
		[Export ("crashDetectionManager:didDetectEvent:")]
		void DidDetectEvent (SACrashDetectionManager crashDetectionManager, SACrashDetectionEvent @event);
	}

	delegate void SAEmergencyResponseManagerDialVoiceCallCompletionHandler (bool requestAccepted, [NullAllowed] NSError error);

	[iOS (16, 0), MacCatalyst (16, 0), NoTV]
	[SupportedSimulator ("ios16.4")]
	[BaseType (typeof (NSObject))]
	interface SAEmergencyResponseManager {
		[Wrap ("WeakDelegate")]
		[NullAllowed]
		ISAEmergencyResponseDelegate Delegate { get; set; }

		[NullAllowed, Export ("delegate", ArgumentSemantic.Weak)]
		NSObject WeakDelegate { get; set; }

		[Async]
		[Export ("dialVoiceCallToPhoneNumber:completionHandler:")]
		void DialVoiceCall (string phoneNumber, SAEmergencyResponseManagerDialVoiceCallCompletionHandler handler);
	}

	interface ISAEmergencyResponseDelegate { }

	[iOS (16, 0), MacCatalyst (16, 0), NoTV]
	[Protocol, Model]
	[BaseType (typeof (NSObject))]
	interface SAEmergencyResponseDelegate {
		[Export ("emergencyResponseManager:didUpdateVoiceCallStatus:")]
		void DidUpdateVoiceCallStatus (SAEmergencyResponseManager emergencyResponseManager, SAEmergencyResponseManagerVoiceCallStatus voiceCallStatus);
	}
}
