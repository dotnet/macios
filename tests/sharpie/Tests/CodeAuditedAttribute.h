// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

// Printing attributes results in clang 20 crashing:
// UNREACHABLE executed at llvm/bin/tools/clang/include/clang/Sema/AttrSpellingListIndex.inc:14!
#pragma clang arc_cf_code_audited begin
void DoSomething();
#pragma clang arc_cf_code_audited end
