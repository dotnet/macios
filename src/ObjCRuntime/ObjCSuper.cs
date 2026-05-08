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
	///     <c>objc_msgSendSuper</c> variants. The second field (<see cref="ClassHandle" />)
	///     must be the receiver's class (i.e. <see cref="NSObject.ClassHandle" />), not the
	///     superclass, because the Objective-C runtime resolves the superclass internally.
	///   </para>
	/// </remarks>
	[StructLayout (LayoutKind.Sequential)]
	[EditorBrowsable (EditorBrowsableState.Never)]
	public readonly ref struct ObjCSuper {
		/// <summary>The receiver's native handle.</summary>
		public readonly NativeHandle Receiver;
		/// <summary>The receiver's class handle (used by the runtime to find the superclass implementation).</summary>
		public readonly NativeHandle ClassHandle;

		/// <summary>Creates a new <see cref="ObjCSuper" /> for the specified object.</summary>
		/// <param name="obj">The object to create the super struct for.</param>
		public ObjCSuper (NSObject obj)
		{
			ArgumentNullException.ThrowIfNull (obj);
#if COREBUILD
			Receiver = NativeHandle.Zero;
			ClassHandle = NativeHandle.Zero;
#else
			Receiver = obj.Handle;
			ClassHandle = obj.ClassHandle;
#endif
		}
	}
}
