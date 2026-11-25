#if !__MACCATALYST__
using System.Drawing;

#nullable enable

namespace AppKit {

	public partial class NSOpenGLContext {

		unsafe void SetValue (int /* GLint */ val, NSOpenGLContextParameter par)
		{
			int* p = &val;
			SetValues ((IntPtr) p, par);
		}

		unsafe int /* GLint */ GetValue (NSOpenGLContextParameter par)
		{
			int ret;
			int* p = &ret;
			GetValues ((IntPtr) p, par);

			return ret;
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		unsafe public Rectangle SwapRectangle {
			get {
				Rectangle ret;
#pragma warning disable CA1422 // This call site is reachable on: 'macOS/OSX' 14.0 and later. 'NSOpenGLContextParameter.SwapRectangle' is obsoleted on: 'macOS/OSX' 10.7 and later.
				GetValues ((IntPtr) (&ret), NSOpenGLContextParameter.SwapRectangle);
				return ret;
			}
			set {
				SetValues ((IntPtr) (&value), NSOpenGLContextParameter.SwapRectangle);
#pragma warning restore CA1422
			}
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public bool SwapRectangleEnabled {
			get {
#pragma warning disable CA1422 // This call site is reachable on: 'macOS/OSX' 14.0 and later. 'NSOpenGLContextParameter.SwapRectangleEnable' is obsoleted on: 'macOS/OSX' 10.7 and later.
				return GetValue (NSOpenGLContextParameter.SwapRectangleEnable) != 0;
			}
			set {
				SetValue (value ? 1 : 0, NSOpenGLContextParameter.SwapRectangleEnable);
#pragma warning restore CA1422
			}
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public bool RasterizationEnabled {
			get {
#pragma warning disable CA1422 // This call site is reachable on: 'macOS/OSX' 14.0 and later. 'NSOpenGLContextParameter.RasterizationEnable' is obsoleted on: 'macOS/OSX' 10.7 and later.
				return GetValue (NSOpenGLContextParameter.RasterizationEnable) != 0;
			}
			set {
				SetValue (value ? 1 : 0, NSOpenGLContextParameter.RasterizationEnable);
#pragma warning restore CA1422
			}
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public bool SwapInterval {
			get {
				return GetValue (NSOpenGLContextParameter.SwapInterval) != 0;
			}
			set {
				SetValue (value ? 1 : 0, NSOpenGLContextParameter.SwapInterval);
			}
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public NSSurfaceOrder SurfaceOrder {
			get {
				switch (GetValue (NSOpenGLContextParameter.SurfaceOrder)) {
				case -1:
					return NSSurfaceOrder.BelowWindow;
				default:
					return NSSurfaceOrder.AboveWindow;
				}
			}
			set {
				SetValue (value == NSSurfaceOrder.BelowWindow ? -1 : 1, NSOpenGLContextParameter.SurfaceOrder);
			}
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public bool SurfaceOpaque {
			get {
				return GetValue (NSOpenGLContextParameter.SurfaceOpacity) != 0;
			}
			set {
				SetValue (value ? 1 : 0, NSOpenGLContextParameter.SurfaceOpacity);
			}
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public bool StateValidation {
			get {
#pragma warning disable CA1422 // This call site is reachable on: 'macOS/OSX' 14.0 and later. 'NSOpenGLContextParameter.StateValidation' is obsoleted on: 'macOS/OSX' 10.7 and later.
				return GetValue (NSOpenGLContextParameter.StateValidation) != 0;
			}
			set {
				SetValue (value ? 1 : 0, NSOpenGLContextParameter.StateValidation);
#pragma warning restore CA1422
			}
		}
	}
}
#endif // !__MACCATALYST__
