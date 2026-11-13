# MSBuild Localization

Messages for new MSBuild error codes live in `MSBStrings.resx`.

When logging a warning, pass its numeric resource code to the `LoggingExtensions.LogWarning`
overload as well as the localized message. For example:

```csharp
Log.LogWarning (7171, null, MSBStrings.W7171, XcodeLocator.EnvironmentVariableName); // The environment variable '{0}' is deprecated, and will be ignored in .NET 11+. Use the 'DEVELOPER_DIR' environment variable or the 'XcodeLocation' MSBuild property to use an Xcode other than the system's current default Xcode.
```

The helper adds the platform prefix and pads the code to four digits (for example,
`MT0051`). Use the file-name argument when the warning refers to a specific file.

The task project uses `Microsoft.CodeAnalysis.BannedApiAnalyzers` and
`BannedSymbols.txt` to reject both message-only and full `TaskLoggingHelper`
warning/error overloads with build error `RS0030`. Use the numeric-code
`LoggingExtensions` overloads for SDK diagnostics, including when logging errors
through task-specific wrappers. Only the logging helpers and external diagnostic
forwarding may suppress this rule, using a narrowly scoped pragma with a reason.

* You can now make changes to `MSBStrings.resx` in the Visual Studio for Mac IDE or from any text editor.

* If you make changes in the IDE, rebuild `Xamarin.MacDev.Tasks.csproj` to regenerate `MSBStrings.Designer.cs`.

* If you make changes from a text editor, build `msbuild/Xamarin.MacDev.Tasks/Xamarin.MacDev.Tasks.csproj`. Its satellite assemblies contain both the `MSBStrings` and `Errors` translations.

See [Localization Wiki][Localization-wiki] for more details on our localization process

or the [OneLocBuild Wiki][OneLocBuild-wiki] for information on OneLocBuild.

[Localization-wiki]: https://github.com/xamarin/maccore/wiki/Localization
[OneLocBuild-wiki]: https://ceapex.visualstudio.com/CEINTL/_wiki/wikis/CEINTL.wiki/107/Localization-with-OneLocBuild-Task
