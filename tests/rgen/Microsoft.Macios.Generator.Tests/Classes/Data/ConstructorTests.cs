// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

#pragma warning disable APL0003

using System;
using System.Runtime.Versioning;
using AVFoundation;
using CoreGraphics;
using Foundation;
using ObjCBindings;
using ObjCRuntime;
using nfloat = System.Runtime.InteropServices.NFloat;

namespace Microsoft.Macios.Generator.Tests.Classes.Data;

[BindingType<Class>]
public class ConstructorTests {

	[Export<Constructor> ("initWithScheme:host:path:")]
	public ConstructorTests (string scheme, string host, string path);

	[Export<Constructor> ("initFileURLWithPath:isDirectory:",
		Flags = Constructor.DesignatedInitializer)]
	public ConstructorTests (string path, bool isDir) { }

	[Export<Constructor> ("initWithString:")]
	public ConstructorTests (string urlString) { }

	[Export<Constructor> ("initWithString:relativeToURL:",
		Flags = Constructor.DesignatedInitializer)]
	public ConstructorTests (string urlString, NSUrl relativeToUrl) { }
}
