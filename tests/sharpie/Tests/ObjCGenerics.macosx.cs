using Foundation;
using ObjCRuntime;

// audit-objc-generics: @interface CNLabeledValue<UnboundedValueType, ValueType : id<NSCopying,NSSecureCoding>, __covariant CovariantValueType> : NSObject <NSCopying, NSSecureCoding>
[BaseType (typeof (NSObject))]
interface CNLabeledValue : INSCopying, INSSecureCoding {
	// @property (readonly, copy, nonatomic) ValueType ValueTypeProperty;
	[Export ("ValueTypeProperty", ArgumentSemantic.Copy)]
	NSObject<NSCopying, INSSecureCoding> ValueTypeProperty { get; }

	// -(ValueType _Nullable)getValueTypeMethod;
	[NullAllowed, Export ("getValueTypeMethod")]
	[Verify (MethodToProperty)]
	NSObject<NSCopying, INSSecureCoding> ValueTypeMethod { get; }

	// -(void)setValueTypeMethod:(ValueType _Nullable)obj;
	[Export ("setValueTypeMethod:")]
	void SetValueTypeMethod ([NullAllowed] NSObject<NSCopying, INSSecureCoding> obj);
}

// @protocol A
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
interface A {
}

// @protocol B
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
interface B {
}

// @interface GenericTypesTest
interface GenericTypesTest {
	// -(NSDictionary<NSString *,NSNumber *> *)NSDictionaryOfNSStringToNSNumber;
	[Export ("NSDictionaryOfNSStringToNSNumber")]
	[Verify (MethodToProperty)]
	NSDictionary<NSString, NSNumber> NSDictionaryOfNSStringToNSNumber { get; }

	// -(NSDictionary<id<A,B>,NSString *> *)NSDictionaryOfNSObjectConformingToAANDBToNSString;
	[Export ("NSDictionaryOfNSObjectConformingToAANDBToNSString")]
	[Verify (MethodToProperty)]
	NSDictionary<NSObject<A, B>, NSString> NSDictionaryOfNSObjectConformingToAANDBToNSString { get; }

	// -(NSArray<NSString *> *)NSArrayOfNSString;
	[Export ("NSArrayOfNSString")]
	[Verify (MethodToProperty)]
	string [] NSArrayOfNSString { get; }

	// -(NSSet<NSArray<NSString *> *> *)NSSetOfNSArrayOfNSString;
	[Export ("NSSetOfNSArrayOfNSString")]
	[Verify (MethodToProperty)]
	NSSet<NSArray<NSString>> NSSetOfNSArrayOfNSString { get; }

	// -(NSDictionary<NSString *,NSSet<NSDictionary<NSString *,NSArray<NSNumber *> *> *> *> *)NSDictionaryOfNSStringToNSSetOfNSDictionaryOfNSStringToNSArrayOfNSNumber;
	[Export ("NSDictionaryOfNSStringToNSSetOfNSDictionaryOfNSStringToNSArrayOfNSNumber")]
	[Verify (MethodToProperty)]
	NSDictionary<NSString, NSSet<NSDictionary<NSString, NSArray<NSNumber>>>> NSDictionaryOfNSStringToNSSetOfNSDictionaryOfNSStringToNSArrayOfNSNumber { get; }
}
