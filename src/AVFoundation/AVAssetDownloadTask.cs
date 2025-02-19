// 
// AVAssetDownloadTask.cs: AVAssetDownloadTask class
//
// Authors:
//	Alex Soto (alex.soto@xamarin.com)
//     
// Copyright 2015 Xamarin Inc.
//

using System;
using Foundation;

#nullable enable

namespace AVFoundation {
#if !MONOMAC && !TVOS
	public partial class AVAssetDownloadTask : NSUrlSessionTask {

		// NSURLRequest and NSURLResponse objects are not available for AVAssetDownloadTask
		/// <summary>Gets the original URL request object that was passed to the task when the task was initialized.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public override NSUrlRequest OriginalRequest {
			get {
				throw new NotSupportedException ("OriginalRequest not available for AVAssetDownloadTask");
			}
		}

		/// <summary>Gets the URL request object that the task is currently handling.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public override NSUrlRequest CurrentRequest {
			get {
				throw new NotSupportedException ("CurrentRequest not available for AVAssetDownloadTask");
			}
		}

		/// <summary>Gets the HTTP response for the current request.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public override NSUrlResponse Response {
			get {
				throw new NotSupportedException ("Response not available for AVAssetDownloadTask");
			}
		}
	}
#endif
}
