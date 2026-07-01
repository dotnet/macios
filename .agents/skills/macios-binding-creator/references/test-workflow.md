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

## Runtime-Only Protocol Conformance ("conformance not in headers")

**Symptom:** After building against a new Xcode (usually a **beta**), a protocol-conformance introspection test fails with a message like:

```
NSViewCornerRadii conforms to NSSecureCoding but does not implement INSSecureCoding
```

(the same pattern applies to `NSCoding`, `NSCopying`, and `NSMutableCopying`). This test lives in `tests/introspection/ApiProtocolTest.cs` and checks the **runtime** via `conformsToProtocol:`.

**Root cause:** The Objective-C **runtime** conforms the type to the protocol, but the **SDK header does not declare it**. Apple frequently adds a conformance to the runtime on a new beta before (or without) updating the header. Confirm by reading the header — e.g. `@interface NSViewCornerRadii : NSObject <NSCopying>` declares only `NSCopying`, so runtime `NSCoding`/`NSSecureCoding` is undeclared.

**Fix — a test-only introspection Skip, NOT a binding change.** Bindings mirror the **header**, and adding an interface to a binding is a public-API commitment — if you add the undeclared conformance and Apple drops it in a later beta, that's a breaking change. xtro is header-driven and won't ask for it (it reports only *missing* header-declared conformances, never extra runtime ones — `ObjCInterfaceCheck.cs` has a `// TODO : check for extraneous protocols`). The binding is already correct; only the test must tolerate the extra runtime conformance.

> ⚠️ **First rule out a real binding bug.** The *same* failure message appears when the **header declares** the conformance but the binding forgot the interface — and then Skipping is WRONG. Before skipping, run xtro: if it reports `!missing-protocol-conformance! <Type> should conform to <Protocol>`, the header (or a **category**) declares it → **add the conformance to the binding** (see [binding-patterns.md](binding-patterns.md) → "Adding Protocol Conformance to Existing Types"), do NOT Skip. Only Skip when xtro is **silent** — i.e. the target-platform headers (including category declarations, not just the primary `@interface`) genuinely lack the conformance and it exists only at runtime.

Add the type to the matching protocol's `Skip (Type type, string protocolName)` block:

| Scope | File |
|-------|------|
| All platforms | `tests/introspection/ApiProtocolTest.cs` (base) |
| macOS only | `tests/introspection/MacApiProtocolTest.cs` |
| iOS / tvOS / Mac Catalyst | `tests/introspection/iOSApiProtocolTest.cs` |

Both platform overrides end with `return base.Skip (type, protocolName)`, so platform-specific and base entries both apply. **When unsure of scope, use the base `ApiProtocolTest.cs`** — it's compiled on every platform and a non-matching `type.Name` is simply inert, which is why many single-platform entries (e.g. iOS-only `HMAccessorySetupPayload`) already live there. Reach for a platform file only for a genuinely platform-specific conformance — e.g. the Xcode 27 CarPlay `NSCopying` skips live in `iOSApiProtocolTest.cs` (which also serves tvOS and Mac Catalyst).

```csharp
// in Skip (Type type, string protocolName)
switch (protocolName) {
case "NSSecureCoding":
    switch (type.Name) {
    // Xcode 27 - Conformance not in headers
    case "NSViewCornerRadii":
        return true;
    // ... existing cases ...
    }
    break;
```

> ⚠️ **`NSSecureCoding` implies `NSCoding` — add BOTH.** Because `NSSecureCoding : NSCoding`, a runtime `NSSecureCoding` conformance makes `conformsToProtocol:` return true for **both**, so the `Coding` *and* `SecureCoding` tests fail. Add a `case` in **both** the `NSCoding` and `NSSecureCoding` blocks (this is exactly why the real `NSViewCornerRadii` fix needed both).

> ❌ **Placement trap — this has broken CI.** The protocol `case` blocks — `NSCopying`, `NSMutableCopying`, `NSCoding`, `NSSecureCoding` — are **adjacent** inside one `switch (protocolName)`. A `case "<TypeName>":` dropped into the wrong block still **compiles**; the intended protocol still errors, so the failure may only surface in the relevant introspection run (often CI), and you can silently disable coverage for whatever type/protocol you displaced. After editing, **verify the enclosing `case "<Protocol>":` for every entry you add** (e.g. `grep`/`awk` the file to confirm each `case "<TypeName>":` sits under the intended protocol, not a neighbour).

> ⚠️ **Match the exact `type.Name`; subclasses are NOT auto-covered.** The inner switch keys on `type.Name`, so every affected type needs its own `case` — including subclasses. Example: the Xcode 27 CarPlay `NSCopying` case adds `CPButton` and its subclasses `CPContactCallButton`, `CPContactDirectionsButton`, `CPContactMessageButton`, plus the independent (`NSObject`-based) `CPTextButton` and `CPTravelEstimates` — each needs its own `case`. Add a `// Xcode NN - Conformance not in headers` comment to match the established convention.

> ❌ **Don't confuse this with the defer decision.** Deferral is about *whether to bind a whole new API at all*: if a beta API is *in the header* but *absent from the device runtime*, you may choose not to bind it yet (add a `!missing-…!` entry to the xtro `.todo`) to avoid a breaking change if Apple later drops it. Runtime-only conformance is a different situation — the type is *already bound correctly* and only a runtime-added conformance differs from the header — so the fix is an introspection Skip, not deferral.

## Monotouch Tests

For manually bound APIs (P/Invokes, manual properties), run the monotouch-test suite per-platform.

**Platform casing matters** — use `iOS`, `tvOS`, `macOS`, `MacCatalyst` exactly.

### Per-Platform Commands

| Platform | Build | Run |
|----------|-------|-----|
| iOS | `make -C .../dotnet/iOS build` | `make -C .../dotnet/iOS run` |
| tvOS | `make -C .../dotnet/tvOS build` | `make -C .../dotnet/tvOS run` |
| macOS | `make -C .../dotnet/macOS build` | `make -C .../dotnet/macOS run-bare` |
| MacCatalyst | `make -C .../dotnet/MacCatalyst build` | `make -C .../dotnet/MacCatalyst run-bare` |

Where `...` = `tests/monotouch-test`.

Alternatively, from the parent directory for the **simulator** platforms: `make -C tests/monotouch-test/dotnet run-iOS`, `run-tvOS`. (The parent `run-macOS`/`run-MacCatalyst` targets delegate to `run`, which does **not** capture desktop output — use the per-subdirectory `run-bare` from the table above for macOS/MacCatalyst.)

> ⚠️ **Desktop platforms (macOS, MacCatalyst)**: Use `run-bare` for captured test output — same as introspection. `run` launches the app via `dotnet build -t:Run` which doesn't capture stdout.

> ⚠️ **`run-bare` is desktop-only.** iOS and tvOS use the simulator via `dotnet build -t:Run` with `SIMCTL_CHILD_NUNIT_AUTOSTART=true` and `SIMCTL_CHILD_NUNIT_AUTOEXIT=true` environment variables (set automatically by the shared Makefile). No manual mlaunch invocation is needed for monotouch-tests — unlike introspection.

### Running Specific Test Fixtures

```bash
# Run via dotnet test with a filter
dotnet test tests/monotouch-test/ --filter "FullyQualifiedName~MonoTouchFixtures.CoreText.FontTest"
```

Test files are in `tests/monotouch-test/{FrameworkName}/`. See [binding-patterns.md](binding-patterns.md) for the test file template and conventions.
