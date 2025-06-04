//
// Copyright 2010, Novell, Inc.
// Copyright 2011, 2012 Xamarin Inc
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
//
// Simple class for passing NSErrors as EventArgs
//
using System;
using System.Diagnostics;
using ObjCRuntime;
using System.Runtime.Versioning;

// Disable until we get around to enable + fix any issues.
#nullable disable

namespace Foundation {
	/// <include file="../../docs/api/Foundation/NSErrorEventArgs.xml" path="/Documentation/Docs[@DocId='T:Foundation.NSErrorEventArgs']/*" />
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	public class NSErrorEventArgs : EventArgs {
		/// <param name="error">The underlying error.</param>
		///         <summary>Initializes a new instance of the NSErrorEventArgs class.</summary>
		///         <remarks>
		///         </remarks>
		public NSErrorEventArgs (NSError error)
		{
			Error = error;
		}

		/// <summary>Retrieves the NSError value.</summary>
		///         <value />
		///         <remarks>To be added.</remarks>
		public NSError Error { get; private set; }
	}

	public partial class NSError : NSObject {
#if !COREBUILD
		/// <summary>Do not use the Default Constructor unless you are dealing with a low-level API that will initialize the object for you.</summary>
		///         <remarks>
		///
		/// 	  The default constructor for NSError leaves the object in a
		/// 	  partial state that can only be initialized by a handful of
		/// 	  low-level Objective-C APIs.  In general, you should not use
		/// 	  this constructor, you should instead use the constructor
		/// 	  that takes an NSString error domain argument.
		///
		///         </remarks>
		[Advice ("Always specify a domain and error code when creating an NSError instance")]
		public NSError () : this (new NSString ("Invalid .ctor used"), 0, null)
		{
			Debug.WriteLine ("Warning: you created an NSError without specifying a domain");
		}

		/// <param name="domain">To be added.</param>
		/// <param name="code">To be added.</param>
		/// <summary>Creates an NSError instance from a given domain and code.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		public static NSError FromDomain (NSString domain, nint code)
		{
			return FromDomain (domain, code, null);
		}

		/// <param name="domain">Error domain</param>
		/// <param name="code">Error code.</param>
		/// <summary>A constructor that initializes the object with a specified domain and an error code.</summary>
		/// <remarks>To be added.</remarks>
		public NSError (NSString domain, nint code) : this (domain, code, null)
		{
		}
		/// <summary>Returns a string representation of the value of the current instance.</summary>
		///         <returns>
		///         </returns>
		///         <remarks>
		///         </remarks>
		public override string ToString ()
		{
			return LocalizedDescription;
		}
#endif
	}
}
