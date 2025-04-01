// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Microsoft.Macios.Generator.Tests.Classes.Data;

// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.Runtime.Versioning;
using CoreGraphics;
using Foundation;
using ObjCBindings;
using ObjCRuntime;

namespace TestNamespace;

[BindingType<Class>]
public partial class TrampolinePropertyTests {

	[Export<Property> ("completionHandler", ArgumentSemantic.Copy)]
	public partial Action CompletionHandler { get; set; }
}
