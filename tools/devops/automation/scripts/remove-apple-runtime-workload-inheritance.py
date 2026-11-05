#!/usr/bin/env python3
# Copyright (c) Microsoft Corporation.
# Licensed under the MIT License.

import json
import os
from pathlib import Path
import re
import shutil


manifest_root = Path(os.environ["DOTNET_ROOT"]) / "sdk-manifests"
log_root = Path(os.environ["DIAGNOSTIC_DIRECTORY"]) / "logs"
manifests = {}
workloads = {}

for path in sorted(manifest_root.rglob("WorkloadManifest.json")):
    # Workload manifests permit comments and trailing commas; preserve string tokens.
    text = re.sub(
        r'("(?:\\.|[^"\\])*")|//[^\r\n]*|/\*.*?\*/|,\s*(?=[}\]])',
        lambda match: match.group(1) or "",
        path.read_text(encoding="utf-8-sig"),
        flags=re.DOTALL,
    )
    manifest = json.loads(text)
    manifests[path] = manifest
    for name, workload in manifest.get("workloads", {}).items():
        if name in workloads:
            raise RuntimeError(f"Duplicate workload definition: {name}")
        workloads[name] = (path, workload)


def shared_tooling(name, visited):
    if name in visited:
        return set()
    visited.add(name)
    _, workload = workloads[name]
    packs = {
        pack for pack in workload.get("packs", [])
        if pack.startswith("Microsoft.NET.Runtime.MonoTargets.Sdk")
    }
    for parent in workload.get("extends", []):
        packs.update(shared_tooling(parent, visited))
    return packs


changes = []
for name in ("ios", "tvos", "maccatalyst", "macos"):
    path, workload = workloads[name]
    tooling = sorted(shared_tooling(name, set()))
    if not tooling:
        raise RuntimeError(f"No shared MonoTargets.Sdk tooling found for {name}")
    changes.append((name, path, tooling))

for path in sorted({path for _, path, _ in changes}):
    backup = log_root / "original-manifests" / path.relative_to(manifest_root)
    backup.parent.mkdir(parents=True, exist_ok=True)
    shutil.copyfile(path, backup)

for name, path, tooling in changes:
    workload = workloads[name][1]
    print(json.dumps({
        "workload": name,
        "removed-extends": workload.get("extends", []),
        "retained-shared-tooling": tooling,
    }), flush=True)
    workload.pop("extends", None)
    packs = workload.setdefault("packs", [])
    packs.extend(pack for pack in tooling if pack not in packs)

for path in sorted({path for _, path, _ in changes}):
    text = json.dumps(manifests[path], indent=2) + "\n"
    path.write_text(text, encoding="utf-8")
    snapshot = log_root / "modified-manifests" / path.relative_to(manifest_root)
    snapshot.parent.mkdir(parents=True, exist_ok=True)
    snapshot.write_text(text, encoding="utf-8")
