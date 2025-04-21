//
// Authors:
//   Sebastien Pouliot  <sebastien@xamarin.com>
//
// Copyright 2013, 2016 Xamarin, Inc.
//

using System;
using System.Collections.Generic;
using Foundation;
using ObjCRuntime;

#nullable enable

namespace CoreImage {

	/// <summary>Options for use with face detection. Used with <see cref="M:CoreImage.CIDetector.CreateFaceDetector(CoreImage.CIContext,System.Nullable{CoreImage.FaceDetectorAccuracy},System.Nullable{System.Single},System.Nullable{System.Boolean})" />.</summary>
	///     <remarks>To be added.</remarks>
	public partial class CIDetectorOptions {

		/// <summary>Gets or sets a value that indicates whether to use high or low detection accuracy.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public FaceDetectorAccuracy? Accuracy { get; set; }
		/// <summary>Gets or sets the smallest feature size to detect.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public float? MinFeatureSize { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		public int? MaxFeatureCount { get; set; }

		/// <summary>Gets or sets whether faces are tracked between frames.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public bool? TrackingEnabled { get; set; }
		/// <summary>Gets whether to detect closed eyes.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public bool? EyeBlink { get; set; }
		/// <summary>Gets or sets whether to detect smiles.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public bool? Smile { get; set; }
		/// <summary>Gets or sets the aspect ratio of features to detect. Values greater than one are wider than they are tall.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public float? AspectRatio { get; set; }
		/// <summary>Gets or sets a value that indicates the focal length of the device that was used to capture the image that the detector will process.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public float? FocalLength { get; set; }

		/// <summary>Gets or sets the number of angles from which to sample video  when detecting faces. Can be 1, 3, 5, 7, 9, or 11.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public float? NumberOfAngles { get; set; }

		/// <summary>Gets or sets a value that controls whether to return subfeature information in addition to feature information.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public bool? ReturnSubFeatures { get; set; }

		/// <summary>The orientation of the source image, or <see langword="null" />.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public CIImageOrientation? ImageOrientation { get; set; }

		internal NSDictionary ToDictionary ()
		{
			// We now have 11 possible keys so begining with 6 *might* be optimal
			List<NSObject> keys = new List<NSObject> (6);
			List<NSObject> values = new List<NSObject> (6);

			if (CIDetector.Accuracy is not null) {
				keys.Add (CIDetector.Accuracy);
				values.Add (Accuracy == FaceDetectorAccuracy.High ? CIDetector.AccuracyHigh : CIDetector.AccuracyLow);
			}

			// MinFeatureSize exists only in iOS6+, before this the field is null (and would throw if used)
			if (CIDetector.MinFeatureSize is not null && MinFeatureSize is not null) {
				keys.Add (CIDetector.MinFeatureSize);
				values.Add (new NSNumber (MinFeatureSize.Value));
			}

			// Tracking exists only in iOS6+, before this the field is null (and would throw if used)
			if (CIDetector.Tracking is not null && TrackingEnabled is not null) {
				keys.Add (CIDetector.Tracking);
				values.Add (NSObject.FromObject (TrackingEnabled.Value));
			}

			// EyeBlink exists only in iOS7+, before this the field is null (and would throw if used)
			if (CIDetector.EyeBlink is not null && EyeBlink is not null) {
				keys.Add (CIDetector.EyeBlink);
				values.Add (NSObject.FromObject (EyeBlink.Value));
			}

			// Smile exists only in iOS7+, before this the field is null (and would throw if used)
			if (CIDetector.Smile is not null && Smile is not null) {
				keys.Add (CIDetector.Smile);
				values.Add (NSObject.FromObject (Smile.Value));
			}
			// AspectRation exists only in iOS8+, before this the field is null (and would throw if used)
			if (CIDetector.AspectRatio is not null && AspectRatio is not null) {
				keys.Add (CIDetector.AspectRatio);
				values.Add (new NSNumber (AspectRatio.Value));
			}
			// FocalLength exists only in iOS8+, before this the field is null (and would throw if used)
			if (CIDetector.FocalLength is not null && FocalLength is not null) {
				keys.Add (CIDetector.FocalLength);
				values.Add (new NSNumber (FocalLength.Value));
			}
			if (CIDetector.NumberOfAngles is not null && NumberOfAngles is not null) {
				keys.Add (CIDetector.NumberOfAngles);
				values.Add (new NSNumber (NumberOfAngles.Value));
			}

			if (CIDetector.ReturnSubFeatures is not null && ReturnSubFeatures is not null) {
				keys.Add (CIDetector.ReturnSubFeatures);
				values.Add (new NSNumber (ReturnSubFeatures.Value));
			}

			if (CIDetector.ImageOrientation is not null && ImageOrientation is not null) {
				keys.Add (CIDetector.ImageOrientation);
				values.Add (new NSNumber ((int) ImageOrientation.Value));
			}

			if (CIDetector.MaxFeatureCount is not null && MaxFeatureCount is not null) {
				keys.Add (CIDetector.MaxFeatureCount);
				values.Add (new NSNumber ((int) MaxFeatureCount.Value));
			}

			return NSDictionary.FromObjectsAndKeys (values.ToArray (), keys.ToArray ());
		}
	}
}
