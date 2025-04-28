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

#nullable enable

#if MONOMAC

using System;
using System.Reflection;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.IO;

using AppKit;
using Foundation;
using Registrar;

namespace ObjCRuntime {

	/// <summary>Provides information about the Xamarin.iOS Runtime.</summary>
	///     <remarks>
	///     </remarks>
	///     <related type="sample" href="https://github.com/xamarin/ios-samples/tree/master/SysSound/">SysSound</related>
	public static partial class Runtime {
#if !COREBUILD
		internal const string ProductName = "Microsoft.macOS";
		internal const string AssemblyName = "Microsoft.macOS.dll";

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public static string? FrameworksPath {
			get; set;
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public static string? ResourcesPath {
			get; set;
		}

		delegate void initialize_func ();
		unsafe delegate sbyte* get_sbyteptr_func ();

		[DllImport ("__Internal")]
		extern static void xamarin_initialize ();

		internal static void EnsureInitialized ()
		{
			if (initialized)
				return;

			xamarin_initialize ();
		}

		unsafe static void InitializePlatform (InitializationOptions* options)
		{
			string basePath = AppDomain.CurrentDomain.BaseDirectory;

			ResourcesPath = Path.Combine (basePath, "Resources");
			FrameworksPath = Path.Combine (basePath, "Frameworks");
		}
#endif // !COREBUILD
	}
}

#endif // MONOMAC
