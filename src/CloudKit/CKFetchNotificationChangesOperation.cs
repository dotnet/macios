#if !XAMCORE_5_0

using System;
using System.ComponentModel;
using System.Runtime.Versioning;
using System.Diagnostics.CodeAnalysis;

using Foundation;
using ObjCRuntime;

#nullable enable

#if !NET
using NativeHandle = System.IntPtr;
#endif

namespace CloudKit {
	/// <summary>A <see cref="T:CloudKit.CKOperation" /> that ret../../summary_set.sh CKFetchNotificationChangesOperation A</summary>
	///     <remarks>To be added.</remarks>
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/CloudKit/Reference/CKFetchNotificationChangesOperation_class/index.html">Apple documentation for <c>CKFetchNotificationChangesOperation</c></related>
	[Register ("CKFetchNotificationChangesOperation", SkipRegistration = true)]
#if NET
	[UnsupportedOSPlatform ("ios", "Use 'CKDatabaseSubscription', 'CKFetchDatabaseChangesOperation' and 'CKFetchRecordZoneChangesOperation' instead.")]
	[UnsupportedOSPlatform ("macos", "Use 'CKDatabaseSubscription', 'CKFetchDatabaseChangesOperation' and 'CKFetchRecordZoneChangesOperation' instead.")]
	[UnsupportedOSPlatform ("tvos", "Use 'CKDatabaseSubscription', 'CKFetchDatabaseChangesOperation' and 'CKFetchRecordZoneChangesOperation' instead.")]
	[UnsupportedOSPlatform ("maccatalyst", "Use 'CKDatabaseSubscription', 'CKFetchDatabaseChangesOperation' and 'CKFetchRecordZoneChangesOperation' instead.")]
#else
	[Obsolete ("Use 'CKDatabaseSubscription', 'CKFetchDatabaseChangesOperation' and 'CKFetchRecordZoneChangesOperation' instead.")]
#endif
	[EditorBrowsable (EditorBrowsableState.Never)]
	public unsafe partial class CKFetchNotificationChangesOperation : CKOperation {
		/// <summary>The handle for this class.</summary>
		///         <value>The pointer to the Objective-C class.</value>
		///         <remarks>Each Xamarin.iOS class mirrors an unmanaged Objective-C class.   This value contains the pointer to the Objective-C class, it is similar to calling objc_getClass with the object name.</remarks>
		public override NativeHandle ClassHandle { get => throw new PlatformNotSupportedException (Constants.TypeRemovedAllPlatforms); }

		/// <include file="../../docs/api/CloudKit/CKFetchNotificationChangesOperation.xml" path="/Documentation/Docs[@DocId='M:CloudKit.CKFetchNotificationChangesOperation.#ctor(Foundation.NSObjectFlag)']/*" />
		protected CKFetchNotificationChangesOperation (NSObjectFlag t) : base (t)
		{
			throw new PlatformNotSupportedException (Constants.TypeRemovedAllPlatforms);
		}

		protected internal CKFetchNotificationChangesOperation (NativeHandle handle) : base (handle)
		{
			throw new PlatformNotSupportedException (Constants.TypeRemovedAllPlatforms);
		}

		/// <summary>Default constructor, initializes a new instance of this class.</summary>
		///         <remarks />
		public CKFetchNotificationChangesOperation ()
			: base (NSObjectFlag.Empty)
		{
			throw new PlatformNotSupportedException (Constants.TypeRemovedAllPlatforms);
		}

		/// <param name="previousServerChangeToken">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public CKFetchNotificationChangesOperation (CKServerChangeToken? previousServerChangeToken)
			: base (NSObjectFlag.Empty)
		{
			throw new PlatformNotSupportedException (Constants.TypeRemovedAllPlatforms);
		}

		/// <summary>Gets or sets the action that is run after the operation completes.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		public unsafe virtual global::System.Action<CKServerChangeToken, NSError>? Completed {
			get {
				throw new PlatformNotSupportedException (Constants.TypeRemovedAllPlatforms);
			}
			set {
				throw new PlatformNotSupportedException (Constants.TypeRemovedAllPlatforms);
			}
		}

		/// <summary>Gets whether more notifications are waiting for delivery.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public virtual bool MoreComing {
			get {
				throw new PlatformNotSupportedException (Constants.TypeRemovedAllPlatforms);
			}
		}

		/// <summary>Action that is executed for each push notification.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		public unsafe virtual global::System.Action<CKNotification>? NotificationChanged {
			get {
				throw new PlatformNotSupportedException (Constants.TypeRemovedAllPlatforms);
			}
			set {
				throw new PlatformNotSupportedException (Constants.TypeRemovedAllPlatforms);
			}
		}

		/// <summary>The <see cref="T:CloudKit.CKServerChangeToken" /> from a previous fetch operation.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		public virtual CKServerChangeToken? PreviousServerChangeToken {
			get {
				throw new PlatformNotSupportedException (Constants.TypeRemovedAllPlatforms);
			}
			set {
				throw new PlatformNotSupportedException (Constants.TypeRemovedAllPlatforms);
			}
		}

		/// <summary>Gets or sets the maximum number of results to fetch.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public virtual nuint ResultsLimit {
			get {
				throw new PlatformNotSupportedException (Constants.TypeRemovedAllPlatforms);
			}
			set {
				throw new PlatformNotSupportedException (Constants.TypeRemovedAllPlatforms);
			}
		}
	} /* class CKFetchNotificationChangesOperation */
}

#endif // !XAMCORE_5_0
