// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

//
// This tests MethodWithNoParametersReturningABlockMassager
// and GenerateUsingStatementsMassager (to ensure that
// MemberType's generic type arguments are also visited)

@interface Blocks
-(Blocks *(^)(id))get_Func_NSObject_Blocks;
@end
