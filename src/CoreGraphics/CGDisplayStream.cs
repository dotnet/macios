// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

#nullable enable

#if __MACOS__ || __MACCATALYST__

using CoreFoundation;
using CoreVideo;

namespace CoreGraphics {
	/// <summary>Handles a display stream frame or a change in stream status.</summary>
	/// <param name="status">The frame status.</param>
	/// <param name="displayTime">The frame's display time, in mach absolute time units.</param>
	/// <param name="frameSurface">The frame's pixel buffer, or <see langword="null" /> if no surface is available.</param>
	/// <param name="update">The frame's update metadata, or <see langword="null" /> if the frame is not complete.</param>
	/// <remarks>To use a surface after this callback returns, increment its use count before returning and decrement it when finished.</remarks>
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("maccatalyst")]
	[UnsupportedOSPlatform ("ios")]
	[UnsupportedOSPlatform ("tvos")]
	public delegate void CGDisplayStreamFrameAvailableHandler (CGDisplayStreamFrameStatus status, ulong displayTime, IOSurface.IOSurface? frameSurface, CGDisplayStreamUpdate? update);

	/// <summary>Captures a stream of display frames.</summary>
	/// <remarks>Use ScreenCaptureKit for new applications. Stop the stream and wait for the stopped callback before disposing it.</remarks>
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("maccatalyst")]
	[UnsupportedOSPlatform ("ios")]
	[UnsupportedOSPlatform ("tvos")]
	[ObsoletedOSPlatform ("macos15.0", "Use ScreenCaptureKit instead.")]
	[ObsoletedOSPlatform ("maccatalyst18.0", "Use ScreenCaptureKit instead.")]
	public class CGDisplayStream : NativeObject {
		[Preserve (Conditional = true)]
		internal CGDisplayStream (NativeHandle handle, bool owns)
			: base (handle, owns)
		{
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		unsafe static extern IntPtr CGDisplayStreamCreate (uint display, nuint outputWidth, nuint outputHeight, int pixelFormat, IntPtr properties, BlockLiteral* handler);

		[DllImport (Constants.CoreGraphicsLibrary)]
		unsafe static extern IntPtr CGDisplayStreamCreateWithDispatchQueue (uint display, nuint outputWidth, nuint outputHeight, int pixelFormat, IntPtr properties, IntPtr queue, BlockLiteral* handler);

		/// <summary>Creates a display stream whose frames are delivered on a run loop.</summary>
		/// <param name="display">The display identifier.</param>
		/// <param name="outputWidth">The frame width in pixels; must be greater than zero.</param>
		/// <param name="outputHeight">The frame height in pixels; must be greater than zero.</param>
		/// <param name="pixelFormat">The output pixel format.</param>
		/// <param name="properties">Optional settings using <see cref="CGDisplayStreamKeys" />.</param>
		/// <param name="handler">The frame callback, or <see langword="null" />.</param>
		/// <returns>A display stream, or <see langword="null" /> if creation fails.</returns>
		/// <remarks>Add <see cref="RunLoopSource" /> to a run loop before starting the stream.</remarks>
		public static CGDisplayStream? Create (int display, nuint outputWidth, nuint outputHeight, CVPixelFormatType pixelFormat, NSDictionary? properties, CGDisplayStreamFrameAvailableHandler? handler)
		{
			return CreateNative (display, outputWidth, outputHeight, pixelFormat, properties, null, handler);
		}

		/// <summary>Creates a display stream whose frames are delivered on a dispatch queue.</summary>
		/// <param name="display">The display identifier.</param>
		/// <param name="outputWidth">The frame width in pixels; must be greater than zero.</param>
		/// <param name="outputHeight">The frame height in pixels; must be greater than zero.</param>
		/// <param name="pixelFormat">The output pixel format.</param>
		/// <param name="properties">Optional settings using <see cref="CGDisplayStreamKeys" />.</param>
		/// <param name="queue">The queue on which to deliver frames.</param>
		/// <param name="handler">The frame callback, or <see langword="null" />.</param>
		/// <returns>A display stream, or <see langword="null" /> if creation fails.</returns>
		public static CGDisplayStream? Create (int display, nuint outputWidth, nuint outputHeight, CVPixelFormatType pixelFormat, NSDictionary? properties, DispatchQueue queue, CGDisplayStreamFrameAvailableHandler? handler)
		{
			if (queue is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (queue));
			return CreateNative (display, outputWidth, outputHeight, pixelFormat, properties, queue, handler);
		}

		[BindingImpl (BindingImplOptions.Optimizable)]
		static unsafe CGDisplayStream? CreateNative (int display, nuint outputWidth, nuint outputHeight, CVPixelFormatType pixelFormat, NSDictionary? properties, DispatchQueue? queue, CGDisplayStreamFrameAvailableHandler? handler)
		{
			if (outputWidth == 0)
				throw new ArgumentOutOfRangeException (nameof (outputWidth));
			if (outputHeight == 0)
				throw new ArgumentOutOfRangeException (nameof (outputHeight));

			delegate* unmanaged<IntPtr, CGDisplayStreamFrameStatus, ulong, IntPtr, IntPtr, void> trampoline = &FrameAvailable;
			using var block = handler is null ? default (BlockLiteral) : new BlockLiteral (trampoline, handler, typeof (CGDisplayStream), nameof (FrameAvailable));
			var handle = queue is null
				? CGDisplayStreamCreate ((uint) display, outputWidth, outputHeight, (int) pixelFormat, properties.GetHandle (), handler is null ? null : &block)
				: CGDisplayStreamCreateWithDispatchQueue ((uint) display, outputWidth, outputHeight, (int) pixelFormat, properties.GetHandle (), queue.GetNonNullHandle (nameof (queue)), handler is null ? null : &block);
			GC.KeepAlive (properties);
			GC.KeepAlive (queue);
			return Runtime.GetINativeObject<CGDisplayStream> (handle, owns: true);
		}

		[UnmanagedCallersOnly]
		static void FrameAvailable (IntPtr block, CGDisplayStreamFrameStatus status, ulong displayTime, IntPtr frameSurface, IntPtr update)
		{
			var handler = BlockLiteral.GetTarget<CGDisplayStreamFrameAvailableHandler> (block);
			if (handler is not null)
				handler (status, displayTime, Runtime.GetNSObject<IOSurface.IOSurface> (frameSurface), Runtime.GetINativeObject<CGDisplayStreamUpdate> (update, owns: false));
		}

		/// <summary>Gets the Core Foundation type identifier of a display stream.</summary>
		/// <returns>The display stream type identifier.</returns>
		[DllImport (Constants.CoreGraphicsLibrary, EntryPoint = "CGDisplayStreamGetTypeID")]
		public static extern nint GetTypeId ();

		[DllImport (Constants.CoreGraphicsLibrary)]
		static extern int CGDisplayStreamStart (IntPtr displayStream);

		/// <summary>Starts delivering frames.</summary>
		/// <returns>Zero on success, or a CoreGraphics error code.</returns>
		public int Start () => CGDisplayStreamStart (GetCheckedHandle ());

		[DllImport (Constants.CoreGraphicsLibrary)]
		static extern int CGDisplayStreamStop (IntPtr displayStream);

		/// <summary>Requests that the stream stop delivering frames.</summary>
		/// <returns>Zero on success, or a CoreGraphics error code.</returns>
		/// <remarks>Wait for the callback with <see cref="CGDisplayStreamFrameStatus.Stopped" /> before disposing the stream.</remarks>
		public int Stop () => CGDisplayStreamStop (GetCheckedHandle ());

		[DllImport (Constants.CoreGraphicsLibrary)]
		static extern IntPtr CGDisplayStreamGetRunLoopSource (IntPtr displayStream);

		/// <summary>Gets the stream's run-loop source, or <see langword="null" /> for a dispatch-queue stream.</summary>
		public CFRunLoopSource? RunLoopSource => Runtime.GetINativeObject<CFRunLoopSource> (CGDisplayStreamGetRunLoopSource (GetCheckedHandle ()), owns: false);
	}
}

#endif // __MACOS__ || __MACCATALYST__
