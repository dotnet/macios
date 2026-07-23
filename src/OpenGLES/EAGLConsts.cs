//
// Author:
//   Jonathan Pryor:
//
// (C) 2009 Novell, Inc.
//

#nullable enable

namespace OpenGLES {
	/// <summary>EAGLDrawable properties.</summary>
	/// <remarks>This class contains the keys for a few properties that can be get and set in EAGLDrawables.</remarks>
	[ObsoletedOSPlatform ("tvos", "Use 'Metal' instead.")]
	[ObsoletedOSPlatform ("ios", "Use 'Metal' instead.")]
	public static partial class EAGLDrawableProperty {
		/// <summary>Can be used to configure the internal color format for drawables.</summary>
		public static readonly NSString? ColorFormat = _ColorFormat;
		/// <summary>Can be used to configure whether drawables retain their contents after displaying them.</summary>
		public static readonly NSString? RetainedBacking = _RetainedBacking;
	}

	/// <summary>EAGLColorFormat strings.</summary>
	/// <remarks>This class contains the keys for a few properties that can be get and set in EAGLDrawables.</remarks>
	[ObsoletedOSPlatform ("tvos", "Use 'Metal' instead.")]
	[ObsoletedOSPlatform ("ios", "Use 'Metal' instead.")]
	public static partial class EAGLColorFormat {
		/// <summary>16-bit RGB color format.</summary>
		/// <remarks>This format uses 5 bits for the red components, 6 bits for the green component and 5 bits for the blue component. Corresponds to the OpenGL ES GL_RGB565 format.</remarks>
		public static readonly NSString? RGB565 = _RGB565;
		/// <summary>32-bit RGBA format.</summary>
		/// <remarks>This is a 32 bit format that uses 8 bits for red, green, blue and alpha channels. Corresponds to the OpenGL ES GL_RGBA8888 value.</remarks>
		public static readonly NSString? RGBA8 = _RGBA8;
	}
}
