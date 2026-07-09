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
# 1. Generate reference bindings from the SDK headers (do this first).
#    Output goes to api/<Platform>/ApiDefinition.cs (gitignored) — grep these
#    for correct selectors, signatures, and [Introduced] versions.
make -C tests/xtro-sharpie gen-all

# 2. Classify all platforms and run the sanity check.
#    There are NO run-ios/run-tvos/run-macos/run-maccatalyst targets.
make -C tests/xtro-sharpie dotnet-classify

# If unclassified entries appear
make -C tests/xtro-sharpie unclassified2todo
```

`dotnet-classify` runs every platform then sanity. A resolved-but-still-present `.todo` entry prints `?fixed-todo?` and returns non-zero — remove the resolved entries (and `git rm` emptied `.todo` files), then re-run until `Sanity check passed`. Setting `AUTO_SANITIZE=1` makes xtro auto-remove those resolved lines and delete emptied files for you, but any surrounding related **comments** in the `.todo`/`.ignore` files must still be removed manually.

### `!extra-enum-value!` — a managed enum value the native platform lacks

Classify reports `!extra-enum-value! Managed value N for <Enum>.<Member> is available for the current platform while the value in the native header is not` when a bound enum **value** exists in the managed assembly for a platform where the SDK header marks it unavailable. Fix it at the **right granularity** — read the header first:

- **The whole native enum is unavailable** on the platform (the `NS_ENUM`/typedef itself is `API_UNAVAILABLE(macos)`, or it has no valid native values/API surface there — e.g. imported only under `#if TARGET_OS_IPHONE`) → mark the **enum type** with `[NoMac]` (type-level `[No<Platform>]` removes the entire enum from that platform's assembly). Precedent: `AVCaptureSessionInterruptionReason` in `src/AVFoundation/Enums.cs` (plain type-level `[NoMac]`).
- **The enum type is available but only certain values are not** → put `[NoMac]` on the **individual member(s)**, leaving the type available. Precedent: `AVAudioSessionCategoryOptions` in `src/AVFoundation/Enums.cs` carries per-value `[NoMac]` on specific members.
- **The managed value has no native counterpart at all** (the variant message is `!extra-enum-value! Managed value N … not found in native headers`) → `[No<Platform>]` cannot fix it; the value simply isn't in any header. Resolve by removing the managed value, or — for a synthetic `0`/`None` — rely on the check's zero-value allowance, or add an `.ignore` entry with justification.

> ❌ **Do not** mark the whole type `[NoMac]` just to silence one value — that strips valid members (e.g. `None`) from the platform assembly and, because the native enum is still available there, flips into a `!missing-enum!` failure. Match the native availability exactly; if a sibling enum already models the same header, copy its platform attributes. (Avoid citing an `#if XAMCORE_5_0`-guarded attribute as a template — those are deferred breaking changes, not active availability.)

### Xtro File Types

| Extension | Purpose |
|-----------|---------|
| `.todo` | APIs that need to be bound |
| `.ignore` | APIs intentionally not bound (with justification) |
| `.deprecated` | Deprecated APIs |

### Xtro only checks native → managed (no "extra selector")

`SelectorCheck` iterates the **native** header declarations and emits `!missing-selector!` for any native selector with no managed binding. It **never** flags an *extra*, managed-only selector. Consequence: when Apple **removes** a selector in a new SDK, deleting the managed binding is xtro-neutral — but you must still delete it, or the introspection `ApiSelectorTest` fails at runtime (the managed selector no longer responds). (`tests/xtro-sharpie/xtro-sharpie/SelectorCheck.cs`.)

## Cecil Commands

```bash
make -C tests/cecil-tests run-tests
```

Cecil tests check for consistency in the compiled assemblies (attribute usage, naming conventions, etc.).

### Undocumented-member failures (`VerifyEveryVisibleMemberIsDocumented`)

Adding public members can fail the `VerifyEveryVisibleMemberIsDocumented` test — it lists every new member that has no XML documentation. Two ways to resolve it:

- **Preferred**: write real XML doc comments for the new members (the skill forbids `"To be added."` placeholders).
- **Baseline path**: if the framework's existing members are already listed in `tests/cecil-tests/Documentation.KnownFailures.txt` (i.e. the whole framework is undocumented), add your new members to that baseline instead, to stay consistent:

  ```bash
  # Regenerates Documentation.KnownFailures.txt (the whole sorted baseline is
  # rewritten, so the git diff should show ONLY your new members added).
  # This run exits non-zero by design — it's the "re-run to confirm" assert.
  WRITE_KNOWN_FAILURES=1 make -C tests/cecil-tests run-tests
  # Confirm a clean pass (exit 0) and commit the updated known-failures file.
  make -C tests/cecil-tests run-tests
  ```

  Review the `git diff` of `Documentation.KnownFailures.txt` — it must contain **only** your new members.

### Which new members need a doc/baseline entry

bgen auto-documents *some* generated members, so not every new public API fails `VerifyEveryVisibleMemberIsDocumented`. Knowing which is which predicts your diff:

- **`[Field]` constant properties** (NSString notification/key constants) → bgen generates a `<summary>` → **no** baseline entry needed.
- **`NS_TYPED_ENUM` smart enum** → the enum **type** and each **field** are **not** auto-documented → they need a `T:` line and one `F:` per member in `Documentation.KnownFailures.txt`. The bgen-generated `...Extensions` class (`GetValue`/`GetConstant`/`ToFlags`) **is** auto-documented → **no** entries for it.
- **`[Export]` methods** → **not** auto-documented → each needs a doc comment or a baseline entry.

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

> ⚠️ **Xcode 27: `simctl create` dropped `--json`.** mlaunch auto-creates the sim device via `simctl create … --json`, which now fails with `MT1008 … simctl: unrecognized option '--json'`. Workaround: **pre-create** the device with the exact name mlaunch expects so it finds it instead of creating it, e.g. `xcrun simctl create "iPhone 16 Pro - iOS 27.0" <devicetype-id> <runtime-id>`, then re-run mlaunch. (Environment-specific to Xcode 27 until mlaunch is updated.)

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

### Desktop `run-bare` crashes on privacy-gated types (TCC)

Running desktop introspection via `run-bare` in a **headless terminal** (no GUI session) can SIGABRT with `__TCC_CRASHING_DUE_TO_PRIVACY_VIOLATION__` when a fixture instantiates a privacy-gated type the OS can't show a permission prompt for. Observed on **Mac Catalyst** (whose ctor suite is `iOSApiCtorInitTest`) instantiating `CoreLocationUI.CLLocationButton` (`src/corelocationui.cs:27` — iOS/Mac Catalyst only). This is **environmental, not a binding bug** — the same class of ctor/dealloc crash as the documented **macOS** `AVKit.AVCaptureView` exclusion (`tests/introspection/MacApiCtorInitTest.cs:158-161`, macOS-only).

To still validate the other fixtures, run a **single fixture** (bypassing the crashing `ApiCtorInitTest`):

```bash
make -C tests/introspection/dotnet/MacCatalyst run-bare \
  RUN_ARGUMENTS="--test=Introspection.iOSApiSelectorTest"
```

`run-bare` forwards `RUN_ARGUMENTS` to the executable (`tests/common/shared-dotnet.mk`), and Touch.Unit's `--test=` filter (`tests/common/Touch.Unit/Touch.Client/Runner/TouchOptions.cs:121`) runs just that fixture. Swap in `iOSApiSignatureTest`, `iOSApiFieldTest`, `iOSApiPInvokeTest`, etc. The real gate remains CI on a signed/GUI image.

### Selector Not Found (Declared but Not Implemented)

A **different** failure mode from ctor-init crashes: `ApiSelectorTest` checks at runtime whether each bound selector is actually implemented (`instancesRespondToSelector:` for instance members, `respondsToSelector:` for static members) and fails if it isn't. On a **beta SDK**, Apple often *declares* a selector in the header (so your `[Export]` and platform attributes are correct) but hasn't *implemented* it in the beta runtime yet — a genuine runtime gap, not a binding bug.

**Confirm it really is a beta-runtime gap before skipping** (otherwise you'd mask a real binding bug): the SDK header declares the selector for this platform/version, the `[Export]`/`[Bind]` string is correct, and the failure is purely a runtime respond-to-selector miss. **Do not** "fix" it by changing availability or adding `[No<Platform>]` — that would be wrong (the header says it's available) and would make **xtro** report the API as *missing* (`!missing-selector!`).

Add a narrow skip in the **selector** test (this is `ApiSelectorTest`, not `ApiCtorInitTest`):
- **macOS** → `tests/introspection/MacApiSelectorTest.cs`, override `Skip (Type type, string selectorName)` (selector-first `switch`). Existing precedent: the `accessibilityNotifiesWhenDestroyed` case ("the header declares this … but it doesn't even respondsToSelector").
- **iOS / tvOS / MacCatalyst** → `tests/introspection/iOSApiSelectorTest.cs`, which **already has** a `Skip (Type type, string selectorName)` override (selector-first `switch`) — **extend its existing `switch (type.Name)`; do not add a second override** (a duplicate is a CS0111 compile error). Follow the existing `AVAssetWriter` Pro Video Storage precedent there: `#if __TVOS__` / `#if __MACCATALYST__`-guarded `case` blocks, `TestRuntime.IsSimulator`-gated for a simulator-only gap (tvOS 27) or unconditional for a real-runtime gap (Mac Catalyst 27). The override already ends in `base.Skip (type, selectorName)`, so the base class's existing skips still apply.

**Skip only the platform(s) that actually fail**, at the narrowest scope (type + selector + platform):
- **Real-hardware platforms** (macOS, MacCatalyst) run on real Macs, so a missing selector is absent everywhere on that OS → skip **unconditionally**. Because such a skip does **not** self-expire, it will keep masking the selector once the GA OS implements it — leave a `// TODO: remove once <OS> GA ships this selector` note and revisit at the next Xcode bump.
- **Simulator-only gaps** (a selector the device implements but the simulator lacks, e.g. iOS/tvOS) → gate with `TestRuntime.IsSimulator` (note `TestRuntime.IsSimulatorOrDesktop` is the broader idiom used elsewhere in these files).
- Never blanket-skip a platform that actually responds — that masks future regressions.

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

> ⚠️ **`run-bare` doesn't work for mobile.** The `run-bare` target exists on every platform (it runs the built executable directly via `$(EXECUTABLE) --autostart --autoexit`), but only desktop (macOS/MacCatalyst) can be launched that way. iOS and tvOS use the simulator via `dotnet build -t:Run` with `SIMCTL_CHILD_NUNIT_AUTOSTART=true` and `SIMCTL_CHILD_NUNIT_AUTOEXIT=true` environment variables (set automatically by the shared Makefile). No manual mlaunch invocation is needed for monotouch-tests — unlike introspection.

### Running Specific Test Fixtures

```bash
# Run via dotnet test with a filter
dotnet test tests/monotouch-test/ --filter "FullyQualifiedName~MonoTouchFixtures.CoreText.FontTest"
```

Test files are in `tests/monotouch-test/{FrameworkName}/`. See [binding-patterns.md](binding-patterns.md) for the test file template and conventions.
