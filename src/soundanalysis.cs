//
// SoundAnalysis C# bindings
//
// Authors:
//	TJ Lambert  <t-anlamb@microsoft.com>
//
// Copyright 2019 Microsoft Corporation All rights reserved.
//

using AVFoundation;
using CoreML;
using CoreMedia;

namespace SoundAnalysis {

	/// <summary>Enumerates the errors that Sound Analysis can report.</summary>
	[ErrorDomain ("SNErrorDomain")]
	[MacCatalyst (13, 1)]
	[Native]
	enum SNErrorCode : long {
		/// <summary>An unknown error occurred.</summary>
		UnknownError = 1,
		/// <summary>The operation failed.</summary>
		OperationFailed,
		/// <summary>The audio format is invalid.</summary>
		InvalidFormat,
		/// <summary>The model is invalid.</summary>
		InvalidModel,
		/// <summary>The file is invalid.</summary>
		InvalidFile,
	}

	/// <summary>Enumerates the types of a time duration constraint.</summary>
	[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
	[Native]
	public enum SNTimeDurationConstraintType : long {
		/// <summary>A duration must be one of a set of discrete permitted values.</summary>
		Enumerated = 1,
		/// <summary>A duration must be within a continuous range of permitted values.</summary>
		Range = 2,
	}

	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface SNAudioStreamAnalyzer {

		[DesignatedInitializer]
		[Export ("initWithFormat:")]
		NativeHandle Constructor (AVAudioFormat format);

		[Export ("addRequest:withObserver:error:")]
		bool AddRequest (ISNRequest request, ISNResultsObserving observer, [NullAllowed] out NSError error);

		[Export ("removeRequest:")]
		void RemoveRequest (ISNRequest request);

		[Export ("removeAllRequests")]
		void RemoveAllRequests ();

		[Export ("analyzeAudioBuffer:atAudioFramePosition:")]
		void Analyze (AVAudioBuffer audioBuffer, long audioFramePosition);

		[Export ("completeAnalysis")]
		void CompleteAnalysis ();
	}

	delegate void SNAudioFileAnalyzerAnalyzeHandler (bool didReachEndOfFile);

	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface SNAudioFileAnalyzer {

		[DesignatedInitializer]
		[Export ("initWithURL:error:")]
		NativeHandle Constructor (NSUrl url, [NullAllowed] out NSError error);

		[Export ("addRequest:withObserver:error:")]
		bool AddRequest (ISNRequest request, ISNResultsObserving observer, [NullAllowed] out NSError error);

		[Export ("removeRequest:")]
		void RemoveRequest (ISNRequest request);

		[Export ("removeAllRequests")]
		void RemoveAllRequests ();

		[Export ("analyze")]
		void Analyze ();

		[Async]
		[Export ("analyzeWithCompletionHandler:")]
		void Analyze (SNAudioFileAnalyzerAnalyzeHandler completionHandler);

		[Export ("cancelAnalysis")]
		void CancelAnalysis ();
	}

	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface SNClassification {

		[Export ("identifier")]
		string Identifier { get; }

		[Export ("confidence")]
		double Confidence { get; }
	}

	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface SNClassificationResult : SNResult {

		[Export ("classifications")]
		SNClassification [] Classifications { get; }

		[Export ("timeRange")]
		CMTimeRange TimeRange { get; }

		[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
		[Export ("classificationForIdentifier:")]
		[return: NullAllowed]
		SNClassification GetClassification (string identifier);
	}

	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface SNClassifySoundRequest : SNRequest {

		[Export ("overlapFactor")]
		double OverlapFactor { get; set; }

		[Export ("initWithMLModel:error:")]
		NativeHandle Constructor (MLModel mlModel, [NullAllowed] out NSError error);

		[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
		[Export ("initWithClassifierIdentifier:error:")]
		NativeHandle Constructor (string classifierIdentifier, [NullAllowed] out NSError error);

		[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
		[Export ("knownClassifications", ArgumentSemantic.Copy)]
		string [] KnownClassifications { get; }

		[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
		[Export ("windowDuration", ArgumentSemantic.Assign)]
		CMTime WindowDuration { get; set; }

		[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
		[Export ("windowDurationConstraint", ArgumentSemantic.Strong)]
		SNTimeDurationConstraint WindowDurationConstraint { get; }
	}

	interface ISNRequest { }

	[MacCatalyst (13, 1)]
	[Protocol]
	interface SNRequest { }

	interface ISNResult { }

	[MacCatalyst (13, 1)]
	[Protocol]
	interface SNResult { }

	interface ISNResultsObserving { }

	[MacCatalyst (13, 1)]
	[Protocol]
	interface SNResultsObserving {

		[Abstract]
		[Export ("request:didProduceResult:")]
		void DidProduceResult (ISNRequest request, ISNResult result);

		[Export ("request:didFailWithError:")]
		void DidFail (ISNRequest request, NSError error);

		[Export ("requestDidComplete:")]
		void DidComplete (ISNRequest request);
	}

	[iOS (15, 0), TV (15, 0), MacCatalyst (15, 0)]
	enum SNClassifierIdentifier {
		[Field ("SNClassifierIdentifierVersion1")]
		Version1,
	}

	[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface SNTimeDurationConstraint /* privately conforms to NSCoding, NSCopying, and NSSecureCoding */
	{
		[Export ("initWithEnumeratedDurations:")]
		NativeHandle Constructor ([BindAs (typeof (CMTime []))] NSValue [] enumeratedDurations);

		[Export ("initWithDurationRange:")]
		NativeHandle Constructor (CMTimeRange durationRange);

		[Export ("type", ArgumentSemantic.Assign)]
		SNTimeDurationConstraintType Type { get; }

		[BindAs (typeof (CMTime []))]
		[Export ("enumeratedDurations", ArgumentSemantic.Strong)]
		NSValue [] EnumeratedDurations { get; }

		[Export ("durationRange", ArgumentSemantic.Assign)]
		CMTimeRange DurationRange { get; }
	}
}
