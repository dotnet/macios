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

Before writing any bindings, determine the SDK version you're targeting:

```bash
# Check the current SDK versions
grep -E 'public const string (iOS|TVOS|OSX|MacCatalyst) ' tools/common/SdkVersions.cs
# Or from Make.versions
grep '_NUGET_OS_VERSION=' Make.versions
```

Use the version from `SdkVersions.cs` (e.g., `26.2`) for all availability attributes. If the user specifies a different version (e.g., binding a beta branch at `26.4`), use that instead. **Ask the user if you're unsure which version to use.**

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

> ❌ **NEVER** use `string.Empty` — use `""`. Never use `Array.Empty<T>()` — use `[]`.

> ❌ **NEVER** forget `#nullable enable` at the top of every new C# file you create.

> ❌ **NEVER** use non-blittable types (`bool`, `char`) as backing fields in structs. Use `byte` (for `bool`) and `ushort`/`short` (for `char`) with property accessors. See [references/binding-patterns.md](references/binding-patterns.md) for the correct pattern.

> ❌ **NEVER** use `XAMCORE_5_0` for new code. `XAMCORE_5_0` is only for fixing breaking API changes on existing types that shipped in prior releases.

> ❌ **NEVER** use `#pragma warning disable 0169` for struct fields. Instead, wrap public methods and properties inside `#if !COREBUILD` (but NOT fields — bgen needs to know the struct size).

> ⚠️ Place a space before parentheses and brackets: `Foo ()`, `Bar (1, 2)`, `myarray [0]`.

> ⚠️ Method names should follow .NET naming conventions — use verb-based names, not direct Objective-C selector translations (e.g., `BuildMenu` not `MenuWithContents`).

> ⚠️ For in depth binding patterns and conventions See [references/binding-patterns.md](references/binding-patterns.md)

> ⚠️ **Struct array parameters**: When an API takes a C struct pointer + count (e.g., `MyStruct*` + `NSUInteger`), bind the raw pointer as `[Internal]` with `IntPtr`, then create a manual public wrapper using the **factory pattern** with `fixed`. See [references/binding-patterns.md](references/binding-patterns.md) § "Struct Array Parameter Binding".

### Step 4b: Platform Exclusion Patterns for Manual Types

When a manually coded type (struct, extension, etc.) is not available on a specific platform (e.g., tvOS), you must handle compilation on that platform:

1. In the manual code file (`src/FrameworkName/MyStruct.cs`), wrap the struct body with `#if !TVOS`
2. Add `[UnsupportedOSPlatform ("tvos")]` on the struct
3. In the API definition file (`src/frameworkname.cs`), add a type alias at the top so compilation succeeds:

```csharp
#if TVOS
using MyStruct = Foundation.NSObject;
#endif
```

The `[NoTV]` attribute on the API definition interface ensures the type won't appear in the final tvOS assembly, while the alias prevents compilation errors from method signatures that reference the struct.

### Step 5: Build

```bash
make -C src build
```

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

```bash
make -C tests/xtro-sharpie run-ios
make -C tests/xtro-sharpie run-tvos
make -C tests/xtro-sharpie run-macos
make -C tests/xtro-sharpie run-maccatalyst
```

Verify all `.todo` entries for the bound framework are resolved. If any remain, they need binding or explicit `.ignore` entries with justification.

> ⚠️ **Delete empty `.todo` files** after resolving all entries: `git rm tests/xtro-sharpie/api-annotations-dotnet/{platform}-{Framework}.todo`. Do not leave empty `.todo` files in the repository.

#### 6b. Cecil Tests

```bash
make -C tests/cecil-tests run-tests
```

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

#### 6d. Monotouch Tests (only if you added tests in Step 5b)

Skip this step if no monotouch-test files were added or modified.

```bash
make -C tests/monotouch-test run
```

### Step 7: Handle Test Failures

If introspection tests fail for newly bound types:
- Check if the type crashes on simulator (common for hardware-dependent APIs)
- Add exclusions in the platform-specific `ApiCtorInitTest.cs` files if needed
- Types that crash on init, dispose, or toString need specific exclusion entries
- **NEVER skip an entire namespace** — always add exclusions for specific types only
- **If a `[DesignatedInitializer]` constructor crashes (segfault) when passed null**, the correct fix is to **remove `[NullAllowed]` from that parameter** rather than adding introspection test exclusions. The null is genuinely not allowed by the native API.

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
