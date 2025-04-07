#if !XAMCORE_5_0

#nullable enable

using System;
using System.ComponentModel;
using System.Threading.Tasks;

using ObjCRuntime;
using Foundation;
using CoreGraphics;
using CoreFoundation;
using CoreLocation;
using UIKit;
using MediaPlayer;

#if !NET
using NativeHandle = System.IntPtr;
#endif

namespace AssetsLibrary {

	/// <param name="result">The asset.</param>
	///     <param name="index">The index of this asset.</param>
	///     <param name="stop">If set to true, the enumeration process will stop.</param>
	///     <summary>Signature for delegates participating in asset enumeration.</summary>
	///     <remarks>
	///     </remarks>
	public delegate void ALAssetsEnumerator (ALAsset result, nint index, ref bool stop);

	/// <param name="group">To be added.</param>
	///     <param name="stop">To be added.</param>
	///     <summary>A delegate that is used as the <c>enumerationBlock</c> parameter in calls to the <see cref="M:AssetsLibrary.ALAssetsLibrary.Enumerate(AssetsLibrary.ALAssetsGroupType,AssetsLibrary.ALAssetsLibraryGroupsEnumerationResultsDelegate,System.Action{Foundation.NSError})" /> method.</summary>
	///     <remarks>To be added.</remarks>
	public delegate void ALAssetsLibraryGroupsEnumerationResultsDelegate (ALAssetsGroup group, ref bool stop);

	/// <summary>An enumeration whose values specify various errors relating to <see cref="T:AssetsLibrary.ALAsset" />s.</summary>
	///     <remarks>To be added.</remarks>
	///     <!-- TODO: Unused? -->
	[EditorBrowsable (EditorBrowsableState.Never)]
	[Obsolete (Constants.AssetsLibraryRemoved)]
	public enum ALAssetsError : int {
		/// <summary>To be added.</summary>
		UnknownError = -1,
		/// <summary>To be added.</summary>
		WriteFailedError = -3300,
		/// <summary>To be added.</summary>
		WriteBusyError = -3301,
		/// <summary>To be added.</summary>
		WriteInvalidDataError = -3302,
		/// <summary>To be added.</summary>
		WriteIncompatibleDataError = -3303,
		/// <summary>To be added.</summary>
		WriteDataEncodingError = -3304,
		/// <summary>To be added.</summary>
		WriteDiskSpaceError = -3305,
		/// <summary>To be added.</summary>
		DataUnavailableError = -3310,
		/// <summary>To be added.</summary>
		AccessUserDeniedError = -3311,
		/// <summary>To be added.</summary>
		AccessGloballyDeniedError = -3312,
	}

	/// <summary>Extension methods for the AssetsLibrary.ALAssetsError enumeration.</summary>
	///     <remarks>
	///       <para>The extension method for the AssetsLibrary.ALAssetsError enumeration can be used to fetch the error domain associated with these error codes.</para>
	///     </remarks>
	[EditorBrowsable (EditorBrowsableState.Never)]
	[Obsolete (Constants.AssetsLibraryRemoved)]
	static public partial class ALAssetsErrorExtensions {
		/// <param name="self">The enumeration value</param>
		///         <summary>Returns the error domain associated with the AssetsLibrary.ALAssetsError value</summary>
		///         <returns>To be added.</returns>
		///         <remarks>
		///           <para>See the <see cref="T:Foundation.NSError" /> for information on how to use the error domains when reporting errors.</para>
		///         </remarks>
		public static NSString? GetDomain (this ALAssetsError self)
		{
			throw new InvalidOperationException (Constants.AssetsLibraryRemoved);
		}
	}

	/// <summary>The orientation of the asset.</summary>
	///     <remarks>
	///     </remarks>
	[EditorBrowsable (EditorBrowsableState.Never)]
	[Obsolete (Constants.AssetsLibraryRemoved)]
	[Native]
	public enum ALAssetOrientation : long {
		/// <summary>The default orientation.
		///         </summary>
		Up = 0,
		/// <summary>The asset has been rotated 180 degrees from <see cref="F:AssetsLibrary.ALAssetOrientation.Up" />.</summary>
		Down = 1,
		/// <summary>The asset has been rotated 90 degrees counter-clockwise from <see cref="F:AssetsLibrary.ALAssetOrientation.Up" />.
		///         </summary>
		Left = 2,
		/// <summary>The asset has been rotated 90 degrees clockwise from <see cref="F:AssetsLibrary.ALAssetOrientation.Up" />.
		///         </summary>
		Right = 3,
		/// <summary>The asset has been horizontally mirrored.
		///         </summary>
		UpMirrored = 4,
		/// <summary>The asset has been rotated 180 degrees from <see cref="F:AssetsLibrary.ALAssetOrientation.Up" /> and then horizontally mirrored.
		///         </summary>
		DownMirrored = 5,
		/// <summary>The asset has been horizontally mirrorer and then rotated 90 degrees counter-clockwise from <see cref="F:AssetsLibrary.ALAssetOrientation.Up" />.
		///         </summary>
		LeftMirrored = 6,
		/// <summary>The asset has been horizontally mirrored and then rotated 90 degrees clockwise from <see cref="F:AssetsLibrary.ALAssetOrientation.Up" />.
		///         </summary>
		RightMirrored = 7,
	}

	/// <summary>Describes the group type.</summary>
	///     <remarks>
	///     </remarks>
	[EditorBrowsable (EditorBrowsableState.Never)]
	[Obsolete (Constants.AssetsLibraryRemoved)]
	[Flags]
	[Native]
	public enum ALAssetsGroupType : ulong {
		/// <summary>To be added.</summary>
		Library = 1,
		/// <summary>A regular album.</summary>
		Album = 2,
		/// <summary>Event assets</summary>
		Event = 4,
		/// <summary>Organized by Faces</summary>
		Faces = 8,
		/// <summary>Saved Photos.</summary>
		SavedPhotos = 16,
		/// <summary>To be added.</summary>
		GroupPhotoStream = 32,
		/// <summary>All asset group types.</summary>
		All = 4294967295,
	}

	/// <summary>The asset type.</summary>
	///     <remarks>To be added.</remarks>
	[EditorBrowsable (EditorBrowsableState.Never)]
	[Obsolete (Constants.AssetsLibraryRemoved)]
	public enum ALAssetType : int {
		/// <summary>The asset is a video.</summary>
		Video = 0,
		/// <summary>The asset is a photo.</summary>
		Photo = 1,
		/// <summary>The asset has an unknown type.</summary>
		Unknown = 2,
	}

	/// <summary>An enumeration whose values specify the authorization status of a <see cref="T:AssetsLibrary.ALAssetsLibrary" />. Retrieved by the <see cref="P:AssetsLibrary.ALAssetsLibrary.AuthorizationStatus" /> property.</summary>
	///     <remarks>To be added.</remarks>
	[EditorBrowsable (EditorBrowsableState.Never)]
	[Obsolete (Constants.AssetsLibraryRemoved)]
	[Native]
	public enum ALAuthorizationStatus : long {
		/// <summary>The user has not interacted with the permissions dialog.</summary>
		NotDetermined = 0,
		/// <summary>Access is denied and the user is not allowed to change permission.</summary>
		Restricted = 1,
		/// <summary>The user has denied access.</summary>
		Denied = 2,
		/// <summary>The user has granted access.</summary>
		Authorized = 3,
	}

	/// <summary>An asset managed by the Photo application (videos and photos).</summary>
	///     <remarks>
	///     </remarks>
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/AssetsLibrary/Reference/ALAsset_Class/index.html">Apple documentation for <c>ALAsset</c></related>
	[EditorBrowsable (EditorBrowsableState.Never)]
	[Obsolete (Constants.AssetsLibraryRemoved)]
	public unsafe partial class ALAsset : NSObject {
		/// <summary>The handle for this class.</summary>
		///         <value>The pointer to the Objective-C class.</value>
		///         <remarks>Each Xamarin.iOS class mirrors an unmanaged Objective-C class.   This value contains the pointer to the Objective-C class, it is similar to calling objc_getClass with the object name.</remarks>
		public override NativeHandle ClassHandle { get { throw new InvalidOperationException (Constants.AssetsLibraryRemoved); } }

		/// <summary>Default constructor that initializes a new instance of this class with no parameters.</summary>
		///         <remarks>
		///         </remarks>
		public ALAsset () : base (NSObjectFlag.Empty)
		{
			throw new InvalidOperationException (Constants.AssetsLibraryRemoved);
		}

		/// <include file="../../docs/api/AssetsLibrary/ALAsset.xml" path="/Documentation/Docs[@DocId='M:AssetsLibrary.ALAsset.#ctor(Foundation.NSObjectFlag)']/*" />
		protected ALAsset (NSObjectFlag t) : base (t)
		{
			throw new InvalidOperationException (Constants.AssetsLibraryRemoved);
		}

		protected internal ALAsset (NativeHandle handle) : base (handle)
		{
			throw new InvalidOperationException (Constants.AssetsLibraryRemoved);
		}

		/// <include file="../../docs/api/AssetsLibrary/ALAsset.xml" path="/Documentation/Docs[@DocId='M:AssetsLibrary.ALAsset.AspectRatioThumbnail']/*" />
		public virtual CGImage AspectRatioThumbnail ()
		{
			throw new InvalidOperationException (Constants.AssetsLibraryRemoved);
		}

		/// <param name="uti">To be added.</param>
		///         <summary>Returns the specific version for the asset that matches the requested UTI.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public virtual ALAssetRepresentation RepresentationForUti (string uti)
		{
			throw new InvalidOperationException (Constants.AssetsLibraryRemoved);
		}

		/// <param name="imageData">To be added.</param>
		///         <param name="metadata">To be added.</param>
		///         <param name="completionBlock">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <summary>Changes the data to <paramref name="imageData" /> and the metadata to <paramref name="metadata" />. On success, executes <paramref name="completionBlock" />.</summary>
		///         <remarks>To be added.</remarks>
		public unsafe virtual void SetImageData (NSData imageData, NSDictionary metadata, global::System.Action<NSUrl, NSError>? completionBlock)
		{
			throw new InvalidOperationException (Constants.AssetsLibraryRemoved);
		}

		/// <param name="imageData">To be added.</param>
		///         <param name="metadata">To be added.</param>
		///         <summary>Changes the data to <paramref name="imageData" /> and the metadata to <paramref name="metadata" />.</summary>
		///         <returns>
		///           <para>A task that represents the asynchronous SetImageData operation.   The value of the TResult parameter is a <format type="text/html"><a href="https://docs.microsoft.com/en-us/search/index?search=Assets%20Library%20ALAssets%20Library%20Write%20Completion%20Delegate&amp;scope=Xamarin" title="T:AssetsLibrary.ALAssetsLibraryWriteCompletionDelegate">T:AssetsLibrary.ALAssetsLibraryWriteCompletionDelegate</a></format>.</para>
		///           <para>Application developers should check the <see cref="P:AssetsLibrary.ALAsset.Editable" /> property priot to using this method.</para>
		///         </returns>
		///         <remarks>To be added.</remarks>
		public unsafe virtual Task<NSUrl> SetImageDataAsync (NSData imageData, NSDictionary metadata)
		{
			throw new InvalidOperationException (Constants.AssetsLibraryRemoved);
		}

		/// <param name="videoPathURL">To be added.</param>
		///         <param name="completionBlock">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <summary>Modifies the <see cref="T:AssetsLibrary.ALAsset" /> to refer to the <paramref name="videoPathURL" />.</summary>
		///         <remarks>To be added.</remarks>
		public unsafe virtual void SetVideoAtPath (NSUrl videoPathURL, global::System.Action<NSUrl, NSError>? completionBlock)
		{
			throw new InvalidOperationException (Constants.AssetsLibraryRemoved);
		}

		/// <include file="../../docs/api/AssetsLibrary/ALAsset.xml" path="/Documentation/Docs[@DocId='M:AssetsLibrary.ALAsset.SetVideoAtPathAsync(Foundation.NSUrl)']/*" />
		public unsafe virtual Task<NSUrl> SetVideoAtPathAsync (NSUrl videoPathURL)
		{
			throw new InvalidOperationException (Constants.AssetsLibraryRemoved);
		}

		/// <param name="property">To be added.</param>
		///         <summary>Low-level access to properties in the ALAsset.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public virtual NSObject ValueForProperty (NSString property)
		{
			throw new InvalidOperationException (Constants.AssetsLibraryRemoved);
		}

		/// <param name="imageData">To be added.</param>
		///         <param name="metadata">To be added.</param>
		///         <param name="completionBlock">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public unsafe virtual void WriteModifiedImageToSavedToPhotosAlbum (NSData imageData, NSDictionary metadata, global::System.Action<NSUrl, NSError>? completionBlock)
		{
			throw new InvalidOperationException (Constants.AssetsLibraryRemoved);
		}

		/// <param name="imageData">To be added.</param>
		///         <param name="metadata">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>
		///           <para>A task that represents the asynchronous WriteModifiedImageToSavedToPhotosAlbum operation.   The value of the TResult parameter is a <format type="text/html"><a href="https://docs.microsoft.com/en-us/search/index?search=Assets%20Library%20ALAssets%20Library%20Write%20Completion%20Delegate&amp;scope=Xamarin" title="T:AssetsLibrary.ALAssetsLibraryWriteCompletionDelegate">T:AssetsLibrary.ALAssetsLibraryWriteCompletionDelegate</a></format>.</para>
		///           <para>Application developers should check the <see cref="P:AssetsLibrary.ALAsset.Editable" /> property prior to calling htis method.</para>
		///         </returns>
		///         <remarks>To be added.</remarks>
		public unsafe virtual Task<NSUrl> WriteModifiedImageToSavedToPhotosAlbumAsync (NSData imageData, NSDictionary metadata)
		{
			throw new InvalidOperationException (Constants.AssetsLibraryRemoved);
		}

		/// <param name="videoPathURL">To be added.</param>
		///         <param name="completionBlock">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public unsafe virtual void WriteModifiedVideoToSavedPhotosAlbum (NSUrl videoPathURL, global::System.Action<NSUrl, NSError>? completionBlock)
		{
			throw new InvalidOperationException (Constants.AssetsLibraryRemoved);
		}

		/// <param name="videoPathURL">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>
		///           <para>A task that represents the asynchronous WriteModifiedVideoToSavedPhotosAlbum operation.   The value of the TResult parameter is a <format type="text/html"><a href="https://docs.microsoft.com/en-us/search/index?search=Assets%20Library%20ALAssets%20Library%20Write%20Completion%20Delegate&amp;scope=Xamarin" title="T:AssetsLibrary.ALAssetsLibraryWriteCompletionDelegate">T:AssetsLibrary.ALAssetsLibraryWriteCompletionDelegate</a></format>.</para>
		///         </returns>
		///         <remarks>To be added.</remarks>
		public unsafe virtual Task<NSUrl> WriteModifiedVideoToSavedPhotosAlbumAsync (NSUrl videoPathURL)
		{
			throw new InvalidOperationException (Constants.AssetsLibraryRemoved);
		}

		/// <summary>Default asset representation.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public virtual ALAssetRepresentation DefaultRepresentation {
			get {
				throw new InvalidOperationException (Constants.AssetsLibraryRemoved);
			}
		}

		/// <summary>Whether the application may edit the asset.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public virtual bool Editable {
			get {
				throw new InvalidOperationException (Constants.AssetsLibraryRemoved);
			}
		}

		/// <summary>The original version, if the <see cref="T:AssetsLibrary.ALAsset" /> is a modification.</summary>
		///         <value>
		///           <see langword="null" /> if the <c>this</c> is not a modified version.</value>
		///         <remarks>To be added.</remarks>
		public virtual ALAsset OriginalAsset {
			get {
				throw new InvalidOperationException (Constants.AssetsLibraryRemoved);
			}
		}

		/// <summary>Returns the thumbnail for the asset.</summary>
		///         <value>
		///         </value>
		///         <remarks>
		///           <para>
		/// 	    This returns a square thumbnail image representing the
		/// 	    asset.  The image will be rendered in the correct
		/// 	    orientation, so it is not necessary to apply any rotation
		/// 	    on the returned image.
		///
		/// 	  </para>
		///           <para>
		/// 	    Starting with iOS 5, you can also use <see cref="M:AssetsLibrary.ALAsset.AspectRatioThumbnail" />
		/// 	    property to get a thumbnail that preserves the original
		/// 	    aspect ratio of the image, instead of the square/cropped
		/// 	    version returned by this property.
		///
		/// 	  </para>
		///         </remarks>
		public virtual CGImage Thumbnail {
			get {
				throw new InvalidOperationException (Constants.AssetsLibraryRemoved);
			}
		}

		internal static NSString _PropertyAssetURL {
			get {
				throw new InvalidOperationException (Constants.AssetsLibraryRemoved);
			}
		}

		internal static NSString _PropertyDate {
			get {
				throw new InvalidOperationException (Constants.AssetsLibraryRemoved);
			}
		}

		internal static NSString _PropertyDuration {
			get {
				throw new InvalidOperationException (Constants.AssetsLibraryRemoved);
			}
		}

		internal static NSString _PropertyLocation {
			get {
				throw new InvalidOperationException (Constants.AssetsLibraryRemoved);
			}
		}

		internal static NSString _PropertyOrientation {
			get {
				throw new InvalidOperationException (Constants.AssetsLibraryRemoved);
			}
		}

		internal static NSString _PropertyRepresentations {
			get {
				throw new InvalidOperationException (Constants.AssetsLibraryRemoved);
			}
		}

		internal static NSString _PropertyType {
			get {
				throw new InvalidOperationException (Constants.AssetsLibraryRemoved);
			}
		}

		internal static NSString _PropertyURLs {
			get {
				throw new InvalidOperationException (Constants.AssetsLibraryRemoved);
			}
		}

		internal static NSString _TypePhoto {
			get {
				throw new InvalidOperationException (Constants.AssetsLibraryRemoved);
			}
		}

		internal static NSString _TypeUnknown {
			get {
				throw new InvalidOperationException (Constants.AssetsLibraryRemoved);
			}
		}

		internal static NSString _TypeVideo {
			get {
				throw new InvalidOperationException (Constants.AssetsLibraryRemoved);
			}
		}

		/// <summary>The asset type (photo, video, unknown).</summary>
		///         <value>
		///         </value>
		///         <remarks>
		///         </remarks>
		public ALAssetType AssetType {
			get {
				throw new InvalidOperationException (Constants.AssetsLibraryRemoved);
			}
		}

		/// <summary>Geographical information about the asset.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public CLLocation Location {
			get {
				throw new InvalidOperationException (Constants.AssetsLibraryRemoved);
			}
		}

		/// <summary>For videos, play time.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public double Duration {
			get {
				throw new InvalidOperationException (Constants.AssetsLibraryRemoved);
			}
		}

		/// <summary>Asset orientation.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public ALAssetOrientation Orientation {
			get {
				throw new InvalidOperationException (Constants.AssetsLibraryRemoved);
			}
		}

		/// <summary>Asset's creation time stamp.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public NSDate Date {
			get {
				throw new InvalidOperationException (Constants.AssetsLibraryRemoved);
			}
		}

		/// <summary>Returns a list of all the available representations for this asset.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public string [] Representations {
			get {
				throw new InvalidOperationException (Constants.AssetsLibraryRemoved);
			}
		}

		/// <summary>The UTI to URL dictionary for the asset.</summary>
		///         <value>
		///         </value>
		///         <remarks>
		///         </remarks>
		public NSDictionary UtiToUrlDictionary {
			get {
				throw new InvalidOperationException (Constants.AssetsLibraryRemoved);
			}
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public NSUrl? AssetUrl {
			get {
				throw new InvalidOperationException (Constants.AssetsLibraryRemoved);
			}
		}
	} /* class ALAsset */

	/// <summary>A specific representation of an asset.</summary>
	///     <remarks>Some assets can have more than one representation.   Consider images that are stored in two different formats for example, this class represents a particular reprensetation of the asset.</remarks>
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/AssetsLibrary/Reference/ALAssetRepresentation_Class/index.html">Apple documentation for <c>ALAssetRepresentation</c></related>
	[EditorBrowsable (EditorBrowsableState.Never)]
	[Obsolete (Constants.AssetsLibraryRemoved)]
	public unsafe partial class ALAssetRepresentation : NSObject {
		/// <summary>The handle for this class.</summary>
		///         <value>The pointer to the Objective-C class.</value>
		///         <remarks>Each Xamarin.iOS class mirrors an unmanaged Objective-C class.   This value contains the pointer to the Objective-C class, it is similar to calling objc_getClass with the object name.</remarks>
		public override NativeHandle ClassHandle { get { throw new InvalidOperationException (Constants.AssetsLibraryRemoved); } }

		/// <summary>Default constructor that initializes a new instance of this class with no parameters.</summary>
		///         <remarks>
		///         </remarks>
		public ALAssetRepresentation () : base (NSObjectFlag.Empty)
		{
			throw new InvalidOperationException (Constants.AssetsLibraryRemoved);
		}

		/// <include file="../../docs/api/AssetsLibrary/ALAssetRepresentation.xml" path="/Documentation/Docs[@DocId='M:AssetsLibrary.ALAssetRepresentation.#ctor(Foundation.NSObjectFlag)']/*" />
		protected ALAssetRepresentation (NSObjectFlag t) : base (t)
		{
			throw new InvalidOperationException (Constants.AssetsLibraryRemoved);
		}

		protected internal ALAssetRepresentation (NativeHandle handle) : base (handle)
		{
			throw new InvalidOperationException (Constants.AssetsLibraryRemoved);
		}

		public virtual nuint GetBytes (global::System.IntPtr buffer, long offset, nuint length, out NSError error)
		{
			throw new InvalidOperationException (Constants.AssetsLibraryRemoved);
		}

		/// <summary>Returns an image suitable for full screen use.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public virtual CGImage GetFullScreenImage ()
		{
			throw new InvalidOperationException (Constants.AssetsLibraryRemoved);
		}

		/// <summary>Returns the image representing this asset.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public virtual CGImage GetImage ()
		{
			throw new InvalidOperationException (Constants.AssetsLibraryRemoved);
		}

		/// <param name="options">CGImage options.</param>
		///         <summary>Returns an image representing the asset.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public virtual CGImage GetImage (NSDictionary options)
		{
			throw new InvalidOperationException (Constants.AssetsLibraryRemoved);
		}

		/// <summary>Gets the size of the asset's representation.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public virtual CGSize Dimensions {
			get {
				throw new InvalidOperationException (Constants.AssetsLibraryRemoved);
			}
		}

		/// <summary>Gets the name of the file that holds the representation.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public virtual string Filename {
			get {
				throw new InvalidOperationException (Constants.AssetsLibraryRemoved);
			}
		}

		/// <summary>Asset metadata stored as a dictionary.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public virtual NSDictionary Metadata {
			get {
				throw new InvalidOperationException (Constants.AssetsLibraryRemoved);
			}
		}

		/// <summary>The asset orientation.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public virtual ALAssetOrientation Orientation {
			get {
				throw new InvalidOperationException (Constants.AssetsLibraryRemoved);
			}
		}

		/// <summary>The scale for this asset representation.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public virtual float Scale {
			get {
				throw new InvalidOperationException (Constants.AssetsLibraryRemoved);
			}
		}

		/// <summary>The size in bytes of this asset representation.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public virtual long Size {
			get {
				throw new InvalidOperationException (Constants.AssetsLibraryRemoved);
			}
		}

		/// <summary>The URL for this specific asset reprensetation.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public virtual NSUrl Url {
			get {
				throw new InvalidOperationException (Constants.AssetsLibraryRemoved);
			}
		}

		/// <summary>The UTI of this asset.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public virtual string Uti {
			get {
				throw new InvalidOperationException (Constants.AssetsLibraryRemoved);
			}
		}
	} /* class ALAssetRepresentation */

	/// <summary>Keys used to limit asset enumeration by a specific kind.</summary>
	///     <remarks>
	///     </remarks>
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/AssetsLibrary/Reference/ALAssetsFilter_Class/index.html">Apple documentation for <c>ALAssetsFilter</c></related>
	[EditorBrowsable (EditorBrowsableState.Never)]
	[Obsolete (Constants.AssetsLibraryRemoved)]
	public unsafe partial class ALAssetsFilter : NSObject {
		/// <summary>The handle for this class.</summary>
		///         <value>The pointer to the Objective-C class.</value>
		///         <remarks>Each Xamarin.iOS class mirrors an unmanaged Objective-C class.   This value contains the pointer to the Objective-C class, it is similar to calling objc_getClass with the object name.</remarks>
		public override NativeHandle ClassHandle { get { throw new InvalidOperationException (Constants.AssetsLibraryRemoved); } }

		/// <summary>Default constructor that initializes a new instance of this class with no parameters.</summary>
		///         <remarks>
		///         </remarks>
		public ALAssetsFilter () : base (NSObjectFlag.Empty)
		{
			throw new InvalidOperationException (Constants.AssetsLibraryRemoved);
		}

		/// <include file="../../docs/api/AssetsLibrary/ALAssetsFilter.xml" path="/Documentation/Docs[@DocId='M:AssetsLibrary.ALAssetsFilter.#ctor(Foundation.NSObjectFlag)']/*" />
		protected ALAssetsFilter (NSObjectFlag t) : base (t)
		{
			throw new InvalidOperationException (Constants.AssetsLibraryRemoved);
		}

		protected internal ALAssetsFilter (NativeHandle handle) : base (handle)
		{
			throw new InvalidOperationException (Constants.AssetsLibraryRemoved);
		}

		/// <summary>Used to list all assets</summary>
		///         <value>
		///         </value>
		///         <remarks>
		///         </remarks>
		public static ALAssetsFilter AllAssets {
			get {
				throw new InvalidOperationException (Constants.AssetsLibraryRemoved);
			}
		}

		/// <summary>Used to list photo assets.</summary>
		///         <value>
		///         </value>
		///         <remarks>
		///         </remarks>
		public static ALAssetsFilter AllPhotos {
			get {
				throw new InvalidOperationException (Constants.AssetsLibraryRemoved);
			}
		}

		/// <summary>Used to list video assets.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public static ALAssetsFilter AllVideos {
			get {
				throw new InvalidOperationException (Constants.AssetsLibraryRemoved);
			}
		}
	} /* class ALAssetsFilter */

	/// <summary>Sets of assets managed by the Photo application.</summary>
	///     <remarks>To be added.</remarks>
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/AssetsLibrary/Reference/ALAssetsGroup_Class/index.html">Apple documentation for <c>ALAssetsGroup</c></related>
	[EditorBrowsable (EditorBrowsableState.Never)]
	[Obsolete (Constants.AssetsLibraryRemoved)]
	public unsafe partial class ALAssetsGroup : NSObject {
		/// <summary>The handle for this class.</summary>
		///         <value>The pointer to the Objective-C class.</value>
		///         <remarks>Each Xamarin.iOS class mirrors an unmanaged Objective-C class.   This value contains the pointer to the Objective-C class, it is similar to calling objc_getClass with the object name.</remarks>
		public override NativeHandle ClassHandle { get { throw new InvalidOperationException (Constants.AssetsLibraryRemoved); } }

		/// <summary>Default constructor that initializes a new instance of this class with no parameters.</summary>
		///         <remarks>
		///         </remarks>
		public ALAssetsGroup () : base (NSObjectFlag.Empty)
		{
			throw new InvalidOperationException (Constants.AssetsLibraryRemoved);
		}

		/// <include file="../../docs/api/AssetsLibrary/ALAssetsGroup.xml" path="/Documentation/Docs[@DocId='M:AssetsLibrary.ALAssetsGroup.#ctor(Foundation.NSObjectFlag)']/*" />
		protected ALAssetsGroup (NSObjectFlag t) : base (t)
		{
			throw new InvalidOperationException (Constants.AssetsLibraryRemoved);
		}

		protected internal ALAssetsGroup (NativeHandle handle) : base (handle)
		{
			throw new InvalidOperationException (Constants.AssetsLibraryRemoved);
		}

		/// <param name="asset">The asset being added.</param>
		///         <summary>Adds an asset to an existing <see cref="T:AssetsLibrary.ALAssetsGroup" />.</summary>
		///         <returns>
		///           <see langword="true" /> if the <paramref name="asset" /> was added successfully.</returns>
		///         <remarks>To be added.</remarks>
		public virtual bool AddAsset (ALAsset asset)
		{
			throw new InvalidOperationException (Constants.AssetsLibraryRemoved);
		}

		/// <param name="result">Delegate or anonymous method to invoke with the matching asset.</param>
		///         <summary>Enumerates all the assets in the group.</summary>
		///         <remarks>To be added.</remarks>
		public unsafe virtual void Enumerate (ALAssetsEnumerator result)
		{
			throw new InvalidOperationException (Constants.AssetsLibraryRemoved);
		}

		/// <param name="options">Enumeration options.</param>
		///         <param name="result">Delegate or anonymous method to invoke with the matching asset.</param>
		///         <summary>Enumerates all the assets in the group with the specified options.</summary>
		///         <remarks>
		///         </remarks>
		public unsafe virtual void Enumerate (NSEnumerationOptions options, ALAssetsEnumerator result)
		{
			throw new InvalidOperationException (Constants.AssetsLibraryRemoved);
		}

		/// <param name="indexSet">The index of the assets to enumerate.</param>
		///         <param name="options">Enumeration options.</param>
		///         <param name="result">Delegate or anonymous method to invoke with the matching asset.</param>
		///         <summary>Enumerates assets in the group with the specified options.</summary>
		///         <remarks>To be added.</remarks>
		public unsafe virtual void Enumerate (NSIndexSet indexSet, NSEnumerationOptions options, ALAssetsEnumerator result)
		{
			throw new InvalidOperationException (Constants.AssetsLibraryRemoved);
		}

		/// <param name="filter">
		///         </param>
		///         <summary>Sets the filter for enumerating assets on this group.</summary>
		///         <remarks>To be added.</remarks>
		public virtual void SetAssetsFilter (ALAssetsFilter filter)
		{
			throw new InvalidOperationException (Constants.AssetsLibraryRemoved);
		}

		internal virtual NSObject ValueForProperty (NSString property)
		{
			throw new InvalidOperationException (Constants.AssetsLibraryRemoved);
		}

		/// <summary>The number of assets in this group.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public virtual nint Count {
			get {
				throw new InvalidOperationException (Constants.AssetsLibraryRemoved);
			}
		}

		/// <summary>Whether the app may edit the group.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public virtual bool Editable {
			get {
				throw new InvalidOperationException (Constants.AssetsLibraryRemoved);
			}
		}

		/// <summary>Poster image used for this group.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public virtual CGImage PosterImage {
			get {
				throw new InvalidOperationException (Constants.AssetsLibraryRemoved);
			}
		}

		internal static NSString _Name {
			get {
				throw new InvalidOperationException (Constants.AssetsLibraryRemoved);
			}
		}

		internal static NSString _PersistentID {
			get {
				throw new InvalidOperationException (Constants.AssetsLibraryRemoved);
			}
		}

		internal static NSString _PropertyUrl {
			get {
				throw new InvalidOperationException (Constants.AssetsLibraryRemoved);
			}
		}

		internal static NSString _Type {
			get {
				throw new InvalidOperationException (Constants.AssetsLibraryRemoved);
			}
		}

		/// <summary>Name of this group.</summary>
		///         <value>The name of the group.</value>
		///         <remarks>To be added.</remarks>
		///         <altmember cref="M:AssetsLibrary.ALAssetsGroup.PersistentGroup" />
		public NSString Name {
			get {
				throw new InvalidOperationException (Constants.AssetsLibraryRemoved);
			}
		}

		/// <summary>The type of assets of this group (Library, SavedPhotos, Faces or Events).</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public ALAssetsGroupType Type {
			get {
				throw new InvalidOperationException (Constants.AssetsLibraryRemoved);
			}
		}

		/// <summary>To be added.</summary>
		///         <value>The persistent ID of the <see cref="T:AssetsLibrary.ALAssetsGroup" />.</value>
		///         <remarks>To be added.</remarks>
		public string PersistentID {
			get {
				throw new InvalidOperationException (Constants.AssetsLibraryRemoved);
			}
		}

		/// <summary>A unique reference URL for the <see cref="T:AssetsLibrary.ALAssetsGroup" />.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public NSUrl PropertyUrl {
			get {
				throw new InvalidOperationException (Constants.AssetsLibraryRemoved);
			}
		}
	} /* class ALAssetsGroup */

	/// <summary>A class that encapsulates access to the video and media of the "Photos" application.</summary>
	///     <remarks>To be added.</remarks>
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/AssetsLibrary/Reference/ALAssetsLibrary_Class/index.html">Apple documentation for <c>ALAssetsLibrary</c></related>
	[EditorBrowsable (EditorBrowsableState.Never)]
	[Obsolete (Constants.AssetsLibraryRemoved)]
	public unsafe partial class ALAssetsLibrary : NSObject {
		/// <summary>The handle for this class.</summary>
		///         <value>The pointer to the Objective-C class.</value>
		///         <remarks>Each Xamarin.iOS class mirrors an unmanaged Objective-C class.   This value contains the pointer to the Objective-C class, it is similar to calling objc_getClass with the object name.</remarks>
		public override NativeHandle ClassHandle { get { throw new InvalidOperationException (Constants.AssetsLibraryRemoved); } }

		/// <summary>Default constructor that initializes a new instance of this class with no parameters.</summary>
		///         <remarks>
		///         </remarks>
		public ALAssetsLibrary () : base (NSObjectFlag.Empty)
		{
			throw new InvalidOperationException (Constants.AssetsLibraryRemoved);
		}

		/// <include file="../../docs/api/AssetsLibrary/ALAssetsLibrary.xml" path="/Documentation/Docs[@DocId='M:AssetsLibrary.ALAssetsLibrary.#ctor(Foundation.NSObjectFlag)']/*" />
		protected ALAssetsLibrary (NSObjectFlag t) : base (t)
		{
			throw new InvalidOperationException (Constants.AssetsLibraryRemoved);
		}

		protected internal ALAssetsLibrary (NativeHandle handle) : base (handle)
		{
			throw new InvalidOperationException (Constants.AssetsLibraryRemoved);
		}

		/// <param name="name">The name of the asset group to be created.</param>
		///         <param name="resultBlock">Executed if there is no error during the asset creation.</param>
		///         <param name="failureBlock">Executed if there was a failure, such as from the user denying the application from accessing the library.</param>
		///         <summary>Creates an asset group (such as an album of photographs) to the <see cref="T:AssetsLibrary.ALAssetsLibrary" />.</summary>
		///         <remarks>To be added.</remarks>
		public unsafe virtual void AddAssetsGroupAlbum (string name, global::System.Action<ALAssetsGroup> resultBlock, global::System.Action<NSError> failureBlock)
		{
			throw new InvalidOperationException (Constants.AssetsLibraryRemoved);
		}

		/// <param name="assetURL">To be added.</param>
		///         <param name="resultBlock">To be added.</param>
		///         <param name="failureBlock">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public unsafe virtual void AssetForUrl (NSUrl assetURL, global::System.Action<ALAsset> resultBlock, global::System.Action<NSError> failureBlock)
		{
			throw new InvalidOperationException (Constants.AssetsLibraryRemoved);
		}

		/// <summary>Disables notifications and asset retrieval from shared photo streams.</summary>
		///         <remarks>To be added.</remarks>
		public static void DisableSharedPhotoStreamsSupport ()
		{
			throw new InvalidOperationException (Constants.AssetsLibraryRemoved);
		}

		/// <param name="types">To be added.</param>
		///         <param name="enumerationBlock">To be added.</param>
		///         <param name="failureBlock">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public unsafe virtual void Enumerate (ALAssetsGroupType types, ALAssetsLibraryGroupsEnumerationResultsDelegate enumerationBlock, global::System.Action<NSError> failureBlock)
		{
			throw new InvalidOperationException (Constants.AssetsLibraryRemoved);
		}

		/// <param name="groupURL">To be added.</param>
		///         <param name="resultBlock">To be added.</param>
		///         <param name="failureBlock">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public unsafe virtual void GroupForUrl (NSUrl groupURL, global::System.Action<ALAssetsGroup> resultBlock, global::System.Action<NSError> failureBlock)
		{
			throw new InvalidOperationException (Constants.AssetsLibraryRemoved);
		}

		/// <param name="videoPathURL">A URL locating a video.</param>
		///         <summary>Whether the video at <paramref name="videoPathURL" /> can be saved in the Photos Album.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>
		///           <para>Application developers implementing custom Audio-Video capture or editing video files should use this method prior to attempting to add the video to the Photos Album.</para>
		///         </remarks>
		public virtual bool VideoAtPathIsIsCompatibleWithSavedPhotosAlbum (NSUrl videoPathURL)
		{
			throw new InvalidOperationException (Constants.AssetsLibraryRemoved);
		}

		/// <param name="imageData">To be added.</param>
		///         <param name="metadata">To be added.</param>
		///         <param name="completionBlock">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public unsafe virtual void WriteImageToSavedPhotosAlbum (NSData imageData, NSDictionary metadata, global::System.Action<NSUrl, NSError>? completionBlock)
		{
			throw new InvalidOperationException (Constants.AssetsLibraryRemoved);
		}

		/// <param name="imageData">To be added.</param>
		///         <param name="metadata">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>
		///           <para>A task that represents the asynchronous WriteImageToSavedPhotosAlbum operation.   The value of the TResult parameter is a <format type="text/html"><a href="https://docs.microsoft.com/en-us/search/index?search=Assets%20Library%20ALAssets%20Library%20Write%20Completion%20Delegate&amp;scope=Xamarin" title="T:AssetsLibrary.ALAssetsLibraryWriteCompletionDelegate">T:AssetsLibrary.ALAssetsLibraryWriteCompletionDelegate</a></format>.</para>
		///         </returns>
		///         <remarks>To be added.</remarks>
		public unsafe virtual Task<NSUrl> WriteImageToSavedPhotosAlbumAsync (NSData imageData, NSDictionary metadata)
		{
			throw new InvalidOperationException (Constants.AssetsLibraryRemoved);
		}

		/// <param name="imageData">To be added.</param>
		///         <param name="metadata">To be added.</param>
		///         <param name="completionBlock">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public unsafe virtual void WriteImageToSavedPhotosAlbum (CGImage imageData, NSDictionary metadata, global::System.Action<NSUrl, NSError>? completionBlock)
		{
			throw new InvalidOperationException (Constants.AssetsLibraryRemoved);
		}

		/// <param name="imageData">To be added.</param>
		///         <param name="metadata">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>
		///           <para>A task that represents the asynchronous WriteImageToSavedPhotosAlbum operation.   The value of the TResult parameter is a <format type="text/html"><a href="https://docs.microsoft.com/en-us/search/index?search=Assets%20Library%20ALAssets%20Library%20Write%20Completion%20Delegate&amp;scope=Xamarin" title="T:AssetsLibrary.ALAssetsLibraryWriteCompletionDelegate">T:AssetsLibrary.ALAssetsLibraryWriteCompletionDelegate</a></format>.</para>
		///         </returns>
		///         <remarks>To be added.</remarks>
		public unsafe virtual Task<NSUrl> WriteImageToSavedPhotosAlbumAsync (CGImage imageData, NSDictionary metadata)
		{
			throw new InvalidOperationException (Constants.AssetsLibraryRemoved);
		}

		/// <param name="imageData">To be added.</param>
		///         <param name="orientation">To be added.</param>
		///         <param name="completionBlock">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public unsafe virtual void WriteImageToSavedPhotosAlbum (CGImage imageData, ALAssetOrientation orientation, global::System.Action<NSUrl, NSError>? completionBlock)
		{
			throw new InvalidOperationException (Constants.AssetsLibraryRemoved);
		}

		/// <param name="imageData">To be added.</param>
		///         <param name="orientation">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>
		///           <para>A task that represents the asynchronous WriteImageToSavedPhotosAlbum operation.   The value of the TResult parameter is a <format type="text/html"><a href="https://docs.microsoft.com/en-us/search/index?search=Assets%20Library%20ALAssets%20Library%20Write%20Completion%20Delegate&amp;scope=Xamarin" title="T:AssetsLibrary.ALAssetsLibraryWriteCompletionDelegate">T:AssetsLibrary.ALAssetsLibraryWriteCompletionDelegate</a></format>.</para>
		///         </returns>
		///         <remarks>To be added.</remarks>
		public unsafe virtual Task<NSUrl> WriteImageToSavedPhotosAlbumAsync (CGImage imageData, ALAssetOrientation orientation)
		{
			throw new InvalidOperationException (Constants.AssetsLibraryRemoved);
		}

		/// <param name="videoPathURL">To be added.</param>
		///         <param name="completionBlock">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public unsafe virtual void WriteVideoToSavedPhotosAlbum (NSUrl videoPathURL, global::System.Action<NSUrl, NSError>? completionBlock)
		{
			throw new InvalidOperationException (Constants.AssetsLibraryRemoved);
		}

		/// <param name="videoPathURL">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>
		///           <para>A task that represents the asynchronous WriteVideoToSavedPhotosAlbum operation.   The value of the TResult parameter is a <format type="text/html"><a href="https://docs.microsoft.com/en-us/search/index?search=Assets%20Library%20ALAssets%20Library%20Write%20Completion%20Delegate&amp;scope=Xamarin" title="T:AssetsLibrary.ALAssetsLibraryWriteCompletionDelegate">T:AssetsLibrary.ALAssetsLibraryWriteCompletionDelegate</a></format>.</para>
		///         </returns>
		///         <remarks>
		///           <para copied="true">The WriteVideoToSavedPhotosAlbumAsync method is suitable to be used with C# async by returning control to the caller with a Task representing the operation.</para>
		///           <para copied="true">To be added.</para>
		///         </remarks>
		public unsafe virtual Task<NSUrl> WriteVideoToSavedPhotosAlbumAsync (NSUrl videoPathURL)
		{
			throw new InvalidOperationException (Constants.AssetsLibraryRemoved);
		}

		/// <summary>Reflects the permissions of the application to access the <see cref="T:AssetsLibrary.ALAssetsLibrary" />.</summary>
		///         <value>The current status of the application.</value>
		///         <remarks>To be added.</remarks>
		public static ALAuthorizationStatus AuthorizationStatus {
			get {
				throw new InvalidOperationException (Constants.AssetsLibraryRemoved);
			}
		}

		/// <include file="../../docs/api/AssetsLibrary/ALAssetsLibrary.xml" path="/Documentation/Docs[@DocId='P:AssetsLibrary.ALAssetsLibrary.ChangedNotification']/*" />
		[Advice ("Use ALAssetsLibrary.Notifications.ObserveChanged helper method instead.")]
		public static NSString ChangedNotification {
			get {
				throw new InvalidOperationException (Constants.AssetsLibraryRemoved);
			}
		}

		/// <summary>Represents the value associated with the constant ALAssetLibraryDeletedAssetGroupsKey</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		public static NSString DeletedAssetGroupsKey {
			get {
				throw new InvalidOperationException (Constants.AssetsLibraryRemoved);
			}
		}

		/// <summary>Represents the value associated with the constant ALAssetLibraryInsertedAssetGroupsKey</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		public static NSString InsertedAssetGroupsKey {
			get {
				throw new InvalidOperationException (Constants.AssetsLibraryRemoved);
			}
		}

		/// <summary>Represents the value associated with the constant ALAssetLibraryUpdatedAssetGroupsKey</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		public static NSString UpdatedAssetGroupsKey {
			get {
				throw new InvalidOperationException (Constants.AssetsLibraryRemoved);
			}
		}

		/// <summary>Represents the value associated with the constant ALAssetLibraryUpdatedAssetsKey</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		public static NSString UpdatedAssetsKey {
			get {
				throw new InvalidOperationException (Constants.AssetsLibraryRemoved);
			}
		}

		//
		// Notifications
		//
		/// <summary>Notification posted by the <see cref="T:AssetsLibrary.ALAssetsLibrary" /> class.</summary>
		///     <remarks>
		///       <para>This is a static class which contains various helper methods that allow developers to observe events posted in the iOS notification hub (<see cref="T:Foundation.NSNotificationCenter" />).</para>
		///       <para>The methods defined in this class post events invoke the provided method or lambda with a <see cref="T:Foundation.NSNotificationEventArgs" /> parameter which contains strongly typed properties for the notification arguments.</para>
		///     </remarks>
		[EditorBrowsable (EditorBrowsableState.Never)]
		[Obsolete (Constants.AssetsLibraryRemoved)]
		public static partial class Notifications {
			/// <include file="../../docs/api/AssetsLibrary.ALAssetsLibrary/Notifications.xml" path="/Documentation/Docs[@DocId='M:AssetsLibrary.ALAssetsLibrary.Notifications.ObserveChanged(System.EventHandler{Foundation.NSNotificationEventArgs})']/*" />
			public static NSObject ObserveChanged (EventHandler<NSNotificationEventArgs> handler)
			{
				throw new InvalidOperationException (Constants.AssetsLibraryRemoved);
			}
			/// <include file="../../docs/api/AssetsLibrary.ALAssetsLibrary/Notifications.xml" path="/Documentation/Docs[@DocId='M:AssetsLibrary.ALAssetsLibrary.Notifications.ObserveChanged(Foundation.NSObject,System.EventHandler{Foundation.NSNotificationEventArgs})']/*" />
			public static NSObject ObserveChanged (NSObject objectToObserve, EventHandler<NSNotificationEventArgs> handler)
			{
				throw new InvalidOperationException (Constants.AssetsLibraryRemoved);
			}
			/// <include file="../../docs/api/AssetsLibrary.ALAssetsLibrary/Notifications.xml" path="/Documentation/Docs[@DocId='M:AssetsLibrary.ALAssetsLibrary.Notifications.ObserveChanged(System.EventHandler{AssetsLibrary.ALAssetLibraryChangedEventArgs})']/*" />
			public static NSObject ObserveChanged (EventHandler<AssetsLibrary.ALAssetLibraryChangedEventArgs> handler)
			{
				throw new InvalidOperationException (Constants.AssetsLibraryRemoved);
			}
			/// <include file="../../docs/api/AssetsLibrary.ALAssetsLibrary/Notifications.xml" path="/Documentation/Docs[@DocId='M:AssetsLibrary.ALAssetsLibrary.Notifications.ObserveChanged(Foundation.NSObject,System.EventHandler{AssetsLibrary.ALAssetLibraryChangedEventArgs})']/*" />
			public static NSObject ObserveChanged (NSObject objectToObserve, EventHandler<AssetsLibrary.ALAssetLibraryChangedEventArgs> handler)
			{
				throw new InvalidOperationException (Constants.AssetsLibraryRemoved);
			}
		}
	} /* class ALAssetsLibrary */

	/// <summary>Provides data for the  event.</summary>
	///     <remarks>
	///     </remarks>
	[EditorBrowsable (EditorBrowsableState.Never)]
	[Obsolete (Constants.AssetsLibraryRemoved)]
	public partial class ALAssetLibraryChangedEventArgs : NSNotificationEventArgs {
		/// <param name="notification">To be added.</param>
		///         <summary>Initializes a new instance of the ALAssetLibraryChangedEventArgs class.</summary>
		///         <remarks>
		///         </remarks>
		public ALAssetLibraryChangedEventArgs (NSNotification notification) : base (notification)
		{
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public Foundation.NSSet UpdatedAssets {
			get {
				throw new InvalidOperationException (Constants.AssetsLibraryRemoved);
			}
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public Foundation.NSSet InsertedAssetGroups {
			get {
				throw new InvalidOperationException (Constants.AssetsLibraryRemoved);
			}
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public Foundation.NSSet UpdatedAssetGroups {
			get {
				throw new InvalidOperationException (Constants.AssetsLibraryRemoved);
			}
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public Foundation.NSSet DeletedAssetGroupsKey {
			get {
				throw new InvalidOperationException (Constants.AssetsLibraryRemoved);
			}
		}
	}

}

#endif // !XAMCORE_5_0
