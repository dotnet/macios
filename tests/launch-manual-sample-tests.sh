#!/bin/bash -ex

# This script will schedule a manual sample test run for the current commit.
# The current commit must already have packages available as GitHub statuses.

# An Azure Personal Access Token with Build/Read&Execute rights must be stored in ~/.local/share/Xamarin/.vsts-pat

HASH="$(git log -1 --pretty=format:%H)"
GIT_BRANCH=$(git rev-parse --abbrev-ref HEAD)
GH_MANIFEST=$(curl https://bosstoragemirror.blob.core.windows.net/wrench/jenkins/"$HASH"/package/manifest)
IOS_PACKAGE_URL=$(echo "$GH_MANIFEST" | grep "xamarin.ios.*pkg$")
MAC_PACKAGE_URL=$(echo "$GH_MANIFEST" | grep "xamarin.mac.*pkg$")

if test -z "$IOS_PACKAGE_URL"; then
	echo "No Xamarin.iOS package found."
	exit 1
elif test -z "$MAC_PACKAGE_URL"; then
	echo "No Xamarin.Mac package found."
	exit 1
fi

make -C "$(git rev-parse --show-toplevel)/../maccore/tests/external" wrench-launch-sample-builds \
	BUILD_BRANCH="$GIT_BRANCH" \
	BUILD_REVISION="$HASH" \
	BUILD_WORK_HOST=jenkins \
	WRENCH_URL=https://github.com/xamarin/xamarin-macios/commit/"$HASH" \
	MAC_PACKAGE_URL="$MAC_PACKAGE_URL" \
	IOS_PACKAGE_URL="$IOS_PACKAGE_URL" \
