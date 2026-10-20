## Struct pointer parameters

Use `[PointerAsStruct]` on a `T*`, `ref T`, or `out T` parameter to describe a
pointer to one blittable struct. Existing raw pointer and by-reference bindings
continue to work without this attribute. `[NullAllowed]` permits a null raw pointer;
`ref` and `out` always refer to a struct.

Use `[PointerAsArray]` on a one-dimensional `T[]` parameter to pass a contiguous
buffer of blittable structs instead of an `NSArray`:

```csharp
[Export ("appendPoints:count:")]
void AppendPoints (
    [PointerAsArray (LengthFromParameter = nameof (count))] CGPoint [] points,
    nint count);
```

Specify exactly one length source:

* `LengthFromParameter`: the name of an integral, by-value parameter.
* `LengthFromMember`: the name of a readable integral property or field. Static
  methods require a static member. Instance members are not available in
  constructors, and member lengths are not supported on categories.
* `Length`: a nonnegative constant number of elements.

The generated wrapper checks the length before calling native code: negative
counts and counts larger than the managed array are rejected with
`ArgumentOutOfRangeException`. A prefix of an array is valid; equality with the
array length is not required. Required arrays are null-checked. A `[NullAllowed]`
array may be null only when the effective length is zero. Empty arrays are passed
as null pointers.

The array is pinned for the duration of the call, so native writes are visible
in the managed array. Native code must not retain the pointer after returning.
This attribute is for synchronous outgoing Objective-C methods and constructors:
array methods are generated as non-virtual methods without callback exports.
Protocols, models, delegates, property accessors, abstract/override methods and
`[Async]` methods are diagnosed rather than given incorrect callback marshalling.
Use raw pointers and manual marshalling for those cases.

The element must be a non-generic struct with sequential or explicit layout and
blittable fields. Managed references, `bool`, `char`, native-sized enums and
fields requiring `[MarshalAs]` are not supported. Pointer-to-pointer parameters,
`in` parameters, array return values, sentinel-terminated buffers, expressions as
length sources, and lengths produced by an `out`/`ref` parameter are not supported
by these attributes.

## Debugging


One of the main problems when adding changes to the generator was that before the generator was moved to dotnet, we could not attach the dotnet debugger to it. 
Things have changed, and now if you want to debug the generator, you can do it with Visual Studio. To do so, you need to follow these steps:

1. Export the environment variable `XAMMACIOS_DEBUGGER` with any value. For example:

    ```bash
    export XAMMACIOS_DEBUGGER=1
    ```

2. Open the src/src/bgen.csproj solution with Visual Studio.
3. Add a break point at the end of the `Main` method in the BindingTouch.cs file.
4. Rebuild the generator.


Now, when the project is rebuilt you will have to pay attention to the output of the build. You will see something like this:

```bash
Waiting for debugger to attach: (55644) dotnet
Waiting for debugger to attach: (55646) dotnet
```

These messages are telling you that the generator is waiting for the debugger to be attach to the process. To do so, you need to go to the Debug menu and select 
the Attach to Process option. In the dialog that will appear, you will need to select the dotnet process that is running the generator. In this case, the process
is the one with the PID 55644. Once you have selected the process, you can click on the Attach button.

Because the generator is called several times, one per platform, you can attach to the different processes in case the issue you are trying to debug
is platform specific.
