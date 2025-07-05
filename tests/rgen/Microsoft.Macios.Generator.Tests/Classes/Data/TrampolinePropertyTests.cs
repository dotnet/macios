// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
#pragma warning disable APL0003

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
using AudioUnit;

namespace TestNamespace;

[BindingType<Class>]
public partial class TrampolinePropertyTests {

	public delegate NSObject CreateObject (NSObject obj);

	[Export<Property> ("createObjectHandler", ArgumentSemantic.Copy)]
	public partial CreateObject CreateObjectHandler { get; set; }

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

	// Property using Action<bool>
	[Export<Property> ("boolActionHandler", ArgumentSemantic.Copy)]
	public partial Action<bool> BoolActionHandler { get; set; }

	// Property using AVAssetImageGenerator.AsynchronouslyForTimeCompletionHandler
	[Export<Property> ("imageGeneratorCompletionHandler", ArgumentSemantic.Copy)]
	public partial AVAssetImageGenerateAsynchronouslyForTimeCompletionHandler ImageGeneratorCompletionHandler { get; set; }

	// Property using CIKernelRoiCallback
	[Export<Property> ("kernelRoiCallback", ArgumentSemantic.Copy)]
	public partial CIKernelRoiCallback KernelRoiCallback { get; set; }

	// Property using AVAssetImageGenerator.AsynchronouslyForTimeCompletionHandler
	[Export<Property> ("manualRenderingCallback", ArgumentSemantic.Copy)]
	public partial AVAudioEngineManualRenderingBlock ManualRendering { get; set; }

	[Export<Property> ("internalRenderBlockHandler", ArgumentSemantic.Copy)]
	public partial AUInternalRenderBlock InternalRenderBlockHandler { get; set; }
}
