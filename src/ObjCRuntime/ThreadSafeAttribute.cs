//
// ThreadSafe attribute
//
// Copyright 2012, 2015, Xamarin Inc.
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

using System;

namespace ObjCRuntime {

	/// <summary>A <see cref="System.Attribute" /> that indicates that a UIKit (for iOS) or AppKit (for macOS) method or class is thread-safe.</summary>
	///     <remarks>
	///       <para>
	/// 	This attribute is used to signal that the methods in the class that this is applied to, or the method that this is applied to can safely be called from a background thread.
	///       </para>
	///       <para>
	/// 	The attribute is only relevant to UIKit/AppKit classes and any subclass of UIKit/AppKit types.   The absence of this method in classes and methods outside of UIKit/AppKit is intentional.
	///       </para>
	///     </remarks>
	public sealed class ThreadSafeAttribute : Attribute {

		/// <summary>Initializes a new ThreadSafe attribute.</summary>
		///         <remarks>
		///         </remarks>
		public ThreadSafeAttribute ()
		{
			Safe = true;
		}

		/// <param name="safe">If the API is thread-safe or not.</param>
		///         <summary>Initializes a new ThreadSafe attribute.</summary>
		///         <remarks>
		///         </remarks>
		public ThreadSafeAttribute (bool safe)
		{
			Safe = safe;
		}

		/// <summary>If the API is thread-safe or not.</summary>
		///         <value>If the API is thread-safe or not.</value>
		///         <remarks>
		///         </remarks>
		public bool Safe { get; private set; }
	}
}
