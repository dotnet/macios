//
// GLK/GLTextureLoader.cs: extenions for GLTextureLoader
//
// Authors:
//   Miguel de Icaza
//
// Copyright 2011-2013 Xamarin, Inc.
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
using System.Runtime.InteropServices;
using Foundation;
using CoreFoundation;
using CoreGraphics;
using ObjCRuntime;

#nullable enable

namespace GLKit {
	public partial class GLKTextureLoader {
		/// <param name="files">Six files that point to the sides of the cube.</param>
		///         <param name="textureOperations">An NSDictionary populated with configuration options.   Alternatively, use the strongly-typed version of this method that takes a GLKTextureOperations object.</param>
		///         <param name="error">Error result.</param>
		///         <summary>Loads a cube map synchronously.</summary>
		///         <returns>On error, this will return null, the details of the error will be stored in the NSError parameter.   Otherwise the instance of the GLKTextureInfo.</returns>
		///         <remarks>To be added.</remarks>
		public static GLKTextureInfo? CubeMapFromFiles (string [] files, NSDictionary? textureOperations, out NSError error)
		{
			using (var array = NSArray.FromStrings (files))
				return CubeMapFromFiles (array, textureOperations, out error);
		}

		/// <param name="urls">Six URLs that point to the sides of the cube.</param>
		///         <param name="textureOperations">An NSDictionary populated with configuration options.   Alternatively, use the strongly-typed version of this method that takes a GLKTextureOperations object.</param>
		///         <param name="error">Error result.</param>
		///         <summary>Loads a cube map synchronously.</summary>
		///         <returns>On error, this will return null, the details of the error will be stored in the NSError parameter.   Otherwise the instance of the GLKTextureInfo.</returns>
		///         <remarks>To be added.</remarks>
		public static GLKTextureInfo? CubeMapFromUrls (NSUrl [] urls, NSDictionary? textureOperations, out NSError error)
		{
			using (var array = NSArray.FromNSObjects (urls))
				return CubeMapFromFiles (array, textureOperations, out error);
		}

		/// <param name="files">Six files that point to the sides of the cube.</param>
		///         <param name="textureOperations">An NSDictionary populated with configuration options.   Alternatively, use the strongly-typed version of this method that takes a GLKTextureOperations object.</param>
		///         <param name="queue">The queue on which the callback method will be invoked, or null to invoke the method on the main dispatch queue.</param>
		///         <param name="onComplete">Callback to invoke when the texture is loaded.   The callback receives a GLKTextureInfo and an NSError.</param>
		///         <summary>Asynchronously loads a cube map.</summary>
		///         <remarks>
		///           <para>
		/// 	    Loads the data in the background.  When the data has
		/// 	    loaded, or an error has been encountered, the provided
		/// 	    callback is invoked on the specified queue.  If no queue
		/// 	    is specified, the callback is invoked on the main queue.
		///
		/// 	  </para>
		///         </remarks>
		public void BeginLoadCubeMap (string [] files, NSDictionary? textureOperations, DispatchQueue queue, GLKTextureLoaderCallback onComplete)
		{
			using (var array = NSArray.FromStrings (files))
				BeginLoadCubeMap (array, textureOperations, queue, onComplete);
		}

		/// <param name="urls">Six URLs that point to the sides of the cube.</param>
		///         <param name="textureOperations">An NSDictionary populated with configuration options.   Alternatively, use the strongly-typed version of this method that takes a GLKTextureOperations object.</param>
		///         <param name="queue">The queue on which the callback method will be invoked, or null to invoke the method on the main dispatch queue.</param>
		///         <param name="onComplete">Callback to invoke when the texture is loaded.   The callback receives a GLKTextureInfo and an NSError.</param>
		///         <summary>Asynchronously loads a cube map.</summary>
		///         <remarks>
		///           <para>
		/// 	    Loads the data in the background.  When the data has
		/// 	    loaded, or an error has been encountered, the provided
		/// 	    callback is invoked on the specified queue.  If no queue
		/// 	    is specified, the callback is invoked on the main queue.
		/// 	  </para>
		///         </remarks>
		public void BeginLoadCubeMap (NSUrl [] urls, NSDictionary? textureOperations, DispatchQueue queue, GLKTextureLoaderCallback onComplete)
		{
			using (var array = NSArray.FromNSObjects (urls))
				BeginLoadCubeMap (array, textureOperations, queue, onComplete);
		}

		//
		// New, strongly typed
		//
		/// <param name="path">File name where the data will be loaded from.</param>
		///         <param name="textureOperations">Operations to be performed during the image loading on the texture.</param>
		///         <param name="error">Error result.</param>
		///         <summary>Loads a texture from a file synchronously.</summary>
		///         <returns>On error, this will return null, the details of the error will be stored in the NSError parameter.   Otherwise the instance of the GLKTextureInfo.</returns>
		///         <remarks>To be added.</remarks>
		public static GLKTextureInfo? FromFile (string path, GLKTextureOperations? textureOperations, out NSError error)
		{
			return FromFile (path, textureOperations?.Dictionary, out error);
		}

		/// <param name="url">URL pointing to the texture to load.</param>
		///         <param name="textureOperations">Operations to be performed during the image loading on the texture.</param>
		///         <param name="error">Error result.</param>
		///         <summary>Loads a texture from a file pointed to by the url.</summary>
		///         <returns>On error, this will return null, the details of the error will be stored in the NSError parameter.   Otherwise the instance of the GLKTextureInfo.</returns>
		///         <remarks>To be added.</remarks>
		public static GLKTextureInfo? FromUrl (NSUrl url, GLKTextureOperations? textureOperations, out NSError error)
		{
			return FromUrl (url, textureOperations?.Dictionary, out error);
		}

		/// <param name="data">NSData object that contains the bitmap that will be loaded into the texture.</param>
		///         <param name="textureOperations">Operations to be performed during the image loading on the texture.</param>
		///         <param name="error">Error result.</param>
		///         <summary>Loads a texture from an NSData source.</summary>
		///         <returns>On error, this will return null, the details of the error will be stored in the NSError parameter.   Otherwise the instance of the GLKTextureInfo.</returns>
		///         <remarks>To be added.</remarks>
		public static GLKTextureInfo? FromData (NSData data, GLKTextureOperations? textureOperations, out NSError error)
		{
			return FromData (data, textureOperations?.Dictionary, out error);
		}

		/// <param name="cgImage">CGImage that contains the image to be loaded into the texture.</param>
		///         <param name="textureOperations">Operations to be performed during the image loading on the texture.</param>
		///         <param name="error">Error result.</param>
		///         <summary>Loads a texture from a CGImage.</summary>
		///         <returns>On error, this will return null, the details of the error will be stored in the NSError parameter.   Otherwise the instance of the GLKTextureInfo.</returns>
		///         <remarks>To be added.</remarks>
		public static GLKTextureInfo? FromImage (CGImage cgImage, GLKTextureOperations? textureOperations, out NSError error)
		{
			return FromImage (cgImage, textureOperations?.Dictionary, out error);
		}

		/// <param name="files">Six files that point to the sides of the cube.</param>
		///         <param name="textureOperations">Operations to be performed during the image loading on the texture.</param>
		///         <param name="error">Error result.</param>
		///         <summary>Loads a cube map synchronously.</summary>
		///         <returns>On error, this will return null, the details of the error will be stored in the NSError parameter.   Otherwise the instance of the GLKTextureInfo.</returns>
		///         <remarks>To be added.</remarks>
		public static GLKTextureInfo? CubeMapFromFiles (string [] files, GLKTextureOperations? textureOperations, out NSError error)
		{
			using (var array = NSArray.FromStrings (files))
				return CubeMapFromFiles (files, textureOperations?.Dictionary, out error);
		}

		/// <param name="urls">Six URLs that point to the sides of the cube.</param>
		///         <param name="textureOperations">Operations to be performed during the image loading on the texture.</param>
		///         <param name="error">Error result.</param>
		///         <summary>Loads a cube map synchronously.</summary>
		///         <returns>On error, this will return null, the details of the error will be stored in the NSError parameter.   Otherwise the instance of the GLKTextureInfo.</returns>
		///         <remarks>To be added.</remarks>
		public static GLKTextureInfo? CubeMapFromUrls (NSUrl [] urls, GLKTextureOperations? textureOperations, out NSError error)
		{
			using (var array = NSArray.FromNSObjects (urls))
				return CubeMapFromFiles (array, textureOperations?.Dictionary, out error);
		}

		/// <param name="path">The file that contains the texture.</param>
		///         <param name="textureOperations">Operations to be performed during the image loading on the texture.</param>
		///         <param name="error">Error result.</param>
		///         <summary>Loads a cube map synchronously.</summary>
		///         <returns>On error, this will return null, the details of the error will be stored in the NSError parameter.   Otherwise the instance of the GLKTextureInfo.</returns>
		///         <remarks>To be added.</remarks>
		public static GLKTextureInfo? CubeMapFromFile (string path, GLKTextureOperations? textureOperations, out NSError error)
		{
			return CubeMapFromFile (path, textureOperations?.Dictionary, out error);
		}

		/// <param name="url">URL pointing to the texture to load.</param>
		///         <param name="textureOperations">Operations to be performed during the image loading on the texture.</param>
		///         <param name="error">Error result.</param>
		///         <summary>Loads a cube map synchronously.</summary>
		///         <returns>On error, this will return null, the details of the error will be stored in the NSError parameter.   Otherwise the instance of the GLKTextureInfo.</returns>
		///         <remarks>To be added.</remarks>
		public static GLKTextureInfo? CubeMapFromUrl (NSUrl url, GLKTextureOperations? textureOperations, out NSError error)
		{
			return CubeMapFromUrl (url, textureOperations?.Dictionary, out error);
		}

		/// <param name="file">The file that contains the texture.</param>
		///         <param name="textureOperations">Operations to be performed during the image loading on the texture.</param>
		///         <param name="queue">The queue on which the callback method will be invoked, or null to invoke the method on the main dispatch queue.</param>
		///         <param name="onComplete">Callback to invoke when the texture is loaded.   The callback receives a GLKTextureInfo and an NSError.</param>
		///         <summary>Asynchronously loads a texture.</summary>
		///         <remarks>
		///           <para>
		/// 	    Loads the data in the background.  When the data has
		/// 	    loaded, or an error has been encountered, the provided
		/// 	    callback is invoked on the specified queue.  If no queue
		/// 	    is specified, the callback is invoked on the main queue.
		///
		/// 	  </para>
		///         </remarks>
		public void BeginTextureLoad (string file, GLKTextureOperations? textureOperations, DispatchQueue queue, GLKTextureLoaderCallback onComplete)
		{
			BeginTextureLoad (file, textureOperations?.Dictionary, queue, onComplete);
		}

		/// <param name="filePath">The file that contains the texture.</param>
		///         <param name="textureOperations">Operations to be performed during the image loading on the texture.</param>
		///         <param name="queue">The queue on which the callback method will be invoked, or null to invoke the method on the main dispatch queue.</param>
		///         <param name="onComplete">Callback to invoke when the texture is loaded.   The callback receives a GLKTextureInfo and an NSError.</param>
		///         <summary>Asynchronously loads a texture.</summary>
		///         <remarks>
		///           <para>
		/// 	    Loads the data in the background.  When the data has
		/// 	    loaded, or an error has been encountered, the provided
		/// 	    callback is invoked on the specified queue.  If no queue
		/// 	    is specified, the callback is invoked on the main queue.
		///
		/// 	  </para>
		///         </remarks>
		public void BeginTextureLoad (NSUrl filePath, GLKTextureOperations? textureOperations, DispatchQueue queue, GLKTextureLoaderCallback onComplete)
		{
			BeginTextureLoad (filePath, textureOperations?.Dictionary, queue, onComplete);
		}

		/// <param name="data">NSData object that contains the bitmap that will be loaded into the texture.</param>
		///         <param name="textureOperations">Operations to be performed during the image loading on the texture.</param>
		///         <param name="queue">The queue on which the callback method will be invoked, or null to invoke the method on the main dispatch queue.</param>
		///         <param name="onComplete">Callback to invoke when the texture is loaded.   The callback receives a GLKTextureInfo and an NSError.</param>
		///         <summary>Asynchronously loads a texture.</summary>
		///         <remarks>
		///           <para>
		/// 	    Loads the data in the background.  When the data has
		/// 	    loaded, or an error has been encountered, the provided
		/// 	    callback is invoked on the specified queue.  If no queue
		/// 	    is specified, the callback is invoked on the main queue.
		///
		/// 	  </para>
		///         </remarks>
		public void BeginTextureLoad (NSData data, GLKTextureOperations? textureOperations, DispatchQueue queue, GLKTextureLoaderCallback onComplete)
		{
			BeginTextureLoad (data, textureOperations?.Dictionary, queue, onComplete);
		}

		/// <param name="image">CGImage that contains the image to be loaded into the texture.</param>
		///         <param name="textureOperations">Operations to be performed during the image loading on the texture.</param>
		///         <param name="queue">The queue on which the callback method will be invoked, or null to invoke the method on the main dispatch queue.</param>
		///         <param name="onComplete">Callback to invoke when the texture is loaded.   The callback receives a GLKTextureInfo and an NSError.</param>
		///         <summary>Asynchronously loads a texture.</summary>
		///         <remarks>
		///           <para>
		/// 	    Loads the data in the background.  When the data has
		/// 	    loaded, or an error has been encountered, the provided
		/// 	    callback is invoked on the specified queue.  If no queue
		/// 	    is specified, the callback is invoked on the main queue.
		///
		/// 	  </para>
		///         </remarks>
		public void BeginTextureLoad (CGImage image, GLKTextureOperations? textureOperations, DispatchQueue queue, GLKTextureLoaderCallback onComplete)
		{
			BeginTextureLoad (image, textureOperations?.Dictionary, queue, onComplete);
		}

		/// <param name="files">Six files that point to the sides of the cube.</param>
		///         <param name="textureOperations">Operations to be performed during the image loading on the texture.</param>
		///         <param name="queue">The queue on which the callback method will be invoked, or null to invoke the method on the main dispatch queue.</param>
		///         <param name="onComplete">Callback to invoke when the texture is loaded.   The callback receives a GLKTextureInfo and an NSError.</param>
		///         <summary>Asynchronously loads a cube map.</summary>
		///         <remarks>
		///           <para>
		/// 	    Loads the data in the background.  When the data has
		/// 	    loaded, or an error has been encountered, the provided
		/// 	    callback is invoked on the specified queue.  If no queue
		/// 	    is specified, the callback is invoked on the main queue.
		///
		/// 	  </para>
		///         </remarks>
		public void BeginLoadCubeMap (string [] files, GLKTextureOperations? textureOperations, DispatchQueue queue, GLKTextureLoaderCallback onComplete)
		{
			using (var array = NSArray.FromStrings (files))
				BeginLoadCubeMap (array, textureOperations?.Dictionary, queue, onComplete);
		}

		/// <param name="urls">Six URLs that point to the sides of the cube.</param>
		///         <param name="textureOperations">Operations to be performed during the image loading on the texture.</param>
		///         <param name="queue">The queue on which the callback method will be invoked, or null to invoke the method on the main dispatch queue.</param>
		///         <param name="onComplete">Callback to invoke when the texture is loaded.   The callback receives a GLKTextureInfo and an NSError.</param>
		///         <summary>Asynchronously loads a cube map.</summary>
		///         <remarks>
		///           <para>
		/// 	    Loads the data in the background.  When the data has
		/// 	    loaded, or an error has been encountered, the provided
		/// 	    callback is invoked on the specified queue.  If no queue
		/// 	    is specified, the callback is invoked on the main queue.
		///
		/// 	  </para>
		///         </remarks>
		public void BeginLoadCubeMap (NSUrl [] urls, GLKTextureOperations? textureOperations, DispatchQueue queue, GLKTextureLoaderCallback onComplete)
		{
			using (var array = NSArray.FromNSObjects (urls))
				BeginLoadCubeMap (array, textureOperations?.Dictionary, queue, onComplete);
		}

		/// <param name="fileName">File name where the data will be loaded from.</param>
		///         <param name="textureOperations">Operations to be performed during the image loading on the texture.</param>
		///         <param name="queue">The queue on which the callback method will be invoked, or null to invoke the method on the main dispatch queue.</param>
		///         <param name="onComplete">Callback to invoke when the texture is loaded.   The callback receives a GLKTextureInfo and an NSError.</param>
		///         <summary>Asynchronously loads a cube map.</summary>
		///         <remarks>
		///           <para>
		/// 	    Loads the data in the background.  When the data has
		/// 	    loaded, or an error has been encountered, the provided
		/// 	    callback is invoked on the specified queue.  If no queue
		/// 	    is specified, the callback is invoked on the main queue.
		///
		/// 	  </para>
		///         </remarks>
		public void BeginLoadCubeMap (string fileName, GLKTextureOperations? textureOperations, DispatchQueue queue, GLKTextureLoaderCallback onComplete)
		{
			BeginLoadCubeMap (fileName, textureOperations?.Dictionary, queue, onComplete);
		}

		/// <param name="filePath">The file that contains the texture.</param>
		///         <param name="textureOperations">Operations to be performed during the image loading on the texture.</param>
		///         <param name="queue">The queue on which the callback method will be invoked, or null to invoke the method on the main dispatch queue.</param>
		///         <param name="onComplete">Callback to invoke when the texture is loaded.   The callback receives a GLKTextureInfo and an NSError.</param>
		///         <summary>Asynchronously loads a cube map.</summary>
		///         <remarks>
		///           <para>
		/// 	    Loads the data in the background.  When the data has loaded,
		/// 	    or an error has been encountered, the provided
		/// 	    callback is invoked on the specified queue.  If no queue is
		/// 	    specified, the callback is invoked on the main queue.
		/// 	  </para>
		///         </remarks>
		public void BeginLoadCubeMap (NSUrl filePath, GLKTextureOperations? textureOperations, DispatchQueue queue, GLKTextureLoaderCallback onComplete)
		{
			BeginLoadCubeMap (filePath, textureOperations?.Dictionary, queue, onComplete);
		}

	}

#if NET
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	[UnsupportedOSPlatform ("maccatalyst")]
	[ObsoletedOSPlatform ("tvos12.0", "Use 'Metal' instead.")]
	[ObsoletedOSPlatform ("macos10.14", "Use 'Metal' instead.")]
	[ObsoletedOSPlatform ("ios12.0", "Use 'Metal' instead.")]
#else
	[Deprecated (PlatformName.iOS, 12, 0, message: "Use 'Metal' instead.")]
	[Deprecated (PlatformName.TvOS, 12, 0, message: "Use 'Metal' instead.")]
	[Deprecated (PlatformName.MacOSX, 10, 14, message: "Use 'Metal' instead.")]
#endif
	public class GLKTextureOperations : DictionaryContainer {
		/// <summary>Default constructor, creates an empty set of configuration options.</summary>
		///         <remarks>
		///         </remarks>
		public GLKTextureOperations () : base (new NSMutableDictionary ()) { }

		/// <param name="options">NSDictionary populated with an initial set of options.</param>
		///         <summary>Creates a GLKTextureOperations from an existing NSDictionary instance.</summary>
		///         <remarks>
		///         </remarks>
		public GLKTextureOperations (NSDictionary options) : base (options) { }

		/// <summary>Whether the texture should be pre-multiplied with the encoded Alpha channel or not.</summary>
		///         <value>
		///         </value>
		///         <remarks>
		///           <para>
		/// 	    This defaults to false.
		/// 	  </para>
		///           <para>
		/// 	    Should not be set to true for textures in compressed formats.
		/// 	  </para>
		///         </remarks>
		public bool? ApplyPremultiplication {
			get {
				return GetBoolValue (GLKTextureLoader.ApplyPremultiplication);
			}
			set {
				SetBooleanValue (GLKTextureLoader.ApplyPremultiplication, value);
			}
		}

		/// <summary>Whether to flip the image coordinates to match the OpenGL coordinate space.</summary>
		///         <value>
		///         </value>
		///         <remarks>
		///           <para>
		/// 	    This defaults to true.
		/// 	  </para>
		///         </remarks>
		public bool? OriginBottomLeft {
			get {
				return GetBoolValue (GLKTextureLoader.OriginBottomLeft);
			}
			set {
				SetBooleanValue (GLKTextureLoader.OriginBottomLeft, value);
			}
		}

		/// <summary>Whether mipmaps should be generated for the texture.</summary>
		///         <value>
		///         </value>
		///         <remarks>
		///           <para>
		/// 	    This defaults to false.
		/// 	  </para>
		///           <para>
		/// 	    If set to true, this will generate mipmaps levels on loading.   In addition the texture's GL_TEXTURE_MIN_FILTER is set to GL_LINEAR_MIPMAP_LINEAR.
		/// 	  </para>
		///         </remarks>
		public bool? GenerateMipmaps {
			get {
				return GetBoolValue (GLKTextureLoader.GenerateMipmaps);
			}
			set {
				SetBooleanValue (GLKTextureLoader.GenerateMipmaps, value);
			}
		}

		/// <summary>On greyscale images, controls whether the image is loaded as GL_ALPHA or GL_LUMINANCE.</summary>
		///         <value>false by default, so it loads the greyscale image as GL_LUMINANCE.</value>
		///         <remarks>
		/// 	  This property controls how greyscale images are loaded.  By default they are loaded as GL_LUMINANCE, but if you set this property to true, they will be loaded as GL_ALPHA.
		/// 	</remarks>
		public bool? GrayscaleAsAlpha {
			get {
				return GetBoolValue (GLKTextureLoader.GrayscaleAsAlpha);
			}
			set {
				SetBooleanValue (GLKTextureLoader.GrayscaleAsAlpha, value);
			}
		}

#if NET
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
		[UnsupportedOSPlatform ("maccatalyst")]
		[ObsoletedOSPlatform ("tvos12.0", "Use 'Metal' instead.")]
		[ObsoletedOSPlatform ("macos10.14", "Use 'Metal' instead.")]
		[ObsoletedOSPlatform ("ios12.0", "Use 'Metal' instead.")]
#endif
		public bool? SRGB {
			get {
				return GetBoolValue (GLKTextureLoader.SRGB);
			}
			set {
				SetBooleanValue (GLKTextureLoader.SRGB, value);
			}
		}
	}
}
