<!-- Copyright (c) Microsoft Corporation. Licensed under the MIT License. -->

## Issue

Sealer failed processing `...`.

## Solution

Something unexpected occured when trying to seal types or methods (final) or when devirtualizing some methods. The assembly causing the issue is named in the error message. To fix this issue the assembly will need to be provided in a new issue on [GitHub](https://github.com/dotnet/macios/issues/new) along with a complete build log with verbosity enabled (i.e. `-v -v -v -v` in the **Additional mtouch arguments**).

<!-- MT2060 - MT2069 used by the above error -->
