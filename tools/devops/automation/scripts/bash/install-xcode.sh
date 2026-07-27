#!/bin/bash

set -euo pipefail

SCRIPT_DIR=$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)
TOP=$(cd "$SCRIPT_DIR/../../../../.." && pwd)

COMMAND=
PACKAGE_DIRECTORY=
ARCHIVE=
QUIET=
SUDO=sudo

usage ()
{
	cat <<EOF
Usage:
  $0 verify [--quiet]
  $0 verify-package --package-directory <directory>
  $0 reconcile [--no-sudo]
  $0 install [--package-directory <directory> | --archive <xip>] [--no-sudo]
EOF
}

while [[ $# -gt 0 ]]; do
	case "$1" in
	verify | verify-package | reconcile | install)
		if [[ -n "$COMMAND" ]]; then
			echo "Only one command may be specified." >&2
			exit 1
		fi
		COMMAND=$1
		shift
		;;
	--package-directory)
		if [[ $# -lt 2 || -z "$2" ]]; then
			echo "--package-directory requires a value." >&2
			exit 1
		fi
		PACKAGE_DIRECTORY=$2
		shift 2
		;;
	--archive)
		if [[ $# -lt 2 || -z "$2" ]]; then
			echo "--archive requires a value." >&2
			exit 1
		fi
		ARCHIVE=$2
		shift 2
		;;
	--no-sudo)
		SUDO=
		shift
		;;
	--quiet)
		QUIET=1
		shift
		;;
	-h | --help)
		usage
		exit 0
		;;
	*)
		echo "Unknown argument: $1" >&2
		usage >&2
		exit 1
		;;
	esac
done

if [[ -z "$COMMAND" ]]; then
	usage >&2
	exit 1
fi

log ()
{
	if [[ -z "$QUIET" ]]; then
		echo "$@"
	fi
}

error ()
{
	if [[ -z "$QUIET" ]]; then
		echo "$@" >&2
	fi
}

run_privileged ()
{
	if [[ -n "$SUDO" ]]; then
		"$SUDO" "$@"
	else
		"$@"
	fi
}

read_config_value ()
{
	local name=$1
	local assignment

	assignment=$(grep -E "^${name}[?:]?=" "$TOP/Make.config" | head -n 1 || true)
	echo "${assignment#*=}"
}

normalize_numeric_version ()
{
	local version=${1%%-*}
	local major minor patch extra

	IFS=. read -r major minor patch extra <<< "$version"
	if [[ -z "$major" || -z "$minor" || -n "$extra" || ! "$major" =~ ^[0-9]+$ || ! "$minor" =~ ^[0-9]+$ || ( -n "$patch" && ! "$patch" =~ ^[0-9]+$ ) ]]; then
		return 1
	fi

	echo "$((10#$major)).$((10#$minor)).$((10#${patch:-0}))"
}

version_is_at_least ()
{
	local actual required
	local actual_major actual_minor actual_patch
	local required_major required_minor required_patch

	actual=$(normalize_numeric_version "$1")
	required=$(normalize_numeric_version "$2")
	IFS=. read -r actual_major actual_minor actual_patch <<< "$actual"
	IFS=. read -r required_major required_minor required_patch <<< "$required"

	if (( actual_major != required_major )); then
		(( actual_major > required_major ))
	elif (( actual_minor != required_minor )); then
		(( actual_minor > required_minor ))
	else
		(( actual_patch >= required_patch ))
	fi
}

canonicalize_directory ()
{
	(
		cd "$1"
		pwd -P
	)
}

plist_value ()
{
	/usr/libexec/PlistBuddy -c "Print :$2" "$1"
}

binary_architectures ()
{
	local path=$1
	local description
	local architectures

	# Deliberately avoid lipo/otool: those are xcrun shims that fail whenever xcode-select
	# points at a missing developer directory, which is exactly the state this script has
	# to be able to repair. /usr/bin/file is a standalone tool and reports the same slices.
	description=$(/usr/bin/env -u DEVELOPER_DIR -u XCODE_DEVELOPER_ROOT /usr/bin/file -b "$path" 2>/dev/null || true)

	# For a universal binary, only trust the per-architecture lines: the summary line
	# abbreviates any slice 'file' couldn't reach in its read buffer, so a large binary
	# reports '[x86_64:Mach-O ...] [arm64e]' and the abbreviated slices would be lost.
	architectures=$(sed -n 's/.*(for architecture \([^)]*\)).*/\1/p' <<< "$description" | tr '\n' ' ')
	if [[ -n "$architectures" ]]; then
		echo "$architectures"
		return
	fi

	case "$description" in
	Mach-O*)
		awk 'NR == 1 { print $NF }' <<< "$description"
		;;
	esac
}

XCODE_VERSION=$(read_config_value XCODE_VERSION)
# TEMPORARY: set to a non-empty value to reuse an Xcode installed under another name.
# Left empty on purpose so CI exercises the Universal Package download and install.
XCODE_REUSE_INSTALLED=
XCODE_PACKAGE_NAME=$(read_config_value XCODE_PACKAGE_NAME)
XCODE_PACKAGE_VERSION=$(read_config_value XCODE_PACKAGE_VERSION)
XCODE_BUILD_VERSION=$(read_config_value XCODE_BUILD_VERSION)
CONFIGURED_XCODE_DEVELOPER_ROOT=${XCODE_DEVELOPER_ROOT:-$(read_config_value XCODE_DEVELOPER_ROOT)}

if [[ -z "$XCODE_VERSION" || -z "$XCODE_PACKAGE_NAME" || -z "$XCODE_PACKAGE_VERSION" || -z "$XCODE_BUILD_VERSION" || -z "$CONFIGURED_XCODE_DEVELOPER_ROOT" ]]; then
	error "Make.config must define XCODE_VERSION, XCODE_PACKAGE_NAME, XCODE_PACKAGE_VERSION, XCODE_BUILD_VERSION, and XCODE_DEVELOPER_ROOT."
	exit 1
fi

if [[ "$CONFIGURED_XCODE_DEVELOPER_ROOT" == *"\$("* ]]; then
	error "XCODE_DEVELOPER_ROOT must resolve to a concrete path: '$CONFIGURED_XCODE_DEVELOPER_ROOT'."
	exit 1
fi

EXPECTED_NORMALIZED_VERSION=$(normalize_numeric_version "$XCODE_PACKAGE_VERSION")
CONFIGURED_NORMALIZED_VERSION=$(normalize_numeric_version "$XCODE_VERSION")
# XCODE_VERSION carries only a major and a minor version, while the package version also
# carries the patch component: the repository ships e.g. XCODE_VERSION=26.4 together with
# Xcode_26.4.1.app. Only the major and minor versions can be required to match here; the
# full package version is still matched exactly against the manifest and the installed
# bundle further down.
if [[ "${CONFIGURED_NORMALIZED_VERSION%.*}" != "${EXPECTED_NORMALIZED_VERSION%.*}" ]]; then
	error "XCODE_VERSION '$XCODE_VERSION' and XCODE_PACKAGE_VERSION '$XCODE_PACKAGE_VERSION' do not identify the same Xcode release."
	exit 1
fi

EXPECTED_APP_NAME="Xcode_${XCODE_PACKAGE_VERSION/beta./beta}.app"

# Make.config has to stay internally consistent: the pinned package version determines the
# application name, so XCODE_DEVELOPER_ROOT must point at that same application. Without
# this check a mismatch only shows up as an install that can never verify.
MAKE_CONFIG_XCODE_APP=$(basename "$(dirname "$(dirname "$(read_config_value XCODE_DEVELOPER_ROOT)")")")
if [[ "$MAKE_CONFIG_XCODE_APP" != "$EXPECTED_APP_NAME" ]]; then
	error "XCODE_DEVELOPER_ROOT in Make.config points at '$MAKE_CONFIG_XCODE_APP', but XCODE_PACKAGE_VERSION '$XCODE_PACKAGE_VERSION' expects '$EXPECTED_APP_NAME'."
	exit 1
fi

# The literal path Make.config names, before any symlink resolution. Privileged install
# operations always target this: canonicalizing first would let an install overwrite a
# bundle we had merely borrowed through the compatibility symlink below.
LITERAL_XCODE_APP=$(dirname "$(dirname "$CONFIGURED_XCODE_DEVELOPER_ROOT")")

set_xcode_target ()
{
	XCODE_APP=$1
	XCODE_DEVELOPER_ROOT="$XCODE_APP/Contents/Developer"
	XCODE_INSTALL_PARENT=$(dirname "$XCODE_APP")
	RECONCILIATION_MARKER="$XCODE_APP.macios-reconcile-pending"
}

if [[ "$COMMAND" == "install" ]]; then
	set_xcode_target "$LITERAL_XCODE_APP"
elif [[ -d "$LITERAL_XCODE_APP" ]]; then
	set_xcode_target "$(canonicalize_directory "$LITERAL_XCODE_APP")"
else
	set_xcode_target "$LITERAL_XCODE_APP"
fi
if [[ "$COMMAND" == "install" && ( "$XCODE_APP" != /* || "$XCODE_APP" != *.app ) ]]; then
	error "The Xcode installation target must be an absolute application path: '$XCODE_APP'."
	exit 1
fi

verify_xcode_bundle ()
{
	local app=$1
	local info_plist="$app/Contents/Info.plist"
	local version_plist="$app/Contents/version.plist"
	local bundle_identifier executable_name executable_path
	local actual_version actual_build minimum_os current_os architectures
	local signature_details

	if [[ ! -d "$app" || -L "$app" ]]; then
		error "The expected Xcode application is not a real directory: '$app'."
		return 1
	fi
	if [[ ! -f "$info_plist" || ! -f "$version_plist" ]]; then
		error "The Xcode bundle is missing Info.plist or version.plist: '$app'."
		return 1
	fi

	bundle_identifier=$(plist_value "$info_plist" CFBundleIdentifier 2>/dev/null || true)
	if [[ "$bundle_identifier" != "com.apple.dt.Xcode" ]]; then
		error "Unexpected Xcode bundle identifier '$bundle_identifier' in '$app'."
		return 1
	fi

	actual_version=$(plist_value "$version_plist" CFBundleShortVersionString 2>/dev/null || true)
	if [[ -z "$actual_version" ]] || ! actual_version=$(normalize_numeric_version "$actual_version"); then
		error "Could not read the Xcode marketing version from '$version_plist'."
		return 1
	fi
	if [[ "$actual_version" != "$EXPECTED_NORMALIZED_VERSION" ]]; then
		error "Xcode '$app' has marketing version '$actual_version'; expected '$EXPECTED_NORMALIZED_VERSION'."
		return 1
	fi

	actual_build=$(plist_value "$version_plist" ProductBuildVersion 2>/dev/null || true)
	if [[ "$actual_build" != "$XCODE_BUILD_VERSION" ]]; then
		error "Xcode '$app' has build '$actual_build'; expected '$XCODE_BUILD_VERSION'."
		return 1
	fi

	executable_name=$(plist_value "$info_plist" CFBundleExecutable 2>/dev/null || true)
	executable_path="$app/Contents/MacOS/$executable_name"
	if [[ -z "$executable_name" || ! -f "$executable_path" ]]; then
		error "The Xcode host executable does not exist: '$executable_path'."
		return 1
	fi

	architectures=$(binary_architectures "$executable_path")
	case "$(uname -m)" in
	arm64)
		if [[ " $architectures " != *" arm64 "* ]]; then
			error "Xcode '$app' cannot execute natively on arm64: '$architectures'."
			return 1
		fi
		;;
	x86_64)
		if [[ " $architectures " != *" x86_64 "* ]]; then
			error "Xcode '$app' cannot execute on x86_64: '$architectures'."
			return 1
		fi
		;;
	*)
		error "Unsupported host architecture: '$(uname -m)'."
		return 1
		;;
	esac

	minimum_os=$(plist_value "$info_plist" LSMinimumSystemVersion 2>/dev/null || true)
	current_os=$(sw_vers -productVersion)
	if [[ -z "$minimum_os" ]] || ! version_is_at_least "$current_os" "$minimum_os"; then
		error "Xcode '$app' requires macOS '$minimum_os', but this host is running '$current_os'."
		return 1
	fi

	if ! /usr/bin/codesign --verify "$app" >/dev/null 2>&1; then
		error "Code signature verification failed for '$app'."
		return 1
	fi
	if ! signature_details=$(/usr/bin/codesign -dv --verbose=4 "$app" 2>&1); then
		error "Could not inspect the code signature for '$app'."
		return 1
	fi
	if ! grep -q '^TeamIdentifier=59GAB85EFG$' <<< "$signature_details" || ! grep -q '^Authority=Apple Root CA$' <<< "$signature_details"; then
		error "Xcode '$app' is not signed by the expected Apple identity."
		return 1
	fi

	return 0
}

verify_native_apple_silicon ()
{
	local translated

	translated=$(sysctl -n sysctl.proc_translated 2>/dev/null || echo 0)
	if [[ "$(uname -m)" != "arm64" || "$translated" == "1" ]]; then
		error "The '$XCODE_PACKAGE_NAME' package requires a native arm64 process."
		return 1
	fi
}

# Mirrors the XCODE_IS_STABLE predicate in Make.config: an application named like
# "Xcode_#.#[.#].app" is stable, and anything else is treated as a preview. Make.config
# classifies lexically, so the name we end up selecting decides how the build is labelled.
xcode_name_is_stable ()
{
	[[ "$1" =~ ^Xcode[_0-9.]*[.]app$ ]]
}

# A bot may already have exactly the Xcode we want, just under a different name than the
# one Make.config pins (say Xcode_26.2.app rather than Xcode_26.2.0.app). Find such a
# bundle so we can reuse it instead of downloading and installing another copy.
find_installed_xcode ()
{
	local candidate name
	local want_stable=
	local candidate_stable=

	# TEMPORARY: reuse is disabled so that CI exercises the Universal Package download
	# and install. Revert this commit once that path has been confirmed to work.
	if [[ -z "$XCODE_REUSE_INSTALLED" ]]; then
		return 1
	fi

	if xcode_name_is_stable "$EXPECTED_APP_NAME"; then
		want_stable=1
	fi

	# Sort deterministically and only consider names we would install ourselves, so that
	# neither a Finder duplicate ("Xcode_26.6.0 2.app") nor an arbitrary application can
	# win over the configured bundle. ./configure also rejects paths containing spaces.
	while IFS= read -r candidate; do
		[[ -d "$candidate" && ! -L "$candidate" ]] || continue
		name=$(basename "$candidate")
		[[ "$name" =~ ^Xcode[_0-9.]*(-[A-Za-z0-9.]+)?[.]app$ ]] || continue

		# Reusing a bundle whose name classifies differently would silently change
		# XCODE_IS_STABLE, and publishing a preview as stable is the dangerous direction.
		candidate_stable=
		if xcode_name_is_stable "$name"; then
			candidate_stable=1
		fi
		[[ "$candidate_stable" == "$want_stable" ]] || continue

		if verify_xcode_bundle "$candidate" >/dev/null 2>&1; then
			echo "$candidate"
			return 0
		fi
	done < <(find /Applications -maxdepth 1 -name 'Xcode*.app' 2>/dev/null | LC_ALL=C sort)

	return 1
}

# Point the path Make.config names at an already-installed bundle found elsewhere, so that
# everything reading Make.config -- this script, system-dependencies.sh, and plain 'make'
# in jobs that never run ./configure -- keeps working unchanged.
adopt_installed_xcode ()
{
	local discovered=$1

	if [[ -e "$LITERAL_XCODE_APP" && ! -L "$LITERAL_XCODE_APP" ]]; then
		error "'$LITERAL_XCODE_APP' already exists and is not a symlink; refusing to replace it."
		return 1
	fi
	if [[ -L "$LITERAL_XCODE_APP" ]]; then
		run_privileged rm -- "$LITERAL_XCODE_APP"
	fi
	run_privileged ln -s "$discovered" "$LITERAL_XCODE_APP"
	if [[ "$(canonicalize_directory "$LITERAL_XCODE_APP")" != "$(canonicalize_directory "$discovered")" ]]; then
		error "'$LITERAL_XCODE_APP' does not resolve to '$discovered'."
		return 1
	fi
	log "Reusing the Xcode already installed in '$discovered'."
}

# Resolve which bundle we are going to operate on. Pass "adopt" from the privileged
# commands to let a bundle found elsewhere be linked into the configured path; 'verify'
# must not mutate anything, because it is the unprivileged download-decision probe.
resolve_xcode_target ()
{
	local adopt=${1:-}
	local discovered

	if verify_xcode_bundle "$XCODE_APP" >/dev/null 2>&1; then
		return 0
	fi
	if [[ -e "$LITERAL_XCODE_APP" && ! -L "$LITERAL_XCODE_APP" ]]; then
		# Something real occupies the configured path; let the normal install replace it
		# rather than adopting a different bundle behind its back.
		return 1
	fi
	discovered=$(find_installed_xcode) || return 1
	if [[ -n "$adopt" ]]; then
		adopt_installed_xcode "$discovered" || return 1
	fi
	set_xcode_target "$discovered"
}

validate_xip_signature ()
{
	local archive=$1
	local signature

	if [[ ! -f "$archive" || -L "$archive" ]]; then
		error "The Xcode archive is not a regular file: '$archive'."
		return 1
	fi
	if ! signature=$(/usr/sbin/pkgutil --check-signature "$archive" 2>&1); then
		error "Apple package signature validation failed for '$archive'."
		return 1
	fi
	if ! grep -q '^[[:space:]]*Status: signed Apple Software$' <<< "$signature"; then
		error "The Xcode archive is not signed as Apple Software: '$archive'."
		return 1
	fi
}

clean_stale_install_paths ()
{
	local path

	for path in "$XCODE_INSTALL_PARENT"/.xcode-install.* "$XCODE_APP".backup.*; do
		if [[ ! -e "$path" && ! -L "$path" ]]; then
			continue
		fi
		case "$path" in
		"$XCODE_INSTALL_PARENT"/.xcode-install.* | "$XCODE_APP".backup.*)
			log "Removing stale Xcode installation path '$path'."
			run_privileged rm -rf -- "$path"
			;;
		*)
			error "Refusing to remove unexpected stale path '$path'."
			return 1
			;;
		esac
	done
}

validate_package ()
{
	local package_directory=$1
	local manifest="$package_directory/xcode-metadata.json"
	local entry_count artifact_name archive expected_size actual_size
	local expected_hash actual_hash manifest_version manifest_build
	local manifest_marketing_version

	if [[ ! -d "$package_directory" || -L "$package_directory" ]]; then
		error "The Xcode package directory is not a real directory: '$package_directory'."
		return 1
	fi

	entry_count=$(find "$package_directory" -mindepth 1 -maxdepth 1 | wc -l | tr -d ' ')
	if [[ "$entry_count" != "2" ]] || [[ $(find "$package_directory" -mindepth 2 | wc -l | tr -d ' ') != "0" ]]; then
		error "The Xcode Universal Package must contain exactly two files at its root."
		return 1
	fi
	if [[ ! -f "$manifest" || -L "$manifest" ]]; then
		error "The Xcode Universal Package is missing xcode-metadata.json."
		return 1
	fi
	if ! jq -e '
		.schemaVersion == 1 and
		(.package.name | type == "string") and
		(.package.version | type == "string") and
		(.xcode.marketingVersion | type == "string") and
		(.xcode.buildVersion | type == "string") and
		(.artifacts | type == "array" and length == 1) and
		(.artifacts[0].fileName | type == "string") and
		(.artifacts[0].hostVariant == "apple-silicon") and
		(.artifacts[0].hostExecutableArchitectures | index("arm64") != null) and
		(.artifacts[0].sizeBytes | type == "number" and . > 0) and
		(.artifacts[0].expandedSizeBytes | type == "number" and . > 0) and
		(.artifacts[0].sha256 | type == "string" and test("^[0-9a-f]{64}$")) and
		(.artifacts[0].appleSignatureStatus == "signed Apple Software")
	' "$manifest" >/dev/null; then
		error "The Xcode package manifest does not satisfy schema version 1."
		return 1
	fi

	if [[ "$(jq -r '.package.name' "$manifest")" != "$XCODE_PACKAGE_NAME" ]]; then
		error "The package manifest does not identify '$XCODE_PACKAGE_NAME'."
		return 1
	fi
	manifest_version=$(jq -r '.package.version' "$manifest")
	if [[ "$manifest_version" != "$XCODE_PACKAGE_VERSION" ]]; then
		error "The package manifest version '$manifest_version' does not match '$XCODE_PACKAGE_VERSION'."
		return 1
	fi
	manifest_marketing_version=$(jq -r '.xcode.marketingVersion' "$manifest")
	if [[ "$(normalize_numeric_version "$manifest_marketing_version")" != "$EXPECTED_NORMALIZED_VERSION" ]]; then
		error "The package marketing version '$manifest_marketing_version' does not match '$XCODE_VERSION'."
		return 1
	fi
	manifest_build=$(jq -r '.xcode.buildVersion' "$manifest")
	if [[ "$manifest_build" != "$XCODE_BUILD_VERSION" ]]; then
		error "The package build '$manifest_build' does not match '$XCODE_BUILD_VERSION'."
		return 1
	fi

	artifact_name=$(jq -r '.artifacts[0].fileName' "$manifest")
	if [[ "$artifact_name" == */* || "$artifact_name" == .* || ! "$artifact_name" =~ ^[A-Za-z0-9._-]+[.]xip$ ]]; then
		error "Unsafe Xcode archive name in package manifest: '$artifact_name'."
		return 1
	fi
	archive="$package_directory/$artifact_name"
	if [[ ! -f "$archive" || -L "$archive" ]]; then
		error "The package archive is not a regular file: '$archive'."
		return 1
	fi

	expected_size=$(jq -r '.artifacts[0].sizeBytes' "$manifest")
	actual_size=$(stat -f %z "$archive")
	if [[ "$actual_size" != "$expected_size" ]]; then
		error "The Xcode archive size '$actual_size' does not match '$expected_size'."
		return 1
	fi

	expected_hash=$(jq -r '.artifacts[0].sha256' "$manifest")
	actual_hash=$(shasum -a 256 "$archive" | awk '{ print tolower($1) }')
	if [[ "$actual_hash" != "$expected_hash" ]]; then
		error "The Xcode archive SHA-256 '$actual_hash' does not match '$expected_hash'."
		return 1
	fi

	validate_xip_signature "$archive"
	echo "$archive"
}

command_line_tools_label ()
{
	local numeric=${XCODE_PACKAGE_VERSION%%-*}
	local prerelease=
	local major minor patch
	local label ordinal

	if [[ "$XCODE_PACKAGE_VERSION" == *-* ]]; then
		prerelease=${XCODE_PACKAGE_VERSION#*-}
	fi
	IFS=. read -r major minor patch <<< "$numeric"
	if [[ "${patch:-0}" == "0" ]]; then
		label="$major.$minor"
	else
		label="$major.$minor.$patch"
	fi

	case "$prerelease" in
	beta*)
		ordinal=${prerelease#beta}
		ordinal=${ordinal#.}
		label="$label beta"
		if [[ -n "$ordinal" ]]; then
			label="$label $ordinal"
		fi
		;;
	rc*)
		label="$label Release Candidate"
		;;
	esac

	echo "Command Line Tools for Xcode-$label"
}

install_command_line_tools ()
{
	local label
	local catalog

	label=$(command_line_tools_label)
	# softwareupdate reports everything through its output rather than its exit status: it
	# exits 0 for an unknown label, for an invalid flag, and for a failed query, and it can
	# hang indefinitely when the update service is unreachable. macOS has no timeout(1), so
	# bound both calls with perl's alarm and decide from the catalog contents. Both streams
	# are captured because softwareupdate splits its output between them.
	catalog=$(/usr/bin/perl -e 'alarm shift; exec @ARGV or exit 127' 120 /usr/sbin/softwareupdate --list 2>&1 || true)
	if ! grep -q '^[[:space:]]*\* Label: ' <<< "$catalog"; then
		# Indistinguishable from "reachable, nothing pending", so don't claim either.
		log "Could not confirm that '$label' is available; keeping the command line tools that are already installed."
		return
	fi

	if ! sed -n 's/^[[:space:]]*\* Label: //p' <<< "$catalog" | sed 's/[[:space:]]*$//' | grep -q -x -F "$label"; then
		log "Apple does not currently offer '$label'; keeping the command line tools that are already installed."
		return
	fi

	# Best effort: the command line tools are an addition, so a failed or stalled install
	# must not take the whole build with it.
	log "Installing '$label'."
	if ! run_privileged /usr/bin/perl -e 'alarm shift; exec @ARGV or exit 127' 1800 /usr/sbin/softwareupdate -i "$label"; then
		log "Installing '$label' did not complete; keeping the command line tools that are already installed."
	fi
}

reconcile_xcode ()
{
	local force_selection_change=${1:-}
	local selected selected_canonical expected_canonical
	local selected_changed=
	local symlink=/Applications/Xcode.app
	local symlink_canonical=
	local reconciliation_pending=
	local settings

	verify_xcode_bundle "$XCODE_APP" || return 1
	if [[ -L "$RECONCILIATION_MARKER" || ( -e "$RECONCILIATION_MARKER" && ! -f "$RECONCILIATION_MARKER" ) ]]; then
		error "The Xcode reconciliation marker is not a regular file: '$RECONCILIATION_MARKER'."
		return 1
	fi
	if [[ -f "$RECONCILIATION_MARKER" ]]; then
		reconciliation_pending=1
	fi

	selected=$(xcode-select -p 2>/dev/null || true)
	expected_canonical=$(canonicalize_directory "$XCODE_DEVELOPER_ROOT")
	if [[ -n "$selected" && -d "$selected" ]]; then
		selected_canonical=$(canonicalize_directory "$selected")
	else
		selected_canonical=
	fi

	if [[ "$XCODE_APP" == "$symlink" ]]; then
		symlink_canonical=$(canonicalize_directory "$XCODE_APP")
	else
		if [[ -e "$symlink" && ! -L "$symlink" ]]; then
			error "'$symlink' is a real file or directory; refusing to replace it."
			return 1
		fi
		if [[ -L "$symlink" ]]; then
			symlink_canonical=$(canonicalize_directory "$symlink" 2>/dev/null || true)
		fi
	fi
	if [[ -n "$force_selection_change" || -n "$reconciliation_pending" || "$selected_canonical" != "$expected_canonical" || "$symlink_canonical" != "$(canonicalize_directory "$XCODE_APP")" ]]; then
		selected_changed=1
	fi
	if [[ "$XCODE_APP" != "$symlink" && "$symlink_canonical" != "$(canonicalize_directory "$XCODE_APP")" ]]; then
		if [[ -L "$symlink" ]]; then
			run_privileged rm -- "$symlink"
		fi
		run_privileged ln -s "$XCODE_APP" "$symlink"
	fi

	for settings in "$HOME/Library/Preferences/maui/Settings.plist" "$HOME/Library/Preferences/Xamarin/Settings.plist"; do
		mkdir -p "$(dirname "$settings")"
		if [[ "$(/usr/bin/defaults read "$settings" AppleSdkRoot 2>/dev/null || true)" != "$XCODE_APP" ]]; then
			/usr/bin/defaults write "$settings" AppleSdkRoot "$XCODE_APP"
		fi
	done

	if [[ -n "$selected_changed" ]]; then
		run_privileged /usr/bin/xcode-select --switch "$XCODE_APP"
		install_command_line_tools
	fi

	if ! "$XCODE_DEVELOPER_ROOT/usr/bin/xcodebuild" -license check >/dev/null 2>&1; then
		run_privileged "$XCODE_DEVELOPER_ROOT/usr/bin/xcodebuild" -license accept
	fi

	if [[ -n "$selected_changed" ]]; then
		run_privileged rm -rf -- \
			/Library/Developer/PrivateFrameworks/CoreSimulator.framework \
			/Library/Developer/PrivateFrameworks/ROCKit.framework
		/usr/bin/env -u DEVELOPER_DIR -u XCODE_DEVELOPER_ROOT /usr/bin/xcrun -k
	fi

	if [[ -n "$selected_changed" ]]; then
		run_privileged "$XCODE_DEVELOPER_ROOT/usr/bin/xcodebuild" -runFirstLaunch
	elif ! "$XCODE_DEVELOPER_ROOT/usr/bin/xcodebuild" -checkFirstLaunchStatus >/dev/null 2>&1; then
		run_privileged "$XCODE_DEVELOPER_ROOT/usr/bin/xcodebuild" -runFirstLaunch
	fi

	verify_xcode_bundle "$XCODE_APP"
	selected=$(xcode-select -p)
	if [[ "$(canonicalize_directory "$selected")" != "$expected_canonical" ]]; then
		error "xcode-select does not point to '$XCODE_DEVELOPER_ROOT'."
		return 1
	fi
	if [[ "$XCODE_APP" != "$symlink" && "$(canonicalize_directory "$symlink")" != "$(canonicalize_directory "$XCODE_APP")" ]]; then
		error "'$symlink' does not point to '$XCODE_APP'."
		return 1
	fi
	for settings in "$HOME/Library/Preferences/maui/Settings.plist" "$HOME/Library/Preferences/Xamarin/Settings.plist"; do
		if [[ "$(/usr/bin/defaults read "$settings" AppleSdkRoot 2>/dev/null || true)" != "$XCODE_APP" ]]; then
			error "'$settings' does not select '$XCODE_APP'."
			return 1
		fi
	done
	if ! "$XCODE_DEVELOPER_ROOT/usr/bin/xcodebuild" -license check >/dev/null 2>&1; then
		error "The Xcode license is not accepted."
		return 1
	fi
	if ! "$XCODE_DEVELOPER_ROOT/usr/bin/xcodebuild" -checkFirstLaunchStatus >/dev/null 2>&1; then
		error "Xcode still has pending first-launch work."
		return 1
	fi
	run_privileged rm -f -- "$RECONCILIATION_MARKER"
}

install_archive ()
(
	local archive=$1
	local expanded_size=${2:-0}
	local available_bytes required_bytes
	local staging_root payload_root extractor expanded_app
	local expanded=
	local backup=
	local target_installed=
	local target_displaced=
	local install_committed=

	# The xip fallback below runs from inside the staging directory.
	if [[ "$archive" != /* ]]; then
		archive="$PWD/$archive"
	fi

	validate_xip_signature "$archive"

	if [[ "$expanded_size" -gt 0 ]]; then
		available_bytes=$(( $(df -Pk "$XCODE_INSTALL_PARENT" | awk 'NR == 2 { print $4 }') * 1024 ))
		required_bytes=$(( expanded_size + 5 * 1024 * 1024 * 1024 ))
		if (( available_bytes < required_bytes )); then
			error "Installing Xcode requires at least '$required_bytes' free bytes on '$XCODE_INSTALL_PARENT'; only '$available_bytes' are available."
			exit 1
		fi
	fi

	staging_root=$(run_privileged /usr/bin/mktemp -d "$XCODE_INSTALL_PARENT/.xcode-install.XXXXXX")
	run_privileged chown "$(id -u):$(id -g)" "$staging_root"
	payload_root="$staging_root/payload"
	extractor="$staging_root/packagekit-extract"

	# The trap handler below is only reached through 'trap', which shellcheck can't see
	# from inside this subshell function. Older shellchecks report that as SC2317 and
	# newer ones as SC2329, and CI doesn't pin a version, so silence both.
	# shellcheck disable=SC2317,SC2329
	cleanup ()
	{
		local status=$?
		trap - EXIT

		if [[ $status -ne 0 && -z "$install_committed" ]]; then
			if [[ -n "$target_installed" && ( -e "$XCODE_APP" || -L "$XCODE_APP" ) ]]; then
				run_privileged rm -rf -- "$XCODE_APP"
			fi
			if [[ -n "$target_displaced" && -n "$backup" && ( -e "$backup" || -L "$backup" ) ]]; then
				run_privileged mv "$backup" "$XCODE_APP"
			fi
		fi
		if [[ -n "$staging_root" && -d "$staging_root" ]]; then
			run_privileged rm -rf -- "$staging_root"
		fi
		exit "$status"
	}
	trap cleanup EXIT

	# PackageKit is a private framework and packagekit-extract.m declares its interface by
	# hand, so an OS update can break it at runtime even though it still compiles. Fall back
	# to the supported 'xip --expand' rather than blocking every Xcode rollout. Both paths
	# refuse archives that aren't Apple-signed, and validate_xip_signature already ran.
	expanded=
	log "Building the signed-XIP extractor."
	if /usr/bin/env -u DEVELOPER_DIR -u XCODE_DEVELOPER_ROOT /usr/bin/clang \
		"$SCRIPT_DIR/packagekit-extract.m" \
		-o "$extractor" \
		-arch "$(uname -m)" \
		-Wall \
		-Werror \
		-fmodules \
		-framework PackageKit \
		-F /System/Library/PrivateFrameworks; then
		log "Expanding '$(basename "$archive")' on the /Applications volume."
		if "$extractor" "$archive" "$payload_root"; then
			expanded=1
		else
			error "The PackageKit extractor failed; falling back to 'xip --expand'."
		fi
	else
		error "The PackageKit extractor did not build; falling back to 'xip --expand'."
	fi

	if [[ -z "$expanded" ]]; then
		rm -rf -- "$payload_root"
		mkdir -p "$payload_root"
		log "Expanding '$(basename "$archive")' with xip."
		(
			cd "$payload_root"
			/usr/bin/xip --expand "$archive"
		)
	fi

	if [[ $(find "$payload_root" -mindepth 1 -maxdepth 1 | wc -l | tr -d ' ') != "1" ]] ||
		[[ $(find "$payload_root" -mindepth 1 -maxdepth 1 -type d -name '*.app' | wc -l | tr -d ' ') != "1" ]]; then
		error "The Xcode archive did not expand to exactly one application bundle."
		exit 1
	fi
	expanded_app=$(find "$payload_root" -mindepth 1 -maxdepth 1 -type d -name '*.app' -print)
	verify_xcode_bundle "$expanded_app"

	if [[ -e "$XCODE_APP" || -L "$XCODE_APP" ]]; then
		backup="$XCODE_APP.backup.$$"
		if [[ -e "$backup" || -L "$backup" ]]; then
			error "The Xcode backup path already exists: '$backup'."
			exit 1
		fi
		run_privileged mv "$XCODE_APP" "$backup"
		target_displaced=1
	fi

	run_privileged mv "$expanded_app" "$XCODE_APP"
	target_installed=1
	run_privileged xattr -dr com.apple.quarantine "$XCODE_APP" 2>/dev/null || true
	verify_xcode_bundle "$XCODE_APP"

	run_privileged touch "$RECONCILIATION_MARKER"
	install_committed=1
	target_installed=
	target_displaced=
	if [[ -n "$backup" && ( -e "$backup" || -L "$backup" ) ]]; then
		run_privileged rm -rf -- "$backup"
	fi
	reconcile_xcode force

	log "Installed Xcode $XCODE_PACKAGE_VERSION ($XCODE_BUILD_VERSION) in '$XCODE_APP'."
)

case "$COMMAND" in
verify)
	if ! resolve_xcode_target; then
		# Report against the configured location: that's the one the operator has to fix.
		verify_xcode_bundle "$XCODE_APP"
		exit 1
	fi
	log "Found Xcode $XCODE_PACKAGE_VERSION ($XCODE_BUILD_VERSION) in '$XCODE_APP'."
	;;
verify-package)
	if [[ -z "$PACKAGE_DIRECTORY" || -n "$ARCHIVE" ]]; then
		error "verify-package requires exactly one --package-directory."
		exit 1
	fi
	verify_native_apple_silicon
	validate_package "$PACKAGE_DIRECTORY" >/dev/null
	log "Verified $XCODE_PACKAGE_NAME $XCODE_PACKAGE_VERSION."
	;;
reconcile)
	if [[ -n "$PACKAGE_DIRECTORY" || -n "$ARCHIVE" ]]; then
		error "reconcile does not accept a package or archive."
		exit 1
	fi
	resolve_xcode_target adopt || true
	reconcile_xcode
	log "Selected Xcode $XCODE_PACKAGE_VERSION ($XCODE_BUILD_VERSION)."
	;;
install)
	if [[ -n "$PACKAGE_DIRECTORY" && -n "$ARCHIVE" ]]; then
		error "install accepts either --package-directory or --archive, not both."
		exit 1
	fi
	clean_stale_install_paths
	if resolve_xcode_target adopt; then
		reconcile_xcode
		log "Xcode $XCODE_PACKAGE_VERSION ($XCODE_BUILD_VERSION) was already installed."
		exit 0
	fi
	if [[ -n "$PACKAGE_DIRECTORY" ]]; then
		verify_native_apple_silicon
		ARCHIVE=$(validate_package "$PACKAGE_DIRECTORY")
		EXPANDED_SIZE=$(jq -r '.artifacts[0].expandedSizeBytes' "$PACKAGE_DIRECTORY/xcode-metadata.json")
	elif [[ -z "$ARCHIVE" ]]; then
		error "Xcode is not installed; provide --package-directory or --archive."
		exit 1
	else
		EXPANDED_SIZE=0
	fi
	install_archive "$ARCHIVE" "$EXPANDED_SIZE"
	;;
esac
