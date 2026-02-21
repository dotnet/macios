using Foundation;

// @interface RequiresSuperTest
interface RequiresSuperTest {
	// -(void)foo __attribute__((objc_requires_super));
	[Export ("foo")]
	[RequiresSuper]
	void Foo ();
}
