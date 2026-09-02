using Foundation;

// @protocol ProtocolA
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
interface ProtocolA {
}

// @protocol ProtocolB
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
interface ProtocolB {
}

// @protocol ProtocolC <ProtocolB>
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
interface ProtocolC : IProtocolB {
}

// @interface Impl_A <ProtocolA>
interface Impl_A : IProtocolA {
}

// @interface Impl_A_C <ProtocolA, ProtocolC>
interface Impl_A_C : IProtocolA, IProtocolC {
}

// @interface Root
interface Root {
}

// @interface Sub : Root <ProtocolA, ProtocolB>
[BaseType (typeof (Root))]
interface Sub : IProtocolA, IProtocolB {
}

// @interface Usage
interface Usage {
	// -(Root<ProtocolA,ProtocolB> *)root_A_C;
	[Export ("root_A_C")]
	[Verify (MethodToProperty)]
	Root<ProtocolA, ProtocolB> Root_A_C { get; }

	// -(Root<ProtocolA> *)root_A;
	[Export ("root_A")]
	[Verify (MethodToProperty)]
	ProtocolA Root_A { get; }

	// -(id<ProtocolA>)proto_A;
	[Export ("proto_A")]
	[Verify (MethodToProperty)]
	ProtocolA Proto_A { get; }
}

// @protocol RequiredAndOptional
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
interface RequiredAndOptional {
	// @required -(void)implicitRequiredMethod;
	[Abstract]
	[Export ("implicitRequiredMethod")]
	void ImplicitRequiredMethod ();

	// @required @property int implicitRequiredProperty;
	[Abstract]
	[Export ("implicitRequiredProperty")]
	int ImplicitRequiredProperty { get; set; }

	// @optional -(void)firstOptionalMethod;
	[Export ("firstOptionalMethod")]
	void FirstOptionalMethod ();

	// @optional @property int firstOptionalProperty;
	[Export ("firstOptionalProperty")]
	int FirstOptionalProperty { get; set; }

	// @required -(void)explicitRequiredMethod;
	[Abstract]
	[Export ("explicitRequiredMethod")]
	void ExplicitRequiredMethod ();

	// @required @property int explicitRequiredProperty;
	[Abstract]
	[Export ("explicitRequiredProperty")]
	int ExplicitRequiredProperty { get; set; }

	// @optional -(void)secondOptionalMethod;
	[Export ("secondOptionalMethod")]
	void SecondOptionalMethod ();

	// @optional @property int secondOptionalProperty;
	[Export ("secondOptionalProperty")]
	int SecondOptionalProperty { get; set; }
}
