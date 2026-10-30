<!-- Copyright (c) Microsoft Corporation. Licensed under the MIT License. -->

## Issue

Remove User Resources failed processing `...`.

## Solution

Something unexpected occurred when trying to remove user resources. The assembly causing the issue is named in the error message. To fix this issue the assembly will need to be provided in a new issue on [GitHub](https://github.com/dotnet/macios/issues/new) along with a complete build log with verbosity enabled (i.e. `-v -v -v -v` in the **Additional mtouch arguments**).

User resources are files included inside assemblies (as resources) that needs to be extracted, at build time, to create the application bundle. This includes:

- `__monotouch_content_*` and `__monotouch_pages_*` resources; and
- Native libraries embedded inside a binding assembly;

<!-- MT2030 - MT2039 used by the above error -->
