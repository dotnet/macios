#!/bin/bash -eu

set -o pipefail

# This script generates the LocProject.json file for OneLocBuild.
# It includes both the static .resx entries and dynamically discovered
# template JSON files (templatestrings.en.json).

WORKING_DIRECTORY="${1:-}"
TOP="${2:-}"

if [ -z "$WORKING_DIRECTORY" ] || [ -z "$TOP" ]; then
	echo "Usage: $0 <working-directory> <top-directory>" >&2
	exit 1
fi

# Start with the static .resx entries
cat <<EOF
{
	"Projects": [
	  {
		"LanguageSet": "VS_Main_Languages",
		"LocItems": [
		  {
			"SourceFile": "$WORKING_DIRECTORY/../../msbuild/Xamarin.Localization.MSBuild/MSBStrings.resx",
			"Languages": "",
			"CopyOption": "LangIDOnName",
			"OutputPath": "$WORKING_DIRECTORY/../../msbuild/Xamarin.Localization.MSBuild/TranslatedAssemblies",
			"LclFile": "",
			"LciFile": "",
			"Parser": "",
			"LssFiles": []
		  },
		  {
			"SourceFile": "$WORKING_DIRECTORY/../../tools/mtouch/Errors.resx",
			"Languages": "",
			"CopyOption": "LangIDOnName",
			"OutputPath": "$WORKING_DIRECTORY/../../tools/mtouch/TranslatedAssemblies",
			"LclFile": "",
			"LciFile": "",
			"Parser": "",
			"LssFiles": []
		  },
		  {
			"SourceFile": "$WORKING_DIRECTORY/../../src/Resources.resx",
			"Languages": "",
			"CopyOption": "LangIDOnName",
			"OutputPath": "$WORKING_DIRECTORY/../../src/TranslatedAssemblies",
			"LclFile": "",
			"LciFile": "",
			"Parser": "",
			"LssFiles": []
		  },
		  {
			"SourceFile": "$WORKING_DIRECTORY/../../src/rgen/Microsoft.Macios.Generator/Resources.resx",
			"Languages": "",
			"CopyOption": "LangIDOnName",
			"OutputPath": "$WORKING_DIRECTORY/../../src/rgen/Microsoft.Macios.Generator/TranslatedAssemblies",
			"LclFile": "",
			"LciFile": "",
			"Parser": "",
			"LssFiles": []
		  },
		  {
			"SourceFile": "$WORKING_DIRECTORY/../../src/rgen/Microsoft.Macios.Bindings.Analyzer/Resources.resx",
			"Languages": "",
			"CopyOption": "LangIDOnName",
			"OutputPath": "$WORKING_DIRECTORY/../../src/rgen/Microsoft.Macios.Bindings.Analyzer/TranslatedAssemblies",
			"LclFile": "",
			"LciFile": "",
			"Parser": "",
			"LssFiles": []
		  },
		  {
			"SourceFile": "$WORKING_DIRECTORY/../../tools/sharpie/Sharpie.Bind/Resources.resx",
			"Languages": "",
			"CopyOption": "LangIDOnName",
			"OutputPath": "$WORKING_DIRECTORY/../../tools/sharpie/Sharpie.Bind/TranslatedAssemblies",
			"LclFile": "",
			"LciFile": "",
			"Parser": "",
			"LssFiles": []
EOF

# Add entries for all template localization JSON files
ABSTOP=$(cd "$TOP" && pwd)
find "$ABSTOP/dotnet/Templates" -name 'templatestrings.en.json' | sort | while read -r f; do
	dir=$(dirname "$f")
	cat <<EOF
		  },
		  {
			"SourceFile": "$f",
			"Languages": "",
			"CopyOption": "LangIDOnName",
			"OutputPath": "$dir",
			"LclFile": "",
			"LciFile": "",
			"Parser": "",
			"LssFiles": []
EOF
done

# Close the JSON structure
cat <<EOF
		  }
		],
		"LssFiles": [],
		"CloneLanguageSet": ""
	  }
	]
}
EOF
