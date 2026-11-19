#!/bin/bash -eu

# Restores and stages a local Xamarin.Messaging.Remote package for ConfigureMac.

PACKAGE=${1:-}
if [[ -z "$PACKAGE" ]]; then
	echo "Set MESSAGING_REMOTE_PACKAGE to the path of a Xamarin.Messaging.Remote .nupkg file." >&2
	exit 1
fi
if [[ ! -f "$PACKAGE" ]]; then
	echo "The package '$PACKAGE' does not exist." >&2
	exit 1
fi

PACKAGE=$(cd "$(dirname "$PACKAGE")" && pwd -P)/$(basename "$PACKAGE")
TOP=$(cd "$(dirname "$0")/.." && pwd -P)
NUSPEC=$(mktemp)
trap 'rm -f "$NUSPEC"' EXIT

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
"$DOTNET" build "$TOP/msbuild/Xamarin.iOS.Tasks.Windows/Xamarin.iOS.Tasks.Windows.csproj" \
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

rm -f "$TOP/msbuild/.copy-stamp-iOS-configure-mac"
"$MAKE" -C "$TOP/msbuild" -o .build-stamp .copy-stamp-iOS-configure-mac .copy-windows-files

echo "Staged Xamarin.Messaging.Remote $PACKAGE_VERSION for ConfigureMac."
echo "Package SHA512: $PACKAGE_HASH"
