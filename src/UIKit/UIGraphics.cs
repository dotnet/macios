//
// UIGraphics.cs: Bindings to some P/Invoke APIs to interface with CoreGraphics
//
// Authors:
//   Miguel de Icaza (miguel@novell.com)
//
// Copyright 2009 Novell, inc.
// Copyright 2012-2014 Xamarin Inc
//
using System.Runtime.InteropServices;
using CoreGraphics;
using Foundation;
using ObjCRuntime;
using System;
using System.ComponentModel;

// Disable until we get around to enable + fix any issues.
#nullable disable

namespace UIKit {

#if XAMCORE_5_0
#error All PDF public name instances in this file need to be turned into Pdf. e.g. EndPDFContext into EndPdfContext.
#endif

	/// <summary>Helper methods to paint on the screen, PDF context or bitmaps.</summary>
	///     <remarks>
	///       <para>Methods in this class generally correspond to <c>UIGraphics*</c> and <c>UIRect*</c> methods in Apple's UIKit Framework.</para>
	///     </remarks>
	public static class UIGraphics {
		[DllImport (Constants.UIKitLibrary)]
		extern static IntPtr UIGraphicsGetCurrentContext ();

		[DllImport (Constants.UIKitLibrary)]
		extern static void UIGraphicsPushContext (IntPtr ctx);

		[DllImport (Constants.UIKitLibrary)]
		extern static void UIGraphicsPopContext ();

		/// <param name="rect">To be added.</param>
		///         <param name="blendMode">To be added.</param>
		///         <summary>Fills <paramref name="rect" /> with the current fill color, using <paramref name="blendMode" />.</summary>
		///         <remarks>
		///           <para>Developers can call this method from any thread.</para>
		///         </remarks>
		[DllImport (Constants.UIKitLibrary, EntryPoint = "UIRectFillUsingBlendMode")]
		public extern static void RectFillUsingBlendMode (CGRect rect, CGBlendMode blendMode);

		/// <param name="rect">The region to fill.</param>
		///         <summary>Fills a rectangle with the current color on the current context.</summary>
		///         <remarks>
		///           <para>Developers can call this method from any thread.</para>
		///         </remarks>
		[DllImport (Constants.UIKitLibrary, EntryPoint = "UIRectFill")]
		public extern static void RectFill (CGRect rect);

		/// <param name="rect">To be added.</param>
		///         <param name="blendMode">To be added.</param>
		///         <summary>Draws a frame inside the specified rectangle and blending it with <paramref name="blendMode" />.</summary>
		///         <remarks>To be added.</remarks>
		[DllImport (Constants.UIKitLibrary, EntryPoint = "UIRectFrameUsingBlendMode")]
		public extern static void RectFrameUsingBlendMode (CGRect rect, CGBlendMode blendMode);

		/// <param name="rect">Region where the frame will be drawn.</param>
		///         <summary>Draws a frame inside the specified rectangle.</summary>
		///         <remarks>.</remarks>
		[DllImport (Constants.UIKitLibrary, EntryPoint = "UIRectFrame")]
		public extern static void RectFrame (CGRect rect);

		/// <param name="rect">New clipping path.</param>
		///         <summary>Intersects the current clipping path with the specified rectangle.</summary>
		///         <remarks>
		///         </remarks>
		[DllImport (Constants.UIKitLibrary, EntryPoint = "UIRectClip")]
		public extern static void RectClip (CGRect rect);

#if NET
		/// <param name="size">Size of the image context.</param>
		///         <summary>Pushes a new image context and makes it the current graphics context.</summary>
		///         <remarks>
		///           <para>UIKit keeps a stack of image context, this method creates a new image context, makes it the default and places it at the top of the graphic context stacks.</para>
		///           <para>To restore the previous graphics context, call the <see cref="M:UIKit.UIGraphics.EndImageContext" /> method.</para>
		///           <para>You can get the current context by calling the <see cref="M:UIKit.UIGraphics.GetImageFromCurrentImageContext" /> method.</para>
		///           <para>Developers can call this method from any thread.</para>
		///         </remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("maccatalyst")]
		[UnsupportedOSPlatform ("ios17.0")]
		[UnsupportedOSPlatform ("tvos17.0")]
		[UnsupportedOSPlatform ("maccatalyst17.0")]
#else
		[Deprecated (PlatformName.iOS, 17, 0)]
		[Deprecated (PlatformName.TvOS, 17, 0)]
#endif
		[DllImport (Constants.UIKitLibrary, EntryPoint = "UIGraphicsBeginImageContext")]
		public extern static void BeginImageContext (CGSize size);

#if NET
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("maccatalyst")]
		[UnsupportedOSPlatform ("ios17.0")]
		[UnsupportedOSPlatform ("tvos17.0")]
		[UnsupportedOSPlatform ("maccatalyst17.0")]
#else
		[Deprecated (PlatformName.iOS, 17, 0)]
		[Deprecated (PlatformName.TvOS, 17, 0)]
#endif
		[DllImport (Constants.UIKitLibrary, EntryPoint = "UIGraphicsBeginImageContextWithOptions")]
		extern static void BeginImageContextWithOptions (CGSize size, byte opaque, nfloat scale);

#if NET
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("maccatalyst")]
		[UnsupportedOSPlatform ("ios17.0")]
		[UnsupportedOSPlatform ("tvos17.0")]
		[UnsupportedOSPlatform ("maccatalyst17.0")]
#else
		[Deprecated (PlatformName.iOS, 17, 0)]
		[Deprecated (PlatformName.TvOS, 17, 0)]
#endif
		public static void BeginImageContextWithOptions (CGSize size, bool opaque, nfloat scale)
		{
			BeginImageContextWithOptions (size, opaque ? (byte) 1 : (byte) 0, scale);
		}

#if NET
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("maccatalyst")]
		[UnsupportedOSPlatform ("ios17.0")]
		[UnsupportedOSPlatform ("tvos17.0")]
		[UnsupportedOSPlatform ("maccatalyst17.0")]
#else
		[Deprecated (PlatformName.iOS, 17, 0)]
		[Deprecated (PlatformName.TvOS, 17, 0)]
#endif
		[DllImport (Constants.UIKitLibrary)]
		static extern IntPtr UIGraphicsGetImageFromCurrentImageContext ();

#if NET
		/// <summary>Pops the current image context.</summary>
		///         <remarks>
		///           <para>UIKit keeps a stack of image context, this method pops the current image context, and makes the new context at the top of the stack, the new default context.</para>
		///           <para>If the current context was not created using the <see cref="M:UIKit.UIGraphics.BeginImageContextWithOptions(CoreGraphics.CGSize,System.Boolean,System.nfloat)" />
		/// 	  or <see cref="M:UIKit.UIGraphics.BeginImageContext(CoreGraphics.CGSize)" />
		/// 	  this method does nothing.</para>
		///         </remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("maccatalyst")]
		[UnsupportedOSPlatform ("ios17.0")]
		[UnsupportedOSPlatform ("tvos17.0")]
		[UnsupportedOSPlatform ("maccatalyst17.0")]
#else
		[Deprecated (PlatformName.iOS, 17, 0)]
		[Deprecated (PlatformName.TvOS, 17, 0)]
#endif
		[DllImport (Constants.UIKitLibrary, EntryPoint = "UIGraphicsEndImageContext")]
		public extern static void EndImageContext ();

		[DllImport (Constants.UIKitLibrary)]
		extern static void UIGraphicsAddPDFContextDestinationAtPoint (IntPtr str, CGPoint point);

		/// <param name="str">Name of destination point.</param>
		///         <param name="point">A point in the current context.</param>
		///         <summary>Adds a PDF destination with the given name at the given position.</summary>
		///         <remarks>Only valid if the current graphics context is a PDF context</remarks>
		public static void AddPDFContextDestination (string str, CGPoint point)
		{
			using (var nsstr = new NSString (str))
				UIGraphicsAddPDFContextDestinationAtPoint (nsstr.Handle, point);
		}

		[DllImport (Constants.UIKitLibrary)]
		extern static void UIGraphicsSetPDFContextDestinationForRect (IntPtr str, CGRect rect);

		/// <param name="str">To be added.</param>
		///         <param name="rect">To be added.</param>
		///         <summary>Sets the PDF destination with the given name at the given position.</summary>
		///         <remarks>To be added.</remarks>
		public static void SetPDFContextDestination (string str, CGRect rect)
		{
			using (var nsstr = new NSString (str))
				UIGraphicsSetPDFContextDestinationForRect (nsstr.Handle, rect);
		}

		[DllImport (Constants.UIKitLibrary)]
		extern static CGRect UIGraphicsGetPDFContextBounds ();

		/// <summary>Return the current bounds for the PDF page.</summary>
		///         <value>
		///         </value>
		///         <remarks>
		///         </remarks>
		public static CGRect PDFContextBounds {
			get {
				return UIGraphicsGetPDFContextBounds ();
			}
		}

		[DllImport (Constants.UIKitLibrary)]
		extern static void UIGraphicsSetPDFContextURLForRect (IntPtr url, CGRect rect);

		/// <param name="url">To be added.</param>
		///         <param name="rect">To be added.</param>
		///         <summary>Links the url to the specified rectangle on the PDF page.</summary>
		///         <remarks>
		///         </remarks>
		public static void SetPDFContextURL (NSUrl url, CGRect rect)
		{
			UIGraphicsSetPDFContextURLForRect (url.Handle, rect);
			GC.KeepAlive (url);
		}

		[DllImport (Constants.UIKitLibrary)]
		extern static void UIGraphicsBeginPDFContextToFile (/* NSString* */ IntPtr path, CGRect bounds,
			/* NSDictionary * __nullable */ IntPtr documentInfo);

		/// <param name="file">To be added.</param>
		///         <param name="bounds">To be added.</param>
		///         <param name="documentInfo">To be added.</param>
		///         <summary>Pushes a new PDF rendering context and make it the current graphics context.</summary>
		///         <remarks>
		///           <para>UIKit keeps a stack of image context, this method creates a new image context, makes it the default and places it at the top of the graphic context stacks.</para>
		///           <para>To restore the previous graphics context, call the <see cref="M:UIKit.UIGraphics.EndImageContext" /> method.</para>
		///           <para>You can get the current context by calling the <see cref="M:UIKit.UIGraphics.GetImageFromCurrentImageContext" /> method.</para>
		///           <para>This function can only be invoked from the UI thread.</para>
		///         </remarks>
		public static void BeginPDFContext (string file, CGRect bounds, NSDictionary documentInfo)
		{
			using (var nsstr = new NSString (file)) {
				UIGraphicsBeginPDFContextToFile (nsstr.Handle, bounds, documentInfo is null ? IntPtr.Zero : documentInfo.Handle);
				GC.KeepAlive (documentInfo);
			}
		}

		/// <param name="file">To be added.</param>
		///         <param name="bounds">To be added.</param>
		///         <param name="documentInfo">To be added.</param>
		///         <summary>Pushes a new PDF rendering context and make it the current graphics context.</summary>
		///         <remarks>
		///           <para>UIKit keeps a stack of image context, this method creates a new image context, makes it the default and places it at the top of the graphic context stacks.</para>
		///           <para>To restore the previous graphics context, call the <see cref="M:UIKit.UIGraphics.EndImageContext" /> method.</para>
		///           <para>You can get the current context by calling the <see cref="M:UIKit.UIGraphics.GetImageFromCurrentImageContext" /> method.</para>
		///           <para>This function can only be invoked from the UI thread.</para>
		///         </remarks>
		public static void BeginPDFContext (string file, CGRect bounds, CGPDFInfo documentInfo)
		{
			using (var dict = documentInfo is null ? null : documentInfo.ToDictionary ())
			using (var nsstr = new NSString (file)) {
				UIGraphicsBeginPDFContextToFile (nsstr.Handle, bounds, dict is null ? IntPtr.Zero : dict.Handle);
			}
		}

		[DllImport (Constants.UIKitLibrary)]
		extern static void UIGraphicsBeginPDFContextToData (/* NSMutableData* */ IntPtr data, CGRect bounds,
			/* NSDictionary * __nullable */ IntPtr documentInfo);

		/// <param name="data">To be added.</param>
		///         <param name="bounds">To be added.</param>
		///         <param name="documentInfo">To be added.</param>
		///         <summary>Pushes a new PDF rendering context and make it the current graphics context.</summary>
		///         <remarks>
		///           <para>UIKit keeps a stack of image context, this method creates a new image context, makes it the default and places it at the top of the graphic context stacks.</para>
		///           <para>To restore the previous graphics context, call the <see cref="M:UIKit.UIGraphics.EndImageContext" /> method.</para>
		///           <para>You can get the current context by calling the <see cref="M:UIKit.UIGraphics.GetImageFromCurrentImageContext" /> method.</para>
		///           <para>This function can only be invoked from the UI thread.</para>
		///         </remarks>
		public static void BeginPDFContext (NSMutableData data, CGRect bounds, NSDictionary documentInfo)
		{
			UIGraphicsBeginPDFContextToData (data.Handle, bounds, documentInfo is null ? IntPtr.Zero : documentInfo.Handle);
			GC.KeepAlive (data);
			GC.KeepAlive (documentInfo);
		}

		[DllImport (Constants.UIKitLibrary)]
		extern static void UIGraphicsBeginPDFPage ();

		/// <summary>Starts a new page using the bounds from the initial PDF context.</summary>
		///         <remarks>Does nothing if the current context is not a PDF context</remarks>
		public static void BeginPDFPage ()
		{
			UIGraphicsBeginPDFPage ();
		}

		[DllImport (Constants.UIKitLibrary)]
		extern static void UIGraphicsBeginPDFPageWithInfo (CGRect bounds, IntPtr info);

		/// <param name="bounds">To be added.</param>
		///         <param name="pageInfo">To be added.</param>
		///         <summary>Starts a new page using the bounds from the initial PDF context.</summary>
		///         <remarks>Does nothing if the current context is not a PDF context</remarks>
		public static void BeginPDFPage (CGRect bounds, NSDictionary pageInfo)
		{
			UIGraphicsBeginPDFPageWithInfo (bounds, pageInfo.Handle);
			GC.KeepAlive (pageInfo);
		}

		[DllImport (Constants.UIKitLibrary)]
		extern static void UIGraphicsEndPDFContext ();

		public static void EndPDFContext ()
		{
			UIGraphicsEndPDFContext ();
		}

#if !XAMCORE_5_0
		/// <summary>Closes the PDF context and pops it from the stack.</summary>
		///         <remarks>
		///           <para>UIKit keeps a stack of contexts, this method pops the current PDF context, and makes the new context at the top of the stack, the new default context.</para>
		///           <para>If the current context was not a PDF context this method does nothing.</para>
		///         </remarks>
		[EditorBrowsable (EditorBrowsableState.Never)]
		[Obsolete ("Use 'EndPDFContext' instead.")]
		public static void EndPDFContent () => EndPDFContext ();
#endif

#if NET
		/// <summary>Returns the contents of the current context as an image.</summary>
		///         <returns>An image, or null on error</returns>
		///         <remarks>
		///           <para>This method is only valid if the current context (the context at the top of the stack) is an image context.</para>
		///           <para>Developers can call this method from any thread.</para>
		///         </remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("maccatalyst")]
		[UnsupportedOSPlatform ("ios17.0")]
		[UnsupportedOSPlatform ("tvos17.0")]
		[UnsupportedOSPlatform ("maccatalyst17.0")]
#else
		[Deprecated (PlatformName.iOS, 17, 0)]
		[Deprecated (PlatformName.TvOS, 17, 0)]
#endif
		public static UIImage GetImageFromCurrentImageContext ()
		{
			using (var pool = new NSAutoreleasePool ()) {
				var handle = UIGraphicsGetImageFromCurrentImageContext ();
				return (UIImage) Runtime.GetNSObject (handle);
			}
		}

		/// <summary>Returns the current graphics context</summary>
		///         <returns>
		///         </returns>
		///         <remarks>
		///           <para>This returns the current graphics context (the context at the top of the stack).   This is only valid after you have pushed a new graphics context with one of the methods in this class.</para>
		///           <para>Developers can call this method from any thread.</para>
		///         </remarks>
		public static CGContext GetCurrentContext ()
		{
			var ctx = UIGraphicsGetCurrentContext ();

			if (ctx == IntPtr.Zero)
				return null;

			return new CGContext (ctx, false);
		}

		/// <param name="ctx">To be added.</param>
		///         <summary>Manually pushes a CGContext into the UIKit graphics context stack.</summary>
		///         <remarks>
		///           <para>Developers can call this method from any thread.</para>
		///         </remarks>
		public static void PushContext (CGContext ctx)
		{
			UIGraphicsPushContext (ctx.Handle);
			GC.KeepAlive (ctx);
		}

		/// <summary>Pops the top context and sets the previous context as the default context.</summary>
		///         <remarks>
		///           <para>Developers can call this method from any thread.</para>
		///         </remarks>
		public static void PopContext ()
		{
			UIGraphicsPopContext ();
		}
	}
}
