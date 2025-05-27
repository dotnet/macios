// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.Runtime.Versioning;
using AVKit;
using Foundation;
using ObjCBindings;
using ObjCRuntime;

namespace TestNamespace;

[BindingType<Class>]
public class AVKitTrampolines {
	
	[Export<Property> ("avCustomRoutingControllerDelegateCompletionHandler", ArgumentSemantic.Copy)]
	public partial AVKit.AVCustomRoutingControllerDelegateCompletionHandler AVCustomRoutingControllerDelegateCompletionHandler { get; set; }
}
