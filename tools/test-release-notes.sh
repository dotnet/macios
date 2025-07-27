#!/bin/bash
# Test script for release notes generation
# Usage: ./test-release-notes.sh [current_tag] [previous_tag]

set -e

CURRENT_TAG="${1:-xamarin-mac-9.3.0.23}"
PREVIOUS_TAG="${2:-xamarin-mac-9.3.0.18}"

echo "🧪 Testing Release Notes Generation"
echo "=================================="
echo "Current tag: $CURRENT_TAG"
echo "Previous tag: $PREVIOUS_TAG"
echo ""

# Test 1: Tag detection logic
echo "✅ Test 1: Tag detection logic"
if [[ "$CURRENT_TAG" == xamarin-mac-* ]]; then
    ALL_TAGS=$(git tag -l "xamarin-mac-*" | sort -V)
    DETECTED_PREV=$(echo "$ALL_TAGS" | grep -B1 "^$CURRENT_TAG$" | head -n1)
    echo "   Detected previous tag: $DETECTED_PREV"
    if [ "$DETECTED_PREV" = "$PREVIOUS_TAG" ]; then
        echo "   ✅ Tag detection working correctly"
    else
        echo "   ⚠️  Tag detection mismatch (expected: $PREVIOUS_TAG, got: $DETECTED_PREV)"
    fi
else
    echo "   ℹ️  Using generic tag detection for non-xamarin-mac tags"
fi
echo ""

# Test 2: Commit extraction
echo "✅ Test 2: Commit extraction"
COMMIT_COUNT=$(git log --oneline --grep="#[0-9]" "$PREVIOUS_TAG..$CURRENT_TAG" | wc -l)
echo "   Found $COMMIT_COUNT commits with PR references"
if [ "$COMMIT_COUNT" -gt 0 ]; then
    echo "   ✅ Commit extraction working"
else
    echo "   ⚠️  No commits with PR references found"
fi
echo ""

# Test 3: PR number extraction
echo "✅ Test 3: PR number extraction"
git log --oneline --grep="#[0-9]" "$PREVIOUS_TAG..$CURRENT_TAG" | head -3 | while read -r line; do
    PR_NUM=$(echo "$line" | grep -o '#[0-9]\+' | head -1 | sed 's/#//')
    if [ -n "$PR_NUM" ]; then
        echo "   ✅ Extracted PR #$PR_NUM from: $(echo "$line" | cut -c1-50)..."
    fi
done
echo ""

# Test 4: Generate sample release notes
echo "✅ Test 4: Sample release notes generation"
echo "   Generated release notes:"
echo "   ========================"

cat << EOF
## What's Changed

### Changes since $PREVIOUS_TAG

EOF

git log --oneline --grep="#[0-9]" "$PREVIOUS_TAG..$CURRENT_TAG" | while read -r line; do
    PR_NUM=$(echo "$line" | grep -o '#[0-9]\+' | head -1 | sed 's/#//')
    if [ -n "$PR_NUM" ]; then
        COMMIT_MSG=$(echo "$line" | sed 's/^[a-f0-9]\+ //')
        echo "- $COMMIT_MSG"
    fi
done

echo ""
echo "**Full Changelog**: https://github.com/dotnet/macios/compare/$PREVIOUS_TAG...$CURRENT_TAG"
echo ""
echo "🎉 Release notes generation test complete!"