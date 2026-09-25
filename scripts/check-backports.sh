#!/bin/bash

# Copyright (c) Microsoft Corporation.
# Licensed under the MIT License.

set -euo pipefail

repository=${REPOSITORY:-dotnet/macios}
net10_branch=${NET10_BRANCH:-release/10.0.1xx}
net11_branch=${NET11_BRANCH:-release/11.0.1xx-rc.2}

red=""
green=""
yellow=""
cyan=""
reset=""
if [[ -t 1 && -z ${NO_COLOR:-} && ${TERM:-} != "dumb" ]]; then
	red=$'\033[31m'
	green=$'\033[32m'
	yellow=$'\033[33m'
	cyan=$'\033[36m'
	reset=$'\033[0m'
fi

usage ()
{
	cat <<EOF
Usage: $(basename "$0")

Verify that every merged pull request with a backport label has a pull request
targeting the corresponding release branch. Open, closed, and merged backport
pull requests all count.

Environment variables:
  REPOSITORY    GitHub repository (default: $repository)
  NET10_BRANCH  .NET 10 target branch (default: $net10_branch)
  NET11_BRANCH  .NET 11 target branch (default: $net11_branch)
  NO_COLOR      Disable colored output when set
EOF
}

if [[ ${1:-} == "--help" || ${1:-} == "-h" ]]; then
	usage
	exit 0
fi

if [[ $# -ne 0 ]]; then
	usage >&2
	exit 2
fi

for command in gh jq; do
	if ! command -v "$command" > /dev/null; then
		echo "error: '$command' is required." >&2
		exit 2
	fi
done

temporary_directory=$(mktemp -d)
trap 'rm -rf "$temporary_directory"' EXIT

missing=0

check_backports ()
{
	local label=$1
	local target_branch=$2
	local sources_file="$temporary_directory/sources.json"
	local backports_file="$temporary_directory/backports.json"
	local results_file="$temporary_directory/results.json"

	gh pr list \
		--repo "$repository" \
		--state merged \
		--label "$label" \
		--limit 1000 \
		--json number,title,url,baseRefName > "$sources_file"

	gh pr list \
		--repo "$repository" \
		--state all \
		--base "$target_branch" \
		--limit 1000 \
		--json number,title,url,state,body > "$backports_file"

	jq \
		--arg branch "$target_branch" \
		--slurpfile backports "$backports_file" \
		'
			def references_source($source):
				(.body // "") | test(
					"(?i)backport of (?:https://github\\.com/[^/\\s]+/[^/\\s]+/pull/)?#?"
					+ ($source.number | tostring)
					+ "(?:[^0-9]|$)"
				);

			map(
				. as $source
				| if .baseRefName == $branch then
					. + { backport: ., result: "already-targets-branch" }
				else
					([ $backports [0][] | select(references_source($source)) ] | first) as $backport
					| . + {
						backport: $backport,
						result: if $backport == null then "missing" else "found" end
					}
				end
			)
		' "$sources_file" > "$results_file"

	printf '%s%s -> %s%s\n' "$cyan" "$label" "$target_branch" "$reset"
	if [[ $(jq length "$results_file") -eq 0 ]]; then
		printf '  %sNo merged pull requests have this label.%s\n' "$yellow" "$reset"
		return
	fi

	while IFS=$'\t' read -r result source_number source_url backport_number backport_state; do
		case "$result" in
		already-targets-branch)
			printf '  %s[OK]%s      #%s already targets %s\n' "$green" "$reset" "$source_number" "$target_branch"
			;;
		found)
			printf '  %s[OK]%s      #%s -> #%s (%s)\n' "$green" "$reset" "$source_number" "$backport_number" "$backport_state"
			;;
		missing)
			printf '  %s[MISSING]%s #%s has no backport PR: %s\n' "$red" "$reset" "$source_number" "$source_url"
			missing=1
			;;
		esac
	done < <(
		jq -r '
			.[] | [
				.result,
				.number,
				.url,
				(.backport.number // ""),
				(.backport.state // "")
			] | @tsv
		' "$results_file"
	)
	echo
}

check_backports "backport-to-net10.0" "$net10_branch"
check_backports "backport-to-net11.0" "$net11_branch"

if [[ $missing -ne 0 ]]; then
	printf '%sOne or more backports are missing.%s\n' "$red" "$reset" >&2
	exit 1
fi

printf '%sAll labeled pull requests have a backport.%s\n' "$green" "$reset"
