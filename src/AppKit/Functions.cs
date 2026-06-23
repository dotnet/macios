//
// Copyright 2010, Joe Mattiello
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

using CoreGraphics;
using CoreImage;
using CoreAnimation;
using CoreText;

#nullable enable

namespace AppKit {

#if MONOMAC
	// Class to access C functions
	/// <summary>To be added.</summary>
	///     <remarks>To be added.</remarks>
	public partial class AppKitFramework {

		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[DllImport (Constants.AppKitLibrary)]
		public static extern void NSBeep ();

		[DllImport (Constants.AppKitLibrary, EntryPoint = "NSTextAlignmentToCTTextAlignment")]
		static extern CTTextAlignment NSTextAlignmentToCTTextAlignmentInternal (nint nsTextAlignment);

		/// <summary>Converts an <see cref="NSTextAlignment" /> value to its equivalent <see cref="CoreText.CTTextAlignment" /> value.</summary>
		/// <param name="nsTextAlignment">The text alignment to convert.</param>
		/// <returns>The equivalent Core Text alignment.</returns>
		public static CTTextAlignment NSTextAlignmentToCTTextAlignment (NSTextAlignment nsTextAlignment)
			=> NSTextAlignmentToCTTextAlignmentInternal ((nint) NSTextAlignmentExtensions.ToNative (nsTextAlignment));

		[DllImport (Constants.AppKitLibrary, EntryPoint = "NSTextAlignmentFromCTTextAlignment")]
		static extern nint NSTextAlignmentFromCTTextAlignmentInternal (CTTextAlignment ctTextAlignment);

		/// <summary>Converts a <see cref="CoreText.CTTextAlignment" /> value to its equivalent <see cref="NSTextAlignment" /> value.</summary>
		/// <param name="ctTextAlignment">The Core Text alignment to convert.</param>
		/// <returns>The equivalent text alignment.</returns>
		public static NSTextAlignment NSTextAlignmentFromCTTextAlignment (CTTextAlignment ctTextAlignment)
			=> NSTextAlignmentExtensions.ToManaged ((nuint) NSTextAlignmentFromCTTextAlignmentInternal (ctTextAlignment));
	}
#endif
}
