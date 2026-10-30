Native runtime
==============

This directory contains the native runtime for .NET for iOS, tvOS, Mac Catalyst and macOS.

Generated code
==============

A few files are generated automatically:

delegates.h|inc, Delegates.cs
-----------------------------

These files contain the code required to glue the native
runtime and the managed runtime.

xamarin/mono-runtime.h
----------------------

This header provides Mono-named compatibility types and function declarations
used by the CoreCLR and NativeAOT native bridges. It is generated from
mono-runtime.h.t4 and exports.t4; it does not load or embed MonoVM.

If a bridge needs another compatibility function, add it to exports.t4.
If it needs another compatibility type or constant, add it to mono-runtime.h.t4.

Shipped headers
===============

These headers ship with the native runtime packages. They are not public APIs,
but are needed to build generated application startup and registrar code.

xamarin/runtime.h
xamarin/trampolines.h
xamarin/main.h
xamarin/xamarin.h