//
// This file describes the API that the generator will produce
//
// Authors:
//   Miguel de Icaza
//
// Copyrigh 2012-2014, Xamarin Inc.
//

using UIKit;
using CoreGraphics;

using System.Threading.Tasks;

#nullable enable

namespace UIKit {

	/// <summary>Notification types for the <see cref="UIAccessibility.PostNotification(UIAccessibilityPostNotification, NSObject?)"/> method.</summary>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("tvos")]
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

	/// <summary>An enumeration that specifies what elements (currently, only the insertion point) is involved in automatic accessibility zooming.</summary>
	[Native]
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("tvos")]
	public enum UIAccessibilityZoomType : long {
		/// <summary>The system zoom type is the text insertion point.</summary>
		InsertionPoint,
	}

	/// <summary>Provides access to the accessibility framework for UIKit.</summary>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("tvos")]
	public static class UIAccessibility {
		// UIAccessibility.h
		[DllImport (Constants.UIKitLibrary)]
		extern static /* BOOL */ byte UIAccessibilityIsVoiceOverRunning ();

		/// <summary>Gets a value indicating whether VoiceOver is currently active.</summary>
		/// <value><see langword="true"/> if VoiceOver is running; otherwise, <see langword="false"/>.</value>
		static public bool IsVoiceOverRunning {
			get {
				return UIAccessibilityIsVoiceOverRunning () != 0;
			}
		}

		// UIAccessibility.h
		[DllImport (Constants.UIKitLibrary)]
		extern static /* BOOL */ byte UIAccessibilityIsMonoAudioEnabled ();

		/// <summary>Gets a value indicating whether mono audio is enabled.</summary>
		/// <value><see langword="true"/> if mono audio is enabled; otherwise, <see langword="false"/>.</value>
		static public bool IsMonoAudioEnabled {
			get {
				return UIAccessibilityIsMonoAudioEnabled () != 0;
			}
		}


		// UIAccessibility.h
		[DllImport (Constants.UIKitLibrary)]
		extern static /* __nullable NSObject */ IntPtr UIAccessibilityFocusedElement (/* __nullable */ IntPtr assistiveTechnologyIdentifier);

		/// <param name="assistiveTechnologyIdentifier">The identifier of the assistive technology to query.</param>
		/// <summary>Retrieves the currently focused element for the specified assistive technology.</summary>
		/// <returns>The focused element, or <see langword="null"/> if no element is focused.</returns>
		public static NSObject? FocusedElement (string? assistiveTechnologyIdentifier)
		{
			using var s = new TransientCFString (assistiveTechnologyIdentifier);
			return Runtime.GetNSObject (UIAccessibilityFocusedElement (s));
		}

		// UIAccessibility.h
		[DllImport (Constants.UIKitLibrary)]
		extern static /* BOOL */ byte UIAccessibilityIsShakeToUndoEnabled ();

		/// <summary>Gets a value indicating whether the "shake to undo" gesture is enabled.</summary>
		/// <value><see langword="true"/> if shake to undo is enabled; otherwise, <see langword="false"/>.</value>
		public static bool IsShakeToUndoEnabled {
			get {
				return UIAccessibilityIsShakeToUndoEnabled () != 0;
			}
		}

		// UIAccessibility.h
		[DllImport (Constants.UIKitLibrary)]
		extern static /* BOOL */ byte UIAccessibilityIsClosedCaptioningEnabled ();

		/// <summary>Gets a value indicating whether closed captioning is enabled.</summary>
		/// <value><see langword="true"/> if closed captioning is enabled; otherwise, <see langword="false"/>.</value>
		static public bool IsClosedCaptioningEnabled {
			get {
				return UIAccessibilityIsClosedCaptioningEnabled () != 0;
			}
		}

		// UIAccessibility.h
		[DllImport (Constants.UIKitLibrary)]
		extern static /* BOOL */ byte UIAccessibilityIsInvertColorsEnabled ();

		/// <summary>Gets a value indicating whether inverted colors are enabled.</summary>
		/// <value><see langword="true"/> if inverted colors are enabled; otherwise, <see langword="false"/>.</value>
		static public bool IsInvertColorsEnabled {
			get {
				return UIAccessibilityIsInvertColorsEnabled () != 0;
			}
		}

		// UIAccessibility.h
		[DllImport (Constants.UIKitLibrary)]
		extern static /* BOOL */ byte UIAccessibilityIsGuidedAccessEnabled ();

		/// <summary>Gets a value indicating whether Guided Access is enabled.</summary>
		/// <value><see langword="true"/> if Guided Access is enabled; otherwise, <see langword="false"/>.</value>
		static public bool IsGuidedAccessEnabled {
			get {
				return UIAccessibilityIsGuidedAccessEnabled () != 0;
			}
		}

		// UIAccessibility.h
		[DllImport (Constants.UIKitLibrary)]
		extern static void UIAccessibilityPostNotification (/* UIAccessibilityNotifications */ int notification, /* id */ IntPtr argument);
		// typedef uint32_t UIAccessibilityNotifications

		/// <summary>Posts an accessibility notification.</summary>
		/// <param name="notification">The notification to post.</param>
		/// <param name="argument">The argument to pass with the notification, or <see langword="null"/>.</param>
		public static void PostNotification (UIAccessibilityPostNotification notification, NSObject? argument)
		{
			PostNotification (NotificationEnumToInt (notification), argument);
		}

		/// <summary>Posts an accessibility notification using a raw notification identifier.</summary>
		/// <param name="notification">The notification identifier to post.</param>
		/// <param name="argument">The argument to pass with the notification, or <see langword="null"/>.</param>
		public static void PostNotification (int notification, NSObject? argument)
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

		/// <summary>Notifies the accessibility system that the zoom focus has changed.</summary>
		/// <param name="type">The type of zoom focus change.</param>
		/// <param name="frame">The frame to zoom to.</param>
		/// <param name="view">The view containing the frame, or <see langword="null"/>.</param>
		public static void ZoomFocusChanged (UIAccessibilityZoomType type, CGRect frame, UIView? view)
		{
			UIAccessibilityZoomFocusChanged ((IntPtr) type, frame, view is not null ? view.Handle : IntPtr.Zero);
			GC.KeepAlive (view);
		}

		// UIAccessibilityZoom.h
		/// <summary>Informs the user that the accessibility zoom gesture conflicts with a gesture used by this application.</summary>
		[DllImport (Constants.UIKitLibrary, EntryPoint = "UIAccessibilityRegisterGestureConflictWithZoom")]
		extern public static void RegisterGestureConflictWithZoom ();

		// UIAccessibility.h
		[DllImport (Constants.UIKitLibrary)]
		extern static /* UIBezierPath* */ IntPtr UIAccessibilityConvertPathToScreenCoordinates (/* UIBezierPath* */ IntPtr path, /* UIView* */ IntPtr view);

		/// <summary>Converts the specified path to screen coordinates.</summary>
		/// <param name="path">The path to convert.</param>
		/// <param name="view">The view whose coordinate system the path is defined in.</param>
		/// <returns>A new <see cref="UIBezierPath"/> with coordinates in screen space.</returns>
		public static UIBezierPath ConvertPathToScreenCoordinates (UIBezierPath path, UIView view)
		{
			ArgumentNullException.ThrowIfNull (path);
			ArgumentNullException.ThrowIfNull (view);

			UIBezierPath result = new UIBezierPath (UIAccessibilityConvertPathToScreenCoordinates (path.Handle, view.Handle));
			GC.KeepAlive (path);
			GC.KeepAlive (view);
			return result;
		}

		// UIAccessibility.h
		[DllImport (Constants.UIKitLibrary)]
		extern static CGRect UIAccessibilityConvertFrameToScreenCoordinates (CGRect rect, /* UIView* */ IntPtr view);

		/// <summary>Converts the specified rectangle to screen coordinates.</summary>
		/// <param name="rect">The rectangle in view coordinates to convert.</param>
		/// <param name="view">The view whose coordinate system the rectangle is defined in.</param>
		/// <returns>A rectangle with coordinates in screen space.</returns>
		public static CGRect ConvertFrameToScreenCoordinates (CGRect rect, UIView view)
		{
			ArgumentNullException.ThrowIfNull (view);

			var result = UIAccessibilityConvertFrameToScreenCoordinates (rect, view.Handle);
			GC.KeepAlive (view);
			return result;
		}

		// UIAccessibility.h
		[DllImport (Constants.UIKitLibrary)]
		extern unsafe static void UIAccessibilityRequestGuidedAccessSession (/* BOOL */ byte enable, /* void(^completionHandler)(BOOL didSucceed) */ BlockLiteral* completionHandler);

		/// <summary>Requests that the system enter or leave Guided Access mode.</summary>
		/// <param name="enable"><see langword="true"/> to enter Guided Access mode; <see langword="false"/> to leave it.</param>
		/// <param name="completionHandler">A callback invoked when the transition completes, with a parameter indicating success.</param>
		/// <remarks>
		///   <para>When an application is running in Guided Access mode, it can prevent the home button from working, and can control other features of the operating system from working.</para>
		///   <para>For this API call to succeed, the application must be Supervised, and the application must have been enabled for single app mode using Mobile Device Management.</para>
		/// </remarks>
		[BindingImpl (BindingImplOptions.Optimizable)]
		public static void RequestGuidedAccessSession (bool enable, Action<bool> completionHandler)
		{
			unsafe {
				delegate* unmanaged<IntPtr, byte, void> trampoline = &TrampolineRequestGuidedAccessSession;
				using var block = new BlockLiteral (trampoline, completionHandler, typeof (UIAccessibility), nameof (TrampolineRequestGuidedAccessSession));
				UIAccessibilityRequestGuidedAccessSession (enable ? (byte) 1 : (byte) 0, &block);
			}
		}

		/// <summary>Asynchronously requests a transition between normal and Guided Access modes.</summary>
		/// <param name="enable"><see langword="true"/> to enter Guided Access mode; <see langword="false"/> to leave it.</param>
		/// <returns>A task that completes with <see langword="true"/> if the transition succeeded; otherwise, <see langword="false"/>.</returns>
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

		[DllImport (Constants.UIKitLibrary)]
		static extern byte UIAccessibilityDarkerSystemColorsEnabled ();

		/// <summary>Gets a value indicating whether darker system colors are enabled.</summary>
		/// <value><see langword="true"/> if darker system colors are enabled; otherwise, <see langword="false"/>.</value>
		public static bool DarkerSystemColorsEnabled {
			get {
				return UIAccessibilityDarkerSystemColorsEnabled () != 0;
			}
		}

		[DllImport (Constants.UIKitLibrary)]
		static extern byte UIAccessibilityIsBoldTextEnabled ();

		/// <summary>Gets a value indicating whether bold text is enabled.</summary>
		/// <value><see langword="true"/> if bold text is enabled; otherwise, <see langword="false"/>.</value>
		public static bool IsBoldTextEnabled {
			get {
				return UIAccessibilityIsBoldTextEnabled () != 0;
			}
		}

		[SupportedOSPlatform ("tvos14.0")]
		[SupportedOSPlatform ("ios14.0")]
		[SupportedOSPlatform ("maccatalyst")]
		[ObsoletedOSPlatform ("tvos26.1", "Use 'AXSettings.ShowBordersEnabled' instead.")]
		[ObsoletedOSPlatform ("ios26.1", "Use 'AXSettings.ShowBordersEnabled' instead.")]
		[ObsoletedOSPlatform ("maccatalyst26.1", "Use 'AXSettings.ShowBordersEnabled' instead.")]
		[DllImport (Constants.UIKitLibrary)]
		static extern byte UIAccessibilityButtonShapesEnabled ();

		/// <summary>Gets a value indicating whether button shapes are enabled.</summary>
		/// <value><see langword="true"/> if button shapes are enabled; otherwise, <see langword="false"/>.</value>
		[SupportedOSPlatform ("tvos14.0")]
		[SupportedOSPlatform ("ios14.0")]
		[SupportedOSPlatform ("maccatalyst")]
		[ObsoletedOSPlatform ("tvos26.1", "Use 'AXSettings.ShowBordersEnabled' instead.")]
		[ObsoletedOSPlatform ("ios26.1", "Use 'AXSettings.ShowBordersEnabled' instead.")]
		[ObsoletedOSPlatform ("maccatalyst26.1", "Use 'AXSettings.ShowBordersEnabled' instead.")]
		public static bool ButtonShapesEnabled => UIAccessibilityButtonShapesEnabled () != 0;

		[DllImport (Constants.UIKitLibrary)]
		static extern byte UIAccessibilityIsGrayscaleEnabled ();

		/// <summary>Gets a value indicating whether grayscale mode is enabled.</summary>
		/// <value><see langword="true"/> if grayscale mode is enabled; otherwise, <see langword="false"/>.</value>
		static public bool IsGrayscaleEnabled {
			get {
				return UIAccessibilityIsGrayscaleEnabled () != 0;
			}
		}

		[DllImport (Constants.UIKitLibrary)]
		static extern byte UIAccessibilityIsReduceMotionEnabled ();

		/// <summary>Gets a value indicating whether reduce motion is enabled.</summary>
		/// <value><see langword="true"/> if reduce motion is enabled; otherwise, <see langword="false"/>.</value>
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

		/// <summary>Gets a value indicating whether the user prefers cross-fade transitions over sliding transitions.</summary>
		/// <value><see langword="true"/> if cross-fade transitions are preferred; otherwise, <see langword="false"/>.</value>
		[SupportedOSPlatform ("tvos14.0")]
		[SupportedOSPlatform ("ios14.0")]
		[SupportedOSPlatform ("maccatalyst")]
		public static bool PrefersCrossFadeTransitions => UIAccessibilityPrefersCrossFadeTransitions () != 0;

		[SupportedOSPlatform ("ios13.0")]
		[SupportedOSPlatform ("tvos13.0")]
		[SupportedOSPlatform ("maccatalyst")]
		[DllImport (Constants.UIKitLibrary)]
		static extern byte UIAccessibilityIsVideoAutoplayEnabled ();

		/// <summary>Gets a value indicating whether videos should autoplay.</summary>
		/// <value><see langword="true"/> if video autoplay is enabled; otherwise, <see langword="false"/>.</value>
		[SupportedOSPlatform ("ios13.0")]
		[SupportedOSPlatform ("tvos13.0")]
		[SupportedOSPlatform ("maccatalyst")]
		static public bool IsVideoAutoplayEnabled => UIAccessibilityIsVideoAutoplayEnabled () != 0;

		[DllImport (Constants.UIKitLibrary)]
		static extern byte UIAccessibilityIsReduceTransparencyEnabled ();

		/// <summary>Gets a value indicating whether reduce transparency is enabled.</summary>
		/// <value><see langword="true"/> if reduce transparency is enabled; otherwise, <see langword="false"/>.</value>
		static public bool IsReduceTransparencyEnabled {
			get {
				return UIAccessibilityIsReduceTransparencyEnabled () != 0;
			}
		}

		[DllImport (Constants.UIKitLibrary)]
		static extern byte UIAccessibilityIsSwitchControlRunning ();

		/// <summary>Gets a value indicating whether Switch Control is running.</summary>
		/// <value><see langword="true"/> if Switch Control is running; otherwise, <see langword="false"/>.</value>
		static public bool IsSwitchControlRunning {
			get {
				return UIAccessibilityIsSwitchControlRunning () != 0;
			}
		}

		[DllImport (Constants.UIKitLibrary)]
		static extern byte UIAccessibilityIsSpeakSelectionEnabled ();

		/// <summary>Gets a value indicating whether Speak Selection is enabled.</summary>
		/// <value><see langword="true"/> if Speak Selection is enabled; otherwise, <see langword="false"/>.</value>
		static public bool IsSpeakSelectionEnabled {
			get {
				return UIAccessibilityIsSpeakSelectionEnabled () != 0;
			}
		}

		[DllImport (Constants.UIKitLibrary)]
		static extern byte UIAccessibilityIsSpeakScreenEnabled ();

		/// <summary>Gets a value indicating whether Speak Screen is enabled.</summary>
		/// <value><see langword="true"/> if Speak Screen is enabled; otherwise, <see langword="false"/>.</value>
		static public bool IsSpeakScreenEnabled {
			get {
				return UIAccessibilityIsSpeakScreenEnabled () != 0;
			}
		}

		[DllImport (Constants.UIKitLibrary)]
		static extern byte UIAccessibilityIsAssistiveTouchRunning ();

		/// <summary>Gets a value indicating whether AssistiveTouch is running.</summary>
		/// <value><see langword="true"/> if AssistiveTouch is running; otherwise, <see langword="false"/>.</value>
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

		/// <summary>Gets a value indicating whether the user prefers UI elements to differentiate without relying solely on color.</summary>
		/// <value><see langword="true"/> if differentiation without color is preferred; otherwise, <see langword="false"/>.</value>
		[SupportedOSPlatform ("ios13.0")]
		[SupportedOSPlatform ("tvos13.0")]
		[SupportedOSPlatform ("maccatalyst")]
		public static bool ShouldDifferentiateWithoutColor => UIAccessibilityShouldDifferentiateWithoutColor () != 0;

		[SupportedOSPlatform ("ios13.0")]
		[SupportedOSPlatform ("tvos13.0")]
		[SupportedOSPlatform ("maccatalyst")]
		[DllImport (Constants.UIKitLibrary)]
		static extern byte UIAccessibilityIsOnOffSwitchLabelsEnabled ();

		/// <summary>Gets a value indicating whether on/off switch labels are enabled.</summary>
		/// <value><see langword="true"/> if on/off switch labels are enabled; otherwise, <see langword="false"/>.</value>
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

		/// <summary>Gets the pairing status of a hearing device to one, both, or no ears.</summary>
		/// <value>A <see cref="UIAccessibilityHearingDeviceEar"/> value indicating which ears have paired hearing devices.</value>
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
