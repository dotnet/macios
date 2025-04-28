//
// UIVideo.cs: Support for saving videos
//
// Authors:
//   Miguel de Icaza
//
// Copyright 2009, Novell, Inc.
// Copyrigh 2014-2015 Xamarin Inc.
//

#if IOS

using System;
using System.Runtime.InteropServices;
using System.Collections;
using Foundation;
using ObjCRuntime;

namespace UIKit {

	[Register ("__MonoTouch_UIVideoStatusDispatcher")]
	internal class UIVideoStatusDispatcher : NSObject {
		public const string callbackSelector = "Xamarin_Internal__video:didFinishSavingWithError:contextInfo:";
		UIVideo.SaveStatus status;

		public UIVideoStatusDispatcher (UIVideo.SaveStatus status)
		{
			this.status = status;
			DangerousRetain ();
			IsDirectBinding = false;
		}

		[Export (callbackSelector)]
		[Preserve (Conditional = true)]
		public void Callback (string str, NSError err, IntPtr ctx)
		{
			status (str, err);
			DangerousRelease ();
		}
	}

	/// <summary>Static class that exposes some helper methods for manipulating video.</summary>
	///     <remarks>To be added.</remarks>
	public static class UIVideo {
		/// <param name="path">To be added.</param>
		///     <param name="error">To be added.</param>
		///     <summary>A delegate signature that is invoked after the video is saved.</summary>
		///     <remarks>To be added.</remarks>
		public delegate void SaveStatus (string path, NSError error);

		[DllImport (Constants.UIKitLibrary)]
		extern static /* BOOL */ byte UIVideoAtPathIsCompatibleWithSavedPhotosAlbum (/* NSString* */ IntPtr videoPath);

		/// <param name="path">The path to probe.</param>
		///         <summary>Determines whether the video file is compatible with the current photos album.</summary>
		///         <returns />
		///         <remarks>To be added.</remarks>
		public static bool IsCompatibleWithSavedPhotosAlbum (string path)
		{
			UIApplication.EnsureUIThread ();
			using (var ns = new NSString (path))
				return UIVideoAtPathIsCompatibleWithSavedPhotosAlbum (ns.Handle) != 0;
		}

		[DllImport (Constants.UIKitLibrary)]
		extern static void UISaveVideoAtPathToSavedPhotosAlbum (/* NSString* */ IntPtr videoPath, /* id */ IntPtr completionTarget, /* SEL */ IntPtr selector, /* void* */ IntPtr contextInfo);

		/// <param name="path">The path to save.</param>
		///         <param name="status">Callback that will be invoked when the saving completes.</param>
		///         <summary>Saves the video to the photos album.</summary>
		///         <remarks>To be added.</remarks>
		public static void SaveToPhotosAlbum (string path, SaveStatus status)
		{
			if (path is null)
				throw new ArgumentNullException ("path");
			if (status is null)
				throw new ArgumentNullException ("status");
			UIApplication.EnsureUIThread ();
			var dis = new UIVideoStatusDispatcher (status);

			using (var ns = new NSString (path)) {
				UISaveVideoAtPathToSavedPhotosAlbum (ns.Handle, dis.Handle, Selector.GetHandle (UIVideoStatusDispatcher.callbackSelector), IntPtr.Zero);
				GC.KeepAlive (dis);
			}
		}

	}
}

#endif // IOS
