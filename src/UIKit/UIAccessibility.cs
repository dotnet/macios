//
// This file describes the API that the generator will produce
//
// Authors:
//   Miguel de Icaza
//
// Copyrigh 2012-2014, Xamarin Inc.
//

using Foundation;
using ObjCRuntime;
using UIKit;
using CoreGraphics;

using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

// Disable until we get around to enable + fix any issues.
#nullable disable

namespace UIKit {

	// helper enum - not part of Apple API
	/// <summary>Notification types for UIAccessibility's PostNotification method.</summary>
	///     <remarks>
	///     </remarks>
	public enum UIAccessibilityPostNotification {
		/// <summary>Inform the accessibility system that an announcement must be made to the user, use an NSString argument for this notification.</summary>
		Announcement,
		/// <summary>Inform the accessibility system that new UI elements have been added or removed from the screen, use an NSString argument with the information to convey the details.</summary>
		LayoutChanged,
		/// <summary>Inform the accessibility system that scrolling has completed, use an NSString argument to pass the information to be conveyed.</summary>
		PageScrolled,
		/// <summary>Inform the accessibility system that a major change to the user interface has taken place (essentially, a new screen is visible), use an NSString argument to convey the details.</summary>
		ScreenChanged,
	}

	// NSInteger -> UIAccessibilityZoom.h
	/// <summary>An enumeration that specifies what elements (currently, only the insertion point) is involved in automatic accessibility zooming.</summary>
	///     <remarks>To be added.</remarks>
	[Native]
	public enum UIAccessibilityZoomType : long {
		/// <summary>The system zoom type is the text insertion point.</summary>
		InsertionPoint,
	}

	/// <summary>Provides access to the accessibility framework for UIKit.</summary>
	///     <remarks>To be added.</remarks>
	public static partial class UIAccessibility {
		// UIAccessibility.h
		[DllImport (Constants.UIKitLibrary)]
		extern static /* BOOL */ byte UIAccessibilityIsVoiceOverRunning ();

		/// <summary>Determines whether voiceover is currently active.</summary>
		///         <value>eturns a Boolean indicating whether voiceover is enabled.
		///         </value>
		///         <remarks>
		///         </remarks>
		static public bool IsVoiceOverRunning {
			get {
				return UIAccessibilityIsVoiceOverRunning () != 0;
			}
		}

		// UIAccessibility.h
		[DllImport (Constants.UIKitLibrary)]
		extern static /* BOOL */ byte UIAccessibilityIsMonoAudioEnabled ();

		/// <summary>Determines whether the system is running with mono audio.</summary>
		///         <value>Returns a Boolean indicating whether mono audio is enabled.
		///         </value>
		///         <remarks>
		///         </remarks>
		static public bool IsMonoAudioEnabled {
			get {
				return UIAccessibilityIsMonoAudioEnabled () != 0;
			}
		}


		// UIAccessibility.h
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		[DllImport (Constants.UIKitLibrary)]
		extern static /* NSObject */ IntPtr UIAccessibilityFocusedElement (IntPtr assistiveTechnologyIdentifier);

		/// <param name="assistiveTechnologyIdentifier">To be added.</param>
		///         <summary>Retrieves the currently focused element for the given <paramref name="assistiveTechnologyIdentifier" /> assistive technology.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		public static NSObject FocusedElement (string assistiveTechnologyIdentifier)
		{
			using (var s = new NSString (assistiveTechnologyIdentifier))
				return Runtime.GetNSObject (UIAccessibilityFocusedElement (s.Handle));
		}

		// UIAccessibility.h
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		[DllImport (Constants.UIKitLibrary)]
		extern static /* BOOL */ byte UIAccessibilityIsShakeToUndoEnabled ();

		/// <summary>Whether the "shake to undo" gesture is enabled on the device.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		public static bool IsShakeToUndoEnabled {
			get {
				return UIAccessibilityIsShakeToUndoEnabled () != 0;
			}
		}

		// UIAccessibility.h
		[DllImport (Constants.UIKitLibrary)]
		extern static /* BOOL */ byte UIAccessibilityIsClosedCaptioningEnabled ();

		/// <summary>Determines whether close captioning is currently enabled.</summary>
		///         <value>Returns a Boolean indicating whether closed captioning is enabled.
		///         </value>
		///         <remarks>
		///         </remarks>
		static public bool IsClosedCaptioningEnabled {
			get {
				return UIAccessibilityIsClosedCaptioningEnabled () != 0;
			}
		}

		// UIAccessibility.h
		[DllImport (Constants.UIKitLibrary)]
		extern static /* BOOL */ byte UIAccessibilityIsInvertColorsEnabled ();

		/// <summary>Determines if the system is currently rendering with inverted colors for accessibility</summary>
		///         <value>Returns a Boolean indicating whether inverted colors are enabled.
		///         </value>
		///         <remarks>
		///         </remarks>
		static public bool IsInvertColorsEnabled {
			get {
				return UIAccessibilityIsInvertColorsEnabled () != 0;
			}
		}

		// UIAccessibility.h
		[DllImport (Constants.UIKitLibrary)]
		extern static /* BOOL */ byte UIAccessibilityIsGuidedAccessEnabled ();

		/// <summary>Determines whether guide access is currently enabled.</summary>
		///         <value>Returns a Boolean indicating whether guide access is enabled.
		///         </value>
		///         <remarks>
		///         </remarks>
		static public bool IsGuidedAccessEnabled {
			get {
				return UIAccessibilityIsGuidedAccessEnabled () != 0;
			}
		}

		// UIAccessibility.h
		[DllImport (Constants.UIKitLibrary)]
		extern static void UIAccessibilityPostNotification (/* UIAccessibilityNotifications */ int notification, /* id */ IntPtr argument);
		// typedef uint32_t UIAccessibilityNotifications

		/// <param name="notification">Notification to post.</param>
		///         <param name="argument">Parameter to the notification.</param>
		///         <summary>Posts an accessibility notification.</summary>
		///         <remarks>
		///         </remarks>
		public static void PostNotification (UIAccessibilityPostNotification notification, NSObject argument)
		{
			PostNotification (NotificationEnumToInt (notification), argument);
		}

		/// <param name="notification">Notification to post.</param>
		///         <param name="argument">Parameter to the notification.</param>
		///         <summary>Posts an accessibility notification.</summary>
		///         <remarks>
		///         </remarks>
		public static void PostNotification (int notification, NSObject argument)
		{
			UIAccessibilityPostNotification (notification, argument is null ? IntPtr.Zero : argument.Handle);
			GC.KeepAlive (argument);
		}

		static int NotificationEnumToInt (UIAccessibilityPostNotification notification)
		{
			switch (notification) {
			case UIKit.UIAccessibilityPostNotification.Announcement:
				return UIView.AnnouncementNotification;
			case UIKit.UIAccessibilityPostNotification.LayoutChanged:
				return UIView.LayoutChangedNotification;
			case UIKit.UIAccessibilityPostNotification.PageScrolled:
				return UIView.PageScrolledNotification;
			case UIKit.UIAccessibilityPostNotification.ScreenChanged:
				return UIView.ScreenChangedNotification;
			default:
				throw new ArgumentOutOfRangeException (string.Format ("Unknown UIAccessibilityPostNotification: {0}", notification.ToString ()));
			}
		}

		// UIAccessibilityZoom.h
		[DllImport (Constants.UIKitLibrary)]
		extern static void UIAccessibilityZoomFocusChanged (/* UIAccessibilityZoomType */ IntPtr type, CGRect frame, IntPtr view);

		/// <param name="type">To be added.</param>
		///         <param name="frame">To be added.</param>
		///         <param name="view">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public static void ZoomFocusChanged (UIAccessibilityZoomType type, CGRect frame, UIView view)
		{
			UIAccessibilityZoomFocusChanged ((IntPtr) type, frame, view is not null ? view.Handle : IntPtr.Zero);
			GC.KeepAlive (view);
		}

		// UIAccessibilityZoom.h
		/// <summary>Used to inform the user that the accessibility zoom gesture conflicts with a gesture used by this application.</summary>
		///         <remarks>
		///         </remarks>
		[DllImport (Constants.UIKitLibrary, EntryPoint = "UIAccessibilityRegisterGestureConflictWithZoom")]
		extern public static void RegisterGestureConflictWithZoom ();

		// UIAccessibility.h
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		[DllImport (Constants.UIKitLibrary)]
		extern static /* UIBezierPath* */ IntPtr UIAccessibilityConvertPathToScreenCoordinates (/* UIBezierPath* */ IntPtr path, /* UIView* */ IntPtr view);

		/// <param name="path">The path object that is the target of conversion.</param>
		///         <param name="view">The view on which the coordinate system path definition was defined.</param>
		///         <summary>Converts the path to screen coordinates and returns a new path using those values.</summary>
		///         <returns>A new path object utilizing the same shape as path, but in which the points are specified in screen coordinates.</returns>
		///         <remarks>Adjusts the points of your path to values the accessibility system can use.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		public static UIBezierPath ConvertPathToScreenCoordinates (UIBezierPath path, UIView view)
		{
			if (path is null)
				throw new ArgumentNullException ("path");
			if (view is null)
				throw new ArgumentNullException ("view");

			UIBezierPath result = new UIBezierPath (UIAccessibilityConvertPathToScreenCoordinates (path.Handle, view.Handle));
			GC.KeepAlive (path);
			GC.KeepAlive (view);
			return result;
		}

		// UIAccessibility.h
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		[DllImport (Constants.UIKitLibrary)]
		extern static CGRect UIAccessibilityConvertFrameToScreenCoordinates (CGRect rect, /* UIView* */ IntPtr view);

		/// <param name="rect">The rectangle, in view coordinates, to convert.</param>
		///         <param name="view">The view in whose coordinates <paramref name="rect" /> are specified.</param>
		///         <summary>Converts the provided rectangle in the specified view to screen coordinates.</summary>
		///         <returns>A rectangle with coordinates in screen space.</returns>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		public static CGRect ConvertFrameToScreenCoordinates (CGRect rect, UIView view)
		{
			if (view is null)
				throw new ArgumentNullException ("view");

			var result = UIAccessibilityConvertFrameToScreenCoordinates (rect, view.Handle);
			GC.KeepAlive (view);
			return result;
		}

		// UIAccessibility.h
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		[DllImport (Constants.UIKitLibrary)]
		extern unsafe static void UIAccessibilityRequestGuidedAccessSession (/* BOOL */ byte enable, /* void(^completionHandler)(BOOL didSucceed) */ BlockLiteral* completionHandler);

		/// <param name="enable">Determines whether you want to enter (true) enable or leave (false) guided access mode.</param>
		///         <param name="completionHandler">Method to invoke once the system has successfully transitioned to that state.   The method takes a boolean that determines if the transition was successfull.</param>
		///         <summary>Requests that the system to enter Guided Access mode.</summary>
		///         <remarks>
		///           <para>
		/// 	    When an application is running in Guided Access mode, it can prevent the home button from working, and can control other features of the operating system from working.
		/// 	  </para>
		///           <para>
		/// 	    For this API call to succeed, the application must be Supervised, and the application must have been enabled for single app mode using Mobile Device Management.
		/// 	  </para>
		///         </remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		[BindingImpl (BindingImplOptions.Optimizable)]
		public static void RequestGuidedAccessSession (bool enable, Action<bool> completionHandler)
		{
			unsafe {
				delegate* unmanaged<IntPtr, byte, void> trampoline = &TrampolineRequestGuidedAccessSession;
				using var block = new BlockLiteral (trampoline, completionHandler, typeof (UIAccessibility), nameof (TrampolineRequestGuidedAccessSession));
				UIAccessibilityRequestGuidedAccessSession (enable ? (byte) 1 : (byte) 0, &block);
			}
		}

		/// <param name="enable">Determines whether you want to enter (true) enable or leave (false) guided access mode.</param>
		///         <summary>Asynchronously requests a transition between normal and Single App modes.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		public static Task<bool> RequestGuidedAccessSessionAsync (bool enable)
		{
			var tcs = new TaskCompletionSource<bool> ();
			RequestGuidedAccessSession (enable, (result) => {
				tcs.SetResult (result);
			});
			return tcs.Task;
		}

		[UnmanagedCallersOnly]
		static unsafe void TrampolineRequestGuidedAccessSession (IntPtr block, byte enable)
		{
			var descriptor = (BlockLiteral*) block;
			var del = (Action<bool>) (descriptor->Target);
			if (del is not null)
				del (enable != 0);
		}

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		[DllImport (Constants.UIKitLibrary)]
		static extern byte UIAccessibilityDarkerSystemColorsEnabled ();

		/// <summary>Determines whether darker system colors are currently enabled</summary>
		///         <value>Returns a Boolean indicating whether colors are enabled.</value>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		public static bool DarkerSystemColorsEnabled {
			get {
				return UIAccessibilityDarkerSystemColorsEnabled () != 0;
			}
		}

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		[DllImport (Constants.UIKitLibrary)]
		static extern byte UIAccessibilityIsBoldTextEnabled ();

		/// <summary>Determines whether bold text is currently enabled</summary>
		///         <value>Returns a Boolean indicating whether bold text is enabled.</value>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		public static bool IsBoldTextEnabled {
			get {
				return UIAccessibilityIsBoldTextEnabled () != 0;
			}
		}

		[SupportedOSPlatform ("tvos14.0")]
		[SupportedOSPlatform ("ios14.0")]
		[SupportedOSPlatform ("maccatalyst")]
		[DllImport (Constants.UIKitLibrary)]
		static extern byte UIAccessibilityButtonShapesEnabled ();

		[SupportedOSPlatform ("tvos14.0")]
		[SupportedOSPlatform ("ios14.0")]
		[SupportedOSPlatform ("maccatalyst")]
		public static bool ButtonShapesEnabled => UIAccessibilityButtonShapesEnabled () != 0;

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		[DllImport (Constants.UIKitLibrary)]
		static extern byte UIAccessibilityIsGrayscaleEnabled ();

		/// <summary>Determines whether gray scale is currently enabled</summary>
		///         <value>Returns a Boolean indicating whether gray scale is enabled.</value>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		static public bool IsGrayscaleEnabled {
			get {
				return UIAccessibilityIsGrayscaleEnabled () != 0;
			}
		}

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		[DllImport (Constants.UIKitLibrary)]
		static extern byte UIAccessibilityIsReduceMotionEnabled ();

		/// <summary>Determines whether the system is running with reduced motion.</summary>
		///         <value>Returns a Boolean indicating whether reduced motion is enabled.</value>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		static public bool IsReduceMotionEnabled {
			get {
				return UIAccessibilityIsReduceMotionEnabled () != 0;
			}
		}

		[SupportedOSPlatform ("tvos14.0")]
		[SupportedOSPlatform ("ios14.0")]
		[SupportedOSPlatform ("maccatalyst")]
		[DllImport (Constants.UIKitLibrary)]
		static extern byte UIAccessibilityPrefersCrossFadeTransitions ();

		[SupportedOSPlatform ("tvos14.0")]
		[SupportedOSPlatform ("ios14.0")]
		[SupportedOSPlatform ("maccatalyst")]
		public static bool PrefersCrossFadeTransitions => UIAccessibilityPrefersCrossFadeTransitions () != 0;

		[SupportedOSPlatform ("ios13.0")]
		[SupportedOSPlatform ("tvos13.0")]
		[SupportedOSPlatform ("maccatalyst")]
		[DllImport (Constants.UIKitLibrary)]
		static extern byte UIAccessibilityIsVideoAutoplayEnabled ();

		[SupportedOSPlatform ("ios13.0")]
		[SupportedOSPlatform ("tvos13.0")]
		[SupportedOSPlatform ("maccatalyst")]
		static public bool IsVideoAutoplayEnabled => UIAccessibilityIsVideoAutoplayEnabled () != 0;

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		[DllImport (Constants.UIKitLibrary)]
		static extern byte UIAccessibilityIsReduceTransparencyEnabled ();

		/// <summary>Determines whether the system is running with reduced transparency.</summary>
		///         <value>Returns a Boolean indicating whether reduced transparency is enabled.</value>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		static public bool IsReduceTransparencyEnabled {
			get {
				return UIAccessibilityIsReduceTransparencyEnabled () != 0;
			}
		}

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		[DllImport (Constants.UIKitLibrary)]
		static extern byte UIAccessibilityIsSwitchControlRunning ();

		/// <summary>Determines whether the system is running with switch control enabled.</summary>
		///         <value>Returns a Boolean indicating whether switch control is enabled.</value>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		static public bool IsSwitchControlRunning {
			get {
				return UIAccessibilityIsSwitchControlRunning () != 0;
			}
		}

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		[DllImport (Constants.UIKitLibrary)]
		static extern byte UIAccessibilityIsSpeakSelectionEnabled ();

		/// <summary>Determines whether the system is running with speak selection enabled.</summary>
		///         <value>Returns a Boolean indicating whether speak selecton is enabled.</value>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		static public bool IsSpeakSelectionEnabled {
			get {
				return UIAccessibilityIsSpeakSelectionEnabled () != 0;
			}
		}

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		[DllImport (Constants.UIKitLibrary)]
		static extern byte UIAccessibilityIsSpeakScreenEnabled ();

		/// <summary>Determines whether the system is running with the speak screen enabled.</summary>
		///         <value>Returns a Boolean indicating whether the speak screen is enabled.</value>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		static public bool IsSpeakScreenEnabled {
			get {
				return UIAccessibilityIsSpeakScreenEnabled () != 0;
			}
		}

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("maccatalyst")]
		[DllImport (Constants.UIKitLibrary)]
		static extern byte UIAccessibilityIsAssistiveTouchRunning ();

		/// <summary>Whether assistive touch is active.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("maccatalyst")]
		public static bool IsAssistiveTouchRunning {
			get {
				return UIAccessibilityIsAssistiveTouchRunning () != 0;
			}
		}

		[SupportedOSPlatform ("ios13.0")]
		[SupportedOSPlatform ("tvos13.0")]
		[SupportedOSPlatform ("maccatalyst")]
		[DllImport (Constants.UIKitLibrary)]
		static extern byte UIAccessibilityShouldDifferentiateWithoutColor ();

		[SupportedOSPlatform ("ios13.0")]
		[SupportedOSPlatform ("tvos13.0")]
		[SupportedOSPlatform ("maccatalyst")]
		public static bool ShouldDifferentiateWithoutColor => UIAccessibilityShouldDifferentiateWithoutColor () != 0;

		[SupportedOSPlatform ("ios13.0")]
		[SupportedOSPlatform ("tvos13.0")]
		[SupportedOSPlatform ("maccatalyst")]
		[DllImport (Constants.UIKitLibrary)]
		static extern byte UIAccessibilityIsOnOffSwitchLabelsEnabled ();

		[SupportedOSPlatform ("ios13.0")]
		[SupportedOSPlatform ("tvos13.0")]
		[SupportedOSPlatform ("maccatalyst")]
		public static bool IsOnOffSwitchLabelsEnabled => UIAccessibilityIsOnOffSwitchLabelsEnabled () != 0;

#if !TVOS
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[UnsupportedOSPlatform ("tvos")]
		[DllImport (Constants.UIKitLibrary)]
		static extern nuint UIAccessibilityHearingDevicePairedEar ();

		/// <summary>Retrieves the status of how a hearing device is paired to one, both, or no ears.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[UnsupportedOSPlatform ("tvos")]
		public static UIAccessibilityHearingDeviceEar HearingDevicePairedEar {
			get {
				return (UIAccessibilityHearingDeviceEar) (ulong) UIAccessibilityHearingDevicePairedEar ();
			}
		}
#endif
	}


}
