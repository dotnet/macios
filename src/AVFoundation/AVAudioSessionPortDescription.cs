// 
// AVAudioSessionPortDescription.cs
//
// Authors: Rolf Bjarne Kvinge <rolf@xamarin.com>
//     
// Copyright 2015 Xamarin Inc.
//

using System;

using Foundation;
using CoreFoundation;
using ObjCRuntime;
using AudioToolbox;

#nullable enable

#if !MONOMAC
namespace AVFoundation {
	/// <summary>Encpasulates information about the input and output ports of an audio session.</summary>
	///     <remarks>To be added.</remarks>
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/AVFoundation/Reference/AVAudioSessionPortDescription_class/index.html">Apple documentation for <c>AVAudioSessionPortDescription</c></related>
	public partial class AVAudioSessionPortDescription {
	}
}
#endif
