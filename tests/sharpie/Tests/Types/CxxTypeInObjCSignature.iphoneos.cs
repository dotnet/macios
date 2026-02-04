using Foundation;

// @interface ForceNSStringRMServicesAdditionsToLoad : NSObject
[BaseType (typeof (NSObject))]
interface ForceNSStringRMServicesAdditionsToLoad {
}

// @interface RMServicesAdditions (NSString)
[Category]
[BaseType (typeof (NSString))]
interface NSString_RMServicesAdditions {
	// +(NSString *)stringWithDPString:(const dp::String &)string;
	[Static]
	[Export ("stringWithDPString:")]
	string StringWithDPString (PlaceholderType /* Unsupported: CXType_LValueReference: const dp::String & */ @string);
}
