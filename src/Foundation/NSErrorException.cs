//
// Authors:
//   Rodrigo Kumpera
//
// Copyright 2011 Xamarin Inc.
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
using System.Runtime.Versioning;

namespace Foundation {
#if NET
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
#endif
	public class NSErrorException : Exception {
		NSError error;

		/// <param name="error">The NSError to wrap.</param>
		///         <summary>Creates an NSErrorException that encapsulates an NSError.</summary>
		///         <remarks>
		///         </remarks>
		public NSErrorException (NSError error)
		{
			if (error is null)
				throw new ArgumentNullException (nameof (error));
			this.error = error;
		}

		/// <summary>The underlying NSError that is being wrapped.</summary>
		///         <value>
		///         </value>
		///         <remarks>
		///         </remarks>
		public NSError Error {
			get { return error; }
		}

		/// <summary>The NSError's Domain property.</summary>
		///         <value>
		///         </value>
		///         <remarks>
		///         </remarks>
		public string Domain {
			get { return error.Domain; }
		}

		/// <summary>The NSError.Code property</summary>
		///         <value>
		///         </value>
		///         <remarks>
		///         </remarks>
		public nint Code {
			get { return error.Code; }
		}

		/// <summary>The NSError's UserInfo.</summary>
		///         <value>
		///         </value>
		///         <remarks>
		///         </remarks>
		public NSDictionary UserInfo {
			get { return error.UserInfo; }
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public override string Message {
			get {
				return error.Description;
			}
		}
	}
}
