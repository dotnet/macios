// 
// CGVector.cs: Implements the managed CGPDFDocument
//
// Authors:  
//     Miguel de Icaza
//
// Copyright 2013-2014 Xamarin Inc
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

#nullable enable

using System;
using System.Globalization;
using System.Runtime.InteropServices;
using Foundation;
using ObjCRuntime;
using CoreFoundation;

namespace CoreGraphics {


	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	// CGGeometry.h
	public struct CGVector {
		/// <summary>X component of the vector</summary>
		///         <remarks>To be added.</remarks>
		public /* CGFloat */ nfloat dx;
		/// <summary>Y component of the vector</summary>
		///         <remarks>To be added.</remarks>
		public /* CGFloat */ nfloat dy;

		public CGVector (nfloat dx, nfloat dy)
		{
			this.dx = dx;
			this.dy = dy;
		}

		public static bool operator == (CGVector left, CGVector right)
		{
			return left.dx == right.dx && left.dy == right.dy;
		}

		public static bool operator != (CGVector left, CGVector right)
		{
			return left.dx != right.dx || left.dy != right.dy;
		}

		/// <summary />
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public override int GetHashCode ()
		{
			return HashCode.Combine (dx, dy);
		}

		/// <param name="other">To be added.</param>
		///         <summary />
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public override bool Equals (object? other)
		{
			if (other is CGVector vector)
				return dx == vector.dx && dy == vector.dy;
			return false;
		}

#if MONOTOUCH
#if !COREBUILD
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		[UnsupportedOSPlatform ("macos")]
		[DllImport (Constants.UIKitLibrary)]
		extern static IntPtr NSStringFromCGVector (CGVector vector);

		/// <summary>String representation of the vector, suitable to be passed later to <see cref="M:CoreGraphics.CGVector.FromString(System.String)" /> method.</summary>
		///         <returns>
		///           <para />
		///         </returns>
		///         <remarks>
		///           <para />
		///         </remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		[UnsupportedOSPlatform ("macos")]
		public override string? ToString ()
		{
			return CFString.FromHandle (NSStringFromCGVector (this));
		}

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		[UnsupportedOSPlatform ("macos")]
		[DllImport (Constants.UIKitLibrary)]
		extern static CGVector CGVectorFromString (IntPtr str);

		/// <param name="s">String representation, created previously with either the <see cref="M:CoreGraphics.CGVector.ToString" /> method or serialized in the CGVector format.</param>
		///         <summary>Creates a CGVector from a stringified representation of the vector.</summary>
		///         <returns>The CGVector represented by the string representation.</returns>
		///         <remarks>
		///           <para />
		///         </remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		[UnsupportedOSPlatform ("macos")]
		static public CGVector FromString (string s)
		{
			// note: null is allowed
			var ptr = CFString.CreateNative (s);
			var value = CGVectorFromString (ptr);
			CFString.ReleaseNative (ptr);
			return value;
		}
#endif
#else // MONOMAC
		public override string ToString ()
		{
			return $"{{{dx}, {dy}}}";
		}
#endif

	}
}
