// Copyright 2011-2014 Xamarin Inc
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

#nullable enable

namespace Foundation {

	// Equals(Object) and GetHashCode are provided by NSObject
	// NSObject.GetHashCode calls GetNativeHash, which means it matches Equals (NSUrl)' behavior (which also calls the native implementation), so there's no need to override it.
	// NSObject.Equals calls the native isEqual: implementation, so that's fine as well, and no need to override.
#pragma warning disable 660 // `Foundation.NSUrl' defines operator == or operator != but does not override Object.Equals(object o)
#pragma warning disable 661 // `Foundation.NSUrl' defines operator == or operator != but does not override Object.GetHashCode()
	public partial class NSUrl : IEquatable<NSUrl> {

		/// <param name="path">To be added.</param>
		///         <param name="relativeToUrl">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public NSUrl (string path, string relativeToUrl)
			: this (path, new NSUrl (relativeToUrl))
		{
		}

		// but NSUrl has it's own isEqual: selector, which we re-expose in a more .NET-ish way
		/// <param name="url">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public bool Equals (NSUrl? url)
		{
			if (url is null)
				return false;
			// we can only ask `isEqual:` to test equality if both objects are direct bindings
			return IsDirectBinding && url.IsDirectBinding ? IsEqual (url) : Equals ((object) url);
		}

		// Converts from an NSURL to a System.Uri
		public static implicit operator Uri? (NSUrl? url)
		{
			if (url?.AbsoluteString is not string absoluteUrl) {
				return null;
			}

			if (Uri.TryCreate (absoluteUrl, UriKind.Absolute, out var uri))
				return uri;
			else
				return new Uri (absoluteUrl, UriKind.Relative);
		}

		public static implicit operator NSUrl? (Uri? uri)
		{
			if (uri is null) {
				return null;
			}
			if (uri.IsAbsoluteUri)
				return new NSUrl (uri.AbsoluteUri);
			else
				return new NSUrl (uri.OriginalString);
		}

		/// <param name="url">The filename.</param>
		///         <summary>Creates an NSUrl from a filename.</summary>
		///         <returns>An NSUrl that points to the given filename.</returns>
		///         <remarks>To be added.</remarks>
		public static NSUrl FromFilename (string url)
		{
			return new NSUrl (url, false);
		}

		/// <param name="url">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public NSUrl MakeRelative (string url)
		{
			return _FromStringRelative (url, this);
		}

		/// <summary>Returns a string representation of the value of the current instance.</summary>
		///         <returns>
		///         </returns>
		///         <remarks>
		///         </remarks>
		public override string ToString ()
		{
			return AbsoluteString ?? base.ToString ();
		}

		/// <param name="nsUrlResourceKey">To be added.</param>
		///         <param name="value">To be added.</param>
		///         <param name="error">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public bool TryGetResource (NSString nsUrlResourceKey, out NSObject value, out NSError error)
		{
			return GetResourceValue (out value, nsUrlResourceKey, out error);
		}

		/// <param name="nsUrlResourceKey">To be added.</param>
		///         <param name="value">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public bool TryGetResource (NSString nsUrlResourceKey, out NSObject value)
		{
			NSError error;
			return GetResourceValue (out value, nsUrlResourceKey, out error);
		}

		/// <param name="nsUrlResourceKey">To be added.</param>
		///         <param name="value">To be added.</param>
		///         <param name="error">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public bool SetResource (NSString nsUrlResourceKey, NSObject value, out NSError error)
		{
			return SetResourceValue (value, nsUrlResourceKey, out error);
		}

		/// <param name="nsUrlResourceKey">To be added.</param>
		///         <param name="value">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public bool SetResource (NSString nsUrlResourceKey, NSObject value)
		{
			NSError error;
			return SetResourceValue (value, nsUrlResourceKey, out error);
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public int Port {
			get {
				return (int) (this.PortNumber ?? -1);
			}
		}

		public static bool operator == (NSUrl? x, NSUrl? y)
		{
			if ((object?) x == (object?) y) // If both are null, or both are same instance, return true.
				return true;

			if (x is null || y is null) // If one is null, but not both, return false.
				return false;

			return x.Equals (y);
		}

		public static bool operator != (NSUrl? x, NSUrl? y)
		{
			return !(x == y);
		}
	}
}
