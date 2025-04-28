using ObjCRuntime;

namespace OpenGLES {

	// NSUInteger -> EAGL.h
	/// <summary>The rendering API supported.</summary>
	[Deprecated (PlatformName.iOS, 12, 0, message: "Use 'Metal' instead.")]
	[Deprecated (PlatformName.TvOS, 12, 0, message: "Use 'Metal' instead.")]
	[Native]
	public enum EAGLRenderingAPI : ulong {
		/// <summary>OpenGLES 1.</summary>
		OpenGLES1 = 1,
		/// <summary>OpenGLES 2.</summary>
		OpenGLES2 = 2,
		/// <summary>OpenGLES 3.</summary>
		OpenGLES3 = 3,
	}
}
