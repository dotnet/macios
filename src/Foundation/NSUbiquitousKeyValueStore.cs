//
// Copyright 2011, Xamarin, Inc.
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
using System.Reflection;
using System.Collections;

#nullable enable

namespace Foundation {

	public partial class NSUbiquitousKeyValueStore {
		/// <summary>Gets or sets the object associated with the specified key.</summary>
		/// <param name="key">The key to get or set the object for.</param>
		/// <value>The object associated with the specified key, or <see langword="null" /> if no object is associated with the key.</value>
		public NSObject? this [NSString key] {
			get {
				return ObjectForKey (key);
			}
			set {
				SetObjectForKey (value, key);
			}
		}

		/// <summary>Gets or sets the object associated with the specified key.</summary>
		/// <param name="key">The key to get or set the object for.</param>
		/// <value>The object associated with the specified key, or <see langword="null" /> if no object is associated with the key.</value>
		public NSObject? this [string key] {
			get {
				return ObjectForKey (key);
			}
			set {
				SetObjectForKey (value, key);
			}
		}

		/// <summary>Sets a string value for the specified key in the ubiquitous key-value store.</summary>
		/// <param name="key">The key to associate the value with.</param>
		/// <param name="value">The string value to store.</param>
		public void SetString (string key, string value)
		{
			_SetString (value, key);
		}

		/// <summary>Sets a data value for the specified key in the ubiquitous key-value store.</summary>
		/// <param name="key">The key to associate the value with.</param>
		/// <param name="value">The <see cref="NSData" /> value to store.</param>
		public void SetData (string key, NSData value)
		{
			_SetData (value, key);
		}

		/// <summary>Sets an array value for the specified key in the ubiquitous key-value store.</summary>
		/// <param name="key">The key to associate the value with.</param>
		/// <param name="value">The array of <see cref="NSObject" /> values to store.</param>
		public void SetArray (string key, NSObject [] value)
		{
			_SetArray (value, key);
		}

		/// <summary>Sets a dictionary value for the specified key in the ubiquitous key-value store.</summary>
		/// <param name="key">The key to associate the value with.</param>
		/// <param name="value">The <see cref="NSDictionary" /> value to store.</param>
		public void SetDictionary (string key, NSDictionary value)
		{
			_SetDictionary (value, key);
		}

		/// <summary>Sets a long integer value for the specified key in the ubiquitous key-value store.</summary>
		/// <param name="key">The key to associate the value with.</param>
		/// <param name="value">The long integer value to store.</param>
		public void SetLong (string key, long value)
		{
			_SetLong (value, key);
		}

		/// <summary>Sets a double-precision floating-point value for the specified key in the ubiquitous key-value store.</summary>
		/// <param name="key">The key to associate the value with.</param>
		/// <param name="value">The double value to store.</param>
		public void SetDouble (string key, double value)
		{
			_SetDouble (value, key);
		}

		/// <summary>Sets a boolean value for the specified key in the ubiquitous key-value store.</summary>
		/// <param name="key">The key to associate the value with.</param>
		/// <param name="value">The boolean value to store.</param>
		public void SetBool (string key, bool value)
		{
			_SetBool (value, key);
		}
	}
}
