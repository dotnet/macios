// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.Runtime.Versioning;
using ARKit;
using Foundation;
using ObjCBindings;
using ObjCRuntime;

namespace TestNamespace;

[BindingType<Class>]
public partial class ARKitTrampolines {

	[Export<Property> ("geolocationCallbackHandler", ArgumentSemantic.Copy)]
	public partial ARKit.GetGeolocationCallback GeolocationCallbackHandler { get; set; }

}
