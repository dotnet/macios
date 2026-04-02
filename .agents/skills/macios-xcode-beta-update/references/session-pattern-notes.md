# Session pattern notes for Xcode beta bumps

## Practical sequence used successfully

1. Update non-test Xcode bump files.
2. Run `make show-versions`.
3. Run `make world`.
4. If blocked by missing Xcode component, install MetalToolchain with:
   - `xcrun xcodebuild -downloadComponent MetalToolchain`
5. Re-run `make world` until green.
6. Run XTRO sanitize pass:
   - `AUTO_SANITIZE=1 make -C tests/xtro-sharpie all`
7. Move unclassified entries and rerun XTRO:
   - `make -C tests/xtro-sharpie unclassified2todo`
   - `AUTO_SANITIZE=1 make -C tests/xtro-sharpie all`
8. Run introspection for all platforms with explicit prebuild.

## Known gotcha

`make -C tests/introspection/dotnet run-ios` may fail if the app isn't built first.
Prefer `build-ios run-ios` (same for other platforms) in the same command.

## Introspection parallelism gotcha

Running introspection tests for multiple platforms in parallel causes NETSDK1005 errors.
The shared test libraries (`Touch.Unit/Touch.Client/dotnet/` and `MonoTouch.Dialog/`) have
per-platform `.csproj` files that share a single `obj/` directory. When one platform restores,
it writes `project.assets.json` targeting only that platform's TFM, breaking other platforms.

**Fix:** Run sequentially and clean shared obj dirs before each platform:
```bash
rm -rf tests/common/Touch.Unit/Touch.Client/dotnet/obj tests/common/MonoTouch.Dialog/obj
```

## Desktop introspection output capture

For macOS and Mac Catalyst, `make run-macOS`/`run-MacCatalyst` uses `dotnet build -t:Run`
which launches the desktop app and exits immediately without waiting or capturing stdout.
The make target will report success (exit 0) while the test app is still running.

**Fix:** After building, run the executable directly with environment variables:
```bash
NUNIT_AUTOSTART=true NUNIT_AUTOEXIT=true \
  tests/introspection/dotnet/macOS/bin/Debug/net10.0-macos/osx-arm64/introspection.app/Contents/MacOS/introspection
```

iOS and tvOS simulator tests do not have this problem — their output is captured correctly.
