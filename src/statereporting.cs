using Foundation;
using ObjCRuntime;

namespace StateReporting {

	[TV (27, 0), Mac (27, 0), iOS (27, 0), MacCatalyst (27, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface SRStateReporter {

		[Export ("domain")]
		string Domain { get; }

		[Export ("reportTransitionToStateLabel:stableMetadata:volatileMetadata:")]
		void ReportTransition ([NullAllowed] string stateLabel, [NullAllowed] NSDictionary<NSString, NSObject> stableMetadata, [NullAllowed] NSDictionary<NSString, NSObject> volatileMetadata);

		[Export ("reportVolatileMetadataUpdate:")]
		void ReportVolatileMetadataUpdate ([NullAllowed] NSDictionary<NSString, NSObject> updatedMetadata);

		[Static]
		[Export ("reporterForDomain:")]
		SRStateReporter FromDomain (string domain);
	}
}
