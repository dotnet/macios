// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

// Compile a separate entry point so unused tracking adds no work, even in debug builds.
#define XAMARIN_TRACK_INVOKED_SELECTOR
#define xamarin_invoke_trampoline xamarin_invoke_trampoline_tracking
#define xamarin_ctor_trampoline xamarin_ctor_trampoline_with_selector
#include "trampolines-invoke.m"
