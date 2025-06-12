//
// AUEnums.cs: AudioUnit enumerations
//
// Authors:
//   AKIHIRO Uehara (u-akihiro@reinforce-lab.com)
//   Marek Safar (marek.safar@gmail.com)
//
// Copyright 2010 Reinforce Lab.
// Copyright 2011-2013 Xamarin Inc
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Runtime.InteropServices;
using System.Threading;
using AudioToolbox;
using ObjCRuntime;
using CoreFoundation;
using Foundation;

#nullable enable

namespace AudioUnit {
	/// <summary>An enumeration whose values specify the status of an <see cref="AudioUnit.AudioUnit" />.</summary>
	public enum AudioUnitStatus { // Implictly cast to OSType
		/// <summary>To be added.</summary>
		NoError = 0,
		/// <summary>To be added.</summary>
		OK = NoError,
		/// <summary>To be added.</summary>
		FileNotFound = -43,
		/// <summary>To be added.</summary>
		ParameterError = -50,
		/// <summary>To be added.</summary>
		InvalidProperty = -10879,
		/// <summary>To be added.</summary>
		InvalidParameter = -10878,
		/// <summary>To be added.</summary>
		InvalidElement = -10877,
		/// <summary>To be added.</summary>
		NoConnection = -10876,
		/// <summary>To be added.</summary>
		FailedInitialization = -10875,
		/// <summary>To be added.</summary>
		TooManyFramesToProcess = -10874,
		/// <summary>To be added.</summary>
		InvalidFile = -10871,
		/// <summary>To be added.</summary>
		FormatNotSupported = -10868,
		/// <summary>To be added.</summary>
		Uninitialized = -10867,
		/// <summary>To be added.</summary>
		InvalidScope = -10866,
		/// <summary>To be added.</summary>
		PropertyNotWritable = -10865,
		/// <summary>To be added.</summary>
		CannotDoInCurrentContext = -10863,
		/// <summary>To be added.</summary>
		InvalidPropertyValue = -10851,
		/// <summary>To be added.</summary>
		PropertyNotInUse = -10850,
		/// <summary>To be added.</summary>
		Initialized = -10849,
		/// <summary>To be added.</summary>
		InvalidOfflineRender = -10848,
		/// <summary>To be added.</summary>
		Unauthorized = -10847,
		/// <summary>To be added.</summary>
		MidiOutputBufferFull = -66753,
		RenderTimeout = -66745,
		/// <summary>To be added.</summary>
		InvalidParameterValue = -66743,
		/// <summary>To be added.</summary>
		ExtensionNotFound = -66744,
		InvalidFilePath = -66742,
		MissingKey = -66741,
		ComponentManagerNotSupported = -66740,
		MultipleVoiceProcessors = -66635,
	}

	/// <summary>Enumerates status values returned by <see cref="AudioUnit.AudioUnit.AudioOutputUnitPublish(AudioUnit.AudioComponentDescription,System.String,System.UInt32)" />.</summary>
	public enum AudioComponentStatus { // Implictly cast to OSType
		/// <summary>To be added.</summary>
		OK = 0,
		/// <summary>To be added.</summary>
		DuplicateDescription = -66752,
		/// <summary>To be added.</summary>
		UnsupportedType = -66751,
		/// <summary>To be added.</summary>
		TooManyInstances = -66750,
		InstanceTimedOut = -66754,
		/// <summary>To be added.</summary>
		InstanceInvalidated = -66749,
		/// <summary>To be added.</summary>
		NotPermitted = -66748,
		/// <summary>To be added.</summary>
		InitializationTimedOut = -66747,
		/// <summary>To be added.</summary>
		InvalidFormat = -66746,
		/// <summary>To be added.</summary>
		[MacCatalyst (13, 1)]
		RenderTimeout = -66745,
	}

	/// <summary>An enumeration whose values specify whether to use a hardware or software encoder.</summary>
	public enum AudioCodecManufacturer : uint  // Implictly cast to OSType in CoreAudio.framework - CoreAudioTypes.h
	{
		/// <summary>To be added.</summary>
		AppleSoftware = 0x6170706c, // 'appl'
		/// <summary>To be added.</summary>
		AppleHardware = 0x61706877, // 'aphw'
	}

	/// <summary>Enumerates instrument types.</summary>
	public enum InstrumentType : byte // UInt8 in AUSamplerInstrumentData
	{
		/// <summary>To be added.</summary>
		DLSPreset = 1,
		/// <summary>To be added.</summary>
		SF2Preset = DLSPreset,
		/// <summary>To be added.</summary>
		AUPreset = 2,
		/// <summary>To be added.</summary>
		Audiofile = 3,
		/// <summary>To be added.</summary>
		EXS24 = 4,
	}

	/// <summary>The unit of measure used by an audio unit parameter.</summary>
	public enum AudioUnitParameterUnit // UInt32 AudioUnitParameterUnit
	{
		/// <summary>To be added.</summary>
		Generic = 0,
		/// <summary>To be added.</summary>
		Indexed = 1,
		/// <summary>To be added.</summary>
		Boolean = 2,
		/// <summary>To be added.</summary>
		Percent = 3,
		/// <summary>To be added.</summary>
		Seconds = 4,
		/// <summary>To be added.</summary>
		SampleFrames = 5,
		/// <summary>To be added.</summary>
		Phase = 6,
		/// <summary>To be added.</summary>
		Rate = 7,
		/// <summary>To be added.</summary>
		Hertz = 8,
		/// <summary>To be added.</summary>
		Cents = 9,
		/// <summary>To be added.</summary>
		RelativeSemiTones = 10,
		/// <summary>To be added.</summary>
		MIDINoteNumber = 11,
		/// <summary>To be added.</summary>
		MIDIController = 12,
		/// <summary>To be added.</summary>
		Decibels = 13,
		/// <summary>To be added.</summary>
		LinearGain = 14,
		/// <summary>To be added.</summary>
		Degrees = 15,
		/// <summary>To be added.</summary>
		EqualPowerCrossfade = 16,
		/// <summary>To be added.</summary>
		MixerFaderCurve1 = 17,
		/// <summary>To be added.</summary>
		Pan = 18,
		/// <summary>To be added.</summary>
		Meters = 19,
		/// <summary>To be added.</summary>
		AbsoluteCents = 20,
		/// <summary>To be added.</summary>
		Octaves = 21,
		/// <summary>To be added.</summary>
		BPM = 22,
		/// <summary>To be added.</summary>
		Beats = 23,
		/// <summary>To be added.</summary>
		Milliseconds = 24,
		/// <summary>To be added.</summary>
		Ratio = 25,
		/// <summary>To be added.</summary>
		CustomUnit = 26,
		[iOS (15, 0), TV (15, 0), MacCatalyst (15, 0)]
		MIDI2Controller = 27,
	}

	/// <summary>Flagging enumeration used with <see cref="AudioUnit.AudioUnitParameterInfo.Flags" />.</summary>
	[Flags]
	public enum AudioUnitParameterFlag : uint // UInt32 in AudioUnitParameterInfo
	{
		/// <summary>To be added.</summary>
		CFNameRelease = (1 << 4),

		/// <summary>To be added.</summary>
		[MacCatalyst (13, 1)]
		OmitFromPresets = (1 << 13),
		/// <summary>To be added.</summary>
		PlotHistory = (1 << 14),
		/// <summary>To be added.</summary>
		MeterReadOnly = (1 << 15),

		// bit positions 18,17,16 are set aside for display scales. bit 19 is reserved.
		/// <summary>To be added.</summary>
		DisplayMask = (7 << 16) | (1 << 22),
		/// <summary>To be added.</summary>
		DisplaySquareRoot = (1 << 16),
		/// <summary>To be added.</summary>
		DisplaySquared = (2 << 16),
		/// <summary>To be added.</summary>
		DisplayCubed = (3 << 16),
		/// <summary>To be added.</summary>
		DisplayCubeRoot = (4 << 16),
		/// <summary>To be added.</summary>
		DisplayExponential = (5 << 16),

		/// <summary>To be added.</summary>
		HasClump = (1 << 20),
		/// <summary>To be added.</summary>
		ValuesHaveStrings = (1 << 21),

		/// <summary>To be added.</summary>
		DisplayLogarithmic = (1 << 22),

		/// <summary>To be added.</summary>
		IsHighResolution = (1 << 23),
		/// <summary>To be added.</summary>
		NonRealTime = (1 << 24),
		/// <summary>To be added.</summary>
		CanRamp = (1 << 25),
		/// <summary>To be added.</summary>
		ExpertMode = (1 << 26),
		/// <summary>To be added.</summary>
		HasCFNameString = (1 << 27),
		/// <summary>To be added.</summary>
		IsGlobalMeta = (1 << 28),
		/// <summary>To be added.</summary>
		IsElementMeta = (1 << 29),
		/// <summary>To be added.</summary>
		IsReadable = (1 << 30),
		/// <summary>To be added.</summary>
		IsWritable = ((uint) 1 << 31),
	}

	/// <summary>Enumerates values used by <see cref="AudioUnit.AudioUnitParameterInfo" />. Currenty reserved for system use.</summary>
	public enum AudioUnitClumpID // UInt32 in AudioUnitParameterInfo
	{
		/// <summary>To be added.</summary>
		System = 0,
	}

	[MacCatalyst (13, 1)]
	[NoTV]
	[NoiOS]
	public enum AudioObjectPropertySelector : uint {
		/// <summary>To be added.</summary>
		PropertyDevices = 1684370979, // 'dev#'
		/// <summary>To be added.</summary>
		Devices = 1684370979, // 'dev#'
		/// <summary>To be added.</summary>
		DefaultInputDevice = 1682533920, // 'dIn '
		/// <summary>To be added.</summary>
		DefaultOutputDevice = 1682929012, // 'dOut'
		/// <summary>To be added.</summary>
		DefaultSystemOutputDevice = 1934587252, // 'sOut'
		/// <summary>To be added.</summary>
		TranslateUIDToDevice = 1969841252, // 'uidd'
		/// <summary>To be added.</summary>
		MixStereoToMono = 1937010031, // 'stmo'
		/// <summary>To be added.</summary>
		PlugInList = 1886152483, // 'plg#'
		/// <summary>To be added.</summary>
		TranslateBundleIDToPlugIn = 1651074160, // 'bidp'
		/// <summary>To be added.</summary>
		TransportManagerList = 1953326883, // 'tmg#'
		/// <summary>To be added.</summary>
		TranslateBundleIDToTransportManager = 1953325673, // 'tmbi'
		/// <summary>To be added.</summary>
		BoxList = 1651472419, // 'box#'
		/// <summary>To be added.</summary>
		TranslateUIDToBox = 1969841250, // 'uidb'
		ClockDeviceList = 1668049699, //'clk#'
		TranslateUidToClockDevice = 1969841251, // 'uidc',
#if !XAMCORE_5_0
		/// <summary>To be added.</summary>
		[MacCatalyst (13, 1)] // This is required for .NET, because otherwise the generator thinks it's not available because it's not available on iOS.
		[Deprecated (PlatformName.iOS, 15, 0, message: "Use the 'ProcessIsMain' element instead.")]
		[Deprecated (PlatformName.TvOS, 15, 0, message: "Use the 'ProcessIsMain' element instead.")]
		[Deprecated (PlatformName.MacCatalyst, 15, 0, message: "Use the 'ProcessIsMain' element instead.")]
		[Deprecated (PlatformName.MacOSX, 12, 0, message: "Use the 'ProcessIsMain' element instead.")]
		[Obsolete ("Use the 'ProcessIsMain' element instead.")]
		ProcessIsMaster = 1835103092, // 'mast'
#endif // !XAMCORE_5_0
		[NoiOS]
		[MacCatalyst (15, 0), NoTV]
		ProcessIsMain = 1835100526, // 'main'
		/// <summary>To be added.</summary>
		IsInitingOrExiting = 1768845172, // 'inot'
		/// <summary>To be added.</summary>
		UserIDChanged = 1702193508, // 'euid'
		/// <summary>To be added.</summary>
		ProcessIsAudible = 1886221684, // 'pmut'
		/// <summary>To be added.</summary>
		SleepingIsAllowed = 1936483696, // 'slep'
		/// <summary>To be added.</summary>
		UnloadingIsAllowed = 1970170980, // 'unld'
		/// <summary>To be added.</summary>
		HogModeIsAllowed = 1752131442, // 'hogr'
		/// <summary>To be added.</summary>
		UserSessionIsActiveOrHeadless = 1970496882, // 'user'
		/// <summary>To be added.</summary>
		ServiceRestarted = 1936880500, // 'srst'
		/// <summary>To be added.</summary>
		PowerHint = 1886353256, // 'powh'
		ActualSampleRate = 1634955892,// 'asrt',
		ClockDevice = 1634755428, // 'apcd',
		IOThreadOSWorkgroup = 1869838183, // 'oswg'
		[NoiOS]
		[MacCatalyst (15, 0), NoTV]
		ProcessMute = 1634758765, // 'appm'
		[MacCatalyst (17, 0), Mac (14, 0), NoTV]
		InputMute = 1852403056, //pmin
	}

	[MacCatalyst (13, 1)]
	[NoTV]
	[NoiOS]
	public enum AudioObjectPropertyScope : uint {
		/// <summary>To be added.</summary>
		Global = 1735159650, // 'glob'
		/// <summary>To be added.</summary>
		Input = 1768845428, // 'inpt'
		/// <summary>To be added.</summary>
		Output = 1869968496, // 'outp'
		/// <summary>To be added.</summary>
		PlayThrough = 1886679669, // 'ptru'
	}

	[MacCatalyst (13, 1)]
	[NoTV]
	[NoiOS]
	public enum AudioObjectPropertyElement : uint {
		/// <summary>To be added.</summary>
		Main = 0, // 0
	}

	/// <summary>An enumeration whose values specify a kind of <see cref="AudioUnit.AudioUnit" />.</summary>
	[Internal]
	enum AudioUnitPropertyIDType { // UInt32 AudioUnitPropertyID
								   // Audio Unit Properties
		/// <summary>To be added.</summary>
		ClassInfo = 0,
		/// <summary>To be added.</summary>
		MakeConnection = 1,
		/// <summary>To be added.</summary>
		SampleRate = 2,
		/// <summary>To be added.</summary>
		ParameterList = 3,
		/// <summary>To be added.</summary>
		ParameterInfo = 4,
		/// <summary>To be added.</summary>
		CPULoad = 6,
		/// <summary>To be added.</summary>
		StreamFormat = 8,
		/// <summary>To be added.</summary>
		ElementCount = 11,
		/// <summary>To be added.</summary>
		Latency = 12,
		/// <summary>To be added.</summary>
		SupportedNumChannels = 13,
		/// <summary>To be added.</summary>
		MaximumFramesPerSlice = 14,
		/// <summary>To be added.</summary>
		ParameterValueStrings = 16,
		/// <summary>To be added.</summary>
		AudioChannelLayout = 19,
		/// <summary>To be added.</summary>
		TailTime = 20,
		/// <summary>To be added.</summary>
		BypassEffect = 21,
		/// <summary>To be added.</summary>
		LastRenderError = 22,
		/// <summary>To be added.</summary>
		SetRenderCallback = 23,
		/// <summary>To be added.</summary>
		FactoryPresets = 24,
		/// <summary>To be added.</summary>
		RenderQuality = 26,
		/// <summary>To be added.</summary>
		HostCallbacks = 27,
		/// <summary>To be added.</summary>
		InPlaceProcessing = 29,
		/// <summary>To be added.</summary>
		ElementName = 30,
		/// <summary>To be added.</summary>
		SupportedChannelLayoutTags = 32,
		/// <summary>To be added.</summary>
		PresentPreset = 36,
		/// <summary>To be added.</summary>
		DependentParameters = 45,
		/// <summary>To be added.</summary>
		InputSampleInOutput = 49,
		/// <summary>To be added.</summary>
		ShouldAllocateBuffer = 51,
		/// <summary>To be added.</summary>
		FrequencyResponse = 52,
		/// <summary>To be added.</summary>
		ParameterHistoryInfo = 53,
		/// <summary>To be added.</summary>
		Nickname = 54,
		/// <summary>To be added.</summary>
		OfflineRender = 37,
		/// <summary>To be added.</summary>
		[MacCatalyst (13, 1)]
		ParameterIDName = 34,
		/// <summary>To be added.</summary>
		[MacCatalyst (13, 1)]
		ParameterStringFromValue = 33,
		/// <summary>To be added.</summary>
		ParameterClumpName = 35,
		/// <summary>To be added.</summary>
		[MacCatalyst (13, 1)]
		ParameterValueFromString = 38,
		/// <summary>To be added.</summary>
		ContextName = 25,
		/// <summary>To be added.</summary>
		PresentationLatency = 40,
		/// <summary>To be added.</summary>
		ClassInfoFromDocument = 50,
		/// <summary>To be added.</summary>
		RequestViewController = 56,
		/// <summary>To be added.</summary>
		ParametersForOverview = 57,
		/// <summary>To be added.</summary>
		[MacCatalyst (13, 1)]
		SupportsMpe = 58,
		[iOS (15, 0), TV (15, 0), MacCatalyst (15, 0)]
		LastRenderSampleTime = 61,
		[iOS (14, 5), TV (14, 5)]
		[MacCatalyst (14, 5)]
		LoadedOutOfProcess = 62,
		[iOS (15, 0), TV (15, 0), MacCatalyst (15, 0)]
		MIDIOutputEventListCallback = 63,
		[iOS (15, 0), TV (15, 0), MacCatalyst (15, 0)]
		AudioUnitMIDIProtocol = 64,
		[iOS (15, 0), TV (15, 0), MacCatalyst (15, 0)]
		HostMIDIProtocol = 65,

#if MONOMAC
		/// <summary>To be added.</summary>
		FastDispatch = 5,
		/// <summary>To be added.</summary>
		SetExternalBuffer = 15,
		/// <summary>To be added.</summary>
		GetUIComponentList = 18,
		/// <summary>To be added.</summary>
		CocoaUI = 31,
		/// <summary>To be added.</summary>
		IconLocation = 39,
		/// <summary>To be added.</summary>
		AUHostIdentifier = 46,
		/// <summary>To be added.</summary>
		MIDIOutputCallbackInfo = 47,
		/// <summary>To be added.</summary>
		MIDIOutputCallback = 48,
#else
		/// <summary>To be added.</summary>
		RemoteControlEventListener = 100,
		/// <summary>To be added.</summary>
		IsInterAppConnected = 101,
		/// <summary>To be added.</summary>
		PeerURL = 102,
#endif // MONOMAC

		// Output Unit
		/// <summary>To be added.</summary>
		IsRunning = 2001,

		// OS X Availability
#if MONOMAC

		// Music Effects and Instruments
		/// <summary>To be added.</summary>
		AllParameterMIDIMappings = 41,
		/// <summary>To be added.</summary>
		AddParameterMIDIMapping = 42,
		/// <summary>To be added.</summary>
		RemoveParameterMIDIMapping = 43,
		/// <summary>To be added.</summary>
		HotMapParameterMIDIMapping = 44,

		// Music Device
		/// <summary>To be added.</summary>
		MIDIXMLNames = 1006,
		/// <summary>To be added.</summary>
		PartGroup = 1010,
		/// <summary>To be added.</summary>
		DualSchedulingMode = 1013,
		/// <summary>To be added.</summary>
		SupportsStartStopNote = 1014,

		// Offline Unit
		/// <summary>To be added.</summary>
		InputSize = 3020,
		/// <summary>To be added.</summary>
		OutputSize = 3021,
		/// <summary>To be added.</summary>
		StartOffset = 3022,
		/// <summary>To be added.</summary>
		PreflightRequirements = 3023,
		/// <summary>To be added.</summary>
		PreflightName = 3024,

		// Translation Service
		/// <summary>To be added.</summary>
		FromPlugin = 4000,
		/// <summary>To be added.</summary>
		OldAutomation = 4001,

#endif // MONOMAC

		// Apple Specific Properties
		// AUConverter
		/// <summary>To be added.</summary>
		SampleRateConverterComplexity = 3014,

		// AUHAL and device units
		/// <summary>To be added.</summary>
		CurrentDevice = 2000,
		/// <summary>To be added.</summary>
		ChannelMap = 2002, // this will also work with AUConverter
		/// <summary>To be added.</summary>
		EnableIO = 2003,
		/// <summary>To be added.</summary>
		StartTime = 2004,
		/// <summary>To be added.</summary>
		SetInputCallback = 2005,
		/// <summary>To be added.</summary>
		HasIO = 2006,
		/// <summary>To be added.</summary>
		StartTimestampsAtZero = 2007, // this will also work with AUConverter

#if !MONOMAC
		/// <summary>To be added.</summary>
		MIDICallbacks = 2010,
		/// <summary>To be added.</summary>
		HostReceivesRemoteControlEvents = 2011,
		/// <summary>To be added.</summary>
		RemoteControlToHost = 2012,
		/// <summary>To be added.</summary>
		HostTransportState = 2013,
		/// <summary>To be added.</summary>
		NodeComponentDescription = 2014,
#endif // !MONOMAC

		// AUVoiceProcessing unit
		/// <summary>To be added.</summary>
		BypassVoiceProcessing = 2100,
		/// <summary>To be added.</summary>
		VoiceProcessingEnableAGC = 2101,
		/// <summary>To be added.</summary>
		MuteOutput = 2104,
		[iOS (15, 0), MacCatalyst (15, 0), NoMac, NoTV]
		MutedSpeechActivityEventListener = 2106,

		// AUNBandEQ unit
		/// <summary>To be added.</summary>
		NumberOfBands = 2200,
		/// <summary>To be added.</summary>
		MaxNumberOfBands = 2201,
		/// <summary>To be added.</summary>
		BiquadCoefficients = 2203,

		// Mixers
		// General mixers
		/// <summary>To be added.</summary>
		MeteringMode = 3007,

		// Matrix Mixer
		/// <summary>To be added.</summary>
		MatrixLevels = 3006,
		/// <summary>To be added.</summary>
		MatrixDimensions = 3009,
		/// <summary>To be added.</summary>
		MeterClipping = 3011,
		/// <summary>To be added.</summary>
		[MacCatalyst (13, 1)]
		InputAnchorTimeStamp = 3016,

		// SpatialMixer
		/// <summary>To be added.</summary>
		ReverbRoomType = 10,
		/// <summary>To be added.</summary>
		UsesInternalReverb = 1005,
		/// <summary>To be added.</summary>
		SpatializationAlgorithm = 3000,
		SpatialMixerRenderingFlags = 3003,
		SpatialMixerSourceMode = 3005,
		SpatialMixerDistanceParams = 3010,
#if !XAMCORE_5_0
		/// <summary>To be added.</summary>
		[Obsolete ("Use 'SpatialMixerDistanceParams' instead.")]
		[EditorBrowsable (EditorBrowsableState.Never)]
		DistanceParams = SpatialMixerDistanceParams,
		/// <summary>Developers should not use this deprecated field. </summary>
		[Obsolete ("Use 'SpatialMixerAttenuationCurve' instead.")]
		[EditorBrowsable (EditorBrowsableState.Never)]
		AttenuationCurve = SpatialMixerAttenuationCurve,
		/// <summary>To be added.</summary>
		[Obsolete ("Use 'SpatialMixerRenderingFlags' instead.")]
		[EditorBrowsable (EditorBrowsableState.Never)]
		RenderingFlags = SpatialMixerRenderingFlags,
#endif
		SpatialMixerAttenuationCurve = 3013,
		SpatialMixerOutputType = 3100,
		SpatialMixerPointSourceInHeadMode = 3103,
		[Mac (12, 3), iOS (18, 0), TV (18, 0), MacCatalyst (18, 0)]
		SpatialMixerEnableHeadTracking = 3111,
		[Mac (13, 0), iOS (18, 0), TV (18, 0), MacCatalyst (18, 0)]
		SpatialMixerPersonalizedHrtfMode = 3113,
		[Mac (14, 0), iOS (18, 0), TV (18, 0), MacCatalyst (18, 0)]
		SpatialMixerAnyInputIsUsingPersonalizedHrtf = 3116,

		// AUScheduledSoundPlayer
		/// <summary>To be added.</summary>
		ScheduleAudioSlice = 3300,
		/// <summary>To be added.</summary>
		ScheduleStartTimeStamp = 3301,
		/// <summary>To be added.</summary>
		CurrentPlayTime = 3302,

		// AUAudioFilePlayer
		/// <summary>To be added.</summary>
		ScheduledFileIDs = 3310,
		/// <summary>To be added.</summary>
		ScheduledFileRegion = 3311,
		/// <summary>To be added.</summary>
		ScheduledFilePrime = 3312,
		/// <summary>To be added.</summary>
		ScheduledFileBufferSizeFrames = 3313,
		/// <summary>To be added.</summary>
		ScheduledFileNumberBuffers = 3314,

#if MONOMAC
		// OS X-specific Music Device Properties
		/// <summary>To be added.</summary>
		SoundBankData = 1008,
		/// <summary>To be added.</summary>
		StreamFromDisk = 1011,
		/// <summary>To be added.</summary>
		SoundBankFSRef = 1012,

#endif // !MONOMAC

		// Music Device Properties
		/// <summary>To be added.</summary>
		InstrumentName = 1001,
		/// <summary>To be added.</summary>
		InstrumentNumber = 1004,

		// Music Device Properties used by DLSMusicDevice and AUMIDISynth
		/// <summary>To be added.</summary>
		InstrumentCount = 1000,
		/// <summary>To be added.</summary>
		BankName = 1007,
		/// <summary>To be added.</summary>
		SoundBankURL = 1100,

		// AUMIDISynth
		/// <summary>To be added.</summary>
		MidiSynthEnablePreload = 4119,

		// AUSampler
		/// <summary>To be added.</summary>
		LoadInstrument = 4102,
		/// <summary>To be added.</summary>
		LoadAudioFiles = 4101,

		// AUDeferredRenderer
		/// <summary>To be added.</summary>
		DeferredRendererPullSize = 3320,
		/// <summary>To be added.</summary>
		DeferredRendererExtraLatency = 3321,
		/// <summary>To be added.</summary>
		DeferredRendererWaitFrames = 3322,

#if MONOMAC
		// AUNetReceive
		/// <summary>To be added.</summary>
		Hostname = 3511,
		/// <summary>To be added.</summary>
		NetReceivePassword = 3512,

		// AUNetSend
		/// <summary>To be added.</summary>
		PortNum = 3513,
		/// <summary>To be added.</summary>
		TransmissionFormat = 3514,
		/// <summary>To be added.</summary>
		TransmissionFormatIndex = 3515,
		/// <summary>To be added.</summary>
		ServiceName = 3516,
		/// <summary>To be added.</summary>
		Disconnect = 3517,
		/// <summary>To be added.</summary>
		NetSendPassword = 3518,
#endif // MONOMAC
	}

	/// <summary>An enumeration whose values represent adjustable attributes such as pitch or volume.</summary>
	public enum AudioUnitParameterType // UInt32 in AudioUnitParameterInfo
	{
		// AUMixer3D unit
		/// <summary>To be added.</summary>
		Mixer3DAzimuth = 0,
		/// <summary>To be added.</summary>
		Mixer3DElevation = 1,
		/// <summary>To be added.</summary>
		Mixer3DDistance = 2,
		/// <summary>To be added.</summary>
		Mixer3DGain = 3,
		/// <summary>To be added.</summary>
		Mixer3DPlaybackRate = 4,
#if MONOMAC
		Mixer3DReverbBlend = 5,
		Mixer3DGlobalReverbGain = 6,
		Mixer3DOcclusionAttenuation = 7,
		Mixer3DObstructionAttenuation = 8,
		Mixer3DMinGain = 9,
		Mixer3DMaxGain = 10,
		/// <summary>To be added.</summary>
		Mixer3DPreAveragePower = 1000,
		/// <summary>To be added.</summary>
		Mixer3DPrePeakHoldLevel = 2000,
		/// <summary>To be added.</summary>
		Mixer3DPostAveragePower = 3000,
		/// <summary>To be added.</summary>
		Mixer3DPostPeakHoldLevel = 4000,
#else
		/// <summary>To be added.</summary>
		Mixer3DEnable = 5,
		/// <summary>To be added.</summary>
		Mixer3DMinGain = 6,
		/// <summary>To be added.</summary>
		Mixer3DMaxGain = 7,
		/// <summary>To be added.</summary>
		Mixer3DReverbBlend = 8,
		/// <summary>To be added.</summary>
		Mixer3DGlobalReverbGain = 9,
		/// <summary>To be added.</summary>
		Mixer3DOcclusionAttenuation = 10,
		/// <summary>To be added.</summary>
		Mixer3DObstructionAttenuation = 11,
#endif

		// AUSpatialMixer unit
		/// <summary>To be added.</summary>
		SpatialAzimuth = 0,
		/// <summary>To be added.</summary>
		SpatialElevation = 1,
		/// <summary>To be added.</summary>
		SpatialDistance = 2,
		/// <summary>To be added.</summary>
		SpatialGain = 3,
		/// <summary>To be added.</summary>
		SpatialPlaybackRate = 4,
		/// <summary>To be added.</summary>
		SpatialEnable = 5,
		/// <summary>To be added.</summary>
		SpatialMinGain = 6,
		/// <summary>To be added.</summary>
		SpatialMaxGain = 7,
		/// <summary>To be added.</summary>
		SpatialReverbBlend = 8,
		/// <summary>To be added.</summary>
		SpatialGlobalReverbGain = 9,
		/// <summary>To be added.</summary>
		SpatialOcclusionAttenuation = 10,
		/// <summary>To be added.</summary>
		SpatialObstructionAttenuation = 11,

		// Reverb applicable to the 3DMixer or AUSpatialMixer
		/// <summary>To be added.</summary>
		ReverbFilterFrequency = 14,
		/// <summary>To be added.</summary>
		ReverbFilterBandwidth = 15,
		/// <summary>To be added.</summary>
		ReverbFilterGain = 16,
		/// <summary>To be added.</summary>
		[MacCatalyst (13, 1)]
		ReverbFilterType = 17,
		/// <summary>To be added.</summary>
		[MacCatalyst (13, 1)]
		ReverbFilterEnable = 18,

		// AUMultiChannelMixer
		/// <summary>To be added.</summary>
		MultiChannelMixerVolume = 0,
		/// <summary>To be added.</summary>
		MultiChannelMixerEnable = 1,
		/// <summary>To be added.</summary>
		MultiChannelMixerPan = 2,

		// AUMatrixMixer unit
		/// <summary>To be added.</summary>
		MatrixMixerVolume = 0,
		/// <summary>To be added.</summary>
		MatrixMixerEnable = 1,

		// AudioDeviceOutput, DefaultOutputUnit, and SystemOutputUnit units
		/// <summary>To be added.</summary>
		HALOutputVolume = 14,

		// AUTimePitch, AUTimePitch (offline), AUPitch units
		/// <summary>To be added.</summary>
		TimePitchRate = 0,
#if MONOMAC
		/// <summary>To be added.</summary>
		TimePitchPitch = 1,
		/// <summary>To be added.</summary>
		TimePitchEffectBlend = 2,
#endif

		// AUNewTimePitch
		/// <summary>To be added.</summary>
		NewTimePitchRate = 0,
		/// <summary>To be added.</summary>
		NewTimePitchPitch = 1,
		/// <summary>To be added.</summary>
		NewTimePitchOverlap = 4,
		/// <summary>To be added.</summary>
		NewTimePitchEnablePeakLocking = 6,

		// AUSampler unit
		/// <summary>To be added.</summary>
		AUSamplerGain = 900,
		/// <summary>To be added.</summary>
		AUSamplerCoarseTuning = 901,
		/// <summary>To be added.</summary>
		AUSamplerFineTuning = 902,
		/// <summary>To be added.</summary>
		AUSamplerPan = 903,

		// AUBandpass
		/// <summary>To be added.</summary>
		BandpassCenterFrequency = 0,
		/// <summary>To be added.</summary>
		BandpassBandwidth = 1,

		// AUHipass
		/// <summary>To be added.</summary>
		HipassCutoffFrequency = 0,
		/// <summary>To be added.</summary>
		HipassResonance = 1,

		// AULowpass
		/// <summary>To be added.</summary>
		LowPassCutoffFrequency = 0,
		/// <summary>To be added.</summary>
		LowPassResonance = 1,

		// AUHighShelfFilter
		/// <summary>To be added.</summary>
		HighShelfCutOffFrequency = 0,
		/// <summary>To be added.</summary>
		HighShelfGain = 1,

		// AULowShelfFilter
		/// <summary>To be added.</summary>
		AULowShelfCutoffFrequency = 0,
		/// <summary>To be added.</summary>
		AULowShelfGain = 1,

#if !XAMCORE_5_0 // I can't find this value in the headers anymore
		/// <summary>To be added.</summary>
		[Obsoleted (PlatformName.iOS, 7, 0)]
		[Obsoleted (PlatformName.MacCatalyst, 13, 1)]
		AUDCFilterDecayTime = 0,
#endif

		// AUParametricEQ
		/// <summary>To be added.</summary>
		ParametricEQCenterFreq = 0,
		/// <summary>To be added.</summary>
		ParametricEQQ = 1,
		/// <summary>To be added.</summary>
		ParametricEQGain = 2,

		// AUPeakLimiter
		/// <summary>To be added.</summary>
		LimiterAttackTime = 0,
		/// <summary>To be added.</summary>
		LimiterDecayTime = 1,
		/// <summary>To be added.</summary>
		LimiterPreGain = 2,

		// AUDynamicsProcessor
		/// <summary>To be added.</summary>
		DynamicsProcessorThreshold = 0,
		/// <summary>To be added.</summary>
		DynamicsProcessorHeadRoom = 1,
		/// <summary>To be added.</summary>
		DynamicsProcessorExpansionRatio = 2,
		/// <summary>To be added.</summary>
		DynamicsProcessorExpansionThreshold = 3,
		/// <summary>To be added.</summary>
		DynamicsProcessorAttackTime = 4,
		/// <summary>To be added.</summary>
		DynamicsProcessorReleaseTime = 5,
		/// <summary>To be added.</summary>
		[Deprecated (PlatformName.iOS, 15, 0, message: "Use 'DynamicsProcessorOverallGain' instead.")]
		[Deprecated (PlatformName.TvOS, 15, 0, message: "Use 'DynamicsProcessorOverallGain' instead.")]
		[Deprecated (PlatformName.MacOSX, 12, 0, message: "Use 'DynamicsProcessorOverallGain' instead.")]
		[Deprecated (PlatformName.MacCatalyst, 15, 0, message: "Use 'DynamicsProcessorOverallGain' instead.")]
		DynamicsProcessorMasterGain = 6,
		[iOS (15, 0), TV (15, 0), MacCatalyst (15, 0)]
		DynamicsProcessorOverallGain = 6,
		/// <summary>To be added.</summary>
		DynamicsProcessorCompressionAmount = 1000,
		/// <summary>To be added.</summary>
		DynamicsProcessorInputAmplitude = 2000,
		/// <summary>To be added.</summary>
		DynamicsProcessorOutputAmplitude = 3000,

		// AUVarispeed
		/// <summary>To be added.</summary>
		VarispeedPlaybackRate = 0,
		/// <summary>To be added.</summary>
		VarispeedPlaybackCents = 1,

		// Distortion unit 
		/// <summary>To be added.</summary>
		DistortionDelay = 0,
		/// <summary>To be added.</summary>
		DistortionDecay = 1,
		/// <summary>To be added.</summary>
		DistortionDelayMix = 2,
		/// <summary>To be added.</summary>
		DistortionDecimation = 3,
		/// <summary>To be added.</summary>
		DistortionRounding = 4,
		/// <summary>To be added.</summary>
		DistortionDecimationMix = 5,
		/// <summary>To be added.</summary>
		DistortionLinearTerm = 6,
		/// <summary>To be added.</summary>
		DistortionSquaredTerm = 7,
		/// <summary>To be added.</summary>
		DistortionCubicTerm = 8,
		/// <summary>To be added.</summary>
		DistortionPolynomialMix = 9,
		/// <summary>To be added.</summary>
		DistortionRingModFreq1 = 10,
		/// <summary>To be added.</summary>
		DistortionRingModFreq2 = 11,
		/// <summary>To be added.</summary>
		DistortionRingModBalance = 12,
		/// <summary>To be added.</summary>
		DistortionRingModMix = 13,
		/// <summary>To be added.</summary>
		DistortionSoftClipGain = 14,
		/// <summary>To be added.</summary>
		DistortionFinalMix = 15,

		// AUDelay
		/// <summary>To be added.</summary>
		DelayWetDryMix = 0,
		/// <summary>To be added.</summary>
		DelayTime = 1,
		/// <summary>To be added.</summary>
		DelayFeedback = 2,
		/// <summary>To be added.</summary>
		DelayLopassCutoff = 3,

		// AUNBandEQ
		/// <summary>To be added.</summary>
		AUNBandEQGlobalGain = 0,
		/// <summary>To be added.</summary>
		AUNBandEQBypassBand = 1000,
		/// <summary>To be added.</summary>
		AUNBandEQFilterType = 2000,
		/// <summary>To be added.</summary>
		AUNBandEQFrequency = 3000,
		/// <summary>To be added.</summary>
		AUNBandEQGain = 4000,
		/// <summary>To be added.</summary>
		AUNBandEQBandwidth = 5000,

		// AURandomUnit
		/// <summary>To be added.</summary>
		RandomBoundA = 0,
		/// <summary>To be added.</summary>
		RandomBoundB = 1,
		/// <summary>To be added.</summary>
		RandomCurve = 2,

#if !MONOMAC
		// iOS reverb
		/// <summary>To be added.</summary>
		Reverb2DryWetMix = 0,
		/// <summary>To be added.</summary>
		Reverb2Gain = 1,
		/// <summary>To be added.</summary>
		Reverb2MinDelayTime = 2,
		/// <summary>To be added.</summary>
		Reverb2MaxDelayTime = 3,
		/// <summary>To be added.</summary>
		Reverb2DecayTimeAt0Hz = 4,
		/// <summary>To be added.</summary>
		Reverb2DecayTimeAtNyquist = 5,
		/// <summary>To be added.</summary>
		Reverb2RandomizeReflections = 6,
#endif

		// RoundTripAAC
		/// <summary>To be added.</summary>
		RoundTripAacFormat = 0,
		/// <summary>To be added.</summary>
		RoundTripAacEncodingStrategy = 1,
		/// <summary>To be added.</summary>
		RoundTripAacRateOrQuality = 2,

		// Spacial Mixer
		/// <summary>To be added.</summary>
		SpacialMixerAzimuth = 0,
		/// <summary>To be added.</summary>
		Elevation = 1,
		/// <summary>To be added.</summary>
		Distance = 2,
		/// <summary>To be added.</summary>
		Gain = 3,
		/// <summary>To be added.</summary>
		PlaybackRate = 4,
		/// <summary>To be added.</summary>
		Enable = 5,
		/// <summary>To be added.</summary>
		MinGain = 6,
		/// <summary>To be added.</summary>
		MaxGain = 7,
		/// <summary>To be added.</summary>
		ReverbBlend = 8,
		/// <summary>To be added.</summary>
		GlobalReverbGain = 9,
		/// <summary>To be added.</summary>
		OcclussionAttenuation = 10,
		/// <summary>To be added.</summary>
		ObstructionAttenuation = 11,
	}

	/// <summary>Enumerates attenuation modes.</summary>
	[MacCatalyst (13, 1)]
	public enum SpatialMixerAttenuation {
		/// <summary>To be added.</summary>
		Power = 0,
		/// <summary>To be added.</summary>
		Exponential = 1,
		/// <summary>To be added.</summary>
		Inverse = 2,
		/// <summary>To be added.</summary>
		Linear = 3,
	}

	/// <summary>Flagging enumeration used to control spatial mixing.</summary>
	[Flags]
	[MacCatalyst (13, 1)]
	public enum SpatialMixerRenderingFlags {
		/// <summary>To be added.</summary>
		InterAuralDelay = (1 << 0),
		/// <summary>Developers should not use this deprecated field. </summary>
		[Deprecated (PlatformName.iOS, 9, 0)]
		[Deprecated (PlatformName.TvOS, 9, 0)]
		[Deprecated (PlatformName.MacCatalyst, 13, 1)]
		[Deprecated (PlatformName.MacOSX, 10, 11)]
		DistanceAttenuation = (1 << 2),
	}

	/// <summary>Enumerates timing flags for rendering audio slices.</summary>
	[Flags]
	public enum ScheduledAudioSliceFlag {
		/// <summary>To be added.</summary>
		Complete = 0x01,
		/// <summary>To be added.</summary>
		BeganToRender = 0x02,
		/// <summary>To be added.</summary>
		BeganToRenderLate = 0x04,

		/// <summary>To be added.</summary>
		[MacCatalyst (13, 1)]
		Loop = 0x08,
		/// <summary>To be added.</summary>
		[MacCatalyst (13, 1)]
		Interrupt = 0x10,
		/// <summary>To be added.</summary>
		[MacCatalyst (13, 1)]
		InterruptAtLoop = 0x20,
	}

	/// <summary>An enumeration whose values specify roles and contexts for audio unit properties.</summary>
	public enum AudioUnitScopeType { // UInt32 AudioUnitScope
		/// <summary>To be added.</summary>
		Global = 0,
		/// <summary>To be added.</summary>
		Input = 1,
		/// <summary>To be added.</summary>
		Output = 2,
		/// <summary>To be added.</summary>
		Group = 3,
		/// <summary>To be added.</summary>
		Part = 4,
		/// <summary>To be added.</summary>
		Note = 5,
		/// <summary>To be added.</summary>
		Layer = 6,
		/// <summary>To be added.</summary>
		LayerItem = 7,
	}

	/// <summary>An enumeration whose values specify configuration flags for audio-unit rendering.</summary>
	[Flags]
	public enum AudioUnitRenderActionFlags { // UInt32 AudioUnitRenderActionFlags
		/// <summary>To be added.</summary>
		PreRender = (1 << 2),
		/// <summary>To be added.</summary>
		PostRender = (1 << 3),
		/// <summary>To be added.</summary>
		OutputIsSilence = (1 << 4),
		/// <summary>To be added.</summary>
		OfflinePreflight = (1 << 5),
		/// <summary>To be added.</summary>
		OfflineRender = (1 << 6),
		/// <summary>To be added.</summary>
		OfflineComplete = (1 << 7),
		/// <summary>To be added.</summary>
		PostRenderError = (1 << 8),
		/// <summary>To be added.</summary>
		DoNotCheckRenderArgs = (1 << 9),
	}

	/// <summary>Enumerates events relating to remote control commands.</summary>
	public enum AudioUnitRemoteControlEvent // Unused?
	{
		/// <summary>To be added.</summary>
		TogglePlayPause = 1,
		/// <summary>To be added.</summary>
		ToggleRecord = 2,
		/// <summary>To be added.</summary>
		Rewind = 3,
	}

	[Native]
	public enum AudioUnitBusType : long {
		/// <summary>To be added.</summary>
		Input = 1,
		/// <summary>To be added.</summary>
		Output = 2,
	}

	/// <summary>Enumerates flag values that describe the state of an audio transport.</summary>
	[Native]
	public enum AUHostTransportStateFlags : ulong {
		/// <summary>Indicates that state change has occured, such as a stop, start, seek, or other change since the host transport state block was last called.</summary>
		Changed = 1,
		/// <summary>Indicates that the transport is moving.</summary>
		Moving = 2,
		/// <summary>Indicates that the host is able to record, or is currently recording.</summary>
		Recording = 4,
		/// <summary>Indicates that the host is cycling.</summary>
		Cycling = 8,
	}

	public enum AUEventSampleTime : long {
		/// <summary>To be added.</summary>
		Immediate = unchecked((long) 0xffffffff00000000),
	}

	/// <summary>Enumerates options that can be used while instantiating a <see cref="AudioUnit.AUAudioUnit" />.</summary>
	[MacCatalyst (13, 1)]
	public enum AudioComponentInstantiationOptions : uint {
		/// <summary>To be added.</summary>
		OutOfProcess = 1,
		/// <summary>To be added.</summary>
		[NoiOS, NoTV, NoMacCatalyst]
		InProcess = 2,
		[iOS (14, 5), TV (14, 5), NoMac]
		[MacCatalyst (14, 5)]
		LoadedRemotely = 1u << 31,
	}

	/// <summary>Enumerates audio unit bus input-ouput capabilities.</summary>
	[Native]
	public enum AUAudioUnitBusType : long {
		/// <summary>Indicates an input bus.</summary>
		Input = 1,
		/// <summary>Indicates an output bus.</summary>
		Output = 2,
	}

	public enum AudioUnitParameterOptions : uint {
		/// <summary>To be added.</summary>
		CFNameRelease = (1 << 4),
		/// <summary>To be added.</summary>
		OmitFromPresets = (1 << 13),
		/// <summary>To be added.</summary>
		PlotHistory = (1 << 14),
		/// <summary>To be added.</summary>
		MeterReadOnly = (1 << 15),
		/// <summary>To be added.</summary>
		DisplayMask = (7 << 16) | (1 << 22),
		/// <summary>To be added.</summary>
		DisplaySquareRoot = (1 << 16),
		/// <summary>To be added.</summary>
		DisplaySquared = (2 << 16),
		/// <summary>To be added.</summary>
		DisplayCubed = (3 << 16),
		/// <summary>To be added.</summary>
		DisplayCubeRoot = (4 << 16),
		/// <summary>To be added.</summary>
		DisplayExponential = (5 << 16),
		/// <summary>To be added.</summary>
		HasClump = (1 << 20),
		/// <summary>To be added.</summary>
		ValuesHaveStrings = (1 << 21),
		/// <summary>To be added.</summary>
		DisplayLogarithmic = (1 << 22),
		/// <summary>To be added.</summary>
		IsHighResolution = (1 << 23),
		/// <summary>To be added.</summary>
		NonRealTime = (1 << 24),
		/// <summary>To be added.</summary>
		CanRamp = (1 << 25),
		/// <summary>To be added.</summary>
		ExpertMode = (1 << 26),
		/// <summary>To be added.</summary>
		HasCFNameString = (1 << 27),
		/// <summary>To be added.</summary>
		IsGlobalMeta = (1 << 28),
		/// <summary>To be added.</summary>
		IsElementMeta = (1 << 29),
		/// <summary>To be added.</summary>
		IsReadable = (1 << 30),
		/// <summary>To be added.</summary>
		IsWritable = unchecked((uint) 1 << 31),
	}

	public enum AudioComponentValidationResult : uint {
		/// <summary>To be added.</summary>
		Unknown = 0,
		/// <summary>To be added.</summary>
		Passed,
		/// <summary>To be added.</summary>
		Failed,
		/// <summary>To be added.</summary>
		TimedOut,
		/// <summary>To be added.</summary>
		UnauthorizedErrorOpen,
		/// <summary>To be added.</summary>
		UnauthorizedErrorInit,
	}

	public enum AUSpatialMixerAttenuationCurve : uint {
		/// <summary>To be added.</summary>
		Power = 0,
		/// <summary>To be added.</summary>
		Exponential = 1,
		/// <summary>To be added.</summary>
		Inverse = 2,
		/// <summary>To be added.</summary>
		Linear = 3,
	}

	public enum AU3DMixerRenderingFlags : uint {
		/// <summary>To be added.</summary>
		InterAuralDelay = (1 << 0),
		/// <summary>To be added.</summary>
		DopplerShift = (1 << 1),
		/// <summary>To be added.</summary>
		DistanceAttenuation = (1 << 2),
		/// <summary>To be added.</summary>
		DistanceFilter = (1 << 3),
		/// <summary>To be added.</summary>
		DistanceDiffusion = (1 << 4),
		/// <summary>To be added.</summary>
		LinearDistanceAttenuation = (1 << 5),
		/// <summary>To be added.</summary>
		ConstantReverbBlend = (1 << 6),
	}

	public enum AUReverbRoomType : uint {
		/// <summary>To be added.</summary>
		SmallRoom = 0,
		/// <summary>To be added.</summary>
		MediumRoom = 1,
		/// <summary>To be added.</summary>
		LargeRoom = 2,
		/// <summary>To be added.</summary>
		MediumHall = 3,
		/// <summary>To be added.</summary>
		LargeHall = 4,
		/// <summary>To be added.</summary>
		Plate = 5,
		/// <summary>To be added.</summary>
		MediumChamber = 6,
		/// <summary>To be added.</summary>
		LargeChamber = 7,
		/// <summary>To be added.</summary>
		Cathedral = 8,
		/// <summary>To be added.</summary>
		LargeRoom2 = 9,
		/// <summary>To be added.</summary>
		MediumHall2 = 10,
		/// <summary>To be added.</summary>
		MediumHall3 = 11,
		/// <summary>To be added.</summary>
		LargeHall2 = 12,
	}

	public enum AUScheduledAudioSliceFlags : uint {
		/// <summary>To be added.</summary>
		Complete = 1,
		/// <summary>To be added.</summary>
		BeganToRender = 2,
		/// <summary>To be added.</summary>
		BeganToRenderLate = 4,
		/// <summary>To be added.</summary>
		Loop = 8,
		/// <summary>To be added.</summary>
		Interrupt = 16,
		/// <summary>To be added.</summary>
		InterruptAtLoop = 32,
	}

	public enum AUSpatializationAlgorithm : uint {
		/// <summary>To be added.</summary>
		EqualPowerPanning = 0,
		/// <summary>To be added.</summary>
		SphericalHead = 1,
		/// <summary>To be added.</summary>
		Hrtf = 2,
		/// <summary>To be added.</summary>
		SoundField = 3,
		/// <summary>To be added.</summary>
		VectorBasedPanning = 4,
		/// <summary>To be added.</summary>
		StereoPassThrough = 5,
		/// <summary>To be added.</summary>
		HrtfHQ = 6,
		[iOS (14, 0)]
		[TV (14, 0)]
		[MacCatalyst (14, 0)]
		UseOutputType = 7,
	}

	/// <summary>Enumerates attentuation curve types.</summary>
	public enum AU3DMixerAttenuationCurve : uint {
		/// <summary>Indicates an equal-power attenuation curve.</summary>
		Power = 0,
		/// <summary>Indicates an exponential attenuation curve.</summary>
		Exponential = 1,
		/// <summary>Indicates an inverse attenuation curve.</summary>
		Inverse = 2,
		/// <summary>Indicates a linear attenuation curve.</summary>
		Linear = 3,
	}

	public enum AUSpatialMixerRenderingFlags : uint {
		/// <summary>To be added.</summary>
		InterAuralDelay = (1 << 0),
		/// <summary>To be added.</summary>
		DistanceAttenuation = (1 << 2),
	}

	[MacCatalyst (13, 1)]
	public enum AUParameterAutomationEventType : uint {
		/// <summary>To be added.</summary>
		Value = 0,
		/// <summary>To be added.</summary>
		Touch = 1,
		/// <summary>To be added.</summary>
		Release = 2,
	}

	[iOS (15, 0), TV (15, 0), MacCatalyst (15, 0)]
	public enum AUVoiceIOSpeechActivityEvent : uint {
		Started = 0,
		Ended = 1,
	}

	[iOS (16, 0), TV (16, 0), Mac (13, 0), MacCatalyst (16, 0)]
	public enum AudioUnitEventType : uint {
		ParameterValueChange = 0,
		BeginParameterChangeGesture = 1,
		EndParameterChangeGesture = 2,
		PropertyChange = 3,
	}


	public enum AudioUnitSubType : uint {
		/// <summary>To be added.</summary>
		AUConverter = 0x636F6E76, // 'conv'
		/// <summary>To be added.</summary>
		Varispeed = 0x76617269, // 'vari'
		/// <summary>To be added.</summary>
		DeferredRenderer = 0x64656672, // 'defr'
		/// <summary>To be added.</summary>
		Splitter = 0x73706C74, // 'splt'
		/// <summary>To be added.</summary>
		MultiSplitter = 0x6D73706C, // 'mspl'
		/// <summary>To be added.</summary>
		Merger = 0x6D657267, // 'merg'
		/// <summary>To be added.</summary>
		NewTimePitch = 0x6E757470, // 'nutp'
		/// <summary>To be added.</summary>
		AUiPodTimeOther = 0x6970746F, // 'ipto'
		/// <summary>To be added.</summary>
		RoundTripAac = 0x72616163, // 'raac'
		/// <summary>To be added.</summary>
		GenericOutput = 0x67656E72, // 'genr'
		/// <summary>To be added.</summary>
		VoiceProcessingIO = 0x7670696F, // 'vpio'
		/// <summary>To be added.</summary>
		Sampler = 0x73616D70, // 'samp'
		/// <summary>To be added.</summary>
		MidiSynth = 0x6D73796E, // 'msyn'
		/// <summary>To be added.</summary>
		PeakLimiter = 0x6C6D7472, // 'lmtr'
		/// <summary>To be added.</summary>
		DynamicsProcessor = 0x64636D70, // 'dcmp'
		/// <summary>To be added.</summary>
		LowPassFilter = 0x6C706173, // 'lpas'
		/// <summary>To be added.</summary>
		HighPassFilter = 0x68706173, // 'hpas'
		/// <summary>To be added.</summary>
		BandPassFilter = 0x62706173, // 'bpas'
		/// <summary>To be added.</summary>
		HighShelfFilter = 0x68736866, // 'hshf'
		/// <summary>To be added.</summary>
		LowShelfFilter = 0x6C736866, // 'lshf'
		/// <summary>To be added.</summary>
		ParametricEQ = 0x706D6571, // 'pmeq'
		/// <summary>To be added.</summary>
		Distortion = 0x64697374, // 'dist'
		/// <summary>To be added.</summary>
		Delay = 0x64656C79, // 'dely'
		/// <summary>To be added.</summary>
		SampleDelay = 0x73646C79, // 'sdly'
		/// <summary>To be added.</summary>
		NBandEQ = 0x6E626571, // 'nbeq'
		/// <summary>To be added.</summary>
		MultiChannelMixer = 0x6D636D78, // 'mcmx'
		/// <summary>To be added.</summary>
		MatrixMixer = 0x6D786D78, // 'mxmx'
		/// <summary>To be added.</summary>
		SpatialMixer = 0x3364656D, // '3dem'
		/// <summary>To be added.</summary>
		ScheduledSoundPlayer = 0x7373706C, // 'sspl'
		/// <summary>To be added.</summary>
		AudioFilePlayer = 0x6166706C, // 'afpl'

#if MONOMAC
		/// <summary>To be added.</summary>
		HALOutput = 0x6168616C, // 'ahal'
		/// <summary>To be added.</summary>
		DefaultOutput = 0x64656620, // 'def '
		/// <summary>To be added.</summary>
		SystemOutput = 0x73797320, // 'sys '
		/// <summary>To be added.</summary>
		DLSSynth = 0x646C7320, // 'dls '
		/// <summary>To be added.</summary>
		TimePitch = 0x746D7074, // 'tmpt'
		/// <summary>To be added.</summary>
		GraphicEQ = 0x67726571, // 'greq'
		/// <summary>To be added.</summary>
		MultiBandCompressor = 0x6D636D70, // 'mcmp'
		/// <summary>To be added.</summary>
		MatrixReverb = 0x6D726576, // 'mrev'
		/// <summary>To be added.</summary>
		Pitch = 0x746D7074, // 'tmpt'
		/// <summary>To be added.</summary>
		AUFilter = 0x66696C74, // 'filt
		/// <summary>To be added.</summary>
		NetSend = 0x6E736E64, // 'nsnd'
		/// <summary>To be added.</summary>
		RogerBeep = 0x726F6772, // 'rogr'
		/// <summary>To be added.</summary>
		StereoMixer = 0x736D7872, // 'smxr'
		/// <summary>To be added.</summary>
		SphericalHeadPanner = 0x73706872, // 'sphr'
		/// <summary>To be added.</summary>
		VectorPanner = 0x76626173, // 'vbas'
		/// <summary>To be added.</summary>
		SoundFieldPanner = 0x616D6269, // 'ambi'
		/// <summary>To be added.</summary>
		HRTFPanner = 0x68727466, // 'hrtf'
		/// <summary>To be added.</summary>
		NetReceive = 0x6E726376, // 'nrcv'
#endif
	}

	[MacCatalyst (17, 0), Mac (14, 0), NoTV, NoiOS]
	public enum AudioAggregateDriftCompensation : uint {
		MinQuality = 0,
		LowQuality = 0x20,
		MediumQuality = 0x40,
		HighQuality = 0x60,
		MaxQuality = 0x7F,
	}
}
