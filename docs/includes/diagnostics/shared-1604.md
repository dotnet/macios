<!-- Copyright (c) Microsoft Corporation. Licensed under the MIT License. -->

## Issue

File of type \* is not a MachO file (\*).

## Solution

An error occurred while processing the MachO file in question.

Please make sure the file is a valid Mach-O dynamic library.

The format of a file can be verified using the `file` command from a terminal:

    file -arch all -l /path/to/file
