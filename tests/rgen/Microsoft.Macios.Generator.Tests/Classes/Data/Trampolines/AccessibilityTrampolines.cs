// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.Runtime.Versioning;
using Accessibility;
using Foundation;
using ObjCBindings;
using ObjCRuntime;

namespace TestNamespace;

[BindingType<Class>]
public class AccessibilityTrampolines {
	
	[Export<Property> ("valueDescriptionProviderHandler", ArgumentSemantic.Copy)]
	public partial Accessibility.ValueDescriptionProviderHandler ValueDescriptionProviderHandler { get; set; }
}
