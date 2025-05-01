//
// CBUUID helpers
//
// Authors:
//	Sebastien Pouliot <sebastien@xamarin.com>
//  Aaron Bockover <abock@xamarin.com>
//
// Copyright 2012-2014 Xamarin Inc. All rights reserved.
//

using System;
using System.Text;
using Foundation;

#nullable enable

namespace CoreBluetooth {

	public partial class CBUUID : IEquatable<CBUUID> {

		const string format16bits = "{0:x2}{1:x2}";
		const string format32bits = "{0:x2}{1:x2}{2:x2}{3:x2}";
		const string format128bits = "{0:x2}{1:x2}{2:x2}{3:x2}-0000-1000-8000-00805f9b34fb";

		const ulong highServiceBits = 0xfb349b5f80000080UL;
		const ulong lowServiceMask = 0x0010000000000000UL;

		/// <param name="bytes">Array of 2, 4 or 16 bytes containing the universal unique identifier.</param>
		///         <summary>Creates a new CBUIID from the specified array of bytes.</summary>
		///         <returns>New instance;   Throws an exception if the bytes array is null or is not 2, 4 or 16 bytes.</returns>
		///         <remarks>
		/// 	  Creates a CBUUID from the specified array of bytes.
		/// 	</remarks>
		public static CBUUID FromBytes (byte [] bytes)
		{
			if (bytes is null) {
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (bytes));
			} else if (bytes.Length != 2 && bytes.Length != 4 && bytes.Length != 16) {
				throw new ArgumentException ("must either be 2, 4, or 16 bytes long", "bytes");
			}

			using (var data = NSData.FromArray (bytes))
				return CBUUID.FromData (data);
		}

		/// <param name="servicePart">16-bit service part.</param>
		///         <summary>Creates a new CBUUID for a commonly used CoreBluetooth service.</summary>
		///         <returns>
		///         </returns>
		///         <remarks>
		///           <para>
		/// 	    While CBUUID objects are 128-bit long, many common services are created just by specifying using 16 bits.
		/// 	  </para>
		///           <para>
		/// 	    For example, if the service part is (ushort)1234, then the CBUUID becomes: (CBUUID)00001234-0000-1000-8000-00805f9b34fb.
		/// 	  </para>
		///         </remarks>
		public static CBUUID FromPartial (ushort servicePart)
		{
			return FromBytes (new [] {
				(byte)(servicePart >> 8),
				(byte)servicePart,
			});
		}

		// allow roundtripping CBUUID.FromString (uuid.ToString ());
		// without string operations, ref: bug #7986
		/// <summary>Returns a string representation of the value of the current instance.</summary>
		///         <returns>
		///         </returns>
		///         <remarks>
		///         </remarks>
		public override string ToString ()
		{
			return ToString (false);
		}

		// note: having IEquatable<CBUUID> should be enough IMO
		/// <param name="a">First CBUUID to compare.</param>
		/// <param name="b">Second CBUUID to compare.</param>
		/// <summary>Determines if two CBUUID are equal.</summary>
		/// <returns>
		///         </returns>
		/// <remarks>True if they are equal (either because they are the same object, or because they represent the same UUID value).</remarks>
		public static bool operator == (CBUUID a, CBUUID b)
		{
			if (ReferenceEquals (a, null)) {
				return ReferenceEquals (b, null);
			}

			return a.Equals (b);
		}

		public static bool operator != (CBUUID a, CBUUID b)
		{
			return !(a == b);
		}

		// to satisfy IEquatable<T>
		/// <param name="obj">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public unsafe bool Equals (CBUUID? obj)
		{
			return base.Equals (obj);
		}

		// base class Equals is good enough
		// this fixes a compiler warning: CS0660: `CoreBluetooth.CBUUID' defines operator == or operator != but does not override Object.Equals(object o)
		/// <param name="obj">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public override bool Equals (object obj)
		{
			return base.Equals (obj);
		}

		// base class GetHashCode is good enough
		// this fixes a compiler warning: CS0661: `CoreBluetooth.CBUUID' defines operator == or operator != but does not override Object.GetHashCode()
		/// <summary>Generates a hash code for the current instance.</summary>
		///         <returns>A int containing the hash code for this instance.</returns>
		///         <remarks>The algorithm used to generate the hash code is unspecified.</remarks>
		public override int GetHashCode ()
		{
			return base.GetHashCode ();
		}

		/// <param name="fullUuid">If true, this renders 16-bit UUIS as a 128-bit constant, otherwise they are rendered as a 16-bit one.   128-bit UUIDS are always rendered as 128-bit values.</param>
		///         <summary>Returns a string representation of the value of the current instance.</summary>
		///         <returns>
		///         </returns>
		///         <remarks>
		///         </remarks>
		public unsafe string ToString (bool fullUuid)
		{
			NSData d = Data;
			if (d is null)
				return String.Empty;

			StringBuilder sb = new StringBuilder ();
			byte* p = (byte*) d.Bytes;

			switch (d.Length) {
			case 2:
				if (fullUuid) {
					sb.AppendFormat (format128bits, 0, 0, *p++, *p++);
				} else {
					sb.AppendFormat (format16bits, *p++, *p++);
				}
				break;
			case 4:
				sb.AppendFormat (fullUuid ? format128bits : format32bits, *p++, *p++, *p++, *p++);
				break;
			case 16:
				sb.AppendFormat ("{0:x2}{1:x2}{2:x2}{3:x2}-{4:x2}{5:x2}-{6:x2}{7:x2}-{8:x2}{9:x2}-{10:x2}{11:x2}{12:x2}{13:x2}{14:x2}{15:x2}",
					*p++, *p++, *p++, *p++, *p++, *p++, *p++, *p++,
					*p++, *p++, *p++, *p++, *p++, *p++, *p++, *p);
				break;
			}

			return sb.ToString ();
		}
	}
}
