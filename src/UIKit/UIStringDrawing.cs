#if IOS

using System;

using UIKit;
using CoreGraphics;
using Foundation;
using ObjCRuntime;

namespace UIKit {
	public unsafe static partial class UIStringDrawing {
		/// <param name="This">To be added.</param>
		///         <param name="point">To be added.</param>
		///         <param name="font">To be added.</param>
		///         <summary>Developers should use <see cref="UIKit.NSStringDrawing.DrawString(Foundation.NSString,CoreGraphics.CGPoint,UIKit.UIStringAttributes)" /> rather than this deprecated method.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[ObsoletedOSPlatform ("ios", "Use NSString.DrawString(CGPoint, UIStringAttributes) instead.")]
		[ObsoletedOSPlatform ("maccatalyst", "Use NSString.DrawString(CGPoint, UIStringAttributes) instead.")]
		[UnsupportedOSPlatform ("tvos")]
		public static CGSize DrawString (this string This, CGPoint point, UIFont font)
		{
			using (var self = ((NSString) This))
				return self.DrawString (point, font);
		}

		/// <param name="This">To be added.</param>
		/// <param name="point">To be added.</param>
		/// <param name="width">To be added.</param>
		/// <param name="font">To be added.</param>
		/// <param name="breakMode">To be added.</param>
		/// <summary>Developers should use <see cref="UIKit.NSStringDrawing.DrawString(NSString, CoreGraphics.CGRect, UIKit.UIStringAttributes)" /> rather than this deprecated method.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[ObsoletedOSPlatform ("ios", "Use NSString.DrawString(CGRect, UIStringAttributes) instead.")]
		[ObsoletedOSPlatform ("maccatalyst", "Use NSString.DrawString(CGRect, UIStringAttributes) instead.")]
		[UnsupportedOSPlatform ("tvos")]
		public static CGSize DrawString (this string This, CGPoint point, nfloat width, UIFont font, UILineBreakMode breakMode)
		{
			using (var self = ((NSString) This))
				return self.DrawString (point, width, font, breakMode);
		}

		/// <param name="This">To be added.</param>
		/// <param name="point">To be added.</param>
		/// <param name="width">To be added.</param>
		/// <param name="font">To be added.</param>
		/// <param name="fontSize">To be added.</param>
		/// <param name="breakMode">To be added.</param>
		/// <param name="adjustment">To be added.</param>
		/// <summary>Developers should use <see cref="UIKit.NSStringDrawing.DrawString(NSString, CoreGraphics.CGRect, UIKit.UIStringAttributes)" /> rather than this deprecated method..</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[ObsoletedOSPlatform ("ios", "Use NSString.DrawString(CGRect, UIStringAttributes) instead.")]
		[ObsoletedOSPlatform ("maccatalyst", "Use NSString.DrawString(CGRect, UIStringAttributes) instead.")]
		[UnsupportedOSPlatform ("tvos")]
		public static CGSize DrawString (this string This, CGPoint point, nfloat width, UIFont font, nfloat fontSize, UILineBreakMode breakMode, UIBaselineAdjustment adjustment)
		{
			using (var self = ((NSString) This))
				return self.DrawString (point, width, font, fontSize, breakMode, adjustment);
		}

		/// <param name="This">To be added.</param>
		/// <param name="point">To be added.</param>
		/// <param name="width">To be added.</param>
		/// <param name="font">To be added.</param>
		/// <param name="minFontSize">To be added.</param>
		/// <param name="actualFontSize">To be added.</param>
		/// <param name="breakMode">To be added.</param>
		/// <param name="adjustment">To be added.</param>
		/// <summary>Developers should use <see cref="UIKit.NSStringDrawing.DrawString(NSString, CoreGraphics.CGRect, UIKit.UIStringAttributes)" /> rather than this deprecated method.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[ObsoletedOSPlatform ("ios", "Use NSString.DrawString(CGRect, UIStringAttributes) instead.")]
		[ObsoletedOSPlatform ("maccatalyst", "Use NSString.DrawString(CGRect, UIStringAttributes) instead.")]
		[UnsupportedOSPlatform ("tvos")]
		public static CGSize DrawString (this string This, CGPoint point, nfloat width, UIFont font, nfloat minFontSize, ref nfloat actualFontSize, UILineBreakMode breakMode, UIBaselineAdjustment adjustment)
		{
			using (var self = ((NSString) This))
				return self.DrawString (point, width, font, minFontSize, ref actualFontSize, breakMode, adjustment);
		}

		/// <param name="This">To be added.</param>
		///         <param name="rect">To be added.</param>
		///         <param name="font">To be added.</param>
		///         <summary>Developers should use <see cref="UIKit.NSStringDrawing.DrawString(NSString, CoreGraphics.CGRect, UIKit.UIStringAttributes)" /> rather than this deprecated method.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[ObsoletedOSPlatform ("ios", "Use NSString.DrawString(CGRect, UIStringAttributes) instead.")]
		[ObsoletedOSPlatform ("maccatalyst", "Use NSString.DrawString(CGRect, UIStringAttributes) instead.")]
		[UnsupportedOSPlatform ("tvos")]
		public static CGSize DrawString (this string This, CGRect rect, UIFont font)
		{
			using (var self = ((NSString) This))
				return self.DrawString (rect, font);
		}

		/// <param name="This">To be added.</param>
		///         <param name="rect">To be added.</param>
		///         <param name="font">To be added.</param>
		///         <param name="mode">To be added.</param>
		///         <summary>Developers should use <see cref="UIKit.NSStringDrawing.DrawString(NSString, CoreGraphics.CGRect, UIKit.UIStringAttributes)" /> rather than this deprecated method.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[ObsoletedOSPlatform ("ios", "Use NSString.DrawString(CGRect, UIStringAttributes) instead.")]
		[ObsoletedOSPlatform ("maccatalyst", "Use NSString.DrawString(CGRect, UIStringAttributes) instead.")]
		[UnsupportedOSPlatform ("tvos")]
		public static CGSize DrawString (this string This, CGRect rect, UIFont font, UILineBreakMode mode)
		{
			using (var self = ((NSString) This))
				return self.DrawString (rect, font, mode);
		}

		/// <param name="This">To be added.</param>
		///         <param name="rect">To be added.</param>
		///         <param name="font">To be added.</param>
		///         <param name="mode">To be added.</param>
		///         <param name="alignment">To be added.</param>
		///         <summary>Developers should use <see cref="UIKit.NSStringDrawing.DrawString(NSString, CoreGraphics.CGRect, UIKit.UIStringAttributes)" /> rather than this deprecated method.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[ObsoletedOSPlatform ("ios", "Use NSString.DrawString(CGRect, UIStringAttributes) instead.")]
		[ObsoletedOSPlatform ("maccatalyst", "Use NSString.DrawString(CGRect, UIStringAttributes) instead.")]
		[UnsupportedOSPlatform ("tvos")]
		public static CGSize DrawString (this string This, CGRect rect, UIFont font, UILineBreakMode mode, UITextAlignment alignment)
		{
			using (var self = ((NSString) This))
				return self.DrawString (rect, font, mode, alignment);
		}

		/// <param name="This">The instance on which this method operates.</param>
		///         <param name="font">To be added.</param>
		///         <summary>Developers should use <see cref="UIKit.NSStringDrawing.GetSizeUsingAttributes(Foundation.NSString,UIKit.UIStringAttributes)" /> rather than this deprecated method.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[ObsoletedOSPlatform ("ios", "Use NSString.GetSizeUsingAttributes(UIStringAttributes) instead.")]
		[ObsoletedOSPlatform ("maccatalyst", "Use NSString.GetSizeUsingAttributes(UIStringAttributes) instead.")]
		[UnsupportedOSPlatform ("tvos")]
		public static CGSize StringSize (this string This, UIFont font)
		{
			using (var self = ((NSString) This))
				return self.StringSize (font);
		}

		/// <param name="This">To be added.</param>
		/// <param name="font">To be added.</param>
		/// <param name="forWidth">To be added.</param>
		/// <param name="breakMode">To be added.</param>
		/// <summary>Gets the <see cref="CoreGraphics.CGSize" /> necessary to display this <see cref="Foundation.NSString" />.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[ObsoletedOSPlatform ("ios", "Use NSString.GetBoundingRect (CGSize, NSStringDrawingOptions, UIStringAttributes, NSStringDrawingContext) instead.")]
		[ObsoletedOSPlatform ("maccatalyst", "Use NSString.GetBoundingRect (CGSize, NSStringDrawingOptions, UIStringAttributes, NSStringDrawingContext) instead.")]
		[UnsupportedOSPlatform ("tvos")]
		public static CGSize StringSize (this string This, UIFont font, nfloat forWidth, UILineBreakMode breakMode)
		{
			using (var self = ((NSString) This))
				return self.StringSize (font, forWidth, breakMode);
		}

		/// <param name="This">The instance on which this method operates.</param>
		///         <param name="font">To be added.</param>
		///         <param name="constrainedToSize">To be added.</param>
		///         <summary>The calculated size of the string if rendered with the <paramref name="font" /> or <paramref name="constrainedToSize" />, whichever is smaller.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[ObsoletedOSPlatform ("ios", "Use NSString.GetBoundingRect (CGSize, NSStringDrawingOptions, UIStringAttributes, NSStringDrawingContext) instead.")]
		[ObsoletedOSPlatform ("maccatalyst", "Use NSString.GetBoundingRect (CGSize, NSStringDrawingOptions, UIStringAttributes, NSStringDrawingContext) instead.")]
		[UnsupportedOSPlatform ("tvos")]
		public static CGSize StringSize (this string This, UIFont font, CGSize constrainedToSize)
		{
			using (var self = ((NSString) This))
				return self.StringSize (font, constrainedToSize);
		}

		/// <param name="This">To be added.</param>
		///         <param name="font">To be added.</param>
		///         <param name="constrainedToSize">To be added.</param>
		///         <param name="lineBreakMode">To be added.</param>
		///         <summary>Gets the <see cref="CoreGraphics.CGSize" /> necessary to display this <see cref="Foundation.NSString" />.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[ObsoletedOSPlatform ("ios", "Use NSString.GetBoundingRect (CGSize, NSStringDrawingOptions, UIStringAttributes, NSStringDrawingContext) instead.")]
		[ObsoletedOSPlatform ("maccatalyst", "Use NSString.GetBoundingRect (CGSize, NSStringDrawingOptions, UIStringAttributes, NSStringDrawingContext) instead.")]
		[UnsupportedOSPlatform ("tvos")]
		public static CGSize StringSize (this string This, UIFont font, CGSize constrainedToSize, UILineBreakMode lineBreakMode)
		{
			using (var self = ((NSString) This))
				return self.StringSize (font, constrainedToSize, lineBreakMode);
		}

		/// <param name="This">To be added.</param>
		/// <param name="font">To be added.</param>
		/// <param name="minFontSize">To be added.</param>
		/// <param name="actualFontSize">To be added.</param>
		/// <param name="forWidth">To be added.</param>
		/// <param name="lineBreakMode">To be added.</param>
		/// <summary>Gets the <see cref="CoreGraphics.CGSize" /> necessary to display this <see cref="Foundation.NSString" />.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[ObsoletedOSPlatform ("ios")]
		[ObsoletedOSPlatform ("maccatalyst")]
		[UnsupportedOSPlatform ("tvos")]
		public static CGSize StringSize (this string This, UIFont font, nfloat minFontSize, ref nfloat actualFontSize, nfloat forWidth, UILineBreakMode lineBreakMode)
		{
			using (var self = ((NSString) This))
				return self.StringSize (font, minFontSize, ref actualFontSize, forWidth, lineBreakMode);
		}
	}
}

#endif // IOS
