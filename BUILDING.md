# Building

For general instructions on how to build and run from source, see the [Build & Run](https://github.com/dotnet/macios/wiki/Build-&-Run) wiki page.

## Developing macios-devtools changes locally

To test changes to [macios-devtools](https://github.com/dotnet/macios-devtools) before publishing a package, pack it into a local feed:

```sh
# In the macios-devtools checkout:
export MACIOS_DEVTOOLS_FEED="$HOME/macios-devtools-feed"
export MACIOS_DEVTOOLS_VERSION=1.0.0-local.1
dotnet pack Xamarin.MacDev/Xamarin.MacDev.csproj -c Release \
    -p:PackageVersion="$MACIOS_DEVTOOLS_VERSION" -o "$MACIOS_DEVTOOLS_FEED"
```

In the macios checkout, select that version and add the local feed to the configured sources:

```sh
export XamarinAppleToolsMaciOSPackageVersion="$MACIOS_DEVTOOLS_VERSION"
export RestoreAdditionalProjectSources="$MACIOS_DEVTOOLS_FEED"
dotnet build tools/assembly-preparer/assembly-preparer.csproj
```

Use a new package version after each change, since NuGet caches packages by version. For Make-based builds, run `touch Directory.Build.props` after changing the version override so Make rebuilds the affected tools.
