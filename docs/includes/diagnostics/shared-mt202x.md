<!-- Copyright (c) Microsoft Corporation. Licensed under the MIT License. -->

## Issue

Binding Optimizer failed processing `...`.

## Solution

Something unexpected occurred when trying to optimize generated binding code. The element causing the issue is named in the error message. To fix this issue the assembly named (or containing the type or method named) will need to be provided in a new issue on [GitHub](https://github.com/dotnet/macios/issues/new) along with a complete build log with verbosity enabled (i.e. `-v -v -v -v` in the **Additional mtouch arguments**).

The last digit `x` will be:

- `0` for an assembly name;
- `1` for a type name;
- `3` for a method name;

<!-- MT2020 - MT2029 used by the above error -->
