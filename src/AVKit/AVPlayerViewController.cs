//
// AVPlayerViewController.cs: Complementing methods
//
// Author:
//   Miguel de Icaza (miguel@xamarin.com)
//
// Copyright 2014 Xamarin Inc
//
using System;
using Foundation;
using CoreMedia;
using CoreVideo;
using AVFoundation;

#nullable enable

namespace AVKit {
#if !MONOMAC
	partial class AVPlayerViewController {
		/// <summary>Defines how the video is shown within its bounds.</summary>
		///         <value>The video gravity for the video, the default value is <see cref="F:AVFoundation.AVLayerVideoGravity.ResizeAspect" /></value>
		///         <remarks>
		///           <para>
		///           </para>
		///         </remarks>
		public AVLayerVideoGravity VideoGravity {
			get {
				return AVPlayerLayer.KeyToEnum (WeakVideoGravity);
			}
			set {
				WeakVideoGravity = AVPlayerLayer.EnumToKey (value);
			}
		}
	}
#endif
}
