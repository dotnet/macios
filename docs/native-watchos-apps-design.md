<!--
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
-->

# Design proposal: embedding native watchOS apps

**Status: proposed; requires approval before implementation.**

This document proposes the design phase of [#20317](https://github.com/dotnet/macios/issues/20317).
It does not introduce supported build items or change existing behavior.
The examples below describe the proposed contract, not functionality available today.

## Goals and boundaries

Provide an official way to include a native watchOS companion app in a .NET iOS
application, including the iOS target of a .NET MAUI application. Support building
an Xcode project on macOS and consuming prebuilt artifacts from macOS or Windows
with a paired Mac. Copying an app is not sufficient: its signatures, provisioning,
bundle relationships, and packaged output must remain valid.

This does not restore managed watchOS support, create a watchOS target framework,
or provide watchOS templates, debugging, deployment, or automatic installation.
Watch Connectivity remains application code, not part of this build feature.
Standalone watch-only apps are not embedded companion apps.

The implementation, test projects, and removal of obsolete code are follow-up
work after this design is approved.

## Current behavior

* [`XcodeProject`](building-apps/build-items.md#xcodeproject) builds frameworks.
  [`Microsoft.MaciOS.Sdk.Xcode.targets`](../dotnet/targets/Microsoft.MaciOS.Sdk.Xcode.targets)
  creates device and simulator archives and combines them into an XCFramework.
  Its targets are gated on the MSBuild host being macOS.
  [`CreateXcArchive`](../msbuild/Xamarin.MacDev.Tasks/Tasks/CreateXcArchive.cs)
  also supplies framework-specific build settings.
* The example linked from the issue injects `_ResolvedWatchAppReferences` and
  `_CopyWatchOS2AppsToBundle`. These are private implementation details in
  [`Xamarin.iOS.Common.targets`](../msbuild/Xamarin.Shared/Xamarin.iOS.Common.targets),
  left over from managed watchOS project support.
* [`Xamarin.Shared.targets`](../msbuild/Xamarin.Shared/Xamarin.Shared.targets)
  reads signing and post-processing sidecar files produced by managed watch
  projects. An Xcode-produced app does not produce these files.
  [`ComputeCodesignItems`](../msbuild/Xamarin.MacDev.Tasks/Tasks/ComputeCodesignItems.cs)
  excludes `Watch/` from its containing app's file-signing discovery.
* [`ValidateAppBundle`](../msbuild/Xamarin.MacDev.Tasks/Tasks/ValidateAppBundle.cs)
  already validates watch bundle identifiers, companion relationships, and both
  single-app and WatchKit-extension layouts.
  [`Archive`](../msbuild/Xamarin.MacDev.Tasks/Tasks/Archive.cs) has separate watch
  symbol handling that must not be lost when replacing managed-project support.

[#23408](https://github.com/dotnet/macios/issues/23408) demonstrates that a Mac
connection alone does not make `XcodeProject` builds work from Windows. The new
feature must not silently succeed without its watch app in that configuration.

## Recommended public contract

Add an `AdditionalWatchOSApp` item. Infer the input kind from the path:

| Input        | Meaning                                       | macOS | Windows with paired Mac |
|--------------|-----------------------------------------------|-------|-------------------------|
| `.xcodeproj` | Build the selected watchOS application scheme | Yes   | Explicit error in v1    |
| `.app`       | Embed an already built watchOS app directory  | Yes   | Use ZIP instead         |
| `.zip`       | Embed an already built watchOS app package    | Yes   | Yes                     |

Only an executable iOS application may consume this item. Report an error for
an item in a library, app extension, tvOS, macOS, or Mac Catalyst project.
Multi-targeting projects must condition their items on the iOS target framework.
An outer cross-targeting build must defer processing to the iOS inner build.
Version 1 accepts one watch app; reject multiple items rather than arbitrarily
selecting one or assuming multiple companion apps are supported.
It also requires a single `RuntimeIdentifier`; reject multi-RID universal builds
with this item until watch simulator bundle merging has a defined contract.

| Metadata        | Applies to   | Proposed behavior                                                         |
|-----------------|--------------|---------------------------------------------------------------------------|
| `SchemeName`    | `.xcodeproj` | Required shared scheme whose selected application product is a watch app. |
| `Configuration` | `.xcodeproj` | Defaults to the containing project's `$(Configuration)`.                  |
| `DSymPath`      | `.app`       | Optional path to a directory containing the matching native dSYMs.        |

Reject metadata for the wrong input kind, missing inputs, and unknown extensions.
Do not add arbitrary `xcodebuild` arguments, an output-directory convention, or
a separate architecture-selection abstraction in v1.
All relative input paths, including `DSymPath`, are relative to the .NET project.
Intermediate outputs are owned by the SDK, not written beside the Xcode sources.

### Build an Xcode project on macOS

```xml
<ItemGroup Condition="$([MSBuild]::GetTargetPlatformIdentifier('$(TargetFramework)')) == 'ios'">
  <AdditionalWatchOSApp Include="../Watch/Companion.xcodeproj"
                       SchemeName="Companion Watch App" />
</ItemGroup>
```

The Xcode project owns the watch app's bundle identifier, versions, entitlements,
and per-target provisioning settings. These must agree with the iOS app.
The .NET build validates them but does not rewrite signed manifests.

### Consume platform-specific artifacts

```xml
<ItemGroup Condition="$([MSBuild]::GetTargetPlatformIdentifier('$(TargetFramework)')) == 'ios'">
  <AdditionalWatchOSApp Include="../WatchArtifacts/$(RuntimeIdentifier)/Companion.zip" />
</ItemGroup>
```

Here `ios-arm64` and `iossimulator-arm64` are artifact-selection directory names,
not watchOS runtime identifiers. Device watch architectures are chosen by Xcode,
not inferred by copying the iPhone's architecture. Simulator artifacts must
contain the corresponding watch simulator architecture. A missing artifact is
an error; never fall back between simulator and device.

A direct `.app` input uses the same platform validation. On macOS, for example,
replace the ZIP path above with `../WatchArtifacts/$(RuntimeIdentifier)/Companion.app`.
`DSymPath` supplies its optional symbols. Requiring ZIP input on Windows avoids
losing executable bits and symlink information through a Windows directory copy.

### ZIP format

The archive contains exactly one top-level `*.app` directory. It may also contain
`dSYMs/`, `SwiftSupport/`, and `WatchKitSupport2/` when supplied by the Xcode
packaging workflow. Reject other entries, including `.xcarchive` or IPA layouts;
there is no recursive search to guess which app to embed.

Create the ZIP on a Mac using an archive tool that preserves executable bits and
symlinks, and treat it as an immutable artifact on Windows. Extract it on the Mac
into an SDK-owned intermediate directory. Reject absolute paths, traversal paths,
duplicate or case-colliding entries, and symlinks escaping that directory.
Validate the resulting app and its signatures after extraction, not just the ZIP
file extension. Supplemental content is validated and routed to the appropriate
archive or IPA location; it is not copied into the watch app.

## Building and embedding

For `.xcodeproj` inputs, invoke `xcodebuild` for the selected scheme using the
same Xcode installation selected for the .NET build. Use `watchos` and a generic
watchOS destination for device builds, and `watchsimulator` and the requested
simulator architecture for simulator builds. Do not build both variants for
every .NET invocation. An architecture unavailable in that Xcode SDK is an error.

Use `xcodebuild build` for simulator products and `xcodebuild archive` for device
products, with isolated derived-data and archive paths under the RID-specific
intermediate directory. Discover the selected application target and its output
from Xcode's build settings and archive metadata. Require exactly one matching
watchOS application product; do not infer the bundle name from `SchemeName`,
select an iOS wrapper, or take the first `.app` found.

Reuse the existing Xcode process runner where practical, but not the framework
archive defaults, dual-platform build, or XCFramework creation pipeline.
Xcode's own incremental build handles source changes; invoke it on each normal
build rather than adding an incomplete Swift-only input glob. Do not invoke it
during restore or design-time builds.

All three input kinds normalize to an internal resolved watch artifact containing
the app path, optional symbols/support files, and expected platform. It must not
depend on managed watch project sidecar files.

Stage and validate the artifact, then copy it to
`$(AppBundleDir)/Watch/<original-bundle-name>.app` before signing the iOS container.
Preserve symlinks, permissions, nested extensions, frameworks, and resources.
Do not merge into a previous copy: remove the previous SDK-owned watch destination
when replacing an artifact so deleted files cannot survive. Track previous
destinations so renaming or removing the item also removes stale embedded apps.
Reject collisions with another producer of the same destination.

Track all generated files in `FileWrites`, including extraction results, copied
bundles, symbols, support files, manifests, and stamps. Targets using `Outputs`
must account for all their outputs, not just a success stamp. Clean must never
delete input artifacts, Xcode sources, or user-selected symbol directories.
Restore resolved items even when expensive work is skipped. Changing configuration,
RID, signing settings, ZIP contents, directory contents, or removing an input
must invalidate the relevant copy, validation, packaging, and container signature.

## Signing ownership

**Recommendation: preserve Xcode signatures in v1, rather than re-sign native
watch bundles in the .NET build.** This is an intentional scope choice.

For source inputs, Xcode signs the watch app and every nested executable target
using that target's provisioning profile and entitlements. For device builds,
pass the containing iOS app's resolved signing identity and team to Xcode,
without passing its provisioning profile or entitlements. Keep profile selection
per target in the Xcode project. Resolve this after .NET signing-identity detection.
Do not enable provisioning updates, create certificates, or change developer
accounts automatically; missing credentials or profiles produce actionable errors.

Prebuilt device artifacts must already be signed for the containing app's
signing identity, team, and distribution method, with suitable watchOS profiles.
Changing the iOS signing configuration requires rebuilding the watch artifact.
There is no implicit ad-hoc signing or development-to-distribution conversion.
Explicitly unsigned device builds may consume unsigned watch artifacts, but must
not claim they are installable or distributable. Disable Xcode signing explicitly
in that case instead of allowing it to select an unrelated signing identity.

For simulator builds, preserve Xcode's unsigned or ad-hoc-signed output as
appropriate for the selected SDK. Device profiles are not required.

Before signing the iOS container:

* Validate watchOS versus watchOS-simulator Mach-O platform information, not just
  CPU names or `Info.plist` declarations. Validate nested executable products too.
* Validate bundle identifiers and `WKCompanionAppBundleIdentifier` against the
  final iOS manifest, device family, executable existence, and version agreement.
  Preserve existing version-mismatch warning severity; do not mutate the artifact.
* For signed device builds, verify each executable bundle's signature and embedded
  profile, the signing certificate and team, profile expiration, application
  identifier, and entitlement authorization. Account for wildcard profile values
  and App ID prefixes rather than assuming they equal the team identifier.
  Check distribution-method compatibility and device eligibility against the
  watch profile, not the paired iPhone's device list.
* Verify the copied watch app and its nested code with Apple's signing tools.
  Never treat `_CodeSignature` directory existence as evidence of valid signing.

Exclude preserved watch artifacts from .NET re-signing, stripping, entitlement
compilation, and Swift-library rewriting. Do not inherit the containing iOS app's
entitlements through a generic `CodesignBundle` entry. Sign the iOS container only
after embedding; subsequent mutations must invalidate its signature.

This trades convenience for a single clear signing owner. A future re-signing
mode would need explicit per-bundle profiles and entitlements, including nested
WatchKit extensions; it must not be introduced as an undocumented fallback.

## Windows and packaging

On Windows, transfer the ZIP as an explicit remote-task input through the
existing build-session transport. Extraction, validation, copying, and Apple-tool
invocations run on the paired Mac. Preserve the distinction between local input
paths and remote output paths; never resolve remote paths using Windows
`System.IO.Path.GetFullPath` in an MSBuild expression.

An unavailable Mac, a `.xcodeproj` input on Windows, or a direct `.app` input on
Windows is an actionable error, not a skipped target. Source-project remoting is
deferred: correctly transferring a complete Xcode source/dependency tree is
separate from transferring one immutable ZIP and must not be implied by this API.
Unpaired/Hot Restart watch-app builds are not supported in v1.

Both the `.app` output and IPA must contain `Watch/<name>.app`; `.xcarchive`
must also contain supplied matching watch and extension dSYMs. Check dSYM UUIDs
against the corresponding Mach-O binaries. Preserve Swift and WatchKit support
files in the locations required by the chosen Xcode distribution workflow.
If required supplemental content is missing or conflicts with other packaging
inputs, report an error rather than emitting a knowingly incomplete package.
Keep support files outside the signed bundles, and do not strip native watch
executables or attempt to regenerate their symbols using iOS tooling.

Export workflows that would need to re-sign a preserved watch app are outside
the v1 contract: rebuild it with the final identity and profiles before packaging.
Validate development, ad-hoc, and App Store packaging separately; a successful
development build does not establish that distribution packaging works.

## Alternatives

Extending `XcodeProject` with an application output kind would reuse its public
name and some metadata, but would still require a distinct application pipeline,
signing contract, and prebuilt input story. It would not fix Windows remoting.
Leave framework behavior and its `Release` default unchanged.

A prebuilt-only item is simpler but leaves every Mac user responsible for
coordinating simulator/device builds. Supporting `.xcodeproj` on Mac avoids that
without claiming to solve general Xcode project remoting.

Automatic RID-subdirectory discovery, accepting arbitrary directories or
archives, and exposing the legacy private items create ambiguous selection
rules. Explicit MSBuild conditions and a small ZIP contract are preferable.

## Compatibility and removal plan

Projects without `AdditionalWatchOSApp` retain current behavior. Keep the
existing private-item workaround working until the supported replacement ships,
as promised in the issue. Reject mixing the new item with legacy watch inputs
in the same build, rather than embedding or signing an app twice.

After approval, implement and document the new path, provide migration examples
for the private-item workaround, and only then remove obsolete managed watchOS
project resolution, configuration, signing/post-processing sidecars, and Windows
project-reference plumbing. Retain native bundle validation, signing exclusions,
symbol collection, and required packaging behavior in the replacement pipeline.
Inventory references before deleting any old target or task; private names used
by the documented workaround need a transition plan, not silent removal.

## Acceptance criteria for the implementation

Add a minimal native Xcode watch project and a .NET iOS project that references
it under the existing native test-project and .NET unit-test infrastructure.
Use a current single-target watch app as the primary fixture and cover the
older app-plus-WatchKit-extension layout in artifact/signing tests.

| Area              | Required evidence                                                                                                      |
|-------------------|------------------------------------------------------------------------------------------------------------------------|
| Source build      | Debug/Release, device and arm64 simulator select the correct watch product; a second build preserves valid outputs.    |
| Prebuilt input    | `.app` and ZIP produce equivalent bundles; ZIP works through a real Windows-to-Mac build with spaces in paths.         |
| Bundle contents   | Assert the exact `Watch/<name>.app` path, manifests, executable platform/architecture, resources, and nested products. |
| Signing           | Verify copied watch app, extension, frameworks, and final iOS app; inspect entitlements and each embedded profile.     |
| Signing isolation | Use differing iOS/watch entitlements; prove the iOS entitlements and profile are never applied to watch bundles.       |
| Distribution      | Inspect IPA/archive contents and matching dSYMs/support files for development, ad-hoc, and App Store configurations.   |
| Incremental/clean | Change/delete sources and resources, replace ZIP, change RID/signing, rename/remove item; no stale app or signature.   |
| Errors            | Missing/ambiguous inputs, unsupported host/project kind, wrong platform, bad companion ID, and archive traversal fail. |
| Signing errors    | Tampered/unsigned device artifact, wrong identity/team/profile, expired profile, and unauthorized entitlements fail.   |
| Compatibility     | Existing framework `XcodeProject` tests, iOS extensions, no-watch builds, and legacy workaround remain unchanged.      |

Simulator tests must run without signing credentials. Device-signing tests
require configured certificates and separate profiles for each executable bundle;
skip explicitly when unavailable, never count that as a signing pass.
Use actual watchOS outputs for platform/signature checks, not renamed iOS apps.
Document a physical paired-device installation and launch check, including
Watch Connectivity if the sample exercises it. Do not equate a valid signature
with proven installation, App Store acceptance, or automatic watch installation.

All new build diagnostics must use the existing English MSBuild resource system.
Promote usage guidance into the build-item documentation only when implemented;
do not present this proposal as a released feature.

## Approval decisions and remaining evidence

Approval is requested for the item name and input formats, Mac-only source
builds with ZIP-based Windows support, the one-watch-app limit, and preservation
of Xcode signatures instead of .NET re-signing. The initial single-RID restriction
also requires approval; universal watch simulator assembly is deferred.

Before committing to implementation details, establish the exact product and
support-file layout generated by the repository's supported Xcode versions for
both watch app layouts. Confirm the proposed source-build identity override
works with per-target automatic and manual provisioning, and exercise the
archive/IPA path with preserved device signatures. These are explicit follow-up
validation requirements, not claims established by this design-only change.
If distribution requires broader re-signing or additional support-file handling,
revise the signing/artifact contract for approval instead of silently expanding it.

## References

* [Scope and design ideas in #20317](https://github.com/dotnet/macios/issues/20317#issuecomment-6284555029)
* [Related documentation request, #16417](https://github.com/dotnet/macios/issues/16417)
* [Windows `XcodeProject` issue, #23408](https://github.com/dotnet/macios/issues/23408)
* [Existing community workaround](https://github.com/vouksh/MauiWithWatchApps/blob/main/Maui.Phone/Maui.Phone.csproj)
* [Apple: WKCompanionAppBundleIdentifier](https://developer.apple.com/documentation/bundleresources/information-property-list/wkcompanionappbundleidentifier)
* [Apple: Entitlements Troubleshooting (TN2415)](https://developer.apple.com/library/archive/technotes/tn2415/_index.html)
