// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.Runtime.Versioning;
using CoreML;
using Foundation;
using ObjCBindings;
using ObjCRuntime;

namespace TestNamespace;

[BindingType<Class>]
public class CoreMLTrampolines {
	
	[Export("mlModelAssetGetFunctionNamesCompletionHandler", ArgumentSemantic.Copy)]
	public partial CoreML.MLModelAssetGetFunctionNamesCompletionHandler MLModelAssetGetFunctionNamesCompletionHandler { get; set; }

	[Export("mlModelAssetGetModelDescriptionCompletionHandler", ArgumentSemantic.Copy)]
	public partial CoreML.MLModelAssetGetModelDescriptionCompletionHandler MLModelAssetGetModelDescriptionCompletionHandler { get; set; }

	[Export("mlStateGetMultiArrayForStateHandler", ArgumentSemantic.Copy)]
	public partial CoreML.MLStateGetMultiArrayForStateHandler MLStateGetMultiArrayForStateHandler { get; set; }

	[Export("mlStateGetPredictionCompletionHandler", ArgumentSemantic.Copy)]
	public partial CoreML.MLStateGetPredictionCompletionHandler MLStateGetPredictionCompletionHandler { get; set; }
}
