<!-- Copyright (c) Microsoft Corporation. Licensed under the MIT License. -->

## Issue

Incompatible Code For Bitcode Remover failed processing `...`.

## Solution

Something unexpected occured when trying to remove incompatible code for
bitcode from the application. The assembly causing the issue is named in the
error message. In order to fix this issue the assembly will need to be
provided in a new issue on
[GitHub](https://github.com/dotnet/macios/issues/new) along with a
complete build log with verbosity enabled (i.e. `-v -v -v -v` in the
**Additional mtouch arguments** in the project's watchOS Build options).

It's usually possible to work around this by adding
`--optimize=-remove-unsupported-il-for-bitcode` to the **Additional mtouch arguments**
in the project's watchOS Build options.

<!-- MT2210 - MT2219 used by the above error -->
