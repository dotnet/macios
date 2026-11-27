using System;

using Foundation;
using ObjCRuntime;

#if HAS_UIKIT
using XWindow = UIKit.UIWindow;
#else
using XWindow = AppKit.NSWindow;
#endif

namespace GameSave {
	/// <summary>Enumerates the sync states of a synced directory.</summary>
	[iOS (26, 0), Mac (26, 0), MacCatalyst (26, 0)]
	[Native]
	public enum GSSyncState : long {
		/// <summary>The directory is fully synced and ready to use.</summary>
		Ready,
		/// <summary>The directory is available locally, but it isn't fully synced because the device is offline.</summary>
		Offline,
		/// <summary>The directory is local-only and isn't synced to iCloud.</summary>
		Local,
		/// <summary>The directory is syncing and isn't ready yet.</summary>
		Syncing,
		/// <summary>The directory has conflicts with the cloud that the game needs to resolve.</summary>
		Conflicted,
		/// <summary>The directory is in an error state and can't be used.</summary>
		Error,
		/// <summary>The directory is closed.</summary>
		Closed,
	}

	[iOS (26, 0), Mac (26, 0), MacCatalyst (26, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface GSSyncedDirectoryVersion {
		[Export ("isLocal")]
		bool IsLocal { get; }

		[Export ("localizedNameOfSavingComputer")]
		string LocalizedNameOfSavingComputer { get; }

		[Export ("modifiedDate")]
		NSDate ModifiedDate { get; }

		[Export ("url")]
		NSUrl Url { get; }

		[Export ("description")]
		string Description { get; }
	}

	[iOS (26, 0), Mac (26, 0), MacCatalyst (26, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface GSSyncedDirectoryState {
		[Export ("state")]
		GSSyncState State { get; }

		[NullAllowed, Export ("url")]
		NSUrl Url { get; }

		[NullAllowed, Export ("conflictedVersions")]
		GSSyncedDirectoryVersion [] ConflictedVersions { get; }

		[NullAllowed, Export ("error")]
		NSError Error { get; }
	}

	[iOS (26, 0), Mac (26, 0), MacCatalyst (26, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface GSSyncedDirectory {
		[Static]
		[Export ("openDirectoryForContainerIdentifier:")]
		GSSyncedDirectory OpenDirectory ([NullAllowed] string containerIdentifier);

		[Export ("close")]
		void Close ();

		[Async]
		[Export ("triggerPendingUploadWithCompletionHandler:")]
		void TriggerPendingUpload (Action<bool> completion);

		[Export ("resolveConflictsWithVersion:")]
		void ResolveConflicts (GSSyncedDirectoryVersion version);

		[Async]
		[Export ("finishSyncingWithCompletionHandler:")]
		void FinishSyncing (Action completion);

		[Async]
		[Export ("finishSyncing:completionHandler:")]
		void FinishSyncing (XWindow statusDisplay, Action completion);

		[Export ("directoryState")]
		GSSyncedDirectoryState DirectoryState { get; }
	}

}
