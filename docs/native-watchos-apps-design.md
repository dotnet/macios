# Native watchOS apps in .NET iOS projects

This design addresses [#20317](https://github.com/dotnet/macios/issues/20317).
The watch app remains a native Xcode project; .NET does not compile managed
watchOS code. The iOS project embeds, provisions, and signs the watch app.

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
a local Mac so that executable permissions and symbolic links are preserved.

The host must use one `RuntimeIdentifier`, not `RuntimeIdentifiers`.
`ios-arm64` selects the watchOS device build; `iossimulator-arm64` selects the
watchOS simulator build. The watch device's architecture does not need to
match the iPhone's.

For source inputs, `Configuration` defaults to the host configuration.
`SchemeName` can be omitted when Xcode lists exactly one scheme. The selected
scheme must produce exactly one watch app; older watch apps that contain a
WatchKit extension are also supported. Xcode builds the app unsigned and keeps
its intermediate files outside the source project.

Set bundle identifiers, versions, deployment targets, assets, capabilities,
and other watch settings in the Xcode project. Its
`WKCompanionAppBundleIdentifier` must match the host bundle identifier.
The build checks bundle identifier relationships and versions.

## ZIP format

A single-variant ZIP contains one app at its root:

```text
WatchApp.app/
Entitlements/
  com.example.Host.watchkitapp.plist
dSYMs/
  WatchApp.app.dSYM/
```

The `Entitlements` and `dSYMs` directories are optional. Name each entitlements
file after a bundle identifier, with a separate file for each embedded extension.
Each file contains the original entitlement template, not values already
expanded for a previous configuration and signature.

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
```

Only the selected runtime identifier is extracted. The ZIP does not need
separate entries for directories. Each entry must use a relative path, must
not contain `..`, and must not place a file inside a symbolic link. Links must
remain inside their bundle. Use a Mac ZIP tool that preserves executable
permissions and symbolic links.

## Creating a reusable ZIP

Run this target in an iOS project on a Mac:

```shell
dotnet build -t:CreateWatchOSAppZip \
  -p:WatchOSXcodeProject=../WatchApp/WatchApp.xcodeproj \
  -p:WatchOSXcodeScheme=WatchApp \
  -p:WatchOSZipPath=output/WatchApp.zip
```

The target builds both supported runtime identifiers, packages their
unsigned bundles, and includes entitlement templates and available dSYMs.
`WatchOSRuntimeIdentifiers` can select a semicolon-separated subset.
This target only runs on a local Mac, not through a paired Mac.

Use its output without depending on the original Xcode project:

```xml
<ItemGroup>
  <AdditionalWatchOSApp Include="../artifacts/WatchApp.zip" />
</ItemGroup>
```

## Signing

Device builds use the host signing certificate and select a compatible
installed provisioning profile for each native bundle. When choosing a profile
automatically, the build selects one with the same distribution type as the
iOS app's profile. Debug and Release builds replace any existing signature.

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
prebuilt inputs, the build reads entitlements from the existing signature, if
there is one. It recalculates values that depend on the profile: the application
identifier, team identifier, debugging entitlement, and push environment.
If you change teams, or use capabilities whose identifiers include the team
or app ID, supply the original entitlements template. Unsigned bundles without
a template get the default entitlements allowed by the selected profile.

The build adds profiles and compiled entitlements only to its own copy of the app.
The input bundle is never re-signed or otherwise modified. Simulator bundles
use normal simulator signing and retain their compiled-in entitlements.
Watch apps, embedded extensions, frameworks, and libraries are signed the same
way as other bundles, innermost first.

## Outputs and migration

The app is copied to `<Host>.app/Watch/<WatchApp>.app`. Replacing the input
removes old files. Renaming or removing the item removes the previously
embedded app. `dotnet clean` removes generated files. Available dSYMs are
included in archives. If the native product contains the legacy
`_WatchKitStub/WK` stub, it is included too.

Support for managed watch projects has been removed, including the watch-specific
asset and signing steps. Replace `IsWatchApp` project references and private
`_ResolvedWatchAppReferences` inputs with `AdditionalWatchOSApp`. The old names
are not accepted as aliases.

This does not add a watchOS target framework, a managed watch runtime,
automatic changes to Xcode source settings, or watch deployment/debugging.
