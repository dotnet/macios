#!/bin/bash -eu

# Create a NuGet feed with the packages we built, and a NuGet.config that
# uses it, on the Mac used for remote tests. Windows passes the path to this
# NuGet.config to the ConfigureMac target (RemoteNuGetConfigFile), so that
# the workloads can be installed on the Mac without depending on the Mac's
# checkout of this repository.

set -o pipefail

cd "$(dirname "${BASH_SOURCE[0]}")/../../../.."
REPO_DIR=$(pwd)

PACKAGES_DIR="$BUILD_SOURCESDIRECTORY/artifacts/${MACIOS_UPLOAD_PREFIX:-}not-signed-package"
OUTPUT_DIR="$HOME/remote_build_testing/nuget"
FEED_DIR="$OUTPUT_DIR/feed"
CONFIG_FILE="$OUTPUT_DIR/NuGet.config"

if ! ls "$PACKAGES_DIR/"*.nupkg >/dev/null 2>&1; then
	echo "No NuGet packages found in $PACKAGES_DIR"
	exit 1
fi

rm -rf "$OUTPUT_DIR"
mkdir -p "$FEED_DIR"
cp "$PACKAGES_DIR/"*.nupkg "$FEED_DIR"

# Start from the repository's NuGet.config:
# * Use the default global packages folder, not one relative to this file.
# * Replace the feed with local test packages with the feed we just created.
sed \
	-e '/key="repositorypath"/d' \
	-e '/key="globalPackagesFolder"/d' \
	-e "s|<add key=\"local-tests-feed\" value=\"[^\"]*\" />|<add key=\"macios-ci-feed\" value=\"$FEED_DIR\" />|" \
	"$REPO_DIR/NuGet.config" > "$CONFIG_FILE"

if ! grep -q 'key="macios-ci-feed"' "$CONFIG_FILE"; then
	echo "Failed to add the local feed to $CONFIG_FILE"
	exit 1
fi

cat "$CONFIG_FILE"
ls -la "$FEED_DIR"

echo "##vso[task.setvariable variable=REMOTE_NUGET_CONFIG_FILE;isOutput=true]$CONFIG_FILE"
