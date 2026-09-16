Debug vs. Release Configuration Property Documentation
========================================================

There are a number of properties that are contingent upon the configuration setting chosen. The properties and their corresponding values are documented here to provide transparency and enable users to better create custom configuration modes.

### Release Configuration

| **Property**                                            | **Default value**                 | **Condition?**                                                                         |
|---------------------------------------------------------|-----------------------------------|----------------------------------------------------------------------------------------|
| DebuggerSupport                                         | false                             |                                                                                        |
| EnableAssemblyILStripping                               | true                              |                                                                                        |
| RuntimeIdentifiers                                      | maccatalyst-x64;maccatalyst-arm64 | TargetFramework == netx.x-maccatalyst and effective SupportedOSPlatformVersion < 27.0  |
| RuntimeIdentifier                                       | Host architecture                 | TargetFramework == netx.x-maccatalyst and effective SupportedOSPlatformVersion >= 27.0 |
| RuntimeIdentifiers                                      | osx-x64;osx-arm64                 | TargetFramework == netx.x-macos and effective SupportedOSPlatformVersion < 27.0        |
| RuntimeIdentifier                                       | Host architecture                 | TargetFramework == netx.x-macos and effective SupportedOSPlatformVersion >= 27.0       |
| UseSystemResourceKeys                                   | true                              |                                                                                        |
| VerifyDependencyInjectionOpenGenericServiceTrimmability | false                             |                                                                                        |

When `SupportedOSPlatformVersion` is not set explicitly, its effective value is the SDK's default `TargetPlatformVersion`.

### Debug Configuration

| **Property**             	| **Default value** 	| **Condition?**                        	|
|--------------------------	|-------------------	|---------------------------------------	|
| CodesignDisableTimestamp 	| true              	|                                       	|
| DebuggerSupport          	| true              	|                                       	|
| NoSymbolStrip          	| true              	|                                       	|
| RuntimeIdentifiers       	| maccatalyst-x64   	| TargetFramework == netx.x-maccatalyst 	|
| RuntimeIdentifiers       	| osx-x64           	| TargetFramework == netx.x-macos       	|
| UseSystemResourceKeys    	| false             	|                                       	|

### Optimize=true

These are options that are set when the `Optimize` property is `true` (which happens by default if `Configuration=Release`).

| **Property**                   | **Value**  |
|--------------------------------|------------|
| EventSourceSupport             | false      |
| HttpActivityPropagationSupport | false      |
| MetricsSupport                 | false      |
