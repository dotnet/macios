// 
// AVAudioSessionPortDescription.cs
//
// Authors: Rolf Bjarne Kvinge <rolf@xamarin.com>
//     
// Copyright 2015 Xamarin Inc.
//

using CoreFoundation;
using AudioToolbox;

#nullable enable

#if !MONOMAC
namespace AVFoundation {
	/// <summary>Encapsulates information about the input and output ports of an audio session.</summary>
	/// <related type="externalDocumentation" href="https://developer.apple.com/documentation/avfaudio/avaudiosessionportdescription">Apple documentation for <c>AVAudioSessionPortDescription</c></related>
	public partial class AVAudioSessionPortDescription {
	}
}
#endif
