using Foundation;
using ObjCRuntime;

// @interface UnavailableCtors
[DisableDefaultCtor]
interface UnavailableCtors {
	// -(instancetype)initWithBar:(int)bar __attribute__((objc_designated_initializer));
	[Export ("initWithBar:")]
	[DesignatedInitializer]
	NativeHandle Constructor (int bar);
}
