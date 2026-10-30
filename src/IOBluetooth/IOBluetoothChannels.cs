// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

#nullable enable

using System;
using System.Runtime.InteropServices;

using ObjCRuntime;

namespace IOBluetooth {

	public partial class IOBluetoothL2CAPChannel {
		/// <summary>Synchronously writes data to the channel.</summary>
		/// <param name="data">The data to write.</param>
		/// <returns>An IOReturn status code (0 for success).</returns>
		public int WriteSync (byte [] data)
		{
			if (data is null)
				throw new ArgumentNullException (nameof (data));
			if (data.Length > ushort.MaxValue)
				throw new ArgumentOutOfRangeException (nameof (data), "The data is too long.");

			var handle = GCHandle.Alloc (data, GCHandleType.Pinned);
			try {
				return WriteSync (handle.AddrOfPinnedObject (), (ushort) data.Length);
			} finally {
				handle.Free ();
			}
		}
	}

	public partial class IOBluetoothRFCOMMChannel {
		/// <summary>Synchronously writes data to the channel.</summary>
		/// <param name="data">The data to write.</param>
		/// <returns>An IOReturn status code (0 for success).</returns>
		public int WriteSync (byte [] data)
		{
			if (data is null)
				throw new ArgumentNullException (nameof (data));
			if (data.Length > ushort.MaxValue)
				throw new ArgumentOutOfRangeException (nameof (data), "The data is too long.");

			var handle = GCHandle.Alloc (data, GCHandleType.Pinned);
			try {
				return WriteSync (handle.AddrOfPinnedObject (), (ushort) data.Length);
			} finally {
				handle.Free ();
			}
		}
	}
}
