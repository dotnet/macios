using System;
using Foundation;

// @protocol Foo
/*
  Check whether adding [Model] to this declaration is appropriate.
  [Model] is used to generate a C# class that implements this protocol,
  and might be useful for protocols that consumers are supposed to implement,
  since consumers can subclass the generated class instead of implementing
  the generated interface. If consumers are not supposed to implement this
  protocol, then [Model] is redundant and will generate code that will never
  be used.
*/
[Mac (10, 9)]
[Protocol]
interface Foo {
	// @required +(id)availability_StaticAbstractInternal_Export:(int)n, ... __attribute__((availability(macos, introduced=10.10))) __attribute__((availability(ios, introduced=8.0)));
	[Mac (10, 10), iOS (8, 0)]
	[Static, Internal, Abstract]
	[Export ("availability_StaticAbstractInternal_Export:", IsVariadic = true)]
	NSObject Availability_StaticAbstractInternal_Export (int n, IntPtr varArgs);
}
