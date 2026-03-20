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

> ❌ **NEVER** forget platform availability attributes. Every new API must have `[iOS]`, `[Mac]`, `[TV]`, `[MacCatalyst]`, and/or `[No*]` attributes matching the `.todo` file platforms where the API appears.

> ❌ **NEVER** use `string.Empty` — use `""`. Never use `Array.Empty<T>()` — use `[]`.

> ⚠️ Place a space before parentheses and brackets: `Foo ()`, `Bar (1, 2)`, `myarray [0]`.

> ⚠️ For in depth binding patterns and conventions See [references/binding-patterns.md](references/binding-patterns.md)

### Step 5: Build

```bash
make -C src build
```

Fix any compilation errors before proceeding. Builds can take up to 60 minutes — do not timeout early.

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

#### 6b. Cecil Tests

```bash
make -C tests/cecil-tests run-tests
```

#### 6c. Introspection Tests (All Platforms)

**IMPORTANT:** Clean shared obj directories before each platform to avoid NETSDK1005 errors:

```bash
# iOS
rm -rf tests/common/Touch.Unit/Touch.Client/dotnet/obj tests/common/MonoTouch.Dialog/obj
make -C tests/introspection/dotnet clean-ios build-ios run-ios

# tvOS
rm -rf tests/common/Touch.Unit/Touch.Client/dotnet/obj tests/common/MonoTouch.Dialog/obj
make -C tests/introspection/dotnet clean-tvos build-tvos run-tvos

# macOS (use run-bare for direct execution with captured output)
rm -rf tests/common/Touch.Unit/Touch.Client/dotnet/obj tests/common/MonoTouch.Dialog/obj
make -C tests/introspection/dotnet clean-macOS build-macOS run-bare-macOS

# MacCatalyst (use run-bare for direct execution with captured output)
rm -rf tests/common/Touch.Unit/Touch.Client/dotnet/obj tests/common/MonoTouch.Dialog/obj
make -C tests/introspection/dotnet clean-MacCatalyst build-MacCatalyst run-bare-MacCatalyst
```

> ⚠️ **macOS/MacCatalyst:** Use `make run-bare` (not `make run`) — `make run` launches the app without waiting or capturing stdout. `run-bare` runs the executable directly to capture test output.

> ⚠️ **iOS/tvOS:** Use `make run` (not `make run-bare`) — these require simulator infrastructure that `run-bare` doesn't provide.

Look for this pattern in test output to confirm results:
```
Tests run: X Passed: X Inconclusive: X Failed: X Ignored: X
```

### Step 7: Handle Test Failures

If introspection tests fail for newly bound types:
- Check if the type crashes on simulator (common for hardware-dependent APIs)
- Add exclusions in the platform-specific `ApiCtorInitTest.cs` files if needed
- Types that crash on init, dispose, or toString need specific exclusion entries

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
3. **Test results** — per-platform pass/fail for xtro, cecil, and introspection
4. **Remaining items** — any `.todo` entries intentionally left unbound, with reasons

## References

- **Binding patterns and conventions**: See [references/binding-patterns.md](references/binding-patterns.md)
- **Test commands and troubleshooting**: See [references/test-workflow.md](references/test-workflow.md)
