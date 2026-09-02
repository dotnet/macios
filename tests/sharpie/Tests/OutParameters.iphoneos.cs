using Foundation;

// @interface OutParams
interface OutParams {
	// -(void)foo:(id)foo withError:(NSError **)error;
	[Export ("foo:withError:")]
	void Foo (NSObject foo, out NSError error);

	// -(void)bar:(NSNumber *)num withOutObject:(id *)obj;
	[Export ("bar:withOutObject:")]
	void Bar (NSNumber num, out NSObject obj);
}
