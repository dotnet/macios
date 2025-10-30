#!/bin/bash -eu

set -o pipefail
IFS=$'\n\t'

# This is a script that builds our platform assemblies, and copies them to the
# repository used to create API reference documentation.

APIDROP_REMOTE=https://apidrop.visualstudio.com/binaries/_git/binaries
APIDROP_REPOSITORY=
BUILD=1
PUSH=
CHECKOUT=

GREEN=$(tput setaf 2 2>/dev/null || true)
YELLOW=$(tput setaf 3 2>/dev/null || true)
MAGENTA=$(tput setaf 5 2>/dev/null || true)
BLUE=$(tput setaf 6 2>/dev/null || true)
RED=$(tput setaf 9 2>/dev/null || true)
CLEAR=$(tput sgr0 2>/dev/null || true)

BACKGROUND_COLOR=$GREEN
BRANCH_COLOR=$BLUE
LINK_COLOR=$MAGENTA
PATH_COLOR=$BLUE
NOTICE_COLOR=$YELLOW
REMOTE_COLOR=$MAGENTA
PLATFORM_COLOR=$BLUE

while ! test -z "${1:-}"; do
	case $1 in
		--help | -\? | -h)
			echo "$(basename "$0"):"
			echo "    Builds the current checkout, and copies the product assemblies to apidrop.visualstudio.com's binaries repository to update the API docs."
			echo ""
			echo "    Options:"
			echo "        -h --help                  Show this help"
			echo "        -v --verbose               Make verbose"
			echo "        -a --apidrop-repository=   Specify the local path to the binaries repository ($REMOTE_COLOR$APIDROP_REMOTE$CLEAR)."
			echo "        -p --push                  If specified, push the commit with the updated assemblies to the binaries repository (otherwise this will have to be done manually)."
			echo "        -s --skip-build            If specified, any existing build will be used instead of rebuilding the checkout."
			echo "        -c --checkout=             The path to the dotnet/macios checkout to use."
			exit 0
			;;
		-v | --verbose)
			set -x
			shift
			;;
		-a=* | --apidrop-repository=*)
			APIDROP_REPOSITORY="${1//*=/}"
			shift
			;;
		-a | --apidrop-repository)
			APIDROP_REPOSITORY="${2:-}"
			shift 2
			;;
		-p | --push)
			PUSH=1
			shift
			;;
		-c | --checkout)
			CHECKOUT="${2:-}"
			shift 2
			;;
		-c=* | --checkout=*)
			CHECKOUT="${1//*=/}"
			shift
			;;
		-s | --skip-build)
			BUILD=
			shift
			;;
		*)
			echo "${RED}$(basename "$0"): Unknown option: $MAGENTA$1$RED. Pass --help to view the available options.${CLEAR}"
			exit 1
			;;
	esac
done

echo "${BACKGROUND_COLOR}$(basename "$0"): Build the current macios checkout and copy the product assemblies to the apidrop.visualstudio.com's binaries repository to update the API docs... Pass --help for help."

if test -n "${CHECKOUT:-}"; then
	cd "$CHECKOUT"
fi

cd "$(git rev-parse --show-toplevel)"
MACIOS_PATH=$(pwd)

if [ -n "$(git status --porcelain --ignore-submodule)" ]; then
	echo "${RED}Working directory is not clean:${CLEAR}"
	git status --ignore-submodule | sed 's/^/    /'
	exit 1
fi

if test -z "$APIDROP_REPOSITORY"; then
	echo "${NOTICE_COLOR}The path to the apidrop repository was not found, so looking for it...${CLEAR}"
	while [[ "$(pwd)" != "/" ]]; do
		cd ..
		if test -d apidrop.visualstudio.com/binaries; then
			cd apidrop.visualstudio.com/binaries
			if REMOTE=$(git remote get-url origin); then
				if [[ "$REMOTE" == "$APIDROP_REMOTE" ]]; then
					APIDROP_REPOSITORY=$(pwd)
					echo "${BACKGROUND_COLOR}    Located the apidrop repository: ${PATH_COLOR}$APIDROP_REPOSITORY${CLEAR}"
					break
				fi
				echo "${NOTICE_COLOR}    Discarded the git checkout in ${PATH_COLOR}$(pwd)${NOTICE_COLOR}, because its remote $REMOTE_COLOR$REMOTE$NOTICE_COLOR does not match the expected remote $REMOTE_COLOR$APIDROP_REMOTE$NOTICE_COLOR.${CLEAR}"
			else
				echo "${NOTICE_COLOR}    Discarded the directory ${PATH_COLOR}$(pwd)${NOTICE_COLOR}, because it's not a git checkout.${CLEAR}"
			fi
			cd ../..
		fi
	done
	if test -z "$APIDROP_REPOSITORY"; then
		echo "${RED}Could not find a local checkout of the $REMOTE_COLOR$APIDROP_REMOTE$RED repository. Check it out, and then pass the path using ${BACKGROUND_COLOR}--apidrop-repository=/path/to/checkout${RED}.${CLEAR}"
		exit 1
	fi
else
	echo "${BACKGROUND_COLOR}Using the directory $PATH_COLOR$APIDROP_REPOSITORY$BACKGROUND_COLOR as the checkout for the $REMOTE_COLOR$APIDROP_REMOTE$BACKGROUND_COLOR repository.${CLEAR}"
fi

cd "$MACIOS_PATH"

if test -n "$BUILD"; then
	echo "${BACKGROUND_COLOR}Building macios (pass --skip-build to skip)...${CLEAR}"
	(
		make reset
		make git-clean-all
		git clean -xfd
		./configure
		make all -j8
		make install -j8
	) 2>&1 | sed 's/^/    /'
else
	echo "${BACKGROUND_COLOR}Skipped building macios...${CLEAR}"
fi

TMPFILE=$(mktemp)
make -C "$MACIOS_PATH"/tools/devops print-variable-value-to-file "FILE=$TMPFILE" VARIABLE=XCODE_VERSION
XCODE_VERSION=$(cat "$TMPFILE")
make -C "$MACIOS_PATH"/tools/devops print-variable-value-to-file "FILE=$TMPFILE" VARIABLE=DOTNET_TFM
DOTNET_TFM=$(cat "$TMPFILE")
make -C "$MACIOS_PATH"/tools/devops print-variable-value-to-file "FILE=$TMPFILE" VARIABLE=DOTNET_PLATFORMS
DOTNET_PLATFORMS=$(cat "$TMPFILE")
rm -f "$TMPFILE"

BINARIES_BRANCH=$DOTNET_TFM-xcode$XCODE_VERSION

echo "${BACKGROUND_COLOR}Copying assemblies to the branch ${BRANCH_COLOR}${BINARIES_BRANCH}${BACKGROUND_COLOR} in the binaries repository (${PATH_COLOR}${APIDROP_REPOSITORY}${BACKGROUND_COLOR})...${CLEAR}"
cd "$APIDROP_REPOSITORY"
git checkout master
git pull
if ! git checkout -b "$BINARIES_BRANCH"; then
	echo "${RED}Failed to checkout the branch $BRANCH_COLOR$BINARIES_BRANCH$RED in $PATH_COLOR$APIDROP_REPOSITORY$RED - if the branch already exists, please delete it first.$CLEAR"
	exit 1
fi
mkdir -p "$APIDROP_REPOSITORY/dotnet-macios/"

cd "$MACIOS_PATH"

NETVERSION=${DOTNET_TFM//net/}
COMMIT_MESSAGE="Add updated assemblies for .NET $NETVERSION:"
IFS=' '; for platform in $DOTNET_PLATFORMS; do
	PLATFORM_UPPERCASE=$(echo "$platform" | tr '[:lower:]' '[:upper:]')
	PLATFORM_LOWERCASE=$(echo "$platform" | tr '[:upper:]' '[:lower:]')

	OSVERSION=$(grep "^${PLATFORM_UPPERCASE}_NUGET_OS_VERSION=" Make.versions | sed 's/.*=//')
	COMMIT_MESSAGE="$COMMIT_MESSAGE $platform $OSVERSION,"

	DIR=$APIDROP_REPOSITORY/dotnet-macios/net-$PLATFORM_LOWERCASE-$OSVERSION-$NETVERSION
	mkdir -p "$DIR"
	cp "_build/Microsoft.$platform.Ref.net${NETVERSION}_$OSVERSION/ref/net$NETVERSION/Microsoft.$platform.dll" "$DIR/"
	cp "_build/Microsoft.$platform.Ref.net${NETVERSION}_$OSVERSION/ref/net$NETVERSION/Microsoft.$platform.xml" "$DIR/"
	echo "${BACKGROUND_COLOR}    Copied $PLATFORM_COLOR$platform$BACKGROUND_COLOR assemblies to $PATH_COLOR$DIR$CLEAR"
done

# remove the last character (",")
COMMIT_MESSAGE="${COMMIT_MESSAGE/%?}"

echo "${BACKGROUND_COLOR}Creating the commit in the binaries repository (${PATH_COLOR}${APIDROP_REPOSITORY}${BACKGROUND_COLOR})...${CLEAR}"
cd "$APIDROP_REPOSITORY/dotnet-macios/"
git add .
git commit -m "$COMMIT_MESSAGE"

if test -n "$PUSH"; then
	git push
	echo "${BACKGROUND_COLOR}Pushed a commit with the updated assemblies to the ${BRANCH_COLOR}$BINARIES_BRANCH${BACKGROUND_COLOR} branch in ${PATH_COLOR}${APIDROP_REPOSITORY}${BACKGROUND_COLOR}.${CLEAR}"
	echo "${BACKGROUND_COLOR}A commit has been created and pushed in ${PATH_COLOR}${APIDROP_REPOSITORY}${BACKGROUND_COLOR}.${CLEAR}"
else
	echo "${BACKGROUND_COLOR}A commit that is ready to be pushed has been created in ${PATH_COLOR}${APIDROP_REPOSITORY}${BACKGROUND_COLOR}.${CLEAR}"
fi

echo "${BACKGROUND_COLOR}Please read the instructions (${LINK_COLOR}update-api-docs.md${BACKGROUND_COLOR}) for the next steps.${CLEAR}"
