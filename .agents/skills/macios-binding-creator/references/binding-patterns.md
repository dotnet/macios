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
- **Struct members**: Wrap public methods and properties in `#if !COREBUILD`, but NOT fields (bgen needs struct size). Never use `#pragma warning disable 0169`.

## Code Style Reminders

- Tabs for indentation, not spaces
- Space before parentheses: `Foo ()`, `Bar (1, 2)`, `array [0]`
- Use `""` not `string.Empty`
- Use `[]` not `Array.Empty<T> ()`
- Follow Mono code-formatting style from `.editorconfig`
- Match existing patterns in the framework's binding file
