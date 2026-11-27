#nullable enable

using System;
using AppKit;
using ObjCRuntime;
using Security;

#if XAMCORE_5_0
using SecKeychain = Security.SecKeychain2;
#endif

namespace SecurityInterface {

	public partial class SFKeychainSavePanel {

#if XAMCORE_5_0
		/// <summary>Gets the keychain created by the panel, or <see langword="null" /> if no keychain was created.</summary>
#endif
		public SecKeychain? Keychain => _Keychain == IntPtr.Zero ? null : new SecKeychain (_Keychain, owns: false);

		/// <summary>Displays the save panel as a sheet and invokes the callback when it closes.</summary>
		public void BeginSheet (string? path, string? name, NSWindow? docWindow, Action<NSModalResponse> didEnd)
		{
			var dispatcher = SecurityInterfaceSheetDidEndDispatcher.Create (didEnd);
			try {
				BeginSheet (path, name, docWindow, dispatcher, SecurityInterfaceSheetDidEndDispatcher.Selector, IntPtr.Zero);
			} catch {
				dispatcher.Cancel ();
				throw;
			}
		}
	}
}
