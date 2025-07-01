# RGen (Roslyn Generator) - Comprehensive Software Specification

## Table of Contents
1. [Executive Summary](#executive-summary)
2. [Project Overview](#project-overview)
3. [Architecture](#architecture)
4. [Components](#components)
5. [Data Models](#data-models)
6. [Code Generation Workflow](#code-generation-workflow)
7. [Type System and Marshalling](#type-system-and-marshalling)
8. [Memory Management](#memory-management)
9. [Thread Safety](#thread-safety)
10. [Binding Patterns](#binding-patterns)
11. [Attribute System](#attribute-system)
12. [Diagnostics and Analysis](#diagnostics-and-analysis)
13. [Incremental Generation](#incremental-generation)
14. [Migration and Transformation](#migration-and-transformation)
15. [Error Handling](#error-handling)
16. [Testing Strategy](#testing-strategy)
17. [Configuration and Deployment](#configuration-and-deployment)
18. [API Reference](#api-reference)
19. [Implementation Details](#implementation-details)
20. [Performance Considerations](#performance-considerations)
21. [Future Considerations](#future-considerations)

## Executive Summary

RGen (Roslyn Generator) is a modern binding generator for Microsoft's macOS and iOS platforms that replaces the legacy bgen tool. It leverages Roslyn-based source generators and analyzers to create C# bindings for Apple platform APIs (iOS, tvOS, macOS, MacCatalyst). The project provides compile-time code generation, real-time diagnostics, and automated migration tools to improve developer productivity and binding quality.

### Key Benefits
- **Compile-time generation**: No runtime reflection, better performance
- **IDE integration**: Real-time diagnostics and code fixes
- **Type safety**: Strong typing with comprehensive marshalling
- **Platform awareness**: Built-in support for Apple platform versioning
- **Migration path**: Automated transformation from legacy bgen bindings
- **Memory safety**: Automatic GC.KeepAlive and handle management
- **Thread safety**: Automatic UI thread checks and marshalling

### Key Technologies
- **Roslyn** (Microsoft.CodeAnalysis 4.x) - Code analysis and generation
- **.NET 8.0+** - Target framework with latest C# language features
- **Incremental Generators** - IIncrementalGenerator for performance
- **Immutable Data Structures** - Thread-safe data models
- **Channel-based Processing** - Marille for parallel transformation

## Project Overview

### Purpose
RGen addresses the limitations of the legacy bgen tool by:
- Moving from reflection-based to source-generator-based binding generation
- Providing compile-time validation and IDE integration
- Supporting incremental compilation for better performance
- Offering a migration path for existing bindings
- Ensuring memory and thread safety through automated patterns

### Historical Context
RGen is part of the initiative documented in [RFC: Migrate bgen to use roslyn instead of the reflection API](https://github.com/dotnet/macios/issues/21308). It represents a fundamental shift in how macOS/iOS bindings are generated, moving from runtime reflection to compile-time source generation.

### Scope
The project encompasses:
- Source generation for Objective-C to C# bindings
- Static analysis and diagnostics for binding code
- Automated code fixes for common issues
- Migration tooling from legacy formats
- Support for all Apple platforms (iOS, tvOS, macOS, MacCatalyst)
- Memory management patterns (GC.KeepAlive, autorelease pools)
- Thread safety enforcement (UI thread checks)
- Type marshalling and conversion
- Async/await pattern generation from completion handlers

### Target Users
- Xamarin/MAUI developers creating Apple platform bindings
- Microsoft platform team maintaining official bindings
- Third-party library binding authors
- Framework developers extending platform capabilities

## Architecture

### High-Level Architecture

```
┌─────────────────────────────────────────────────────────────────┐
│                    Developer Experience Layer                     │
├─────────────────┬───────────────────┬────────────────────────────┤
│   IDE Support   │  Compiler Errors  │    Code Fixes             │
│   (Analyzers)   │   (Diagnostics)   │   (CodeFixers)            │
└────────┬────────┴─────────┬─────────┴──────────┬─────────────────┘
         │                  │                    │
┌────────▼────────┐ ┌───────▼──────┐ ┌──────────▼───────────┐
│    Bindings     │ │   Generator  │ │    Transformer       │
│    Analyzer     │ │  (Roslyn SG) │ │  (Migration Tool)    │
└────────┬────────┘ └───────┬──────┘ └──────────┬───────────┘
         │                  │                    │
         └──────────────────┴────────────────────┘
                            │
                 ┌──────────▼──────────┐
                 │   Common Library    │
                 │  (Shared Types)     │
                 └─────────────────────┘
```

### Component Dependencies

```
Microsoft.Macios.Binding.Common (shared foundation)
    ├── Microsoft.Macios.Generator (core code generator)
    │   └── Microsoft.Macios.Generator.Sample
    ├── Microsoft.Macios.Bindings.Analyzer (Roslyn analyzer)
    │   └── Microsoft.Macios.Bindings.Analyzer.Sample
    ├── Microsoft.Macios.Bindings.CodeFixers (code fixes)
    └── Microsoft.Macios.Transformer (standalone tool)
        └── Microsoft.Macios.Transformer.Generator (source generator)

Test Projects:
- Microsoft.Macios.Generator.Tests
- Microsoft.Macios.Bindings.Analyzer.Tests
- Microsoft.Macios.Transformer.Tests
- Microsoft.Macios.Bindings.CodeFixers.Tests
```

### Design Principles
1. **Incremental Generation**: Optimize for compilation performance through caching
2. **Immutability**: Data models are immutable for thread safety
3. **Separation of Concerns**: Clear boundaries between analysis, generation, and transformation
4. **Platform Agnostic Core**: Core logic independent of specific platforms
5. **Extensibility**: Factory patterns for adding new binding types
6. **Zero Allocation**: Minimize allocations in hot paths using struct types
7. **Deterministic Output**: Same input always produces same output

### Architectural Patterns
- **Visitor Pattern**: For traversing syntax trees (BaseTypeDeclarationSyntax)
- **Factory Pattern**: EmitterFactory creates appropriate emitters
- **Builder Pattern**: TabbedStringBuilder for code generation
- **Equality Comparers**: Custom comparers for incremental generation
- **Channel Pattern**: Parallel processing in transformer
- **Strategy Pattern**: Different emitters for different binding types

## Components

### 1. Microsoft.Macios.Generator (Core Source Generator)

**Purpose**: Implements the Roslyn incremental source generator for binding code generation.

**Key Classes**:
- `BindingSourceGeneratorGenerator`: Main entry point implementing IIncrementalGenerator
- `EmitterFactory`: Creates appropriate emitters based on binding type
- `BindingSyntaxFactory`: Centralized syntax node creation with sub-factories:
  - `Arguments.cs`: Argument marshalling syntax
  - `Dlfcn.cs`: Dynamic library loading syntax
  - `KnownTypes.cs`: Common type references
  - `ObjCRuntime.cs`: Runtime interop syntax
  - `Property.cs`: Property-specific syntax
  - `Runtime.cs`: Runtime helper methods
  - `Trampoline.cs`: Delegate bridge syntax
- `TrampolineEmitter`: Generates delegate trampolines for callbacks
- `AsyncResultEmitter`: Creates Task-based async wrappers
- `LibraryEmitter`: Generates library handle management
- `ClassEmitter`: Emits binding code for classes
- `InterfaceEmitter`: Emits binding code for protocols
- `CategoryEmitter`: Emits extension methods for categories
- `EnumEmitter`: Handles smart enum generation

**Responsibilities**:
- Parse binding attributes from source code
- Generate native method bindings and trampolines
- Create library handle management code
- Emit platform-specific availability attributes
- Generate async wrappers for completion handlers
- Insert memory management patterns (GC.KeepAlive)
- Add thread safety checks for UI frameworks

**Generated Files**:
- `[TypeName].g.cs`: Individual binding files
- `Libraries.g.cs`: Static library handle management
- `Trampolines.g.cs`: Delegate trampolines
- `AsyncResults/[TypeName]Async.g.cs`: Async result types

### 2. Microsoft.Macios.Bindings.Analyzer (Roslyn Analyzers)

**Purpose**: Provides compile-time diagnostics for binding code correctness.

**Key Analyzers**:
- `BindingTypeSemanticAnalyzer`: Validates binding type declarations
- `SmartEnumsAnalyzer`: Ensures smart enum implementations are correct
- `NativeObjectHandleAnalyzer`: Checks native handle usage patterns

**Diagnostic Categories**:
- **Structure** (RBI0001-RBI0007): Partial class requirements, correct attribute usage
- **Smart Enums** (RBI0008-RBI0010): Backing field validation
- **Libraries** (RBI0011-RBI0012): Library naming conventions
- **Type Safety** (RBI0013): Correct attribute type usage
- **Memory** (RBI0014): Native object lifetime management

**Diagnostic Details**:
- RBI0000: Unexpected error handler
- RBI0001: Binding types must be partial
- RBI0002: BindingType<Class> must be on a class
- RBI0003: BindingType<Category> must be on a class
- RBI0004: Category classes must be static
- RBI0005: BindingType<Protocol> must be on an interface
- RBI0006: BindingType without type argument must be on enum
- RBI0007: BindingType<StrongDictionary> must be on a class
- RBI0008: Smart enum values need Field<EnumValue> attribute
- RBI0009: Duplicate backing fields not allowed
- RBI0010: Backing field names must be valid identifiers
- RBI0011: Non-Apple frameworks require library names
- RBI0012: Apple frameworks cannot specify library names
- RBI0013: Wrong attribute type on enum values
- RBI0014: Native objects accessed by handle must be kept alive

### 3. Microsoft.Macios.Bindings.CodeFixers

**Purpose**: Automated fixes for common binding issues detected by analyzers.

**Key Features**:
- Quick fixes for missing partial modifiers
- Automatic backing field generation for smart enums
- Correction of attribute placement
- Library name formatting fixes
- GC.KeepAlive insertion for handle access

**Code Fix Providers**:
- `BindingTypeCodeFixProvider`: Fixes structural issues
- Smart enum field generators
- Attribute correction providers

### 4. Microsoft.Macios.Transformer (Migration Tool)

**Purpose**: Command-line tool for migrating legacy bgen bindings to rgen format.

**Architecture**:
- Channel-based parallel processing using Marille
- Multi-platform RSP file support
- Roslyn-based parsing and transformation
- Preservation of binding semantics during migration

**Key Components**:
- `Transformer`: Main orchestration logic
- `Main.cs`: CLI entry point with System.CommandLine
- Worker classes for different binding types:
  - `ClassTransformer`: Transforms class bindings
  - `ProtocolTransformer`: Transforms protocol bindings
  - `CategoryTransformer`: Transforms category extensions
  - `SmartEnumTransformer`: Transforms smart enums
  - `StrongDictionaryTransformer`: Transforms dictionaries
  - `CoreImageFilterTransformer`: Special Core Image handling
  - `ErrorDomainTransformer`: Error domain transformations
- Platform-specific transformations
- Serilog integration for structured logging

**Command Line Options**:
- Multiple RSP file inputs for different platforms
- Output directory specification
- Verbosity control
- Platform-specific processing flags

### 5. Microsoft.Macios.Binding.Common

**Purpose**: Shared utilities and types used across all components.

**Contents**:
- Common type definitions
- Shared constants and enumerations
- Utility methods for binding operations
- Platform-agnostic helpers
- Shared data structures

## Data Models

### Core Binding Model

```csharp
[StructLayout(LayoutKind.Auto)]
public readonly record struct Binding
{
    // Identity
    public string Name { get; init; }
    public ImmutableArray<string> Namespace { get; init; }
    public string FullyQualifiedSymbol { get; init; }
    
    // Type Information
    public BindingType BindingType { get; init; }
    public BindingInfo BindingInfo { get; init; }
    public string? RegisterName { get; init; }
    public ImmutableArray<AttributeCodeChange> Attributes { get; init; }
    
    // Modifiers
    public bool IsStatic { get; init; }
    public bool IsPartial { get; init; }
    public bool IsAbstract { get; init; }
    public bool IsSealed { get; init; }
    public bool IsUnsafe { get; init; }
    public bool IsReadOnly { get; init; }
    public ImmutableArray<string> Modifiers { get; init; }
    
    // Inheritance
    public TypeInfo? Base { get; init; }
    public ImmutableArray<TypeInfo> Interfaces { get; init; }
    
    // Members
    public ImmutableArray<Property> Properties { get; init; }
    public ImmutableArray<Method> Methods { get; init; }
    public ImmutableArray<Constructor> Constructors { get; init; }
    public ImmutableArray<Event> Events { get; init; }
    public ImmutableArray<EnumMember> EnumMembers { get; init; }
    
    // Availability
    public SymbolAvailability SymbolAvailability { get; init; }
    
    // Context
    public ImmutableArray<string> UsingDirectives { get; init; }
    public IEnumerable<(string LibraryName, string? LibraryPath)> LibraryPaths { get; init; }
    public IEnumerable<TypeInfo> Trampolines { get; init; }
    public IEnumerable<AsyncResultInfo> AsyncResults { get; init; }
    
    // Performance Optimizations
    public ImmutableDictionary<string, ImmutableHashSet<int>> PropertiesWithSelector { get; init; }
    public ImmutableDictionary<string, ImmutableHashSet<int>> MethodsWithSelector { get; init; }
}
```

### Type Information Model

```csharp
[StructLayout(LayoutKind.Auto)]
public readonly partial struct TypeInfo : IEquatable<TypeInfo>
{
    // Basic Information
    public string Name { get; init; }
    public ImmutableArray<string> Namespace { get; init; }
    public string FullyQualifiedName { get; init; }
    public string? MetadataName { get; init; }
    
    // Type Characteristics
    public bool IsNullable { get; init; }
    public bool IsBlittable { get; init; }
    public bool IsArray { get; init; }
    public bool IsGenericType { get; init; }
    public bool IsTuple { get; init; }
    public bool IsPointer { get; init; }
    public bool IsReferenceType { get; init; }
    public bool IsStruct { get; init; }
    public bool IsInterface { get; init; }
    public bool IsWrapped { get; init; }
    public bool IsTask { get; init; }
    
    // ObjC Interop
    public bool IsNSObject { get; init; }
    public bool IsINativeObject { get; init; }
    public bool IsProtocol { get; init; }
    public bool IsDelegate { get; init; }
    public bool IsSmartEnum { get; init; }
    public bool IsNativeEnum { get; init; }
    public bool IsDictionaryContainer { get; init; }
    
    // Type Details
    public SpecialType SpecialType { get; init; }
    public SpecialType? EnumUnderlyingType { get; init; }
    public SpecialType? ArrayElementType { get; init; }
    public ImmutableArray<TypeInfo> TypeArguments { get; init; }
    public DelegateInfo? Delegate { get; init; }
    
    // Inheritance
    public ImmutableArray<string> Parents { get; init; }
    public ImmutableArray<string> Interfaces { get; init; }
    
    // Array Element Details
    public bool ArrayElementTypeIsWrapped { get; init; }
    public bool ArrayElementIsINativeObject { get; init; }
    
    // Special Types
    public bool IsVoid => SpecialType == SpecialType.System_Void;
    public bool IsNativeIntegerType { get; init; }
    
    // Marshalling
    public string? ToMarshallType() { /* ... */ }
    public TypeInfo ToArrayElementType() { /* ... */ }
    public TypeInfo WithNullable(bool isNullable) { /* ... */ }
    public TypeInfo ToPointedAtType() { /* ... */ }
}
```

### Method Model

```csharp
[StructLayout(LayoutKind.Auto)]
public readonly record struct Method
{
    public string MethodName { get; init; }
    public string Selector { get; init; }
    public ImmutableArray<Parameter> Parameters { get; init; }
    public TypeInfo ReturnType { get; init; }
    public ImmutableArray<AttributeCodeChange> Attributes { get; init; }
    public SymbolAvailability SymbolAvailability { get; init; }
    public ObjcBindingFlags Flags { get; init; }
    public ExportData<ObjCBindings.Method> ExportMethodData { get; init; }
    
    // Computed Properties
    public bool IsAsync { get; }
    public bool IsVariadic { get; }
    public bool IsStatic { get; }
    public bool IsProtected { get; }
    public bool IsInternal { get; }
    public bool IsPublic { get; }
    public bool IsVirtual { get; }
    public bool IsAbstract { get; }
    public bool IsOverride { get; }
    public bool IsForcedType { get; }
    public bool IsReturnRelease { get; }
}
```

### Property Model

```csharp
[StructLayout(LayoutKind.Auto)]
public readonly record struct Property
{
    public string PropertyName { get; init; }
    public string? Selector { get; init; }
    public TypeInfo Type { get; init; }
    public ImmutableArray<Accessor> Accessors { get; init; }
    public ImmutableArray<AttributeCodeChange> Attributes { get; init; }
    public SymbolAvailability SymbolAvailability { get; init; }
    public PropertyKind Kind { get; init; }
    
    // Export Data
    public ExportData<ObjCBindings.Property>? ExportPropertyData { get; init; }
    public FieldData<ObjCBindings.Property>? FieldPropertyData { get; init; }
    
    // Computed Properties
    public bool IsProperty => Kind == PropertyKind.Export;
    public bool IsField => Kind == PropertyKind.Field;
    public bool IsNotification => IsField && FieldPropertyData?.Flags.HasFlag(ObjCBindings.Property.Notification) == true;
    public bool IsStatic { get; }
    public bool IsReferenceType { get; }
    public bool IsNullable { get; }
    public bool IsBindAs { get; }
    public bool IsWeakDelegate { get; }
    public bool IsThreadSafe { get; }
    public bool RequiresDirtyCheck { get; }
}
```

## Code Generation Workflow

### 1. Source Analysis Phase
```
Source Code → Syntax Trees → Semantic Model → Binding Discovery
```

**Steps**:
1. **Syntax Tree Parsing**: 
   - Filter for BaseTypeDeclarationSyntax nodes
   - Check for BindingType attributes
   - Extract using directives
   
2. **Semantic Analysis**: 
   - Resolve type symbols
   - Extract type relationships
   - Determine type characteristics
   
3. **Attribute Extraction**: 
   - Parse binding attributes
   - Extract parameters and flags
   - Build attribute data models
   
4. **Data Model Creation**: 
   - Create immutable Binding structs
   - Build member collections
   - Calculate availability

### 2. Incremental Generation Pipeline

```csharp
// Pipeline structure in BindingSourceGeneratorGenerator
var provider = context.SyntaxProvider
    .CreateSyntaxProvider(
        predicate: (node, _) => IsValidNode(node),
        transform: (ctx, _) => GetChangesForSourceGen(ctx))
    .Where(tuple => tuple.BindingAttributeFound);

var bindings = provider
    .Select((tuple, _) => (tuple.RootBindingContext, tuple.Bindings))
    .WithComparer(equalityComparer);

var libraryProvider = provider
    .Select((tuple, _) => (tuple.RootBindingContext, tuple.Bindings.LibraryPaths));

var trampolineProvider = provider
    .Select((tuple, _) => (tuple.RootBindingContext, tuple.Bindings.Trampolines));

var asyncResultsProvider = provider
    .Select((tuple, _) => (tuple.RootBindingContext, tuple.Bindings.AsyncResults));
```

### 3. Code Generation Phase
```
Binding Models → Emitters → Syntax Nodes → Generated Files
```

**Generation Process**:
1. **Emitter Selection**: 
   - EmitterFactory.TryCreate based on BindingType
   - Each binding type has specialized emitter
   
2. **Member Generation**: 
   - Process properties with backing fields
   - Generate methods with marshalling
   - Create constructors with initialization
   - Emit events with proper delegates
   
3. **Trampoline Creation**: 
   - Identify delegate parameters
   - Generate static bridge classes
   - Create native invocation classes
   - Build P/Invoke compatible signatures
   
4. **Library Management**: 
   - Collect unique library references
   - Generate Dlfcn.dlopen calls
   - Create static handle fields
   
5. **File Output**: 
   - Use TabbedStringBuilder for formatting
   - Write to SourceProductionContext
   - Include proper namespacing

### 4. Generated File Structure
```
Output/
├── Foundation/
│   ├── NSObject.g.cs
│   ├── NSString.g.cs
│   └── NSArray.g.cs
├── UIKit/
│   ├── UIView.g.cs
│   └── UIViewController.g.cs
├── Libraries.g.cs
├── Trampolines.g.cs
└── AsyncResults/
    ├── NSUrlSessionTaskAsync.g.cs
    └── NSAttributedStringAsync.g.cs
```

### 5. Code Generation Templates

**Class Template**:
```csharp
// Header with copyright and generated code attributes
namespace [Namespace]
{
    [Register("[RegisterName]")]
    public partial class [ClassName] : [BaseClass], [Interfaces]
    {
        // Constructors
        [Export("init")]
        public [ClassName]() : base(NSObjectFlag.Empty) { /* ... */ }
        
        // Properties with backing fields
        static [Type]? __mt_[PropertyName]_var;
        [Export("[selector]", ArgumentSemantic.[Semantic])]
        public [Type] [PropertyName] 
        { 
            get { /* backing field pattern */ }
            set { /* setter with dirty check */ }
        }
        
        // Methods with marshalling
        [Export("[selector]")]
        public [ReturnType] [MethodName]([Parameters])
        {
            // UI thread check if needed
            // Parameter marshalling
            // Native call with GC.KeepAlive
            // Result marshalling
        }
    }
}
```

## Type System and Marshalling

### Type Marshalling Rules

The `TypeInfo.ToMarshallType()` method implements comprehensive marshalling rules:

```csharp
public string? ToMarshallType()
{
    return this switch
    {
        // Arrays always marshal as NativeHandle
        { IsArray: true } => "NativeHandle",
        
        // Special numeric types
        { Name: "nfloat" or "NFloat" } => "nfloat",
        { Name: "nint" or "nuint" } => MetadataName,
        
        // Strings marshal as NSString (NativeHandle)
        { SpecialType: SpecialType.System_String } => "NativeHandle",
        
        // Objects implementing NSObject or INativeObject
        { IsNSObject: true } => "NativeHandle",
        { IsINativeObject: true } => "NativeHandle",
        
        // Structs
        { IsStruct: true, SpecialType: SpecialType.System_Double } => "Double",
        { IsStruct: true } => Name,
        
        // Enums
        { IsNativeEnum: true, EnumUnderlyingType: SpecialType.System_Int64 } => "IntPtr",
        { IsNativeEnum: true, EnumUnderlyingType: SpecialType.System_UInt64 } => "UIntPtr",
        { IsSmartEnum: true } => "NativeHandle", // NSString backing
        { IsEnum: true, EnumUnderlyingType: not null } => EnumUnderlyingType.GetKeyword(),
        
        // Special types
        { SpecialType: SpecialType.System_Void } => SpecialType.GetKeyword(),
        { IsReferenceType: false } => Name,
        { IsDelegate: true } => "NativeHandle",
        
        _ => null
    };
}
```

### Parameter Marshalling

**Handle Creation for Objects**:
```csharp
// For nullable types
var handle = parameter?.GetHandle() ?? NativeHandle.Zero;

// For non-nullable types
var handle = parameter!.GetNonNullHandle(nameof(parameter));
```

**String Marshalling**:
```csharp
// Create native string
var nsstring = CFString.CreateNative(managedString);
try {
    // Use native string
} finally {
    CFString.ReleaseNative(nsstring);
}
```

**Array Marshalling**:
```csharp
// Managed to native
var nsa_array = NSArray.FromNSObjects(managedArray);

// Native to managed  
var managedArray = NSArray.ArrayFromHandle<T>(nativeHandle);
```

### Delegate Trampolines

**Trampoline Structure**:
```csharp
internal sealed class D[DelegateName] : [DelegateType] {
    // Static bridge from native to managed
    public static class SD[DelegateName] {
        static readonly IntPtr ptrToMethod = /* ... */;
        
        [MonoPInvokeCallback(typeof(D[DelegateName]))]
        static [ReturnType] Invoke(IntPtr block, [Parameters]) {
            var del = BlockLiteral.GetTarget<[DelegateType]>(block);
            return del?.Invoke([Arguments]) ?? default;
        }
    }
    
    // Native invocation from managed to native
    internal sealed class NID[DelegateName] {
        IntPtr BlockPtr;
        D[DelegateName] invoker;
        
        [Preserve]
        public unsafe NID[DelegateName](BlockLiteral* block) {
            BlockPtr = (IntPtr)block;
            invoker = block->GetDelegateForBlock<D[DelegateName]>();
        }
        
        [Preserve]
        public [ReturnType] Invoke([Parameters]) {
            return invoker(BlockPtr, [Arguments]);
        }
    }
}
```

## Memory Management

### GC.KeepAlive Patterns

**Property Getters**:
```csharp
public NSString Title {
    get {
        NSString? ret;
        if (IsDirectBinding) {
            ret = Runtime.GetNSObjectTx(Messaging.IntPtr_objc_msgSend(this.Handle, selHandle));
        } else {
            ret = Runtime.GetNSObjectTx(Messaging.IntPtr_objc_msgSendSuper(this.SuperHandle, selHandle));
        }
        GC.KeepAlive(this); // Prevent collection during native call
        return ret!;
    }
}
```

**Method Calls**:
```csharp
public void DoSomething(NSObject parameter) {
    var parameterHandle = parameter?.GetHandle() ?? NativeHandle.Zero;
    if (IsDirectBinding) {
        Messaging.void_objc_msgSend_IntPtr(this.Handle, Selector.GetHandle("doSomething:"), parameterHandle);
    } else {
        Messaging.void_objc_msgSendSuper_IntPtr(this.SuperHandle, Selector.GetHandle("doSomething:"), parameterHandle);
    }
    GC.KeepAlive(parameter); // Keep parameter alive
    GC.KeepAlive(this);      // Keep receiver alive
}
```

### Autorelease Pool Management

**AutoRelease Attribute**:
```csharp
[Export<Method>("processItems:", Flags = Method.AutoRelease)]
public void ProcessItems(NSArray items) {
    // Generated code wraps in autorelease pool
    using var autorelease_pool = new NSAutoreleasePool();
    // Method implementation
}
```

### Handle Lifetime Management

**RetainAndAutorelease Patterns**:
```csharp
// For NSObject arrays
return Runtime.RetainAndAutoreleaseNSObject(NSArray.FromNSObjects(managedArray));

// For INativeObject types
return Runtime.RetainAndAutoreleaseNativeObject(nativeObject.Handle);
```

**String Lifecycle**:
```csharp
var nsstring = CFString.CreateNative(str);
try {
    // Use native string
    Messaging.void_objc_msgSend_IntPtr(Handle, Selector.GetHandle("setTitle:"), nsstring);
} finally {
    CFString.ReleaseNative(nsstring);
}
```

### Weak References

**Weak Delegate Pattern**:
```csharp
// Weak property
object? __mt_WeakDelegate_var;
[Export("delegate", ArgumentSemantic.Weak)]
public NSObject? WeakDelegate {
    get => __mt_WeakDelegate_var as NSObject;
    set {
        __mt_WeakDelegate_var = value;
        MarkDirty();
    }
}

// Companion strong property
public IMyDelegate Delegate {
    get => WeakDelegate as IMyDelegate;
    set {
        var rvalue = value as NSObject;
        if (value is not null && rvalue is null)
            throw new ArgumentException($"The object passed of type {value.GetType()} does not derive from NSObject");
        WeakDelegate = rvalue;
    }
}
```

## Thread Safety

### UI Thread Checking

**Automatic UI Thread Enforcement**:
```csharp
// Determined by namespace and platform
bool NeedsThreadChecks = !IsThreadSafe && UINamespaces.Contains(Namespace);

// Generated check
if (NeedsThreadChecks) {
    UIApplication.EnsureUIThread(); // iOS/tvOS/Catalyst
    NSApplication.EnsureUIThread(); // macOS
}
```

**UI Namespaces**:
- iOS/tvOS/Catalyst: `UIKit`
- macOS: `AppKit`

### ThreadSafe Attribute

**Binding Level**:
```csharp
[BindingType<Class>(Flags = Class.IsThreadSafe)]
public partial class MyThreadSafeClass { }
```

**Method Level**:
```csharp
[Export<Method>("threadSafeMethod", Flags = Method.IsThreadSafe)]
public void ThreadSafeMethod() { }
```

**Property Level**:
```csharp
[Export<Property>("threadSafeProperty", Flags = Property.IsThreadSafe)]
public string ThreadSafeProperty { get; set; }
```

### Thread Static Properties

```csharp
[Export<Property>("current", Flags = Property.IsThreadStatic)]
public static MyClass Current {
    get => __mt_Current_var;
    set => __mt_Current_var = value;
}
```

## Binding Patterns

### 1. Class Bindings

**Basic Class**:
```csharp
[BindingType<Class>]
public partial class MyView : NSView
{
    [Export<Constructor>("initWithFrame:")]
    public MyView(CGRect frame) : base(NSObjectFlag.Empty)
    {
        if (IsDirectBinding)
            InitializeHandle(Messaging.IntPtr_objc_msgSend_CGRect(this.Handle, Selector.GetHandle("initWithFrame:"), frame), "initWithFrame:");
        else
            InitializeHandle(Messaging.IntPtr_objc_msgSendSuper_CGRect(this.SuperHandle, Selector.GetHandle("initWithFrame:"), frame), "initWithFrame:");
    }
    
    [Export<Method>("performAction:")]
    public void PerformAction(NSObject target)
    {
        // Method implementation with marshalling
    }
    
    [Export<Property>("title", ArgumentSemantic.Retain)]
    public NSString Title { get; set; }
}
```

**Class with Disabled Default Constructor**:
```csharp
[BindingType<Class>(Flags = Class.DisableDefaultCtor)]
public partial class SingletonClass : NSObject
{
    [Export<Property>("sharedInstance")]
    public static SingletonClass SharedInstance { get; }
}
```

### 2. Protocol Bindings

```csharp
[BindingType<Protocol>]
public interface IMyDelegate : INativeObject
{
    [Export<Method>("didFinishWithResult:")]
    void DidFinish(NSString result);
    
    [Export<Method>("shouldProcessItem:")]
    [Abstract]
    bool ShouldProcessItem(NSObject item);
    
    [Export<Method>("willBeginProcessing")]
    [Optional]
    void WillBeginProcessing();
}
```

### 3. Smart Enums

**Basic Smart Enum**:
```csharp
[BindingType<SmartEnum>]
public enum AVCaptureDeviceType
{
    [Field<EnumValue>("AVCaptureDeviceTypeBuiltInMicrophone")]
    BuiltInMicrophone,
    
    [Field<EnumValue>("AVCaptureDeviceTypeBuiltInWideAngleCamera")]
    BuiltInWideAngleCamera,
    
    [Field<EnumValue>("AVCaptureDeviceTypeExternalUnknown")]
    ExternalUnknown
}
```

**Error Code Smart Enum**:
```csharp
[BindingType<SmartEnum>(ErrorDomain = "GKErrorDomain", Flags = SmartEnum.ErrorCode)]
public enum GKError
{
    [Field<EnumValue>("GKErrorUnknown")]
    Unknown = 1,
    
    [Field<EnumValue>("GKErrorCancelled")]
    Cancelled = 2,
    
    [Field<EnumValue>("GKErrorCommunicationsFailure")]
    CommunicationsFailure = 3
}
```

**Custom Library Smart Enum**:
```csharp
[BindingType<SmartEnum>(LibraryName = "MyCustomFramework")]
public enum CustomOptions
{
    [Field<EnumValue>("kCustomOptionA", LibraryName = "MyCustomFramework")]
    OptionA,
    
    [Field<EnumValue>("kCustomOptionB", LibraryName = "MyCustomFramework")]
    OptionB
}
```

### 4. Categories

```csharp
[BindingType<Category>]
public static class NSStringExtensions
{
    [Export<Method>("reversedString")]
    public static NSString ReversedString(this NSString self)
    {
        // Extension method implementation
    }
    
    [Export<Method>("trimmedString")]
    public static NSString TrimmedString(this NSString self)
    {
        // Extension method implementation
    }
}
```

### 5. Async Methods

**Completion Handler to Task**:
```csharp
[Export<Method>("loadDataWithCompletion:", 
    Flags = Method.Async, 
    ResultTypeName = "NSDataTaskResult")]
public static void LoadData(Action<NSData, NSError> completionHandler);

// Generates:
public static Task<NSData> LoadDataAsync() { }
```

**Complex Async Pattern**:
```csharp
[Export<Method>("fetchUserWithId:completion:", 
    Flags = Method.Async,
    ResultType = typeof(User),
    MethodName = "FetchUserAsync",
    PostNonResultSnippet = "if (error != null) throw new NSErrorException(error);")]
public void FetchUser(string userId, Action<User, NSError> completion);
```

### 6. Notifications

```csharp
[BindingType<Class>]
public partial class NSUserDefaults : NSObject
{
    // Simple notification
    [Field<Property>("NSUserDefaultsSizeLimitExceededNotification", 
        Flags = Property.Notification)]
    public static partial NSString SizeLimitExceededNotification { get; }
    
    // Notification with custom args
    [Field<Property>("NSUbiquitousUserDefaultsDidChangeAccountsNotification", 
        Flags = Property.Notification, 
        Type = typeof(MyNotificationArgs))]
    public static partial NSString DidChangeAccountsNotification { get; }
    
    // Notification with custom center
    [Field<Property>("NSUbiquitousUserDefaultsNoCloudAccountNotification", 
        Flags = Property.Notification, 
        NotificationCenter = "SharedWorkspace.NotificationCenter")]
    public static partial NSString NoCloudAccountNotification { get; }
}
```

### 7. Strong Dictionaries

```csharp
[BindingType<StrongDictionary>]
public partial class CGImageProperties
{
    public int? DPIWidth { get; set; }
    public int? DPIHeight { get; set; }
    public CGImageOrientation? Orientation { get; set; }
    public bool? IsFloat { get; set; }
    public bool? IsIndexed { get; set; }
    public bool? HasAlpha { get; set; }
}
```

### 8. Core Image Filters

```csharp
[BindingType<CoreImageFilter>(
    DefaultCtorVisibility = MethodAttributes.Public,
    IntPtrCtorVisibility = MethodAttributes.Family,
    StringCtorVisibility = MethodAttributes.Public)]
public partial class CIGaussianBlur : CIFilter
{
    [CoreImageFilterProperty("inputImage")]
    public CIImage InputImage { get; set; }
    
    [CoreImageFilterProperty("inputRadius")]
    public float Radius { get; set; }
}
```

## Attribute System

### Core Binding Attributes

#### BindingTypeAttribute<T>
**Purpose**: Marks types for binding generation

**Type Parameters**:
- `Class`: Objective-C classes
- `Protocol`: Objective-C protocols  
- `Category`: Extension methods
- `StrongDictionary`: Typed dictionaries
- `CoreImageFilter`: Core Image filters
- `SmartEnum`: String-backed enums

**Parameters**:
- `Name` (string): Objective-C name (default: C# name)
- `Flags` (T): Type-specific flags
- `ErrorDomain` (string): For error enums
- `LibraryName` (string): Custom library
- `DefaultCtorVisibility`: For Core Image filters
- `IntPtrCtorVisibility`: For Core Image filters
- `StringCtorVisibility`: For Core Image filters

#### ExportAttribute<T>
**Purpose**: Exports members to Objective-C

**Type Parameters**:
- `Method`: Instance/static methods
- `Property`: Properties
- `Constructor`: Constructors

**Parameters**:
- `Selector` (string): Objective-C selector
- `ArgumentSemantic`: Copy/Retain/Assign
- `Flags` (T): Member-specific flags
- `NativePrefix/Suffix`: Custom marshaling
- `Library`: Custom marshaling library
- `ResultType`: Async result type
- `MethodName`: Async method name
- `ResultTypeName`: Async result type name
- `PostNonResultSnippet`: Async error handling
- `StrongDelegateType`: For weak delegates

#### FieldAttribute<T>
**Purpose**: Native field bindings

**Type Parameters**:
- `Property`: Notification properties
- `EnumValue`: Enum values

**Parameters**:
- `SymbolName` (string): Native symbol
- `LibraryName` (string): Containing library
- `Flags` (T): Field-specific flags
- `Type` (Type): Notification arg type
- `NotificationCenter` (string): Custom center

### Flag Enumerations

#### Class Flags
```csharp
[Flags]
public enum Class
{
    Default = 0,
    DisableDefaultCtor = 1 << 2,
    IsThreadSafe = 1 << 3
}
```

#### Method Flags  
```csharp
[Flags]
public enum Method
{
    Default = 0,
    IsVariadic = 1 << 2,
    IgnoredInDelegate = 1 << 3,
    MarshalNativeExceptions = 1 << 4,
    CustomMarshalDirective = 1 << 5,
    IsThreadSafe = 1 << 6,
    PlainString = 1 << 7,
    AutoRelease = 1 << 8,
    RetainReturnValue = 1 << 9,
    ReleaseReturnValue = 1 << 10,
    Proxy = 1 << 11,
    Factory = 1 << 12,
    Async = 1 << 13
}
```

#### Property Flags
```csharp
[Flags]
public enum Property
{
    Default = 0,
    IsThreadStatic = 1 << 2,
    Notification = 1 << 3,
    MarshalNativeExceptions = 1 << 4,
    CustomMarshalDirective = 1 << 5,
    DisableZeroCopy = 1 << 6,
    IsThreadSafe = 1 << 7,
    Transient = 1 << 8,
    PlainString = 1 << 9,
    CoreImageFilterProperty = 1 << 10,
    AutoRelease = 1 << 11,
    RetainReturnValue = 1 << 12,
    ReleaseReturnValue = 1 << 13,
    Proxy = 1 << 14,
    WeakDelegate = 1 << 15
}
```

#### SmartEnum Flags
```csharp
[Flags]
public enum SmartEnum
{
    Default = 0,
    ErrorCode = 1 << 2
}
```

### Supporting Attributes

#### ForcedTypeAttribute
Forces specific managed type creation
- `Owns` (bool): Object ownership

#### BindAsAttribute
Type conversion for NSNumber/NSValue/NSString
- `Type` (Type): Target managed type
- `OriginalType` (Type): Source ObjC type

#### Platform Availability
- `SupportedOSPlatformAttribute`
- `UnsupportedOSPlatformAttribute`
- `ObsoletedOSPlatformAttribute`

## Diagnostics and Analysis

### Diagnostic Reference

| ID | Severity | Category | Description |
|---|---|---|---|
| RBI0000 | Error | Usage | Unexpected error during processing |
| RBI0001 | Error | Usage | Binding types must be partial |
| RBI0002 | Error | Usage | BindingType<Class> must be on class |
| RBI0003 | Error | Usage | BindingType<Category> must be on class |
| RBI0004 | Error | Usage | Categories must be static |
| RBI0005 | Error | Usage | BindingType<Protocol> must be on interface |
| RBI0006 | Error | Usage | BindingType must be on enum |
| RBI0007 | Error | Usage | BindingType<StrongDictionary> must be on class |
| RBI0008 | Error | Usage | Smart enum values need backing field |
| RBI0009 | Error | Usage | Duplicate backing fields |
| RBI0010 | Error | Usage | Invalid backing field identifier |
| RBI0011 | Error | Usage | Non-Apple frameworks need library name |
| RBI0012 | Warning | Usage | Apple frameworks shouldn't specify library |
| RBI0013 | Error | Usage | Wrong attribute type on enum |
| RBI0014 | Warning | Usage | Native handle access needs GC.KeepAlive |

### Diagnostic Messages

```csharp
// RBI0001
"The binding type '{0}' must be declared partial"

// RBI0008  
"The enum value '{0}' must be tagged with a Field<EnumValue> attribute"

// RBI0009
"The backing field '{0}' for the enum value '{1}' is already in use for the enum value '{2}'"

// RBI0011
"The field attribute for the enum value '{0}' must set the property 'LibraryName'"

// RBI0014
"Variable '{0}' has its Handle property accessed but is not kept alive later in the method"
```

### Code Fix Providers

1. **Partial Modifier Fix**
   - Adds `partial` to binding types
   - Preserves other modifiers

2. **Smart Enum Field Fix**
   - Generates Field<EnumValue> attributes
   - Infers backing field names

3. **GC.KeepAlive Fix**
   - Inserts GC.KeepAlive calls
   - Places after native handle usage

4. **Library Name Fix**
   - Formats library names correctly
   - Removes from Apple frameworks

## Incremental Generation

### Equality Comparison Strategy

The incremental generator uses custom equality comparers to minimize regeneration:

```csharp
class BindingEqualityComparer : EqualityComparer<Binding>
{
    public override bool Equals(Binding x, Binding y)
    {
        // Compare all relevant properties
        if (x.Name != y.Name) return false;
        if (x.FullyQualifiedSymbol != y.FullyQualifiedSymbol) return false;
        if (x.BindingType != y.BindingType) return false;
        
        // Use specialized comparers for collections
        var attrComparer = new AttributesEqualityComparer();
        if (!attrComparer.Equals(x.Attributes, y.Attributes)) return false;
        
        var propertyComparer = new PropertiesEqualityComparer();
        if (!propertyComparer.Equals(x.Properties, y.Properties)) return false;
        
        // ... more comparisons
        return true;
    }
    
    public override int GetHashCode(Binding obj)
    {
        return HashCode.Combine(obj.FullyQualifiedSymbol, obj.EnumMembers);
    }
}
```

### Pipeline Optimization

```csharp
// Separate providers for different outputs
var bindings = provider
    .Select((tuple, _) => (tuple.RootBindingContext, tuple.Bindings))
    .WithComparer(equalityComparer);

var libraryProvider = provider
    .Select((tuple, _) => (tuple.RootBindingContext, tuple.Bindings.LibraryPaths));

var trampolineProvider = provider
    .Select((tuple, _) => (tuple.RootBindingContext, tuple.Bindings.Trampolines));

// Register separate outputs to minimize regeneration
context.RegisterSourceOutput(bindings.Collect(), GenerateCode);
context.RegisterSourceOutput(libraryProvider.Collect(), GenerateLibraryCode);
context.RegisterSourceOutput(trampolineProvider.Collect(), GenerateTrampolineCode);
```

### Caching Strategy

1. **Immutable Data Models**: All binding data is immutable
2. **Structural Equality**: Deep comparison of binding structures
3. **Selective Updates**: Only changed bindings regenerate
4. **Shared Context**: RootContext carries compilation-wide data

## Migration and Transformation

### Transformation Architecture

```
Legacy Bindings → Parse → Transform → Validate → Generate
     ↓              ↓         ↓          ↓          ↓
RSP Files    Roslyn AST  Data Model  Analyzer  New Format
```

### Channel-Based Processing

```csharp
// Parallel transformation using channels
var channel = Channel.CreateUnbounded<TransformTask>();

// Producer
await foreach (var file in GetSourceFiles())
{
    await channel.Writer.WriteAsync(new TransformTask(file));
}

// Consumers (parallel)
await Task.WhenAll(
    Enumerable.Range(0, Environment.ProcessorCount)
        .Select(_ => ProcessTransformations(channel.Reader))
);
```

### Transformation Rules

1. **Attribute Migration**
   ```csharp
   // Legacy
   [BaseType(typeof(NSObject))]
   interface MyClass { }
   
   // New
   [BindingType<Class>]
   public partial class MyClass : NSObject { }
   ```

2. **Async Method Detection**
   ```csharp
   // Legacy
   [Export("loadWithCompletion:")]
   void Load(Action<NSData, NSError> completion);
   
   // New
   [Export<Method>("loadWithCompletion:", Flags = Method.Async)]
   public void Load(Action<NSData, NSError> completion);
   ```

3. **Smart Enum Transformation**
   ```csharp
   // Legacy
   [Field("AVMediaTypeVideo", "AVFoundation")]
   NSString AVMediaTypeVideo { get; }
   
   // New
   [Field<EnumValue>("AVMediaTypeVideo")]
   Video,
   ```

### Platform-Specific Handling

- Processes multiple RSP files for different platforms
- Preserves platform availability attributes
- Handles conditional compilation directives
- Maintains platform-specific code paths

## Error Handling

### TryEmit Pattern

All emitters implement a consistent error handling pattern:

```csharp
public bool TryEmit(in BindingContext context, 
    [NotNullWhen(false)] out ImmutableArray<Diagnostic>? diagnostics)
{
    var diags = ImmutableArray.CreateBuilder<Diagnostic>();
    
    try 
    {
        // Validation
        if (!ValidateBinding(context.Binding))
        {
            diags.Add(Diagnostic.Create(descriptor, location));
            diagnostics = diags.ToImmutable();
            return false;
        }
        
        // Generation
        EmitCode(context);
        diagnostics = null;
        return true;
    }
    catch (Exception ex)
    {
        diags.Add(Diagnostic.Create(
            Diagnostics.RBI0000, 
            null, 
            context.Binding.FullyQualifiedSymbol));
        diagnostics = diags.ToImmutable();
        return false;
    }
}
```

### Exception Marshalling

**Objective-C Exception Handling**:
```csharp
[Export<Method>("riskyOperation", Flags = Method.MarshalNativeExceptions)]
public void RiskyOperation()
{
    // Generated code includes exception marshalling
    IntPtr exception_gchandle = IntPtr.Zero;
    try {
        Messaging.void_objc_msgSend_ref_IntPtr(Handle, Selector.GetHandle("riskyOperation"), ref exception_gchandle);
    } finally {
        if (exception_gchandle != IntPtr.Zero) {
            var exception = GCHandle.FromIntPtr(exception_gchandle).Target as Exception;
            GCHandle.FromIntPtr(exception_gchandle).Free();
            throw exception!;
        }
    }
}
```

### Edge Cases

1. **Null Handling**: Comprehensive null checks with proper default values
2. **Generic Types**: Special handling for generic type arguments
3. **Variadic Methods**: IsVariadic flag prevents invalid generation
4. **Circular Dependencies**: Detection in type resolution
5. **Name Conflicts**: Nomenclator ensures unique names

## Testing Strategy

### Test Structure

```
tests/rgen/Microsoft.Macios.Generator.Tests/
├── BaseGeneratorTestClass.cs      # Test infrastructure
├── BaseTestDataGenerator.cs       # Platform test data
├── Classes/                       # Class binding tests
│   └── Data/                     # Test input/expected output
├── SmartEnum/                    # Smart enum tests
│   └── Data/
├── Attributes/                   # Attribute parsing tests
├── DataModel/                    # Data model tests
├── Emitters/                     # Emitter tests
├── Extensions/                   # Extension method tests
└── Formatters/                   # Formatter tests
```

### Test Patterns

**Platform Testing**:
```csharp
[Theory]
[AllSupportedPlatforms]
public void TestMethod(ApplePlatform platform)
{
    var (compilation, syntaxTrees) = CreateCompilation(platform, sources: code);
    var driver = GeneratorDriver.Create(generator).RunGenerators(compilation);
    
    // Verify generated code
    var result = driver.GetRunResult();
    Assert.True(result.Diagnostics.IsEmpty);
    
    // Compare with expected output
    var generated = result.GeneratedTrees.Single(t => t.FilePath.EndsWith("MyClass.g.cs"));
    var expected = GetExpectedOutput(platform, "MyClass");
    Assert.Equal(expected, generated.GetText().ToString());
}
```

**Data-Driven Tests**:
```csharp
public class BaseTestDataGenerator : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return new object[] { ApplePlatform.iOS, "15.0" };
        yield return new object[] { ApplePlatform.TVOS, "15.0" };
        yield return new object[] { ApplePlatform.MacOSX, "12.0" };
        yield return new object[] { ApplePlatform.MacCatalyst, "15.0" };
    }
}
```

### Test Categories

1. **Unit Tests**
   - Data model validation
   - Attribute parsing
   - Type marshalling
   - Nomenclator uniqueness

2. **Integration Tests**
   - End-to-end generation
   - Multi-file scenarios
   - Platform combinations
   - Incremental compilation

3. **Analyzer Tests**
   - Diagnostic accuracy
   - Code fix validation
   - Edge case handling

4. **Performance Tests**
   - Generation benchmarks
   - Memory usage
   - Incremental performance

## Configuration and Deployment

### Build Configuration

```xml
<PropertyGroup>
    <TargetFramework>net$(BundledNETCoreAppTargetFrameworkVersion)</TargetFramework>
    <LangVersion>latest</LangVersion>
    <Nullable>enable</Nullable>
    <TreatWarningsAsErrors>true</TreatWarningsAsErrors>
    <IsRoslynComponent>true</IsRoslynComponent>
    <EnforceExtendedAnalyzerRules>true</EnforceExtendedAnalyzerRules>
    <EmitCompilerGeneratedFiles>true</EmitCompilerGeneratedFiles>
    <CompilerGeneratedFilesOutputPath>$(IntermediateOutputPath)Generated</CompilerGeneratedFilesOutputPath>
</PropertyGroup>

<!-- Suppressed warnings -->
<PropertyGroup>
    <NoWarn>$(NoWarn);RS2007;RS1041;RS1038;APL0003</NoWarn>
</PropertyGroup>
```

### Package Structure

```
Microsoft.Macios.Generator.nupkg
├── analyzers/
│   └── dotnet/cs/
│       ├── Microsoft.Macios.Generator.dll
│       ├── Microsoft.Macios.Bindings.Analyzer.dll
│       └── Microsoft.Macios.Binding.Common.dll
├── tools/
│   └── net8.0/
│       ├── Microsoft.Macios.Transformer.exe
│       └── dependencies/
└── build/
    └── Microsoft.Macios.Generator.props
```

### MSBuild Integration

```xml
<!-- In consuming project -->
<ItemGroup>
    <PackageReference Include="Microsoft.Macios.Generator" 
                      Version="1.0.0" 
                      PrivateAssets="all"
                      IncludeAssets="runtime; build; native; contentfiles; analyzers" />
</ItemGroup>

<!-- Automatic configuration -->
<PropertyGroup>
    <EmitCompilerGeneratedFiles>true</EmitCompilerGeneratedFiles>
    <CompilerGeneratedFilesOutputPath>$(IntermediateOutputPath)Generated</CompilerGeneratedFilesOutputPath>
</PropertyGroup>
```

### Global Configuration

```csharp
// Configuration.cs
public static class Configuration
{
    public const bool UseGlobalNamespace = true;
    
    // Attribute names for identification
    public const string GeneratedCodeAttribute = "GeneratedCode";
    public const string CompilerGeneratedAttribute = "CompilerGenerated";
    public const string EditorBrowsableAttribute = "EditorBrowsable";
    public const string DebuggerBrowsableAttribute = "DebuggerBrowsable";
}
```

### Environment Variables

- `MACIOS_GENERATOR_VERBOSE`: Enable verbose logging
- `MACIOS_GENERATOR_DEBUG`: Enable debug output
- `MACIOS_GENERATOR_PARALLEL`: Control parallelism

## API Reference

### Generator Interfaces

```csharp
public interface ICodeEmitter
{
    string GetSymbolName(in Binding binding);
    bool TryEmit(in BindingContext bindingContext, 
        [NotNullWhen(false)] out ImmutableArray<Diagnostic>? diagnostics);
    IEnumerable<string> UsingStatements { get; }
}
```

### Context Types

```csharp
public class RootContext
{
    public SemanticModel SemanticModel { get; }
    public Compilation Compilation { get; }
    public ImmutableHashSet<string> UINamespaces { get; }
    public ApplePlatform Platform { get; }
}

public readonly struct BindingContext
{
    public RootContext RootContext { get; }
    public TabbedStringBuilder StringBuilder { get; }
    public Binding Changes { get; }
    public bool NeedsThreadChecks { get; }
}
```

### Syntax Factory Methods

```csharp
public static class BindingSyntaxFactory
{
    // Object lifecycle
    public static ExpressionSyntax KeepAlive(string variableName);
    public static LocalDeclarationStatementSyntax GetAutoreleasePoolVariable();
    
    // String marshalling
    public static InvocationExpressionSyntax StringCreateNative(ImmutableArray<ArgumentSyntax> arguments);
    public static InvocationExpressionSyntax StringReleaseNative(ImmutableArray<ArgumentSyntax> arguments);
    
    // Handle management
    public static LocalDeclarationStatementSyntax? GetHandleAuxVariable(string parameterName, in TypeInfo parameterType);
    
    // Thread safety
    public static ExpressionStatementSyntax? EnsureUiThread(PlatformName platform);
}
```

### Formatters

```csharp
public static class MethodFormatter
{
    public static string ToDeclaration(in Method method);
    public static string ToSignature(in Method method);
}

public static class PropertyFormatter
{
    public static string ToDeclaration(in Property property);
    public static string GetBackingFieldName(in Property property);
}
```

## Implementation Details

### Nomenclator System

The Nomenclator ensures unique and consistent naming:

```csharp
public static class Nomenclator
{
    // Trampoline naming
    public static string GetTrampolineName(in TypeInfo typeInfo)
    {
        if (typeInfo.IsGenericType && typeInfo.TypeArguments.Length > 0)
            return $"{typeInfo.Name}Arity{typeInfo.TypeArguments.Length}";
        return typeInfo.Name;
    }
    
    // Variable naming based on type
    public static string GetNameForVariableType(TypeNameHint hint, in TypeInfo type)
    {
        return hint switch
        {
            TypeNameHint.NSObject or TypeNameHint.NativeObject => type.IsNullable ? $"__{type.Name}_var__" : type.Name.ToLowerInvariant(),
            TypeNameHint.NativeHandle or TypeNameHint.IntPtr => $"{type.Name}__handle__",
            TypeNameHint.BlockLiteral => $"block_ptr_{type.Name}",
            TypeNameHint.NSArray => $"nsa_{type.Name}",
            TypeNameHint.NSString => $"ns{type.Name}",
            TypeNameHint.BindFrom => $"nsb_{type.Name}",
            _ => type.Name.ToLowerInvariant()
        };
    }
    
    // Property backing fields
    public static string GetPropertyBackingFieldName(string propertyName, bool isStatic)
    {
        return isStatic ? $"__mt_{propertyName}_var__static" : $"__mt_{propertyName}_var";
    }
}
```

### Message Send Patterns

```csharp
// Direct binding
if (IsDirectBinding) {
    ret = Runtime.GetNSObject(Messaging.IntPtr_objc_msgSend(this.Handle, selHandle));
} else {
    ret = Runtime.GetNSObject(Messaging.IntPtr_objc_msgSendSuper(this.SuperHandle, selHandle));
}
```

### Selector Caching

```csharp
// Cached selector handles
static readonly IntPtr selInitWithFrame_Handle = Selector.GetHandle("initWithFrame:");
static readonly IntPtr selTitle_Handle = Selector.GetHandle("title");
static readonly IntPtr selSetTitle_Handle = Selector.GetHandle("setTitle:");
```

### Library Loading

```csharp
// System libraries
static class Libraries
{
    static public class Foundation
    {
        static public readonly IntPtr Handle = Dlfcn._dlopen(Constants.FoundationLibrary, 0);
    }
    
    static public class UIKit  
    {
        static public readonly IntPtr Handle = Dlfcn._dlopen(Constants.UIKitLibrary, 0);
    }
    
    // Custom framework
    static public class MyFramework
    {
        static public readonly IntPtr Handle = Dlfcn.dlopen("/path/to/MyFramework.framework/MyFramework", 0);
    }
}
```

## Performance Considerations

### Optimization Strategies

1. **Struct-based Data Models**: Zero-allocation for binding data
2. **Immutable Collections**: Thread-safe without locks
3. **Indexed Lookups**: O(1) selector-to-member mapping
4. **Incremental Generation**: Only regenerate changed bindings
5. **Parallel Processing**: Multi-threaded transformation
6. **String Interning**: Reuse common strings
7. **Lazy Initialization**: Defer expensive operations

### Memory Efficiency

```csharp
// Property backing field pattern (lazy + cached)
NSString? __mt_Title_var;
public NSString Title {
    get {
        if (__mt_Title_var is null)
            __mt_Title_var = GetNSObject<NSString>(/* native call */);
        return __mt_Title_var;
    }
}
```

### Compilation Performance

- **Provider Separation**: Independent pipelines for libraries/trampolines
- **Early Filtering**: IsValidNode predicate reduces processing
- **Minimal Comparisons**: Efficient equality comparers
- **Batch Operations**: Collect before generation

## Future Considerations

### Planned Enhancements

1. **Performance Optimizations**
   - Roslyn fork-based optimization
   - Span<T> usage in hot paths
   - More aggressive caching
   - Parallel emitter execution

2. **Feature Additions**
   - Swift interop exploration
   - SwiftUI binding patterns
   - Async enumerable support
   - Custom marshaller attributes
   - Generic constraint handling
   - Function pointer support

3. **Tooling Improvements**
   - Visual binding designer
   - Migration wizard UI
   - Performance profiler
   - Binding compatibility analyzer
   - API diff tools

4. **Platform Expansion**
   - VisionOS support
   - Catalyst optimizations
   - Cross-platform abstractions

### Technical Debt

1. **Code Consolidation**
   - Merge duplicate extensions
   - Unify data models
   - Standardize formatters

2. **Test Coverage**
   - Stress tests for large bindings
   - Fuzzing for edge cases
   - Performance regression tests

3. **Documentation**
   - API reference generation
   - Video tutorials
   - Migration guides
   - Architecture deep dives

### Research Areas

1. **Source Generator v2**: Next generation Roslyn APIs
2. **AI-Assisted Binding**: ML-based attribute inference
3. **Cross-Language Interop**: Beyond Objective-C
4. **Hot Reload Support**: Runtime binding updates

---

*Version 2.0 - Comprehensive Specification*
*Last Updated: Current*
*Status: Living Document*

This specification represents a comprehensive analysis of the rgen project based on thorough code examination. It serves as the authoritative reference for understanding, implementing, and extending the modern macOS/iOS binding generator.