<!-- Copyright (c) Microsoft Corporation. Licensed under the MIT License. -->

## Issue

MarkNSObjects failed processing `...`.

## Solution

Something unexpected occurred when trying to mark `NSObject` subclasses from the application. The assembly causing the issue is named in the error message. To fix this issue the assembly will need to be provided in a new issue on [GitHub](https://github.com/dotnet/macios/issues/new) along with a complete build log with verbosity enabled (i.e. `-v -v -v -v` in the **Additional mtouch arguments**).

<!-- MT2080 - MT2089 used by the above error -->
