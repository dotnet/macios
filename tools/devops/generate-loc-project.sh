#!/bin/bash -eu

set -o pipefail

# Generates the checked-in OneLocBuild project, including template localization files.
cd "$(dirname "$0")/../.."

cat <<'EOF'
{
  "Projects": [
    {
      "LanguageSet": "VS_Main_Languages",
      "LocItems": [
EOF

separator=""
emit_loc_item () {
	local source="$1"
	local output="$2"
	local lcl="$3"

	printf '%s' "$separator"
	separator=$',\n'
	cat <<EOF
        {
          "SourceFile": "$source",
          "Languages": "",
          "CopyOption": "LangIDOnName",
          "OutputPath": "$output",
          "LclFile": "$lcl",
          "LciFile": "",
          "Parser": "",
          "LssFiles": []
EOF
	printf '        }'
}

for source in \
	msbuild/Xamarin.Localization.MSBuild/MSBStrings.resx \
	tools/mtouch/Errors.resx \
	src/Resources.resx \
	src/rgen/Microsoft.Macios.Generator/Resources.resx \
	src/rgen/Microsoft.Macios.Bindings.Analyzer/Resources.resx \
	tools/sharpie/Sharpie.Bind/Resources.resx
do
	emit_loc_item "$source" "${source%/*}/TranslatedAssemblies" "Localize/loc/{Lang}/$source.lcl"
done

find dotnet/Templates -name 'templatestrings.en.json' | LC_ALL=C sort | while IFS= read -r source; do
	emit_loc_item "$source" "${source%/*}" "Localize/loc/{Lang}/${source%.en.json}.json.lcl"
done

printf '\n'
cat <<'EOF'
      ],
      "LssFiles": [],
      "CloneLanguageSet": ""
    }
  ]
}
EOF
