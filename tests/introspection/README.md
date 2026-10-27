# Introspection Tests

Introspection tests are executed on specific target (both simulator and device
for iOS and tvOS) or a specific version of macOS or Mac Catalyst. The
application proceed to analyze itself using:

* `System.Reflection` for managed code; and
* the Objective-C runtime library for native code

and compare the results. E.g. if using .NET reflection it can see a binding
for a `NSBundle` type then it should be able to find a native `NSBundle` 
type using the ObjC runtime functions. Otherwise an error is raised...

Since the application analyze itself it must contains everything we wish
to test. That's why the introspection tests needs to be built with the
managed linker disabled, i.e. **"Don't link"**.

Pros

* The tests always tell the truth, which can differ from documentation or header files;

Cons

* Incomplete - Not everything is encoded in the metadata / executable;
* Too complete - Not every truth is good to be known (or published), which requires creating special cases in the tests

## Structure sizes

`ApiStructSizeTest` compares the marshaled size of managed structures with native
`sizeof` values compiled against the current SDK for the target platform and
architecture. The tests do not instantiate native objects, so they can also
validate structures introduced in newer OS versions than the test device runs.

The structure list is in `WriteStructSizeTests` in
`tests/test-libraries/testgenerator.cs`. Add entries there to generate both a
strongly typed test and a native size lookup. Use `ManagedName:NativeName` when
the names differ, and matching managed/native platform conditions for types
that are not available on every platform. When adding a framework, import its
native headers in `tests/test-libraries/libstructsizes.m`. Only include structures
whose managed representation is intended to match the native layout, not managed
wrappers that require custom conversion.

`AllStructsAreListed` uses reflection to enumerate all structures in the platform
assembly, including non-public and nested types, and compares them with the
generated catalog. Any new structure must have either a size test (`Add`) or an
exact-name exclusion with a reason (`Exclude`) in `WriteStructSizeTests`.
Exclusions account for managed wrappers, custom marshaling, implementation
details, and types unavailable in the native SDK. Compiler-generated types
(including types nested in compiler-generated types), enums, and primitives are
not considered structure bindings. A structure cannot be both tested and excluded.

The `CVTime` size test is explicitly ignored: its public `TimeScale` field is
currently a `long`, while the native field is `int32_t`. Correcting that existing
layout mismatch requires an API-breaking change. `AURenderEventHeader` and
`AUParameterEvent` also have existing packing mismatches, and `AURenderEvent`
additionally omits native MIDI union variants. Their tests are explicitly ignored
without changing the public layouts. Known failures use an explicit
`ignoreReason` in the generator rather than changing the expected native size.

Build `tests/test-libraries` before building introspection to generate the tests
and the `libstructsizes.xcframework` native reference.
