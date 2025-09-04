#!/bin/bash -eu

PLATFORM=$1
LEFT=$2
RIGHT=$3
OUTPUT_FILE=$4
OUTPUT_DIR=$5

LEFT_VERSION_FILE=$6
RIGHT_VERSION_FILE=$7
NUGET_PRERELEASE_IDENTIFIER=$8

rm -f "$OUTPUT_DIR/diff/Microsoft.$PLATFORM.non-breaking.txt"
rm -f "$OUTPUT_DIR/diff/Microsoft.$PLATFORM.breaking.txt"

if ! LEFT_VERSION=$(cat "$LEFT_VERSION_FILE"); then
	exit 1
fi

if ! RIGHT_VERSION=$(cat "$RIGHT_VERSION_FILE"); then
	exit 1
fi

STRICT_MODE=
if test -z "$NUGET_PRERELEASE_IDENTIFIER"; then
	if [[ "$RIGHT_VERSION" == "$LEFT_VERSION" ]]; then
		STRICT_MODE=--strict-mode
	fi
fi

if ! apicompat --left "$LEFT" --right "$RIGHT" $STRICT_MODE --suppression-file "suppression-files/$PLATFORM.xml" > "${OUTPUT_FILE}.tmp" 2>&1; then
	cat "${OUTPUT_FILE}.tmp"
	rm -f "${OUTPUT_FILE}.tmp"
	exit 1
fi

if grep "APICompat ran successfully without finding any breaking changes." "${OUTPUT_FILE}.tmp" > /dev/null 2>&1; then
	touch "$OUTPUT_DIR/diff/Microsoft.$PLATFORM.non-breaking.txt"
	echo "[$PLATFORM] ✅ No breaking change found."
elif grep "API breaking changes found." "${OUTPUT_FILE}.tmp" > /dev/null 2>&1; then
	touch "$OUTPUT_DIR/diff/Microsoft.$PLATFORM.breaking.txt"
	echo "[$PLATFORM] ❌ Found breaking changes:"
	cat "${OUTPUT_FILE}.tmp" | sed "s/^/    [$PLATFORM] /"
	echo "[$PLATFORM] ⚠️ Run 'make regenerate-suppression-files' to re-generate the suppression files if these breaking changes are intentional."
else
	echo "Unexpected response from apicompat, expected either 'APICompat ran successfully without finding any breaking changes.' or 'API breaking changes found.'"
	exit 1
fi

mv "${OUTPUT_FILE}.tmp" "${OUTPUT_FILE}"
