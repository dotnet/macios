#if !TVOS
using System;
using System.Runtime.InteropServices;
using Foundation;
using ObjCRuntime;

#nullable enable

namespace Metal {
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	// add some extension methods to make the API of the protocol nicer
	public static class MTLIntersectionFunctionTableExtensions {
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios14.0")]
		[SupportedOSPlatform ("maccatalyst")]
		[UnsupportedOSPlatform ("tvos")]
		public static void SetBuffers (this IMTLIntersectionFunctionTable table, IMTLBuffer [] buffers, nuint [] offsets, NSRange range)
		{
			if (buffers is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (buffers));
			if (offsets is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (offsets));

			var bufferPtrArray = buffers.Length <= 1024 ? stackalloc IntPtr [buffers.Length] : new IntPtr [buffers.Length];
			// get all intptr from the array to pass to the lower level call
			for (var i = 0; i < buffers.Length; i++) {
				bufferPtrArray [i] = buffers [i].Handle;
			}

			unsafe {
				fixed (void* buffersPtr = bufferPtrArray)
				fixed (void* offsetsPtr = offsets) { // can use fixed
					table.SetBuffers ((IntPtr) buffersPtr, (IntPtr) offsetsPtr, range);
				}
			}
			GC.KeepAlive (buffers);
		}
	}
}
#endif
