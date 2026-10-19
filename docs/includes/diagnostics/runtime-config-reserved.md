<!-- Copyright (c) Microsoft Corporation. Licensed under the MIT License. -->

## Issue

The project attempts to set a runtime configuration property that the Apple runtime integration reserves for its own use. The diagnostic names the reserved property.

## Solution

Remove the user-supplied value for that property, including any corresponding `RuntimeHostConfigurationOption` item in the project. Let the runtime integration generate its required value.
