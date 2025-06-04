//
// Copyright 2012 Xamarin Inc
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
using System.Runtime.InteropServices;

using ObjCRuntime;

#nullable enable

namespace Foundation {

	public partial class NSThread {
		/// <summary>Get or set the current thread's priority.</summary>
		/// <value>The current thread's priority, between 0.0 (lowest priority) and 1.0 (highest priority).</value>
		public static double Priority {
			get { return _GetPriority (); }
			// ignore the boolean return value
			set { _SetPriority (value); }
		}

		[DllImport ("__Internal")]
		static extern NativeHandle xamarin_init_nsthread (IntPtr handle, byte is_direct_binding, IntPtr target, IntPtr selector, IntPtr argument);

		NativeHandle InitNSThread (NSObject target, Selector selector, NSObject? argument)
		{
			if (target is null)
				ThrowHelper.ThrowArgumentNullException (nameof (target));
			if (selector is null)
				ThrowHelper.ThrowArgumentNullException (nameof (selector));

			IntPtr result = xamarin_init_nsthread (IsDirectBinding ? this.Handle : this.SuperHandle, IsDirectBinding.AsByte (), target.Handle, selector.Handle, argument.GetHandle ());
			GC.KeepAlive (target);
			GC.KeepAlive (selector);
			GC.KeepAlive (argument);
			return result;
		}

		/// <summary>Create a new thread.</summary>
		/// <param name="target">The object with the method to execute on the new thread.</param>
		/// <param name="selector">The selector that specifies the method to execute on the new thread.</param>
		/// <param name="argument">The argument to pass to the method to execute on the new thread.</param>
		[Export ("initWithTarget:selector:object:")]
		public NSThread (NSObject target, Selector selector, NSObject? argument)
			: base (NSObjectFlag.Empty)
		{
			InitializeHandle (InitNSThread (target, selector, argument), "initWithTarget:selector:object:");
		}
	}
}
