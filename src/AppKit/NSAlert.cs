//
// NSAlert.cs
//
// Author:
//   Aaron Bockover <abock@xamarin.com>
//
// Copyright 2012 Xamarin Inc. (http://xamarin.com)
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

#if !__MACCATALYST__ // Mac Catalyst doesn't have NSAlert

using System.Collections.Generic;

#nullable enable

namespace AppKit {
	[Register ("__MonoMac_NSAlertDidEndDispatcher")]
	internal class NSAlertDidEndDispatcher : NSObject {
		const string selector = "alertDidEnd:returnCode:contextInfo:";
		public static readonly Selector Selector = new Selector (selector);

		Action<nint>? action;

		public NSAlertDidEndDispatcher (Action<nint> action)
		{
			this.action = action;
			IsDirectBinding = false;
			DangerousRetain ();
		}

		[Export (selector)]
		[Preserve (Conditional = true)]
		public void OnAlertDidEnd (NSAlert alert, nint returnCode, IntPtr context)
		{
			try {
				if (action is not null)
					action (returnCode);
			} finally {
				action = null;
				DangerousRelease ();
			}
		}
	}

	public partial class NSAlert {
		/// <summary>Shows the alert as a sheet attached to the specified window.</summary>
		/// <param name="window">The window to attach the sheet to.</param>
		/// <remarks>
		///   <para>This method is obsolete. Use <see cref="BeginSheet(NSWindow,Action{NSModalResponse})" /> instead.</para>
		/// </remarks>
		[SupportedOSPlatform ("macos")]
		[ObsoletedOSPlatform ("macos10.10", "Use BeginSheet (NSWindow, Action<NSModalResponse>) instead.")]
		public void BeginSheet (NSWindow window)
		{
			BeginSheet (window, null, null, IntPtr.Zero);
		}

		/// <summary>Shows the alert as a sheet attached to the specified window, and calls a callback when the sheet is dismissed.</summary>
		/// <param name="window">The window to attach the sheet to.</param>
		/// <param name="onEnded">The callback to call when the sheet is dismissed.</param>
		/// <remarks>
		///   <para>This method is obsolete. Use <see cref="BeginSheet(NSWindow,Action{NSModalResponse})" /> instead.</para>
		/// </remarks>
		[SupportedOSPlatform ("macos")]
		[ObsoletedOSPlatform ("macos10.10", "Use BeginSheet (NSWindow, Action<NSModalResponse>) instead.")]
		public void BeginSheet (NSWindow window, Action? onEnded)
		{
			BeginSheetForResponse (window, r => {
				if (onEnded is not null)
					onEnded ();
			});
		}

		/// <summary>Shows the alert as a sheet attached to the specified window, and calls a callback with the response when the sheet is dismissed.</summary>
		/// <param name="window">The window to attach the sheet to.</param>
		/// <param name="onEnded">The callback to call when the sheet is dismissed. The argument is the return code for the button the user clicked.</param>
		/// <remarks>
		///   <para>This method is obsolete. Use <see cref="BeginSheet(NSWindow,Action{NSModalResponse})" /> instead.</para>
		/// </remarks>
		[SupportedOSPlatform ("macos")]
		[ObsoletedOSPlatform ("macos10.10", "Use BeginSheet (NSWindow, Action<NSModalResponse>) instead.")]
		public void BeginSheetForResponse (NSWindow window, Action<nint> onEnded)
		{
			BeginSheet (window, new NSAlertDidEndDispatcher (onEnded), NSAlertDidEndDispatcher.Selector, IntPtr.Zero);
		}

		/// <summary>Shows the alert as a sheet attached to the specified window, and waits until the user dismisses it.</summary>
		/// <param name="window">The window to attach the sheet to.</param>
		/// <returns>The return code for the button the user clicked.</returns>
		/// <remarks>
		///   <para>This method is obsolete. Use <see cref="BeginSheet(NSWindow,Action{NSModalResponse})" /> instead.</para>
		///   <para>This method runs a modal session for <see cref="NSApplication.SharedApplication" />.</para>
		/// </remarks>
		[SupportedOSPlatform ("macos")]
		[ObsoletedOSPlatform ("macos10.10", "Use BeginSheet (NSWindow, Action<NSModalResponse>) instead.")]
		public nint RunSheetModal (NSWindow window)
		{
			return RunSheetModal (window, NSApplication.SharedApplication);
		}

		/// <summary>Shows the alert as a sheet attached to the specified window, and waits until the user dismisses it.</summary>
		/// <param name="window">The window to attach the sheet to. If <see langword="null" />, the alert is shown as a modal dialog instead, using <see cref="RunModal" />.</param>
		/// <param name="application">The application used to run the modal session.</param>
		/// <returns>The return code for the button the user clicked.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="application" /> is <see langword="null" />.</exception>
		/// <remarks>
		///   <para>This method is obsolete. Use <see cref="BeginSheet(NSWindow,Action{NSModalResponse})" /> instead.</para>
		/// </remarks>
		[SupportedOSPlatform ("macos")]
		[ObsoletedOSPlatform ("macos10.10", "Use BeginSheet (NSWindow, Action<NSModalResponse>) instead.")]
		public nint RunSheetModal (NSWindow? window, NSApplication application)
		{
			if (application is null)
				throw new ArgumentNullException ("application");

			// same behavior as BeginSheet with a null window
			if (window is null)
				return RunModal ();

			nint returnCode = -1000;

			BeginSheetForResponse (window, r => {
				returnCode = r;
				application.StopModal ();
			});

			application.RunModalForWindow (Window);

			return returnCode;
		}
	}
}
#endif // __MACCATALYST__
