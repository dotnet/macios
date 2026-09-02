// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

// RUN: -x objective-c
// RUN custom_delegates: -x objective-c -custom-delegates

typedef void (^Action)();
typedef int (^Anon_Func_Long_Int)(long long);
typedef int (^Named_Func_Long_Int)(long long longArg);
typedef void (^Variadic)(int, ...);

@interface TypedefBlockTests
-(Action)get_Action;
-(Anon_Func_Long_Int)get_Anon_Func_Long_Int;
-(Named_Func_Long_Int)get_Named_Func_Long_Int;
@end

@interface PropertyBlockTests
@property (readonly, copy) void (^action)();
@property (readonly, copy) void (^action_int)(int);
@property (readonly, copy) void (^action_action)(void (^)());
@property (readonly, copy) void (^action_action_int)(void (^)(int));
@property (readonly, copy) void (^action_action_action)(void (^)(void (^)()));
@property (readonly, copy) int (^func_int_int)(int);
@property (readonly, copy) long (^func_short_nint)(short);
@end

@interface AnonymousBlockTests
-(void)set_Action:(void (^)())handler;
-(void)set_Action_int:(void (^)(int))handler;
-(void)set_Action_short_int_long:(void (^)(short, int, long long))handler;
-(void)set_Func_int:(int (^)())handler;
-(void)set_Func_int_int:(int (^)(int))handler;
-(void)set_Func_short_int_long_bool:(_Bool (^)(short, int, long long))handler;
-(void)set_Func_Func_short_Action_long_short_bool:(_Bool (^)(long long (^)(short, void (^)()), short))handler;
-(void)set_Func_Func_short_Action_Action_int_nint_long_short_bool:(_Bool (^)(long long (^)(short, void (^)(void (^)(int, long))), short))handler;
@end
