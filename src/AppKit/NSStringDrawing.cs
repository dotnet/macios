#if !__MACCATALYST__
using System;
using System.Runtime.Versioning;
using AppKit;
using Foundation;
using CoreGraphics;

#nullable enable

namespace AppKit {
	/// <summary>To be added.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("macos")]
	[UnsupportedOSPlatform ("ios")]
	[UnsupportedOSPlatform ("tvos")]
	[UnsupportedOSPlatform ("maccatalyst")]
	// Manual bindings, easier than make the generator support extension methods on non-NSObject-derived types (string in this case).
	public unsafe static partial class NSStringDrawing {
		/// <param name="This">To be added.</param>
		///         <param name="point">To be added.</param>
		///         <param name="attributes">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public static void DrawAtPoint (this string This, CGPoint point, NSDictionary? attributes)
		{
			using (var self = ((NSString) This))
				self.DrawAtPoint (point, attributes);
		}

		/// <param name="This">To be added.</param>
		///         <param name="point">To be added.</param>
		///         <param name="attributes">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public static void DrawAtPoint (this string This, CGPoint point, NSStringAttributes? attributes)
		{
			This.DrawAtPoint (point, attributes is null ? null : attributes.Dictionary);
		}

		/// <param name="This">To be added.</param>
		///         <param name="rect">To be added.</param>
		///         <param name="attributes">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public static void DrawInRect (this string This, CGRect rect, NSDictionary? attributes)
		{
			using (var self = ((NSString) This))
				self.DrawInRect (rect, attributes);
		}

		/// <param name="This">To be added.</param>
		///         <param name="rect">To be added.</param>
		///         <param name="attributes">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public static void DrawInRect (this string This, CGRect rect, NSStringAttributes? attributes)
		{
			This.DrawInRect (rect, attributes is null ? null : attributes.Dictionary);
		}

		/// <param name="This">To be added.</param>
		///         <param name="attributes">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static CGSize StringSize (this string This, NSDictionary? attributes)
		{
			using (var self = ((NSString) This))
				return self.StringSize (attributes);
		}

		/// <param name="This">To be added.</param>
		///         <param name="attributes">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static CGSize StringSize (this string This, NSStringAttributes? attributes)
		{
			return This.StringSize (attributes is null ? null : attributes.Dictionary);
		}
	}
}
#endif // !__MACCATALYST__
