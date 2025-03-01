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
