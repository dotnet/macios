using System;
using System.Runtime.Versioning;
using Foundation;
using ObjCRuntime;

#if HAS_OPENGLES
using OpenGLES;
using EAGLContext = global::OpenGLES.EAGLContext;
#else
using EAGLContext = global::Foundation.NSObject; // won't be used -> but must compile
#endif

#nullable enable

namespace SceneKit {
	public partial class SCNRenderer {

#if HAS_OPENGLES

		[UnsupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
		public static SCNRenderer FromContext (EAGLContext context, NSDictionary? options)
		{

			// GetHandle will return IntPtr.Zero is context is null
			// GLContext == CGLContext on macOS and EAGLContext in iOS and tvOS (using on top of file)
			var renderer = FromContext (context.GetHandle (), options); 
			GC.KeepAlive (context);
			return renderer;
		}
#endif

	}
}
