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

		/// <summary>Gets or sets the area of the drawable that is affected when the buffers are swapped.</summary>
		/// <value>The swap rectangle. It's only used if <see cref="SwapRectangleEnabled" /> is <see langword="true" />.</value>
		/// <remarks>
		///   <para>This property is obsolete, because OpenGL is deprecated. Use Metal instead.</para>
		/// </remarks>
		[SupportedOSPlatform ("macos")]
		[ObsoletedOSPlatform ("macos10.7")]
		unsafe public Rectangle SwapRectangle {
			get {
				Rectangle ret;
				GetValues ((IntPtr) (&ret), NSOpenGLContextParameter.SwapRectangle);
				return ret;
			}
			set {
				SetValues ((IntPtr) (&value), NSOpenGLContextParameter.SwapRectangle);
			}
		}

		/// <summary>Gets or sets whether <see cref="SwapRectangle" /> is used when the buffers are swapped.</summary>
		/// <value><see langword="true" /> if only the area in <see cref="SwapRectangle" /> is swapped, <see langword="false" /> otherwise.</value>
		/// <remarks>
		///   <para>This property is obsolete, because OpenGL is deprecated. Use Metal instead.</para>
		/// </remarks>
		[SupportedOSPlatform ("macos")]
		[ObsoletedOSPlatform ("macos10.7")]
		public bool SwapRectangleEnabled {
			get {
				return GetValue (NSOpenGLContextParameter.SwapRectangleEnable) != 0;
			}
			set {
				SetValue (value ? 1 : 0, NSOpenGLContextParameter.SwapRectangleEnable);
			}
		}

		/// <summary>Gets or sets whether rasterization is enabled.</summary>
		/// <value><see langword="true" /> if rasterization is enabled, <see langword="false" /> otherwise.</value>
		/// <remarks>
		///   <para>This property is obsolete, because OpenGL is deprecated. Use Metal instead.</para>
		/// </remarks>
		[SupportedOSPlatform ("macos")]
		[ObsoletedOSPlatform ("macos10.7")]
		public bool RasterizationEnabled {
			get {
				return GetValue (NSOpenGLContextParameter.RasterizationEnable) != 0;
			}
			set {
				SetValue (value ? 1 : 0, NSOpenGLContextParameter.RasterizationEnable);
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

		/// <summary>Gets or sets whether state validation is enabled.</summary>
		/// <value><see langword="true" /> if state validation is enabled, <see langword="false" /> otherwise.</value>
		/// <remarks>
		///   <para>This property is obsolete, because OpenGL is deprecated. Use Metal instead.</para>
		/// </remarks>
		[SupportedOSPlatform ("macos")]
		[ObsoletedOSPlatform ("macos10.7")]
		public bool StateValidation {
			get {
				return GetValue (NSOpenGLContextParameter.StateValidation) != 0;
			}
			set {
				SetValue (value ? 1 : 0, NSOpenGLContextParameter.StateValidation);
			}
		}
	}
}
#endif // !__MACCATALYST__
