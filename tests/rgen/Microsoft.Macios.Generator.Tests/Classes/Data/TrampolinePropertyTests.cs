// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Microsoft.Macios.Generator.Tests.Classes.Data;

// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.Runtime.Versioning;
using AVFoundation;
using CoreImage;
using CoreGraphics;
using Foundation;
using ObjCBindings;
using ObjCRuntime;

namespace TestNamespace;

[BindingType<Class>]
public partial class TrampolinePropertyTests {

	[Export<Property> ("completionHandler", ArgumentSemantic.Copy)]
	public partial Action CompletionHandler { get; set; }

	// Duplicate property using Action
	[Export<Property> ("duplicateCompletionHandler", ArgumentSemantic.Copy)]
	public partial Action DuplicateCompletionHandler { get; set; }

	[Export<Property> ("imageGeneratorCompletionHandler", ArgumentSemantic.Copy)]
	public partial AVAssetImageGenerator.AsynchronouslyForTimeCompletionHandler ImageGeneratorCompletionHandler { get; set; }

	// Property using CIKernelRoiCallback
	[Export<Property> ("kernelRoiCallback", ArgumentSemantic.Copy)]
	public partial CIKernelRoiCallback KernelRoiCallback { get; set; }]

	// Property using Action<string>
	[Export<Property> ("stringActionHandler", ArgumentSemantic.Copy)]
	public partial Action<string> StringActionHandler { get; set; }

	// Property using Action<int>
	[Export<Property> ("intActionHandler", ArgumentSemantic.Copy)]
	public partial Action<int> IntActionHandler { get; set; }

	// Property using AVAssetImageGenerator.AsynchronouslyForTimeCompletionHandler
	[Export<Property> ("imageGeneratorCompletionHandler", ArgumentSemantic.Copy)]
	public partial AVAssetImageGenerateAsynchronouslyForTimeCompletionHandler ImageGeneratorCompletionHandler { get; set; }

	// Property using CIKernelRoiCallback
	[Export<Property> ("kernelRoiCallback", ArgumentSemantic.Copy)]
	public partial CIKernelRoiCallback KernelRoiCallback { get; set; }
}
