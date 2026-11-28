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

Native diagnostics
==================

Use `PRINT`/`LOG` for normal runtime logging. For diagnostics that must preserve
stdout/stderr output or cannot enter the Objective-C runtime, use
`xamarin_write_formatted` from the internal `native-support.h` header. It uses
libc formatting and descriptor writes, checks failures, and handles interrupted
and partial writes without calling Cocoa. Pass trusted format strings and supply
external text as arguments, not as the format.

The low-level writer still allocates and uses standard-stream locks; it is not
an async-signal-safe logger. Do not replace weak-reference/finalizer diagnostics
with `NSLog`, `os_log`, or `PRINT`, which can re-enter Objective-C while its
internal locks are held.

Runtime failures
================

When managed code is available, report failures as managed exceptions rather
than assertions. Trampolines must release their temporary storage before passing
the exception to the existing exception-marshaling path. Diagnostic helpers should
return whatever information is available instead of terminating the process.

The registration map, command-line arguments, environment snapshots, assembly
locations, and host runtime contract are also used before the managed runtime
is initialized. Their fatal validation and allocation failures cannot rely on
managed exceptions.
