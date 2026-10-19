// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

#nullable enable

#if __MACOS__ || __MACCATALYST__

using CoreFoundation;

namespace CoreGraphics {
	/// <summary>Describes changes between display stream frames.</summary>
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("maccatalyst")]
	[UnsupportedOSPlatform ("ios")]
	[UnsupportedOSPlatform ("tvos")]
	[ObsoletedOSPlatform ("macos15.0", "Use ScreenCaptureKit instead.")]
	[ObsoletedOSPlatform ("maccatalyst18.0", "Use ScreenCaptureKit instead.")]
	public class CGDisplayStreamUpdate : NativeObject {
		[Preserve (Conditional = true)]
		internal CGDisplayStreamUpdate (NativeHandle handle, bool owns)
			: base (handle, owns)
		{
		}

		/// <summary>Gets the Core Foundation type identifier of display stream update metadata.</summary>
		/// <returns>The display stream update type identifier.</returns>
		[DllImport (Constants.CoreGraphicsLibrary, EntryPoint = "CGDisplayStreamUpdateGetTypeID")]
		public static extern nint GetTypeId ();

		[DllImport (Constants.CoreGraphicsLibrary)]
		unsafe static extern CGRect* CGDisplayStreamUpdateGetRects (IntPtr update, CGDisplayStreamUpdateRectType rectType, out nuint rectCount);

		/// <summary>Copies the rectangles describing the specified kind of frame change.</summary>
		/// <param name="rectType">The kind of rectangles to retrieve.</param>
		/// <returns>A managed copy of the update rectangles.</returns>
		public unsafe CGRect [] GetRects (CGDisplayStreamUpdateRectType rectType)
		{
			var rects = CGDisplayStreamUpdateGetRects (GetCheckedHandle (), rectType, out var count);
			var result = new CGRect [checked ((int) count)];
			for (var i = 0; i < result.Length; i++)
				result [i] = rects [i];
			return result;
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		static extern IntPtr CGDisplayStreamUpdateCreateMergedUpdate (IntPtr firstUpdate, IntPtr secondUpdate);

		/// <summary>Merges changes from two successive frame updates.</summary>
		/// <param name="firstUpdate">The earlier update, or <see langword="null" />.</param>
		/// <param name="secondUpdate">The later update, or <see langword="null" />.</param>
		/// <returns>The merged update, or <see langword="null" /> if no update is available.</returns>
		public static CGDisplayStreamUpdate? CreateMergedUpdate (CGDisplayStreamUpdate? firstUpdate, CGDisplayStreamUpdate? secondUpdate)
		{
			var handle = CGDisplayStreamUpdateCreateMergedUpdate (firstUpdate.GetHandle (), secondUpdate.GetHandle ());
			GC.KeepAlive (firstUpdate);
			GC.KeepAlive (secondUpdate);
			return Runtime.GetINativeObject<CGDisplayStreamUpdate> (handle, owns: true);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		static extern void CGDisplayStreamUpdateGetMovedRectsDelta (IntPtr update, out nfloat dx, out nfloat dy);

		/// <summary>Gets the offset from moved rectangles to their original locations.</summary>
		/// <param name="dx">The horizontal offset.</param>
		/// <param name="dy">The vertical offset.</param>
		public void GetMovedRectsDelta (out nfloat dx, out nfloat dy)
		{
			CGDisplayStreamUpdateGetMovedRectsDelta (GetCheckedHandle (), out dx, out dy);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		static extern nuint CGDisplayStreamUpdateGetDropCount (IntPtr update);

		/// <summary>Gets the number of frames dropped since the previous callback.</summary>
		public nuint DropCount => CGDisplayStreamUpdateGetDropCount (GetCheckedHandle ());
	}
}

#endif // __MACOS__ || __MACCATALYST__
