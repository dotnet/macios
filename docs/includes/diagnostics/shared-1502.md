<!-- Copyright (c) Microsoft Corporation. Licensed under the MIT License. -->

## Issue

This warning might be reported when using `--warn-on-type-ref=X` if any loaded (unmodified) assembly has a type reference to the type `X`.

This can be used along with `--warnaserror:1502` to ensure a reference to a specific type (e.g. `UIKit.UIWebView`) is not being used by any assembly used the application.

Notes:
* Custom attributes are encoded differently and not included inside an assembly type references metadata.
* Assembly that define a type `X` do not have a reference (but the definition) of the type (and won't be reported).

## Solution

Follow the diagnostic-specific guidance above.
