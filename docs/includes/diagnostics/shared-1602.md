<!-- Copyright (c) Microsoft Corporation. Licensed under the MIT License. -->

## Issue

Not a Mach-O dynamic library (unknown header '0x*'): *.

## Solution

An error occurred while processing the dynamic library in question.

Please make sure the dynamic library is a valid Mach-O dynamic library.

The format of a library can be verified using the `file` command from a terminal:

```
file -arch all -l /path/to/library.dylib
```
