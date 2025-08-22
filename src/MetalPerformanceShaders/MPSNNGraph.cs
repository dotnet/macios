// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

#nullable enable

using CoreGraphics;
using Metal;

namespace MetalPerformanceShaders {
	public partial class MPSNNGraph {
		/// <summary>Create a new <see cref="MPSNNGraph" /> instance.</summary>
		/// <param name="device">The device where the filter will run.</param>
		/// <param name="resultImages">The last images in the graph.</param>
		/// <param name="resultsAreNeeded">An array of boolean for each element in the <paramref name="resultImages" /> array.</param>
		/// <returns>A new <see cref="MPSNNGraph" /> if successful, <see langword="null" /> otherwise.</returns>
		[SupportedOSPlatform ("tvos13.0")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios13.0")]
		[SupportedOSPlatform ("maccatalyst")]
		public unsafe static MPSNNGraph? Create (IMTLDevice device, MPSNNImageNode [] resultImages, bool []? resultsAreNeeded)
		{
			ValidateParameters (resultImages, resultsAreNeeded, out var results);

			unsafe {
				fixed (void* resultsPointer = results)
					return Create (device, resultImages, (IntPtr) resultsPointer);
			}
		}

		/// <summary>Create a new <see cref="MPSNNGraph" /> instance.</summary>
		/// <param name="device">The device where the filter will run.</param>
		/// <param name="resultImages">The last images in the graph.</param>
		/// <param name="resultsAreNeeded">An array of boolean for each element in the <paramref name="resultImages" /> array.</param>
		[SupportedOSPlatform ("tvos13.0")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios13.0")]
		[SupportedOSPlatform ("maccatalyst")]
		public MPSNNGraph (IMTLDevice device, MPSNNImageNode [] resultImages, bool []? resultsAreNeeded)
			: base (NSObjectFlag.Empty)
		{
			ValidateParameters (resultImages, resultsAreNeeded, out var results);

			unsafe {
				fixed (void* resultsPointer = results)
					InitializeHandle (_InitWithDevice (device, resultImages, (IntPtr) resultsPointer), "initWithDevice:resultImages:resultsAreNeeded:");
			}
		}

		static void ValidateParameters (MPSNNImageNode [] resultImages, bool []? resultsAreNeeded, out byte []? results)
		{
			if (resultImages is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (resultImages));

			if (resultsAreNeeded is not null && resultsAreNeeded.Length < resultImages.Length)
				ObjCRuntime.ThrowHelper.ThrowArgumentOutOfRangeException (nameof (resultsAreNeeded), "The length of 'resultsAreNeeded' must be at least as much as the length of 'resultImages' (or null).");

			results = null;
			if (resultsAreNeeded is not null) {
				results = new byte [resultsAreNeeded.Length];
				for (var i = 0; i < resultsAreNeeded.Length; i++)
					results [i] = resultsAreNeeded [i].AsByte ();
			}
		}
	}
}
