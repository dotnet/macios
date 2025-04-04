// 
// CGPDFScanner.cs: Implement the managed CGPDFScanner binding
//
// Authors:
//	Sebastien Pouliot  <sebastien@xamarin.com>
//     
// Copyright 2014 Xamarin Inc. All rights reserved.

#nullable enable

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Text;
using Foundation;
using ObjCRuntime;
using CoreFoundation;

namespace CoreGraphics {
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	public class CGPDFScanner : NativeObject {

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static /* CGPDFScannerRef */ IntPtr CGPDFScannerCreate (/* CGPDFContentStreamRef */ IntPtr cs,
			/* CGPDFOperatorTableRef */ IntPtr table, /* void* */ IntPtr info);

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static /* CGPDFScannerRef */ IntPtr CGPDFScannerRetain (/* CGPDFScannerRef */ IntPtr scanner);

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGPDFScannerRelease (/* CGPDFScannerRef */ IntPtr scanner);

		object? info;
		GCHandle gch;

		/// <param name="cs">To be added.</param>
		///         <param name="table">To be added.</param>
		///         <param name="userInfo">To be added.</param>
		///         <summary>Creates a <see cref="T:CoreGraphics.CGPDFScanner" /> object that invokes callbacks in the <paramref name="cs" /> parameter when it encounters an operator specified by the <paramref name="table" /> parameter.</summary>
		///         <remarks>To be added.</remarks>
		public CGPDFScanner (CGPDFContentStream cs, CGPDFOperatorTable table, object userInfo)
		{
			if (cs is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (cs));
			if (table is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (table));

			info = userInfo;
			gch = GCHandle.Alloc (this);
			InitializeHandle (CGPDFScannerCreate (cs.Handle, table.Handle, GCHandle.ToIntPtr (gch)));
			GC.KeepAlive (cs);
			GC.KeepAlive (table);
		}

		[Preserve (Conditional = true)]
		internal CGPDFScanner (NativeHandle handle, bool owns)
			: base (handle, owns)
		{
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public object? UserInfo {
			get { return info; }
		}

		protected internal override void Retain ()
		{
			CGPDFScannerRetain (GetCheckedHandle ());
		}

		protected internal override void Release ()
		{
			CGPDFScannerRelease (GetCheckedHandle ());
		}

		/// <include file="../../docs/api/CoreGraphics/CGPDFScanner.xml" path="/Documentation/Docs[@DocId='M:CoreGraphics.CGPDFScanner.Dispose(System.Boolean)']/*" />
		protected override void Dispose (bool disposing)
		{
			if (gch.IsAllocated)
				gch.Free ();
			base.Dispose (disposing);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static /* CGPDFContentStreamRef */ IntPtr CGPDFScannerGetContentStream (/* CGPDFScannerRef */ IntPtr scanner);

		/// <summary>Gets the content stream for <c>this</c> <see cref="T:CoreGraphics.CGPDFScanner" /> object.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public CGPDFContentStream GetContentStream ()
		{
			return new CGPDFContentStream (CGPDFScannerGetContentStream (Handle), false);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static byte CGPDFScannerScan (/* CGPDFScannerRef */ IntPtr scanner);

		/// <summary>Parses <c>this</c> <see cref="T:CoreGraphics.CGPDFScanner" /> object, and then returns whether the parsing succeeded.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public bool Scan ()
		{
			return CGPDFScannerScan (Handle) != 0;
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		unsafe extern static byte CGPDFScannerPopObject (/* CGPDFScannerRef */ IntPtr scanner, /* CGPDFObjectRef* */ IntPtr* value);

		/// <param name="value">To be added.</param>
		///         <summary>Pops an object from the stack of <c>this</c> <see cref="T:CoreGraphics.CGPDFScanner" /> object, returns that object by using the <paramref name="value" /> parameter, and then returns whether this method succeeded.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public bool TryPop (out CGPDFObject? value)
		{
			IntPtr ip;
			bool rv;
			unsafe {
				rv = CGPDFScannerPopObject (Handle, &ip) != 0;
			}
			value = rv ? new CGPDFObject (ip) : null;
			return rv;
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		unsafe extern static byte CGPDFScannerPopBoolean (/* CGPDFScannerRef */ IntPtr scanner, /* CGPDFBoolean* */ byte* value);

		/// <param name="value">To be added.</param>
		///         <summary>Pops a <see cref="T:System.Boolean" /> object from the stack of <c>this</c> <see cref="T:CoreGraphics.CGPDFScanner" /> object, returns that object by using the <paramref name="value" /> parameter, and then returns whether this method succeeded.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public unsafe bool TryPop (out bool value)
		{
			byte bytevalue;
			var rv = CGPDFScannerPopBoolean (Handle, &bytevalue) != 0;
			value = bytevalue != 0;
			return rv;
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		unsafe extern static byte CGPDFScannerPopInteger (/* CGPDFScannerRef */ IntPtr scanner, /* CGPDFInteger* */ nint* value);

		public bool TryPop (out nint value)
		{
			value = default;
			unsafe {
				return CGPDFScannerPopInteger (Handle, (nint*) Unsafe.AsPointer<nint> (ref value)) != 0;
			}
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		unsafe extern static byte CGPDFScannerPopNumber (/* CGPDFScannerRef */ IntPtr scanner, /* CGPDFReal* */ nfloat* value);

		public bool TryPop (out nfloat value)
		{
			value = default;
			unsafe {
				return CGPDFScannerPopNumber (Handle, (nfloat*) Unsafe.AsPointer<nfloat> (ref value)) != 0;
			}
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		unsafe extern static byte CGPDFScannerPopName (/* CGPDFScannerRef */ IntPtr scanner, /* const char** */ IntPtr* value);
		// note: that string is not ours to free

		// not to be confusing with CGPDFScannerPopString (value)
		/// <param name="name">To be added.</param>
		///         <summary>Pops a character string object from the stack of <c>this</c><see cref="T:CoreGraphics.CGPDFScanner" /> object, returns that object by using the <paramref name="name" /> parameter, and then returns whether this method succeeded.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public bool TryPopName (out string? name)
		{
			IntPtr ip;
			bool rv;
			unsafe {
				rv = CGPDFScannerPopName (Handle, &ip) != 0;
			}
			name = rv ? Marshal.PtrToStringAnsi (ip) : null;
			return rv;
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		unsafe extern static byte CGPDFScannerPopString (/* CGPDFScannerRef */ IntPtr scanner, /* CGPDFStringRef* */ IntPtr* value);

		/// <param name="value">To be added.</param>
		///         <summary>Pops a <format type="text/html"><a href="https://docs.microsoft.com/en-us/search/index?search=System%20String&amp;scope=Xamarin" title="T:System.String">T:System.String</a></format> object from the stack of <c>this</c> <see cref="T:CoreGraphics.CGPDFScanner" /> object, returns that object by using the <paramref name="value" /> parameter, and then returns whether this method succeeded.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public bool TryPop (out string? value)
		{
			IntPtr ip;
			bool rv;
			unsafe {
				rv = CGPDFScannerPopString (Handle, &ip) != 0;
			}
			value = rv ? CGPDFString.ToString (ip) : null;
			return rv;
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		unsafe extern static byte CGPDFScannerPopArray (/* CGPDFScannerRef */ IntPtr scanner, /* CGPDFArrayRef* */ IntPtr* value);

		/// <param name="value">To be added.</param>
		///         <summary>Pops an array from the stack of <c>this</c> <see cref="T:CoreGraphics.CGPDFScanner" /> object, returns that array by using the <paramref name="value" /> parameter, and then returns whether this method succeeded.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public bool TryPop (out CGPDFArray? value)
		{
			IntPtr ip;
			bool rv;
			unsafe {
				rv = CGPDFScannerPopArray (Handle, &ip) != 0;
			}
			value = rv ? new CGPDFArray (ip) : null;
			return rv;
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		unsafe extern static byte CGPDFScannerPopDictionary (/* CGPDFScannerRef */ IntPtr scanner, /* CGPDFDictionaryRef* */ IntPtr* value);

		/// <param name="value">To be added.</param>
		///         <summary>Pops a PDF dictionary from the stack of <c>this</c> <see cref="T:CoreGraphics.CGPDFScanner" /> object, returns that dictionary by using the <paramref name="value" /> parameter, and then returns whether this method succeeded.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public bool TryPop (out CGPDFDictionary? value)
		{
			IntPtr ip;
			bool rv;
			unsafe {
				rv = CGPDFScannerPopDictionary (Handle, &ip) != 0;
			}
			value = rv ? new CGPDFDictionary (ip) : null;
			return rv;
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		unsafe extern static byte CGPDFScannerPopStream (/* CGPDFScannerRef */ IntPtr scanner, /* CGPDFStreamRef* */ IntPtr* value);

		/// <param name="value">To be added.</param>
		///         <summary>Pops a PDF stream from the stack of <c>this</c> <see cref="T:CoreGraphics.CGPDFScanner" /> object, returns that stream by using the <paramref name="value" /> parameter, and then returns whether this method succeeded.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public bool TryPop (out CGPDFStream? value)
		{
			IntPtr ip;
			bool rv;
			unsafe {
				rv = CGPDFScannerPopStream (Handle, &ip) != 0;
			}
			value = rv ? new CGPDFStream (ip) : null;
			return rv;
		}

		[SupportedOSPlatform ("ios16.0")]
		[SupportedOSPlatform ("maccatalyst16.0")]
		[SupportedOSPlatform ("macos13.0")]
		[SupportedOSPlatform ("tvos16.0")]
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGPDFScannerStop (/* CGPDFScannerRef */ IntPtr scanner);

		[SupportedOSPlatform ("ios16.0")]
		[SupportedOSPlatform ("maccatalyst16.0")]
		[SupportedOSPlatform ("macos13.0")]
		[SupportedOSPlatform ("tvos16.0")]
		public void Stop ()
		{
			CGPDFScannerStop (Handle);
		}
	}
}
