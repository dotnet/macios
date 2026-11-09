# Templates for iOS, tvOS, macOS and Mac Catalyst

Templating Engine: https://github.com/dotnet/templating

## Background download extension

`ios-background-download-extension` creates a self-hosted, managed Background Assets
extension for iOS 26.0 or later:

```sh
dotnet new ios-background-download-extension -n Downloader \
  --bundleId com.example.MyApp --appGroupId group.com.example.MyApp
```

Add it to the containing application's project:

```xml
<ItemGroup>
  <ProjectReference Include="../Downloader/Downloader.csproj">
    <IsAppExtension>true</IsAppExtension>
  </ProjectReference>
  <CustomEntitlements Include="com.apple.security.application-groups"
                      Type="StringArray" Value="group.com.example.MyApp" />
</ItemGroup>
```

Register the app group for both bundle identifiers and use provisioning profiles
that include it. In the containing app's `Info.plist`, set `BAAppGroupID` to that
group, `BAHasManagedAssetPacks` to `true`, and `BAManifestURL` to the HTTPS URL of
your download manifest. Leave `BAUsesAppleHosting` unset or `false`. Create the
asset packs and download manifest with Xcode's `ba-package` tool. Self-hosted
downloads also need the size and domain restrictions described in Apple's
[Background Assets configuration guide](https://developer.apple.com/documentation/backgroundassets/configuring-an-unmanaged-background-assets-project).

The extension uses `EXAppExtensionAttributes`, not `NSExtension`, and belongs in the
containing app's `Extensions` directory, not `PlugIns`. Its `EXPrincipalClass`
matches the Objective-C name in `[Register]`. The SDK initializes .NET and uses
`NSExtensionMain`, which is also the entry point inherited by Xcode's
`com.apple.product-type.extensionkit-extension` product type. No managed `Main`
method or Swift `AppExtension` implementation is needed.

Only override `ShouldDownload` to filter asset packs. Do not implement the
inherited `IBADownloaderExtension` download callbacks: the system supplies those
for managed asset packs. Apple's
[Objective-C downloader instructions](https://developer.apple.com/documentation/backgroundassets/bamanageddownloaderextension)
describe the principal-class contract used by this template.
