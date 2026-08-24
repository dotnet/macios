<!--
Copyright (c) Microsoft Corporation.
Licensed under the MIT License.
-->
# Changelog

This tool reads dependency changes from a pull request and formats them as a
Markdown changelog.

## Usage

```sh
dotnet run --project scripts/changelog/changelog.csproj -- https://github.com/dotnet/macios/pull/11175
```

Additional arguments filter the dependency repositories included in the output.
