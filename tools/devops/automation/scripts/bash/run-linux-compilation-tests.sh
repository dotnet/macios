#!/bin/bash
set -euo pipefail

cd "$BUILD_SOURCESDIRECTORY/$BUILD_REPOSITORY_TITLE"
./configure --enable-ios --enable-tvos --enable-maccatalyst --enable-macos --no-xcode
MACIOS_UPLOAD_PREFIX="" bash tools/devops/automation/scripts/bash/install-workloads.sh
make -C tests test.config
dotnet_path=$(make --no-print-directory -s -C tools/devops print-variable VARIABLE=DOTNET)
dotnet_path="${dotnet_path#*=}"
"$dotnet_path" test tests/dotnet/UnitTests/DotNetUnitTests.csproj \
    --filter 'TestCategory=Linux' \
    --logger 'trx;LogFileName=linux-compilation.trx' \
    --results-directory "$BUILD_SOURCESDIRECTORY/linux-test-results"

python3 - "$BUILD_SOURCESDIRECTORY/linux-test-results/linux-compilation.trx" <<'PY'
import sys
import xml.etree.ElementTree as ET

root = ET.parse(sys.argv[1]).getroot()
namespace = {"trx": "http://microsoft.com/schemas/VisualStudio/TeamTest/2010"}
counters = root.find("trx:ResultSummary/trx:Counters", namespace)
if counters is None or counters.get("total") != "4" or counters.get("passed") != "4":
    raise RuntimeError(f"Expected four passing Linux compilation tests, got {None if counters is None else counters.attrib}")
PY
