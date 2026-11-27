#!/bin/bash -eu

set -o pipefail

# Generates eng/Localize/LocProject.json on stdout, including template localization files.
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

	if [ ! -f "$source" ]; then
		printf 'Missing localization source: %s\n' "$source" >&2
		return 1
	fi

	# Hand-backs still use paths from the old s/macios checkout.
	local lcl="macios/Localize/loc/{Lang}/macios/$source.lcl"

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
	emit_loc_item "$source" "${source%/*}/TranslatedAssemblies"
done

find dotnet/Templates -name 'templatestrings.en.json' | LC_ALL=C sort | while IFS= read -r source; do
	emit_loc_item "$source" "${source%/*}"
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
