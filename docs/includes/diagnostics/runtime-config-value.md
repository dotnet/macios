<!-- Copyright (c) Microsoft Corporation. Licensed under the MIT License. -->

## Issue

A property in `runtimeOptions.configProperties` has a value that the native application launcher cannot represent. The diagnostic identifies the property and the runtime configuration file.

## Solution

Use a supported scalar value for the property rather than a JSON object or array. Check the documentation for the specific .NET runtime configuration option before changing its value. If the configuration file is generated, correct the corresponding project setting rather than editing the generated file.
