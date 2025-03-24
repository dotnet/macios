// 
// AVPlayerLayer.cs: AVPlayerLayer class
//
// Authors:
//	Alex Soto (alex.soto@xamarin.com)
//     
// Copyright 2015 Xamarin Inc.
//

using ObjCRuntime;
using CoreVideo;

#nullable enable

namespace AVFoundation {
	public partial class AVPlayerLayer {
		/// <summary>Gets or sets the attributes of the client visual buffer.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		public CVPixelBufferAttributes? PixelBufferAttributes {
			get {
				if (WeakPixelBufferAttributes is not null) {
					var strongDict = new CVPixelBufferAttributes (WeakPixelBufferAttributes);
					return strongDict;
				}
				return null;
			}
			set {
				WeakPixelBufferAttributes = value?.Dictionary;
			}
		}
	}
}
