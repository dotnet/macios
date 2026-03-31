using Foundation;

// @interface NSArrayToArrayOfNSObjectTest
interface NSArrayToArrayOfNSObjectTest {
	// -(NSArray *)getNSArray;
	[Export ("getNSArray")]
	[Verify (MethodToProperty), Verify (StronglyTypedNSArray)]
	NSObject [] NSArray { get; }
}
