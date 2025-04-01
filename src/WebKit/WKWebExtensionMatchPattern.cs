// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Foundation;

namespace WebKit {
	public partial class WKWebExtensionMatchPattern {
		/// <summary>Create a new <see cref="WKWebExtensionMatchPattern" /> for the specified pattern.</summary>
		/// <param name="string">The pattern to initialize the new instance with.</param>
		/// <param name="error">The error object, if an error occurred, otherwise null.</param>
		/// <returns>A newly created <see cref="WKWebExtensionMatchPattern" /> if successful, otherwise null.</returns>
		public static WKWebExtensionMatchPattern? Create (string @string, out NSError? error)
		{
			var rv = new WKWebExtensionMatchPattern (NSObjectFlag.Empty);
			rv.InitializeHandle (rv._InitWithString (@string, out error), string.Empty, false);
			if (rv.Handle == IntPtr.Zero) {
				rv.Dispose ();
				return null;
			}
			return rv;
		}

		/// <summary>Create a new <see cref="WKWebExtensionMatchPattern" /> for the specified scheme, host and path.</summary>
		/// <param name="scheme">The scheme to initialize the new instance with.</param>
		/// <param name="host">The host to initialize the new instance with.</param>
		/// <param name="path">The path to initialize the new instance with.</param>
		/// <param name="error">The error object, if an error occurred, otherwise null.</param>
		/// <returns>A newly created <see cref="WKWebExtensionMatchPattern" /> if successful, otherwise null.</returns>
		public static WKWebExtensionMatchPattern? Create (string scheme, string host, string path, out NSError? error)
		{
			var rv = new WKWebExtensionMatchPattern (NSObjectFlag.Empty);
			rv.InitializeHandle (rv._InitWithScheme (scheme, host, path, out error), string.Empty, false);
			if (rv.Handle == IntPtr.Zero) {
				rv.Dispose ();
				return null;
			}
			return rv;
		}
	}
}
