using ObjCRuntime;

namespace Metal {
	public partial interface IMTL4RenderCommandEncoder {
		/// <summary>Set the viewports.</summary>
		/// <param name="viewports">The viewports to set.</param>
		public void SetViewports (params MTLViewport [] viewports)
		{
			if (viewports is null)
				ThrowHelper.ThrowArgumentNullException (nameof (viewports));

			unsafe {
				fixed (MTLViewport* ptr = viewports) {
					SetViewports ((IntPtr) ptr, (nuint) viewports.Length);
				}
			}
		}

		/// <summary>Set the view mappings and associated vertex amplification count.</summary>
		/// <param name="viewMappings">An array of view mappings.</param>
		public void SetVertexAmplificationCount (params MTLVertexAmplificationViewMapping []? viewMappings)
		{
			unsafe {
				fixed (MTLVertexAmplificationViewMapping* ptr = viewMappings) {
					SetVertexAmplificationCount ((nuint) (viewMappings?.Length ?? 0), (IntPtr) ptr);
				}
			}
		}

		/// <summary>Set an array of rectangles for a fragment scissor test.</summary>
		/// <param name="scissorRects">The rectangles to set.</param>
		public void SetScissorRects (params MTLScissorRect [] scissorRects)
		{
			if (scissorRects is null)
				ThrowHelper.ThrowArgumentNullException (nameof (scissorRects));

			unsafe {
				fixed (MTLScissorRect* ptr = scissorRects) {
					SetScissorRects ((IntPtr) ptr, (nuint) scissorRects.Length);
				}
			}
		}
	}
}
