#!/bin/bash -e

set -o pipefail

cd $(dirname $0)

# Detect if we're running on Linux
if [[ "$(uname -s)" == "Linux" ]]; then
	NO_XCODE=1
	# On Linux, ignore all macOS-specific dependencies
	IGNORE_OSX=1
	IGNORE_XCODE=1
	IGNORE_XCODE_COMPONENTS=1
	IGNORE_VISUAL_STUDIO=1
	IGNORE_SIMULATORS=1
	IGNORE_OLD_SIMULATORS=1
	IGNORE_7Z=1
	IGNORE_HOMEBREW=1
	IGNORE_SHELLCHECK=1
	IGNORE_YAMLLINT=1
	IGNORE_PYTHON3=1
else
	NO_XCODE=
fi

FAIL=
PROVISION_DOWNLOAD_DIR=/tmp/x-provisioning
SUDO=sudo
VERBOSE=
XCODE_PACKAGE_DIRECTORY=

OPTIONAL_SIMULATORS=1
OPTIONAL_OLD_SIMULATORS=1

if test -f configure.inc; then
	source configure.inc

	if test -n "$NO_XCODE"; then
		IGNORE_OSX=1
		IGNORE_XCODE=1
		IGNORE_SIMULATORS=1
		IGNORE_OLD_SIMULATORS=1
		IGNORE_XCODE_COMPONENTS=1
	fi
fi

function get_xcode_developer_root ()
{
	local suffix="${1:-}"

	# When we're provisioning Xcode ourselves, Make.config is the only source of truth:
	# install-xcode.sh reads it directly, so honoring an inherited variable or a stale
	# configure.inc here would make the rest of this script inspect a different Xcode
	# than the one we just installed and selected.
	if test -n "$XCODE_PACKAGE_DIRECTORY" || test -n "${PROVISION_XCODE:-}"; then
		grep "^XCODE${suffix}_DEVELOPER_ROOT[?:]*=" Make.config | sed 's/^[^=]*=//'
		return
	fi

	if test -z "$suffix"; then
		if test -n "${XCODE_DEVELOPER_ROOT:-}"; then
			echo "$XCODE_DEVELOPER_ROOT"
			return
		fi

		if XCODE_DEVELOPER_ROOT_ASSIGNMENT=$(grep "^XCODE_DEVELOPER_ROOT=" configure.inc 2>/dev/null); then
			echo "${XCODE_DEVELOPER_ROOT_ASSIGNMENT#*=}"
			return
		fi
	fi

	grep "^XCODE${suffix}_DEVELOPER_ROOT[?:]*=" Make.config | sed 's/^[^=]*=//'
}

# parse command-line arguments
while ! test -z $1; do
	case $1 in
		--no-sudo)
			SUDO=
			shift
			;;
		--provision-xcode)
			PROVISION_XCODE=1
			unset IGNORE_XCODE
			shift
			;;
		--xcode-package-directory)
			if [[ $# -lt 2 || -z "$2" ]]; then
				echo "--xcode-package-directory requires a value."
				exit 1
			fi
			XCODE_PACKAGE_DIRECTORY=$2
			PROVISION_XCODE=1
			unset IGNORE_XCODE
			shift 2
			;;
		--provision-xcode-components)
			PROVISION_XCODE_COMPONENTS=1
			unset IGNORE_XCODE_COMPONENTS
			shift
			;;
		--provision)
			# historical reasons :(
			PROVISION_XCODE=1
			PROVISION_VS=1
			unset IGNORE_XCODE
			unset IGNORE_VISUAL_STUDIO
			shift
			;;
		--provision-*-studio)
			PROVISION_VS=1
			unset IGNORE_VISUAL_STUDIO
			shift
			;;
		--provision-7z)
			PROVISION_7Z=1
			unset IGNORE_7Z
			shift
			;;
		--provision-autotools)
			# this is an old argument, just ignore it
			shift
			;;
		--provision-python3)
			# building mono from source requires having python3 installed
			PROVISION_PYTHON3=1
			unset IGNORE_PYTHON3
			shift
			;;
		--provision-simulators)
			PROVISION_SIMULATORS=1
			unset OPTIONAL_SIMULATORS
			unset IGNORE_SIMULATORS
			shift
			;;
		--provision-old-simulators)
			PROVISION_OLD_SIMULATORS=1
			unset OPTIONAL_OLD_SIMULATORS
			unset IGNORE_OLD_SIMULATORS
			shift
			;;
		--provision-dotnet)
			PROVISION_DOTNET=1
			unset IGNORE_DOTNET
			shift
			;;
		--provision-shellcheck)
			PROVISION_SHELLCHECK=1
			unset IGNORE_SHELLCHECK
			shift
			;;
		--provision-yamllint)
			PROVISION_YAMLLINT=1
			unset IGNORE_YAMLLINT
			shift
			;;
		--provision-all)
			PROVISION_VS=1
			unset IGNORE_VISUAL_STUDIO
			PROVISION_XCODE=1
			unset IGNORE_XCODE
			PROVISION_7Z=1
			unset IGNORE_7Z
			PROVISION_HOMEBREW=1
			unset IGNORE_HOMEBREW
			PROVISION_SIMULATORS=1
			unset IGNORE_SIMULATORS
			PROVISION_OLD_SIMULATORS=1
			unset IGNORE_OLD_SIMULATORS
			PROVISION_PYTHON3=1
			unset IGNORE_PYTHON3
			PROVISION_DOTNET=1
			unset IGNORE_DOTNET
			PROVISION_SHELLCHECK=1
			unset IGNORE_SHELLCHECK
			PROVISION_YAMLLINT=1
			unset IGNORE_YAMLLINT
			PROVISION_XCODE_COMPONENTS=1
			unset IGNORE_XCODE_COMPONENTS
			shift
			;;
		--ignore-all)
			IGNORE_OSX=1
			IGNORE_VISUAL_STUDIO=1
			IGNORE_XCODE=1
			IGNORE_7Z=1
			IGNORE_HOMEBREW=1
			IGNORE_SIMULATORS=1
			IGNORE_PYTHON3=1
			IGNORE_DOTNET=1
			IGNORE_SHELLCHECK=1
			IGNORE_YAMLLINT=1
			IGNORE_XCODE_COMPONENTS=1
			shift
			;;
		--ignore-osx)
			IGNORE_OSX=1
			shift
			;;
		--ignore-xcode)
			IGNORE_XCODE=1
			shift
			;;
		--ignore-xcode-components)
			IGNORE_XCODE_COMPONENTS=1
			shift
			;;
		--ignore-*-studio)
			IGNORE_VISUAL_STUDIO=1
			shift
			;;
		--ignore-autotools)
			# this is an old argument, just ignore it
			shift
			;;
		--ignore-python3)
			IGNORE_PYTHON3=1
			shift
			;;
		--ignore-7z)
			IGNORE_7Z=1
			shift
			;;
		--ignore-simulators)
			IGNORE_SIMULATORS=1
			shift
			;;
		--ignore-old-simulators)
			IGNORE_OLD_SIMULATORS=1
			shift
			;;
		--enforce-simulators)
			unset IGNORE_SIMULATORS
			unset OPTIONAL_SIMULATORS
			shift
			;;
		--ignore-dotnet)
			IGNORE_DOTNET=1
			shift
			;;
		--ignore-shellcheck)
			IGNORE_SHELLCHECK=1
			shift
			;;
		--ignore-yamllint)
			IGNORE_YAMLLINT=1
			shift
			;;
		-v | --verbose)
			set -x
			shift
			VERBOSE=1
			;;
		*)
			echo "Unknown argument: $1"
			exit 1
			;;
	esac
done

if test -z "${NO_XCODE:-}"; then
	XCODE_DEVELOPER_ROOT=$(get_xcode_developer_root)
	export XCODE_DEVELOPER_ROOT
	export DEVELOPER_DIR="$XCODE_DEVELOPER_ROOT"
fi

# reporting functions
COLOR_RED=$(tput setaf 1 2>/dev/null || true)
COLOR_ORANGE=$(tput setaf 3 2>/dev/null || true)
COLOR_MAGENTA=$(tput setaf 5 2>/dev/null || true)
COLOR_BLUE=$(tput setaf 6 2>/dev/null || true)
COLOR_CLEAR=$(tput sgr0 2>/dev/null || true)
COLOR_RESET=uniquesearchablestring
FAILURE_PREFIX=
if test -z "$COLOR_RED"; then FAILURE_PREFIX="** failure ** "; fi

function fail () {
	echo "    $FAILURE_PREFIX${COLOR_RED}${1//${COLOR_RESET}/${COLOR_RED}}${COLOR_CLEAR}"
	FAIL=1
}

function warn () {
	echo "    ${COLOR_ORANGE}${1//${COLOR_RESET}/${COLOR_ORANGE}}${COLOR_CLEAR}"
}

function ok () {
	echo "    ${1//${COLOR_RESET}/${COLOR_CLEAR}}"
}

function log () {
	echo "        ${1//${COLOR_RESET}/${COLOR_CLEAR}}"
}

# $1: the version to check
# $2: the minimum version to check against
function is_at_least_version () {
	ACT_V=$1
	MIN_V=$2

	if [[ "$ACT_V" == "$MIN_V" ]]; then
		return 0
	fi

	IFS=. read -a V_ACT <<< "$ACT_V"
	IFS=. read -a V_MIN <<< "$MIN_V"
	
	# get the minimum # of elements
	AC=${#V_ACT[@]}
	MC=${#V_MIN[@]}
	COUNT=$(($AC>$MC?$MC:$AC))

	C=0
	while (( $C < $COUNT )); do
		ACT=${V_ACT[$C]}
		MIN=${V_MIN[$C]}
		if (( $ACT > $MIN )); then
			return 0
		elif (( "$MIN" > "$ACT" )); then
			return 1
		fi
		let C++
	done

	if (( $AC == $MC )); then
		# identical?
		return 0
	fi

	if (( $AC > $MC )); then
		# more version fields in actual than min: OK
		return 0
	elif (( $AC == $MC )); then
		# entire strings aren't equal (first check in function), but each individual field is?
		return 0
	else
		# more version fields in min than actual (1.0 vs 1.0.1 for instance): not OK
		return 1
	fi
}

function delete_all_simulator_runtimes ()
{
	log "Executing 'xcrun simctl runtime delete all'..."
	xcrun simctl runtime delete all

	local TMPFILE
	TMPFILE=$(mktemp)

	local COUNT

	# sadly simulator deletion is done asynchronously, so we have to wait until they're all gone
	log "Waiting for the simulator runtimes to be deleted..."
	printf "            "
	for i in $(seq 1 60); do
		sleep 1
		xcrun simctl runtime list -j --json-output="$TMPFILE"
		COUNT=$(jq "length" -r "$TMPFILE")
		if [[ "$COUNT" == "0" ]]; then
			break
		fi
		printf "$COUNT"
	done
	printf "\n"
	if [[ "$COUNT" != "0" ]]; then
		warn "Waited for 60 seconds, but there are still $COUNT simulators waiting to deleted."
	fi

	rm -rf "$TMPFILE"
}

function xcodebuild_download_selected_platforms ()
{
	local XCODE_DEVELOPER_ROOT
	local XCODE_NAME
	local XCODE_IS_STABLE
	local XCODE_VERSION
	local IOS_NUGET_OS_VERSION
	local IOS_BUILD_VERSION
	local TVOS_NUGET_OS_VERSION
	local TVOS_BUILD_VERSION

	XCODE_DEVELOPER_ROOT=$(get_xcode_developer_root)
	XCODE_NAME=$(basename "$(dirname "$(dirname "$XCODE_DEVELOPER_ROOT")")")
	# we use the same logic here as in Make.config to determine whether we're using a stable version of Xcode or not (search for XCODE_IS_STABLE/XCODE_IS_PREVIEW)
	XCODE_IS_STABLE=$(echo "$XCODE_NAME" | sed -e 's@^Xcode[_0-9.]*[.]app$@YES@')
	XCODE_VERSION=$(grep ^XCODE_VERSION= Make.config | sed 's/.*=//')

	IOS_BUILD_VERSION=
	TVOS_BUILD_VERSION=
	# kept the is_at_least_version "$XCODE_VERSION" 26.0 guard because -architectureVariant only exists in Xcode 26+; on older Xcode it falls back to a plain -downloadPlatform.
	if is_at_least_version "$XCODE_VERSION" 26.0; then
		# we always want the arm64 variant (the x64 simulators aren't supported anymore)
		IOS_BUILD_VERSION=" -architectureVariant arm64"
		TVOS_BUILD_VERSION=" -architectureVariant arm64"
	fi

	local TMPFILE
	TMPFILE=$(mktemp)
	log "Checking if there are any simulator runtimes that aren't ready..."
	xcrun simctl runtime list --json "--json-output=$TMPFILE"
	NOT_READY_COUNT=$(jq 'map({identifier: .identifier, state: .state}) | map(select(.state != "Ready")) | length' "$TMPFILE")
	if [[ "$NOT_READY_COUNT" != "0" ]]; then
		log "Found simulator runtimes that aren't ready, will proceed to delete all simulator runtimes:"
		(
			export IFS=$'\n'
			for line in $(jq 'map({identifier: .identifier, state: .state}) | map(select(.state != "Ready"))[] | "\(.identifier): \(.state)"' -r "$TMPFILE"); do
				log "    $line"
			done
		)
		delete_all_simulator_runtimes
	else
		log "    none found."
	fi

	log "Executing '$XCODE_DEVELOPER_ROOT/usr/bin/xcodebuild -downloadPlatform iOS$IOS_BUILD_VERSION' $1"
	"$XCODE_DEVELOPER_ROOT/usr/bin/xcodebuild" -downloadPlatform iOS $IOS_BUILD_VERSION   2>&1 | sed 's/^/        /'

	log "Executing '$XCODE_DEVELOPER_ROOT/usr/bin/xcodebuild -downloadPlatform tvOS$TVOS_BUILD_VERSION' $1"
	"$XCODE_DEVELOPER_ROOT/usr/bin/xcodebuild" -downloadPlatform tvOS $TVOS_BUILD_VERSION 2>&1 | sed 's/^/        /'

	return 0
}

function download_xcode_platforms ()
{
	if test -n "$IGNORE_SIMULATORS"; then return; fi

	local XCODE_VERSION
	local XCODE_DEVELOPER_ROOT="$1"

	XCODE_VERSION=$(grep ^XCODE_VERSION= Make.config | sed 's/.*=//')

	if ! is_at_least_version "$XCODE_VERSION" 14.0; then
		# Nothing to do here
		log "This version of Xcode ($XCODE_VERSION) does not have any additional platforms to download"
		return
	fi

	if test -z "$PROVISION_SIMULATORS"; then
		warn "    Xcode may have additional platforms that must be downloaded. Execute './system-dependencies.sh --provision-simulators' to install those platforms (or alternatively ${COLOR_MAGENTA}export IGNORE_SIMULATORS=1${COLOR_RESET} to skip this check)"
		return
	fi

	log "Xcode has additional platforms that must be downloaded ($MUST_INSTALL_RUNTIMES), so installing those."

	log "Executing '$SUDO pkill -9 -f CoreSimulator.framework'"
	$SUDO pkill -9 -f "CoreSimulator.framework" || true
	if ! xcodebuild_download_selected_platforms; then
		log "Executing '$XCODE_DEVELOPER_ROOT/usr/bin/simctl runtime list -v"
		"$XCODE_DEVELOPER_ROOT/usr/bin/simctl" runtime list -v 2>&1 | sed 's/^/        /'
		# Don't exit here, just hope for the best instead.
		(
			set +x
			echo "##vso[task.logissue type=warning;sourcepath=system-dependencies.sh]Failed to download all simulator platforms, this may result in problems executing tests in the simulator."
			set -x
		)
	else
		log "Executing '$XCODE_DEVELOPER_ROOT/usr/bin/simctl runtime list -v"
		"$XCODE_DEVELOPER_ROOT/usr/bin/simctl" runtime list -v 2>&1 | sed 's/^/        /'
		log "Executing '$XCODE_DEVELOPER_ROOT/usr/bin/simctl list -v"
		"$XCODE_DEVELOPER_ROOT/usr/bin/simctl" list -v 2>&1 | sed 's/^/        /'
	fi

	log "Executing '$SUDO $XCODE_DEVELOPER_ROOT/usr/bin/xcodebuild -runFirstLaunch'"
	$SUDO "$XCODE_DEVELOPER_ROOT/usr/bin/xcodebuild" -runFirstLaunch
	log "Executed '$SUDO $XCODE_DEVELOPER_ROOT/usr/bin/xcodebuild -runFirstLaunch'"

	log "Finished installing Xcode platforms"
}

function run_xcode_first_launch ()
{
	local XCODE_VERSION="$1"
	local XCODE_DEVELOPER_ROOT="$2"

	# xcodebuild -runFirstLaunch seems to have been introduced in Xcode 9
	if ! is_at_least_version "$XCODE_VERSION" 9.0; then
		return
	fi

	if ! "$XCODE_DEVELOPER_ROOT/usr/bin/xcodebuild" -checkFirstLaunchStatus; then
		if ! test -z "$PROVISION_XCODE"; then
			# Run the first launch tasks
			log "Executing '$SUDO $XCODE_DEVELOPER_ROOT/usr/bin/xcodebuild -runFirstLaunch'"
			$SUDO "$XCODE_DEVELOPER_ROOT/usr/bin/xcodebuild" -runFirstLaunch
			log "Executed '$SUDO $XCODE_DEVELOPER_ROOT/usr/bin/xcodebuild -runFirstLaunch'"
		else
			fail "Xcode has pending first launch tasks. Execute 'make fix-xcode-first-run' to execute those tasks."
			return
		fi
	fi
}

function install_specific_xcode () {
	local XCODE_URL
	local XCODE_VERSION
	local XCODE_NAME
	local XCODE_ARCHIVE
	local INSTALLER_ARGS=(install)

	XCODE_URL=$(grep "XCODE$1_URL=" Make.config | sed 's/.*=//')
	XCODE_VERSION=$(grep "XCODE$1_VERSION=" Make.config | sed 's/.*=//')

	if test -z "$SUDO"; then
		INSTALLER_ARGS+=(--no-sudo)
	fi

	# CI downloads an immutable Universal Package from Azure Artifacts and hands us the
	# directory it was expanded into; install-xcode.sh validates it before installing.
	if test -n "$XCODE_PACKAGE_DIRECTORY"; then
		INSTALLER_ARGS+=(--package-directory "$XCODE_PACKAGE_DIRECTORY")
		"$PWD/tools/devops/automation/scripts/bash/install-xcode.sh" "${INSTALLER_ARGS[@]}"
		return
	fi

	if test -z "$XCODE_URL"; then
		fail "No XCODE$1_URL set in Make.config, cannot provision"
		return
	fi

	# CI must never silently fall back to the storage-account URL: that dependency is
	# exactly what the Universal Package replaced, and there are no credentials for it
	# here, so it would fail obscurely much later.
	if test -n "${TF_BUILD:-}"; then
		fail "Xcode $XCODE_VERSION is not installed and no Xcode Universal Package was supplied. Re-run the 'Download Xcode Universal Package' step."
		return
	fi

	mkdir -p "$PROVISION_DOWNLOAD_DIR"
	log "Downloading Xcode $XCODE_VERSION from $XCODE_URL to $PROVISION_DOWNLOAD_DIR..."
	XCODE_NAME=$(basename "$XCODE_URL")
	XCODE_ARCHIVE="$PROVISION_DOWNLOAD_DIR/$XCODE_NAME"

	# To test this script with a local archive, place it in ~/Downloads and run
	# ./system-dependencies.sh --provision-xcode.
	if test -f "$HOME/Downloads/$XCODE_NAME"; then
		log "Found $XCODE_NAME in your ~/Downloads folder, copying that version to $XCODE_ARCHIVE instead of re-downloading it."
		cp "$HOME/Downloads/$XCODE_NAME" "$XCODE_ARCHIVE"
	else
		curl --fail --location --retry 5 --retry-all-errors "$XCODE_URL" > "$XCODE_ARCHIVE"
	fi

	if [[ "$XCODE_ARCHIVE" == *.xip ]]; then
		INSTALLER_ARGS+=(--archive "$XCODE_ARCHIVE")
		"$PWD/tools/devops/automation/scripts/bash/install-xcode.sh" "${INSTALLER_ARGS[@]}"
	elif [[ "$XCODE_ARCHIVE" == *.dmg ]]; then
		fail "DMG-based Xcode provisioning is no longer supported. Provide an Apple-signed XIP archive."
		return
	else
		fail "Don't know how to install $XCODE_ARCHIVE"
		return
	fi

	rm -f "$XCODE_ARCHIVE"
	ok "Xcode $XCODE_VERSION provisioned"
}

function install_coresimulator ()
{
	local XCODE_DEVELOPER_ROOT
	local CORESIMULATOR_PKG
	local CORESIMULATOR_PKG_DIR
	local XCODE_ROOT
	local TARGET_CORESIMULATOR_VERSION
	local CURRENT_CORESIMULATOR_VERSION

	XCODE_DEVELOPER_ROOT=$(get_xcode_developer_root)
	XCODE_ROOT=$(dirname "$(dirname "$XCODE_DEVELOPER_ROOT")")
	CORESIMULATOR_PKG=$XCODE_ROOT/Contents/Resources/Packages/XcodeSystemResources.pkg

	if ! test -f "$CORESIMULATOR_PKG"; then
		warn "Could not find XcodeSystemResources.pkg (which contains CoreSimulator.framework) in $XCODE_DEVELOPER_ROOT ($CORESIMULATOR_PKG doesn't exist)."
		return
	fi

	# Get the CoreSimulator.framework version from our Xcode
	# Extract the .pkg to get the pkg's PackageInfo file, which contains the CoreSimulator.framework version.
	CORESIMULATOR_PKG_DIR=$(mktemp -d)
	pkgutil --expand "$CORESIMULATOR_PKG" "$CORESIMULATOR_PKG_DIR/extracted"

	if ! TARGET_CORESIMULATOR_VERSION=$(xmllint --xpath 'string(/pkg-info/bundle-version/bundle[@id="com.apple.CoreSimulator"]/@CFBundleShortVersionString)' "$CORESIMULATOR_PKG_DIR/extracted/PackageInfo"); then
		rm -rf "$CORESIMULATOR_PKG_DIR"
		warn "Failed to look up the CoreSimulator version of $XCODE_DEVELOPER_ROOT"
		return
	fi
	rm -rf "$CORESIMULATOR_PKG_DIR"

	# Get the CoreSimulator.framework currently installed
	local CURRENT_CORESIMULATOR_PATH=/Library/Developer/PrivateFrameworks/CoreSimulator.framework/Versions/A/CoreSimulator
	local CURRENT_CORESIMULATOR_VERSION=0.0
	if test -f "$CURRENT_CORESIMULATOR_PATH"; then
		CURRENT_CORESIMULATOR_VERSION=$(otool -L $CURRENT_CORESIMULATOR_PATH | grep "$CURRENT_CORESIMULATOR_PATH.*current version" | sed -e 's/.*current version//' -e 's/)//' -e 's/[[:space:]]//g' | uniq)
	fi

	# Either version may be composed of either 1, 2 or 3 numbers.
	# We only care about the first two, so strip off the 3rd number if it exists.
	# shellcheck disable=SC2001
	CURRENT_CORESIMULATOR_VERSION=$(echo "$CURRENT_CORESIMULATOR_VERSION" | sed 's/\([0-9]*[.][0-9]*\).*/\1/')
	# shellcheck disable=SC2001
	TARGET_CORESIMULATOR_VERSION=$(echo "$TARGET_CORESIMULATOR_VERSION" | sed 's/\([0-9]*[.][0-9]*\).*/\1/')
	# Add a .0 if we only got one number
	if [[ "${CURRENT_CORESIMULATOR_VERSION/./}" == "${CURRENT_CORESIMULATOR_VERSION}" ]]; then
		CURRENT_CORESIMULATOR_VERSION=$CURRENT_CORESIMULATOR_VERSION.0
	fi
	if [[ "${TARGET_CORESIMULATOR_VERSION/./}" == "${TARGET_CORESIMULATOR_VERSION}" ]]; then
		TARGET_CORESIMULATOR_VERSION=$TARGET_CORESIMULATOR_VERSION.0
	fi

	# Compare versions to see if we got what we need
	if [[ x"$TARGET_CORESIMULATOR_VERSION" == x"$CURRENT_CORESIMULATOR_VERSION" ]]; then
		log "Found CoreSimulator.framework $CURRENT_CORESIMULATOR_VERSION (exactly $TARGET_CORESIMULATOR_VERSION is recommended)"
		return
	fi

	if test -z $PROVISION_XCODE; then
		# This is not a failure for now, until this logic has been tested thoroughly
		warn "You should have exactly CoreSimulator.framework version $TARGET_CORESIMULATOR_VERSION (found $CURRENT_CORESIMULATOR_VERSION). Execute './system-dependencies.sh --provision-xcode' to install the expected version."
		return
	fi

	# Just installing the package won't work, because there's a version check somewhere
	# that prevents the macOS installer from downgrading, so remove the existing
	# CoreSimulator.framework manually first.
	log "Installing CoreSimulator.framework $CURRENT_CORESIMULATOR_VERSION..."
	$SUDO rm -Rf /Library/Developer/PrivateFrameworks/CoreSimulator.framework
	$SUDO installer -pkg "$CORESIMULATOR_PKG" -target /

	CURRENT_CORESIMULATOR_VERSION=$(otool -L $CURRENT_CORESIMULATOR_PATH | grep "$CURRENT_CORESIMULATOR_PATH.*current version" | sed -e 's/.*current version//' -e 's/)//' -e 's/[[:space:]]//g')
	log "Installed CoreSimulator.framework $CURRENT_CORESIMULATOR_VERSION successfully."
}

function check_specific_xcode () {
	local XCODE_DEVELOPER_ROOT
	local XCODE_VERSION
	local XCODE_ROOT
	local INSTALLER="$PWD/tools/devops/automation/scripts/bash/install-xcode.sh"
	local INSTALLER_ARGS=(verify --quiet)

	XCODE_DEVELOPER_ROOT=$(get_xcode_developer_root "$1")
	XCODE_VERSION=$(grep "XCODE$1_VERSION=" Make.config | sed 's/.*=//')
	XCODE_ROOT=$(dirname "$(dirname "$XCODE_DEVELOPER_ROOT")")

	if ! "$INSTALLER" "${INSTALLER_ARGS[@]}"; then
		if ! test -z $PROVISION_XCODE; then
			install_specific_xcode "$1" "$XCODE_DEVELOPER_ROOT"
		else
			# The probe above is quiet so that the common "not installed yet" case doesn't
			# look like an error; repeat it verbosely so the actual reason is visible.
			"$INSTALLER" verify || true
			fail "You must install Xcode ($XCODE_VERSION) in $XCODE_ROOT. You can download Xcode $XCODE_VERSION here: https://developer.apple.com/downloads/index.action?name=Xcode"
			return
		fi
	fi

	if ! test -z $PROVISION_XCODE; then
		INSTALLER_ARGS=(reconcile)
		if test -z "$SUDO"; then
			INSTALLER_ARGS+=(--no-sudo)
		fi
		"$INSTALLER" "${INSTALLER_ARGS[@]}"
	elif ! "$XCODE_DEVELOPER_ROOT/usr/bin/xcodebuild" -license check >/dev/null 2>&1; then
		fail "The license for Xcode $XCODE_VERSION has not been accepted. Execute '$SUDO $XCODE_DEVELOPER_ROOT/usr/bin/xcodebuild' to review the license and accept it."
		return
	fi

	run_xcode_first_launch "$XCODE_VERSION" "$XCODE_DEVELOPER_ROOT"
	ok "Found Xcode $XCODE_VERSION in $XCODE_ROOT"
}

function check_xcode () {
	if ! test -z $IGNORE_XCODE; then return; fi

	# must have latest Xcode in /Applications/Xcode<version>.app
	check_specific_xcode ""
	install_coresimulator

	local IOS_SDK_VERSION MACOS_SDK_VERSION TVOS_SDK_VERSION
	local XCODE_DEVELOPER_ROOT

	XCODE_DEVELOPER_ROOT=$(get_xcode_developer_root)
	IOS_SDK_VERSION=$(grep ^IOS_NUGET_OS_VERSION= Make.versions | sed -e 's/.*=//')
	MACOS_SDK_VERSION=$(grep ^MACOS_NUGET_OS_VERSION= Make.versions | sed -e 's/.*=//')
	TVOS_SDK_VERSION=$(grep ^TVOS_NUGET_OS_VERSION= Make.versions | sed -e 's/.*=//')

	download_xcode_platforms "$XCODE_DEVELOPER_ROOT" "$TVOS_SDK_VERSION"

	local D=$XCODE_DEVELOPER_ROOT/Platforms/iPhoneSimulator.platform/Developer/SDKs/iPhoneSimulator${IOS_SDK_VERSION}.sdk
	if test ! -d $D -a -z "$FAIL"; then
		fail "The directory $D does not exist. If you've updated the Xcode location it means you also need to update IOS_SDK_VERSION in Make.config."
	fi

	local D=$XCODE_DEVELOPER_ROOT/Platforms/MacOSX.platform/Developer/SDKs/MacOSX${MACOS_SDK_VERSION}.sdk
	if test ! -d $D -a -z "$FAIL"; then
		fail "The directory $D does not exist. If you've updated the Xcode location it means you also need to update MACOS_SDK_VERSION in Make.config."
	fi

	local D=$XCODE_DEVELOPER_ROOT/Platforms/AppleTVOS.platform/Developer/SDKs/AppleTVOS${TVOS_SDK_VERSION}.sdk
	if test ! -d $D -a -z "$FAIL"; then
		fail "The directory $D does not exist. If you've updated the Xcode location it means you also need to update TVOS_SDK_VERSION in Make.config."
	fi
}

function check_xcode_components ()
{
	if ! test -z "$IGNORE_XCODE_COMPONENTS"; then return; fi

	local COMPONENTS=(MetalToolchain)

	for comp in "${COMPONENTS[@]}"; do
		componentInfo=$(xcrun xcodebuild -showComponent "$comp")
		local NEEDS_INSTALL=
		local NEEDS_UPDATE=
		if [[ "$componentInfo" =~ .*Status:" "installedUpdateAvailable.* ]]; then
			NEEDS_UPDATE=1
		elif [[ "$componentInfo" =~ .*Status:" "installed.* ]]; then
			NEEDS_INSTALL=
		else
			NEEDS_INSTALL=1
		fi

		if test -z "$NEEDS_INSTALL$NEEDS_UPDATE"; then
			ok "The Xcode component ${COLOR_BLUE}$comp${COLOR_CLEAR} is installed."
		elif test -z "$PROVISION_XCODE_COMPONENTS"; then
			if test -n "$NEEDS_UPDATE"; then
				fail "The Xcode component ${COLOR_BLUE}$comp${COLOR_RESET} is installed, but an update is available. Execute ${COLOR_MAGENTA}xcrun xcodebuild -downloadComponent $comp${COLOR_RESET} or ${COLOR_MAGENTA}./system-dependencies.sh --provision-xcode-components${COLOR_RESET} to install."
			else
				fail "The Xcode component ${COLOR_BLUE}$comp${COLOR_RESET} is not installed. Execute ${COLOR_MAGENTA}xcrun xcodebuild -downloadComponent $comp${COLOR_RESET} or ${COLOR_MAGENTA}./system-dependencies.sh --provision-xcode-components${COLOR_RESET} to install."
			fi
			fail "Alternatively you can ${COLOR_MAGENTA}export IGNORE_XCODE_COMPONENTS=1${COLOR_RED} to skip this check."
		elif test -n "$PROVISION_XCODE_COMPONENTS"; then
			log "Installing the Xcode component ${COLOR_BLUE}$comp${COLOR_CLEAR} by executing ${COLOR_BLUE}xcrun xcodebuild -downloadComponent $comp${COLOR_CLEAR}..."
			xcrun xcodebuild -downloadComponent "$comp"

			ok "Successfully installed the Xcode component ${COLOR_BLUE}$comp${COLOR_CLEAR}."
		fi
	done

	log "Clearing xcrun cache..."
	xcrun -k
}

function install_shellcheck () {
	if ! brew --version >& /dev/null; then
		fail "Asked to install shellcheck, but brew is not installed."
		return
	fi

	ok "Installing ${COLOR_BLUE}shellcheck${COLOR_RESET}..."
	brew install shellcheck
}

function install_yamllint () {
	if ! brew --version >& /dev/null; then
		fail "Asked to install yamllint, but brew is not installed."
		return
	fi

	ok "Installing ${COLOR_BLUE}yamllint${COLOR_RESET}..."
	brew install yamllint
}

function install_python3 () {
	if ! brew --version >& /dev/null; then
		fail "Asked to install python3, but brew is not installed."
		return
	fi

	ok "Installing ${COLOR_BLUE}python3${COLOR_RESET}..."
	brew install python3
}

function check_shellcheck () {
	if ! test -z $IGNORE_SHELLCHECK; then return; fi

IFStmp=$IFS
IFS='
'
	if SHELLCHECK_VERSION=($(shellcheck --version 2>/dev/null)); then
		ok "Found shellcheck ${SHELLCHECK_VERSION[1]} (no specific version is required)"
	elif ! test -z $PROVISION_SHELLCHECK; then
		install_shellcheck
	else
		fail "You must install shellcheck. The easiest way is to use homebrew, and execute ${COLOR_MAGENTA}brew install shellcheck${COLOR_RESET}."
	fi

IFS=$IFS_tmp
}

function check_yamllint () {
	if ! test -z $IGNORE_YAMLLINT; then return; fi

IFStmp=$IFS
IFS='
'
	if YAMLLINT_VERSION=($(yamllint --version 2>/dev/null)); then
		ok "Found ${YAMLLINT_VERSION[0]} (no specific version is required)"
	elif ! test -z $PROVISION_YAMLLINT; then
		install_yamllint
	else
		fail "You must install yamllint. The easiest way is to use homebrew, and execute ${COLOR_MAGENTA}brew install yamllint${COLOR_RESET}."
	fi

IFS=$IFS_tmp
}

function check_python3 () {
	if ! test -z $IGNORE_PYTHON3; then return; fi

IFStmp=$IFS
IFS='
'
	if PYTHON3_VERSION=$(python3 --version 2>/dev/null); then
		ok "Found $PYTHON3_VERSION (no specific version is required)"
	elif ! test -z $PROVISION_PYTHON3; then
		install_python3
	else
		fail "You must install python3. The easiest way is to use homebrew, and execute ${COLOR_MAGENTA}brew install python3${COLOR_RESET}."
	fi

IFS=$IFS_tmp
}

function check_osx_version () {
	if ! test -z $IGNORE_OSX; then return; fi

	MIN_OSX_BUILD_VERSION=`grep MIN_OSX_BUILD_VERSION= Make.config | sed 's/.*=//'`

	ACTUAL_OSX_VERSION=$(sw_vers -productVersion)
	if ! is_at_least_version $ACTUAL_OSX_VERSION $MIN_OSX_BUILD_VERSION; then
		fail "You must have at least OSX $MIN_OSX_BUILD_VERSION (found $ACTUAL_OSX_VERSION)"
		return
	fi

	ok "Found OSX $ACTUAL_OSX_VERSION (at least $MIN_OSX_BUILD_VERSION is required)"
}

function check_checkout_dir () {
	# Skip without Xcode - this check is macOS-specific
	if test -n "$NO_XCODE"; then
		return
	fi
	
	# use apple script to get the possibly translated special folders and check that we are not a subdir
	for special in documents downloads desktop; do
		path=$(osascript -e "set result to POSIX path of (path to $special folder as string)")
		if [[ $PWD == $path* ]]; then
			fail "Your checkout is under $path which is a special path. This can result in problems running the tests."
		fi
	done
	ok "Checkout location will not result in test problems."
}

function install_7z () {
	if ! brew --version >& /dev/null; then
		fail "Asked to install 7z, but brew is not installed."
		return
	fi

	brew install p7zip
}

function check_7z () {
	if ! test -z $IGNORE_7Z; then return; fi


	if ! 7z &> /dev/null; then
		if ! test -z $PROVISION_7Z; then
			install_7z
		else
			fail "You must install 7z (no specific version is required)"
		fi
		return
	fi

	ok "Found 7z (no specific version is required)"
}

function check_homebrew ()
{
	if ! test -z $IGNORE_HOMEBREW; then return; fi

IFStmp=$IFS
IFS='
'
	if HOMEBREW_VERSION=($(brew --version 2>/dev/null)); then
		ok "Found Homebrew ($HOMEBREW_VERSION)"
	elif ! test -z $PROVISION_HOMEBREW; then
		log "Installing Homebrew..."
		/bin/bash -c "$(curl -fsSL https://raw.githubusercontent.com/Homebrew/install/master/install.sh)"	
		HOMEBREW_VERSION=($(brew --version 2>/dev/null))
		log "Installed Homebrew ($HOMEBREW_VERSION)"
	else
		warn "Could not find Homebrew. Homebrew is required to auto-provision some dependencies (p7zip), but not required otherwise."
	fi
IFS=$IFS_tmp
}

# Download and install a simulator runtime from Apple's downloadable simulator index.
#
# This is the same index Xcode's UI uses to download simulators, and we need it because
# 'xcodebuild -downloadPlatform' can't download the older simulators anymore with Xcode 27+
# (Apple removed them from the platform catalog xcodebuild uses, but they're still available
# in this index).
#
# $1: the simulator's os (iOS, tvOS, ...)
# $2: the simulator's version (16.0, ...)
function install_old_simulator_from_index ()
{
	local os="$1"
	local version="$2"

	local platform
	local dldir
	local index_url
	local query
	local source
	local expectedSize
	local path
	local dmg
	local actualSize

	case "$os" in
		iOS)             platform=com.apple.platform.iphoneos ;;
		tvOS)            platform=com.apple.platform.appletvos ;;
		watchOS)         platform=com.apple.platform.watchos ;;
		xrOS | visionOS) platform=com.apple.platform.xros ;;
		*)
			warn "Don't know the platform identifier for the $os simulator."
			return 1
			;;
	esac

	dldir="$SD_TMP_DIR/$os-$version-runtime"
	rm -rf -- "$dldir"
	mkdir -p "$dldir"

	# This is the index Xcode uses to find downloadable simulator runtimes.
	index_url=https://devimages-cdn.apple.com/downloads/xcode/simulators/index2.dvtdownloadableindex
	log "Downloading the simulator runtime index from $index_url..."
	if ! curl --fail --location --silent --show-error --max-time 120 "$index_url" --output "$dldir/index.plist"; then
		warn "Failed to download the simulator runtime index."
		return 1
	fi
	plutil -convert json -o "$dldir/index.json" "$dldir/index.plist"

	# Find the download url (and expected file size) for the requested simulator in the index.
	query=".downloadables[] | select(.platform == \"$platform\" and .simulatorVersion.version == \"$version\")"
	source=$(jq -r "first($query) | .source // empty" "$dldir/index.json")
	if test -z "$source"; then
		warn "Could not find the $os $version simulator in the downloadable simulator index."
		return 1
	fi
	expectedSize=$(jq -r "first($query) | .fileSize // empty" "$dldir/index.json")

	# The runtime dmgs require a download authorization cookie. No account is needed, but Apple's
	# cdn rejects requests without the cookie, and requesting the download path from developerservices2
	# hands out the cookie without requiring any authentication.
	path=$(printf '%s' "$source" | sed -E 's,^https?://[^/]+,,')
	log "Fetching a download authorization cookie..."
	if ! curl --fail --location --silent --show-error --max-time 60 --cookie-jar "$dldir/cookies.txt" "https://developerservices2.apple.com/services/download?path=$path" --output /dev/null; then
		warn "Failed to fetch a download authorization cookie for the $os $version simulator."
		return 1
	fi

	dmg="$dldir/$(basename "$source")"
	log "Downloading the $os $version simulator runtime from $source..."
	if ! curl --fail --location --silent --show-error --cookie "$dldir/cookies.txt" "$source" --output "$dmg"; then
		warn "Failed to download the $os $version simulator runtime."
		return 1
	fi

	# Sanity check the size of the downloaded file (in case we got an error page instead of the dmg).
	if test -n "$expectedSize"; then
		actualSize=$(stat -f '%z' "$dmg")
		if [[ "$actualSize" != "$expectedSize" ]]; then
			warn "The downloaded $os $version simulator runtime has an unexpected size (expected $expectedSize bytes, got $actualSize bytes)."
			return 1
		fi
	fi

	log "Installing the $os $version simulator runtime..."
	if ! xcrun simctl runtime add "$dmg" 2>&1 | sed 's/^/        /'; then
		warn "Failed to install the $os $version simulator runtime."
		return 1
	fi

	rm -rf -- "$dldir"
	return 0
}

function check_old_simulators ()
{
	if test -n "$IGNORE_OLD_SIMULATORS"; then return; fi

	local EXTRA_SIMULATORS
	local XCODE
	local XCODE_DEVELOPER_ROOT

	XCODE_DEVELOPER_ROOT=$(get_xcode_developer_root "$1")

	IFS=' ' read -r -a EXTRA_SIMULATORS <<< "$(grep ^EXTRA_SIMULATORS= Make.config | sed 's/.*=//')"
	XCODE=$(dirname "$(dirname "$XCODE_DEVELOPER_ROOT")")

	if ! test -d "$XCODE"; then
		# can't test unless Xcode is present
		warn "Can't check if simulators are available unless Xcode is already installed."
		return
	fi

	SD_TMP_DIR=$(mktemp -d /tmp/system-dependencies.XXXXXX)
	trap 'rm -rf -- "$SD_TMP_DIR"' EXIT
	TMP_FILE=$SD_TMP_DIR/simulator-runtimes.json
	xcrun simctl list runtimes --json --json-output "$TMP_FILE"

	local action=warn
	if test -z $OPTIONAL_OLD_SIMULATORS; then
		action=fail
	fi

	local os
	local versionName
	local version

	for spec in "${EXTRA_SIMULATORS[@]}"; do
		os=${spec/:*/}
		versionName=${spec/*:/}
		version=$(grep "^${versionName}=" Make.config | sed 's/.*=//')

		OS_TMP_FILE=$SD_TMP_DIR/$os-$version.json
		jq ".runtimes[] | select(.platform == \"$os\" and .version == \"$version\" and .isAvailable == true and .isInternal == false) | [ { \"version\":.version, \"name\":.name, \"identifier\":.identifier } ]" < "$TMP_FILE" > "$OS_TMP_FILE"
		#echo $OS_TMP_FILE
		LENGTH=$(jq length < "$OS_TMP_FILE")
		if [[ "$LENGTH" != "" && "$LENGTH" -gt 0 ]]; then
			ok "Found the $os $version simulator."
		elif test -z "$PROVISION_OLD_SIMULATORS"; then
			$action "The $os $version simulator is not installed. Execute ${COLOR_MAGENTA}xcodebuild -downloadPlatform $os -buildVersion $version${COLOR_RESET} to install."
		else
			warn "The $os $version simulator is not installed. Now executing ${COLOR_BLUE}"$XCODE_DEVELOPER_ROOT/usr/bin/xcodebuild" -downloadPlatform $os -buildVersion $version${COLOR_RESET} to install..."
			if "$XCODE_DEVELOPER_ROOT/usr/bin/xcodebuild" -downloadPlatform "$os" -buildVersion "$version" 2>&1 | sed 's/^/        /'; then
				warn "Successfully executed ${COLOR_BLUE}"$XCODE_DEVELOPER_ROOT/usr/bin/xcodebuild" -downloadPlatform $os -buildVersion $version${COLOR_RESET}."
			else
				# Starting with Xcode 27 'xcodebuild -downloadPlatform' can't download the old simulators
				# anymore (Apple removed them from the platform catalog xcodebuild uses), so fall back to
				# downloading the runtime directly from Apple's downloadable simulator index (the same index
				# Xcode's UI uses, which still has the old simulators).
				warn "Failed to install the $os $version simulator using xcodebuild; falling back to Apple's downloadable simulator index..."
				if install_old_simulator_from_index "$os" "$version"; then
					ok "Successfully installed the $os $version simulator from the downloadable simulator index."
				else
					$action "The $os $version simulator is not installed, and it couldn't be downloaded."
				fi
			fi
		fi
	done
}

echo "Checking system..."

if test -n "$NO_XCODE"; then
	ok "No Xcode available - skipping Xcode-specific checks"
	ok "Only .NET download and managed code builds will be available"
fi

check_osx_version
check_checkout_dir
check_xcode
check_xcode_components
check_homebrew
check_shellcheck
check_yamllint
check_python3
check_7z
check_old_simulators
if test -z "$IGNORE_DOTNET"; then
	if test -f /usr/local/share/dotnet/dotnet; then
		ok "Installed .NET SDKs:"
		(IFS=$'\n'; for i in $(/usr/local/share/dotnet/dotnet --list-sdks); do log "$i"; done)
	else
		warn ".NET is not installed"
	fi
fi

if test -z $FAIL; then
	echo "System check succeeded"
else
	echo "System check failed"
	exit 1
fi
