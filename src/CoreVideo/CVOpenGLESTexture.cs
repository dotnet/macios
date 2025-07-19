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

	// CVOpenGLESTexture.h
	[SupportedOSPlatform ("tvos")]
	[SupportedOSPlatform ("ios")]
	[ObsoletedOSPlatform ("tvos12.0", "Use 'CVMetalTexture' instead.")]
	[ObsoletedOSPlatform ("ios12.0", "Use 'CVMetalTexture' instead.")]
	public class CVOpenGLESTexture : NativeObject {
		[Preserve (Conditional = true)]
		internal CVOpenGLESTexture (NativeHandle handle, bool owns)
			: base (handle, owns)
		{
		}

		[DllImport (Constants.CoreVideoLibrary)]
		extern static /* GLenum */ uint CVOpenGLESTextureGetTarget (
			/* CVOpenGLESTextureRef __nonnull */ IntPtr image);

		[DllImport (Constants.CoreVideoLibrary)]
		extern static /* GLuint */ int CVOpenGLESTextureGetName (
			/* CVOpenGLESTextureRef __nonnull */ IntPtr image);
		// note: kept int for compatibility

		[DllImport (Constants.CoreVideoLibrary)]
		extern static /* Boolean */ byte CVOpenGLESTextureIsFlipped (
			/* CVOpenGLESTextureRef __nonnull */ IntPtr image);

		[DllImport (Constants.CoreVideoLibrary)]
		extern static void CVOpenGLESTextureGetCleanTexCoords (
			/* CVOpenGLESTextureRef __nonnull */ IntPtr image, 
			/* GLfloat[2] */ IntPtr lowerLeft, /* GLfloat[2] */ IntPtr lowerRight, /* GLfloat[2] */ IntPtr upperRight, 
			/* GLfloat[2] */ IntPtr upperLeft);
		// note: a GLfloat is 4 bytes even on 64bits iOS

		public uint Target {
			get {
				return CVOpenGLESTextureGetTarget (GetCheckedHandle ());
			}
		}

		public int Name {
			get {
				return CVOpenGLESTextureGetName (GetCheckedHandle ());
			}
		}

		public bool IsFlipped {
			get {
				return CVOpenGLESTextureIsFlipped (GetCheckedHandle ()) != 0;
			}
		}

		public void GetCleanTexCoords (out float [] lowerLeft, out float [] lowerRight, out float [] upperRight, out float [] upperLeft)
		{
			lowerLeft = new float [2];
			lowerRight = new float [2];
			upperRight = new float [2];
			upperLeft = new float [2];

			unsafe {
				fixed (float *ll = lowerLeft, lr = lowerRight, ur = upperRight, ul = upperLeft){
					CVOpenGLESTextureGetCleanTexCoords (GetCheckedHandle (), (IntPtr) ll, (IntPtr) lr, (IntPtr) ur, (IntPtr) ul);
				}
			}
		}
	}
}

#endif
