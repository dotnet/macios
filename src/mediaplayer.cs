//
// This file describes the API that the generator will produce
//
// Authors:
//   Miguel de Icaza
//
// Copyright 2009, Novell, Inc.
// Copyright 2011-2015, Xamarin Inc
//
using System.ComponentModel;
using AVFoundation;
using ObjCRuntime;
using Foundation;
using CoreFoundation;
using CoreGraphics;
using CoreLocation;
using CoreMedia;
#if MONOMAC
using AppKit;
#else
using UIKit;
#endif
using System;

#if MONOMAC
using UIColor = AppKit.NSImage;
using UIControlState = Foundation.NSObject;
using UIImage = AppKit.NSImage;
using UIInterfaceOrientation = Foundation.NSObject;
using UIView = AppKit.NSView;
using UIViewAnimationCurve = Foundation.NSObject;
using UIViewController = AppKit.NSViewController;
#else
using NSImage = UIKit.UIImage;
#endif

#if !NET
using NativeHandle = System.IntPtr;
#endif

namespace MediaPlayer {
	[BaseType (typeof (NSObject))]
#if !MONOMAC
#if NET
#endif // NET
	[TV (14, 0)]
	[MacCatalyst (13, 1)]
	interface MPMediaEntity : NSSecureCoding {
#else
	interface MPMediaItem : NSSecureCoding {
#endif // !MONOMAC
		/// <param name="property">To be added.</param>
		///         <summary>Returns a Boolean value that tells whether the specified <paramref name="property" /> can be used in a media property predicate.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Static]
		[Export ("canFilterByProperty:")]
		bool CanFilterByProperty (NSString property);

		/// <param name="property">To be added.</param>
		///         <summary>Returns the value for the specified <paramref name="property" />.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("valueForProperty:")]
		[return: NullAllowed]
		NSObject ValueForProperty (NSString property);

		/// <param name="propertiesToEnumerate">To be added.</param>
		///         <param name="enumerator">To be added.</param>
		///         <summary>Runs the provided <paramref name="enumerator" /> on the values for the specified properties.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("enumerateValuesForProperties:usingBlock:")]
		void EnumerateValues (NSSet propertiesToEnumerate, MPMediaItemEnumerator enumerator);

		/// <param name="key">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[return: NullAllowed]
		[Export ("objectForKeyedSubscript:")]
		NSObject GetObject (NSObject key);

#if NET
		/// <summary>The value that is associated with the MPMediaEntityPropertyPersistentID constant.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
#endif
		[Field ("MPMediaEntityPropertyPersistentID")]
		NSString PropertyPersistentID { get; }

		[NoiOS, NoMac]
		[MacCatalyst (13, 1)]
		[Export ("persistentID")]
		ulong PersistentID { get; }

#if IOS || TVOS
	}
#if MONOMAC
	[Static]
#else
	[BaseType (typeof (MPMediaEntity))]
#endif
	interface MPMediaItem {
#endif
		/// <param name="groupingType">Grouping type.</param>
		///         <summary>Returns the persistent ID for the specified grouping type.</summary>
		///         <returns>
		///         </returns>
		///         <remarks>
		///         </remarks>
		[NoMac]
		[NoTV]
		[MacCatalyst (13, 1)]
		[Export ("persistentIDPropertyForGroupingType:")]
		[Static]
		string GetPersistentIDProperty (MPMediaGrouping groupingType);

		/// <param name="groupingType">Grouping type.</param>
		///         <summary>Returns the title for the specified grouping type</summary>
		///         <returns>
		///         </returns>
		///         <remarks>
		///         </remarks>
		[NoMac]
		[NoTV]
		[MacCatalyst (13, 1)]
		[Export ("titlePropertyForGroupingType:")]
		[Static]
		string GetTitleProperty (MPMediaGrouping groupingType);

		/// <summary>Represents the value associated with the constant MPMediaItemPropertyPersistentID</summary>
		///         <value>
		///         </value>
		///         <remarks>
		///         </remarks>
		[Field ("MPMediaItemPropertyPersistentID")]
		[EditorBrowsable (EditorBrowsableState.Advanced)]
		NSString PersistentIDProperty { get; }

		/// <summary>Represents the value associated with the constant MPMediaItemPropertyAlbumPersistentID</summary>
		///         <value>
		///         </value>
		///         <remarks>
		/// 	  Persistent IDs can be used as unique identifiers across
		/// 	  application launches and as long as the media item has not
		/// 	  been changed or synchronized again with the host computer.
		/// 	</remarks>
		[Field ("MPMediaItemPropertyAlbumPersistentID")]
		[EditorBrowsable (EditorBrowsableState.Advanced)]
		NSString AlbumPersistentIDProperty { get; }

		/// <summary>Represents the value associated with the constant MPMediaItemPropertyArtistPersistentID</summary>
		///         <value>
		///         </value>
		///         <remarks>
		///         </remarks>
		[Field ("MPMediaItemPropertyArtistPersistentID")]
		[EditorBrowsable (EditorBrowsableState.Advanced)]
		NSString ArtistPersistentIDProperty { get; }

		/// <summary>Represents the value associated with the constant MPMediaItemPropertyAlbumArtistPersistentID</summary>
		///         <value>
		///         </value>
		///         <remarks>
		/// 	  Persistent IDs can be used as unique identifiers across
		/// 	  application launches and as long as the media item has not
		/// 	  been changed or synchronized again with the host computer.
		/// 	</remarks>
		[Field ("MPMediaItemPropertyAlbumArtistPersistentID")]
		[EditorBrowsable (EditorBrowsableState.Advanced)]
		NSString AlbumArtistPersistentIDProperty { get; }

		/// <summary>Represents the value associated with the constant MPMediaItemPropertyGenrePersistentID</summary>
		///         <value>
		///         </value>
		///         <remarks>
		///         </remarks>
		[Field ("MPMediaItemPropertyGenrePersistentID")]
		[EditorBrowsable (EditorBrowsableState.Advanced)]
		NSString GenrePersistentIDProperty { get; }

		/// <summary>Represents the value associated with the constant MPMediaItemPropertyComposerPersistentID</summary>
		///         <value>
		///         </value>
		///         <remarks>
		///         </remarks>
		[Field ("MPMediaItemPropertyComposerPersistentID")]
		[EditorBrowsable (EditorBrowsableState.Advanced)]
		NSString ComposerPersistentIDProperty { get; }

		/// <summary>Represents the value associated with the constant MPMediaItemPropertyPodcastPersistentID</summary>
		///         <value>
		///         </value>
		///         <remarks>
		///         </remarks>
		[Field ("MPMediaItemPropertyPodcastPersistentID")]
		[EditorBrowsable (EditorBrowsableState.Advanced)]
		NSString PodcastPersistentIDProperty { get; }

		/// <summary>Represents the value associated with the constant MPMediaItemPropertyMediaType</summary>
		///         <value>
		///         </value>
		///         <remarks>
		///         </remarks>
		[Field ("MPMediaItemPropertyMediaType")]
		[EditorBrowsable (EditorBrowsableState.Advanced)]
		NSString MediaTypeProperty { get; }

		/// <summary>Represents the value associated with the constant MPMediaItemPropertyTitle</summary>
		///         <value>
		///         </value>
		///         <remarks>
		///         </remarks>
		[Field ("MPMediaItemPropertyTitle")]
		[EditorBrowsable (EditorBrowsableState.Advanced)]
		NSString TitleProperty { get; }

		/// <summary>Represents the value associated with the constant MPMediaItemPropertyAlbumTitle</summary>
		///         <value>
		///         </value>
		///         <remarks>
		///         </remarks>
		[Field ("MPMediaItemPropertyAlbumTitle")]
		[EditorBrowsable (EditorBrowsableState.Advanced)]
		NSString AlbumTitleProperty { get; }

		/// <summary>Represents the value associated with the constant MPMediaItemPropertyArtist</summary>
		///         <value>
		///         </value>
		///         <remarks>
		///         </remarks>
		[Field ("MPMediaItemPropertyArtist")]
		[EditorBrowsable (EditorBrowsableState.Advanced)]
		NSString ArtistProperty { get; }

		/// <summary>Represents the value associated with the constant MPMediaItemPropertyAlbumArtist</summary>
		///         <value>
		///         </value>
		///         <remarks>
		///         </remarks>
		[Field ("MPMediaItemPropertyAlbumArtist")]
		[EditorBrowsable (EditorBrowsableState.Advanced)]
		NSString AlbumArtistProperty { get; }

		/// <summary>Represents the value associated with the constant MPMediaItemPropertyGenre</summary>
		///         <value>
		///         </value>
		///         <remarks>
		///         </remarks>
		[Field ("MPMediaItemPropertyGenre")]
		[EditorBrowsable (EditorBrowsableState.Advanced)]
		NSString GenreProperty { get; }

		/// <summary>Represents the value associated with the constant MPMediaItemPropertyComposer</summary>
		///         <value>
		///         </value>
		///         <remarks>
		///         </remarks>
		[Field ("MPMediaItemPropertyComposer")]
		[EditorBrowsable (EditorBrowsableState.Advanced)]
		NSString ComposerProperty { get; }

		/// <summary>Represents the value associated with the constant MPMediaItemPropertyPlaybackDuration</summary>
		///         <value>
		///         </value>
		///         <remarks>
		///         </remarks>
		[Field ("MPMediaItemPropertyPlaybackDuration")]
		[EditorBrowsable (EditorBrowsableState.Advanced)]
		NSString PlaybackDurationProperty { get; }

		/// <summary>Represents the value associated with the constant MPMediaItemPropertyAlbumTrackNumber</summary>
		///         <value>
		///         </value>
		///         <remarks>
		///         </remarks>
		[Field ("MPMediaItemPropertyAlbumTrackNumber")]
		[EditorBrowsable (EditorBrowsableState.Advanced)]
		NSString AlbumTrackNumberProperty { get; }

		/// <summary>Represents the value associated with the constant MPMediaItemPropertyAlbumTrackCount</summary>
		///         <value>
		///         </value>
		///         <remarks>
		///         </remarks>
		[Field ("MPMediaItemPropertyAlbumTrackCount")]
		[EditorBrowsable (EditorBrowsableState.Advanced)]
		NSString AlbumTrackCountProperty { get; }

		/// <summary>Represents the value associated with the constant MPMediaItemPropertyDiscNumber</summary>
		///         <value>
		///         </value>
		///         <remarks>
		///         </remarks>
		[Field ("MPMediaItemPropertyDiscNumber")]
		[EditorBrowsable (EditorBrowsableState.Advanced)]
		NSString DiscNumberProperty { get; }

		/// <summary>Represents the value associated with the constant MPMediaItemPropertyDiscCount</summary>
		///         <value>
		///         </value>
		///         <remarks>
		///         </remarks>
		[Field ("MPMediaItemPropertyDiscCount")]
		[EditorBrowsable (EditorBrowsableState.Advanced)]
		NSString DiscCountProperty { get; }

		/// <summary>Represents the value associated with the constant MPMediaItemPropertyArtwork</summary>
		///         <value>
		///         </value>
		///         <remarks>
		///         </remarks>
		[MacCatalyst (13, 1)]
		[Field ("MPMediaItemPropertyArtwork")]
		[EditorBrowsable (EditorBrowsableState.Advanced)]
		NSString ArtworkProperty { get; }

		/// <summary>The value that is associated with the MPMediaItemPropertyIsExplicit constant.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MPMediaItemPropertyIsExplicit")]
		[EditorBrowsable (EditorBrowsableState.Advanced)]
		NSString IsExplicitProperty { get; }

		/// <summary>Represents the value associated with the constant MPMediaItemPropertyLyrics</summary>
		///         <value>
		///         </value>
		///         <remarks>
		///         </remarks>
		[Field ("MPMediaItemPropertyLyrics")]
		[EditorBrowsable (EditorBrowsableState.Advanced)]
		NSString LyricsProperty { get; }

		/// <summary>Represents the value associated with the constant MPMediaItemPropertyIsCompilation</summary>
		///         <value>
		///         </value>
		///         <remarks>
		///         </remarks>
		[Field ("MPMediaItemPropertyIsCompilation")]
		[EditorBrowsable (EditorBrowsableState.Advanced)]
		NSString IsCompilationProperty { get; }

		/// <summary>Represents the value associated with the constant MPMediaItemPropertyReleaseDate</summary>
		///         <value>
		///         </value>
		///         <remarks>
		///         </remarks>
		[Field ("MPMediaItemPropertyReleaseDate")]
		[EditorBrowsable (EditorBrowsableState.Advanced)]
		NSString ReleaseDateProperty { get; }

		/// <summary>Represents the value associated with the constant MPMediaItemPropertyBeatsPerMinute</summary>
		///         <value>
		///         </value>
		///         <remarks>
		///         </remarks>
		[Field ("MPMediaItemPropertyBeatsPerMinute")]
		[EditorBrowsable (EditorBrowsableState.Advanced)]
		NSString BeatsPerMinuteProperty { get; }

		/// <summary>Represents the value associated with the constant MPMediaItemPropertyComments</summary>
		///         <value>
		///         </value>
		///         <remarks>
		///         </remarks>
		[Field ("MPMediaItemPropertyComments")]
		[EditorBrowsable (EditorBrowsableState.Advanced)]
		NSString CommentsProperty { get; }

		/// <summary>Represents the value associated with the constant MPMediaItemPropertyAssetURL</summary>
		///         <value>
		///         </value>
		///         <remarks>
		///         </remarks>
		[Field ("MPMediaItemPropertyAssetURL")]
		[EditorBrowsable (EditorBrowsableState.Advanced)]
		NSString AssetURLProperty { get; }

		/// <summary>Represents the value associated with the constant MPMediaItemPropertyPlayCount</summary>
		///         <value>
		///         </value>
		///         <remarks>
		///         </remarks>
		[Field ("MPMediaItemPropertyPlayCount")]
		[EditorBrowsable (EditorBrowsableState.Advanced)]
		NSString PlayCountProperty { get; }

		/// <summary>Represents the value associated with the constant MPMediaItemPropertySkipCount</summary>
		///         <value>
		///         </value>
		///         <remarks>
		///         </remarks>
		[Field ("MPMediaItemPropertySkipCount")]
		[EditorBrowsable (EditorBrowsableState.Advanced)]
		NSString SkipCountProperty { get; }

		/// <summary>Represents the value associated with the constant MPMediaItemPropertyRating</summary>
		///         <value>
		///         </value>
		///         <remarks>
		///         </remarks>
		[Field ("MPMediaItemPropertyRating")]
		[EditorBrowsable (EditorBrowsableState.Advanced)]
		NSString RatingProperty { get; }

		/// <summary>Represents the value associated with the constant MPMediaItemPropertyLastPlayedDate</summary>
		///         <value>
		///         </value>
		///         <remarks>
		///         </remarks>
		[Field ("MPMediaItemPropertyLastPlayedDate")]
		[EditorBrowsable (EditorBrowsableState.Advanced)]
		NSString LastPlayedDateProperty { get; }

		/// <summary>Represents the value associated with the constant MPMediaItemPropertyUserGrouping</summary>
		///         <value>
		///         </value>
		///         <remarks>
		///         </remarks>
		[Field ("MPMediaItemPropertyUserGrouping")]
		[EditorBrowsable (EditorBrowsableState.Advanced)]
		NSString UserGroupingProperty { get; }

		/// <summary>Represents the value associated with the constant MPMediaItemPropertyPodcastTitle</summary>
		///         <value>
		///         </value>
		///         <remarks>
		///         </remarks>
		[Field ("MPMediaItemPropertyPodcastTitle")]
		[EditorBrowsable (EditorBrowsableState.Advanced)]
		NSString PodcastTitleProperty { get; }

		/// <summary>Represents the value associated with the constant MPMediaItemPropertyBookmarkTime</summary>
		///         <value>
		///         </value>
		///         <remarks>
		///         </remarks>
		[Field ("MPMediaItemPropertyBookmarkTime")]
		[EditorBrowsable (EditorBrowsableState.Advanced)]
		NSString BookmarkTimeProperty { get; }

		/// <summary>Represents the value associated with the constant MPMediaItemPropertyIsCloudItem</summary>
		///         <value>
		///         </value>
		///         <remarks>
		///         </remarks>
		[Field ("MPMediaItemPropertyIsCloudItem")]
		[EditorBrowsable (EditorBrowsableState.Advanced)]
		NSString IsCloudItemProperty { get; }

		/// <summary>The value that is associated with the MPMediaItemPropertyHasProtectedAsset constant.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Field ("MPMediaItemPropertyHasProtectedAsset")]
		[EditorBrowsable (EditorBrowsableState.Advanced)]
		NSString HasProtectedAssetProperty { get; }

		/// <summary>The value that is associated with the MPMediaItemPropertyDateAdded constant.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Field ("MPMediaItemPropertyDateAdded")]
		[EditorBrowsable (EditorBrowsableState.Advanced)]
		NSString DateAddedProperty { get; }

		/// <summary>Backing store for the <format type="text/html"><a href="https://docs.microsoft.com/en-us/search/index?search=Media%20Player%20Media%20Item%20Playback%20Store%20ID&amp;scope=Xamarin" title="P:MediaPlayer.MediaItem.PlaybackStoreID">P:MediaPlayer.MediaItem.PlaybackStoreID</a></format> property.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Field ("MPMediaItemPropertyPlaybackStoreID")]
		[EditorBrowsable (EditorBrowsableState.Advanced)]
		NSString PlaybackStoreIDProperty { get; }

		[TV (14, 5), iOS (14, 5)]
		[MacCatalyst (14, 5)]
		[EditorBrowsable (EditorBrowsableState.Advanced)]
		[Field ("MPMediaItemPropertyIsPreorder")]
		NSString IsPreorderProperty { get; }
	}

	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface MPMediaItemArtwork {
		[MacCatalyst (13, 1)]
		[Export ("initWithBoundsSize:requestHandler:")]
		[DesignatedInitializer]
		NativeHandle Constructor (CGSize boundsSize, Func<CGSize, UIImage> requestHandler);

		[NoMac]
		[Deprecated (PlatformName.iOS, 10, 0)]
		[Deprecated (PlatformName.TvOS, 10, 0)]
		[MacCatalyst (13, 1)]
		[Deprecated (PlatformName.MacCatalyst, 13, 1)]
		[Export ("initWithImage:")]
		NativeHandle Constructor (UIImage image);

		/// <param name="size">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>The return type is <see cref="T:UIKit.UIImage" /> on iOS and <see cref="T:AppKit.NSImage" /> on MacOS.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("imageWithSize:")]
		[return: NullAllowed]
		UIImage ImageWithSize (CGSize size);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("bounds")]
		CGRect Bounds { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NoMac]
		[Deprecated (PlatformName.iOS, 10, 0)]
		[Deprecated (PlatformName.TvOS, 10, 0)]
		[MacCatalyst (13, 1)]
		[Deprecated (PlatformName.MacCatalyst, 13, 1)]
		[Export ("imageCropRect")]
		CGRect ImageCropRectangle { get; }
	}

	/// <summary>A sorted set of <see cref="T:MediaPlayer.MPMediaItem" />s.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/MediaPlayer/Reference/MPMediaItemCollection_ClassReference/index.html">Apple documentation for <c>MPMediaItemCollection</c></related>
	[NoMac]
	[NoTV]
	[MacCatalyst (13, 1)]
	// Objective-C exception thrown.  Name: MPMediaItemCollectionInitException Reason: -init is not supported, use -initWithItems:
	[DisableDefaultCtor]
#if IOS
	// introduced in 4.2 - but the type was never added to classic
	[BaseType (typeof (MPMediaEntity))]
#else
	[BaseType (typeof (NSObject))]
#endif
	interface MPMediaItemCollection : NSSecureCoding {
		/// <param name="items">To be added.</param>
		///         <summary>Creates a new <see cref="T:MediaPlayer.MPMediaItemCollection" /> by copying the provided <paramref name="items" />.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Static]
		[Export ("collectionWithItems:")]
		MPMediaItemCollection FromItems (MPMediaItem [] items);

		[DesignatedInitializer]
		[Export ("initWithItems:")]
		NativeHandle Constructor (MPMediaItem [] items);

		/// <summary>Gets an array that contains the items in the collection.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("items")]
		MPMediaItem [] Items { get; }

		/// <summary>Gets a representative item.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("representativeItem")]
		[NullAllowed]
		MPMediaItem RepresentativeItem { get; }

		/// <summary>Gets the number of items in the collection.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("count")]
		nint Count { get; }

		/// <summary>Gets an array that contains the media types of the items in the collection.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("mediaTypes")]
		MPMediaType MediaTypes { get; }
	}

	/// <summary>Represents the synced set of <see cref="T:MediaPlayer.MPMediaItem" />s on a device.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/MediaPlayer/Reference/MPMediaLibrary_ClassReference/index.html">Apple documentation for <c>MPMediaLibrary</c></related>
	[NoMac]
	[NoTV]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	interface MPMediaLibrary : NSSecureCoding {
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Static, Export ("defaultMediaLibrary")]
		MPMediaLibrary DefaultMediaLibrary { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("lastModifiedDate")]
		NSDate LastModifiedDate { get; }

		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("beginGeneratingLibraryChangeNotifications")]
		void BeginGeneratingLibraryChangeNotifications ();

		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("endGeneratingLibraryChangeNotifications")]
		void EndGeneratingLibraryChangeNotifications ();

		/// <include file="../docs/api/MediaPlayer/MPMediaLibrary.xml" path="/Documentation/Docs[@DocId='P:MediaPlayer.MPMediaLibrary.DidChangeNotification']/*" />
		[Field ("MPMediaLibraryDidChangeNotification")]
		[Notification]
		NSString DidChangeNotification { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Static]
		[Export ("authorizationStatus")]
		MPMediaLibraryAuthorizationStatus AuthorizationStatus { get; }

		/// <param name="handler">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Static]
		[Async]
		[Export ("requestAuthorization:")]
		void RequestAuthorization (Action<MPMediaLibraryAuthorizationStatus> handler);

		/// <param name="productID">To be added.</param>
		///         <param name="completionHandler">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("addItemWithProductID:completionHandler:")]
		[Async]
#if IOS
		void AddItem (string productID, [NullAllowed] Action<MPMediaEntity [], NSError> completionHandler);
#else
		void AddItem (string productID, [NullAllowed] Action<MPMediaItem [], NSError> completionHandler);
#endif

		/// <param name="uuid">To be added.</param>
		///         <param name="creationMetadata">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <param name="completionHandler">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Async]
		[Export ("getPlaylistWithUUID:creationMetadata:completionHandler:")]
		void GetPlaylist (NSUuid uuid, [NullAllowed] MPMediaPlaylistCreationMetadata creationMetadata, Action<MPMediaPlaylist, NSError> completionHandler);
	}

	/// <summary>A <see cref="T:UIKit.UIViewController" /> that allows the application user to select a media item.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/MediaPlayer/Reference/MPMediaPickerController_ClassReference/index.html">Apple documentation for <c>MPMediaPickerController</c></related>
	[NoTV]
	[NoMac]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (UIViewController), Delegates = new string [] { "WeakDelegate" }, Events = new Type [] { typeof (MPMediaPickerControllerDelegate) })]
	interface MPMediaPickerController {
		[DesignatedInitializer]
		[Export ("initWithMediaTypes:")]
		NativeHandle Constructor (MPMediaType mediaTypes);

		/// <summary>Gets the media types that the picker enables the user to pick.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("mediaTypes")]
		MPMediaType MediaTypes { get; }

		/// <summary>An object that can respond to the delegate protocol for this type</summary>
		///         <value>The instance that will respond to events and data requests.</value>
		///         <remarks>
		///           <para>The delegate instance assigned to this object will be used to handle events or provide data on demand to this class.</para>
		///           <para>When setting the Delegate or WeakDelegate values events will be delivered to the specified instance instead of being delivered to the C#-style events</para>
		///           <para>   Methods must be decorated with the [Export ("selectorName")] attribute to respond to each method from the protocol.   Alternatively use the Delegate method which is strongly typed and does not require the [Export] attributes on methods.</para>
		///         </remarks>
		[Export ("delegate", ArgumentSemantic.Weak), NullAllowed]
		NSObject WeakDelegate { get; set; }

		/// <summary>An instance of the MediaPlayer.IMPMediaPickerControllerDelegate model class which acts as the class delegate.</summary>
		///         <value>The instance of the MediaPlayer.IMPMediaPickerControllerDelegate model class</value>
		///         <remarks>
		///           <para>The delegate instance assigned to this object will be used to handle events or provide data on demand to this class.</para>
		///           <para>When setting the Delegate or WeakDelegate values events will be delivered to the specified instance instead of being delivered to the C#-style events</para>
		///           <para>This is the strongly typed version of the object, developers should use the WeakDelegate property instead if they want to merely assign a class derived from NSObject that has been decorated with [Export] attributes.</para>
		///         </remarks>
		[Wrap ("WeakDelegate")]
		IMPMediaPickerControllerDelegate Delegate { get; set; }

		/// <summary>Gets or sets a Boolean value that controls whether the user can pick multiple items.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("allowsPickingMultipleItems")]
		bool AllowsPickingMultipleItems { get; set; }

		/// <summary>Gets or sets a string that appears above the picker.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed] // by default this property is null
		[Export ("prompt", ArgumentSemantic.Copy)]
		string Prompt { get; set; }

		/// <summary>Gets or sets a Boolean value that controls whether items from the cloud are shown.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("showsCloudItems")]
		bool ShowsCloudItems { get; set; }

		/// <summary>Gets or sets a Boolean value that controls whether items that have protected assets are shown.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("showsItemsWithProtectedAssets")]
		bool ShowsItemsWithProtectedAssets { get; set; }
	}

	/// <summary>Interface representing the required methods (if any) of the protocol <see cref="T:MediaPlayer.MPMediaPickerControllerDelegate" />.</summary>
	///     <remarks>
	///       <para>This interface contains the required methods (if any) from the protocol defined by <see cref="T:MediaPlayer.MPMediaPickerControllerDelegate" />.</para>
	///       <para>If developers create classes that implement this interface, the implementation methods will automatically be exported to Objective-C with the matching signature from the method defined in the <see cref="T:MediaPlayer.MPMediaPickerControllerDelegate" /> protocol.</para>
	///       <para>Optional methods (if any) are provided by the <see cref="T:MediaPlayer.MPMediaPickerControllerDelegate_Extensions" /> class as extension methods to the interface, allowing developers to invoke any optional methods on the protocol.</para>
	///     </remarks>
	interface IMPMediaPickerControllerDelegate { }

	/// <summary>A delegate object for the <see cref="T:MediaPlayer.MPMediaPickerController" /> class. Application developers can use this deelegate to respond to events relating to media-item selection.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/MediaPlayer/Reference/MPMediaPickerControllerDelegate_ProtocolReference/index.html">Apple documentation for <c>MPMediaPickerControllerDelegate</c></related>
	[NoTV]
	[NoMac]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	[Model]
	[Protocol]
	interface MPMediaPickerControllerDelegate {
		/// <param name="sender">To be added.</param>
		///         <param name="mediaItemCollection">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("mediaPicker:didPickMediaItems:"), EventArgs ("ItemsPicked"), EventName ("ItemsPicked")]
		void MediaItemsPicked (MPMediaPickerController sender, MPMediaItemCollection mediaItemCollection);

		/// <param name="sender">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("mediaPickerDidCancel:"), EventArgs ("MPMediaPickerController"), EventName ("DidCancel")]
		void MediaPickerDidCancel (MPMediaPickerController sender);
	}

	/// <summary>A playable collection of <see cref="T:MediaPlayer.MPMediaItem" />s.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/MediaPlayer/Reference/MPMediaPlaylist_ClassReference/index.html">Apple documentation for <c>MPMediaPlaylist</c></related>
	[NoMac]
	[NoTV]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (MPMediaItemCollection))]
	// Objective-C exception thrown.  Name: MPMediaItemCollectionInitException Reason: -init is not supported, use -initWithItems:
	[DisableDefaultCtor]
	interface MPMediaPlaylist : NSSecureCoding {
		[Export ("initWithItems:")]
		NativeHandle Constructor (MPMediaItem [] items);

		/// <param name="property">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Static, Export ("canFilterByProperty:")]
		bool CanFilterByProperty (string property);

		/// <param name="property">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("valueForProperty:")]
		NSObject ValueForProperty (string property);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("persistentID")]
		ulong PersistentID { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("name")]
		[NullAllowed]
		string Name { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("playlistAttributes")]
		MPMediaPlaylistAttribute PlaylistAttributes { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("seedItems")]
		[NullAllowed]
		MPMediaItem [] SeedItems { get; }

		/// <summary>Gets the user-supplied playlist description.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[NullAllowed, Export ("descriptionText")]
		string DescriptionText { get; }

		/// <summary>When overridden by the developer, gets the playlist display name. The default value is the application name.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[NullAllowed, Export ("authorDisplayName")]
		string AuthorDisplayName { get; }

		/// <param name="productID">To be added.</param>
		///         <param name="completionHandler">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Async]
		[Export ("addItemWithProductID:completionHandler:")]
		void AddItem (string productID, [NullAllowed] Action<NSError> completionHandler);

		/// <param name="mediaItems">To be added.</param>
		///         <param name="completionHandler">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Async]
		[Export ("addMediaItems:completionHandler:")]
		void AddMediaItems (MPMediaItem [] mediaItems, [NullAllowed] Action<NSError> completionHandler);

		[iOS (14, 0)]
		[MacCatalyst (14, 0)]
		[NullAllowed, Export ("cloudGlobalID")]
		string CloudGlobalId { get; }
	}

	[Mac (10, 16)]
	[MacCatalyst (13, 1)]
	[Static]
	interface MPMediaPlaylistProperty {
		/// <summary>Represents the value associated with the constant MPMediaPlaylistPropertyPersistentID</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("MPMediaPlaylistPropertyPersistentID")]
		NSString PersistentID { get; }

		/// <summary>Represents the value associated with the constant MPMediaPlaylistPropertyName</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("MPMediaPlaylistPropertyName")]
		NSString Name { get; }

		/// <summary>Represents the value associated with the constant MPMediaPlaylistPropertyPlaylistAttributes</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("MPMediaPlaylistPropertyPlaylistAttributes")]
		NSString PlaylistAttributes { get; }

		/// <summary>Represents the value associated with the constant MPMediaPlaylistPropertySeedItems</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("MPMediaPlaylistPropertySeedItems")]
		NSString SeedItems { get; }

		/// <summary>Gets the description text.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NoTV] // do not work on AppleTV devices (only in simulator)
		[MacCatalyst (13, 1)]
		[Field ("MPMediaPlaylistPropertyDescriptionText")]
		NSString DescriptionText { get; }

		/// <summary>Gets the display name for the author.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NoTV] // do not work on AppleTV devices (only in simulator)
		[MacCatalyst (13, 1)]
		[Field ("MPMediaPlaylistPropertyAuthorDisplayName")]
		NSString AuthorDisplayName { get; }

		[iOS (14, 0)]
		[TV (14, 0)]
		[MacCatalyst (14, 0)]
		[Field ("MPMediaPlaylistPropertyCloudGlobalID")]
		NSString CloudGlobalId { get; }
	}

	/// <summary>Allows the application developer to programmatically search an <see cref="T:MediaPlayer.MPMediaLibrary" /> by applying zero-or-more <see cref="T:MediaPlayer.MPMediaPredicate" />s and an optional <see cref="T:MediaPlayer.MPMediaGrouping" />.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/MediaPlayer/Reference/MPMediaQuery_ClassReference/index.html">Apple documentation for <c>MPMediaQuery</c></related>
	[NoMac]
	[NoTV]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	interface MPMediaQuery : NSSecureCoding, NSCopying {
		[DesignatedInitializer]
		[Export ("initWithFilterPredicates:")]
		NativeHandle Constructor ([NullAllowed] NSSet filterPredicates);

		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed] // by default this property is null
		[Export ("filterPredicates", ArgumentSemantic.Retain)]
		NSSet FilterPredicates { get; set; }

		/// <param name="predicate">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("addFilterPredicate:")]
		void AddFilterPredicate (MPMediaPredicate predicate);

		/// <param name="predicate">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("removeFilterPredicate:")]
		void RemoveFilterPredicate (MPMediaPredicate predicate);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("items")]
		[NullAllowed]
		MPMediaItem [] Items { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("collections")]
		[NullAllowed]
		MPMediaItemCollection [] Collections { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("groupingType")]
		MPMediaGrouping GroupingType { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("albumsQuery")]
		[Static]
		MPMediaQuery AlbumsQuery { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("artistsQuery")]
		[Static]
		MPMediaQuery ArtistsQuery { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("songsQuery")]
		[Static]
		MPMediaQuery SongsQuery { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("playlistsQuery")]
		[Static]
		MPMediaQuery PlaylistsQuery { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("podcastsQuery")]
		[Static]
		MPMediaQuery PodcastsQuery { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("audiobooksQuery")]
		[Static]
		MPMediaQuery AudiobooksQuery { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("compilationsQuery")]
		[Static]
		MPMediaQuery CompilationsQuery { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("composersQuery")]
		[Static]
		MPMediaQuery ComposersQuery { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("genresQuery")]
		[Static]
		MPMediaQuery GenresQuery { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("collectionSections")]
		[NullAllowed]
		MPMediaQuerySection [] CollectionSections { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("itemSections")]
		[NullAllowed]
		MPMediaQuerySection [] ItemSections { get; }
	}

	/// <summary>A base class for types that specify filters for <see cref="T:MediaPlayer.MPMediaQuery" /> queries.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/MediaPlayer/Reference/MPMediaPredicate_ClassReference/index.html">Apple documentation for <c>MPMediaPredicate</c></related>
	[NoMac]
	[NoTV]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	interface MPMediaPredicate : NSSecureCoding {
	}

	/// <summary>A type of <see cref="T:MediaPlayer.MPMediaPredicate" /> that evaluates <format type="text/html"><a href="https://docs.microsoft.com/en-us/search/index?search=Media%20Player%20MPMedia%20Item%20Property&amp;scope=Xamarin" title="T:MediaPlayer.MPMediaItemProperty">T:MediaPlayer.MPMediaItemProperty</a></format>s.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/MediaPlayer/Reference/MPMediaPropertyPredicate_ClassReference/index.html">Apple documentation for <c>MPMediaPropertyPredicate</c></related>
	[NoMac]
	[NoTV]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (MPMediaPredicate))]
	interface MPMediaPropertyPredicate {
		/// <param name="value">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <param name="property">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Static, Export ("predicateWithValue:forProperty:")]
		MPMediaPropertyPredicate PredicateWithValue ([NullAllowed] NSObject value, string property);

		/// <param name="value">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <param name="property">To be added.</param>
		///         <param name="comparisonType">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Static, Export ("predicateWithValue:forProperty:comparisonType:")]
		MPMediaPropertyPredicate PredicateWithValue ([NullAllowed] NSObject value, string property, MPMediaPredicateComparison comparisonType);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("property", ArgumentSemantic.Copy)]
		string Property { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("value", ArgumentSemantic.Copy)]
		[NullAllowed]
		NSObject Value { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("comparisonType")]
		MPMediaPredicateComparison ComparisonType { get; }
	}

	/// <summary>Logs metrics relating to streaming playback of a movie.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/MediaPlayer/Reference/MPMovieAccessLog_Class/index.html">Apple documentation for <c>MPMovieAccessLog</c></related>
	[NoMac]
	[NoTV]
	[Deprecated (PlatformName.iOS, 9, 0)]
	[MacCatalyst (13, 1)]
	[Deprecated (PlatformName.MacCatalyst, 13, 1)]
	[BaseType (typeof (NSObject))]
	interface MPMovieAccessLog : NSCopying {
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("events")]
		MPMovieAccessLogEvent [] Events { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("extendedLogDataStringEncoding")]
		NSStringEncoding ExtendedLogDataStringEncoding { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("extendedLogData")]
		NSData ExtendedLogData { get; }
	}

	/// <summary>A log of errors that occurred during network resources.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/MediaPlayer/Reference/MPMovieErrorLog_Class/index.html">Apple documentation for <c>MPMovieErrorLog</c></related>
	[NoMac]
	[NoTV]
	[Deprecated (PlatformName.iOS, 9, 0)]
	[MacCatalyst (13, 1)]
	[Deprecated (PlatformName.MacCatalyst, 13, 1)]
	[BaseType (typeof (NSObject))]
	interface MPMovieErrorLog : NSCopying {
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("events")]
		MPMovieErrorLogEvent [] Events { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("extendedLogDataStringEncoding")]
		NSStringEncoding ExtendedLogDataStringEncoding { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("extendedLogData")]
		NSData ExtendedLogData { get; }
	}

	/// <summary>Represents a specific metric or event that occurred during network playback of a movie.</summary>
	///     
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/MediaPlayer/Reference/MPMovieAccessLogEvent_Class/index.html">Apple documentation for <c>MPMovieAccessLogEvent</c></related>
	[NoMac]
	[NoTV]
	[Deprecated (PlatformName.iOS, 9, 0)]
	[MacCatalyst (13, 1)]
	[Deprecated (PlatformName.MacCatalyst, 13, 1)]
	[BaseType (typeof (NSObject))]
	interface MPMovieAccessLogEvent : NSCopying {
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("numberOfSegmentsDownloaded")]
		nint SegmentedDownloadedCount { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("playbackStartDate")]
		NSData PlaybackStartDate { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("URI")]
		string Uri { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("serverAddress")]
		string ServerAddress { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("numberOfServerAddressChanges")]
		nint ServerAddressChangeCount { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("playbackSessionID")]
		string PlaybackSessionID { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("playbackStartOffset")]
		double PlaybackStartOffset { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("segmentsDownloadedDuration")]
		double SegmentsDownloadedDuration { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("durationWatched")]
		double DurationWatched { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("numberOfStalls")]
		nint StallCount { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("numberOfBytesTransferred")]
		long BytesTransferred { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("observedBitrate")]
		double ObservedBitrate { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("indicatedBitrate")]
		double IndicatedBitrate { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("numberOfDroppedVideoFrames")]
		nint DroppedVideoFrameCount { get; }
	}

	/// <summary>An event documenting a network playback error.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/MediaPlayer/Reference/MPMovieErrorLogEvent_Class/index.html">Apple documentation for <c>MPMovieErrorLogEvent</c></related>
	[NoMac]
	[NoTV]
	[Deprecated (PlatformName.iOS, 9, 0)]
	[MacCatalyst (13, 1)]
	[Deprecated (PlatformName.MacCatalyst, 13, 1)]
	[BaseType (typeof (NSObject))]
	interface MPMovieErrorLogEvent : NSCopying {
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("date")]
		NSDate Date { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("URI")]
		string Uri { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("serverAddress")]
		string ServerAddress { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("playbackSessionID")]
		string PlaybackSessionID { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("errorStatusCode")]
		nint ErrorStatusCode { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("errorDomain")]
		string ErrorDomain { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("errorComment")]
		string ErrorComment { get; }
	}

	/// <summary>Provides data for the  event.</summary>
	[NoMac]
	[Deprecated (PlatformName.iOS, 9, 0)]
	[MacCatalyst (13, 1)]
	[Deprecated (PlatformName.MacCatalyst, 13, 1)]
	interface MPMoviePlayerFinishedEventArgs {
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("MPMoviePlayerPlaybackDidFinishReasonUserInfoKey")]
		MPMovieFinishReason FinishReason { get; }
	}

	/// <summary>Provides data for the  event.</summary>
	[NoMac]
	[Deprecated (PlatformName.iOS, 9, 0)]
	[MacCatalyst (13, 1)]
	[Deprecated (PlatformName.MacCatalyst, 13, 1)]
	interface MPMoviePlayerFullScreenEventArgs {
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("MPMoviePlayerFullscreenAnimationDurationUserInfoKey")]
		double AnimationDuration { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("MPMoviePlayerFullscreenAnimationCurveUserInfoKey")]
		UIViewAnimationCurve AnimationCurve { get; }
	}

	/// <summary>Provides data for the  event.</summary>
	[NoMac]
	[Deprecated (PlatformName.iOS, 9, 0)]
	[MacCatalyst (13, 1)]
	[Deprecated (PlatformName.MacCatalyst, 13, 1)]
	interface MPMoviePlayerThumbnailEventArgs {
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("MPMoviePlayerThumbnailImageKey")]
		UIImage Image { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("MPMoviePlayerThumbnailTimeKey")]
		double Time { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("MPMoviePlayerThumbnailErrorKey")]
		NSError Error { get; }
	}

	/// <summary>Provides data for the  event.</summary>
	[NoMac]
	[Deprecated (PlatformName.iOS, 9, 0)]
	[MacCatalyst (13, 1)]
	[Deprecated (PlatformName.MacCatalyst, 13, 1)]
	interface MPMoviePlayerTimedMetadataEventArgs {
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("MPMoviePlayerTimedMetadataUserInfoKey")]
		MPTimedMetadata [] TimedMetadata { get; }
	}

	[NoMac]
#if NET
	[TV (16, 0)]
	[MacCatalyst (13, 1)]
#else
	[Obsoleted (PlatformName.TvOS, 14, 0, message: "Removed in Xcode 12.")]
#endif
	[Protocol]
	interface MPMediaPlayback {
		[Abstract]
		[Export ("play")]
		void Play ();

		[Abstract]
		[Export ("stop")]
		void Stop ();

		[Abstract]
		[Export ("pause")]
		void Pause ();

		[Abstract]
		[Export ("prepareToPlay")]
		void PrepareToPlay ();

		[Abstract]
		[Export ("isPreparedToPlay")]
		bool IsPreparedToPlay { get; }

		[Abstract]
		[Export ("currentPlaybackTime")]
		double CurrentPlaybackTime { get; set; }

		[Abstract]
		[Export ("currentPlaybackRate")]
		float CurrentPlaybackRate { get; set; } // float, not CGFloat

		[Abstract]
		[Export ("beginSeekingForward")]
		void BeginSeekingForward ();

		[Abstract]
		[Export ("beginSeekingBackward")]
		void BeginSeekingBackward ();

		[Abstract]
		[Export ("endSeeking")]
		void EndSeeking ();
	}

	/// <summary>A class that manages the playback of a movie from a file or via streaming.</summary>
	///     
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/MediaPlayer/Reference/MPMoviePlayerController_Class/index.html">Apple documentation for <c>MPMoviePlayerController</c></related>
	[NoMac]
	[NoTV]
	[Deprecated (PlatformName.iOS, 9, 0, message: "Use 'AVPlayerViewController' (AVKit) instead.")]
	[Deprecated (PlatformName.MacCatalyst, 14, 0, message: "Use 'AVPlayerViewController' (AVKit) instead.")]
	[MacCatalyst (14, 0)] // docs says 13.0 but this throws: NSInvalidArgumentException Reason: MPMoviePlayerController is no longer available. Use AVPlayerViewController in AVKit.
	[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'AVPlayerViewController' (AVKit) instead.")]
	[BaseType (typeof (NSObject))]
	interface MPMoviePlayerController : MPMediaPlayback {
		[MacCatalyst (13, 1)]
		[DesignatedInitializer]
		[Export ("initWithContentURL:")]
		NativeHandle Constructor (NSUrl url);

#if !NET
		[Export ("backgroundColor", ArgumentSemantic.Retain)]
		// <quote>You should avoid using this property. It is available only when you use the initWithContentURL: method to initialize the movie player controller object.</quote>
		[Deprecated (PlatformName.iOS, 3, 2, message: "Do not use; this API was removed and is not always available.")]
		[Obsoleted (PlatformName.iOS, 8, 0, message: "Do not use; this API was removed and is not always available.")]
		UIColor BackgroundColor { get; set; }
#endif

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("scalingMode")]
		MPMovieScalingMode ScalingMode { get; set; }

#if !NET
		[Export ("movieControlMode")]
		[Deprecated (PlatformName.iOS, 3, 2, message: "Do not use; this API was removed.")]
		[Obsoleted (PlatformName.iOS, 8, 0, message: "Do not use; this API was removed.")]
		MPMovieControlMode MovieControlMode { get; set; }
#endif

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("initialPlaybackTime")]
		double InitialPlaybackTime { get; set; }

		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[NullAllowed] // by default this property is null
		[Export ("contentURL", ArgumentSemantic.Copy)]
		NSUrl ContentUrl { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("view")]
		UIView View { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("backgroundView")]
		UIView BackgroundView { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("playbackState")]
		MPMoviePlaybackState PlaybackState { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("loadState")]
		MPMovieLoadState LoadState { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("controlStyle")]
		MPMovieControlStyle ControlStyle { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("repeatMode")]
		MPMovieRepeatMode RepeatMode { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("shouldAutoplay")]
		bool ShouldAutoplay { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("useApplicationAudioSession")]
		[Deprecated (PlatformName.iOS, 9, 0, message: "Use 'AVPlayerViewController' (AVKit) instead.")]
		[MacCatalyst (13, 1)]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'AVPlayerViewController' (AVKit) instead.")]
		bool UseApplicationAudioSession { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("fullscreen")]
		bool Fullscreen { [Bind ("isFullscreen")] get; set; }

		/// <param name="fullscreen">To be added.</param>
		///         <param name="animated">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("setFullscreen:animated:")]
		void SetFullscreen (bool fullscreen, bool animated);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("allowsAirPlay")]
		bool AllowsAirPlay { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("airPlayVideoActive")]
		bool AirPlayVideoActive { [Bind ("isAirPlayVideoActive")] get; }

		/// <summary>Developers should not use this deprecated property. </summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Deprecated (PlatformName.iOS, 9, 0)]
		[MacCatalyst (13, 1)]
		[Deprecated (PlatformName.MacCatalyst, 13, 1)]
		[Export ("accessLog")]
		MPMovieAccessLog AccessLog { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Deprecated (PlatformName.iOS, 9, 0)]
		[MacCatalyst (13, 1)]
		[Deprecated (PlatformName.MacCatalyst, 13, 1)]
		[Export ("errorLog")]
		MPMovieErrorLog ErrorLog { get; }

		// Brought it from the MPMediaPlayback.h

		/// <param name="time">To be added.</param>
		///         <param name="timeOption">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("thumbnailImageAtTime:timeOption:")]
		[Deprecated (PlatformName.iOS, 7, 0, message: "Use 'RequestThumbnails' instead.")]
		[MacCatalyst (13, 1)]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'RequestThumbnails' instead.")]
		UIImage ThumbnailImageAt (double time, MPMovieTimeOption timeOption);

		/// <param name="doubleNumbers">To be added.</param>
		///         <param name="timeOption">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("requestThumbnailImagesAtTimes:timeOption:")]
		void RequestThumbnails (NSNumber [] doubleNumbers, MPMovieTimeOption timeOption);

		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("cancelAllThumbnailImageRequests")]
		void CancelAllThumbnailImageRequests ();

		//
		// From interface MPMovieProperties
		//
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("movieMediaTypes")]
		MPMovieMediaType MovieMediaTypes { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("movieSourceType")]
		MPMovieSourceType SourceType { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("duration")]
		double Duration { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("playableDuration")]
		double PlayableDuration { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("naturalSize")]
		CGSize NaturalSize { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("endPlaybackTime")]
		double EndPlaybackTime { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("timedMetadata")]
		MPTimedMetadata [] TimedMetadata { get; }

		/// <include file="../docs/api/MediaPlayer/MPMoviePlayerController.xml" path="/Documentation/Docs[@DocId='P:MediaPlayer.MPMoviePlayerController.ScalingModeDidChangeNotification']/*" />
		[Deprecated (PlatformName.iOS, 9, 0, message: "Use 'AVPlayerViewController' (AVKit) instead.")]
		[MacCatalyst (13, 1)]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'AVPlayerViewController' (AVKit) instead.")]
		[Field ("MPMoviePlayerScalingModeDidChangeNotification")]
		[Notification]
		NSString ScalingModeDidChangeNotification { get; }

		/// <include file="../docs/api/MediaPlayer/MPMoviePlayerController.xml" path="/Documentation/Docs[@DocId='P:MediaPlayer.MPMoviePlayerController.PlaybackDidFinishNotification']/*" />
		[Deprecated (PlatformName.iOS, 9, 0, message: "Use 'AVPlayerViewController' (AVKit) instead.")]
		[MacCatalyst (13, 1)]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'AVPlayerViewController' (AVKit) instead.")]
		[Field ("MPMoviePlayerPlaybackDidFinishNotification")]
		[Notification (typeof (MPMoviePlayerFinishedEventArgs))]
		NSString PlaybackDidFinishNotification { get; }

		/// <summary>Represents the value associated with the constant MPMoviePlayerPlaybackDidFinishReasonUserInfoKey</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Deprecated (PlatformName.iOS, 9, 0, message: "Use 'AVPlayerViewController' (AVKit) instead.")]
		[MacCatalyst (13, 1)]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'AVPlayerViewController' (AVKit) instead.")]
		[Field ("MPMoviePlayerPlaybackDidFinishReasonUserInfoKey")] // NSNumber (MPMovieFinishReason)
		NSString PlaybackDidFinishReasonUserInfoKey { get; }

		/// <include file="../docs/api/MediaPlayer/MPMoviePlayerController.xml" path="/Documentation/Docs[@DocId='P:MediaPlayer.MPMoviePlayerController.PlaybackStateDidChangeNotification']/*" />
		[Deprecated (PlatformName.iOS, 9, 0, message: "Use 'AVPlayerViewController' (AVKit) instead.")]
		[MacCatalyst (13, 1)]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'AVPlayerViewController' (AVKit) instead.")]
		[Field ("MPMoviePlayerPlaybackStateDidChangeNotification")]
		[Notification]
		NSString PlaybackStateDidChangeNotification { get; }

		/// <include file="../docs/api/MediaPlayer/MPMoviePlayerController.xml" path="/Documentation/Docs[@DocId='P:MediaPlayer.MPMoviePlayerController.LoadStateDidChangeNotification']/*" />
		[Deprecated (PlatformName.iOS, 9, 0, message: "Use 'AVPlayerViewController' (AVKit) instead.")]
		[MacCatalyst (13, 1)]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'AVPlayerViewController' (AVKit) instead.")]
		[Field ("MPMoviePlayerLoadStateDidChangeNotification")]
		[Notification]
		NSString LoadStateDidChangeNotification { get; }

		/// <include file="../docs/api/MediaPlayer/MPMoviePlayerController.xml" path="/Documentation/Docs[@DocId='P:MediaPlayer.MPMoviePlayerController.NowPlayingMovieDidChangeNotification']/*" />
		[Deprecated (PlatformName.iOS, 9, 0, message: "Use 'AVPlayerViewController' (AVKit) instead.")]
		[MacCatalyst (13, 1)]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'AVPlayerViewController' (AVKit) instead.")]
		[Field ("MPMoviePlayerNowPlayingMovieDidChangeNotification")]
		[Notification]
		NSString NowPlayingMovieDidChangeNotification { get; }

		/// <include file="../docs/api/MediaPlayer/MPMoviePlayerController.xml" path="/Documentation/Docs[@DocId='P:MediaPlayer.MPMoviePlayerController.WillEnterFullscreenNotification']/*" />
		[Deprecated (PlatformName.iOS, 9, 0, message: "Use 'AVPlayerViewController' (AVKit) instead.")]
		[MacCatalyst (13, 1)]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'AVPlayerViewController' (AVKit) instead.")]
		[Field ("MPMoviePlayerWillEnterFullscreenNotification")]
		[Notification (typeof (MPMoviePlayerFullScreenEventArgs))]
		[Notification]
		NSString WillEnterFullscreenNotification { get; }

		/// <include file="../docs/api/MediaPlayer/MPMoviePlayerController.xml" path="/Documentation/Docs[@DocId='P:MediaPlayer.MPMoviePlayerController.DidEnterFullscreenNotification']/*" />
		[Deprecated (PlatformName.iOS, 9, 0, message: "Use 'AVPlayerViewController' (AVKit) instead.")]
		[MacCatalyst (13, 1)]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'AVPlayerViewController' (AVKit) instead.")]
		[Field ("MPMoviePlayerDidEnterFullscreenNotification")]
		[Notification]
		NSString DidEnterFullscreenNotification { get; }

		/// <include file="../docs/api/MediaPlayer/MPMoviePlayerController.xml" path="/Documentation/Docs[@DocId='P:MediaPlayer.MPMoviePlayerController.WillExitFullscreenNotification']/*" />
		[Deprecated (PlatformName.iOS, 9, 0, message: "Use 'AVPlayerViewController' (AVKit) instead.")]
		[MacCatalyst (13, 1)]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'AVPlayerViewController' (AVKit) instead.")]
		[Field ("MPMoviePlayerWillExitFullscreenNotification")]
		[Notification (typeof (MPMoviePlayerFullScreenEventArgs))]
		NSString WillExitFullscreenNotification { get; }

		/// <include file="../docs/api/MediaPlayer/MPMoviePlayerController.xml" path="/Documentation/Docs[@DocId='P:MediaPlayer.MPMoviePlayerController.DidExitFullscreenNotification']/*" />
		[Deprecated (PlatformName.iOS, 9, 0, message: "Use 'AVPlayerViewController' (AVKit) instead.")]
		[MacCatalyst (13, 1)]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'AVPlayerViewController' (AVKit) instead.")]
		[Field ("MPMoviePlayerDidExitFullscreenNotification")]
		[Notification]
		NSString DidExitFullscreenNotification { get; }

		/// <summary>Represents the value associated with the constant MPMoviePlayerFullscreenAnimationDurationUserInfoKey</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Deprecated (PlatformName.iOS, 9, 0, message: "Use 'AVPlayerViewController' (AVKit) instead.")]
		[MacCatalyst (13, 1)]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'AVPlayerViewController' (AVKit) instead.")]
		[Field ("MPMoviePlayerFullscreenAnimationDurationUserInfoKey")]
		NSString FullscreenAnimationDurationUserInfoKey { get; }

		/// <summary>Represents the value associated with the constant MPMoviePlayerFullscreenAnimationCurveUserInfoKey</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Deprecated (PlatformName.iOS, 9, 0, message: "Use 'AVPlayerViewController' (AVKit) instead.")]
		[MacCatalyst (13, 1)]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'AVPlayerViewController' (AVKit) instead.")]
		[Field ("MPMoviePlayerFullscreenAnimationCurveUserInfoKey")]
		NSString FullscreenAnimationCurveUserInfoKey { get; }

		/// <include file="../docs/api/MediaPlayer/MPMoviePlayerController.xml" path="/Documentation/Docs[@DocId='P:MediaPlayer.MPMoviePlayerController.TypesAvailableNotification']/*" />
		[Deprecated (PlatformName.iOS, 9, 0, message: "Use 'AVPlayerViewController' (AVKit) instead.")]
		[MacCatalyst (13, 1)]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'AVPlayerViewController' (AVKit) instead.")]
		[Field ("MPMovieMediaTypesAvailableNotification")]
		[Notification]
		NSString TypesAvailableNotification { get; }

		/// <include file="../docs/api/MediaPlayer/MPMoviePlayerController.xml" path="/Documentation/Docs[@DocId='P:MediaPlayer.MPMoviePlayerController.SourceTypeAvailableNotification']/*" />
		[Deprecated (PlatformName.iOS, 9, 0, message: "Use 'AVPlayerViewController' (AVKit) instead.")]
		[MacCatalyst (13, 1)]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'AVPlayerViewController' (AVKit) instead.")]
		[Field ("MPMovieSourceTypeAvailableNotification")]
		[Notification]
		NSString SourceTypeAvailableNotification { get; }

		/// <include file="../docs/api/MediaPlayer/MPMoviePlayerController.xml" path="/Documentation/Docs[@DocId='P:MediaPlayer.MPMoviePlayerController.DurationAvailableNotification']/*" />
		[Deprecated (PlatformName.iOS, 9, 0, message: "Use 'AVPlayerViewController' (AVKit) instead.")]
		[MacCatalyst (13, 1)]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'AVPlayerViewController' (AVKit) instead.")]
		[Field ("MPMovieDurationAvailableNotification")]
		[Notification]
		NSString DurationAvailableNotification { get; }

		/// <include file="../docs/api/MediaPlayer/MPMoviePlayerController.xml" path="/Documentation/Docs[@DocId='P:MediaPlayer.MPMoviePlayerController.NaturalSizeAvailableNotification']/*" />
		[Deprecated (PlatformName.iOS, 9, 0, message: "Use 'AVPlayerViewController' (AVKit) instead.")]
		[MacCatalyst (13, 1)]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'AVPlayerViewController' (AVKit) instead.")]
		[Field ("MPMovieNaturalSizeAvailableNotification")]
		[Notification]
		NSString NaturalSizeAvailableNotification { get; }

		/// <include file="../docs/api/MediaPlayer/MPMoviePlayerController.xml" path="/Documentation/Docs[@DocId='P:MediaPlayer.MPMoviePlayerController.ThumbnailImageRequestDidFinishNotification']/*" />
		[Deprecated (PlatformName.iOS, 9, 0, message: "Use 'AVPlayerViewController' (AVKit) instead.")]
		[MacCatalyst (13, 1)]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'AVPlayerViewController' (AVKit) instead.")]
		[Field ("MPMoviePlayerThumbnailImageRequestDidFinishNotification")]
		[Notification (typeof (MPMoviePlayerThumbnailEventArgs))]
		NSString ThumbnailImageRequestDidFinishNotification { get; }

		/// <summary>Represents the value associated with the constant MPMoviePlayerThumbnailImageKey</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Deprecated (PlatformName.iOS, 9, 0, message: "Use 'AVPlayerViewController' (AVKit) instead.")]
		[MacCatalyst (13, 1)]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'AVPlayerViewController' (AVKit) instead.")]
		[Field ("MPMoviePlayerThumbnailImageKey")]
		NSString ThumbnailImageKey { get; }

		/// <summary>Represents the value associated with the constant MPMoviePlayerThumbnailTimeKey</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Deprecated (PlatformName.iOS, 9, 0, message: "Use 'AVPlayerViewController' (AVKit) instead.")]
		[MacCatalyst (13, 1)]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'AVPlayerViewController' (AVKit) instead.")]
		[Field ("MPMoviePlayerThumbnailTimeKey")]
		NSString ThumbnailTimeKey { get; }

		/// <summary>Represents the value associated with the constant MPMoviePlayerThumbnailErrorKey</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Deprecated (PlatformName.iOS, 9, 0, message: "Use 'AVPlayerViewController' (AVKit) instead.")]
		[MacCatalyst (13, 1)]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'AVPlayerViewController' (AVKit) instead.")]
		[Field ("MPMoviePlayerThumbnailErrorKey")]
		NSString ThumbnailErrorKey { get; }

		/// <include file="../docs/api/MediaPlayer/MPMoviePlayerController.xml" path="/Documentation/Docs[@DocId='P:MediaPlayer.MPMoviePlayerController.TimedMetadataUpdatedNotification']/*" />
		[Deprecated (PlatformName.iOS, 9, 0, message: "Use 'AVPlayerViewController' (AVKit) instead.")]
		[MacCatalyst (13, 1)]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'AVPlayerViewController' (AVKit) instead.")]
		[Field ("MPMoviePlayerTimedMetadataUpdatedNotification")]
		[Notification (typeof (MPMoviePlayerTimedMetadataEventArgs))]
		NSString TimedMetadataUpdatedNotification { get; }

		/// <summary>Represents the value associated with the constant MPMoviePlayerTimedMetadataUserInfoKey</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Deprecated (PlatformName.iOS, 9, 0, message: "Use 'AVPlayerViewController' (AVKit) instead.")]
		[MacCatalyst (13, 1)]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'AVPlayerViewController' (AVKit) instead.")]
		[Field ("MPMoviePlayerTimedMetadataUserInfoKey")]
		NSString TimedMetadataUserInfoKey { get; }

		/// <summary>Represents the value associated with the constant MPMoviePlayerTimedMetadataKeyName</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Deprecated (PlatformName.iOS, 9, 0, message: "Use 'AVPlayerViewController' (AVKit) instead.")]
		[MacCatalyst (13, 1)]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'AVPlayerViewController' (AVKit) instead.")]
		[Field ("MPMoviePlayerTimedMetadataKeyName")]
		NSString TimedMetadataKeyName { get; }

		/// <summary>Represents the value associated with the constant MPMoviePlayerTimedMetadataKeyInfo</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Deprecated (PlatformName.iOS, 9, 0, message: "Use 'AVPlayerViewController' (AVKit) instead.")]
		[MacCatalyst (13, 1)]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'AVPlayerViewController' (AVKit) instead.")]
		[Field ("MPMoviePlayerTimedMetadataKeyInfo")]
		NSString TimedMetadataKeyInfo { get; }

		/// <summary>Represents the value associated with the constant MPMoviePlayerTimedMetadataKeyMIMEType</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Deprecated (PlatformName.iOS, 9, 0, message: "Use 'AVPlayerViewController' (AVKit) instead.")]
		[MacCatalyst (13, 1)]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'AVPlayerViewController' (AVKit) instead.")]
		[Field ("MPMoviePlayerTimedMetadataKeyMIMEType")]
		NSString TimedMetadataKeyMIMEType { get; }

		/// <summary>Represents the value associated with the constant MPMoviePlayerTimedMetadataKeyDataType</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Deprecated (PlatformName.iOS, 9, 0, message: "Use 'AVPlayerViewController' (AVKit) instead.")]
		[MacCatalyst (13, 1)]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'AVPlayerViewController' (AVKit) instead.")]
		[Field ("MPMoviePlayerTimedMetadataKeyDataType")]
		NSString TimedMetadataKeyDataType { get; }

		/// <summary>Represents the value associated with the constant MPMoviePlayerTimedMetadataKeyLanguageCode</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Deprecated (PlatformName.iOS, 9, 0, message: "Use 'AVPlayerViewController' (AVKit) instead.")]
		[MacCatalyst (13, 1)]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'AVPlayerViewController' (AVKit) instead.")]
		[Field ("MPMoviePlayerTimedMetadataKeyLanguageCode")]
		NSString TimedMetadataKeyLanguageCode { get; }

		/// <include file="../docs/api/MediaPlayer/MPMoviePlayerController.xml" path="/Documentation/Docs[@DocId='P:MediaPlayer.MPMoviePlayerController.MediaPlaybackIsPreparedToPlayDidChangeNotification']/*" />
		[Deprecated (PlatformName.iOS, 9, 0, message: "Use 'AVPlayerViewController' (AVKit) instead.")]
		[MacCatalyst (13, 1)]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'AVPlayerViewController' (AVKit) instead.")]
		[Field ("MPMediaPlaybackIsPreparedToPlayDidChangeNotification")]
		[Notification]
		NSString MediaPlaybackIsPreparedToPlayDidChangeNotification { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("readyForDisplay")]
		bool ReadyForDisplay { get; }

		/// <include file="../docs/api/MediaPlayer/MPMoviePlayerController.xml" path="/Documentation/Docs[@DocId='P:MediaPlayer.MPMoviePlayerController.MoviePlayerReadyForDisplayDidChangeNotification']/*" />
		[Deprecated (PlatformName.iOS, 9, 0, message: "Use 'AVPlayerViewController' (AVKit) instead.")]
		[MacCatalyst (13, 1)]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'AVPlayerViewController' (AVKit) instead.")]
		[Field ("MPMoviePlayerReadyForDisplayDidChangeNotification")]
		[Notification]
		NSString MoviePlayerReadyForDisplayDidChangeNotification { get; }

		/// <include file="../docs/api/MediaPlayer/MPMoviePlayerController.xml" path="/Documentation/Docs[@DocId='P:MediaPlayer.MPMoviePlayerController.MPMoviePlayerIsAirPlayVideoActiveDidChangeNotification']/*" />
		[Deprecated (PlatformName.iOS, 9, 0, message: "Use 'AVPlayerViewController' (AVKit) instead.")]
		[MacCatalyst (13, 1)]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'AVPlayerViewController' (AVKit) instead.")]
		[Field ("MPMoviePlayerIsAirPlayVideoActiveDidChangeNotification")]
		[Notification]
		NSString MPMoviePlayerIsAirPlayVideoActiveDidChangeNotification { get; }
	}

	/// <summary>Encapsulates timed metadata for use with time-based HTTP media streams.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/MediaPlayer/Reference/MPTimedMetadata_Class/index.html">Apple documentation for <c>MPTimedMetadata</c></related>
	[NoMac]
	[NoTV]
	[Deprecated (PlatformName.iOS, 9, 0)]
	[MacCatalyst (13, 1)]
	[Deprecated (PlatformName.MacCatalyst, 13, 1)]
	[BaseType (typeof (NSObject))]
	// Objective-C exception thrown.  Name: NSGenericException Reason: MPTimedMetadata cannot be created directly
	[DisableDefaultCtor]
	interface MPTimedMetadata {
		/// <summary>A key that identifies a piece of timed metadata.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("key")]
		string Key { get; }

		/// <summary>The namespace of the <see cref="T:MediaPlayer.MPTimedMetadata" />.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("keyspace")]
		string Keyspace { get; }

		[Export ("value")]
#if NET
		NSObject Value { get; }
#else
		NSObject value { get; }
#endif

		/// <summary>The timestamp of the metadata, in the timebase of the media.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("timestamp")]
		double Timestamp { get; }

		/// <summary>All of the metadata.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("allMetadata")]
		NSDictionary AllMetadata { get; }
	}

	/// <summary>A subclass of <see cref="T:UIKit.UIViewController" /> that can present and play a movie.</summary>
	///     
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/MediaPlayer/Reference/MPMoviePlayerViewController_class/index.html">Apple documentation for <c>MPMoviePlayerViewController</c></related>
	[NoTV]
	[NoMac]
	[BaseType (typeof (UIViewController))]
	[Deprecated (PlatformName.iOS, 9, 0, message: "Use 'AVPlayerViewController' (AVKit) instead.")]
	[MacCatalyst (14, 0)] // docs says 13.0 but this throws: NSInvalidArgumentException Reason: MPMoviePlayerViewController is no longer available. Use AVPlayerViewController in AVKit.
	[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'AVPlayerViewController' (AVKit) instead.")]
	interface MPMoviePlayerViewController {
		[DesignatedInitializer]
		[Export ("initWithContentURL:")]
		NativeHandle Constructor (NSUrl url);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("moviePlayer")]
		MPMoviePlayerController MoviePlayer { get; }

#if !NET
		// Directly removed, shows up in iOS 6.1 SDK, but not any later SDKs.
		[Deprecated (PlatformName.iOS, 7, 0, message: "Do not use; this API was removed.")]
		[Obsoleted (PlatformName.iOS, 7, 0, message: "Do not use; this API was removed.")]
		[Export ("shouldAutorotateToInterfaceOrientation:")]
		bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation orientation);
#endif // !NET
	}

	[NoMac]
	[TV (14, 0)]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface MPMusicPlayerController : MPMediaPlayback {

		[Export ("init")]
		[Deprecated (PlatformName.iOS, 11, 3)]
		[NoTV]
		[MacCatalyst (13, 1)]
		[Deprecated (PlatformName.MacCatalyst, 13, 1)]
		NativeHandle Constructor ();

		/// <summary>Gets the music player for the application queue.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Static, Export ("applicationMusicPlayer")]
		MPMusicPlayerController ApplicationMusicPlayer { get; }

		/// <summary>Gets the music player for the application queue.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Static]
		[Export ("applicationQueuePlayer")]
		MPMusicPlayerApplicationController ApplicationQueuePlayer { get; }

		/// <summary>Developers should not use this deprecated property. Developers should use 'SystemMusicPlayer' instead.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Static, Export ("iPodMusicPlayer")]
		[Deprecated (PlatformName.iOS, 8, 0, message: "Use 'SystemMusicPlayer' instead.")]
		[NoTV]
		[MacCatalyst (13, 1)]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'SystemMusicPlayer' instead.")]
		MPMusicPlayerController iPodMusicPlayer { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Static, Export ("systemMusicPlayer")]
		MPMusicPlayerController SystemMusicPlayer { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("playbackState")]
		MPMusicPlaybackState PlaybackState { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("repeatMode")]
		MPMusicRepeatMode RepeatMode { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("shuffleMode")]
		MPMusicShuffleMode ShuffleMode { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Deprecated (PlatformName.iOS, 7, 0, message: "Use 'MPVolumeView' for volume control instead.")]
		[Deprecated (PlatformName.TvOS, 9, 0, message: "Use 'MPVolumeView' for volume control instead.")]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'MPVolumeView' for volume control instead.")]
		[Export ("volume")]
		float Volume { get; set; } // nfloat, not CGFloat

		/// <summary>Gets the index of the currently playing item.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("indexOfNowPlayingItem")]
		nuint IndexOfNowPlayingItem { get; }

		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[ForcedType]
		[Export ("nowPlayingItem", ArgumentSemantic.Copy), NullAllowed]
		MPMediaItem NowPlayingItem { get; set; }

		/// <param name="query">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[NoTV]
		[MacCatalyst (13, 1)]
		[Export ("setQueueWithQuery:")]
		void SetQueue (MPMediaQuery query);

		/// <param name="collection">To be added.</param>
		///         <summary>Sets the queue to the provided <paramref name="collection" />.</summary>
		///         <remarks>To be added.</remarks>
		[NoTV]
		[MacCatalyst (13, 1)]
		[Export ("setQueueWithItemCollection:")]
		void SetQueue (MPMediaItemCollection collection);

		/// <param name="storeIDs">To be added.</param>
		///         <summary>Assigns the player queue to <paramref name="storeIDs" />.</summary>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("setQueueWithStoreIDs:")]
		void SetQueue (string [] storeIDs);

		/// <param name="descriptor">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("setQueueWithDescriptor:")]
		void SetQueue (MPMusicPlayerQueueDescriptor descriptor);

		/// <param name="descriptor">The queue with the items to prepend.</param>
		///         <summary>Inserts the items that are described by the supplied descriptor immediately after the currently playing item.</summary>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("prependQueueDescriptor:")]
		void Prepend (MPMusicPlayerQueueDescriptor descriptor);

		/// <param name="descriptor">The queue with the items to append.</param>
		///         <summary>Appends the items that are described by <paramref name="descriptor" /> to the current queue.</summary>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("appendQueueDescriptor:")]
		void Append (MPMusicPlayerQueueDescriptor descriptor);

		/// <param name="completionHandler">A handler to run after the first item in the queue is buffered.</param>
		///         <summary>Puts the first item in the queue into the buffer and runs a handler after the item has been buffered.</summary>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Async]
		[Export ("prepareToPlayWithCompletionHandler:")]
		void PrepareToPlay (Action<NSError> completionHandler);

		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("skipToNextItem")]
		void SkipToNextItem ();

		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("skipToBeginning")]
		void SkipToBeginning ();

		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("skipToPreviousItem")]
		void SkipToPreviousItem ();

		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("beginGeneratingPlaybackNotifications")]
		void BeginGeneratingPlaybackNotifications ();

		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("endGeneratingPlaybackNotifications")]
		void EndGeneratingPlaybackNotifications ();

		/// <include file="../docs/api/MediaPlayer/MPMusicPlayerController.xml" path="/Documentation/Docs[@DocId='P:MediaPlayer.MPMusicPlayerController.PlaybackStateDidChangeNotification']/*" />
		[Field ("MPMusicPlayerControllerPlaybackStateDidChangeNotification")]
		[Notification]
		NSString PlaybackStateDidChangeNotification { get; }

		/// <include file="../docs/api/MediaPlayer/MPMusicPlayerController.xml" path="/Documentation/Docs[@DocId='P:MediaPlayer.MPMusicPlayerController.NowPlayingItemDidChangeNotification']/*" />
		[Field ("MPMusicPlayerControllerNowPlayingItemDidChangeNotification")]
		[Notification]
		NSString NowPlayingItemDidChangeNotification { get; }

		/// <include file="../docs/api/MediaPlayer/MPMusicPlayerController.xml" path="/Documentation/Docs[@DocId='P:MediaPlayer.MPMusicPlayerController.VolumeDidChangeNotification']/*" />
		[NoTV]
		[MacCatalyst (13, 1)]
		[Field ("MPMusicPlayerControllerVolumeDidChangeNotification")]
		[Notification]
		NSString VolumeDidChangeNotification { get; }
	}

	[NoMac]
	[TV (14, 0)]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (UIView))]
	interface MPVolumeView {
		[Export ("initWithFrame:")]
		NativeHandle Constructor (CGRect frame);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Deprecated (PlatformName.iOS, 13, 0, message: "Use 'AVRoutePickerView' instead.")]
		[Deprecated (PlatformName.TvOS, 13, 0, message: "Use 'AVRoutePickerView' instead.")]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'AVRoutePickerView' instead.")]
		[Export ("showsRouteButton")]
		bool ShowsRouteButton { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Deprecated (PlatformName.iOS, 13, 0, message: "Use 'AVRouteDetector.MultipleRoutesDetected' instead.")]
		[Deprecated (PlatformName.TvOS, 13, 0, message: "Use 'AVRouteDetector.MultipleRoutesDetected' instead.")]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'AVRouteDetector.MultipleRoutesDetected' instead.")]
		[Export ("showsVolumeSlider")]
		bool ShowsVolumeSlider { get; set; }

		/// <param name="image">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <param name="state">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Deprecated (PlatformName.iOS, 13, 0, message: "Use 'AVPlayer.ExternalPlaybackActive' instead.")]
		[Deprecated (PlatformName.TvOS, 13, 0, message: "Use 'AVPlayer.ExternalPlaybackActive' instead.")]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'AVPlayer.ExternalPlaybackActive' instead.")]
		[Export ("setMinimumVolumeSliderImage:forState:")]
		void SetMinimumVolumeSliderImage ([NullAllowed] UIImage image, UIControlState state);

		/// <param name="image">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <param name="state">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("setMaximumVolumeSliderImage:forState:")]
		void SetMaximumVolumeSliderImage ([NullAllowed] UIImage image, UIControlState state);

		/// <param name="image">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <param name="state">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("setVolumeThumbImage:forState:")]
		void SetVolumeThumbImage ([NullAllowed] UIImage image, UIControlState state);

		/// <param name="state">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[return: NullAllowed]
		[Export ("minimumVolumeSliderImageForState:")]
		UIImage GetMinimumVolumeSliderImage (UIControlState state);

		/// <param name="state">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[return: NullAllowed]
		[Export ("maximumVolumeSliderImageForState:")]
		UIImage GetMaximumVolumeSliderImage (UIControlState state);

		/// <param name="state">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[return: NullAllowed]
		[Export ("volumeThumbImageForState:")]
		UIImage GetVolumeThumbImage (UIControlState state);

		/// <param name="bounds">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("volumeSliderRectForBounds:")]
		CGRect GetVolumeSliderRect (CGRect bounds);

		/// <param name="bounds">To be added.</param>
		///         <param name="columeSliderRect">To be added.</param>
		///         <param name="value">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("volumeThumbRectForBounds:volumeSliderRect:value:")]
		CGRect GetVolumeThumbRect (CGRect bounds, CGRect columeSliderRect, float /* float, not CGFloat */ value);

		/// <param name="image">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <param name="state">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Deprecated (PlatformName.iOS, 13, 0, message: "Use 'AVRoutePickerView.RoutePickerButtonStyle' instead.")]
		[Deprecated (PlatformName.TvOS, 13, 0, message: "Use 'AVRoutePickerView.RoutePickerButtonStyle' instead.")]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'AVRoutePickerView.RoutePickerButtonStyle' instead.")]
		[Export ("setRouteButtonImage:forState:")]
		void SetRouteButtonImage ([NullAllowed] UIImage image, UIControlState state);

		/// <param name="state">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Deprecated (PlatformName.iOS, 13, 0, message: "See 'AVRoutePickerView' for possible replacements.")]
		[Deprecated (PlatformName.TvOS, 13, 0, message: "See 'AVRoutePickerView' for possible replacements.")]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "See 'AVRoutePickerView' for possible replacements.")]
		[return: NullAllowed]
		[Export ("routeButtonImageForState:")]
		UIImage GetRouteButtonImage (UIControlState state);

		/// <param name="bounds">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Deprecated (PlatformName.iOS, 13, 0, message: "See 'AVRoutePickerView' for possible replacements.")]
		[Deprecated (PlatformName.TvOS, 13, 0, message: "See 'AVRoutePickerView' for possible replacements.")]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "See 'AVRoutePickerView' for possible replacements.")]
		[Export ("routeButtonRectForBounds:")]
		CGRect GetRouteButtonRect (CGRect bounds);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Deprecated (PlatformName.iOS, 13, 0, message: "Use 'AVRouteDetector.MultipleRoutesDetected' instead.")]
		[Deprecated (PlatformName.TvOS, 13, 0, message: "Use 'AVRouteDetector.MultipleRoutesDetected' instead.")]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'AVRouteDetector.MultipleRoutesDetected' instead.")]
		[Export ("wirelessRoutesAvailable")]
		bool AreWirelessRoutesAvailable { [Bind ("areWirelessRoutesAvailable")] get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Deprecated (PlatformName.iOS, 13, 0, message: "Use 'AVPlayer.ExternalPlaybackActive' instead.")]
		[Deprecated (PlatformName.TvOS, 13, 0, message: "Use 'AVPlayer.ExternalPlaybackActive' instead.")]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'AVPlayer.ExternalPlaybackActive' instead.")]
		[Export ("wirelessRouteActive")]
		bool IsWirelessRouteActive { [Bind ("isWirelessRouteActive")] get; }

		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Deprecated (PlatformName.iOS, 17, 0)]
		[Deprecated (PlatformName.TvOS, 17, 0)]
		[Deprecated (PlatformName.MacCatalyst, 17, 0)]
		[NullAllowed] // by default this property is null
		[Export ("volumeWarningSliderImage", ArgumentSemantic.Retain)]
		UIImage VolumeWarningSliderImage { get; set; }

		/// <include file="../docs/api/MediaPlayer/MPVolumeView.xml" path="/Documentation/Docs[@DocId='P:MediaPlayer.MPVolumeView.WirelessRoutesAvailableDidChangeNotification']/*" />
		[Deprecated (PlatformName.iOS, 13, 0, message: "Use 'AVRouteDetector.MultipleRoutesDetectedDidChange' instead.")]
		[Deprecated (PlatformName.TvOS, 13, 0, message: "Use 'AVRouteDetector.MultipleRoutesDetectedDidChange' instead.")]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'AVRouteDetector.MultipleRoutesDetectedDidChange' instead.")]
		[Notification]
		[Field ("MPVolumeViewWirelessRoutesAvailableDidChangeNotification")]
		NSString WirelessRoutesAvailableDidChangeNotification { get; }

		/// <include file="../docs/api/MediaPlayer/MPVolumeView.xml" path="/Documentation/Docs[@DocId='P:MediaPlayer.MPVolumeView.WirelessRouteActiveDidChangeNotification']/*" />
		[Deprecated (PlatformName.iOS, 13, 0, message: "Use 'AVPlayer.ExternalPlaybackActive' KVO instead.")]
		[Deprecated (PlatformName.TvOS, 13, 0, message: "Use 'AVPlayer.ExternalPlaybackActive' KVO instead.")]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'AVPlayer.ExternalPlaybackActive' KVO instead.")]
		[Notification]
		[Field ("MPVolumeViewWirelessRouteActiveDidChangeNotification")]
		NSString WirelessRouteActiveDidChangeNotification { get; }
	}

	/// <include file="../docs/api/MediaPlayer/MPMediaQuerySection.xml" path="/Documentation/Docs[@DocId='T:MediaPlayer.MPMediaQuerySection']/*" />
	[NoMac]
	[NoTV]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	// Objective-C exception thrown.  Name: NSInvalidArgumentException Reason: MPMediaQuerySection is a read-only object
	[DisableDefaultCtor]
	interface MPMediaQuerySection : NSSecureCoding, NSCopying {
		/// <summary>The location and length, inside the originating <see cref="T:MediaPlayer.MPMediaQuery" />, of this section.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("range", ArgumentSemantic.Assign)]
		NSRange Range { get; }

		/// <summary>The identifier of this section.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("title", ArgumentSemantic.Copy)]
		string Title { get; }
	}

	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	// Objective-C exception thrown.  Name: NSInvalidArgumentException Reason: -init is not supported, use +defaultCenter
	[DisableDefaultCtor]
	interface MPNowPlayingInfoCenter {
		[Export ("nowPlayingInfo", ArgumentSemantic.Copy), NullAllowed, Internal]
		NSDictionary _NowPlayingInfo { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Static]
		[Export ("defaultCenter")]
		MPNowPlayingInfoCenter DefaultCenter { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NoiOS]
		[NoTV]
		[NoMacCatalyst]
		[Export ("playbackState")]
		MPNowPlayingPlaybackState PlaybackState { get; set; }

		[Internal]
		[Field ("MPNowPlayingInfoPropertyElapsedPlaybackTime")]
		NSString PropertyElapsedPlaybackTime { get; }

		[Internal]
		[Field ("MPNowPlayingInfoPropertyPlaybackRate")]
		NSString PropertyPlaybackRate { get; }

		[Internal]
		[Field ("MPNowPlayingInfoPropertyPlaybackQueueIndex")]
		NSString PropertyPlaybackQueueIndex { get; }

		[Internal]
		[Field ("MPNowPlayingInfoPropertyPlaybackQueueCount")]
		NSString PropertyPlaybackQueueCount { get; }

		[Internal]
		[Field ("MPNowPlayingInfoPropertyChapterNumber")]
		NSString PropertyChapterNumber { get; }

		[Internal]
		[Field ("MPNowPlayingInfoPropertyChapterCount")]
		NSString PropertyChapterCount { get; }

		[MacCatalyst (13, 1)]
		[Internal]
		[Field ("MPNowPlayingInfoPropertyDefaultPlaybackRate")]
		NSString PropertyDefaultPlaybackRate { get; }

		[MacCatalyst (13, 1)]
		[Internal]
		[Field ("MPNowPlayingInfoPropertyAvailableLanguageOptions")]
		NSString PropertyAvailableLanguageOptions { get; }

		[MacCatalyst (13, 1)]
		[Internal]
		[Field ("MPNowPlayingInfoPropertyCurrentLanguageOptions")]
		NSString PropertyCurrentLanguageOptions { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Field ("MPNowPlayingInfoCollectionIdentifier")]
		NSString PropertyCollectionIdentifier { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Field ("MPNowPlayingInfoPropertyExternalContentIdentifier")]
		NSString PropertyExternalContentIdentifier { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Field ("MPNowPlayingInfoPropertyExternalUserProfileIdentifier")]
		NSString PropertyExternalUserProfileIdentifier { get; }

		/// <summary>Gets the identifier for the property that specifies the service provider for the currently playing item.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Field ("MPNowPlayingInfoPropertyServiceIdentifier")]
		NSString PropertyServiceIdentifier { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Field ("MPNowPlayingInfoPropertyPlaybackProgress")]
		NSString PropertyPlaybackProgress { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Field ("MPNowPlayingInfoPropertyMediaType")]
		NSString PropertyMediaType { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Field ("MPNowPlayingInfoPropertyIsLiveStream")]
		NSString PropertyIsLiveStream { get; }

		/// <summary>Represents the value that is associated with the MPNowPlayingInfoPropertyAssetURL constant.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Field ("MPNowPlayingInfoPropertyAssetURL")]
		NSString PropertyAssetUrl { get; }

		[MacCatalyst (13, 1)]
		[Internal]
		[Field ("MPNowPlayingInfoPropertyCurrentPlaybackDate")]
		NSString PropertyCurrentPlaybackDate { get; }

		[TV (16, 0), Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0)]
		[Field ("MPNowPlayingInfoPropertyAdTimeRanges")]
		NSString PropertyAdTimeRanges { get; }

		[TV (16, 0), Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0)]
		[Field ("MPNowPlayingInfoPropertyCreditsStartTime")]
		NSString PropertyCreditsStartTime { get; }

		[TV (18, 0), Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
		[Field ("MPNowPlayingInfoPropertyInternationalStandardRecordingCode")]
		NSString PropertyInternationalStandardRecordingCode { get; }

		[TV (18, 0), Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
		[Field ("MPNowPlayingInfoPropertyExcludeFromSuggestions")]
		NSString PropertyExcludeFromSuggestions { get; }
	}

	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor] // crash if used
	interface MPContentItem {

		[DesignatedInitializer]
		[Export ("initWithIdentifier:")]
		NativeHandle Constructor (string identifier);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NullAllowed]
		[Export ("artwork")]
		MPMediaItemArtwork Artwork { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("identifier")]
		string Identifier { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("playbackProgress")]
		float PlaybackProgress { get; set; } // float, not CGFloat

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NullAllowed]
		[Export ("subtitle")]
		string Subtitle { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NullAllowed]
		[Export ("title")]
		string Title { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("streamingContent")]
		bool StreamingContent { [Bind ("isStreamingContent")] get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("explicitContent")]
		bool ExplicitContent { [Bind ("isExplicitContent")] get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("container")]
		bool Container { [Bind ("isContainer")] get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("playable")]
		bool Playable { [Bind ("isPlayable")] get; set; }
	}

	/// <summary>Provides media metadata information to external media players.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/MediaPlayer/Reference/MPPlayableContentDataSource_Ref/index.html">Apple documentation for <c>MPPlayableContentDataSource</c></related>
	[NoMac]
	[NoTV]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	[Model]
	[Protocol]
	interface MPPlayableContentDataSource {

		[Abstract]
		[Export ("contentItemAtIndexPath:")]
		[return: NullAllowed]
#if NET
		MPContentItem GetContentItem (NSIndexPath indexPath);
#else
		MPContentItem ContentItem (NSIndexPath indexPath);
#endif

		/// <param name="indexPath">To be added.</param>
		///         <param name="completionHandler">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("beginLoadingChildItemsAtIndexPath:completionHandler:")]
		void BeginLoadingChildItems (NSIndexPath indexPath, Action<NSError> completionHandler);

		/// <param name="indexPath">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("childItemsDisplayPlaybackProgressAtIndexPath:")]
		bool ChildItemsDisplayPlaybackProgress (NSIndexPath indexPath);

		/// <param name="indexPath">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Abstract]
		[Export ("numberOfChildItemsAtIndexPath:")]
		nint NumberOfChildItems (NSIndexPath indexPath);

		/// <param name="identifier">To be added.</param>
		///         <param name="completionHandler">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[NoMac]
		[Deprecated (PlatformName.iOS, 14, 0, message: "Use 'CarPlay' API instead.")]
		[MacCatalyst (13, 1)]
		[Deprecated (PlatformName.MacCatalyst, 14, 0, message: "Use 'CarPlay' API instead.")]
		[Async]
		[Export ("contentItemForIdentifier:completionHandler:")]
		void GetContentItem (string identifier, Action<MPContentItem, NSError> completionHandler);
	}

	/// <summary>Interface representing the required methods (if any) of the protocol <see cref="T:MediaPlayer.MPPlayableContentDataSource" />.</summary>
	///     <remarks>
	///       <para>This interface contains the required methods (if any) from the protocol defined by <see cref="T:MediaPlayer.MPPlayableContentDataSource" />.</para>
	///       <para>If developers create classes that implement this interface, the implementation methods will automatically be exported to Objective-C with the matching signature from the method defined in the <see cref="T:MediaPlayer.MPPlayableContentDataSource" /> protocol.</para>
	///       <para>Optional methods (if any) are provided by the <see cref="T:MediaPlayer.MPPlayableContentDataSource_Extensions" /> class as extension methods to the interface, allowing developers to invoke any optional methods on the protocol.</para>
	///     </remarks>
	interface IMPPlayableContentDataSource {
	}

	/// <summary>Interface representing the required methods (if any) of the protocol <see cref="T:MediaPlayer.MPPlayableContentDelegate" />.</summary>
	///     <remarks>
	///       <para>This interface contains the required methods (if any) from the protocol defined by <see cref="T:MediaPlayer.MPPlayableContentDelegate" />.</para>
	///       <para>If developers create classes that implement this interface, the implementation methods will automatically be exported to Objective-C with the matching signature from the method defined in the <see cref="T:MediaPlayer.MPPlayableContentDelegate" /> protocol.</para>
	///       <para>Optional methods (if any) are provided by the <see cref="T:MediaPlayer.MPPlayableContentDelegate_Extensions" /> class as extension methods to the interface, allowing developers to invoke any optional methods on the protocol.</para>
	///     </remarks>
	interface IMPPlayableContentDelegate { }

	/// <summary>Delegate object providing methods for external media players to send playback commands to the app.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/MediaPlayer/Reference/MPPlayableContentDelegate_Ref/index.html">Apple documentation for <c>MPPlayableContentDelegate</c></related>
	[NoMac]
	[NoTV]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	[Model]
	[Protocol]
	interface MPPlayableContentDelegate {

		/// <param name="contentManager">To be added.</param>
		///         <param name="indexPath">To be added.</param>
		///         <param name="completionHandler">To be added.</param>
		///         <summary>Method that is called to request item playback.</summary>
		///         <remarks>To be added.</remarks>
		[Deprecated (PlatformName.iOS, 14, 0, message: "Use 'CarPlay' API instead.")]
		[Deprecated (PlatformName.MacCatalyst, 14, 0, message: "Use 'CarPlay' API instead.")]
		[Export ("playableContentManager:initiatePlaybackOfContentItemAtIndexPath:completionHandler:")]
		void InitiatePlaybackOfContentItem (MPPlayableContentManager contentManager, NSIndexPath indexPath, Action<NSError> completionHandler);

		/// <param name="contentManager">To be added.</param>
		///         <param name="context">To be added.</param>
		///         <summary>Method that is called after the context changes.</summary>
		///         <remarks>To be added.</remarks>
		[Deprecated (PlatformName.iOS, 14, 0, message: "Use 'CarPlay' API instead.")]
		[MacCatalyst (13, 1)]
		[Deprecated (PlatformName.MacCatalyst, 14, 0, message: "Use 'CarPlay' API instead.")]
		[Export ("playableContentManager:didUpdateContext:")]
		void ContextUpdated (MPPlayableContentManager contentManager, MPPlayableContentManagerContext context);

		/// <param name="contentManager">To be added.</param>
		///         <param name="completionHandler">To be added.</param>
		///         <summary>Developers should not use this deprecated method. Developers should use 'InitializePlaybackQueue (MPPlayableContentManager, MPContentItem[], Action&lt;NSError&gt;)' instead.</summary>
		///         <remarks>To be added.</remarks>
		[Deprecated (PlatformName.iOS, 9, 3, message: "Use 'InitializePlaybackQueue (MPPlayableContentManager, MPContentItem[], Action<NSError>)' instead.")]
		[MacCatalyst (13, 1)]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'InitializePlaybackQueue (MPPlayableContentManager, MPContentItem[], Action<NSError>)' instead.")]
		[Export ("playableContentManager:initializePlaybackQueueWithCompletionHandler:")]
		void InitializePlaybackQueue (MPPlayableContentManager contentManager, Action<NSError> completionHandler);

		/// <param name="contentManager">To be added.</param>
		///         <param name="contentItems">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <param name="completionHandler">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Deprecated (PlatformName.iOS, 12, 0, message: "Use the Intents framework API instead.")]
		[MacCatalyst (13, 1)]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use the Intents framework API instead.")]
		[Export ("playableContentManager:initializePlaybackQueueWithContentItems:completionHandler:")]
		void InitializePlaybackQueue (MPPlayableContentManager contentManager, [NullAllowed] MPContentItem [] contentItems, Action<NSError> completionHandler);
	}

	/// <summary>Controls interactions between the app and external media players.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/MediaPlayer/Reference/MPPlayableContentManager_Ref/index.html">Apple documentation for <c>MPPlayableContentManager</c></related>
	[NoMac]
	[NoTV]
	[Deprecated (PlatformName.iOS, 14, 0, message: "Use 'CarPlay' API instead.")]
	[MacCatalyst (13, 1)]
	[Deprecated (PlatformName.MacCatalyst, 14, 0, message: "Use 'CarPlay' API instead.")]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor] // NSInvalidArgumentException Reason: -init is invalid. Use +sharedManager. <- [sic]
	interface MPPlayableContentManager {

		/// <summary>Gets the shared data source instance.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Static]
		[Export ("sharedContentManager")]
		MPPlayableContentManager Shared { get; }

		/// <summary>Gets or sets a weak reference to the application data source.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Export ("dataSource", ArgumentSemantic.Weak)]
		[NullAllowed]
		NSObject WeakDataSource { get; set; }

		/// <summary>Gets or sets the application data source.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Wrap ("WeakDataSource")]
		IMPPlayableContentDataSource DataSource { get; set; }

		/// <summary>An object that can respond to the delegate protocol for this type</summary>
		///         <value>The instance that will respond to events and data requests.</value>
		///         <remarks>
		///           <para>The delegate instance assigned to this object will be used to handle events or provide data on demand to this class.</para>
		///           <para>When setting the Delegate or WeakDelegate values events will be delivered to the specified instance instead of being delivered to the C#-style events</para>
		///           <para>   Methods must be decorated with the [Export ("selectorName")] attribute to respond to each method from the protocol.   Alternatively use the Delegate method which is strongly typed and does not require the [Export] attributes on methods.</para>
		///         </remarks>
		[Export ("delegate", ArgumentSemantic.Weak)]
		[NullAllowed]
		NSObject WeakDelegate { get; set; }

		/// <summary>An instance of the MediaPlayer.IMPPlayableContentDelegate model class which acts as the class delegate.</summary>
		///         <value>The instance of the MediaPlayer.IMPPlayableContentDelegate model class</value>
		///         <remarks>
		///           <para>The delegate instance assigned to this object will be used to handle events or provide data on demand to this class.</para>
		///           <para>When setting the Delegate or WeakDelegate values events will be delivered to the specified instance instead of being delivered to the C#-style events</para>
		///           <para>This is the strongly typed version of the object, developers should use the WeakDelegate property instead if they want to merely assign a class derived from NSObject that has been decorated with [Export] attributes.</para>
		///         </remarks>
		[Wrap ("WeakDelegate")]
		IMPPlayableContentDelegate Delegate { get; set; }

		/// <summary>Begins simultanewously updating multiple Media Player content items.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("beginUpdates")]
		void BeginUpdates ();

		/// <summary>Ends updates.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("endUpdates")]
		void EndUpdates ();

		/// <summary>Reloads the source data.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("reloadData")]
		void ReloadData ();

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("context")]
		MPPlayableContentManagerContext Context { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("nowPlayingIdentifiers", ArgumentSemantic.Copy)]
		string [] NowPlayingIdentifiers { get; set; }
	}

	/// <related type="externalDocumentation" href="https://developer.apple.com/reference/MediaPlayer/MPPlayableContentManagerContext">Apple documentation for <c>MPPlayableContentManagerContext</c></related>
	[NoMac]
	[NoTV]
	[Deprecated (PlatformName.iOS, 14, 0, message: "Use 'CarPlay' API instead.")]
	[MacCatalyst (13, 1)]
	[Deprecated (PlatformName.MacCatalyst, 14, 0, message: "Use 'CarPlay' API instead.")]
	[BaseType (typeof (NSObject))]
	interface MPPlayableContentManagerContext {
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("enforcedContentItemsCount")]
		nint EnforcedContentItemsCount { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("enforcedContentTreeDepth")]
		nint EnforcedContentTreeDepth { get; }

		// iOS 9 beta 2 changed this from contentLimitsEnabled - but the final iOS8.4 release used contentLimitsEnabled
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("contentLimitsEnforced")]
		bool ContentLimitsEnforced { get; }

		/// <summary>Developers should not use this deprecated property. Replaced by 'ContentLimitsEnforced'.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Deprecated (PlatformName.iOS, 9, 0, message: "Replaced by 'ContentLimitsEnforced'.")]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Replaced by 'ContentLimitsEnforced'.")]
		[Export ("contentLimitsEnabled")]
		bool ContentLimitsEnabled { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("endpointAvailable")]
		bool EndpointAvailable { get; }
	}

	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor] // NSGenericException Reason: MPRemoteCommands cannot be initialized externally.
	interface MPRemoteCommand {

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("enabled")]
		bool Enabled { [Bind ("isEnabled")] get; set; }

		/// <param name="target">To be added.</param>
		///         <param name="action">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("addTarget:action:")]
		void AddTarget (NSObject target, Selector action);

		/// <param name="handler">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("addTargetWithHandler:")]
		NSObject AddTarget (Func<MPRemoteCommandEvent, MPRemoteCommandHandlerStatus> handler);

		/// <param name="target">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("removeTarget:")]
		void RemoveTarget ([NullAllowed] NSObject target);

		/// <param name="target">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <param name="action">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("removeTarget:action:")]
		void RemoveTarget ([NullAllowed] NSObject target, [NullAllowed] Selector action);
	}

	[MacCatalyst (13, 1)]
	[BaseType (typeof (MPRemoteCommand))]
	[DisableDefaultCtor] // NSGenericException Reason: MPChangePlaybackRateCommands cannot be initialized externally.
	interface MPChangePlaybackRateCommand {

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("supportedPlaybackRates")]
		NSNumber [] SupportedPlaybackRates { get; set; }
	}

	[MacCatalyst (13, 1)]
	[BaseType (typeof (MPRemoteCommand))]
	[DisableDefaultCtor] // NSGenericException Reason: MPChangeShuffleModeCommand cannot be initialized externally.
	interface MPChangeShuffleModeCommand {
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("currentShuffleType", ArgumentSemantic.Assign)]
		MPShuffleType CurrentShuffleType { get; set; }
	}

	[MacCatalyst (13, 1)]
	[BaseType (typeof (MPRemoteCommand))]
	[DisableDefaultCtor] // NSGenericException Reason: MPChangeRepeatModeCommand cannot be initialized externally.
	interface MPChangeRepeatModeCommand {
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("currentRepeatType", ArgumentSemantic.Assign)]
		MPRepeatType CurrentRepeatType { get; set; }
	}

	[MacCatalyst (13, 1)]
	[BaseType (typeof (MPRemoteCommand))]
	[DisableDefaultCtor] // NSGenericException Reason: MPFeedbackCommands cannot be initialized externally.
	interface MPFeedbackCommand {

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("active")]
		bool Active { [Bind ("isActive")] get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("localizedTitle")]
		string LocalizedTitle { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("localizedShortTitle")]
		string LocalizedShortTitle { get; set; }
	}

	[MacCatalyst (13, 1)]
	[BaseType (typeof (MPRemoteCommand))]
	[DisableDefaultCtor] // NSGenericException Reason: MPRatingCommands cannot be initialized externally.
	interface MPRatingCommand {

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("maximumRating")]
		float MaximumRating { get; set; } /* float, not CGFloat */

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("minimumRating")]
		float MinimumRating { get; set; } /* float, not CGFloat */
	}

	[MacCatalyst (13, 1)]
	[BaseType (typeof (MPRemoteCommand))]
	[DisableDefaultCtor] // NSGenericException Reason: MPSkipIntervalCommands cannot be initialized externally.
	interface MPSkipIntervalCommand {

		[Internal] // -> we can't do double[] for an NSArray of NSTimeInterval
		[Export ("preferredIntervals")]
		NSArray _PreferredIntervals { get; set; }
	}

	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface MPRemoteCommandCenter {

		/// <summary>Gets the shared command center object.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Static]
		[Export ("sharedCommandCenter")]
		MPRemoteCommandCenter Shared { get; }

		/// <summary>Gets the bookmarking command.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("bookmarkCommand")]
		MPFeedbackCommand BookmarkCommand { get; }

		/// <summary>Gets the command that is used to change the playback rate.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("changePlaybackRateCommand")]
		MPChangePlaybackRateCommand ChangePlaybackRateCommand { get; }

		/// <summary>Gets the command that is used to change the repeat mode.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("changeRepeatModeCommand")]
		MPChangeRepeatModeCommand ChangeRepeatModeCommand { get; }

		/// <summary>Gets the command that is used to change the shuffle mode.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("changeShuffleModeCommand")]
		MPChangeShuffleModeCommand ChangeShuffleModeCommand { get; }

		/// <summary>Gets the command that is used to dislike the currently playing item.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("dislikeCommand")]
		MPFeedbackCommand DislikeCommand { get; }

		/// <summary>Gets the command that is used to like the currently playing item.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("likeCommand")]
		MPFeedbackCommand LikeCommand { get; }

		/// <summary>Gets the command that is used to play the next track.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("nextTrackCommand")]
		MPRemoteCommand NextTrackCommand { get; }

		/// <summary>Gets the command that is used to pause playback.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("pauseCommand")]
		MPRemoteCommand PauseCommand { get; }

		/// <summary>Gets the command that is used to begin or resume playback.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("playCommand")]
		MPRemoteCommand PlayCommand { get; }

		/// <summary>Gets the command that is used to play the previous track.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("previousTrackCommand")]
		MPRemoteCommand PreviousTrackCommand { get; }

		/// <summary>Gets the command that is used to rate the currently playing item.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("ratingCommand")]
		MPRatingCommand RatingCommand { get; }

		/// <summary>Gets the command that is used to seek backward.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("seekBackwardCommand")]
		MPRemoteCommand SeekBackwardCommand { get; }

		/// <summary>Gets the command that is used to seek forward.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("seekForwardCommand")]
		MPRemoteCommand SeekForwardCommand { get; }

		/// <summary>Gets the command that is used to skip backward.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("skipBackwardCommand")]
		MPSkipIntervalCommand SkipBackwardCommand { get; }

		/// <summary>Gets the command that is used to skip forward.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("skipForwardCommand")]
		MPSkipIntervalCommand SkipForwardCommand { get; }

		/// <summary>Gets the command that is used to stop playback.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("stopCommand")]
		MPRemoteCommand StopCommand { get; }

		/// <summary>Gets the command that is used to toggle the paused/playing state.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("togglePlayPauseCommand")]
		MPRemoteCommand TogglePlayPauseCommand { get; }

		/// <summary>Gets the command that is used to enable language options.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("enableLanguageOptionCommand")]
		MPRemoteCommand EnableLanguageOptionCommand { get; }

		/// <summary>Gets the command that is used to disable language options.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("disableLanguageOptionCommand")]
		MPRemoteCommand DisableLanguageOptionCommand { get; }

		/// <summary>Gets the command that is used to change the playback position.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("changePlaybackPositionCommand")]
		MPChangePlaybackPositionCommand ChangePlaybackPositionCommand { get; }
	}

	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor] // NSGenericException Reason: MPRemoteCommandEvents cannot be initialized externally.
	interface MPRemoteCommandEvent {

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("command")]
		MPRemoteCommand Command { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("timestamp")]
		double /* NSTimeInterval */ Timestamp { get; }
	}

	[MacCatalyst (13, 1)]
	[BaseType (typeof (MPRemoteCommandEvent))]
	[DisableDefaultCtor] // NSGenericException Reason: MPChangePlaybackRateCommandEvents cannot be initialized externally.
	interface MPChangePlaybackRateCommandEvent {

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("playbackRate")]
		float PlaybackRate { get; } // float, not CGFloat
	}

	[MacCatalyst (13, 1)]
	[BaseType (typeof (MPRemoteCommandEvent))]
	[DisableDefaultCtor] // NSGenericException Reason: MPRatingCommandEvents cannot be initialized externally.
	interface MPRatingCommandEvent {

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("rating")]
		float Rating { get; } // float, not CGFloat
	}

	[MacCatalyst (13, 1)]
	[BaseType (typeof (MPRemoteCommandEvent))]
	[DisableDefaultCtor] // Name: NSGenericException Reason: MPSeekCommandEvents cannot be initialized externally.
	interface MPSeekCommandEvent {

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("type")]
		MPSeekCommandEventType Type { get; }
	}

	[MacCatalyst (13, 1)]
	[BaseType (typeof (MPRemoteCommandEvent))]
	[DisableDefaultCtor] // NSGenericException Reason: MPSkipIntervalCommandEvents cannot be initialized externally.
	interface MPSkipIntervalCommandEvent {

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("interval")]
		double /* NSTimeInterval */ Interval { get; }
	}

	[MacCatalyst (13, 1)]
	[BaseType (typeof (MPRemoteCommandEvent))]
	[DisableDefaultCtor]
	interface MPFeedbackCommandEvent {

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("negative")]
		bool Negative { [Bind ("isNegative")] get; }
	}

	[MacCatalyst (13, 1)]
	[BaseType (typeof (MPRemoteCommandEvent))]
	[DisableDefaultCtor] // NSGenericException Reason: MPChangeLanguageOptionCommandEvents cannot be initialized externally.
	interface MPChangeLanguageOptionCommandEvent {
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("languageOption")]
		MPNowPlayingInfoLanguageOption LanguageOption { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("setting")]
		MPChangeLanguageOptionSetting Setting { get; }
	}

	[MacCatalyst (13, 1)]
	[BaseType (typeof (MPRemoteCommandEvent))]
	[DisableDefaultCtor] // NSGenericException Reason: MPChangeShuffleModeCommandEvent cannot be initialized externally.
	interface MPChangeShuffleModeCommandEvent {
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("shuffleType")]
		MPShuffleType ShuffleType { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("preservesShuffleMode")]
		bool PreservesShuffleMode { get; }
	}

	[MacCatalyst (13, 1)]
	[BaseType (typeof (MPRemoteCommandEvent))]
	[DisableDefaultCtor] // NSGenericException Reason: MPChangeRepeatModeCommandEvent cannot be initialized externally.
	interface MPChangeRepeatModeCommandEvent {
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("repeatType")]
		MPRepeatType RepeatType { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("preservesRepeatMode")]
		bool PreservesRepeatMode { get; }
	}

	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor] // pre-emptive
	interface MPNowPlayingInfoLanguageOption {
		[Export ("initWithType:languageTag:characteristics:displayName:identifier:")]
		NativeHandle Constructor (MPNowPlayingInfoLanguageOptionType languageOptionType, string languageTag, [NullAllowed] NSString [] languageOptionCharacteristics, string displayName, string identifier);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("languageOptionType")]
		MPNowPlayingInfoLanguageOptionType LanguageOptionType { get; }

		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("languageTag")]
		string LanguageTag { get; }

		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("languageOptionCharacteristics")]
		NSString [] LanguageOptionCharacteristics { get; }

		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed]
		[Export ("displayName")]
		string DisplayName { get; }

		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed]
		[Export ("identifier")]
		string Identifier { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("isAutomaticLegibleLanguageOption")]
		bool IsAutomaticLegibleLanguageOption { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("isAutomaticAudibleLanguageOption")]
		bool IsAutomaticAudibleLanguageOption { get; }
	}

	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor] // pre-emptive
	interface MPNowPlayingInfoLanguageOptionGroup {
		[Export ("initWithLanguageOptions:defaultLanguageOption:allowEmptySelection:")]
		NativeHandle Constructor (MPNowPlayingInfoLanguageOption [] languageOptions, [NullAllowed] MPNowPlayingInfoLanguageOption defaultLanguageOption, bool allowEmptySelection);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("languageOptions")]
		MPNowPlayingInfoLanguageOption [] LanguageOptions { get; }

		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("defaultLanguageOption")]
		MPNowPlayingInfoLanguageOption DefaultLanguageOption { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("allowEmptySelection")]
		bool AllowEmptySelection { get; }
	}

	[MacCatalyst (13, 1)]
	[Static]
	// not [Internal] since they are exposed as an NSString[] property in MPNowPlayingInfoLanguageOption
	interface MPLanguageOptionCharacteristics {
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MPLanguageOptionCharacteristicIsMainProgramContent")]
		NSString IsMainProgramContent { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MPLanguageOptionCharacteristicIsAuxiliaryContent")]
		NSString IsAuxiliaryContent { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MPLanguageOptionCharacteristicContainsOnlyForcedSubtitles")]
		NSString ContainsOnlyForcedSubtitles { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MPLanguageOptionCharacteristicTranscribesSpokenDialog")]
		NSString TranscribesSpokenDialog { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MPLanguageOptionCharacteristicDescribesMusicAndSound")]
		NSString DescribesMusicAndSound { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MPLanguageOptionCharacteristicEasyToRead")]
		NSString EasyToRead { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MPLanguageOptionCharacteristicDescribesVideo")]
		NSString DescribesVideo { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MPLanguageOptionCharacteristicLanguageTranslation")]
		NSString LanguageTranslation { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MPLanguageOptionCharacteristicDubbedTranslation")]
		NSString DubbedTranslation { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MPLanguageOptionCharacteristicVoiceOverTranslation")]
		NSString VoiceOverTranslation { get; }
	}

	[MacCatalyst (13, 1)]
	[BaseType (typeof (MPRemoteCommand))]
	[DisableDefaultCtor] // Objective-C exception thrown.  Name: NSGenericException Reason: MPChangePlaybackPositionCommands cannot be initialized externally.
	interface MPChangePlaybackPositionCommand {
	}

	[MacCatalyst (13, 1)]
	[BaseType (typeof (MPRemoteCommandEvent))]
	[DisableDefaultCtor] // Objective-C exception thrown.  Name: NSGenericException Reason: MPChangePlaybackPositionCommandEvents cannot be initialized externally.
	interface MPChangePlaybackPositionCommandEvent {
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("positionTime")]
		double PositionTime { get; }
	}

	/// <related type="externalDocumentation" href="https://developer.apple.com/reference/MediaPlayer/MPMediaPlaylistCreationMetadata">Apple documentation for <c>MPMediaPlaylistCreationMetadata</c></related>
	[NoMac]
	[NoTV]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface MPMediaPlaylistCreationMetadata {
		[Export ("initWithName:")]
		[DesignatedInitializer]
		NativeHandle Constructor (string name);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("name")]
		string Name { get; }

		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed] // null_resettable
		[Export ("authorDisplayName")]
		string AuthorDisplayName { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("descriptionText")]
		string DescriptionText { get; set; }
	}

	[NoMac]
	[TV (14, 0)]
	[MacCatalyst (13, 1)]
	[DisableDefaultCtor]
	[BaseType (typeof (NSObject))]
	interface MPMusicPlayerQueueDescriptor : NSSecureCoding {

		[Export ("init")]
		[Deprecated (PlatformName.iOS, 11, 3)]
		[Deprecated (PlatformName.TvOS, 11, 3)]
		[Deprecated (PlatformName.MacCatalyst, 13, 1)]
		NativeHandle Constructor ();
	}

	/// <summary>Operates on media player queues.</summary>
	[NoMac]
	[NoTV]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (MPMusicPlayerQueueDescriptor))]
	interface MPMusicPlayerMediaItemQueueDescriptor {
		[Export ("initWithQuery:")]
		NativeHandle Constructor (MPMediaQuery query);

		[Export ("initWithItemCollection:")]
		NativeHandle Constructor (MPMediaItemCollection itemCollection);

		/// <summary>Gets a query that returns the items that were used to create this media item queue descriptor.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("query", ArgumentSemantic.Copy)]
		MPMediaQuery Query { get; }

		/// <summary>Gets the collection of media items that was used to create this media item queue descriptor.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("itemCollection", ArgumentSemantic.Strong)]
		MPMediaItemCollection ItemCollection { get; }

		/// <summary>Gets or sets which item will be played first.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("startItem", ArgumentSemantic.Strong)]
		MPMediaItem StartItem { get; set; }

		/// <param name="startTime">The time at which the media will start playing.</param>
		///         <param name="mediaItem">The media item to modify.</param>
		///         <summary>Sets the time that the media item will start playing.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("setStartTime:forItem:")]
		void SetStartTime (double startTime, MPMediaItem mediaItem);

		/// <param name="endTime">The time at which the media will stop playing.</param>
		///         <param name="mediaItem">The media item to modify.</param>
		///         <summary>Sets the time that the media item will stop playing.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("setEndTime:forItem:")]
		void SetEndTime (double endTime, MPMediaItem mediaItem);
	}

	[NoMac]
	[TV (14, 0)]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (MPMusicPlayerQueueDescriptor))]
	interface MPMusicPlayerStoreQueueDescriptor {
		[Export ("initWithStoreIDs:")]
		NativeHandle Constructor (string [] storeIDs);

		/// <summary>Gets or sets the store identifiers for the items in the queue descriptor.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("storeIDs", ArgumentSemantic.Copy)]
		string [] StoreIDs { get; set; }

		/// <summary>Gets the ID of the start item.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("startItemID")]
		string StartItemID { get; set; }

		/// <param name="startTime">The time at which the item will start playing.</param>
		///         <param name="storeID">The store ID of the item to start.</param>
		///         <summary>Sets the time that the media item will start playing.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("setStartTime:forItemWithStoreID:")]
		void SetStartTime (double startTime, string storeID);

		/// <param name="endTime">The time at which the item will stop playing.</param>
		///         <param name="storeID">The store ID of the item to stop.</param>
		///         <summary>Sets the time that the media item will stop playing.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("setEndTime:forItemWithStoreID:")]
		void SetEndTime (double endTime, string storeID);
	}

	[NoMac]
	[TV (14, 0)]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface MPMusicPlayerControllerQueue {
		/// <summary>Gets the array of media items that are contained in the queue.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("items", ArgumentSemantic.Copy)]
		MPMediaItem [] Items { get; }

		/// <include file="../docs/api/MediaPlayer/MPMusicPlayerControllerQueue.xml" path="/Documentation/Docs[@DocId='P:MediaPlayer.MPMusicPlayerControllerQueue.DidChangeNotification']/*" />
		[Field ("MPMusicPlayerControllerQueueDidChangeNotification")]
		[Notification]
		NSString DidChangeNotification { get; }
	}

	[NoMac]
	[TV (14, 0)]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (MPMusicPlayerControllerQueue))]
	interface MPMusicPlayerControllerMutableQueue {
		/// <param name="queueDescriptor">The queue descriptor with the items to insert.</param>
		///         <param name="item">The item after which to insert the queued items. May be <see langword="null" />.<para tool="nullallowed">This parameter can be <see langword="null" />.</para></param>
		///         <summary>Inserts the queue that is identified by <paramref name="queueDescriptor" /> after <paramref name="item" />.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("insertQueueDescriptor:afterItem:")]
		void InsertAfter (MPMusicPlayerQueueDescriptor queueDescriptor, [NullAllowed] MPMediaItem item);

		/// <param name="item">The item to remove.</param>
		///         <summary>Removes the specified <paramref name="item" /> from the queue.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("removeItem:")]
		void RemoveItem (MPMediaItem item);
	}

	[NoMac]
	[TV (14, 0)]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (MPMusicPlayerController))]
	interface MPMusicPlayerApplicationController {
		/// <param name="queueTransaction">The action to perform while the queue is created.</param>
		///         <param name="completionHandler">A handler to run when the operation completes.</param>
		///         <summary>Performs the requested queue transformation and runs a handler when the operation is complete.</summary>
		///         <remarks>To be added.</remarks>
		[Async]
		[Export ("performQueueTransaction:completionHandler:")]
		void Perform (Action<MPMusicPlayerControllerMutableQueue> queueTransaction, Action<MPMusicPlayerControllerQueue, NSError> completionHandler);
	}

	[NoMac]
	[TV (14, 0)]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface MPMusicPlayerPlayParameters : NSSecureCoding {
		[Export ("initWithDictionary:")]
		NativeHandle Constructor (NSDictionary dictionary);

		/// <summary>Gets the dictionary of parameters.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("dictionary", ArgumentSemantic.Copy)]
		NSDictionary Dictionary { get; }
	}

	[NoMac]
	[TV (14, 0)]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (MPMusicPlayerQueueDescriptor))]
	[DisableDefaultCtor]
	interface MPMusicPlayerPlayParametersQueueDescriptor {
		[Export ("initWithPlayParametersQueue:")]
		NativeHandle Constructor (MPMusicPlayerPlayParameters [] playParametersQueue);

		/// <summary>Gets or sets the play parameters queue</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("playParametersQueue", ArgumentSemantic.Copy)]
		MPMusicPlayerPlayParameters [] PlayParametersQueue { get; set; }

		/// <summary>Gets or sets the the play parameters that identify the item that will be played first.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("startItemPlayParameters", ArgumentSemantic.Strong)]
		MPMusicPlayerPlayParameters StartItemPlayParameters { get; set; }

		/// <param name="startTime">The time at which the described item will start playing.</param>
		///         <param name="playParameters">The parameters that describe the item.</param>
		///         <summary>Sets the start time for the item that is described by the provided play parameters.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("setStartTime:forItemWithPlayParameters:")]
		void SetStartTime (/* NSTimeInterval */ double startTime, MPMusicPlayerPlayParameters playParameters);

		/// <param name="endTime">The time at which the described item will stop playing.</param>
		///         <param name="playParameters">The parameters that describe the item.</param>
		///         <summary>Sets the end time for the item that is described by the provided play parameters.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("setEndTime:forItemWithPlayParameters:")]
		void SetEndTime (/* NSTimeInterval */ double endTime, MPMusicPlayerPlayParameters playParameters);
	}

	interface IMPSystemMusicPlayerController { }

	/// <summary>Protocol for playing videos with the Music app.</summary>
	[NoTV]
	[NoMac] // headers have no availability macros on the protocol itself but the only member is not available on macOS
	[MacCatalyst (13, 1)]
	[Protocol]
	interface MPSystemMusicPlayerController {
		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("openToPlayQueueDescriptor:")]
		void OpenToPlay (MPMusicPlayerQueueDescriptor queueDescriptor);
	}

	[Category]
	[BaseType (typeof (NSUserActivity))]
	[NoMac]
	[MacCatalyst (13, 1)]
	interface NSUserActivity_MediaPlayerAdditions {
		[return: NullAllowed]
		[Export ("externalMediaContentIdentifier")]
		NSString GetExternalMediaContentIdentifier ();

		[Export ("setExternalMediaContentIdentifier:")]
		void SetExternalMediaContentIdentifier ([NullAllowed] NSString identifier);
	}

	[MacCatalyst (13, 1)]
	[Category]
	[BaseType (typeof (AVMediaSelectionOption))]
	interface AVMediaSelectionOption_MPNowPlayingInfoLanguageOptionAdditions {
		[Export ("makeNowPlayingInfoLanguageOption")]
		[return: NullAllowed]
		MPNowPlayingInfoLanguageOption CreateNowPlayingInfoLanguageOption ();
	}

	[MacCatalyst (13, 1)]
	[Category]
	[BaseType (typeof (AVMediaSelectionGroup))]
	interface AVMediaSelectionGroup_MPNowPlayingInfoLanguageOptionAdditions {
		[Export ("makeNowPlayingInfoLanguageOptionGroup")]
		MPNowPlayingInfoLanguageOptionGroup CreateNowPlayingInfoLanguageOptionGroup ();
	}

	interface IMPNowPlayingSessionDelegate { }

	[TV (14, 0), iOS (16, 0)]
	[NoMac, MacCatalyst (18, 4)]
#if NET
	[Protocol, Model]
#else
	[Protocol, Model (AutoGeneratedName = true)]
#endif
	[BaseType (typeof (NSObject))]
	interface MPNowPlayingSessionDelegate {

		[Export ("nowPlayingSessionDidChangeActive:")]
		void DidChangeActive (MPNowPlayingSession nowPlayingSession);

		[Export ("nowPlayingSessionDidChangeCanBecomeActive:")]
		void DidChangeCanBecomeActive (MPNowPlayingSession nowPlayingSession);
	}

	[TV (14, 0), iOS (16, 0)]
	[NoMac, MacCatalyst (18, 4)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface MPNowPlayingSession {

		[Export ("initWithPlayers:")]
		NativeHandle Constructor (AVPlayer [] players);

		[Export ("players", ArgumentSemantic.Strong)]
		AVPlayer [] Players { get; }

		[Wrap ("WeakDelegate")]
		[NullAllowed]
		IMPNowPlayingSessionDelegate Delegate { get; set; }

		[NullAllowed, Export ("delegate", ArgumentSemantic.Weak)]
		NSObject WeakDelegate { get; set; }

		[Export ("nowPlayingInfoCenter", ArgumentSemantic.Strong)]
		MPNowPlayingInfoCenter NowPlayingInfoCenter { get; }

		[Export ("remoteCommandCenter", ArgumentSemantic.Strong)]
		MPRemoteCommandCenter RemoteCommandCenter { get; }

		[Export ("canBecomeActive")]
		bool CanBecomeActive { get; }

		[Export ("active")]
		bool Active { [Bind ("isActive")] get; }

		[Async]
		[Export ("becomeActiveIfPossibleWithCompletion:")]
		void BecomeActiveIfPossible ([NullAllowed] Action<bool> completion);

		[Export ("addPlayer:")]
		void AddPlayer (AVPlayer player);

		[Export ("removePlayer:")]
		void RemovePlayer (AVPlayer player);

		[TV (16, 0), MacCatalyst (18, 4), NoMac]
		[Export ("automaticallyPublishesNowPlayingInfo")]
		bool AutomaticallyPublishesNowPlayingInfo { get; set; }
	}

	[TV (16, 0), MacCatalyst (18, 4), NoMac, iOS (16, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface MPAdTimeRange : NSCopying {
		[Export ("timeRange", ArgumentSemantic.Assign)]
		CMTimeRange TimeRange { get; set; }

		[Export ("initWithTimeRange:")]
		NativeHandle Constructor (CMTimeRange timeRange);
	}


}
