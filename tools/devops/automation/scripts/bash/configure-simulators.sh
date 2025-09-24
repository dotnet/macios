#!/bin/bash -ex

if test -z "$BUILD_SOURCESDIRECTORY"; then
	pushd .
	cd "$(dirname "${BASH_SOURCE[0]}")/../../../../../.."
	BUILD_SOURCESDIRECTORY=$(pwd)
	popd
fi
if test -z "$BUILD_REPOSITORY_TITLE"; then
	BUILD_REPOSITORY_TITLE="macios"
fi

xcrun simctl shutdown all
xcrun simctl erase all

FILE=$(pwd)/tmp.txt

make -C "$BUILD_SOURCESDIRECTORY/$BUILD_REPOSITORY_TITLE/tools/devops" print-variable-value-to-file FILE="$FILE" VARIABLE=IOS_SIMULATOR_DEVICE_TYPE
IOS_SIMULATOR_DEVICE_TYPE=$(cat "$FILE")

make -C "$BUILD_SOURCESDIRECTORY/$BUILD_REPOSITORY_TITLE/tools/devops" print-variable-value-to-file FILE="$FILE" VARIABLE=TVOS_SIMULATOR_DEVICE_TYPE
TVOS_SIMULATOR_DEVICE_TYPE=$(cat "$FILE")

make -C "$BUILD_SOURCESDIRECTORY/$BUILD_REPOSITORY_TITLE/tools/devops" print-variable-value-to-file FILE="$FILE" VARIABLE=IOS_NUGET_OS_VERSION
IOS_NUGET_OS_VERSION=$(cat "$FILE")

make -C "$BUILD_SOURCESDIRECTORY/$BUILD_REPOSITORY_TITLE/tools/devops" print-variable-value-to-file FILE="$FILE" VARIABLE=TVOS_NUGET_OS_VERSION
TVOS_NUGET_OS_VERSION=$(cat "$FILE")

IOS_OS_VERSION=$IOS_NUGET_OS_VERSION
TVOS_OS_VERSION=$TVOS_NUGET_OS_VERSION

IOS_SIMRUNTIME_VERSION=${IOS_OS_VERSION/./-}
TVOS_SIMRUNTIME_VERSION=${TVOS_OS_VERSION/./-}

rm -f "$FILE"

xcrun simctl create "Apple TV (tvOS $TVOS_OS_VERSION) - created by CI" $TVOS_SIMULATOR_DEVICE_TYPE com.apple.CoreSimulator.SimRuntime.tvOS-$TVOS_SIMRUNTIME_VERSION
xcrun simctl create "iPhone 14 (iOS $IOS_OS_VERSION) - created by CI"  $IOS_SIMULATOR_DEVICE_TYPE  com.apple.CoreSimulator.SimRuntime.iOS-$IOS_SIMRUNTIME_VERSION

xcrun simctl list --json
