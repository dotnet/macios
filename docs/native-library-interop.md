# NativeLibraryInterop

## Overview
Native Library Interop (formerly referred to as the "Slim Binding" approach), refers to a
pattern for accessing native SDKs in .NET for iOS, Mac Catalyst, macOS, and tvOS projects.

Starting in .NET 9, the .NET for iOS, Mac Catalyst, macOS, and tvOS SDKs support building
Xcode framework projects by using the `@(XcodeProject)` build action. This is declared in
an MSBuild ItemGroup in a project file:

```xml
<ItemGroup>
  <XcodeProject Include="path/to/MyProject.xcodeproj" SchemeName="MyLibrary" />
</ItemGroup>
```

When an `@(XcodeProject)` item is added to a .NET for iOS, Mac Catalyst, macOS, or tvOS binding project,
the build process will attempt to create an XCFramework from the specified Xcode project. The XCFramework
output will be added as a `@(NativeReference)` to the .NET project so that it can be bound and have its
API surfaced via an [API definition][0] file.

Frameworks installed in the Xcode archive, including dynamic framework dependencies,
are each packaged as a separate XCFramework and added as a `@(NativeReference)`.
This ensures that dynamic dependencies are also included in applications consuming the binding.
The Xcode project must be configured to include these dependencies in its archive.

Please see the [build-items](building-apps/build-items.md) docs for more information about
the `@(XcodeProject)` build action.

Additional documentation and references can be found below:

* https://learn.microsoft.com/en-us/dotnet/communitytoolkit/maui/native-library-interop
* https://github.com/CommunityToolkit/Maui.NativeLibraryInterop

[0]: https://learn.microsoft.com/en-us/previous-versions/xamarin/cross-platform/macios/binding/objective-c-libraries?tabs=macos#The_API_definition_file
