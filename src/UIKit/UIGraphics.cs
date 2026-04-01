//
// UIGraphics.cs: Bindings to some P/Invoke APIs to interface with CoreGraphics
//
// Authors:
//   Miguel de Icaza (miguel@novell.com)
//
// Copyright 2009 Novell, inc.
// Copyright 2012-2014 Xamarin Inc
//
using CoreGraphics;
using System.ComponentModel;

#nullable enable

namespace UIKit {

#if XAMCORE_5_0
#error All PDF public name instances in this file need to be turned into Pdf. e.g. EndPDFContext into EndPdfContext.
#endif

	/// <summary>Helper methods to paint on the screen, PDF context or bitmaps.</summary>
	/// <remarks>
	///   <para>Methods in this class generally correspond to <c>UIGraphics*</c> and <c>UIRect*</c> methods in Apple's UIKit Framework.</para>
	/// </remarks>
	public static class UIGraphics {
		[DllImport (Constants.UIKitLibrary)]
		extern static IntPtr UIGraphicsGetCurrentContext ();

		[DllImport (Constants.UIKitLibrary)]
		extern static void UIGraphicsPushContext (IntPtr ctx);

		[DllImport (Constants.UIKitLibrary)]
		extern static void UIGraphicsPopContext ();

		/// <summary>Fills <paramref name="rect" /> with the current fill color, using <paramref name="blendMode" />.</summary>
		/// <param name="rect">The rectangle to fill.</param>
		/// <param name="blendMode">The blend mode to use when filling.</param>
		/// <remarks>
		///   <para>Developers can call this method from any thread.</para>
		/// </remarks>
		[DllImport (Constants.UIKitLibrary, EntryPoint = "UIRectFillUsingBlendMode")]
		public extern static void RectFillUsingBlendMode (CGRect rect, CGBlendMode blendMode);

		/// <summary>Fills a rectangle with the current color on the current context.</summary>
		/// <param name="rect">The region to fill.</param>
		/// <remarks>
		///   <para>Developers can call this method from any thread.</para>
		/// </remarks>
		[DllImport (Constants.UIKitLibrary, EntryPoint = "UIRectFill")]
		public extern static void RectFill (CGRect rect);

		/// <summary>Draws a frame inside the specified rectangle and blending it with <paramref name="blendMode" />.</summary>
		/// <param name="rect">The rectangle in which to draw the frame.</param>
		/// <param name="blendMode">The blend mode to use when drawing.</param>
		[DllImport (Constants.UIKitLibrary, EntryPoint = "UIRectFrameUsingBlendMode")]
		public extern static void RectFrameUsingBlendMode (CGRect rect, CGBlendMode blendMode);

		/// <summary>Draws a frame inside the specified rectangle.</summary>
		/// <param name="rect">Region where the frame will be drawn.</param>
		[DllImport (Constants.UIKitLibrary, EntryPoint = "UIRectFrame")]
		public extern static void RectFrame (CGRect rect);

		/// <summary>Intersects the current clipping path with the specified rectangle.</summary>
		/// <param name="rect">The new clipping path.</param>
		[DllImport (Constants.UIKitLibrary, EntryPoint = "UIRectClip")]
		public extern static void RectClip (CGRect rect);

		/// <summary>Pushes a new image context and makes it the current graphics context.</summary>
		/// <param name="size">Size of the image context.</param>
		/// <remarks>
		///   <para>UIKit keeps a stack of image contexts. This method creates a new image context, makes it the default and places it at the top of the graphic context stack.</para>
		///   <para>To restore the previous graphics context, call the <see cref="EndImageContext" /> method.</para>
		///   <para>You can get the image from the current image context by calling the <see cref="GetImageFromCurrentImageContext" /> method.</para>
		///   <para>Developers can call this method from any thread.</para>
		/// </remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("maccatalyst")]
		[UnsupportedOSPlatform ("ios17.0")]
		[UnsupportedOSPlatform ("tvos17.0")]
		[UnsupportedOSPlatform ("maccatalyst17.0")]
		[DllImport (Constants.UIKitLibrary, EntryPoint = "UIGraphicsBeginImageContext")]
		public extern static void BeginImageContext (CGSize size);

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("maccatalyst")]
		[UnsupportedOSPlatform ("ios17.0")]
		[UnsupportedOSPlatform ("tvos17.0")]
		[UnsupportedOSPlatform ("maccatalyst17.0")]
		[DllImport (Constants.UIKitLibrary, EntryPoint = "UIGraphicsBeginImageContextWithOptions")]
		extern static void BeginImageContextWithOptions (CGSize size, byte opaque, nfloat scale);

		/// <summary>Pushes a new image context with the specified options and makes it the current graphics context.</summary>
		/// <param name="size">The size of the image context.</param>
		/// <param name="opaque">Whether the image context is opaque.</param>
		/// <param name="scale">The scale factor to apply (use 0 for the device's main screen scale).</param>
		/// <remarks>
		///   <para>UIKit keeps a stack of image contexts. This method creates a new image context, makes it the default and places it at the top of the graphic context stack.</para>
		///   <para>To restore the previous graphics context, call the <see cref="EndImageContext" /> method.</para>
		///   <para>You can get the image from the current image context by calling the <see cref="GetImageFromCurrentImageContext" /> method.</para>
		///   <para>Developers can call this method from any thread.</para>
		/// </remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("maccatalyst")]
		[UnsupportedOSPlatform ("ios17.0")]
		[UnsupportedOSPlatform ("tvos17.0")]
		[UnsupportedOSPlatform ("maccatalyst17.0")]
		public static void BeginImageContextWithOptions (CGSize size, bool opaque, nfloat scale)
		{
			BeginImageContextWithOptions (size, opaque ? (byte) 1 : (byte) 0, scale);
		}

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("maccatalyst")]
		[UnsupportedOSPlatform ("ios17.0")]
		[UnsupportedOSPlatform ("tvos17.0")]
		[UnsupportedOSPlatform ("maccatalyst17.0")]
		[DllImport (Constants.UIKitLibrary)]
		static extern IntPtr UIGraphicsGetImageFromCurrentImageContext ();

		/// <summary>Pops the current image context.</summary>
		/// <remarks>
		///   <para>UIKit keeps a stack of image contexts. This method pops the current image context, and makes the new context at the top of the stack the new default context.</para>
		///   <para>If the current context was not created using <see cref="BeginImageContextWithOptions(CGSize,bool,nfloat)" />
		///     or <see cref="BeginImageContext(CGSize)" />,
		///     this method does nothing.</para>
		/// </remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("maccatalyst")]
		[UnsupportedOSPlatform ("ios17.0")]
		[UnsupportedOSPlatform ("tvos17.0")]
		[UnsupportedOSPlatform ("maccatalyst17.0")]
		[DllImport (Constants.UIKitLibrary, EntryPoint = "UIGraphicsEndImageContext")]
		public extern static void EndImageContext ();

		[DllImport (Constants.UIKitLibrary)]
		extern static void UIGraphicsAddPDFContextDestinationAtPoint (IntPtr str, CGPoint point);

		/// <summary>Adds a PDF destination with the given name at the given position.</summary>
		/// <param name="str">Name of the destination point.</param>
		/// <param name="point">A point in the current context.</param>
		/// <remarks>Only valid if the current graphics context is a PDF context.</remarks>
		public static void AddPDFContextDestination (string str, CGPoint point)
		{
			using (var nsstr = new NSString (str))
				UIGraphicsAddPDFContextDestinationAtPoint (nsstr.Handle, point);
		}

		[DllImport (Constants.UIKitLibrary)]
		extern static void UIGraphicsSetPDFContextDestinationForRect (IntPtr str, CGRect rect);

		/// <summary>Sets the PDF destination with the given name at the given position.</summary>
		/// <param name="str">The name of the destination.</param>
		/// <param name="rect">The rectangle to associate with the destination.</param>
		public static void SetPDFContextDestination (string str, CGRect rect)
		{
			using (var nsstr = new NSString (str))
				UIGraphicsSetPDFContextDestinationForRect (nsstr.Handle, rect);
		}

		[DllImport (Constants.UIKitLibrary)]
		extern static CGRect UIGraphicsGetPDFContextBounds ();

		/// <summary>Returns the current bounds for the PDF page.</summary>
		/// <value>The bounds of the current PDF page.</value>
		public static CGRect PDFContextBounds {
			get {
				return UIGraphicsGetPDFContextBounds ();
			}
		}

		[DllImport (Constants.UIKitLibrary)]
		extern static void UIGraphicsSetPDFContextURLForRect (IntPtr url, CGRect rect);

		/// <summary>Links the URL to the specified rectangle on the PDF page.</summary>
		/// <param name="url">The URL to link.</param>
		/// <param name="rect">The rectangle on the PDF page to associate with the URL.</param>
		public static void SetPDFContextURL (NSUrl url, CGRect rect)
		{
			UIGraphicsSetPDFContextURLForRect (url.Handle, rect);
			GC.KeepAlive (url);
		}

		[DllImport (Constants.UIKitLibrary)]
		extern static void UIGraphicsBeginPDFContextToFile (/* NSString* */ IntPtr path, CGRect bounds,
			/* NSDictionary * __nullable */ IntPtr documentInfo);

		/// <summary>Pushes a new PDF rendering context that writes to a file and makes it the current graphics context.</summary>
		/// <param name="file">The path to the PDF file to create.</param>
		/// <param name="bounds">The bounds of the PDF page.</param>
		/// <param name="documentInfo">Optional dictionary containing PDF document metadata, or <see langword="null" />.</param>
		/// <remarks>
		///   <para>UIKit keeps a stack of contexts. This method creates a new PDF context, makes it the default and places it at the top of the graphic context stack.</para>
		///   <para>To close the PDF context and restore the previous graphics context, call the <see cref="EndPDFContext" /> method.</para>
		///   <para>This function can only be invoked from the UI thread.</para>
		/// </remarks>
		public static void BeginPDFContext (string file, CGRect bounds, NSDictionary? documentInfo)
		{
			using (var nsstr = new NSString (file)) {
				UIGraphicsBeginPDFContextToFile (nsstr.Handle, bounds, documentInfo.GetHandle ());
				GC.KeepAlive (documentInfo);
			}
		}

		/// <summary>Pushes a new PDF rendering context that writes to a file and makes it the current graphics context.</summary>
		/// <param name="file">The path to the PDF file to create.</param>
		/// <param name="bounds">The bounds of the PDF page.</param>
		/// <param name="documentInfo">Optional <see cref="CGPDFInfo" /> containing PDF document metadata, or <see langword="null" />.</param>
		/// <remarks>
		///   <para>UIKit keeps a stack of contexts. This method creates a new PDF context, makes it the default and places it at the top of the graphic context stack.</para>
		///   <para>To close the PDF context and restore the previous graphics context, call the <see cref="EndPDFContext" /> method.</para>
		///   <para>This function can only be invoked from the UI thread.</para>
		/// </remarks>
		public static void BeginPDFContext (string file, CGRect bounds, CGPDFInfo? documentInfo)
		{
			using (var dict = documentInfo?.ToDictionary ())
			using (var nsstr = new NSString (file)) {
				UIGraphicsBeginPDFContextToFile (nsstr.Handle, bounds, dict.GetHandle ());
			}
		}

		[DllImport (Constants.UIKitLibrary)]
		extern static void UIGraphicsBeginPDFContextToData (/* NSMutableData* */ IntPtr data, CGRect bounds,
			/* NSDictionary * __nullable */ IntPtr documentInfo);

		/// <summary>Pushes a new PDF rendering context that writes to an <see cref="NSMutableData" /> object and makes it the current graphics context.</summary>
		/// <param name="data">The mutable data object to write PDF output to.</param>
		/// <param name="bounds">The bounds of the PDF page.</param>
		/// <param name="documentInfo">Optional dictionary containing PDF document metadata, or <see langword="null" />.</param>
		/// <remarks>
		///   <para>UIKit keeps a stack of contexts. This method creates a new PDF context, makes it the default and places it at the top of the graphic context stack.</para>
		///   <para>To close the PDF context and restore the previous graphics context, call the <see cref="EndPDFContext" /> method.</para>
		///   <para>This function can only be invoked from the UI thread.</para>
		/// </remarks>
		public static void BeginPDFContext (NSMutableData data, CGRect bounds, NSDictionary? documentInfo)
		{
			System.ArgumentNullException.ThrowIfNull (data);

			UIGraphicsBeginPDFContextToData (data.Handle, bounds, documentInfo.GetHandle ());
			GC.KeepAlive (data);
			GC.KeepAlive (documentInfo);
		}

		[DllImport (Constants.UIKitLibrary)]
		extern static void UIGraphicsBeginPDFPage ();

		/// <summary>Starts a new page using the bounds from the initial PDF context.</summary>
		/// <remarks>Does nothing if the current context is not a PDF context.</remarks>
		public static void BeginPDFPage ()
		{
			UIGraphicsBeginPDFPage ();
		}

		[DllImport (Constants.UIKitLibrary)]
		extern static void UIGraphicsBeginPDFPageWithInfo (CGRect bounds, IntPtr info);

		/// <summary>Starts a new page with the specified bounds and page information in the current PDF context.</summary>
		/// <param name="bounds">The bounds of the new PDF page.</param>
		/// <param name="pageInfo">A dictionary containing page-specific metadata.</param>
		/// <remarks>Does nothing if the current context is not a PDF context.</remarks>
		public static void BeginPDFPage (CGRect bounds, NSDictionary pageInfo)
		{
			UIGraphicsBeginPDFPageWithInfo (bounds, pageInfo.Handle);
			GC.KeepAlive (pageInfo);
		}

		[DllImport (Constants.UIKitLibrary)]
		extern static void UIGraphicsEndPDFContext ();

		/// <summary>Closes the PDF context and pops it from the stack.</summary>
		/// <remarks>
		///   <para>UIKit keeps a stack of contexts. This method pops the current PDF context, and makes the new context at the top of the stack the new default context.</para>
		///   <para>If the current context was not a PDF context, this method does nothing.</para>
		/// </remarks>
		public static void EndPDFContext ()
		{
			UIGraphicsEndPDFContext ();
		}

#if !XAMCORE_5_0
		/// <summary>Closes the PDF context and pops it from the stack.</summary>
		/// <remarks>
		///   <para>UIKit keeps a stack of contexts. This method pops the current PDF context, and makes the new context at the top of the stack the new default context.</para>
		///   <para>If the current context was not a PDF context, this method does nothing.</para>
		/// </remarks>
		[EditorBrowsable (EditorBrowsableState.Never)]
		[Obsolete ("Use 'EndPDFContext' instead.")]
		public static void EndPDFContent () => EndPDFContext ();
#endif

		/// <summary>Returns the contents of the current context as an image.</summary>
		/// <returns>An image, or <see langword="null" /> on error.</returns>
		/// <remarks>
		///   <para>This method is only valid if the current context (the context at the top of the stack) is an image context.</para>
		///   <para>Developers can call this method from any thread.</para>
		/// </remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("maccatalyst")]
		[UnsupportedOSPlatform ("ios17.0")]
		[UnsupportedOSPlatform ("tvos17.0")]
		[UnsupportedOSPlatform ("maccatalyst17.0")]
		public static UIImage? GetImageFromCurrentImageContext ()
		{
			using (var pool = new NSAutoreleasePool ()) {
				var handle = UIGraphicsGetImageFromCurrentImageContext ();
				return Runtime.GetNSObject (handle) as UIImage;
			}
		}

		/// <summary>Returns the current graphics context.</summary>
		/// <returns>The current <see cref="CGContext" />, or <see langword="null" /> if there is no current context.</returns>
		/// <remarks>
		///   <para>This returns the current graphics context (the context at the top of the stack). This is only valid after you have pushed a new graphics context with one of the methods in this class.</para>
		///   <para>Developers can call this method from any thread.</para>
		/// </remarks>
		public static CGContext? GetCurrentContext ()
		{
			var ctx = UIGraphicsGetCurrentContext ();

			if (ctx == IntPtr.Zero)
				return null;

			return new CGContext (ctx, false);
		}

		/// <summary>Manually pushes a <see cref="CGContext" /> into the UIKit graphics context stack.</summary>
		/// <param name="ctx">The graphics context to push onto the stack.</param>
		/// <remarks>
		///   <para>Developers can call this method from any thread.</para>
		/// </remarks>
		public static void PushContext (CGContext ctx)
		{
			UIGraphicsPushContext (ctx.Handle);
			GC.KeepAlive (ctx);
		}

		/// <summary>Pops the top context and sets the previous context as the default context.</summary>
		/// <remarks>
		///   <para>Developers can call this method from any thread.</para>
		/// </remarks>
		public static void PopContext ()
		{
			UIGraphicsPopContext ();
		}
	}
}
