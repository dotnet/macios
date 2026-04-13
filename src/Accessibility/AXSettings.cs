#nullable enable

using CoreGraphics;

namespace Accessibility {

	[SupportedOSPlatform ("ios18.0")]
	[SupportedOSPlatform ("maccatalyst18.0")]
	[SupportedOSPlatform ("macos15.0")]
	[SupportedOSPlatform ("tvos18.0")]
	[Native]
	public enum AXSettingsFeature : long {
		/// <summary>Jump to the "Allow Apps to Request to Use" setting in Personal Voice.</summary>
		PersonalVoiceAllowAppsToRequestToUse = 1,
		/// <summary>Jump to the "Allow Apps to Add Audio to Calls." setting in Personal Voice.</summary>
		[SupportedOSPlatform ("ios18.2")]
		[SupportedOSPlatform ("maccatalyst18.2")]
		[SupportedOSPlatform ("macos15.2")]
		[SupportedOSPlatform ("tvos18.2")]
		AllowAppsToAddAudioToCalls,
		[SupportedOSPlatform ("ios26.0")]
		[SupportedOSPlatform ("maccatalyst26.0")]
		[SupportedOSPlatform ("macos26.0")]
		[SupportedOSPlatform ("tvos26.0")]
		AssistiveTouch,
		[SupportedOSPlatform ("ios26.0")]
		[SupportedOSPlatform ("maccatalyst26.0")]
		[SupportedOSPlatform ("macos26.0")]
		[SupportedOSPlatform ("tvos26.0")]
		AssistiveTouchDevices,
		[SupportedOSPlatform ("ios26.0")]
		[SupportedOSPlatform ("maccatalyst26.0")]
		[SupportedOSPlatform ("macos26.0")]
		[SupportedOSPlatform ("tvos26.0")]
		DwellControl,
		/// <summary>Jump to the caption styles setting.</summary>
		[SupportedOSPlatform ("ios26.4")]
		[SupportedOSPlatform ("maccatalyst26.4")]
		[SupportedOSPlatform ("macos26.4")]
		[SupportedOSPlatform ("tvos26.4")]
		CaptionStyles,
	}

	public static partial class AXSettings {
		[SupportedOSPlatform ("ios18.0")]
		[SupportedOSPlatform ("maccatalyst18.0")]
		[SupportedOSPlatform ("macos15.0")]
		[SupportedOSPlatform ("tvos18.0")]
		[DllImport (Constants.AccessibilityLibrary)]
		static extern byte AXAssistiveAccessEnabled ();

		/// <summary>Returns whether Assistive Access is running.</summary>
		[SupportedOSPlatform ("ios18.0")]
		[SupportedOSPlatform ("maccatalyst18.0")]
		[SupportedOSPlatform ("macos15.0")]
		[SupportedOSPlatform ("tvos18.0")]
		public static bool IsAssistiveAccessEnabled {
			get {
				return AXAssistiveAccessEnabled () != 0;
			}
		}

		[SupportedOSPlatform ("ios18.0")]
		[SupportedOSPlatform ("maccatalyst18.0")]
		[SupportedOSPlatform ("macos15.0")]
		[SupportedOSPlatform ("tvos18.0")]
		[DllImport (Constants.AccessibilityLibrary)]
		unsafe static extern void AXOpenSettingsFeature (nint /* AXSettingsFeature */ feature, BlockLiteral* block);

		/// <summary>Open the Settings app to the specified section.</summary>
		/// <param name="feature">The section to open.</param>
		/// <param name="completionHandler">This callback is called when the section has been opened. The <see cref="Foundation.NSError" /> argument will be null if successful.</param>
		[SupportedOSPlatform ("ios18.0")]
		[SupportedOSPlatform ("maccatalyst18.0")]
		[SupportedOSPlatform ("macos15.0")]
		[SupportedOSPlatform ("tvos18.0")]
		[BindingImpl (BindingImplOptions.Optimizable)]
		public unsafe static void OpenSettingsFeature (AXSettingsFeature feature, Action<NSError?> completionHandler)
		{
			delegate* unmanaged<IntPtr, IntPtr, void> trampoline = &OpenSettingsFeatureCompletionHandler;
			using var block = new BlockLiteral (trampoline, completionHandler, typeof (AXSettings), nameof (OpenSettingsFeatureCompletionHandler));
			AXOpenSettingsFeature ((nint) (long) feature, &block);
		}

		[UnmanagedCallersOnly]
		static void OpenSettingsFeatureCompletionHandler (IntPtr block, IntPtr error)
		{
			var del = BlockLiteral.GetTarget<Action<NSError?>> (block);
			if (del is not null) {
				var errorObject = Runtime.GetNSObject<NSError> (error);
				del (errorObject);
			}
		}

		[SupportedOSPlatform ("ios26.1")]
		[SupportedOSPlatform ("maccatalyst26.1")]
		[SupportedOSPlatform ("macos26.1")]
		[SupportedOSPlatform ("tvos26.1")]
		[DllImport (Constants.AccessibilityLibrary)]
		static extern byte AXShowBordersEnabled ();

		[SupportedOSPlatform ("ios26.1")]
		[SupportedOSPlatform ("maccatalyst26.1")]
		[SupportedOSPlatform ("macos26.1")]
		[SupportedOSPlatform ("tvos26.1")]
		public static bool ShowBordersEnabled ()
		{
			return AXShowBordersEnabled () != 0;
		}

		[SupportedOSPlatform ("ios26.4")]
		[SupportedOSPlatform ("maccatalyst26.4")]
		[SupportedOSPlatform ("macos26.4")]
		[SupportedOSPlatform ("tvos26.4")]
		[DllImport (Constants.AccessibilityLibrary)]
		static extern byte AXReduceHighlightingEffectsEnabled ();

		/// <summary>Returns whether the system preference for reduce highlighting effects is enabled.</summary>
		[SupportedOSPlatform ("ios26.4")]
		[SupportedOSPlatform ("maccatalyst26.4")]
		[SupportedOSPlatform ("macos26.4")]
		[SupportedOSPlatform ("tvos26.4")]
		public static bool IsReduceHighlightingEffectsEnabled {
			get {
				return AXReduceHighlightingEffectsEnabled () != 0;
			}
		}

		[SupportedOSPlatform ("ios26.4")]
		[SupportedOSPlatform ("maccatalyst26.4")]
		[SupportedOSPlatform ("macos26.4")]
		[SupportedOSPlatform ("tvos26.4")]
		[DllImport (Constants.AccessibilityLibrary)]
		static extern byte AXOpenSettingsFeatureIsSupported (nint /* AXSettingsFeature */ feature);

		/// <summary>Returns whether the specified settings feature is supported on this device.</summary>
		/// <param name="feature">The settings feature to check.</param>
		/// <returns><see langword="true" /> if the feature is supported; otherwise, <see langword="false" />.</returns>
		[SupportedOSPlatform ("ios26.4")]
		[SupportedOSPlatform ("maccatalyst26.4")]
		[SupportedOSPlatform ("macos26.4")]
		[SupportedOSPlatform ("tvos26.4")]
		public static bool OpenSettingsFeatureIsSupported (AXSettingsFeature feature)
		{
			return AXOpenSettingsFeatureIsSupported ((nint) (long) feature) != 0;
		}
	}
}
