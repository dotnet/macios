#!/bin/bash

set -euo pipefail

# Downloads the immutable Xcode Universal Package pinned in Make.config.
#
# The same feed name and package name exist in every supported Azure DevOps
# organization; only the organization and the project that hosts the feed differ:
#
#   DevDiv: https://devdiv.visualstudio.com/DevDiv/_artifacts/feed/macios-tools-internal/UPack/xcode-apple-silicon/
#   dnceng: https://dnceng.visualstudio.com/internal/_artifacts/feed/macios-tools-internal/UPack/xcode-apple-silicon/
#
# The organization is taken from the running build (SYSTEM_COLLECTIONURI) and mapped to
# the project that owns the feed. Both feeds are project-scoped, so the pipeline's own
# build identity can read them once it has been granted the Feed Reader role, and no
# Azure subscription service connection, storage SAS token or GitHub token is involved.
#
# The mapping is a default, not a policy: --organization, --project and --feed all
# override it, so a new organization can be onboarded without changing this script.

SCRIPT_DIR=$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)
TOP=$(cd "$SCRIPT_DIR/../../../../.." && pwd)

ORGANIZATION=${SYSTEM_COLLECTIONURI:-}
PROJECT=
FEED=
DESTINATION=
PRINT=

usage ()
{
	cat <<EOF
Usage: $0 --path <new-absolute-directory> [options]
       $0 --print organization|project|feed|name|version [options]

Options:
  --organization <url>   Azure DevOps organization URL. Defaults to \$SYSTEM_COLLECTIONURI.
  --project <name>       Project that hosts the feed. Defaults to the organization mapping.
  --feed <name>          Feed name. Defaults to XCODE_PACKAGE_FEED in Make.config.
  --path <directory>     Directory to create for the downloaded package.
  --print <field>        Print a single resolved value instead of downloading.

SYSTEM_ACCESSTOKEN must contain an Azure DevOps token that can read the feed.
EOF
}

while [[ $# -gt 0 ]]; do
	case "$1" in
	--organization | --project | --feed | --path | --print)
		if [[ $# -lt 2 || -z "$2" ]]; then
			echo "$1 requires a value." >&2
			exit 1
		fi
		case "$1" in
		--organization) ORGANIZATION=$2 ;;
		--project) PROJECT=$2 ;;
		--feed) FEED=$2 ;;
		--path) DESTINATION=$2 ;;
		--print) PRINT=$2 ;;
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

read_config_value ()
{
	local name=$1
	local assignment

	assignment=$(grep -E "^${name}[?:]?=" "$TOP/Make.config" | head -n 1 || true)
	echo "${assignment#*=}"
}

PACKAGE_NAME=$(read_config_value XCODE_PACKAGE_NAME)
PACKAGE_VERSION=$(read_config_value XCODE_PACKAGE_VERSION)
if [[ -z "$FEED" ]]; then
	FEED=$(read_config_value XCODE_PACKAGE_FEED)
fi

if [[ -z "$PACKAGE_NAME" || -z "$PACKAGE_VERSION" || -z "$FEED" ]]; then
	echo "Make.config must define XCODE_PACKAGE_FEED, XCODE_PACKAGE_NAME and XCODE_PACKAGE_VERSION." >&2
	exit 1
fi

if [[ -z "$ORGANIZATION" ]]; then
	echo "No Azure DevOps organization: pass --organization or set SYSTEM_COLLECTIONURI." >&2
	exit 1
fi

while [[ "$ORGANIZATION" == */ ]]; do
	ORGANIZATION=${ORGANIZATION%/}
done

if [[ "$ORGANIZATION" =~ ^https://dev[.]azure[.]com/([A-Za-z0-9._-]+)$ ]]; then
	ORGANIZATION_NAME=${BASH_REMATCH[1]}
elif [[ "$ORGANIZATION" =~ ^https://([A-Za-z0-9._-]+)[.]visualstudio[.]com$ ]]; then
	ORGANIZATION_NAME=${BASH_REMATCH[1]}
else
	echo "Unsupported Azure DevOps organization URL: '$ORGANIZATION'." >&2
	exit 1
fi
ORGANIZATION_NAME=$(tr '[:upper:]' '[:lower:]' <<< "$ORGANIZATION_NAME")

if [[ -z "$PROJECT" ]]; then
	case "$ORGANIZATION_NAME" in
	devdiv)
		PROJECT=DevDiv
		;;
	dnceng)
		PROJECT=internal
		;;
	*)
		echo "The '$FEED' feed location is not known for the '$ORGANIZATION_NAME' organization." >&2
		echo "Supported organizations are 'devdiv' (project 'DevDiv') and 'dnceng' (project 'internal')." >&2
		echo "Note that 'dnceng' means the 'internal' project; the 'public' project has no access to this feed." >&2
		echo "Pass --project to use '$FEED' from another project." >&2
		exit 1
		;;
	esac
fi

# Talk to a single canonical endpoint per organization so the result doesn't depend on
# whether the agent reported the legacy '<org>.visualstudio.com' host or the modern one.
case "$ORGANIZATION_NAME" in
devdiv | dnceng)
	ORGANIZATION="https://dev.azure.com/$ORGANIZATION_NAME"
	;;
esac

if [[ -n "$PRINT" ]]; then
	case "$PRINT" in
	organization) echo "$ORGANIZATION" ;;
	project) echo "$PROJECT" ;;
	feed) echo "$FEED" ;;
	name) echo "$PACKAGE_NAME" ;;
	version) echo "$PACKAGE_VERSION" ;;
	*)
		echo "Unknown field: '$PRINT'." >&2
		usage >&2
		exit 1
		;;
	esac
	exit 0
fi

if [[ -z "$DESTINATION" ]]; then
	usage >&2
	exit 1
fi

echo "Resolved '$PACKAGE_NAME/$PACKAGE_VERSION' to '$ORGANIZATION/$PROJECT' feed '$FEED'."
"$SCRIPT_DIR/download-universal-package.sh" \
	--organization "$ORGANIZATION" \
	--project "$PROJECT" \
	--feed "$FEED" \
	--name "$PACKAGE_NAME" \
	--version "$PACKAGE_VERSION" \
	--path "$DESTINATION"
