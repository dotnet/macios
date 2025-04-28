//
// UIBezierPath.cs: Extra methods for UIBezierPath
//
// Authors:
//   Miguel de Icaza
//
// Copyright 2014-2015, Xamarin Inc
//

using System;
using System.Collections;
using ObjCRuntime;
using Foundation;

namespace UIKit {
	public partial class UIBezierPath {

		// from AppKit/NSBezierPath.cs
		/// <param name="pattern">To be added.</param>
		/// <param name="phase">To be added.</param>
		/// <summary>Stores the stroking pattern and phase in the provided <see langword="out" /> parameters.</summary>
		/// <remarks>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="threads">This can be used from a background thread.</para>
		///         </remarks>
		public unsafe void GetLineDash (out nfloat [] pattern, out nfloat phase)
		{
			nint length;

			//Call the internal method with null to get the length of the pattern array
			_GetLineDash (IntPtr.Zero, out length, out phase);

			pattern = new nfloat [length];
			fixed (nfloat* ptr = pattern)
				_GetLineDash ((IntPtr) ptr, out length, out phase);
		}

		/// <param name="values">An array of lenghts that alternate between the solid portion and the gap portion, measured in points.</param>
		/// <param name="phase">Point at which the pattern starts to get rendered (counting from the origin).</param>
		/// <summary>Sets line stroking pattern for the path.</summary>
		/// <remarks>
		///           <para tool="threads">This can be used from a background thread.</para>
		///         </remarks>
		public void SetLineDash (nfloat [] values, nfloat phase)
		{
			if (values is null) {
				SetLineDash (IntPtr.Zero, 0, phase);
				return;
			}
			unsafe {
				fixed (nfloat* fp = values) {
					SetLineDash ((IntPtr) fp, values.Length, phase);
				}
			}
		}

		/// <param name="values">An array of lenghts that alternate between the solid portion and the gap portion, measured in points.</param>
		/// <param name="offset">Offset into the values to start rendering from.</param>
		/// <param name="count">Number of items in the array to consider.</param>
		/// <param name="phase">Point at which the pattern starts to get rendered (counting from the origin).</param>
		/// <summary>Sets the dash pattern for the line.</summary>
		/// <remarks>
		///           <para>This variation of the method allows a segment of the "values" array to be specified via the offset and count parameters.</para>
		///           <para tool="threads">This can be used from a background thread.</para>
		///         </remarks>
		public void SetLineDash (nfloat [] values, nint offset, nint count, nfloat phase)
		{
			if (offset + count > values.Length)
				throw new ArgumentException ("the provided offset and count would access data beyond the values array");

			unsafe {
				fixed (nfloat* fp = &values [offset]) {
					SetLineDash ((IntPtr) fp, count, phase);
				}
			}
		}
	}

}
