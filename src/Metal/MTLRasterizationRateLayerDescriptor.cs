using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

using Foundation;
using ObjCRuntime;

#nullable enable

namespace Metal {
	public partial class MTLRasterizationRateLayerDescriptor {
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("ios13.0")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos16.0")]
#if XAMCORE_5_0
		public float [] HorizontalSampleStorage {
#else
		public double [] HorizontalSampleStorage {
#endif
			get {
				var width = (int) SampleCount.Width;
				var floatArray = new float [width];
				Marshal.Copy (_HorizontalSampleStorage, floatArray, 0, width);
#if XAMCORE_5_0
				return floatArray;
#else
				return Array.ConvertAll (floatArray, v => (double) v);
#endif
			}
		}

		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("ios13.0")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos16.0")]
#if XAMCORE_5_0
		public float [] VerticalSampleStorage {
#else
		public double [] VerticalSampleStorage {
#endif
			get {
				var height = (int) SampleCount.Height;
				var floatArray = new float [height];
				Marshal.Copy (_VerticalSampleStorage, floatArray, 0, height);
#if XAMCORE_5_0
				return floatArray;
#else
				return Array.ConvertAll (floatArray, v => (double) v);
#endif
			}
		}

		/// <summary>Create a new <see cref="MTLRasterizationRateLayerDescriptor" /> instance with the specified rasterization rates.</summary>
		/// <param name="sampleCount">The number of horizontal (<see cref="MTLSize.Width" />) and vertical (<see cref="MTLSize.Height" />) rasterization rates. The depth component (<see cref="MTLSize.Depth" />) is ignored.</param>
		/// <param name="horizontal">The horizontal rasterization rates for the new <see cref="MTLRasterizationRateLayerDescriptor" /> instance.</param>
		/// <param name="vertical">The vertical rasterization rates for the new <see cref="MTLRasterizationRateLayerDescriptor" /> instance.</param>
		/// <returns>A new <see cref="MTLRasterizationRateLayerDescriptor" /> instance with the specified rasterization rates.</returns>
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("ios13.0")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos16.0")]
		static public MTLRasterizationRateLayerDescriptor Create (MTLSize sampleCount, float [] horizontal, float [] vertical)
		{
			if (horizontal is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (horizontal));
			if (vertical is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (vertical));
			if (sampleCount.Width != horizontal.Length)
				throw new ArgumentOutOfRangeException ("Horizontal length should be equal to the sampleCount.Width.");
			if (sampleCount.Height != vertical.Length)
				throw new ArgumentOutOfRangeException ("Vertical length should be equal to the sampleCount.Height.");

			unsafe {
				fixed (void* horizontalHandle = horizontal)
				fixed (void* verticalHandle = vertical) {
					return new MTLRasterizationRateLayerDescriptor (sampleCount, (IntPtr) horizontalHandle, (IntPtr) verticalHandle);
				}
			}
		}

		/// <summary>Create a new <see cref="MTLRasterizationRateLayerDescriptor" /> instance with the specified rasterization rates.</summary>
		/// <param name="horizontal">The horizontal rasterization rates for the new <see cref="MTLRasterizationRateLayerDescriptor" /> instance.</param>
		/// <param name="vertical">The vertical rasterization rates for the new <see cref="MTLRasterizationRateLayerDescriptor" /> instance.</param>
		/// <returns>A new <see cref="MTLRasterizationRateLayerDescriptor" /> instance with the specified rasterization rates.</returns>
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("ios13.0")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos16.0")]
		public static MTLRasterizationRateLayerDescriptor Create (float [] horizontal, float [] vertical)
		{
			if (horizontal is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (horizontal));
			if (vertical is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (vertical));

			return Create (new MTLSize (horizontal.Length, vertical.Length, 0), horizontal, vertical);
		}
	}
}
