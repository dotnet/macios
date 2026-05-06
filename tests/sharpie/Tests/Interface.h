// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

@interface Root
@end

@interface Sub : Root
@end

@interface Members
-(void)instanceMethod;
-(int)instanceMethod:(int *)takingIntPtr andOutSub:(Sub **)sub;
+(void)staticMethod;
@property int intProperty;
@property (readonly) int intPropertyReadonly;
+(int)staticCsharpProperty;
+(void)staticCsharpProperty:(int)value;
@end

@interface Fields
{
	int intField;
}

-(int)getIntField;
-(void)setIntField:(int)value;
@end
