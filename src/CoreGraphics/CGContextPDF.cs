// 
// CGContextPDF.cs: Implements the managed CGContextPDF
//
// Authors: Mono Team
//     
// Copyright 2009-2010 Novell, Inc
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
using System.Runtime.InteropServices;

using ObjCRuntime;
using Foundation;
using CoreFoundation;

namespace CoreGraphics {

	public partial class CGPDFPageInfo {

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public CGRect? MediaBox { get; set; }
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public CGRect? CropBox { get; set; }
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public CGRect? BleedBox { get; set; }
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public CGRect? TrimBox { get; set; }
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public CGRect? ArtBox { get; set; }

		static void Add (NSMutableDictionary dict, IntPtr key, CGRect? val)
		{
			if (!val.HasValue)
				return;
			NSData data;
			unsafe {
				CGRect f = val.Value;
				CGRect* pf = &f;
				data = NSData.FromBytes ((IntPtr) pf, 16);
			}
			dict.LowlevelSetObject (data, key);
		}

		internal virtual NSMutableDictionary ToDictionary ()
		{
			var ret = new NSMutableDictionary ();
			Add (ret, kCGPDFContextMediaBox, MediaBox);
			Add (ret, kCGPDFContextCropBox, CropBox);
			Add (ret, kCGPDFContextBleedBox, BleedBox);
			Add (ret, kCGPDFContextTrimBox, TrimBox);
			Add (ret, kCGPDFContextArtBox, ArtBox);
			return ret;
		}
	}

	public partial class CGPDFInfo : CGPDFPageInfo {

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public string? Title { get; set; }
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public string? Author { get; set; }
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public string? Subject { get; set; }
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public string []? Keywords { get; set; }
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public string? Creator { get; set; }
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public string? OwnerPassword { get; set; }
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public string? UserPassword { get; set; }
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public int? EncryptionKeyLength { get; set; }
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public bool? AllowsPrinting { get; set; }
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public bool? AllowsCopying { get; set; }
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public CGPDFAccessPermissions? AccessPermissions { get; set; }
		//public NSDictionary OutputIntent { get; set; }
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios14.0")]
		[SupportedOSPlatform ("tvos14.0")]
		[SupportedOSPlatform ("maccatalyst")]
		public bool? CreateLinearizedPdf { get; set; }

		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios14.0")]
		[SupportedOSPlatform ("tvos14.0")]
		[SupportedOSPlatform ("maccatalyst")]
		public bool? CreatePdfA2u { get; set; }

		internal override NSMutableDictionary ToDictionary ()
		{
			var ret = base.ToDictionary ();

			if (Title is not null)
				ret.LowlevelSetObject (Title, kCGPDFContextTitle);
			if (Author is not null)
				ret.LowlevelSetObject (Author, kCGPDFContextAuthor);
			if (Subject is not null)
				ret.LowlevelSetObject (Subject, kCGPDFContextSubject);
			if (Keywords is not null && Keywords.Length > 0) {
				if (Keywords.Length == 1)
					ret.LowlevelSetObject (Keywords [0], kCGPDFContextKeywords);
				else
					ret.LowlevelSetObject (NSArray.FromStrings (Keywords), kCGPDFContextKeywords);
			}
			if (Creator is not null)
				ret.LowlevelSetObject (Creator, kCGPDFContextCreator);
			if (OwnerPassword is not null)
				ret.LowlevelSetObject (OwnerPassword, kCGPDFContextOwnerPassword);
			if (UserPassword is not null)
				ret.LowlevelSetObject (UserPassword, kCGPDFContextUserPassword);
			if (EncryptionKeyLength.HasValue)
				ret.LowlevelSetObject (NSNumber.FromInt32 (EncryptionKeyLength.Value), kCGPDFContextEncryptionKeyLength);
			if (AllowsPrinting.HasValue && AllowsPrinting.Value == false)
				ret.LowlevelSetObject (CFBoolean.FalseHandle, kCGPDFContextAllowsPrinting);
			if (AllowsCopying.HasValue && AllowsCopying.Value == false)
				ret.LowlevelSetObject (CFBoolean.FalseHandle, kCGPDFContextAllowsCopying);
			if (AccessPermissions.HasValue)
				ret.LowlevelSetObject (NSNumber.FromInt32 ((int) AccessPermissions.Value), kCGPDFContextAccessPermissions);
#pragma warning disable CA1416 // This call site is reachable on: 'ios' 12.2 and later, 'maccatalyst' 12.2 and later, 'macOS/OSX' 12.0 and later, 'tvos' 12.2 and later. 'CGPDFInfo.CreateLinearizedPdf' is only supported on: 'ios' 14.0 and later, 'tvos' 14.0 and later.
			// only set the keys if they exists in the current OS version
			if ((kCGPDFContextCreateLinearizedPDF != IntPtr.Zero) && CreateLinearizedPdf.HasValue)
				ret.LowlevelSetObject (CFBoolean.ToHandle (CreateLinearizedPdf.Value), kCGPDFContextCreateLinearizedPDF);
#pragma warning restore CA1416
#pragma warning disable CA1416 // This call site is reachable on: 'ios' 12.2 and later, 'maccatalyst' 12.2 and later, 'macOS/OSX' 12.0 and later, 'tvos' 12.2 and later. 'CGPDFInfo.kCGPDFContextCreatePDFA' is only supported on: 'ios' 14.0 and later, 'tvos' 14.0 and later.
			// default to kCFBooleanFalse
			if ((kCGPDFContextCreatePDFA != IntPtr.Zero) && CreatePdfA2u.HasValue && CreatePdfA2u == true)
				ret.LowlevelSetObject (CFBoolean.TrueHandle, kCGPDFContextCreatePDFA);
#pragma warning restore CA1416
			return ret;
		}
	}

	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	public class CGContextPDF : CGContext {
		bool closed;

		[DllImport (Constants.CoreGraphicsLibrary)]
		unsafe extern static /* CGContextRef */ IntPtr CGPDFContextCreateWithURL (/* CFURLRef */ IntPtr url, CGRect* mediaBox, /* CFDictionaryRef */ IntPtr auxiliaryInfo);

		[DllImport (Constants.CoreGraphicsLibrary)]
		unsafe extern static /* CGContextRef */ IntPtr CGPDFContextCreate (/* CGDataConsumerRef */ IntPtr consumer, CGRect* mediaBox, /* CFDictionaryRef */ IntPtr auxiliaryInfo);

		unsafe static IntPtr Create (CGDataConsumer? dataConsumer, CGRect* mediaBox, CGPDFInfo? info)
		{
			using var dict = info?.ToDictionary ();
			IntPtr result = CGPDFContextCreate (dataConsumer.GetHandle (), mediaBox, dict.GetHandle ());
			GC.KeepAlive (dataConsumer);
			return result;
		}

		unsafe CGContextPDF (CGDataConsumer? dataConsumer, CGRect* mediaBox, CGPDFInfo? info)
			: base (Create (dataConsumer, mediaBox, info), true)
		{
		}

		/// <param name="dataConsumer">To be added.</param>
		///         <param name="mediaBox">To be added.</param>
		///         <param name="info">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public unsafe CGContextPDF (CGDataConsumer dataConsumer, CGRect mediaBox, CGPDFInfo? info) :
			this (dataConsumer, &mediaBox, info)
		{
		}

		/// <param name="dataConsumer">To be added.</param>
		///         <param name="mediaBox">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public unsafe CGContextPDF (CGDataConsumer dataConsumer, CGRect mediaBox) :
			this (dataConsumer, &mediaBox, null)
		{
		}

		/// <param name="dataConsumer">To be added.</param>
		///         <param name="info">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public unsafe CGContextPDF (CGDataConsumer dataConsumer, CGPDFInfo? info) :
			this (dataConsumer, null, info)
		{
		}

		/// <param name="dataConsumer">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public unsafe CGContextPDF (CGDataConsumer dataConsumer) :
			this (dataConsumer, null, null)
		{
		}

		unsafe static IntPtr Create (NSUrl? url, CGRect* mediaBox, CGPDFInfo? info)
		{
			using var dict = info?.ToDictionary ();
			IntPtr result = CGPDFContextCreateWithURL (url.GetHandle (), mediaBox, dict.GetHandle ());
			GC.KeepAlive (url);
			return result;
		}

		unsafe CGContextPDF (NSUrl? url, CGRect* mediaBox, CGPDFInfo? info)
			: base (Create (url, mediaBox, info), true)
		{
		}

		/// <param name="url">To be added.</param>
		///         <param name="mediaBox">To be added.</param>
		///         <param name="info">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public unsafe CGContextPDF (NSUrl url, CGRect mediaBox, CGPDFInfo? info) :
			this (url, &mediaBox, info)
		{
		}

		/// <param name="url">The PDF file will be stored in this url</param>
		///         <param name="mediaBox">The size of the media box to generate, specified in points.</param>
		///         <summary>Creates a new CGContext that records its commands into a PDF file with the mediaBox dimensions stored in the specified url.</summary>
		///         <remarks>
		///         </remarks>
		public unsafe CGContextPDF (NSUrl url, CGRect mediaBox) :
			this (url, &mediaBox, null)
		{
		}

		/// <param name="url">The PDF file will be stored in this url</param>
		///         <param name="info">PDF Configuration options</param>
		///         <summary>Creates a new CGContext that records its commands into a PDF file in the specified url.</summary>
		///         <remarks>
		///         </remarks>
		public unsafe CGContextPDF (NSUrl url, CGPDFInfo? info) :
			this (url, null, info)
		{
		}

		/// <param name="url">The PDF file will be stored in this url</param>
		///         <summary>Creates a new CGContext that records its commands into a PDF file with the mediaBox dimensions stored in the specified url.</summary>
		///         <remarks>
		///         </remarks>
		public unsafe CGContextPDF (NSUrl url) :
			this (url, null, null)
		{
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGPDFContextClose (/* CGContextRef */ IntPtr context);

		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public void Close ()
		{
			if (closed)
				return;
			CGPDFContextClose (Handle);
			closed = true;
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGPDFContextBeginPage (/* CGContextRef */ IntPtr context, /* CFDictionaryRef */ IntPtr pageInfo);

		/// <param name="info">PDF Configuration options</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public void BeginPage (CGPDFPageInfo? info)
		{
			using (var dict = info?.ToDictionary ())
				CGPDFContextBeginPage (Handle, dict.GetHandle ());
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGPDFContextEndPage (/* CGContextRef */ IntPtr context);

		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public void EndPage ()
		{
			CGPDFContextEndPage (Handle);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGPDFContextAddDocumentMetadata (/* CGContextRef */ IntPtr context, /* CFDataRef */ IntPtr metadata);

		/// <param name="data">PDF Metadata encoded in XML format following the specification of the "Extensible Metadata Platform" from the PDF spec.</param>
		///         <summary>To be added.</summary>
		///         <remarks>
		///         </remarks>
		public void AddDocumentMetadata (NSData data)
		{
			if (data is null)
				return;
			CGPDFContextAddDocumentMetadata (Handle, data.Handle);
			GC.KeepAlive (data);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGPDFContextSetURLForRect (/* CGContextRef */ IntPtr context, /* CFURLRef */ IntPtr url, CGRect rect);

		/// <param name="url">The target url.</param>
		///         <param name="region">The region.</param>
		///         <summary>Associates a region in the screen with a url.   When the user clicks or taps in that region, he will be redirected to that url on their PDF viewer.</summary>
		///         <remarks>To be added.</remarks>
		public void SetUrl (NSUrl url, CGRect region)
		{
			if (url is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (url));
			CGPDFContextSetURLForRect (Handle, url.Handle, region);
			GC.KeepAlive (url);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGPDFContextAddDestinationAtPoint (/* CGContextRef */ IntPtr context, /* CFStringRef */ IntPtr name, CGPoint point);

		/// <param name="name">The name of the destination point.</param>
		///         <param name="point">The location of the destination.</param>
		///         <summary>Adds a destination name at the specified location.</summary>
		///         <remarks>Use this to add destinations in a PDF document.   These destinations can be reached by the user when they click on a region of the document that was defined with SetDestination.</remarks>
		public void AddDestination (string name, CGPoint point)
		{
			if (name is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (name));

			var nameHandle = CFString.CreateNative (name);
			try {
				CGPDFContextAddDestinationAtPoint (Handle, nameHandle, point);
			} finally {
				CFString.ReleaseNative (nameHandle);
			}
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGPDFContextSetDestinationForRect (/* CGContextRef */ IntPtr context, /* CFStringRef */ IntPtr name, CGRect rect);

		/// <param name="name">The name for the destination.</param>
		///         <param name="rect">The region that will respond to user input.</param>
		///         <summary>If the user clicks or taps in the specified region, the PDF viewer will jump to the named destination</summary>
		///         <remarks>To be added.</remarks>
		public void SetDestination (string name, CGRect rect)
		{
			if (name is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (name));

			var nameHandle = CFString.CreateNative (name);
			try {
				CGPDFContextSetDestinationForRect (Handle, nameHandle, rect);
			} finally {
				CFString.ReleaseNative (nameHandle);
			}
		}

		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios13.0")]
		[SupportedOSPlatform ("tvos13.0")]
		[SupportedOSPlatform ("maccatalyst")]
		[DllImport (Constants.CoreGraphicsLibrary)]
		static extern void CGPDFContextBeginTag (/* CGContextRef* */ IntPtr context, CGPdfTagType tagType, /* CFDictionaryRef* _Nullable */ IntPtr tagProperties);

		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios13.0")]
		[SupportedOSPlatform ("tvos13.0")]
		[SupportedOSPlatform ("maccatalyst")]
		public void BeginTag (CGPdfTagType tagType, NSDictionary tagProperties)
		{
			CGPDFContextBeginTag (Handle, tagType, tagProperties.GetHandle ());
			GC.KeepAlive (tagProperties);
		}

		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios13.0")]
		[SupportedOSPlatform ("tvos13.0")]
		[SupportedOSPlatform ("maccatalyst")]
		public void BeginTag (CGPdfTagType tagType, CGPdfTagProperties tagProperties)
		{
			var d = tagProperties?.Dictionary;
			CGPDFContextBeginTag (Handle, tagType, d.GetHandle ());
			GC.KeepAlive (d);
		}

		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios13.0")]
		[SupportedOSPlatform ("tvos13.0")]
		[SupportedOSPlatform ("maccatalyst")]
		[DllImport (Constants.CoreGraphicsLibrary)]
		static extern void CGPDFContextEndTag (/* CGContextRef* */ IntPtr context);

		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios13.0")]
		[SupportedOSPlatform ("tvos13.0")]
		[SupportedOSPlatform ("maccatalyst")]
		public void EndTag ()
		{
			CGPDFContextEndTag (Handle);
		}

		[SupportedOSPlatform ("ios18.0")]
		[SupportedOSPlatform ("maccatalyst18.0")]
		[SupportedOSPlatform ("macos15.0")]
		[SupportedOSPlatform ("tvos18.0")]
		[DllImport (Constants.CoreGraphicsLibrary)]
		static extern void CGPDFContextSetParentTree (/* CGContextRef cg_nullable */ IntPtr context, /* CGPDFDictionaryRef */ IntPtr parentTreeDictionary);

		[SupportedOSPlatform ("ios18.0")]
		[SupportedOSPlatform ("maccatalyst18.0")]
		[SupportedOSPlatform ("macos15.0")]
		[SupportedOSPlatform ("tvos18.0")]
		public void SetParentTree (CGPDFDictionary parentTreeDictionary)
		{
			CGPDFContextSetParentTree (GetCheckedHandle (), parentTreeDictionary.GetNonNullHandle (nameof (parentTreeDictionary)));
			GC.KeepAlive (parentTreeDictionary);
		}

		[SupportedOSPlatform ("ios18.0")]
		[SupportedOSPlatform ("maccatalyst18.0")]
		[SupportedOSPlatform ("macos15.0")]
		[SupportedOSPlatform ("tvos18.0")]
		[DllImport (Constants.CoreGraphicsLibrary)]
		static extern void CGPDFContextSetIDTree (/* CGContextRef cg_nullable */ IntPtr context, /* CGPDFDictionaryRef */ IntPtr IDTreeDictionary);

		[SupportedOSPlatform ("ios18.0")]
		[SupportedOSPlatform ("maccatalyst18.0")]
		[SupportedOSPlatform ("macos15.0")]
		[SupportedOSPlatform ("tvos18.0")]
		public void SetIdTree (CGPDFDictionary idTreeDictionary)
		{
			CGPDFContextSetIDTree (GetCheckedHandle (), idTreeDictionary.GetNonNullHandle (nameof (idTreeDictionary)));
			GC.KeepAlive (idTreeDictionary);
		}

		[SupportedOSPlatform ("ios18.0")]
		[SupportedOSPlatform ("maccatalyst18.0")]
		[SupportedOSPlatform ("macos15.0")]
		[SupportedOSPlatform ("tvos18.0")]
		[DllImport (Constants.CoreGraphicsLibrary)]
		static extern void CGPDFContextSetPageTagStructureTree (/* CGContextRef cg_nullable */ IntPtr context, /* CFDictionaryRef */ IntPtr pageTagStructureTreeDictionary);

		[SupportedOSPlatform ("ios18.0")]
		[SupportedOSPlatform ("maccatalyst18.0")]
		[SupportedOSPlatform ("macos15.0")]
		[SupportedOSPlatform ("tvos18.0")]
		public void SetPageTagStructureTree (NSDictionary pageTagStructureTreeDictionary)
		{
			CGPDFContextSetPageTagStructureTree (GetCheckedHandle (), pageTagStructureTreeDictionary.GetNonNullHandle (nameof (pageTagStructureTreeDictionary)));
			GC.KeepAlive (pageTagStructureTreeDictionary);
		}

		protected override void Dispose (bool disposing)
		{
			if (disposing)
				Close ();

			base.Dispose (disposing);
		}
	}
}
