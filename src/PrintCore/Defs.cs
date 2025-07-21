//
// Defs.cs: PrintCore definitions
//
// Authors:
//   Miguel de Icaza (miguel@gnome.org)
//
// Copyright 2016 Microsoft Inc
//

#nullable enable

using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.IO;
using ObjCRuntime;
using Foundation;

using PMObject = System.IntPtr;
using System.Runtime.Versioning;

namespace PrintCore {

	/// <summary>To be added.</summary>
	///     <remarks>To be added.</remarks>
	public enum PMStatusCode {
		/// <summary>To be added.</summary>
		Ok = 0,
		/// <summary>To be added.</summary>
		GeneralError = -30870,
		/// <summary>To be added.</summary>
		OutOfScope = -30871,
		/// <summary>To be added.</summary>
		InvalidParameter = -50,
		/// <summary>To be added.</summary>
		NoDefaultPrinter = -30872,
		/// <summary>To be added.</summary>
		NotImplemented = -30873,
		/// <summary>To be added.</summary>
		NoSuchEntry = -30874,
		/// <summary>To be added.</summary>
		InvalidPrintSettings = -30875,
		/// <summary>To be added.</summary>
		InvalidPageFormat = -30876,
		/// <summary>To be added.</summary>
		ValueOutOfRange = -30877,
		/// <summary>To be added.</summary>
		LockIgnored = -30878,

		/// <summary>To be added.</summary>
		InvalidPrintSession = -30879,
		/// <summary>To be added.</summary>
		InvalidPrinter = -30880,
		/// <summary>To be added.</summary>
		ObjectInUse = -30881,
		/// <summary>To be added.</summary>
		InvalidPreset = -30899,

		/// <summary>To be added.</summary>
		AllocationFailure = -108,
		/// <summary>To be added.</summary>
		InternalError = -30870,

		/// <summary>To be added.</summary>
		InvalidIndex = -30882,
		/// <summary>To be added.</summary>
		StringConversionFailure = -30883,
		/// <summary>To be added.</summary>
		XMLParseError = -30884,

		/// <summary>To be added.</summary>
		InvalidJobTemplate = -30885,
		/// <summary>To be added.</summary>
		InvalidPrinterInfo = -30886,
		/// <summary>To be added.</summary>
		InvalidConnection = -30887,
		/// <summary>To be added.</summary>
		InvalidKey = -30888,
		/// <summary>To be added.</summary>
		InvalidValue = -30889,
		/// <summary>To be added.</summary>
		InvalidAllocator = -30890,
		/// <summary>To be added.</summary>
		InvalidTicket = -30891,
		/// <summary>To be added.</summary>
		InvalidItem = -30892,
		/// <summary>To be added.</summary>
		InvalidType = -30893,
		/// <summary>To be added.</summary>
		InvalidReply = -30894,
		/// <summary>To be added.</summary>
		InvalidFileType = -30895,
		/// <summary>To be added.</summary>
		InvalidObject = -30896,
		/// <summary>To be added.</summary>
		InvalidPaper = -30897,
		/// <summary>To be added.</summary>
		InvalidCalibrationTarget = -30898,

		/// <summary>To be added.</summary>
		NoDefaultItem = -9500,
		/// <summary>To be added.</summary>
		NoDefaultSettings = -9501,
		/// <summary>To be added.</summary>
		InvalidPdeContext = -9530,
		/// <summary>To be added.</summary>
		DontSwitchPdeError = -9531,
		/// <summary>To be added.</summary>
		UnableToFindProcess = -9532,
		/// <summary>To be added.</summary>
		FeatureNotInstalled = -9533,

		/// <summary>To be added.</summary>
		InvalidPbmRef = -9540,
		/// <summary>To be added.</summary>
		NoSelectedPrinters = -9541,
		/// <summary>To be added.</summary>
		InvalidLookupSpec = -9542,
		/// <summary>To be added.</summary>
		SyncRequestFailed = -9543,
		/// <summary>To be added.</summary>
		EditRequestFailed = -9544,
		/// <summary>To be added.</summary>
		PrBrowserNoUI = -9545,

		/// <summary>To be added.</summary>
		TicketTypeNotFound = -9580,
		/// <summary>To be added.</summary>
		UpdateTicketFailed = -9581,
		/// <summary>To be added.</summary>
		ValidateTicketFailed = -9582,
		/// <summary>To be added.</summary>
		SubTicketNotFound = -9583,
		/// <summary>To be added.</summary>
		InvalidSubTicket = -9584,
		/// <summary>To be added.</summary>
		DeleteSubTicketFailed = -9585,
		/// <summary>To be added.</summary>
		ItemIsLocked = -9586,
		/// <summary>To be added.</summary>
		TicketIsLocked = -9587,
		/// <summary>To be added.</summary>
		TemplateIsLocked = -9588,
		/// <summary>To be added.</summary>
		KeyNotFound = -9589,
		/// <summary>To be added.</summary>
		KeyNotUnique = -9590,
		/// <summary>To be added.</summary>
		UnknownDataType = -9591,

		/// <summary>To be added.</summary>
		CreateMessageFailed = -9620,
		/// <summary>To be added.</summary>
		ServerCommunicationFailed = -9621,
		/// <summary>To be added.</summary>
		KeyOrValueNotFound = -9623,
		/// <summary>To be added.</summary>
		MessagingError = -9624,

		/// <summary>To be added.</summary>
		ServerNotFound = -9630,
		/// <summary>To be added.</summary>
		ServerAlreadyRunning = -9631,
		/// <summary>To be added.</summary>
		ServerSuspended = -9632,
		/// <summary>To be added.</summary>
		ServerAttributeRestricted = -9633,
		/// <summary>To be added.</summary>
		FileOrDirOperationFailed = -9634,
		/// <summary>To be added.</summary>
		UserOrGroupNotFound = -9635,
		/// <summary>To be added.</summary>
		PermissionError = -9636,
		/// <summary>To be added.</summary>
		UnknownMessage = -9637,
		/// <summary>To be added.</summary>
		QueueNotFound = -9638,
		/// <summary>To be added.</summary>
		QueueAlreadyExists = -9639,
		/// <summary>To be added.</summary>
		QueueJobFailed = -9640,
		/// <summary>To be added.</summary>
		JobNotFound = -9641,
		/// <summary>To be added.</summary>
		JobBusy = -9642,
		/// <summary>To be added.</summary>
		JobCanceled = -9643,
		/// <summary>To be added.</summary>
		DocumentNotFound = -9644,

		/// <summary>To be added.</summary>
		PMSymbolNotFound = -9660,
		/// <summary>To be added.</summary>
		IOMSymbolNotFound = -9661,
		/// <summary>To be added.</summary>
		CvmSymbolNotFound = -9662,
		/// <summary>To be added.</summary>
		InvalidPMContext = -9663,
		/// <summary>To be added.</summary>
		InvalidIOMContext = -9664,
		/// <summary>To be added.</summary>
		InvalidCvmContext = -9665,
		/// <summary>To be added.</summary>
		InvalidJobID = -9666,
		/// <summary>To be added.</summary>
		NoPrinterJobID = -9667,
		/// <summary>To be added.</summary>
		JobStreamOpenFailed = -9668,
		/// <summary>To be added.</summary>
		JobStreamReadFailed = -9669,
		/// <summary>To be added.</summary>
		JobStreamEndError = -9670,
		/// <summary>To be added.</summary>
		JobManagerAborted = -9671,
		/// <summary>To be added.</summary>
		JobGetTicketBadFormatError = -9672,
		/// <summary>To be added.</summary>
		JobGetTicketReadError = -9673,

		/// <summary>To be added.</summary>
		PluginNotFound = -9701,
		/// <summary>To be added.</summary>
		PluginRegisterationFailed = -9702,
		/// <summary>To be added.</summary>
		FontNotFound = -9703,
		/// <summary>To be added.</summary>
		FontNameTooLong = -9704,
		/// <summary>To be added.</summary>
		GeneralCGError = -9705,
		/// <summary>To be added.</summary>
		InvalidState = -9706,
		/// <summary>To be added.</summary>
		UnexpectedImagingError = -9707,

		/// <summary>To be added.</summary>
		InvalidPrinterAddress = -9780,
		/// <summary>To be added.</summary>
		OpenFailed = -9781,
		/// <summary>To be added.</summary>
		ReadFailed = -9782,
		/// <summary>To be added.</summary>
		WriteFailed = -9783,
		/// <summary>To be added.</summary>
		StatusFailed = -9784,
		/// <summary>To be added.</summary>
		CloseFailed = -9785,
		/// <summary>To be added.</summary>
		UnsupportedConnection = -9786,
		/// <summary>To be added.</summary>
		IOAttrNotAvailable = -9787,
		/// <summary>To be added.</summary>
		ReadGotZeroData = -9788,
	}

	/// <summary>To be added.</summary>
	///     <remarks>To be added.</remarks>
	public enum PMPrinterState : System.UInt16 {
		/// <summary>To be added.</summary>
		Idle = 3,
		/// <summary>To be added.</summary>
		Processing = 4,
		/// <summary>To be added.</summary>
		Stopped = 5,
	}

	/// <summary>To be added.</summary>
	///     <remarks>To be added.</remarks>
	public enum PMDuplexMode : System.UInt32 {
		/// <summary>To be added.</summary>
		None = 1,
		/// <summary>To be added.</summary>
		NoTumble = 2,
		/// <summary>To be added.</summary>
		Tumble = 3,
		/// <summary>To be added.</summary>
		SimplexTumble = 4,
	}

	/// <summary>To be added.</summary>
	///     <remarks>To be added.</remarks>
	public enum PMOrientation : System.UInt16 {
		/// <summary>To be added.</summary>
		Portrait = 1,
		/// <summary>To be added.</summary>
		Landscape = 2,
		/// <summary>To be added.</summary>
		ReversePortrait = 3,
		/// <summary>To be added.</summary>
		ReverseLandscape = 4,
	}

	/// <summary>To be added.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("macos")]
	[StructLayout (LayoutKind.Sequential)]
	public struct PMResolution {

		double hRes;
		double vRes;
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public double HorizontalResolution => hRes;
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public double VerticalResolution => vRes;

		/// <param name="horizontal">To be added.</param>
		///         <param name="vertical">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public PMResolution (double horizontal, double vertical)
		{
			hRes = horizontal;
			vRes = vertical;
		}
		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public override string ToString ()
		{
			return $"[HorizontalResolution={hRes},VerticalResolution={vRes}]";
		}
	}

	/// <summary>To be added.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("macos")]
	[StructLayout (LayoutKind.Sequential)]
	public struct PMRect {
		internal double top;
		internal double left;
		internal double bottom;
		internal double right;

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public double Top => top;
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public double Left => left;
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public double Bottom => bottom;
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public double Right => right;

		/// <param name="top">To be added.</param>
		///         <param name="bottom">To be added.</param>
		///         <param name="left">To be added.</param>
		///         <param name="right">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public PMRect (double top, double bottom, double left, double right)
		{
			this.top = top;
			this.bottom = bottom;
			this.left = left;
			this.right = right;
		}

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public override string ToString ()
		{
			return $"[Top={top},Bottom={bottom},Left={left},Right={right}]";
		}
	}

	/// <summary>To be added.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("macos")]
	[StructLayout (LayoutKind.Sequential)]
	public struct PMPaperMargins {
		PMRect Rect;

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public double Top => Rect.top;
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public double Left => Rect.left;
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public double Bottom => Rect.bottom;
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public double Right => Rect.right;

		/// <param name="top">To be added.</param>
		///         <param name="bottom">To be added.</param>
		///         <param name="left">To be added.</param>
		///         <param name="right">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public PMPaperMargins (double top, double bottom, double left, double right)
		{
			Rect = new PMRect (top, bottom, left, right);
		}

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public override string ToString ()
		{
			return Rect.ToString ();
		}
	}
}
