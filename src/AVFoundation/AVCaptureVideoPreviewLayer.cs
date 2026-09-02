#if !TVOS

using CoreFoundation;
using AudioToolbox;

#nullable enable

namespace AVFoundation {
	public partial class AVCaptureVideoPreviewLayer {

		/// <summary>Enumerates values that specify the presence or absence of a capture session connection.</summary>
		public enum InitMode {
			/// <summary>Creates the preview layer with a connection to the capture session.</summary>
			WithConnection,
			/// <summary>Creates the preview layer without a connection to the capture session.</summary>
			[SupportedOSPlatform ("ios")]
			[SupportedOSPlatform ("macos")]
			[SupportedOSPlatform ("maccatalyst")]
			WithNoConnection,
		}

		/// <summary>Creates a new preview layer with the supplied capture session and initialization mode.</summary>
		/// <param name="session">The capture session to preview.</param>
		/// <param name="mode">The mode that determines whether to create a connection to <paramref name="session" />.</param>
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

		/// <summary>Creates a new preview layer connected to the supplied capture session.</summary>
		/// <param name="session">The capture session to preview.</param>
		public AVCaptureVideoPreviewLayer (AVCaptureSession session) : this (session, InitMode.WithConnection) { }
	}
}

#endif
