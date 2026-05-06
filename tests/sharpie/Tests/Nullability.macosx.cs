using System;
using System.Runtime.InteropServices;
using Foundation;
using ObjCRuntime;

static class CFunctions {
	// extern void Func (const char * _Nullable str);
	[DllImport ("__Internal")]
	[Verify (PlatformInvoke)]
	static extern unsafe void Func ([NullAllowed] sbyte* str);

	// extern NSObject * _Nullable NullableReturnObject ();
	[DllImport ("__Internal")]
	[Verify (PlatformInvoke)]
	[return: NullAllowed]
	static extern NSObject NullableReturnObject ();

	// extern NSString * _Nullable NullableReturnString (int code);
	[DllImport ("__Internal")]
	[Verify (PlatformInvoke)]
	[return: NullAllowed]
	static extern NSString NullableReturnString (int code);

	// extern void FuncWithNullableObject (NSObject * _Nullable obj);
	[DllImport ("__Internal")]
	[Verify (PlatformInvoke)]
	static extern void FuncWithNullableObject ([NullAllowed] NSObject obj);

	// extern void FuncWithNullableString (NSString * _Nullable str);
	[DllImport ("__Internal")]
	[Verify (PlatformInvoke)]
	static extern void FuncWithNullableString ([NullAllowed] NSString str);

	// extern void FuncWithMixedNullability (NSObject * _Nonnull required, NSObject * _Nullable optional);
	[DllImport ("__Internal")]
	[Verify (PlatformInvoke)]
	static extern void FuncWithMixedNullability (NSObject required, [NullAllowed] NSObject optional);

	// extern NSObject * _Nullable FuncNullableInAndOut (NSString * _Nullable input);
	[DllImport ("__Internal")]
	[Verify (PlatformInvoke)]
	[return: NullAllowed]
	static extern NSObject FuncNullableInAndOut ([NullAllowed] NSString input);

	// extern void FuncWithNullableBlock (void (^ _Nullable)(void) block);
	[DllImport ("__Internal")]
	[Verify (PlatformInvoke)]
	static extern void FuncWithNullableBlock ([NullAllowed] Action block);

	// extern void FuncWithNullableBlockParam (void (^ _Nullable)(NSObject * _Nullable) block);
	[DllImport ("__Internal")]
	[Verify (PlatformInvoke)]
	static extern void FuncWithNullableBlockParam ([NullAllowed] Action<NSObject> block);

	// extern void FuncWithNonnullBlock (void (^ _Nonnull)(void) block);
	[DllImport ("__Internal")]
	[Verify (PlatformInvoke)]
	static extern void FuncWithNonnullBlock (Action block);

	// extern SimpleBlock _Nullable FuncReturningNullableBlock ();
	[DllImport ("__Internal")]
	[Verify (PlatformInvoke)]
	[return: NullAllowed]
	static extern SimpleBlock FuncReturningNullableBlock ();
}

// typedef void (^SimpleBlock)();
delegate void SimpleBlock ();

// @interface Foo
interface Foo {
	// @property SEL _Nullable selector;
	[NullAllowed, Export ("selector", ArgumentSemantic.Assign)]
	Selector Selector { get; set; }

	// @property (readonly) id _Nullable someObject;
	[NullAllowed, Export ("someObject")]
	NSObject SomeObject { get; }

	// @property (readonly, nonatomic) __kindof NSObject * _Nullable presentedObject;
	[NullAllowed, Export ("presentedObject")]
	NSObject PresentedObject { get; }

	// -(id _Nullable)nullableReturnPointer;
	[NullAllowed, Export ("nullableReturnPointer")]
	[Verify (MethodToProperty)]
	NSObject NullableReturnPointer { get; }

	// -(id _Nullable)nullableReturnPointer:(int)arg withNullable:(id _Nullable)obj;
	[Export ("nullableReturnPointer:withNullable:")]
	[return: NullAllowed]
	NSObject NullableReturnPointer (int arg, [NullAllowed] NSObject obj);

	// -(__kindof NSObject * _Nullable)AnObject;
	[NullAllowed, Export ("AnObject")]
	[Verify (MethodToProperty)]
	NSObject AnObject { get; }

	// -(__kindof NSObject * _Nullable)createObjectWithObject:(__kindof NSObject * _Nullable)otherObject;
	[Export ("createObjectWithObject:")]
	[return: NullAllowed]
	NSObject CreateObjectWithObject ([NullAllowed] NSObject otherObject);
}
