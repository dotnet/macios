#if !XAMCORE_5_0

using System;
using System.ComponentModel;
using System.Runtime.Versioning;
using System.Diagnostics.CodeAnalysis;

using Foundation;
using ObjCRuntime;

#nullable enable

namespace CloudKit {
	/// <summary>Marks push notifications as read. Typically used by apps that use push notifications to track record changes.</summary>
	///     <remarks>To be added.</remarks>
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/CloudKit/Reference/CKMarkNotificationsReadOperation_class/index.html">Apple documentation for <c>CKMarkNotificationsReadOperation</c></related>
	[Register ("CKMarkNotificationsReadOperation", SkipRegistration = true)]
	[UnsupportedOSPlatform ("ios", "Use 'CKDatabaseSubscription', 'CKFetchDatabaseChangesOperation' and 'CKFetchRecordZoneChangesOperation' instead.")]
	[UnsupportedOSPlatform ("macos", "Use 'CKDatabaseSubscription', 'CKFetchDatabaseChangesOperation' and 'CKFetchRecordZoneChangesOperation' instead.")]
	[UnsupportedOSPlatform ("tvos", "Use 'CKDatabaseSubscription', 'CKFetchDatabaseChangesOperation' and 'CKFetchRecordZoneChangesOperation' instead.")]
	[UnsupportedOSPlatform ("maccatalyst", "Use 'CKDatabaseSubscription', 'CKFetchDatabaseChangesOperation' and 'CKFetchRecordZoneChangesOperation' instead.")]
	[EditorBrowsable (EditorBrowsableState.Never)]
	public unsafe partial class CKMarkNotificationsReadOperation : CKOperation {
		/// <summary>The handle for this class.</summary>
		///         <value>The pointer to the Objective-C class.</value>
		///         <remarks>Each Xamarin.iOS class mirrors an unmanaged Objective-C class.   This value contains the pointer to the Objective-C class, it is similar to calling objc_getClass with the object name.</remarks>
		public override NativeHandle ClassHandle { get => throw new PlatformNotSupportedException (Constants.TypeRemovedAllPlatforms); }

		/// <include file="../../docs/api/CloudKit/CKMarkNotificationsReadOperation.xml" path="/Documentation/Docs[@DocId='M:CloudKit.CKMarkNotificationsReadOperation.#ctor(Foundation.NSObjectFlag)']/*" />
		protected CKMarkNotificationsReadOperation (NSObjectFlag t) : base (t)
		{
			throw new PlatformNotSupportedException (Constants.TypeRemovedAllPlatforms);
		}

		protected internal CKMarkNotificationsReadOperation (NativeHandle handle) : base (handle)
		{
			throw new PlatformNotSupportedException (Constants.TypeRemovedAllPlatforms);
		}

		/// <param name="notificationIds">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public CKMarkNotificationsReadOperation (CKNotificationID [] notificationIds)
			: base (NSObjectFlag.Empty)
		{
			throw new PlatformNotSupportedException (Constants.TypeRemovedAllPlatforms);
		}

		/// <summary>Gets or sets the handler that is run after the operation completes.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public unsafe virtual CKMarkNotificationsReadHandler? Completed {
			get {
				throw new PlatformNotSupportedException (Constants.TypeRemovedAllPlatforms);
			}
			set {
				throw new PlatformNotSupportedException (Constants.TypeRemovedAllPlatforms);
			}
		}

		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		public virtual CKNotificationID []? NotificationIds {
			get {
				throw new PlatformNotSupportedException (Constants.TypeRemovedAllPlatforms);
			}
			set {
				throw new PlatformNotSupportedException (Constants.TypeRemovedAllPlatforms);
			}
		}
	} /* class CKMarkNotificationsReadOperation */

	/// <param name="notificationIDsMarkedRead">To be added.</param>
	///     <param name="operationError">To be added.</param>
	///     <summary>Delegate for the <see cref="CloudKit.CKMarkNotificationsReadOperation.Completed" /> property.</summary>
	///     <remarks>To be added.</remarks>
	[EditorBrowsable (EditorBrowsableState.Never)]
	public delegate void CKMarkNotificationsReadHandler (CKNotificationID [] notificationIDsMarkedRead, NSError operationError);
}
#endif // !XAMCORE_5_0
