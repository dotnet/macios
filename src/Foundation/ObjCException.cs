//
// Copyright 2013, Xamarin Inc.
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

using System.Text;

#nullable enable

namespace ObjCRuntime {
	/// <summary>Represents an Objective-C exception that has been caught and wrapped as a managed exception.</summary>
	public class ObjCException : Exception {
		NSException native_exc;

		/// <summary>Initializes a new instance of the <see cref="ObjCException" /> class with a default NSException.</summary>
		public ObjCException () : base ()
		{
			native_exc = new NSException ("default", String.Empty, null);
		}

		/// <summary>Initializes a new instance of the <see cref="ObjCException" /> class wrapping the specified Objective-C exception.</summary>
		/// <param name="exc">The native <see cref="Foundation.NSException" /> to wrap.</param>
		public ObjCException (NSException exc) : base ()
		{
			native_exc = exc;
		}

		/// <summary>Gets the underlying <see cref="Foundation.NSException" /> that this exception wraps.</summary>
		/// <value>The native Objective-C exception, or <see langword="null" /> if not available.</value>
		public NSException? NSException {
			get {
				return native_exc;
			}
		}

		/// <summary>Gets the reason string from the underlying Objective-C exception.</summary>
		/// <value>The reason for the exception, or <see langword="null" /> if not available.</value>
		public string? Reason {
			get {
				return native_exc?.Reason;
			}
		}

		/// <summary>Gets the name of the underlying Objective-C exception.</summary>
		/// <value>The exception name, or <see langword="null" /> if not available.</value>
		public string? Name {
			get {
				return native_exc?.Name;
			}
		}

		/// <summary>Gets a message that includes the exception name, reason, and native stack trace.</summary>
		/// <value>A formatted string describing the Objective-C exception.</value>
		public override string Message {
			get {
				var sb = new StringBuilder ("Objective-C exception thrown.  Name: ").Append (Name);
				sb.Append (" Reason: ").AppendLine (Reason);
				AppendNativeStackTrace (sb);
				return sb.ToString ();
			}
		}

		void AppendNativeStackTrace (StringBuilder sb)
		{
			if (native_exc?.CallStackSymbols?.Length > 0) {
				sb.AppendLine ("Native stack trace:");
				foreach (var symbol in native_exc.CallStackSymbols)
					sb.Append ('\t').AppendLine (symbol);
			}
		}

		public override string ToString ()
		{
			var msg = base.ToString ();
			if (native_exc is null)
				return msg;

			var sb = new StringBuilder (msg);
			sb.AppendLine ();
			AppendNativeStackTrace (sb);
			return sb.ToString ();
		}
	}
}
