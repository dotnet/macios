using Foundation;

// @interface Root
interface Root {
}

// @interface Sub : Root
[BaseType (typeof (Root))]
interface Sub {
}

// @interface Members
interface Members {
	// -(void)instanceMethod;
	[Export ("instanceMethod")]
	void InstanceMethod ();

	// -(int)instanceMethod:(int *)takingIntPtr andOutSub:(Sub **)sub;
	[Export ("instanceMethod:andOutSub:")]
	unsafe int InstanceMethod (int* takingIntPtr, out Sub sub);

	// +(void)staticMethod;
	[Static]
	[Export ("staticMethod")]
	void StaticMethod ();

	// @property int intProperty;
	[Export ("intProperty")]
	int IntProperty { get; set; }

	// @property (readonly) int intPropertyReadonly;
	[Export ("intPropertyReadonly")]
	int IntPropertyReadonly { get; }

	// +(int)staticCsharpProperty;
	[Static]
	[Export ("staticCsharpProperty")]
	[Verify (MethodToProperty)]
	int StaticCsharpProperty { get; }

	// +(void)staticCsharpProperty:(int)value;
	[Static]
	[Export ("staticCsharpProperty:")]
	void StaticCsharpProperty (int value);
}

// @interface Fields
interface Fields {
	// -(int)getIntField;
	[Export ("getIntField")]
	[Verify (MethodToProperty)]
	int IntField { get; }

	// -(void)setIntField:(int)value;
	[Export ("setIntField:")]
	void SetIntField (int value);
}
