using Foundation;
using ObjCRuntime;

// @interface UnavailableWatchOS
[NoWatch]
interface UnavailableWatchOS {
}

// @interface UnavailableTvOS
[NoTV]
interface UnavailableTvOS {
}

// @interface iOSAndMacOnly
[NoWatch, NoTV, Mac (10, 11), iOS (9, 1)]
interface iOSAndMacOnly {
}

// @interface Availability
[Mac (10, 8), iOS (7, 0)]
interface Availability {
	// -(void)thisIsDeprecated __attribute__((availability(macos, deprecated=10.10.3))) __attribute__((availability(ios, deprecated=8.3)));
	[Deprecated (PlatformName.MacOSX, 10, 10, 3, message: "Don't use this in Mac homie")]
	[Deprecated (PlatformName.iOS, 8, 3, message: "Don't use this in iOS homie")]
	[Export ("thisIsDeprecated")]
	void ThisIsDeprecated ();

	// -(void)thisWasIntroducedThenDeprecatedThenObsoletedMacOnly __attribute__((availability(macos, introduced=10.8.1))) __attribute__((availability(macos, deprecated=10.9))) __attribute__((availability(macos, obsoleted=10.10)));
	[Introduced (PlatformName.MacOSX, 10, 8, 1)]
	[Deprecated (PlatformName.MacOSX, 10, 9, message: "Deprecated Message")]
	[Obsoleted (PlatformName.MacOSX, 10, 10, message: "Obsoleted Message")]
	[Export ("thisWasIntroducedThenDeprecatedThenObsoletedMacOnly")]
	void ThisWasIntroducedThenDeprecatedThenObsoletedMacOnly ();

	// -(void)thisWasIntroducedThenDeprecatedThenObsoletedInBothMacAndIos __attribute__((availability(macos, introduced=10.8.1))) __attribute__((availability(macos, deprecated=10.9))) __attribute__((availability(macos, obsoleted=10.10))) __attribute__((availability(ios, introduced=7.0))) __attribute__((availability(ios, deprecated=7.1))) __attribute__((availability(ios, obsoleted=8.0)));
	[Introduced (PlatformName.MacOSX, 10, 8, 1)]
	[Deprecated (PlatformName.MacOSX, 10, 9, message: "Mac Deprecated Message")]
	[Obsoleted (PlatformName.MacOSX, 10, 10, message: "Mac Obsoleted Message")]
	[Introduced (PlatformName.iOS, 7, 0)]
	[Deprecated (PlatformName.iOS, 7, 1, message: "iOS Deprecated Message")]
	[Obsoleted (PlatformName.iOS, 8, 0, message: "iOS Obsoleted Message")]
	[Export ("thisWasIntroducedThenDeprecatedThenObsoletedInBothMacAndIos")]
	void ThisWasIntroducedThenDeprecatedThenObsoletedInBothMacAndIos ();

	// -(void)thisShouldBeShorthandMac __attribute__((availability(macos, introduced=10.10.3)));
	[Mac (10, 10, 3)]
	[Export ("thisShouldBeShorthandMac")]
	void ThisShouldBeShorthandMac ();

	// -(void)thisShouldBeShorthandMacOnlyOn64 __attribute__((availability(macos, introduced=10.13.4)));
	[Mac (10, 13, 4)]
	[Export ("thisShouldBeShorthandMacOnlyOn64")]
	void ThisShouldBeShorthandMacOnlyOn64 ();

	// -(void)thisShouldBeShorthandIos __attribute__((availability(ios, introduced=8.3.1)));
	[iOS (8, 3, 1)]
	[Export ("thisShouldBeShorthandIos")]
	void ThisShouldBeShorthandIos ();

	// -(void)thisIsUnavailableOnMac __attribute__((availability(macos, unavailable)));
	[NoMac]
	[Export ("thisIsUnavailableOnMac")]
	void ThisIsUnavailableOnMac ();

	// -(void)thisIsUnavailableOnMacAndIos __attribute__((availability(macos, unavailable))) __attribute__((availability(ios, unavailable)));
	[NoMac, NoiOS]
	[Export ("thisIsUnavailableOnMacAndIos")]
	void ThisIsUnavailableOnMacAndIos ();

	// -(void)introducedOnMac64BitOnlyAndLaterDeprecated __attribute__((availability(macos, introduced=10.13.4))) __attribute__((availability(macos, deprecated=10.13.5)));
	[Deprecated (PlatformName.MacOSX, 10, 13, 5)]
	[Mac (10, 13, 4)]
	[Export ("introducedOnMac64BitOnlyAndLaterDeprecated")]
	void IntroducedOnMac64BitOnlyAndLaterDeprecated ();
}
