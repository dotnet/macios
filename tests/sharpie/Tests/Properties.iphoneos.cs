using Foundation;
using ObjCRuntime;

// @interface PropertyTests
interface PropertyTests {
	// @property (readonly) int readonly;
	[Export ("readonly")]
	int Readonly { get; }

	// @property int readWrite;
	[Export ("readWrite")]
	int ReadWrite { get; set; }

	// @property (getter = customGetter) int readWriteCustomGetterInt32;
	[Export ("readWriteCustomGetterInt32")]
	int ReadWriteCustomGetterInt32 { [Bind ("customGetter")] get; set; }

	// @property (setter = customSetter:) int readWriteCustomSetterInt32;
	[Export ("readWriteCustomSetterInt32")]
	int ReadWriteCustomSetterInt32 { get; [Bind ("customSetter:")] set; }

	// @property (getter = customGetter, setter = customSetterInt32:) int readWriteCustomGetterAndSetter;
	[Export ("readWriteCustomGetterAndSetter")]
	int ReadWriteCustomGetterAndSetter { [Bind ("customGetter")] get; [Bind ("customSetterInt32:")] set; }

	// @property (readonly, copy) PropertyTests * clone;
	[Export ("clone", ArgumentSemantic.Copy)]
	PropertyTests Clone { get; }

	// @property (readonly, copy) NSString * stringValue;
	[Export ("stringValue")]
	string StringValue { get; }

	// @property (class) int staticInt;
	[Static]
	[Export ("staticInt")]
	int StaticInt { get; set; }

	// @property (readonly, class) int staticReadonlyInt;
	[Static]
	[Export ("staticReadonlyInt")]
	int StaticReadonlyInt { get; }

	// @property (copy, nonatomic, null_resettable) NSDate * _Null_unspecified nullResettableDate;
	[NullAllowed, Export ("nullResettableDate", ArgumentSemantic.Copy)]
	NSDate NullResettableDate { get; set; }
}
