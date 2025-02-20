//
// Extensions.cs: C#isms for JavaScriptCore
//
// Authors:
//   Aaron Bockover (abock@xamarin.com)
//
// Copyright 2013 Xamarin, Inc.

#nullable enable

using System;
using Foundation;

namespace JavaScriptCore {

	public partial class JSContext {

		public JSValue this [NSObject key] {
			get { return _GetObject (key); }
			set { _SetObject (value, key); }
		}
	}

	public partial class JSValue {

		/// <summary>Returns a string representation of the value of the current instance.</summary>
		///         <returns>
		///         </returns>
		///         <remarks>
		///         </remarks>
		public override string ToString ()
		{
			return _ToString ();
		}

		/// <param name="value">To be added.</param>
		///         <param name="context">To be added.</param>
		///         <summary>Creates a JavaScript string from the provided string.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		static public JSValue From (string value, JSContext context)
		{
			using (var str = new NSString (value)) {
				return From ((NSObject) str, context);
			}
		}

		public JSValue this [nuint index] {
			get { return _ObjectAtIndexedSubscript (index); }
			set { _SetObject (value, index); }
		}

		public JSValue this [NSObject key] {
			get { return _ObjectForKeyedSubscript (key); }
			set { _SetObject (value, key); }
		}
	}
}
