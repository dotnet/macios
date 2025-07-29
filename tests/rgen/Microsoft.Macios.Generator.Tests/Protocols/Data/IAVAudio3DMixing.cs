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
[BindingType<Protocol>]
public interface IAVAudio3DMixing {

	[Export<Property> ("renderingAlgorithm")]
	public partial AVAudio3DMixingRenderingAlgorithm RenderingAlgorithm { get; set; }

	[Export<Property> ("rate")]
	public partial float Rate { get; set; }

	[Export<Property> ("reverbBlend")]
	public partial float ReverbBlend { get; set; }

	[Export<Property> ("obstruction")]
	public partial float Obstruction { get; set; }

	[Export<Property> ("occlusion")]
	public partial float Occlusion { get; set; }

	[Export<Property> ("sourceMode", ArgumentSemantic.Assign)]
	public partial AVAudio3DMixingSourceMode SourceMode { get; set; }

	[Export<Property> ("pointSourceInHeadMode", ArgumentSemantic.Assign)]
	public partial AVAudio3DMixingPointSourceInHeadMode PointSourceInHeadMode { get; set; }
}
