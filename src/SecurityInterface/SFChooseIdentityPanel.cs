#nullable enable

using System;
using AppKit;
using Foundation;
using Security;

namespace SecurityInterface {

	public partial class SFChooseIdentityPanel {

		/// <summary>Gets the policies used to evaluate the displayed identities.</summary>
		public SecPolicy [] Policies {
			get {
				if (_Policies == IntPtr.Zero)
					throw new InvalidOperationException ("The native identity panel returned a null policies array.");
				return NSArray.NonNullArrayFromHandleDropNullElements<SecPolicy> (_Policies);
			}
		}

		/// <summary>Sets the policies used to evaluate the displayed identities.</summary>
		public void SetPolicies (params SecPolicy [] policies)
		{
			ArgumentNullException.ThrowIfNull (policies);
			if (policies.Length == 0)
				throw new ArgumentException ("At least one policy is required.", nameof (policies));
			using var array = NSArray.FromNativeObjects (policies);
			_SetPolicies (array);
			GC.KeepAlive (policies);
		}

		/// <summary>Resets the panel to the default X.509 policy.</summary>
		public void ResetPolicies ()
		{
			_SetPolicies (null);
		}

		/// <summary>Displays an identity chooser sheet and invokes the callback when it closes.</summary>
		public void BeginSheet (NSWindow docWindow, SecIdentity [] identities, string? message, Action<NSModalResponse> didEnd)
		{
			var dispatcher = SecurityInterfaceSheetDidEndDispatcher.Create (didEnd);
			try {
				BeginSheet (docWindow, dispatcher, SecurityInterfaceSheetDidEndDispatcher.Selector, IntPtr.Zero, identities, message);
			} catch {
				dispatcher.Cancel ();
				throw;
			}
		}
	}
}
