// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

using Foundation;

#nullable enable

namespace ObjCRuntime {

	/// <summary>
	///   Represents the Objective-C <c>objc_super</c> structure used for super message sends.
	/// </summary>
	/// <remarks>
	///   <para>
	///     This struct is intended to be stack-allocated and passed by pointer to
	///     <c>objc_msgSendSuper</c> variants. The second field (<c>classHandle</c>)
	///     must be the receiver's class (i.e. <see cref="NSObject.ClassHandle" />), not the
	///     superclass, because the Objective-C runtime resolves the superclass internally.
	///   </para>
	/// </remarks>
	[StructLayout (LayoutKind.Sequential)]
	[EditorBrowsable (EditorBrowsableState.Never)]
	public readonly ref struct ObjCSuper {
		readonly NativeHandle receiver;
		readonly NativeHandle classHandle;

		/// <summary>Creates a new <see cref="ObjCSuper" /> for the specified object.</summary>
		/// <param name="obj">The object to create the super struct for.</param>
		public ObjCSuper (NSObject obj)
		{
			ArgumentNullException.ThrowIfNull (obj);
#if COREBUILD
			receiver = NativeHandle.Zero;
			classHandle = NativeHandle.Zero;
#else
			receiver = obj.Handle;
			classHandle = obj.ClassHandle;
#endif
		}
	}
}
