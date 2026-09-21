// Copyright 2015 Xamarin Inc. All rights reserved.

#nullable enable

using Metal;

namespace MetalPerformanceShaders {

	public partial class MPSImageScale {
		static int size_of_scale_transform = Marshal.SizeOf<MPSScaleTransform> ();

		/// <summary>Gets or sets the scale transform applied to the image.</summary>
		/// <value>The scale transform, or <see langword="null" /> if no transform is set.</value>
		public virtual MPSScaleTransform? ScaleTransform {
			get {
				var ptr = _GetScaleTransform ();
				if (ptr == IntPtr.Zero)
					return null;
				return Marshal.PtrToStructure<MPSScaleTransform> (ptr);
			}
			set {
				if (value.HasValue) {
					IntPtr ptr = Marshal.AllocHGlobal (size_of_scale_transform);
					try {
						Marshal.StructureToPtr<MPSScaleTransform> (value.Value, ptr, false);
						_SetScaleTransform (ptr);
					} finally {
						Marshal.FreeHGlobal (ptr);
					}
				} else {
					_SetScaleTransform (IntPtr.Zero);
				}
			}
		}
	}
}
