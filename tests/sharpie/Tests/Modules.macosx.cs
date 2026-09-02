using Foundation;

// @interface HelloModules
interface HelloModules {
	// -(NSString *)hello;
	[Export ("hello")]
	[Verify (MethodToProperty)]
	string Hello { get; }
}
