#!/bin/bash
# Copyright (c) Microsoft Corporation.
# Licensed under the MIT License.

set -euo pipefail

dotnet=$1
test_directory=$(cd "$(dirname "$0")" && pwd)
log_directory=$(mktemp -d)
trap 'rm -r "$log_directory"' EXIT

for platform in iOS tvOS macOS MacCatalyst; do
	for project_type in Library Exe Binding; do
		output_type=$project_type
		is_binding_project=
		if [[ "$project_type" == Binding ]]; then
			output_type=Library
			is_binding_project=true
		fi

		log="$log_directory/$platform-$project_type.log"
		succeeded=false
		if "$dotnet" msbuild "$test_directory/BuildHost.proj" -nologo -v:minimal \
			-p:_PlatformName="$platform" -p:OutputType="$output_type" -p:IsBindingProject="$is_binding_project" > "$log" 2>&1; then
			succeeded=true
		fi

		if [[ "$(uname -s)" == Linux ]]; then
			expected="Building .NET for $platform projects on Linux is not supported. Use macOS, or Windows with a connection to a Mac, to build this project."
			if [[ "$succeeded" == true ]] || ! grep -Fq "$expected" "$log" || grep -Fq "Build host check passed." "$log"; then
				echo "FAIL: $platform $project_type: expected the unsupported Linux host error before executing Build."
				cat "$log"
				exit 1
			fi
		else
			if [[ "$succeeded" != true ]] || ! grep -Fq "Build host check passed." "$log"; then
				echo "FAIL: $platform $project_type: expected the supported host check to succeed."
				cat "$log"
				exit 1
			fi
		fi
		echo "PASS: $platform $project_type build host check"
	done
done
