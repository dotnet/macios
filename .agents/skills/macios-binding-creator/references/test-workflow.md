# Test Workflow

Commands and troubleshooting for validating C# bindings in dotnet/macios.

## Test Suites Overview

| Suite | Purpose | Location |
|-------|---------|----------|
| **Xtro** | Compares managed bindings against native SDK headers | `tests/xtro-sharpie/` |
| **Cecil** | Static analysis of compiled assemblies | `tests/cecil-tests/` |
| **Introspection** | Runtime validation on simulator/device | `tests/introspection/dotnet/` |

## Xtro Commands

```bash
# Generate reference bindings from SDK (do this first)
make -C tests/xtro-sharpie gen-all

# Run per-platform
make -C tests/xtro-sharpie run-ios
make -C tests/xtro-sharpie run-tvos
make -C tests/xtro-sharpie run-macos
make -C tests/xtro-sharpie run-maccatalyst

# If unclassified entries appear
make -C tests/xtro-sharpie unclassified2todo
```

### Xtro File Types

| Extension | Purpose |
|-----------|---------|
| `.todo` | APIs that need to be bound |
| `.ignore` | APIs intentionally not bound (with justification) |
| `.deprecated` | Deprecated APIs |

## Cecil Commands

```bash
make -C tests/cecil-tests run-tests
```

Cecil tests check for consistency in the compiled assemblies (attribute usage, naming conventions, etc.).

## Introspection Commands

### Critical: Clean Shared Directories

The shared `obj/` directories cause NETSDK1005 errors when different platforms overwrite `project.assets.json`. **Always clean before each platform:**

```bash
rm -rf tests/common/Touch.Unit/Touch.Client/dotnet/obj tests/common/MonoTouch.Dialog/obj
```

### Platform-Specific Commands

**Important:** `clean` and `run-bare` must be run from the **platform subdirectory** (e.g., `tests/introspection/dotnet/macOS/`). The parent `dotnet/` directory only has `build-%` and `run-%` pattern rules.

| Platform | Clean | Build | Run |
|----------|-------|-------|-----|
| iOS | `make -C .../dotnet/iOS clean` | `make -C .../dotnet build-ios` | mlaunch directly (see below) |
| tvOS | `make -C .../dotnet/tvOS clean` | `make -C .../dotnet build-tvos` | mlaunch directly (see below) |
| macOS | `make -C .../dotnet/macOS clean` | `make -C .../dotnet/macOS build` | `make -C .../dotnet/macOS run-bare` |
| MacCatalyst | `make -C .../dotnet/MacCatalyst clean` | `make -C .../dotnet/MacCatalyst build` | `make -C .../dotnet/MacCatalyst run-bare` |

### Running iOS/tvOS via mlaunch

`make run-ios`/`run-tvos` uses `dotnet build -t:Run`, which does **NOT reliably capture** the app's stdout. The `com.apple.gamed` stderr message causes MSBuild to report failure (exit code -1) even when mlaunch returns 0, and NUnit test results are lost.

Instead, build first with `make build-ios`/`build-tvos`, then run mlaunch directly:

```bash
# Get the app path
APP_PATH=$(make -C tests/introspection/dotnet/iOS print-executable | sed 's|/introspection$||')

# Run via mlaunch with output capture
SIMCTL_CHILD_NUNIT_AUTOSTART=true \
SIMCTL_CHILD_NUNIT_AUTOEXIT=true \
$DOTNET_DESTDIR/Microsoft.iOS.Sdk/tools/bin/mlaunch \
  --launchsim "$APP_PATH" \
  --device :v2:runtime=com.apple.CoreSimulator.SimRuntime.iOS-26-4,devicetype=com.apple.CoreSimulator.SimDeviceType.iPhone-16-Pro \
  --wait-for-exit:true --
```

Use `xcrun simctl list runtimes` and `xcrun simctl list devicetypes` to find the correct identifiers for your Xcode version.

### Why run-bare for Desktop Platforms

`make run-macOS` / `make run-MacCatalyst` uses `dotnet build -t:Run` which launches the app without waiting or capturing stdout. The make command exits immediately with success even while tests are still running.

`make run-bare-macOS` / `make run-bare-MacCatalyst` runs the executable directly, capturing test output so you can see results.

### Why NOT run-bare for Mobile Platforms

iOS and tvOS tests require simulator infrastructure (boot simulator, install app, etc.) that `run-bare` doesn't provide. Use **mlaunch directly** to launch the app in the simulator with output capture.

**Why not `make run-ios`/`run-tvos`?** These use `dotnet build -t:Run` which wraps mlaunch through MSBuild. The `com.apple.gamed` stderr noise from the simulator causes MSBuild to treat the run as failed (exit code -1), even though mlaunch returns 0 and the tests pass. The NUnit results are also not reliably captured to stdout through the MSBuild layer.

Running mlaunch directly with `SIMCTL_CHILD_NUNIT_AUTOSTART=true` and `SIMCTL_CHILD_NUNIT_AUTOEXIT=true` bypasses MSBuild's error detection and captures the simulator app's stdout (including NUnit results) directly to the terminal.

## Reading Test Results

Look for this NUnit output pattern:

```
Tests run: 41 Passed: 41 Inconclusive: 0 Failed: 0 Ignored: 0
```

All tests should show **Failed: 0**.

## Handling Introspection Failures

### Type Crashes on Simulator

Some types crash when instantiated on simulator (hardware-dependent APIs). Add exclusions in:
- `tests/introspection/iOSApiCtorInitTest.cs` — iOS exclusions
- `tests/introspection/MacApiCtorInitTest.cs` — macOS exclusions

Exclusion mechanisms:
- **`Skip()` method** — Return `true` to skip a type entirely
- **`do_not_dispose` list** — Types that crash on disposal
- **`CheckHandle()` override** — Types returning `IntPtr.Zero`
- **`CheckToString()` override** — Types that crash on `.Description`

### Simulator Infrastructure Errors

`com.apple.gamed` connection errors are a known simulator environment issue. When running via mlaunch directly, these appear as stderr noise but don't affect the test results. When running via `make run-ios`/`run-tvos` (`dotnet build -t:Run`), these stderr messages cause MSBuild to report failure (exit code -1) even though tests pass — this is why running mlaunch directly is preferred.

## Build Timeouts

Builds can take up to 60 minutes. Do not set short timeouts on make/build commands.

## Stale Build Artifacts

If you encounter unexpected failures — types crashing in unrelated frameworks, false "pre-existing" failures, protocol conformance mismatches that shouldn't exist — the most likely cause is **stale `_build/` artifacts**.

**Fix:** Run a full `make all && make install` before re-testing. This rebuilds everything cleanly and installs fresh assemblies.

**Warning signs of stale artifacts:**
- Introspection tests report failures not seen on a clean checkout
- Types crash in `-[description]` or `-[dealloc]` in frameworks you didn't modify
- Cecil tests report unexpected known-failure mismatches

## Introspection Exclusion Rules

When adding exclusions for types that crash on simulator:

- **NEVER skip an entire namespace.** Always add exclusions for specific types only.
- **Prefer fixing the binding over adding test exclusions.** For example, if a `[DesignatedInitializer]` constructor crashes when passed null, remove `[NullAllowed]` from the parameter rather than excluding the type from introspection tests.
- Only add exclusions for genuine simulator/beta SDK bugs that can't be fixed in managed code.

## Monotouch Tests

For manually bound APIs (P/Invokes, manual properties), run the monotouch-test suite:

```bash
# Build and run monotouch-tests (uses simulator)
make -C tests/monotouch-test run
```

Or run specific test fixtures:

```bash
# Run via dotnet test with a filter
dotnet test tests/monotouch-test/ --filter "FullyQualifiedName~MonoTouchFixtures.CoreText.FontTest"
```

Test files are in `tests/monotouch-test/{FrameworkName}/`. See [binding-patterns.md](binding-patterns.md) for the test file template and conventions.
