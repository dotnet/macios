// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

#nullable enable


using System;

using Foundation;
using Metal;
using ObjCRuntime;

namespace MetalPerformanceShaders {
	partial class MPSImageCanny {
		/// <summary>Create a new <see cref="MPSImageCanny" /> instance.</summary>
		/// <param name="device">The device where the filter will run.</param>
		/// <param name="transform">An array of 3 floats that describe how to transform rgb pixels to monochrome pixels.</param>
		/// <param name="sigma">The standard deviation of a gaussian blur filter.</param>
		public MPSImageCanny (IMTLDevice device, float [] transform, float sigma)
			: this (NSObjectFlag.Empty)
		{
			if (transform is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (transform));
			if (transform.Length < 3)
				ObjCRuntime.ThrowHelper.ThrowArgumentOutOfRangeException (nameof (transform), $"Length must be at least '3'.");

			unsafe {
				fixed (float* transformPtr = transform) {
					InitializeHandle (_InitWithDevice (device, (IntPtr) transformPtr, sigma), "initWithDevice:linearToGrayScaleTransform:sigma:");
				}
			}
		}

		/// <summary>Get an array with the 3 floats values that describe how to transform rgb pixels to monochrome pixels.</summary>
		public float [] ColorTransform {
			get {
				var rv = new float [3];
				unsafe {
					fixed (float* ptr = rv)
						NativeMemory.Copy ((void*) WeakColorTransform, ptr, (nuint) (sizeof (float) * rv.Length));
				}
				return rv;
			}
		}
	}
}
