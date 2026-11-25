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
		/// <param name="sheet">To be added.</param>
		///         <param name="docWindow">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public void BeginSheet (NSWindow sheet, NSWindow docWindow)
		{
#pragma warning disable CA1422 // This call site is reachable on: 'macOS/OSX' 14.0 and later. 'NSApplication.BeginSheet(NSWindow, NSWindow, NSObject?, Selector?, nint)' is obsoleted on: 'macOS/OSX' 10.10 and later (Use NSWindow.BeginSheet instead.).
			BeginSheet (sheet, docWindow, null, null, IntPtr.Zero);
#pragma warning restore CA1422
		}

		/// <param name="sheet">To be added.</param>
		///         <param name="docWindow">To be added.</param>
		///         <param name="onEnded">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public void BeginSheet (NSWindow sheet, NSWindow docWindow, Action onEnded)
		{
			var obj = new NSAsyncActionDispatcher (onEnded);
#pragma warning disable CA1422 // This call site is reachable on: 'macOS/OSX' 14.0 and later. 'NSApplication.BeginSheet(NSWindow, NSWindow, NSObject?, Selector?, nint)' is obsoleted on: 'macOS/OSX' 10.10 and later (Use NSWindow.BeginSheet instead.).
			BeginSheet (sheet, docWindow, obj, NSActionDispatcher.Selector, IntPtr.Zero);
#pragma warning restore CA1422
		}
	}

	public partial class NSOpenPanel {
		/// <param name="directory">To be added.</param>
		///         <param name="fileName">To be added.</param>
		///         <param name="fileTypes">To be added.</param>
		///         <param name="modalForWindow">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public void BeginSheet (string directory, string fileName, string [] fileTypes, NSWindow modalForWindow)
		{
#pragma warning disable CA1422 // This call site is reachable on: 'macOS/OSX' 14.0 and later. 'NSOpenPanel.BeginSheet(string?, string?, string[]?, NSWindow?, NSObject?, Selector?, nint)' is obsoleted on: 'macOS/OSX' 10.6 and later (Use NSApplication.RunModal instead.).
			BeginSheet (directory, fileName, fileTypes, modalForWindow, null, null, IntPtr.Zero);
#pragma warning restore CA1422
		}

		/// <param name="directory">To be added.</param>
		///         <param name="fileName">To be added.</param>
		///         <param name="fileTypes">To be added.</param>
		///         <param name="modalForWindow">To be added.</param>
		///         <param name="onEnded">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public void BeginSheet (string directory, string fileName, string [] fileTypes, NSWindow modalForWindow, Action onEnded)
		{
			var obj = new NSAsyncActionDispatcher (onEnded);
#pragma warning disable CA1422 // This call site is reachable on: 'macOS/OSX' 14.0 and later. 'NSOpenPanel.BeginSheet(string?, string?, string[]?, NSWindow?, NSObject?, Selector?, nint)' is obsoleted on: 'macOS/OSX' 10.6 and later (Use NSApplication.RunModal instead.).
			BeginSheet (directory, fileName, fileTypes, modalForWindow, obj, NSActionDispatcher.Selector, IntPtr.Zero);
#pragma warning restore CA1422
		}
	}

	public partial class NSPageLayout {
		/// <param name="printInfo">To be added.</param>
		///         <param name="docWindow">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public void BeginSheet (NSPrintInfo printInfo, NSWindow docWindow)
		{
#pragma warning disable CA1422 // This call site is reachable on: 'macOS/OSX' 14.0 and later. 'NSPageLayout.BeginSheet(NSPrintInfo, NSWindow, NSObject?, Selector?, nint)' is obsoleted on: 'macOS/OSX' 14.0 and later.
			BeginSheet (printInfo, docWindow, null, null, IntPtr.Zero);
#pragma warning restore CA1422
		}

		/// <param name="printInfo">To be added.</param>
		///         <param name="docWindow">To be added.</param>
		///         <param name="onEnded">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public void BeginSheet (NSPrintInfo printInfo, NSWindow docWindow, Action onEnded)
		{
			var obj = new NSAsyncActionDispatcher (onEnded);
#pragma warning disable CA1422 // This call site is reachable on: 'macOS/OSX' 14.0 and later. 'NSPageLayout.BeginSheet(NSPrintInfo, NSWindow, NSObject?, Selector?, nint)' is obsoleted on: 'macOS/OSX' 14.0 and later.
			BeginSheet (printInfo, docWindow, obj, NSActionDispatcher.Selector, IntPtr.Zero);
#pragma warning restore CA1422
		}
	}
}
#endif // !__MACCATALYST__
