//
// Author:
//   Jonathan Pryor:
//
// (C) 2009 Novell, Inc.
//
using System;
using System.Runtime.InteropServices;

using Foundation;
using ObjCRuntime;

// Disable until we get around to enable + fix any issues.
#nullable disable

namespace OpenGLES {

#if NET
	[SupportedOSPlatform ("tvos")]
	[SupportedOSPlatform ("ios")]
	[ObsoletedOSPlatform ("tvos12.0", "Use 'Metal' instead.")]
	[ObsoletedOSPlatform ("ios12.0", "Use 'Metal' instead.")]
#else
	[Deprecated (PlatformName.iOS, 12, 0, message: "Use 'Metal' instead.")]
	[Deprecated (PlatformName.TvOS, 12, 0, message: "Use 'Metal' instead.")]
#endif
	public static class EAGLDrawableProperty {
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public static readonly NSString ColorFormat;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public static readonly NSString RetainedBacking;

		static EAGLDrawableProperty ()
		{
			var handle = Libraries.OpenGLES.Handle;
			ColorFormat = Dlfcn.GetStringConstant (handle,
					"kEAGLDrawablePropertyColorFormat");
			RetainedBacking = Dlfcn.GetStringConstant (handle,
					"kEAGLDrawablePropertyRetainedBacking");
		}
	}

#if NET
	[SupportedOSPlatform ("tvos")]
	[SupportedOSPlatform ("ios")]
	[ObsoletedOSPlatform ("tvos12.0", "Use 'Metal' instead.")]
	[ObsoletedOSPlatform ("ios12.0", "Use 'Metal' instead.")]
#else
	[Deprecated (PlatformName.iOS, 12, 0, message: "Use 'Metal' instead.")]
	[Deprecated (PlatformName.TvOS, 12, 0, message: "Use 'Metal' instead.")]
#endif
	public static class EAGLColorFormat {
		/// <summary>16-bit RGB color format.</summary>
		///         <remarks>This format uses 5 bits for the red components, 6 bits for the green component and 5 bits for the blue component.   Correspondsto the OpenGL ES GL_RGB565 format.</remarks>
		public static readonly NSString RGB565;
		/// <summary>32-bit RGBA format.</summary>
		///         <remarks>This is a 32 bit format that uses 8 bits for red, green, blue and alpha channels.   Corresponds to the OpenGL ES GL_RGBA8888 value.</remarks>
		public static readonly NSString RGBA8;

		static EAGLColorFormat ()
		{
			var handle = Libraries.OpenGLES.Handle;
			RGB565 = Dlfcn.GetStringConstant (handle, "kEAGLColorFormatRGB565");
			RGBA8 = Dlfcn.GetStringConstant (handle, "kEAGLColorFormatRGBA8");
		}
	}
}
