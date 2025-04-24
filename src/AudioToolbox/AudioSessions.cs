// 
// AudioSessions.cs:
//
// Authors: Mono Team
//     
// Copyright 2009 Novell, Inc
// Copyright 2011, 2012 Xamarin Inc.
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
using System.Collections.Generic;
using System.Runtime.InteropServices;
using CoreFoundation;
using ObjCRuntime;

#nullable enable

namespace AudioToolbox {

	/// <summary>An enumeration whose values can be returned by the <see cref="AudioToolbox.AudioSession.SetActive(System.Boolean,AudioToolbox.AudioSessionActiveFlags)" /> method or the <see cref="AudioToolbox.AudioSessionException.ErrorCode" /> property.</summary>
	public enum AudioSessionErrors { // Implictly cast to OSType 
		/// <summary>To be added.</summary>
		None = 0,
		/// <summary>To be added.</summary>
		NotInitialized = 0x21696e69, // '!ini',
		/// <summary>To be added.</summary>
		AlreadyInitialized = 0x696e6974, // 'init',
		/// <summary>To be added.</summary>
		InitializationError = 0x696e693f, // 'ini?',
		/// <summary>To be added.</summary>
		UnsupportedPropertyError = 0x7074793f, // 'pty?',
		/// <summary>To be added.</summary>
		BadPropertySizeError = 0x2173697a, // '!siz',
		/// <summary>To be added.</summary>
		NotActiveError = 0x21616374, // '!act',
		/// <summary>To be added.</summary>
		NoHardwareError = 0x6e6f6877, // 'nohw'
		/// <summary>To be added.</summary>
		IncompatibleCategory = 0x21636174, // '!cat'
		/// <summary>To be added.</summary>
		NoCategorySet = 0x3f636174, // '?cat'
		/// <summary>To be added.</summary>
		UnspecifiedError = 0x77686371, // 'what'
	}

	/// <summary>An enumeration whose values represent the beginning and end of an audio session interruption.</summary>
	public enum AudioSessionInterruptionState { // UInt32 in AudioSessionInterruptionListener
		/// <summary>To be added.</summary>
		End = 0,
		/// <summary>To be added.</summary>
		Begin = 1,
	}

	/// <summary>Categories for your audio session.</summary>
	public enum AudioSessionCategory { // UInt32 AudioSessionPropertyID
		/// <summary>
		/// 	    Use this category if playing audio is not your
		/// 	    application's main use and if your application can be used
		/// 	    with the sound off.  This allows background music to be
		/// 	    mixed with the audio your application produces.
		/// 	  </summary>
		AmbientSound = 0x616d6269, // 'ambi'
		/// <summary>
		/// 	    This is the default audio session category.
		/// 	  </summary>
		SoloAmbientSound = 0x736f6c6f, // 'solo'
		/// <summary>
		/// 	    Use this if the main use of your application is to play back music.
		/// 	  </summary>
		MediaPlayback = 0x6d656469, // 'medi'
		/// <summary>
		/// 	    Used to record audio.   This will prevent background audio and messages from playing back.
		/// 	  </summary>
		RecordAudio = 0x72656361, // 'reca'
		/// <summary>
		/// 	    Used to perform both playback and recording at the same time, for example for live chat applications.   
		/// 	  </summary>
		PlayAndRecord = 0x706c6172, // 'plar'
		/// <summary>
		/// 	    Use this category when you want to use the hardware accelerated audio support to perform audio conversions.   
		/// 	  </summary>
		AudioProcessing = 0x70726f63, // 'proc'
	}

	/// <summary>Possible values to override the audio route</summary>
	public enum AudioSessionRoutingOverride { // UInt32 AudioSessionPropertyID
		/// <summary>Use the normal audio route.</summary>
		None = 0,
		/// <summary>Send audio to the built-in speaker.  If a headset is plugged when this value is set, the microphone is also redirected to the built-in microphone.</summary>
		Speaker = 0x73706b72, // 'spkr'
	}

	/// <summary>The reason for AudioSession route change event to be raised.</summary>
	public enum AudioSessionRouteChangeReason { // UInt32 AudioSessionPropertyID
		/// <summary>The route changed, but there is no information about why.</summary>
		Unknown = 0,
		/// <summary>A new audio device has been plugged.</summary>
		NewDeviceAvailable = 1,
		/// <summary>The previous device is no longer available.</summary>
		OldDeviceUnavailable = 2,
		/// <summary>The AudioSession category has changed.</summary>
		CategoryChange = 3,
		/// <summary>Audio has been forcibly been redirected by the user setting the AudioSession Category.</summary>
		Override = 4,
		/// <summary>Device has woken up from sleep</summary>
		WakeFromSleep = 6,
		/// <summary>There is no audio device for the current set category in the audio session.  For example, recording when there is no microphone available.</summary>
		NoSuitableRouteForCategory = 7,
		/// <summary>To be added.</summary>
		RouteConfigurationChange = 8,
	}

	/// <summary>An enumeration whose values indicate whether the application should resume after an interruption. Returned by the <see cref="AudioToolbox.AudioSession.InterruptionType" /> property.</summary>
	public enum AudioSessionInterruptionType { // UInt32 AudioSessionInterruptionType
		/// <summary>To be added.</summary>
		ShouldResume = 1769108333, // 'irsm'
		/// <summary>To be added.</summary>
		ShouldNotResume = 561148781, // '!rsm'
	}

	// Should be internal with AudioSessionPropertyListener public
	/// <summary>An enumeration whose values specify properties for audio sessions.</summary>
	public enum AudioSessionProperty { // typedef UInt32 AudioSessionPropertyID
		/// <summary>To be added.</summary>
		PreferredHardwareSampleRate = 0x68777372,
		/// <summary>To be added.</summary>
		PreferredHardwareIOBufferDuration = 0x696f6264,
		/// <summary>To be added.</summary>
		AudioCategory = 0x61636174, // 'acat'
		/// <summary>Developers should not use this deprecated field. </summary>
		[Deprecated (PlatformName.iOS, 5, 0)]
		[Deprecated (PlatformName.MacOSX, 10, 7)]
		[Deprecated (PlatformName.TvOS, 9, 0)]
		[Deprecated (PlatformName.MacCatalyst, 13, 1)]
		AudioRoute = 0x726f7574,
		/// <summary>To be added.</summary>
		AudioRouteChange = 0x726f6368,
		/// <summary>To be added.</summary>
		CurrentHardwareSampleRate = 0x63687372,
		/// <summary>To be added.</summary>
		CurrentHardwareInputNumberChannels = 0x63686963,
		/// <summary>To be added.</summary>
		CurrentHardwareOutputNumberChannels = 0x63686f63,
		/// <summary>To be added.</summary>
		CurrentHardwareOutputVolume = 0x63686f76,
		/// <summary>To be added.</summary>
		CurrentHardwareInputLatency = 0x63696c74,
		/// <summary>To be added.</summary>
		CurrentHardwareOutputLatency = 0x636f6c74,
		/// <summary>To be added.</summary>
		CurrentHardwareIOBufferDuration = 0x63686264,
		/// <summary>To be added.</summary>
		OtherAudioIsPlaying = 0x6f746872,
		/// <summary>To be added.</summary>
		OverrideAudioRoute = 0x6f767264,
		/// <summary>To be added.</summary>
		AudioInputAvailable = 0x61696176,
		/// <summary>To be added.</summary>
		ServerDied = 0x64696564,
		/// <summary>To be added.</summary>
		[Deprecated (PlatformName.iOS, 7, 0)]
		[Deprecated (PlatformName.MacOSX, 10, 9)]
		[Deprecated (PlatformName.TvOS, 9, 0)]
		[Deprecated (PlatformName.MacCatalyst, 13, 1)]
		OtherMixableAudioShouldDuck = 0x6475636b,
		/// <summary>To be added.</summary>
		OverrideCategoryMixWithOthers = 0x636d6978,
		/// <summary>To be added.</summary>
		OverrideCategoryDefaultToSpeaker = 0x6373706b, //'cspk'
		/// <summary>To be added.</summary>
		OverrideCategoryEnableBluetoothInput = 0x63626c75, //'cblu'
		/// <summary>To be added.</summary>
		InterruptionType = 0x74797065,      // 'type'
		/// <summary>To be added.</summary>
		Mode = 0x6d6f6465,
		/// <summary>To be added.</summary>
		InputSources = 0x73726373,      // 'srcs'
		/// <summary>To be added.</summary>
		OutputDestinations = 0x64737473,        // 'dsts'
		/// <summary>To be added.</summary>
		InputSource = 0x69737263,       // 'isrc'
		/// <summary>To be added.</summary>
		OutputDestination = 0x6f647374,     // 'odst'
		/// <summary>To be added.</summary>
		InputGainAvailable = 0x69676176,        // 'igav'
		/// <summary>To be added.</summary>
		InputGainScalar = 0x69677363,       // 'igsc'
		/// <summary>To be added.</summary>
		AudioRouteDescription = 0x63726172,     // 'crar'
	}

	/// <summary>The fine detail over the audio mode, an extension to the AudioSession Category</summary>
	///     <remarks>	    This property is used to do the fine-level control over
	/// 	    the audio session.  The major component is determined by
	/// 	    the <see cref="AudioToolbox.AudioSession.Category" />
	/// 	    property and this is used to tune whether the session
	/// 	    behaves in the default mode (what every app got in the pre
	/// 	    iOS 5.0 days), Voice Chat, Video Recording or is being
	/// 	    used in Measurement mode (where you get raw data, without
	/// 	    any gain changes for Audio).
	/// </remarks>
	public enum AudioSessionMode { // UInt32 AudioSessionPropertyID
		/// <summary>
		///
		/// 	  The default value uses the same settings that existed in the
		/// 	  pre iOS 5.0 days.  If there is more than one microphone on
		/// 	  the device, the primary one is used.
		///
		/// 	</summary>
		Default = 0x64666c74,
		/// <summary>
		///
		/// 	  Used when you want to perform voice chats over a network connection.    The audio stack is configured for voice recording, the default system microphone is used and sets the <see cref="AudioToolbox.AudioSession.OverrideCategoryEnableBluetoothInput" /> to true. 
		///
		/// 	</summary>
		VoiceChat = 0x76636374,
		/// <summary>Configures the audio for video recording, and uses the microphone closer to the camera as its input.</summary>
		VideoRecording = 0x76726364,
		/// <summary>Used when you want to measure the audio, so no gains are applied to the data.   This is used when setting the <see cref="AudioToolbox.AudioSession.Category" /> property to PlayAndRecord or RecordAudio values.</summary>
		Measurement = 0x6d736d74,   // 'msmt'
		/// <summary>Set by GameKit, users should not set this directly.  Use VoiceChat instead. </summary>
		GameChat = 0x676d6374,  // 'gmct'
	}

	/// <summary>Flags passed to AudioSession's SetActive method.</summary>
	[Deprecated (PlatformName.iOS, 6, 0)]
	[Deprecated (PlatformName.TvOS, 9, 0)]
	[Deprecated (PlatformName.MacCatalyst, 13, 1)]
	public enum AudioSessionActiveFlags : uint // UInt32 in AudioSessionSetActiveWithFlags
	{
		/// <summary>If this value is set, the operating system might notify and resume background applications that were playing audio back.</summary>
		NotifyOthersOnDeactivation = (1 << 0),
	}

	/// <summary>The available audio input sources on the device.</summary>
	public enum AudioSessionInputRouteKind { // UInt32 AudioSessionPropertyID
		/// <summary>No input device.</summary>
		None,
		/// <summary>Line input</summary>
		LineIn,
		/// <summary>The built-in microphone (this device is not available on iPod touch devices for example).</summary>
		BuiltInMic,
		/// <summary>Headset based microphone</summary>
		HeadsetMic,
		/// <summary>Bluetooth based microphone</summary>
		BluetoothHFP,
		/// <summary>Microphone connected through USB (connected either throught the Lightning or 30-pin ports).</summary>
		USBAudio,
	}

	/// <summary>An enumeration whose values specify different kinds of output routes. </summary>
	public enum AudioSessionOutputRouteKind { // UInt32           (set only) in AudioSession.h
		/// <summary>To be added.</summary>
		None,
		/// <summary>To be added.</summary>
		LineOut,
		/// <summary>To be added.</summary>
		Headphones,
		/// <summary>To be added.</summary>
		BluetoothHFP,
		/// <summary>To be added.</summary>
		BluetoothA2DP,
		/// <summary>To be added.</summary>
		BuiltInReceiver,
		/// <summary>To be added.</summary>
		BuiltInSpeaker,
		/// <summary>To be added.</summary>
		USBAudio,
		/// <summary>To be added.</summary>
		HDMI,
		/// <summary>To be added.</summary>
		AirPlay,
	}
}
