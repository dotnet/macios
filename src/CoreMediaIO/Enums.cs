// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

#nullable enable

namespace CoreMediaIO {
	/// <summary>The direction of a camera-extension stream.</summary>
	[NoiOS, NoTV, Mac (12, 3), MacCatalyst (15, 4)]
	[Native]
	public enum CMIOExtensionStreamDirection : long {
		/// <summary>The extension produces buffers for clients.</summary>
		Source = 0,
		/// <summary>The extension consumes buffers from clients.</summary>
		Sink = 1,
	}

	/// <summary>The clock used by a camera-extension stream.</summary>
	[NoiOS, NoTV, Mac (12, 3), MacCatalyst (15, 4)]
	[Native]
	public enum CMIOExtensionStreamClockType : long {
		/// <summary>The host's time clock.</summary>
		HostTime = 0,
		/// <summary>The clock of the linked Core Audio device.</summary>
		LinkedCoreAudioDeviceUid = 1,
		/// <summary>A custom clock.</summary>
		Custom = 2,
	}

	/// <summary>Discontinuities reported by a camera-extension stream.</summary>
	[NoiOS, NoTV, Mac (12, 3), MacCatalyst (15, 4)]
	[Flags]
	public enum CMIOExtensionStreamDiscontinuityFlags : uint {
		/// <summary>No discontinuity.</summary>
		None = 0,
		/// <summary>A discontinuity of unknown origin.</summary>
		Unknown = 1,
		/// <summary>A time discontinuity.</summary>
		Time = 1 << 1,
		/// <summary>A sample was dropped.</summary>
		SampleDropped = 1 << 6,
	}

	/// <summary>Discontinuities attached to Core Media I/O sample buffers.</summary>
	[NoiOS, NoTV, MacCatalyst (13, 1)]
	[Flags]
	public enum CMIOSampleBufferDiscontinuityFlags : uint {
		/// <summary>No discontinuity.</summary>
		None = 0,
		/// <summary>Data was lost for an unknown reason.</summary>
		Unknown = 1,
		/// <summary>A break in timecode.</summary>
		Timecode = 1 << 1,
		/// <summary>A packet error.</summary>
		PacketError = 1 << 2,
		/// <summary>A stream discontinuity.</summary>
		Stream = 1 << 3,
		/// <summary>Malformed data.</summary>
		MalformedData = 1 << 4,
		/// <summary>Data was flushed.</summary>
		DataWasFlushed = 1 << 5,
		/// <summary>Data was dropped.</summary>
		DataWasDropped = 1 << 6,
		/// <summary>A buffer overrun.</summary>
		BufferOverrun = 1 << 7,
		/// <summary>A decoding timestamp discontinuity.</summary>
		DecodingTimestamp = 1 << 8,
		/// <summary>Data is related to a discontinuity.</summary>
		RelatedToDiscontinuity = 1 << 9,
		/// <summary>A client synchronization discontinuity.</summary>
		ClientSync = 1 << 10,
		/// <summary>A trick-play discontinuity.</summary>
		TrickPlay = 1 << 11,
		/// <summary>A marker indicating missing data.</summary>
		NoDataMarker = 1 << 12,
		/// <summary>The data format changed.</summary>
		DataFormatChanged = 1 << 13,
		/// <summary>The timing reference jumped.</summary>
		TimingReferenceJumped = 1 << 14,
		/// <summary>The duration was extended.</summary>
		DurationWasExtended = 1 << 15,
		/// <summary>A sleep/wake cycle occurred.</summary>
		SleepWakeCycle = 1 << 16,
		/// <summary>The codec settings changed.</summary>
		CodecSettingsChanged = 1 << 17,
	}

	/// <summary>The reason media data is unavailable.</summary>
	[NoiOS, NoTV, MacCatalyst (13, 1)]
	public enum CMIOSampleBufferNoDataEvent : uint {
		/// <summary>The reason is unknown.</summary>
		Unknown = 0,
		/// <summary>No media is available.</summary>
		NoMedia = 1,
		/// <summary>The device could not synchronize.</summary>
		DeviceDidNotSync = 2,
		/// <summary>The device is in the wrong mode.</summary>
		DeviceInWrongMode = 3,
		/// <summary>Media processing failed.</summary>
		ProcessingError = 4,
		/// <summary>A sleep/wake cycle interrupted the media.</summary>
		SleepWakeCycle = 5,
	}

	/// <summary>Scopes used to address Core Media I/O properties.</summary>
	[NoiOS, NoTV, MacCatalyst (13, 1)]
	public enum CMIOObjectPropertyScope : uint {
		/// <summary>The global scope.</summary>
		Global = 0x676c6f62,
		/// <summary>The input scope.</summary>
		Input = 0x696e7074,
		/// <summary>The output scope.</summary>
		Output = 0x6f757470,
		/// <summary>The play-through scope.</summary>
		PlayThrough = 0x70747275,
		/// <summary>All scopes, for property listeners.</summary>
		Wildcard = 0x2a2a2a2a,
	}

	/// <summary>Common object, system, device, and stream property selectors. Custom selectors may be used by casting their four-character codes to this type.</summary>
	[NoiOS, NoTV, MacCatalyst (13, 1)]
	public enum CMIOObjectPropertySelector : uint {
		/// <summary>All selectors, for property listeners.</summary>
		Wildcard = 0x2a2a2a2a,
		/// <summary>The object's class identifier.</summary>
		Class = 0x636c6173,
		/// <summary>The object's owner.</summary>
		Owner = 0x73746476,
		/// <summary>The plug-in that created the object.</summary>
		Creator = 0x6f706c67,
		/// <summary>The object's localized name.</summary>
		Name = 0x6c6e616d,
		/// <summary>The object's manufacturer.</summary>
		Manufacturer = 0x6c6d616b,
		/// <summary>The objects owned by this object.</summary>
		OwnedObjects = 0x6f776e64,
		/// <summary>The list of available device identifiers on the system object.</summary>
		HardwareDevices = 0x64657623,
		/// <summary>The default input device identifier on the system object.</summary>
		HardwareDefaultInputDevice = 0x64496e20,
		/// <summary>The default output device identifier on the system object.</summary>
		HardwareDefaultOutputDevice = 0x644f7574,
		/// <summary>The device identifier matching a device UID qualifier.</summary>
		HardwareDeviceForUid = 0x64756964,
		/// <summary>Whether screen capture devices are exposed by the system object.</summary>
		HardwareAllowScreenCaptureDevices = 0x79657320,
		/// <summary>Whether wireless screen capture devices are exposed by the system object.</summary>
		HardwareAllowWirelessScreenCaptureDevices = 0x77736364,
		/// <summary>The device's unique identifier.</summary>
		DeviceUid = 0x75696420,
		/// <summary>The device's model identifier.</summary>
		DeviceModelUid = 0x6d756964,
		/// <summary>The device's transport type.</summary>
		DeviceTransportType = 0x7472616e,
		/// <summary>Whether the device is connected and available.</summary>
		DeviceIsAlive = 0x6c69766e,
		/// <summary>Whether the device is running in this process.</summary>
		DeviceIsRunning = 0x676f696e,
		/// <summary>Whether any process is running the device.</summary>
		DeviceIsRunningSomewhere = 0x676f6e65,
		/// <summary>The device's latency.</summary>
		DeviceLatency = 0x6c746e63,
		/// <summary>The device's stream identifiers in the selected scope.</summary>
		DeviceStreams = 0x73746d23,
		/// <summary>The channel counts for the device's streams.</summary>
		DeviceStreamConfiguration = 0x736c6179,
		/// <summary>The unique identifier of the linked Core Audio device.</summary>
		DeviceLinkedCoreAudioDeviceUid = 0x706c7564,
		/// <summary>The stream's direction.</summary>
		StreamDirection = 0x73646972,
		/// <summary>The stream's terminal type.</summary>
		StreamTerminalType = 0x7465726d,
		/// <summary>The stream's starting channel.</summary>
		StreamStartingChannel = 0x7363686e,
		/// <summary>The stream's format description.</summary>
		StreamFormatDescription = 0x70667420,
		/// <summary>The stream's available format descriptions.</summary>
		StreamFormatDescriptions = 0x70667461,
		/// <summary>The stream's frame rate.</summary>
		StreamFrameRate = 0x6e667274,
		/// <summary>The stream's minimum frame rate.</summary>
		StreamMinimumFrameRate = 0x6d667274,
		/// <summary>The stream's available frame rates.</summary>
		StreamFrameRates = 0x6e667223,
		/// <summary>The stream's available frame rate ranges.</summary>
		StreamFrameRateRanges = 0x66727267,
		/// <summary>The stream's clock.</summary>
		StreamClock = 0x706d636c,
		/// <summary>The stream's output queue capacity.</summary>
		StreamOutputBufferQueueSize = 0x706d6f71,
		/// <summary>The number of output buffers required to start streaming.</summary>
		StreamOutputBuffersRequiredForStartup = 0x706d6f73,
		/// <summary>Whether the output stream has reached the end of its data.</summary>
		StreamEndOfData = 0x706d6564,
		/// <summary>The localized name of an element.</summary>
		ElementName = 0x6c63686e,
		/// <summary>The localized category name of an element.</summary>
		ElementCategoryName = 0x6c63636e,
		/// <summary>The localized number name of an element.</summary>
		ElementNumberName = 0x6c636e6e,
		/// <summary>Notifies an object that a property listener was added.</summary>
		ListenerAdded = 0x6c697361,
		/// <summary>Notifies an object that a property listener was removed.</summary>
		ListenerRemoved = 0x6c697372,
		/// <summary>Whether this process is the main Core Media I/O process.</summary>
		HardwareProcessIsMain = 0x6d61696e,
		/// <summary>Whether the hardware system is initializing or exiting.</summary>
		HardwareIsInitingOrExiting = 0x696e6f74,
		/// <summary>Whether the hardware system allows sleep.</summary>
		HardwareSleepingIsAllowed = 0x736c6570,
		/// <summary>Whether the hardware system may unload plug-ins.</summary>
		HardwareUnloadingIsAllowed = 0x756e6c64,
		/// <summary>The plug-in identifier matching a bundle identifier qualifier.</summary>
		HardwarePlugInForBundleId = 0x70696269,
		/// <summary>Whether the user session is active or headless.</summary>
		HardwareUserSessionIsActiveOrHeadless = 0x75736572,
		/// <summary>Whether the hardware system is suspended.</summary>
		HardwareSuspendedBySystem = 0x73627973,
		/// <summary>The plug-in that owns a device.</summary>
		DevicePlugIn = 0x706c7567,
		/// <summary>Whether the device's configuration changed.</summary>
		DeviceHasChanged = 0x64696666,
		/// <summary>Whether the device may be selected as the default.</summary>
		DeviceCanBeDefaultDevice = 0x64666c74,
		/// <summary>The process with exclusive device access.</summary>
		DeviceHogMode = 0x6f696e6b,
		/// <summary>The process controlling the device.</summary>
		DeviceControl = 0x706d6e68,
		/// <summary>Whether non-DAL clients are excluded.</summary>
		DeviceExcludeNonDalAccess = 0x69786e61,
		/// <summary>The device's client synchronization discontinuity behavior.</summary>
		DeviceClientSyncDiscontinuity = 0x706d6373,
		/// <summary>The callback used to obtain SMPTE time.</summary>
		DeviceSmpteTimeCallback = 0x706d7363,
		/// <summary>Whether the device accepts AV/C commands.</summary>
		DeviceCanProcessAvcCommand = 0x706d6163,
		/// <summary>The device's AV/C type.</summary>
		DeviceAvcDeviceType = 0x706d6174,
		/// <summary>The device's AV/C signal mode.</summary>
		DeviceAvcDeviceSignalMode = 0x706d736d,
		/// <summary>Whether the device accepts RS-422 commands.</summary>
		DeviceCanProcessRS422Command = 0x72343232,
		/// <summary>The device's video digitizer components.</summary>
		DeviceVideoDigitizerComponents = 0x76646967,
		/// <summary>Whether the user suspended the device.</summary>
		DeviceSuspendedByUser = 0x73627975,
		/// <summary>The identifier of the linked and synchronized Core Audio device.</summary>
		DeviceLinkedAndSyncedCoreAudioDeviceUid = 0x706c7364,
		/// <summary>The initial unit space for an IIDC device.</summary>
		DeviceIidcInitialUnitSpace = 0x69756e73,
		/// <summary>The IIDC control and status register data.</summary>
		DeviceIidcCsrData = 0x63737264,
		/// <summary>Whether frame rates can be changed without dropping frames.</summary>
		DeviceCanSwitchFrameRatesWithoutFrameDrops = 0x66726e64,
		/// <summary>The device's physical location.</summary>
		DeviceLocation = 0x646c6f63,
		/// <summary>Whether the device encountered a streaming error.</summary>
		DeviceHasStreamingError = 0x73657272,
		/// <summary>The stream's latency.</summary>
		StreamLatency = 0x6c746e63,
		/// <summary>A still image from the stream.</summary>
		StreamStillImage = 0x73746d67,
		/// <summary>The stream's available still-image formats.</summary>
		StreamStillImageFormatDescriptions = 0x73746674,
		/// <summary>The no-data timeout in milliseconds.</summary>
		StreamNoDataTimeoutInMilliseconds = 0x706d6e31,
		/// <summary>The device synchronization timeout in milliseconds.</summary>
		StreamDeviceSyncTimeoutInMilliseconds = 0x706d6e32,
		/// <summary>The number of no-data events.</summary>
		StreamNoDataEventCount = 0x706d6e33,
		/// <summary>The number of output buffer underruns.</summary>
		StreamOutputBufferUnderrunCount = 0x706d6f75,
		/// <summary>The number of repeated output buffers.</summary>
		StreamOutputBufferRepeatCount = 0x706d6f72,
		/// <summary>The number of output buffers required for throttled playback.</summary>
		StreamOutputBuffersNeededForThrottledPlayback = 0x6d696666,
		/// <summary>The first output presentation timestamp.</summary>
		StreamFirstOutputPresentationTimeStamp = 0x706f7074,
		/// <summary>Whether the stream accepts deck commands.</summary>
		StreamCanProcessDeckCommand = 0x70646364,
		/// <summary>The deck's status and state.</summary>
		StreamDeck = 0x6465636b,
		/// <summary>The deck's current frame number.</summary>
		StreamDeckFrameNumber = 0x74636f64,
		/// <summary>Whether the deck uses drop-frame timecode.</summary>
		StreamDeckDropness = 0x64726f70,
		/// <summary>Whether the deck is threaded.</summary>
		StreamDeckThreaded = 0x74687264,
		/// <summary>Whether the deck is under local control.</summary>
		StreamDeckLocal = 0x6c6f636c,
		/// <summary>The deck's cueing state.</summary>
		StreamDeckCueing = 0x63756563,
		/// <summary>The initial presentation timestamp for linked and synchronized audio.</summary>
		StreamInitialPresentationTimeStampForLinkedAndSyncedAudio = 0x69706c73,
		/// <summary>The scheduled-output notification callback and context.</summary>
		StreamScheduledOutputNotificationProc = 0x736f6e70,
		/// <summary>The stream's preferred format.</summary>
		StreamPreferredFormatDescription = 0x70726664,
		/// <summary>The stream's preferred frame rate.</summary>
		StreamPreferredFrameRate = 0x70726672,
		/// <summary>The control's scope.</summary>
		ControlScope = 0x63736370,
		/// <summary>The control's element.</summary>
		ControlElement = 0x63656c6d,
		/// <summary>The control's variant.</summary>
		ControlVariant = 0x63766172,
		/// <summary>The boolean control's value.</summary>
		BooleanControlValue = 0x6263766c,
		/// <summary>The selector control's current item.</summary>
		SelectorControlCurrentItem = 0x73636369,
		/// <summary>The selector control's available items.</summary>
		SelectorControlAvailableItems = 0x73636169,
		/// <summary>The name of a selector control item.</summary>
		SelectorControlItemName = 0x7363696e,
		/// <summary>The names of the selector control's available items.</summary>
		SelectorControlAvailableItemNames = 0x7363616e,
		/// <summary>Whether the feature control is enabled.</summary>
		FeatureControlOnOff = 0x66636f6f,
		/// <summary>The feature control's automatic/manual mode.</summary>
		FeatureControlAutomaticManual = 0x6663616d,
		/// <summary>The feature control's absolute/native mode.</summary>
		FeatureControlAbsoluteNative = 0x66636e61,
		/// <summary>The feature control's tuning operation.</summary>
		FeatureControlTune = 0x6663746e,
		/// <summary>The feature control's native value.</summary>
		FeatureControlNativeValue = 0x66636e76,
		/// <summary>The feature control's absolute value.</summary>
		FeatureControlAbsoluteValue = 0x66636176,
		/// <summary>The feature control's native range.</summary>
		FeatureControlNativeRange = 0x66636e72,
		/// <summary>The feature control's absolute range.</summary>
		FeatureControlAbsoluteRange = 0x66636172,
		/// <summary>Converts a feature control's native value to absolute units.</summary>
		FeatureControlConvertNativeToAbsolute = 0x666e3261,
		/// <summary>Converts a feature control's absolute value to native units.</summary>
		FeatureControlConvertAbsoluteToNative = 0x6661326e,
		/// <summary>The name of the feature control's absolute unit.</summary>
		FeatureControlAbsoluteUnitName = 0x6663756e,
		/// <summary>The feature control's native data.</summary>
		FeatureControlNativeData = 0x66636e64,
		/// <summary>The feature control's native data range.</summary>
		FeatureControlNativeDataRange = 0x66636472,
		/// <summary>The exposure control's region of interest.</summary>
		ExposureControlRegionOfInterest = 0x65726f69,
		/// <summary>The exposure control's lock threshold.</summary>
		ExposureControlLockThreshold = 0x656c636b,
		/// <summary>The exposure control's unlock threshold.</summary>
		ExposureControlUnlockThreshold = 0x65756c6b,
		/// <summary>The exposure control's target.</summary>
		ExposureControlTarget = 0x65746774,
		/// <summary>The exposure control's convergence speed.</summary>
		ExposureControlConvergenceSpeed = 0x65637370,
		/// <summary>The exposure control's stability setting.</summary>
		ExposureControlStability = 0x65737479,
		/// <summary>Whether the exposure control is stable.</summary>
		ExposureControlStable = 0x65737462,
		/// <summary>The exposure control's integration time.</summary>
		ExposureControlIntegrationTime = 0x65696e74,
		/// <summary>The exposure control's maximum gain.</summary>
		ExposureControlMaximumGain = 0x656d6178,
	}
}
