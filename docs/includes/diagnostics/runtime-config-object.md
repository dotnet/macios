<!-- Copyright (c) Microsoft Corporation. Licensed under the MIT License. -->

## Issue

The `runtimeOptions.configProperties` value in the runtime configuration file is not a JSON object. The diagnostic identifies the file and the actual JSON value kind.

## Solution

Supply an object whose members are runtime configuration properties, for example:

```json
{
  "runtimeOptions": {
    "configProperties": {
      "System.Globalization.Invariant": true
    }
  }
}
```

If the file is generated, fix the project setting or build step that produces the invalid value.
