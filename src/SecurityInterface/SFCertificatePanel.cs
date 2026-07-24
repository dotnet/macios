#nullable enable

using System;
using AppKit;
using Foundation;
using Security;

namespace SecurityInterface {

	public partial class SFCertificatePanel {

		/// <summary>Gets the policies used to evaluate the displayed certificates.</summary>
		public SecPolicy [] Policies {
			get {
				if (_Policies == IntPtr.Zero)
					throw new InvalidOperationException ("The native certificate panel returned a null policies array.");
				return NSArray.NonNullArrayFromHandleDropNullElements<SecPolicy> (_Policies);
			}
		}

		/// <summary>Sets the policies used to evaluate the displayed certificates.</summary>
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

		/// <summary>Displays a certificate trust sheet and invokes the callback when it closes.</summary>
		public void BeginSheet (NSWindow docWindow, SecTrust trust, bool showGroup, Action<NSModalResponse> didEnd)
		{
			var dispatcher = SecurityInterfaceSheetDidEndDispatcher.Create (didEnd);
			try {
				BeginSheet (docWindow, dispatcher, SecurityInterfaceSheetDidEndDispatcher.Selector, IntPtr.Zero, trust, showGroup);
			} catch {
				dispatcher.Cancel ();
				throw;
			}
		}

		/// <summary>Displays a certificate sheet and invokes the callback when it closes.</summary>
		public void BeginSheet (NSWindow docWindow, SecCertificate [] certificates, bool showGroup, Action<NSModalResponse> didEnd)
		{
			var dispatcher = SecurityInterfaceSheetDidEndDispatcher.Create (didEnd);
			try {
				BeginSheet (docWindow, dispatcher, SecurityInterfaceSheetDidEndDispatcher.Selector, IntPtr.Zero, certificates, showGroup);
			} catch {
				dispatcher.Cancel ();
				throw;
			}
		}
	}

	public partial class SFCertificateTrustPanel {

		/// <summary>Displays a trust sheet and invokes the callback when it closes.</summary>
		public void BeginSheet (NSWindow docWindow, SecTrust trust, string? message, Action<NSModalResponse> didEnd)
		{
			var dispatcher = SecurityInterfaceSheetDidEndDispatcher.Create (didEnd);
			try {
				BeginSheet (docWindow, dispatcher, SecurityInterfaceSheetDidEndDispatcher.Selector, IntPtr.Zero, trust, message);
			} catch {
				dispatcher.Cancel ();
				throw;
			}
		}
	}
}
