using Foundation;
using ObjCRuntime;

// @interface DisableDefaultCtorTest
[DisableDefaultCtor]
interface DisableDefaultCtorTest {
	// -(instancetype)initWithString:(NSString * _Nonnull)str;
	[Export ("initWithString:")]
	NativeHandle Constructor (string str);
}
