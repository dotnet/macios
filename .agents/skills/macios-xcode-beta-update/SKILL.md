---
name: macios-xcode-beta-update
description: Update dotnet/macios to a new Xcode beta and validate it end-to-end. Use this skill when a user asks to bump Xcode beta versions, update macios SDK/version constants, run xtro-sharpie sanitization, and run introspection tests for iOS/tvOS/macOS/Mac Catalyst.
---

# macios Xcode Beta Update

Run this workflow from the macios repository root.

## Inputs

Collect or confirm:
- Target Xcode version (for example `26.4`)
- Beta number (for example `2`)
- Xcode URL (for example `https://dl.internalx.com/internal-files/xcodes/Xcode_26.4_beta_2.xip`)
- Optional prior bump PR to mirror
- Whether to do this in two phases (non-test changes first, tests second)

If a prior PR is provided, mirror its file-touch pattern and intentionally skip/add tests based on user scope.

## File update scope

Apply minimal, surgical changes in these files:

1. `Make.config`
   - `NUGET_HARDCODED_PRERELEASE_IDENTIFIER=xcode<major.minor>`
   - `NUGET_HARDCODED_PRERELEASE_BRANCH=xcode<major.minor>`
   - `XCODE_VERSION=<major.minor>`
   - `XCODE_URL=<user-provided-xip-url>`
   - `XCODE_DEVELOPER_ROOT=/Applications/Xcode_<major.minor>.0-beta<beta>.app/Contents/Developer` (for betas)

2. `Make.versions`
   - Bump:
     - `IOS_NUGET_OS_VERSION`
     - `TVOS_NUGET_OS_VERSION`
     - `MACOS_NUGET_OS_VERSION`
     - `MACCATALYST_NUGET_OS_VERSION`

3. `builds/Versions-iOS.plist.in`
4. `builds/Versions-tvOS.plist.in`
5. `builds/Versions-macOS.plist.in`
6. `builds/Versions-MacCatalyst.plist.in`
   - Add the new version to `KnownVersions` and `SupportedTargetPlatformVersions`
   - For MacCatalyst also update `MacCatalystVersionMap`

7. `tools/common/SdkVersions.cs`
   - Bump Xcode/platform constants and executable/deployment max versions to the new beta OS version

8. `tools/devops/automation/templates/variables/common.yml`
   - Set `xcodeChannel` to `Beta` for beta/RC builds

## Validation workflow

### 1) Basic configuration check

Run:
```bash
make show-versions
```

### 2) Full build validation

Run:
```bash
make world
```

Do **not** bypass Xcode component checks by default.

If `check-system` fails because MetalToolchain is missing, install it and rerun:
```bash
xcrun xcodebuild -downloadComponent MetalToolchain
make world
```

If simulator runtimes are missing, provision and rerun:
```bash
./system-dependencies.sh --provision-simulators
make world
```

## XTRO workflow (test updates)

Run:
```bash
AUTO_SANITIZE=1 make -C tests/xtro-sharpie all
```

If it fails with unclassified entries, run:
```bash
make -C tests/xtro-sharpie unclassified2todo
AUTO_SANITIZE=1 make -C tests/xtro-sharpie all
```

Capture resulting `tests/xtro-sharpie/api-annotations-dotnet/*.todo` and `*.ignore` changes.

## Introspection workflow (all platforms)

Use explicit prebuild + run to avoid mobile run target issues:

```bash
make -C tests/introspection/dotnet build-ios run-ios
make -C tests/introspection/dotnet build-tvos run-tvos
make -C tests/introspection/dotnet build-macOS run-macOS
make -C tests/introspection/dotnet build-MacCatalyst run-MacCatalyst
```

These runs can take a long time; wait for completion and summarize outcomes per platform.

## Completion checklist

Before finishing, confirm all items:
- Non-test version bumps are complete and minimal
- `make world` succeeds without ignore flags
- XTRO sanitized run succeeds after applying `unclassified2todo` if needed
- Introspection passes for iOS, tvOS, macOS, and Mac Catalyst
- Final output includes: changed files, key commands run, and pass/fail summary

## Output format

When reporting results, use this structure:

1. **Files changed** (group non-test and test changes)
2. **Validation commands run** (with pass/fail)
3. **Platform test summary**:
   - iOS
   - tvOS
   - macOS
   - Mac Catalyst
4. **Any follow-up required**
