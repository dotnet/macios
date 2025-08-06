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
[BindingType<ObjCBindings.Protocol> (ObjCBindings.Protocol.CreateModel)]
public partial interface IAVAudioPlayerDelegate {
	[Export<Method> ("audioPlayerDidFinishPlaying:successfully:")]
	public virtual partial void FinishedPlaying (AVAudioPlayer player, bool flag);

	[Export<Method> ("audioPlayerDecodeErrorDidOccur:error:")]
	public virtual partial void DecoderError (AVAudioPlayer player, NSError? error);

#if !__MACOS__
	[UnsupportedOSPlatform ("macos")]
	[Export<Method> ("audioPlayerBeginInterruption:")]
	public virtual partial void BeginInterruption (AVAudioPlayer player);

	[UnsupportedOSPlatform ("macos")]
	[Export<Method> ("audioPlayerBeginInterruption:")]
	public virtual partial void EndInterruption (AVAudioPlayer player);

	[UnsupportedOSPlatform ("macos")]
	[Export<Method> ("audioPlayerEndInterruption:withOptions:")]
	public virtual partial void EndInterruption (AVAudioPlayer player, AVAudioSessionInterruptionOptions flags);
#endif
}
