<!-- Copyright (c) Microsoft Corporation. Licensed under the MIT License. -->

## Issue

Smart Enum Conversion Preserver failed processing `...`.

## Solution

Something unexpected occurred when trying to mark the conversion methods for smart enums from the application. The assembly causing the issue is named in the error message. In order to fix this issue the assembly will need to be provided in a new issue on [GitHub](https://github.com/dotnet/macios/issues/new) along with a complete build log with verbosity enabled (i.e. `-v -v -v -v` in the **Additional mtouch arguments**).

<!-- MT2200 - MT2209 used by the above error -->
