// 
// CALayerDelegate.cs
//
// Authors:
//   Rolf Bjarne Kvinge
//     
// Copyright 2014 Xamarin Inc
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
using System;

using Foundation;
using ObjCRuntime;
using CoreGraphics;

#nullable enable

namespace CoreAnimation {

	public unsafe partial class CALayerDelegate {
		WeakReference<CALayer>? calayer;

		internal void SetCALayer (CALayer? layer)
		{
			calayer = layer is null ? null : new WeakReference<CALayer> (layer);
		}

		/// <include file="../../docs/api/CoreAnimation/CALayerDelegate.xml" path="/Documentation/Docs[@DocId='M:CoreAnimation.CALayerDelegate.Dispose(System.Boolean)']/*" />
		protected override void Dispose (bool disposing)
		{
			if (calayer?.TryGetTarget (out var layer) == true) {
				Messaging.void_objc_msgSend_IntPtr (layer.Handle, Selector.GetHandle ("setDelegate:"), IntPtr.Zero);
				GC.KeepAlive (layer);
				calayer = null;
			}

			base.Dispose (disposing);
		}
	}
}
