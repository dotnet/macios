#!/bin/bash
set -euo pipefail

mkdir -p "$DIAGNOSTIC_DIRECTORY/logs"
(
    set -x

    # Stay outside the checkout to avoid its global.json and NuGet.config.
    cd "$DIAGNOSTIC_DIRECTORY"
    export DOTNET_ROOT="$DIAGNOSTIC_DIRECTORY/dotnet"
    export DOTNET_CLI_HOME="$DIAGNOSTIC_DIRECTORY/home"
    export NUGET_PACKAGES="$DIAGNOSTIC_DIRECTORY/packages"
    export DOTNET_CLI_TELEMETRY_OPTOUT=1
    export DOTNET_SKIP_FIRST_TIME_EXPERIENCE=1

    uname -a
    cat /etc/os-release
    curl --fail --show-error --location https://dot.net/v1/dotnet-install.sh --output dotnet-install.sh
    bash dotnet-install.sh --version 11.0.100-rc.2.26504.105 --install-dir "$DOTNET_ROOT" --no-path --verbose
    "$DOTNET_ROOT/dotnet" --info
    "$DOTNET_ROOT/dotnet" workload --info

    # Keep the RC 2 SDK's bundled manifests instead of updating to newer workload versions.
    "$DOTNET_ROOT/dotnet" workload install ios --skip-manifest-update \
        --source https://api.nuget.org/v3/index.json --verbosity diagnostic
    "$DOTNET_ROOT/dotnet" workload list
) 2>&1 | tee "$DIAGNOSTIC_DIRECTORY/logs/install.log"
