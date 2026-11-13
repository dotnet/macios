<!-- Copyright (c) Microsoft Corporation. Licensed under the MIT License. -->

## Issue

Unknown format for fat entry at position \* in \*.

## Solution

An error occurred while processing the fat archive in question.

Please make sure the fat archive is valid.

The format of a fat archive can be verified using the `file` command from a terminal:

```
file -arch all -l /path/to/file
```
