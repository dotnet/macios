#!/bin/bash -eu

# Create a NuGet feed with the packages we built, and a NuGet.config that
# uses it, on the Mac used for remote tests. Windows passes the path to this
# NuGet.config to the ConfigureMac target (RemoteNuGetConfigFile), so that
# the workloads can be installed on the Mac without depending on the Mac's
# checkout of this repository.
# Set PACKAGES_DIR, FEED_DIR and CONFIG_FILE to override the CI paths.

set -o pipefail

cd "$(dirname "${BASH_SOURCE[0]}")/../../../.."
REPO_DIR=$(pwd)

PACKAGES_DIR="${PACKAGES_DIR:-${BUILD_SOURCESDIRECTORY:?Set BUILD_SOURCESDIRECTORY or PACKAGES_DIR}/artifacts/${MACIOS_UPLOAD_PREFIX:-}not-signed-package}"
FEED_DIR="${FEED_DIR:-$HOME/remote_build_testing/nuget/feed}"
CONFIG_FILE="${CONFIG_FILE:-$HOME/remote_build_testing/nuget/NuGet.config}"

if ! ls "$PACKAGES_DIR/"*.nupkg >/dev/null 2>&1; then
	echo "No NuGet packages found in $PACKAGES_DIR"
	exit 1
fi

PACKAGES_DIR=$(cd "$PACKAGES_DIR" && pwd -P)
mkdir -p "$FEED_DIR" "$(dirname "$CONFIG_FILE")"
FEED_DIR=$(cd "$FEED_DIR" && pwd -P)
if [[ "$PACKAGES_DIR" == "$FEED_DIR" ]]; then
	echo "The package source and feed directories must be different: $FEED_DIR"
	exit 1
fi

ESCAPED_FEED_DIR=$(printf '%s' "$FEED_DIR" | sed \
	-e 's/&/\&amp;/g' \
	-e 's/</\&lt;/g' \
	-e 's/>/\&gt;/g' \
	-e 's/"/\&quot;/g' \
	-e 's/[\\&|]/\\&/g')

# Remove stale packages without deleting other files in a caller-supplied directory.
find "$FEED_DIR" -maxdepth 1 -type f -name '*.nupkg' -delete
cp "$PACKAGES_DIR/"*.nupkg "$FEED_DIR"

# Start from the repository's NuGet.config:
# * Use the default global packages folder, not one relative to this file.
# * Replace the feed with local test packages with the feed we just created.
sed \
	-e '/key="repositorypath"/d' \
	-e '/key="globalPackagesFolder"/d' \
	-e "s|<add key=\"local-tests-feed\" value=\"[^\"]*\" />|<add key=\"macios-ci-feed\" value=\"$ESCAPED_FEED_DIR\" />|" \
	"$REPO_DIR/NuGet.config" > "$CONFIG_FILE"

if ! grep -q 'key="macios-ci-feed"' "$CONFIG_FILE"; then
	echo "Failed to add the local feed to $CONFIG_FILE"
	exit 1
fi

cat "$CONFIG_FILE"
ls -la "$FEED_DIR"

ESCAPED_CONFIG_FILE=${CONFIG_FILE//%/%AZP25}
ESCAPED_CONFIG_FILE=${ESCAPED_CONFIG_FILE//$'\r'/%0D}
ESCAPED_CONFIG_FILE=${ESCAPED_CONFIG_FILE//$'\n'/%0A}
echo "##vso[task.setvariable variable=REMOTE_NUGET_CONFIG_FILE;isOutput=true]$ESCAPED_CONFIG_FILE"
