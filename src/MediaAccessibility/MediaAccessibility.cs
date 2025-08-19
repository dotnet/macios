//
// MediaAccessibility.cs: binding for iOS (7+) MediaAccessibility framework
//
// Authors:
//   Aaron Bockover (abock@xamarin.com)
//
// Copyright 2013, 2015 Xamarin Inc.

#nullable enable

using System;
using System.Runtime.InteropServices;

using ObjCRuntime;
using CoreFoundation;
using CoreGraphics;
using CoreText;
using Foundation;

namespace MediaAccessibility {
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("tvos")]
	public static partial class MACaptionAppearance {
		[DllImport (Constants.MediaAccessibilityLibrary)]
		static extern byte MACaptionAppearanceAddSelectedLanguage (nint domain,
			/* CFStringRef __nonnull */ IntPtr language);

		/// <param name="domain">To be added.</param>
		///         <param name="language">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static bool AddSelectedLanguage (MACaptionAppearanceDomain domain, string language)
		{
			// this will throw an ANE if language is null
			using (var lang = new CFString (language)) {
				bool result = MACaptionAppearanceAddSelectedLanguage ((int) domain, lang.Handle) != 0;
				GC.KeepAlive (lang);
				return result;
			}
		}

		[DllImport (Constants.MediaAccessibilityLibrary)]
		static extern /* CFArrayRef __nonnull */ IntPtr MACaptionAppearanceCopySelectedLanguages (nint domain);

		/// <param name="domain">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static string? [] GetSelectedLanguages (MACaptionAppearanceDomain domain)
		{
			using (var langs = new CFArray (MACaptionAppearanceCopySelectedLanguages ((int) domain), owns: true)) {
				var languages = new string? [langs.Count];
				for (int i = 0; i < langs.Count; i++) {
					languages [i] = CFString.FromHandle (langs.GetValue (i));
				}
				return languages;
			}
		}

		[DllImport (Constants.MediaAccessibilityLibrary)]
		static extern nint MACaptionAppearanceGetDisplayType (nint domain);

		/// <param name="domain">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static MACaptionAppearanceDisplayType GetDisplayType (MACaptionAppearanceDomain domain)
		{
			return (MACaptionAppearanceDisplayType) (int) MACaptionAppearanceGetDisplayType ((int) domain);
		}

		[DllImport (Constants.MediaAccessibilityLibrary)]
		static extern void MACaptionAppearanceSetDisplayType (nint domain, nint displayType);

		/// <param name="domain">To be added.</param>
		///         <param name="displayType">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public static void SetDisplayType (MACaptionAppearanceDomain domain, MACaptionAppearanceDisplayType displayType)
		{
			MACaptionAppearanceSetDisplayType ((int) domain, (int) displayType);
		}

		[DllImport (Constants.MediaAccessibilityLibrary)]
		static extern /* CFArrayRef __nonnull */ IntPtr MACaptionAppearanceCopyPreferredCaptioningMediaCharacteristics (nint domain);

		/// <param name="domain">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static NSString [] GetPreferredCaptioningMediaCharacteristics (MACaptionAppearanceDomain domain)
		{
			using (var chars = new CFArray (MACaptionAppearanceCopyPreferredCaptioningMediaCharacteristics ((int) domain), owns: true)) {
				NSString [] characteristics = new NSString [chars.Count];
				for (int i = 0; i < chars.Count; i++) {
					characteristics [i] = new NSString (chars.GetValue (i));
				}
				return characteristics;
			}
		}

		[DllImport (Constants.MediaAccessibilityLibrary)]
		unsafe static extern /* CGColorRef __nonnull */ IntPtr MACaptionAppearanceCopyForegroundColor (nint domain,
			/* MACaptionAppearanceBehavior * __nullable */ nint* behavior);

		/// <param name="domain">To be added.</param>
		///         <param name="behavior">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static CGColor GetForegroundColor (MACaptionAppearanceDomain domain, ref MACaptionAppearanceBehavior behavior)
		{
			nint b = (int) behavior;
			IntPtr handle;
			unsafe {
				handle = MACaptionAppearanceCopyForegroundColor ((int) domain, &b);
			}
			var rv = new CGColor (handle, owns: true);
			behavior = (MACaptionAppearanceBehavior) (int) b;
			return rv;
		}

		[DllImport (Constants.MediaAccessibilityLibrary)]
		unsafe static extern /* CGColorRef __nonnull */ IntPtr MACaptionAppearanceCopyBackgroundColor (nint domain,
			/* MACaptionAppearanceBehavior * __nullable */ nint* behavior);

		/// <param name="domain">To be added.</param>
		///         <param name="behavior">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static CGColor GetBackgroundColor (MACaptionAppearanceDomain domain, ref MACaptionAppearanceBehavior behavior)
		{
			nint b = (int) behavior;
			IntPtr handle;
			unsafe {
				handle = MACaptionAppearanceCopyBackgroundColor ((int) domain, &b);
			}
			var rv = new CGColor (handle, owns: true);
			behavior = (MACaptionAppearanceBehavior) (int) b;
			return rv;
		}

		[DllImport (Constants.MediaAccessibilityLibrary)]
		unsafe static extern /* CGColorRef __nonnull */ IntPtr MACaptionAppearanceCopyWindowColor (nint domain,
			/* MACaptionAppearanceBehavior * __nullable */ nint* behavior);

		/// <param name="domain">To be added.</param>
		///         <param name="behavior">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static CGColor GetWindowColor (MACaptionAppearanceDomain domain, ref MACaptionAppearanceBehavior behavior)
		{
			nint b = (int) behavior;
			IntPtr handle;
			unsafe {
				handle = MACaptionAppearanceCopyWindowColor ((int) domain, &b);
			}
			var rv = new CGColor (handle, owns: true);
			behavior = (MACaptionAppearanceBehavior) (int) b;
			return rv;
		}

		[DllImport (Constants.MediaAccessibilityLibrary)]
		unsafe static extern nfloat MACaptionAppearanceGetForegroundOpacity (nint domain, nint* behavior);

		/// <param name="domain">To be added.</param>
		///         <param name="behavior">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static nfloat GetForegroundOpacity (MACaptionAppearanceDomain domain, ref MACaptionAppearanceBehavior behavior)
		{
			nint b = (int) behavior;
			nfloat rv;
			unsafe {
				rv = MACaptionAppearanceGetForegroundOpacity ((int) domain, &b);
			}
			behavior = (MACaptionAppearanceBehavior) (int) b;
			return rv;
		}

		[DllImport (Constants.MediaAccessibilityLibrary)]
		unsafe static extern nfloat MACaptionAppearanceGetBackgroundOpacity (nint domain,
			/* MACaptionAppearanceBehavior * __nullable */ nint* behavior);

		/// <param name="domain">To be added.</param>
		///         <param name="behavior">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static nfloat GetBackgroundOpacity (MACaptionAppearanceDomain domain, ref MACaptionAppearanceBehavior behavior)
		{
			nint b = (int) behavior;
			nfloat rv;
			unsafe {
				rv = MACaptionAppearanceGetBackgroundOpacity ((int) domain, &b);
			}
			behavior = (MACaptionAppearanceBehavior) (int) b;
			return rv;
		}

		[DllImport (Constants.MediaAccessibilityLibrary)]
		unsafe static extern nfloat MACaptionAppearanceGetWindowOpacity (nint domain,
			/* MACaptionAppearanceBehavior * __nullable */ nint* behavior);

		/// <param name="domain">To be added.</param>
		///         <param name="behavior">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static nfloat GetWindowOpacity (MACaptionAppearanceDomain domain, ref MACaptionAppearanceBehavior behavior)
		{
			nint b = (int) behavior;
			nfloat rv;
			unsafe {
				rv = MACaptionAppearanceGetWindowOpacity ((int) domain, &b);
			}
			behavior = (MACaptionAppearanceBehavior) (int) b;
			return rv;
		}

		[DllImport (Constants.MediaAccessibilityLibrary)]
		unsafe static extern nfloat MACaptionAppearanceGetWindowRoundedCornerRadius (nint domain,
			/* MACaptionAppearanceBehavior * __nullable */ nint* behavior);

		/// <param name="domain">To be added.</param>
		///         <param name="behavior">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static nfloat GetWindowRoundedCornerRadius (MACaptionAppearanceDomain domain, ref MACaptionAppearanceBehavior behavior)
		{
			nint b = (int) behavior;
			nfloat rv;
			unsafe {
				rv = MACaptionAppearanceGetWindowRoundedCornerRadius ((int) domain, &b);
			}
			behavior = (MACaptionAppearanceBehavior) (int) b;
			return rv;
		}

		[DllImport (Constants.MediaAccessibilityLibrary)]
		unsafe static extern /* CTFontDescriptorRef __nonnull */ IntPtr MACaptionAppearanceCopyFontDescriptorForStyle (nint domain,
			/* MACaptionAppearanceBehavior * __nullable */ nint* behavior, nint fontStyle);

		/// <param name="domain">To be added.</param>
		///         <param name="behavior">To be added.</param>
		///         <param name="fontStyle">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static CTFontDescriptor GetFontDescriptor (MACaptionAppearanceDomain domain, ref MACaptionAppearanceBehavior behavior, MACaptionAppearanceFontStyle fontStyle)
		{
			nint b = (int) behavior;
			IntPtr handle;
			unsafe {
				handle = MACaptionAppearanceCopyFontDescriptorForStyle ((int) domain, &b, (int) fontStyle);
			}
			var rv = new CTFontDescriptor (handle, owns: true);
			behavior = (MACaptionAppearanceBehavior) (int) b;
			return rv;
		}

		[DllImport (Constants.MediaAccessibilityLibrary)]
		unsafe static extern nfloat MACaptionAppearanceGetRelativeCharacterSize (nint domain,
			/* MACaptionAppearanceBehavior * __nullable */ nint* behavior);

		/// <param name="domain">To be added.</param>
		///         <param name="behavior">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static nfloat GetRelativeCharacterSize (MACaptionAppearanceDomain domain, ref MACaptionAppearanceBehavior behavior)
		{
			nint b = (int) behavior;
			nfloat rv;
			unsafe {
				rv = MACaptionAppearanceGetRelativeCharacterSize ((int) domain, &b);
			}
			behavior = (MACaptionAppearanceBehavior) (int) b;
			return rv;
		}

		[DllImport (Constants.MediaAccessibilityLibrary)]
		unsafe static extern nint MACaptionAppearanceGetTextEdgeStyle (nint domain,
			/* MACaptionAppearanceBehavior * __nullable */ nint* behavior);

		/// <param name="domain">To be added.</param>
		///         <param name="behavior">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static MACaptionAppearanceTextEdgeStyle GetTextEdgeStyle (MACaptionAppearanceDomain domain, ref MACaptionAppearanceBehavior behavior)
		{
			nint b = (int) behavior;
			nint rv;
			unsafe {
				rv = MACaptionAppearanceGetTextEdgeStyle ((int) domain, &b);
			}
			behavior = (MACaptionAppearanceBehavior) (int) b;
			return (MACaptionAppearanceTextEdgeStyle) (int) rv;
		}

		[SupportedOSPlatform ("tvos13.0")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios13.0")]
		[SupportedOSPlatform ("maccatalyst")]
		[DllImport (Constants.MediaAccessibilityLibrary)]
		static extern void MACaptionAppearanceDidDisplayCaptions (IntPtr /* CFArratRef */ strings);

		[SupportedOSPlatform ("tvos13.0")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios13.0")]
		[SupportedOSPlatform ("maccatalyst")]
		public static void DidDisplayCaptions (string [] strings)
		{
			if ((strings is null) || (strings.Length == 0))
				MACaptionAppearanceDidDisplayCaptions (IntPtr.Zero);
			else {
				using (var array = NSArray.FromStrings (strings))
					MACaptionAppearanceDidDisplayCaptions (array.Handle);
			}
		}

		[SupportedOSPlatform ("tvos13.0")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios13.0")]
		[SupportedOSPlatform ("maccatalyst")]
		public static void DidDisplayCaptions (NSAttributedString [] strings)
		{
			// CFAttributedString is “toll-free bridged” with its Foundation counterpart, NSAttributedString.
			// https://developer.apple.com/documentation/corefoundation/cfattributedstring?language=objc
			if ((strings is null) || (strings.Length == 0))
				MACaptionAppearanceDidDisplayCaptions (IntPtr.Zero);
			else {
				using (var array = NSArray.FromNSObjects (strings))
					MACaptionAppearanceDidDisplayCaptions (array.Handle);
			}
		}

		[SupportedOSPlatform ("tvos18.0")]
		[SupportedOSPlatform ("macos15.0")]
		[SupportedOSPlatform ("ios18.0")]
		[SupportedOSPlatform ("maccatalyst18.0")]
		[DllImport (Constants.MediaAccessibilityLibrary)]
		static extern byte MACaptionAppearanceIsCustomized (nint /* MACaptionAppearanceDomain */ domain);

		/// <summary>Checks whether the specified style has been customized by the user.</summary>
		/// <param name="domain">The style to check</param>
		/// <returns>True if the specified style has been customized by the user, false otherwise.</returns>
		[SupportedOSPlatform ("tvos18.0")]
		[SupportedOSPlatform ("macos15.0")]
		[SupportedOSPlatform ("ios18.0")]
		[SupportedOSPlatform ("maccatalyst18.0")]
		public static bool IsCustomized (MACaptionAppearanceDomain domain)
		{
			return MACaptionAppearanceIsCustomized ((nint) (long) domain) != 0;
		}

		[SupportedOSPlatform ("tvos26.0")]
		[SupportedOSPlatform ("macos26.0")]
		[SupportedOSPlatform ("ios26.0")]
		[SupportedOSPlatform ("maccatalyst26.0")]
		[DllImport (Constants.MediaAccessibilityLibrary)]
		static extern /* CFArrayRef CF_RETURNS_RETAINED */ IntPtr MACaptionAppearanceCopyProfileIDs ();

		/// <summary>Gets all the system and user-defined profile identifiers.</summary>
		/// <returns>A string array of profile identifiers.</returns>
		[SupportedOSPlatform ("tvos26.0")]
		[SupportedOSPlatform ("macos26.0")]
		[SupportedOSPlatform ("ios26.0")]
		[SupportedOSPlatform ("maccatalyst26.0")]
		public static string []? GetProfileIds ()
		{
			var handle = MACaptionAppearanceCopyProfileIDs ();
			var rv = CFArray.StringArrayFromHandle (handle, releaseHandle: true);
			return (string []?) (object?) rv;
		}

		[SupportedOSPlatform ("tvos26.0")]
		[SupportedOSPlatform ("macos26.0")]
		[SupportedOSPlatform ("ios26.0")]
		[SupportedOSPlatform ("maccatalyst26.0")]
		[DllImport (Constants.MediaAccessibilityLibrary)]
		static extern void MACaptionAppearanceSetActiveProfileID (IntPtr /* CFStringRef */ profileID);

		[SupportedOSPlatform ("tvos26.0")]
		[SupportedOSPlatform ("macos26.0")]
		[SupportedOSPlatform ("ios26.0")]
		[SupportedOSPlatform ("maccatalyst26.0")]
		[DllImport (Constants.MediaAccessibilityLibrary)]
		static extern /* CFStringRef CF_RETURNS_RETAINED */ IntPtr MACaptionAppearanceCopyActiveProfileID ();

		/// <summary>Gets or sets the currently active system-wide caption-drawing profile by its identifier.</summary>
		[SupportedOSPlatform ("tvos26.0")]
		[SupportedOSPlatform ("macos26.0")]
		[SupportedOSPlatform ("ios26.0")]
		[SupportedOSPlatform ("maccatalyst26.0")]
		public static string? ActiveProfileId {
			get {
				return CFString.FromHandle (MACaptionAppearanceCopyActiveProfileID (), true);
			}
			set {
				if (value is null)
					ThrowHelper.ThrowArgumentNullException (nameof (value));

				using var profileIdHandle = new TransientCFString (value);
				MACaptionAppearanceSetActiveProfileID (profileIdHandle);
			}
		}

		[SupportedOSPlatform ("tvos26.0")]
		[SupportedOSPlatform ("macos26.0")]
		[SupportedOSPlatform ("ios26.0")]
		[SupportedOSPlatform ("maccatalyst26.0")]
		[DllImport (Constants.MediaAccessibilityLibrary)]
		static extern /* CFStringRef CF_RETURNS_RETAINED */ IntPtr MACaptionAppearanceCopyProfileName (/* CFStringRef */ IntPtr profileId);

		/// <summary>Get the human-readable name for a given profile identifier.</summary>
		/// <param name="profileId">The profile identifier whose human-readable name to return.</param>
		/// <returns>The human-readable name for the specified profile identifier.</returns>
		[SupportedOSPlatform ("tvos26.0")]
		[SupportedOSPlatform ("macos26.0")]
		[SupportedOSPlatform ("ios26.0")]
		[SupportedOSPlatform ("maccatalyst26.0")]
		public static string? GetProfileName (string profileId)
		{
			if (profileId is null)
				ThrowHelper.ThrowArgumentNullException (nameof (profileId));

			using var profileIdHandle = new TransientCFString (profileId);
			return CFString.FromHandle (MACaptionAppearanceCopyProfileName (profileIdHandle), true);
		}

		[SupportedOSPlatform ("tvos26.0")]
		[SupportedOSPlatform ("macos26.0")]
		[SupportedOSPlatform ("ios26.0")]
		[SupportedOSPlatform ("maccatalyst26.0")]
		[DllImport (Constants.MediaAccessibilityLibrary)]
		unsafe static extern void MACaptionAppearanceExecuteBlockForProfileID (IntPtr /* CFStringRef */ profileId, BlockLiteral* aBlock);

		/// <summary>Execute a callback as if the specified profile was the currently active profile.</summary>
		/// <param name="profileId">The identifier for the profile that will be active when the callback is executed.</param>
		/// <param name="callback">The callback to call with the specified profile as the currently active profile.</param>
		/// <remarks>This method can be used to get the fonts and colors for a profile without changing the currently selected profile.</remarks>
		[SupportedOSPlatform ("tvos26.0")]
		[SupportedOSPlatform ("macos26.0")]
		[SupportedOSPlatform ("ios26.0")]
		[SupportedOSPlatform ("maccatalyst26.0")]
		[BindingImpl (BindingImplOptions.Optimizable)]
		public static void ExecuteCallbackForProfile (string profileId, Action callback)
		{
			if (profileId is null)
				ThrowHelper.ThrowArgumentNullException (nameof (profileId));

			if (callback is null)
				ThrowHelper.ThrowArgumentNullException (nameof (callback));

			using var profileIdHandle = new TransientCFString (profileId);

			unsafe {
				delegate* unmanaged<IntPtr, void> trampoline = &Callback;
				using var block = new BlockLiteral (trampoline, callback, typeof (MACaptionAppearance), nameof (Callback));
				MACaptionAppearanceExecuteBlockForProfileID (profileIdHandle, &block);
			}
		}

		[UnmanagedCallersOnly]
		static void Callback (IntPtr block)
		{
			var del = BlockLiteral.GetTarget<Action> (block);
			if (del is null)
				return;

			del ();
		}
	}

	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	static partial class MAAudibleMedia {
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		[DllImport (Constants.MediaAccessibilityLibrary)]
		static extern unsafe IntPtr /* CFArrayRef __nonnull */ MAAudibleMediaCopyPreferredCharacteristics ();

		// according to webkit source code (the only use I could find) this is an array of CFString
		// https://github.com/WebKit/webkit/blob/master/Source/WebCore/page/CaptionUserPreferencesMediaAF.cpp
		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		static public string? []? GetPreferredCharacteristics ()
		{
			var handle = MAAudibleMediaCopyPreferredCharacteristics ();
			if (handle == IntPtr.Zero)
				return null;
			var result = CFArray.StringArrayFromHandle (handle);
			CFObject.CFRelease (handle); // *Copy* API
			return result;
		}
	}

	[SupportedOSPlatform ("ios16.4")]
	[SupportedOSPlatform ("maccatalyst16.4")]
	[SupportedOSPlatform ("macos13.3")]
	[SupportedOSPlatform ("tvos16.4")]
	public static partial class MAVideoAccommodations {
		[SupportedOSPlatform ("ios16.4")]
		[SupportedOSPlatform ("maccatalyst16.4")]
		[SupportedOSPlatform ("macos13.3")]
		[SupportedOSPlatform ("tvos16.4")]
		[DllImport (Constants.MediaAccessibilityLibrary)]
		static extern byte MADimFlashingLightsEnabled ();

		[SupportedOSPlatform ("ios16.4")]
		[SupportedOSPlatform ("maccatalyst16.4")]
		[SupportedOSPlatform ("macos13.3")]
		[SupportedOSPlatform ("tvos16.4")]
		public static bool IsDimFlashingLightsEnabled () => MADimFlashingLightsEnabled () != 0;
	}
}
