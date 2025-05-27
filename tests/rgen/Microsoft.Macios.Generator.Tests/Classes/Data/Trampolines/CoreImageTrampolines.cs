// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.Runtime.Versioning;
using CoreImage;
using Foundation;
using ObjCBindings;
using ObjCRuntime;

namespace TestNamespace;

[BindingType<Class>]
public class CoreImageTrampolines {

	[Export<Property> ("ciKernelRoiCallback", ArgumentSemantic.Copy)]
	public partial CoreImage.CIKernelRoiCallback CIKernelRoiCallback { get; set; }
}
