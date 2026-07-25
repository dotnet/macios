#if !COREBUILD

#nullable enable

namespace Metal {
	public partial class MTLRenderPassDescriptor {
		/// <summary>Sets the programmable sample positions with data from <paramref name="positions" />.</summary>
		/// <param name="positions">The positions to set.</param>
		public unsafe void SetSamplePositions (MTLSamplePosition [] positions)
		{
			fixed (void* handle = positions)
				SetSamplePositions ((IntPtr) handle, (nuint) (positions?.Length ?? 0));
		}

		/// <summary>Fills <paramref name="positions" /> with programmable sample positions.</summary>
		/// <param name="positions">An array to fill with sample positions.</param>
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
