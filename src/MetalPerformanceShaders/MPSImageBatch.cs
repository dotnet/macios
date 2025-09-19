//
// MPSImageBatch.cs
//
// Authors:
//	Alex Soto (alexsoto@microsoft.com)
//
// Copyright 2019 Microsoft Corporation.
//

#nullable enable

using System;
using System.Runtime.InteropServices;
using ObjCRuntime;
using Foundation;
using Metal;

namespace MetalPerformanceShaders {
	/// <summary>To be added.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("tvos")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("maccatalyst")]
	public static partial class MPSImageBatch {

		[DllImport (Constants.MetalPerformanceShadersLibrary)]
		static extern nuint MPSImageBatchIncrementReadCount (IntPtr batch, nint amount);

		// Using 'NSArray<MPSImage>' instead of `MPSImage[]` because image array 'Handle' matters.
		public static nuint IncrementReadCount (NSArray<MPSImage> imageBatch, nint amount)
		{
			if (imageBatch is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (imageBatch));

			nuint count = MPSImageBatchIncrementReadCount (imageBatch.Handle, amount);
			GC.KeepAlive (imageBatch);
			return count;
		}

		[DllImport (Constants.MetalPerformanceShadersLibrary)]
		static extern void MPSImageBatchSynchronize (IntPtr batch, IntPtr /* id<MTLCommandBuffer> */ cmdBuf);

		// Using 'NSArray<MPSImage>' instead of `MPSImage[]` because image array 'Handle' matters.
		/// <param name="imageBatch">To be added.</param>
		///         <param name="commandBuffer">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public static void Synchronize (NSArray<MPSImage> imageBatch, IMTLCommandBuffer commandBuffer)
		{
			if (imageBatch is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (imageBatch));
			if (commandBuffer is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (commandBuffer));

			MPSImageBatchSynchronize (imageBatch.Handle, commandBuffer.Handle);
			GC.KeepAlive (imageBatch);
			GC.KeepAlive (commandBuffer);
		}

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[DllImport (Constants.MetalPerformanceShadersLibrary)]
		static extern nuint MPSImageBatchResourceSize (IntPtr batch);

		// Using 'NSArray<MPSImage>' instead of `MPSImage[]` because image array 'Handle' matters.
		/// <param name="imageBatch">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		public static nuint GetResourceSize (NSArray<MPSImage> imageBatch)
		{
			if (imageBatch is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (imageBatch));

			nuint size = MPSImageBatchResourceSize (imageBatch.Handle);
			GC.KeepAlive (imageBatch);
			return size;
		}

		[DllImport (Constants.MetalPerformanceShadersLibrary)]
		unsafe static extern nint MPSImageBatchIterate (IntPtr batch, BlockLiteral* iterator);

		public delegate nint MPSImageBatchIterator (MPSImage image, nuint index);

		[UnmanagedCallersOnly]
		static unsafe nint InvokeIterator (IntPtr block, IntPtr image, nuint index)
		{
			var del = (MPSImageBatchIterator) BlockLiteral.GetTarget<MPSImageBatchIterator> (block);
			if (del is not null) {
				using var img = Runtime.GetNSObject<MPSImage> (image)!;
				return del (img, index);
			}
			return 0;
		}

		/// <summary>Iterate over the unique images in the image batch.</summary>
		/// <param name="imageBatch">The batch of images to iterate over.</param>
		/// <param name="iterator">The callback to call for each unique image.</param>
		/// <returns>The value returned by the callback for the last image iterated over.</returns>
		[BindingImpl (BindingImplOptions.Optimizable)]
		public static nint Iterate (NSArray<MPSImage> imageBatch, MPSImageBatchIterator iterator)
		{
			if (iterator is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (iterator));

			unsafe {
				delegate* unmanaged<IntPtr, IntPtr, nuint, nint> trampoline = &InvokeIterator;
				using var block = new BlockLiteral (trampoline, iterator, typeof (MPSImageBatch), nameof (InvokeIterator));
				var rv = MPSImageBatchIterate (imageBatch.GetNonNullHandle (nameof (imageBatch)), &block);
				GC.KeepAlive (imageBatch);
				return rv;
			}
		}
	}
}
