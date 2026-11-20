# NativeLibraryInterop

## Overview

Native libraries and frameworks (`.xcframework`, `.framework`, `.a`, and `.dylib`) can be integrated into .NET for iOS, Mac Catalyst, macOS, and tvOS projects in several ways depending on how the native code is built and distributed:

1. **Consuming prebuilt native binaries (`@(NativeReference)`)**:
   Prebuilt XCFrameworks (`*.xcframework` or `*.xcframework.zip`), frameworks (`*.framework`), static libraries (`*.a`), and dynamic libraries (`*.dylib`) can be added directly to a binding project or application project using the [`@(NativeReference)`](building-apps/build-items.md#nativereference) build item:

   ```xml
   <ItemGroup>
     <NativeReference Include="path/to/MyLibrary.xcframework">
       <Kind>Framework</Kind>
       <SmartLink>True</SmartLink>
     </NativeReference>
   </ItemGroup>
   ```

   When building a binding project (`IsBindingProject=true`), all `@(NativeReference)` items (including any supporting native dependency XCFrameworks) are packaged into a sidecar binding resource package (`$(AssemblyName).resources` or `$(AssemblyName).resources.zip`) next to the binding DLL, as controlled by [`NoBindingEmbedding`](building-apps/build-properties.md#nobindingembedding) (which defaults to `true`; setting `NoBindingEmbedding=false` is not supported in .NET 12+) and [`CompressBindingResourcePackage`](building-apps/build-properties.md#compressbindingresourcepackage).

2. **Shipping native libraries in NuGet packages**:
   * **Objective-C binding packages**: Running `dotnet pack` on a binding project automatically packs the managed binding `.dll` together with its `$(AssemblyName).resources.zip` (or `$(AssemblyName).resources` directory) under `lib/<TargetFramework>/`. Consuming apps only need a `PackageReference` to the package.
   * **RID-specific native assets (`runtimes/<rid>/native/`)**: Libraries that use P/Invoke (`[DllImport]` / `[LibraryImport]`) or package per-RID native assets directly can place `.xcframework`, `.framework`, `.a`, or `.dylib` files under `runtimes/<rid>/native/` in the NuGet package (see [`dotnet/BundleContents.md`](../dotnet/BundleContents.md)).

3. **Building Xcode framework projects during the .NET build (`@(XcodeProject)`)**:
   Native Library Interop (formerly referred to as the "Slim Binding" approach) is a pattern for wrapping native SDKs in thin Xcode wrapper projects and consuming them from .NET for iOS, Mac Catalyst, macOS, and tvOS projects.

   Starting in .NET 9, the .NET for iOS, Mac Catalyst, macOS, and tvOS SDKs support building Xcode framework projects by using the [`@(XcodeProject)`](building-apps/build-items.md#xcodeproject) build action. This is declared in an MSBuild `ItemGroup` in a project file:

   ```xml
   <ItemGroup>
     <XcodeProject Include="path/to/MyProject.xcodeproj" SchemeName="MyLibrary" />
   </ItemGroup>
   ```

   When an `@(XcodeProject)` item is added to a .NET for iOS, Mac Catalyst, macOS, or tvOS binding project, the build process creates an XCFramework from the specified Xcode project. The XCFramework output is automatically added as a [`@(NativeReference)`](building-apps/build-items.md#nativereference) to the .NET project so that it can be bound and have its API surfaced via an [API definition][0] file.

Please see the [build-items](building-apps/build-items.md#nativereference) and [build-properties](building-apps/build-properties.md#nobindingembedding) documentation for more information about `@(NativeReference)`, `@(XcodeProject)`, and binding resource packages.

Additional documentation and references can be found below:

* https://learn.microsoft.com/en-us/dotnet/communitytoolkit/maui/native-library-interop
* https://github.com/CommunityToolkit/Maui.NativeLibraryInterop

[0]: https://learn.microsoft.com/en-us/previous-versions/xamarin/cross-platform/macios/binding/objective-c-libraries?tabs=macos#The_API_definition_file
