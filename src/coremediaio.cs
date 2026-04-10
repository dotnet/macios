//
// coremediaio.cs: Definitions for CoreMediaIO
//
// Authors:
//   GitHub Copilot
//

using System;
using AVFoundation;
using CoreFoundation;
using CoreMedia;
using Foundation;
using ObjCRuntime;

#if !NET
using NativeHandle = System.IntPtr;
#endif

#nullable enable

namespace CoreMediaIO {

	/// <summary>Represents the attributes of a CoreMediaIO extension property, including min/max values, valid values, and read-only state.</summary>
	[NoiOS, NoTV, Mac (12, 3), MacCatalyst (15, 4)]
	[DisableDefaultCtor]
	[BaseType (typeof (NSObject))]
	interface CMIOExtensionPropertyAttributes : NSCopying, NSSecureCoding {

		[Static]
		[Export ("propertyAttributesWithMinValue:maxValue:validValues:readOnly:")]
		CMIOExtensionPropertyAttributes Create ([NullAllowed] NSObject minValue, [NullAllowed] NSObject maxValue, [NullAllowed] NSObject [] validValues, bool readOnly);

		[Export ("initWithMinValue:maxValue:validValues:readOnly:")]
		[DesignatedInitializer]
		NativeHandle Constructor ([NullAllowed] NSObject minValue, [NullAllowed] NSObject maxValue, [NullAllowed] NSObject [] validValues, bool readOnly);

		[Static]
		[Export ("readOnlyPropertyAttribute")]
		CMIOExtensionPropertyAttributes ReadOnlyPropertyAttribute { get; }

		[NullAllowed]
		[Export ("minValue", ArgumentSemantic.Copy)]
		NSObject MinValue { get; }

		[NullAllowed]
		[Export ("maxValue", ArgumentSemantic.Copy)]
		NSObject MaxValue { get; }

		[NullAllowed]
		[Export ("validValues", ArgumentSemantic.Copy)]
		NSObject [] ValidValues { get; }

		[Export ("isReadOnly")]
		bool IsReadOnly { get; }
	}

	/// <summary>Represents the state of a CoreMediaIO extension property, including its value and optional attributes.</summary>
	[NoiOS, NoTV, Mac (12, 3), MacCatalyst (15, 4)]
	[DisableDefaultCtor]
	[BaseType (typeof (NSObject))]
	interface CMIOExtensionPropertyState : NSCopying, NSSecureCoding {

		[Static]
		[Export ("propertyStateWithValue:")]
		CMIOExtensionPropertyState Create ([NullAllowed] NSObject value);

		[Static]
		[Export ("propertyStateWithValue:attributes:")]
		CMIOExtensionPropertyState Create ([NullAllowed] NSObject value, [NullAllowed] CMIOExtensionPropertyAttributes attributes);

		[Export ("initWithValue:")]
		NativeHandle Constructor ([NullAllowed] NSObject value);

		[Export ("initWithValue:attributes:")]
		[DesignatedInitializer]
		NativeHandle Constructor ([NullAllowed] NSObject value, [NullAllowed] CMIOExtensionPropertyAttributes attributes);

		[NullAllowed]
		[Export ("value", ArgumentSemantic.Copy)]
		NSObject Value { get; }

		[NullAllowed]
		[Export ("attributes", ArgumentSemantic.Strong)]
		CMIOExtensionPropertyAttributes Attributes { get; }
	}

	/// <summary>Defines the configuration for a custom clock used by a CoreMediaIO extension stream.</summary>
	[NoiOS, NoTV, Mac (12, 3), MacCatalyst (15, 4)]
	[DisableDefaultCtor]
	[BaseType (typeof (NSObject))]
	interface CMIOExtensionStreamCustomClockConfiguration : NSCopying, NSSecureCoding {

		[Static]
		[Export ("customClockConfigurationWithClockName:sourceIdentifier:getTimeCallMinimumInterval:numberOfEventsForRateSmoothing:numberOfAveragesForRateSmoothing:")]
		CMIOExtensionStreamCustomClockConfiguration Create (string clockName, NSUuid sourceIdentifier, CMTime getTimeCallMinimumInterval, uint numberOfEventsForRateSmoothing, uint numberOfAveragesForRateSmoothing);

		[Export ("initWithClockName:sourceIdentifier:getTimeCallMinimumInterval:numberOfEventsForRateSmoothing:numberOfAveragesForRateSmoothing:")]
		[DesignatedInitializer]
		NativeHandle Constructor (string clockName, NSUuid sourceIdentifier, CMTime getTimeCallMinimumInterval, uint numberOfEventsForRateSmoothing, uint numberOfAveragesForRateSmoothing);

		[Export ("clockName", ArgumentSemantic.Strong)]
		string ClockName { get; }

		[Export ("sourceIdentifier", ArgumentSemantic.Strong)]
		NSUuid SourceIdentifier { get; }

		[Export ("getTimeCallMinimumInterval")]
		CMTime GetTimeCallMinimumInterval { get; }

		[Export ("numberOfEventsForRateSmoothing")]
		uint NumberOfEventsForRateSmoothing { get; }

		[Export ("numberOfAveragesForRateSmoothing")]
		uint NumberOfAveragesForRateSmoothing { get; }
	}

	/// <summary>Describes a stream format for a CoreMediaIO extension stream, including format description, frame durations, and valid frame durations.</summary>
	[NoiOS, NoTV, Mac (12, 3), MacCatalyst (15, 4)]
	[DisableDefaultCtor]
	[BaseType (typeof (NSObject))]
	interface CMIOExtensionStreamFormat : NSCopying, NSSecureCoding {

		[Static]
		[Export ("streamFormatWithFormatDescription:maxFrameDuration:minFrameDuration:validFrameDurations:")]
		CMIOExtensionStreamFormat Create (CMFormatDescription formatDescription, CMTime maxFrameDuration, CMTime minFrameDuration, [NullAllowed] NSDictionary [] validFrameDurations);

		[Export ("initWithFormatDescription:maxFrameDuration:minFrameDuration:validFrameDurations:")]
		[DesignatedInitializer]
		NativeHandle Constructor (CMFormatDescription formatDescription, CMTime maxFrameDuration, CMTime minFrameDuration, [NullAllowed] NSDictionary [] validFrameDurations);

		[Export ("formatDescription", ArgumentSemantic.Strong)]
		CMFormatDescription FormatDescription { get; }

		[Export ("minFrameDuration")]
		CMTime MinFrameDuration { get; }

		[Export ("maxFrameDuration")]
		CMTime MaxFrameDuration { get; }

		[NullAllowed]
		[Export ("validFrameDurations", ArgumentSemantic.Strong)]
		NSDictionary [] ValidFrameDurations { get; }
	}

	/// <summary>Represents scheduled output information for a CoreMediaIO extension stream, including a sequence number and host time.</summary>
	[NoiOS, NoTV, Mac (12, 3), MacCatalyst (15, 4)]
	[DisableDefaultCtor]
	[BaseType (typeof (NSObject))]
	interface CMIOExtensionScheduledOutput : NSCopying, NSSecureCoding {

		[Static]
		[Export ("scheduledOutputWithSequenceNumber:hostTimeInNanoseconds:")]
		CMIOExtensionScheduledOutput Create (ulong sequenceNumber, ulong hostTimeInNanoseconds);

		[Export ("initWithSequenceNumber:hostTimeInNanoseconds:")]
		[DesignatedInitializer]
		NativeHandle Constructor (ulong sequenceNumber, ulong hostTimeInNanoseconds);

		[Export ("sequenceNumber")]
		ulong SequenceNumber { get; }

		[Export ("hostTimeInNanoseconds")]
		ulong HostTimeInNanoseconds { get; }
	}

	/// <summary>Represents a client connected to a CoreMediaIO extension provider.</summary>
	[NoiOS, NoTV, Mac (12, 3), MacCatalyst (15, 4)]
	[DisableDefaultCtor]
	[BaseType (typeof (NSObject))]
	interface CMIOExtensionClient : NSCopying {

		[Export ("clientID", ArgumentSemantic.Copy)]
		NSUuid ClientId { get; }

		[NoiOS, NoTV, Mac (13, 0), MacCatalyst (16, 0)]
		[NullAllowed]
		[Export ("signingID", ArgumentSemantic.Copy)]
		string SigningId { get; }

		[Export ("pid")]
		int Pid { get; }
	}

	/// <summary>Represents the properties of a CoreMediaIO extension stream.</summary>
	[NoiOS, NoTV, Mac (12, 3), MacCatalyst (15, 4)]
	[DisableDefaultCtor]
	[BaseType (typeof (NSObject))]
	interface CMIOExtensionStreamProperties {

		[Static]
		[Export ("streamPropertiesWithDictionary:")]
		CMIOExtensionStreamProperties Create (NSDictionary<NSString, CMIOExtensionPropertyState> propertiesDictionary);

		[Export ("initWithDictionary:")]
		[DesignatedInitializer]
		NativeHandle Constructor (NSDictionary<NSString, CMIOExtensionPropertyState> propertiesDictionary);

		[NullAllowed]
		[Export ("activeFormatIndex", ArgumentSemantic.Strong)]
		NSNumber ActiveFormatIndex { get; set; }

		[NullAllowed]
		[Export ("frameDuration", ArgumentSemantic.Strong)]
		NSDictionary FrameDuration { get; set; }

		[NullAllowed]
		[Export ("maxFrameDuration", ArgumentSemantic.Strong)]
		NSDictionary MaxFrameDuration { get; set; }

		[NullAllowed]
		[Export ("sinkBufferQueueSize", ArgumentSemantic.Strong)]
		NSNumber SinkBufferQueueSize { get; set; }

		[NullAllowed]
		[Export ("sinkBuffersRequiredForStartup", ArgumentSemantic.Strong)]
		NSNumber SinkBuffersRequiredForStartup { get; set; }

		[NullAllowed]
		[Export ("sinkBufferUnderrunCount", ArgumentSemantic.Strong)]
		NSNumber SinkBufferUnderrunCount { get; set; }

		[NullAllowed]
		[Export ("sinkEndOfData", ArgumentSemantic.Strong)]
		NSNumber SinkEndOfData { get; set; }

		[Export ("setPropertyState:forProperty:")]
		void SetPropertyState ([NullAllowed] CMIOExtensionPropertyState propertyState, NSString property);

		[Export ("propertiesDictionary", ArgumentSemantic.Copy)]
		NSDictionary<NSString, CMIOExtensionPropertyState> PropertiesDictionary { get; set; }
	}

	/// <summary>A protocol that defines the source of data and properties for a CoreMediaIO extension stream.</summary>
	interface ICMIOExtensionStreamSource { }

	/// <summary>A protocol that defines the source of data and properties for a CoreMediaIO extension stream.</summary>
	[NoiOS, NoTV, Mac (12, 3), MacCatalyst (15, 4)]
	[Protocol (BackwardsCompatibleCodeGeneration = false)]
	interface CMIOExtensionStreamSource {

		[Abstract]
		[Export ("formats")]
		CMIOExtensionStreamFormat [] Formats { get; }

		[Abstract]
		[Export ("availableProperties", ArgumentSemantic.Copy)]
		NSSet<NSString> AvailableProperties { get; }

		[Abstract]
		[Export ("streamPropertiesForProperties:error:")]
		[return: NullAllowed]
		CMIOExtensionStreamProperties GetStreamProperties (NSSet<NSString> properties, [NullAllowed] out NSError outError);

		[Abstract]
		[Export ("setStreamProperties:error:")]
		bool SetStreamProperties (CMIOExtensionStreamProperties streamProperties, [NullAllowed] out NSError outError);

		[Abstract]
		[Export ("authorizedToStartStreamForClient:")]
		bool IsAuthorizedToStartStream (CMIOExtensionClient client);

		[Abstract]
		[Export ("startStreamAndReturnError:")]
		bool StartStream ([NullAllowed] out NSError outError);

		[Abstract]
		[Export ("stopStreamAndReturnError:")]
		bool StopStream ([NullAllowed] out NSError outError);
	}

	/// <summary>Represents a CoreMediaIO extension stream that provides or consumes media data.</summary>
	[NoiOS, NoTV, Mac (12, 3), MacCatalyst (15, 4)]
	[DisableDefaultCtor]
	[BaseType (typeof (NSObject))]
	interface CMIOExtensionStream {

		[Static]
		[Export ("streamWithLocalizedName:streamID:direction:clockType:source:")]
		CMIOExtensionStream Create (string localizedName, NSUuid streamId, CMIOExtensionStreamDirection direction, CMIOExtensionStreamClockType clockType, ICMIOExtensionStreamSource source);

		[Static]
		[Export ("streamWithLocalizedName:streamID:direction:customClockConfiguration:source:")]
		CMIOExtensionStream Create (string localizedName, NSUuid streamId, CMIOExtensionStreamDirection direction, CMIOExtensionStreamCustomClockConfiguration customClockConfiguration, ICMIOExtensionStreamSource source);

		[Export ("initWithLocalizedName:streamID:direction:clockType:source:")]
		NativeHandle Constructor (string localizedName, NSUuid streamId, CMIOExtensionStreamDirection direction, CMIOExtensionStreamClockType clockType, ICMIOExtensionStreamSource source);

		[Export ("initWithLocalizedName:streamID:direction:customClockConfiguration:source:")]
		NativeHandle Constructor (string localizedName, NSUuid streamId, CMIOExtensionStreamDirection direction, CMIOExtensionStreamCustomClockConfiguration customClockConfiguration, ICMIOExtensionStreamSource source);

		[Export ("localizedName", ArgumentSemantic.Copy)]
		string LocalizedName { get; }

		[Export ("streamID", ArgumentSemantic.Copy)]
		NSUuid StreamId { get; }

		[Export ("direction")]
		CMIOExtensionStreamDirection Direction { get; }

		[Export ("clockType")]
		CMIOExtensionStreamClockType ClockType { get; }

		[NullAllowed]
		[Export ("customClockConfiguration", ArgumentSemantic.Strong)]
		CMIOExtensionStreamCustomClockConfiguration CustomClockConfiguration { get; }

		[NullAllowed]
		[Export ("source", ArgumentSemantic.Weak)]
		ICMIOExtensionStreamSource Source { get; }

		[Export ("streamingClients", ArgumentSemantic.Copy)]
		CMIOExtensionClient [] StreamingClients { get; }

		[Export ("notifyPropertiesChanged:")]
		void NotifyPropertiesChanged (NSDictionary<NSString, CMIOExtensionPropertyState> propertyStates);

		[Export ("sendSampleBuffer:discontinuity:hostTimeInNanoseconds:")]
		void SendSampleBuffer (CMSampleBuffer sampleBuffer, CMIOExtensionStreamDiscontinuityFlags discontinuity, ulong hostTimeInNanoseconds);

		[Export ("consumeSampleBufferFromClient:completionHandler:")]
		[Async (ResultTypeName = "CMIOExtensionStreamConsumeResult")]
		void ConsumeSampleBuffer (CMIOExtensionClient client, CMIOExtensionStreamConsumeHandler completionHandler);

		[Export ("notifyScheduledOutputChanged:")]
		void NotifyScheduledOutputChanged (CMIOExtensionScheduledOutput scheduledOutput);
	}

	/// <summary>Completion handler for consuming a sample buffer from a CoreMediaIO extension stream client.</summary>
	delegate void CMIOExtensionStreamConsumeHandler ([NullAllowed] CMSampleBuffer sampleBuffer, ulong sampleBufferSequenceNumber, CMIOExtensionStreamDiscontinuityFlags discontinuity, bool hasMoreSampleBuffers, [NullAllowed] NSError error);

	/// <summary>Represents the properties of a CoreMediaIO extension device.</summary>
	[NoiOS, NoTV, Mac (12, 3), MacCatalyst (15, 4)]
	[DisableDefaultCtor]
	[BaseType (typeof (NSObject))]
	interface CMIOExtensionDeviceProperties {

		[Static]
		[Export ("devicePropertiesWithDictionary:")]
		CMIOExtensionDeviceProperties Create (NSDictionary<NSString, CMIOExtensionPropertyState> propertiesDictionary);

		[Export ("initWithDictionary:")]
		[DesignatedInitializer]
		NativeHandle Constructor (NSDictionary<NSString, CMIOExtensionPropertyState> propertiesDictionary);

		[NullAllowed]
		[Export ("model", ArgumentSemantic.Strong)]
		string Model { get; set; }

		[NullAllowed]
		[Export ("suspended", ArgumentSemantic.Strong)]
		NSNumber Suspended { get; set; }

		[NullAllowed]
		[Export ("transportType", ArgumentSemantic.Strong)]
		NSNumber TransportType { get; set; }

		[NullAllowed]
		[Export ("linkedCoreAudioDeviceUID", ArgumentSemantic.Strong)]
		string LinkedCoreAudioDeviceUid { get; set; }

		[Export ("setPropertyState:forProperty:")]
		void SetPropertyState ([NullAllowed] CMIOExtensionPropertyState propertyState, NSString property);

		[Export ("propertiesDictionary", ArgumentSemantic.Copy)]
		NSDictionary<NSString, CMIOExtensionPropertyState> PropertiesDictionary { get; set; }
	}

	/// <summary>A protocol that defines the source of data and properties for a CoreMediaIO extension device.</summary>
	interface ICMIOExtensionDeviceSource { }

	/// <summary>A protocol that defines the source of data and properties for a CoreMediaIO extension device.</summary>
	[NoiOS, NoTV, Mac (12, 3), MacCatalyst (15, 4)]
	[Protocol (BackwardsCompatibleCodeGeneration = false)]
	interface CMIOExtensionDeviceSource {

		[Abstract]
		[Export ("availableProperties", ArgumentSemantic.Copy)]
		NSSet<NSString> AvailableProperties { get; }

		[Abstract]
		[Export ("devicePropertiesForProperties:error:")]
		[return: NullAllowed]
		CMIOExtensionDeviceProperties GetDeviceProperties (NSSet<NSString> properties, [NullAllowed] out NSError outError);

		[Abstract]
		[Export ("setDeviceProperties:error:")]
		bool SetDeviceProperties (CMIOExtensionDeviceProperties deviceProperties, [NullAllowed] out NSError outError);
	}

	/// <summary>Represents a CoreMediaIO extension device that contains one or more streams.</summary>
	[NoiOS, NoTV, Mac (12, 3), MacCatalyst (15, 4)]
	[DisableDefaultCtor]
	[BaseType (typeof (NSObject))]
	interface CMIOExtensionDevice {

		[Static]
		[Export ("deviceWithLocalizedName:deviceID:legacyDeviceID:source:")]
		CMIOExtensionDevice Create (string localizedName, NSUuid deviceId, [NullAllowed] string legacyDeviceId, ICMIOExtensionDeviceSource source);

		[Export ("initWithLocalizedName:deviceID:legacyDeviceID:source:")]
		[DesignatedInitializer]
		NativeHandle Constructor (string localizedName, NSUuid deviceId, [NullAllowed] string legacyDeviceId, ICMIOExtensionDeviceSource source);

		[NoiOS, NoTV, NoMac, MacCatalyst (15, 4)]
		[Static]
		[Export ("deviceWithLocalizedName:deviceID:source:")]
		CMIOExtensionDevice Create (string localizedName, NSUuid deviceId, ICMIOExtensionDeviceSource source);

		[NoiOS, NoTV, NoMac, MacCatalyst (15, 4)]
		[Export ("initWithLocalizedName:deviceID:source:")]
		NativeHandle Constructor (string localizedName, NSUuid deviceId, ICMIOExtensionDeviceSource source);

		[Export ("localizedName", ArgumentSemantic.Copy)]
		string LocalizedName { get; }

		[Export ("deviceID", ArgumentSemantic.Copy)]
		NSUuid DeviceId { get; }

		[Export ("legacyDeviceID", ArgumentSemantic.Copy)]
		string LegacyDeviceId { get; }

		[NullAllowed]
		[Export ("source", ArgumentSemantic.Weak)]
		ICMIOExtensionDeviceSource Source { get; }

		[Export ("streams", ArgumentSemantic.Copy)]
		CMIOExtensionStream [] Streams { get; }

		[Export ("addStream:error:")]
		bool AddStream (CMIOExtensionStream stream, [NullAllowed] out NSError outError);

		[Export ("removeStream:error:")]
		bool RemoveStream (CMIOExtensionStream stream, [NullAllowed] out NSError outError);

		[Export ("notifyPropertiesChanged:")]
		void NotifyPropertiesChanged (NSDictionary<NSString, CMIOExtensionPropertyState> propertyStates);
	}

	/// <summary>Represents the properties of a CoreMediaIO extension provider.</summary>
	[NoiOS, NoTV, Mac (12, 3), MacCatalyst (15, 4)]
	[DisableDefaultCtor]
	[BaseType (typeof (NSObject))]
	interface CMIOExtensionProviderProperties {

		[Static]
		[Export ("providerPropertiesWithDictionary:")]
		CMIOExtensionProviderProperties Create (NSDictionary<NSString, CMIOExtensionPropertyState> propertiesDictionary);

		[Export ("initWithDictionary:")]
		[DesignatedInitializer]
		NativeHandle Constructor (NSDictionary<NSString, CMIOExtensionPropertyState> propertiesDictionary);

		[NullAllowed]
		[Export ("name", ArgumentSemantic.Strong)]
		string Name { get; set; }

		[NullAllowed]
		[Export ("manufacturer", ArgumentSemantic.Strong)]
		string Manufacturer { get; set; }

		[Export ("setPropertyState:forProperty:")]
		void SetPropertyState ([NullAllowed] CMIOExtensionPropertyState propertyState, NSString property);

		[Export ("propertiesDictionary", ArgumentSemantic.Copy)]
		NSDictionary<NSString, CMIOExtensionPropertyState> PropertiesDictionary { get; set; }
	}

	/// <summary>A protocol that defines the source for a CoreMediaIO extension provider.</summary>
	interface ICMIOExtensionProviderSource { }

	/// <summary>A protocol that defines the source for a CoreMediaIO extension provider.</summary>
	[NoiOS, NoTV, Mac (12, 3), MacCatalyst (15, 4)]
	[Protocol (BackwardsCompatibleCodeGeneration = false)]
	interface CMIOExtensionProviderSource {

		[Abstract]
		[Export ("connectClient:error:")]
		bool ConnectClient (CMIOExtensionClient client, [NullAllowed] out NSError outError);

		[Abstract]
		[Export ("disconnectClient:")]
		void DisconnectClient (CMIOExtensionClient client);

		[Abstract]
		[Export ("availableProperties", ArgumentSemantic.Copy)]
		NSSet<NSString> AvailableProperties { get; }

		[Abstract]
		[Export ("providerPropertiesForProperties:error:")]
		[return: NullAllowed]
		CMIOExtensionProviderProperties GetProviderProperties (NSSet<NSString> properties, [NullAllowed] out NSError outError);

		[Abstract]
		[Export ("setProviderProperties:error:")]
		bool SetProviderProperties (CMIOExtensionProviderProperties providerProperties, [NullAllowed] out NSError outError);
	}

	/// <summary>Represents a CoreMediaIO extension provider that manages devices and client connections.</summary>
	[NoiOS, NoTV, Mac (12, 3), MacCatalyst (15, 4)]
	[DisableDefaultCtor]
	[BaseType (typeof (NSObject))]
	interface CMIOExtensionProvider {

		[Static]
		[Export ("startServiceWithProvider:")]
		void StartService (CMIOExtensionProvider provider);

		[NoiOS, NoTV, NoMacCatalyst, Mac (14, 4)]
		[Static]
		[Export ("stopServiceWithProvider:")]
		void StopService (CMIOExtensionProvider provider);

		[Static]
		[Export ("providerWithSource:clientQueue:")]
		CMIOExtensionProvider Create (ICMIOExtensionProviderSource source, [NullAllowed] DispatchQueue clientQueue);

		[Export ("initWithSource:clientQueue:")]
		[DesignatedInitializer]
		NativeHandle Constructor (ICMIOExtensionProviderSource source, [NullAllowed] DispatchQueue clientQueue);

		[NullAllowed]
		[Export ("source", ArgumentSemantic.Weak)]
		ICMIOExtensionProviderSource Source { get; }

		[Export ("clientQueue", ArgumentSemantic.Strong)]
		DispatchQueue ClientQueue { get; }

		[Export ("connectedClients", ArgumentSemantic.Copy)]
		CMIOExtensionClient [] ConnectedClients { get; }

		[Export ("devices", ArgumentSemantic.Copy)]
		CMIOExtensionDevice [] Devices { get; }

		[Export ("addDevice:error:")]
		bool AddDevice (CMIOExtensionDevice device, [NullAllowed] out NSError outError);

		[Export ("removeDevice:error:")]
		bool RemoveDevice (CMIOExtensionDevice device, [NullAllowed] out NSError outError);

		[Export ("notifyPropertiesChanged:")]
		void NotifyPropertiesChanged (NSDictionary<NSString, CMIOExtensionPropertyState> propertyStates);

		[NoiOS, NoTV, Mac (14, 0), MacCatalyst (17, 0)]
		[Static]
		[Export ("ignoreSIGTERM")]
		void IgnoreSigterm ();
	}

	/// <summary>Provides CoreMediaIO extension property key constants.</summary>
	[NoiOS, NoTV, Mac (12, 3), MacCatalyst (15, 4)]
	[Static]
	[Partial]
	interface CMIOExtensionPropertyKeys {

		[Field ("CMIOExtensionPropertyProviderName")]
		NSString ProviderName { get; }

		[Field ("CMIOExtensionPropertyProviderManufacturer")]
		NSString ProviderManufacturer { get; }

		[Field ("CMIOExtensionPropertyDeviceModel")]
		NSString DeviceModel { get; }

		[Field ("CMIOExtensionPropertyDeviceIsSuspended")]
		NSString DeviceIsSuspended { get; }

		[Field ("CMIOExtensionPropertyDeviceTransportType")]
		NSString DeviceTransportType { get; }

		[Field ("CMIOExtensionPropertyDeviceLinkedCoreAudioDeviceUID")]
		NSString DeviceLinkedCoreAudioDeviceUid { get; }

		[Field ("CMIOExtensionPropertyDeviceCanBeDefaultInputDevice")]
		NSString DeviceCanBeDefaultInputDevice { get; }

		[Field ("CMIOExtensionPropertyDeviceCanBeDefaultOutputDevice")]
		NSString DeviceCanBeDefaultOutputDevice { get; }

		[NoiOS, NoTV, Mac (14, 4), MacCatalyst (17, 4)]
		[Field ("CMIOExtensionPropertyDeviceLatency")]
		NSString DeviceLatency { get; }

		[Field ("CMIOExtensionPropertyStreamActiveFormatIndex")]
		NSString StreamActiveFormatIndex { get; }

		[Field ("CMIOExtensionPropertyStreamFrameDuration")]
		NSString StreamFrameDuration { get; }

		[Field ("CMIOExtensionPropertyStreamMaxFrameDuration")]
		NSString StreamMaxFrameDuration { get; }

		[Field ("CMIOExtensionPropertyStreamSinkBufferQueueSize")]
		NSString StreamSinkBufferQueueSize { get; }

		[Field ("CMIOExtensionPropertyStreamSinkBuffersRequiredForStartup")]
		NSString StreamSinkBuffersRequiredForStartup { get; }

		[Field ("CMIOExtensionPropertyStreamSinkBufferUnderrunCount")]
		NSString StreamSinkBufferUnderrunCount { get; }

		[Field ("CMIOExtensionPropertyStreamSinkEndOfData")]
		NSString StreamSinkEndOfData { get; }

		[NoiOS, NoTV, Mac (14, 4), MacCatalyst (17, 4)]
		[Field ("CMIOExtensionPropertyStreamLatency")]
		NSString StreamLatency { get; }
	}

	/// <summary>Provides CoreMediaIO extension info dictionary and Mach service name key constants.</summary>
	[NoiOS, NoTV, Mac (12, 3), MacCatalyst (15, 4)]
	[Static]
	[Partial]
	interface CMIOExtensionKeys {

		[Field ("CMIOExtensionInfoDictionaryKey")]
		NSString InfoDictionaryKey { get; }

		[Field ("CMIOExtensionMachServiceNameKey")]
		NSString MachServiceNameKey { get; }
	}

	/// <summary>Provides CoreMediaIO sample buffer attachment key constants.</summary>
	[NoiOS, NoTV, Mac (10, 7), MacCatalyst (15, 4)]
	[Static]
	[Partial]
	interface CMIOSampleBufferAttachmentKeys {

		[Field ("kCMIOSampleBufferAttachmentKey_DiscontinuityFlags")]
		NSString DiscontinuityFlags { get; }

		[Field ("kCMIOSampleBufferAttachmentKey_SequenceNumber")]
		NSString SequenceNumber { get; }

		[Field ("kCMIOSampleBufferAttachmentKey_HDV1_PackData")]
		NSString Hdv1PackData { get; }

		[Field ("kCMIOSampleBufferAttachmentKey_HDV2_VAUX")]
		NSString Hdv2Vaux { get; }

		[Field ("kCMIOSampleBufferAttachmentKey_CAAudioTimeStamp")]
		NSString CAAudioTimeStamp { get; }

		[Field ("kCMIOSampleBufferAttachmentKey_SMPTETime")]
		NSString SmpteTime { get; }

		[Field ("kCMIOSampleBufferAttachmentKey_NativeSMPTEFrameCount")]
		NSString NativeSmpteFrameCount { get; }

		[Field ("kCMIOSampleBufferAttachmentKey_NumberOfVideoFramesInBuffer")]
		NSString NumberOfVideoFramesInBuffer { get; }

		[Field ("kCMIOSampleBufferAttachmentKey_NumberOfVideoFramesInGOP")]
		NSString NumberOfVideoFramesInGop { get; }

		[Field ("kCMIOSampleBufferAttachmentKey_MuxedSourcePresentationTimeStamp")]
		NSString MuxedSourcePresentationTimeStamp { get; }

		[Field ("kCMIOSampleBufferAttachmentKey_HostTime")]
		NSString HostTime { get; }

		[Field ("kCMIOSampleBufferAttachmentKey_RepeatedBufferContents")]
		NSString RepeatedBufferContents { get; }

		[Field ("kCMIOSampleBufferAttachmentKey_SourceAudioFormatDescription")]
		NSString SourceAudioFormatDescription { get; }

		[Field ("kCMIOSampleBufferAttachmentKey_PulldownCadenceInfo")]
		NSString PulldownCadenceInfo { get; }

		[Field ("kCMIOSampleBufferAttachmentKey_ClosedCaptionSampleBuffer")]
		NSString ClosedCaptionSampleBuffer { get; }

		[Field ("kCMIOSampleBufferAttachmentKey_ClientSequenceID")]
		NSString ClientSequenceId { get; }

		[Field ("kCMIOSampleBufferAttachmentKey_MouseAndKeyboardModifiers")]
		NSString MouseAndKeyboardModifiers { get; }

		[Mac (14, 0), MacCatalyst (17, 0)]
		[Field ("kCMIOSampleBufferAttachmentKey_PixelBufferOverlaidByStaticImage")]
		NSString PixelBufferOverlaidByStaticImage { get; }

		[Field ("kCMIOSampleBufferAttachmentKey_NoDataMarker")]
		NSString NoDataMarker { get; }
	}

	/// <summary>Provides CoreMediaIO mouse and keyboard modifier attachment key constants.</summary>
	[NoiOS, NoTV, Mac (10, 7), MacCatalyst (15, 4)]
	[Static]
	[Partial]
	interface CMIOSampleBufferMouseAndKeyboardModifiersKeys {

		[Field ("kCMIOSampleBufferAttachment_MouseAndKeyboardModifiersKey_CursorPositionX")]
		NSString CursorPositionX { get; }

		[Field ("kCMIOSampleBufferAttachment_MouseAndKeyboardModifiersKey_CursorPositionY")]
		NSString CursorPositionY { get; }

		[Field ("kCMIOSampleBufferAttachment_MouseAndKeyboardModifiersKey_MouseButtonState")]
		NSString MouseButtonState { get; }

		[Field ("kCMIOSampleBufferAttachment_MouseAndKeyboardModifiersKey_CursorIsVisible")]
		NSString CursorIsVisible { get; }

		[Field ("kCMIOSampleBufferAttachment_MouseAndKeyboardModifiersKey_CursorFrameRect")]
		NSString CursorFrameRect { get; }

		[Field ("kCMIOSampleBufferAttachment_MouseAndKeyboardModifiersKey_CursorReference")]
		NSString CursorReference { get; }

		[Field ("kCMIOSampleBufferAttachment_MouseAndKeyboardModifiersKey_CursorSeed")]
		NSString CursorSeed { get; }

		[Field ("kCMIOSampleBufferAttachment_MouseAndKeyboardModifiersKey_CursorScale")]
		NSString CursorScale { get; }

		[Field ("kCMIOSampleBufferAttachment_MouseAndKeyboardModifiersKey_CursorIsDrawnInFramebuffer")]
		NSString CursorIsDrawnInFramebuffer { get; }

		[Field ("kCMIOSampleBufferAttachment_MouseAndKeyboardModifiersKey_KeyboardModifiers")]
		NSString KeyboardModifiers { get; }

		[Field ("kCMIOSampleBufferAttachment_MouseAndKeyboardModifiersKey_KeyboardModifiersEvent")]
		NSString KeyboardModifiersEvent { get; }
	}

	/// <summary>Provides CoreMediaIO block buffer attachment key constants.</summary>
	[NoiOS, NoTV, Mac (10, 7), MacCatalyst (15, 4)]
	[Static]
	[Partial]
	interface CMIOBlockBufferAttachmentKeys {

		[Field ("kCMIOBlockBufferAttachmentKey_CVPixelBufferReference")]
		NSString CVPixelBufferReference { get; }
	}
}
