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

using System.ComponentModel;

using CoreGraphics;
using CoreFoundation;
using Metal;
#if HAS_OPENGLES
using OpenGLES;
#endif

#nullable enable

namespace CoreImage {
	/// <summary>Use to configure the CIContext rendering pipeline.</summary>
	/// <remarks>You would use an instance of this class to configure the CIContext rendering operations.</remarks>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	public partial class CIContextOptions : DictionaryContainer {
#if !XAMCORE_5_0
		[EditorBrowsable (EditorBrowsableState.Never)]
		[Obsolete ("Use 'NullableUseSoftwareRenderer' instead.")]
		public bool UseSoftwareRenderer {
			get {
				var b = GetBoolValue (CIContextOptionKeys.UseSoftwareRenderer);
				return b.HasValue ? b.Value : false;
			}
			set {
				SetBooleanValue (CIContextOptionKeys.UseSoftwareRenderer, value);
			}
		}

		/// <summary>Gets or sets the image format to use for storing intermediate rendering results.</summary>
		[EditorBrowsable (EditorBrowsableState.Never)]
		[Obsolete ("Use 'WorkingFormatField' instead.")]
		public int? CIImageFormat {
			get {
				return GetInt32Value (CIContextOptionKeys.WorkingFormatField);
			}
			set {
				SetNumberValue (CIContextOptionKeys.WorkingFormatField, value);
			}
		}
#endif // !XAMCORE_5_0
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
