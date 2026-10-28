// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

#nullable enable

using CoreFoundation;
using CoreMedia;

namespace CoreMediaIO {
	interface ICMIOExtensionProviderSource { }
	interface ICMIOExtensionDeviceSource { }
	interface ICMIOExtensionStreamSource { }

	/// <summary>Receives a sample buffer consumed from a camera-extension client.</summary>
	[NoiOS, NoTV, Mac (12, 3), MacCatalyst (15, 4)]
	delegate void CMIOExtensionStreamConsumeSampleBufferHandler ([NullAllowed] CMSampleBuffer sampleBuffer, ulong sequenceNumber, CMIOExtensionStreamDiscontinuityFlags discontinuity, bool hasMoreSampleBuffers, [NullAllowed] NSError error);

	/// <summary>Information dictionary keys for camera extensions.</summary>
	[NoiOS, NoTV, Mac (12, 3), MacCatalyst (15, 4)]
	[Static]
	interface CMIOExtensionKeys {
		/// <summary>The extension's information dictionary.</summary>
		[Field ("CMIOExtensionInfoDictionaryKey")]
		NSString InfoDictionary { get; }

		/// <summary>The extension's Mach service name.</summary>
		[Field ("CMIOExtensionMachServiceNameKey")]
		NSString MachServiceName { get; }
	}

	/// <summary>Standard camera-extension property identifiers. Custom properties may also use string identifiers.</summary>
	[NoiOS, NoTV, Mac (12, 3), MacCatalyst (15, 4)]
	[Static]
	interface CMIOExtensionProperty {
		/// <summary>The provider's name.</summary>
		[Field ("CMIOExtensionPropertyProviderName")]
		NSString ProviderName { get; }
		/// <summary>The provider's manufacturer.</summary>
		[Field ("CMIOExtensionPropertyProviderManufacturer")]
		NSString ProviderManufacturer { get; }
		/// <summary>The device's model.</summary>
		[Field ("CMIOExtensionPropertyDeviceModel")]
		NSString DeviceModel { get; }
		/// <summary>Whether the device is suspended.</summary>
		[Field ("CMIOExtensionPropertyDeviceIsSuspended")]
		NSString DeviceIsSuspended { get; }
		/// <summary>The device's transport type.</summary>
		[Field ("CMIOExtensionPropertyDeviceTransportType")]
		NSString DeviceTransportType { get; }
		/// <summary>The linked Core Audio device's unique identifier.</summary>
		[Field ("CMIOExtensionPropertyDeviceLinkedCoreAudioDeviceUID")]
		NSString DeviceLinkedCoreAudioDeviceUid { get; }
		/// <summary>Whether the device may be the default input device.</summary>
		[Field ("CMIOExtensionPropertyDeviceCanBeDefaultInputDevice")]
		NSString DeviceCanBeDefaultInputDevice { get; }
		/// <summary>Whether the device may be the default output device.</summary>
		[Field ("CMIOExtensionPropertyDeviceCanBeDefaultOutputDevice")]
		NSString DeviceCanBeDefaultOutputDevice { get; }
		/// <summary>The device's latency.</summary>
		[Mac (14, 4), MacCatalyst (17, 4)]
		[Field ("CMIOExtensionPropertyDeviceLatency")]
		NSString DeviceLatency { get; }
		/// <summary>The index of the stream's active format.</summary>
		[Field ("CMIOExtensionPropertyStreamActiveFormatIndex")]
		NSString StreamActiveFormatIndex { get; }
		/// <summary>The stream's frame duration.</summary>
		[Field ("CMIOExtensionPropertyStreamFrameDuration")]
		NSString StreamFrameDuration { get; }
		/// <summary>The stream's maximum frame duration.</summary>
		[Field ("CMIOExtensionPropertyStreamMaxFrameDuration")]
		NSString StreamMaxFrameDuration { get; }
		/// <summary>The sink's buffer queue capacity.</summary>
		[Field ("CMIOExtensionPropertyStreamSinkBufferQueueSize")]
		NSString StreamSinkBufferQueueSize { get; }
		/// <summary>The sink's required startup buffer count.</summary>
		[Field ("CMIOExtensionPropertyStreamSinkBuffersRequiredForStartup")]
		NSString StreamSinkBuffersRequiredForStartup { get; }
		/// <summary>The sink's buffer underrun count.</summary>
		[Field ("CMIOExtensionPropertyStreamSinkBufferUnderrunCount")]
		NSString StreamSinkBufferUnderrunCount { get; }
		/// <summary>Whether the sink has reached the end of its data.</summary>
		[Field ("CMIOExtensionPropertyStreamSinkEndOfData")]
		NSString StreamSinkEndOfData { get; }
		/// <summary>The stream's latency.</summary>
		[Mac (14, 4), MacCatalyst (17, 4)]
		[Field ("CMIOExtensionPropertyStreamLatency")]
		NSString StreamLatency { get; }
	}

	/// <summary>Describes a camera-extension property's permitted values and mutability.</summary>
	[NoiOS, NoTV, Mac (12, 3), MacCatalyst (15, 4)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface CMIOExtensionPropertyAttributes : NSCopying, NSSecureCoding {
		/// <summary>Creates property attributes with optional bounds and valid values.</summary>
		[DesignatedInitializer]
		[Export ("initWithMinValue:maxValue:validValues:readOnly:")]
		NativeHandle Constructor ([NullAllowed] NSObject minValue, [NullAllowed] NSObject maxValue, [NullAllowed] NSObject [] validValues, bool readOnly);

		/// <summary>Creates property attributes with optional bounds and valid values.</summary>
		[Static]
		[Export ("propertyAttributesWithMinValue:maxValue:validValues:readOnly:")]
		CMIOExtensionPropertyAttributes Create ([NullAllowed] NSObject minValue, [NullAllowed] NSObject maxValue, [NullAllowed] NSObject [] validValues, bool readOnly);

		/// <summary>Gets attributes describing a read-only property.</summary>
		[Static]
		[Export ("readOnlyPropertyAttribute")]
		CMIOExtensionPropertyAttributes ReadOnlyPropertyAttribute { get; }
		/// <summary>Gets the minimum permitted value.</summary>
		[NullAllowed, Export ("minValue", ArgumentSemantic.Copy)]
		NSObject MinValue { get; }
		/// <summary>Gets the maximum permitted value.</summary>
		[NullAllowed, Export ("maxValue", ArgumentSemantic.Copy)]
		NSObject MaxValue { get; }
		/// <summary>Gets the permitted values.</summary>
		[NullAllowed, Export ("validValues", ArgumentSemantic.Copy)]
		NSObject [] ValidValues { get; }
		/// <summary>Gets whether the property is read-only.</summary>
		[Export ("isReadOnly")]
		bool IsReadOnly { get; }
	}

	/// <summary>Holds a camera-extension property's value and attributes.</summary>
	[NoiOS, NoTV, Mac (12, 3), MacCatalyst (15, 4)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface CMIOExtensionPropertyState : NSCopying, NSSecureCoding {
		/// <summary>Creates a state with an optional value.</summary>
		[Export ("initWithValue:")]
		NativeHandle Constructor ([NullAllowed] NSObject value);
		/// <summary>Creates a state with an optional value and attributes.</summary>
		[DesignatedInitializer]
		[Export ("initWithValue:attributes:")]
		NativeHandle Constructor ([NullAllowed] NSObject value, [NullAllowed] CMIOExtensionPropertyAttributes attributes);
		/// <summary>Creates a state with an optional value.</summary>
		[Static, Export ("propertyStateWithValue:")]
		CMIOExtensionPropertyState Create ([NullAllowed] NSObject value);
		/// <summary>Creates a state with an optional value and attributes.</summary>
		[Static, Export ("propertyStateWithValue:attributes:")]
		CMIOExtensionPropertyState Create ([NullAllowed] NSObject value, [NullAllowed] CMIOExtensionPropertyAttributes attributes);
		/// <summary>Gets the property's value.</summary>
		[NullAllowed, Export ("value", ArgumentSemantic.Copy)]
		NSObject Value { get; }
		/// <summary>Gets the property's attributes.</summary>
		[NullAllowed, Export ("attributes", ArgumentSemantic.Strong)]
		CMIOExtensionPropertyAttributes Attributes { get; }
	}

	/// <summary>Configures a camera-extension stream's custom clock.</summary>
	[NoiOS, NoTV, Mac (12, 3), MacCatalyst (15, 4)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface CMIOExtensionStreamCustomClockConfiguration : NSCopying, NSSecureCoding {
		/// <summary>Creates a custom clock configuration with timing and rate-smoothing parameters.</summary>
		[DesignatedInitializer]
		[Export ("initWithClockName:sourceIdentifier:getTimeCallMinimumInterval:numberOfEventsForRateSmoothing:numberOfAveragesForRateSmoothing:")]
		NativeHandle Constructor (string clockName, NSUuid sourceIdentifier, CMTime getTimeCallMinimumInterval, uint numberOfEventsForRateSmoothing, uint numberOfAveragesForRateSmoothing);
		/// <summary>Creates a custom clock configuration with timing and rate-smoothing parameters.</summary>
		[Static]
		[Export ("customClockConfigurationWithClockName:sourceIdentifier:getTimeCallMinimumInterval:numberOfEventsForRateSmoothing:numberOfAveragesForRateSmoothing:")]
		CMIOExtensionStreamCustomClockConfiguration Create (string clockName, NSUuid sourceIdentifier, CMTime getTimeCallMinimumInterval, uint numberOfEventsForRateSmoothing, uint numberOfAveragesForRateSmoothing);
		/// <summary>Gets the clock's name.</summary>
		[Export ("clockName", ArgumentSemantic.Strong)]
		string ClockName { get; }
		/// <summary>Gets the clock source's identifier.</summary>
		[Export ("sourceIdentifier", ArgumentSemantic.Strong)]
		NSUuid SourceIdentifier { get; }
		/// <summary>Gets the minimum interval between time queries.</summary>
		[Export ("getTimeCallMinimumInterval")]
		CMTime GetTimeCallMinimumInterval { get; }
		/// <summary>Gets the number of timing events used for rate smoothing.</summary>
		[Export ("numberOfEventsForRateSmoothing")]
		uint NumberOfEventsForRateSmoothing { get; }
		/// <summary>Gets the number of averages used for rate smoothing.</summary>
		[Export ("numberOfAveragesForRateSmoothing")]
		uint NumberOfAveragesForRateSmoothing { get; }
	}

	/// <summary>A supported format and frame-duration range for a camera-extension stream.</summary>
	[NoiOS, NoTV, Mac (12, 3), MacCatalyst (15, 4)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface CMIOExtensionStreamFormat : NSCopying, NSSecureCoding {
		/// <summary>Creates a stream format. Valid frame durations are Core Media time dictionaries.</summary>
		[DesignatedInitializer]
		[Export ("initWithFormatDescription:maxFrameDuration:minFrameDuration:validFrameDurations:")]
		NativeHandle Constructor (CMFormatDescription formatDescription, CMTime maxFrameDuration, CMTime minFrameDuration, [NullAllowed] NSDictionary [] validFrameDurations);
		/// <summary>Creates a stream format. Valid frame durations are Core Media time dictionaries.</summary>
		[Static]
		[Export ("streamFormatWithFormatDescription:maxFrameDuration:minFrameDuration:validFrameDurations:")]
		CMIOExtensionStreamFormat Create (CMFormatDescription formatDescription, CMTime maxFrameDuration, CMTime minFrameDuration, [NullAllowed] NSDictionary [] validFrameDurations);
		/// <summary>Gets the media format description.</summary>
		[Export ("formatDescription", ArgumentSemantic.Strong)]
		CMFormatDescription FormatDescription { get; }
		/// <summary>Gets the minimum frame duration.</summary>
		[Export ("minFrameDuration")]
		CMTime MinFrameDuration { get; }
		/// <summary>Gets the maximum frame duration.</summary>
		[Export ("maxFrameDuration")]
		CMTime MaxFrameDuration { get; }
		/// <summary>Gets the valid frame durations as Core Media time dictionaries.</summary>
		[NullAllowed, Export ("validFrameDurations", ArgumentSemantic.Strong)]
		NSDictionary [] ValidFrameDurations { get; }
	}

	/// <summary>Describes when a camera-extension output buffer was scheduled.</summary>
	[NoiOS, NoTV, Mac (12, 3), MacCatalyst (15, 4)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface CMIOExtensionScheduledOutput : NSCopying, NSSecureCoding {
		/// <summary>Creates a scheduled-output description.</summary>
		[DesignatedInitializer]
		[Export ("initWithSequenceNumber:hostTimeInNanoseconds:")]
		NativeHandle Constructor (ulong sequenceNumber, ulong hostTimeInNanoseconds);
		/// <summary>Creates a scheduled-output description.</summary>
		[Static, Export ("scheduledOutputWithSequenceNumber:hostTimeInNanoseconds:")]
		CMIOExtensionScheduledOutput Create (ulong sequenceNumber, ulong hostTimeInNanoseconds);
		/// <summary>Gets the buffer's sequence number.</summary>
		[Export ("sequenceNumber")]
		ulong SequenceNumber { get; }
		/// <summary>Gets the host time in nanoseconds.</summary>
		[Export ("hostTimeInNanoseconds")]
		ulong HostTimeInNanoseconds { get; }
	}

	/// <summary>A client connected to a camera-extension provider.</summary>
	[NoiOS, NoTV, Mac (12, 3), MacCatalyst (15, 4)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface CMIOExtensionClient : NSCopying {
		/// <summary>Gets the client's identifier.</summary>
		[Export ("clientID", ArgumentSemantic.Copy)]
		NSUuid ClientId { get; }
		/// <summary>Gets the client's code-signing identifier.</summary>
		[Mac (13, 0), MacCatalyst (16, 1)]
		[NullAllowed, Export ("signingID", ArgumentSemantic.Copy)]
		string SigningId { get; }
		/// <summary>Gets the client's process identifier.</summary>
		[Export ("pid")]
		int Pid { get; }
	}

	/// <summary>A collection of camera-extension provider properties.</summary>
	[NoiOS, NoTV, Mac (12, 3), MacCatalyst (15, 4)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface CMIOExtensionProviderProperties {
		/// <summary>Creates provider properties from a dictionary of property states.</summary>
		[DesignatedInitializer, Export ("initWithDictionary:")]
		NativeHandle Constructor (NSDictionary<NSString, CMIOExtensionPropertyState> propertiesDictionary);
		/// <summary>Creates provider properties from a dictionary of property states.</summary>
		[Static, Export ("providerPropertiesWithDictionary:")]
		CMIOExtensionProviderProperties Create (NSDictionary<NSString, CMIOExtensionPropertyState> propertiesDictionary);
		/// <summary>Gets or sets the provider's name.</summary>
		[NullAllowed, Export ("name", ArgumentSemantic.Strong)]
		string Name { get; set; }
		/// <summary>Gets or sets the provider's manufacturer.</summary>
		[NullAllowed, Export ("manufacturer", ArgumentSemantic.Strong)]
		string Manufacturer { get; set; }
		/// <summary>Sets or removes a property's state.</summary>
		[Export ("setPropertyState:forProperty:")]
		void SetPropertyState ([NullAllowed] CMIOExtensionPropertyState propertyState, NSString property);
		/// <summary>Gets or sets the dictionary of property states.</summary>
		[Export ("propertiesDictionary", ArgumentSemantic.Copy)]
		NSDictionary<NSString, CMIOExtensionPropertyState> PropertiesDictionary { get; set; }
	}

	/// <summary>A collection of camera-extension device properties.</summary>
	[NoiOS, NoTV, Mac (12, 3), MacCatalyst (15, 4)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface CMIOExtensionDeviceProperties {
		/// <summary>Creates device properties from a dictionary of property states.</summary>
		[DesignatedInitializer, Export ("initWithDictionary:")]
		NativeHandle Constructor (NSDictionary<NSString, CMIOExtensionPropertyState> propertiesDictionary);
		/// <summary>Creates device properties from a dictionary of property states.</summary>
		[Static, Export ("devicePropertiesWithDictionary:")]
		CMIOExtensionDeviceProperties Create (NSDictionary<NSString, CMIOExtensionPropertyState> propertiesDictionary);
		/// <summary>Gets or sets the device's model.</summary>
		[NullAllowed, Export ("model", ArgumentSemantic.Strong)]
		string Model { get; set; }
		/// <summary>Gets or sets whether the device is suspended, or null if unspecified.</summary>
		[NullAllowed, Export ("suspended", ArgumentSemantic.Strong)]
		NSNumber Suspended { get; set; }
		/// <summary>Gets or sets the device's transport type, or null if unspecified.</summary>
		[NullAllowed, Export ("transportType", ArgumentSemantic.Strong)]
		NSNumber TransportType { get; set; }
		/// <summary>Gets or sets the linked Core Audio device's unique identifier.</summary>
		[NullAllowed, Export ("linkedCoreAudioDeviceUID", ArgumentSemantic.Strong)]
		string LinkedCoreAudioDeviceUid { get; set; }
		/// <summary>Sets or removes a property's state.</summary>
		[Export ("setPropertyState:forProperty:")]
		void SetPropertyState ([NullAllowed] CMIOExtensionPropertyState propertyState, NSString property);
		/// <summary>Gets or sets the dictionary of property states.</summary>
		[Export ("propertiesDictionary", ArgumentSemantic.Copy)]
		NSDictionary<NSString, CMIOExtensionPropertyState> PropertiesDictionary { get; set; }
	}

	/// <summary>A collection of camera-extension stream properties.</summary>
	[NoiOS, NoTV, Mac (12, 3), MacCatalyst (15, 4)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface CMIOExtensionStreamProperties {
		/// <summary>Creates stream properties from a dictionary of property states.</summary>
		[DesignatedInitializer, Export ("initWithDictionary:")]
		NativeHandle Constructor (NSDictionary<NSString, CMIOExtensionPropertyState> propertiesDictionary);
		/// <summary>Creates stream properties from a dictionary of property states.</summary>
		[Static, Export ("streamPropertiesWithDictionary:")]
		CMIOExtensionStreamProperties Create (NSDictionary<NSString, CMIOExtensionPropertyState> propertiesDictionary);
		/// <summary>Gets or sets the active format's index.</summary>
		[NullAllowed, Export ("activeFormatIndex", ArgumentSemantic.Strong)]
		NSNumber ActiveFormatIndex { get; set; }
		/// <summary>Gets or sets the frame duration as a Core Media time dictionary.</summary>
		[NullAllowed, Export ("frameDuration", ArgumentSemantic.Strong)]
		NSDictionary FrameDuration { get; set; }
		/// <summary>Gets or sets the maximum frame duration as a Core Media time dictionary.</summary>
		[NullAllowed, Export ("maxFrameDuration", ArgumentSemantic.Strong)]
		NSDictionary MaxFrameDuration { get; set; }
		/// <summary>Gets or sets the sink's buffer queue capacity.</summary>
		[NullAllowed, Export ("sinkBufferQueueSize", ArgumentSemantic.Strong)]
		NSNumber SinkBufferQueueSize { get; set; }
		/// <summary>Gets or sets the sink's required startup buffer count.</summary>
		[NullAllowed, Export ("sinkBuffersRequiredForStartup", ArgumentSemantic.Strong)]
		NSNumber SinkBuffersRequiredForStartup { get; set; }
		/// <summary>Gets or sets the sink's buffer underrun count.</summary>
		[NullAllowed, Export ("sinkBufferUnderrunCount", ArgumentSemantic.Strong)]
		NSNumber SinkBufferUnderrunCount { get; set; }
		/// <summary>Gets or sets whether the sink has reached the end of its data.</summary>
		[NullAllowed, Export ("sinkEndOfData", ArgumentSemantic.Strong)]
		NSNumber SinkEndOfData { get; set; }
		/// <summary>Sets or removes a property's state.</summary>
		[Export ("setPropertyState:forProperty:")]
		void SetPropertyState ([NullAllowed] CMIOExtensionPropertyState propertyState, NSString property);
		/// <summary>Gets or sets the dictionary of property states.</summary>
		[Export ("propertiesDictionary", ArgumentSemantic.Copy)]
		NSDictionary<NSString, CMIOExtensionPropertyState> PropertiesDictionary { get; set; }
	}

	/// <summary>Supplies devices and properties to a camera-extension provider.</summary>
	[NoiOS, NoTV, Mac (12, 3), MacCatalyst (15, 4)]
	[BaseType (typeof (NSObject))]
	[Protocol (BackwardsCompatibleCodeGeneration = false), Model]
	interface CMIOExtensionProviderSource {
		/// <summary>Connects a client to the provider.</summary>
		[Abstract, Export ("connectClient:error:")]
		bool ConnectClient (CMIOExtensionClient client, [NullAllowed] out NSError error);
		/// <summary>Disconnects a client from the provider.</summary>
		[Abstract, Export ("disconnectClient:")]
		void DisconnectClient (CMIOExtensionClient client);
		/// <summary>Gets the provider's supported property identifiers.</summary>
		[Abstract, Export ("availableProperties", ArgumentSemantic.Copy)]
		NSSet<NSString> AvailableProperties { get; }
		/// <summary>Gets the requested provider properties.</summary>
		[Abstract, Export ("providerPropertiesForProperties:error:")]
		[return: NullAllowed]
		CMIOExtensionProviderProperties GetProviderProperties (NSSet<NSString> properties, [NullAllowed] out NSError error);
		/// <summary>Sets the provider's properties.</summary>
		[Abstract, Export ("setProviderProperties:error:")]
		bool SetProviderProperties (CMIOExtensionProviderProperties providerProperties, [NullAllowed] out NSError error);
	}

	/// <summary>Supplies properties to a camera-extension device.</summary>
	[NoiOS, NoTV, Mac (12, 3), MacCatalyst (15, 4)]
	[BaseType (typeof (NSObject))]
	[Protocol (BackwardsCompatibleCodeGeneration = false), Model]
	interface CMIOExtensionDeviceSource {
		/// <summary>Gets the device's supported property identifiers.</summary>
		[Abstract, Export ("availableProperties", ArgumentSemantic.Copy)]
		NSSet<NSString> AvailableProperties { get; }
		/// <summary>Gets the requested device properties.</summary>
		[Abstract, Export ("devicePropertiesForProperties:error:")]
		[return: NullAllowed]
		CMIOExtensionDeviceProperties GetDeviceProperties (NSSet<NSString> properties, [NullAllowed] out NSError error);
		/// <summary>Sets the device's properties.</summary>
		[Abstract, Export ("setDeviceProperties:error:")]
		bool SetDeviceProperties (CMIOExtensionDeviceProperties deviceProperties, [NullAllowed] out NSError error);
	}

	/// <summary>Supplies formats, properties, and streaming behavior to a camera-extension stream.</summary>
	[NoiOS, NoTV, Mac (12, 3), MacCatalyst (15, 4)]
	[BaseType (typeof (NSObject))]
	[Protocol (BackwardsCompatibleCodeGeneration = false), Model]
	interface CMIOExtensionStreamSource {
		/// <summary>Gets the stream's supported formats.</summary>
		[Abstract, Export ("formats")]
		CMIOExtensionStreamFormat [] Formats { get; }
		/// <summary>Gets the stream's supported property identifiers.</summary>
		[Abstract, Export ("availableProperties", ArgumentSemantic.Copy)]
		NSSet<NSString> AvailableProperties { get; }
		/// <summary>Gets the requested stream properties.</summary>
		[Abstract, Export ("streamPropertiesForProperties:error:")]
		[return: NullAllowed]
		CMIOExtensionStreamProperties GetStreamProperties (NSSet<NSString> properties, [NullAllowed] out NSError error);
		/// <summary>Sets the stream's properties.</summary>
		[Abstract, Export ("setStreamProperties:error:")]
		bool SetStreamProperties (CMIOExtensionStreamProperties streamProperties, [NullAllowed] out NSError error);
		/// <summary>Determines whether a client may start streaming.</summary>
		[Abstract, Export ("authorizedToStartStreamForClient:")]
		bool IsAuthorizedToStartStream (CMIOExtensionClient client);
		/// <summary>Starts streaming.</summary>
		[Abstract, Export ("startStreamAndReturnError:")]
		bool StartStream ([NullAllowed] out NSError error);
		/// <summary>Stops streaming.</summary>
		[Abstract, Export ("stopStreamAndReturnError:")]
		bool StopStream ([NullAllowed] out NSError error);
	}

	/// <summary>A camera-extension provider and its connected devices.</summary>
	[NoiOS, NoTV, Mac (12, 3), MacCatalyst (15, 4)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface CMIOExtensionProvider {
		/// <summary>Starts the provider's extension service.</summary>
		[Static, Export ("startServiceWithProvider:")]
		void StartService (CMIOExtensionProvider provider);
		/// <summary>Stops the provider's extension service.</summary>
		[NoMacCatalyst, Mac (14, 4)]
		[Static, Export ("stopServiceWithProvider:")]
		void StopService (CMIOExtensionProvider provider);
		/// <summary>Configures the extension to ignore SIGTERM.</summary>
		[Mac (14, 0), MacCatalyst (17, 0)]
		[Static, Export ("ignoreSIGTERM")]
		void IgnoreSigterm ();
		/// <summary>Creates a provider. Keep the weakly referenced source alive while the provider uses it.</summary>
		[DesignatedInitializer, Export ("initWithSource:clientQueue:")]
		NativeHandle Constructor (ICMIOExtensionProviderSource source, [NullAllowed] DispatchQueue clientQueue);
		/// <summary>Creates a provider. Keep the weakly referenced source alive while the provider uses it.</summary>
		[Static, Export ("providerWithSource:clientQueue:")]
		CMIOExtensionProvider Create (ICMIOExtensionProviderSource source, [NullAllowed] DispatchQueue clientQueue);
		/// <summary>Gets the provider's weakly referenced source.</summary>
		[Export ("source", ArgumentSemantic.Weak)]
		ICMIOExtensionProviderSource Source { get; }
		/// <summary>Gets the queue used for client requests.</summary>
		[Export ("clientQueue", ArgumentSemantic.Strong)]
		DispatchQueue ClientQueue { get; }
		/// <summary>Gets the connected clients.</summary>
		[Export ("connectedClients", ArgumentSemantic.Copy)]
		CMIOExtensionClient [] ConnectedClients { get; }
		/// <summary>Gets the provider's devices.</summary>
		[Export ("devices", ArgumentSemantic.Copy)]
		CMIOExtensionDevice [] Devices { get; }
		/// <summary>Adds a device to the provider.</summary>
		[Export ("addDevice:error:")]
		bool AddDevice (CMIOExtensionDevice device, [NullAllowed] out NSError error);
		/// <summary>Removes a device from the provider.</summary>
		[Export ("removeDevice:error:")]
		bool RemoveDevice (CMIOExtensionDevice device, [NullAllowed] out NSError error);
		/// <summary>Notifies clients of changed provider properties.</summary>
		[Export ("notifyPropertiesChanged:")]
		void NotifyPropertiesChanged (NSDictionary<NSString, CMIOExtensionPropertyState> propertyStates);
	}

	/// <summary>A device exposed by a camera-extension provider.</summary>
	[NoiOS, NoTV, Mac (12, 3), MacCatalyst (15, 4)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface CMIOExtensionDevice {
		/// <summary>Creates a device. Keep the weakly referenced source alive while the device uses it.</summary>
		[DesignatedInitializer, Export ("initWithLocalizedName:deviceID:legacyDeviceID:source:")]
		NativeHandle Constructor (string localizedName, NSUuid deviceId, [NullAllowed] string legacyDeviceId, ICMIOExtensionDeviceSource source);
		/// <summary>Creates a device. Keep the weakly referenced source alive while the device uses it.</summary>
		[Static, Export ("deviceWithLocalizedName:deviceID:legacyDeviceID:source:")]
		CMIOExtensionDevice Create (string localizedName, NSUuid deviceId, [NullAllowed] string legacyDeviceId, ICMIOExtensionDeviceSource source);
		/// <summary>Creates a device without a legacy identifier.</summary>
		[NoMac, Export ("initWithLocalizedName:deviceID:source:")]
		NativeHandle Constructor (string localizedName, NSUuid deviceId, ICMIOExtensionDeviceSource source);
		/// <summary>Creates a device without a legacy identifier.</summary>
		[NoMac, Static, Export ("deviceWithLocalizedName:deviceID:source:")]
		CMIOExtensionDevice Create (string localizedName, NSUuid deviceId, ICMIOExtensionDeviceSource source);
		/// <summary>Gets the device's localized name.</summary>
		[Export ("localizedName", ArgumentSemantic.Copy)]
		string LocalizedName { get; }
		/// <summary>Gets the device's identifier.</summary>
		[Export ("deviceID", ArgumentSemantic.Copy)]
		NSUuid DeviceId { get; }
		/// <summary>Gets the device's legacy identifier.</summary>
		[Export ("legacyDeviceID", ArgumentSemantic.Copy)]
		string LegacyDeviceId { get; }
		/// <summary>Gets the device's weakly referenced source.</summary>
		[Export ("source", ArgumentSemantic.Weak)]
		ICMIOExtensionDeviceSource Source { get; }
		/// <summary>Gets the device's streams.</summary>
		[Export ("streams", ArgumentSemantic.Copy)]
		CMIOExtensionStream [] Streams { get; }
		/// <summary>Adds a stream to the device.</summary>
		[Export ("addStream:error:")]
		bool AddStream (CMIOExtensionStream stream, [NullAllowed] out NSError error);
		/// <summary>Removes a stream from the device.</summary>
		[Export ("removeStream:error:")]
		bool RemoveStream (CMIOExtensionStream stream, [NullAllowed] out NSError error);
		/// <summary>Notifies clients of changed device properties.</summary>
		[Export ("notifyPropertiesChanged:")]
		void NotifyPropertiesChanged (NSDictionary<NSString, CMIOExtensionPropertyState> propertyStates);
	}

	/// <summary>A source or sink stream exposed by a camera-extension device.</summary>
	[NoiOS, NoTV, Mac (12, 3), MacCatalyst (15, 4)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface CMIOExtensionStream {
		/// <summary>Creates a stream with a standard clock. Keep its weakly referenced source alive.</summary>
		[Export ("initWithLocalizedName:streamID:direction:clockType:source:")]
		NativeHandle Constructor (string localizedName, NSUuid streamId, CMIOExtensionStreamDirection direction, CMIOExtensionStreamClockType clockType, ICMIOExtensionStreamSource source);
		/// <summary>Creates a stream with a custom clock. Keep its weakly referenced source alive.</summary>
		[Export ("initWithLocalizedName:streamID:direction:customClockConfiguration:source:")]
		NativeHandle Constructor (string localizedName, NSUuid streamId, CMIOExtensionStreamDirection direction, CMIOExtensionStreamCustomClockConfiguration customClockConfiguration, ICMIOExtensionStreamSource source);
		/// <summary>Creates a stream with a standard clock. Keep its weakly referenced source alive.</summary>
		[Static, Export ("streamWithLocalizedName:streamID:direction:clockType:source:")]
		CMIOExtensionStream Create (string localizedName, NSUuid streamId, CMIOExtensionStreamDirection direction, CMIOExtensionStreamClockType clockType, ICMIOExtensionStreamSource source);
		/// <summary>Creates a stream with a custom clock. Keep its weakly referenced source alive.</summary>
		[Static, Export ("streamWithLocalizedName:streamID:direction:customClockConfiguration:source:")]
		CMIOExtensionStream Create (string localizedName, NSUuid streamId, CMIOExtensionStreamDirection direction, CMIOExtensionStreamCustomClockConfiguration customClockConfiguration, ICMIOExtensionStreamSource source);
		/// <summary>Gets the stream's localized name.</summary>
		[Export ("localizedName", ArgumentSemantic.Copy)]
		string LocalizedName { get; }
		/// <summary>Gets the stream's identifier.</summary>
		[Export ("streamID", ArgumentSemantic.Copy)]
		NSUuid StreamId { get; }
		/// <summary>Gets the stream's direction.</summary>
		[Export ("direction")]
		CMIOExtensionStreamDirection Direction { get; }
		/// <summary>Gets the clock type.</summary>
		[Export ("clockType")]
		CMIOExtensionStreamClockType ClockType { get; }
		/// <summary>Gets the custom clock configuration, if any.</summary>
		[NullAllowed, Export ("customClockConfiguration", ArgumentSemantic.Strong)]
		CMIOExtensionStreamCustomClockConfiguration CustomClockConfiguration { get; }
		/// <summary>Gets the stream's weakly referenced source.</summary>
		[Export ("source", ArgumentSemantic.Weak)]
		ICMIOExtensionStreamSource Source { get; }
		/// <summary>Gets the clients currently streaming.</summary>
		[Export ("streamingClients", ArgumentSemantic.Copy)]
		CMIOExtensionClient [] StreamingClients { get; }
		/// <summary>Notifies clients of changed stream properties.</summary>
		[Export ("notifyPropertiesChanged:")]
		void NotifyPropertiesChanged (NSDictionary<NSString, CMIOExtensionPropertyState> propertyStates);
		/// <summary>Sends a sample buffer to clients.</summary>
		[Export ("sendSampleBuffer:discontinuity:hostTimeInNanoseconds:")]
		void SendSampleBuffer (CMSampleBuffer sampleBuffer, CMIOExtensionStreamDiscontinuityFlags discontinuity, ulong hostTimeInNanoseconds);
		/// <summary>Consumes a sample buffer from a sink client.</summary>
		[Export ("consumeSampleBufferFromClient:completionHandler:")]
		void ConsumeSampleBuffer (CMIOExtensionClient client, CMIOExtensionStreamConsumeSampleBufferHandler completionHandler);
		/// <summary>Notifies clients that scheduled output has changed.</summary>
		[Export ("notifyScheduledOutputChanged:")]
		void NotifyScheduledOutputChanged (CMIOExtensionScheduledOutput scheduledOutput);
	}

	/// <summary>Attachment keys used by Core Media I/O sample buffers.</summary>
	[NoiOS, NoTV, MacCatalyst (13, 1)]
	[Static]
	interface CMIOSampleBufferAttachmentKey {
		/// <summary>The sample buffer's sequence number.</summary>
		[Field ("kCMIOSampleBufferAttachmentKey_SequenceNumber")]
		NSString SequenceNumber { get; }

		/// <summary>The host time associated with the sample buffer.</summary>
		[Field ("kCMIOSampleBufferAttachmentKey_HostTime")]
		NSString HostTime { get; }

		/// <summary>Flags describing discontinuities in the stream.</summary>
		[Field ("kCMIOSampleBufferAttachmentKey_DiscontinuityFlags")]
		NSString DiscontinuityFlags { get; }

		/// <summary>HDV1 pack data.</summary>
		[Field ("kCMIOSampleBufferAttachmentKey_HDV1_PackData")]
		NSString Hdv1PackData { get; }
		/// <summary>HDV2 video auxiliary data.</summary>
		[Field ("kCMIOSampleBufferAttachmentKey_HDV2_VAUX")]
		NSString Hdv2Vaux { get; }
		/// <summary>The Core Audio timestamp.</summary>
		[Field ("kCMIOSampleBufferAttachmentKey_CAAudioTimeStamp")]
		NSString AudioTimeStamp { get; }
		/// <summary>The SMPTE time.</summary>
		[Field ("kCMIOSampleBufferAttachmentKey_SMPTETime")]
		NSString SmpteTime { get; }
		/// <summary>The native SMPTE frame count.</summary>
		[Field ("kCMIOSampleBufferAttachmentKey_NativeSMPTEFrameCount")]
		NSString NativeSmpteFrameCount { get; }
		/// <summary>The number of video frames in the buffer.</summary>
		[Field ("kCMIOSampleBufferAttachmentKey_NumberOfVideoFramesInBuffer")]
		NSString NumberOfVideoFramesInBuffer { get; }
		/// <summary>The number of video frames in the group of pictures.</summary>
		[Field ("kCMIOSampleBufferAttachmentKey_NumberOfVideoFramesInGOP")]
		NSString NumberOfVideoFramesInGop { get; }
		/// <summary>The source presentation timestamp of multiplexed data.</summary>
		[Field ("kCMIOSampleBufferAttachmentKey_MuxedSourcePresentationTimeStamp")]
		NSString MuxedSourcePresentationTimeStamp { get; }
		/// <summary>Whether the buffer repeats previous contents.</summary>
		[Field ("kCMIOSampleBufferAttachmentKey_RepeatedBufferContents")]
		NSString RepeatedBufferContents { get; }
		/// <summary>The source audio format description.</summary>
		[Field ("kCMIOSampleBufferAttachmentKey_SourceAudioFormatDescription")]
		NSString SourceAudioFormatDescription { get; }
		/// <summary>Pulldown cadence information.</summary>
		[Field ("kCMIOSampleBufferAttachmentKey_PulldownCadenceInfo")]
		NSString PulldownCadenceInfo { get; }
		/// <summary>A sample buffer containing closed captions.</summary>
		[Field ("kCMIOSampleBufferAttachmentKey_ClosedCaptionSampleBuffer")]
		NSString ClosedCaptionSampleBuffer { get; }
		/// <summary>The client's sequence identifier.</summary>
		[Field ("kCMIOSampleBufferAttachmentKey_ClientSequenceID")]
		NSString ClientSequenceId { get; }
		/// <summary>Mouse and keyboard modifier information.</summary>
		[Field ("kCMIOSampleBufferAttachmentKey_MouseAndKeyboardModifiers")]
		NSString MouseAndKeyboardModifiers { get; }
		/// <summary>Whether a static image overlays the pixel buffer.</summary>
		[Mac (14, 0), MacCatalyst (13, 1)]
		[Field ("kCMIOSampleBufferAttachmentKey_PixelBufferOverlaidByStaticImage")]
		NSString PixelBufferOverlaidByStaticImage { get; }
		/// <summary>The reason no media data is available.</summary>
		[Field ("kCMIOSampleBufferAttachmentKey_NoDataMarker")]
		NSString NoDataMarker { get; }
	}

	/// <summary>Keys in a sample buffer's mouse and keyboard modifier dictionary.</summary>
	[NoiOS, NoTV, MacCatalyst (13, 1)]
	[Static]
	interface CMIOMouseAndKeyboardModifiersKey {
		/// <summary>The cursor's horizontal position.</summary>
		[Field ("kCMIOSampleBufferAttachment_MouseAndKeyboardModifiersKey_CursorPositionX")]
		NSString CursorPositionX { get; }
		/// <summary>The cursor's vertical position.</summary>
		[Field ("kCMIOSampleBufferAttachment_MouseAndKeyboardModifiersKey_CursorPositionY")]
		NSString CursorPositionY { get; }
		/// <summary>The mouse button state.</summary>
		[Field ("kCMIOSampleBufferAttachment_MouseAndKeyboardModifiersKey_MouseButtonState")]
		NSString MouseButtonState { get; }
		/// <summary>Whether the cursor is visible.</summary>
		[Field ("kCMIOSampleBufferAttachment_MouseAndKeyboardModifiersKey_CursorIsVisible")]
		NSString CursorIsVisible { get; }
		/// <summary>The cursor's frame rectangle.</summary>
		[Field ("kCMIOSampleBufferAttachment_MouseAndKeyboardModifiersKey_CursorFrameRect")]
		NSString CursorFrameRect { get; }
		/// <summary>A reference to the cursor image.</summary>
		[Field ("kCMIOSampleBufferAttachment_MouseAndKeyboardModifiersKey_CursorReference")]
		NSString CursorReference { get; }
		/// <summary>The cursor's change seed.</summary>
		[Field ("kCMIOSampleBufferAttachment_MouseAndKeyboardModifiersKey_CursorSeed")]
		NSString CursorSeed { get; }
		/// <summary>The cursor's scale.</summary>
		[Field ("kCMIOSampleBufferAttachment_MouseAndKeyboardModifiersKey_CursorScale")]
		NSString CursorScale { get; }
		/// <summary>Whether the cursor is already drawn in the framebuffer.</summary>
		[Field ("kCMIOSampleBufferAttachment_MouseAndKeyboardModifiersKey_CursorIsDrawnInFramebuffer")]
		NSString CursorIsDrawnInFramebuffer { get; }
		/// <summary>The active keyboard modifiers.</summary>
		[Field ("kCMIOSampleBufferAttachment_MouseAndKeyboardModifiersKey_KeyboardModifiers")]
		NSString KeyboardModifiers { get; }
		/// <summary>The keyboard modifier event.</summary>
		[Field ("kCMIOSampleBufferAttachment_MouseAndKeyboardModifiersKey_KeyboardModifiersEvent")]
		NSString KeyboardModifiersEvent { get; }
	}

	/// <summary>Attachment keys used by Core Media I/O block buffers.</summary>
	[NoiOS, NoTV, MacCatalyst (13, 1)]
	[Static]
	interface CMIOBlockBufferAttachmentKey {
		/// <summary>The Core Video pixel buffer referenced by the block buffer.</summary>
		[Field ("kCMIOBlockBufferAttachmentKey_CVPixelBufferReference")]
		NSString PixelBufferReference { get; }
	}
}
