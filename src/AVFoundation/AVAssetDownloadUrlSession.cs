// 
// AVAssetDownloadTask.cs: AVAssetDownloadTask class
//
// Authors:
//	Alex Soto (alex.soto@xamarin.com)
//     
// Copyright 2015 Xamarin Inc.
//
#if !TVOS
using System;
using Foundation;
using ObjCRuntime;

#nullable enable

namespace AVFoundation {
#if !MONOMAC
	public partial class AVAssetDownloadUrlSession : NSUrlSession {

		/// <summary>Gets the shared session data.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public new static NSUrlSession SharedSession {
			get {
				throw new NotSupportedException ("NS_UNAVAILABLE");
			}
		}

		/// <param name="configuration">To be added.</param>
		///         <summary>Creates a new <see cref="T:Foundation.NSUrlSession" /> from the specified <paramref name="configuration" />.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public new static NSUrlSession FromConfiguration (NSUrlSessionConfiguration configuration)
		{
			throw new NotSupportedException ("NS_UNAVAILABLE");
		}

		/// <param name="configuration">The session configuration to use.</param>
		///         <param name="sessionDelegate">The session delegate to use.</param>
		///         <param name="delegateQueue">The operation cue to use.</param>
		///         <summary>Creates a new <see cref="T:Foundation.NSUrlSession" /> by using the specified configuration, delegate, and delegate cue.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public new static NSUrlSession FromConfiguration (NSUrlSessionConfiguration configuration, INSUrlSessionDelegate sessionDelegate, NSOperationQueue delegateQueue)
		{
			throw new NotSupportedException ("NS_UNAVAILABLE");
		}

		/// <param name="configuration">To be added.</param>
		///         <param name="weakDelegate">To be added.</param>
		///         <param name="delegateQueue">To be added.</param>
		///         <summary>Creates a new <see cref="T:Foundation.NSUrlSession" /> from the specified <paramref name="configuration" />, weakly-referenced session delegate, and delegate queue.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public new static NSUrlSession FromWeakConfiguration (NSUrlSessionConfiguration configuration, NSObject weakDelegate, NSOperationQueue delegateQueue)
		{
			throw new NotSupportedException ("NS_UNAVAILABLE");
		}

		/// <param name="request">To be added.</param>
		///         <summary>Creates a new <see cref="T:Foundation.NSUrlSessionDataTask" /> with the specified request.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public override NSUrlSessionDataTask CreateDataTask (NSUrlRequest request)
		{
			throw new NotSupportedException ("NS_UNAVAILABLE");
		}

		/// <param name="url">To be added.</param>
		///         <summary>Creates a new <see cref="T:Foundation.NSUrlSessionDataTask" /> for the specified URL.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public override NSUrlSessionDataTask CreateDataTask (NSUrl url)
		{
			throw new NotSupportedException ("NS_UNAVAILABLE");
		}

		/// <param name="request">To be added.</param>
		///         <param name="fileURL">To be added.</param>
		///         <summary>Creates a new <see cref="T:Foundation.NSUrlSessionUploadTask" /> for the specified <paramref name="request" /> and <paramref name="fileURL" />.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public override NSUrlSessionUploadTask CreateUploadTask (NSUrlRequest request, NSUrl fileURL)
		{
			throw new NotSupportedException ("NS_UNAVAILABLE");
		}

		/// <param name="request">To be added.</param>
		///         <param name="bodyData">To be added.</param>
		///         <summary>Creates a new <see cref="T:Foundation.NSUrlSessionUploadTask" /> for the specified <paramref name="request" /> and <paramref name="bodyData" />.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public override NSUrlSessionUploadTask CreateUploadTask (NSUrlRequest request, NSData bodyData)
		{
			throw new NotSupportedException ("NS_UNAVAILABLE");
		}

		/// <param name="request">To be added.</param>
		///         <summary>Creates a new <see cref="T:Foundation.NSUrlSessionUploadTask" /> for the specified request.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public override NSUrlSessionUploadTask CreateUploadTask (NSUrlRequest request)
		{
			throw new NotSupportedException ("NS_UNAVAILABLE");
		}

		/// <param name="request">To be added.</param>
		///         <summary>Creates a new <see cref="T:Foundation.NSUrlSessionDownloadTask" /> for the specified request.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public override NSUrlSessionDownloadTask CreateDownloadTask (NSUrlRequest request)
		{
			throw new NotSupportedException ("NS_UNAVAILABLE");
		}

		/// <param name="url">To be added.</param>
		///         <summary>Creates a new <see cref="T:Foundation.NSUrlSessionDownloadTask" /> for the specified URL.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public override NSUrlSessionDownloadTask CreateDownloadTask (NSUrl url)
		{
			throw new NotSupportedException ("NS_UNAVAILABLE");
		}

		/// <param name="resumeData">To be added.</param>
		///         <summary>reates a new <see cref="T:Foundation.NSUrlSessionDataTask" /> that resumes downloading <paramref name="resumeData" />.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public override NSUrlSessionDownloadTask CreateDownloadTask (NSData resumeData)
		{
			throw new NotSupportedException ("NS_UNAVAILABLE");
		}

		/// <param name="request">To be added.</param>
		///         <param name="completionHandler">To be added.</param>
		///         <summary>reates a new <see cref="T:Foundation.NSUrlSessionDataTask" /> for the specified request, and runs a completion handler when it is finished.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public override NSUrlSessionDataTask CreateDataTask (NSUrlRequest request, NSUrlSessionResponse? completionHandler)
		{
			throw new NotSupportedException ("NS_UNAVAILABLE");
		}

		/// <param name="url">To be added.</param>
		///         <param name="completionHandler">To be added.</param>
		///         <summary>Creates a new <see cref="T:Foundation.NSUrlSessionDataTask" /> for the specified URL, and runs a completion handler when it is finished.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public override NSUrlSessionDataTask CreateDataTask (NSUrl url, NSUrlSessionResponse? completionHandler)
		{
			throw new NotSupportedException ("NS_UNAVAILABLE");
		}

		/// <param name="request">To be added.</param>
		///         <param name="fileURL">To be added.</param>
		///         <param name="completionHandler">To be added.</param>
		///         <summary>Creates a new <see cref="T:Foundation.NSUrlSessionUploadTask" /> for the specified <paramref name="request" /> and <paramref name="fileURL" />, and runs <paramref name="completionHandler" /> when it is finished.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public override NSUrlSessionUploadTask CreateUploadTask (NSUrlRequest request, NSUrl fileURL, NSUrlSessionResponse completionHandler)
		{
			throw new NotSupportedException ("NS_UNAVAILABLE");
		}

		/// <param name="request">To be added.</param>
		///         <param name="bodyData">To be added.</param>
		///         <param name="completionHandler">To be added.</param>
		///         <summary>Creates a new <see cref="T:Foundation.NSUrlSessionUploadTask" /> for the specified <paramref name="request" /> and <paramref name="bodyData" />, and runs <paramref name="completionHandler" /> when it is finished.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public override NSUrlSessionUploadTask CreateUploadTask (NSUrlRequest request, NSData bodyData, NSUrlSessionResponse completionHandler)
		{
			throw new NotSupportedException ("NS_UNAVAILABLE");
		}

		/// <param name="request">To be added.</param>
		///         <param name="completionHandler">To be added.</param>
		///         <summary>Creates a new <see cref="T:Foundation.NSUrlSessionDownloadTask" /> for the specified request, and runs a completion handler when it is finished.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public override NSUrlSessionDownloadTask CreateDownloadTask (NSUrlRequest request, NSUrlDownloadSessionResponse? completionHandler)
		{
			throw new NotSupportedException ("NS_UNAVAILABLE");
		}

		/// <param name="url">To be added.</param>
		///         <param name="completionHandler">To be added.</param>
		///         <summary>Creates a new <see cref="T:Foundation.NSUrlSessionDownloadTask" /> for the specified url, and runs a completion handler when it is finished.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public override NSUrlSessionDownloadTask CreateDownloadTask (NSUrl url, NSUrlDownloadSessionResponse? completionHandler)
		{
			throw new NotSupportedException ("NS_UNAVAILABLE");
		}

		/// <param name="resumeData">To be added.</param>
		///         <param name="completionHandler">To be added.</param>
		///         <summary>Creates a new <see cref="T:Foundation.NSUrlSessionDownloadTask" /> that resumes downloading the <paramref name="resumeData" />, and runs a completion handler when it is finished.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public override NSUrlSessionDownloadTask CreateDownloadTaskFromResumeData (NSData resumeData, NSUrlDownloadSessionResponse? completionHandler)
		{
			throw new NotSupportedException ("NS_UNAVAILABLE");
		}
	}
#endif
}
#endif
