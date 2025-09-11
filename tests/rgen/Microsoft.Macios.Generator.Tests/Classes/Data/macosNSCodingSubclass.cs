// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.Runtime.Versioning;
using AVFoundation;
using CoreGraphics;
using Foundation;
using ObjCBindings;
using ObjCRuntime;
using nfloat = System.Runtime.InteropServices.NFloat;

namespace AppKit;

[SupportedOSPlatform ("macos")]
[SupportedOSPlatform ("ios")]
[SupportedOSPlatform ("tvos")]
[SupportedOSPlatform ("maccatalyst13.1")]
[BindingType<Class>]
public partial class NSCodingSubclass : NSCoding {
	// nothing to add we want to test that the generator adds the ctor
}
