// 
// CGPDFDocument.cs: Implements the managed CGPDFDocument
//
// Authors: Mono Team
//     
// Copyright 2009 Novell, Inc
// Copyright 2011, 2012 Xamarin Inc
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
using Foundation;
using ObjCRuntime;
using CoreFoundation;

namespace CoreGraphics {
	/// <summary>PDF Document.</summary>
	///     <remarks>To be added.</remarks>
	///     <related type="sample" href="https://github.com/xamarin/ios-samples/tree/master/QuartzSample/">QuartzSample</related>
	///     <related type="sample" href="https://github.com/xamarin/ios-samples/tree/master/ZoomingPdfViewer/">ZoomingPdfViewer</related>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	public class CGPDFDocument : NativeObject {
#if !COREBUILD
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGPDFDocumentRelease (/* CGPDFDocumentRef */ IntPtr document);

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static /* CGPDFDocumentRef */ IntPtr CGPDFDocumentRetain (/* CGPDFDocumentRef */ IntPtr document);

		[Preserve (Conditional = true)]
		internal CGPDFDocument (NativeHandle handle, bool owns)
			: base (handle, owns)
		{
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static /* CGPDFDocumentRef */ IntPtr CGPDFDocumentCreateWithProvider (/* CGDataProviderRef */ IntPtr provider);

		/// <param name="provider">Data provider.</param>
		///         <summary>Creates a CGPDFDocument from a data provider, typically an array of bytes.</summary>
		///         <remarks>You can use this to create PDF documents dynamically.   CGDataProviders can deliver the data either from a block of memory or from the contents of a file.</remarks>
		public CGPDFDocument (CGDataProvider provider)
			: base (CGPDFDocumentCreateWithProvider (provider.GetNonNullHandle (nameof (provider))), true)
		{
			GC.KeepAlive (provider);
		}

		protected internal override void Retain ()
		{
			CGPDFDocumentRetain (GetCheckedHandle ());
		}

		protected internal override void Release ()
		{
			CGPDFDocumentRelease (GetCheckedHandle ());
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static /* CGPDFDocumentRef */ IntPtr CGPDFDocumentCreateWithURL (/* CFURLRef */ IntPtr url);

		/// <param name="str">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static CGPDFDocument? FromFile (string str)
		{
			using (var url = CFUrl.FromFile (str)) {
				if (url is null)
					return null;
				IntPtr handle = CGPDFDocumentCreateWithURL (url.Handle);
				if (handle == IntPtr.Zero)
					return null;
				return new CGPDFDocument (handle, true);
			}

		}

		/// <param name="str">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static CGPDFDocument? FromUrl (string str)
		{
			using (var url = CFUrl.FromUrlString (str, null)) {
				if (url is null)
					return null;
				IntPtr handle = CGPDFDocumentCreateWithURL (url.Handle);
				if (handle == IntPtr.Zero)
					return null;
				return new CGPDFDocument (handle, true);
			}
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static /* size_t */ nint CGPDFDocumentGetNumberOfPages (/* CGPDFDocumentRef */ IntPtr document);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public nint Pages {
			get {
				return CGPDFDocumentGetNumberOfPages (Handle);
			}
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static /* CGPDFPageRef */ IntPtr CGPDFDocumentGetPage (/* CGPDFDocumentRef */ IntPtr document, /* size_t */ nint page);

		public CGPDFPage? GetPage (nint page)
		{
			var h = CGPDFDocumentGetPage (Handle, page);
			return h == IntPtr.Zero ? null : new CGPDFPage (h, false);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		unsafe extern static void CGPDFDocumentGetVersion (/* CGPDFDocumentRef */ IntPtr document, /* int* */ int* majorVersion, /* int* */ int* minorVersion);

		/// <param name="major">To be added.</param>
		///         <param name="minor">To be added.</param>
		///         <summary>Gets the version of <c>this</c> <see cref="CoreGraphics.CGPDFDocument" /> object, including the <paramref name="major" /> and <paramref name="minor" /> version numbers.</summary>
		///         <remarks>To be added.</remarks>
		public void GetVersion (out int major, out int minor)
		{
			major = default;
			minor = default;
			unsafe {
				CGPDFDocumentGetVersion (Handle, (int*) Unsafe.AsPointer<int> (ref major), (int*) Unsafe.AsPointer<int> (ref minor));
			}
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static byte CGPDFDocumentIsEncrypted (/* CGPDFDocumentRef */ IntPtr document);

		/// <summary>Gets whether <c>this</c> <see cref="CoreGraphics.CGPDFDocument" /> object is encrypted.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public bool IsEncrypted {
			get {
				return CGPDFDocumentIsEncrypted (Handle) != 0;
			}
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static byte CGPDFDocumentUnlockWithPassword (/* CGPDFDocumentRef */ IntPtr document, /* const char* */ IntPtr password);

		/// <param name="password">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public bool Unlock (string password)
		{
			using var passwordPtr = new TransientString (password);
			return CGPDFDocumentUnlockWithPassword (Handle, passwordPtr) != 0;
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static byte CGPDFDocumentIsUnlocked (/* CGPDFDocumentRef */ IntPtr document);

		/// <summary>Gets whether the <c>this</c> <see cref="CoreGraphics.CGPDFDocument" /> object is not locked, either because the object is not encrypted or a password has been supplied.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public bool IsUnlocked {
			get {
				return CGPDFDocumentIsUnlocked (Handle) != 0;
			}
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static byte CGPDFDocumentAllowsPrinting (/* CGPDFDocumentRef */ IntPtr document);

		/// <summary>Gets whether <c>this</c> <see cref="CoreGraphics.CGPDFDocument" /> object allows printing.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public bool AllowsPrinting {
			get {
				return CGPDFDocumentAllowsPrinting (Handle) != 0;
			}
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static byte CGPDFDocumentAllowsCopying (/* CGPDFDocumentRef */ IntPtr document);

		/// <summary>Gets whether <c>this</c> <see cref="CoreGraphics.CGPDFDocument" /> object allows copying.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public bool AllowsCopying {
			get {
				return CGPDFDocumentAllowsCopying (Handle) != 0;
			}
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static /* CGPDFDictionaryRef */ IntPtr CGPDFDocumentGetCatalog (/* CGPDFDocumentRef */ IntPtr document);
		/// <summary>Gets the catalog for <c>this</c> <see cref="CoreGraphics.CGPDFDocument" /> object.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public CGPDFDictionary GetCatalog ()
		{
			return new CGPDFDictionary (CGPDFDocumentGetCatalog (Handle));
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static /* CGPDFDictionaryRef */ IntPtr CGPDFDocumentGetInfo (/* CGPDFDocumentRef */ IntPtr document);

		/// <summary>Gets information for <c>this</c> <see cref="CoreGraphics.CGPDFDocument" /> as a <see cref="CoreGraphics.CGPDFDictionary" /> dictionary.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public CGPDFDictionary GetInfo ()
		{
			return new CGPDFDictionary (CGPDFDocumentGetInfo (Handle));
		}

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("maccatalyst")]
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGPDFContextSetOutline (/* CGPDFDocumentRef */ IntPtr document, IntPtr /* dictionary */ outline);

		/// <param name="options">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("maccatalyst")]
		public void SetOutline (CGPDFOutlineOptions? options)
		{
			CGPDFContextSetOutline (Handle, options.GetHandle ());
		}

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("maccatalyst")]
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static /* CFDictionaryPtry */ IntPtr CGPDFDocumentGetOutline (/* CGPDFDocumentRef */ IntPtr document);

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("maccatalyst")]
		public CGPDFOutlineOptions GetOutline ()
		{
			var ptr = CGPDFDocumentGetOutline (Handle);
			return new CGPDFOutlineOptions (Runtime.GetNSObject<NSDictionary> (ptr));
		}

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("maccatalyst")]
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static CGPDFAccessPermissions CGPDFDocumentGetAccessPermissions (IntPtr document);

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("maccatalyst")]
		public CGPDFAccessPermissions GetAccessPermissions ()
		{
			return CGPDFDocumentGetAccessPermissions (Handle);
		}

#endif // !COREBUILD
	}
}
