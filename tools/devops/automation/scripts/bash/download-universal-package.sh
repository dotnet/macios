#!/bin/bash

set -euo pipefail

ORGANIZATION=
PROJECT=
FEED=
PACKAGE_NAME=
PACKAGE_VERSION=
DESTINATION=
FILTER=

usage ()
{
	cat <<EOF
Usage: $0 --organization <url> --project <name> --feed <name> \
  --name <package> --version <version> --path <new-absolute-directory> [--filter <pattern>]

SYSTEM_ACCESSTOKEN must contain an Azure DevOps token that can read the feed.
EOF
}

while [[ $# -gt 0 ]]; do
	case "$1" in
	--organization | --project | --feed | --name | --version | --path | --filter)
		if [[ $# -lt 2 || -z "$2" ]]; then
			echo "$1 requires a value." >&2
			usage >&2
			exit 1
		fi
		case "$1" in
		--organization) ORGANIZATION=$2 ;;
		--project) PROJECT=$2 ;;
		--feed) FEED=$2 ;;
		--name) PACKAGE_NAME=$2 ;;
		--version) PACKAGE_VERSION=$2 ;;
		--path) DESTINATION=$2 ;;
		--filter) FILTER=$2 ;;
		esac
		shift 2
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

if [[ -z "$ORGANIZATION" || -z "$PROJECT" || -z "$FEED" || -z "$PACKAGE_NAME" || -z "$PACKAGE_VERSION" || -z "$DESTINATION" ]]; then
	usage >&2
	exit 1
fi
if [[ -z "${SYSTEM_ACCESSTOKEN:-}" ]]; then
	echo "SYSTEM_ACCESSTOKEN is required." >&2
	exit 1
fi

while [[ "$DESTINATION" != "/" && "$DESTINATION" == */ ]]; do
	DESTINATION=${DESTINATION%/}
done
if [[ "$DESTINATION" != /* || "$DESTINATION" == "/" ]]; then
	echo "The Universal Package destination must be an absolute non-root path: '$DESTINATION'." >&2
	exit 1
fi

DESTINATION_PARENT=$(dirname "$DESTINATION")
DESTINATION_NAME=$(basename "$DESTINATION")
if [[ -z "$DESTINATION_NAME" || "$DESTINATION_NAME" == "." || "$DESTINATION_NAME" == ".." ]]; then
	echo "The Universal Package destination has an unsafe name: '$DESTINATION'." >&2
	exit 1
fi
mkdir -p "$DESTINATION_PARENT"
DESTINATION_PARENT=$(cd "$DESTINATION_PARENT" && pwd -P)
DESTINATION="$DESTINATION_PARENT/$DESTINATION_NAME"
if [[ -e "$DESTINATION" || -L "$DESTINATION" ]]; then
	echo "The Universal Package destination already exists: '$DESTINATION'." >&2
	exit 1
fi

ORGANIZATION=${ORGANIZATION%/}
if [[ "$ORGANIZATION" =~ ^https://dev[.]azure[.]com/([A-Za-z0-9._-]+)$ ]]; then
	ORGANIZATION_NAME=${BASH_REMATCH[1]}
elif [[ "$ORGANIZATION" =~ ^https://([A-Za-z0-9._-]+)[.]visualstudio[.]com$ ]]; then
	ORGANIZATION_NAME=${BASH_REMATCH[1]}
else
	echo "Unsupported Azure DevOps organization URL: '$ORGANIZATION'." >&2
	exit 1
fi

case "$(uname -s)/$(uname -m)" in
Darwin/arm64)
	ARTIFACTTOOL_ARCH=x86_64
	if ! /usr/bin/arch -x86_64 /usr/bin/true 2>/dev/null; then
		echo "Rosetta is required to run Azure DevOps ArtifactTool on arm64." >&2
		exit 1
	fi
	RUN_WITH_ROSETTA=1
	;;
Darwin/x86_64)
	ARTIFACTTOOL_ARCH=x86_64
	RUN_WITH_ROSETTA=
	;;
*)
	echo "Unsupported Universal Package host: '$(uname -s)/$(uname -m)'." >&2
	exit 1
	;;
esac

CLIENT_TOOLS_URL="https://vsblob.dev.azure.com/$ORGANIZATION_NAME/_apis/clienttools/ArtifactTool/release"
RELEASE_JSON=$(
	printf 'header = "%s: %s %s"\n' Authorization Bearer "$SYSTEM_ACCESSTOKEN" |
		curl \
			--config - \
			--fail \
			--silent \
			--show-error \
			--retry 5 \
			--retry-all-errors \
			--get \
			--data-urlencode "osName=Darwin" \
			--data-urlencode "arch=$ARTIFACTTOOL_ARCH" \
			--data-urlencode "distroName=darwin" \
			--data-urlencode "distroVersion=$(sw_vers -productVersion)" \
			"$CLIENT_TOOLS_URL"
)

if ! jq -e '
	.name == "ArtifactTool" and
	.rid == "osx-x64" and
	(.version | type == "string" and test("^[0-9]+[.][0-9]+[.][0-9]+$")) and
	(.uri | type == "string" and startswith("https://"))
' <<< "$RELEASE_JSON" >/dev/null; then
	echo "Azure DevOps returned an invalid ArtifactTool release." >&2
	exit 1
fi

ARTIFACTTOOL_VERSION=$(jq -r '.version' <<< "$RELEASE_JSON")
ARTIFACTTOOL_URI=$(jq -r '.uri' <<< "$RELEASE_JSON")
TOOLS_ROOT="${AGENT_TEMPDIRECTORY:-${TMPDIR:-/tmp}}/macios-artifacttool"
ARTIFACTTOOL_DIRECTORY="$TOOLS_ROOT/ArtifactTool-osx-x64-$ARTIFACTTOOL_VERSION"
ARTIFACTTOOL="$ARTIFACTTOOL_DIRECTORY/artifacttool"

if [[ ! -x "$ARTIFACTTOOL" ]]; then
	TEMP_DIRECTORY=$(mktemp -d "${TMPDIR:-/tmp}/macios-artifacttool.XXXXXX")
	cleanup ()
	{
		rm -rf "$TEMP_DIRECTORY"
	}
	trap cleanup EXIT

	echo "Downloading Azure DevOps ArtifactTool $ARTIFACTTOOL_VERSION for osx-x64."
	curl \
		--fail \
		--silent \
		--show-error \
		--location \
		--retry 5 \
		--retry-all-errors \
		"$ARTIFACTTOOL_URI" \
		--output "$TEMP_DIRECTORY/artifacttool.zip"
	/usr/bin/ditto -x -k "$TEMP_DIRECTORY/artifacttool.zip" "$TEMP_DIRECTORY/extracted"
	if [[ ! -f "$TEMP_DIRECTORY/extracted/artifacttool" ]]; then
		echo "The ArtifactTool release did not contain the expected executable." >&2
		exit 1
	fi
	chmod +x "$TEMP_DIRECTORY/extracted/artifacttool"
	mkdir -p "$TOOLS_ROOT"
	rm -rf "$ARTIFACTTOOL_DIRECTORY"
	mv "$TEMP_DIRECTORY/extracted" "$ARTIFACTTOOL_DIRECTORY"
	trap - EXIT
	cleanup
fi

DOWNLOAD_DIRECTORY=$(mktemp -d "$DESTINATION_PARENT/.${DESTINATION_NAME}.download.XXXXXX")
# shellcheck disable=SC2329
cleanup_download ()
{
	local status=$?
	trap - EXIT

	if [[ -n "$DOWNLOAD_DIRECTORY" && ( -e "$DOWNLOAD_DIRECTORY" || -L "$DOWNLOAD_DIRECTORY" ) ]]; then
		rm -rf -- "$DOWNLOAD_DIRECTORY" || true
	fi
	exit "$status"
}
trap cleanup_download EXIT

export UNIVERSAL_PACKAGE_PAT=$SYSTEM_ACCESSTOKEN
ARGUMENTS=(
	universal download
	--service "$ORGANIZATION"
	--patvar UNIVERSAL_PACKAGE_PAT
	--feed "$FEED"
	--package-name "$PACKAGE_NAME"
	--package-version "$PACKAGE_VERSION"
	--path "$DOWNLOAD_DIRECTORY"
	--project "$PROJECT"
)
if [[ -n "$FILTER" ]]; then
	ARGUMENTS+=(--filter "$FILTER")
fi

echo "Downloading Universal Package '$PACKAGE_NAME/$PACKAGE_VERSION' from '$PROJECT/$FEED'."
if [[ -n "$RUN_WITH_ROSETTA" ]]; then
	/usr/bin/arch -x86_64 "$ARTIFACTTOOL" "${ARGUMENTS[@]}"
else
	"$ARTIFACTTOOL" "${ARGUMENTS[@]}"
fi

if [[ -e "$DESTINATION" || -L "$DESTINATION" ]]; then
	echo "The Universal Package destination appeared while downloading: '$DESTINATION'." >&2
	exit 1
fi
mv "$DOWNLOAD_DIRECTORY" "$DESTINATION"
DOWNLOAD_DIRECTORY=
trap - EXIT
