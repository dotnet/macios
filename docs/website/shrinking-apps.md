Minimizing app size
===================

This document describes various build settings that can be set to reduce the final app size.

All of these options can have undesired side effects, so not all projects can enable them. Please verify that your app works as expected if you want to try them.

# Enable NativeAOT

```xml
<PropertyGroup>
	<PublishAot>true</PublishAot>
</PropertyGroup>
```

This is only possible for project that are fully and safely trimmable, so this isn't possible for many projects without code changes.

On the other hand, for projects that can enable this option, it typically results in a drastic reduction in app size.

References:

* https://learn.microsoft.com/en-us/dotnet/core/deploying/native-aot/ios-like-platforms/

# Disable dynamic code support

```xml
<PropertyGroup>
	<DynamicCodeSupport>false</DynamicCodeSupport>
</PropertyGroup>
```

This will tell the trimmer that the app won't generate code dynamically, so the trimmer can remove any related managed code.

References:

* https://learn.microsoft.com/en-us/dotnet/api/system.runtime.compilerservices.runtimefeature.isdynamiccodesupported?view=net-10.0

# Disable support for line numbers in managed stack traces

```xml
<ItemGroup>
	<RuntimeHostConfigurationOption Include="System.Diagnostics.StackTrace.IsLineNumberSupported" Value="false" Trim="true" />
</ItemGroup>
```

This will disable support for line numbers in managed stack traces.

Native stack traces (from crash reports) will still have line numbers for managed code that was ReadyToRun-compiled (which most code, except for dynamic code, is for release builds).

