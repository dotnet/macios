using System;
using System.Diagnostics.CodeAnalysis;
using Foundation;
using ObjCRuntime;

namespace NS {
	[BaseType (typeof (NSObject))]
	interface MyClass {
		[Static, Export ("currentContext"), NullAllowed]
		NSObject CurrentContext {
			// (DynamicallyAccessedMemberTypes, string, string)
			[DynamicDependency (DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors, "Foundation.NSProxy", "Microsoft.macOS")]
			get;
			set;
		}

		// (string, string, string)
		[Export ("doSomething")]
		[DynamicDependency ("Create", "NS.MyClass", "api0")]
		void DoSomething ();

		// (string) - single member signature
		[Export ("doSomethingElse")]
		[DynamicDependency ("Activate")]
		void DoSomethingElse ();

		// (DynamicallyAccessedMemberTypes, Type)
		[Export ("doAnother")]
		[DynamicDependency (DynamicallyAccessedMemberTypes.PublicMethods | DynamicallyAccessedMemberTypes.PublicProperties, typeof (NSObject))]
		void DoAnother ();

		// (string, Type)
		[Export ("doYetAnother")]
		[DynamicDependency ("Create", typeof (NSObject))]
		void DoYetAnother ();

		// (DynamicallyAccessedMemberTypes.All, string, string)
		[Export ("doAll")]
		[DynamicDependency (DynamicallyAccessedMemberTypes.All, "NS.MyClass", "api0")]
		void DoAll ();
	}
}
