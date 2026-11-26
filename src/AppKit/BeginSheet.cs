//
// BeginSheet.cs: Helper methods that support NSAction overloads instead of
// (NSObject target, Selector selector) overloads for BeginSheet methods.
// 
// Copyright 2010, Novell, Inc.
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

#if !__MACCATALYST__ // Mac Catalyst doesn't have NSApplication

using System.Collections;

#nullable enable

namespace AppKit {
	public partial class NSApplication {
		/// <summary>Shows the specified sheet attached to the specified window.</summary>
		/// <param name="sheet">The sheet to show.</param>
		/// <param name="docWindow">The window to attach the sheet to.</param>
		/// <remarks>
		///   <para>This method is obsolete. Use <see cref="NSWindow.BeginSheet(NSWindow,Action{nint})" /> instead.</para>
		/// </remarks>
		[SupportedOSPlatform ("macos")]
		[ObsoletedOSPlatform ("macos10.10", "Use NSWindow.BeginSheet instead.")]
		public void BeginSheet (NSWindow sheet, NSWindow docWindow)
		{
			BeginSheet (sheet, docWindow, null, null, IntPtr.Zero);
		}

		/// <summary>Shows the specified sheet attached to the specified window, and calls a callback when the sheet is dismissed.</summary>
		/// <param name="sheet">The sheet to show.</param>
		/// <param name="docWindow">The window to attach the sheet to.</param>
		/// <param name="onEnded">The callback to call when the sheet is dismissed.</param>
		/// <remarks>
		///   <para>This method is obsolete. Use <see cref="NSWindow.BeginSheet(NSWindow,Action{nint})" /> instead.</para>
		/// </remarks>
		[SupportedOSPlatform ("macos")]
		[ObsoletedOSPlatform ("macos10.10", "Use NSWindow.BeginSheet instead.")]
		public void BeginSheet (NSWindow sheet, NSWindow docWindow, Action onEnded)
		{
			var obj = new NSAsyncActionDispatcher (onEnded);
			BeginSheet (sheet, docWindow, obj, NSActionDispatcher.Selector, IntPtr.Zero);
		}
	}

	public partial class NSOpenPanel {
		/// <summary>Shows the open panel as a sheet attached to the specified window.</summary>
		/// <param name="directory">The directory to show initially.</param>
		/// <param name="fileName">The file to select initially.</param>
		/// <param name="fileTypes">The file types the user can select.</param>
		/// <param name="modalForWindow">The window to attach the sheet to.</param>
		/// <remarks>
		///   <para>This method is obsolete. Use <see cref="NSSavePanel.BeginSheet(NSWindow,NSSavePanelComplete)" /> instead.</para>
		/// </remarks>
		[SupportedOSPlatform ("macos")]
		[ObsoletedOSPlatform ("macos10.6", "Use NSSavePanel.BeginSheet (NSWindow, NSSavePanelComplete) instead.")]
		public void BeginSheet (string directory, string fileName, string [] fileTypes, NSWindow modalForWindow)
		{
			BeginSheet (directory, fileName, fileTypes, modalForWindow, null, null, IntPtr.Zero);
		}

		/// <summary>Shows the open panel as a sheet attached to the specified window, and calls a callback when the sheet is dismissed.</summary>
		/// <param name="directory">The directory to show initially.</param>
		/// <param name="fileName">The file to select initially.</param>
		/// <param name="fileTypes">The file types the user can select.</param>
		/// <param name="modalForWindow">The window to attach the sheet to.</param>
		/// <param name="onEnded">The callback to call when the sheet is dismissed.</param>
		/// <remarks>
		///   <para>This method is obsolete. Use <see cref="NSSavePanel.BeginSheet(NSWindow,NSSavePanelComplete)" /> instead.</para>
		/// </remarks>
		[SupportedOSPlatform ("macos")]
		[ObsoletedOSPlatform ("macos10.6", "Use NSSavePanel.BeginSheet (NSWindow, NSSavePanelComplete) instead.")]
		public void BeginSheet (string directory, string fileName, string [] fileTypes, NSWindow modalForWindow, Action onEnded)
		{
			var obj = new NSAsyncActionDispatcher (onEnded);
			BeginSheet (directory, fileName, fileTypes, modalForWindow, obj, NSActionDispatcher.Selector, IntPtr.Zero);
		}
	}

	public partial class NSPageLayout {
		/// <summary>Shows the page layout panel as a sheet attached to the specified window.</summary>
		/// <param name="printInfo">The print information to show and edit.</param>
		/// <param name="docWindow">The window to attach the sheet to.</param>
		/// <remarks>
		///   <para>This method is obsolete. Use <see cref="BeginSheet(NSPrintInfo,NSWindow,Action{NSPageLayoutResult})" /> instead.</para>
		/// </remarks>
		[SupportedOSPlatform ("macos")]
		[ObsoletedOSPlatform ("macos14.0")]
		public void BeginSheet (NSPrintInfo printInfo, NSWindow docWindow)
		{
			BeginSheet (printInfo, docWindow, null, null, IntPtr.Zero);
		}

		/// <summary>Shows the page layout panel as a sheet attached to the specified window, and calls a callback when the sheet is dismissed.</summary>
		/// <param name="printInfo">The print information to show and edit.</param>
		/// <param name="docWindow">The window to attach the sheet to.</param>
		/// <param name="onEnded">The callback to call when the sheet is dismissed.</param>
		/// <remarks>
		///   <para>This method is obsolete. Use <see cref="BeginSheet(NSPrintInfo,NSWindow,Action{NSPageLayoutResult})" /> instead.</para>
		/// </remarks>
		[SupportedOSPlatform ("macos")]
		[ObsoletedOSPlatform ("macos14.0")]
		public void BeginSheet (NSPrintInfo printInfo, NSWindow docWindow, Action onEnded)
		{
			var obj = new NSAsyncActionDispatcher (onEnded);
			BeginSheet (printInfo, docWindow, obj, NSActionDispatcher.Selector, IntPtr.Zero);
		}
	}
}
#endif // !__MACCATALYST__
