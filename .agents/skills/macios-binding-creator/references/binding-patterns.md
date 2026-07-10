# Binding Patterns

Detailed patterns for creating C# bindings in dotnet/macios. Derived from codebase conventions.

## File Organization

```
src/
├── frameworkname.cs              # API definitions (lowercase)
├── FrameworkName/                # Manual code
│   ├── *.cs                     # Partial classes, extensions
│   └── Enums.cs                 # Framework-specific enumerations
└── frameworks.sources            # Build configuration mapping
```

- **API definitions** (`src/frameworkname.cs`) — C# interfaces with `[Export]` attributes
- **Manual code** (`src/FrameworkName/*.cs`) — Partial classes, P/Invokes, helpers, complex conversions
- **Enums** — Smart enums backed by NSString constants or numeric enums

## Registering a Brand-New Framework

Most binding work adds APIs to an **existing** framework. Binding an **entirely new** Apple framework (no `src/<fw>.cs` yet) also requires wiring it into the build and test infrastructure — miss a step and the build or CI fails in a non-obvious way. Running precedent below: the **GameSave** framework (added for Xcode 26 in commit `8ece955d31`).

Checklist (apply to every platform the framework ships on):

1. **`src/<frameworkname>.cs`** — the API-definition file (lowercase), plus any manual code under `src/<FrameworkName>/`.
2. **`src/frameworks.sources`** — add the framework to each per-platform list it ships on: `IOS_FRAMEWORKS`, `MACOS_FRAMEWORKS`, `TVOS_FRAMEWORKS`, `MACCATALYST_FRAMEWORKS`. (GameSave is in the iOS/macOS/Mac Catalyst lists.)
3. **`tools/common/Frameworks.cs`** — add an entry to the matching `iOSFrameworks` / `MacFrameworks` / `TVOSFrameworks` / `MacCatalystFrameworks` dictionary so the framework is registered for linking. Format: `{ "ManagedName", "NativeName", major, minor }`. Precedent: `{ "GameSave", "GameSave", 26, 0 }`. Keep the dictionary **sorted** (there's an in-file reminder to that effect at `Frameworks.cs:509`).
4. **Remove the framework from xtro's ignore list** for the platform(s) you're now binding. `tests/xtro-sharpie/Makefile` has `IGNORED_<PLATFORM>_FRAMEWORKS` lists; a framework left there is **not checked** by xtro at all. Example: `AccessoryAccess` currently sits in `IGNORED_MACCATALYST_FRAMEWORKS`.
5. **`tests/dotnet/UnitTests/ProjectTest.cs`** — add the framework's native path to the `expectedFrameworks_<platform>_None` array(s). The `LinkedWithNativeLibraries` test builds an app with **LinkMode=None**, under which **every** bound SDK framework is force-linked, so a newly-registered framework appears and fails the exact-match assertion unless listed. Only the `_None` arrays need updating (the maintainer comment at `GetLinkedWithNativeLibrariesTestCases`, ~line 3750, explains why `_Full` should not). Mind the per-platform path forms:
   - iOS / tvOS: `/System/Library/Frameworks/GameSave.framework/GameSave`
   - macOS: `/System/Library/Frameworks/GameSave.framework/Versions/A/GameSave`
   - Mac Catalyst: `/System/iOSSupport/System/Library/Frameworks/GameSave.framework/Versions/A/GameSave`
6. **Delete the resolved xtro `.todo`** once the framework's entries are bound. **If** the new public members are undocumented, also update the cecil docs baseline (see [test-workflow.md](test-workflow.md) § Cecil) — that's the generic undocumented-member flow, not framework-specific (the GameSave commit didn't need it).

> ❌ **Two generated-file build gotchas after editing the framework lists — different files, different fixes, both easy to misdiagnose:**
> - **`src/build/dotnet/generator-frameworks.g.cs` is git-*tracked* and auto-regenerated.** Editing `src/frameworks.sources` makes the next `make all` regenerate it; the make rule (`src/Makefile.generator`) then runs `git diff` and **stops with `exit 1`** and *"please commit the changes."* The file is already regenerated on disk — so `git add` it and re-run `make all` (the GameSave commit committed this file). It is **not** a silent success: the first build after the edit fails until you commit it.
> - **`Constants.generated.cs` is *untracked* and goes stale after editing `tools/common/Frameworks.cs`.** The `generate-frameworks-constants` tool is **not** rebuilt just because you edited `Frameworks.cs` (that file is a *linked* compile item; the tool's make rule only depends on files under `scripts/generate-frameworks-constants/`). The stale tool regenerates a `Constants.generated.cs` lacking your new `<Framework>Library` constant, and the build fails `CS0117: 'Constants' does not contain a definition for '<Framework>Library'` from the generated `Libraries.g.cs`. **Fix:** force-rebuild the tool — delete its `bin`/`obj` and the stale `src/build/dotnet/*/Constants.generated.cs`, then rebuild.

### Types from Deliberately-Unbound Frameworks

macios intentionally does **not** bind a handful of low-level frameworks. Some are **ignored by xtro** — `DriverKit`, `IOUSBHost`, `Kerberos` sit in `IGNORED_*_FRAMEWORKS` (`tests/xtro-sharpie/Makefile`) so their absence isn't even reported. Others are **registered for linking only** in `tools/common/Frameworks.cs` (e.g. `IOBluetooth` at line 164) with no `src/*.cs` API surface. The IOKit family as a whole is out of scope.

When a framework you **are** binding hands back a type that belongs to one of these unbound frameworks (an `IOUSBHostDevice *`, an `xpc_object_t`, …), you can't "properly" bind it — use the established fallbacks:
- **Unbound Objective-C class** → alias it to `Foundation.NSObject` with a `using` at the top of the API-definition file (a short comment naming the owning framework helps reviewers). Precedent: `src/browserenginekit.cs:47` aliases the XPC `xpc_object_t` type this way — `using OS_xpc_object = Foundation.NSObject;` — for its `xpc_object_t` parameters/returns. (This is *different* from platform-stubbing an otherwise-bound type under `#if` — for that, see "Platform Exclusion for Manual Types" below.)
- **C primitive** (`kern_return_t`, `IOReturn`, `io_service_t`, …) → map to a plain integer / `IntPtr`, never a bound type. Verified precedent: `kern_return_t` → `int` in `src/IOSurface/IOSurface.cs` (`// kern_return_t` → `public int Lock (...)`). For other C typedefs, confirm the width/signedness from the header before choosing `int`/`uint`/`nint`/`IntPtr`.

> ⚠️ Reviewers may ask "why not bind this type?" — the answer is that the owning framework is deliberately out of macios's scope (IOKit-family), so the NSObject-alias / integer fallback is the intended convention, not a shortcut.

## Platform Availability Attributes

Every bound API must declare platform availability:

```csharp
// Available on all platforms from specific versions
[iOS (18, 0), TV (18, 0), Mac (15, 0), MacCatalyst (18, 0)]

// Not available on specific platforms
[NoTV, NoMac, iOS (18, 0), MacCatalyst (18, 0)]

// Changing availability (e.g., API added to tvOS in a later Xcode)
[TV (26, 4)]  // was previously [NoTV]
```

When an API appears in `.todo` files for some platforms but not others, use `[No*]` attributes for the missing platforms.

### Common Availability Patterns

```csharp
// Mobile-only (iOS, tvOS)
[NoMac, NoMacCatalyst]

// Desktop-only (macOS)
[NoiOS, NoTV, NoMacCatalyst]

// Phone/tablet only (iOS, Mac Catalyst)
[NoTV, NoMac]

// Introduced at different times per platform
[iOS (14, 0), Mac (11, 0), TV (15, 0), MacCatalyst (14, 5)]
```

### Deprecation and Obsolescence

```csharp
// Deprecated — still available but discouraged
[Deprecated (PlatformName.iOS, 15, 0, message: "Use 'NewMethod' instead.")]
[Export ("oldMethod")]
void OldMethod ();

// Obsoleted — no longer available (compile error)
[Obsoleted (PlatformName.iOS, 16, 0, message: "Use 'ModernMethod' instead.")]
[Export ("veryOldMethod")]
void VeryOldMethod ();
```

> ❌ **Deprecate on EVERY platform where the member is still *available* — not on `[No*]`/unsupported platforms.** In macios a source `[Deprecated]` compiles to `[ObsoletedOSPlatform]` (bgen `Attributes.cs`), and the cecil test `FindMissingObsoleteAttributes` (`tests/cecil-tests/ApiAvailabilityTest.cs`) fails an API that is deprecated on **some** platforms but still **supported** (neither deprecated nor unavailable) on **another** — because bgen does **not** auto-propagate an iOS `[Deprecated]` to Mac Catalyst. So a member available on iOS **and** Mac Catalyst needs **both** `[Deprecated (PlatformName.iOS, …)]` **and** `[Deprecated (PlatformName.MacCatalyst, …)]`; a `[NoMacCatalyst]` member needs **no** Catalyst deprecation. Use the same message on each for self-consistency (good practice — the test collects but does **not** assert message equality). xtro is per-platform and won't catch a missing pair. Real precedent: `AVExternalStorageDevice.NotRecommendedForCaptureUse` (`src/avfoundation.cs:26071-26074`) carries paired `[Deprecated]` for **all four** platforms (iOS + TvOS + MacOSX + MacCatalyst), identical message.

### Best Practices

- Always check Apple's documentation for platform availability
- Use `[NoTV]` over `[Unavailable (PlatformName.TvOS)]`
- Include deprecation messages to guide developers
- Consider Mac Catalyst separately from iOS — availability may differ

## Basic Class Binding

```csharp
[BaseType (typeof (NSObject))]
interface MyClass {
    [Export ("name")]
    string Name { get; set; }

    [Export ("doSomething:")]
    void DoSomething (string parameter);

    [Static]
    [Export ("sharedInstance")]
    MyClass SharedInstance { get; }
}
```

## Property Bindings

```csharp
// Read-write property
[Export ("propertyName")]
Type PropertyName { get; set; }

// Read-only property
[Export ("propertyName")]
Type PropertyName { get; }

// Nullable property
[NullAllowed]
[Export ("propertyName")]
Type PropertyName { get; set; }

// Property with specific semantics
[Export ("delegate", ArgumentSemantic.Weak)]
[NullAllowed]
NSObject WeakDelegate { get; set; }
```

## Method Bindings

```csharp
// Simple method
[Export ("doSomething")]
void DoSomething ();

// Method with parameters
[Export ("setTitle:forState:")]
void SetTitle ([NullAllowed] string title, UIControlState state);

// Method returning a value
[Export ("titleForState:")]
[return: NullAllowed]
string GetTitle (UIControlState state);

// Static method
[Static]
[Export ("captionPreviewForCaptionProfile:")]
[return: NullAllowed]
AVCaptionPreview GetCaptionPreview (string profileId);
```

## Enum Bindings

```csharp
// Smart enum backed by NSString fields
[Native]
public enum AVPlayerRateDidChangeReason : long {
    [Field ("AVPlayerRateDidChangeReasonSetRateCalled")]
    SetRateCalled = 0,
    [Field ("AVPlayerRateDidChangeReasonPlayheadReachedLiveEdge")]
    PlayheadReachedLiveEdge,
}

// Numeric enum
[Native]
public enum SomeEnum : long {
    Value1 = 0,
    Value2,
}

// NSString-backed smart enum with BindAs
[BindAs (typeof (MyOption))]
[Export ("selectedOption")]
NSString SelectedOption { get; set; }
```

### Adding New Members to Existing Enums

When adding a new value to an existing enum, match the native header **per member**. Add a per-member availability attribute **only if** the header annotates that member differently from the enum — its own `API_AVAILABLE` version (newer than the enum), or `API_UNAVAILABLE`/absence on a platform (→ `[No<Platform>]`). Use the version the **member** was introduced, not the enum's. A member the header doesn't annotate inherits the enum's availability — leave it bare, matching its siblings:

```csharp
[NoTV, NoMacCatalyst, NoMac, iOS (26, 1)]
[Native]
public enum PHAssetResourceUploadJobAction : long {
    Acknowledge = 1,
    Retry = 2,
    [iOS (26, 5)]
    Process = 3,
}
```

Check the generated reference bindings (`make -C tests/xtro-sharpie gen-all`) for the correct per-member introduction version. (The example above is a single-platform, iOS-only enum, so the new member needs only `[iOS (26, 5)]`.)

> ❌ **Error enums are the exception — never add availability *or* unavailability attributes to their members.** If the enum's name ends in `Error`/`ErrorCode` or it carries `[ErrorDomain]`, cecil's `EnumTest.NoAvailabilityOnError` (issue #9724) fails — with **no** known-failures allowlist — on **any** field carrying `[iOS]`/`[Mac]`/`[TV]`/`[No*]`/`[Introduced]`/`[Unavailable]`/`[Supported/UnsupportedOSPlatform]` (only `[Obsolete]`/`[Obsoleted…]` are exempt). Bind new error-code values bare, matching their siblings (e.g. `VNErrorCode.ResourceUnavailable`, `UNErrorCode.AttachmentUnsupportedType`).

> ⚠️ **Multi-platform enums (numeric *or* smart): give a new member every applicable platform's version, not just one.** For any enum available on more than one platform, bgen back-fills each platform where the member has no introduced attribute with the parent enum's version (`AttributeFactory.FindHighestIntroducedAttributes`, `Generator.PrintAttributes.cs`). So `[iOS (27, 0)]` alone on a member of an enum introduced at 26.0 silently leaves the member reporting `Mac`/`MacCatalyst` **26.0**. Specify all applicable `[iOS]`/`[Mac]`/`[MacCatalyst]`/`[TV]` versions (repeating a parent `[NoTV]` is redundant but harmless). A single-platform enum needs only that platform's version, as in the example above.

## Notification Fields

```csharp
// Simple notification
[Notification]
[Field ("MYClassDidChangeNotification")]
NSString DidChangeNotification { get; }

// Notification with event args
[Notification (typeof (MyClassEventArgs))]
[Field ("MYClassDidUpdateNotification")]
NSString DidUpdateNotification { get; }
```

## Delegate / Protocol Binding

```csharp
// Empty stub interface definition required for intermediate assembly to compile
// Follows the name of the interface with the [Protocol, Model] and [BaseType] 
// adding an 'I' to the interface name so it can be used inside the Weak Delegate Pattern members
interface IMyDelegate { }

// Protocol definition
[Protocol, Model]
[BaseType (typeof (NSObject))]
interface MyDelegate {
    // Required method
    [Abstract]
    [Export ("requiredMethod:")]
    void RequiredMethod (MyClass sender);

    // Optional method (no [Abstract])
    [Export ("optionalMethod:")]
    void OptionalMethod (MyClass sender);
}
```

> ❌ **New protocols** must set `BackwardsCompatibleCodeGeneration = false`. The cecil test `MustSetBackwardsCompatibleCodeGenerationToFalse` enforces this. Do NOT add it to existing protocols unless you're intentionally changing their code generation.

```csharp
// New protocol — must have BackwardsCompatibleCodeGeneration = false
[Protocol (BackwardsCompatibleCodeGeneration = false), Model]
[BaseType (typeof (NSObject))]
[iOS (26, 4), TV (26, 4), Mac (26, 4), MacCatalyst (26, 4)]
interface MyNewDelegate {
    [Abstract]
    [Export ("didFinish:")]
    void DidFinish (MyClass sender);
}
```

### Adding Protocol Conformance to Existing Types

When a `.todo` entry says `!missing-protocol-conformance!`, add the protocol to the existing type's interface declaration. Use the **plain protocol name** (no `I` prefix) in the conformance list:

```csharp
// Before: MPNowPlayingSession without protocol conformance
[BaseType (typeof (NSObject))]
interface MPNowPlayingSession {
    // existing members...
}

// After: Add protocol conformance
[BaseType (typeof (NSObject))]
interface MPNowPlayingSession : MyPlayableItem {  // <-- plain name, NO I prefix
    // existing members unchanged
}
```

> ❌ **NEVER** use the `I`-prefixed name in protocol definitions or protocol conformance declarations. The `I` prefix is ONLY used when referencing a protocol as a **type** in method parameters, return types, and properties (e.g., `INSCopying Identifier { get; }`, `void Foo (INSCoding item)`). Protocol definitions use plain names (`[Protocol, Model] interface MyDelegate`), and protocol conformance uses plain names (`interface MyClass : MyProtocol`).

> ⚠️ **Only add conformance the *header* declares.** `!missing-protocol-conformance!` comes from xtro, which is header-driven, so acting on it is safe. But if a type conforms to a protocol only at **runtime** (common on new Xcode betas) while the header doesn't declare it, do NOT add the conformance here — that would be a wrong binding (and xtro never asked for it). Handle it with a test-only introspection Skip instead — see [test-workflow.md](test-workflow.md) → "Runtime-Only Protocol Conformance".

> ⚠️ **Don't redeclare protocol-inherited properties.** When a type conforms to a protocol, it inherits the protocol's properties. If the type already has those properties bound (e.g., `title`, `artist`), do NOT redeclare them or you'll get CS0108 (member hides inherited member) warnings. Remove the duplicates from the conforming type.

> ⚠️ **Don't bind `initWithCoder:` on `NSCoding`/`NSSecureCoding` types.** bgen auto-generates the `Constructor (NSCoder)` from an `NSCoding`/`NSSecureCoding` conformance. Adding an explicit `[Export ("initWithCoder:")]` constructor triggers a **CS0108** "member hides inherited member" warning — the explicit constructor hides the one the conformance already provides — which fails the build under bgen's `-warnaserror`. Let the conformance generate it — e.g. `NSTextList : NSCoding, NSSecureCoding` binds no `initWithCoder:`.

### Weak Delegate Pattern

Always use this pattern for delegate properties:

```csharp
[BaseType (typeof (NSObject))]
interface MyClass {
    [Export ("delegate", ArgumentSemantic.Weak)]
    [NullAllowed]
    NSObject WeakDelegate { get; set; }

    [Wrap ("WeakDelegate")]
    [NullAllowed]
    IMyDelegate Delegate { get; set; }
}
```

## Blocks and Completion Handlers

```csharp
// Define the delegate type
delegate void CompletionHandler (bool success, [NullAllowed] NSError error);

// Use in method binding
[Export ("performTaskWithCompletion:")]
void PerformTask ([NullAllowed] CompletionHandler completion);

// Block returning a value
delegate bool ValidationHandler (string input);

[Export ("validateWithHandler:")]
bool Validate (ValidationHandler handler);
```

> ❌ **NEVER** use `Action<T>` or `Func<T>` for completion handler parameters. Always define a **named delegate type** (e.g., `delegate void MyHandler (...)`) — this produces better API documentation and IntelliSense. Note: xtro-sharpie may generate `Action`/`Func` delegates; always convert them to named delegates in your binding.

> ⚠️ **Use `string`, not `NSString`**, for string parameters in delegates, methods, and properties. The binding generator marshals between `string` and `NSString` automatically. Use `NSString` only when the parameter is specifically a dictionary key, a strong-typed constant, or part of an `NSDictionary<NSString, ...>` signature.

## Async/Await Support

```csharp
// Simple async — generates Task<NSData> LoadDataAsync ()
delegate void LoadCompletionHandler ([NullAllowed] NSData data, [NullAllowed] NSError error);

[Export ("loadDataWithCompletion:")]
[Async]
void LoadData (LoadCompletionHandler completion);

// Custom result type — generates Task<FetchResult> FetchValuesAsync () 
delegate void FetchValuesCompletionHandler (string value, nint count, [NullAllowed] NSError error);

[Export ("fetchMultipleValues:")]
[Async (ResultTypeName = "FetchResult")]
void FetchValues (FetchValuesCompletionHandler completion);
```

> ⚠️ Always prefer the delegate pattern over blocks for async. Use `[Async]` to generate `Task`-based wrappers.

## Categories (Objective-C Extensions)

```csharp
[Category]
[BaseType (typeof (UIView))]
interface UIView_MyExtensions {
    [Export ("makeRounded")]
    void MakeRounded ();
}
```

## C-Style API Binding

For C functions and structs, create manual bindings in `src/FrameworkName/`:

```csharp
// C Function (P/Invoke)
[DllImport (Constants.CoreGraphicsLibrary)]
public static extern void CGContextFillRect (IntPtr context, CGRect rect);

// C Struct — use byte backing fields for bools to keep struct blittable
[StructLayout (LayoutKind.Sequential)]
public struct MyStruct {
	byte enabled;
	nfloat x;
	nfloat y;

#if !COREBUILD
	public bool Enabled {
		get => enabled != 0;
		set => enabled = value ? (byte) 1 : (byte) 0;
	}

	public nfloat X { get => x; set => x = value; }
	public nfloat Y { get => y; set => y = value; }
#endif
}

// Global constant
[Field ("kMyConstant", "MyFramework")]
public static NSString MyConstant { get; }
```

### C Callback Handler Binding

When a C API sets a persistent callback handler (e.g., state change handlers, data providers), use the **BlockLiteral trampoline** pattern for Objective-C blocks or **GCHandle** for C function pointer contexts. Follow these patterns from `src/Network/` and `src/CoreFoundation/`.

#### BlockLiteral Trampoline (preferred for ObjC block callbacks)

This is the standard pattern used in Network framework (`NWConnection`, `NWBrowser`, `NWListener`):

```csharp
[DllImport (Constants.NetworkLibrary)]
static extern void nw_connection_set_state_changed_handler (
	IntPtr handle, /* BlockLiteral* */ IntPtr handler);

[UnmanagedCallersOnly]
static void TrampolineStateChanged (IntPtr block, int state, IntPtr error)
{
	var del = BlockLiteral.GetTarget<Action<NWConnectionState, NWError?>> (block);
	if (del is not null)
		del (/* marshal args */);
}

public void SetStateChangedHandler (Action<NWConnectionState, NWError?> handler)
{
	if (handler is null) {
		nw_connection_set_state_changed_handler (GetCheckedHandle (), IntPtr.Zero);
		return;
	}

	unsafe {
		// The function-pointer target MUST be a named static method with
		// [UnmanagedCallersOnly] (see TrampolineStateChanged above) — a lambda
		// cannot carry [UnmanagedCallersOnly], so it can't be used here.
		delegate* unmanaged<IntPtr, int, IntPtr, void> trampoline = &TrampolineStateChanged;
		using var block = new BlockLiteral (trampoline, handler, typeof (MyClass),
			nameof (TrampolineStateChanged));
		nw_connection_set_state_changed_handler (GetCheckedHandle (), (IntPtr) (&block));
	}
}
```

#### GCHandle Context (for C function pointer + void* context)

Used in CoreFoundation (`CFStream.cs`) when the native API takes a function pointer + context:

```csharp
GCHandle gch;

void EnableEvents ()
{
	if (!gch.IsAllocated)
		gch = GCHandle.Alloc (this);

	var ctx = new CFStreamClientContext {
		Info = GCHandle.ToIntPtr (gch)
	};
	DoSetClient (&NativeCallback, ref ctx);
}

[UnmanagedCallersOnly]
static void NativeCallback (IntPtr stream, int eventType, IntPtr info)
{
	var instance = GCHandle.FromIntPtr (info).Target as MyClass;
	instance?.OnCallback (eventType);
}
```

#### Key Rules for Callback Handlers

> ❌ **NEVER** access a nullable parameter (e.g., `DispatchQueue? queue`) without null-checking it first. Note: `.GetHandle()` is safe on a `null` instance (it returns `NativeHandle.Zero`), but other member accesses on nullable parameters still require null checks. Check all nullable parameters before use on every code path.

> ⚠️ **Memory management ordering**: When replacing a stored handler, set the new managed reference **before** freeing the old `GCHandle`. This prevents premature collection if GC runs between the free and the assignment.

> ⚠️ **GC.KeepAlive**: Call `GC.KeepAlive (queue)` or `GC.KeepAlive (handler)` after passing native handles to P/Invokes. This prevents the GC from collecting the managed object while the native call is still using its handle.

Real examples: `src/Network/NWConnection.cs`, `src/Network/NWBrowser.cs`, `src/CoreFoundation/CFStream.cs`, `src/Security/SecTrust.cs`

### Struct Binding Rules

- **Only use blittable types as backing fields in structs.** `bool` and `char` aren't blittable — use `byte` and `ushort`/`short` instead. This avoids `[MarshalAs]` and cecil test known failures.
- **Wrap all public methods and properties in `#if !COREBUILD`** — never use `#pragma warning disable 0169`. Do NOT wrap fields, because bgen may do different things depending on the size of a struct, so it needs to know the final size.
- **NEVER use `XAMCORE_5_0` for new code.** `XAMCORE_5_0` is only for fixing breaking API changes on existing types that shipped in prior releases.
- **Don't use arrays** — they're not blittable. Add the corresponding number of individual fields instead (`byte b1; byte b2; …`).
- **Don't use explicit layout** (`[StructLayout (LayoutKind.Explicit)]`). Use opaque backing fields instead (`byte b1; byte b2; …`) with properties that read/write to the opaque backing fields.
- If a struct member is platform-specific, use `#if !__TVOS__` (or similar) to exclude it.

### Platform Exclusion for Manual Types

When a manual type (struct, helper class) is not available on tvOS:

```csharp
// In src/FrameworkName/MyStruct.cs:
#if !__TVOS__
[UnsupportedOSPlatform ("tvos")]
[StructLayout (LayoutKind.Sequential)]
public struct MyStruct {
	byte enabled;

#if !COREBUILD
	public bool Enabled {
		get => enabled != 0;
		set => enabled = value ? (byte) 1 : (byte) 0;
	}
#endif // !COREBUILD
}
#endif // !__TVOS__

// In src/frameworkname.cs (at the top of the file):
#if __TVOS__
using MyStruct = Foundation.NSObject;
#endif
```

The type alias lets tvOS compilation succeed. The `[NoTV]` attribute on the API definition interface ensures the type won't appear in the final tvOS assembly.

### Platform-Specific Code Within Shared Files

Use preprocessor directives for platform-conditional code — **not** platform-specific source file lists in `frameworks.sources`:

```csharp
// macOS-only type or member
#if __MACOS__
public class ARCollaborationData : NSObject {
	// macOS-only implementation
}
#endif

// iOS-specific behavior
#if __IOS__
	[DllImport (Constants.ARKitLibrary)]
	static extern void ar_session_run (IntPtr session, IntPtr config);
#endif
```

Available preprocessor symbols:
| Symbol | Platform |
|--------|----------|
| `__MACOS__` (preferred) / `MONOMAC` | macOS |
| `__IOS__` | iOS |
| `__TVOS__` (preferred) / `TVOS` | tvOS |
| `__MACCATALYST__` | Mac Catalyst |

> ❌ **NEVER** use platform-specific source file entries (e.g., appending a per-framework list to `MACOS_DOTNET_SOURCES`) for conditional compilation. Use `#if` directives instead — they keep the code in shared files and are the established convention across the codebase.

### Shared AppKit/UIKit Types (`src/xkit.cs`)

Some Foundation/TextKit types (Apple's `UIFoundation`) are exposed to **both** AppKit (macOS) and UIKit (iOS/tvOS/Mac Catalyst). Bind these **once** in `src/xkit.cs` — the shared API-definition file compiled into *both* assemblies — never as duplicate copies in `appkit.cs` and `uikit.cs`.

`xkit.cs` is compiled twice: `#if MONOMAC namespace AppKit` else `namespace UIKit`. `#if !MONOMAC` aliases near the top (`using NSColor = UIKit.UIColor;`, `using NSView = System.Object;`, …) let AppKit type names compile on the UIKit side. `NSTextList` is a bound example (`interface NSTextList : NSCoding, NSCopying, NSSecureCoding`). (The file's namespace switch and top-of-file aliases use the legacy `MONOMAC` symbol; `__MACOS__` and `MONOMAC` are both defined for the macOS build. Prefer `[No*]` attributes over `#if` here — see below — but on the rare divergence that genuinely needs `#if`, prefer `#if __MACOS__` over `#if MONOMAC`.)

**When to consolidate:** when a type currently in `appkit.cs` becomes exposed to UIKit too (common on a new Xcode, usually **back-dated** to its original macOS availability), and the same Objective-C type maps to one shared binding, move it into `xkit.cs` rather than adding a second copy to `uikit.cs`. The reverse applies too — when a type currently in `uikit.cs` becomes exposed to AppKit, move it into `xkit.cs` (not a second copy in `appkit.cs`). Precedent: `NSTextList` was moved `appkit.cs → xkit.cs` in commit `8cecb962a4` when it became shared in UIKit.

Steps:
1. Add the interface to `xkit.cs` (near related types, e.g. after `NSTextList`), preserving the **exact member order, attributes, and parameter names** from the old `appkit.cs` copy.
2. Remove the type from `appkit.cs` **and** from `uikit.cs`.
3. Remove any `#if !MONOMAC` `using <Type> = System.Object;` dummy alias for that type near the top of `xkit.cs` — it's now a real shared type.

**Handle each divergence with the narrowest tool — prefer `[No*]` attributes over `#if`.** rolfbjarne's guidance: use the `[No*]` platform attributes and avoid `#if __MACOS__`/`#if MONOMAC` whenever possible.
- **A member that exists on only one platform** → put `[NoiOS, NoTV, NoMacCatalyst]` (macOS-only) or `[NoMac]` (mobile-only) on that member — **not** `#if`. Even when the signature references a type that exists only on the other platform (e.g. `NSView`), the top-of-file dummy aliases (`using NSView = System.Object;`) let it compile on the UIKit side, so the attribute alone is enough. Real example: `NSTextBlock`'s macOS-only members in `src/xkit.cs` (e.g. `setWidth:type:forLayer:edge:`, `drawBackgroundWithFrame:inView:characterRange:layoutManager:`) carry `[NoiOS, NoTV, NoMacCatalyst]`, no `#if`.
- **`#if` is only for divergences an attribute can't express** — the *same* member with a different `[Export]`/`ArgumentSemantic`/`[NullAllowed]` per platform (real: `NSShadow.ShadowColor` uses `#if MONOMAC` because macOS is `Copy`+non-null while iOS is `Retain`+`[NullAllowed]`), a different managed **name** (real: `NSTextTable` `Columns` vs `NumberOfColumns` under `#if MONOMAC && !XAMCORE_5_0`, unified to `NumberOfColumns` in `XAMCORE_5_0`), or a differing base **protocol-conformance list**.

**Enums: share when identical, split when they diverge.** If the AppKit and UIKit native enum declarations are identical, bind the enum **once** in `xkit.cs` (e.g. `NSTextListOptions`, `NSTextListMarkerFormats` live there). **Split** it into `AppKit/Enums.cs` and `UIKit/UIEnums.cs` only when the platforms diverge — most often when a long-shipped AppKit enum is ABI-frozen as `ulong` while the new UIKit enum should be the correct `long`. When split, the shared interface references the enum by **simple name**, which resolves to `AppKit.<Enum>` on macOS and `UIKit.<Enum>` otherwise. Do **not** unify a divergent pair with `#if XAMCORE_5_0 long #else ulong` (that would wrongly narrow the new UIKit enum). xtro flags only `!wrong-enum-size!` (size mismatch), never signedness, so `long` vs `ulong` both pass at the same 8-byte size.

**Availability for back-dated shared types.** Use the header-derived original availability, not the new-Xcode version — a UIFoundation type newly *exposed* to UIKit in iOS 27 was actually introduced at iOS 6, and stamping it `27` fails xtro/cecil. Follow the `NSTextList` shape: implicit type-level availability + a single `[MacCatalyst (13, 0)]` attribute (no explicit `[iOS]`/`[TV]`/`[Mac]`). Only genuinely new members carry the new version.

> ⚠️ **Verify no macOS ABI break when moving a type.** Moving a type between binding files must not change the generated macOS binding. Compare the regenerated `src/build/dotnet/macos/generated-sources/AppKit/<Type>.g.cs` against a copy saved **before** the move — it must be **byte-identical**. If it differs, adjust the shared `xkit.cs` interface (member order, `[No*]` attributes, and any `#if MONOMAC` divergence guards) to match the old `appkit.cs` exactly — member order matters. There is no apidiff make target for `.g.cs`, so this baseline diff is the safety net.

### Frameworks with Mixed API Surfaces (ObjC + C)

Occasionally a framework exposes Objective-C APIs on the mobile platforms (iOS, tvOS, Mac Catalyst) but only a C-level API on macOS. When you hit this, keep a single framework and guard by platform rather than splitting source lists (`SomeFramework` below is a stand-in — substitute the real framework name):

**The pattern:**

1. Add the framework to `MACOS_FRAMEWORKS` in `src/frameworks.sources` — this tells the build system to compile it for macOS.
2. Guard the **entire bgen file** (`src/someframework.cs`) with `#if !__MACOS__` / `#endif` — the ObjC API definitions have UIKit/AVFoundation dependencies that won't compile on macOS.
3. Put the macOS-specific C API bindings in `src/SomeFramework/*.cs` guarded with `#if __MACOS__`.
4. Keep everything in one `SOMEFRAMEWORK_SOURCES` list — no split source lists needed.

```
# In src/frameworks.sources — single unified list:
SOMEFRAMEWORK_SOURCES =              \
	SomeFramework/SomeObject.cs     \
	SomeFramework/SomeSession.cs    \
	SomeFramework/SomeEnums.cs      \
```

```csharp
// src/someframework.cs — entire ObjC bgen file guarded for non-macOS
#if !__MACOS__
using System;
using Foundation;
using UIKit;
// ... all ObjC API definitions ...
#endif // !__MACOS__
```

```csharp
// src/SomeFramework/SomeSession.cs — macOS C API manual binding
#if __MACOS__
[SupportedOSPlatform ("macos")]
public class SomeSession : SomeObject {
	// C API P/Invokes and wrappers
}
#endif // __MACOS__
```

> ⚠️ Confirm the framework really has this split (ObjC-only on mobile, C-only on macOS) before applying this. Most frameworks instead ship a single shared bgen file and use per-type `[NoMac]`/`[NoiOS]` attributes to exclude platforms; the whole-file `#if !__MACOS__` guard is only for the genuine ObjC-vs-C-surface case.

> ❌ **NEVER** create separate source file lists (e.g., `SOMEFRAMEWORK_C_API_SOURCES`) and append them with `MACOS_DOTNET_SOURCES += $(SOMEFRAMEWORK_C_API_SOURCES)`. This creates a maintenance burden. Add the framework to `MACOS_FRAMEWORKS` and use `#if` guards instead.

## Struct Array Parameter Binding

When an Objective-C API takes a C struct pointer + count (e.g., `MyStruct*` + `NSUInteger`), create a manual public wrapper that marshals a managed array to/from the native pointer. This is a common Apple API pattern (MapKit, CarPlay, ARKit, etc.).

### Recognition

You need this pattern when:
- A constructor or method takes `T*` + `NSUInteger count` (struct array input)
- A property returns `T*` with a separate `count` property (struct array output)
- The generated reference binding shows `IntPtr` where you'd expect a struct array

### API Definition (`src/frameworkname.cs`)

Mark struct pointer APIs as `[Internal]` so they're not exposed publicly:

```csharp
[BaseType (typeof (NSObject))]
[NoTV, NoMac, iOS (26, 4), MacCatalyst (26, 4)]
interface MyClass {
	// Static factory — [Internal] + IntPtr
	[Static]
	[Internal]
	[Export ("classWithCoordinates:count:")]
	MyClass _Create (IntPtr coords, nint count);

	// Constructor — [Internal] + IntPtr
	[Internal]
	[Export ("initWithPoints:count:")]
	NativeHandle Constructor (IntPtr points, nuint count);

	// Property getter — [Internal] + IntPtr
	[Internal]
	[Export ("points")]
	IntPtr _Points { get; }

	[Export ("pointCount")]
	nuint PointCount { get; }
}
```

### Manual Wrappers (`src/FrameworkName/MyClass.cs`)

> ⚠️ Always use the **factory pattern** (static `Create` method) instead of a public constructor for struct array parameters. This avoids issues with `fixed` in constructor chains.
>
> ⚠️ Manual code should also have **XML documentation comments** (`<summary>`, `<param>`, `<returns>`, etc.).

#### Factory for Static Methods

When the API definition has a `[Static] [Internal]` method:

```csharp
#nullable enable

namespace FrameworkName {

	public partial class MyClass {

		[SupportedOSPlatform ("ios26.4")]
		[SupportedOSPlatform ("maccatalyst26.4")]
		/// <summary>Creates a new <see cref="MyClass" /> from the specified coordinates.</summary>
		/// <param name="coords">The array of coordinates.</param>
		/// <returns>A new <see cref="MyClass" /> instance.</returns>
		public static unsafe MyClass Create (MyStruct [] coords)
		{
			if (coords is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (coords));

			fixed (MyStruct* first = coords) {
				return _Create ((IntPtr) first, coords.Length);
			}
		}
	}
}
```

Real examples: `src/MapKit/MKPolyline.cs`, `src/MapKit/MKPolygon.cs`

#### Factory for Constructors

When the API definition has an `[Internal]` `Constructor`:

```csharp
		[SupportedOSPlatform ("ios26.4")]
		[SupportedOSPlatform ("maccatalyst26.4")]
		/// <summary>Creates a new <see cref="MyClass" /> from the specified points.</summary>
		/// <param name="points">The array of points.</param>
		/// <returns>A new <see cref="MyClass" /> instance.</returns>
		public static unsafe MyClass Create (MyStruct [] points)
		{
			if (points is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (points));

			fixed (MyStruct* first = points) {
				return new MyClass ((IntPtr) first, (nuint) points.Length);
			}
		}
```

If the API definition uses `_InitWith*` methods instead of `Constructor`, use `NSObjectFlag.Empty` + `InitializeHandle`:

```csharp
		public static unsafe MyClass Create (MyStruct [] points)
		{
			// ... null/empty checks ...
			var instance = new MyClass (NSObjectFlag.Empty);
			fixed (MyStruct* first = points) {
				instance.InitializeHandle (
					instance._InitWithPoints ((IntPtr) first, (nuint) points.Length), "initWithPoints:length:");
			}
			return instance;
		}
```

#### Property Getter for Struct Arrays

When the API has an `[Internal]` `IntPtr` property + a count property:

```csharp
		[SupportedOSPlatform ("ios26.4")]
		[SupportedOSPlatform ("maccatalyst26.4")]
		/// <summary>Gets the array of points.</summary>
		public unsafe MyStruct [] Points {
			get {
				var count = (int) PointCount;
				var source = (MyStruct*) _Points;
				if (source == null)
					return [];
				var result = new MyStruct [count];
				for (int i = 0; i < count; i++)
					result [i] = source [i];
				return result;
			}
		}
```

Real example: `src/MapKit/MKMultiPoint.cs`

### frameworks.sources

Add the manual file to the framework's `*_SOURCES`. If the file defines types needed by the API definition (like structs), add it to both `*_API_SOURCES` and `*_SOURCES`.

If the file defines types that bgen needs to resolve (e.g., NativeObject subclasses used as marshal types — see "NativeObject Return Types in Protocol Methods" below), add it to `*_CORE_SOURCES`:

```
# In src/frameworks.sources:
PRINTCORE_CORE_SOURCES =                    \
	PrintCore/Defs.cs                       \
	PrintCore/PrintCore.cs                  \
```

`*_CORE_SOURCES` files are compiled into bgen's core assembly, making their types available for type resolution during code generation. Use this when bgen reports `BI1078: Do not know how to make a signature for <Type>`.

## NativeObject Return Types in Protocol Methods

When a protocol method returns an opaque C type (e.g., `PMPrintSession`, `PMPageFormat`) that has a managed `NativeObject` wrapper, bgen doesn't know how to marshal it by default. Bgen only handles `NSObject` subclasses, primitives, and `IntPtr` in protocol return positions.

Types like `CGColor`, `CGImage`, and `CMSampleBuffer` work because they're explicitly registered as **marshal types** in bgen. The default `MarshalType` generates `Runtime.GetINativeObject<T>(ptr, owns)` marshaling — exactly what NativeObject wrappers need.

### Recognition

You need this pattern when:
- A protocol method returns an opaque C type that has a managed `NativeObject` wrapper
- bgen reports `BI1078: Do not know how to make a signature for <Type>`
- You've been using `IntPtr` as the return type but a concrete managed type exists

### Step 1: Add to CORE_SOURCES

The type's manual code file must be in `FRAMEWORKNAME_CORE_SOURCES` in `src/frameworks.sources` so bgen's core assembly can resolve it. See the `frameworks.sources` section above.

### Step 2: Add #if !COREBUILD guards

In the manual code file (e.g., `src/PrintCore/PrintCore.cs`), wrap the class **body** in `#if !COREBUILD` but keep the class shell visible. This is the same pattern used in `src/StoreKit/StoreProductParameters.cs`:

```csharp
#nullable enable

using System;
using System.Runtime.InteropServices;
using ObjCRuntime;

namespace PrintCore {

	public class PMPrintSession : PMPrintCoreBase {
		// The base type (PMPrintCoreBase) has no parameterless constructor, so this
		// base-chaining ctor MUST stay OUTSIDE #if !COREBUILD — otherwise the class
		// has no constructor in the core build and fails to compile.
		[Preserve (Conditional = true)]
		internal PMPrintSession (NativeHandle handle, bool owns)
			: base (handle, owns)
		{
		}

#if !COREBUILD
		// ... P/Invokes, properties, methods ...
#endif // !COREBUILD
	}
}
```

The class shell (name, inheritance, and the base-chaining `(NativeHandle, bool)` constructor when the base lacks a default ctor) stays visible to bgen's core assembly for type resolution. The implementation body (P/Invokes, properties, methods) is excluded from the core build since bgen only needs the type identity, not the implementation.

> ⚠️ **Use `#if !COREBUILD`, not separate files.** Don't split the type into a shell file and an implementation file. The `#if !COREBUILD` pattern keeps everything in one file and is the established convention (see `src/StoreKit/StoreProductParameters.cs`, `src/CoreGraphics/CGColor.cs`).

### Step 3: Register in TypeCache

Add a `Type?` property and register it in the constructor via `ConditionalLookup` (inside the framework guard) in `src/bgen/Caches/TypeCache.cs`:

```csharp
// Add the property (Type?, not TypeReference)
public Type? PMPrintSession { get; }

// Register it in the constructor, inside the framework guard:
if (frameworks.HavePrintCore) {
	// ... existing PrintCore lookups ...
	PMPrintSession = ConditionalLookup (platformAssembly, "PrintCore", "PMPrintSession");
}
```

### Step 4: Register in MarshalTypeList

Add the type in `MarshalTypeList.Load (TypeCache typeCache, …)` in `src/bgen/Models/MarshalTypeList.cs`, inside the same framework guard:

```csharp
if (frameworks.HavePrintCore) {
	// ... existing PrintCore adds ...
	Add (typeCache.PMPrintSession);
}
```

The default `MarshalType` constructor generates `Runtime.GetINativeObject<T>(ptr, owns)` marshaling, which is correct for all `NativeObject` subclasses.

### Step 5: Use concrete types in API definition

Now replace `IntPtr` return types with the concrete type in the bgen file:

```csharp
// Before — IntPtr with XML docs suggesting cast
[Export ("printSession")]
IntPtr PrintSession { get; }

// After — concrete type, bgen handles marshaling
[Export ("printSession")]
PMPrintSession PrintSession { get; }
```

### Real Example

In the PrintCore framework, `PMPrintSession`, `PMPrintSettings`, `PMPageFormat`, `PMPrinter`, and `PMPaper` were all registered as marshal types so protocol methods in `PDEPanel`, `PDEPlugIn`, and `PDEPlugInCallbackProtocol` could return them directly instead of `IntPtr`.

> ❌ **NEVER leave protocol methods returning IntPtr when a managed NativeObject wrapper exists.** Always check the `src/FrameworkName/` directory for existing wrapper classes before using `IntPtr`. If a wrapper exists, register it as a bgen marshal type using this pattern.

## Strongly-Typed Dictionaries

```csharp
[StrongDictionary ("MyOptionsKeys")]
interface MyOptions {
    string Name { get; set; }
    bool EnableFeature { get; set; }
}

[Static]
interface MyOptionsKeys {
    [Field ("MYNameKey")]
    NSString NameKey { get; }

    [Field ("MYEnableFeatureKey")]
    NSString EnableFeatureKey { get; }
}

// Usage in API
[Export ("configureWithOptions:")]
void Configure ([NullAllowed] NSDictionary options);

[Wrap ("Configure (options?.Dictionary)")]
void Configure (MyOptions options);
```

## NSSet of Typed-String Enums (NS_TYPED_ENUM)

When a native property returns an `NSSet<NSString>` whose elements are `NS_TYPED_ENUM` string constants (a set of "reasons" / "statuses"), bind a **weak** raw set (`[Export]`, `Weak` prefix) plus a **`[Wrap]`** that projects to a strongly-typed value (Xcode 26+ convention). The typed shape depends on the smart enum:

**Non-flags smart enum → `HashSet<TEnum>`** via `ToHashSet`:

```csharp
[iOS (27, 0), TV (27, 0), Mac (27, 0), MacCatalyst (27, 0)]
[Export ("reasonsNotRecommendedForCaptureUse")]
NSSet<NSString> WeakReasonsNotRecommendedForCaptureUse { get; }

[iOS (27, 0), TV (27, 0), Mac (27, 0), MacCatalyst (27, 0)]
[Wrap ("WeakReasonsNotRecommendedForCaptureUse.ToHashSet (v => AVExternalStorageDeviceReasonNotRecommendedForCaptureUseExtensions.GetValue (v))")]
HashSet<AVExternalStorageDeviceReasonNotRecommendedForCaptureUse> ReasonsNotRecommendedForCaptureUse { get; }
```

**`[Flags]` smart enum → the flags enum itself** via `ToFlags` (read/write shown):

```csharp
[Export ("textAlignments", ArgumentSemantic.Copy)]
NSSet<NSString> WeakTextAlignments { get; set; }

UITextFormattingViewControllerTextAlignment TextAlignments {
	[Wrap ("UITextFormattingViewControllerTextAlignmentExtensions.ToFlags (WeakTextAlignments)")]
	get;
	[Wrap ("WeakTextAlignments = new NSSet<NSString> (value.ToArray ())")]
	set;
}
```

- `...Extensions.GetValue` / `...Extensions.ToFlags` are **auto-generated** by bgen for a `[Field]`-backed smart enum; `ToHashSet<T> (Func<NSString, T>)` lives on `NSSet<NSString>` (`src/Foundation/NSSet_1.cs`).
- ⚠️ The `...Extensions` class must belong to the **same** enum you return — `XyzExtensions.ToFlags(...)` must project to `Xyz` (the `[Flags]` enum). Do **not** wrap one enum's `Extensions.ToFlags` into an *unrelated* enum's property.
- The older raw-`NSSet<NSString>`-only binding (no typed projection) is superseded — add the typed wrapper for new bindings.
- Precedents: `src/avfoundation.cs` `AVExternalStorageDevice.ReasonsNotRecommendedForCaptureUse` (`ToHashSet`); `src/uikit.cs` `UITextFormattingViewControllerFormattingDescriptor.TextAlignments` (`ToFlags`).

## Complex Type Conversions

```csharp
// Automatic conversion with BindAs
[return: BindAs (typeof (MyEnum[]))]
[Export ("getSupportedModes")]
NSString[] GetSupportedModes ();

[BindAs (typeof (CGRect))]
[Export ("bounds")]
NSValue Bounds { get; set; }
```

## Memory Management Attributes

```csharp
// Retained return value
[Export ("createObject")]
[return: Release]
NSObject CreateObject ();

// Transient parameter
[Export ("processObject:")]
void ProcessObject ([Transient] NSObject obj);

// Forced type for inheritance issues
[Export ("downloadTask")]
[return: ForcedType]
NSUrlSessionDownloadTask CreateDownloadTask ();
```

## Error Handling

Methods that take `NSError**` (bound as `out NSError`) **must always** have `[NullAllowed]` on the error parameter. The error output is `null` on success and only populated on failure — the Objective-C runtime does not guarantee a non-null error, so `[NullAllowed]` is required.

```csharp
// ✅ Correct — [NullAllowed] on the error parameter
[Export ("doSomethingWithError:")]
bool DoSomething ([NullAllowed] out NSError error);

[Export ("getSmartCardWithError:")]
[return: NullAllowed]
TKSmartCard GetSmartCard ([NullAllowed] out NSError error);

// ❌ Wrong — missing [NullAllowed]
[Export ("doSomethingWithError:")]
bool DoSomething (out NSError error);
```

> ❌ **NEVER** omit `[NullAllowed]` from `out NSError error` parameters. This is a consistent pattern across the entire codebase — every `out NSError` parameter uses `[NullAllowed]`.

## Re-exposing Designated Initializers in Subclasses

.NET constructors are not virtual, so when you bind a **new type that subclasses** an ObjC class that has a designated initializer, the subclass must re-expose that inherited initializer — otherwise the introspection `DesignatedInitializer` test (`tests/introspection/ApiCtorInitTest.cs`) fails with `<Type> should re-expose <Base>::.ctor(...)`. How you re-expose it depends on whether the inherited designated initializer is **failable** (returns `nil` + `NSError`).

**Non-failable designated init (no `out NSError`) — re-declare it as a public `[DesignatedInitializer]` `Constructor`** with the same selector and signature. Giving the subclass its own designated ctor satisfies the test directly, with no test-file change:

```csharp
[iOS (27, 0)]                       // illustrative subclass — not a real repo type
[BaseType (typeof (SomeBaseType))]
[DisableDefaultCtor]
interface MySubclass {
	// Re-exposed from the base type's designated initializer.
	[Export ("initWithName:")]
	[DesignatedInitializer]
	NativeHandle Constructor (string name);   // NativeHandle, not IntPtr — matches the base binding

	// ... the subclass's own members ...
}
```

**Failable designated init (`out NSError`) — you MUST use the factory variant, not a public constructor.** A public constructor with an `out NSError` parameter fails the cecil test `ConstructorTest.NoConstructorsWithOutErrorArguments` (`tests/cecil-tests/ConstructorTest.cs`): *"This constructor has an 'out NSError' parameter. Such constructors should be bound as factory methods instead."* (Only a fixed set of legacy ctors is grandfathered in `ConstructorTest.KnownFailures.cs`.) Bind the init as `[Internal]` `_Init...` and add a manual static `Create (...) → Type?` factory — this also gives clean nullable semantics instead of a throwing ctor. Real precedent — `AUHeadTrackingBinauralRenderer` (a subclass of `AUAudioUnit`, whose designated init `initWithComponentDescription:options:error:` is failable):

```csharp
[iOS (27, 0)]
[NoMac, NoTV, NoMacCatalyst]
[BaseType (typeof (AUAudioUnit))]
[DisableDefaultCtor]
interface AUHeadTrackingBinauralRenderer {
	// re-exposed from base class, bound [Internal] because a public ctor can't take out NSError
	[Export ("initWithComponentDescription:options:error:")]
	[DesignatedInitializer]
	[Internal]
	NativeHandle _InitWithComponentDescription (AudioComponentDescription componentDescription, AudioComponentInstantiationOptions options, [NullAllowed] out NSError outError);

	// ... the subclass's own members ...
}
```

Then add the manual factory in `src/AudioUnit/AUHeadTrackingBinauralRenderer.cs` — a `public static AUHeadTrackingBinauralRenderer? Create (...)` that does `new AUHeadTrackingBinauralRenderer (NSObjectFlag.Empty)` then calls the `_Init...` (see [Factory for Constructors](#factory-for-constructors) for the `NSObjectFlag.Empty` + `InitializeHandle (handle, "", false)` mechanics).

- The `[Internal]` `_Init...` keeps `[NullAllowed]` on the `out NSError` parameter — see [Error Handling](#error-handling).
- A re-exposed inherited selector is **not** reported by xtro as an extra selector (the base declares it); no `.ignore` entry is needed.

> ⚠️ The factory variant is **not** a real constructor, so the introspection test's generic re-expose check can't find it. You **must** add a `case "<YourType>": ... return true;` to the `Match ()` override in `tests/introspection/ApiCtorInitTest.cs` (the base file covers all platforms), or the `DesignatedInitializer` test still fails. Real precedents: `AVSpeechSynthesisProviderAudioUnit` and `AUHeadTrackingBinauralRenderer` — both bind the designated init as `[Internal]` and carry a `Match ()` case in `ApiCtorInitTest.cs` with a `// This constructor is exposed using a factory method.` note.

## Per-Member Platform Attributes

When a type is available on a platform but specific members are not:

```csharp
[TV (26, 4)]  // Type now available on tvOS
interface AVCaptionRenderer {
    // Existing members that are NOT on tvOS
    [NoTV]
    [Export ("existingMethod")]
    void ExistingMethod ();

    // New member that IS on tvOS
    [Export ("newMethod")]
    void NewMethod ();
}
```

## Resolving [Verify] Attributes

The generator adds `[Verify]` when it needs human confirmation:

```csharp
// StronglyTypedNSArray — replace NSObject[] with specific type
[Verify (StronglyTypedNSArray)]
[Export ("items")]
NSObject[] Items { get; }
// Fix: MyItem[] Items { get; }

// MethodToProperty — convert method to property if appropriate
[Verify (MethodToProperty)]
[Export ("isEnabled")]
bool IsEnabled ();
// Fix: bool IsEnabled { get; }

// PlatformInvoke — verify P/Invoke return type
[Verify (PlatformInvoke)]
[Export ("complexMethod")]
IntPtr ComplexMethod ();
```

All `[Verify]` attributes must be resolved before submitting a PR.

## Common Pitfalls

- **Null handling**: Always use `[NullAllowed]` where Apple's docs indicate nullability. Default assumption is non-null. However, if a `[DesignatedInitializer]` constructor crashes (segfault) when passed null, **remove `[NullAllowed]`** — the native API genuinely doesn't accept null, and removing it is better than adding introspection test exclusions.
- **Struct backing fields**: Only use blittable types. `bool` and `char` aren't blittable — use `byte` and `ushort`/`short` instead, with typed property accessors.
- **Threading**: UI APIs require main thread. Use `[ThreadSafe]` for thread-safe APIs.
- **Naming**: Follow .NET PascalCase for methods/properties. Remove redundant ObjC prefixes (`NSString name` → `string Name`). **C# type names preserve the Objective-C class prefix exactly** — `ARSession` stays `ARSession` (not `ArSession`), `AVPlayer` stays `AVPlayer` (not `AvPlayer`), `CGColor` stays `CGColor` (not `CgColor`). But an acronym *inside* the name (after the prefix) follows .NET rules — `NSURLSession` → `NSUrlSession`, `NSURLSessionHandler` → `NSUrlSessionHandler` (the `NS` prefix stays, but `URL` becomes `Url`). When creating new manual types for a framework, match the established prefix (e.g., new ARKit types use `AR*`). The .NET acronym casing rules apply within property/method names **and** to acronyms inside type names (SIMD → Simd, ID → Id when it means "identifier", URL → Url), never to the leading class prefix. (A few frameworks instead preserve an inner acronym across their whole family — e.g. CoreGraphics `CGPDF*` — so match the existing sibling types when a framework is consistent.) **Also match the casing of existing APIs on the same type**: if a type already exposes `GetExistingURLSession ()`, name a new sibling `GetNewURLSession ()` (not `GetNewUrlSession ()`) for consistency, and add a short comment explaining why the general acronym rule wasn't followed. Methods should be verbs, properties should be nouns. Don't blindly translate ObjC selector names — use .NET-appropriate verb names (e.g., `BuildMenu` not `MenuWithContents`).
- **Selectors**: Must match exactly — a single typo causes runtime crashes.
- **Protocol conformance**: All `[Abstract]` methods in a protocol are required.
- **nint/nuint**: Use `nint`/`nuint` for Objective-C `NSInteger`/`NSUInteger`.
- **XAMCORE_5_0**: Only for fixing breaking changes on existing shipped types. Never use for new code. See "XAMCORE_5_0 Pattern for Existing Types" below.
- **Handle access in manual code**: Use `GetCheckedHandle ()` instead of `Handle` when passing the native handle to P/Invokes in manual bindings. `GetCheckedHandle ()` throws `ObjectDisposedException` if the object has been disposed, preventing hard-to-debug native crashes.
- **Struct members**: Wrap public methods and properties in `#if !COREBUILD`, but NOT fields (bgen needs struct size). Never use `#pragma warning disable 0169`.
- **NativeObject class shells**: When a NativeObject type is in `CORE_SOURCES`, wrap the class body in `#if !COREBUILD` but keep the class declaration visible. See "NativeObject Return Types in Protocol Methods" above.
- **String types**: Use `string` (not `NSString`) for string parameters in methods, properties, and delegates. The binding generator handles marshaling automatically. Only use `NSString` for dictionary keys or strong-typed constants.

## XAMCORE_5_0 Pattern for Existing Types

When xtro reports a mismatch on an **existing** type that has already shipped (e.g., enum size wrong, missing `[Native]`, property type mismatch), **do not fix it directly** — that would be a binary-breaking change. Instead, use `#if XAMCORE_5_0` guards to queue the fix for the future while preserving current compatibility.

### Enum backing type fix

When xtro reports an enum should be `[Native]` (`: long`) but it already shipped without it:

```csharp
// In src/FrameworkName/Defs.cs or the enum file:
#if XAMCORE_5_0
	[Native]
	public enum ICReturnCodeOffset : long {
#else
	public enum ICReturnCodeOffset {
#endif
		DeviceNotFound = 0x9E00,
		DeviceNotOpen = 0x9E01,
		// ... values ...
	}
```

Then add a `.ignore` entry for the xtro mismatch:

```
# ICReturnCodeOffset is not [Native] for binary compatibility; fixed in XAMCORE_5_0
!wrong-enum-size! ICReturnCodeOffset managed 4 vs native 8
```

### Property/method type fix

```csharp
#if XAMCORE_5_0
	[Export ("name")]
	string Name { get; set; }  // correct type
#else
	[Export ("name")]
	NSString Name { get; set; }  // legacy type for binary compatibility
#endif
```

> ❌ **NEVER** apply a breaking change to an existing shipped type without `XAMCORE_5_0` guards. If you're unsure whether a type has shipped, check `git log` for the file — if the type existed before the current Xcode release cycle, it has shipped.

## Code Style Reminders

- Tabs for indentation, not spaces
- Space before parentheses: `Foo ()`, `Bar (1, 2)`, `array [0]`
- Use `""` not `string.Empty`
- Use `[]` not `Array.Empty<T> ()`
- Follow Mono code-formatting style from `.editorconfig`
- Match existing patterns in the framework's binding file

## Availability on Manual Code

API definition files (`src/frameworkname.cs`) use binding-style attributes:

```csharp
[iOS (26, 2), TV (26, 2), Mac (26, 2), MacCatalyst (26, 2)]
[Export ("newProperty")]
string NewProperty { get; }
```

Manual code files (`src/FrameworkName/*.cs`) use `[SupportedOSPlatform]` attributes on P/Invokes, properties, and methods:

```csharp
[SupportedOSPlatform ("ios26.2")]
[SupportedOSPlatform ("tvos26.2")]
[SupportedOSPlatform ("macos26.2")]
[SupportedOSPlatform ("maccatalyst26.2")]
public CTUIFontType UIFontType {
	get {
		return CTFontGetUIFontType (GetCheckedHandle ());
	}
}
```

Both styles are required. Omitting availability from P/Invokes or manual properties is a common mistake.

### Determining the Correct Version

The availability version represents **when Apple introduced the API**, not the current SDK version. Use these sources in order:

1. **Generated reference bindings** (best source) — after running `make -C tests/xtro-sharpie gen-all`, search for the API in the generated `.cs` files. These include `[Introduced]` attributes extracted from Apple's SDK headers:
   ```bash
   grep -rn "SomeApiName" tests/xtro-sharpie/api/*/ApiDefinition.cs
   ```

2. **Apple SDK headers** — search for `API_AVAILABLE` macros under `$XCODE_DEVELOPER_ROOT`

3. **Current SDK version** (`SdkVersions.cs`) — use only for **brand-new APIs** introduced in the current Xcode release:
   ```bash
   grep -E 'public const string (iOS|TVOS|OSX|MacCatalyst) ' tools/common/SdkVersions.cs
   ```

If the user specifies a different version (e.g., for a beta branch), use that instead.

### Common Version Mistakes

| Scenario | ❌ Wrong | ✅ Correct |
|----------|---------|-----------|
| Framework introduced on a new platform (e.g., MediaSetup → MacCatalyst) | Use current SDK version (26.5) | Research when Apple actually introduced it on that platform (could be 16.0) |
| New enum member added to existing enum | No per-member attribute, or enum-level version | Per-member attribute with the member's own introduction version |
| Brand-new API in current Xcode | — | Current SDK version from `SdkVersions.cs` is correct |

## Monotouch-Test Patterns

When manually binding C# APIs (P/Invokes, manual properties, struct accessors), add tests in `tests/monotouch-test/{FrameworkName}/`.

### File Structure

```
tests/monotouch-test/
├── CoreText/
│   ├── FontTest.cs
│   ├── FontDescriptorTest.cs
│   └── ...
├── CoreGraphics/
│   ├── FontTest.cs
│   ├── ContextTest.cs
│   └── ...
```

### Template

> ⚠️ **Framework availability guards:** If the framework is not available on all platforms (e.g., CarPlay is iOS-only), wrap the entire test file in `#if HAS_FRAMEWORKNAME` (e.g., `#if HAS_CARPLAY`). The build system defines these symbols based on which frameworks are available for each platform. Check the framework's existing test files for the correct symbol name.

```csharp
#if HAS_CORETEXT  // only needed if framework isn't on all platforms
using NUnit.Framework;
using Foundation;
using CoreText;  // framework under test
#if __MACOS__
using AppKit;
#else
using UIKit;
#endif

namespace MonoTouchFixtures.CoreText {  // MonoTouchFixtures.{FrameworkName}

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class FontTest {

		[Test]
		public void UIFontType_SystemFont ()
		{
			// Guard: skip test on runtimes older than the API's availability version
			TestRuntime.AssertXcodeVersion (26, 2);

			using (var font = new CTFont ("Helvetica", 12)) {
				var fontType = font.UIFontType;
				Assert.AreEqual (CTUIFontType.System, fontType, "UIFontType");
			}
		}

		[Test]
		public void LanguageAttribute_RoundTrip ()
		{
			TestRuntime.AssertXcodeVersion (26, 2);

			var attrs = new CTFontDescriptorAttributes () { Language = "en" };
			using (var desc = new CTFontDescriptor (attrs)) {
				// Round-trip test: set a value, read it back
				var readAttrs = desc.GetAttributes ();
				Assert.AreEqual ("en", readAttrs.Language, "Language");
			}
		}
	}
}
#endif // HAS_CORETEXT — only needed if framework isn't on all platforms
```

### Key Patterns

| Pattern | Usage |
|---------|-------|
| `TestRuntime.AssertXcodeVersion (X, Y)` | Skip test if runtime is older than API availability |
| `TestRuntime.CheckXcodeVersion (X, Y)` | Boolean check for conditional logic within a test |
| `[Preserve (AllMembers = true)]` | Prevents linker from stripping test methods |
| `using` statements | Always clean up handle-based objects |
| Namespace `MonoTouchFixtures.*` | Match framework name (e.g., `MonoTouchFixtures.CoreText`) |
| Platform-conditional imports | `#if __MACOS__` for AppKit vs UIKit |

### What to Test

- **P/Invoke wrappers**: Call the C# wrapper and verify it returns sensible values
- **Manual properties**: Set a value, read it back (round-trip test)
- **Struct accessors**: Create a struct, set properties, verify getters return expected values
- **Null handling**: Verify null parameters behave correctly (return null, throw `ArgumentNullException`, etc.)
- **Enum conversions**: Verify known native values map to the correct C# enum values
