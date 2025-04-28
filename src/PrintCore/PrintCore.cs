//
// PrintCore.cs: PrintCore bindings
//
// Authors:
//   Miguel de Icaza (miguel@gnome.org)
//
// Copyright 2016 Microsoft Inc
//

#nullable enable

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.IO;
using System.Runtime.Versioning;
using ObjCRuntime;
using Foundation;
using CoreGraphics;
using CoreFoundation;
using PMObject = System.IntPtr;

#if !NET
using NativeHandle = System.IntPtr;
#endif

namespace PrintCore {
#if NET
	/// <summary>To be added.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("macos")]
#endif
	public class PMPrintCoreBase : NativeObject {
		[Preserve (Conditional = true)]
		internal PMPrintCoreBase (NativeHandle handle, bool owns)
			: base (handle, owns)
		{
		}

		[DllImport (Constants.PrintCoreLibrary)]
		internal extern static OSStatus PMRetain (PMObject obj);

		[DllImport (Constants.PrintCoreLibrary)]
		internal extern static OSStatus PMRelease (PMObject obj);

		protected internal override void Retain ()
		{
			PMRetain (Handle);
		}

		protected internal override void Release ()
		{
			PMRelease (Handle);
		}
	}

#if NET
	/// <summary>To be added.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("macos")]
#endif
	public class PMPrintException : Exception {
		/// <param name="code">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public PMPrintException (PMStatusCode code) : base (code.ToString ()) { }
	}

#if NET
	/// <summary>To be added.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("macos")]
#endif
	public class PMPrintSession : PMPrintCoreBase {
		[DllImport (Constants.PrintCoreLibrary)]
		unsafe extern static PMStatusCode PMCreateSession (IntPtr* session);

		[Preserve (Conditional = true)]
		internal PMPrintSession (NativeHandle handle, bool owns) : base (handle, owns) { }

		static IntPtr Create ()
		{
			PMStatusCode code;
			IntPtr value;
			unsafe {
				code = PMCreateSession (&value);
			}
			if (code == PMStatusCode.Ok)
				return value;
			throw new PMPrintException (code);
		}

		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public PMPrintSession ()
			: base (Create (), true)
		{
		}

		/// <param name="session">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static PMStatusCode TryCreate (out PMPrintSession? session)
		{
			PMStatusCode code;
			IntPtr value;
			unsafe {
				code = PMCreateSession (&value);
			}
			if (code == PMStatusCode.Ok) {
				session = new PMPrintSession (value, true);
				return PMStatusCode.Ok;
			}
			session = null;
			return code;
		}

		[DllImport (Constants.PrintCoreLibrary)]
		extern static PMStatusCode PMSessionError (IntPtr handle);
		[DllImport (Constants.PrintCoreLibrary)]
		extern static PMStatusCode PMSessionSetError (IntPtr handle, PMStatusCode code);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public PMStatusCode SessionError {
			get {
				return PMSessionError (Handle);
			}
			set {
				PMSessionSetError (Handle, value);
			}
		}

		[DllImport (Constants.PrintCoreLibrary)]
		extern static PMStatusCode PMSessionDefaultPrintSettings (IntPtr session, IntPtr settings);

		/// <param name="settings">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public void AssignDefaultSettings (PMPrintSettings settings)
		{
			if (settings is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (settings));
			PMSessionDefaultPrintSettings (Handle, settings.Handle);
			GC.KeepAlive (settings);
		}

		[DllImport (Constants.PrintCoreLibrary)]
		extern static PMStatusCode PMSessionDefaultPageFormat (IntPtr session, IntPtr pageFormat);

		/// <param name="pageFormat">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public void AssignDefaultPageFormat (PMPageFormat pageFormat)
		{
			if (pageFormat is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (pageFormat));
			PMSessionDefaultPageFormat (Handle, pageFormat.Handle);
			GC.KeepAlive (pageFormat);
		}

		[DllImport (Constants.PrintCoreLibrary)]
		unsafe extern static PMStatusCode PMSessionCreatePrinterList (IntPtr printSession, IntPtr* printerListArray, int* index, IntPtr* printer);
		/// <param name="printerList">To be added.</param>
		///         <param name="index">To be added.</param>
		///         <param name="printer">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public PMStatusCode CreatePrinterList (out string? []? printerList, out int index, out PMPrinter? printer)
		{
			PMStatusCode code;
			IntPtr array;
			IntPtr printerHandle;

			index = default;
			unsafe {
				code = PMSessionCreatePrinterList (Handle, &array, (int*) Unsafe.AsPointer<int> (ref index), &printerHandle);
			}
			if (code != PMStatusCode.Ok) {
				printerList = null;
				printer = null;
				return code;
			}

			printerList = CFArray.StringArrayFromHandle (array, true);
			if (printerHandle != IntPtr.Zero) {
				// Now get the printer, we do not own it, so retain.
				printer = new PMPrinter (printerHandle, owns: false);
			} else
				printer = null;

			return PMStatusCode.Ok;
		}

		[DllImport (Constants.PrintCoreLibrary)]
		unsafe extern static PMStatusCode PMSessionValidatePrintSettings (IntPtr handle, IntPtr printSettings, byte* changed);

		/// <param name="settings">To be added.</param>
		///         <param name="changed">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public PMStatusCode ValidatePrintSettings (PMPrintSettings settings, out bool changed)
		{
			if (settings is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (settings));

			PMStatusCode code;
			byte c;
			unsafe {
				code = PMSessionValidatePrintSettings (Handle, settings.Handle, &c);
				GC.KeepAlive (settings);
			}
			if (code != PMStatusCode.Ok) {
				changed = false;
				return code;
			}
			changed = c != 0;
			return code;
		}
	}

#if NET
	/// <summary>To be added.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("macos")]
#endif
	public class PMPrintSettings : PMPrintCoreBase {
		[DllImport (Constants.PrintCoreLibrary)]
		unsafe extern static PMStatusCode PMCreatePrintSettings (IntPtr* session);

		[Preserve (Conditional = true)]
		internal PMPrintSettings (NativeHandle handle, bool owns)
			: base (handle, owns)
		{
		}

		static IntPtr Create ()
		{
			PMStatusCode code;
			IntPtr value;
			unsafe {
				code = PMCreatePrintSettings (&value);
			}
			if (code == PMStatusCode.Ok)
				return value;
			throw new PMPrintException (code);
		}

		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public PMPrintSettings ()
			: base (Create (), true)
		{
		}

		/// <param name="settings">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static PMStatusCode TryCreate (out PMPrintSettings? settings)
		{
			PMStatusCode code;
			IntPtr value;
			unsafe {
				code = PMCreatePrintSettings (&value);
			}
			if (code == PMStatusCode.Ok) {
				settings = new PMPrintSettings (value, true);
				return PMStatusCode.Ok;
			}
			settings = null;
			return code;
		}

		[DllImport (Constants.PrintCoreLibrary)]
		unsafe extern static PMStatusCode PMGetFirstPage (IntPtr handle, uint* first);
		[DllImport (Constants.PrintCoreLibrary)]
		extern static PMStatusCode PMSetFirstPage (IntPtr handle, uint first, byte lockb);
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public uint FirstPage {
			get {
				uint val;
				unsafe {
					PMGetFirstPage (Handle, &val);
				}
				return val;
			}
			set {
				PMSetFirstPage (Handle, value, 0);
			}
		}

		[DllImport (Constants.PrintCoreLibrary)]
		unsafe extern static PMStatusCode PMGetLastPage (IntPtr handle, uint* last);
		[DllImport (Constants.PrintCoreLibrary)]
		extern static PMStatusCode PMSetLastPage (IntPtr handle, uint last, byte lockb);
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public uint LastPage {
			get {
				uint val;
				unsafe {
					PMGetLastPage (Handle, &val);
				}
				return val;
			}
			set {
				PMSetLastPage (Handle, value, 0);
			}
		}

		[DllImport (Constants.PrintCoreLibrary)]
		unsafe extern static PMStatusCode PMGetPageRange (IntPtr handle, uint* minPage, uint* maxPage);
		[DllImport (Constants.PrintCoreLibrary)]
		extern static PMStatusCode PMSetPageRange (IntPtr handle, uint minPage, uint maxPage);
		/// <param name="minPage">To be added.</param>
		///         <param name="maxPage">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public PMStatusCode GetPageRange (out uint minPage, out uint maxPage)
		{
			minPage = default;
			maxPage = default;
			unsafe {
				return PMGetPageRange (Handle, (uint*) Unsafe.AsPointer<uint> (ref minPage), (uint*) Unsafe.AsPointer<uint> (ref maxPage));
			}
		}

		/// <param name="minPage">To be added.</param>
		///         <param name="maxPage">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public PMStatusCode SetPageRange (uint minPage, uint maxPage)
		{
			return PMSetPageRange (Handle, minPage, maxPage);
		}


		[DllImport (Constants.PrintCoreLibrary)]
		extern static PMStatusCode PMCopyPrintSettings (IntPtr source, IntPtr dest);

		/// <param name="destination">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public PMStatusCode CopySettings (PMPrintSettings destination)
		{
			if (destination is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (destination));
			PMStatusCode status = PMCopyPrintSettings (Handle, destination.Handle);
			GC.KeepAlive (destination);
			return status;
		}

		[DllImport (Constants.PrintCoreLibrary)]
		unsafe extern static PMStatusCode PMGetCopies (IntPtr handle, uint* copies);
		[DllImport (Constants.PrintCoreLibrary)]
		extern static PMStatusCode PMSetCopies (IntPtr handle, uint copies, byte elock);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public uint Copies {
			get {
				PMStatusCode code;
				uint c;
				unsafe {
					code = PMGetCopies (Handle, &c);
				}
				if (code == PMStatusCode.Ok)
					return c;
				else
					return 0;
			}
			set {
				PMSetCopies (Handle, value, 0);
			}
		}

		[DllImport (Constants.PrintCoreLibrary)]
		unsafe extern static PMStatusCode PMGetCollate (IntPtr handle, byte* collate);
		[DllImport (Constants.PrintCoreLibrary)]
		extern static PMStatusCode PMSetCollate (IntPtr handle, byte collate);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public bool Collate {
			get {
				PMStatusCode code;
				byte c;
				unsafe {
					code = PMGetCollate (Handle, &c);
				}
				if (code == PMStatusCode.Ok)
					return c != 0;
				else
					return false;
			}
			set {
				PMSetCollate (Handle, (byte) (value ? 1 : 0));
			}
		}

		[DllImport (Constants.PrintCoreLibrary)]
		unsafe extern static PMStatusCode PMGetDuplex (IntPtr handle, PMDuplexMode* mode);
		[DllImport (Constants.PrintCoreLibrary)]
		extern static PMStatusCode PMSetDuplex (IntPtr handle, PMDuplexMode mode);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public PMDuplexMode DuplexMode {
			get {
				PMStatusCode code;
				PMDuplexMode c;
				unsafe {
					code = PMGetDuplex (Handle, &c);
				}
				if (code == PMStatusCode.Ok)
					return c;
				else
					return PMDuplexMode.None;
			}
			set {
				PMSetDuplex (Handle, value);
			}
		}

		[DllImport (Constants.PrintCoreLibrary)]
		unsafe extern static PMStatusCode PMGetScale (IntPtr handle, double* mode);
		[DllImport (Constants.PrintCoreLibrary)]
		extern static PMStatusCode PMSetScale (IntPtr handle, double scale);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public double Scale {
			get {
				PMStatusCode code;
				double c;
				unsafe {
					code = PMGetScale (Handle, &c);
				}
				if (code == PMStatusCode.Ok)
					return c;
				else
					return 100;
			}
			set {
				PMSetScale (Handle, value);
			}
		}

	}

#if NET
	/// <summary>To be added.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("macos")]
#endif
	public class PMPageFormat : PMPrintCoreBase {
		[DllImport (Constants.PrintCoreLibrary)]
		unsafe extern static PMStatusCode PMCreatePageFormat (IntPtr* handle);
		[DllImport (Constants.PrintCoreLibrary)]
		unsafe extern static PMStatusCode PMCreatePageFormatWithPMPaper (IntPtr* handle, IntPtr paper);

		[Preserve (Conditional = true)]
		internal PMPageFormat (NativeHandle handle, bool owns) : base (handle, owns) { }

		static IntPtr Create (PMPaper? paper = null)
		{
			IntPtr value;
			PMStatusCode code;
			if (paper is null) {
				unsafe {
					code = PMCreatePageFormat (&value);
				}
			} else {
				unsafe {
					code = PMCreatePageFormatWithPMPaper (&value, paper.Handle);
					GC.KeepAlive (paper);
				}
			}
			if (code == PMStatusCode.Ok)
				return value;

			throw new PMPrintException (code);
		}

		/// <param name="paper">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public PMPageFormat (PMPaper? paper = null)
			: base (Create (paper), true)
		{
		}

		/// <param name="pageFormat">To be added.</param>
		///         <param name="paper">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static PMStatusCode TryCreate (out PMPageFormat? pageFormat, PMPaper? paper = null)
		{
			PMStatusCode code;
			IntPtr value;

			if (paper is null) {
				unsafe {
					code = PMCreatePageFormat (&value);
				}
			} else {
				unsafe {
					code = PMCreatePageFormatWithPMPaper (&value, paper.Handle);
					GC.KeepAlive (paper);
				}
			}

			if (code == PMStatusCode.Ok) {
				pageFormat = new PMPageFormat (value, true);
				return PMStatusCode.Ok;
			}
			pageFormat = null;
			return code;
		}

		[DllImport (Constants.PrintCoreLibrary)]
		extern static PMStatusCode PMSetOrientation (IntPtr handle, PMOrientation orientation, byte setToFalse);
		[DllImport (Constants.PrintCoreLibrary)]
		unsafe extern static PMStatusCode PMGetOrientation (IntPtr handle, PMOrientation* orientation);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public PMOrientation Orientation {
			get {
				PMStatusCode code;
				PMOrientation o;
				unsafe {
					code = PMGetOrientation (Handle, &o);
				}
				if (code == PMStatusCode.Ok)
					return o;
				return PMOrientation.Portrait;
			}
			set {
				PMSetOrientation (Handle, value, 0);
			}
		}

		[DllImport (Constants.PrintCoreLibrary)]
		unsafe extern static PMStatusCode PMGetAdjustedPageRect (IntPtr pageFormat, PMRect* pageRect);
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public PMRect AdjustedPageRect {
			get {
				PMStatusCode code;
				PMRect rect;
				unsafe {
					code = PMGetAdjustedPageRect (Handle, &rect);
				}
				if (code == PMStatusCode.Ok)
					return rect;
				return new PMRect (0, 0, 0, 0);
			}
		}

		[DllImport (Constants.PrintCoreLibrary)]
		unsafe extern static PMStatusCode PMGetAdjustedPaperRect (IntPtr pageFormat, PMRect* pageRect);
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public PMRect AdjustedPaperRect {
			get {
				PMRect rect;
				unsafe {
					if (PMGetAdjustedPaperRect (Handle, &rect) == PMStatusCode.Ok)
						return rect;
				}
				return new PMRect (0, 0, 0, 0);
			}
		}
	}

#if NET
	/// <summary>To be added.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("macos")]
#endif
	public class PMPaper : PMPrintCoreBase {
		[Preserve (Conditional = true)]
		internal PMPaper (NativeHandle handle, bool owns) : base (handle, owns) { }
		[DllImport (Constants.PrintCoreLibrary)]
		unsafe extern static PMStatusCode PMPaperGetID (IntPtr handle, IntPtr* str);
		[DllImport (Constants.PrintCoreLibrary)]
		unsafe extern static PMStatusCode PMPaperGetWidth (IntPtr handle, double* v);
		[DllImport (Constants.PrintCoreLibrary)]
		unsafe extern static PMStatusCode PMPaperGetHeight (IntPtr handle, double* v);
		[DllImport (Constants.PrintCoreLibrary)]
		unsafe extern static PMStatusCode PMPaperGetMargins (IntPtr handle, PMPaperMargins* margins);
		[DllImport (Constants.PrintCoreLibrary)]
		unsafe extern static PMStatusCode PMPaperCreateLocalizedName (IntPtr handle, IntPtr printer, IntPtr* name);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public string? ID {
			get {
				PMStatusCode code;
				IntPtr s;
				unsafe {
					code = PMPaperGetID (Handle, &s);
				}
				if (code != PMStatusCode.Ok)
					return null;
				return CFString.FromHandle (s);
			}
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public double Width {
			get {
				PMStatusCode code;
				double s;
				unsafe {
					code = PMPaperGetWidth (Handle, &s);
				}
				if (code != PMStatusCode.Ok)
					return 0;
				return s;
			}
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public double Height {
			get {
				PMStatusCode code;
				double s;
				unsafe {
					code = PMPaperGetHeight (Handle, &s);
				}
				if (code != PMStatusCode.Ok)
					return 0;
				return s;
			}
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public PMPaperMargins? Margins {
			get {
				PMStatusCode code;
				PMPaperMargins margins;
				unsafe {
					code = PMPaperGetMargins (Handle, &margins);
				}
				if (code != PMStatusCode.Ok)
					return null;
				return margins;
			}
		}

		/// <param name="printer">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public string? GetLocalizedName (PMPrinter printer)
		{
			if (printer is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (printer));
			PMStatusCode code;
			IntPtr name;
			unsafe {
				code = PMPaperCreateLocalizedName (Handle, printer.Handle, &name);
				GC.KeepAlive (printer);
			}
			if (code != PMStatusCode.Ok)
				return null;
			return CFString.FromHandle (name, true);
		}

	}

#if NET
	/// <summary>To be added.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("macos")]
#endif
	public class PMPrinter : PMPrintCoreBase {
		[DllImport (Constants.PrintCoreLibrary)]
		unsafe extern static PMStatusCode PMCreateGenericPrinter (IntPtr* session);

		[DllImport (Constants.PrintCoreLibrary)]
		extern static IntPtr PMPrinterCreateFromPrinterID (IntPtr id);

		[Preserve (Conditional = true)]
		internal PMPrinter (NativeHandle handle, bool owns) : base (handle, owns) { }

		static IntPtr Create ()
		{
			PMStatusCode code;
			IntPtr value;
			unsafe {
				code = PMCreateGenericPrinter (&value);
			}
			if (code == PMStatusCode.Ok)
				return value;
			throw new PMPrintException (code);
		}

		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public PMPrinter ()
			: base (Create (), true)
		{
		}

		static IntPtr Create (string printerId)
		{
			if (printerId is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (printerId));

			var printerIdHandle = CFString.CreateNative (printerId);
			try {
				var value = PMPrinterCreateFromPrinterID (printerIdHandle);
				if (value == IntPtr.Zero)
					throw new PMPrintException (PMStatusCode.InvalidPrinter);
				return value;
			} finally {
				CFString.ReleaseNative (printerIdHandle);
			}
		}

		/// <param name="printerId">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public PMPrinter (string printerId)
			: base (Create (printerId), true)
		{
		}

		/// <param name="printer">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static PMStatusCode TryCreate (out PMPrinter? printer)
		{
			IntPtr value;
			PMStatusCode code;
			unsafe {
				code = PMCreateGenericPrinter (&value);
			}
			if (code == PMStatusCode.Ok) {
				printer = new PMPrinter (value, owns: true);
				return PMStatusCode.Ok;
			}
			printer = null;
			return code;
		}

		/// <param name="printerId">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static PMPrinter? TryCreate (string printerId)
		{
			using (var idf = new CFString (printerId)) {
				var h = PMPrinterCreateFromPrinterID (idf.Handle);
				if (h == IntPtr.Zero)
					return null;
				return new PMPrinter (h, owns: true);
			}
		}

		[DllImport (Constants.PrintCoreLibrary)]
		extern static IntPtr PMPrinterGetName (IntPtr handle);
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public string? Name => CFString.FromHandle (PMPrinterGetName (Handle));

		[DllImport (Constants.PrintCoreLibrary)]
		unsafe extern static PMStatusCode PMPrinterCopyDeviceURI (IntPtr handle, IntPtr* url);

		/// <param name="url">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public PMStatusCode TryGetDeviceUrl (out NSUrl? url)
		{
			PMStatusCode code;
			IntPtr urlH;
			unsafe {
				code = PMPrinterCopyDeviceURI (Handle, &urlH);
			}
			if (code != PMStatusCode.Ok) {
				url = null;
				return code;
			}
			url = Runtime.GetNSObject<NSUrl> (urlH, true);
			return PMStatusCode.Ok;
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public NSUrl? DeviceUrl {
			get {
				if (TryGetDeviceUrl (out var url) != PMStatusCode.Ok)
					return null;
				return url;
			}
		}

		[DllImport (Constants.PrintCoreLibrary)]
		unsafe extern static PMStatusCode PMPrinterGetMakeAndModelName (IntPtr printer, IntPtr* makeAndModel);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public string? MakeAndModel {
			get {
				PMStatusCode code;
				IntPtr v;
				unsafe {
					code = PMPrinterGetMakeAndModelName (Handle, &v);
				}
				if (code == PMStatusCode.Ok) {
					return CFString.FromHandle (v);
				}
				return null;
			}
		}

		[DllImport (Constants.PrintCoreLibrary)]
		unsafe extern static PMStatusCode PMPrinterGetState (IntPtr printer, PMPrinterState* state);

		// Return is overloaded - if negative, a PMStatusCode.
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public PMPrinterState PrinterState {
			get {
				PMStatusCode code;
				PMPrinterState s;
				unsafe {
					code = PMPrinterGetState (Handle, &s);
				}
				if (code == PMStatusCode.Ok)
					return s;

				return (PMPrinterState) code;
			}
		}

		[DllImport (Constants.PrintCoreLibrary)]
		unsafe extern static PMStatusCode PMPrinterGetMimeTypes (IntPtr printer, IntPtr settings, IntPtr* arrayStr);

		/// <param name="settings">To be added.</param>
		///         <param name="mimeTypes">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public PMStatusCode TryGetMimeTypes (PMPrintSettings settings, out string? []? mimeTypes)
		{
			PMStatusCode code;
			IntPtr m;
			unsafe {
				code = PMPrinterGetMimeTypes (Handle, settings.GetHandle (), &m);
				GC.KeepAlive (settings);
			}
			if (code != PMStatusCode.Ok) {
				mimeTypes = null;
				return code;
			}
			mimeTypes = CFArray.StringArrayFromHandle (m);
			return PMStatusCode.Ok;
		}

		[DllImport (Constants.PrintCoreLibrary)]
		unsafe extern static PMStatusCode PMPrinterGetPaperList (IntPtr printer, IntPtr* arrayStr);
		/// <param name="paperList">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public PMStatusCode TryGetPaperList (out PMPaper []? paperList)
		{
			PMStatusCode code;
			IntPtr m;
			unsafe {
				code = PMPrinterGetPaperList (Handle, &m);
			}
			if (code != PMStatusCode.Ok) {
				paperList = null;
				return code;
			}
			paperList = (PMPaper []) CFArray.ArrayFromHandleFunc<PMPaper> (m, (handle) => new PMPaper (handle, false))!;
			return PMStatusCode.Ok;
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public PMPaper [] PaperList {
			get {
				if (TryGetPaperList (out var paperList) != PMStatusCode.Ok)
					return Array.Empty<PMPaper> ();
				return paperList!;
			}
		}

		[DllImport (Constants.PrintCoreLibrary)]
		extern static PMStatusCode PMPrinterPrintWithFile (IntPtr handle, IntPtr settings, IntPtr pageFormat, IntPtr strMimeType, IntPtr fileUrl);

		/// <param name="settings">To be added.</param>
		///         <param name="pageFormat">To be added.</param>
		///         <param name="fileUrl">To be added.</param>
		///         <param name="mimeType">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public PMStatusCode TryPrintFile (PMPrintSettings settings, PMPageFormat? pageFormat, NSUrl fileUrl, string? mimeType = null)
		{
			if (settings is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (settings));
			if (fileUrl is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (fileUrl));

			IntPtr mime = CFString.CreateNative (mimeType);
			try {
				PMStatusCode status = PMPrinterPrintWithFile (Handle, settings.Handle, pageFormat.GetHandle (), mime, fileUrl.Handle);
				GC.KeepAlive (settings);
				GC.KeepAlive (pageFormat);
				GC.KeepAlive (fileUrl);
				return status;
			} finally {
				CFString.ReleaseNative (mime);
			}
		}

		[DllImport (Constants.PrintCoreLibrary)]
		extern static PMStatusCode PMPrinterPrintWithProvider (IntPtr printer, IntPtr settings, IntPtr pageFormat, IntPtr strMimeType, IntPtr cgDataProvider);

		/// <param name="settings">To be added.</param>
		///         <param name="pageFormat">To be added.</param>
		///         <param name="provider">To be added.</param>
		///         <param name="mimeType">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public PMStatusCode TryPrintFromProvider (PMPrintSettings settings, PMPageFormat? pageFormat, CGDataProvider provider, string? mimeType = null)
		{
			if (settings is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (settings));
			if (provider is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (provider));

			IntPtr mime = CFString.CreateNative (mimeType);
			try {
				PMStatusCode status = PMPrinterPrintWithProvider (Handle, settings.Handle, pageFormat.GetHandle (), mime, provider.Handle);
				GC.KeepAlive (settings);
				GC.KeepAlive (pageFormat);
				GC.KeepAlive (provider);
				return status;
			} finally {
				CFString.ReleaseNative (mime);
			}
		}

		[DllImport (Constants.PrintCoreLibrary)]
		unsafe extern static PMStatusCode PMPrinterGetOutputResolution (IntPtr printer, IntPtr printSettings, PMResolution* resolutionP);
		[DllImport (Constants.PrintCoreLibrary)]
		unsafe extern static PMStatusCode PMPrinterSetOutputResolution (IntPtr printer, IntPtr printSettings, PMResolution* resolutionP);

		/// <param name="settings">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public PMResolution GetOutputResolution (PMPrintSettings settings)
		{
			if (settings is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (settings));

			PMResolution res;
			unsafe {
				PMStatusCode status = PMPrinterGetOutputResolution (Handle, settings.Handle, &res);
				GC.KeepAlive (settings);
				if (status == PMStatusCode.Ok)
					return res;
			}
			return new PMResolution (0, 0);
		}

		/// <param name="settings">To be added.</param>
		///         <param name="res">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public void SetOutputResolution (PMPrintSettings settings, PMResolution res)
		{
			if (settings is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (settings));
			unsafe {
				PMPrinterSetOutputResolution (Handle, settings.Handle, &res);
				GC.KeepAlive (settings);
			}
		}

		[DllImport (Constants.PrintCoreLibrary)]
		extern static PMStatusCode PMPrinterSetDefault (IntPtr printer);

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public PMStatusCode SetDefault ()
		{
			return PMPrinterSetDefault (Handle);
		}

		[DllImport (Constants.PrintCoreLibrary)]
		extern static byte PMPrinterIsFavorite (IntPtr printer);
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public bool IsFavorite => PMPrinterIsFavorite (Handle) != 0;

		[DllImport (Constants.PrintCoreLibrary)]
		extern static byte PMPrinterIsDefault (IntPtr printer);
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public bool IsDefault => PMPrinterIsDefault (Handle) != 0;

		[DllImport (Constants.PrintCoreLibrary)]
		extern static byte PMPrinterIsPostScriptCapable (IntPtr printer);
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public bool IsPostScriptCapable => PMPrinterIsPostScriptCapable (Handle) != 0;

		[DllImport (Constants.PrintCoreLibrary)]
		unsafe extern static PMStatusCode PMPrinterIsPostScriptPrinter (IntPtr printer, byte* isps);
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public bool IsPostScriptPrinter {
			get {
				byte r;
				unsafe {
					if (PMPrinterIsPostScriptPrinter (Handle, &r) == PMStatusCode.Ok)
						return r != 0;
				}
				return false;
			}
		}

		[DllImport (Constants.PrintCoreLibrary)]
		unsafe extern static PMStatusCode PMPrinterIsRemote (IntPtr printer, byte* isrem);
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public bool IsRemote {
			get {
				byte r;
				unsafe {
					if (PMPrinterIsRemote (Handle, &r) == PMStatusCode.Ok)
						return r != 0;
				}
				return false;
			}
		}

		[DllImport (Constants.PrintCoreLibrary)]
		extern static IntPtr PMPrinterGetID (IntPtr printer);

		public string? Id {
			get {
				return CFString.FromHandle (PMPrinterGetID (Handle));
			}
		}

		[DllImport (Constants.PrintCoreLibrary)]
		unsafe extern static PMStatusCode PMPrinterCopyHostName (IntPtr printer, IntPtr* hostName);

		public string? HostName {
			get {
				PMStatusCode code;
				IntPtr hostName;
				unsafe {
					code = PMPrinterCopyHostName (Handle, &hostName);
				}
				if (code != PMStatusCode.Ok)
					return null;
				return CFString.FromHandle (hostName, true);
			}
		}
	}

#if NET
	/// <summary>To be added.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("macos")]
#endif
	public class PMServer : PMPrintCoreBase {
		// A private constructor so that nobody can create an instance of this class.
		PMServer ()
			: base (IntPtr.Zero, true)
		{
		}

		[DllImport (Constants.PrintCoreLibrary)]
		extern static PMStatusCode PMServerLaunchPrinterBrowser (IntPtr server, IntPtr dictFutureUse);

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static PMStatusCode LaunchPrinterBrowser ()
		{
			return PMServerLaunchPrinterBrowser (IntPtr.Zero /* Server Local */, IntPtr.Zero);
		}

		[DllImport (Constants.PrintCoreLibrary)]
		unsafe extern static PMStatusCode PMServerCreatePrinterList (IntPtr server, IntPtr* printerListArray);
		/// <param name="printerList">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static PMStatusCode CreatePrinterList (out PMPrinter []? printerList)
		{
			PMStatusCode code;
			IntPtr arr;
			unsafe {
				code = PMServerCreatePrinterList (IntPtr.Zero /* ServerLocal */, &arr);
			}
			if (code != PMStatusCode.Ok) {
				printerList = null;
				return code;
			}
			printerList = CFArray.ArrayFromHandleFunc<PMPrinter> (arr, (handle) => new PMPrinter (handle, false), true);
			return PMStatusCode.Ok;
		}
	}
}
