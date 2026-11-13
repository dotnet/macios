# Templates for iOS, tvOS, macOS and Mac Catalyst

Templating Engine: https://github.com/dotnet/templating

## Background download extension

This template requires the SDK fix for
[#19861](https://github.com/dotnet/macios/issues/19861), which puts ExtensionKit
extensions in the app's `Extensions` directory and validates their manifests.
That fix isn't available in a released SDK yet. Older SDKs don't support embedding
these extensions correctly.

`ios-background-download-extension` creates a Background Assets downloader
extension for iOS 26.0 or later. It works with asset packs that you host on your own
server. The system manages the downloads. Create the extension:

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
[unmanaged Background Assets configuration guide](https://developer.apple.com/documentation/backgroundassets/configuring-an-unmanaged-background-assets-project).
Use that guide only for those keys; this template is a managed downloader, not
the unmanaged extension described there.

The extension's `Info.plist` uses `EXAppExtensionAttributes` instead of
`NSExtension`, and the extension goes in the app's `Extensions` directory instead
of `PlugIns`. `EXPrincipalClass` must match the Objective-C name in `[Register]`.
The SDK starts .NET and then calls `NSExtensionMain`, the same entry point Xcode
uses for Objective-C ExtensionKit extensions. You don't need a `Main` method or a
Swift `AppExtension` implementation.

Use `ShouldDownload` to choose which asset packs to download. The system provides
the inherited `IBADownloaderExtension` methods for managed asset packs. Don't
implement them, except for `DidReceiveChallenge` if you need to handle
authentication challenges. Apple's
[Objective-C downloader instructions](https://developer.apple.com/documentation/backgroundassets/bamanageddownloaderextension)
describe the principal-class contract used by this template.
