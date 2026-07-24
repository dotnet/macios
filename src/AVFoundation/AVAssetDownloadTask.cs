// 
// AVAssetDownloadTask.cs: AVAssetDownloadTask class
//
// Authors:
//	Alex Soto (alex.soto@xamarin.com)
//     
// Copyright 2015 Xamarin Inc.
//

#nullable enable

namespace AVFoundation {
#if !MONOMAC && !TVOS
	public partial class AVAssetDownloadTask : NSUrlSessionTask {

		// NSURLRequest and NSURLResponse objects are not available for AVAssetDownloadTask
		/// <summary>Not supported. Always throws <see cref="NotSupportedException" />.</summary>
		public override NSUrlRequest OriginalRequest {
			get {
				throw new NotSupportedException ("OriginalRequest not available for AVAssetDownloadTask");
			}
		}

		/// <summary>Not supported. Always throws <see cref="NotSupportedException" />.</summary>
		public override NSUrlRequest CurrentRequest {
			get {
				throw new NotSupportedException ("CurrentRequest not available for AVAssetDownloadTask");
			}
		}

		/// <summary>Not supported. Always throws <see cref="NotSupportedException" />.</summary>
		public override NSUrlResponse Response {
			get {
				throw new NotSupportedException ("Response not available for AVAssetDownloadTask");
			}
		}
	}
#endif
}
