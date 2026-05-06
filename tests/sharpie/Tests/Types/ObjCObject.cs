using System.Runtime.InteropServices;
using Foundation;
using ObjCRuntime;

// @interface NSString
interface NSString {
}

static class CFunctions {
	// extern NSString * _NSString ();
	[DllImport ("__Internal")]
	[Verify (PlatformInvoke)]
	static extern NSString _NSString ();

	// extern NSString ** _Ptr_NSString ();
	[DllImport ("__Internal")]
	[Verify (PlatformInvoke)]
	static extern unsafe NSString* _Ptr_NSString ();

	// extern NSString *** _Ptr_Ptr_NSString ();
	[DllImport ("__Internal")]
	[Verify (PlatformInvoke)]
	static extern unsafe NSString** _Ptr_Ptr_NSString ();

	// extern id _Id ();
	[DllImport ("__Internal")]
	[Verify (PlatformInvoke)]
	static extern NSObject _Id ();

	// extern Class _Class ();
	[DllImport ("__Internal")]
	[Verify (PlatformInvoke)]
	static extern Class _Class ();

	// extern id<ProtoA> _Id_of_ProtoA ();
	[DllImport ("__Internal")]
	[Verify (PlatformInvoke)]
	static extern ProtoA _Id_of_ProtoA ();

	// extern Class<ProtoA> _Class_of_ProtoA ();
	[DllImport ("__Internal")]
	[Verify (PlatformInvoke)]
	static extern ProtoA _Class_of_ProtoA ();

	// extern SEL _SEL ();
	[DllImport ("__Internal")]
	[Verify (PlatformInvoke)]
	static extern Selector _SEL ();

	// extern SEL * _Ptr_SEL ();
	[DllImport ("__Internal")]
	[Verify (PlatformInvoke)]
	static extern unsafe Selector* _Ptr_SEL ();

	// extern SEL ** _Ptr_Ptr_SEL ();
	[DllImport ("__Internal")]
	[Verify (PlatformInvoke)]
	static extern unsafe Selector** _Ptr_Ptr_SEL ();

	// extern Foo<ProtoA,ProtoB> * _Foo_of_ProtoA_and_ProtoB ();
	[DllImport ("__Internal")]
	[Verify (PlatformInvoke)]
	static extern Foo<ProtoA, ProtoB> _Foo_of_ProtoA_and_ProtoB ();
}

// @protocol ProtoA
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
interface ProtoA {
}

// @protocol ProtoB
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
interface ProtoB {
}

// @interface Foo
interface Foo {
	// +(instancetype)create;
	[Static]
	[Export ("create")]
	Foo Create ();
}
