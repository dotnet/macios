// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

// RUN iphoneos: -x objective-c -sdk iphoneos
// RUN macosx: -x objective-c -sdk macosx

#if !__has_feature(attribute_availability_watchos)
#error "compiler does not support attribute_availability_watchos"
#endif

#if !__has_feature(attribute_availability_tvos)
#error "compiler does not support attribute_availability_tvos"
#endif

__attribute__((availability(watchos,unavailable)))
@interface UnavailableWatchOS
@end

__attribute__((availability(tvos,unavailable)))
@interface UnavailableTvOS
@end

__attribute__((availability(ios,introduced=9.1)))
__attribute__((availability(macosx,introduced=10.11)))
__attribute__((availability(watchos,unavailable)))
__attribute__((availability(tvos,unavailable)))
@interface iOSAndMacOnly
@end

__attribute__((availability(macosx,introduced=10.8)))
__attribute__((availability(ios,introduced=7.0)))
@interface Availability

-(void)thisIsDeprecated
	__attribute__((availability(macosx,deprecated=10.10.3,message="Don't use this in Mac homie")))
	__attribute__((availability(ios,deprecated=8.3,message="Don't use this in iOS homie")));

-(void)thisWasIntroducedThenDeprecatedThenObsoletedMacOnly
	__attribute__((availability(macosx,introduced=10.8.1,message="Introduced Message")))
	__attribute__((availability(macosx,deprecated=10.9,message="Deprecated Message")))
	__attribute__((availability(macosx,obsoleted=10.10,message="Obsoleted Message")));

-(void)thisWasIntroducedThenDeprecatedThenObsoletedInBothMacAndIos
	__attribute__((availability(macosx,introduced=10.8.1,message="Mac Introduced Message")))
	__attribute__((availability(macosx,deprecated=10.9,message="Mac Deprecated Message")))
	__attribute__((availability(macosx,obsoleted=10.10,message="Mac Obsoleted Message")))
	__attribute__((availability(ios,introduced=7.0,message="iOS Introduced Message")))
	__attribute__((availability(ios,deprecated=7.1,message="iOS Deprecated Message")))
	__attribute__((availability(ios,obsoleted=8.0,message="iOS Obsoleted Message")));

-(void)thisShouldBeShorthandMac
	__attribute__((availability(macosx,introduced=10.10.3)));

-(void)thisShouldBeShorthandMacOnlyOn64
	__attribute__((availability(macosx,introduced=10.13.4)));

-(void)thisShouldBeShorthandIos
	__attribute__((availability(ios,introduced=8.3.1)));

-(void)thisIsUnavailableOnMac
	__attribute__((availability(macosx,unavailable)));

-(void)thisIsUnavailableOnMacAndIos
	__attribute__((availability(macosx,unavailable)))
	__attribute__((availability(ios,unavailable)));

-(void)introducedOnMac64BitOnlyAndLaterDeprecated
	__attribute__((availability(macosx,introduced=10.13.4)))
	__attribute__((availability(macosx,deprecated=10.13.5)));

@end
