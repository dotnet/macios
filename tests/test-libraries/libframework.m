#include "libframework.h"

int
theUltimateAnswer ()
{
	return 42;
}

@implementation FrameworkTest 
{
}
-(int) func
{
	// Send an Objective-C message so this dynamic library imports an Objective-C runtime
	// function (objc_msgSend/objc_opt_self/...) referenced via the indirect symbol table.
	// This means the framework binary can't be fully stripped ('strip <binary>' fails with
	// "symbols referenced by indirect symbol table entries that can't be stripped"), it has
	// to be stripped with 'strip -S -x' instead. This lets tests that embed this framework as
	// a dynamic library exercise the symbol stripping code path.
	// Ref: https://github.com/dotnet/macios/issues/25952
	(void) [self self];
	return 42;
}
@end