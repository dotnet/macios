//
// CoreNFC C# bindings
//
// Authors:
//	Alex Soto  <alexsoto@microsoft.com>
//
// Copyright 2017 Xamarin Inc. All rights reserved.
//

using System;
using ObjCRuntime;
using CoreFoundation;
using Foundation;

namespace CoreNFC {

	/// <summary>Enumerates errors relating to NFC tag detection and reading.</summary>
	[MacCatalyst (13, 1)]
	[ErrorDomain ("NFCErrorDomain")]
	[Native]
	public enum NFCReaderError : long {
		/// <summary>The session does not support the requested feature.</summary>
		UnsupportedFeature = 1,
		/// <summary>The session violated a security protocol.</summary>
		SecurityViolation,
		InvalidParameter,
		InvalidParameterLength,
		ParameterOutOfBound,
		RadioDisabled = 6,
		Ineligible = 7,
		AccessNotAccepted = 8,

		/// <summary>The connection to the tag was lost.</summary>
		ReaderTransceiveErrorTagConnectionLost = 100,
		/// <summary>The number of configured retries was exceeded.</summary>
		ReaderTransceiveErrorRetryExceeded,
		/// <summary>The tag responded with an error message.</summary>
		ReaderTransceiveErrorTagResponseError,
		ReaderTransceiveErrorSessionInvalidated,
		ReaderTransceiveErrorTagNotConnected,
		ReaderTransceiveErrorPacketTooLong = 105,

		/// <summary>The session was canceled by the user.</summary>
		ReaderSessionInvalidationErrorUserCanceled = 200,
		/// <summary>The session timed out.</summary>
		ReaderSessionInvalidationErrorSessionTimeout,
		/// <summary>The session terminated unexpectedly.</summary>
		ReaderSessionInvalidationErrorSessionTerminatedUnexpectedly,
		/// <summary>The session was invalidated because the system was busy (only one session can run at a time).</summary>
		ReaderSessionInvalidationErrorSystemIsBusy,
		/// <summary>The initial NDEF tag was invalid.</summary>
		ReaderSessionInvalidationErrorFirstNDEFTagRead,

		/// <summary>The command configuration for the tag included invalid values.</summary>
		TagCommandConfigurationErrorInvalidParameters = 300,

		NdefReaderSessionErrorTagNotWritable = 400,
		NdefReaderSessionErrorTagUpdateFailure = 401,
		NdefReaderSessionErrorTagSizeTooSmall = 402,
		NdefReaderSessionErrorZeroLengthMessage = 403,
	}

	//[NoTV, NoMac]
	/// <summary>Enumerates the kinds of NFC tags that can be read (currently only ISO-15693).</summary>
	[MacCatalyst (13, 1)]
	[Native]
	public enum NFCTagType : ulong {
		/// <summary>An ISO-15693 vicinity card.</summary>
		Iso15693 = 1,
		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		FeliCa = 2,
		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		Iso7816Compatible = 3,
		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		MiFare = 4,
	}

	//[NoTV, NoMac]
	/// <summary>Enumerates the kinds of content-type available to <see cref="CoreNFC.NFCNdefPayload" /> objects.</summary>
	[MacCatalyst (13, 1)]
	public enum NFCTypeNameFormat : byte { // uint8_t
		/// <summary>The payload contains no data.</summary>
		Empty = 0x00,
		/// <summary>The data follows the NFC record-type definition specification.</summary>
		NFCWellKnown = 0x01,
		/// <summary>The data is a media type, as defined in RFC-2046.</summary>
		Media = 0x02,
		/// <summary>The data is a URI.</summary>
		AbsoluteUri = 0x03,
		/// <summary>The data is defined using the NFC record-type definition for external types.</summary>
		NFCExternal = 0x04,
		/// <summary>The data format is unknown.</summary>
		Unknown = 0x05,
		/// <summary>The data is part of a chunked-data record series and is not the initial record (which defines the overall format).</summary>
		Unchanged = 0x06,
	}

	//[NoTV, NoMac]
	/// <summary>A <see cref="CoreNFC.NFCReaderSession" /> for reading ISO-15693 vicinity cards.</summary>
	[Deprecated (PlatformName.iOS, 17, 0)]
	[Deprecated (PlatformName.MacCatalyst, 17, 0)]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NFCReaderSession), Name = "NFCISO15693ReaderSession")]
	[DisableDefaultCtor]
	interface NFCIso15693ReaderSession {

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("NFCISO15693TagResponseErrorKey")]
		NSString TagResponseErrorKey { get; }

		/// <param name="delegate">To be added.</param>
		/// <param name="queue">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("initWithDelegate:queue:")]
		[DesignatedInitializer]
		NativeHandle Constructor (INFCReaderSessionDelegate @delegate, [NullAllowed] DispatchQueue queue);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Static]
		[Export ("readingAvailable")]
		bool ReadingAvailable { get; }

		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("restartPolling")]
		void RestartPolling ();
	}

	//[NoTV, NoMac]
	/// <summary>Subclass of <see cref="CoreNFC.NFCTagCommandConfiguration" /> that contains further configuration for commands to ISO-15693 tags.</summary>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NFCTagCommandConfiguration), Name = "NFCISO15693CustomCommandConfiguration")]
	interface NFCIso15693CustomCommandConfiguration {

		/// <summary>Gets or sets the ISO-7816-6 manufacturer code.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("manufacturerCode")]
		nuint ManufacturerCode { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("customCommandCode")]
		nuint CustomCommandCode { get; set; }

		/// <summary>Gets or sets the command-specific parameters, as <see cref="Foundation.NSData" />.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("requestParameters", ArgumentSemantic.Copy)]
		NSData RequestParameters { get; set; }

		/// <param name="manufacturerCode">To be added.</param>
		/// <param name="customCommandCode">To be added.</param>
		/// <param name="requestParameters">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("initWithManufacturerCode:customCommandCode:requestParameters:")]
		NativeHandle Constructor (nuint manufacturerCode, nuint customCommandCode, [NullAllowed] NSData requestParameters);

		/// <param name="manufacturerCode">To be added.</param>
		/// <param name="customCommandCode">To be added.</param>
		/// <param name="requestParameters">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		/// <param name="maximumRetries">To be added.</param>
		/// <param name="retryInterval">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("initWithManufacturerCode:customCommandCode:requestParameters:maximumRetries:retryInterval:")]
		NativeHandle Constructor (nuint manufacturerCode, nuint customCommandCode, [NullAllowed] NSData requestParameters, nuint maximumRetries, double retryInterval);
	}

	//[NoTV, NoMac]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NFCTagCommandConfiguration), Name = "NFCISO15693ReadMultipleBlocksConfiguration")]
	interface NFCIso15693ReadMultipleBlocksConfiguration {

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("range", ArgumentSemantic.Assign)]
		NSRange Range { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("chunkSize")]
		nuint ChunkSize { get; set; }

		/// <param name="range">To be added.</param>
		/// <param name="chunkSize">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("initWithRange:chunkSize:")]
		NativeHandle Constructor (NSRange range, nuint chunkSize);

		/// <param name="range">To be added.</param>
		/// <param name="chunkSize">To be added.</param>
		/// <param name="maximumRetries">To be added.</param>
		/// <param name="retryInterval">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("initWithRange:chunkSize:maximumRetries:retryInterval:")]
		NativeHandle Constructor (NSRange range, nuint chunkSize, nuint maximumRetries, double retryInterval);
	}

	delegate void NFCGetSystemInfoCompletionHandler (nint dsfid, nint afi, nint blockSize, nint blockCount, nint icReference, [NullAllowed] NSError error);

	interface INFCIso15693Tag { }

	delegate void NFCIso15693TagReadMultipleBlocksCallback (NSData [] dataBlocks, [NullAllowed] NSError error);
	delegate void NFCIso15693TagResponseCallback (NFCIso15693ResponseFlag responseFlag, NSData response, [NullAllowed] NSError error);
	delegate void NFCIso15693TagGetMultipleBlockSecurityStatusCallback (NSNumber [] securityStatus, [NullAllowed] NSError error);
	delegate void NFCIso15693TagGetSystemInfoAndUidCallback (NSData uid, nint dsfid, nint afi, nint blockSize, nint blockCount, nint icReference, [NullAllowed] NSError error);

	//[NoTV, NoMac]
	/// <summary>Implements <see cref="CoreNFC.INFCTag" /> with data associated with an ISO-15693 vicinity card.</summary>
	[MacCatalyst (13, 1)]
	[Protocol (Name = "NFCISO15693Tag")]
	interface NFCIso15693Tag : NFCTag, NFCNdefTag {

		/// <summary>Gets the identifier of the tag, as <see cref="Foundation.NSData" />.</summary>
		/// <value>To be added.</value>
		/// <remarks>To be added.</remarks>
		[Abstract]
		[Export ("identifier", ArgumentSemantic.Copy)]
		NSData Identifier { get; }

		/// <summary>Manufacturer, as defined in ISO-7816-6.</summary>
		/// <value>To be added.</value>
		/// <remarks>To be added.</remarks>
		[Abstract]
		[Export ("icManufacturerCode")]
		nuint IcManufacturerCode { get; }

		/// <summary>Gets the serial number of the tag, as <see cref="Foundation.NSData" />.</summary>
		/// <value>To be added.</value>
		/// <remarks>To be added.</remarks>
		[Abstract]
		[Export ("icSerialNumber", ArgumentSemantic.Copy)]
		NSData IcSerialNumber { get; }

		/// <param name="commandConfiguration">To be added.</param>
		/// <param name="completionHandler">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Abstract]
		[Export ("sendCustomCommandWithConfiguration:completionHandler:")]
		void SendCustomCommand (NFCIso15693CustomCommandConfiguration commandConfiguration, Action<NSData, NSError> completionHandler);

		/// <param name="readConfiguration">To be added.</param>
		/// <param name="completionHandler">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Abstract]
		[Export ("readMultipleBlocksWithConfiguration:completionHandler:")]
		void ReadMultipleBlocks (NFCIso15693ReadMultipleBlocksConfiguration readConfiguration, Action<NSData, NSError> completionHandler);

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("stayQuietWithCompletionHandler:")]
		void StayQuiet (Action<NSError> completionHandler);

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("readSingleBlockWithRequestFlags:blockNumber:completionHandler:")]
		void ReadSingleBlock (NFCIso15693RequestFlag flags, byte blockNumber, Action<NSData, NSError> completionHandler);

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("writeSingleBlockWithRequestFlags:blockNumber:dataBlock:completionHandler:")]
		void WriteSingleBlock (NFCIso15693RequestFlag flags, byte blockNumber, NSData dataBlock, Action<NSError> completionHandler);

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("lockBlockWithRequestFlags:blockNumber:completionHandler:")]
		void LockBlock (NFCIso15693RequestFlag flags, byte blockNumber, Action<NSError> completionHandler);

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("readMultipleBlocksWithRequestFlags:blockRange:completionHandler:")]
		void ReadMultipleBlocks (NFCIso15693RequestFlag flags, NSRange blockRange, Action<NSData [], NSError> completionHandler);

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("writeMultipleBlocksWithRequestFlags:blockRange:dataBlocks:completionHandler:")]
		void WriteMultipleBlocks (NFCIso15693RequestFlag flags, NSRange blockRange, NSData [] dataBlocks, Action<NSError> completionHandler);

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("selectWithRequestFlags:completionHandler:")]
		void Select (NFCIso15693RequestFlag flags, Action<NSError> completionHandler);

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("resetToReadyWithRequestFlags:completionHandler:")]
		void ResetToReady (NFCIso15693RequestFlag flags, Action<NSError> completionHandler);

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("writeAFIWithRequestFlag:afi:completionHandler:")]
		void WriteAfi (NFCIso15693RequestFlag flags, byte afi, Action<NSError> completionHandler);

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("lockAFIWithRequestFlag:completionHandler:")]
		void LockAfi (NFCIso15693RequestFlag flags, Action<NSError> completionHandler);

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("writeDSFIDWithRequestFlag:dsfid:completionHandler:")]
		void WriteDsfi (NFCIso15693RequestFlag flags, byte dsfid, Action<NSError> completionHandler);

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("lockDFSIDWithRequestFlag:completionHandler:")]
		void LockDfsi (NFCIso15693RequestFlag flags, Action<NSError> completionHandler);

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("getSystemInfoWithRequestFlag:completionHandler:")]
		void GetSystemInfo (NFCIso15693RequestFlag flags, NFCGetSystemInfoCompletionHandler completionHandler);

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("getMultipleBlockSecurityStatusWithRequestFlag:blockRange:completionHandler:")]
		void GetMultipleBlockSecurityStatus (NFCIso15693RequestFlag flags, NSRange blockRange, Action<NSNumber [], NSError> completionHandler);

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("customCommandWithRequestFlag:customCommandCode:customRequestParameters:completionHandler:")]
		void CustomCommand (NFCIso15693RequestFlag flags, nint customCommandCode, NSData customRequestParameters, Action<NSData, NSError> completionHandler);

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("extendedReadSingleBlockWithRequestFlags:blockNumber:completionHandler:")]
		void ExtendedReadSingleBlock (NFCIso15693RequestFlag flags, nint blockNumber, Action<NSData, NSError> completionHandler);

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("extendedWriteSingleBlockWithRequestFlags:blockNumber:dataBlock:completionHandler:")]
		void ExtendedWriteSingleBlock (NFCIso15693RequestFlag flags, nint blockNumber, NSData dataBlock, Action<NSError> completionHandler);

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("extendedLockBlockWithRequestFlags:blockNumber:completionHandler:")]
		void ExtendedLockBlock (NFCIso15693RequestFlag flags, nint blockNumber, Action<NSError> completionHandler);

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("extendedReadMultipleBlocksWithRequestFlags:blockRange:completionHandler:")]
		void ExtendedReadMultipleBlocks (NFCIso15693RequestFlag flags, NSRange blockRange, Action<NSData [], NSError> completionHandler);

		[iOS (14, 0)]
		[MacCatalyst (14, 0)]
		[Abstract]
		[Export ("extendedWriteMultipleBlocksWithRequestFlags:blockRange:dataBlocks:completionHandler:")]
		void ExtendedWriteMultipleBlocks (NFCIso15693RequestFlag flags, NSRange blockRange, NSData [] dataBlocks, Action<NSError> completionHandler);

		[iOS (14, 0)]
		[MacCatalyst (14, 0)]
		[Abstract]
		[Export ("authenticateWithRequestFlags:cryptoSuiteIdentifier:message:completionHandler:")]
		void Authenticate (NFCIso15693RequestFlag flags, nint cryptoSuiteIdentifier, NSData message, NFCIso15693TagResponseCallback completionHandler);

		[iOS (14, 0)]
		[MacCatalyst (14, 0)]
		[Abstract]
		[Export ("keyUpdateWithRequestFlags:keyIdentifier:message:completionHandler:")]
		void KeyUpdate (NFCIso15693RequestFlag flags, nint keyIdentifier, NSData message, NFCIso15693TagResponseCallback completionHandler);

		[iOS (14, 0)]
		[MacCatalyst (14, 0)]
		[Abstract]
		[Export ("challengeWithRequestFlags:cryptoSuiteIdentifier:message:completionHandler:")]
		void Challenge (NFCIso15693RequestFlag flags, nint cryptoSuiteIdentifier, NSData message, Action<NSError> completionHandler);

		[iOS (14, 0)]
		[MacCatalyst (14, 0)]
		[Abstract]
		[Export ("readBufferWithRequestFlags:completionHandler:")]
		void ReadBuffer (NFCIso15693RequestFlag flags, NFCIso15693TagResponseCallback completionHandler);

		[iOS (14, 0)]
		[MacCatalyst (14, 0)]
		[Abstract]
		[Export ("extendedGetMultipleBlockSecurityStatusWithRequestFlag:blockRange:completionHandler:")]
		void ExtendedGetMultipleBlockSecurityStatus (NFCIso15693RequestFlag flags, NSRange blockRange, NFCIso15693TagGetMultipleBlockSecurityStatusCallback completionHandler);

		[iOS (14, 0)]
		[MacCatalyst (14, 0)]
		[Abstract]
		[Export ("extendedFastReadMultipleBlocksWithRequestFlag:blockRange:completionHandler:")]
		void ExtendedFastReadMultipleBlocks (NFCIso15693RequestFlag flags, NSRange blockRange, NFCIso15693TagReadMultipleBlocksCallback completionHandler);

		[iOS (14, 0)]
		[MacCatalyst (14, 0)]
		[Abstract]
		[Export ("sendRequestWithFlag:commandCode:data:completionHandler:")]
		void SendRequest (nint flags, nint commandCode, [NullAllowed] NSData data, NFCIso15693TagResponseCallback completionHandler);

		[iOS (14, 0)]
		[MacCatalyst (14, 0)]
		[Abstract]
		[Export ("getSystemInfoAndUIDWithRequestFlag:completionHandler:")]
		void GetSystemInfoAndUid (NFCIso15693RequestFlag flags, NFCIso15693TagGetSystemInfoAndUidCallback completionHandler);

		[iOS (14, 0)]
		[MacCatalyst (14, 0)]
		[Abstract]
		[Export ("fastReadMultipleBlocksWithRequestFlag:blockRange:completionHandler:")]
		void FastReadMultipleBlocks (NFCIso15693RequestFlag flags, NSRange blockRange, NFCIso15693TagReadMultipleBlocksCallback completionHandler);

		[iOS (14, 0)]
		[MacCatalyst (14, 0)]
		[Abstract]
		[Export ("lockDSFIDWithRequestFlag:completionHandler:")]
		void LockDsfId (NFCIso15693RequestFlag flags, Action<NSError> completionHandler);

	}

	/// <summary>An element that helps comprise an <see cref="CoreNFC.NFCNdefMessage" />.</summary>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject), Name = "NFCNDEFPayload")]
	[DisableDefaultCtor]
	interface NFCNdefPayload : NSSecureCoding {

		/// <summary>Gets or sets the <see cref="CoreNFC.NFCTypeNameFormat" /> that defines the content type of the payload.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("typeNameFormat", ArgumentSemantic.Assign)]
		NFCTypeNameFormat TypeNameFormat { get; set; }

		/// <summary>Gets or sets the NDEF-specified kind of the payload.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("type", ArgumentSemantic.Copy)]
		NSData Type { get; set; }

		/// <summary>Gets or sets the NDEF-specified identifier of the payload.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("identifier", ArgumentSemantic.Copy)]
		NSData Identifier { get; set; }

		/// <summary>Gets or sets the payload data, as <see cref="Foundation.NSData" />.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("payload", ArgumentSemantic.Copy)]
		NSData Payload { get; set; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Static]
		[Export ("wellKnownTypeURIPayloadWithString:")]
		[return: NullAllowed]
		NFCNdefPayload CreateWellKnownTypePayload (string uri);

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Static]
		[Export ("wellKnownTypeURIPayloadWithURL:")]
		[return: NullAllowed]
		NFCNdefPayload CreateWellKnownTypePayload (NSUrl url);

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Static]
		[Export ("wellKnownTypeTextPayloadWithString:locale:")]
		[return: NullAllowed]
		NFCNdefPayload CreateWellKnownTypePayload (string text, NSLocale locale);

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[NullAllowed, Export ("wellKnownTypeURIPayload")]
		NSUrl WellKnownTypeUriPayload { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Export ("wellKnownTypeTextPayloadWithLocale:")]
		[return: NullAllowed]
		string GetWellKnownTypeTextPayload (out NSLocale locale);

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Export ("initWithFormat:type:identifier:payload:")]
		NativeHandle Constructor (NFCTypeNameFormat format, NSData type, NSData identifier, NSData payload);

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Export ("initWithFormat:type:identifier:payload:chunkSize:")]
		NativeHandle Constructor (NFCTypeNameFormat format, NSData type, NSData identifier, NSData payload, nuint chunkSize);
	}

	/// <summary>An NDEF message, consisting of an array of <see cref="CoreNFC.NFCNdefPayload" /> objects.</summary>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject), Name = "NFCNDEFMessage")]
	[DisableDefaultCtor]
	interface NFCNdefMessage : NSSecureCoding {

		/// <summary>Gets or sets the array of <see cref="CoreNFC.NFCNdefPayload" /> objects that constitute the message.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("records", ArgumentSemantic.Copy)]
		NFCNdefPayload [] Records { get; set; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Static]
		[Export ("ndefMessageWithData:")]
		[return: NullAllowed]
		NFCNdefMessage Create (NSData data);

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Export ("initWithNDEFRecords:")]
		NativeHandle Constructor (NFCNdefPayload [] records);

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Export ("length")]
		nuint Length { get; }
	}

	/// <summary>Defines the delegate-object interface for <see cref="CoreNFC.NFCNdefReaderSession" /> objects.</summary>
	interface INFCNdefReaderSessionDelegate { }

	/// <summary>Delegate object for <see cref="CoreNFC.NFCNdefReaderSession" /> objects, allowing the developer to respond to detection of NFC tags and session invalidation.</summary>
	[MacCatalyst (13, 1)]
	[Protocol, Model]
	[BaseType (typeof (NSObject), Name = "NFCNDEFReaderSessionDelegate")]
	interface NFCNdefReaderSessionDelegate {

		/// <param name="session">The session that was invalidated.</param>
		///         <param name="error">The error that invalidated the session.</param>
		///         <summary>Developers may override this method to respond to the invalidation of the NFC session.</summary>
		///         <remarks>To be added.</remarks>
		[Abstract]
		[Export ("readerSession:didInvalidateWithError:")]
		void DidInvalidate (NFCNdefReaderSession session, NSError error);

		/// <param name="session">The session that detected the messages.</param>
		///         <param name="messages">To be added.</param>
		///         <summary>Developers may override this method to respond to the detection of NFC tags.</summary>
		///         <remarks>To be added.</remarks>
		[Abstract]
		[Export ("readerSession:didDetectNDEFs:")]
		void DidDetect (NFCNdefReaderSession session, NFCNdefMessage [] messages);

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Export ("readerSession:didDetectTags:")]
		void DidDetectTags (NFCNdefReaderSession session, INFCNdefTag [] tags);

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Export ("readerSessionDidBecomeActive:")]
		void DidBecomeActive (NFCNdefReaderSession session);
	}

	/// <summary>Subclass of <see cref="CoreNFC.NFCReaderSession" /> that works with NFC Data Exchange Format (NDEF) tags.</summary>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NFCReaderSession), Name = "NFCNDEFReaderSession")]
	[DisableDefaultCtor]
	interface NFCNdefReaderSession {

		/// <param name="delegate">To be added.</param>
		/// <param name="queue">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		/// <param name="invalidateAfterFirstRead">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("initWithDelegate:queue:invalidateAfterFirstRead:")]
		[DesignatedInitializer]
		NativeHandle Constructor (INFCNdefReaderSessionDelegate @delegate, [NullAllowed] DispatchQueue queue, bool invalidateAfterFirstRead);

		/// <summary>Gets whether the device supports NFC tag reading.</summary>
		///         <value>To be added.</value>
		///         <remarks>
		///           <para>NFC tag reading is only available on iPhone 7 and later models.</para>
		///         </remarks>
		[Static]
		[Export ("readingAvailable")]
		bool ReadingAvailable { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Export ("restartPolling")]
		void RestartPolling ();

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Export ("connectToTag:completionHandler:")]
		[Async]
		void ConnectToTag (INFCNdefTag tag, Action<NSError> completionHandler);
	}

	/// <summary>Abstract class implementing <see cref="CoreNFC.INFCReaderSessionContract" /> and base class for <see cref="CoreNFC.NFCNdefReaderSession" />. A session for detecting and reading NFC tags.</summary>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface NFCReaderSession : NFCReaderSessionContract {

		/// <summary>An instance of the CoreNFC.INFCReaderSessionDelegate model class which acts as the class delegate.</summary>
		///         <value>The instance of the CoreNFC.INFCReaderSessionDelegate model class</value>
		///         <remarks>
		///           <para>The delegate instance assigned to this object will be used to handle events or provide data on demand to this class.</para>
		///           <para>When setting the Delegate or WeakDelegate values events will be delivered to the specified instance instead of being delivered to the C#-style events</para>
		///           <para>This is the strongly typed version of the object, developers should use the WeakDelegate property instead if they want to merely assign a class derived from NSObject that has been decorated with [Export] attributes.</para>
		///         </remarks>
		[Export ("delegate", ArgumentSemantic.Weak), NullAllowed]
		INFCReaderSessionDelegate Delegate { get; }

		/// <summary>Gets the <see cref="CoreFoundation.DispatchQueue" /> on which the NFC detection and reading is performed.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("sessionQueue")]
		DispatchQueue SessionQueue { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Static]
		[Export ("readingAvailable")]
		bool ReadingAvailable { get; }
	}

	interface INFCReaderSessionContract { }

	/// <summary>Interface defining the methods for an NFC detection- and reading- session.</summary>
	[MacCatalyst (13, 1)]
	[Protocol (Name = "NFCReaderSession")]
	interface NFCReaderSessionContract {

		/// <summary>Gets whether the session is ready to detect and read NFC tags.</summary>
		/// <value>To be added.</value>
		/// <remarks>To be added.</remarks>
		[Abstract]
		[Export ("ready")]
		bool Ready { [Bind ("isReady")] get; }

		/// <summary>Gets or sets a user-meaningful message describing the application's use of NFC.</summary>
		/// <value>To be added.</value>
		/// <remarks>To be added.</remarks>
		[Abstract]
		[Export ("alertMessage")]
		string AlertMessage { get; set; }

		/// <summary>Starts a session for detecting and reading NFC tags.</summary>
		/// <remarks>To be added.</remarks>
		[Abstract]
		[Export ("beginSession")]
		void BeginSession ();

		/// <summary>Closes an NFC session. Once invalidated, a session cannot be reused.</summary>
		/// <remarks>To be added.</remarks>
		[Abstract]
		[Export ("invalidateSession")]
		void InvalidateSession ();

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("invalidateSessionWithErrorMessage:")]
		void InvalidateSession (string errorMessage);
	}

	/// <summary>The delegate-object interface for <see cref="CoreNFC.NFCReaderSession" /> objects.</summary>
	interface INFCReaderSessionDelegate { }

	//[NoTV, NoMac]
	/// <summary>Delegate object for <see cref="CoreNFC.NFCReaderSession" /> objects.</summary>
	[MacCatalyst (13, 1)]
	[Protocol, Model]
	[BaseType (typeof (NSObject))]
	interface NFCReaderSessionDelegate {

		/// <param name="session">The session that became active.</param>
		///         <summary>Developers may override this method to react to the <see cref="CoreNFC.NFCReaderSession" /> activating.</summary>
		///         <remarks>To be added.</remarks>
		[Abstract]
		[Export ("readerSessionDidBecomeActive:")]
		void DidBecomeActive (NFCReaderSession session);

		/// <param name="session">The session that detected the tags.</param>
		///         <param name="tags">The tags that were detected.</param>
		///         <summary>Developers may override this method to react to the detection of NFC <paramref name="tags" />.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("readerSession:didDetectTags:")]
		void DidDetectTags (NFCReaderSession session, INFCTag [] tags);

		/// <param name="session">The session that was invalidated.</param>
		///         <param name="error">The error that invalidated the session.</param>
		///         <summary>Developers may override this method to react to the invalidation of the <see cref="CoreNFC.NFCReaderSession" />.</summary>
		///         <remarks>To be added.</remarks>
		[Abstract]
		[Export ("readerSession:didInvalidateWithError:")]
		void DidInvalidate (NFCReaderSession session, NSError error);
	}

	interface INFCTag { }

	/// <summary>Interface defining the common features of all NFC tags.</summary>
	[MacCatalyst (13, 1)]
	[Protocol]
	interface NFCTag : NSSecureCoding, NSCopying {

		/// <summary>Gets the kind of NFC tag.</summary>
		/// <value>To be added.</value>
		/// <remarks>To be added.</remarks>
		[Abstract]
		[Export ("type", ArgumentSemantic.Assign)]
		NFCTagType Type { get; }

		/// <summary>Gets the <see cref="CoreNFC.NFCReaderSession" /> that provided the tag.</summary>
		/// <value>To be added.</value>
		/// <remarks>To be added.</remarks>
		[Abstract]
		[NullAllowed, Export ("session", ArgumentSemantic.Weak)]
		NFCReaderSession Session { get; }

		/// <summary>Gets whether the tag is available for reading.</summary>
		/// <value>To be added.</value>
		/// <remarks>To be added.</remarks>
		[Abstract]
		[Export ("available")]
		bool Available { [Bind ("isAvailable")] get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Export ("asNFCISO15693Tag")]
		[Abstract]
		[NullAllowed]
		INFCIso15693Tag AsNFCIso15693Tag { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Export ("asNFCISO7816Tag")]
		[Abstract]
		[NullAllowed]
		INFCIso7816Tag AsNFCIso7816Tag { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Export ("asNFCFeliCaTag")]
		[Abstract]
		[NullAllowed]
		INFCFeliCaTag AsNFCFeliCaTag { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Export ("asNFCMiFareTag")]
		[Abstract]
		[NullAllowed]
		INFCMiFareTag AsNFCMiFareTag { get; }
	}

	//[NoTV, NoMac]
	/// <summary>Parameters for sending a command to an NFC tag.</summary>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	interface NFCTagCommandConfiguration : NSCopying {

		/// <summary>Gets or sets the number of tries a command may be resent, if necessary.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("maximumRetries")]
		nuint MaximumRetries { get; set; }

		/// <summary>Gets or sets the time, in seconds, between retry attempts.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("retryInterval")]
		double RetryInterval { get; set; }
	}

	[MacCatalyst (13, 1)]
	[Category]
	[BaseType (typeof (NSUserActivity))]
	interface NSUserActivity_CoreNFC {

		/// <summary>To be added.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		[Export ("ndefMessagePayload")]
		NFCNdefMessage GetNdefMessagePayload ();
	}

	[iOS (13, 0)]
	[MacCatalyst (13, 1)]
	[Native]
	enum NFCFeliCaEncryptionId
		: long {
		Aes = 79,
		Des = 65,
	}

	[iOS (13, 0)]
	[MacCatalyst (13, 1)]
	[Native]
	enum NFCMiFareFamily : long {
		Unknown = 1,
		Ultralight = 2,
		Plus = 3,
		DesFire = 4,
	}

	[iOS (13, 0)]
	[MacCatalyst (13, 1)]
	[Native]
	enum NFCNdefStatus : ulong {
		NotSupported = 1,
		ReadWrite = 2,
		ReadOnly = 3,
	}

	[iOS (13, 0)]
	[MacCatalyst (13, 1)]
	[Flags]
	[Native]
	enum NFCPollingOption : ulong {
		Iso14443 = 0x1,
		Iso15693 = 0x2,
		Iso18092 = 0x4,
		Pace = 0x8,
	}

	[iOS (13, 0)]
	[MacCatalyst (13, 1)]
	[Native]
	enum NFCFeliCaPollingRequestCode
		: long {
		NoRequest = 0,
		SystemCode = 1,
		CommunicationPerformance = 2,
	}

	[iOS (13, 0)]
	[MacCatalyst (13, 1)]
	[Native]
	enum NFCFeliCaPollingTimeSlot
		: long {
		Max1 = 0,
		Max2 = 1,
		Max4 = 3,
		Max8 = 7,
		Max16 = 15,
	}

	[iOS (13, 0)]
	[MacCatalyst (13, 1)]
	[Flags]
	[NativeName ("NFCISO15693RequestFlag")]
	enum NFCIso15693RequestFlag
		: byte {
		DualSubCarriers = (1 << 0),
		HighDataRate = (1 << 1),
		ProtocolExtension = (1 << 3),
		Select = (1 << 4),
		Address = (1 << 5),
		Option = (1 << 6),
		[iOS (14, 0)]
		[MacCatalyst (14, 0)]
		CommandSpecificBit8 = (1 << 7),
	}

	[Flags, iOS (14, 0)]
	[MacCatalyst (14, 0)]
	public enum NFCIso15693ResponseFlag : byte {
		Error = (1 << 0),
		ResponseBufferValid = (1 << 1),
		FinalResponse = (1 << 2),
		ProtocolExtension = (1 << 3),
		BlockSecurityStatusBit5 = (1 << 4),
		BlockSecurityStatusBit6 = (1 << 5),
		WaitTimeExtension = (1 << 6),
	}

	[iOS (13, 0)]
	[MacCatalyst (13, 1)]
	[Native]
	enum NFCVasErrorCode
		: long {
		Success = 36864,
		DataNotFound = 27267,
		DataNotActivated = 25223,
		WrongParameters = 27392,
		WrongLCField = 26368,
		UserIntervention = 27012,
		IncorrectData = 27264,
		UnsupportedApplicationVersion = 25408,
	}

	[iOS (13, 0)]
	[MacCatalyst (13, 1)]
	[Native]
	enum NFCVasMode
		: long {
		UrlOnly = 0,
		Normal = 1,
	}

	interface INFCNdefTag { }

	delegate void NFCQueryNdefStatusCompletionHandler (NFCNdefStatus status, nuint capacity, NSError error);

	[iOS (13, 0)]
	[MacCatalyst (13, 1)]
	[Protocol (Name = "NFCNDEFTag")]
	interface NFCNdefTag : NSSecureCoding, NSCopying {

		[Abstract]
		[Export ("available")]
		bool Available { [Bind ("isAvailable")] get; }

		[Abstract]
		[Export ("queryNDEFStatusWithCompletionHandler:")]
		void QueryNdefStatus (NFCQueryNdefStatusCompletionHandler completionHandler);

		[Abstract]
		[Export ("readNDEFWithCompletionHandler:")]
		void ReadNdef (Action<NFCNdefMessage, NSError> completionHandler);

		[Abstract]
		[Export ("writeNDEF:completionHandler:")]
		void WriteNdef (NFCNdefMessage ndefMessage, Action<NSError> completionHandler);

		[Abstract]
		[Export ("writeLockWithCompletionHandler:")]
		void WriteLock (Action<NSError> completionHandler);
	}

	interface INFCFeliCaTag { }

	delegate void NFCFeliCaPollingCompletionHandler (NSData pmm, NSData requestData, [NullAllowed] NSError error);
	delegate void NFCFeliCaReadWithoutEncryptionCompletionHandler (nint statusFlag1, nint statusFlag2, NSData [] blockData, [NullAllowed] NSError error);
	delegate void NFCFeliCaStatus1Status2CompletionHandler (nint statusFlag1, nint statusFlag2, [NullAllowed] NSError error);
	delegate void NFCFeliCaRequestServiceV2CompletionHandler (nint statusFlag1, nint statusFlag2, NFCFeliCaEncryptionId encryptionIdentifier, NSData [] nodeKeyVersionListAes, NSData [] nodeKeyVersionListDes, [NullAllowed] NSError error);
	delegate void NFCFeliCaRequestSpecificationVersionCompletionHandler (nint statusFlag1, nint statusFlag2, NSData basicVersion, NSData optionVersion, [NullAllowed] NSError error);

	[iOS (13, 0)]
	[MacCatalyst (13, 1)]
	[Protocol]
	interface NFCFeliCaTag : NFCTag, NFCNdefTag {

		[Abstract]
		[Export ("currentSystemCode", ArgumentSemantic.Retain)]
		NSData CurrentSystemCode { get; }

		[Abstract]
		[Export ("currentIDm", ArgumentSemantic.Retain)]
		NSData CurrentIdm { get; }

		[Abstract]
		[Export ("pollingWithSystemCode:requestCode:timeSlot:completionHandler:")]
		void Polling (NSData systemCode, NFCFeliCaPollingRequestCode requestCode, NFCFeliCaPollingTimeSlot timeSlot, NFCFeliCaPollingCompletionHandler completionHandler);

		[Abstract]
		[Export ("requestServiceWithNodeCodeList:completionHandler:")]
		void RequestService (NSData [] nodeCodeList, Action<NSData [], NSError> completionHandler);

		[Abstract]
		[Export ("requestResponseWithCompletionHandler:")]
		void RequestResponse (Action<nint, NSError> completionHandler);

		[Abstract]
		[Export ("readWithoutEncryptionWithServiceCodeList:blockList:completionHandler:")]
		void ReadWithoutEncryption (NSData [] serviceCodeList, NSData [] blockList, NFCFeliCaReadWithoutEncryptionCompletionHandler completionHandler);

		[Abstract]
		[Export ("writeWithoutEncryptionWithServiceCodeList:blockList:blockData:completionHandler:")]
		void WriteWithoutEncryption (NSData [] serviceCodeList, NSData [] blockList, NSData [] blockData, NFCFeliCaStatus1Status2CompletionHandler completionHandler);

		[Abstract]
		[Export ("requestSystemCodeWithCompletionHandler:")]
		void RequestSystemCode (Action<NSData [], NSError> completionHandler);

		[Abstract]
		[Export ("requestServiceV2WithNodeCodeList:completionHandler:")]
		void RequestServiceV2 (NSData [] nodeCodeList, NFCFeliCaRequestServiceV2CompletionHandler completionHandler);

		[Abstract]
		[Export ("requestSpecificationVersionWithCompletionHandler:")]
		void RequestSpecificationVersion (NFCFeliCaRequestSpecificationVersionCompletionHandler completionHandler);

		[Abstract]
		[Export ("resetModeWithCompletionHandler:")]
		void ResetMode (NFCFeliCaStatus1Status2CompletionHandler completionHandler);

		[Abstract]
		[Export ("sendFeliCaCommandPacket:completionHandler:")]
		void Send (NSData commandPacket, Action<NSData, NSError> completionHandler);
	}

	interface INFCIso7816Tag { }

	[iOS (13, 0)]
	[MacCatalyst (13, 1)]
	[Protocol (Name = "NFCISO7816Tag")]
	interface NFCIso7816Tag : NFCTag, NFCNdefTag {

		[Abstract]
		[Export ("initialSelectedAID", ArgumentSemantic.Retain)]
		string InitialSelectedAid { get; }

		[Abstract]
		[Export ("identifier", ArgumentSemantic.Copy)]
		NSData Identifier { get; }

		[Abstract]
		[NullAllowed, Export ("historicalBytes", ArgumentSemantic.Copy)]
		NSData HistoricalBytes { get; }

		[Abstract]
		[NullAllowed, Export ("applicationData", ArgumentSemantic.Copy)]
		NSData ApplicationData { get; }

		[Abstract]
		[Export ("proprietaryApplicationDataCoding")]
		bool ProprietaryApplicationDataCoding { get; }

		[Abstract]
		[Export ("sendCommandAPDU:completionHandler:")]
		void SendCommand (NFCIso7816Apdu apdu, NFCIso7816SendCompletionHandler completionHandler);
	}

	[iOS (13, 0)]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject), Name = "NFCISO7816APDU")]
	[DisableDefaultCtor]
	interface NFCIso7816Apdu : NSCopying {

		[Export ("initWithInstructionClass:instructionCode:p1Parameter:p2Parameter:data:expectedResponseLength:")]
		NativeHandle Constructor (byte instructionClass, byte instructionCode, byte p1Parameter, byte p2Parameter, NSData data, nint expectedResponseLength);

		[Export ("initWithData:")]
		NativeHandle Constructor (NSData data);

		[Export ("instructionClass")]
		byte InstructionClass { get; }

		[Export ("instructionCode")]
		byte InstructionCode { get; }

		[Export ("p1Parameter")]
		byte P1Parameter { get; }

		[Export ("p2Parameter")]
		byte P2Parameter { get; }

		[NullAllowed, Export ("data", ArgumentSemantic.Copy)]
		NSData Data { get; }

		[Export ("expectedResponseLength")]
		nint ExpectedResponseLength { get; }
	}

	interface INFCMiFareTag { }

	delegate void NFCIso7816SendCompletionHandler (NSData responseData, byte sw1, byte sw2, [NullAllowed] NSError error);

	[iOS (13, 0)]
	[MacCatalyst (13, 1)]
	[Protocol]
	interface NFCMiFareTag : NFCTag, NFCNdefTag {
		[Abstract]
		[Export ("mifareFamily", ArgumentSemantic.Assign)]
		NFCMiFareFamily MifareFamily { get; }

		[Abstract]
		[Export ("identifier", ArgumentSemantic.Copy)]
		NSData Identifier { get; }

		[Abstract]
		[NullAllowed, Export ("historicalBytes", ArgumentSemantic.Copy)]
		NSData HistoricalBytes { get; }

		[Abstract]
		[Export ("sendMiFareCommand:completionHandler:")]
		void SendMiFareCommand (NSData command, Action<NSData, NSError> completionHandler);

		[Abstract]
		[Export ("sendMiFareISO7816Command:completionHandler:")]
		void SendMiFareIso7816Command (NFCIso7816Apdu apdu, NFCIso7816SendCompletionHandler completionHandler);
	}

	interface INFCTagReaderSessionDelegate { }

	[iOS (13, 0)]
	[MacCatalyst (13, 1)]
	[Protocol]
	[Model]
	[BaseType (typeof (NSObject))]
	interface NFCTagReaderSessionDelegate {

		[Abstract]
		[Export ("tagReaderSession:didInvalidateWithError:")]
		void DidInvalidate (NFCTagReaderSession session, NSError error);

		[Export ("tagReaderSessionDidBecomeActive:")]
		void DidBecomeActive (NFCTagReaderSession session);

		[Export ("tagReaderSession:didDetectTags:")]
		void DidDetectTags (NFCTagReaderSession session, INFCTag [] tags);
	}

	[iOS (13, 0)]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NFCReaderSession))]
	[DisableDefaultCtor]
	[Advice ("Not available for application extensions.")]
	interface NFCTagReaderSession {

		[Export ("initWithPollingOption:delegate:queue:")]
		NativeHandle Constructor (NFCPollingOption pollingOption, INFCTagReaderSessionDelegate @delegate, [NullAllowed] DispatchQueue queue);

		[NullAllowed, Export ("connectedTag", ArgumentSemantic.Retain)]
		INFCTag ConnectedTag { get; }

		[Export ("restartPolling")]
		void RestartPolling ();

		[Export ("connectToTag:completionHandler:")]
		[Async]
		void ConnectTo (INFCTag tag, Action<NSError> completionHandler);

		[Field ("NFCTagResponseUnexpectedLengthErrorKey")]
		NSString UnexpectedLengthErrorKey { get; }
	}

	interface INFCVasReaderSessionDelegate { }

	[iOS (13, 0)]
	[MacCatalyst (13, 1)]
	[Protocol]
	[Model]
	[BaseType (typeof (NSObject), Name = "NFCVASReaderSessionDelegate")]
	interface NFCVasReaderSessionDelegate {

		[Export ("readerSessionDidBecomeActive:")]
		void DidBecomeActive (NFCVasReaderSession session);

		[Abstract]
		[Export ("readerSession:didInvalidateWithError:")]
		void DidInvalidate (NFCVasReaderSession session, NSError error);

		[Abstract]
		[Export ("readerSession:didReceiveVASResponses:")]
		void DidReceiveVasResponses (NFCVasReaderSession session, NFCVasResponse [] responses);
	}

	[iOS (13, 0)]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject), Name = "NFCVASResponse")]
	[DisableDefaultCtor] // implied
	interface NFCVasResponse : NSCopying {

		[Export ("status", ArgumentSemantic.Assign)]
		NFCVasErrorCode Status { get; }

		[Export ("vasData", ArgumentSemantic.Retain)]
		NSData VasData { get; }

		[Export ("mobileToken", ArgumentSemantic.Retain)]
		NSData MobileToken { get; }
	}

	[iOS (13, 0)]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject), Name = "NFCVASCommandConfiguration")]
	[DisableDefaultCtor] // implied (designated initializer)
	interface NFCVasCommandConfiguration : NSCopying {

		[Export ("initWithVASMode:passTypeIdentifier:url:")]
		[DesignatedInitializer]
		NativeHandle Constructor (NFCVasMode mode, string passTypeIdentifier, [NullAllowed] NSUrl url);

		[Export ("mode", ArgumentSemantic.Assign)]
		NFCVasMode Mode { get; set; }

		[Export ("passTypeIdentifier", ArgumentSemantic.Retain)]
		string PassTypeIdentifier { get; set; }

		[NullAllowed, Export ("url", ArgumentSemantic.Retain)]
		NSUrl Url { get; set; }
	}

	[iOS (13, 0)]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NFCReaderSession), Name = "NFCVASReaderSession")]
	[DisableDefaultCtor]
	[Advice ("Not available for application extensions.")]
	interface NFCVasReaderSession {

		[Export ("initWithVASCommandConfigurations:delegate:queue:")]
		[DesignatedInitializer]
		NativeHandle Constructor (NFCVasCommandConfiguration [] commandConfigurations, INFCVasReaderSessionDelegate @delegate, [NullAllowed] DispatchQueue queue);
	}

	[MacCatalyst (26, 0), NoTV, NoMac, iOS (26, 0)]
	[BaseType (typeof (NFCTagReaderSession))]
	[DisableDefaultCtor]
	interface NFCPaymentTagReaderSession {
		[Export ("initWithDelegate:queue:")]
		NativeHandle Constructor (INFCTagReaderSessionDelegate @delegate, [NullAllowed] DispatchQueue queue);
	}

}
