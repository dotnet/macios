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
- Azure Artifacts package version (for example `26.4.0-beta.2`)
- Xcode product build version (for example `17E5212f`)
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
   - `XCODE_PACKAGE_NAME=xcode-apple-silicon`
   - `XCODE_PACKAGE_VERSION=<major.minor.patch-beta.number>`
   - `XCODE_BUILD_VERSION=<ProductBuildVersion>`
   - `XCODE_URL=<user-provided-xip-url>`
   - `XCODE_DEVELOPER_ROOT=/Applications/Xcode_<major.minor>.0-beta<beta>.app/Contents/Developer` (for betas)

   `XCODE_DEVELOPER_ROOT` must name the same application that `XCODE_PACKAGE_VERSION`
   implies; `install-xcode.sh` refuses to run when the two disagree. The package version is
   strict SemVer (Azure Artifacts requires it) while the application name follows this
   repository's convention, so they are deliberately spelled differently:

   | Release | `XCODE_PACKAGE_VERSION` | `XCODE_DEVELOPER_ROOT` application |
   | --- | --- | --- |
   | beta 1 | `27.0.0-beta` | `Xcode_27.0.0-beta.app` |
   | beta 3 | `27.0.0-beta.3` | `Xcode_27.0.0-beta3.app` |
   | RC 1 | `27.0.0-rc` | `Xcode_27.0.0-rc.app` |
   | RC 2 | `27.0.0-rc.2` | `Xcode_27.0.0-rc.2.app` |
   | stable | `27.0.0` | `Xcode_27.0.0.app` |
   | patch | `27.0.1` | `Xcode_27.0.1.app` (`XCODE_VERSION` stays `27.0`) |

   Betas drop the dot before the ordinal, release candidates keep it, and the first beta or
   RC carries no ordinal at all. `Xcode_26.3.0-rc2.app` is wrong: it has been written twice
   and corrected both times. Note that `XCODE_BUILD_VERSION` is what actually distinguishes
   one beta or RC from the next, so a stale value passes every path check and only fails
   once the package is installed.

   Before queuing CI, verify that the matching immutable package exists — with its
   Apple-signed XIP and `xcode-metadata.json` — in **both** feeds, because the two
   pipelines resolve the same coordinates in their own organization:
   - `devdiv/DevDiv/macios-tools-internal`
   - `dnceng/internal/macios-tools-internal`

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

Use explicit prebuild + run to avoid mobile run target issues.

**IMPORTANT: Run platforms sequentially, not in parallel.** The shared `obj/` directories
(`tests/common/Touch.Unit/Touch.Client/dotnet/obj` and `tests/common/MonoTouch.Dialog/obj`)
cause NETSDK1005 errors when concurrent restores overwrite `project.assets.json` with
different platform TFMs. Clean shared obj dirs before each platform build:

```bash
rm -rf tests/common/Touch.Unit/Touch.Client/dotnet/obj tests/common/MonoTouch.Dialog/obj
make -C tests/introspection/dotnet build-ios run-ios

rm -rf tests/common/Touch.Unit/Touch.Client/dotnet/obj tests/common/MonoTouch.Dialog/obj
make -C tests/introspection/dotnet build-tvos run-tvos

rm -rf tests/common/Touch.Unit/Touch.Client/dotnet/obj tests/common/MonoTouch.Dialog/obj
make -C tests/introspection/dotnet build-macOS run-macOS

rm -rf tests/common/Touch.Unit/Touch.Client/dotnet/obj tests/common/MonoTouch.Dialog/obj
make -C tests/introspection/dotnet build-MacCatalyst run-MacCatalyst
```

**Desktop test output:** For macOS and Mac Catalyst, `make run-macOS`/`run-MacCatalyst` uses
`dotnet build -t:Run` which launches the app without waiting or capturing stdout. The make
command exits immediately with success even while tests are still running. To get actual test
results, run the executable directly after building:

```bash
# Build first
make -C tests/introspection/dotnet build-macOS
# Then run directly to capture output
NUNIT_AUTOSTART=true NUNIT_AUTOEXIT=true \
  tests/introspection/dotnet/macOS/bin/Debug/net10.0-macos/osx-arm64/introspection.app/Contents/MacOS/introspection
```

Same pattern for Mac Catalyst (replace `macOS` → `MacCatalyst`, `net10.0-macos` → `net10.0-maccatalyst`, `osx-arm64` → `maccatalyst-arm64`).

iOS and tvOS simulator tests capture output correctly via `make run-ios`/`run-tvos`.

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
