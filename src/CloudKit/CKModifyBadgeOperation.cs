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
	/// <summary>A <see cref="T:CloudKit.CKOperation" /> that modifies the badge of the app's icon, either on the current device or all the user's devices.</summary>
	///     <remarks>To be added.</remarks>
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/CloudKit/Reference/CKModifyBadgeOperation_class/index.html">Apple documentation for <c>CKModifyBadgeOperation</c></related>
	[Register ("CKModifyBadgeOperation", SkipRegistration = true)]
#if NET
	[UnsupportedOSPlatform ("ios", "Modifying badge counts is no longer supported.")]
	[UnsupportedOSPlatform ("macos", "Modifying badge counts is no longer supported.")]
	[UnsupportedOSPlatform ("tvos", "Modifying badge counts is no longer supported.")]
	[UnsupportedOSPlatform ("maccatalyst", "Modifying badge counts is no longer supported.")]
#else
	[Deprecated (PlatformName.MacOSX, 15, 0, message: "Modifying badge counts is no longer supported.")]
	[Deprecated (PlatformName.iOS, 18, 0, message: "Modifying badge counts is no longer supported.")]
	[Deprecated (PlatformName.TvOS, 18, 0, message: "Modifying badge counts is no longer supported.")]
#endif
	[EditorBrowsable (EditorBrowsableState.Never)]
	public class CKModifyBadgeOperation : CKOperation {
		/// <summary>The handle for this class.</summary>
		///         <value>The pointer to the Objective-C class.</value>
		///         <remarks>Each Xamarin.iOS class mirrors an unmanaged Objective-C class.   This value contains the pointer to the Objective-C class, it is similar to calling objc_getClass with the object name.</remarks>
		public override NativeHandle ClassHandle { get => throw new PlatformNotSupportedException (Constants.TypeRemovedAllPlatforms); }

		/// <summary>Default constructor, initializes a new instance of this class.</summary>
		///         <remarks>To be added.</remarks>
		public CKModifyBadgeOperation () : base (NSObjectFlag.Empty)
		{
			throw new PlatformNotSupportedException (Constants.TypeRemovedAllPlatforms);
		}

		/// <include file="../../docs/api/CloudKit/CKModifyBadgeOperation.xml" path="/Documentation/Docs[@DocId='M:CloudKit.CKModifyBadgeOperation.#ctor(Foundation.NSObjectFlag)']/*" />
		protected CKModifyBadgeOperation (NSObjectFlag t) : base (t)
		{
			throw new PlatformNotSupportedException (Constants.TypeRemovedAllPlatforms);
		}

		protected internal CKModifyBadgeOperation (NativeHandle handle) : base (handle)
		{
			throw new PlatformNotSupportedException (Constants.TypeRemovedAllPlatforms);
		}

		public CKModifyBadgeOperation (nuint badgeValue)
			: base (NSObjectFlag.Empty)
		{
			throw new PlatformNotSupportedException (Constants.TypeRemovedAllPlatforms);
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public virtual nuint BadgeValue {
			get {
				throw new PlatformNotSupportedException (Constants.TypeRemovedAllPlatforms);
			}
			set {
				throw new PlatformNotSupportedException (Constants.TypeRemovedAllPlatforms);
			}
		}

		/// <summary>Gets or sets the action that is run after the operation completes.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		public unsafe virtual global::System.Action<NSError>? Completed {
			get {
				throw new PlatformNotSupportedException (Constants.TypeRemovedAllPlatforms);
			}
			set {
				throw new PlatformNotSupportedException (Constants.TypeRemovedAllPlatforms);
			}
		}
	} /* class CKModifyBadgeOperation */
}
#endif // !XAMCORE_5_0
