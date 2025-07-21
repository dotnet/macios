//
// CVOpenGLESTexture.cs: Implementation of the CVOpenGLESTexture class
//
// Author:
//   Miguel de Icaza (miguel@xamarin.com)
//
// Copyright 2012-2015 Xamarin Inc
//
//

#if HAS_OPENGLES && !__MACOS__ && !__MACCATALYST__

using System;
using System.Runtime.InteropServices;

using ObjCRuntime;
using CoreFoundation;
using Foundation;
using OpenGLES;

#nullable enable

namespace CoreVideo {

	// CVOpenGLESTextureCache.h
	[SupportedOSPlatform ("tvos")]
	[SupportedOSPlatform ("ios")]
	[ObsoletedOSPlatform ("tvos12.0", "Use 'CVMetalTextureCache' instead.")]
	[ObsoletedOSPlatform ("ios12.0", "Use 'CVMetalTextureCache' instead.")]
	public class CVOpenGLESTextureCache : NativeObject {
		[DllImport (Constants.CoreVideoLibrary)]
		unsafe extern static int CVOpenGLESTextureCacheCreate (
			/* CFAllocatorRef __nullable */ IntPtr allocator,
			/* CFDictionaryRef __nullable */ IntPtr cacheAttributes,
			/* CVEAGLContext __nonnull */ IntPtr eaglContext,
			/* CFDictionaryRef __nullable */ IntPtr textureAttextureAttributestr,
			/* CVOpenGLESTextureCacheRef __nullable * __nonnull */ IntPtr* cacheOut);

		[Preserve (Conditional = true)]
		CVOpenGLESTextureCache (NativeHandle handle, bool owns)
			: base (handle, owns)
		{
		}

		public CVOpenGLESTextureCache (EAGLContext context)
			: base (Create (context), true)
		{
		}

		static IntPtr Create (EAGLContext context, bool throwIfFailure = true)
		{
			if (context is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (context));

			var handle = default (IntPtr);
			int errorCode;

			unsafe {
				errorCode = CVOpenGLESTextureCacheCreate (IntPtr.Zero,
							  IntPtr.Zero, /* change one day to support cache attributes */
							  context.GetNonNullHandle (nameof (context)),
							  IntPtr.Zero, /* change one day to support texture attributes */
							  &handle);

				GC.KeepAlive (context);
			}
			if (errorCode == 0)
				return handle;

			if (!throwIfFailure)
				return IntPtr.Zero;

			throw new Exception ("Could not create the texture cache");
		}

		public static CVOpenGLESTextureCache? FromEAGLContext (EAGLContext context)
		{
			var handle = Create (context, false);
			if (handle == IntPtr.Zero)
				return null;

			return new CVOpenGLESTextureCache (handle, true);
		}

		public CVOpenGLESTexture? CreateTexture (CVImageBuffer imageBuffer, bool isTexture2d, int internalFormat, int width, int height, uint pixelFormat, uint pixelType, int planeIndex, out CVReturn errorCode)
		{
			if (imageBuffer is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (imageBuffer));

			uint target = isTexture2d ? (uint) 0x0DE1 /* GL_TEXTURE_2D */ : (uint) 0x8D41 /* GL_RENDERBUFFER */;
			var texture = default (IntPtr);
			unsafe {
				errorCode = CVOpenGLESTextureCacheCreateTextureFromImage (
					IntPtr.Zero,
					GetCheckedHandle (), /* textureCache dict, one day we might add it */
					imageBuffer.GetCheckedHandle (),
					IntPtr.Zero,
					target,
					internalFormat, width, height, pixelFormat,
					pixelType, (IntPtr) planeIndex, &texture);

				GC.KeepAlive (imageBuffer);
			}
			if (errorCode != 0)
				return null;
			return new CVOpenGLESTexture (texture, true);
		}

		[DllImport (Constants.CoreVideoLibrary)]
		extern static void CVOpenGLESTextureCacheFlush (
			/* CVOpenGLESTextureCacheRef __nonnull */ IntPtr textureCache, CVOptionFlags flags);

		public void Flush (CVOptionFlags flags)
		{
			CVOpenGLESTextureCacheFlush (GetCheckedHandle (), flags);
		}

		[DllImport (Constants.CoreVideoLibrary)]
		unsafe extern static CVReturn CVOpenGLESTextureCacheCreateTextureFromImage (
			/* CFAllocatorRef __nullable */ IntPtr allocator,
			/* CVOpenGLESTextureCacheRef */ IntPtr textureCache,
			/* CVImageBufferRef __nonnull */ IntPtr sourceImage,
			/* CFDictionaryRef __nullable */ IntPtr textureAttr,
			/* GLenum */ uint target,
			/* GLint */ int internalFormat,
			/* GLsizei */ int width,
			/* GLsizei */ int height,
			/* GLenum */ uint format,
			/* GLenum */ uint type,
			/* size_t */ IntPtr planeIndex,
			/* CVOpenGLESTextureRef __nullable * __nonnull */ IntPtr* textureOut);
	}
}

#endif
