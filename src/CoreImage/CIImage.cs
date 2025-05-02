//
// CIImage.cs: Extensions
//
// Copyright 2011-2015 Xamarin Inc.
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
using CoreFoundation;
using CoreGraphics;
using ObjCRuntime;
#if !MONOMAC
using UIKit;
using CoreVideo;
#endif

#nullable enable

namespace CoreImage {
	/// <summary>When passed to <see cref="CoreImage.CIImage.GetAutoAdjustmentFilters(CoreImage.CIAutoAdjustmentFilterOptions)" />, limits the results.</summary>
	///     <remarks>
	///       <para>The sample below shows a typical use.</para>
	///       <example>
	///         <code lang="csharp lang-csharp"><![CDATA[
	/// void PrepareFixes (CIImage img)
	/// {
	///     var opt = new CIAutoAdjustmentFilterOptions () {
	///     	RedEye = true,
	///     	AutoAdjustCrop = true
	///     };
	///     CIImage img = null;
	///     foreach (var filter in img.GetAutoAdjustmentFilters (opt)) {
	///     	filter.Image = img;
	///     	img = filter.OutputImage;
	///     }
	/// }
	/// ]]></code>
	///       </example>
	///     </remarks>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	public class CIAutoAdjustmentFilterOptions {

		// The default value is true.
		/// <summary>Set to false if you want to prevent filters that perform enhancements to be returned.</summary>
		///         <remarks>
		///
		/// 	  By default an invocation to <see cref="CoreImage.CIImage.GetAutoAdjustmentFilters(CoreImage.CIAutoAdjustmentFilterOptions)" /> will return filters that include enhancement filters.  To
		/// 	  prevent this, set this property to false.
		///
		/// 	</remarks>
		public bool? Enhance;

		// The default value is true
		/// <summary>Set to false if you want to prevent the filter that performs red-eye removal from being returned.</summary>
		///         <remarks>
		/// 	  By default an invocation to <see cref="CoreImage.CIImage.GetAutoAdjustmentFilters(CoreImage.CIAutoAdjustmentFilterOptions)" /> will return filters that include red-eye removal.  To
		/// 	  prevent this, set this property to false.
		/// 	</remarks>
		public bool? RedEye;

		/// <summary>Provides a set of features to use during Red-Eye filters.</summary>
		///         <remarks>
		/// 	  If this array is empty, CoreImage will use the CIDetector class to detect where the face is on the image and use the result.
		/// 	</remarks>
		public CIFeature []? Features;

		/// <summary>Provides image orientation information to the auto-enhance system.</summary>
		///         <remarks>Some adjustments depend on image orientation to work, providing this hint helps the auto correction software pick the best results.</remarks>
		public CIImageOrientation? ImageOrientation;

		/// <summary>Whether or not to automatically crop the image.</summary>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
		public bool? AutoAdjustCrop;

		/// <summary>Gets or sets the automatic adjustment level.</summary>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
		public bool? AutoAdjustLevel;

		internal NSDictionary? ToDictionary ()
		{
			int n = 0;
			if (Enhance.HasValue && Enhance.Value == false)
				n++;
			if (RedEye.HasValue && RedEye.Value == false)
				n++;
			if (ImageOrientation.HasValue)
				n++;
			if (Features is not null && Features.Length != 0)
				n++;
			if (AutoAdjustCrop.HasValue && AutoAdjustCrop.Value == true)
				n++;
			if (AutoAdjustLevel.HasValue && AutoAdjustLevel.Value == true)
				n++;
			if (n == 0)
				return null;

			NSMutableDictionary dict = new NSMutableDictionary ();

			if (Enhance.HasValue && Enhance.Value == false) {
				dict.LowlevelSetObject (CFBoolean.FalseHandle, CIImage.AutoAdjustEnhanceKey.Handle);
			}
			if (RedEye.HasValue && RedEye.Value == false) {
				dict.LowlevelSetObject (CFBoolean.FalseHandle, CIImage.AutoAdjustRedEyeKey.Handle);
			}
			if (Features is not null && Features.Length != 0) {
				dict.LowlevelSetObject (NSArray.FromObjects (Features), CIImage.AutoAdjustFeaturesKey.Handle);
			}
			if (ImageOrientation.HasValue) {
				dict.LowlevelSetObject (new NSNumber ((int) ImageOrientation.Value), global::ImageIO.CGImageProperties.Orientation.Handle);
			}
			if (AutoAdjustCrop.HasValue && AutoAdjustCrop.Value == true) {
				dict.LowlevelSetObject (CFBoolean.TrueHandle, CIImage.AutoAdjustCrop.Handle);
			}
			if (AutoAdjustLevel.HasValue && AutoAdjustLevel.Value == true) {
				dict.LowlevelSetObject (CFBoolean.TrueHandle, CIImage.AutoAdjustLevel.Handle);
			}

#if false
			for (i = 0; i < n; i++){
				Console.WriteLine ("{0} {1}-{2}", i, keys [i], values [i]);
			}
#endif
			return dict;
		}
	}

	public partial class CIImage {

		static CIFilter [] WrapFilters (NSArray filters)
		{
			if (filters is null)
				return new CIFilter [0];

			nuint count = filters.Count;
			if (count == 0)
				return new CIFilter [0];
			var ret = new CIFilter [count];
			for (nuint i = 0; i < count; i++) {
				var filterHandle = filters.ValueAt (i);
				string? filterName = CIFilter.GetFilterName (filterHandle);

				ret [i] = CIFilter.FromName (filterName, filterHandle);
			}
			return ret;
		}

		/// <param name="image">CoreGraphics image.</param>
		///         <param name="colorSpace">Colorspace to use.</param>
		///         <summary>Creates a <see cref="CoreImage.CIImage" /> in <paramref name="colorSpace" /> from a <see cref="CoreGraphics.CGImage" />.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static CIImage FromCGImage (CGImage image, CGColorSpace colorSpace)
		{
			if (colorSpace is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (colorSpace));

			CIImage result;
			using (var arr = NSArray.FromIntPtrs (new [] { colorSpace.Handle })) {
				using (var keys = NSArray.FromIntPtrs (new [] { CIImageInitializationOptionsKeys.ColorSpaceKey.Handle })) {
					using (var dict = NSDictionary.FromObjectsAndKeysInternal (arr, keys)) {
						result = FromCGImage (image, dict);
					}
				}
			}
			GC.KeepAlive (colorSpace);
			return result;
		}

		// Apple removed this API in iOS9 SDK
		/// <summary>Gets the filters that are required to perform some common image correction steps to an image.</summary>
		///         <returns>Returns an array of configured filters to apply to the image to automatically adjust it.</returns>
		///         <remarks>
		///           <para>
		/// 	    In general, you should try to use the <see cref="CoreImage.CIImage.GetAutoAdjustmentFilters(CoreImage.CIAutoAdjustmentFilterOptions)" />
		/// 	    as that method allows you to customize which kind of filters you want to get.
		///
		/// 	  </para>
		///           <para>
		/// 	    This method is used to get a list of pre-configured
		/// 	    filters to remedy various common problems found in photos.   
		///
		/// 	  </para>
		///           <example>
		///             <code lang="csharp lang-csharp"><![CDATA[
		/// void PrepareFixes (CIImage img)
		/// {
		///     foreach (var filter in img.GetAutoAdjustmentFilters ()) {
		///     	filter.Image = img;
		///     	img = filter.OutputImage;
		///     }
		/// }
		/// ]]></code>
		///           </example>
		///         </remarks>
		public CIFilter [] GetAutoAdjustmentFilters ()
		{
			return GetAutoAdjustmentFilters (null);
		}

		/// <param name="options">Options to initialize the image with.</param>
		///         <summary>Gets the filters requires to perform some common image correction steps to an image.</summary>
		///         <returns>Returns an array of configured filters to apply to the image to automatically adjust it.</returns>
		///         <remarks>
		///           <para>
		/// 	    This method is used to get a list of pre-configured
		/// 	    filters to remedy various common problems found in photos.   
		///
		/// 	  </para>
		///           <example>
		///             <code lang="csharp lang-csharp"><![CDATA[
		/// void PrepareFixes (CIImage img)
		/// {
		///     var opt = new CIAutoAdjustmentFilterOptions () {
		///     	RedEye = true,
		///     	AutoAdjustCrop = true
		///     };
		///     CIImage img = null;
		///     foreach (var filter in img.GetAutoAdjustmentFilters (opt)) {
		///     	filter.Image = img;
		///     	img = filter.OutputImage;
		///     }
		/// }
		/// 	    ]]></code>
		///           </example>
		///         </remarks>
		public CIFilter [] GetAutoAdjustmentFilters (CIAutoAdjustmentFilterOptions? options)
		{
			var dict = options?.ToDictionary ();
			return WrapFilters (_GetAutoAdjustmentFilters (dict));
		}

		/// <param name="image">CoreGraphics image</param>
		/// <summary>Implicit constructor that wraps a CGImage as a CIImage.</summary>
		/// <returns>
		///         </returns>
		/// <remarks>
		///         </remarks>
		public static implicit operator CIImage (CGImage image)
		{
			return FromCGImage (image);
		}

		internal static int CIFormatToInt (CIFormat format)
		{
			switch (format) {
			case CIFormat.ARGB8: return FormatARGB8;
			case CIFormat.RGBAh: return FormatRGBAh;
#if MONOMAC
			case CIFormat.RGBA16: return FormatRGBA16;
			case CIFormat.RGBAf: return FormatRGBAf;
#endif
			case CIFormat.kRGBAf: return FormatRGBAf;
			case CIFormat.kBGRA8: return FormatBGRA8;
			case CIFormat.kRGBA8: return FormatRGBA8;
			case CIFormat.ABGR8: return FormatABGR8;
			case CIFormat.A8: return FormatA8;
			case CIFormat.A16: return FormatA16;
			case CIFormat.Ah: return FormatAh;
			case CIFormat.Af: return FormatAf;
			case CIFormat.R8: return FormatR8;
			case CIFormat.R16: return FormatR16;
			case CIFormat.Rh: return FormatRh;
			case CIFormat.Rf: return FormatRf;
			case CIFormat.RG8: return FormatRG8;
			case CIFormat.RG16: return FormatRG16;
			case CIFormat.RGh: return FormatRGh;
			case CIFormat.RGf: return FormatRGf;
			default:
				throw new ArgumentOutOfRangeException ("format");
			}
		}

		/// <param name="bitmapData">To be added.</param>
		/// <param name="bytesPerRow">To be added.</param>
		/// <param name="size">To be added.</param>
		/// <param name="pixelFormat">To be added.</param>
		/// <param name="colorSpace">To be added.</param>
		/// <summary>To be added.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		public static CIImage FromData (NSData bitmapData, nint bytesPerRow, CGSize size, CIFormat pixelFormat, CGColorSpace colorSpace)
		{
			return FromData (bitmapData, bytesPerRow, size, CIImage.CIFormatToInt (pixelFormat), colorSpace);
		}

		/// <param name="provider">To be added.</param>
		/// <param name="width">To be added.</param>
		/// <param name="height">To be added.</param>
		/// <param name="pixelFormat">To be added.</param>
		/// <param name="colorSpace">To be added.</param>
		/// <param name="options">To be added.</param>
		/// <summary>To be added.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		public static CIImage FromProvider (ICIImageProvider provider, nuint width, nuint height, CIFormat pixelFormat, CGColorSpace colorSpace, CIImageProviderOptions options)
		{
			return FromProvider (provider, width, height, CIImage.CIFormatToInt (pixelFormat), colorSpace, options?.Dictionary);
		}

		/// <param name="provider">To be added.</param>
		/// <param name="width">To be added.</param>
		/// <param name="height">To be added.</param>
		/// <param name="pixelFormat">To be added.</param>
		/// <param name="colorSpace">To be added.</param>
		/// <param name="options">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		public CIImage (ICIImageProvider provider, nuint width, nuint height, CIFormat pixelFormat, CGColorSpace colorSpace, CIImageProviderOptions options)
			: this (provider, width, height, CIImage.CIFormatToInt (pixelFormat), colorSpace, options?.Dictionary)
		{
		}
	}
}
