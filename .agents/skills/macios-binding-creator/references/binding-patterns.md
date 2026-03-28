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

> ⚠️ **Don't redeclare protocol-inherited properties.** When a type conforms to a protocol, it inherits the protocol's properties. If the type already has those properties bound (e.g., `title`, `artist`), do NOT redeclare them or you'll get CS0108 (member hides inherited member) warnings. Remove the duplicates from the conforming type.

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

### Struct Binding Rules

- **Only use blittable types as backing fields in structs.** `bool` and `char` aren't blittable — use `byte` and `ushort`/`short` instead. This avoids `[MarshalAs]` and cecil test known failures.
- **Wrap all public methods and properties in `#if !COREBUILD`** — never use `#pragma warning disable 0169`. Do NOT wrap fields, because bgen may do different things depending on the size of a struct, so it needs to know the final size.
- **NEVER use `XAMCORE_5_0` for new code.** `XAMCORE_5_0` is only for fixing breaking API changes on existing types that shipped in prior releases.
- If a struct member is platform-specific, use `#if !TVOS` (or similar) to exclude it.

### Platform Exclusion for Manual Types

When a manual type (struct, helper class) is not available on tvOS:

```csharp
// In src/FrameworkName/MyStruct.cs:
#if !TVOS
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
#endif // !TVOS

// In src/frameworkname.cs (at the top of the file):
#if TVOS
using MyStruct = Foundation.NSObject;
#endif
```

The type alias lets tvOS compilation succeed. The `[NoTV]` attribute on the API definition interface ensures the type won't appear in the final tvOS assembly.

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

```csharp
// Methods that take NSError** and always add [NullAllowed] to the error parameter
[Export ("doSomething:")]
bool DoSomething ([NullAllowed] out NSError error);
```

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
- **Naming**: Follow .NET PascalCase for methods/properties. Remove redundant ObjC prefixes (`NSString name` → `string Name`). Acronyms shouldn't be all uppercase (SIMD → Simd, ID → Id when it means "identifier", URL → Url). Methods should be verbs, properties should be nouns. Don't blindly translate ObjC selector names — use .NET-appropriate verb names (e.g., `BuildMenu` not `MenuWithContents`).
- **Selectors**: Must match exactly — a single typo causes runtime crashes.
- **Protocol conformance**: All `[Abstract]` methods in a protocol are required.
- **nint/nuint**: Use `nint`/`nuint` for Objective-C `NSInteger`/`NSUInteger`.
- **XAMCORE_5_0**: Only for fixing breaking changes on existing shipped types. Never use for new code.
- **Handle access in manual code**: Use `GetCheckedHandle ()` instead of `Handle` when passing the native handle to P/Invokes in manual bindings. `GetCheckedHandle ()` throws `ObjectDisposedException` if the object has been disposed, preventing hard-to-debug native crashes.
- **Struct members**: Wrap public methods and properties in `#if !COREBUILD`, but NOT fields (bgen needs struct size). Never use `#pragma warning disable 0169`.

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

Check `tools/common/SdkVersions.cs` for the current SDK versions:

```bash
grep -E 'public const string (iOS|TVOS|OSX|MacCatalyst) ' tools/common/SdkVersions.cs
```

Or check `Make.versions`:

```bash
grep '_NUGET_OS_VERSION=' Make.versions
```

Use these values for all availability attributes. If the user specifies a different version (e.g., for a beta branch), use that instead.

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
#if MONOMAC
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
| Platform-conditional imports | `#if MONOMAC` for AppKit vs UIKit |

### What to Test

- **P/Invoke wrappers**: Call the C# wrapper and verify it returns sensible values
- **Manual properties**: Set a value, read it back (round-trip test)
- **Struct accessors**: Create a struct, set properties, verify getters return expected values
- **Null handling**: Verify null parameters behave correctly (return null, throw `ArgumentNullException`, etc.)
- **Enum conversions**: Verify known native values map to the correct C# enum values
