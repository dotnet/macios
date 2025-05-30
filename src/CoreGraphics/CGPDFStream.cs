// 
// CGPDFStream.cs: Implements the managed CGPDFStream binding
//
// Authors: Miguel de Icaza
//     
// Copyright 2010 Novell, Inc
// Copyright 2011-2014 Xamarin Inc
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
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using Foundation;
using ObjCRuntime;
using CoreFoundation;

namespace CoreGraphics {

	// untyped enum -> CGPDFStream.h
	/// <summary>Enumerates values that indicate the data format of a PDF.</summary>
	///     <remarks>To be added.</remarks>
	public enum CGPDFDataFormat {
		/// <summary>To be added.</summary>
		Raw,
		/// <summary>To be added.</summary>
		JPEGEncoded,
		/// <summary>To be added.</summary>
		JPEG2000,
	};

	/// <summary>A PDF Stream.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	// CGPDFStream.h
	public class CGPDFStream : CGPDFObject {
		// The lifetime management of CGPDFObject (and CGPDFArray, CGPDFDictionary and CGPDFStream) are tied to
		// the containing CGPDFDocument, and not possible to handle independently, which is why this class
		// does not subclass NativeObject (there's no way to retain/release CGPDFObject instances). It's
		// also why this constructor doesn't have a 'bool owns' parameter: it's always owned by the containing CGPDFDocument.
		internal CGPDFStream (NativeHandle handle)
			: base (handle)
		{
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static /* CGPDFDictionaryRef */ IntPtr CGPDFStreamGetDictionary (/* CGPDFStreamRef */ IntPtr stream);

		/// <summary>The dictionary associated with the PDF stream.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public CGPDFDictionary Dictionary {
			get {
				return new CGPDFDictionary (CGPDFStreamGetDictionary (Handle));
			}
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		unsafe extern static /* CFDataRef */ IntPtr CGPDFStreamCopyData (/* CGPDFStreamRef */ IntPtr stream, /* CGPDFDataFormat* */ CGPDFDataFormat* format);

		/// <param name="format">To be added.</param>
		///         <summary>The data associated with the PDF stream, and also returns the file format of the data.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public NSData? GetData (out CGPDFDataFormat format)
		{
			format = default;
			unsafe {
				IntPtr obj = CGPDFStreamCopyData (Handle, (CGPDFDataFormat*) Unsafe.AsPointer<CGPDFDataFormat> (ref format));
				return Runtime.GetNSObject<NSData> (obj, true);
			}
		}
	}
}
