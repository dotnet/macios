# Native watchOS apps in .NET iOS projects

This design addresses [#20317](https://github.com/dotnet/macios/issues/20317).
The watch app remains a native Xcode project; .NET does not compile managed
watchOS code. The iOS project owns embedding, provisioning, and signing.

## Project input

Add exactly one `AdditionalWatchOSApp` to an iOS executable project:

```xml
<ItemGroup>
  <AdditionalWatchOSApp Include="../WatchApp/WatchApp.xcodeproj"
                        SchemeName="WatchApp" />
</ItemGroup>
```

The input can instead be a prebuilt `.app` directory or `.zip`. Xcode projects
and ZIPs work on a Mac or on Windows with a paired Mac. Directory inputs require
a local Mac, so executable permissions and symbolic links are preserved.

The host must use one `RuntimeIdentifier`, not `RuntimeIdentifiers`.
`ios-arm64` selects watchOS device output; `iossimulator-arm64` and
`iossimulator-x64` select the matching watchOS simulator architecture.
The host's architecture is not treated as the watch device's architecture.

For source inputs, `Configuration` defaults to the host configuration.
`SchemeName` can be omitted when Xcode lists exactly one scheme. The selected
scheme must produce exactly one watch app; older layouts with an embedded
watch extension are also supported. Xcode builds without signing and uses
intermediate directories separate from the source project.

The native project owns bundle identifiers, versions, deployment targets,
assets, capabilities, and watch-specific settings. Its
`WKCompanionAppBundleIdentifier` must match the host bundle identifier.
The existing bundle validator checks identifier relationships and versions.

## ZIP format

A single-variant ZIP contains one app at its root:

```text
WatchApp.app/
Entitlements/
  com.example.Host.watchkitapp.plist
dSYMs/
  WatchApp.app.dSYM/
```

The `Entitlements` and `dSYMs` directories are optional. Entitlement filenames
are bundle identifiers, including a separate file for each embedded extension.
They contain the original entitlement templates, not configuration-specific
expanded values from a previous signature.

A multi-variant ZIP puts this layout under iOS runtime identifier directories:

```text
ios-arm64/
  WatchApp.app/
  Entitlements/
  dSYMs/
iossimulator-arm64/
  WatchApp.app/
  Entitlements/
  dSYMs/
iossimulator-x64/
  WatchApp.app/
  Entitlements/
  dSYMs/
```

Only the selected runtime identifier is extracted. Explicit ZIP directory
entries are not required. Entries must be relative, must not contain traversal
segments, and must not write through symbolic links. Links must remain inside
their bundle. Use a Mac ZIP tool that preserves executable permissions and
symbolic links.

## Creating a reusable ZIP

Run this target in an iOS project on a Mac:

```shell
dotnet build -t:CreateWatchOSAppZip \
  -p:WatchOSXcodeProject=../WatchApp/WatchApp.xcodeproj \
  -p:WatchOSXcodeScheme=WatchApp \
  -p:WatchOSZipPath=output/WatchApp.zip
```

The helper builds all three supported runtime identifiers, packages their
unsigned bundles, and includes entitlement templates and available dSYMs.
`WatchOSRuntimeIdentifiers` can select a semicolon-separated subset.
This helper deliberately does not support remote execution.

Consume its output without depending on the original Xcode project:

```xml
<ItemGroup>
  <AdditionalWatchOSApp Include="../artifacts/WatchApp.zip" />
</ItemGroup>
```

## Signing

Device builds use the host signing certificate and select a compatible
installed provisioning profile for each native bundle. Automatic selection
uses the host profile's distribution type. Debug and Release do not depend on
the signature that happened to be present in the input.

Use `CodesignProvision` metadata to select an installed profile by name or UUID.
Use `CodesignEntitlements` to supply a template explicitly. On
`AdditionalWatchOSApp`, these settings apply to the root watch app. To configure
an embedded extension, or override the root app by bundle identifier, use:

```xml
<ItemGroup>
  <WatchOSAppSigning Include="com.example.Host.watchkitapp.extension"
                     CodesignProvision="Watch Extension Development"
                     CodesignEntitlements="WatchExtension.entitlements" />
</ItemGroup>
```

Unmatched or duplicate signing entries are errors. Xcode source builds and
helper-generated ZIPs carry their original entitlement templates. For other
prebuilt inputs, existing signature entitlements are recovered when available;
profile-specific application, team, debugging, and push environment values are
recomputed. Supply the original template explicitly when changing teams or
using capabilities with expanded identifiers. Unsigned bundles without a
template use the selected profile's allowed default entitlements.

Profiles and compiled entitlements are written only into staged copies.
The input bundle is never re-signed or otherwise modified. Simulator bundles
use normal simulator signing and retain their compiled-in entitlements.
Watch apps, embedded extensions, frameworks, and libraries participate in the
existing inside-out codesigning pipeline.

## Outputs and migration

The app is staged at `<Host>.app/Watch/<WatchApp>.app`. Replacing an input
removes stale files; renaming or removing the item removes the previous
embedded app. Generated files participate in MSBuild clean. Available dSYMs
are included in archives, and legacy `_WatchKitStub/WK` support is included
when the native product contains it.

The old managed watch project resolution, watch-specific asset compilation,
and signing/post-processing sidecars are removed. Replace `IsWatchApp` project
references and private `_ResolvedWatchAppReferences` hooks with
`AdditionalWatchOSApp`; they are not compatibility aliases.

This does not add a watchOS target framework, a managed watch runtime,
automatic changes to Xcode source settings, or watch deployment/debugging.
