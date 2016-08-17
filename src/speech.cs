﻿//
// Speech bindings
//
// Authors:
//	Alex Soto  <alex.soto@xamarin.com>
//
// Copyright 2016 Xamarin Inc. All rights reserved.
//

using System;
using XamCore.AVFoundation;
using XamCore.CoreMedia;
using XamCore.Foundation;
using XamCore.ObjCRuntime;

namespace XamCore.Speech {

	[Native]
	[Introduced (PlatformName.iOS, 10, 0)]
	public enum SFSpeechRecognitionTaskState : nint {
		Starting = 0,
		Running = 1,
		Finishing = 2,
		Canceling = 3,
		Completed = 4
	}

	[Native]
	[Introduced (PlatformName.iOS, 10, 0)]
	public enum SFSpeechRecognitionTaskHint : nint {
		Unspecified = 0,
		Dictation = 1,
		Search = 2,
		Confirmation = 3
	}

	[Native]
	[Introduced (PlatformName.iOS, 10, 0)]
	public enum SFSpeechRecognizerAuthorizationStatus : nint {
		NotDetermined,
		Denied,
		Restricted,
		Authorized
	}

	[Introduced (PlatformName.iOS, 10, 0)]
	[DisableDefaultCtor]
	[BaseType (typeof (NSObject))]
	interface SFSpeechRecognitionRequest {

		[Export ("taskHint", ArgumentSemantic.Assign)]
		SFSpeechRecognitionTaskHint TaskHint { get; set; }

		[Export ("shouldReportPartialResults", ArgumentSemantic.Assign)]
		bool ShouldReportPartialResults { get; set; }

		[Export ("contextualStrings", ArgumentSemantic.Copy)]
		string [] ContextualStrings { get; set; }

		[NullAllowed, Export ("interactionIdentifier")]
		string InteractionIdentifier { get; set; }
	}

	[Introduced (PlatformName.iOS, 10, 0)]
	[BaseType (typeof (SFSpeechRecognitionRequest), Name = "SFSpeechURLRecognitionRequest")]
	[DisableDefaultCtor]
	interface SFSpeechUrlRecognitionRequest {

		[Export ("initWithURL:")]
		[DesignatedInitializer]
		IntPtr Constructor (NSUrl url);

		[Export ("URL", ArgumentSemantic.Copy)]
		NSUrl Url { get; }
	}

	[Introduced (PlatformName.iOS, 10, 0)]
	[BaseType (typeof (SFSpeechRecognitionRequest))]
	interface SFSpeechAudioBufferRecognitionRequest {

		[Export ("nativeAudioFormat")]
		AVAudioFormat NativeAudioFormat { get; }

		[Export ("appendAudioPCMBuffer:")]
		void Append (AVAudioPcmBuffer audioPcmBuffer);

		[Export ("appendAudioSampleBuffer:")]
		void Append (CMSampleBuffer sampleBuffer);

		[Export ("endAudio")]
		void EndAudio ();
	}

	[Introduced (PlatformName.iOS, 10, 0)]
	[BaseType (typeof (NSObject))]
	interface SFSpeechRecognitionResult : NSCopying, NSSecureCoding {

		[Export ("bestTranscription", ArgumentSemantic.Copy)]
		SFTranscription BestTranscription { get; }

		[Export ("transcriptions", ArgumentSemantic.Copy)]
		SFTranscription [] Transcriptions { get; }

		[Export ("final")]
		bool Final { [Bind ("isFinal")] get; }
	}

	[Introduced (PlatformName.iOS, 10, 0)]
	[BaseType (typeof (NSObject))]
	interface SFSpeechRecognitionTask {

		[Export ("state")]
		SFSpeechRecognitionTaskState State { get; }

		[Export ("finishing")]
		bool Finishing { [Bind ("isFinishing")] get; }

		[Export ("finish")]
		void Finish ();

		[Export ("cancelled")]
		bool Cancelled { [Bind ("isCancelled")] get; }

		[Export ("cancel")]
		void Cancel ();

		[NullAllowed, Export ("error", ArgumentSemantic.Copy)]
		NSError Error { get; }
	}

	interface ISFSpeechRecognitionTaskDelegate { }

	[Introduced (PlatformName.iOS, 10, 0)]
	[Protocol, Model]
	[BaseType (typeof (NSObject))]
	interface SFSpeechRecognitionTaskDelegate {

		[Export ("speechRecognitionDidDetectSpeech:")]
		void DidDetectSpeech (SFSpeechRecognitionTask task);

		[Export ("speechRecognitionTask:didHypothesizeTranscription:")]
		void DidHypothesizeTranscription (SFSpeechRecognitionTask task, SFTranscription transcription);

		[Export ("speechRecognitionTask:didFinishRecognition:")]
		void DidFinishRecognition (SFSpeechRecognitionTask task, SFSpeechRecognitionResult recognitionResult);

		[Export ("speechRecognitionTaskFinishedReadingAudio:")]
		void FinishedReadingAudio (SFSpeechRecognitionTask task);

		[Export ("speechRecognitionTaskWasCancelled:")]
		void TaskWasCancelled (SFSpeechRecognitionTask task);

		[Export ("speechRecognitionTask:didFinishSuccessfully:")]
		void DidFinishSuccessfully (SFSpeechRecognitionTask task, bool successfully);
	}

	interface ISFSpeechRecognizerDelegate { }

	[Introduced (PlatformName.iOS, 10, 0)]
	[Protocol, Model]
	[BaseType (typeof (NSObject))]
	interface SFSpeechRecognizerDelegate {

		[Export ("speechRecognizer:availabilityDidChange:")]
		void AvailabilityDidChange (SFSpeechRecognizer speechRecognizer, bool available);
	}

	[Introduced (PlatformName.iOS, 10, 0)]
	[BaseType (typeof (NSObject))]
	interface SFSpeechRecognizer {

		[Static]
		[Export ("supportedLocales")]
		NSSet<NSLocale> SupportedLocales { get; }

		[Static]
		[Export ("authorizationStatus")]
		SFSpeechRecognizerAuthorizationStatus AuthorizationStatus { get; }

		[Static]
		[Export ("requestAuthorization:")]
		void RequestAuthorization (Action<SFSpeechRecognizerAuthorizationStatus> handler);

		[Export ("initWithLocale:")]
		[DesignatedInitializer]
		IntPtr Constructor (NSLocale locale);

		[Export ("available")]
		bool Available { [Bind ("isAvailable")] get; }

		[Export ("locale", ArgumentSemantic.Copy)]
		NSLocale Locale { get; }

		[NullAllowed, Export ("delegate", ArgumentSemantic.Weak)]
		ISFSpeechRecognizerDelegate Delegate { get; set; }

		[Export ("defaultTaskHint", ArgumentSemantic.Assign)]
		SFSpeechRecognitionTaskHint DefaultTaskHint { get; set; }

		[Export ("recognitionTaskWithRequest:resultHandler:")]
		SFSpeechRecognitionTask GetRecognitionTask (SFSpeechRecognitionRequest request, Action<SFSpeechRecognitionResult, NSError> resultHandler);

		[Export ("recognitionTaskWithRequest:delegate:")]
		SFSpeechRecognitionTask GetRecognitionTask (SFSpeechRecognitionRequest request, ISFSpeechRecognitionTaskDelegate aDelegate);

		[Export ("queue", ArgumentSemantic.Strong)]
		NSOperationQueue Queue { get; set; }
	}

	[Introduced (PlatformName.iOS, 10, 0)]
	[BaseType (typeof (NSObject))]
	interface SFTranscription : NSCopying, NSSecureCoding {

		[Export ("formattedString")]
		string FormattedString { get; }

		[Export ("segments", ArgumentSemantic.Copy)]
		SFTranscriptionSegment [] Segments { get; }
	}

	[Introduced (PlatformName.iOS, 10, 0)]
	[BaseType (typeof (NSObject))]
	interface SFTranscriptionSegment : NSCopying, NSSecureCoding {

		[Export ("substring")]
		string Substring { get; }

		[Export ("substringRange")]
		NSRange SubstringRange { get; }

		[Export ("timestamp")]
		double Timestamp { get; }

		[Export ("duration")]
		double Duration { get; }

		[Export ("confidence")]
		float Confidence { get; }

		[Export ("alternativeSubstrings")]
		string [] AlternativeSubstrings { get; }
	}
}

