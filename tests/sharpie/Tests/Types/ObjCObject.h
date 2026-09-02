// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

@interface NSString
@end

NSString *_NSString ();
NSString **_Ptr_NSString ();
NSString ***_Ptr_Ptr_NSString ();

@protocol ProtoA
@end

@protocol ProtoB
@end

id _Id ();
Class _Class ();
id<ProtoA> _Id_of_ProtoA ();
Class<ProtoA> _Class_of_ProtoA ();
SEL _SEL ();
SEL *_Ptr_SEL ();
SEL **_Ptr_Ptr_SEL ();

@interface Foo
+(instancetype)create;
@end

Foo<ProtoA, ProtoB> *_Foo_of_ProtoA_and_ProtoB ();
