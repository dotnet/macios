// Copyright 2016 Xamarin, Inc.
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System;

using AppKit;
using Foundation;
using ObjCRuntime;

#if !NET
using NativeHandle = System.IntPtr;
#endif

namespace MediaLibrary {
	[Static]
	[Deprecated (PlatformName.MacOSX, 10, 15)]
	interface MediaLibraryTypeIdentifierKey {
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MLFolderRootGroupTypeIdentifier")]
		NSString FolderRootGroupTypeIdentifier { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MLFolderGroupTypeIdentifier")]
		NSString FolderGroupTypeIdentifier { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MLiTunesRootGroupTypeIdentifier")]
		NSString ITunesRootGroupTypeIdentifier { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MLiTunesPlaylistTypeIdentifier")]
		NSString ITunesPlaylistTypeIdentifier { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MLiTunesPurchasedPlaylistTypeIdentifier")]
		NSString ITunesPurchasedPlaylistTypeIdentifier { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MLiTunesPodcastPlaylistTypeIdentifier")]
		NSString ITunesPodcastPlaylistTypeIdentifier { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MLiTunesVideoPlaylistTypeIdentifier")]
		NSString ITunesVideoPlaylistTypeIdentifier { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MLiTunesSmartPlaylistTypeIdentifier")]
		NSString ITunesSmartPlaylistTypeIdentifier { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MLiTunesFolderPlaylistTypeIdentifier")]
		NSString ITunesFolderPlaylistTypeIdentifier { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MLiTunesMoviesPlaylistTypeIdentifier")]
		NSString ITunesMoviesPlaylistTypeIdentifier { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MLiTunesTVShowsPlaylistTypeIdentifier")]
		NSString ITunesTVShowsPlaylistTypeIdentifier { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MLiTunesAudioBooksPlaylistTypeIdentifier")]
		NSString ITunesAudioBooksPlaylistTypeIdentifier { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MLiTunesMusicPlaylistTypeIdentifier")]
		NSString ITunesMusicPlaylistTypeIdentifier { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MLiTunesMusicVideosPlaylistTypeIdentifier")]
		NSString ITunesMusicVideosPlaylistTypeIdentifier { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MLiTunesGeniusPlaylistTypeIdentifier")]
		NSString ITunesGeniusPlaylistTypeIdentifier { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MLiTunesSavedGeniusPlaylistTypeIdentifier")]
		NSString ITunesSavedGeniusPlaylistTypeIdentifier { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MLiTunesiTunesUPlaylistTypeIdentifier")]
		NSString ITunesiTunesUPlaylistTypeIdentifier { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MLPhotosRootGroupTypeIdentifier")]
		NSString PhotosRootGroupTypeIdentifier { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MLPhotosSharedGroupTypeIdentifier")]
		NSString PhotosSharedGroupTypeIdentifier { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MLPhotosAlbumsGroupTypeIdentifier")]
		NSString PhotosAlbumsGroupTypeIdentifier { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MLPhotosAlbumTypeIdentifier")]
		NSString PhotosAlbumTypeIdentifier { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MLPhotosFolderTypeIdentifier")]
		NSString PhotosFolderTypeIdentifier { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MLPhotosSmartAlbumTypeIdentifier")]
		NSString PhotosSmartAlbumTypeIdentifier { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MLPhotosPublishedAlbumTypeIdentifier")]
		NSString PhotosPublishedAlbumTypeIdentifier { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Deprecated (PlatformName.MacOSX, 10, 15)]
		[Field ("MLPhotosAllMomentsGroupTypeIdentifier")]
		NSString PhotosAllMomentsGroupTypeIdentifier { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Deprecated (PlatformName.MacOSX, 10, 15)]
		[Field ("MLPhotosMomentGroupTypeIdentifier")]
		NSString PhotosMomentGroupTypeIdentifier { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Deprecated (PlatformName.MacOSX, 10, 15)]
		[Field ("MLPhotosAllCollectionsGroupTypeIdentifier")]
		NSString PhotosAllCollectionsGroupTypeIdentifier { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Deprecated (PlatformName.MacOSX, 10, 15)]
		[Field ("MLPhotosCollectionGroupTypeIdentifier")]
		NSString PhotosCollectionGroupTypeIdentifier { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Deprecated (PlatformName.MacOSX, 10, 15)]
		[Field ("MLPhotosAllYearsGroupTypeIdentifier")]
		NSString PhotosAllYearsGroupTypeIdentifier { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Deprecated (PlatformName.MacOSX, 10, 15)]
		[Field ("MLPhotosYearGroupTypeIdentifier")]
		NSString PhotosYearGroupTypeIdentifier { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MLPhotosLastImportGroupTypeIdentifier")]
		NSString PhotosLastImportGroupTypeIdentifier { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MLPhotosMyPhotoStreamTypeIdentifier")]
		NSString PhotosMyPhotoStreamTypeIdentifier { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MLPhotosSharedPhotoStreamTypeIdentifier")]
		NSString PhotosSharedPhotoStreamTypeIdentifier { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MLPhotosFavoritesGroupTypeIdentifier")]
		NSString PhotosFavoritesGroupTypeIdentifier { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MLPhotosFrontCameraGroupTypeIdentifier")]
		NSString PhotosFrontCameraGroupTypeIdentifier { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MLPhotosLivePhotosGroupTypeIdentifier")]
		NSString PhotosLivePhotosGroupTypeIdentifier { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MLPhotosLongExposureGroupTypeIdentifier")]
		NSString PhotosLongExposureGroupTypeIdentifier { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MLPhotosAnimatedGroupTypeIdentifier")]
		NSString PhotosAnimatedGroupTypeIdentifier { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MLPhotosPanoramasGroupTypeIdentifier")]
		NSString PhotosPanoramasGroupTypeIdentifier { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MLPhotosVideosGroupTypeIdentifier")]
		NSString PhotosVideosGroupTypeIdentifier { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MLPhotosSloMoGroupTypeIdentifier")]
		NSString PhotosSloMoGroupTypeIdentifier { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MLPhotosTimelapseGroupTypeIdentifier")]
		NSString PhotosTimelapseGroupTypeIdentifier { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MLPhotosBurstGroupTypeIdentifier")]
		NSString PhotosBurstGroupTypeIdentifier { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MLPhotosScreenshotGroupTypeIdentifier")]
		NSString PhotosScreenshotGroupTypeIdentifier { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MLPhotosFacesAlbumTypeIdentifier")]
		NSString PhotosFacesAlbumTypeIdentifier { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MLPhotosAllPhotosAlbumTypeIdentifier")]
		NSString PhotosAllPhotosAlbumTypeIdentifier { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MLPhotosDepthEffectGroupTypeIdentifier")]
		NSString PhotosDepthEffectGroupTypeIdentifier { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MLiPhotoRootGroupTypeIdentifier")]
		NSString IPhotoRootGroupTypeIdentifier { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MLiPhotoAlbumTypeIdentifier")]
		NSString IPhotoAlbumTypeIdentifier { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MLiPhotoLibraryAlbumTypeIdentifier")]
		NSString IPhotoLibraryAlbumTypeIdentifier { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MLiPhotoEventsFolderTypeIdentifier")]
		NSString IPhotoEventsFolderTypeIdentifier { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MLiPhotoSmartAlbumTypeIdentifier")]
		NSString IPhotoSmartAlbumTypeIdentifier { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MLiPhotoEventAlbumTypeIdentifier")]
		NSString IPhotoEventAlbumTypeIdentifier { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MLiPhotoLastImportAlbumTypeIdentifier")]
		NSString IPhotoLastImportAlbumTypeIdentifier { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MLiPhotoLastNMonthsAlbumTypeIdentifier")]
		NSString IPhotoLastNMonthsAlbumTypeIdentifier { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MLiPhotoFlaggedAlbumTypeIdentifier")]
		NSString IPhotoFlaggedAlbumTypeIdentifier { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MLiPhotoFolderAlbumTypeIdentifier")]
		NSString IPhotoFolderAlbumTypeIdentifier { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MLiPhotoSubscribedAlbumTypeIdentifier")]
		NSString IPhotoSubscribedAlbumTypeIdentifier { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MLiPhotoFacesAlbumTypeIdentifier")]
		NSString IPhotoFacesAlbumTypeIdentifier { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MLiPhotoPlacesAlbumTypeIdentifier")]
		NSString IPhotoPlacesAlbumTypeIdentifier { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MLiPhotoPlacesCountryAlbumTypeIdentifier")]
		NSString IPhotoPlacesCountryAlbumTypeIdentifier { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MLiPhotoPlacesProvinceAlbumTypeIdentifier")]
		NSString IPhotoPlacesProvinceAlbumTypeIdentifier { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MLiPhotoPlacesCityAlbumTypeIdentifier")]
		NSString IPhotoPlacesCityAlbumTypeIdentifier { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MLiPhotoPlacesPointOfInterestAlbumTypeIdentifier")]
		NSString IPhotoPlacesPointOfInterestAlbumTypeIdentifier { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MLiPhotoFacebookAlbumTypeIdentifier")]
		NSString IPhotoFacebookAlbumTypeIdentifier { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MLiPhotoFlickrAlbumTypeIdentifier")]
		NSString IPhotoFlickrAlbumTypeIdentifier { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MLiPhotoFacebookGroupTypeIdentifier")]
		NSString IPhotoFacebookGroupTypeIdentifier { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MLiPhotoFlickrGroupTypeIdentifier")]
		NSString IPhotoFlickrGroupTypeIdentifier { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MLiPhotoSlideShowAlbumTypeIdentifier")]
		NSString IPhotoSlideShowAlbumTypeIdentifier { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MLiPhotoLastViewedEventAlbumTypeIdentifier")]
		NSString IPhotoLastViewedEventAlbumTypeIdentifier { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MLiPhotoPhotoStreamAlbumTypeIdentifier")]
		NSString IPhotoPhotoStreamAlbumTypeIdentifier { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MLApertureRootGroupTypeIdentifier")]
		NSString ApertureRootGroupTypeIdentifier { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MLApertureUserAlbumTypeIdentifier")]
		NSString ApertureUserAlbumTypeIdentifier { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MLApertureUserSmartAlbumTypeIdentifier")]
		NSString ApertureUserSmartAlbumTypeIdentifier { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MLApertureProjectAlbumTypeIdentifier")]
		NSString ApertureProjectAlbumTypeIdentifier { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MLApertureFolderAlbumTypeIdentifier")]
		NSString ApertureFolderAlbumTypeIdentifier { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MLApertureProjectFolderAlbumTypeIdentifier")]
		NSString ApertureProjectFolderAlbumTypeIdentifier { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MLApertureLightTableTypeIdentifier")]
		NSString ApertureLightTableTypeIdentifier { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MLApertureFlickrGroupTypeIdentifier")]
		NSString ApertureFlickrGroupTypeIdentifier { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MLApertureFlickrAlbumTypeIdentifier")]
		NSString ApertureFlickrAlbumTypeIdentifier { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MLApertureFacebookGroupTypeIdentifier")]
		NSString ApertureFacebookGroupTypeIdentifier { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MLApertureFacebookAlbumTypeIdentifier")]
		NSString ApertureFacebookAlbumTypeIdentifier { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MLApertureSmugMugGroupTypeIdentifier")]
		NSString ApertureSmugMugGroupTypeIdentifier { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MLApertureSmugMugAlbumTypeIdentifier")]
		NSString ApertureSmugMugAlbumTypeIdentifier { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MLApertureSlideShowTypeIdentifier")]
		NSString ApertureSlideShowTypeIdentifier { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MLApertureAllPhotosTypeIdentifier")]
		NSString ApertureAllPhotosTypeIdentifier { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MLApertureFlaggedTypeIdentifier")]
		NSString ApertureFlaggedTypeIdentifier { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MLApertureAllProjectsTypeIdentifier")]
		NSString ApertureAllProjectsTypeIdentifier { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MLApertureFacesAlbumTypeIdentifier")]
		NSString ApertureFacesAlbumTypeIdentifier { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MLAperturePlacesAlbumTypeIdentifier")]
		NSString AperturePlacesAlbumTypeIdentifier { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MLAperturePlacesCountryAlbumTypeIdentifier")]
		NSString AperturePlacesCountryAlbumTypeIdentifier { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MLAperturePlacesProvinceAlbumTypeIdentifier")]
		NSString AperturePlacesProvinceAlbumTypeIdentifier { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MLAperturePlacesCityAlbumTypeIdentifier")]
		NSString AperturePlacesCityAlbumTypeIdentifier { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MLAperturePlacesPointOfInterestAlbumTypeIdentifier")]
		NSString AperturePlacesPointOfInterestAlbumTypeIdentifier { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MLApertureLastImportAlbumTypeIdentifier")]
		NSString ApertureLastImportAlbumTypeIdentifier { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MLApertureLastNMonthsAlbumTypeIdentifier")]
		NSString ApertureLastNMonthsAlbumTypeIdentifier { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MLApertureLastViewedEventAlbumTypeIdentifier")]
		NSString ApertureLastViewedEventAlbumTypeIdentifier { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MLAperturePhotoStreamAlbumTypeIdentifier")]
		NSString AperturePhotoStreamAlbumTypeIdentifier { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MLGarageBandRootGroupTypeIdentifier")]
		NSString GarageBandRootGroupTypeIdentifier { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MLGarageBandFolderGroupTypeIdentifier")]
		NSString GarageBandFolderGroupTypeIdentifier { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MLLogicRootGroupTypeIdentifier")]
		NSString LogicRootGroupTypeIdentifier { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MLLogicBouncesGroupTypeIdentifier")]
		NSString LogicBouncesGroupTypeIdentifier { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MLLogicProjectsGroupTypeIdentifier")]
		NSString LogicProjectsGroupTypeIdentifier { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MLLogicProjectTypeIdentifier")]
		NSString LogicProjectTypeIdentifier { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MLiMovieRootGroupTypeIdentifier")]
		NSString IMovieRootGroupTypeIdentifier { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MLiMovieEventGroupTypeIdentifier")]
		NSString IMovieEventGroupTypeIdentifier { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MLiMovieProjectGroupTypeIdentifier")]
		NSString IMovieProjectGroupTypeIdentifier { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MLiMovieEventLibraryGroupTypeIdentifier")]
		NSString IMovieEventLibraryGroupTypeIdentifier { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MLiMovieEventCalendarGroupTypeIdentifier")]
		NSString IMovieEventCalendarGroupTypeIdentifier { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MLiMovieFolderGroupTypeIdentifier")]
		NSString IMovieFolderGroupTypeIdentifier { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MLFinalCutRootGroupTypeIdentifier")]
		NSString FinalCutRootGroupTypeIdentifier { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MLFinalCutEventGroupTypeIdentifier")]
		NSString FinalCutEventGroupTypeIdentifier { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MLFinalCutProjectGroupTypeIdentifier")]
		NSString FinalCutProjectGroupTypeIdentifier { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MLFinalCutEventLibraryGroupTypeIdentifier")]
		NSString FinalCutEventLibraryGroupTypeIdentifier { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MLFinalCutEventCalendarGroupTypeIdentifier")]
		NSString FinalCutEventCalendarGroupTypeIdentifier { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MLFinalCutFolderGroupTypeIdentifier")]
		NSString FinalCutFolderGroupTypeIdentifier { get; }
	}

	[Deprecated (PlatformName.MacOSX, 10, 15)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface MLMediaLibrary {
		[Export ("initWithOptions:")]
		[DesignatedInitializer]
		NativeHandle Constructor (NSDictionary<NSString, NSObject> options);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("mediaSources", ArgumentSemantic.Copy)]
		NSDictionary<NSString, MLMediaSource> MediaSources { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MLMediaLoadSourceTypesKey")]
		NSString MediaLoadSourceTypesKey { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MLMediaLoadIncludeSourcesKey")]
		NSString MediaLoadIncludeSourcesKey { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MLMediaLoadExcludeSourcesKey")]
		NSString MediaLoadExcludeSourcesKey { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MLMediaLoadFoldersKey")]
		NSString MediaLoadFoldersKey { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MLMediaLoadAppleLoops")]
		NSString MediaLoadAppleLoops { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MLMediaLoadMoviesFolder")]
		NSString MediaLoadMoviesFolder { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MLMediaLoadAppFoldersKey")]
		NSString MediaLoadAppFoldersKey { get; }
	}

	[Deprecated (PlatformName.MacOSX, 10, 15)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface MLMediaSource {
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("mediaLibrary", ArgumentSemantic.Assign)]
		MLMediaLibrary MediaLibrary { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("mediaSourceIdentifier")]
		NSString MediaSourceIdentifier { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("attributes", ArgumentSemantic.Copy)]
		NSDictionary<NSString, NSObject> Attributes { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("rootMediaGroup", ArgumentSemantic.Retain)]
		MLMediaGroup RootMediaGroup { get; }

		/// <param name="mediaGroupIdentifier">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("mediaGroupForIdentifier:")]
		[return: NullAllowed]
		MLMediaGroup MediaGroupForIdentifier (NSString mediaGroupIdentifier);

		/// <param name="mediaGroupIdentifiers">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("mediaGroupsForIdentifiers:")]
		NSDictionary<NSString, MLMediaGroup> MediaGroupsForIdentifiers (NSString [] mediaGroupIdentifiers);

		/// <param name="mediaObjectIdentifier">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("mediaObjectForIdentifier:")]
		[return: NullAllowed]
		MLMediaObject MediaObjectForIdentifier (NSString mediaObjectIdentifier);

		/// <param name="mediaObjectIdentifiers">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("mediaObjectsForIdentifiers:")]
		NSDictionary<NSString, MLMediaObject> MediaObjectsForIdentifiers (NSString [] mediaObjectIdentifiers);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MLMediaSourcePhotosIdentifier")]
		NSString MediaSourcePhotosIdentifier { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MLMediaSourceiPhotoIdentifier")]
		NSString MediaSourceiPhotoIdentifier { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MLMediaSourceiTunesIdentifier")]
		NSString MediaSourceiTunesIdentifier { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MLMediaSourceApertureIdentifier")]
		NSString MediaSourceApertureIdentifier { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MLMediaSourceiMovieIdentifier")]
		NSString MediaSourceiMovieIdentifier { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MLMediaSourceFinalCutIdentifier")]
		NSString MediaSourceFinalCutIdentifier { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MLMediaSourceGarageBandIdentifier")]
		NSString MediaSourceGarageBandIdentifier { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MLMediaSourceLogicIdentifier")]
		NSString MediaSourceLogicIdentifier { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MLMediaSourcePhotoBoothIdentifier")]
		NSString MediaSourcePhotoBoothIdentifier { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MLMediaSourceCustomFoldersIdentifier")]
		NSString MediaSourceCustomFoldersIdentifier { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MLMediaSourceMoviesFolderIdentifier")]
		NSString MediaSourceMoviesFolderIdentifier { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MLMediaSourceAppDefinedFoldersIdentifier")]
		NSString MediaSourceAppDefinedFoldersIdentifier { get; }
	}

	[Deprecated (PlatformName.MacOSX, 10, 15)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface MLMediaGroup {
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("mediaLibrary", ArgumentSemantic.Assign)]
		MLMediaLibrary MediaLibrary { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("parent", ArgumentSemantic.Assign)]
		MLMediaGroup Parent { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("mediaSourceIdentifier")]
		NSString MediaSourceIdentifier { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("name")]
		string Name { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("identifier")]
		NSString Identifier { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("typeIdentifier")]
		NSString TypeIdentifier { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("attributes", ArgumentSemantic.Copy)]
		NSDictionary<NSString, NSObject> Attributes { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("childGroups", ArgumentSemantic.Copy)]
		MLMediaGroup [] ChildGroups { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("URL", ArgumentSemantic.Copy)]
		NSUrl Url { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("modificationDate", ArgumentSemantic.Copy)]
		NSDate ModificationDate { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("iconImage", ArgumentSemantic.Copy)]
		NSImage IconImage { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("mediaObjects", ArgumentSemantic.Copy)]
		MLMediaObject [] MediaObjects { get; }
	}

	[Deprecated (PlatformName.MacOSX, 10, 15)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface MLMediaObject {
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("mediaLibrary", ArgumentSemantic.Assign)]
		MLMediaLibrary MediaLibrary { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("identifier")]
		NSString Identifier { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("mediaSourceIdentifier")]
		NSString MediaSourceIdentifier { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("attributes", ArgumentSemantic.Copy)]
		NSDictionary<NSString, NSObject> Attributes { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("mediaType", ArgumentSemantic.Assign)]
		MLMediaType MediaType { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("contentType")]
		string ContentType { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("name")]
		string Name { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("URL", ArgumentSemantic.Copy)]
		NSUrl Url { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("originalURL", ArgumentSemantic.Copy)]
		NSUrl OriginalUrl { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("fileSize")]
		nuint FileSize { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("modificationDate", ArgumentSemantic.Copy)]
		NSDate ModificationDate { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("thumbnailURL", ArgumentSemantic.Copy)]
		NSUrl ThumbnailUrl { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("artworkImage", ArgumentSemantic.Copy)]
		NSImage ArtworkImage { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MLMediaObjectDurationKey")]
		NSString MediaObjectDurationKey { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MLMediaObjectArtistKey")]
		NSString MediaObjectArtistKey { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MLMediaObjectAlbumKey")]
		NSString MediaObjectAlbumKey { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MLMediaObjectGenreKey")]
		NSString MediaObjectGenreKey { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MLMediaObjectKindKey")]
		NSString MediaObjectKindKey { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MLMediaObjectTrackNumberKey")]
		NSString MediaObjectTrackNumberKey { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MLMediaObjectBitRateKey")]
		NSString MediaObjectBitRateKey { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MLMediaObjectSampleRateKey")]
		NSString MediaObjectSampleRateKey { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MLMediaObjectChannelCountKey")]
		NSString MediaObjectChannelCountKey { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MLMediaObjectResolutionStringKey")]
		NSString MediaObjectResolutionStringKey { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MLMediaObjectCommentsKey")]
		NSString MediaObjectCommentsKey { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MLMediaObjectKeywordsKey")]
		NSString MediaObjectKeywordsKey { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("MLMediaObjectProtectedKey")]
		NSString MediaObjectProtectedKey { get; }
	}
}
