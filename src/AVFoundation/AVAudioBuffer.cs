//
// Copyright 2014 Xamarin Inc
//
// Authors:
//   Miguel de Icaza
//

using Foundation;
using System;
using AudioToolbox;

#nullable enable

namespace AVFoundation {
	/// <summary>A buffer for audio data.</summary>
	///     <remarks>To be added.</remarks>
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/AVFoundation/Reference/AVAudioBuffer_Class/index.html">Apple documentation for <c>AVAudioBuffer</c></related>
	public partial class AVAudioBuffer {
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public AudioBuffers AudioBufferList {
			get {
				return new AudioBuffers (audioBufferList);
			}
		}

		/// <summary>Gets a mutable version of the underlying <see cref="T:AudioToolbox.AudioBuffers" />.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public AudioBuffers MutableAudioBufferList {
			get {
				return new AudioBuffers (mutableAudioBufferList);
			}
		}
	}
}
