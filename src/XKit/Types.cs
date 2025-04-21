//
// Copyright 2019 Microsoft Inc
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
using System.Runtime.InteropServices;

using ObjCRuntime;
using Foundation;
using CoreGraphics;

#if MONOMAC
namespace AppKit {
#else
namespace UIKit {
#endif

#if NET
	/// <summary>Edge insets that account for text direction.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("tvos")]
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
#endif
	[StructLayout (LayoutKind.Sequential)]
	public struct NSDirectionalEdgeInsets {

		// API match for NSDirectionalEdgeInsetsZero field/constant
		/// <summary>Gets an NSDirectionalEdgeInsets that has zero top, leading, bottom, and trailing insets.</summary>
		///         <remarks>To be added.</remarks>
		[Field ("NSDirectionalEdgeInsetsZero")] // fake (but helps testing and could also help documentation)
		public static readonly NSDirectionalEdgeInsets Zero;

		/// <summary>The top inset.</summary>
		///         <remarks>To be added.</remarks>
		public nfloat Top;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public nfloat Leading;
		/// <summary>The bottom edge inset.</summary>
		///         <remarks>To be added.</remarks>
		public nfloat Bottom;
		/// <summary>The trailing inset.</summary>
		///         <remarks>To be added.</remarks>
		public nfloat Trailing;

#if !COREBUILD
		public NSDirectionalEdgeInsets (nfloat top, nfloat leading, nfloat bottom, nfloat trailing)
		{
			Top = top;
			Leading = leading;
			Bottom = bottom;
			Trailing = trailing;
		}

		// note: NSDirectionalEdgeInsetsEqualToDirectionalEdgeInsets (UIGeometry.h) is a macro
		/// <param name="other">The other edge inset object to compare.</param>
		///         <summary>Returns true if <paramref name="other" /> has the same values as this NSDirectionalEdgeInset.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public bool Equals (NSDirectionalEdgeInsets other)
		{
			if (Leading != other.Leading)
				return false;
			if (Trailing != other.Trailing)
				return false;
			if (Top != other.Top)
				return false;
			return (Bottom == other.Bottom);
		}

		/// <param name="obj">The other object to compare.</param>
		///         <summary>Returns true if <paramref name="obj" /> is an NSDirectionalEdgeInset and has the same values as this object.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public override bool Equals (object? obj)
		{
			if (obj is NSDirectionalEdgeInsets insets)
				return Equals (insets);
			return false;
		}

		public static bool operator == (NSDirectionalEdgeInsets insets1, NSDirectionalEdgeInsets insets2)
		{
			return insets1.Equals (insets2);
		}

		public static bool operator != (NSDirectionalEdgeInsets insets1, NSDirectionalEdgeInsets insets2)
		{
			return !insets1.Equals (insets2);
		}

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public override int GetHashCode ()
		{
			return HashCode.Combine (Top, Leading, Trailing, Bottom);
		}

#if !MONOMAC
		[DllImport (Constants.UIKitLibrary)]
		extern static NSDirectionalEdgeInsets NSDirectionalEdgeInsetsFromString (IntPtr /* NSString */ s);

		/// <param name="s">The string that describes the new insets.</param>
		///         <summary>Creates a new NSDirectionalEdgeInset object from a curly-braced, comma-separated list of the top, leading, bottom, and trailing inset values.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		static public NSDirectionalEdgeInsets FromString (string s)
		{
			// note: null is allowed
			var ptr = NSString.CreateNative (s);
			var value = NSDirectionalEdgeInsetsFromString (ptr);
			NSString.ReleaseNative (ptr);
			return value;
		}
#endif

#if !MONOMAC
		[DllImport (Constants.UIKitLibrary)]
		extern static IntPtr /* NSString */ NSStringFromDirectionalEdgeInsets (NSDirectionalEdgeInsets insets);

		// note: ensure we can roundtrip ToString into FromString
		/// <summary>Converts this object to a string that contains a curly-braced, comma-separated list of the top, leading, bottom, and trailing inset values.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public override string ToString ()
		{
			using (var ns = new NSString (NSStringFromDirectionalEdgeInsets (this)))
				return ns.ToString ();
		}
#endif
#endif
	}
}
