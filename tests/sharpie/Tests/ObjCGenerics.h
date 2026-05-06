// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

// RUN macosx: -sdk macosx
// RUN iphoneos: -sdk iphoneos

#if __has_feature(objc_generics_variance)
#else
#  error objc_generics_variance feature should be defined
#endif

@import Foundation;

@interface CNLabeledValue<UnboundedValueType, ValueType : id <NSCopying, NSSecureCoding>, __covariant CovariantValueType> : NSObject <NSCopying, NSSecureCoding>
@property (copy, readonly, nonatomic) ValueType ValueTypeProperty;
-(nullable ValueType)getValueTypeMethod;
-(void)setValueTypeMethod:(nullable ValueType)obj;
@end

@protocol A
@end

@protocol B
@end

@interface GenericTypesTest
-(NSDictionary<NSString *, NSNumber *> *)NSDictionaryOfNSStringToNSNumber;
-(NSDictionary<id<A, B>, NSString *> *)NSDictionaryOfNSObjectConformingToAANDBToNSString;
-(NSArray<NSString *> *)NSArrayOfNSString;
-(NSSet<NSArray<NSString *> *> *)NSSetOfNSArrayOfNSString;
-(NSDictionary<NSString *, NSSet<NSDictionary<NSString *, NSArray<NSNumber *> *> *> *> *)NSDictionaryOfNSStringToNSSetOfNSDictionaryOfNSStringToNSArrayOfNSNumber;
@end
