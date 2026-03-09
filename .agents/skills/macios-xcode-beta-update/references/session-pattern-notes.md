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
