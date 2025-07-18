// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;

using Foundation;
using ObjCRuntime;

#nullable enable

namespace FSKit {
	public partial class FSFileName {
		/// <summary>Create a new <see cref="FSFileName" /> instance using an array of bytes.</summary>
		/// <param name="bytes">The byte array to use to initialize the new <see cref="FSFileName" /> instance.</param>
		public FSFileName (byte [] bytes)
			: base (NSObjectFlag.Empty)
		{
			if (bytes is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (bytes));

			unsafe {
				fixed (byte* bytesPtr = bytes) {
					InitializeHandle (_InitWithBytes ((IntPtr) bytesPtr, (nuint) bytes.Length), "initWithBytes:length:");
				}
			}
		}

		/// <summary>Create a new <see cref="FSFileName" /> instance using an array of bytes.</summary>
		/// <param name="bytes">The byte array to use to initialize the new <see cref="FSFileName" /> instance.</param>
		/// <returns>A new <see cref="FSFileName" /> instance.</returns>
		public FSFileName Create (byte [] bytes)
		{
			if (bytes is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (bytes));

			unsafe {
				fixed (byte* bytesPtr = bytes) {
					return _Create ((IntPtr) bytesPtr, (nuint) bytes.Length);
				}
			}
		}
	}
}
