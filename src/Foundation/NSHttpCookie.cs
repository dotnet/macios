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
using ObjCRuntime;
using System;
using System.Net;

#nullable enable

namespace Foundation {
	public partial class NSHttpCookie {
		// same order as System.Net.Cookie
		// http://msdn.microsoft.com/en-us/library/a18ka3h2.aspx
		/// <summary>Create a new cookie with the supplied name and value.</summary>
		/// <param name="name">Cookie's name. Cannot be null.</param>
		/// <param name="value">Cookie's value. Cannot be null.</param>
		/// <remarks>A default Path and Domain will be used to ensure a valid instance is created.</remarks>
		public NSHttpCookie (string name, string value)
			: this (name, value, null, null)
		{
		}

		/// <summary>Create a new cookie with the supplied name, value and path.</summary>
		/// <param name="name">Cookie's name. Cannot be null.</param>
		/// <param name="value">Cookie's value. Cannot be null.</param>
		/// <param name="path">Path where the cookie will be applied on the domain. Using "/" will send the cookie to every URL on the domain.</param>
		/// <remarks>A default Domain will be used to ensure a valid instance is created</remarks>
		public NSHttpCookie (string name, string value, string? path)
			: this (name, value, path, null)
		{
		}

		/// <summary>Create a new cookie with the supplied name, value, path and domain.</summary>
		/// <param name="name">Cookie's name. Cannot be null.</param>
		/// <param name="value">Cookie's value. Cannot be null.</param>
		/// <param name="path">Path where the cookie will be applied on the domain. Using "/" will send the cookie to every URL on the domain.</param>
		/// <param name="domain">Domain (e.g. microsoft.com) related to the cookie</param>
		/// <remarks>An <see cref="ArgumentNullException" /> will be thrown if either <paramref name="name" /> or <paramref name="value" /> are null.</remarks>
		public NSHttpCookie (string name, string value, string? path, string? domain)
			: base (NSObjectFlag.Empty)
		{
			CreateCookie (name, value, path, domain, null, null, null, null, null, null, null, null);
		}

		// FIXME: should we expose more complex/long ctor or point people to use a Cookie ?

		/// <summary>Create a new cookie from the supplied <see cref="System.Net.Cookie" /> instance's properties.</summary>
		/// <param name="cookie">An existing <see cref="System.Net.Cookie" /> from the .NET framework</param>
		/// <remarks>This constructor will throw an <see cref="ArgumentNullException" /> if <paramref name="cookie" /> is null</remarks>
		public NSHttpCookie (Cookie cookie)
			: base (NSObjectFlag.Empty)
		{
			if (cookie is null)
				throw new ArgumentNullException (nameof (cookie));

			var commentUrl = cookie.CommentUri?.ToString ();
			bool? discard = null;
			if (cookie.Discard)
				discard = true;
			CreateCookie (cookie.Name, cookie.Value, cookie.Path, cookie.Domain, cookie.Comment, commentUrl, discard, cookie.Expires, null, cookie.Port, cookie.Secure, cookie.Version);
		}

		void CreateCookie (string name, string value, string? path, string? domain, string? comment, string? commentUrl, bool? discard, DateTime? expires, int? maximumAge, string? ports, bool? secure, int? version)
		{
			// mandatory checks or defaults
			if (name is null)
				throw new ArgumentNullException (nameof (name));
			if (value is null)
				throw new ArgumentNullException (nameof (value));
			if (String.IsNullOrEmpty (path))
				path = "/"; // default in .net
			if (String.IsNullOrEmpty (domain))
				domain = "*";

			using (var properties = new NSMutableDictionary ()) {
				// mandatory to create the cookie
				properties.Add (NSHttpCookie.KeyName, new NSString (name));
				properties.Add (NSHttpCookie.KeyValue, new NSString (value));
				properties.Add (NSHttpCookie.KeyPath, new NSString (path));
				properties.Add (NSHttpCookie.KeyDomain, new NSString (domain));

				// optional to create the cookie
				if (!String.IsNullOrEmpty (comment))
					properties.Add (NSHttpCookie.KeyComment, new NSString (comment));
				if (!String.IsNullOrEmpty (commentUrl))
					properties.Add (NSHttpCookie.KeyCommentUrl, new NSString (commentUrl));
				if (discard.HasValue)
					properties.Add (NSHttpCookie.KeyDiscard, new NSString (discard.Value ? "TRUE" : "FALSE"));
				if (expires.HasValue && expires.Value != DateTime.MinValue)
					properties.Add (NSHttpCookie.KeyExpires, (NSDate) expires.Value);
				if (maximumAge.HasValue)
					properties.Add (NSHttpCookie.KeyMaximumAge, new NSString (maximumAge.Value.ToString ()));
				if (!String.IsNullOrEmpty (ports))
					properties.Add (NSHttpCookie.KeyPort, new NSString (ports.Replace ("\"", String.Empty)));
				// any value means secure is true
				if (secure.HasValue && secure.Value)
					properties.Add (NSHttpCookie.KeySecure, new NSString ("1"));
				if (version.HasValue)
					properties.Add (NSHttpCookie.KeyVersion, new NSString (version.Value.ToString ()));

#if NET10_0_OR_GREATER
				var throwOnInitFailure = true;
#else
				var throwOnInitFailure = false;
#endif

				if (IsDirectBinding) {
					InitializeHandle (Messaging.IntPtr_objc_msgSend_IntPtr (this.Handle, Selector.GetHandle ("initWithProperties:"), properties.Handle), "initWithProperties:", throwOnInitFailure);
				} else {
					InitializeHandle (Messaging.IntPtr_objc_msgSendSuper_IntPtr (this.SuperHandle, Selector.GetHandle ("initWithProperties:"), properties.Handle), "initWithProperties:", throwOnInitFailure);
				}
			}
		}
	}
}
