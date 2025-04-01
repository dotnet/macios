//
// Authors:
//   Miguel de Icaza
//   Sebastien Pouliot  <sebastien@xamarin.com>
//
// Copyright 2011-2014, Xamarin, Inc.
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
//

using System;
using Foundation;

#nullable enable

namespace CoreImage {

	// convenience enum for CIDetectorAccuracy[High|Low] internal fields in CIDetector (coreimage.cs)
	public enum FaceDetectorAccuracy {
		/// <summary>Low detection accuracy.</summary>
		High,
		/// <summary>High detection accuracy.</summary>
		Low,
	}

	public partial class CIDetector {
		/// <param name="context">Image context.</param>
		///         <param name="highAccuracy">If true, it uses a more precise but slower scanning method;   If false, it uses a fast path, but not as precise..</param>
		///         <summary>Create a new face detector using the specified parameters.</summary>
		///         <returns>A CIDetector instance.</returns>
		///         <remarks>
		///         </remarks>
		public static CIDetector? CreateFaceDetector (CIContext context, bool highAccuracy)
		{
			// TypeFace is the only detector supported now
			using (var options = NSDictionary.FromObjectsAndKeys (new NSObject [] { highAccuracy ? AccuracyHigh : AccuracyLow },
										  new NSObject [] { Accuracy }))
				return FromType (TypeFace, context, options);
		}

		/// <param name="context">Image context.</param>
		///         <param name="highAccuracy">If true, it uses a more precise but slower scanning method;   If false, it uses a fast path, but not as precise..</param>
		///         <param name="minFeatureSize">Minimum size that the detector will discover as a feature.</param>
		///         <summary>Create a new face detector using the specified parameters.</summary>
		///         <returns>A CIDetector instance.</returns>
		///         <remarks>minFeatureSize is new in iOS 6, it will be ignored in earlier releases.</remarks>
		public static CIDetector? CreateFaceDetector (CIContext context, bool highAccuracy, float minFeatureSize)
		{
			// MinFeatureSize exists only in iOS6+, before this the field is null (and would throw if used)
			if (MinFeatureSize is null)
				return CreateFaceDetector (context, highAccuracy);

			// TypeFace is the only detector supported now
			using (var options = NSDictionary.FromObjectsAndKeys (new NSObject [] { highAccuracy ? AccuracyHigh : AccuracyLow, new NSNumber (minFeatureSize) },
										  new NSObject [] { Accuracy, MinFeatureSize, }))
				return FromType (TypeFace, context, options);
		}

		/// <param name="context">Image context.</param>
		///         <param name="accuracy">To be added.</param>
		///         <param name="minFeatureSize">Minimum size that the detector will discover as a feature.</param>
		///         <param name="trackingEnabled">Enables feature tracking.</param>
		///         <summary>Create a new face detector using the specified parameters.</summary>
		///         <returns>A CIDetector instance.</returns>
		///         <remarks>Both minFeatureSize and trackingEnabled are new in iOS 6, they will be ignored in earlier releases.</remarks>
		public static CIDetector? CreateFaceDetector (CIContext context, FaceDetectorAccuracy? accuracy = null, float? minFeatureSize = null, bool? trackingEnabled = null)
		{
			CIDetectorOptions dopt = new CIDetectorOptions () {
				Accuracy = accuracy,
				MinFeatureSize = minFeatureSize,
				TrackingEnabled = trackingEnabled,
			};

			using (var options = dopt.ToDictionary ())
				return FromType (TypeFace, context, options);
		}

		/// <param name="context">Image context.</param>
		///         <param name="detectorOptions">Options to use for the face detector.</param>
		///         <summary>Create a new face detector using the specified parameters.</summary>
		///         <returns>A CIDetector instance.</returns>
		///         <remarks>
		///         </remarks>
		public static CIDetector? CreateFaceDetector (CIContext context, CIDetectorOptions detectorOptions)
		{
			using (var options = detectorOptions?.ToDictionary ())
				return FromType (TypeFace, context, options);
		}

		/// <param name="context">To be added.</param>
		///         <param name="detectorOptions">To be added.</param>
		///         <summary>Create a detector that recognizes rectangular objects in the image.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static CIDetector? CreateRectangleDetector (CIContext context, CIDetectorOptions detectorOptions)
		{
			using (var options = detectorOptions?.ToDictionary ())
				return FromType (TypeRectangle, context, options);
		}


		/// <param name="context">To be added.</param>
		///         <param name="detectorOptions">To be added.</param>
		///         <summary>Create a detector that recognizes QR codes.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static CIDetector? CreateQRDetector (CIContext context, CIDetectorOptions detectorOptions)
		{
			using (var options = detectorOptions?.ToDictionary ())
				return FromType (TypeQRCode, context, options);
		}

		/// <param name="context">To be added.</param>
		///         <param name="detectorOptions">To be added.</param>
		///         <summary>Creates a new CIDetector with the specified context and detection options.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static CIDetector? CreateTextDetector (CIContext context, CIDetectorOptions detectorOptions)
		{
			using (var options = detectorOptions?.ToDictionary ())
				return FromType (TypeText, context, options);
		}

		/// <param name="image">Image to analyze.</param>
		///         <param name="orientation">Orientation for the image.</param>
		///         <summary>Analyzes the image and returns a list of features discovered in the image (faces, QR codes, rectangles).</summary>
		///         <returns>Array of discovered features.</returns>
		///         <remarks>
		///         </remarks>
		public CIFeature [] FeaturesInImage (CIImage image, CIImageOrientation orientation)
		{
			using (var options = NSDictionary.FromObjectsAndKeys (new NSObject [] { new NSNumber ((int) orientation) },
										  new NSObject [] { ImageOrientation })) {
				return FeaturesInImage (image, options);
			}
		}
	}
}
