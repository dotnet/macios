// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

#nullable enable

namespace ObjCRuntime {

	/// <summary>
	///   Represents the Objective-C <c>objc_super</c> structure used for super message sends.
	/// </summary>
	/// <remarks>
	///   <para>
	///     This struct is intended to be stack-allocated and passed by pointer to
	///     <c>objc_msgSendSuper</c> variants. The second field (<see cref="ClassHandle" />)
	///     must be the receiver's class (i.e. the receiver's ClassHandle property), not the
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

		/// <summary>Creates a new <see cref="ObjCSuper" /> for the specified receiver and class handles.</summary>
		/// <param name="receiver">The receiver's native handle.</param>
		/// <param name="classHandle">The receiver's class handle.</param>
		public ObjCSuper (NativeHandle receiver, NativeHandle classHandle)
		{
			Receiver = receiver;
			ClassHandle = classHandle;
		}
	}
}
