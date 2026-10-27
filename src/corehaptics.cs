//
// CoreHaptics C# bindings
//
// Authors:
//	Manuel de la Pena Saenz <mandel@microsoft.com>
//
// Copyright 2019 Microsoft Corporation All rights reserved.
//

using AVFoundation;
using CoreFoundation;

namespace CoreHaptics {

	// we are not binding the API on Mac OS X yet due to an issue on Apples side: https://github.com/xamarin/maccore/issues/1951
#if MONOMAC || TV
	interface AVAudioSession { }
#endif

	[TV (14, 0)]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface CHHapticEventParameter {
		[BindAs (typeof (CHHapticEventParameterId))]
		[Export ("parameterID")]
		NSString ParameterId { get; }

		[Export ("value")]
		float Value { get; set; }

		[Export ("initWithParameterID:value:")]
		[DesignatedInitializer]
		NativeHandle Constructor ([BindAs (typeof (CHHapticEventParameterId))] NSString parameterId, float value);
	}

	[TV (14, 0)]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface CHHapticDynamicParameter {
		[BindAs (typeof (CHHapticDynamicParameterId))]
		[Export ("parameterID")]
		NSString ParameterId { get; }

		[Export ("value")]
		float Value { get; set; }

		/// <summary>Gets or sets the relative time.</summary>
		/// <value>The relative time, in seconds.</value>
		[Export ("relativeTime")]
		double RelativeTime { get; set; }

		/// <summary>Creates a new <see cref="CHHapticDynamicParameter" /> instance.</summary>
		/// <param name="parameterId">The parameter identifier.</param>
		/// <param name="value">The value.</param>
		/// <param name="time">The time, in seconds.</param>
		[Export ("initWithParameterID:value:relativeTime:")]
		[DesignatedInitializer]
		NativeHandle Constructor ([BindAs (typeof (CHHapticDynamicParameterId))] NSString parameterId, float value, double time);
	}

	[TV (14, 0)]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface CHHapticParameterCurveControlPoint {
		/// <summary>Gets or sets the relative time.</summary>
		/// <value>The relative time, in seconds.</value>
		[Export ("relativeTime")]
		double RelativeTime { get; set; }

		[Export ("value")]
		float Value { get; set; }

		/// <summary>Creates a new <see cref="CHHapticParameterCurveControlPoint" /> instance.</summary>
		/// <param name="time">The time, in seconds.</param>
		/// <param name="value">The value.</param>
		[Export ("initWithRelativeTime:value:")]
		[DesignatedInitializer]
		NativeHandle Constructor (double time, float value);
	}

	[TV (14, 0)]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface CHHapticParameterCurve {
		[BindAs (typeof (CHHapticDynamicParameterId))]
		[Export ("parameterID")]
		NSString ParameterId { get; }

		/// <summary>Gets or sets the relative time.</summary>
		/// <value>The relative time, in seconds.</value>
		[Export ("relativeTime")]
		double RelativeTime { get; set; }

		[Export ("controlPoints")]
		CHHapticParameterCurveControlPoint [] ControlPoints { get; }

		/// <summary>Creates a new <see cref="CHHapticParameterCurve" /> instance.</summary>
		/// <param name="parameterId">The parameter identifier.</param>
		/// <param name="controlPoints">The control points.</param>
		/// <param name="relativeTime">The relative time, in seconds.</param>
		[Export ("initWithParameterID:controlPoints:relativeTime:")]
		[DesignatedInitializer]
		NativeHandle Constructor ([BindAs (typeof (CHHapticDynamicParameterId))] NSString parameterId, CHHapticParameterCurveControlPoint [] controlPoints, double relativeTime);
	}

	[TV (14, 0)]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface CHHapticEvent {
		[BindAs (typeof (CHHapticEventType))]
		[Export ("type")]
		NSString Type { get; }

		[Export ("eventParameters")]
		CHHapticEventParameter [] EventParameters { get; }

		/// <summary>Gets or sets the relative time.</summary>
		/// <value>The relative time, in seconds.</value>
		[Export ("relativeTime")]
		double RelativeTime { get; set; }

		/// <summary>Gets or sets the duration.</summary>
		/// <value>The duration, in seconds.</value>
		[Export ("duration")]
		double Duration { get; set; }

		/// <summary>Creates a new <see cref="CHHapticEvent" /> instance.</summary>
		/// <param name="type">The type.</param>
		/// <param name="eventParams">The event parameters.</param>
		/// <param name="time">The time, in seconds.</param>
		[Export ("initWithEventType:parameters:relativeTime:")]
		NativeHandle Constructor ([BindAs (typeof (CHHapticEventType))] NSString type, CHHapticEventParameter [] eventParams, double time);

		/// <summary>Creates a new <see cref="CHHapticEvent" /> instance.</summary>
		/// <param name="type">The type.</param>
		/// <param name="eventParams">The event parameters.</param>
		/// <param name="time">The time, in seconds.</param>
		/// <param name="duration">The duration, in seconds.</param>
		[Export ("initWithEventType:parameters:relativeTime:duration:")]
		NativeHandle Constructor ([BindAs (typeof (CHHapticEventType))] NSString type, CHHapticEventParameter [] eventParams, double time, double duration);

		/// <summary>Creates a new <see cref="CHHapticEvent" /> instance.</summary>
		/// <param name="resourceId">The audio resource identifier.</param>
		/// <param name="eventParams">The event parameters.</param>
		/// <param name="time">The time, in seconds.</param>
		[Export ("initWithAudioResourceID:parameters:relativeTime:")]
		NativeHandle Constructor (nuint resourceId, CHHapticEventParameter [] eventParams, double time);

		/// <summary>Creates a new <see cref="CHHapticEvent" /> instance.</summary>
		/// <param name="resourceId">The audio resource identifier.</param>
		/// <param name="eventParams">The event parameters.</param>
		/// <param name="time">The time, in seconds.</param>
		/// <param name="duration">The duration, in seconds.</param>
		[Export ("initWithAudioResourceID:parameters:relativeTime:duration:")]
		NativeHandle Constructor (nuint resourceId, CHHapticEventParameter [] eventParams, double time, double duration);
	}

	interface ICHHapticParameterAttributes { }

	[TV (14, 0)]
	[MacCatalyst (13, 1)]
	[Protocol]
	interface CHHapticParameterAttributes {
		[Abstract]
		[Export ("minValue")]
		float MinValue { get; }

		[Abstract]
		[Export ("maxValue")]
		float MaxValue { get; }

		[Abstract]
		[Export ("defaultValue")]
		float DefaultValue { get; }
	}

	interface ICHHapticDeviceCapability { }

	[TV (14, 0)]
	[MacCatalyst (13, 1)]
	[Protocol]
	interface CHHapticDeviceCapability {
		[Abstract]
		[Export ("supportsHaptics")]
		bool SupportsHaptics { get; }

		[Abstract]
		[Export ("supportsAudio")]
		bool SupportsAudio { get; }

		// Protocols do not like BindAs yet we know is the enum CHHapticEventType
		[Abstract]
		[Export ("attributesForEventParameter:eventType:error:")]
		[return: NullAllowed]
		ICHHapticParameterAttributes GetAttributes (NSString eventParameter, string type, [NullAllowed] out NSError outError);

		// Protocols do not like BindAs yet we know is the enum CHHapticEventType
		[Abstract]
		[Export ("attributesForDynamicParameter:error:")]
		[return: NullAllowed]
		ICHHapticParameterAttributes GetAttributes (NSString eventParameter, [NullAllowed] out NSError outError);
	}

	interface ICHHapticPatternPlayer { }

	[TV (14, 0)]
	[MacCatalyst (13, 1)]
	[Protocol]
	interface CHHapticPatternPlayer {
		/// <summary>Starts playback at the specified time.</summary>
		/// <param name="time">The time, in seconds.</param>
		/// <param name="outError">On return, the error if the operation failed.</param>
		[Abstract]
		[Export ("startAtTime:error:")]
		bool Start (double time, [NullAllowed] out NSError outError);

		/// <summary>Stops playback at the specified time.</summary>
		/// <param name="time">The time, in seconds.</param>
		/// <param name="outError">On return, the error if the operation failed.</param>
		[Abstract]
		[Export ("stopAtTime:error:")]
		bool Stop (double time, [NullAllowed] out NSError outError);

		/// <summary>Sends dynamic parameters at the specified time.</summary>
		/// <param name="parameters">The parameters.</param>
		/// <param name="time">The time, in seconds.</param>
		/// <param name="outError">On return, the error if the operation failed.</param>
		[Abstract]
		[Export ("sendParameters:atTime:error:")]
		bool Send (CHHapticDynamicParameter [] parameters, double time, [NullAllowed] out NSError outError);

		/// <summary>Schedules a parameter curve at the specified time.</summary>
		/// <param name="parameterCurve">The parameter curve.</param>
		/// <param name="time">The time, in seconds.</param>
		/// <param name="outError">On return, the error if the operation failed.</param>
		[Abstract]
		[Export ("scheduleParameterCurve:atTime:error:")]
		bool Schedule (CHHapticParameterCurve parameterCurve, double time, [NullAllowed] out NSError outError);

		[Abstract]
		[Export ("cancelAndReturnError:")]
		bool Cancel ([NullAllowed] out NSError outError);

		[Abstract]
		[Export ("isMuted")]
		bool IsMuted { get; set; }
	}

	interface ICHHapticAdvancedPatternPlayer { }

	[TV (14, 0)]
	[MacCatalyst (13, 1)]
	[Protocol]
	interface CHHapticAdvancedPatternPlayer : CHHapticPatternPlayer {
		/// <summary>Pauses playback at the specified time.</summary>
		/// <param name="time">The time, in seconds.</param>
		/// <param name="outError">On return, the error if the operation failed.</param>
		[Abstract]
		[Export ("pauseAtTime:error:")]
		bool Pause (double time, [NullAllowed] out NSError outError);

		/// <summary>Resumes playback at the specified time.</summary>
		/// <param name="time">The time, in seconds.</param>
		/// <param name="outError">On return, the error if the operation failed.</param>
		[Abstract]
		[Export ("resumeAtTime:error:")]
		bool Resume (double time, [NullAllowed] out NSError outError);

		/// <summary>Seeks to the specified offset.</summary>
		/// <param name="offsetTime">The offset time, in seconds.</param>
		/// <param name="outError">On return, the error if the operation failed.</param>
		[Abstract]
		[Export ("seekToOffset:error:")]
		bool Seek (double offsetTime, [NullAllowed] out NSError outError);

		[Abstract]
		[Export ("loopEnabled")]
		bool LoopEnabled { get; set; }

		/// <summary>Gets or sets the loop end.</summary>
		/// <value>The loop end, in seconds.</value>
		[Abstract]
		[Export ("loopEnd")]
		double LoopEnd { get; set; }

		[Abstract]
		[Export ("playbackRate")]
		float PlaybackRate { get; set; }

		[Abstract]
		[Export ("completionHandler", ArgumentSemantic.Assign)]
		Action<NSError> CompletionHandler { get; set; }

		[Abstract]
		[Export ("isMuted")]
		new bool IsMuted { get; set; }
	}

	[TV (14, 0)]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface CHHapticEngine {

		[Static]
		[Export ("capabilitiesForHardware")]
		ICHHapticDeviceCapability GetHardwareCapabilities ();

		/// <summary>Gets the current time.</summary>
		/// <value>The current time, in seconds.</value>
		[Export ("currentTime")]
		double CurrentTime { get; }

		[Export ("stoppedHandler", ArgumentSemantic.Assign)]
		Action<CHHapticEngineStoppedReason> StoppedHandler { get; set; }

		[Export ("resetHandler", ArgumentSemantic.Assign)]
		Action ResetHandler { get; set; }

		[Export ("playsHapticsOnly")]
		bool PlaysHapticsOnly { get; set; }

		[Export ("isMutedForAudio")]
		bool IsMutedForAudio { get; set; }

		[Export ("isMutedForHaptics")]
		bool IsMutedForHaptics { get; set; }

		[Export ("autoShutdownEnabled")]
		bool AutoShutdownEnabled { [Bind ("isAutoShutdownEnabled")] get; set; }

		[Export ("initAndReturnError:")]
		[DesignatedInitializer]
		NativeHandle Constructor ([NullAllowed] out NSError error);

		[NoMac] // AVAudioSession is not available on macOS.
		[MacCatalyst (13, 1)]
		[Export ("initWithAudioSession:error:")]
		[DesignatedInitializer]
		NativeHandle Constructor ([NullAllowed] AVAudioSession audioSession, [NullAllowed] out NSError error);

		[Async]
		[Export ("startWithCompletionHandler:")]
		void Start ([NullAllowed] Action<NSError> completionHandler);

		[Export ("startAndReturnError:")]
		bool Start ([NullAllowed] out NSError outError);

		[Async]
		[Export ("stopWithCompletionHandler:")]
		void Stop ([NullAllowed] Action<NSError> completionHandler);

		[Async]
		[Export ("notifyWhenPlayersFinished:")]
		void NotifyWhenPlayersFinished (Action<NSError> finishedHandler);

		[Export ("createPlayerWithPattern:error:")]
		[return: NullAllowed]
		ICHHapticPatternPlayer CreatePlayer (CHHapticPattern pattern, [NullAllowed] out NSError outError);

		[Export ("createAdvancedPlayerWithPattern:error:")]
		[return: NullAllowed]
		ICHHapticAdvancedPatternPlayer CreateAdvancedPlayer (CHHapticPattern pattern, [NullAllowed] out NSError outError);

		[Export ("registerAudioResource:options:error:")]
		nuint RegisterAudioResource (NSUrl resourceUrl, NSDictionary options, [NullAllowed] out NSError outError);

		[Export ("unregisterAudioResource:error:")]
		bool UnregisterAudioResource (nuint resourceId, [NullAllowed] out NSError outError);

		[Export ("playPatternFromURL:error:")]
		bool PlayPattern (NSUrl fileUrl, [NullAllowed] out NSError outError);

		[Export ("playPatternFromData:error:")]
		bool PlayPattern (NSData data, [NullAllowed] out NSError outError);

		[TV (16, 0), MacCatalyst (16, 0), iOS (16, 0)]
		[Export ("playsAudioOnly")]
		bool PlaysAudioOnly { get; set; }
	}

	[Static]
	[Internal]
	[TV (14, 0)]
	[MacCatalyst (13, 1)]
	partial interface CHHapticPatternDefinitionKeys {
		[Field ("CHHapticPatternKeyVersion")]
		NSString VersionKey { get; }

		[Field ("CHHapticPatternKeyPattern")]
		NSString PatternKey { get; }

		[Field ("CHHapticPatternKeyEvent")]
		NSString EventKey { get; }

		[Field ("CHHapticPatternKeyEventType")]
		NSString EventTypeKey { get; }

		[Field ("CHHapticPatternKeyTime")]
		NSString TimeKey { get; }

		[Field ("CHHapticPatternKeyEventDuration")]
		NSString EventDurationKey { get; }

		[Field ("CHHapticPatternKeyEventWaveformPath")]
		NSString EventWaveformPathKey { get; }

		[Field ("CHHapticPatternKeyEventParameters")]
		NSString EventParametersKey { get; }

		[Field ("CHHapticPatternKeyParameter")]
		NSString ParameterKey { get; }

		[Field ("CHHapticPatternKeyParameterID")]
		NSString ParameterIdKey { get; }

		[Field ("CHHapticPatternKeyParameterValue")]
		NSString ParameterValueKey { get; }

		[Field ("CHHapticPatternKeyParameterCurve")]
		NSString ParameterCurveKey { get; }

		[Field ("CHHapticPatternKeyParameterCurveControlPoints")]
		NSString ParameterCurveControlPointsKey { get; }

		[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
		[Field ("CHHapticPatternKeyEventWaveformUseVolumeEnvelope")]
		NSString EventWaveformUseVolumeEnvelopeKey { get; }

		[TV (16, 0), MacCatalyst (16, 0), iOS (16, 0)]
		[Field ("CHHapticPatternKeyEventWaveformLoopEnabled")]
		NSString EventWaveformLoopEnabledKey { get; }
	}

	[TV (14, 0)]
	[MacCatalyst (13, 1)]
	[StrongDictionary ("CHHapticPatternDefinitionKeys")]
	partial interface CHHapticPatternDefinition {
		double Version { get; set; }
		NSArray Pattern { get; set; }
		NSDictionary Event { get; set; }
		CHHapticEventType EventType { get; set; }
		double Time { get; set; }
		double EventDuration { get; set; }
		NSString EventWaveformPath { get; set; }
		NSArray EventParameters { get; set; }
		NSDictionary Parameter { get; set; }
		// we need to do a NSString because it can be a CHHapticEventParameterID or a CHHapticDynamicParameterID 
		// which are two different enums. User will have to do the conversion
		[Advice ("Value can be either a CHHapticEventParameterId or a CHHapticDynamicParameterId. A check is needed against both enums.")]
		NSString ParameterId { get; set; }
		double ParameterValue { get; set; }
		// we do not have docs or header information about the exact type
		[Export ("ParameterCurveKey")]
		NSObject WeakParameterCurve { get; set; }
		[Export ("ParameterCurveControlPointsKey")]
		NSObject WeakParameterCurveControlPoints { get; set; }
		[Advice ("The default value is true.")]
		[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
		bool EventWaveformUseVolumeEnvelope { get; set; }
	}

	[TV (14, 0)]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface CHHapticPattern {
		/// <summary>Gets the duration.</summary>
		/// <value>The duration, in seconds.</value>
		[Export ("duration")]
		double Duration { get; }

		[Export ("initWithEvents:parameters:error:")]
		NativeHandle Constructor (CHHapticEvent [] events, CHHapticDynamicParameter [] parameters, [NullAllowed] out NSError outError);

		[Export ("initWithEvents:parameterCurves:error:")]
		NativeHandle Constructor (CHHapticEvent [] events, CHHapticParameterCurve [] parameterCurves, [NullAllowed] out NSError outError);

		[Export ("initWithDictionary:error:")]
		NativeHandle Constructor (NSDictionary patternDict, [NullAllowed] out NSError outError);

		[Wrap ("this (patternDefinition.GetDictionary ()!, out outError)")]
		NativeHandle Constructor (CHHapticPatternDefinition patternDefinition, [NullAllowed] out NSError outError);

		[TV (16, 0), MacCatalyst (16, 0), iOS (16, 0)]
		[Export ("initWithContentsOfURL:error:")]
		NativeHandle Constructor (NSUrl url, [NullAllowed] out NSError error);

		[Internal]
		[Export ("exportDictionaryAndReturnError:")]
		[return: NullAllowed]
		NSDictionary<NSString, NSObject> _ExportDictionary ([NullAllowed] out NSError outError);

		[Wrap ("new CHHapticPatternDefinition (_ExportDictionary (out outError)!)")]
		[return: NullAllowed]
		CHHapticPatternDefinition Export ([NullAllowed] out NSError outError);
	}

	[Static]
	[Internal]
	[iOS (15, 0), TV (15, 0), MacCatalyst (15, 0)]
	partial interface CHHapticAudioResourceKeys {
		[Field ("CHHapticAudioResourceKeyUseVolumeEnvelope")]
		NSString UseVolumeEnvelopeKey { get; }

		[TV (16, 0), MacCatalyst (16, 0), iOS (16, 0)]
		[Field ("CHHapticAudioResourceKeyLoopEnabled")]
		NSString LoopEnabledKey { get; }

	}

	[iOS (15, 0), TV (15, 0), MacCatalyst (15, 0)]
	[StrongDictionary ("CHHapticAudioResourceKeys")]
	partial interface CHHapticAudioResourceDefinition {
		[Advice ("The default value is true.")]
		bool UseVolumeEnvelope { get; set; }

		[Advice ("The default value is false.")]
		bool LoopEnabled { get; set; }
	}

}
