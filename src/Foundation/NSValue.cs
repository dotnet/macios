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

using System.Drawing;

#nullable enable

namespace Foundation {
	public partial class NSValue : NSObject {
#if !COREBUILD
		/// <summary>Gets the Objective-C type encoding string for the data contained in the receiver.</summary>
		/// <value>A C string containing the Objective-C type encoding for the data contained in the receiver.</value>
		public string? ObjCType {
			get {
				return Marshal.PtrToStringAnsi (ObjCTypePtr ());
			}
		}

		/// <summary>Creates an <see cref="NSValue" /> that wraps a <see cref="RectangleF" /> object.</summary>
		/// <param name="rect">The rectangle to wrap.</param>
		/// <returns>A new <see cref="NSValue" /> instance containing the specified rectangle.</returns>
		public static NSValue FromRectangleF (RectangleF rect)
		{
			return FromCGRect (rect);
		}

		/// <summary>Creates an <see cref="NSValue" /> that wraps a <see cref="SizeF" /> object.</summary>
		/// <param name="size">The size to wrap.</param>
		/// <returns>A new <see cref="NSValue" /> instance containing the specified size.</returns>
		public static NSValue FromSizeF (SizeF size)
		{
			return FromCGSize (size);
		}

		/// <summary>Creates an <see cref="NSValue" /> that wraps a <see cref="PointF" /> object.</summary>
		/// <param name="point">The point to wrap.</param>
		/// <returns>A new <see cref="NSValue" /> instance containing the specified point.</returns>
		public static NSValue FromPointF (PointF point)
		{
			return FromCGPoint (point);
		}

		/// <summary>Gets the <see cref="RectangleF" /> value wrapped by this <see cref="NSValue" /> object.</summary>
		/// <value>The rectangle value.</value>
		public RectangleF RectangleFValue {
			get { return (RectangleF) CGRectValue; }
		}

		/// <summary>Gets the <see cref="SizeF" /> value wrapped by this <see cref="NSValue" /> object.</summary>
		/// <value>The size value.</value>
		public SizeF SizeFValue {
			get { return (SizeF) CGSizeValue; }
		}

		/// <summary>Gets the <see cref="PointF" /> value wrapped by this <see cref="NSValue" /> object.</summary>
		/// <value>The point value.</value>
		public PointF PointFValue {
			get { return (PointF) CGPointValue; }
		}
#endif
	}
}
