using System.Runtime.InteropServices;
using Foundation;
using ObjCRuntime;

static class CFunctions {
	// extern void Func (const char * _Nullable str);
	[DllImport ("__Internal")]
	[Verify (PlatformInvoke)]
	static extern unsafe void Func ([NullAllowed] sbyte* str);
}

// @interface Foo
interface Foo {
	// @property SEL _Nullable selector;
	[NullAllowed, Export ("selector", ArgumentSemantic.Assign)]
	Selector Selector { get; set; }

	// @property (readonly) id _Nullable someObject;
	[NullAllowed, Export ("someObject")]
	NSObject SomeObject { get; }

	// -(id _Nullable)nullableReturnPointer;
	[NullAllowed, Export ("nullableReturnPointer")]
	[Verify (MethodToProperty)]
	NSObject NullableReturnPointer { get; }

	// -(id _Nullable)nullableReturnPointer:(int)arg withNullable:(id _Nullable)obj;
	[Export ("nullableReturnPointer:withNullable:")]
	[return: NullAllowed]
	NSObject NullableReturnPointer (int arg, [NullAllowed] NSObject obj);
}
