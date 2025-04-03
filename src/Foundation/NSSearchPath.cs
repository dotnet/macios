//
// NSSearchPath.cs
//
// Authors:
//   Marek Safar (marek.safar@gmail.com)
//
// Copyright 2013 Xamarin Inc.
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

using CoreFoundation;
using ObjCRuntime;
using System.Runtime.Versioning;

// Disable until we get around to enable + fix any issues.
#nullable disable

namespace Foundation {
#if NET
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
#endif
	public static class NSSearchPath {
		/// <param name="directory">Searched directory kind.</param>
		///         <param name="domainMask">Searched domains mask.</param>
		///         <param name="expandTilde">Controls whether to expand tildes.</param>
		///         <summary>Builds an array of directory search paths in specified directory and domains.</summary>
		///         <returns>
		///         </returns>
		///         <remarks>The array is in the order in which you should search the directories. The directory returned may not exist.</remarks>
		public static string [] GetDirectories (NSSearchPathDirectory directory, NSSearchPathDomain domainMask, bool expandTilde = true)
		{
			return CFArray.StringArrayFromHandle (NSSearchPathForDirectoriesInDomains ((nuint) (ulong) directory, (nuint) (ulong) domainMask, expandTilde.AsByte ()));
		}

		[DllImport (Constants.FoundationLibrary)]
		static extern IntPtr NSSearchPathForDirectoriesInDomains (nuint directory, nuint domainMask, byte expandTilde);
	}
}
