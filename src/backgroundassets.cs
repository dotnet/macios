//
// BackgroundAssets C# bindings
//
// Authors:
//	Manuel de la Pena Saenz <mandel@microsoft.com>
//
// Copyright 2022 Microsoft Corporation All rights reserved.
//

using System;

using CoreFoundation;
using Foundation;
using ObjCRuntime;

namespace BackgroundAssets {
	[TV (18, 4), Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0)]
	[Native]
	public enum BADownloadState : long {
		Failed = -1,
		Created = 0,
		Waiting,
		Downloading,
		Finished,
	}

	[TV (18, 4), Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0)]
	[Native]
	public enum BAContentRequest : long {
		Install = 1,
		Update,
		Periodic,
	}

	[TV (18, 4), Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0)]
	[ErrorDomain ("BAErrorDomain")]
	[Native]
	public enum BAErrorCode : long {
		DownloadInvalid = 0,
		CallFromExtensionNotAllowed = 50,
		CallFromInactiveProcessNotAllowed = 51,
		CallerConnectionNotAccepted = 55,
		CallerConnectionInvalid = 56,
		DownloadAlreadyScheduled = 100,
		DownloadNotScheduled = 101,
		DownloadFailedToStart = 102,
		DownloadAlreadyFailed = 103,
		DownloadEssentialDownloadNotPermitted = 109,
		DownloadBackgroundActivityProhibited = 111,
		DownloadWouldExceedAllowance = 112,
		DownloadDoesNotExist = 113,
		SessionDownloadDisallowedByDomain = 202,
		SessionDownloadDisallowedByAllowance = 203,
		SessionDownloadAllowanceExceeded = 204,
		SessionDownloadNotPermittedBeforeAppLaunch = 206,
	}

	[TV (26, 0), iOS (26, 0), MacCatalyst (26, 0), Mac (26, 0)]
	[Flags]
	[Native]
	public enum BAAssetPackStatus : ulong {
		DownloadAvailable = 1uL << 0,
		UpdateAvailable = 1uL << 1,
		UpToDate = 1uL << 2,
		OutOfDate = 1uL << 3,
		Obsolete = 1uL << 4,
		Downloading = 1uL << 5,
		Downloaded = 1uL << 6,
	}

	[TV (26, 0), iOS (26, 0), MacCatalyst (26, 0), Mac (26, 0)]
	[ErrorDomain ("BAManagedErrorDomain")]
	[Native]
	public enum BAManagedErrorCode : long {
		AssetPackNotFound,
		FileNotFound,
	}

	[TV (18, 4), Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface BADownload : NSCoding, NSSecureCoding, NSCopying {
		[Export ("state")]
		BADownloadState State { get; }

		[Export ("identifier")]
		string Identifier { get; }

		[Export ("uniqueIdentifier")]
		string UniqueIdentifier { get; }

		[Export ("priority")]
		nint Priority { get; }

		[Mac (13, 3), iOS (16, 4), MacCatalyst (16, 4)]
		[Export ("isEssential")]
		bool IsEssential { get; }

		[Mac (13, 3), iOS (16, 4), MacCatalyst (16, 4)]
		[return: Release]
		[Export ("copyAsNonEssential")]
		BADownload CopyAsNonEssential ();
	}

	[TV (18, 4), Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface BAAppExtensionInfo : NSSecureCoding {

		[Mac (13, 0), iOS (16, 1), MacCatalyst (16, 1)]
		[NullAllowed]
		[Export ("restrictedDownloadSizeRemaining", ArgumentSemantic.Strong)]
		NSNumber RestrictedDownloadSizeRemaining { get; }

		[Mac (13, 3), iOS (16, 4), MacCatalyst (16, 4)]
		[NullAllowed]
		[Export ("restrictedEssentialDownloadSizeRemaining", ArgumentSemantic.Strong)]
		NSNumber RestrictedEssentialDownloadSizeRemaining { get; }
	}

	[TV (18, 4), Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0)]
	[Protocol]
	interface BADownloaderExtension {

		[NoTV]
		[Deprecated (PlatformName.iOS, 16, 4, message: "'WillTerminate' will not be called in all applicable scenarios, do not rely on it.")]
		[Deprecated (PlatformName.MacOSX, 13, 3, message: "'WillTerminate' will not be invoked in all applicable scenarios, do not rely on it.")]
		[Deprecated (PlatformName.MacCatalyst, 16, 4, message: "'WillTerminate' will not be invoked in all applicable scenarios, do not rely on it.")]
		[Export ("extensionWillTerminate")]
		void WillTerminate ();

		[Export ("backgroundDownload:didReceiveChallenge:completionHandler:")]
		void DidReceiveChallenge (BADownload download, NSUrlAuthenticationChallenge challenge, Action<NSUrlSessionAuthChallengeDisposition, NSUrlCredential> completionHandler);

		[Export ("backgroundDownload:failedWithError:")]
		void Failed (BADownload download, NSError error);

		[Export ("backgroundDownload:finishedWithFileURL:")]
		void Finished (BADownload download, NSUrl fileUrl);

		[Export ("downloadsForRequest:manifestURL:extensionInfo:")]
		NSSet<BADownload> GetDownloads (BAContentRequest contentRequest, NSUrl manifestUrl, BAAppExtensionInfo extensionInfo);
	}

	interface IBADownloadManagerDelegate { }

	[TV (18, 4), Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0)]
	[Protocol]
	[Model]
	[BaseType (typeof (NSObject))]
	interface BADownloadManagerDelegate {
		[Export ("downloadDidBegin:")]
		void DidBegin (BADownload download);

		[Export ("downloadDidPause:")]
		void DidPause (BADownload download);

		[Export ("download:didWriteBytes:totalBytesWritten:totalBytesExpectedToWrite:")]
		void DidWriteBytes (BADownload download, long bytesWritten, long totalBytesWritten, long totalExpectedBytes);

		[Export ("download:didReceiveChallenge:completionHandler:")]
		void DidReceiveChallenge (BADownload download, NSUrlAuthenticationChallenge challenge, Action<NSUrlSessionAuthChallengeDisposition, NSUrlCredential> completionHandler);

		[Export ("download:failedWithError:")]
		void Failed (BADownload download, NSError error);

		[Export ("download:finishedWithFileURL:")]
		void Finished (BADownload download, NSUrl fileUrl);
	}

	[TV (18, 4), Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface BADownloadManager {
		[Static]
		[Export ("sharedManager", ArgumentSemantic.Strong)]
		BADownloadManager SharedManager { get; }

		[Wrap ("WeakDelegate")]
		[NullAllowed]
		IBADownloadManagerDelegate Delegate { get; set; }

		[NullAllowed, Export ("delegate", ArgumentSemantic.Weak)]
		NSObject WeakDelegate { get; set; }

		[Mac (13, 3), iOS (16, 4), MacCatalyst (16, 4)]
		[Export ("fetchCurrentDownloads:")]
		[return: NullAllowed]
		BADownload [] FetchCurrentDownloads ([NullAllowed] out NSError error);

		[Async]
		[Export ("fetchCurrentDownloadsWithCompletionHandler:")]
		void FetchCurrentDownloads (Action<NSArray<BADownload>, NSError> completionHandler);

		[Export ("scheduleDownload:error:")]
		bool ScheduleDownload (BADownload download, [NullAllowed] out NSError outError);

		[Export ("performWithExclusiveControl:")]
		void PerformWithExclusiveControl (Action<NSError> performHandler);

		[Export ("startForegroundDownload:error:")]
		bool StartForegroundDownload (BADownload download, [NullAllowed] out NSError outError);

		[Export ("cancelDownload:error:")]
		bool CancelDownload (BADownload download, [NullAllowed] out NSError error);

		[MacCatalyst (16, 1), iOS (16, 1)]
		[Export ("performWithExclusiveControlBeforeDate:performHandler:")]
		void PerformWithExclusiveControlBeforeDate (NSDate date, Action<bool, NSError> performHandler);
	}

	[TV (18, 4), Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0)]
	[BaseType (typeof (BADownload), Name = "BAURLDownload")]
	[DisableDefaultCtor]
	interface BAUrlDownload {

		[Field ("BADownloaderPriorityMin")]
		nint MinPriority { get; }

		[Field ("BADownloaderPriorityDefault")]
		nint DefaultPriority { get; }

		[Field ("BADownloaderPriorityMax")]
		nint MaxPriority { get; }

		[NoTV]
		[Deprecated (PlatformName.iOS, 16, 4)]
		[Deprecated (PlatformName.MacOSX, 13, 3)]
		[Deprecated (PlatformName.MacCatalyst, 16, 4)]
		[Export ("initWithIdentifier:request:applicationGroupIdentifier:")]
		NativeHandle Constructor (string identifier, NSUrlRequest request, string applicationGroupIdentifier);

		[NoTV]
		[Deprecated (PlatformName.iOS, 16, 4)]
		[Deprecated (PlatformName.MacOSX, 13, 3)]
		[Deprecated (PlatformName.MacCatalyst, 16, 4)]
		[Export ("initWithIdentifier:request:applicationGroupIdentifier:priority:")]
		NativeHandle Constructor (string identifier, NSUrlRequest request, string applicationGroupIdentifier, nint priority);

		[Mac (13, 3), iOS (16, 4), MacCatalyst (16, 4)]
		[Export ("initWithIdentifier:request:fileSize:applicationGroupIdentifier:")]
		NativeHandle Constructor (string identifier, NSUrlRequest request, nuint fileSize, string applicationGroupIdentifier);

		[Mac (13, 3), iOS (16, 4), MacCatalyst (16, 4)]
		[Export ("initWithIdentifier:request:essential:fileSize:applicationGroupIdentifier:priority:")]
		[DesignatedInitializer]
		NativeHandle Constructor (string identifier, NSUrlRequest request, bool essential, nuint fileSize, string applicationGroupIdentifier, nint priority);
	}

	[TV (26, 0), iOS (26, 0), MacCatalyst (26, 0), Mac (26, 0)]
	[Protocol (BackwardsCompatibleCodeGeneration = false), Model]
	[BaseType (typeof (NSObject))]
	interface BAManagedAssetPackDownloadDelegate {
		[Export ("downloadOfAssetPackBegan:")]
		void DownloadBegan (BAAssetPack assetPack);

		[Export ("downloadOfAssetPack:hasProgress:")]
		void DownloadProgress (BAAssetPack assetPack, NSProgress progress);

		[Export ("downloadOfAssetPackPaused:")]
		void DownloadPaused (BAAssetPack assetPack);

		[Export ("downloadOfAssetPackFinished:")]
		void DownloadFinished (BAAssetPack assetPack);

		[Export ("downloadOfAssetPack:failedWithError:")]
		void DownloadFailed (BAAssetPack assetPack, NSError error);
	}

	interface IBAManagedAssetPackDownloadDelegate { }

	[TV (26, 0), iOS (26, 0), MacCatalyst (26, 0), Mac (26, 0)]
	[Protocol (BackwardsCompatibleCodeGeneration = false)]
	interface BAManagedDownloaderExtension : BADownloaderExtension {
		[Export ("shouldDownloadAssetPack:")]
		bool ShouldDownload (BAAssetPack assetPack);
	}

	[TV (26, 0), iOS (26, 0), MacCatalyst (26, 0), Mac (26, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface BAAssetPack {
		[Export ("identifier")]
		string Identifier { get; }

		[Export ("downloadSize")]
		nint DownloadSize { get; }

		[Export ("version")]
		nint Version { get; }

		[NullAllowed, Export ("userInfo", ArgumentSemantic.Copy)]
		NSData UserInfo { get; }

		[Export ("download")]
		BADownload Download ();

		[Export ("downloadForContentRequest:")]
		BADownload Download (BAContentRequest contentRequest);

		[TV (26, 0), iOS (26, 0), MacCatalyst (26, 0), Mac (26, 0)]
		[Field ("BAAssetPackIdentifierErrorKey")]
		NSString IdentifierErrorKey { get; }
	}

	delegate void BAAssetPackManagerGetAllAssetPacksCompletionHandler ([NullAllowed] NSSet<BAAssetPack> assetPacks, [NullAllowed] NSError error);
	delegate void BAAssetPackManagerGetAssetPackCompletionHandler ([NullAllowed] BAAssetPack assetPack, [NullAllowed] NSError error);
	delegate void BAAssetPackManagerGetStatusCompletionHandler ([NullAllowed] BAAssetPackStatus status, [NullAllowed] NSError error);
	delegate void BAAssetPackManagerEnsureLocalAvailabilityCompletionHandler ([NullAllowed] NSError error);
	delegate void BAAssetPackManagerCheckForUpdatesCompletionHandler ([NullAllowed] NSSet<NSString> updatingIdentifiers, [NullAllowed] NSSet<NSString> removedIdentifiers, [NullAllowed] NSError error);
	delegate void BAAssetPackManagerRemoveAssetPackCompletionHandler ([NullAllowed] NSError error);

	[TV (26, 0), iOS (26, 0), MacCatalyst (26, 0), Mac (26, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface BAAssetPackManager {
		[Static]
		[Export ("sharedManager", ArgumentSemantic.Strong)]
		BAAssetPackManager SharedManager { get; }

		[Wrap ("WeakDelegate")]
		[NullAllowed]
		IBAManagedAssetPackDownloadDelegate Delegate { get; set; }

		[NullAllowed, Export ("delegate", ArgumentSemantic.Weak)]
		NSObject WeakDelegate { get; set; }

		[Export ("getAllAssetPacksWithCompletionHandler:")]
		[Async]
		void GetAllAssetPacks (BAAssetPackManagerGetAllAssetPacksCompletionHandler completionHandler);

		[Export ("getAssetPackWithIdentifier:completionHandler:")]
		[Async]
		void GetAssetPack (string assetPackIdentifier, BAAssetPackManagerGetAssetPackCompletionHandler completionHandler);

		[Export ("getStatusOfAssetPackWithIdentifier:completionHandler:")]
		[Async]
		void GetStatus (string assetPackIdentifier, BAAssetPackManagerGetStatusCompletionHandler completionHandler);

		[Export ("ensureLocalAvailabilityOfAssetPack:completionHandler:")]
		[Async]
		void EnsureLocalAvailability (BAAssetPack assetPack, BAAssetPackManagerEnsureLocalAvailabilityCompletionHandler completionHandler);

		[Export ("checkForUpdatesWithCompletionHandler:")]
		[Async (ResultTypeName = "BAAssetPackManagerCheckForUpdatesResult")]
		void CheckForUpdates ([NullAllowed] BAAssetPackManagerCheckForUpdatesCompletionHandler completionHandler);

		[Export ("contentsAtPath:searchingInAssetPackWithIdentifier:options:error:")]
		[return: NullAllowed]
		NSData GetContents (string path, [NullAllowed] string assetPackIdentifier, NSDataReadingOptions options, [NullAllowed] out NSError error);

		[Export ("fileDescriptorForPath:searchingInAssetPackWithIdentifier:error:")]
		int GetFileDescriptor (string path, [NullAllowed] string assetPackIdentifier, [NullAllowed] out NSError error);

		[Export ("URLForPath:error:")]
		[return: NullAllowed]
		NSUrl GetUrl (string path, [NullAllowed] out NSError error);

		[Export ("removeAssetPackWithIdentifier:completionHandler:")]
		[Async]
		void RemoveAssetPack (string assetPackIdentifier, [NullAllowed] BAAssetPackManagerRemoveAssetPackCompletionHandler completionHandler);
	}

	[TV (26, 0), iOS (26, 0), MacCatalyst (26, 0), Mac (26, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface BAAssetPackManifest {
		[Export ("assetPacks", ArgumentSemantic.Copy)]
		NSSet<BAAssetPack> AssetPacks { get; }

		[Internal]
		[Export ("initWithContentsOfURL:applicationGroupIdentifier:error:")]
		NativeHandle _InitWithContentsOfUrl (NSUrl url, string applicationGroupIdentifier, [NullAllowed] out NSError error);

		[Internal]
		[Export ("initFromData:applicationGroupIdentifier:error:")]
		NativeHandle _InitFromData (NSData data, string applicationGroupIdentifier, [NullAllowed] out NSError error);

		[Export ("allDownloads")]
		NSSet<BADownload> GetAllDownloads ();

		// -(NSSet<BADownload *> * _Nonnull)allDownloadsForContentRequest:(BAContentRequest)contentRequest;
		[Export ("allDownloadsForContentRequest:")]
		NSSet<BADownload> GetAllDownloads (BAContentRequest contentRequest);
	}

}
