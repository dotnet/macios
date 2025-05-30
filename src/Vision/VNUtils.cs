//
// VNUtils.cs
//
// Authors:
//	Alex Soto  <alexsoto@microsoft.com>
//
// Copyright 2017 Xamarin Inc. All rights reserved.
//

#nullable enable

using System;
using System.Numerics;
using System.Runtime.InteropServices;
using CoreGraphics;
using ObjCRuntime;
using Foundation;

namespace Vision {
	/// <summary>A set of utility functions for working with images.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("tvos")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	public static partial class VNUtils {

		// initialized only once (see tests/cecil-tests/)
		/// <summary>Gets the normalized identity <see cref="CoreGraphics.CGRect" /> [0, 0, 1, 1].</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("VNNormalizedIdentityRect", Constants.VisionLibrary)]
		public static CGRect NormalizedIdentityRect { get; } = Dlfcn.GetCGRect (Libraries.Vision.Handle, "VNNormalizedIdentityRect");

		[DllImport (Constants.VisionLibrary, EntryPoint = "VNNormalizedRectIsIdentityRect")]
		static extern byte _IsIdentityRect (CGRect rect);

		/// <param name="rect">To be added.</param>
		///         <summary>Returns <see langword="true" /> if the <paramref name="rect" /> is [0, 0, 1, 1].</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static bool IsIdentityRect (CGRect rect)
		{
			return _IsIdentityRect (rect) != 0;
		}

		[DllImport (Constants.VisionLibrary, EntryPoint = "VNImagePointForNormalizedPoint")]
		public static extern CGPoint GetImagePoint (CGPoint normalizedPoint, nuint imageWidth, nuint imageHeight);

		[SupportedOSPlatform ("tvos14.0")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios14.0")]
		[SupportedOSPlatform ("maccatalyst")]
		[DllImport (Constants.VisionLibrary, EntryPoint = "VNNormalizedPointForImagePoint")]
		public static extern CGPoint GetNormalizedPoint (CGPoint imagePoint, nuint imageWidth, nuint imageHeight);

		[DllImport (Constants.VisionLibrary, EntryPoint = "VNImageRectForNormalizedRect")]
		public static extern CGRect GetImageRect (CGRect normalizedRect, nuint imageWidth, nuint imageHeight);

		[DllImport (Constants.VisionLibrary, EntryPoint = "VNNormalizedRectForImageRect")]
		public static extern CGRect GetNormalizedRect (CGRect imageRect, nuint imageWidth, nuint imageHeight);

		[SupportedOSPlatform ("tvos15.0")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios15.0")]
		[SupportedOSPlatform ("maccatalyst")]
		[DllImport (Constants.VisionLibrary, EntryPoint = "VNImagePointForNormalizedPointUsingRegionOfInterest")]
		public static extern CGPoint GetImagePoint (CGPoint normalizedPoint, nuint imageWidth, nuint imageHeight, CGRect regionOfInterest);

		[SupportedOSPlatform ("tvos15.0")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios15.0")]
		[SupportedOSPlatform ("maccatalyst")]
		[DllImport (Constants.VisionLibrary, EntryPoint = "VNNormalizedPointForImagePointUsingRegionOfInterest")]
		public static extern CGPoint GetNormalizedPoint (CGPoint imagePoint, nuint imageWidth, nuint imageHeight, CGRect regionOfInterest);

		[SupportedOSPlatform ("tvos15.0")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios15.0")]
		[SupportedOSPlatform ("maccatalyst")]
		[DllImport (Constants.VisionLibrary, EntryPoint = "VNImageRectForNormalizedRectUsingRegionOfInterest")]
		public static extern CGRect GetImageRect (CGRect normalizedRect, nuint imageWidth, nuint imageHeight, CGRect regionOfInterest);

		[SupportedOSPlatform ("tvos15.0")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios15.0")]
		[SupportedOSPlatform ("maccatalyst")]
		[DllImport (Constants.VisionLibrary, EntryPoint = "VNNormalizedRectForImageRectUsingRegionOfInterest")]
		public static extern CGRect GetNormalizedRect (CGRect imageRect, nuint imageWidth, nuint imageHeight, CGRect regionOfInterest);

		[DllImport ("__Internal", EntryPoint = "xamarin_CGPoint__VNNormalizedFaceBoundingBoxPointForLandmarkPoint_Vector2_CGRect_nuint_nuint_string")]
		unsafe static extern CGPoint VNNormalizedFaceBoundingBoxPointForLandmarkPoint (Vector2 faceLandmarkPoint, CGRect faceBoundingBox, nuint imageWidth, nuint imageHeight, IntPtr* error);

		public static CGPoint GetNormalizedFaceBoundingBoxPoint (Vector2 faceLandmarkPoint, CGRect faceBoundingBox, nuint imageWidth, nuint imageHeight)
		{
			IntPtr error;
			CGPoint result;
			unsafe {
				result = VNNormalizedFaceBoundingBoxPointForLandmarkPoint (faceLandmarkPoint, faceBoundingBox, imageWidth, imageHeight, &error);
			}
			if (error != IntPtr.Zero)
				throw new InvalidOperationException (Marshal.PtrToStringAuto (error));

			return result;
		}

		[DllImport ("__Internal", EntryPoint = "xamarin_CGPoint__VNImagePointForFaceLandmarkPoint_Vector2_CGRect_nuint_nuint_string")]
		unsafe static extern CGPoint VNImagePointForFaceLandmarkPoint (Vector2 faceLandmarkPoint, CGRect faceBoundingBox, nuint imageWidth, nuint imageHeight, IntPtr* error);

		public static CGPoint GetImagePoint (Vector2 faceLandmarkPoint, CGRect faceBoundingBox, nuint imageWidth, nuint imageHeight)
		{
			IntPtr error;
			CGPoint result;
			unsafe {
				result = VNImagePointForFaceLandmarkPoint (faceLandmarkPoint, faceBoundingBox, imageWidth, imageHeight, &error);
			}
			if (error != IntPtr.Zero)
				throw new InvalidOperationException (Marshal.PtrToStringAuto (error));

			return result;
		}

		[SupportedOSPlatform ("tvos13.0")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios13.0")]
		[SupportedOSPlatform ("maccatalyst")]
		[DllImport (Constants.VisionLibrary)]
		static extern nuint VNElementTypeSize (nuint elementType);

		[SupportedOSPlatform ("tvos13.0")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios13.0")]
		[SupportedOSPlatform ("maccatalyst")]
		public static nuint GetElementTypeSize (VNElementType elementType) => VNElementTypeSize ((nuint) (ulong) elementType);
	}

	public partial class VNGeometryUtils {

		public static VNCircle? CreateBoundingCircle (Vector2 [] points, out NSError error)
		{
			if (points is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (points));
			if (points.Length == 0)
				throw new ArgumentException ($"'{nameof (points)}' array must have more than zero elements.");

			unsafe {
				fixed (Vector2* points_ptr = points)
					return CreateBoundingCircle ((IntPtr) points_ptr, points.Length, out error);
			}
		}
	}
}
