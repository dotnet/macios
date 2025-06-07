#!/bin/bash -eu

set -o pipefail
IFS=$'\n\t'

function showHelp ()
{
	TOOLNAME=$(basename "$0")
	cat <<EOF
$TOOLNAME: Script that will merge main into an existing release branch, overwriting any changes in that release branch with main's content.

	This is typically used when releasing support for minor Xcode bumps.

    Options:
        -h --help:                  Show this help
        -v --verbose:               Enable verbose script
        --source-branch=...         The source branch (defaults to 'main').
        --target-branch=...         The release branch that will receive the source branch's contents.

    This is how the Xcode 16.4 release was done:

        \$ ./$TOOLNAME --target-branch=release/9.0.1xx
EOF
}

TARGET_BRANCH=
SOURCE_BRANCH=main
ORIGIN=origin

BLUE=$(tput setaf 6 2>/dev/null || true)
RED=$(tput setaf 9 2>/dev/null || true)
CLEAR=$(tput sgr0 2>/dev/null || true)

while ! test -z "${1:-}"; do
	case $1 in
		-h | --help)
			showHelp
			exit 0
			;;
		--verbose | -v)
			set -x
			shift
			;;
		--target-branch=*)
			TARGET_BRANCH="${1//*=/}"
			shift
			;;
		--target-branch)
			TARGET_BRANCH="$2"
			shift 2
			;;
		--source-branch=*)
			SOURCE_BRANCH="${1//*=/}"
			shift
			;;
		--source-branch)
			SOURCE_BRANCH="$2"
			shift 2
			;;
		*)
			echo "${RED}Unexpected argument: $1${CLEAR}"
			exit 1
			;;
	esac
done

# Validate input
if test -z "$TARGET_BRANCH"; then
	echo "${RED}No target branch specified (use --target-branch=...)${CLEAR}"
	exit 1
elif ! git rev-parse --quiet --verify "$ORIGIN/$TARGET_BRANCH" > /dev/null; then
	echo "${RED}The target branch ${BLUE}$TARGET_BRANCH${RED} does not exist in the remote ${BLUE}$ORIGIN${RED}.${CLEAR}"
	exit 1
fi
if test -z "$SOURCE_BRANCH"; then
	echo "${RED}No source branch specified (the default is ${BLUE}main${RED}, otherwise use --source-branch=... with a non-empty value)${CLEAR}"
	exit 1
elif ! git rev-parse --quiet --verify "$ORIGIN/$SOURCE_BRANCH" > /dev/null; then
	echo "${RED}The target branch ${BLUE}$SOURCE_BRANCH${RED} does not exist in the remote ${BLUE}$ORIGIN${RED}.${CLEAR}"
	exit 1
fi

# Go to the root git directory
cd "$(git rev-parse --show-toplevel)"

# Verify that we're not overwriting any local changes
if [ -n "$(git status --porcelain)" ]; then
	echo "${RED}Working directory is not clean:${CLEAR}"
	git status | sed 's/^/    /'
	exit 1
fi

echo "Fetching ${BLUE}$ORIGIN${CLEAR} to make sure our working copy has everything it needs..."
git fetch $ORIGIN

# Compute a few variables
XCODE_VERSION=$(git show $ORIGIN/$SOURCE_BRANCH:Make.config | grep ^XCODE_VERSION= | sed 's/.*=//')
DOTNET_VERSION=$(git show $ORIGIN/$SOURCE_BRANCH:Make.config | grep ^DOTNET_TFM= | sed -e 's/.*net//' -e 's/[.].*//')

echo "Creating release for Xcode $XCODE_VERSION, by applying all the changes from ${BLUE}$SOURCE_BRANCH${CLEAR} into ${BLUE}$TARGET_BRANCH${CLEAR}."

echo "Creating our working branch, from the source branch ${BLUE}$SOURCE_BRANCH${CLEAR}..."
# delete it if it already exists (probably due to running this script already)
WORKING_BRANCH=merge-xcode${XCODE_VERSION}-into-${TARGET_BRANCH}-$(date +%Y-%m-%d)
if git rev-parse --quiet --verify "$WORKING_BRANCH" > /dev/null; then
	git branch --quiet -D "$WORKING_BRANCH"
fi
git checkout -b "$WORKING_BRANCH" "$ORIGIN/$SOURCE_BRANCH" --no-track 2>&1 | sed 's/^/    /'

TITLE="[$TARGET_BRANCH] Merge remote-tracking branch '$ORIGIN/$SOURCE_BRANCH' into $TARGET_BRANCH."
BODY="This brings Xcode $XCODE_VERSION support to .NET $DOTNET_VERSION."

echo "Merging ${BLUE}$TARGET_BRANCH${CLEAR} into the working branch, but without actually bringing in any changes..."
git merge --quiet $ORIGIN/$TARGET_BRANCH --no-commit --strategy ours -m "$TITLE"$'\n\n'"$BODY" 2>&1 | sed 's/^/    /'

# Fix the NUGET_RELEASE_BRANCH variable, we don't want the version from source branch, we want it to point to the target branch.
sed -i '' "s|^[# ]*NUGET_RELEASE_BRANCH=.*|NUGET_RELEASE_BRANCH=$TARGET_BRANCH|" Make.config
git add Make.config 2>&1 | sed 's/^/    /'
git commit --quiet --amend -C head 2>&1 | sed 's/^/    /'

echo ""
echo "The current branch is ready to create a pull request, please review to make sure everything is OK."
echo ""
echo "If you have ${BLUE}gh${CLEAR} installed (${BLUE}brew install gh${CLEAR}), you can execute the following to create the pull request:"
echo ""
echo "    git push --set-upstream $ORIGIN $WORKING_BRANCH:dev/$USER/$WORKING_BRANCH"
echo "    gh pr create --base $TARGET_BRANCH --title \"$TITLE\" --body \"$BODY\""
echo ""
