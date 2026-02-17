using System.Runtime.InteropServices;
using Foundation;

// @interface NSArrayToArrayOfNSObjectTest
interface NSArrayToArrayOfNSObjectTest {
	// -(NSArray *)getNSArray;
	[Export ("getNSArray")]
	[Verify (MethodToProperty), Verify (StronglyTypedNSArray)]
	NSObject [] NSArray { get; }
}

// @interface NSArrayOfNSStringToArrayOfStringTest
interface NSArrayOfNSStringToArrayOfStringTest {
	// -(NSArray<NSString *> *)getStrings;
	[Export ("getStrings")]
	[Verify (MethodToProperty)]
	string [] Strings { get; }
}

// @interface NSStringPropertyTest
interface NSStringPropertyTest {
	// @property (readonly, copy) NSString * someString;
	[Export ("someString")]
	string SomeString { get; }
}

static class CFunctions {
	// extern NSString * _Nullable CFunctionWithNSStringTest (NSString * _Nonnull , int c, NSString * _Nullable );
	[DllImport ("__Internal")]
	[Verify (PlatformInvoke)]
	[return: NullAllowed]
	static extern NSString CFunctionWithNSStringTest (NSString unnamedParameter0, int c, [NullAllowed] NSString unnamedParameter2);
}
