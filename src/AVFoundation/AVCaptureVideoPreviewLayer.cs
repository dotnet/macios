#if !TVOS

using System;

using Foundation;
using CoreFoundation;
using ObjCRuntime;
using AudioToolbox;

#nullable enable

namespace AVFoundation {
	public partial class AVCaptureVideoPreviewLayer {

		public enum InitMode {
			/// <summary>Indicates a connection.</summary>
			WithConnection,
			/// <summary>Indicates no connection.</summary>
			[SupportedOSPlatform ("ios")]
			[SupportedOSPlatform ("macos")]
			[SupportedOSPlatform ("maccatalyst")]
			WithNoConnection,
		}

		/// <param name="session">To be added.</param>
		///         <param name="mode">To be added.</param>
		///         <summary>Creates a new preview layer with the supplied capture session and initialization mode.</summary>
		///         <remarks>To be added.</remarks>
		public AVCaptureVideoPreviewLayer (AVCaptureSession session, InitMode mode) : base (NSObjectFlag.Empty)
		{
			switch (mode) {
			case InitMode.WithConnection:
				InitializeHandle (InitWithConnection (session));
				break;
			case InitMode.WithNoConnection:
				InitializeHandle (InitWithNoConnection (session));
				break;
			default:
				throw new ArgumentException (nameof (mode));
			}
		}

		/// <param name="session">To be added.</param>
		///         <summary>Creates a new preview layer with the supplied capture session.</summary>
		///         <remarks>To be added.</remarks>
		public AVCaptureVideoPreviewLayer (AVCaptureSession session) : this (session, InitMode.WithConnection) { }
	}
}

#endif
