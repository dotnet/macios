//
// Copyright 2011, Novell, Inc.
// Copyright 2012 Xamarin Inc.
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

#if !__MACCATALYST__

using System;

using Foundation;
using CoreGraphics;

#nullable enable

namespace AppKit {

	public partial class NSImage {
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public CGImage CGImage {
			get {
				var rect = CGRect.Empty;
				return AsCGImage (ref rect, null, null);
			}
		}

		/// <param name="stream">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static NSImage? FromStream (System.IO.Stream stream)
		{
			using (var data = NSData.FromStream (stream)) {
				return data is null ? null : new NSImage (data);
			}
		}

		/// <summary>Create a new <see cref="NSImage" /> instance.</summary>
		/// <param name="fileName">The path of the file to load for the new <see cref="NSImage" /> instance.</param>
		/// <param name="lazy">Whether the file should be loaded right away or lazily.</param>
		public NSImage (string fileName, bool lazy)
			: base (NSObjectFlag.Empty)
		{
			if (lazy)
				InitializeHandle (_InitByReferencingFile (fileName));
			else
				InitializeHandle (_InitWithContentsOfFile (fileName));
		}

		/// <summary>Create a new <see cref="NSImage" /> instance.</summary>
		/// <param name="data">The image data for the new <see cref="NSImage" /> instance.</param>
		/// <param name="ignoresOrientation">Whether the orientation in the image data is ignored or not.</param>
		public NSImage (NSData data, bool ignoresOrientation)
			: base (NSObjectFlag.Empty)
		{
			if (ignoresOrientation) {
				InitializeHandle (_InitWithDataIgnoringOrientation (data));
			} else {
				InitializeHandle (_InitWithData (data));
			}
		}

		// note: if needed override the protected Get|Set methods
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public string? Name {
			get { return GetName (); }
			// ignore return value (bool)
			set { SetName (value); }
		}

		/// <param name="name">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static NSImage? ImageNamed (NSImageName name)
		{
			return ImageNamed (name.GetConstant ()!);
		}
	}

	public partial class NSImageRep {

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public CGImage CGImage {
			get {
				var rect = CGRect.Empty;
				return AsCGImage (ref rect, null, null);
			}
		}
	}
}
#endif // !__MACCATALYST__
