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
using System.ComponentModel;
using System.Diagnostics;

#nullable enable

namespace Foundation {
	/// <summary>Provides data for the numerous events in various frameworks.</summary>
	/// <remarks>
	/// <para>Use this class when you want to create event handlers that get an <see cref="NSError" />.</para>
	/// <example>
	/// <code lang="csharp lang-csharp"><![CDATA[
	/// public class Sample {
	///     public event EventHandler<NSErrorEventArgs> Failed;
	/// }
	/// ]]></code>
	/// </example>
	/// </remarks>
	public class NSErrorEventArgs : EventArgs {
		/// <summary>Initializes a new instance of the <see cref="NSErrorEventArgs" /> class.</summary>
		/// <param name="error">The underlying error.</param>
		public NSErrorEventArgs (NSError? error)
		{
			Error = error;
		}

		/// <summary>Gets the <see cref="NSError" /> value.</summary>
		/// <value>The error that occurred, or <see langword="null" /> if no error occurred.</value>
		public NSError? Error { get; private set; }
	}

	public partial class NSError : NSObject {
#if !COREBUILD
#if !XAMCORE_5_0
		/// <summary>Do not use the default constructor unless you are dealing with a low-level API that will initialize the object for you.</summary>
		/// <remarks>
		/// <para>The default constructor for <see cref="NSError" /> leaves the object in a partial state that can only be initialized by a handful of low-level Objective-C APIs. In general, you should not use this constructor, you should instead use the constructor that takes an <see cref="NSString" /> error domain argument.</para>
		/// </remarks>
		[Obsolete ("Use the other constructor or factory method instead.")]
		[EditorBrowsable (EditorBrowsableState.Never)]
		public NSError () : this (new NSString ("Invalid .ctor used"), 0, null)
		{
		}
#endif // XAMCORE_5_0

		/// <summary>Creates an <see cref="NSError" /> instance from a given domain and code.</summary>
		/// <param name="domain">The error domain.</param>
		/// <param name="code">The error code.</param>
		/// <returns>A new <see cref="NSError" /> instance.</returns>
		public static NSError FromDomain (NSString domain, nint code)
		{
			return FromDomain (domain, code, null);
		}

		/// <summary>Initializes a new instance of the <see cref="NSError" /> class with a specified domain and error code.</summary>
		/// <param name="domain">The error domain.</param>
		/// <param name="code">The error code.</param>
		public NSError (NSString domain, nint code) : this (domain, code, null)
		{
		}
		/// <summary>Returns a string representation of the value of the current instance.</summary>
		/// <returns>The localized description of the error.</returns>
		public override string ToString ()
		{
			return LocalizedDescription;
		}
#endif
	}
}
