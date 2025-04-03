using System;
using System.ComponentModel;
using System.Threading.Tasks;

using Foundation;
using ObjCRuntime;

#if !NET
using NativeHandle = System.IntPtr;
#endif

#nullable enable

namespace PassKit {

#if !NET
	partial class PKPaymentAuthorizationViewController {

		[Obsolete ("This constructor does not create a valid instance of the type")]
		public PKPaymentAuthorizationViewController ()
		{
		}
	}
#endif // !NET


#if !XAMCORE_5_0
#if __IOS__ || __MACCATALYST__
	// Apple just removed this class from their headers in Xcode 15 (beta 1).
	// It's also not found on their documentation site, so I'm assuming it's done on purpose.
#if NET
	/// <summary>To be added.</summary>
	///     <remarks>To be added.</remarks>
	[UnsupportedOSPlatform ("macos")]
	[UnsupportedOSPlatform ("ios")]
	[UnsupportedOSPlatform ("maccatalyst")]
#endif // NET
	[EditorBrowsable (EditorBrowsableState.Never)]
	[Obsolete ("This class is removed.")]
	public unsafe partial class PKDisbursementVoucher : NSObject {

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public override NativeHandle ClassHandle => throw new InvalidOperationException (Constants.RemovedFromPassKit);

		/// <param name="t">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		protected PKDisbursementVoucher (NSObjectFlag t) : base (t)
		{
			throw new InvalidOperationException (Constants.RemovedFromPassKit);
		}

		protected internal PKDisbursementVoucher (NativeHandle handle) : base (handle)
		{
			throw new InvalidOperationException (Constants.RemovedFromPassKit);
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public virtual NSData Data => throw new InvalidOperationException (Constants.RemovedFromPassKit);
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public virtual NSUrl RedemptionUrl => throw new InvalidOperationException (Constants.RemovedFromPassKit);
	} /* class PKDisbursementVoucher */

	// Apple just removed this class from their headers in Xcode 15 (beta 1).
	// It's also not found on their documentation site, so I'm assuming it's done on purpose.
#if NET
	/// <summary>To be added.</summary>
	///     <remarks>To be added.</remarks>
	[UnsupportedOSPlatform ("macos")]
	[UnsupportedOSPlatform ("ios")]
	[UnsupportedOSPlatform ("maccatalyst")]
#endif // NET
	[EditorBrowsable (EditorBrowsableState.Never)]
	[Obsolete ("This class is removed.")]
	public unsafe partial class PKDisbursementAuthorizationController : NSObject {

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public override NativeHandle ClassHandle { get { throw new InvalidOperationException (Constants.RemovedFromPassKit); } }

		/// <param name="t">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		protected PKDisbursementAuthorizationController (NSObjectFlag t) : base (t)
		{
			throw new InvalidOperationException (Constants.RemovedFromPassKit);
		}

		protected internal PKDisbursementAuthorizationController (NativeHandle handle) : base (handle)
		{
			throw new InvalidOperationException (Constants.RemovedFromPassKit);
		}

		/// <param name="disbursementRequest">To be added.</param>
		///         <param name="delegate">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public PKDisbursementAuthorizationController (PKDisbursementRequest disbursementRequest, IPKDisbursementAuthorizationControllerDelegate @delegate)
			: base (NSObjectFlag.Empty)
		{
			throw new InvalidOperationException (Constants.RemovedFromPassKit);
		}

		/// <param name="completion">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public unsafe virtual void AuthorizeDisbursement (global::System.Action<bool, NSError> completion)
		{
			throw new InvalidOperationException (Constants.RemovedFromPassKit);
		}

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public unsafe virtual Task<Tuple<bool, NSError>> AuthorizeDisbursementAsync ()
		{
			throw new InvalidOperationException (Constants.RemovedFromPassKit);
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public IPKDisbursementAuthorizationControllerDelegate Delegate {
			get {
				throw new InvalidOperationException (Constants.RemovedFromPassKit);
			}
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public static bool SupportsDisbursements {
			get {
				throw new InvalidOperationException (Constants.RemovedFromPassKit);
			}
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public virtual NSObject? WeakDelegate {
			get {
				throw new InvalidOperationException (Constants.RemovedFromPassKit);
			}
		}

		/// <param name="disposing">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		protected override void Dispose (bool disposing)
		{
			throw new InvalidOperationException (Constants.RemovedFromPassKit);
		}
	} /* class PKDisbursementAuthorizationController */

	// Apple just removed this protocol from their headers in Xcode 15 (beta 1).
	// It's also not found on their documentation site, so I'm assuming it's done on purpose.
#if NET
	/// <summary>To be added.</summary>
	///     <remarks>To be added.</remarks>
	[UnsupportedOSPlatform ("macos")]
	[UnsupportedOSPlatform ("ios")]
	[UnsupportedOSPlatform ("maccatalyst")]
#endif // NET
	[EditorBrowsable (EditorBrowsableState.Never)]
	[Obsolete ("This class is removed.")]
	public partial interface IPKDisbursementAuthorizationControllerDelegate : INativeObject, IDisposable {
		/// <param name="controller">To be added.</param>
		///         <param name="disbursementVoucher">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		void DidAuthorize (PKDisbursementAuthorizationController controller, PKDisbursementVoucher disbursementVoucher);
		/// <param name="controller">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		void DidFinish (PKDisbursementAuthorizationController controller);
	}

	// Apple just removed this protocol from their headers in Xcode 15 (beta 1).
	// It's also not found on their documentation site, so I'm assuming it's done on purpose.
#if NET
	/// <summary>To be added.</summary>
	///     <remarks>To be added.</remarks>
	[UnsupportedOSPlatform ("macos")]
	[UnsupportedOSPlatform ("ios")]
	[UnsupportedOSPlatform ("maccatalyst")]
#endif // NET
	[EditorBrowsable (EditorBrowsableState.Never)]
	[Obsolete ("This class is removed.")]
	public unsafe abstract partial class PKDisbursementAuthorizationControllerDelegate : NSObject, IPKDisbursementAuthorizationControllerDelegate {
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		protected PKDisbursementAuthorizationControllerDelegate () : base (NSObjectFlag.Empty)
		{
			throw new InvalidOperationException (Constants.RemovedFromPassKit);
		}

		/// <param name="t">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		protected PKDisbursementAuthorizationControllerDelegate (NSObjectFlag t) : base (t)
		{
			throw new InvalidOperationException (Constants.RemovedFromPassKit);
		}

		protected internal PKDisbursementAuthorizationControllerDelegate (NativeHandle handle) : base (handle)
		{
			throw new InvalidOperationException (Constants.RemovedFromPassKit);
		}

		/// <param name="controller">To be added.</param>
		///         <param name="disbursementVoucher">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public virtual void DidAuthorize (PKDisbursementAuthorizationController controller, PKDisbursementVoucher disbursementVoucher)
		{
			throw new InvalidOperationException (Constants.RemovedFromPassKit);
		}

		/// <param name="controller">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public virtual void DidFinish (PKDisbursementAuthorizationController controller)
		{
			throw new InvalidOperationException (Constants.RemovedFromPassKit);
		}
	} /* class PKDisbursementAuthorizationControllerDelegate */

	/// <summary>To be added.</summary>
	///     <remarks>To be added.</remarks>
	public partial class PKDisbursementRequest {

		// Apple just removed this protocol from their headers in Xcode 15 (beta 1).
		// It's also not found on their documentation site, so I'm assuming it's done on purpose.
#if NET
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[UnsupportedOSPlatform ("macos")]
		[UnsupportedOSPlatform ("ios")]
		[UnsupportedOSPlatform ("tvos")]
		[UnsupportedOSPlatform ("maccatalyst")]
#endif // NET
		[EditorBrowsable (EditorBrowsableState.Never)]
		[Obsolete ("This property is removed.")]
		public virtual NSDecimalNumber Amount {
			get => throw new InvalidOperationException (Constants.RemovedFromPassKit);
			set => throw new InvalidOperationException (Constants.RemovedFromPassKit);
		}

#if NET
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[UnsupportedOSPlatform ("macos")]
		[UnsupportedOSPlatform ("ios")]
		[UnsupportedOSPlatform ("tvos")]
		[UnsupportedOSPlatform ("maccatalyst")]
#endif // NET
		[EditorBrowsable (EditorBrowsableState.Never)]
		[Obsolete ("This property is removed.")]
		public virtual string CountryCode {
			get => throw new InvalidOperationException (Constants.RemovedFromPassKit);
			set => throw new InvalidOperationException (Constants.RemovedFromPassKit);
		}

#if NET
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[UnsupportedOSPlatform ("macos")]
		[UnsupportedOSPlatform ("ios")]
		[UnsupportedOSPlatform ("tvos")]
		[UnsupportedOSPlatform ("maccatalyst")]
#endif // NET
		[EditorBrowsable (EditorBrowsableState.Never)]
		[Obsolete ("This property is removed.")]
		public virtual PKDisbursementRequestSchedule RequestSchedule {
			get => throw new InvalidOperationException (Constants.RemovedFromPassKit);
			set => throw new InvalidOperationException (Constants.RemovedFromPassKit);
		}

	} /* class PKDisbursementRequest */
#endif // __IOS__ || __MACCATALYST__
#endif // !XAMCORE_5_0
}
