// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.Runtime.Versioning;
using CoreLocation;
using Foundation;
using ObjCBindings;
using ObjCRuntime;

namespace TestNamespace;

[BindingType<Class>]
public class CoreLocationTrampolines {

	[Export ("clBackgroundActivitySessionCreateHandler", ArgumentSemantic.Copy)]
	public partial CoreLocation.CLBackgroundActivitySessionCreateHandler CLBackgroundActivitySessionCreateHandler { get; set; }

	[Export ("clGeocodeCompletionHandler", ArgumentSemantic.Copy)]
	public partial CoreLocation.CLGeocodeCompletionHandler CLGeocodeCompletionHandler { get; set; }

	[Export ("clServiceSessionCreateHandler", ArgumentSemantic.Copy)]
	public partial CoreLocation.CLServiceSessionCreateHandler CLServiceSessionCreateHandler { get; set; }
}
