//
// Speech bindings
//
// Authors:
//	Alex Soto  <alex.soto@xamarin.com>
//	TJ Lambert  <t-anlamb@microsoft.com>
//
// Copyright 2016 Xamarin Inc. All rights reserved.
// Copyright 2019 Microsoft Corporation All rights reserved.
//

using System;
using AVFoundation;
using CoreMedia;
using Foundation;
using ObjCRuntime;

using System.ComponentModel;

namespace Speech {

	/// <summary>Enumerates the states of a <see cref="Speech.SFSpeechRecognitionTask" />.</summary>
	[Native]
	[MacCatalyst (13, 1), TV (18, 0)]
	public enum SFSpeechRecognitionTaskState : long {
		/// <summary>The recognition task is beginning.</summary>
		Starting = 0,
		/// <summary>The task is attempting to recognize speech.</summary>
		Running = 1,
		/// <summary>The task is ending.</summary>
		Finishing = 2,
		/// <summary>Speech recognition is in the process of being canceled.</summary>
		Canceling = 3,
		/// <summary>Speech recognition has completed.</summary>
		Completed = 4,
	}

	[Native]
	[MacCatalyst (13, 1), TV (18, 0)]
	public enum SFSpeechRecognitionTaskHint : long {
		/// <summary>No hint exists.</summary>
		Unspecified = 0,
		/// <summary>Free-form dictation.</summary>
		Dictation = 1,
		/// <summary>A request that may involve keywords associated with search.</summary>
		Search = 2,
		/// <summary>Confirming or denying a request.</summary>
		Confirmation = 3,
	}

	/// <include file="../docs/api/Speech/SFSpeechRecognizerAuthorizationStatus.xml" path="/Documentation/Docs[@DocId='T:Speech.SFSpeechRecognizerAuthorizationStatus']/*" />
	[Native]
	[MacCatalyst (13, 1), TV (18, 0)]
	public enum SFSpeechRecognizerAuthorizationStatus : long {
		/// <summary>Permission has not been presented.</summary>
		NotDetermined,
		/// <summary>The user has denied permission for speech recognition.</summary>
		Denied,
		/// <summary>To be added.</summary>
		Restricted,
		/// <summary>The user has allowed speech recognition.</summary>
		Authorized,
	}

	[Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0), TV (18, 0)]
	[Native]
	[ErrorDomain ("SFSpeechErrorDomain")]
	public enum SFSpeechErrorCode : long {
		InternalServiceError = 1,
		AudioReadFailed = 2,
		UndefinedTemplateClassName = 7,
		MalformedSupplementalModel = 8,
		Timeout = 10,
	}

	/// <summary>Abstract base class for speech recognition requests (see <see cref="Speech.SFSpeechAudioBufferRecognitionRequest" /> and <see cref="Speech.SFSpeechUrlRecognitionRequest" />).</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/reference/Speech/SFSpeechRecognitionRequest">Apple documentation for <c>SFSpeechRecognitionRequest</c></related>
	[MacCatalyst (13, 1), TV (18, 0)]
	[DisableDefaultCtor]
	[Abstract] // no docs (yet) but it has no means (init*) to create it, unlike its subclasses
	[BaseType (typeof (NSObject))]
	interface SFSpeechRecognitionRequest {

		[Export ("taskHint", ArgumentSemantic.Assign)]
		SFSpeechRecognitionTaskHint TaskHint { get; set; }

		[Export ("shouldReportPartialResults", ArgumentSemantic.Assign)]
		bool ShouldReportPartialResults { get; set; }

		[Export ("contextualStrings", ArgumentSemantic.Copy)]
		string [] ContextualStrings { get; set; }

		[Deprecated (PlatformName.iOS, 15, 0)]
		[Deprecated (PlatformName.MacCatalyst, 15, 0)]
		[Deprecated (PlatformName.MacOSX, 12, 0)]
		[NullAllowed, Export ("interactionIdentifier")]
		string InteractionIdentifier { get; set; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Export ("requiresOnDeviceRecognition")]
		bool RequiresOnDeviceRecognition { get; set; }

		[Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0)]
		[Export ("addsPunctuation")]
		bool AddsPunctuation { get; set; }

		[Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0)]
		[NullAllowed, Export ("customizedLanguageModel", ArgumentSemantic.Copy)]
		SFSpeechLanguageModelConfiguration CustomizedLanguageModel { get; set; }
	}

	/// <summary>A <see cref="Speech.SFSpeechRecognitionRequest" /> whose audio source is specified in a URL</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/reference/Speech/SFSpeechURLRecognitionRequest">Apple documentation for <c>SFSpeechURLRecognitionRequest</c></related>
	[MacCatalyst (13, 1), TV (18, 0)]
	[BaseType (typeof (SFSpeechRecognitionRequest), Name = "SFSpeechURLRecognitionRequest")]
	[DisableDefaultCtor]
	interface SFSpeechUrlRecognitionRequest {

		[Export ("initWithURL:")]
		[DesignatedInitializer]
		NativeHandle Constructor (NSUrl url);

		[Export ("URL", ArgumentSemantic.Copy)]
		NSUrl Url { get; }
	}

	/// <summary>An <see cref="Speech.SFSpeechRecognitionRequest" /> that takes its input from an audio buffer.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/reference/Speech/SFSpeechAudioBufferRecognitionRequest">Apple documentation for <c>SFSpeechAudioBufferRecognitionRequest</c></related>
	[MacCatalyst (13, 1), TV (18, 0)]
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

	/// <summary>Contains transcriptions of a speech recognition task.</summary>
	///     <remarks>
	///       <para>
	///         <see cref="Speech.SFSpeechRecognitionResult" />contain a <see cref="Speech.SFTranscription" /> in<see cref="Speech.SFSpeechRecognitionResult.BestTranscription" /> that is the highest-confidence transcription. Additionally, the <see cref="Speech.SFSpeechRecognitionResult.Transcriptions" /> may contain additional <see cref="Speech.SFTranscription" /> objects that the developer may search for more domain-appropriate results.</para>
	///     </remarks>
	///     <related type="externalDocumentation" href="https://developer.apple.com/reference/Speech/SFSpeechRecognitionResult">Apple documentation for <c>SFSpeechRecognitionResult</c></related>
	[MacCatalyst (13, 1), TV (18, 0)]
	[BaseType (typeof (NSObject))]
	interface SFSpeechRecognitionResult : NSCopying, NSSecureCoding {

		[Export ("bestTranscription", ArgumentSemantic.Copy)]
		SFTranscription BestTranscription { get; }

		[Export ("transcriptions", ArgumentSemantic.Copy)]
		SFTranscription [] Transcriptions { get; }

		/// <summary>Gets whether this is the final attempt at recognition.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("final")]
		bool Final { [Bind ("isFinal")] get; }

		[iOS (14, 5)]
		[MacCatalyst (14, 5)]
		[NullAllowed, Export ("speechRecognitionMetadata")]
		SFSpeechRecognitionMetadata SpeechRecognitionMetadata { get; }
	}

	/// <summary>Object that holds state and provides control of an asynchronous speech recognition task.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/reference/Speech/SFSpeechRecognitionTask">Apple documentation for <c>SFSpeechRecognitionTask</c></related>
	[MacCatalyst (13, 1), TV (18, 0)]
	[BaseType (typeof (NSObject))]
	interface SFSpeechRecognitionTask {

		[Export ("state")]
		SFSpeechRecognitionTaskState State { get; }

		/// <summary>Gets whether the speech recognition activity is in its final stage.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("finishing")]
		bool Finishing { [Bind ("isFinishing")] get; }

		[Export ("finish")]
		void Finish ();

		/// <summary>Gets whether the speech recognition attempt was cancelled.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("cancelled")]
		bool Cancelled { [Bind ("isCancelled")] get; }

		[Export ("cancel")]
		void Cancel ();

		[NullAllowed, Export ("error", ArgumentSemantic.Copy)]
		NSError Error { get; }
	}

	interface ISFSpeechRecognitionTaskDelegate { }

	/// <summary>Delegate object whose members are called in reaction to speech-recognition events.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/reference/Speech/SFSpeechRecognitionTaskDelegate">Apple documentation for <c>SFSpeechRecognitionTaskDelegate</c></related>
	[MacCatalyst (13, 1), TV (18, 0)]
	[Protocol, Model]
	[BaseType (typeof (NSObject))]
	interface SFSpeechRecognitionTaskDelegate {

		/// <param name="task">The <see cref="Speech.SFSpeechRecognitionTask" /> for which this is the delegate object.</param>
		/// <summary>The system calls this method periodically as speech is detected.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("speechRecognitionDidDetectSpeech:")]
		void DidDetectSpeech (SFSpeechRecognitionTask task);

		/// <param name="task">The <see cref="Speech.SFSpeechRecognitionTask" /> for which this is the delegate object.</param>
		/// <param name="transcription">To be added.</param>
		/// <summary>The system calls this method periodically, as the speech recognition attempts to refine the results.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("speechRecognitionTask:didHypothesizeTranscription:")]
		void DidHypothesizeTranscription (SFSpeechRecognitionTask task, SFTranscription transcription);

		/// <param name="task">The <see cref="Speech.SFSpeechRecognitionTask" /> for which this is the delegate object.</param>
		/// <param name="recognitionResult">To be added.</param>
		/// <summary>The system calls this method  after it has completed recognition.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("speechRecognitionTask:didFinishRecognition:")]
		void DidFinishRecognition (SFSpeechRecognitionTask task, SFSpeechRecognitionResult recognitionResult);

		/// <param name="task">The <see cref="Speech.SFSpeechRecognitionTask" /> for which this is the delegate object.</param>
		/// <summary>Called by the system after the audio input has finished.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("speechRecognitionTaskFinishedReadingAudio:")]
		void FinishedReadingAudio (SFSpeechRecognitionTask task);

		/// <param name="task">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("speechRecognitionTaskWasCancelled:")]
		void WasCancelled (SFSpeechRecognitionTask task);

		/// <param name="task">The <see cref="Speech.SFSpeechRecognitionTask" /> for which this is the delegate object.</param>
		/// <param name="successfully">
		///           <see langword="true" /> if the speech recognition ended without error or cancellation.</param>
		/// <summary>The system calls this method after the <paramref name="task" /> has finished.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("speechRecognitionTask:didFinishSuccessfully:")]
		void DidFinishSuccessfully (SFSpeechRecognitionTask task, bool successfully);

		[iOS (18, 0), Mac (10, 15), TV (18, 0)]
		[Export ("speechRecognitionTask:didProcessAudioDuration:")]
		void DidProcessAudioDuration (SFSpeechRecognitionTask task, double duration);
	}

	interface ISFSpeechRecognizerDelegate { }

	/// <summary>Delegate object for <see cref="Speech.SFSpeechRecognizer" />.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/reference/Speech/SFSpeechRecognizerDelegate">Apple documentation for <c>SFSpeechRecognizerDelegate</c></related>
	[MacCatalyst (13, 1), TV (18, 0)]
	[Protocol, Model]
	[BaseType (typeof (NSObject))]
	interface SFSpeechRecognizerDelegate {

		/// <param name="speechRecognizer">To be added.</param>
		/// <param name="available">
		///           <see langword="true" /> if speech recognition is permitted.</param>
		/// <summary>The system calls this when the availability of speech recognition has been changed.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("speechRecognizer:availabilityDidChange:")]
		void AvailabilityDidChange (SFSpeechRecognizer speechRecognizer, bool available);
	}

	/// <include file="../docs/api/Speech/SFSpeechRecognizer.xml" path="/Documentation/Docs[@DocId='T:Speech.SFSpeechRecognizer']/*" />
	[MacCatalyst (13, 1), TV (18, 0)]
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
		NativeHandle Constructor (NSLocale locale);

		/// <summary>Gets whether speech recognition is currently available.</summary>
		///         <value>To be added.</value>
		///         <remarks>
		///           <para>Speech recognition is not supported on all devices and is always relient on Internet access.</para>
		///         </remarks>
		[Export ("available")]
		bool Available { [Bind ("isAvailable")] get; }

		[Export ("locale", ArgumentSemantic.Copy)]
		NSLocale Locale { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Export ("supportsOnDeviceRecognition")]
		bool SupportsOnDeviceRecognition { get; set; }

		[NullAllowed, Export ("delegate", ArgumentSemantic.Weak)]
		ISFSpeechRecognizerDelegate Delegate { get; set; }

		[Export ("defaultTaskHint", ArgumentSemantic.Assign)]
		SFSpeechRecognitionTaskHint DefaultTaskHint { get; set; }

		// no [Async] as this gets called multiple times, leading to an exception
		[Export ("recognitionTaskWithRequest:resultHandler:")]
		SFSpeechRecognitionTask GetRecognitionTask (SFSpeechRecognitionRequest request, Action<SFSpeechRecognitionResult, NSError> resultHandler);

		[Export ("recognitionTaskWithRequest:delegate:")]
		SFSpeechRecognitionTask GetRecognitionTask (SFSpeechRecognitionRequest request, ISFSpeechRecognitionTaskDelegate @delegate);

		[Export ("queue", ArgumentSemantic.Strong)]
		NSOperationQueue Queue { get; set; }
	}

	[iOS (14, 5), TV (18, 0)]
	[MacCatalyst (14, 5)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface SFSpeechRecognitionMetadata : NSCopying, NSSecureCoding {

		[Export ("speakingRate")]
		double SpeakingRate { get; }

		[Export ("averagePauseDuration")]
		double AveragePauseDuration { get; }

		[Export ("speechStartTimestamp")]
		double SpeechStartTimestamp { get; }

		[Export ("speechDuration")]
		double SpeechDuration { get; }

		[NullAllowed, Export ("voiceAnalytics")]
		SFVoiceAnalytics VoiceAnalytics { get; }
	}

	/// <summary>A conversion of speech into text.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/reference/Speech/SFTranscription">Apple documentation for <c>SFTranscription</c></related>
	[MacCatalyst (13, 1), TV (18, 0)]
	[BaseType (typeof (NSObject))]
	interface SFTranscription : NSCopying, NSSecureCoding {

		[Export ("formattedString")]
		string FormattedString { get; }

		[Export ("segments", ArgumentSemantic.Copy)]
		SFTranscriptionSegment [] Segments { get; }

		[iOS (13, 0)]
		[Export ("speakingRate")]
		[Deprecated (PlatformName.iOS, 14, 5)]
		[MacCatalyst (13, 1)]
		[Deprecated (PlatformName.MacCatalyst, 14, 5)]
		[Deprecated (PlatformName.TvOS, 14, 5)]
		[Advice ("Use 'SpeakingRate' from 'SFSpeechRecognitionMetadata' instead.")]
		double SpeakingRate { get; }

		[iOS (13, 0)]
		[Export ("averagePauseDuration")]
		[Deprecated (PlatformName.iOS, 14, 5)]
		[MacCatalyst (13, 1)]
		[Deprecated (PlatformName.MacCatalyst, 14, 5)]
		[Deprecated (PlatformName.TvOS, 14, 5)]
		[Advice ("Use 'AveragePauseDuration' from 'SFSpeechRecognitionMetadata' instead.")]
		double AveragePauseDuration { get; }
	}

	/// <related type="externalDocumentation" href="https://developer.apple.com/reference/Speech/SFTranscriptionSegment">Apple documentation for <c>SFTranscriptionSegment</c></related>
	[MacCatalyst (13, 1), TV (18, 0)]
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

		[iOS (13, 0)]
		[NullAllowed, Export ("voiceAnalytics")]
		[Deprecated (PlatformName.iOS, 14, 5)]
		[MacCatalyst (13, 1)]
		[Deprecated (PlatformName.MacCatalyst, 14, 5)]
		[Deprecated (PlatformName.TvOS, 14, 5)]
		[Advice ("Use 'VoiceAnalytics' from 'SFSpeechRecognitionMetadata' instead.")]
		SFVoiceAnalytics VoiceAnalytics { get; }
	}

	[iOS (13, 0)]
	[MacCatalyst (13, 1), TV (18, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface SFAcousticFeature : NSCopying, NSSecureCoding {

		[Export ("acousticFeatureValuePerFrame", ArgumentSemantic.Copy)]
		NSNumber [] AcousticFeatureValuePerFrame { get; }

		[Export ("frameDuration")]
		double FrameDuration { get; }
	}

	[iOS (13, 0)]
	[MacCatalyst (13, 1), TV (18, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface SFVoiceAnalytics : NSCopying, NSSecureCoding {

		[Export ("jitter", ArgumentSemantic.Copy)]
		SFAcousticFeature Jitter { get; }

		[Export ("shimmer", ArgumentSemantic.Copy)]
		SFAcousticFeature Shimmer { get; }

		[Export ("pitch", ArgumentSemantic.Copy)]
		SFAcousticFeature Pitch { get; }

		[Export ("voicing", ArgumentSemantic.Copy)]
		SFAcousticFeature Voicing { get; }
	}

	[Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0), TV (18, 0)]
	[BaseType (typeof (NSObject))]
	interface SFSpeechLanguageModelConfiguration : NSCopying, NSSecureCoding {
		[Export ("initWithLanguageModel:")]
		NativeHandle Constructor (NSUrl languageModel);

		[Export ("initWithLanguageModel:vocabulary:")]
		NativeHandle Constructor (NSUrl languageModel, [NullAllowed] NSUrl vocabulary);

		[Export ("languageModel", ArgumentSemantic.Copy)]
		NSUrl LanguageModel { get; }

		[NullAllowed, Export ("vocabulary", ArgumentSemantic.Copy)]
		NSUrl Vocabulary { get; }
	}

	[Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0), TV (18, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface SFSpeechLanguageModel {
		[Static]
		[Export ("prepareCustomLanguageModelForUrl:clientIdentifier:configuration:completion:")]
		[Async]
		void PrepareCustomModel (NSUrl asset, string clientIdentifier, SFSpeechLanguageModelConfiguration configuration, Action<NSError> completion);

		[Static]
		[Export ("prepareCustomLanguageModelForUrl:clientIdentifier:configuration:ignoresCache:completion:")]
		[Async]
		void PrepareCustomModel (NSUrl asset, string clientIdentifier, SFSpeechLanguageModelConfiguration configuration, bool ignoresCache, Action<NSError> completion);
	}
}
