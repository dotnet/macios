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

using ObjCRuntime;
using Foundation;
using CoreGraphics;
using Photos;
using System;
using System.Drawing;

// Disable until we get around to enable + fix any issues.
#nullable disable

namespace UIKit {
	public partial class UIImagePickerController {
		//
		// The following construct emulates the support for:
		// id<UINavigationControllerDelegate, UIImagePickerControllerDelegate>
		//
		// That is, the type can contain either one, but we still want it strongly typed
		//
		/// <summary>The delegate object that can be used to respond to events relating to this UIImagePickerController.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public IUIImagePickerControllerDelegate ImagePickerControllerDelegate {
			get {
				return Delegate as IUIImagePickerControllerDelegate;
			}
			set {
				Delegate = (NSObject) value;
			}
		}

		/// <summary>A delegate object that can be used to respond to navigation events.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public IUINavigationControllerDelegate NavigationControllerDelegate {
			get {
				return Delegate as IUINavigationControllerDelegate;
			}
			set {
				Delegate = (NSObject) value;
			}
		}
	}

	/// <summary>Provides data for the <see cref="UIKit.UIImagePickerController.FinishedPickingMedia" /> event.</summary>
	///     <remarks>These arguments are available if you use the <see cref="UIKit.UIImagePickerController.FinishedPickingMedia" /> event in <see cref="UIKit.UIImagePickerController" />.</remarks>
	public partial class UIImagePickerMediaPickedEventArgs {
		/// <summary>Indicates the media type.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public string MediaType {
			get {
				return ((NSString) Info [UIImagePickerController.MediaType]).ToString ();
			}
		}

		/// <summary>The original image prior to editing by the user.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public UIImage OriginalImage {
			get {
				return (UIImage) Info [UIImagePickerController.OriginalImage];
			}
		}

		/// <summary>The image edited by the user.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public UIImage EditedImage {
			get {
				return (UIImage) Info [UIImagePickerController.EditedImage];
			}
		}

		/// <summary>The cropping rectangle that was applied to the original image.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public CGRect? CropRect {
			get {
				var nsv = ((NSValue) Info [UIImagePickerController.CropRect]);
				if (nsv is null)
					return null;
				return nsv.CGRectValue;
			}
		}

		/// <summary>The filesystem URL for a movie.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public NSUrl MediaUrl {
			get {
				return (NSUrl) Info [UIImagePickerController.MediaURL];
			}
		}

		/// <summary>Gets the <see cref="Photos.PHLivePhoto" /> of the photo.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		public PHLivePhoto LivePhoto {
			get {
				return (PHLivePhoto) Info [UIImagePickerController.LivePhoto];
			}
		}

		/// <summary>For newly-captured photos only, retrieves the metadata of the photo.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public NSDictionary MediaMetadata {
			get {
				return (NSDictionary) Info [UIImagePickerController.MediaMetadata];
			}
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[ObsoletedOSPlatform ("ios", "Use 'UIImagePickerController.PHAsset' instead.")]
		[ObsoletedOSPlatform ("maccatalyst", "Use 'UIImagePickerController.PHAsset' instead.")]
		public NSUrl ReferenceUrl {
			get {
				return (NSUrl) Info [UIImagePickerController.ReferenceUrl];
			}
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		public PHAsset PHAsset {
			get {
				return (PHAsset) Info [UIImagePickerController.PHAsset];
			}
		}

		/// <summary>Gets the <see cref="Foundation.NSUrl" /> of the image file.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		public NSUrl ImageUrl {
			get {
				return (NSUrl) Info [UIImagePickerController.ImageUrl];
			}
		}
	}
}

#endif // !TVOS
