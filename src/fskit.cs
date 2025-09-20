// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

using CoreFoundation;
using Darwin;
using Foundation;
using ObjCRuntime;

// Let's hope that by .NET 11 we've ironed out all the bugs in the API.
// This can of course be adjusted as needed (until we've released as stable).
#if NET11_0_OR_GREATER
#define STABLE_FSKIT
#endif

using FSDirectoryCookie = System.UInt64;
using FSDirectoryVerifier = System.UInt64;
using FSOperationId = System.UIntPtr; // typedef NSUInteger FSOperationID

namespace FSKit {
#if !STABLE_FSKIT
	[Experimental ("APL0002")]
#endif
	[Mac (15, 4)]
	delegate void FetchInstalledExtensionsCallback ([NullAllowed] FSModuleIdentity [] identities, [NullAllowed] NSError error);

#if !STABLE_FSKIT
	[Experimental ("APL0002")]
#endif
	[Mac (15, 4)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface FSClient {
		[Static]
		[Export ("sharedInstance")]
		FSClient SharedInstance { get; }

		[Async]
		[Export ("fetchInstalledExtensionsWithCompletionHandler:")]
		void FetchInstalledExtensions (FetchInstalledExtensionsCallback results);
	}

#if !STABLE_FSKIT
	[Experimental ("APL0002")]
#endif
	[Mac (15, 4)]
	[Native]
	public enum FSContainerState : long {
		NotReady = 0,
		Blocked,
		Ready,
		Active,
	}

#if !STABLE_FSKIT
	[Experimental ("APL0002")]
#endif
	[Mac (15, 4)]
	[BaseType (typeof (FSEntityIdentifier))]
	interface FSContainerIdentifier {
		[Export ("volumeIdentifier")]
		FSVolumeIdentifier VolumeIdentifier { get; }
	}

#if !STABLE_FSKIT
	[Experimental ("APL0002")]
#endif
	[Mac (15, 4)]
	[BaseType (typeof (NSObject))]
	interface FSEntityIdentifier : INSCopying, INSSecureCoding {
		[Export ("initWithUUID:")]
		NativeHandle Constructor (NSUuid uuid);

		[Export ("initWithUUID:qualifier:")]
		NativeHandle Constructor (NSUuid uuid, ulong qualifier);

		[Export ("initWithUUID:data:")]
		NativeHandle Constructor (NSUuid uuid, NSData qualifier);

		[Export ("uuid", ArgumentSemantic.Retain)]
		NSUuid Uuid { get; set; }

		[NullAllowed, Export ("qualifier", ArgumentSemantic.Retain)]
		NSData Qualifier { get; set; }
	}

#if !STABLE_FSKIT
	[Experimental ("APL0002")]
#endif
	[Mac (15, 4)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface FSFileName : INSSecureCoding, INSCopying {
		[Export ("data")]
		NSData Data { get; }

		[NullAllowed, Export ("string")]
		string String { get; }

		// The C# binding for this ends up being the same as the the initWithCString: selector, which we've already bound.
		// [Export ("initWithCString:")]
		// [DesignatedInitializer]
		// [Internal]
		// NativeHandle Constructor (IntPtr name);

		[Export ("initWithBytes:length:")]
		[Internal]
		NativeHandle _InitWithBytes (IntPtr bytes, nuint length);

		[Export ("initWithData:")]
		NativeHandle Constructor (NSData name);

		[Export ("initWithString:")]
		NativeHandle Constructor (string name);

		// The C# binding for this ends up being the same as the the nameWithString: selector, which we've already bound.
		// [Static]
		// [Export ("nameWithCString:")]
		// [Internal]
		// FSFileName _Create (IntPtr name);

		[Static]
		[Export ("nameWithBytes:length:")]
		[Internal]
		FSFileName _Create (IntPtr bytes, nuint length);

		[Static]
		[Export ("nameWithData:")]
		FSFileName Create (NSData name);

		[Static]
		[Export ("nameWithString:")]
		FSFileName Create (string name);
	}

#if !STABLE_FSKIT
	[Experimental ("APL0002")]
#endif
	delegate void FSFileSystemBaseWipeResourceCompletionHandler ([NullAllowed] NSError error);

#if !STABLE_FSKIT
	[Experimental ("APL0002")]
#endif
	[Mac (15, 4)]
	[Protocol (BackwardsCompatibleCodeGeneration = false)]
	interface FSFileSystemBase {
		[Abstract]
		[Export ("containerStatus", ArgumentSemantic.Copy)]
		FSContainerStatus ContainerStatus { get; set; }

		[Abstract]
		[Export ("wipeResource:completionHandler:")]
		void WipeResource (FSBlockDeviceResource resource, FSFileSystemBaseWipeResourceCompletionHandler completionHandler);
	}

#if !STABLE_FSKIT
	[Experimental ("APL0002")]
#endif
	[Mac (15, 4)]
	[Native]
	public enum FSItemAttribute : long {
		Type = 1L << 0,
		Mode = 1L << 1,
		LinkCount = 1L << 2,
		Uid = 1L << 3,
		Gid = 1L << 4,
		Flags = 1L << 5,
		Size = 1L << 6,
		AllocSize = 1L << 7,
		FileId = 1L << 8,
		ParentId = 1L << 9,
		AccessTime = 1L << 10,
		ModifyTime = 1L << 11,
		ChangeTime = 1L << 12,
		BirthTime = 1L << 13,
		BackupTime = 1L << 14,
		AddedTime = 1L << 15,
		SupportsLimitedXAttrs = 1L << 16,
		InhibitKernelOffloadedIo = 1L << 17,
	}

#if !STABLE_FSKIT
	[Experimental ("APL0002")]
#endif
	[Mac (15, 4)]
	[Native]
	public enum FSItemType : long {
		Unknown = 0,
		File,
		Directory,
		Symlink,
		Fifo,
		CharDevice,
		BlockDevice,
		Socket,
	}

#if !STABLE_FSKIT
	[Experimental ("APL0002")]
#endif
	[Mac (15, 4)]
	[BaseType (typeof (NSObject))]
	interface FSItemAttributes : INSSecureCoding {
		[Export ("invalidateAllProperties")]
		void InvalidateAllProperties ();

		[Export ("uid")]
		uint Uid { get; set; }

		[Export ("gid")]
		uint Gid { get; set; }

		[Export ("mode")]
		uint Mode { get; set; }

		[Export ("type", ArgumentSemantic.Assign)]
		FSItemType Type { get; set; }

		[Export ("linkCount")]
		uint LinkCount { get; set; }

		[Export ("flags")]
		uint Flags { get; set; }

		[Export ("size")]
		ulong Size { get; set; }

		[Export ("allocSize")]
		ulong AllocSize { get; set; }

		[Export ("fileID")]
		FSItemId FileId { get; set; }

		[Export ("parentID")]
		FSItemId ParentId { get; set; }

		[Export ("supportsLimitedXAttrs")]
		bool SupportsLimitedXAttrs { get; set; }

		[Export ("inhibitKernelOffloadedIO")]
		bool InhibitKernelOffloadedIo { get; set; }

		[Export ("modifyTime")]
		TimeSpec ModifyTime { get; set; }

		[Export ("addedTime")]
		TimeSpec AddedTime { get; set; }

		[Export ("changeTime")]
		TimeSpec ChangeTime { get; set; }

		[Export ("accessTime")]
		TimeSpec AccessTime { get; set; }

		[Export ("birthTime")]
		TimeSpec BirthTime { get; set; }

		[Export ("backupTime")]
		TimeSpec BackupTime { get; set; }

		[Export ("isValid:")]
		bool IsValid (FSItemAttribute attribute);
	}

#if !STABLE_FSKIT
	[Experimental ("APL0002")]
#endif
	[Mac (15, 4)]
	[BaseType (typeof (FSItemAttributes))]
	interface FSItemSetAttributesRequest {
		[Export ("consumedAttributes")]
		FSItemAttribute ConsumedAttributes { get; set; }

		[Export ("wasAttributeConsumed:")]
		bool WasAttributeConsumed (FSItemAttribute attribute);
	}

#if !STABLE_FSKIT
	[Experimental ("APL0002")]
#endif
	[Mac (15, 4)]
	[BaseType (typeof (NSObject))]
	interface FSItemGetAttributesRequest : INSSecureCoding {
		[Export ("wantedAttributes")]
		FSItemAttribute WantedAttributes { get; set; }

		[Export ("isAttributeWanted:")]
		bool IsAttributeWanted (FSItemAttribute attribute);
	}

#if !STABLE_FSKIT
	[Experimental ("APL0002")]
#endif
	[Mac (15, 4)]
	[BaseType (typeof (NSObject))]
	interface FSItem {
	}

#if !STABLE_FSKIT
	[Experimental ("APL0002")]
#endif
	[Mac (15, 4)]
	[Partial]
	interface FSConstants {
		[Field ("FSKitVersionNumber")]
		double FSKitVersionNumber { get; }

		[Field ("FSKitVersionString")]
		[Internal]
		IntPtr _FSKitVersionString { get; }

		[Static]
		string FSKitVersionString {
			[Wrap ("Marshal.PtrToStringUTF8 (_FSKitVersionString)!")]
			get;
		}

		[Field ("FSDirectoryCookieInitial")]
		nuint FSDirectoryCookieInitial { get; }

		[Field ("FSDirectoryVerifierInitial")]
		nuint FSDirectoryVerifierInitial { get; }

		[Field ("FSOperationIDUnspecified")]
		FSOperationId FSOperationIdUnspecified { get; }
	}

#if !STABLE_FSKIT
	[Experimental ("APL0002")]
#endif
	delegate void FSMessageConnectionDidCompleteHandler ([NullAllowed] NSError deliveryError);

#if !STABLE_FSKIT
	[Experimental ("APL0002")]
#endif
	[Mac (15, 4)]
	[BaseType (typeof (NSObject))]
	interface FSModuleIdentity {
		[Export ("bundleIdentifier")]
		string BundleIdentifier { get; }

		[Export ("url")]
		NSUrl Url { get; }

		[Export ("enabled")]
		bool Enabled { [Bind ("isEnabled")] get; }
	}

#if !STABLE_FSKIT
	[Experimental ("APL0002")]
#endif
	[Mac (15, 4)]
	[Native]
	public enum FSMatchResult : long {
		NotRecognized = 0,
		Recognized,
		UsableButLimited,
		Usable,
	}

#if !STABLE_FSKIT
	[Experimental ("APL0002")]
#endif
	[Mac (15, 4)]
	[BaseType (typeof (NSObject))]
	interface FSResource : INSSecureCoding {
		[Export ("isRevoked")]
		bool IsRevoked { get; }

		[Export ("makeProxy")]
		FSResource MakeProxy ();

		[Export ("revoke")]
		void Revoke ();
	}

#if !STABLE_FSKIT
	[Experimental ("APL0002")]
#endif
	delegate void FSBlockDeviceResourceReadReplyHandler (nuint actuallyRead, [NullAllowed] NSError error);
#if !STABLE_FSKIT
	[Experimental ("APL0002")]
#endif
	delegate void FSBlockDeviceResourceWriteReplyHandler (nuint actuallyWritten, [NullAllowed] NSError error);

#if !STABLE_FSKIT
	[Experimental ("APL0002")]
#endif
	[Mac (15, 4)]
	[BaseType (typeof (FSResource))]
	[DisableDefaultCtor]
	interface FSBlockDeviceResource {
		[Static]
		[Export ("proxyResourceForBSDName:")]
		[return: NullAllowed]
		FSBlockDeviceResource CreateProxyResource (string bsdName);

		[Static]
		[Export ("proxyResourceForBSDName:isWritable:")]
		[return: NullAllowed]
		FSBlockDeviceResource CreateProxyResource (string bsdName, bool isWritable);

		[Export ("BSDName", ArgumentSemantic.Copy)]
		string BsdName { get; }

		[Export ("writable")]
		bool Writable { [Bind ("isWritable")] get; }

		[Export ("blockSize")]
		ulong BlockSize { get; }

		[Export ("blockCount")]
		ulong BlockCount { get; }

		[Export ("physicalBlockSize")]
		ulong PhysicalBlockSize { get; }

		[Async]
		[Export ("readInto:startingAt:length:completionHandler:")]
		void Read (IntPtr buffer, long offset, nuint length, FSBlockDeviceResourceReadReplyHandler completionHandler);

		[Export ("readInto:startingAt:length:error:")]
		nuint Read (IntPtr buffer, long offset, nuint length, [NullAllowed] out NSError error);

		[Async]
		[Export ("writeFrom:startingAt:length:completionHandler:")]
		void Write (IntPtr buffer, long offset, nuint length, FSBlockDeviceResourceWriteReplyHandler completionHandler);

		[Export ("writeFrom:startingAt:length:error:")]
		nuint Write (IntPtr buffer, long offset, nuint length, [NullAllowed] out NSError error);

		[Export ("metadataReadInto:startingAt:length:error:")]
		bool MetadataRead (IntPtr buffer, long offset, nuint length, [NullAllowed] out NSError error);

		[Export ("metadataWriteFrom:startingAt:length:error:")]
		bool MetadataWrite (IntPtr buffer, long offset, nuint length, [NullAllowed] out NSError error);

		[Export ("delayedMetadataWriteFrom:startingAt:length:error:")]
		bool DelayedMetadataWrite (IntPtr buffer, long offset, nuint length, [NullAllowed] out NSError error);

		[Export ("metadataFlushWithError:")]
		bool MetadataFlush ([NullAllowed] out NSError error);

		[Export ("asynchronousMetadataFlushWithError:")]
		bool AsynchronousMetadataFlush ([NullAllowed] out NSError error);

		[Export ("metadataClear:withDelayedWrites:error:")]
		bool MetadataClear (FSMetadataRange [] rangesToClear, bool withDelayedWrites, [NullAllowed] out NSError error);

		[Export ("metadataPurge:error:")]
		bool MetadataPurge (FSMetadataRange [] rangesToPurge, [NullAllowed] out NSError error);
	}

#if !STABLE_FSKIT
	[Experimental ("APL0002")]
#endif
	delegate void FSManageableResourceMaintenanceOperationsHandler ([NullAllowed] NSProgress progress, [NullAllowed] NSError error);

#if !STABLE_FSKIT
	[Experimental ("APL0002")]
#endif
	[Mac (15, 4)]
	[Protocol (BackwardsCompatibleCodeGeneration = false)]
	interface FSManageableResourceMaintenanceOperations {
		[Abstract]
		[Export ("startCheckWithTask:options:error:")]
		NSProgress StartCheckWithTask (FSTask task, FSTaskOptions options, [NullAllowed] out NSError error);

		[Abstract]
		[Export ("startFormatWithTask:options:error:")]
		[return: NullAllowed]
		NSProgress StartFormat (FSTask task, FSTaskOptions options, [NullAllowed] out NSError error);
	}

#if !STABLE_FSKIT
	[Experimental ("APL0002")]
#endif
	[Mac (15, 4)]
	[DisableDefaultCtor]
	[BaseType (typeof (NSObject))]
	interface FSProbeResult : NSSecureCoding {
		[Export ("result")]
		FSMatchResult Result { get; }

		[Export ("name", ArgumentSemantic.Copy), NullAllowed]
		string Name { get; }

		[Export ("containerID"), NullAllowed]
		FSContainerIdentifier ContainerId { get; }

		[Static]
		[Export ("notRecognizedProbeResult")]
		FSProbeResult NotRecognizedProbeResult { get; }

		[Static]
		[Export ("recognizedProbeResultWithName:containerID:")]
		FSProbeResult CreateRecognizedProbeResult (string name, FSContainerIdentifier containerId);

		[Static]
		[Export ("usableButLimitedProbeResult")]
		FSProbeResult UsableButLimitedProbeResult { get; }

		[Static]
		[Export ("usableButLimitedProbeResultWithName:containerID:")]
		FSProbeResult CreateUsableButLimitedProbeResult (string name, FSContainerIdentifier containerId);

		[Static]
		[Export ("usableProbeResultWithName:containerID:")]
		FSProbeResult CreateUsableProbeResult (string name, FSContainerIdentifier containerId);
	}

#if !STABLE_FSKIT
	[Experimental ("APL0002")]
#endif
	[Mac (15, 4)]
	[BaseType (typeof (NSObject))]
	interface FSUnaryFileSystem : FSFileSystemBase {
	}

#if !STABLE_FSKIT
	[Experimental ("APL0002")]
#endif
	delegate void FSUnaryFileSystemOperationsLoadResourceResult ([NullAllowed] FSVolume volume, [NullAllowed] NSError error);
#if !STABLE_FSKIT
	[Experimental ("APL0002")]
#endif
	delegate void FSUnaryFileSystemOperationsProbeResourceResult ([NullAllowed] FSProbeResult volume, [NullAllowed] NSError error);
#if !STABLE_FSKIT
	[Experimental ("APL0002")]
#endif
	delegate void FSUnaryFileSystemOperationsUnloadResourceResult ([NullAllowed] NSError error);


#if !STABLE_FSKIT
	[Experimental ("APL0002")]
#endif
	[Mac (15, 4)]
	[Protocol (BackwardsCompatibleCodeGeneration = false)]
	interface FSUnaryFileSystemOperations {
		[Abstract]
		[Export ("probeResource:replyHandler:")]
		void ProbeResource (FSResource resource, FSUnaryFileSystemOperationsProbeResourceResult replyHandler);

		[Abstract]
		[Export ("loadResource:options:replyHandler:")]
		void LoadResource (FSResource resource, FSTaskOptions options, FSUnaryFileSystemOperationsLoadResourceResult replyHandler);

		[Abstract]
		[Export ("unloadResource:options:replyHandler:")]
		void UnloadResource (FSResource resource, FSTaskOptions options, FSUnaryFileSystemOperationsUnloadResourceResult reply);

		[Export ("didFinishLoading")]
		void DidFinishLoading ();

	}

#if !STABLE_FSKIT
	[Experimental ("APL0002")]
#endif
	[Mac (15, 4)]
	[BaseType (typeof (FSEntityIdentifier))]
	interface FSVolumeIdentifier {
	}

#if !STABLE_FSKIT
	[Experimental ("APL0002")]
#endif
	[Mac (15, 4)]
	[Native]
	public enum FSDeactivateOptions : long {
		Force = 1 << 0,
	}

#if !STABLE_FSKIT
	[Experimental ("APL0002")]
#endif
	[Mac (15, 4)]
	[BaseType (typeof (NSObject))]
	interface FSVolumeSupportedCapabilities : INSSecureCoding {
		[Export ("supportsPersistentObjectIDs")]
		bool SupportsPersistentObjectIds { get; set; }

		[Export ("supportsSymbolicLinks")]
		bool SupportsSymbolicLinks { get; set; }

		[Export ("supportsHardLinks")]
		bool SupportsHardLinks { get; set; }

		[Export ("supportsJournal")]
		bool SupportsJournal { get; set; }

		[Export ("supportsActiveJournal")]
		bool SupportsActiveJournal { get; set; }

		[Export ("doesNotSupportRootTimes")]
		bool DoesNotSupportRootTimes { get; set; }

		[Export ("supportsSparseFiles")]
		bool SupportsSparseFiles { get; set; }

		[Export ("supportsZeroRuns")]
		bool SupportsZeroRuns { get; set; }

		[Export ("supportsFastStatFS")]
		bool SupportsFastStatFS { get; set; }

		[Export ("supports2TBFiles")]
		bool Supports2TBFiles { get; set; }

		[Export ("supportsOpenDenyModes")]
		bool SupportsOpenDenyModes { get; set; }

		[Export ("supportsHiddenFiles")]
		bool SupportsHiddenFiles { get; set; }

		[Export ("doesNotSupportVolumeSizes")]
		bool DoesNotSupportVolumeSizes { get; set; }

		[Export ("supports64BitObjectIDs")]
		bool Supports64BitObjectIds { get; set; }

		[Export ("supportsDocumentID")]
		bool SupportsDocumentId { get; set; }

		[Export ("doesNotSupportImmutableFiles")]
		bool DoesNotSupportImmutableFiles { get; set; }

		[Export ("doesNotSupportSettingFilePermissions")]
		bool DoesNotSupportSettingFilePermissions { get; set; }

		[Export ("supportsSharedSpace")]
		bool SupportsSharedSpace { get; set; }

		[Export ("supportsVolumeGroups")]
		bool SupportsVolumeGroups { get; set; }

		[Export ("caseFormat", ArgumentSemantic.Assign)]
		FSVolumeCaseFormat CaseFormat { get; set; }
	}

#if !STABLE_FSKIT
	[Experimental ("APL0002")]
#endif
	[Mac (15, 4)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface FSVolume {
		[Export ("volumeID", ArgumentSemantic.Strong)]
		FSVolumeIdentifier VolumeId { get; }

		[Export ("name", ArgumentSemantic.Copy)]
		FSFileName Name { get; set; }

		[DesignatedInitializer]
		[Export ("initWithVolumeID:volumeName:")]
		NativeHandle Constructor (FSVolumeIdentifier volumeId, FSFileName volumeName);
	}

#if !STABLE_FSKIT
	[Experimental ("APL0002")]
#endif
	[Mac (15, 4)]
	[Protocol (BackwardsCompatibleCodeGeneration = false)]
	interface FSVolumePathConfOperations {
		[Abstract]
		[Export ("maximumLinkCount")]
		nint MaximumLinkCount { get; }

		[Abstract]
		[Export ("maximumNameLength")]
		nint MaximumNameLength { get; }

		[Abstract]
		[Export ("restrictsOwnershipChanges")]
		bool RestrictsOwnershipChanges { get; }

		[Abstract]
		[Export ("truncatesLongNames")]
		bool TruncatesLongNames { get; }

		[Export ("maximumXattrSize")]
		nint MaximumXattrSize { get; }

		[Export ("maximumXattrSizeInBits")]
		nint MaximumXattrSizeInBits { get; }

		[Export ("maximumFileSize")]
		ulong MaximumFileSize { get; }

		[Export ("maximumFileSizeInBits")]
		nint MaximumFileSizeInBits { get; }
	}

#if !STABLE_FSKIT
	[Experimental ("APL0002")]
#endif
	interface IFSVolumePathConfOperations { }

#if !STABLE_FSKIT
	[Experimental ("APL0002")]
#endif
	[Mac (15, 4)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface FSStatFSResult : INSSecureCoding {
		[Export ("blockSize")]
		nint BlockSize { get; set; }

		[Export ("ioSize")]
		nint IoSize { get; set; }

		[Export ("totalBlocks")]
		ulong TotalBlocks { get; set; }

		[Export ("availableBlocks")]
		ulong AvailableBlocks { get; set; }

		[Export ("freeBlocks")]
		ulong FreeBlocks { get; set; }

		[Export ("usedBlocks")]
		ulong UsedBlocks { get; set; }

		[Export ("totalBytes")]
		ulong TotalBytes { get; set; }

		[Export ("availableBytes")]
		ulong AvailableBytes { get; set; }

		[Export ("freeBytes")]
		ulong FreeBytes { get; set; }

		[Export ("usedBytes")]
		ulong UsedBytes { get; set; }

		[Export ("totalFiles")]
		ulong TotalFiles { get; set; }

		[Export ("freeFiles")]
		ulong FreeFiles { get; set; }

		[Export ("fileSystemSubType")]
		nint FileSystemSubType { get; set; }

		[Export ("fileSystemTypeName", ArgumentSemantic.Copy)]
		string FileSystemTypeName { get; }

		[Export ("initWithFileSystemTypeName:")]
		NativeHandle Constructor (string fileSystemTypeName);
	}

#if !STABLE_FSKIT
	[Experimental ("APL0002")]
#endif
	delegate void FSVolumeOperationsMountHandler ([NullAllowed] NSError error);

#if !STABLE_FSKIT
	[Experimental ("APL0002")]
#endif
	delegate void FSVolumeOperationsSynchronizeHandler ([NullAllowed] NSError error);

#if !STABLE_FSKIT
	[Experimental ("APL0002")]
#endif
	delegate void FSVolumeOperationsAttributesHandler ([NullAllowed] FSItemAttributes attributes, [NullAllowed] NSError error);

#if !STABLE_FSKIT
	[Experimental ("APL0002")]
#endif
	delegate void FSVolumeOperationsLookupItemHandler ([NullAllowed] FSItem item, [NullAllowed] FSFileName itemName, [NullAllowed] NSError error);

#if !STABLE_FSKIT
	[Experimental ("APL0002")]
#endif
	delegate void FSVolumeOperationsReclaimHandler ([NullAllowed] NSError error);

#if !STABLE_FSKIT
	[Experimental ("APL0002")]
#endif
	delegate void FSVolumeOperationsReadSymbolicLinkHandler ([NullAllowed] FSFileName attributes, [NullAllowed] NSError error);

#if !STABLE_FSKIT
	[Experimental ("APL0002")]
#endif
	delegate void FSVolumeOperationsCreateItemHandler ([NullAllowed] FSItem newItem, [NullAllowed] FSFileName newItemName, [NullAllowed] NSError error);

#if !STABLE_FSKIT
	[Experimental ("APL0002")]
#endif
	delegate void FSVolumeOperationsCreateLinkHandler ([NullAllowed] FSFileName newItemName, [NullAllowed] NSError error);

#if !STABLE_FSKIT
	[Experimental ("APL0002")]
#endif
	delegate void FSVolumeOperationsRemoveItemHandler ([NullAllowed] NSError error);

#if !STABLE_FSKIT
	[Experimental ("APL0002")]
#endif
	delegate void FSVolumeOperationsRenameItemHandler ([NullAllowed] FSFileName newName, [NullAllowed] NSError error);

#if !STABLE_FSKIT
	[Experimental ("APL0002")]
#endif
	delegate void FSVolumeOperationsEnumerateDirectoryHandler (FSDirectoryVerifier currentVerifier, [NullAllowed] NSError error);

#if !STABLE_FSKIT
	[Experimental ("APL0002")]
#endif
	delegate void FSVolumeOperationsActivateHandler ([NullAllowed] FSItem rootItem, [NullAllowed] NSError error);

#if !STABLE_FSKIT
	[Experimental ("APL0002")]
#endif
	delegate void FSVolumeOperationsDeactivateHandler ([NullAllowed] NSError error);

#if !STABLE_FSKIT
	[Experimental ("APL0002")]
#endif
	[Mac (15, 4)]
	[Protocol (BackwardsCompatibleCodeGeneration = false)]
	interface FSVolumeOperations : FSVolumePathConfOperations {
		[Abstract]
		[Export ("supportedVolumeCapabilities")]
		FSVolumeSupportedCapabilities SupportedVolumeCapabilities { get; }

		[Abstract]
		[Export ("volumeStatistics")]
		FSStatFSResult VolumeStatistics { get; }

		[Abstract]
		[Export ("mountWithOptions:replyHandler:")]
		void Mount (FSTaskOptions options, FSVolumeOperationsMountHandler reply);

		[Abstract]
		[Export ("unmountWithReplyHandler:")]
		void Unmount (Action reply);

		[Abstract]
		[Export ("synchronizeWithFlags:replyHandler:")]
		void Synchronize (FSSyncFlags flags, FSVolumeOperationsSynchronizeHandler reply);

		[Abstract]
		[Export ("getAttributes:ofItem:replyHandler:")]
		void GetAttributes (FSItemGetAttributesRequest desiredAttributes, FSItem item, FSVolumeOperationsAttributesHandler reply);

		[Abstract]
		[Export ("setAttributes:onItem:replyHandler:")]
		void SetAttributes (FSItemSetAttributesRequest newAttributes, FSItem item, FSVolumeOperationsAttributesHandler reply);

		[Abstract]
		[Export ("lookupItemNamed:inDirectory:replyHandler:")]
		void LookupItem (FSFileName name, FSItem directory, FSVolumeOperationsLookupItemHandler reply);

		[Abstract]
		[Export ("reclaimItem:replyHandler:")]
		void Reclaim (FSItem item, FSVolumeOperationsReclaimHandler reply);

		[Abstract]
		[Export ("readSymbolicLink:replyHandler:")]
		void ReadSymbolicLink (FSItem item, FSVolumeOperationsReadSymbolicLinkHandler reply);

		[Abstract]
		[Export ("createItemNamed:type:inDirectory:attributes:replyHandler:")]
		void CreateItem (FSFileName name, FSItemType type, FSItem directory, FSItemSetAttributesRequest newAttributes, FSVolumeOperationsCreateItemHandler reply);

		[Abstract]
		[Export ("createSymbolicLinkNamed:inDirectory:attributes:linkContents:replyHandler:")]
		void CreateSymbolicLink (FSFileName name, FSItem directory, FSItemSetAttributesRequest newAttributes, FSFileName contents, FSVolumeOperationsCreateItemHandler reply);

		[Abstract]
		[Export ("createLinkToItem:named:inDirectory:replyHandler:")]
		void CreateLink (FSItem item, FSFileName name, FSItem directory, FSVolumeOperationsCreateLinkHandler reply);

		[Abstract]
		[Export ("removeItem:named:fromDirectory:replyHandler:")]
		void RemoveItem (FSItem item, FSFileName name, FSItem directory, FSVolumeOperationsRemoveItemHandler reply);

		[Abstract]
		[Export ("renameItem:inDirectory:named:toNewName:inDirectory:overItem:replyHandler:")]
		void RenameItem (FSItem item, FSItem sourceDirectory, FSFileName sourceName, FSFileName destinationName, FSItem destinationDirectory, [NullAllowed] FSItem overItem, FSVolumeOperationsRenameItemHandler reply);

		[Abstract]
		[Export ("enumerateDirectory:startingAtCookie:verifier:providingAttributes:usingPacker:replyHandler:")]
		void EnumerateDirectory (FSItem directory, FSDirectoryCookie startingAt, FSDirectoryVerifier verifier, [NullAllowed] FSItemGetAttributesRequest attributes, FSDirectoryEntryPacker packer, FSVolumeOperationsEnumerateDirectoryHandler reply);

		[Abstract]
		[Export ("activateWithOptions:replyHandler:")]
		void Activate (FSTaskOptions options, FSVolumeOperationsActivateHandler reply);

		[Abstract]
		[Export ("deactivateWithOptions:replyHandler:")]
		void Deactivate (FSDeactivateOptions options, FSVolumeOperationsDeactivateHandler reply);

		[Mac (26, 0)]
		[Export ("enableOpenUnlinkEmulation")]
		bool EnableOpenUnlinkEmulation { get; set; }
	}

#if !STABLE_FSKIT
	[Experimental ("APL0002")]
#endif
	[Mac (15, 4)]
	[Native]
	enum FSSetXattrPolicy : ulong {
		AlwaysSet = 0,
		MustCreate = 1,
		MustReplace = 2,
		Delete = 3,
	}

#if !STABLE_FSKIT
	[Experimental ("APL0002")]
#endif
	delegate void FSVolumeXattrOperationsGetHandler ([NullAllowed] NSData value, [NullAllowed] NSError error);

#if !STABLE_FSKIT
	[Experimental ("APL0002")]
#endif
	delegate void FSVolumeXattrOperationsSetHandler ([NullAllowed] NSError error);

#if !STABLE_FSKIT
	[Experimental ("APL0002")]
#endif
	delegate void FSVolumeXattrOperationsListHandler ([NullAllowed] FSFileName [] value, [NullAllowed] NSError error);

#if !STABLE_FSKIT
	[Experimental ("APL0002")]
#endif
	[Mac (15, 4)]
	[Protocol (BackwardsCompatibleCodeGeneration = false)]
	interface FSVolumeXattrOperations {
		[Export ("xattrOperationsInhibited")]
		bool XattrOperationsInhibited { get; set; }

		[return: NullAllowed] // header says to return null instead of empty array
		[Export ("supportedXattrNamesForItem:")]
		FSFileName [] GetSupportedXattrNames (FSItem item);

		[Abstract]
		[Export ("getXattrNamed:ofItem:replyHandler:")]
		void GetXattr (FSFileName name, FSItem item, FSVolumeXattrOperationsGetHandler reply);

		[Abstract]
		[Export ("setXattrNamed:toData:onItem:policy:replyHandler:")]
		void SetXattr (FSFileName name, [NullAllowed] NSData value, FSItem item, FSSetXattrPolicy policy, FSVolumeXattrOperationsSetHandler reply);

		[Abstract]
		[Export ("listXattrsOfItem:replyHandler:")]
		void ListXattrs (FSItem item, FSVolumeXattrOperationsListHandler reply);
	}

#if !STABLE_FSKIT
	[Experimental ("APL0002")]
#endif
	[Mac (15, 4)]
	[Native]
	[Flags]
	enum FSVolumeOpenModes : ulong {
		Read = 0x1,/* FREAD */
		Write = 0x2,
	}

#if !STABLE_FSKIT
	[Experimental ("APL0002")]
#endif
	delegate void FSVolumeOpenCloseOperationsHandler ([NullAllowed] NSError error);

#if !STABLE_FSKIT
	[Experimental ("APL0002")]
#endif
	[Mac (15, 4)]
	[Protocol (BackwardsCompatibleCodeGeneration = false)]
	interface FSVolumeOpenCloseOperations {
		[Export ("openCloseInhibited")]
		bool IsOpenCloseInhibited { [Bind ("isOpenCloseInhibited")] get; set; }

		[Abstract]
		[Export ("openItem:withModes:replyHandler:")]
		void OpenItem (FSItem item, FSVolumeOpenModes mode, FSVolumeOpenCloseOperationsHandler reply);

		[Abstract]
		[Export ("closeItem:keepingModes:replyHandler:")]
		void CloseItem (FSItem item, FSVolumeOpenModes mode, FSVolumeOpenCloseOperationsHandler reply);
	}

#if !STABLE_FSKIT
	[Experimental ("APL0002")]
#endif
	delegate void FSVolumeReadWriteOperationsReadHandler (nuint actuallyRead, [NullAllowed] NSError error);

#if !STABLE_FSKIT
	[Experimental ("APL0002")]
#endif
	delegate void FSVolumeReadWriteOperationsWriteHandler (nuint actuallyWritten, [NullAllowed] NSError error);

#if !STABLE_FSKIT
	[Experimental ("APL0002")]
#endif
	[Mac (15, 4)]
	[Protocol (BackwardsCompatibleCodeGeneration = false)]
	interface FSVolumeReadWriteOperations {
		[Abstract]
		[Export ("readFromFile:offset:length:intoBuffer:replyHandler:")]
		void Read (FSItem item, long offset, nuint length, FSMutableFileDataBuffer buffer, FSVolumeReadWriteOperationsReadHandler reply);

		[Abstract]
		[Export ("writeContents:toFile:atOffset:replyHandler:")]
		void Write (NSData contents, FSItem item, long offset, FSVolumeReadWriteOperationsWriteHandler reply);
	}

#if !STABLE_FSKIT
	[Experimental ("APL0002")]
#endif
	[Mac (15, 4)]
	[Flags]
	[Native]
	enum FSAccessMask : ulong {
		ReadData = (1 << 1),
		ListDirectory = ReadData,
		WriteData = (1 << 2),
		AddFile = WriteData,
		Execute = (1 << 3),
		Search = Execute,
		Delete = (1 << 4),
		AppendData = (1 << 5),
		AddSubdirectory = AppendData,
		DeleteChild = (1 << 6),
		ReadAttributes = (1 << 7),
		WriteAttributes = (1 << 8),
		ReadXattr = (1 << 9),
		WriteXattr = (1 << 10),
		ReadSecurity = (1 << 11),
		WriteSecurity = (1 << 12),
		TakeOwnership = (1 << 13),
	}

#if !STABLE_FSKIT
	[Experimental ("APL0002")]
#endif
	delegate void FSVolumeAccessCheckOperationsCheckAccessHandler (bool shouldAllowAccess, [NullAllowed] NSError error);

#if !STABLE_FSKIT
	[Experimental ("APL0002")]
#endif
	[Mac (15, 4)]
	[Protocol (BackwardsCompatibleCodeGeneration = false)]
	interface FSVolumeAccessCheckOperations {
		[Export ("accessCheckInhibited")]
		bool IsAccessCheckInhibited { [Bind ("isAccessCheckInhibited")] get; set; }

		[Abstract]
		[Export ("checkAccessToItem:requestedAccess:replyHandler:")]
		void RequestedAccess (FSItem theItem, FSAccessMask access, FSVolumeAccessCheckOperationsCheckAccessHandler reply);
	}

#if !STABLE_FSKIT
	[Experimental ("APL0002")]
#endif
	delegate void FSVolumeRenameOperationsSetVolumeNameHandler ([NullAllowed] FSFileName newName, [NullAllowed] NSError error);

#if !STABLE_FSKIT
	[Experimental ("APL0002")]
#endif
	[Mac (15, 4)]
	[Protocol (BackwardsCompatibleCodeGeneration = false)]
	interface FSVolumeRenameOperations {
		[Export ("volumeRenameInhibited")]
		bool IsVolumeRenameInhibited { [Bind ("isVolumeRenameInhibited")] get; set; }

		[Abstract]
		[Export ("setVolumeName:replyHandler:")]
		void SetVolumeName (FSFileName name, FSVolumeRenameOperationsSetVolumeNameHandler reply);
	}

#if !STABLE_FSKIT
	[Experimental ("APL0002")]
#endif
	[Mac (15, 4)]
	[Flags]
	[Native]
	public enum FSBlockmapFlags : ulong {
		Read = 0x100,
		Write = 0x200,
	}

#if !STABLE_FSKIT
	[Experimental ("APL0002")]
#endif
	[Mac (15, 4)]
	[Native]
	public enum FSExtentType : long {
		Data = 0,
		ZeroFill = 1,
	}

#if !STABLE_FSKIT
	[Experimental ("APL0002")]
#endif
	[Mac (15, 4)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface FSExtentPacker {
		[Export ("packExtentWithResource:type:logicalOffset:physicalOffset:length:")]
		bool PackExtent (FSBlockDeviceResource resource, FSExtentType type, long logicalOffset, long physicalOffset, nuint length);
	}

#if !STABLE_FSKIT
	[Experimental ("APL0002")]
#endif
	delegate void FSVolumeKernelOffloadedIoOperationsHandler ([NullAllowed] NSError error);

#if !STABLE_FSKIT
	[Experimental ("APL0002")]
#endif
	delegate void FSVolumeKernelOffloadedIoOperationsCreateFileHandler ([NullAllowed] FSItem newItem, [NullAllowed] FSFileName newItemName, [NullAllowed] NSError error);

#if !STABLE_FSKIT
	[Experimental ("APL0002")]
#endif
	delegate void FSVolumeKernelOffloadedIoOperationsLookupItemHandler ([NullAllowed] FSItem newItem, [NullAllowed] FSFileName newItemName, [NullAllowed] NSError error);

#if !STABLE_FSKIT
	[Experimental ("APL0002")]
#endif
	delegate void FSVolumeKernelOffloadedIoOperationsPreallocateSpaceHandler (nuint bytesAllocated, [NullAllowed] NSError error);

#if !STABLE_FSKIT
	[Experimental ("APL0002")]
#endif
	[Mac (15, 4)]
	[Protocol (Name = "FSVolumeKernelOffloadedIOOperations", BackwardsCompatibleCodeGeneration = false)]
	interface FSVolumeKernelOffloadedIoOperations {
		[Abstract]
		[Export ("blockmapFile:offset:length:flags:operationID:packer:replyHandler:")]
		void BlockmapFile (FSItem item, long offset, nuint lengthIo, FSBlockmapFlags flags, FSOperationId operationId, FSExtentPacker packer, FSVolumeKernelOffloadedIoOperationsHandler reply);

		[Abstract]
		[Export ("completeIOForFile:offset:length:status:flags:operationID:replyHandler:")]
		void CompleteIo (FSItem item, long offset, nuint length, NSError status, FSCompleteIoFlags flags, FSOperationId operationId, FSVolumeKernelOffloadedIoOperationsHandler reply);

		[Abstract]
		[Export ("createFileNamed:inDirectory:attributes:packer:replyHandler:")]
		void CreateFile (FSFileName name, FSItem directory, FSItemSetAttributesRequest newAttributes, FSExtentPacker packer, FSVolumeKernelOffloadedIoOperationsCreateFileHandler reply);

		[Abstract]
		[Export ("lookupItemNamed:inDirectory:packer:replyHandler:")]
		void LookupItem (FSFileName name, FSItem directory, FSExtentPacker packer, FSVolumeKernelOffloadedIoOperationsLookupItemHandler reply);

		[Export ("preallocateSpaceForFile:atOffset:length:flags:packer:replyHandler:")]
		void PreallocateSpace (FSItem file, long offset, nuint length, FSPreallocateFlags flags, FSExtentPacker packer, FSVolumeKernelOffloadedIoOperationsPreallocateSpaceHandler reply);
	}

#if !STABLE_FSKIT
	[Experimental ("APL0002")]
#endif
	[Mac (15, 4)]
	[Flags]
	[Native]
	enum FSPreallocateFlags : ulong {
		Contiguous = 0x2,
		All = 0x4,
		Persist = 0x8,
		FromEof = 0x10,
	}

#if !STABLE_FSKIT
	[Experimental ("APL0002")]
#endif
	delegate void FSVolumePreallocateOperationsHandler (nuint bytesAllocated, [NullAllowed] NSError error);

#if !STABLE_FSKIT
	[Experimental ("APL0002")]
#endif
	[Mac (15, 4)]
	[Protocol (BackwardsCompatibleCodeGeneration = false)]
	interface FSVolumePreallocateOperations {
		[Export ("preallocateInhibited")]
		bool IsPreallocateInhibited { [Bind ("isPreallocateInhibited")] get; set; }

		[Abstract]
		[Export ("preallocateSpaceForItem:atOffset:length:flags:replyHandler:")]
		void PreallocateSpace (FSItem item, long offset, nuint length, FSPreallocateFlags flags, FSVolumePreallocateOperationsHandler reply);
	}

#if !STABLE_FSKIT
	[Experimental ("APL0002")]
#endif
	[Mac (15, 4)]
	[Flags]
	[Native]
	[NativeName ("FSCompleteIOFlags")]
	public enum FSCompleteIoFlags : ulong {
		Read = FSBlockmapFlags.Read,
		Write = FSBlockmapFlags.Write,
		Async = 0x400,
	}

#if !STABLE_FSKIT
	[Experimental ("APL0002")]
#endif
	[ErrorDomain ("FSKitErrorDomain")]
	[Mac (15, 4)]
	[Native]
	public enum FSErrorCode : long {
		ModuleLoadFailed = 4500,
		ResourceUnrecognized = 4501,
		ResourceDamaged = 4502,
		ResourceUnusable = 4503,
		StatusOperationInProgress = 4504,
		StatusOperationPaused = 4505,
		InvalidDirectoryCookie = 4506,
	}

#if !STABLE_FSKIT
	[Experimental ("APL0002")]
#endif
	[Mac (15, 4)]
	[Flags]
	[Native]
	public enum FSItemDeactivationOptions : ulong {
		Never = 0x0,
		Always = ulong.MaxValue,
		ForRemovedItems = (1uL << 0),
		ForPreallocatedItems = (1uL << 1)
	}

#if !STABLE_FSKIT
	[Experimental ("APL0002")]
#endif
	[Mac (15, 4)]
	public enum FSItemId : ulong {
		Invalid = 0,
		ParentOfRoot = 1,
		RootDirectory = 2,
	}

#if !STABLE_FSKIT
	[Experimental ("APL0002")]
#endif
	[Mac (15, 4)]
	[Native]
	public enum FSSyncFlags : long {
		Wait = 1,
		NoWait = 2,
		DWait = 4,
	}

#if !STABLE_FSKIT
	[Experimental ("APL0002")]
#endif
	[Mac (15, 4)]
	[Native]
	public enum FSVolumeCaseFormat : long {
		Sensitive = 0,
		Insensitive = 1,
		InsensitiveCasePreserving = 2,
	}

#if !STABLE_FSKIT
	[Experimental ("APL0002")]
#endif
	delegate void FSVolumeItemDeactivationDeactivateItemHandler ([NullAllowed] NSError error);

#if !STABLE_FSKIT
	[Experimental ("APL0002")]
#endif
	[Mac (15, 4)]
	[Protocol (BackwardsCompatibleCodeGeneration = false)]
	interface FSVolumeItemDeactivation {
		[Abstract]
		[Export ("itemDeactivationPolicy")]
		FSItemDeactivationOptions ItemDeactivationPolicy { get; }

		[Abstract]
		[Export ("deactivateItem:replyHandler:")]
		void DeactivateItem (FSItem item, FSVolumeItemDeactivationDeactivateItemHandler reply);
	}

#if !STABLE_FSKIT
	[Experimental ("APL0002")]
#endif
	[Mac (15, 4)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface FSContainerStatus : NSCopying {
		[Export ("state")]
		FSContainerState State { get; }

		[NullAllowed, Export ("status", ArgumentSemantic.Copy)]
		NSError Status { get; }

		[Static]
		[Export ("active")]
		FSContainerStatus Active { get; }

		[Static]
		[Export ("activeWithStatus:")]
		FSContainerStatus CreateActive (NSError errorStatus);

		[Static]
		[Export ("blockedWithStatus:")]
		FSContainerStatus CreateBlocked (NSError errorStatus);

		[Static]
		[Export ("notReadyWithStatus:")]
		FSContainerStatus CreateNotReady (NSError errorStatus);

		[Static]
		[Export ("ready")]
		FSContainerStatus Ready { get; }

		[Static]
		[Export ("readyWithStatus:")]
		FSContainerStatus CreateReady (NSError errorStatus);
	}

#if !STABLE_FSKIT
	[Experimental ("APL0002")]
#endif
	[Mac (15, 4)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface FSDirectoryEntryPacker {
		[Export ("packEntryWithName:itemType:itemID:nextCookie:attributes:")]
		bool PackEntry (FSFileName name, FSItemType itemType, FSItemId itemId, FSDirectoryCookie nextCookie, [NullAllowed] FSItemAttributes attributes);
	}

#if !STABLE_FSKIT
	[Experimental ("APL0002")]
#endif
	[Mac (15, 4)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface FSMetadataRange {
		[Export ("startOffset")]
		long StartOffset { get; }

		[Export ("segmentLength")]
		ulong SegmentLength { get; }

		[Export ("segmentCount")]
		ulong SegmentCount { get; }

		[Export ("initWithOffset:segmentLength:segmentCount:")]
		NativeHandle Constructor (long startOffset, ulong segmentLength, ulong segmentCount);

		[Static]
		[Export ("rangeWithOffset:segmentLength:segmentCount:")]
		FSMetadataRange Create (long startOffset, ulong segmentLength, ulong segmentCount);
	}

#if !STABLE_FSKIT
	[Experimental ("APL0002")]
#endif
	[Mac (15, 4)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface FSMutableFileDataBuffer {
		[Export ("length")]
		nuint Length { get; }

		[Export ("mutableBytes")]
		IntPtr MutableBytes { get; }
	}

#if !STABLE_FSKIT
	[Experimental ("APL0002")]
#endif
	[Mac (15, 4)]
	[BaseType (typeof (NSObject))]
	interface FSTask {
		[Export ("logMessage:")]
		void LogMessage (string str);

		[Export ("didCompleteWithError:")]
		void DidComplete ([NullAllowed] NSError error);

		[Mac (26, 0)]
		[NullAllowed, Export ("cancellationHandler", ArgumentSemantic.Copy)]
		FSTaskCancellationCallback CancellationHandler { get; set; }
	}

#if !STABLE_FSKIT
	[Experimental ("APL0002")]
#endif
	[return: NullAllowed]
	delegate NSError FSTaskCancellationCallback ();

#if !STABLE_FSKIT
	[Experimental ("APL0002")]
#endif
	[Mac (15, 4)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface FSTaskOptions {
		[Export ("taskOptions", ArgumentSemantic.Copy)]
		string [] TaskOptions { get; }

		[Export ("urlForOption:")]
		[return: NullAllowed]
		NSUrl GetUrl (string option);
	}

#if !STABLE_FSKIT
	[Experimental ("APL0002")]
#endif
	[Mac (26, 0)]
	[BaseType (typeof (FSResource), Name = "FSGenericURLResource")]
	[DisableDefaultCtor]
	interface FSGenericUrlResource {
		[Export ("url", ArgumentSemantic.Copy)]
		NSUrl Url { get; }

		[Export ("initWithURL:")]
		NativeHandle Constructor (NSUrl url);
	}

#if !STABLE_FSKIT
	[Experimental ("APL0002")]
#endif
	[Mac (26, 0)]
	[BaseType (typeof (FSResource), Name = "FSPathURLResource")]
	[DisableDefaultCtor]
	interface FSPathUrlResource {
		[Export ("url", ArgumentSemantic.Copy)]
		NSUrl Url { get; }

		[Export ("initWithURL:writable:")]
		NativeHandle Constructor (NSUrl url, bool writable);

		[Export ("writable")]
		bool Writable { [Bind ("isWritable")] get; }
	}
}
