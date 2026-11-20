# About

Objective Sharpie is a command line tool to help bootstrap the first pass of a C# binding for an Objective-C framework. It works by parsing the header files of a framework to map the public API into the binding definition.

Objective Sharpie uses Clang to parse header files, so the binding is as exact and thorough as possible.

Install Objective Sharpie on an Apple Silicon Mac with `dotnet tool install -g Sharpie.Bind.Tool`.
Install the .NET workload for the target platform, such as `dotnet workload install ios` or `dotnet workload install macos`.

Bind a framework with `sharpie bind -f ./MyFramework.framework -o Binding`, or bind an individual header:

```bash
sharpie bind --header=Headers/MyFramework-Swift.h --scope=Headers --sdk=iphoneos -o Binding
```

Specify the input with `--header` or `--framework` (`-f`); positional inputs are not supported.
Pass any additional compiler arguments after `--clang`.
Use `xcodebuild -showsdks` to list installed SDKs.
Generated files are named `ApiDefinition.cs` and `StructsAndEnums.cs`, depending on the declarations found.
If no bindable declarations remain after filtering, Sharpie reports a warning instead of claiming that bindings were generated.

To generate bindings for an entire SDK, use `sharpie sdk-db -s iphoneos -o output/`.
Modules are enabled by default; use `--modules=false` to disable them.
The `-x/--exclude` option excludes frameworks, rather than selecting a framework to bind.

> [!IMPORTANT]
> Objective Sharpie is a tool for experienced developers with
> advanced knowledge of Objective-C (and by extension, C). Before
> attempting to bind an Objective-C library you should understand
> of how the native framework works, and how to use it.

# More information

See [Creating Bindings with Objective Sharpie](https://aka.ms/sharpie-docs) for in-depth documentation on how to use Objective Sharpie.
