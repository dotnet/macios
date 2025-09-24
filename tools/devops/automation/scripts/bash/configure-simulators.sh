#!/bin/bash -ex

# This script will:
# * Delete all simulator devices.
# * Create two new simulator devices, one for iOS and one for tvOS,
#   and try to work around how unreliable the process is. For some
#   reason, successfully creating a simulator device doesn't always
#   result in an actual simulator device.

set -o pipefail

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
JSON=$(pwd)/tmp.json
trap 'rm -f $FILE $JSON' EXIT

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

function killCoreSimulator ()
{
	launchctl kill -9 system/com.apple.CoreSimulator.simdiskimaged || true
	pkill -9 com.apple.CoreSimulator.CoreSimulatorService || true
	pkill -9 CoreSimulator.framework || true
}

function createDevice ()
{
	local PLATFORM=$1
	local NAME=$2
	local DEVICE_TYPE=$3
	local RUNTIME=$4

	local ATTEMPTS=0
	local DEVICE_UDID

	# condition here really is just a failsafe, we're not trying 10 times
	while [[ $ATTEMPTS -lt 10 ]] ; do
		echo "Trying to create an $PLATFORM device..."
		DEVICE_UDID=$(xcrun simctl create "$NAME" "$DEVICE_TYPE" "$RUNTIME")
		echo "Created $PLATFORM device with UDID=$DEVICE_UDID"

		xcrun simctl list devices > "$FILE" 2>&1
		xcrun simctl list devices --json > "$JSON" 2>&1
		cat "$FILE"
		cat "$JSON"
		if grep "$DEVICE_UDID" "$JSON"; then
			return
		fi

		# device doesn't exists (yet?), cleanup, wait a bit and check again
		killCoreSimulator
		sleep "$(( ATTEMPTS * 10 ))"

		xcrun simctl list devices > "$FILE" 2>&1
		xcrun simctl list devices --json > "$JSON" 2>&1
		cat "$FILE"
		cat "$JSON"
		if grep "$DEVICE_UDID" "$JSON"; then
			return
		fi

		# ok, looks like the device won't exist, so trying again
		(( ATTEMPTS++ ))
		if [[ $ATTEMPTS -gt 5 ]]; then
			echo "Unable to create $PLATFORM simulator device"
			exit 1
		fi
	done
}

createDevice tvOS "Apple TV (tvOS $TVOS_OS_VERSION) - created by CI" "$TVOS_SIMULATOR_DEVICE_TYPE" "com.apple.CoreSimulator.SimRuntime.tvOS-$TVOS_SIMRUNTIME_VERSION"

sleep 3 # the eternal 🤞 solution

createDevice iOS "iPhone 14 (iOS $IOS_OS_VERSION) - created by CI"  "$IOS_SIMULATOR_DEVICE_TYPE"  "com.apple.CoreSimulator.SimRuntime.iOS-$IOS_SIMRUNTIME_VERSION"

xcrun simctl list --json
