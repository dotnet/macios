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
public partial interface IAVAudioMixing {

	[Export<Method> ("destinationForMixer:bus:")]
	public virtual partial AVAudioMixingDestination? DestinationForMixer (AVAudioNode mixer, nuint bus);

	[Export<Property> ("volume")]
	public virtual partial float Volume { get; set; } /* float, not CGFloat */
}
