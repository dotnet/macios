using Foundation;
using ObjCRuntime;

// @interface NSDesignatedInitializerTest
[DisableDefaultCtor]
interface NSDesignatedInitializerTest {
	// -(instancetype)initWithInt:(int)value;
	[Export ("initWithInt:")]
	NativeHandle Constructor (int value);

	// -(instancetype)initWithString:(NSString * _Nonnull)value __attribute__((objc_designated_initializer));
	[Export ("initWithString:")]
	[DesignatedInitializer]
	NativeHandle Constructor (string value);
}

// @interface ObjDesignatedInitializerAttributeTest
[DisableDefaultCtor]
interface ObjDesignatedInitializerAttributeTest {
	// -(instancetype)initWithInt:(int)value;
	[Export ("initWithInt:")]
	NativeHandle Constructor (int value);

	// -(instancetype)initWithString:(NSString * _Nonnull)value __attribute__((objc_designated_initializer));
	[Export ("initWithString:")]
	[DesignatedInitializer]
	NativeHandle Constructor (string value);
}
