#if !COREBUILD
using System;

#nullable enable

namespace Metal {
	public partial class MTLRenderPassDescriptor {
		/// <param name="positions">The positions to set.</param>
		///         <summary>Sets the programmable sample postions with data from <paramref name="positions" />.</summary>
		///         <remarks>To be added.</remarks>
		public unsafe void SetSamplePositions (MTLSamplePosition [] positions)
		{
			fixed (void* handle = positions)
				SetSamplePositions ((IntPtr) handle, (nuint) (positions?.Length ?? 0));
		}

		/// <param name="positions">An array to fill with sample positions.</param>
		///         <summary>Fills <paramref name="positions" /> with programmable sample positions.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public unsafe nuint GetSamplePositions (MTLSamplePosition [] positions)
		{
			fixed (void* handle = positions) {
				nuint count = GetSamplePositions ((IntPtr) handle, (nuint) (positions?.Length ?? 0));
				return count;
			}
		}
	}
}
#endif
