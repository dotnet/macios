//
// UIImage.cs: Extra helper routines for images
//
// Authors:
//   Miguel de Icaza
//
// Copyright 2009, Novell, Inc.
// Copyrigh 2014 Xamarin Inc.
//
using System;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Collections;

using Foundation;
using ObjCRuntime;
using CoreGraphics;

#nullable enable

namespace UIKit {

	partial class UIImage {
#if IOS
		/// <param name="image">The image saved.</param>
		///     <param name="error">Errors, if any.</param>
		///     <summary>A delegate signature for getting a notification when the file has been saved.</summary>
		///     <remarks>To be added.</remarks>
		public delegate void SaveStatus (UIImage image, NSError error);

		[DllImport (Constants.UIKitLibrary)]
		extern static void UIImageWriteToSavedPhotosAlbum (/* UIImage */ IntPtr image, /* id */ IntPtr obj, /* SEL */ IntPtr selector, /*vcoid* */ IntPtr ctx);

		/// <include file="../../docs/api/UIKit/UIImage.xml" path="/Documentation/Docs[@DocId='M:UIKit.UIImage.SaveToPhotosAlbum(UIKit.UIImage.SaveStatus)']/*" />
	public void SaveToPhotosAlbum (SaveStatus status)
		{
			UIImageStatusDispatcher? dis = null;
			UIApplication.EnsureUIThread ();

			if (status is not null)
				dis = new UIImageStatusDispatcher (status);

			UIImageWriteToSavedPhotosAlbum (Handle, dis is not null ? dis.Handle : IntPtr.Zero, dis is not null ? Selector.GetHandle (UIImageStatusDispatcher.callbackSelector) : IntPtr.Zero, IntPtr.Zero);
			GC.KeepAlive (dis);
		}
#endif

		[DllImport (Constants.UIKitLibrary)]
		extern static /* NSData */ IntPtr UIImagePNGRepresentation (/* UIImage */ IntPtr image);

		/// <summary>Encodes the image into a <see cref="Foundation.NSData" /> byte blob using the PNG encoding.</summary>
		///         <returns>The encoded image in an NSData wrapper or null if there was an error.</returns>
		///         <remarks>
		///           <para>
		///           </para>
		///           <para tool="threads">This can be used from a background thread.</para>
		///         </remarks>
		public NSData? AsPNG ()
		{
			using (var pool = new NSAutoreleasePool ())
				return Runtime.GetNSObject<NSData> (UIImagePNGRepresentation (Handle));
		}

		[DllImport (Constants.UIKitLibrary)]
		extern static /* NSData */ IntPtr UIImageJPEGRepresentation (/* UIImage */ IntPtr image, /* CGFloat */ nfloat compressionQuality);

		/// <summary>Encodes the image with minimal compression (maximum quality) into a <see cref="Foundation.NSData" /> byte blob using the JPEG encoding.</summary>
		///         <returns>The encoded image in an NSData wrapper or null if there was an error.</returns>
		///         <remarks>
		///           <para>
		///           </para>
		///           <para tool="threads">This can be used from a background thread.</para>
		///         </remarks>
		public NSData? AsJPEG ()
		{
			using (var pool = new NSAutoreleasePool ())
				return Runtime.GetNSObject<NSData> (UIImageJPEGRepresentation (Handle, 1.0f));
		}

		/// <param name="compressionQuality">The compression quality to use, 0.0 is the maximum compression (worse quality), and 1.0 minimum compression (best quality)</param>
		/// <summary>Encodes the image into a <see cref="Foundation.NSData" /> byte blob using the JPEG encoding.</summary>
		/// <returns>The encoded image in an NSData wrapper or null if there was an error.</returns>
		/// <remarks>
		///           <para>
		///           </para>
		///           <para tool="threads">This can be used from a background thread.</para>
		///         </remarks>
		public NSData? AsJPEG (nfloat compressionQuality)
		{
			using (var pool = new NSAutoreleasePool ())
				return Runtime.GetNSObject<NSData> (UIImageJPEGRepresentation (Handle, compressionQuality));
		}

		/// <summary>Scales the image up or down.</summary>
		/// <param name="newSize">The desired size for the scaled image.</param>
		/// <param name="scaleFactor">Scale factor to apply to the scaled image. If the value specified is zero, the device's scale factor is used.</param>
		/// <returns>The scaled image.</returns>
		/// <remarks>
		///   <para tool="threads">This can be used from a background thread.</para>
		/// </remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("maccatalyst")]
		[UnsupportedOSPlatform ("ios17.0")]
		[UnsupportedOSPlatform ("tvos17.0")]
		[UnsupportedOSPlatform ("maccatalyst17.0")]
		public UIImage Scale (CGSize newSize, nfloat scaleFactor)
		{
			UIGraphics.BeginImageContextWithOptions (newSize, false, scaleFactor);

			Draw (new CGRect (0, 0, newSize.Width, newSize.Height));

			var scaledImage = UIGraphics.GetImageFromCurrentImageContext ();
			UIGraphics.EndImageContext ();

			return scaledImage;
		}

		/// <summary>Scales the image up or down.</summary>
		/// <param name="newSize">The desired size for the scaled image.</param>
		/// <returns>The scaled image.</returns>
		/// <remarks>
		///   <para tool="threads">This can be used from a background thread.</para>
		/// </remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("maccatalyst")]
		[UnsupportedOSPlatform ("ios17.0")]
		[UnsupportedOSPlatform ("tvos17.0")]
		[UnsupportedOSPlatform ("maccatalyst17.0")]
		public UIImage Scale (CGSize newSize)
		{
			UIGraphics.BeginImageContext (newSize);

			Draw (new CGRect (0, 0, newSize.Width, newSize.Height));

			var scaledImage = UIGraphics.GetImageFromCurrentImageContext ();
			UIGraphics.EndImageContext ();

			return scaledImage;
		}

		// required because of GetCallingAssembly (if we ever inline across assemblies)
		/// <param name="assembly">The resource is looked up in this assembly.   If the value is null, the resource is looked up in the assembly that calls this method.</param>
		///         <param name="name">The name of the embedded resource</param>
		///         <summary>Loads an image from a resource embedded in the assembly.</summary>
		///         <returns>The image loaded from the specified assembly.</returns>
		///         <remarks>
		///           <para>If the passed parameter for assembly is null, then the resource is looked up in the calling assembly using <see cref="System.Reflection.Assembly.GetCallingAssembly" />.</para>
		///           <para tool="threads">This can be used from a background thread.</para>
		///         </remarks>
		[MethodImpl (MethodImplOptions.NoInlining)]
		public static UIImage? FromResource (Assembly assembly, string name)
		{
			if (name is null)
				throw new ArgumentNullException ("name");
			if (assembly is null)
				assembly = Assembly.GetCallingAssembly ();
			var stream = assembly.GetManifestResourceStream (name);
			if (stream is null)
				throw new ArgumentException (String.Format ("No resource named `{0}' found", name));

			byte [] buffer = new byte [stream.Length];
			stream.ReadExactly (buffer, 0, buffer.Length);
			unsafe {
				fixed (byte* p = buffer) {
					var data = NSData.FromBytes ((IntPtr) p, (uint) stream.Length);
					return LoadFromData (data);
				}
			}
		}

		[SupportedOSPlatform ("ios17.0")]
		[SupportedOSPlatform ("tvos17.0")]
		[SupportedOSPlatform ("maccatalyst17.0")]
		[DllImport (Constants.UIKitLibrary)]
		static extern /* NSData */ IntPtr UIImageHEICRepresentation (/* UIImage */ IntPtr image);

		[SupportedOSPlatform ("ios17.0")]
		[SupportedOSPlatform ("tvos17.0")]
		[SupportedOSPlatform ("maccatalyst17.0")]
		public NSData? HeicRepresentation
			=> Runtime.GetNSObject<NSData> (UIImageHEICRepresentation (Handle));


		// that was used (03be3e0d43085dfef2e732494216d9b2bf8fc079) to implement FromResource but that code 
		// was changed later (d485b61793b0d986f416c8d6154fb92c7a57d79d) making it unused AFAICS
#if false
		internal class DataWrapper : NSData {
			IntPtr buffer;
			uint len;
			
			public DataWrapper (IntPtr buffer, uint len)
			{
				this.buffer = buffer;
				this.len = len;
			}

			public override nuint Length { get { return len; } }
			public override IntPtr Bytes { get { return buffer; } }
		}
#endif
	}

#if IOS
	[Register ("__MonoTouch_UIImageStatusDispatcher")]
	internal class UIImageStatusDispatcher : NSObject {
		public const string callbackSelector = "Xamarin_Internal__image:didFinishSavingWithError:contextInfo:";
		UIImage.SaveStatus status;

		public UIImageStatusDispatcher (UIImage.SaveStatus status)
		{
			IsDirectBinding = false;
			this.status = status;
			DangerousRetain ();
		}

		[Export (callbackSelector)]
		[Preserve (Conditional = true)]
		public void Callback (UIImage image, NSError err, IntPtr ctx)
		{
			status (image, err);
			DangerousRelease ();
		}
	}
#endif
}
