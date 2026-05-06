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
// Copyright 2011, 2012 Xamarin Inc
//
using CoreFoundation;

#nullable enable

namespace Foundation {

	public partial class NSKeyedArchiver {

		/// <summary>Sets the global class name for a specified class.</summary>
		/// <param name="name">The class name to use during archiving.</param>
		/// <param name="kls">The <see cref="ObjCRuntime.Class" /> to associate with the name.</param>
		/// <remarks>This method associates a class name with a class for all instances of <see cref="NSKeyedArchiver" />. Use <see cref="SetClassName(System.String,ObjCRuntime.Class)" /> to set the class name for a specific archiver instance.</remarks>
		public static void GlobalSetClassName (string name, Class kls)
		{
			ArgumentNullException.ThrowIfNull (name);
			ArgumentNullException.ThrowIfNull (kls);

			var ptr = CFString.CreateNative (name);
			ObjCRuntime.Messaging.void_objc_msgSend_IntPtr_IntPtr (class_ptr, Selector.GetHandle ("setClassName:forClass:"), ptr, kls.Handle);
			GC.KeepAlive (kls);
			CFString.ReleaseNative (ptr);
		}

		/// <summary>Gets the global class name for a specified class.</summary>
		/// <param name="kls">The <see cref="ObjCRuntime.Class" /> to query.</param>
		/// <returns>The class name associated with the class, or <see langword="null" /> if no class name has been set.</returns>
		/// <remarks>This method retrieves the class name associated with a class for all instances of <see cref="NSKeyedArchiver" />. Use <see cref="GetClassName(ObjCRuntime.Class)" /> to get the class name for a specific archiver instance.</remarks>
		public static string? GlobalGetClassName (Class kls)
		{
			ArgumentNullException.ThrowIfNull (kls);

			string? result = CFString.FromHandle (ObjCRuntime.Messaging.IntPtr_objc_msgSend_IntPtr (class_ptr, Selector.GetHandle ("classNameForClass:"), kls.Handle));
			GC.KeepAlive (kls);
			return result;
		}
	}
}
