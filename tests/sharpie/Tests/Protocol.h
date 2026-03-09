// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

@protocol ProtocolA
@end

@protocol ProtocolB
@end

@protocol ProtocolC <ProtocolB>
@end

@interface Impl_A <ProtocolA>
@end

@interface Impl_A_C <ProtocolA, ProtocolC>
@end

@interface Root
@end

@interface Sub : Root <ProtocolA, ProtocolB>
@end

@interface Usage
-(Root<ProtocolA, ProtocolB> *)root_A_C;
-(Root<ProtocolA> *)root_A;
-(id<ProtocolA>)proto_A;
@end

@protocol RequiredAndOptional
-(void)implicitRequiredMethod;
@property int implicitRequiredProperty;

@optional
-(void)firstOptionalMethod;
@property int firstOptionalProperty;

@required
-(void)explicitRequiredMethod;
@property int explicitRequiredProperty;

@optional
-(void)secondOptionalMethod;
@property int secondOptionalProperty;
@end
