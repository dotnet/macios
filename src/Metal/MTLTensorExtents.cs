using System;

using Foundation;
using Metal;
using ObjCRuntime;

#nullable enable

namespace Metal {
	public partial class MTLTensorExtents {
		/// <summary>Create a new <see cref="MTLTensorExtents" /> instance with the specified dimensions.</summary>
		/// <param name="values">The dimensions of the new tensor extents.</param>
		/// <remarks>At the time of this writing, a maximum of 16 dimensions are supported.</remarks>
		public MTLTensorExtents (params nint []? values)
			: base (NSObjectFlag.Empty)
		{
			unsafe {
				fixed (nint* valuesPtr = values) {
					InitializeHandle (_InitWithRank ((nuint) (values?.Length ?? 0), (IntPtr) valuesPtr), "initWithRank:values:");
				}
			}
		}
	}
}
