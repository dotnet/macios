#!/bin/bash -e

if test -z "${XCODE_CHANNEL:-}"; then
	echo "The environment variable XCODE_CHANNEL must be specified."
	exit 1
fi

cd "$(dirname "${BASH_SOURCE[0]}")/../../.."

FILE=$(pwd)/tmp.txt

make print-variable-value-to-file FILE="$FILE" VARIABLE=XCODE_IS_PREVIEW
XCODE_IS_PREVIEW=$(cat "$FILE")

make print-variable-value-to-file FILE="$FILE" VARIABLE=XCODE_IS_STABLE
XCODE_IS_STABLE=$(cat "$FILE")

rm -f "$FILE"

if [[ "$XCODE_IS_PREVIEW $XCODE_IS_STABLE" == "true false" ]]; then
	if [[ "${XCODE_CHANNEL}" != "Beta" ]]; then
		echo "XCODE_CHANNEL must be 'Beta' (not '$XCODE_CHANNEL') when XCODE_IS_PREVIEW=$XCODE_IS_PREVIEW (and XCODE_IS_STABLE=$XCODE_IS_STABLE)"
		exit 1
	fi
elif [[ "$XCODE_IS_PREVIEW $XCODE_IS_STABLE" == "false true" ]]; then
	if [[ "${XCODE_CHANNEL}" != "Stable" ]]; then
		echo "XCODE_CHANNEL must be 'Stable' (not '$XCODE_CHANNEL') when XCODE_IS_STABLE=$XCODE_IS_STABLE (and XCODE_IS_PREVIEW=$XCODE_IS_PREVIEW)"
		exit 1
	fi
else
	echo "Unexpected values for XCODE_IS_STABLE ($XCODE_IS_STABLE) and XCODE_IS_PREVIEW ($XCODE_IS_PREVIEW): must be either 'true' or 'false', and not equal to eachother."
	exit 1
fi

echo "The current Xcode channel ('$XCODE_CHANNEL') is correct."
