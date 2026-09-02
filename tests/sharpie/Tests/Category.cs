using Foundation;

// @interface Interface
interface Interface {
}

// @protocol SomeProtocol
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
interface SomeProtocol {
}

// @interface Category (Interface)
[Category]
[BaseType (typeof (Interface))]
interface Interface_Category {
}

// @interface CategoryWithProtocol (Interface) <SomeProtocol>
[Category]
[BaseType (typeof (Interface))]
interface Interface_CategoryWithProtocol : ISomeProtocol {
}

// @interface CategoryWithInstanceType (Interface)
[Category]
[BaseType (typeof (Interface))]
interface Interface_CategoryWithInstanceType {
	// +(instancetype)hello;
	[Static]
	[Export ("hello")]
	Interface Hello ();
}

// @interface NSString
interface NSString {
}

// @interface NSData
interface NSData {
}

// @interface Extensions (NSString)
[Category]
[BaseType (typeof (NSString))]
interface NSString_Extensions {
}

// @interface Extensions (NSData)
[Category]
[BaseType (typeof (NSData))]
interface NSData_Extensions {
}
