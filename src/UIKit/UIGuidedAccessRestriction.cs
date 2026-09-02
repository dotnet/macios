//
// UIGuidedAccessRestriction
//
// Authors:
//	Sebastien Pouliot <sebastien@xamarin.com>
//
// Copyrigh 2013-2014 Xamarin Inc.
//

using System.Threading.Tasks;
using UIKit;

#nullable enable

namespace UIKit {

	/// <summary>A static class that provides a method to determine the state of a Guided Access restriction.</summary>
	public static partial class UIGuidedAccessRestriction {
#if !COREBUILD
		[DllImport (Constants.UIKitLibrary)]
		extern static /* UIGuidedAccessRestrictionState */ nint UIGuidedAccessRestrictionStateForIdentifier (/* NSString */ IntPtr restrictionIdentifier);

		/// <summary>Returns the state (allow, deny) for the specified <paramref name="restrictionIdentifier" />.</summary>
		/// <param name="restrictionIdentifier">The identifier of the restriction.</param>
		/// <returns>
		/// <see cref="UIGuidedAccessRestrictionState.Allow" /> means that the application should allow the behavior.
		/// <see cref="UIGuidedAccessRestrictionState.Deny" /> means that the application should not allow the behavior.
		/// </returns>
		/// <remarks>
		/// You can enable Guided Access mode by calling <see cref="UIAccessibility.RequestGuidedAccessSession(bool, Action{bool})" />.
		/// </remarks>
		public static UIGuidedAccessRestrictionState GetState (string restrictionIdentifier)
		{
			IntPtr p = NSString.CreateNative (restrictionIdentifier);
			var result = UIGuidedAccessRestrictionStateForIdentifier (p);
			NSString.ReleaseNative (p);
			return (UIGuidedAccessRestrictionState) (int) result;
		}

#if IOS
		[DllImport (Constants.UIKitLibrary)]
		static unsafe extern void UIGuidedAccessConfigureAccessibilityFeatures (/* UIGuidedAccessAccessibilityFeature */ nuint features, byte enabled, BlockLiteral* completion);

		/// <summary>A handler that is called when the guided access accessibility features configuration completes.</summary>
		/// <param name="success">Whether the configuration was successful.</param>
		/// <param name="error">The error that occurred, or <see langword="null" /> if the operation was successful.</param>
		public delegate void UIGuidedAccessConfigureAccessibilityFeaturesCompletionHandler (bool success, NSError? error);

		static internal class UIGuidedAccessConfigureAccessibilityFeaturesTrampoline {
			[UnmanagedCallersOnlyAttribute]
			internal static unsafe void Invoke (IntPtr block, byte success, IntPtr error)
			{
				var del = BlockLiteral.GetTarget<UIGuidedAccessConfigureAccessibilityFeaturesCompletionHandler> (block);
				if (del is not null)
					del (success != 0, Runtime.GetNSObject<NSError> (error));
			}
		}

		/// <summary>Configures the specified guided access accessibility features.</summary>
		/// <param name="features">The accessibility features to configure.</param>
		/// <param name="enabled">Whether the features should be enabled or disabled.</param>
		/// <param name="completionHandler">The handler that is called when the configuration completes.</param>
		[BindingImpl (BindingImplOptions.Optimizable)]
		public static void ConfigureAccessibilityFeatures (UIGuidedAccessAccessibilityFeature features, bool enabled, UIGuidedAccessConfigureAccessibilityFeaturesCompletionHandler completionHandler)
		{
			ArgumentNullException.ThrowIfNull (completionHandler);

			unsafe {
				delegate* unmanaged<IntPtr, byte, IntPtr, void> trampoline = &UIGuidedAccessConfigureAccessibilityFeaturesTrampoline.Invoke;
				using var block = new BlockLiteral (trampoline, completionHandler, typeof (UIGuidedAccessConfigureAccessibilityFeaturesTrampoline), nameof (UIGuidedAccessConfigureAccessibilityFeaturesTrampoline.Invoke));
				UIGuidedAccessConfigureAccessibilityFeatures ((nuint) (ulong) features, enabled ? (byte) 1 : (byte) 0, &block);
			}
		}

		/// <summary>Asynchronously configures the specified guided access accessibility features.</summary>
		/// <param name="features">The accessibility features to configure.</param>
		/// <param name="enabled">Whether the features should be enabled or disabled.</param>
		/// <returns>A task that represents the asynchronous operation, containing a tuple with the success status and any error that occurred.</returns>
		[BindingImpl (BindingImplOptions.Optimizable)]
		public static Task<(bool Success, NSError? Error)> ConfigureAccessibilityFeaturesAsync (UIGuidedAccessAccessibilityFeature features, bool enabled)
		{
			var tcs = new TaskCompletionSource<(bool, NSError?)> ();
			ConfigureAccessibilityFeatures (features, enabled, (success_, error_) => tcs.SetResult ((success_, error_)));
			return tcs.Task;
		}
#endif
#endif // !COREBUILD
	}
}
