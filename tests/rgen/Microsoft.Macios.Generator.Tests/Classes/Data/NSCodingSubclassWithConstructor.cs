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

namespace UIKit;

[SupportedOSPlatform ("macos")]
[SupportedOSPlatform ("ios")]
[SupportedOSPlatform ("tvos")]
[SupportedOSPlatform ("maccatalyst13.1")]
[BindingType<Class>]
public partial class NSCodingSubclassWithConstructor : NSCoding {
	
	// ensure that we do not have issues if it was already defined
	[Export<Constructor> ("initWithCoder:",
		Flags = Constructor.DesignatedInitializer)]
	public NSCodingSubclassWithConstructor (NSCoder coder);
}
