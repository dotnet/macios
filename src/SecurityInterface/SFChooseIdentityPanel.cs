#nullable enable

using System;
using AppKit;
using Foundation;
using Security;

namespace SecurityInterface {

	public partial class SFChooseIdentityPanel {

		/// <summary>Gets or sets the policies used to evaluate the displayed identities.</summary>
		/// <remarks>Set this property to <see langword="null" /> to restore the default X.509 policy.</remarks>
		public SecPolicy []? Policies {
			get => _Policies == IntPtr.Zero ? null : NSArray.NonNullArrayFromHandleDropNullElements<SecPolicy> (_Policies);
			set {
				if (value is null) {
					_Policies = IntPtr.Zero;
					return;
				}
				if (value.Length == 0)
					throw new ArgumentException ("At least one policy is required.", nameof (value));
				using var array = NSArray.FromNativeObjects (value);
				_Policies = array.Handle;
				GC.KeepAlive (value);
			}
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
