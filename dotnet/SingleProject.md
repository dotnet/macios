# .NET "Single Project"

In order to improve the cross-platform experience between Android and our
Apple platforms, there are certain properties that can be set in the project
file that will be added to the app in a platform-specific way.

More a more detailed description see this document: [OneDotNetSingleProject.md][1]

For our Apple platforms this means we're mapping the following MSBuild
properties to Info.plist keys (this mapping will only take place if the
Info.plist in the project doesn't already contain entries for these keys):

| MSBuild Property          | Info.plist key             | Notes                                     |
| --------------------------|----------------------------|-------------------------------------------|
| ApplicationId             | CFBundleIdentifier         |                                           |
| ApplicationTitle          | CFBundleDisplayName        |                                           |
| ApplicationVersion        | CFBundleVersion            |                                           |
| ApplicationDisplayVersion | CFBundleShortVersionString | Defaults to ApplicationVersion when blank |

This is only enabled if the `GenerateApplicationManifest` is set to `true`
(which is the default for all supported .NET versions)

Additionally, `$(ApplicationDisplayVersion)` will overwrite the value for `$(Version)`,
so the following properties will be set with the same value:

* `$(AssemblyVersion)`
* `$(FileVersion)`
* `$(InformationalVersion)`

Ref: [Issue #10473][2]

[1]: https://github.com/dotnet/android/blob/main/Documentation/guides/OneDotNetSingleProject.md
[2]: https://github.com/dotnet/macios/issues/10473
