// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

#nullable enable

namespace CoreMediaIO {
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
	}
}
