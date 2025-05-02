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

#if !__MACCATALYST__

using System;
using Foundation;
using ObjCRuntime;

#nullable enable

namespace AppKit {

	public partial class NSBitmapImageRep {
		static string selInitForIncrementalLoad = "initForIncrementalLoad";

		// Do not actually export because NSObjectFlag is not exportable.
		// The Objective C method already exists. This is just to allow
		// access on the managed side via the static method.
		//[Export ("initForIncrementalLoad")]
		private NSBitmapImageRep (NSObjectFlag a, NSObjectFlag b) : base (a)
		{
			if (IsDirectBinding) {
				InitializeHandle (ObjCRuntime.Messaging.IntPtr_objc_msgSend (this.Handle, Selector.GetHandle (selInitForIncrementalLoad)), selInitForIncrementalLoad);
			} else {
				InitializeHandle (ObjCRuntime.Messaging.IntPtr_objc_msgSendSuper (this.SuperHandle, Selector.GetHandle (selInitForIncrementalLoad)), selInitForIncrementalLoad);
			}
		}

		/// <summary>Convert the bitmap to a specific file format.</summary>
		/// <param name="storageType">The target filetype for the bitmap image.</param>
		/// <returns>An <see cref="NSData" /> with the data of the bitmap stored as the specified file type.</returns>
		public NSData? RepresentationUsingTypeProperties (NSBitmapImageFileType storageType)
		{
			return RepresentationUsingTypeProperties (storageType, new NSDictionary ());
		}

		/// <summary>Create a new <see cref="NSBitmapImageRep" /> that for incremental loading.</summary>
		public static NSBitmapImageRep IncrementalLoader ()
		{
			return new NSBitmapImageRep (NSObjectFlag.Empty, NSObjectFlag.Empty);
		}
	}
}
#endif // !__MACCATALYST__
