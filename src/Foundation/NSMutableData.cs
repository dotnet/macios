//
// NSMutableData.cs:
// Author:
//   Miguel de Icaza

// Copyright 2010, Novell, Inc.
// Copyright 2013-2014 Xamarin Inc (http://www.xamarin.com)

#nullable enable

using System;
using System.Runtime.InteropServices;
using System.Collections;
using System.Collections.Generic;

namespace Foundation {
	public partial class NSMutableData : IEnumerable, IEnumerable<byte> {

		public override byte this [nint idx] {
			set {
				if (idx < 0 || (ulong) idx > Length)
					throw new ArgumentException (nameof (idx));
				Marshal.WriteByte (new IntPtr (((long) Bytes) + idx), value);
			}
		}

		/// <param name="bytes">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public void AppendBytes (byte [] bytes)
		{
			if (bytes is null)
				throw new ArgumentNullException (nameof (bytes));

			unsafe {
				fixed (byte* p = bytes) {
					AppendBytes ((IntPtr) p, (nuint) bytes.Length);
				}
			}
		}

		/// <param name="bytes">To be added.</param>
		/// <param name="start">To be added.</param>
		/// <param name="len">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		public void AppendBytes (byte [] bytes, nint start, nint len)
		{
			if (bytes is null)
				throw new ArgumentNullException (nameof (bytes));

			if (start < 0 || start > bytes.Length)
				throw new ArgumentException (nameof (start));
			if (start + len > bytes.Length)
				throw new ArgumentException (nameof (len));

			unsafe {
				fixed (byte* p = &bytes [start]) {
					AppendBytes ((IntPtr) p, (nuint) len);
				}
			}
		}

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		IEnumerator IEnumerable.GetEnumerator ()
		{
			IntPtr source = Bytes;
			nuint top = Length;

			for (nuint i = 0; i < top; i++) {
				if (source == Bytes && top == Length)
					yield return Marshal.ReadByte (source, (int) i);
				else
					throw new InvalidOperationException ("The NSMutableData has changed");
			}
		}

		/// <summary>Gets an enumerator for the bytes in the mutable data.</summary>
		/// <returns>An enumerator for the bytes in the mutable data.</returns>
		IEnumerator<byte> IEnumerable<byte>.GetEnumerator ()
		{
			IntPtr source = Bytes;
			nuint top = Length;

			for (nuint i = 0; i < top; i++) {
				if (source == Bytes && top == Length)
					yield return Marshal.ReadByte (source, (int) i);
				else
					throw new InvalidOperationException ("The NSMutableData has changed");
			}
		}
	}
}
