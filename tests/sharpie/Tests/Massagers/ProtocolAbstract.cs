using Foundation;

// @protocol ProtocolAbstract
/*
  Check whether adding [Model] to this declaration is appropriate.
  [Model] is used to generate a C# class that implements this protocol,
  and might be useful for protocols that consumers are supposed to implement,
  since consumers can subclass the generated class instead of implementing
  the generated interface. If consumers are not supposed to implement this
  protocol, then [Model] is redundant and will generate code that will never
  be used.
*/
[Protocol]
interface ProtocolAbstract {
	// @required -(void)implicitRequired;
	[Abstract]
	[Export ("implicitRequired")]
	void ImplicitRequired ();

	// @optional -(void)explicitOptional;
	[Export ("explicitOptional")]
	void ExplicitOptional ();

	// @required -(void)explicitRequired;
	[Abstract]
	[Export ("explicitRequired")]
	void ExplicitRequired ();
}
