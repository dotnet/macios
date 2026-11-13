#nullable enable

using System;
using AppKit;
using Foundation;
using ObjCRuntime;
using Security;

#if XAMCORE_5_0
using SecKeychain = Security.SecKeychain2;
#endif

namespace SecurityInterface {

	public partial class SFKeychainSettingsPanel {

#if XAMCORE_5_0
		/// <summary>Displays the panel modally for the specified keychain and settings.</summary>
#endif
		public NSModalResponse RunModal (ref SecKeychainSettings settings, SecKeychain keychain)
		{
			ArgumentNullException.ThrowIfNull (keychain);
			var response = _RunModalForSettings (ref settings, keychain.GetCheckedHandle ());
			GC.KeepAlive (keychain);
			return response;
		}

#if XAMCORE_5_0
		/// <summary>Displays the panel as a sheet for the specified keychain and settings.</summary>
#endif
		public void BeginSheet (NSWindow? docWindow, NSObject? modalDelegate, Selector? didEndSelector, IntPtr contextInfo, ref SecKeychainSettings settings, SecKeychain keychain)
		{
			ArgumentNullException.ThrowIfNull (keychain);
			_BeginSheet (docWindow, modalDelegate, didEndSelector, contextInfo, ref settings, keychain.GetCheckedHandle ());
			GC.KeepAlive (keychain);
		}

#if XAMCORE_5_0
		/// <summary>Displays the panel as a sheet and invokes the callback when it closes.</summary>
#endif
		public void BeginSheet (NSWindow? docWindow, ref SecKeychainSettings settings, SecKeychain keychain, Action<NSModalResponse> didEnd)
		{
			var dispatcher = SecurityInterfaceSheetDidEndDispatcher.Create (didEnd);
			try {
				BeginSheet (docWindow, dispatcher, SecurityInterfaceSheetDidEndDispatcher.Selector, IntPtr.Zero, ref settings, keychain);
			} catch {
				dispatcher.Cancel ();
				throw;
			}
		}
	}
}
