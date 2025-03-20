using System;
using ObjCRuntime;
using Foundation;
using CoreFoundation;

#if !MONOMAC
using UIKit;
#else
using AppKit;
using UIViewController = AppKit.NSViewController;
#endif

#if !NET
using NativeHandle = System.IntPtr;
#endif

namespace ScreenTime {

	[iOS (14, 0)]
	[MacCatalyst (14, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface STScreenTimeConfiguration : NSSecureCoding {
		[Export ("enforcesChildRestrictions")]
		bool EnforcesChildRestrictions { get; }
	}

	[iOS (14, 0)]
	[MacCatalyst (14, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface STScreenTimeConfigurationObserver {
		[Export ("initWithUpdateQueue:")]
		[DesignatedInitializer]
		NativeHandle Constructor (DispatchQueue updateQueue);

		[Export ("startObserving")]
		void StartObserving ();

		[Export ("stopObserving")]
		void StopObserving ();

		[NullAllowed, Export ("configuration", ArgumentSemantic.Strong)]
		STScreenTimeConfiguration Configuration { get; }
	}

	[iOS (14, 0)]
	[MacCatalyst (14, 0)]
	[BaseType (typeof (NSObject))]
	interface STWebHistory {
#if !XAMCORE_5_0
		[Obsolete ("Use the 'Create' method instead, because there's no way to return an error from a constructor.")]
		[Export ("initWithBundleIdentifier:error:")]
		NativeHandle Constructor (string bundleIdentifier, [NullAllowed] out NSError error);
#endif

#if XAMCORE_5_0
		[Internal]
#else
		[Internal, Sealed]
#endif
		[Export ("initWithBundleIdentifier:error:")]
		NativeHandle _InitWithBundleIdentifier (string bundleIdentifier, [NullAllowed] out NSError error);

		// STWebHistoryProfileIdentifier is a strongly typed enum, but Apple doesn't define any values for it, so bind as NSString
		[iOS (18, 4), MacCatalyst (18, 4), Mac (15, 4)]
		[Internal]
		[Export ("initWithBundleIdentifier:profileIdentifier:error:")]
		NativeHandle _InitWithBundleIdentifier (string bundleIdentifier, [NullAllowed] /* STWebHistoryProfileIdentifier */ NSString profileIdentifier, [NullAllowed] out NSError error);

		// STWebHistoryProfileIdentifier is a strongly typed enum, but Apple doesn't define any values for it, so bind as NSString
		[iOS (18, 4), MacCatalyst (18, 4), Mac (15, 4)]
		[Export ("initWithProfileIdentifier:")]
		NativeHandle Constructor ([NullAllowed] /* STWebHistoryProfileIdentifier */ NSString profileIdentifier);

		[iOS (18, 4), MacCatalyst (18, 4), Mac (15, 4)]
		[Export ("fetchHistoryDuringInterval:completionHandler:")]
		[Async]
		void FetchHistory (NSDateInterval interval, STWebHistoryFetchHistoryCallback completionHandler);

		[iOS (18, 4), MacCatalyst (18, 4), Mac (15, 4)]
		[Export ("fetchAllHistoryWithCompletionHandler:")]
		[Async]
		void FetchHistory (STWebHistoryFetchHistoryCallback completionHandler);

		[Export ("deleteHistoryForURL:")]
		void DeleteHistory (NSUrl url);

		[Export ("deleteHistoryDuringInterval:")]
		void DeleteHistory (NSDateInterval interval);

		[Export ("deleteAllHistory")]
		void DeleteAllHistory ();
	}

	delegate void STWebHistoryFetchHistoryCallback ([NullAllowed] NSSet<NSUrl> urls, [NullAllowed] NSError error);

	[iOS (14, 0)]
	[MacCatalyst (14, 0)]
	[BaseType (typeof (UIViewController))]
	[DisableDefaultCtor]
	interface STWebpageController {
		[DesignatedInitializer]
		[Export ("initWithNibName:bundle:")]
		NativeHandle Constructor ([NullAllowed] string nibNameOrNull, [NullAllowed] NSBundle nibBundleOrNull);

		[Export ("suppressUsageRecording")]
		bool SuppressUsageRecording { get; set; }

		[NullAllowed, Export ("URL", ArgumentSemantic.Copy)]
		NSUrl Url { get; set; }

		[Export ("URLIsPlayingVideo")]
		bool UrlIsPlayingVideo { get; set; }

		[Export ("URLIsPictureInPicture")]
		bool UrlIsPictureInPicture { get; set; }

		[Export ("URLIsBlocked")]
		bool UrlIsBlocked { get; }

		[Export ("setBundleIdentifier:error:")]
		bool SetBundleIdentifier (string bundleIdentifier, [NullAllowed] out NSError error);

		// STWebHistoryProfileIdentifier is a strongly typed enum, but Apple doesn't define any values for it, so bind as NSString
		[iOS (18, 4), MacCatalyst (18, 4), Mac (15, 4)]
		[Export ("profileIdentifier", ArgumentSemantic.Copy), NullAllowed]
		/* STWebHistoryProfileIdentifier */
		NSString ProfileIdentifier { get; set; }
	}

}
