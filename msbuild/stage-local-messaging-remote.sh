#!/bin/bash -eu

# Restores and builds with a local Xamarin.Messaging.Remote package for ConfigureMac.

TOP=$(cd "$(dirname "$0")/.." && pwd -P)
BUILD_ONLY=false
if [[ "${1:-}" == "--build" ]]; then
	BUILD_ONLY=true
	shift
fi
PACKAGE=${1:-}
TEMP_DIR=$(mktemp -d)
trap 'rm -rf "$TEMP_DIR"' EXIT

if [[ -z "$PACKAGE" && -n "${MESSAGING_REMOTE_REPO:-}" ]]; then
	if [[ ! -f "$MESSAGING_REMOTE_REPO/remote/Makefile" ]]; then
		echo "MESSAGING_REMOTE_REPO must point to a checkout containing remote/Makefile: '$MESSAGING_REMOTE_REPO'." >&2
		exit 1
	fi
	(
		# The devtools checkout restores to its own cache, not the macios cache.
		unset NuGetPackageRoot
		"$MAKE" -C "$MESSAGING_REMOTE_REPO/remote" pack "PACKAGE_PATH_FILE=$TEMP_DIR/package-path"
	)
	if [[ ! -s "$TEMP_DIR/package-path" ]] || ! IFS= read -r PACKAGE < "$TEMP_DIR/package-path" || [[ -z "$PACKAGE" ]]; then
		echo "The remote pack target did not write the new package path to PACKAGE_PATH_FILE. Update the messaging remote checkout." >&2
		exit 1
	fi
	if [[ "$PACKAGE" != /* ]]; then
		echo "The remote pack target must write an absolute package path: '$PACKAGE'." >&2
		exit 1
	fi
fi
if [[ -z "$PACKAGE" ]]; then
	echo "Set MESSAGING_REMOTE_PACKAGE to a Xamarin.Messaging.Remote .nupkg file, or set MESSAGING_REMOTE_REPO to its source checkout." >&2
	exit 1
fi
if [[ ! -f "$PACKAGE" ]]; then
	echo "The package '$PACKAGE' does not exist." >&2
	exit 1
fi

PACKAGE=$(cd "$(dirname "$PACKAGE")" && pwd -P)/$(basename "$PACKAGE")
NUSPEC="$TEMP_DIR/package.nuspec"

NUSPEC_PATH=$(unzip -Z1 "$PACKAGE" | grep -E '^[^/]+\.nuspec$' | head -n 1 || true)
if [[ -z "$NUSPEC_PATH" ]]; then
	echo "The package '$PACKAGE' does not contain a .nuspec file." >&2
	exit 1
fi
unzip -p "$PACKAGE" "$NUSPEC_PATH" > "$NUSPEC"
PACKAGE_ID=$(xmllint --xpath 'string(/*[local-name()="package"]/*[local-name()="metadata"]/*[local-name()="id"])' "$NUSPEC")
PACKAGE_VERSION=$(xmllint --xpath 'string(/*[local-name()="package"]/*[local-name()="metadata"]/*[local-name()="version"])' "$NUSPEC")
if [[ "$PACKAGE_ID" != "Xamarin.Messaging.Remote" ]]; then
	echo "The package '$PACKAGE' is '$PACKAGE_ID', not 'Xamarin.Messaging.Remote'." >&2
	exit 1
fi
if [[ ! "$PACKAGE_VERSION" =~ ^(0|[1-9][0-9]*)\.(0|[1-9][0-9]*)\.(0|[1-9][0-9]*)(-[0-9A-Za-z-]+(\.[0-9A-Za-z-]+)*)?$ ]]; then
	echo "The package '$PACKAGE' must have a normalized three-part version without build metadata: '$PACKAGE_VERSION'." >&2
	exit 1
fi
for TASK_FRAMEWORK in net12.0 netstandard2.0; do
	if ! unzip -Z1 "$PACKAGE" | grep -Fxq "tools/$TASK_FRAMEWORK/Xamarin.Messaging.Remote.Tasks.dll"; then
		echo "The package '$PACKAGE' does not contain tools/$TASK_FRAMEWORK/Xamarin.Messaging.Remote.Tasks.dll." >&2
		exit 1
	fi
done

PACKAGE_ROOT="$TOP/packages"
PACKAGE_VERSION_LOWER=$(printf '%s' "$PACKAGE_VERSION" | tr '[:upper:]' '[:lower:]')
RESTORED_PACKAGE="$PACKAGE_ROOT/xamarin.messaging.remote/$PACKAGE_VERSION_LOWER"
PACKAGE_HASH=$(openssl dgst -sha512 -binary "$PACKAGE" | base64 | tr -d '\n')

if [[ -d "$RESTORED_PACKAGE" ]]; then
	RESTORED_PACKAGE_PHYSICAL=$(cd "$RESTORED_PACKAGE" && pwd -P)
	case "$PACKAGE" in
		"$RESTORED_PACKAGE_PHYSICAL"/*)
			echo "The package must not be inside its NuGet cache directory: '$RESTORED_PACKAGE_PHYSICAL'." >&2
			exit 1
			;;
	esac
	if [[ "$PACKAGE" -ef "$RESTORED_PACKAGE/xamarin.messaging.remote.$PACKAGE_VERSION_LOWER.nupkg" ]]; then
		echo "The package must not be inside its NuGet cache directory: '$RESTORED_PACKAGE_PHYSICAL'." >&2
		exit 1
	fi
fi

# Remove an existing package with the same version, so NuGet doesn't reuse it.
rm -rf "$RESTORED_PACKAGE"
BUILD_ARGUMENTS=("$TOP/msbuild/Xamarin.iOS.Tasks.Windows/Xamarin.iOS.Tasks.Windows.csproj")
if $BUILD_ONLY; then
	BUILD_ARGUMENTS=("$TOP/msbuild/Xamarin.MacDev.Tasks.slnx" "/bl:$TOP/msbuild/.build-stamp.binlog" /nologo /verbosity:quiet)
fi
"$DOTNET" build "${BUILD_ARGUMENTS[@]}" \
	-restore \
	-p:RestoreForce=true \
	-p:RestoreAdditionalProjectSources="$(dirname "$PACKAGE")" \
	-p:MessagingRemoteVersion="[$PACKAGE_VERSION]" \
	-p:NuGetPackageRoot="$PACKAGE_ROOT/" \
	-m:1

RESTORED_HASH=$(tr -d '\r\n' < "$RESTORED_PACKAGE/xamarin.messaging.remote.$PACKAGE_VERSION_LOWER.nupkg.sha512")
if [[ "$RESTORED_HASH" != "$PACKAGE_HASH" ]]; then
	echo "NuGet restored a different Xamarin.Messaging.Remote $PACKAGE_VERSION package. Use a package version that isn't on another configured feed." >&2
	exit 1
fi

if $BUILD_ONLY; then
	echo "Built with Xamarin.Messaging.Remote $PACKAGE_VERSION from $PACKAGE."
	exit 0
fi

rm -f "$TOP/msbuild/.copy-stamp-iOS-configure-mac"
"$MAKE" -C "$TOP/msbuild" -o .build-stamp .copy-stamp-iOS-configure-mac .copy-windows-files

echo "Staged Xamarin.Messaging.Remote $PACKAGE_VERSION for ConfigureMac."
echo "Package SHA512: $PACKAGE_HASH"
