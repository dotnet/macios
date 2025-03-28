// 
// CGPDFOperatorTable.cs: Implement the managed CGPDFOperatorTable bindings
//
// Authors:
//	Sebastien Pouliot  <sebastien@xamarin.com>
//     
// Copyright 2014 Xamarin Inc. All rights reserved.

#nullable enable

using System;
using System.Collections.Generic;
using System.Runtime.Versioning;
using System.Runtime.InteropServices;
using Foundation;
using ObjCRuntime;
using CoreFoundation;

namespace CoreGraphics {
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	// CGPDFOperatorTable.h
	public class CGPDFOperatorTable : NativeObject {

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static /* CGPDFOperatorTableRef */ IntPtr CGPDFOperatorTableCreate ();

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static /* CGPDFOperatorTableRef */ IntPtr CGPDFOperatorTableRetain (/* CGPDFOperatorTableRef */ IntPtr table);

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGPDFOperatorTableRelease (/* CGPDFOperatorTableRef */ IntPtr table);

		// CGPDFOperatorCallback
		delegate void CGPDFOperatorCallback (/* CGPDFScannerRef */ IntPtr scanner, /* void* */ IntPtr info);

		public CGPDFOperatorTable ()
			: base (CGPDFOperatorTableCreate (), true)
		{
		}

		[Preserve (Conditional = true)]
		internal CGPDFOperatorTable (NativeHandle handle, bool owns)
			 : base (handle, owns)
		{
		}

		protected internal override void Retain ()
		{
			CGPDFOperatorTableRetain (GetCheckedHandle ());
		}

		protected internal override void Release ()
		{
			CGPDFOperatorTableRelease (GetCheckedHandle ());
		}

		// The good part about this signature is that it enforces at compile time that 'callback' is callable from native code in a FullAOT scenario.
		// The bad part is that it's unsafe code (and callers must be in unsafe mode as well).
		[DllImport (Constants.CoreGraphicsLibrary)]
		unsafe extern static void CGPDFOperatorTableSetCallback (/* CGPDFOperatorTableRef */ IntPtr table, /* const char */ IntPtr name, /* CGPDFOperatorCallback */ delegate* unmanaged<IntPtr, IntPtr, void> callback);

		public unsafe void SetCallback (string name, delegate* unmanaged<IntPtr, IntPtr, void> callback)
		{
			if (name is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (name));

			using var namePtr = new TransientString (name);
			CGPDFOperatorTableSetCallback (Handle, namePtr, callback);
		}

		static public CGPDFScanner? GetScannerFromInfo (IntPtr gchandle)
		{
			return GCHandle.FromIntPtr (gchandle).Target as CGPDFScanner;
		}
	}
}
