# Objective-C classes

Objective-C classes can be referenced from managed code in several ways:

* Calls to Class.GetHandle / GetHandleIntrinsic

It's highly desirable to use a direct native reference to Objective-C classes when building a mobile app, for a few reasons:

* It's faster at runtime, and the app is smaller.
* If the referenced Objective-C class comes from a third-party static library, the
  native linker can remove it if it's configured to remove unused code
  (because the native linker can't see that the class is in fact used
  at runtime) unless there's a direct native reference to the class.

On the other hand there's one scenario when a direct native reference is not desirable: when the native Objective-C class does not exist.

This can happen for third-party bindings: for instance a binding might declare a
`[BaseType (typeof (NSObject))]` type for something that is a protocol - and not a class -
natively, in which case there's no native Objective-C class to reference. A direct native
reference to such a class would turn into a hard link error (`Undefined symbols:
_OBJC_CLASS_$_TheClass`), even if the class is never actually used at runtime.

To avoid this, we only emit a direct native reference for classes we can prove exist: classes
that belong to a known platform (SDK) framework. For any other class (a class from a
third-party binding, or a class we couldn't find a managed type for) we can't prove the native
class exists, so we:

* tell the native linker that the corresponding `_OBJC_CLASS_$_*` symbol is allowed to be
  undefined (by passing `-U` to the native linker), so that a missing native class doesn't
  break the link, and
* fall back to `objc_getClass` at runtime if the direct reference turns out to be null.

This mirrors the weak + `dlsym` fallback we do for inlined `dlfcn` symbols (see
[native-symbols.md](native-symbols.md)).

In order to create a direct native reference to Objective-C classes, we need to know the names of those Objective-C classes.

## The `InlineClassGetHandle` property

This behavior is controlled by the `InlineClassGetHandle` MSBuild property, which
can either be enabled or disabled.

See the [build properties documentation](../building-apps/build-properties.md) for default values.

## How it works

During the build we try to collect the following:

* Any calls to `Class.GetHandle[Intrinsic]` APIs: we try to collect the class name (this might not always succeed, if the class name is not a constant).

This is further complicated by the fact that we only want to create native
references for managed references that survive trimming.

So we do the following:

1. During trimming, two custom linker steps execute:

	* `InlineClassGetHandleStep`: for every call `Class.GetHandle` we've
	  collected, this step creates a P/Invoke to a native method that will
	  return the Objective-C class for that symbol (using a direct native
	  reference), and modifies the code that fetches that symbol to call said
	  P/Invoke.

2. After trimming, we figure out which of those symbols survived:

	* For ILTrim: the `_CollectPostILTrimInformation` MSBuild target inspects
	  the trimmed assemblies and collects all the inlined P/Invokes that
	  survived. Per-assembly results are cached to speed up incremental builds.
	* For NativeAOT: the `_CollectPostNativeAOTTrimInformation` MSBuild target
	  inspects the native object file (or static library) produced by NativeAOT,
	  collects all unresolved native references, and filters them against the
	  Objective-C classes to determine which survived.

3. The `_PostTrimmingProcessing` MSBuild target takes the surviving symbols
   from either path, generates the corresponding native Objective-C code, and
   adds it to the list of files to compile and link into the final executable.
