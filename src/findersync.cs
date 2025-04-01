using System;
using Foundation;
using ObjCRuntime;
using AppKit;

namespace FinderSync {
	delegate void GetValuesCompletionHandler (NSDictionary<NSString, NSObject> values, NSError error);

	[BaseType (typeof (NSExtensionContext))]
	interface FIFinderSyncController : NSSecureCoding, NSCopying {
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Static]
		[Export ("defaultController")]
		FIFinderSyncController DefaultController { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NullAllowed] // null_resettable
		[Export ("directoryURLs", ArgumentSemantic.Copy)]
		NSSet DirectoryUrls { get; set; }

		/// <param name="image">To be added.</param>
		///         <param name="label">To be added.</param>
		///         <param name="badgeID">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("setBadgeImage:label:forBadgeIdentifier:")]
		void SetBadgeImage (NSImage image, [NullAllowed] string label, string badgeID);

		/// <param name="badgeID">To be added.</param>
		///         <param name="url">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("setBadgeIdentifier:forURL:")]
		void SetBadgeIdentifier (string badgeID, NSUrl url);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("targetedURL")]
		NSUrl TargetedURL { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("selectedItemURLs")]
		NSUrl [] SelectedItemURLs { get; }

		/// <param name="itemUrl">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("lastUsedDateForItemWithURL:")]
		[return: NullAllowed]
		NSDate GetLastUsedDate (NSUrl itemUrl);

		/// <param name="lastUsedDate">To be added.</param>
		///         <param name="itemUrl">To be added.</param>
		///         <param name="completion">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Async, Export ("setLastUsedDate:forItemWithURL:completion:")]
		void SetLastUsedDate (NSDate lastUsedDate, NSUrl itemUrl, Action<NSError> completion);

		/// <param name="itemUrl">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("tagDataForItemWithURL:")]
		[return: NullAllowed]
		NSData GetTagData (NSUrl itemUrl);

		/// <param name="tagData">To be added.</param>
		///         <param name="itemUrl">To be added.</param>
		///         <param name="completion">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Async]
		[Export ("setTagData:forItemWithURL:completion:")]
		void SetTagData ([NullAllowed] NSData tagData, NSUrl itemUrl, Action<NSError> completion);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Static]
		[Export ("extensionEnabled")]
		bool ExtensionEnabled { [Bind ("isExtensionEnabled")] get; }

		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Static]
		[Export ("showExtensionManagementInterface")]
		void ShowExtensionManagementInterface ();
	}

	[Protocol (Name = "FIFinderSync")]
	interface FIFinderSyncProtocol {
		[Export ("menuForMenuKind:")]
		[return: NullAllowed]
		NSMenu GetMenu (FIMenuKind menuKind);

		[Export ("beginObservingDirectoryAtURL:")]
		void BeginObservingDirectory (NSUrl url);

		[Export ("endObservingDirectoryAtURL:")]
		void EndObservingDirectory (NSUrl url);

		[Export ("requestBadgeIdentifierForURL:")]
		void RequestBadgeIdentifier (NSUrl url);

		[Export ("toolbarItemName")]
		string ToolbarItemName { get; }

		[Export ("toolbarItemImage", ArgumentSemantic.Copy)]
		NSImage ToolbarItemImage { get; }

		[Export ("toolbarItemToolTip")]
		string ToolbarItemToolTip { get; }

		[Export ("supportedServiceNamesForItemWithURL:")]
		string [] SupportedServiceNames (NSUrl itemUrl);

#if FALSE // TODO: Activate after 10.13 foundation APIs have been merged.  Bug 57800
		[Export ("makeListenerEndpointForServiceName:andReturnError:")]
		[return: NullAllowed]
		NSXpcListenerEndpoint MakeListenerEndpoint (string serviceName, [NullAllowed] out NSError error);
#endif
		[Async, Export ("valuesForAttributes:forItemWithURL:completion:")]
		void GetValues (string [] attributes, NSUrl itemUrl, GetValuesCompletionHandler completion);
	}

	[BaseType (typeof (NSObject))]
	interface FIFinderSync : NSExtensionRequestHandling, FIFinderSyncProtocol {
	}
}
