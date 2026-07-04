#!/bin/bash -eu

# Copyright (c) Microsoft Corporation.
# Licensed under the MIT License.

# This script updates the target framework values in the project templates
# for a new .NET version.
#
# It updates:
#   - template.json: choices, defaultValue, and description fields
#   - localize/*.json: Framework choice keys and descriptions
#
# The 'replaces' field in template.json and the <TargetFramework> in .csproj/.vbproj/.fsproj
# files use the fixed placeholder 'DOTNET_TFM' and do not need updating.
#
# Usage: ./dotnet/update-tfm-in-templates.sh
#
# The script reads DOTNET_TFM from Make.config to determine the current .NET version,
# then computes which TFMs to offer based on the support policy:
#   - The previous TFM is supported until May 15th of the year after the current
#     major .NET version was released.
#   - At most two TFMs are offered (current + previous, if still supported).

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
REPO_ROOT="$(cd "$SCRIPT_DIR/.." && pwd)"

# Read DOTNET_TFM from Make.config
DOTNET_TFM=$(grep '^DOTNET_TFM=' "$REPO_ROOT/Make.config" | head -1 | cut -d= -f2)
if [ -z "$DOTNET_TFM" ]; then
    echo "Error: Could not read DOTNET_TFM from Make.config"
    exit 1
fi

# Extract major version number (e.g., "net10.0" -> 10)
CURRENT_MAJOR=$(echo "$DOTNET_TFM" | sed -E 's/net([0-9]+)\.0/\1/')
PREVIOUS_MAJOR=$((CURRENT_MAJOR - 1))

# Determine if the previous TFM should still be offered.
# Policy: previous TFM is supported until May 15th of the year after the
# current major .NET version was released.
# .NET versions are released in November of their version year (e.g., .NET 10
# was released in November 2025), so the previous TFM is supported until
# May 15th of the following year (e.g., May 15th, 2026 for .NET 10).
SUPPORT_CUTOFF_YEAR=$((CURRENT_MAJOR + 2016))
SUPPORT_CUTOFF_DATE="${SUPPORT_CUTOFF_YEAR}-05-15"
TODAY=$(date +%Y-%m-%d)

INCLUDE_PREVIOUS=false
if [[ "$TODAY" < "$SUPPORT_CUTOFF_DATE" ]]; then
    INCLUDE_PREVIOUS=true
fi

PREVIOUS_TFM="net${PREVIOUS_MAJOR}.0"

echo "Current DOTNET_TFM: $DOTNET_TFM (major: $CURRENT_MAJOR)"
echo "Previous TFM: $PREVIOUS_TFM (major: $PREVIOUS_MAJOR)"
echo "Support cutoff for previous TFM: $SUPPORT_CUTOFF_DATE"
echo "Today: $TODAY"
echo "Include previous TFM: $INCLUDE_PREVIOUS"
echo ""

TEMPLATES_DIR="$REPO_ROOT/dotnet/Templates"

# Map template directories to their platform suffix
get_platform () {
    case "$1" in
        Microsoft.iOS.Templates) echo "ios" ;;
        Microsoft.tvOS.Templates) echo "tvos" ;;
        Microsoft.macOS.Templates) echo "macos" ;;
        Microsoft.MacCatalyst.Templates) echo "maccatalyst" ;;
    esac
}

TEMPLATE_DIRS="Microsoft.iOS.Templates Microsoft.tvOS.Templates Microsoft.macOS.Templates Microsoft.MacCatalyst.Templates"

update_template_json () {
    local file="$1"
    local platform="$2"
    local current_tfm="${DOTNET_TFM}-${platform}"
    local previous_tfm="${PREVIOUS_TFM}-${platform}"
    local include_previous="$INCLUDE_PREVIOUS"

    python3 - "$file" "$current_tfm" "$previous_tfm" "$include_previous" << 'PYTHON_EOF'
import re
import sys
import codecs
import json

file_path = sys.argv[1]
current_tfm = sys.argv[2]
previous_tfm = sys.argv[3]
include_previous = sys.argv[4] == "true"

with open(file_path, 'rb') as f:
    raw = f.read()

# Detect and preserve BOM
has_bom = raw.startswith(codecs.BOM_UTF8)
if has_bom:
    text = raw.decode('utf-8-sig')
else:
    text = raw.decode('utf-8')

if '"Framework"' not in text:
    sys.exit(0)

# Find the Framework block and replace choices/defaultValue within it only.
# Locate "Framework": { ... } by finding its start and matching braces.
fw_match = re.search(r'"Framework"\s*:\s*\{', text)
if not fw_match:
    sys.exit(0)

# Find the matching closing brace for the Framework object
start = fw_match.end() - 1  # position of the opening {
depth = 0
end = start
for i in range(start, len(text)):
    if text[i] == '{':
        depth += 1
    elif text[i] == '}':
        depth -= 1
        if depth == 0:
            end = i + 1
            break

fw_text = text[start:end]

# Build the new choices block
if include_previous:
    new_choices = (
        '[\n'
        '        {\n'
        f'          "choice": "{current_tfm}",\n'
        f'          "description": "Target {current_tfm}"\n'
        '        },\n'
        '        {\n'
        f'          "choice": "{previous_tfm}",\n'
        f'          "description": "Target {previous_tfm}"\n'
        '        }\n'
        '      ]'
    )
else:
    new_choices = (
        '[\n'
        '        {\n'
        f'          "choice": "{current_tfm}",\n'
        f'          "description": "Target {current_tfm}"\n'
        '        }\n'
        '      ]'
    )

# Replace choices and defaultValue within the Framework block only
fw_text = re.sub(
    r'("choices"\s*:\s*)\[.*?\]',
    r'\g<1>' + new_choices,
    fw_text,
    count=1,
    flags=re.DOTALL
)
fw_text = re.sub(
    r'("defaultValue"\s*:\s*)"[^"]*"',
    rf'\g<1>"{current_tfm}"',
    fw_text,
    count=1
)

text = text[:start] + fw_text + text[end:]

with open(file_path, 'wb') as f:
    if has_bom:
        f.write(codecs.BOM_UTF8)
    f.write(text.encode('utf-8'))
PYTHON_EOF
}

update_localize_json () {
    local file="$1"
    local platform="$2"
    local current_tfm="${DOTNET_TFM}-${platform}"
    local previous_tfm="${PREVIOUS_TFM}-${platform}"
    local include_previous="$INCLUDE_PREVIOUS"

    python3 - "$file" "$current_tfm" "$previous_tfm" "$include_previous" << 'PYTHON_EOF'
import re
import sys
import codecs

file_path = sys.argv[1]
current_tfm = sys.argv[2]
previous_tfm = sys.argv[3]
include_previous = sys.argv[4] == "true"

with open(file_path, 'rb') as f:
    raw = f.read()

# Detect and preserve BOM
has_bom = raw.startswith(codecs.BOM_UTF8)
if has_bom:
    text = raw.decode('utf-8-sig')
else:
    text = raw.decode('utf-8')

had_trailing_newline = text.endswith('\n')

# Remove all existing Framework choice lines
lines = text.split('\n')
result = []
for line in lines:
    if '"symbols/Framework/choices/' in line:
        continue
    result.append(line)
text = '\n'.join(result)

# Build replacement lines
new_lines = f'  "symbols/Framework/choices/{current_tfm}/description": "Target {current_tfm}"'
if include_previous:
    new_lines += f',\n  "symbols/Framework/choices/{previous_tfm}/description": "Target {previous_tfm}"'

# Insert before the closing }
# Find the last closing brace and insert before it
lines = text.split('\n')
# Find the index of the last line that is just '}'
close_idx = None
for i in range(len(lines) - 1, -1, -1):
    if lines[i].strip() == '}':
        close_idx = i
        break

if close_idx is not None:
    # Ensure the previous content line has a trailing comma
    for j in range(close_idx - 1, -1, -1):
        stripped = lines[j].strip()
        if stripped and stripped != '{':
            if not stripped.endswith(','):
                lines[j] = lines[j].rstrip() + ','
            break
    lines.insert(close_idx, new_lines)

text = '\n'.join(lines)

# Preserve original trailing newline behavior
if had_trailing_newline and not text.endswith('\n'):
    text += '\n'
elif not had_trailing_newline and text.endswith('\n'):
    text = text.rstrip('\n')

with open(file_path, 'wb') as f:
    if has_bom:
        f.write(codecs.BOM_UTF8)
    f.write(text.encode('utf-8'))
PYTHON_EOF
}

for template_dir_name in $TEMPLATE_DIRS; do
    platform=$(get_platform "$template_dir_name")
    template_base="$TEMPLATES_DIR/$template_dir_name"

    if [ ! -d "$template_base" ]; then
        echo "Warning: $template_base not found, skipping."
        continue
    fi

    echo "Processing $template_dir_name (platform: $platform)..."

    # Find and update all template.json files
    while IFS= read -r -d '' template_json; do
        # Only process templates that have a Framework parameter
        if ! grep -q '"Framework"' "$template_json"; then
            continue
        fi

        echo "  Updating: $template_json"
        update_template_json "$template_json" "$platform"

        # Update localize files in the same .template.config directory
        localize_dir="$(dirname "$template_json")/localize"
        if [ -d "$localize_dir" ]; then
            for localize_file in "$localize_dir"/templatestrings.*.json; do
                if [ -f "$localize_file" ]; then
                    update_localize_json "$localize_file" "$platform"
                fi
            done
        fi
    done < <(find "$template_base" -path '*/.template.config/template.json' -print0)
done

echo ""
echo "Done. Templates updated for $DOTNET_TFM."
if [ "$INCLUDE_PREVIOUS" = true ]; then
    echo "Previous TFM ($PREVIOUS_TFM) is included as an option (cutoff: $SUPPORT_CUTOFF_DATE)."
else
    echo "Previous TFM ($PREVIOUS_TFM) is NOT included (past cutoff: $SUPPORT_CUTOFF_DATE)."
fi
