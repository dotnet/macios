using AVFoundation;
using CoreGraphics;
using CoreVideo;
using VideoToolbox;

namespace SensitiveContentAnalysis {
	[NoTV, Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface SCSensitivityAnalysis {
		[Export ("sensitive")]
		bool Sensitive { [Bind ("isSensitive")] get; }

		// From the VideoStreamAnalysis (SCSensitiveAnalysis) category
		[NoTV, NoMacCatalyst, NoMac, iOS (26, 0)]
		[Export ("shouldInterruptVideo")]
		bool ShouldInterruptVideo { get; }

		// From the VideoStreamAnalysis (SCSensitiveAnalysis) category
		[NoTV, NoMacCatalyst, NoMac, iOS (26, 0)]
		[Export ("shouldIndicateSensitivity")]
		bool ShouldIndicateSensitivity { get; }

		// From the VideoStreamAnalysis (SCSensitiveAnalysis) category
		[NoTV, NoMacCatalyst, NoMac, iOS (26, 0)]
		[Export ("shouldMuteAudio")]
		bool ShouldMuteAudio { get; }
	}

	[NoTV, Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0)]
	[Native]
	public enum SCSensitivityAnalysisPolicy : long {
		Disabled = 0,
		SimpleInterventions = 1,
		DescriptiveInterventions = 2,
	}

	[NoTV, Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0)]
	[BaseType (typeof (NSObject))]
	interface SCSensitivityAnalyzer {
		[Export ("analysisPolicy", ArgumentSemantic.Assign)]
		SCSensitivityAnalysisPolicy AnalysisPolicy { get; }

		[Export ("analyzeImageFile:completionHandler:")]
		[Async]
		void AnalyzeImage (NSUrl fileUrl, Action<SCSensitivityAnalysis, NSError> completionHandler);

		[Export ("analyzeCGImage:completionHandler:")]
		[Async]
		void AnalyzeImage (CGImage image, Action<SCSensitivityAnalysis, NSError> completionHandler);

		[Export ("analyzeVideoFile:completionHandler:")]
		[Async]
		NSProgress AnalyzeVideo (NSUrl fileUrl, Action<SCSensitivityAnalysis, NSError> completionHandler);
	}

	[NoTV, NoMacCatalyst, NoMac, iOS (26, 0)]
	[Native]
	public enum SCVideoStreamAnalyzerStreamDirection : long {
		Outgoing = 1,
		Incoming = 2,
	}

	delegate void SCVideoStreamAnalysisChangeHandler ([NullAllowed] SCSensitivityAnalysis analysis, [NullAllowed] NSError error);

	[NoTV, NoMacCatalyst, NoMac, iOS (26, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface SCVideoStreamAnalyzer {
		[NullAllowed, Export ("analysis")]
		SCSensitivityAnalysis Analysis { get; }

		[Export ("analysisChangedHandler", ArgumentSemantic.Copy)]
		[NullAllowed]
		SCVideoStreamAnalysisChangeHandler AnalysisChangedHandler { get; set; }

		[Export ("initWithParticipantUUID:streamDirection:error:")]
		[Internal]
		NativeHandle _InitWithParticipantUuid (string participantUuid, SCVideoStreamAnalyzerStreamDirection streamDirection, [NullAllowed] out NSError error);

		[Export ("analyzePixelBuffer:")]
		void AnalyzePixelBuffer (CVPixelBuffer pixelBuffer);

		// From the SessionManagement (SCVideoStreamAnalyzer) category
		[Export ("beginAnalysisOfDecompressionSession:error:")]
		bool BeginAnalysisOfDecompressionSession (VTDecompressionSession decompressionSession, [NullAllowed] out NSError error);

		// From the SessionManagement (SCVideoStreamAnalyzer) category
		[Export ("beginAnalysisOfCaptureDeviceInput:error:")]
		bool BeginAnalysisOfCaptureDeviceInput (AVCaptureDeviceInput captureDeviceInput, [NullAllowed] out NSError error);

		// From the SessionManagement (SCVideoStreamAnalyzer) category
		[Export ("endAnalysis")]
		void EndAnalysis ();

		// From the SessionManagement (SCVideoStreamAnalyzer) category
		[Export ("continueStream")]
		void ContinueStream ();
	}
}
