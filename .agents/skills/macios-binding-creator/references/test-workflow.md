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

| Platform | Build | Run | Notes |
|----------|-------|-----|-------|
| iOS | `make -C tests/introspection/dotnet build-ios` | `make run-ios` | Simulator, captures output |
| tvOS | `make -C tests/introspection/dotnet build-tvos` | `make run-tvos` | Simulator, captures output |
| macOS | `make -C tests/introspection/dotnet build-macOS` | `make run-bare-macOS` | Direct execution |
| MacCatalyst | `make -C tests/introspection/dotnet build-MacCatalyst` | `make run-bare-MacCatalyst` | Direct execution |

### Why run-bare for Desktop Platforms

`make run-macOS` / `make run-MacCatalyst` uses `dotnet build -t:Run` which launches the app without waiting or capturing stdout. The make command exits immediately with success even while tests are still running.

`make run-bare-macOS` / `make run-bare-MacCatalyst` runs the executable directly, capturing test output so you can see results.

### Why NOT run-bare for Mobile Platforms

iOS and tvOS tests require simulator infrastructure (mlaunch, boot simulator, install app, etc.) that `run-bare` doesn't provide. Always use `make run-ios` / `make run-tvos` for these.

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

`com.apple.gamed` connection errors are a known simulator environment issue. If mlaunch returns exit code 0 and you see the NUnit results pattern, the tests passed despite the error.

## Build Timeouts

Builds can take up to 60 minutes. Do not set short timeouts on make/build commands.
