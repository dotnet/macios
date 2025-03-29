using System;
using System.Runtime.Versioning;
using Foundation;
using ObjCRuntime;

#if MONOMAC
using GLContext = global::OpenGL.CGLContext;
#else

#if HAS_OPENGLES
using OpenGLES;
using GLContext = global::OpenGLES.EAGLContext;
#else
using GLContext = global::Foundation.NSObject; // won't be used -> but must compile
#endif
#endif

#nullable enable

namespace SceneKit {
	public partial class SCNRenderer {

#if !__MACCATALYST__

		[UnsupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
		public static SCNRenderer FromContext (GLContext context, NSDictionary? options)
		{

			// GetHandle will return IntPtr.Zero if context is null
			// GLContext == CGLContext on macOS and EAGLContext in iOS and tvOS (using on top of file)
			var renderer = FromContext (context.GetHandle (), options);
			GC.KeepAlive (context);
			return renderer;
		}
#endif

	}
}
