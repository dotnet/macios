//
// Authors:
//   Miguel de Icaza
//
// Copyright 2011, 2012, 2015 Xamarin Inc.
// Copyright 2010, Novell, Inc.
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
using CoreGraphics;
using CoreFoundation;
using ObjCRuntime;
using Metal;
#if HAS_OPENGLES
using OpenGLES;
#endif

#nullable enable

namespace CoreImage {
	/// <summary>Use to configure the CIContext rendering pipeline.</summary>
	///     <remarks>You would use an instance of this class to configure the CIContext rendering operations.</remarks>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	public class CIContextOptions : DictionaryContainer {

		/// <summary>Creates an empty set of options for CIContext rendering.</summary>
		///         <remarks>
		///         </remarks>
		public CIContextOptions ()
		{
		}

		/// <param name="dictionary">To be added.</param>
		///         <summary>Constructs a new <see cref="CoreImage.CIContextOptions" /> object using the options specified in <paramref name="dictionary" />.</summary>
		///         <remarks>To be added.</remarks>
		public CIContextOptions (NSDictionary dictionary)
			: base (dictionary)
		{
		}

		/// <summary>The desired CIColorSpace to be used for the CIContext rendering operation.</summary>
		///         <value>
		///         </value>
		///         <remarks>This color space is used before the image is rendered into the output.</remarks>
		public CGColorSpace? OutputColorSpace {
			get {
				return GetNativeValue<CGColorSpace> (CIContext.OutputColorSpace);
			}
			set {
				SetNativeValue (CIContext.OutputColorSpace, value);
			}
		}

		/// <summary>The colorspace used by image processing operations, this is different than the colorspace used for the final rendering.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public CGColorSpace? WorkingColorSpace {
			get {
				return GetNativeValue<CGColorSpace> (CIContext._WorkingColorSpace);
			}
			set {
				SetNativeValue (CIContext._WorkingColorSpace, value);
			}
		}

#if __MACOS__
		/// <include file="../../docs/api/CoreImage/CIContextOptions.xml" path="/Documentation/Docs[@DocId='macOS:P:CoreImage.CIContextOptions.UseSoftwareRenderer']/*" />
#else
		/// <include file="../../docs/api/CoreImage/CIContextOptions.xml" path="/Documentation/Docs[@DocId='P:CoreImage.CIContextOptions.UseSoftwareRenderer']/*" />
#endif
		public bool UseSoftwareRenderer {
			get {
				var b = GetBoolValue (CIContext.UseSoftwareRenderer);
				return b.HasValue ? b.Value : false;
			}
			set {
				SetBooleanValue (CIContext.UseSoftwareRenderer, value);
			}
		}

		/// <summary>Gets or sets the image format to use for storing intermediate rendering results.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public int? CIImageFormat {
			get {
				return GetInt32Value (CIContext.WorkingFormatField);
			}
			set {
				SetNumberValue (CIContext.WorkingFormatField, value);
			}
		}

		/// <summary>Gets or sets whether to request low priority from the GPU.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		public bool? PriorityRequestLow {
			get {
				return GetBoolValue (CIContext.PriorityRequestLow);
			}
			set {
				SetBooleanValue (CIContext.PriorityRequestLow, value);
			}
		}

		/// <summary>
		///           <see langword="true" /> if downsampling should be higher quality at the expense of performance.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public bool? HighQualityDownsample {
			get {
				return GetBoolValue (CIContext.HighQualityDownsample);
			}
			set {
				SetBooleanValue (CIContext.HighQualityDownsample, value);
			}
		}

		/// <summary>If <see langword="true" />, the output should premultiply pixel values by their alpha values.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
		public bool? OutputPremultiplied {
			get {
				return GetBoolValue (CIContext.OutputPremultiplied);
			}
			set {
				SetBooleanValue (CIContext.OutputPremultiplied, value);
			}
		}

		/// <summary>If not <see langword="null" />, <see langword="true" /> indicates that intermediate images should be cached.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		public bool? CacheIntermediates {
			get {
				return GetBoolValue (CIContext.CacheIntermediates);
			}
			set {
				SetBooleanValue (CIContext.CacheIntermediates, value);
			}
		}

		[SupportedOSPlatform ("ios13.0")]
		[SupportedOSPlatform ("tvos13.0")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		public bool? AllowLowPower {
			get {
				return GetBoolValue (CIContext.AllowLowPower);
			}
			set {
				SetBooleanValue (CIContext.AllowLowPower, value);
			}
		}

		[SupportedOSPlatform ("ios14.0")]
		[SupportedOSPlatform ("tvos14.0")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		public string? Name {
			get {
				return GetStringValue (CIContext.Name);
			}
			set {
				SetStringValue (CIContext.Name, value);
			}
		}
	}

	public partial class CIContext {
		/// <param name="options">The context options to use.</param>
		///         <summary>Creates a new Core Image context with the specified <paramref name="options" />.</summary>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
		public CIContext (CIContextOptions options) :
			this (options?.Dictionary)
		{
		}

		/// <param name="ctx">To be added.</param>
		///         <param name="options">To be added.</param>
		///         <summary>Creates a new CIContext from an existing one, along with the provided </summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static CIContext FromContext (CGContext ctx, CIContextOptions? options)
		{
			return FromContext (ctx, options?.Dictionary);
		}

		/// <param name="ctx">To be added.</param>
		///         <summary>Creates a new CIContext from an existing one.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static CIContext FromContext (CGContext ctx)
		{
			return FromContext (ctx, (NSDictionary?) null);
		}

#if HAS_OPENGLES
		/// <param name="eaglContext">The source <see cref="OpenGLES.EAGLContext" />.</param>
		///         <param name="options">The desired <see cref="CoreImage.CIContextOptions" />.</param>
		///         <summary>Creates a <see cref="CoreImage.CIContext" /> based on the <paramref name="eaglContext" />, with the specified <paramref name="options" />.</summary>
		///         <returns>A new <see cref="CoreImage.CIContext" />.</returns>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("tvos")]
		[UnsupportedOSPlatform ("maccatalyst")]
		[UnsupportedOSPlatform ("macos")]
		[ObsoletedOSPlatform ("ios12.0")]
		[ObsoletedOSPlatform ("tvos12.0")]
		public static CIContext FromContext (EAGLContext eaglContext, CIContextOptions? options)
		{
			if (options is null)
				return FromContext (eaglContext);

			return FromContext (eaglContext, options.Dictionary);
		}
#endif

		/// <param name="device">To be added.</param>
		///         <param name="options">To be added.</param>
		///         <summary>Creates a new CIContext from the provided Metal device, along with the specified context.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static CIContext FromMetalDevice (IMTLDevice device, CIContextOptions? options)
		{
			if (options is null)
				return FromMetalDevice (device);

			return FromMetalDevice (device, options.Dictionary);
		}

#if MONOMAC
		/// <param name="size">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[UnsupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[ObsoletedOSPlatform ("macos10.11")]
		public CGLayer? CreateCGLayer (CGSize size)
		{
			return CreateCGLayer (size, null);
		}
#else
		/// <param name="options">To be added.</param>
		///         <summary>Creates a new <see cref="CoreImage.CIContext" /> from the options that are named in <paramref name="options" />.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static CIContext FromOptions (CIContextOptions? options)
		{
			return FromOptions (options?.Dictionary);
		}

		/// <param name="image">To be added.</param>
		///         <param name="fromRect">To be added.</param>
		///         <param name="ciImageFormat">To be added.</param>
		///         <param name="colorSpace">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public CGImage? CreateCGImage (CIImage image, CGRect fromRect, CIFormat ciImageFormat, CGColorSpace? colorSpace)
		{
			return CreateCGImage (image, fromRect, CIImage.CIFormatToInt (ciImageFormat), colorSpace);
		}
#endif
	}
}
