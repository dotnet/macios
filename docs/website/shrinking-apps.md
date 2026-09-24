Minimizing app size
===================

This document describes various build settings that can be set to reduce the final app size.

All of these options can have undesired side effects, so not all projects can enable them. Please verify that your app works as expected if you want to try them.

# Scope and expectations

These options require .NET 11 or later, and apply to all platforms unless otherwise noted. They are intended for release builds that will be published.

The size reduction from each option depends heavily on the app, so it's not possible to predict the impact for any particular app. The best way to find out is to enable an option and compare the resulting app packages. Looking at the size of the `.ipa` is usually the easiest comparison for platforms that produce one.

Testing requirements also depend on the option. Test the resulting app and any affected diagnostic tooling before publishing it.

These options do not currently have any known implications for App Store submission, TestFlight, or notarization.

# Applying options selectively

The following read-only MSBuild properties can be used in `Condition` attributes to apply options selectively:

* `$(SdkIsSimulator)` is `true` for simulator builds.
* `$(SdkIsDevice)` is `true` for iOS and tvOS device builds.
* `$(SdkIsMobile)` is `true` for iOS and tvOS builds.
* `$(SdkIsDesktop)` is `true` for macOS and Mac Catalyst builds.

For example, this only disables dynamic code support for device builds:

```xml
<PropertyGroup>
	<DynamicCodeSupport Condition="'$(SdkIsDevice)' == 'true'">false</DynamicCodeSupport>
</PropertyGroup>
```

# Enable NativeAOT

```xml
<PropertyGroup>
	<PublishAot>true</PublishAot>
</PropertyGroup>
```

This is only possible for projects that are fully and safely trimmable, so this isn't possible for many projects without code changes.

NativeAOT does not support generating code at runtime. Code and libraries that use reflection in ways the trimmer cannot analyze, `System.Reflection.Emit`, `DynamicMethod`, compiled expression trees, dynamically generated proxies or serializers, or runtime-generated assemblies may require changes or may not be compatible.

There should not be any trimmer or AOT warnings when publishing a NativeAOT app. If publishing produces no such warnings but the app does not work correctly, report a bug.

On the other hand, for projects that can enable this option, it typically results in a drastic reduction in app size.

NativeAOT normally takes effect when publishing with `dotnet publish`. Setting `<_IsPublishing>true</_IsPublishing>` in the project file makes it possible to use NativeAOT with `dotnet build` or when launching from an IDE. NativeAOT is not compatible with debuggers, so it will not be possible to debug such a build.

References:

* https://learn.microsoft.com/dotnet/maui/deployment/nativeaot

# Disable dynamic code support

```xml
<PropertyGroup>
	<DynamicCodeSupport>false</DynamicCodeSupport>
</PropertyGroup>
```

This will tell the trimmer that the app won't generate code dynamically, so the trimmer can remove any related managed code. Trimming must be enabled for this option to reduce app size.

This option can cause problems in code and libraries that use `System.Reflection.Emit`, `DynamicMethod`, compiled expression trees, dynamically generated proxies or serializers, or other forms of runtime code generation.

This option is redundant when NativeAOT is enabled, because NativeAOT does not support dynamic code and sets this option automatically.

References:

* https://learn.microsoft.com/en-us/dotnet/api/system.runtime.compilerservices.runtimefeature.isdynamiccodesupported?view=net-10.0

# Disable support for line numbers in managed stack traces

```xml
<ItemGroup>
	<RuntimeHostConfigurationOption Include="System.Diagnostics.StackTrace.IsLineNumberSupported" Value="false" Trim="true" />
</ItemGroup>
```

This will disable support for source information in managed stack traces. Managed frames and method names are still available, but source file names, line numbers, and column numbers are not. Trimming must be enabled for this option to reduce app size.

This does not affect app behavior, but it affects anything that reads managed debug symbols to collect diagnostics, including managed crash reporters.

Native stack traces (from crash reports) will still have line numbers for managed code that was ReadyToRun-compiled (which most code, except for dynamic code, is for release builds).

References:

* https://github.com/dotnet/runtime/blob/main/src/coreclr/System.Private.CoreLib/src/System/Diagnostics/StackFrameHelper.cs
