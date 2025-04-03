#if !XAMCORE_5_0

using System;
using System.ComponentModel;

using Foundation;
using ObjCRuntime;

#nullable enable
#if !NET
using NativeHandle = System.IntPtr;
#endif

namespace NewsstandKit {
	[EditorBrowsable (EditorBrowsableState.Never)]
	[Obsolete ("The NewsstandKit framework has been removed from iOS.")]
	public unsafe partial class NKAssetDownload : NSObject {
		/// <summary>The handle for this class.</summary>
		///         <value>The pointer to the Objective-C class.</value>
		///         <remarks>Each Xamarin.iOS class mirrors an unmanaged Objective-C class.   This value contains the pointer to the Objective-C class, it is similar to calling objc_getClass with the object name.</remarks>
		public override NativeHandle ClassHandle { get { throw new InvalidOperationException (Constants.NewsstandKitRemoved); } }

		/// <include file="../../docs/api/NewsstandKit/NKAssetDownload.xml" path="/Documentation/Docs[@DocId='M:NewsstandKit.NKAssetDownload.#ctor(Foundation.NSObjectFlag)']/*" />
		protected NKAssetDownload (NSObjectFlag t) : base (t)
		{
			throw new InvalidOperationException (Constants.NewsstandKitRemoved);
		}

		protected internal NKAssetDownload (NativeHandle handle) : base (handle)
		{
			throw new InvalidOperationException (Constants.NewsstandKitRemoved);
		}

		/// <param name="downloadDelegate">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public virtual NSUrlConnection DownloadWithDelegate (INSUrlConnectionDownloadDelegate downloadDelegate)
		{
			throw new InvalidOperationException (Constants.NewsstandKitRemoved);
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public virtual string Identifier {
			get {
				throw new InvalidOperationException (Constants.NewsstandKitRemoved);
			}
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public virtual NKIssue? Issue {
			get {
				throw new InvalidOperationException (Constants.NewsstandKitRemoved);
			}
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public virtual NSUrlRequest UrlRequest {
			get {
				throw new InvalidOperationException (Constants.NewsstandKitRemoved);
			}
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public virtual NSDictionary? UserInfo {
			get {
				throw new InvalidOperationException (Constants.NewsstandKitRemoved);
			}
			set {
				throw new InvalidOperationException (Constants.NewsstandKitRemoved);
			}
		}

		/// <include file="../../docs/api/NewsstandKit/NKAssetDownload.xml" path="/Documentation/Docs[@DocId='M:NewsstandKit.NKAssetDownload.Dispose(System.Boolean)']/*" />
		protected override void Dispose (bool disposing)
		{
			throw new InvalidOperationException (Constants.NewsstandKitRemoved);
		}
	} /* class NKAssetDownload */

	[EditorBrowsable (EditorBrowsableState.Never)]
	[Obsolete ("The NewsstandKit framework has been removed from iOS.")]
	public unsafe partial class NKIssue : NSObject {
		/// <summary>The handle for this class.</summary>
		///         <value>The pointer to the Objective-C class.</value>
		///         <remarks>Each Xamarin.iOS class mirrors an unmanaged Objective-C class.   This value contains the pointer to the Objective-C class, it is similar to calling objc_getClass with the object name.</remarks>
		public override NativeHandle ClassHandle { get { throw new InvalidOperationException (Constants.NewsstandKitRemoved); } }

		/// <include file="../../docs/api/NewsstandKit/NKIssue.xml" path="/Documentation/Docs[@DocId='M:NewsstandKit.NKIssue.#ctor(Foundation.NSObjectFlag)']/*" />
		protected NKIssue (NSObjectFlag t) : base (t)
		{
			throw new InvalidOperationException (Constants.NewsstandKitRemoved);
		}

		protected internal NKIssue (NativeHandle handle) : base (handle)
		{
			throw new InvalidOperationException (Constants.NewsstandKitRemoved);
		}

		/// <param name="request">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public virtual NKAssetDownload AddAsset (NSUrlRequest request)
		{
			throw new InvalidOperationException (Constants.NewsstandKitRemoved);
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public virtual NSUrl ContentUrl {
			get {
				throw new InvalidOperationException (Constants.NewsstandKitRemoved);
			}
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public virtual NSDate Date {
			get {
				throw new InvalidOperationException (Constants.NewsstandKitRemoved);
			}
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public virtual NKAssetDownload [] DownloadingAssets {
			get {
				throw new InvalidOperationException (Constants.NewsstandKitRemoved);
			}
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public virtual string Name {
			get {
				throw new InvalidOperationException (Constants.NewsstandKitRemoved);
			}
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public virtual NKIssueContentStatus Status {
			get {
				throw new InvalidOperationException (Constants.NewsstandKitRemoved);
			}
		}

		/// <include file="../../docs/api/NewsstandKit/NKIssue.xml" path="/Documentation/Docs[@DocId='P:NewsstandKit.NKIssue.DownloadCompletedNotification']/*" />
		public static NSString DownloadCompletedNotification {
			get {
				throw new InvalidOperationException (Constants.NewsstandKitRemoved);
			}
		}

		//
		// Notifications
		//
		public static partial class Notifications {
			/// <include file="../../docs/api/NewsstandKit.NKIssue/Notifications.xml" path="/Documentation/Docs[@DocId='M:NewsstandKit.NKIssue.Notifications.ObserveDownloadCompleted(System.EventHandler{Foundation.NSNotificationEventArgs})']/*" />
			public static NSObject ObserveDownloadCompleted (EventHandler<NSNotificationEventArgs> handler)
			{
				throw new InvalidOperationException (Constants.NewsstandKitRemoved);
			}
			/// <include file="../../docs/api/NewsstandKit.NKIssue/Notifications.xml" path="/Documentation/Docs[@DocId='M:NewsstandKit.NKIssue.Notifications.ObserveDownloadCompleted(Foundation.NSObject,System.EventHandler{Foundation.NSNotificationEventArgs})']/*" />
			public static NSObject ObserveDownloadCompleted (NSObject objectToObserve, EventHandler<NSNotificationEventArgs> handler)
			{
				throw new InvalidOperationException (Constants.NewsstandKitRemoved);
			}
		}
	} /* class NKIssue */

	[EditorBrowsable (EditorBrowsableState.Never)]
	[Obsolete ("The NewsstandKit framework has been removed from iOS.")]
	public enum NKIssueContentStatus : long {
		/// <summary>To be added.</summary>
		None = 0,
		/// <summary>To be added.</summary>
		Downloading = 1,
		/// <summary>To be added.</summary>
		Available = 2,
	}

	[EditorBrowsable (EditorBrowsableState.Never)]
	[Obsolete ("The NewsstandKit framework has been removed from iOS.")]
	public unsafe partial class NKLibrary : NSObject {
		/// <summary>The handle for this class.</summary>
		///         <value>The pointer to the Objective-C class.</value>
		///         <remarks>Each Xamarin.iOS class mirrors an unmanaged Objective-C class.   This value contains the pointer to the Objective-C class, it is similar to calling objc_getClass with the object name.</remarks>
		public override NativeHandle ClassHandle { get { throw new InvalidOperationException (Constants.NewsstandKitRemoved); } }

		/// <include file="../../docs/api/NewsstandKit/NKLibrary.xml" path="/Documentation/Docs[@DocId='M:NewsstandKit.NKLibrary.#ctor(Foundation.NSObjectFlag)']/*" />
		protected NKLibrary (NSObjectFlag t) : base (t)
		{
			throw new InvalidOperationException (Constants.NewsstandKitRemoved);
		}

		protected internal NKLibrary (NativeHandle handle) : base (handle)
		{
			throw new InvalidOperationException (Constants.NewsstandKitRemoved);
		}

		/// <param name="name">To be added.</param>
		///         <param name="date">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public virtual NKIssue AddIssue (string name, NSDate date)
		{
			throw new InvalidOperationException (Constants.NewsstandKitRemoved);
		}

		/// <param name="name">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public virtual NKIssue? GetIssue (string name)
		{
			throw new InvalidOperationException (Constants.NewsstandKitRemoved);
		}

		/// <param name="issue">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public virtual void RemoveIssue (NKIssue issue)
		{
			throw new InvalidOperationException (Constants.NewsstandKitRemoved);
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public virtual NKIssue? CurrentlyReadingIssue {
			get {
				throw new InvalidOperationException (Constants.NewsstandKitRemoved);
			}
			set {
				throw new InvalidOperationException (Constants.NewsstandKitRemoved);
			}
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public virtual NKAssetDownload [] DownloadingAssets {
			get {
				throw new InvalidOperationException (Constants.NewsstandKitRemoved);
			}
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public virtual NKIssue [] Issues {
			get {
				throw new InvalidOperationException (Constants.NewsstandKitRemoved);
			}
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public static NKLibrary? SharedLibrary {
			get {
				throw new InvalidOperationException (Constants.NewsstandKitRemoved);
			}
		}
	} /* class NKLibrary */
}


#endif // !XAMCORE_5_0
