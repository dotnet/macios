// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

// RUN objc: -x objective-c

// Regression test for https://github.com/dotnet/macios/issues/26166
// Clang attaches an availability attribute written after a `typedef enum { } Name;`
// declaration to the TypedefDecl that names the enum, not to the EnumDecl itself.
// Sharpie must still propagate that availability to the generated C# enum.

typedef enum {
	MacUnavailableEnumOne,
	MacUnavailableEnumTwo
} MacUnavailableEnum __attribute__((availability(macosx,unavailable)));
