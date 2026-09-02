//
// UIImagePickerContrller.cs
//
// Authors:
//   Miguel de Icaza
//
// Copyright 2009, Novell, Inc.
// Copyright 2012-2014 Xamarin Inc
//

#if !TVOS // __TVOS_PROHIBITED

using CoreGraphics;
using Photos;

#nullable enable

namespace UIKit {
	public partial class UIImagePickerController {
		//
		// The following construct emulates the support for:
		// id<UINavigationControllerDelegate, UIImagePickerControllerDelegate>
		//
		// That is, the type can contain either one, but we still want it strongly typed
		//
		/// <summary>Gets or sets the delegate object used to respond to events relating to this <see cref="UIImagePickerController" />.</summary>
		public IUIImagePickerControllerDelegate? ImagePickerControllerDelegate {
			get {
				return Delegate as IUIImagePickerControllerDelegate;
			}
			set {
				Delegate = (NSObject?) value;
			}
		}

		/// <summary>Gets or sets a delegate object used to respond to navigation events.</summary>
		public IUINavigationControllerDelegate? NavigationControllerDelegate {
			get {
				return Delegate as IUINavigationControllerDelegate;
			}
			set {
				Delegate = (NSObject?) value;
			}
		}
	}

	/// <summary>Provides data for the <see cref="UIImagePickerController.FinishedPickingMedia" /> event.</summary>
	/// <remarks>These arguments are available when using the <see cref="UIImagePickerController.FinishedPickingMedia" /> event in <see cref="UIImagePickerController" />.</remarks>
	public partial class UIImagePickerMediaPickedEventArgs {
		/// <summary>Gets the media type of the picked media.</summary>
		public string? MediaType {
			get {
				return ((NSString?) Info [UIImagePickerController.MediaType])?.ToString ();
			}
		}

		/// <summary>Gets the original image prior to editing by the user.</summary>
		public UIImage? OriginalImage {
			get {
				return (UIImage?) Info [UIImagePickerController.OriginalImage];
			}
		}

		/// <summary>Gets the image edited by the user.</summary>
		public UIImage? EditedImage {
			get {
				return (UIImage?) Info [UIImagePickerController.EditedImage];
			}
		}

		/// <summary>Gets the cropping rectangle that was applied to the original image.</summary>
		public CGRect? CropRect {
			get {
				var nsv = (NSValue?) Info [UIImagePickerController.CropRect];
				if (nsv is null)
					return null;
				return nsv.CGRectValue;
			}
		}

		/// <summary>Gets the filesystem URL for a movie.</summary>
		public NSUrl? MediaUrl {
			get {
				return (NSUrl?) Info [UIImagePickerController.MediaURL];
			}
		}

		/// <summary>Gets the <see cref="PHLivePhoto" /> of the photo.</summary>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		public PHLivePhoto? LivePhoto {
			get {
				return (PHLivePhoto?) Info [UIImagePickerController.LivePhoto];
			}
		}

		/// <summary>Gets the metadata for newly-captured photos.</summary>
		public NSDictionary? MediaMetadata {
			get {
				return (NSDictionary?) Info [UIImagePickerController.MediaMetadata];
			}
		}

		/// <summary>Gets the reference URL for the picked asset.</summary>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[ObsoletedOSPlatform ("ios", "Use 'UIImagePickerController.PHAsset' instead.")]
		[ObsoletedOSPlatform ("maccatalyst", "Use 'UIImagePickerController.PHAsset' instead.")]
		public NSUrl? ReferenceUrl {
			get {
				return (NSUrl?) Info [UIImagePickerController.ReferenceUrl];
			}
		}

		/// <summary>Gets the <see cref="Photos.PHAsset" /> associated with the picked media.</summary>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		public PHAsset? PHAsset {
			get {
				return (PHAsset?) Info [UIImagePickerController.PHAsset];
			}
		}

		/// <summary>Gets the <see cref="Foundation.NSUrl" /> of the image file.</summary>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		public NSUrl? ImageUrl {
			get {
				return (NSUrl?) Info [UIImagePickerController.ImageUrl];
			}
		}
	}
}

#endif // !TVOS
