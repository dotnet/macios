---
name: macios-binding-creator
description: >
  Create C# bindings for Apple frameworks in dotnet/macios. USE FOR: binding new
  APIs, implementing .todo file entries, creating Xcode SDK bindings, binding
  AVFoundation/UIKit/AppKit or any Apple framework, "bind this framework",
  "implement these APIs". DO NOT USE FOR: Xcode beta version bumps (use
  macios-xcode-beta-update skill), CI failure investigation (use
  macios-ci-failure-inspector skill).
---

# macios Binding Creator

Create C# bindings for Apple platform APIs in the dotnet/macios repository. This skill encodes the end-to-end workflow: from reading `.todo` files through implementation, building, and validating with xtro, cecil, and introspection tests on all platforms.

## When to Use This Skill

Use this skill when:
- Asked to bind a new Apple framework or add missing API bindings
- Implementing entries from `.todo` files in `tests/xtro-sharpie/api-annotations-dotnet/`
- Creating bindings for a new Xcode SDK release
- Adding new types, properties, methods, or enum values to existing framework bindings
- Asked to "bind", "implement", or "add bindings for" any Apple framework

## Prerequisites

- Repository checked out and configured (`./configure` already run)
- Xcode installed at the expected `XCODE_DEVELOPER_ROOT` path
- A successful `make world` or `make all && make install` already completed

## Process

### Step 1: Understand What to Bind

Check the `.todo` files to see what APIs are missing:

```bash
ls tests/xtro-sharpie/api-annotations-dotnet/*-{FrameworkName}.todo
cat tests/xtro-sharpie/api-annotations-dotnet/iOS-{FrameworkName}.todo
```

Each `.todo` file lists missing APIs per platform (iOS, tvOS, macOS, MacCatalyst). The format is:
```
!missing-selector! ClassName::methodName: not bound
!missing-type! ClassName not bound
!missing-field! ClassName FieldName not bound
!missing-enum-value! EnumName::ValueName not bound
```

> ❌ **NEVER** bind APIs that aren't in the `.todo` files unless explicitly asked. The `.todo` files are the source of truth for what's missing.

### Step 2: Generate Reference Bindings

Run the xtro generator to produce reference C# bindings from the SDK headers:

```bash
make -C tests/xtro-sharpie gen-all
```

This creates generated `.cs` files you can search to find the correct C# signatures, attributes, and patterns for the APIs you need to bind. Use these as reference — don't copy them verbatim.

### Step 3: Research the Native API

Before implementing, understand the native API:
- Search the generated reference bindings for the correct Objective-C selectors
- Read Apple header files when available (under `$XCODE_DEVELOPER_ROOT`)
- Check existing bindings in `src/frameworkname.cs` for patterns used in the same framework

### Step 4: Implement Bindings

#### Determine the Correct Availability Version

Before writing any bindings, determine the correct availability version for each API. The version represents **when Apple introduced the API**, not the current SDK version.

**Primary source of truth: the generated reference bindings** from Step 2. After running `make -C tests/xtro-sharpie gen-all`, search the generated `.cs` files for the API you're binding — they include `[Introduced]` attributes extracted from Apple's SDK headers with the correct per-platform introduction versions. Always use these versions.

```bash
# Find the generated reference binding for a specific API.
# gen-all (Step 2) writes these to api/<Platform>/ApiDefinition.cs (gitignored),
# so run Step 2 first. Widen to api/*/ if a symbol isn't in ApiDefinition.cs.
grep -rn "SomeClassName\|SomeMethodName" tests/xtro-sharpie/api/*/ApiDefinition.cs
```

If the generated reference bindings don't include version information, fall back to these sources:
1. **Apple SDK headers** — search under `$XCODE_DEVELOPER_ROOT` for `API_AVAILABLE` macros
2. **Current SDK version from `SdkVersions.cs`** — use this only for **brand-new APIs** introduced in the current Xcode release:

```bash
grep -E 'public const string (iOS|TVOS|OSX|MacCatalyst) ' tools/common/SdkVersions.cs
```

> ❌ **NEVER assume the current SDK version is the introduction version for all APIs.** The SDK version (e.g., `26.5`) is only correct for APIs that are genuinely new in this Xcode release. When adding an existing framework to a new platform (e.g., MediaSetup to MacCatalyst), or adding enum members that were introduced in an earlier release, the introduction version will be different — check the generated reference bindings or Apple headers.

If the user specifies a version, use that instead. **Ask the user if you're unsure which version to use.**

#### File Locations

Bindings go in these locations:
- **`src/frameworkname.cs`** — API definitions (interfaces with `[Export]` attributes)
- **`src/FrameworkName/`** — Manual code (partial classes, enums, P/Invokes, extensions)
- **`src/frameworks.sources`** — Maps frameworks to source files (update if adding new files)

Key binding patterns:

```csharp
// New property on existing class
[Export ("allowsCaptureOfClearKeyVideo")]
bool AllowsCaptureOfClearKeyVideo { get; set; }

// New method on existing class
[Export ("setCaptionPreviewProfileId:")]
void SetCaptionPreviewProfileId ([NullAllowed] string profileId);

// New notification field
[Field ("AVPlayerInterstitialEventMonitorScheduleRequestedNotification")]
[Notification]
NSString ScheduleRequestedNotification { get; }
```

> ❌ **NEVER** forget platform availability attributes. Every new API must have `[iOS]`, `[Mac]`, `[TV]`, `[MacCatalyst]`, and/or `[No*]` attributes matching the `.todo` file platforms where the API appears. This includes **all** binding types:
> - API definition interfaces and members in `src/frameworkname.cs` — use `[iOS (X, Y)]`, `[Mac (X, Y)]`, etc.
> - P/Invoke wrappers and manual properties in `src/FrameworkName/*.cs` — use `[SupportedOSPlatform ("iosX.Y")]`, `[SupportedOSPlatform ("macos")]`, etc.
> - Fields, constants, and enum values
> - **Individual enum members** added to an existing enum — each new member needs its own `[iOS (X, Y)]` etc. with the version the **member** was introduced, even if the enum itself has an older version. Check the generated reference bindings for the correct per-member version.

> ❌ **NEVER** use `string.Empty` — use `""`. Never use `Array.Empty<T>()` — use `[]`.

> ❌ **NEVER** add placeholder XML documentation text like `"To be added."` anywhere — not in `<remarks>`, `<summary>`, `<returns>`, `[Async (XmlDocs = ...)]`, or any other XML doc element. Either write meaningful documentation or omit the element entirely.

> ❌ **NEVER** forget `[NullAllowed]` on `out NSError error` parameters. Every method that takes `NSError**` (bound as `out NSError error`) must use `[NullAllowed] out NSError error`. This applies to all error-returning methods — the error output is null on success.

> ❌ **NEVER** forget `#nullable enable` at the top of every new C# file you create.

> ❌ **NEVER** use non-blittable types (`bool`, `char`) as backing fields in structs. Use `byte` (for `bool`) and `ushort`/`short` (for `char`) with property accessors. See [references/binding-patterns.md](references/binding-patterns.md) for the correct pattern.

> ❌ **NEVER** use `XAMCORE_5_0` for new code. `XAMCORE_5_0` is only for fixing breaking API changes on existing types that shipped in prior releases. However, when xtro reports a mismatch on an **existing** type (e.g., wrong enum backing type, missing `[Native]`), and fixing it directly would be a breaking change, you **must** use `#if XAMCORE_5_0` guards to preserve binary compatibility while queuing the fix for the future. Add a `.ignore` entry for the xtro mismatch. See [references/binding-patterns.md](references/binding-patterns.md) § "XAMCORE_5_0 Pattern for Existing Types".

> ❌ **NEVER** use `#pragma warning disable 0169` for struct fields. Instead, wrap public methods and properties inside `#if !COREBUILD` (but NOT fields — bgen needs to know the struct size).

> ⚠️ **Protocol methods returning opaque types**: If a protocol method returns an opaque C type (e.g., `PMPrintSession`) that has a managed `NativeObject` wrapper in `src/FrameworkName/`, do NOT use `IntPtr`. Register the type as a bgen marshal type so bgen can generate proper `Runtime.GetINativeObject<T>()` marshaling. See [references/binding-patterns.md](references/binding-patterns.md) § "NativeObject Return Types in Protocol Methods".

> ⚠️ Place a space before parentheses and brackets: `Foo ()`, `Bar (1, 2)`, `myarray [0]`.

> ⚠️ Method names should follow .NET naming conventions — use verb-based names, not direct Objective-C selector translations (e.g., `BuildMenu` not `MenuWithContents`).

> ❌ **NEVER** change the casing of the Objective-C class name **prefix** in C# type names. `ARSession` stays `ARSession` (not `ArSession`), `AVPlayer` stays `AVPlayer` (not `AvPlayer`). But an acronym *inside* the name (after the prefix) DOES follow .NET rules — `NSURLSession` → `NSUrlSession`, `NSURLSessionHandler` → `NSUrlSessionHandler` (the `NS` prefix is kept, but `URL` becomes `Url`). When creating new manual types, match the framework's established prefix (e.g., all ARKit types use `AR*`, all CoreGraphics types use `CG*`). The .NET acronym rules (SIMD → Simd, URL → Url) apply within property/method names **and** to acronyms inside type names, NOT to the leading class prefix. (A few frameworks instead preserve an inner acronym across their whole family — e.g. CoreGraphics `CGPDF*` — so match the existing sibling types when a framework is consistent.)

> ⚠️ For in depth binding patterns and conventions See [references/binding-patterns.md](references/binding-patterns.md)

> ⚠️ **Struct array parameters**: When an API takes a C struct pointer + count (e.g., `MyStruct*` + `NSUInteger`), bind the raw pointer as `[Internal]` with `IntPtr`, then create a manual public wrapper using the **factory pattern** with `fixed`. See [references/binding-patterns.md](references/binding-patterns.md) § "Struct Array Parameter Binding".

### Step 4b: Platform Exclusion Patterns for Manual Types

When a manually coded type (struct, extension, etc.) is not available on a specific platform (e.g., tvOS), you must handle compilation on that platform:

1. In the manual code file (`src/FrameworkName/MyStruct.cs`), wrap the struct body with `#if !__TVOS__`
2. Add `[UnsupportedOSPlatform ("tvos")]` on the struct
3. In the API definition file (`src/frameworkname.cs`), add a type alias at the top so compilation succeeds:

```csharp
#if __TVOS__
using MyStruct = Foundation.NSObject;
#endif
```

The `[NoTV]` attribute on the API definition interface ensures the type won't appear in the final tvOS assembly, while the alias prevents compilation errors from method signatures that reference the struct.

> ❌ **NEVER** use platform-specific source file lists (e.g., appending a per-framework list to `MACOS_DOTNET_SOURCES`) for platform-conditional code. Instead, use preprocessor directives (`#if __MACOS__`, `#if !__TVOS__`, `#if __IOS__`) within shared source files. Platform-specific source file lists are for the build system, not for conditional compilation of individual types or members.

Available preprocessor symbols for platform checks:
- `__MACOS__` (preferred) / `MONOMAC` — macOS
- `__IOS__` — iOS
- `__TVOS__` (preferred) / `TVOS` — tvOS
- `__MACCATALYST__` — Mac Catalyst

> ⚠️ **Foundation/TextKit types shared by AppKit and UIKit** (e.g. `NSTextList`, `NSParagraphStyle`) are bound once in `src/xkit.cs`, not duplicated in `appkit.cs`/`uikit.cs`. If a macOS-only type becomes exposed to UIKit, consolidate it there (share identical enums, split only divergent ones, guard macOS divergences with `#if MONOMAC`, keep back-dated availability). See [references/binding-patterns.md](references/binding-patterns.md) → "Shared AppKit/UIKit Types".

### Step 5: Build

Rebuild **and install** so the test suites — which read the installed NuGet packs, not `src/build/` — pick up your changes:

```bash
make all && make install
```

> ❌ **NEVER** use `make -C src build`. There is no `build` target in `src/Makefile`, so it matches the `src/build/` output directory and is a silent no-op ("Nothing to be done for `build'") that compiles nothing — you then validate against **stale** assemblies. Use `make all && make install` (or `make world` for a full rebuild).

Fix any compilation errors before proceeding. Builds can take up to 60 minutes — do not timeout early.

### Step 5b: Write Monotouch Tests for Manual Bindings

For any manually bound APIs (P/Invokes, manual properties on partial classes, struct accessors), add tests in `tests/monotouch-test/{FrameworkName}/`.

> ⚠️ **Only run monotouch-tests (Step 6d) if you added or modified test files in this step.** If no manual bindings were added (i.e., all APIs were bound via `[Export]` in the API definition file), skip both this step and Step 6d.

```csharp
using CoreText;  // framework being tested
using NUnit.Framework;

namespace MonoTouchFixtures.CoreText {  // MonoTouchFixtures.{FrameworkName}

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class FontTest {

		[Test]
		public void UIFontType_SystemFont ()
		{
			TestRuntime.AssertXcodeVersion (26, 4);  // match the availability version

			using (var font = new CTFont ("Helvetica", 12)) {
				var fontType = font.UIFontType;
				Assert.AreEqual (CTUIFontType.System, fontType);
			}
		}
	}
}
```

Key patterns:
- **Namespace**: `MonoTouchFixtures.{FrameworkName}` (e.g., `MonoTouchFixtures.CoreText`)
- **Version guards**: Use `TestRuntime.AssertXcodeVersion (major, minor)` matching the API's availability version. This skips the test on older runtimes instead of failing.
- **Resource cleanup**: Always use `using` statements for handle-based types
- **Test focus**: Exercise the manual binding — call the P/Invoke wrapper, verify the property returns sensible values, test round-trip behavior for setters

> ⚠️ If adding a new test file, make sure the `.csproj` at `tests/monotouch-test/` picks it up (it typically uses wildcard includes, but verify).

See [references/binding-patterns.md](references/binding-patterns.md) for more monotouch-test patterns.

> ⚠️ **Stale build artifacts**: If you encounter unexpected test failures (SIGABRT, segfaults in unrelated types, false "pre-existing" failures), **always run `make world` FIRST** before investigating. Never conclude a failure is "pre-existing" without rebuilding — stale `_build/` artifacts are the #1 cause of spurious introspection crashes after binding changes.

### Step 6: Validate with Tests

Run all three test suites. **Run them sequentially, not in parallel.**

#### 6a. Xtro Tests

There are **no** `run-ios`/`run-tvos`/`run-macos`/`run-maccatalyst` xtro targets. Regenerate the reference bindings, then classify every platform (this also runs the sanity check):

```bash
make -C tests/xtro-sharpie gen-all
make -C tests/xtro-sharpie dotnet-classify
```

`dotnet-classify` classifies all platforms and then runs sanity. When a `.todo` entry has been resolved by your binding but the `.todo` file still lists it, sanity prints `?fixed-todo?` and exits non-zero — that is the cleanup signal, **not** a passing result. Loop until it passes:

1. For each `?fixed-todo?` entry you bound, remove that line from its `.todo` file (and `git rm` the file if it becomes empty — see next note).
2. Re-run `make -C tests/xtro-sharpie dotnet-classify` until it prints `Sanity check passed` (exit 0).

Any entries that remain unresolved need binding or explicit `.ignore` entries with justification.

> ⚠️ **`!extra-enum-value!`**: if classify reports a managed enum value that the native header marks unavailable on a platform, fix it at the right scope — put `[No<Platform>]` on the **whole enum type** only if the *entire* native enum is unavailable there, otherwise put `[No<Platform>]` on the **individual value(s)**. Never mark the whole type just to silence one value (it strips valid members like `None`). See [references/test-workflow.md](references/test-workflow.md) § "`!extra-enum-value!`".

> ❌ **ALWAYS delete empty `.todo` files** after resolving all entries: `git rm tests/xtro-sharpie/api-annotations-dotnet/{platform}-{Framework}.todo`. Do not leave empty `.todo` files in the repository — they cause xtro test noise.

#### 6b. Cecil Tests

```bash
make -C tests/cecil-tests run-tests
```

> ⚠️ Adding public members can fail `VerifyEveryVisibleMemberIsDocumented` — the failure lists your new, undocumented members. Either write real XML documentation for them, or — if the framework's existing members are already listed in `tests/cecil-tests/Documentation.KnownFailures.txt` (the whole framework is undocumented) — regenerate that baseline to stay consistent: `WRITE_KNOWN_FAILURES=1 make -C tests/cecil-tests run-tests` (this run exits non-zero by design), then re-run **without** the env var to confirm exit 0. Verify the `git diff` of the known-failures file contains **only** your new members. See [references/test-workflow.md](references/test-workflow.md).

#### 6c. Introspection Tests (All Platforms)

**IMPORTANT:** Clean shared obj directories before each platform to avoid NETSDK1005 errors:

```bash
# iOS — build, then run via mlaunch directly for reliable output capture
rm -rf tests/common/Touch.Unit/Touch.Client/dotnet/obj tests/common/MonoTouch.Dialog/obj
make -C tests/introspection/dotnet/iOS clean
make -C tests/introspection/dotnet build-ios
# Get the app path and run via mlaunch directly:
APP_PATH=$(make -C tests/introspection/dotnet/iOS print-executable | sed 's|/introspection$||')
SIMCTL_CHILD_NUNIT_AUTOSTART=true \
SIMCTL_CHILD_NUNIT_AUTOEXIT=true \
$DOTNET_DESTDIR/Microsoft.iOS.Sdk/tools/bin/mlaunch \
  --launchsim "$APP_PATH" \
  --device :v2:runtime=com.apple.CoreSimulator.SimRuntime.iOS-26-4,devicetype=com.apple.CoreSimulator.SimDeviceType.iPhone-16-Pro \
  --wait-for-exit:true --

# tvOS — same approach as iOS
rm -rf tests/common/Touch.Unit/Touch.Client/dotnet/obj tests/common/MonoTouch.Dialog/obj
make -C tests/introspection/dotnet/tvOS clean
make -C tests/introspection/dotnet build-tvos
APP_PATH=$(make -C tests/introspection/dotnet/tvOS print-executable | sed 's|/introspection$||')
SIMCTL_CHILD_NUNIT_AUTOSTART=true \
SIMCTL_CHILD_NUNIT_AUTOEXIT=true \
$DOTNET_DESTDIR/Microsoft.tvOS.Sdk/tools/bin/mlaunch \
  --launchsim "$APP_PATH" \
  --device :v2:runtime=com.apple.CoreSimulator.SimRuntime.tvOS-26-4,devicetype=com.apple.CoreSimulator.SimDeviceType.Apple-TV-4K-3rd-generation-4K \
  --wait-for-exit:true --

# macOS (use run-bare for direct execution with captured output)
rm -rf tests/common/Touch.Unit/Touch.Client/dotnet/obj tests/common/MonoTouch.Dialog/obj
make -C tests/introspection/dotnet/macOS clean build
make -C tests/introspection/dotnet/macOS run-bare

# MacCatalyst (use run-bare for direct execution with captured output)
rm -rf tests/common/Touch.Unit/Touch.Client/dotnet/obj tests/common/MonoTouch.Dialog/obj
make -C tests/introspection/dotnet/MacCatalyst clean build
make -C tests/introspection/dotnet/MacCatalyst run-bare
```

> ⚠️ **iOS/tvOS output capture:** `make run-ios`/`run-tvos` uses `dotnet build -t:Run` which does NOT reliably capture the app's stdout. The `com.apple.gamed` stderr message causes MSBuild to report failure (exit code -1) even when tests pass, and NUnit results are lost. Use **mlaunch directly** as shown above to capture test output reliably.

> ⚠️ **mlaunch device strings:** Use `xcrun simctl list runtimes` and `xcrun simctl list devicetypes` to find the correct runtime and device type identifiers for your Xcode version. The `--device` format is `:v2:runtime=<runtime-id>,devicetype=<devicetype-id>`.

> ⚠️ **`clean` and `run-bare` must be run from the platform subdirectory** (e.g., `tests/introspection/dotnet/macOS/`), not from the parent `dotnet/` directory. The parent only has `build-%` and `run-%` pattern rules — there are no `clean-%` or `run-bare-%` targets.

> ⚠️ **macOS/MacCatalyst:** Use `run-bare` (not `run`) — `run` launches the app without waiting or capturing stdout. `run-bare` runs the executable directly to capture test output.

Look for this pattern in test output to confirm results:
```
Tests run: X Passed: X Inconclusive: X Failed: X Ignored: X
```

> ⚠️ **Beta-SDK protocol-conformance failures** (e.g. `X conforms to NSSecureCoding but does not implement INSSecureCoding`) usually mean the **runtime** conforms `X` to a protocol the **header doesn't declare**. If xtro is silent (no `!missing-protocol-conformance!` — confirm the header truly lacks it, else fix the binding), tolerate it with a test-only introspection **Skip** in `ApiProtocolTest.cs` (or `MacApiProtocolTest.cs`/`iOSApiProtocolTest.cs`) under the matching `case "<Protocol>":` — **not** by adding the conformance to the binding. See [references/test-workflow.md](references/test-workflow.md) → "Runtime-Only Protocol Conformance".

#### 6d. Monotouch Tests (only if you added tests in Step 5b)

Skip this step if no monotouch-test files were added or modified.

Run per-platform, using **exact casing** for platform names:

```bash
# iOS
make -C tests/monotouch-test/dotnet/iOS run

# tvOS
make -C tests/monotouch-test/dotnet/tvOS run

# macOS (use run-bare for captured output)
make -C tests/monotouch-test/dotnet/macOS run-bare

# MacCatalyst (use run-bare for captured output)
make -C tests/monotouch-test/dotnet/MacCatalyst run-bare
```

> ⚠️ **Platform casing matters**: Use `iOS`, `tvOS`, `macOS`, `MacCatalyst` exactly — not `ios`, `macos`, etc.

> ⚠️ **Desktop platforms**: Use `run-bare` (not `run`) for macOS and MacCatalyst — same reason as introspection: `run` launches without capturing stdout. `run-bare` is only available for desktop platforms.

### Step 7: Handle Test Failures

If introspection tests fail for newly bound types:
- Check if the type crashes on simulator (common for hardware-dependent APIs)
- Add exclusions in the platform-specific `ApiCtorInitTest.cs` files if needed
- Types that crash on init, dispose, or toString need specific exclusion entries
- **NEVER skip an entire namespace** — always add exclusions for specific types only
- **If a `[DesignatedInitializer]` constructor crashes (segfault) when passed null**, the correct fix is to **remove `[NullAllowed]` from that parameter** rather than adding introspection test exclusions. The null is genuinely not allowed by the native API.
- **If the `DesignatedInitializer` test reports `<Type> should re-expose <Base>::.ctor(...)`** — you bound a subclass (e.g. of `AUAudioUnit`) that inherits a designated initializer but doesn't re-declare it. The subclass must re-expose it. Simplest fix that passes with no other changes: re-declare the init as a public `[DesignatedInitializer]` `Constructor` with the same selector/signature. See [references/binding-patterns.md](references/binding-patterns.md) § "Re-exposing Designated Initializers in Subclasses" — including the failable-initializer (factory) variant, which additionally requires a `Match ()` case in `ApiCtorInitTest.cs`.
- **If introspection reports a selector is `not found` / does not respond** for an API you just bound (common on a **beta OS**), and the SDK header *does* declare that selector for this platform — it's a **beta-runtime gap**, not a binding bug: the binding is correct but the beta OS hasn't implemented the selector yet. **Do not** change availability or add `[No<Platform>]` (that would make xtro report the API *missing*). Add a narrow skip in the **selector** test (`ApiSelectorTest`, not `ApiCtorInitTest`) — `MacApiSelectorTest.cs` (macOS) or `iOSApiSelectorTest.cs` (iOS/tvOS/MacCatalyst) — for **only the failing platform(s)**, unconditional on real hardware (macOS/MacCatalyst) and `TestRuntime.IsSimulator`-gated only for simulator-only gaps. See [references/test-workflow.md](references/test-workflow.md) § "Selector Not Found (Declared but Not Implemented)".

If xtro still shows unresolved entries:
- Some APIs may be platform-specific (only available on device, not simulator)
- Create `.ignore` entries with comments explaining why they can't be bound
- Or create remaining `.todo` entries for known limitations

## Stop Signals

- Stop investigating test failures after identifying the root cause. Don't trace full call stacks.
- If a type crashes on simulator, add an exclusion and move on — don't try to fix simulator issues.
- Don't bind APIs beyond what's listed in the `.todo` files unless explicitly asked.
- Report results per platform after all tests pass. Don't re-run passing tests.

## Output Format

When reporting results, use this structure:

1. **APIs bound** — table of types/members added with their platforms
2. **Files changed** — list of modified files
3. **Test results** — per-platform pass/fail for xtro, cecil, introspection, and monotouch-tests
4. **Remaining items** — any `.todo` entries intentionally left unbound, with reasons

## References

- **Binding patterns and conventions**: See [references/binding-patterns.md](references/binding-patterns.md)
- **Test commands and troubleshooting**: See [references/test-workflow.md](references/test-workflow.md)
