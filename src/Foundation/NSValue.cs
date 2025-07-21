//
// Copyright 2010, Novell, Inc.
// Copyright 2011, 2012, 2013 Xamarin Inc
// Copyright 2019 Microsoft Corporation
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using System;
#if !NO_SYSTEM_DRAWING
using System.Drawing;
#endif
using System.Runtime.InteropServices;

// Disable until we get around to enable + fix any issues.
#nullable disable

namespace Foundation {
	public partial class NSValue : NSObject {
#if !COREBUILD
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public string ObjCType {
			get {
				return Marshal.PtrToStringAnsi (ObjCTypePtr ());
			}
		}

#if !NO_SYSTEM_DRAWING
		/// <param name="rect">To be added.</param>
		///         <summary>Creates an NSValue that wraps a RectangleF object.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static NSValue FromRectangleF (RectangleF rect)
		{
			return FromCGRect (rect);
		}

		/// <param name="size">To be added.</param>
		///         <summary>Creates an NSValue that wraps a SizeF object.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static NSValue FromSizeF (SizeF size)
		{
			return FromCGSize (size);
		}

		/// <param name="point">To be added.</param>
		///         <summary>Creates an NSValue that wraps a PointF object.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static NSValue FromPointF (PointF point)
		{
			return FromCGPoint (point);
		}

		/// <summary>Returns the RectangleF value wrapped by this NSValue object.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public RectangleF RectangleFValue {
			get { return (RectangleF) CGRectValue; }
		}

		/// <summary>Returns the SizeF value wrapped by this NSValue object.</summary>
		///         <value>
		///         </value>
		///         <remarks>
		///         </remarks>
		public SizeF SizeFValue {
			get { return (SizeF) CGSizeValue; }
		}

		/// <summary>Returns the PointF value wrapped by this NSValue object.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public PointF PointFValue {
			get { return (PointF) CGPointValue; }
		}
#endif

#endif
	}
}
