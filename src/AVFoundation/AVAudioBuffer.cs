//
// Copyright 2014 Xamarin Inc
//
// Authors:
//   Miguel de Icaza
//

using AudioToolbox;

#nullable enable

namespace AVFoundation {
	/// <summary>A buffer for audio data.</summary>
	/// <related type="externalDocumentation" href="https://developer.apple.com/documentation/avfaudio/avaudiobuffer">Apple documentation for <c>AVAudioBuffer</c></related>
	public partial class AVAudioBuffer {
		/// <summary>Gets the audio buffer list containing the buffer's audio data.</summary>
		/// <value>The audio buffer list.</value>
		public AudioBuffers AudioBufferList {
			get {
				return new AudioBuffers (audioBufferList);
			}
		}
		/// <summary>Gets a mutable version of the underlying <see cref="AudioToolbox.AudioBuffers" />.</summary>
		/// <summary>Gets a mutable version of the underlying <see cref="AudioToolbox.AudioBuffers" />.</summary>
		/// <value>The mutable audio buffer list.</value>
		public AudioBuffers MutableAudioBufferList {
			get {
				return new AudioBuffers (mutableAudioBufferList);
			}
		}
	}
}
