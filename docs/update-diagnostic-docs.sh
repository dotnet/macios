#!/bin/bash
# Copyright (c) Microsoft Corporation.
# Licensed under the MIT License.
set -eu

root=$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)
dotnet=${DOTNET:-"$root/builds/downloads/dotnet/dotnet"}
if ! test -x "$dotnet"; then
	dotnet=dotnet
fi
exec "$dotnet" run --project "$root/scripts/diagnostic-docs/diagnostic-docs.csproj" -- "${1:-update}" "$root" "${@:2}"
