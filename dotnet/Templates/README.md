# Templates for iOS, tvOS, macOS and Mac Catalyst

Templating Engine: https://github.com/dotnet/templating

## Background download extension

`ios-background-download-extension` creates a Background Assets downloader
extension for iOS 26.0 or later. It works with asset packs that you host on your own
server. The system manages the downloads:

```sh
dotnet new ios-background-download-extension -n Downloader \
  --bundleId com.example.MyApp --appGroupId group.com.example.MyApp
```

In the app project, reference the extension and add the app group entitlement:

```xml
<ItemGroup>
  <ProjectReference Include="../Downloader/Downloader.csproj">
    <IsAppExtension>true</IsAppExtension>
  </ProjectReference>
  <CustomEntitlements Include="com.apple.security.application-groups"
                      Type="StringArray" Value="group.com.example.MyApp" />
</ItemGroup>
```

Register the app group for the app and the extension, and use provisioning
profiles that include that group. In the app's `Info.plist`, set `BAAppGroupID` to
that group, `BAHasManagedAssetPacks` to `true`, and `BAManifestURL` to the HTTPS URL of
your download manifest. Leave `BAUsesAppleHosting` unset or `false`. Create the
asset packs and download manifest with Xcode's `ba-package` tool. For self-hosted
downloads, the app's `Info.plist` must also set the size limits and allowed domains
described in Apple's
[Background Assets configuration guide](https://developer.apple.com/documentation/backgroundassets/configuring-an-unmanaged-background-assets-project).

The extension's `Info.plist` uses `EXAppExtensionAttributes` instead of
`NSExtension`, and the extension goes in the app's `Extensions` directory instead
of `PlugIns`. `EXPrincipalClass` must match the Objective-C name in `[Register]`.
The SDK starts .NET and then calls `NSExtensionMain`, the same entry point Xcode
uses for Objective-C ExtensionKit extensions. You don't need a `Main` method or a
Swift `AppExtension` implementation.

Use `ShouldDownload` to choose which asset packs to download. Don't implement the
inherited `IBADownloaderExtension` download callbacks: the system implements those
for managed asset packs. Apple's
[Objective-C downloader instructions](https://developer.apple.com/documentation/backgroundassets/bamanageddownloaderextension)
describe the principal-class contract used by this template.
