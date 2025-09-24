using System;

using Foundation;
using Metal;
using ObjCRuntime;

#nullable enable

namespace Metal {
	public partial class MTL4RenderPassDescriptor {
		/// <summary>Get or set the custom sample positions.</summary>
		/// <remarks>
		///   The getter supports a maximum of 128 sample positions. If there are more sample positions,
		///   use <see cref="GetSamplePositions(MTLSamplePosition[])" /> with a pre-allocated array instead.
		/// </remarks>
		public MTLSamplePosition []? SamplePositions {
			get {
				var array = new MTLSamplePosition [128];
				nuint count;
				unsafe {
					fixed (MTLSamplePosition* ptr = array) {
						count = GetSamplePositions ((IntPtr) ptr, (nuint) array.Length);
					}
				}
				if (count == 0)
					return null;

				Array.Resize (ref array, (int) count);
				return array;
			}
			set {
				unsafe {
					fixed (MTLSamplePosition* valuePtr = value) {
						SetSamplePositions ((IntPtr) valuePtr, (nuint) (value?.Length ?? 0));
					}
				}
			}
		}

		/// <summary>Get the custom sample positions.</summary>
		/// <param name="positions">A pre-allocated array of <see cref="MTLSamplePosition" />s where the returned sample positions will be stored.</param>
		/// <returns>The number of sample positions stored in the array.</returns>
		public nuint GetSamplePositions (MTLSamplePosition []? positions)
		{
			unsafe {
				fixed (MTLSamplePosition* ptr = positions) {
					return GetSamplePositions ((IntPtr) ptr, (nuint) (positions?.Length ?? 0));
				}
			}
		}
	}
}
