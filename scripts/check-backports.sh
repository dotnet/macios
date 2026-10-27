#!/bin/bash

# Copyright (c) Microsoft Corporation.
# Licensed under the MIT License.

set -euo pipefail

repository=${REPOSITORY:-dotnet/macios}
remote=${REMOTE:-origin}
net10_branch=${NET10_BRANCH:-release/10.0.1xx}
net11_branch=${NET11_BRANCH:-release/11.0.1xx}

red=""
green=""
blue=""
yellow=""
cyan=""
reset=""
if [[ -t 1 && -z ${NO_COLOR:-} && ${TERM:-} != "dumb" ]]; then
	red=$'\033[31m'
	green=$'\033[32m'
	blue=$'\033[34m'
	yellow=$'\033[33m'
	cyan=$'\033[36m'
	reset=$'\033[0m'
fi

usage ()
{
	cat <<EOF
Usage: $(basename "$0")

Verify that every merged pull request with a backport label has a pull request
targeting the corresponding release branch, or a matching backport commit in
that branch's history. Open, closed, and merged backport pull requests all
count.

Environment variables:
  REPOSITORY    GitHub repository (default: $repository)
  REMOTE        Git remote to fetch release branches from (default: $remote)
  NET10_BRANCH  .NET 10 target branch (default: $net10_branch)
  NET11_BRANCH  .NET 11 target branch (default: $net11_branch)
  NO_COLOR      Disable colored output when set
EOF
}

if [[ $# -eq 1 && (${1:-} == "--help" || ${1:-} == "-h") ]]; then
	usage
	exit 0
fi

if [[ $# -ne 0 ]]; then
	usage >&2
	exit 2
fi

for command in gh git jq; do
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
	local target_ref
	local sources_file="$temporary_directory/sources.json"
	local backports_file="$temporary_directory/backports.json"
	local history_file="$temporary_directory/history.json"
	local results_file="$temporary_directory/results.json"
	local message prefix source_title index remaining
	local message_width=0
	local messages=() prefixes=() titles=()

	git fetch "$remote" "$target_branch" --quiet
	target_ref=FETCH_HEAD

	# shellcheck disable=SC2016 # GraphQL variables are not shell variables.
	gh api graphql \
		--paginate \
		--slurp \
		-F owner="${repository%/*}" \
		-F name="${repository#*/}" \
		-f label="$label" \
		-f query='
			query($owner: String!, $name: String!, $label: String!, $endCursor: String) {
				repository(owner: $owner, name: $name) {
					pullRequests(first: 100, after: $endCursor, labels: [$label], states: MERGED, orderBy: {field: CREATED_AT, direction: DESC}) {
						nodes { number title url baseRefName }
						pageInfo { hasNextPage endCursor }
					}
				}
			}
		' |
		jq '[.[].data.repository.pullRequests.nodes[]]' > "$sources_file"

	# shellcheck disable=SC2016 # GraphQL variables are not shell variables.
	gh api graphql \
		--paginate \
		--slurp \
		-F owner="${repository%/*}" \
		-F name="${repository#*/}" \
		-f branch="$target_branch" \
		-f query='
			query($owner: String!, $name: String!, $branch: String!, $endCursor: String) {
				repository(owner: $owner, name: $name) {
					pullRequests(first: 100, after: $endCursor, baseRefName: $branch, orderBy: {field: CREATED_AT, direction: DESC}) {
						nodes { number title url state body }
						pageInfo { hasNextPage endCursor }
					}
				}
			}
		' |
		jq '[.[].data.repository.pullRequests.nodes[]]' > "$backports_file"

	git log --format='%H%x1f%B%x1e' "$target_ref" |
		jq -Rs '
			split("\u001e") |
			map(
				ltrimstr("\n") |
				select(length > 0) |
				split("\u001f") |
				{
					commit: .[0],
					message: (.[1:] | join("\u001f"))
				}
			)
		' > "$history_file"

	jq \
		--arg branch "$target_branch" \
		--slurpfile backports "$backports_file" \
		--slurpfile history "$history_file" \
		'
			def index_backports($items; $field):
				reduce $items[] as $item (
					{};
					reduce (
						($item[$field] // "") |
						scan("(?i)backport of (?:https://github\\.com/[^/\\s]+/[^/\\s]+/pull/)?#?([0-9]+)") |
						.[0]
					) as $number (
						.;
						.[$number] //= $item
					)
				);

			index_backports($backports[0]; "body") as $backports_by_source |
			index_backports($history[0]; "message") as $commits_by_source |
			map(
				. as $source
				| if .baseRefName == $branch then
					. + { backport: ., result: "already-targets-branch" }
				else
					($backports_by_source[$source.number | tostring]) as $backport
					| ($commits_by_source[$source.number | tostring]) as $commit
					| . + {
						backport: $backport,
						commit: $commit,
						result: (
							if $backport != null then "found"
							elif $commit != null then "found-in-history"
							else "missing"
							end
						)
					}
				end
			)
		' "$sources_file" > "$results_file"

	printf '%s%s -> %s%s\n' "$cyan" "$label" "$target_branch" "$reset"
	if [[ $(jq length "$results_file") -eq 0 ]]; then
		printf '  %sNo merged pull requests have this label.%s\n' "$yellow" "$reset"
		return
	fi

	while IFS=$'\t' read -r result source_number backport_number backport_state history_commit source_title; do
		prefix="${green}[OK]$reset      "
		case "$result" in
		already-targets-branch)
			printf -v message '#%s already targets %s' "$source_number" "$target_branch"
			;;
		found)
			printf -v message '#%s -> #%s (%s)' "$source_number" "$backport_number" "$backport_state"
			;;
		found-in-history)
			printf -v message '#%s -> commit %.12s (in %s history)' "$source_number" "$history_commit" "$target_branch"
			;;
		missing)
			prefix="${red}[MISSING]$reset "
			printf -v message '#%s has no backport PR or commit' "$source_number"
			missing=1
			;;
		esac
		prefixes+=("$prefix")
		messages+=("$message")
		titles+=("$source_title")
		if [[ ${#message} -gt $message_width ]]; then
			message_width=${#message}
		fi
	done < <(
		jq -r '
			.[] | [
				.result,
				.number,
				(.backport.number // "-"),
				(.backport.state // "-"),
				(.commit.commit // "-"),
				.title
			] | @tsv
		' "$results_file"
	)
	for index in "${!messages[@]}"; do
		message=${messages[$index]}
		printf '  %s' "${prefixes[$index]}"
		remaining=$message
		while [[ $remaining =~ ^([^#]*)#([0-9]+)(.*)$ ]]; do
			printf '%s' "${BASH_REMATCH[1]}"
			if [[ -t 1 && ${TERM:-} != "dumb" ]]; then
				printf '\033]8;;https://github.com/%s/pull/%s\a#%s\033]8;;\a' "$repository" "${BASH_REMATCH[2]}" "${BASH_REMATCH[2]}"
			else
				printf '#%s' "${BASH_REMATCH[2]}"
			fi
			remaining=${BASH_REMATCH[3]}
		done
		remaining=${remaining//"(MERGED)"/"(${green}MERGED${reset})"}
		remaining=${remaining//"(OPEN)"/"(${blue}OPEN${reset})"}
		printf '%s%*s  %s\n' "$remaining" "$((message_width - ${#message}))" "" "${titles[$index]}"
	done
	echo
}

check_backports "backport-to-net11.0" "$net11_branch"
check_backports "backport-to-net10.0" "$net10_branch"

if [[ $missing -ne 0 ]]; then
	printf '%sOne or more backports are missing.%s\n' "$red" "$reset" >&2
	exit 1
fi

printf '%sAll labeled pull requests have a backport.%s\n' "$green" "$reset"
