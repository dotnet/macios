// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using AVFoundation;
using Foundation;
using ObjCBindings;
using ObjCRuntime;
using System.Runtime.Versioning;

namespace Microsoft.Macios.Generator.Tests.Protocols.Data;

[SupportedOSPlatform ("ios")]
[SupportedOSPlatform ("tvos")]
[SupportedOSPlatform ("macos")]
[SupportedOSPlatform ("maccatalyst13.1")]
[BindingType<ObjCBindings.Protocol> (ObjCBindings.Protocol.CreateModel, ModelName = "MyCustomAVAudio3DMixing")]
public partial interface IAVAudio3DMixing {

	[Export<Property> ("renderingAlgorithm")]
	public virtual partial AVAudio3DMixingRenderingAlgorithm RenderingAlgorithm { get; set; }

	[Export<Property> ("rate")]
	public virtual partial float Rate { get; set; }

	[Export<Property> ("reverbBlend")]
	public virtual partial float ReverbBlend { get; set; }

	[Export<Property> ("obstruction")]
	public virtual partial float Obstruction { get; set; }

	[Export<Property> ("occlusion")]
	public virtual partial float Occlusion { get; set; }

	[Export<Property> ("sourceMode", ArgumentSemantic.Assign)]
	public virtual partial AVAudio3DMixingSourceMode SourceMode { get; set; }

	[Export<Property> ("pointSourceInHeadMode", ArgumentSemantic.Assign)]
	public virtual partial AVAudio3DMixingPointSourceInHeadMode PointSourceInHeadMode { get; set; }
}
